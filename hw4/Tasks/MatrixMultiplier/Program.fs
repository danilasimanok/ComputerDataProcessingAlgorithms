open Argu
open Types
open Maybe
open Boolean
open Integer
open Real
open ExtendedReal
open Helpers

type Args =
    | [<Mandatory>] T of elementType : ElementType
    | [<Mandatory>] M1 of path : string
    | [<Mandatory>] M2 of path : string
    | [<Mandatory>] R of path : string
    with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | T _ -> "specify type of matrix elements. (REAL | EXTENDED_REAL | BOOLEAN | INTEGER)"
            | M1 _ -> "specify the path to the first factor."
            | M2 _ -> "specify the path to the second factor."
            | R _ -> "specify where result of multiplication should be written."

let parser = ArgumentParser.Create<Args>(programName = "MatrixMultiplier")

let maybe = new MaybeBuilder ()

let boolOps = helpers fromWordB toWordB booleanSemiring
let intOps = helpers fromWordI toWordI integerSemiring
let realOps = helpers fromWordR toWordR realSemiring
let extendedRealOps = helpers fromWordER toWordER extendedRealSemiring

[<EntryPoint>]
let main argv =
    try
        let results = parser.ParseCommandLine argv
        let elementType, m1src, m2src, dst =
            results.GetResult T, results.GetResult M1, results.GetResult M2, results.GetResult R
        let continuation ops =
            let result = maybe {
                let! m1 = ops.ReadMatrix m1src
                let! m2 = ops.ReadMatrix m2src
                let! result = MatrixParallel.multiply ops.Semiring m1 m2
                return result
            }
            match result with
            | None -> eprintf "Something wrong with matrices."
            | Some matrix -> ops.WriteMatrix matrix dst
        match elementType with
        | BOOLEAN -> continuation boolOps
        | INTEGER -> continuation intOps
        | REAL -> continuation realOps
        | EXTENDED_REAL -> continuation extendedRealOps
    with e -> eprintf "%s" e.Message
    0