namespace FsMatrix

module MatrixIO =
    
    open System.IO
    
    let readRowsList toMatrixElement path =
        let lineIsNotEmpty = not << (=) ""
        let lineToRow (line : string) =
            let words = line.Split " " |> List.ofArray
            List.map toMatrixElement words
        let lines = File.ReadLines path
        (List.ofSeq << Seq.map lineToRow << Seq.filter lineIsNotEmpty) lines

    let writeRowsList toWord rows path =
        let rowToLine =
            String.concat " " << List.map toWord
        File.WriteAllLines (path, ((Seq.ofList << List.map rowToLine) rows))