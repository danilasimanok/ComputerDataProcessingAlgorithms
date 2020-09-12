module Controller

open System
open MyFailings
open Messages
open Railway

type Handler = MailboxProcessor<Command>

type TypeHandlers =
    {
    Creator : Handler;
    Writer : Handler;
    Reader : Handler;
    Multiplier : Handler;
    TRCExecutor : Handler;
    }

type VariableInformation =
    | Promise of promise : Async<Answer>
    | Value of variable : obj
    | Fail of failings : MyFailing list

type HandlerType =
    | Creator
    | Writer
    | Reader
    | Multiplier
    | TRCExecutor

let createMainLoop (handlers : Map<string, TypeHandlers>) =
    let getHandler (t, ht) =
        match Map.tryFind t handlers with
        | Some handlers ->
            let getHandler () =
                match ht with
                | Creator -> handlers.Creator
                | Writer -> handlers.Writer
                | Reader -> handlers.Reader
                | Multiplier -> handlers.Multiplier
                | TRCExecutor -> handlers.TRCExecutor
            (Success << getHandler) ()
        | None ->
            Failure [TypeDoesNotExist]
    let strToInt =
        let exnToErr _ = [IntegerParameterIsIncorrect]
        noExns int exnToErr
    let create (handler : Handler, size) =
        let f r = Create (size, r)
        (Promise << handler.PostAndAsyncReply) f
    let awaitComputation info =
        match info with
        | Promise ans ->
            match Async.RunSynchronously ans with
            | Answer.Success result -> Value result
            | Answer.Failure msg -> Fail msg
            | Answer.Dead -> Fail [ActorWasDead]
        | info -> info
    let getValue info =
        match info with
        | Promise _ -> Failure [PromiseIsNotExpected]
        | Value value -> Success value
        | Fail msg -> Failure msg
    let write (handler : Handler, value, path) =
        let f r = Write (value, path, r)
        handler.PostAndAsyncReply f
    let complain = List.iter (eprintfn "\tError : %s" << failingToString)
    let update f res state=
        match res with
        | Success res -> f res state
        | Failure msgs ->
            complain msgs
            state
    let read (handler : Handler, path) =
        let f r = Read (path, r)
        (Promise << handler.PostAndAsyncReply) f
    let compareTypes (t1, t2) =
        if t1 = t2
        then Success t1
        else Failure [TypesDoesNotMatch]
    let multiply (handler : Handler, value1, value2) =
        let f r = Multiply (value1, value2, r)
        handler.PostAndAsyncReply f
    let trc (handler : Handler, value) =
        let f r = ExecuteTRC (value, r)
        handler.PostAndAsyncReply f
    
    let rec step (variables : Map<string, string * VariableInformation>) writeQueue =
        let updateVars (name, t, info) vars = Map.add name (t, info) vars
        let updateWr a l = a :: l
        let getVariable name =
            match Map.tryFind name variables with
            | Some info -> Success info
            | None -> Failure [VariableDoesNotExists]
        let getTAndInfo name =
            let tAndInfo = getVariable name
            (map fst) tAndInfo, (map (awaitComputation << snd)) tAndInfo

        let input = Console.ReadLine().Trim()
        match input.Split ' ' with
        | [|"CREATE"; name; t; size|] ->
            let handler = getHandler (t, Creator)
            let size = strToInt size
            let info = (map create << couple handler) size
            let variable = triple (Success name) (Success t) info
            let variables = update updateVars variable variables
            step variables writeQueue
        | [|"WRITE"; name; path|] ->
            let t, info = getTAndInfo name
            let value, path = (bind getValue) info, Success path
            let handler = (bind getHandler << couple t << Success) Writer
            let writing = (map write) (triple handler value path)
            let writeQueue = update updateWr writing writeQueue
            let variable = triple (Success name) t info
            let variables = update updateVars variable variables
            step variables writeQueue
        | [|"READ"; name; t; path|] ->
            let handler = getHandler (t, Reader)
            let info = (map read << couple handler << Success) path
            let variable = triple (Success name) (Success t) info
            let variables = update updateVars variable variables
            step variables writeQueue
        | [|"MUL"; name; op1; op2|] ->
            let t1, info1 = getTAndInfo op1
            let t2, info2 = getTAndInfo op2
            let t = bind compareTypes (couple t1 t2)
            let handler = (bind getHandler << couple t << Success) Multiplier
            let value1, value2 = (bind getValue) info1, (bind getValue info2)
            let promise = map (Promise << multiply) (triple handler value1 value2)
            let variable = triple (Success name) t promise
            let variables = update updateVars variable variables
            step variables writeQueue
        | [|"TRC"; name; op|] ->
            let t, info = getTAndInfo op
            let handler = (bind getHandler << couple t << Success) TRCExecutor
            let promise = (map (Promise << trc) << couple handler << bind getValue) info
            let variable = triple (Success name) t promise
            let variables = update updateVars variable variables
            step variables writeQueue
        | [|"EXIT"|] ->
            let wait = ignore << List.map Async.RunSynchronously 
            wait writeQueue
            let toList (_, handlers) = [handlers.Creator; handlers.Writer; handlers.Reader; handlers.Multiplier; handlers.TRCExecutor]
            let mapping (handler : Handler) =
                let f r = Die r
                handler.PostAndAsyncReply f
            (wait << List.map mapping << List.concat << List.map toList << Map.toList) handlers
        | _ ->
            eprintfn "Command '%s' was not recognized." input
            step variables writeQueue
    step