open Controller
open ConcreteActors

[<EntryPoint>]
let main _ =
    let actors = [("BOOL", boolActors ()); ("INT", intActors ()); ("REAL", realActors ()); ("EREAL", extendedRealActors ())]
    let step = (createMainLoop << Map.ofList) actors
    step Map.empty []
    //let ans = (Async.RunSynchronously << boolActors.Creator.PostAndAsyncReply) (fun r -> Create (2,r))
    0