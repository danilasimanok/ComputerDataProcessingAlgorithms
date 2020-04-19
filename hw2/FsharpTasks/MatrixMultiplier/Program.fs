open Matrix
open MatrixIO
open Maybe
open Argu

type Argument =
    | [<Mandatory>] FirstMatrix of path : string
    | [<Mandatory>] SecondMatrix of path : string
    | [<Mandatory>] Result of path : string
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | FirstMatrix _ -> "specify path to the first matrix."
            | SecondMatrix _ -> "specify path to the second matrix."
            | Result _ -> "specify path in which the result of multiplication should be written."

let parser = ArgumentParser.Create<Argument>(programName = "MatrixMultiplier.exe");

let sr = {IdentityElement = 0; Add = (+); Multiply = (*)}

let maybe = new MaybeBuilder()

let readMatrix = fromRowsList << readRowsList int

[<EntryPoint>]
let main argv =
    try
        let results = parser.Parse argv
        let m1src, m2src, dst = results.GetResult FirstMatrix, results.GetResult SecondMatrix, results.GetResult Result
        let result = maybe {
            let! matrix1 = readMatrix m1src
            let! matrix2 = readMatrix m2src
            let! result = multiply sr matrix1 matrix2
            return result
        }
        match result with
        | None -> eprintfn "Something wrong with matrices."
        | Some matrix -> (toRowsList >> writeRowsList string) matrix dst
    with
        e -> eprintfn "%s" e.Message
    0
