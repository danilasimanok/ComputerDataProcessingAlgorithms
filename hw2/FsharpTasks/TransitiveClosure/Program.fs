open Argu
open Matrix
open MatrixIO
open Maybe
open System.IO
open System.Diagnostics
open DotGenerator

type Argument =
    | [<Mandatory>] Matrix of path : string
    | [<Mandatory>] Result of path : string
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Matrix _ -> "specify path to the origin matrix."
            | Result _ -> "specify path in which the result of transitive closure should be written."

let parser = ArgumentParser.Create<Argument>(programName = "TransitiveClosure.exe");

let sg = {Multiply = (&&); Le = fun x y -> x || not y}

let toBool word =
    match word with
    | "t" -> true
    | "f" -> false
    | _ -> failwith "Boolean can be restored only from 't' or 'f'."

let toString f = if f then "t" else "f"

let maybe = new MaybeBuilder()

[<EntryPoint>]
let main argv =
    try
        let results = parser.Parse argv
        let msrc, dst = results.GetResult Matrix, results.GetResult Result
        let result = maybe {
            let! matrix = (fromRowsList << readRowsList toBool) msrc
            let! result = floydWarshall sg matrix
            return matrix, result
        }
        match result with
        | None -> eprintfn "Something wrong with matrix."
        | Some (origin, result) ->
            let dotString = generateDot origin result
            let temp = dst + ".tmp"
            File.WriteAllText(temp, dotString)
            let startInfo =
                ProcessStartInfo (
                    FileName = "dot",
                    Arguments = "-Tpdf -o" + dst + " " + temp
                )
            let dot = new Process(StartInfo = startInfo)
            if dot.Start()
            then dot.WaitForExit()
            else failwith "Failed to start process dot."
            File.Delete(temp)
    with
        e -> eprintfn "%s" e.Message
    0
