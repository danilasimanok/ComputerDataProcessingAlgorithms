module RealTests

open NUnit.Framework
open FsUnit
open Real
open System

[<TestFixture>]
type RealTests () =

    [<Test>]
    member _.testSerialization () =
        toWordR 1.0 |> should equal "1"

    [<Test>]
    member _.testDeserialization () =
        fromWordR "1.0" |> should equal 1.0

    [<Test>]
    member _.testDeserializationExtreneCase () =
        (fun () -> (ignore << fromWordR) "oo") |> should throw typeof<FormatException>

    [<TestCase(1.65555555)>]
    [<TestCase(-3.14)>]
    [<TestCase(0)>]
    member _.testAdditionOppositeElement x =
        realSemiring.Add x -x =! realSemiring.IdentityElement |> should equal true

    [<TestCase(1.65555555)>]
    [<TestCase(-3.14)>]
    [<TestCase(0.0)>]
    member _.testAdditionIdentityElement x =
        realSemiring.Add x realSemiring.IdentityElement =! realSemiring.Add realSemiring.IdentityElement x |> should equal true

    [<TestCase(1.01010101, 0.0)>]
    [<TestCase(-3.14, 7.77)>]
    [<TestCase(-6.5, 6.5)>]
    member _.testAddition (x, y) =
        // он же realSemigroupWithPartialOrder.Multiply
        realSemiring.Add x y =! x + y |> should equal true

    [<TestCase(1.01010101, 0.0)>]
    [<TestCase(-3.14, 7.77)>]
    [<TestCase(-6.5, 6.5)>]
    member _.testMultiplication (x, y) =
        realSemiring.Multiply x y =! x * y |> should equal true

    [<TestCase(1.01010101, 0.0)>]
    [<TestCase(-3.14, 7.77)>]
    [<TestCase(-6.5, 6.5)>]
    member _.testLessOrEqual (x, y) =
        realSemigroupWithPartialOrder.Le x y |> should equal <| (x <= y)