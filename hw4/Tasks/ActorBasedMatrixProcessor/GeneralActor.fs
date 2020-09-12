module GeneralActor

open Railway
open MyFailings
open Messages

let reply (result, r : ReplyChannel) =
    match result with
    | Result.Success result -> (r.Reply << Answer.Success) (result :> obj)
    | Result.Failure msg -> (r.Reply << Answer.Failure) msg

let creativeActor f (inbox : MailboxProcessor<Command>) =
    let rec loop () = async {
        let! msg = inbox.Receive ()
        match f msg with
        | Some resultAndR ->
            reply resultAndR
            return! loop ()
        | None ->
            match msg with
            | Die r -> r.Reply Answer.Dead
            | msg ->
                ((getReplyChannel msg).Reply << Failure) [(UnexpectedMessage << cmdToStr) msg]
                return! loop ()
    }
    loop ()

let processingActor t cast f a (inbox : MailboxProcessor<Command>) =
    let cast =
        let exnToErr _ = [AnotherTypeRequired t]
        noExns cast exnToErr
    let f = f cast a
    creativeActor f inbox