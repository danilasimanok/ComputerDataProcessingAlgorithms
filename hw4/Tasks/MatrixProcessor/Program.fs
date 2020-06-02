open Types
open System
open FsMatrix.Matrix
open Creator
open Reader
open Writer
open Multiplier
open TransitiveClosure

type ProcessorCommands =
    | Create of id : string * t : ElementType * size : int
    | Read of id : string * t : ElementType * path : string
    | Write of id : string * path : string
    | Multiply of id : string * op1 : string * op2 : string
    | TransitiveClosure of id : string * op : string
    | Exit

type Content =
    | Matrix
    | Option of errorMessage : string

type VariableInformation =
    | Promise of ElementType * obj * Content
    | Value of ElementType * obj

type getValueResult =
    | ErrorMessage of string
    | Result of ElementType * obj * Map<string, VariableInformation>

let getValue (matrices : Map<string, VariableInformation>) id =
    match matrices.TryFind id with
    | None -> ErrorMessage ("Variable " + id + " is not defined.")
    | Some info ->
        match info with
        | Value (t, value) -> Result (t, value, matrices)
        | Promise (t, reply, content) ->
            let continuation reply =
                let matrices' = (matrices.Remove id).Add (id, Value (t, reply))
                Result (t, reply, matrices')
            match content with
            | Matrix ->
                let reply =
                    match t with
                    | BOOLEAN -> (Async.RunSynchronously (reply :?> Async<Matrix<bool>>)) :> obj
                    | INTEGER -> (Async.RunSynchronously (reply :?> Async<Matrix<int>>)) :> obj
                    | REAL -> (Async.RunSynchronously (reply :?> Async<Matrix<float>>)) :> obj
                    | EXTENDED_REAL -> (Async.RunSynchronously (reply :?> Async<Matrix<ExtendedReal>>)) :> obj
                continuation reply
            | Option errorMessage ->
                try
                    let reply =
                        match t with
                        | BOOLEAN -> (Async.RunSynchronously (reply :?> Async<Matrix<bool> option>)).Value :> obj
                        | INTEGER -> (Async.RunSynchronously (reply :?> Async<Matrix<int> option>)).Value :> obj
                        | REAL -> (Async.RunSynchronously (reply :?> Async<Matrix<float> option>)).Value :> obj
                        | EXTENDED_REAL -> (Async.RunSynchronously (reply :?> Async<Matrix<ExtendedReal> option>)).Value :> obj
                    continuation reply
                with :? NullReferenceException -> ErrorMessage errorMessage

let rec readCommand () =
    let toType str =
        match str with
        | "BOOL" -> BOOLEAN
        | "INT" -> INTEGER
        | "REAL" -> REAL
        | "EREAL" -> EXTENDED_REAL
        | _ -> failwith "The type is incorrect."
    try
        match Console.ReadLine().Trim().Split " " with
        | [|id; "="; "CREATE"; t; size|] -> Create (id, toType t, int size)
        | [|id; "="; "READ"; t; path|] -> Read (id, toType t, path)
        | [|"WRITE"; id; path|] -> Write (id, path)
        | [|id; "="; "MUL"; op1; op2|] -> Multiply (id, op1, op2)
        | [|id; "="; "TRC"; op|] -> TransitiveClosure (id, op)
        | [|"EXIT"|] -> Exit
        | _ -> failwith "Incorrect command."
    with e ->
        eprintfn "%s" e.Message
        readCommand ()

type ControllerCommands =
    Start of exited : AsyncReplyChannel<bool>

[<EntryPoint>]
let main _ =
    let creators = createCreators ()
    let readers = createReaders ()
    let writers = createWriters ()
    let multipliers = createMultipliers ()
    let transitiveClosures = createTransitiveClosures ()
    let processMail (inbox : MailboxProcessor<ControllerCommands>) =
        let replyChannel = match Async.RunSynchronously (inbox.Receive ()) with Start r -> r
        let rec innerLoop (matrices : Map<string, VariableInformation>) writeQueue  = async {
            let killEveryone () =
                let reportWritingError (boolAsync, path) =
                    if (not << Async.RunSynchronously) boolAsync
                    then eprintfn "%s" ("Failed to write " + path)
                (List.iter reportWritingError << List.rev) writeQueue
                let reports = [creators.BoolCreator.PostAndAsyncReply Creator.Exit; creators.IntCreator.PostAndAsyncReply Creator.Exit;
                    creators.RealCreator.PostAndAsyncReply Creator.Exit; creators.ExtendedRealCreator.PostAndAsyncReply Creator.Exit;
                    readers.BoolReader.PostAndAsyncReply Reader.Exit; readers.IntReader.PostAndAsyncReply Reader.Exit;
                    readers.RealReader.PostAndAsyncReply Reader.Exit; readers.ExtendedRealReader.PostAndAsyncReply Reader.Exit;
                    writers.BoolWriter.PostAndAsyncReply Writer.Exit; writers.IntWriter.PostAndAsyncReply Writer.Exit;
                    writers.RealWriter.PostAndAsyncReply Writer.Exit; writers.ExtendedRealWriter.PostAndAsyncReply Writer.Exit;
                    multipliers.BoolMultiplier.PostAndAsyncReply Multiplier.Exit; multipliers.IntMultiplier.PostAndAsyncReply Multiplier.Exit;
                    multipliers.RealMultiplier.PostAndAsyncReply Multiplier.Exit; multipliers.ExtendedRealMultiplier.PostAndAsyncReply Multiplier.Exit;
                    transitiveClosures.BoolTransitiveClosure.PostAndAsyncReply TransitiveClosure.Exit; transitiveClosures.IntTransitiveClosure.PostAndAsyncReply TransitiveClosure.Exit;
                    transitiveClosures.RealTransitiveClosure.PostAndAsyncReply TransitiveClosure.Exit; transitiveClosures.ExtendedRealTransitiveClosure.PostAndAsyncReply TransitiveClosure.Exit]
                List.iter (ignore << Async.RunSynchronously) reports
                replyChannel.Reply true
            let terminate emsg =
                eprintfn "%s" emsg
                killEveryone ()
            let message = readCommand ()
            match message with
            | Create (id, t, size) ->
                let createPromise () =
                    let f r = CreateMatrix (size, r)
                    match t with
                    | BOOLEAN -> creators.BoolCreator.PostAndAsyncReply f :> obj
                    | INTEGER -> creators.IntCreator.PostAndAsyncReply f :> obj
                    | REAL -> creators.RealCreator.PostAndAsyncReply f :> obj
                    | EXTENDED_REAL -> creators.ExtendedRealCreator.PostAndAsyncReply f :> obj
                return! innerLoop (matrices.Add (id, Promise (t, createPromise (), Matrix))) writeQueue
            | Read (id, t, path) ->
                let promise =
                    let f r = ReadMatrix (path, r)
                    match t with
                    | BOOLEAN -> readers.BoolReader.PostAndAsyncReply f :> obj
                    | INTEGER -> readers.IntReader.PostAndAsyncReply f :> obj
                    | REAL -> readers.RealReader.PostAndAsyncReply f :> obj
                    | EXTENDED_REAL -> readers.ExtendedRealReader.PostAndAsyncReply f :> obj
                return! innerLoop (matrices.Add (id, Promise (t, promise, Option ("Failed to read " + path)))) writeQueue
            | Write (id, path) ->
                match getValue matrices id with
                | ErrorMessage msg ->
                    terminate msg
                | Result (t, value, matrices) ->
                    let f m r = WriteMatrix (path, m, r)
                    let continuation = innerLoop matrices
                    return!
                        match t with
                        | BOOLEAN -> continuation ((writers.BoolWriter.PostAndAsyncReply (f (value :?> Matrix<bool>)), path) :: writeQueue)
                        | INTEGER -> continuation ((writers.IntWriter.PostAndAsyncReply (f (value :?> Matrix<int>)), path) :: writeQueue)
                        | REAL -> continuation ((writers.RealWriter.PostAndAsyncReply (f (value :?> Matrix<float>)), path) :: writeQueue)
                        | EXTENDED_REAL -> continuation ((writers.ExtendedRealWriter.PostAndAsyncReply (f (value :?> Matrix<ExtendedReal>)), path) :: writeQueue)
            | Multiply (id, op1, op2) ->
                match getValue matrices op1 with
                | ErrorMessage msg ->
                    terminate msg
                | Result (t1, value1, matrices) ->
                    match getValue matrices op2 with
                        | ErrorMessage msg ->
                            terminate msg
                        | Result (t2, value2, matrices) ->
                            if t1 = t2
                            then
                                let promise =
                                    let f m1 m2 r = MultiplyMatrices (m1, m2, r)
                                    match t1 with
                                    | BOOLEAN ->
                                        let m1, m2 = value1 :?> Matrix<bool>, value2 :?> Matrix<bool>
                                        multipliers.BoolMultiplier.PostAndAsyncReply (f m1 m2) :> obj
                                    | INTEGER ->
                                        let m1, m2 = value1 :?> Matrix<int>, value2 :?> Matrix<int>
                                        multipliers.IntMultiplier.PostAndAsyncReply (f m1 m2) :> obj
                                    | REAL ->
                                        let m1, m2 = value1 :?> Matrix<float>, value2 :?> Matrix<float>
                                        multipliers.RealMultiplier.PostAndAsyncReply (f m1 m2) :> obj
                                    | EXTENDED_REAL ->
                                        let m1, m2 = value1 :?> Matrix<ExtendedReal>, value2 :?> Matrix<ExtendedReal>
                                        multipliers.ExtendedRealMultiplier.PostAndAsyncReply (f m1 m2) :> obj
                                return! innerLoop (matrices.Add (id, Promise (t1, promise, Option ("Sizes are incorrect: " + op1 + " " + op2)))) writeQueue
                            else
                                terminate (op1 + " and " + op2 + " have different types.")
            | TransitiveClosure (id, op) ->
                match getValue matrices op with
                | ErrorMessage msg ->
                    terminate msg
                | Result (t, value, matrices) ->
                    let promise =
                        let f m r = TRC (m, r)
                        match t with
                        | BOOLEAN -> transitiveClosures.BoolTransitiveClosure.PostAndAsyncReply (f (value :?> Matrix<bool>)) :> obj
                        | INTEGER -> transitiveClosures.IntTransitiveClosure.PostAndAsyncReply (f (value :?> Matrix<int>)) :> obj
                        | REAL -> transitiveClosures.RealTransitiveClosure.PostAndAsyncReply (f (value :?> Matrix<float>)) :> obj
                        | EXTENDED_REAL -> transitiveClosures.ExtendedRealTransitiveClosure.PostAndAsyncReply (f (value :?> Matrix<ExtendedReal>)) :> obj
                    return! innerLoop (matrices.Add (id, Promise (t, promise, Matrix))) writeQueue
            | Exit -> killEveryone ()
        }
        innerLoop Map.empty []
    let controller = MailboxProcessor<ControllerCommands>.Start processMail
    (ignore << Async.RunSynchronously) (controller.PostAndAsyncReply Start)
    0