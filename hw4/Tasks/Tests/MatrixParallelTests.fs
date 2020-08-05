module ParallelMultiplicationTests

open NUnit.Framework
open FsCheck.NUnit
open FsMatrix.Matrix
open Random

[<TestFixture>]
type ParallelMultiplicationTests () =
    
    let sr = {IdentityElement = 0; Add = (+); Multiply = (*)}
    let randomMatrix n =
        match (fromRowsList << randomLists randomInteger) n with
        | None -> failwith "Something strange occured."
        | Some matrix -> matrix

    [<Property>]
    member _.multiplicationsAreEquivalent (n : int) =
        let n =
            if n < 0
            then -n
            elif n = 0
            then 1
            else n
        let x, y = randomMatrix n, randomMatrix n
        multiply sr x y = MatrixParallel.multiply sr x y