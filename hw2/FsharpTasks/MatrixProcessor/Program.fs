open Argu
open Args
open FsMatrix.Matrix
open FsMatrix.MatrixIO
open Maybe
open TransitiveClosure
open System.IO
open Translator

let parser = ArgumentParser.Create<Argument>(programName = "MatrixProcessor")

let maybe = new MaybeBuilder ()

let checkMatricesCount n list =
    let len = List.length list
    if len < n
    then failwith "Not enough arguments."
    elif len > n
    then failwith "Too many arguments."
    else ()

let writeMatrix f = toRowsList >> writeRowsList f

let processResult result continuation dst =
    match result with
    | None -> eprintfn "Something wrong with matrices."
    | Some x -> continuation x dst

let checkTRCMatricesPaths = checkMatricesCount 2
let readForTRC f = fromRowsList << readRowsList f
    

[<EntryPoint>]
let main argv =
    
    try

        let results = parser.ParseCommandLine argv

        let task, matricesPaths = results.GetResult Task, results.GetResult Matrices
        match task with
        | MUL ->
            checkMatricesCount 3 matricesPaths
                
            let sr = {IdentityElement = 0; Add = (+); Multiply = (*)}
            let readMatrix = fromRowsList << readRowsList int
                
            let [m1src; m2src; dst] = matricesPaths
            let result = maybe {
                let! matrix1 = readMatrix m1src
                let! matrix2 = readMatrix m2src
                let! result = multiply sr matrix1 matrix2
                return result
            }
            processResult result (writeMatrix string) dst

        | APSP ->
            checkTRCMatricesPaths matricesPaths

            let mul x y =
                let result = x + y
                if String.length result >= 5
                then "infty"
                else result
            let le x y = String.length x <= String.length y
            let sg = {Multiply = mul; Le = le}

            let [msrc; dst] = matricesPaths
            let result = maybe {
                let! matrix = readForTRC id msrc
                let! result = floydWarshall sg matrix
                return result
            }
            processResult result (writeMatrix id) dst

        | TRC ->
            checkTRCMatricesPaths matricesPaths

            let sg = {Multiply = (&&); Le = fun x y -> x || not y}
            let toBool word =
                match word with
                | "t" -> true
                | "f" -> false
                | _ -> failwith "Boolean can be restored only from 't' or 'f'."
            let toSeq = Seq.ofList << (List.map Seq.ofList) << toRowsList

            let [msrc; dst] = matricesPaths
            let result = maybe {
                let! matrix = readForTRC toBool msrc
                let! result = floydWarshall sg matrix
                return matrix, result
            }

            let continuation (origin, result) dst =
                let temp = dst + ".tmp"
                let mt = new MatrixTranslator<bool, Boolean>(fun f -> new Boolean(f));
                let originArray, resultArray = (mt.Translate << toSeq) origin, (mt.Translate << toSeq) result
                DotMediator.CreateDot(originArray, resultArray, temp)
                DotMediator.ProcessDot(temp, dst)
                File.Delete(temp)

            processResult result continuation dst
    
    with e -> eprintfn "%s" e.Message

    0