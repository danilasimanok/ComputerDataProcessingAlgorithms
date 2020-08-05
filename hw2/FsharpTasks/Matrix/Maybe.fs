module Maybe

    type MaybeBuilder () =
        member _.Bind (x, f) =
            match x with
            | None -> None
            | Some x -> f x
        member _.Return (x) = Some x