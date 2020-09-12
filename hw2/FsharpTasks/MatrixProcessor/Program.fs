open Argu
open FsMatrix.Matrix
open FsMatrix.MatrixIO
open Maybe
open TransitiveClosure
open System.IO
open Translator
open Integer
open Boolean

type Task =
    | MUL
    | APSP
    | TRC

type Argument = 
    | [<Mandatory>] Task of task : Task
    | [<MainCommand; ExactlyOnce; Last>] Matrices of paths : string list
    with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Task _ -> "specify what is to be done. (TRC | APSP | MUL)"
            | Matrices _ -> "specify paths to matrices."

let parser = ArgumentParser.Create<Argument>(programName = "MatrixProcessor")

let maybe = new MaybeBuilder ()

let checkMatricesCount n list =
    let len = List.length list
    if len < n
    then failwith "Not enough arguments."
    elif len > n
    then failwith "Too many arguments."

let writeMatrix f = toRowsList >> writeRowsList f

let processResult result continuation dst =
    match result with
    | None -> eprintfn "Something wrong with matrices."
    | Some x -> continuation x dst

let checkTRCMatricesPaths = checkMatricesCount 2
let readMatrix f = fromRowsList << readRowsList f
    

[<EntryPoint>]
let main argv =
    
    try

        let results = parser.ParseCommandLine argv

        let task, matricesPaths = results.GetResult Task, results.GetResult Matrices
        match task with
        | MUL ->
            checkMatricesCount 3 matricesPaths
                
            let readMatrix = readMatrix fromWordI
                
            let [m1src; m2src; dst] = matricesPaths
            let result = maybe {
                let! matrix1 = readMatrix m1src
                let! matrix2 = readMatrix m2src
                let! result = multiply integerSemiring matrix1 matrix2
                return result
            }
            processResult result (writeMatrix toWordI) dst

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
                let! matrix = readMatrix id msrc
                let! result = floydWarshall sg matrix
                return result
            }
            processResult result (writeMatrix id) dst

        | TRC ->
            checkTRCMatricesPaths matricesPaths

            let toSeq = Seq.ofList << (List.map Seq.ofList) << toRowsList

            let [msrc; dst] = matricesPaths
            let result = maybe {
                let! matrix = readMatrix fromWordB msrc
                let! result = floydWarshall booleanSemigroupWithPartialOrder matrix
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