open Matrix
open MatrixIO
open Maybe
open Argu

type Argument =
    | [<Mandatory>] Matrix of path : string
    | [<Mandatory>] Result of path : string
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Matrix _ -> "specify path to the origin matrix."
            | Result _ -> "specify path in which the result of transitive closure should be written."

let parser = ArgumentParser.Create<Argument>(programName = "APSP.exe");

let maybe = new MaybeBuilder()

let mul x y =
    let result = x + y
    if String.length result >= 5
    then "infty"
    else result
let le x y = String.length x <= String.length y
let sg = {Multiply = mul; Le = le}

[<EntryPoint>]
let main argv =
    try
        let results = parser.Parse argv
        let msrc, dst = results.GetResult Matrix, results.GetResult Result
        let result = maybe {
            let! matrix = (fromRowsList << readRowsList id) msrc
            let! result = floydWarshall sg matrix
            return result
        }
        match result with
        | None -> eprintfn "Something wrong with matrix."
        | Some matrix -> (toRowsList >> writeRowsList id) matrix dst
    with
        e -> eprintfn "%s" e.Message
    0
