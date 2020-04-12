namespace Matrix

open System.IO

module MatrixIO =
    
    let readRowsList toMatrixElement path =
        let lineIsNotEmpty line = line = "" |> not
        let lineToRow (line : string) =
            let words = List.ofArray <| line.Split " "
            List.map toMatrixElement words
        try
            let lines = File.ReadLines path
            Some <| (List.ofSeq <| (Seq.map lineToRow <| (Seq.filter lineIsNotEmpty lines)))
        with
            _ -> None

    let writeRowsList toWord rows path =
        let rowToLine row =
            List.map toWord row |> String.concat " "
        try
            File.WriteAllLines (path, (Seq.ofList <| List.map rowToLine rows))
            true
        with
            _ -> false