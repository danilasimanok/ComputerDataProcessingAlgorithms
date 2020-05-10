module Maybe

    type MaybeBuilder () =
        member this.Bind (x, f) =
            match x with
            | None -> None
            | Some x -> f x
        member this.Return (x) = Some x