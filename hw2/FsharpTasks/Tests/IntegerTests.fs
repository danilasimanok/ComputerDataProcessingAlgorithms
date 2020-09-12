module IntegerTests

open NUnit.Framework
open FsUnit
open Integer
open System

[<TestFixture>]
type IntegerTests () =
    
    [<Test>]
    member _.testSerialization () =
        toWordI 1 |> should equal "1"

    [<Test>]
    member _.testDeserialization () =
        fromWordI "1" |> should equal 1

    [<Test>]
    member _.testDeserializationExtreneCase () =
        (fun () -> (ignore << fromWordI) "oo") |> should throw typeof<FormatException>

    [<TestCase(1)>]
    [<TestCase(-3)>]
    [<TestCase(0)>]
    member _.testAdditionOppositeElement x =
        integerSemiring.Add x -x |> should equal integerSemiring.IdentityElement

    [<TestCase(1)>]
    [<TestCase(-3)>]
    [<TestCase(0)>]
    member _.testAdditionIdentityElement x =
        integerSemiring.Add x integerSemiring.IdentityElement |> should equal <| integerSemiring.Add integerSemiring.IdentityElement x

    [<TestCase(1, 0)>]
    [<TestCase(-3, 7)>]
    [<TestCase(-6, 6)>]
    member _.testAddition (x, y) =
        // он же integerSemigroupWithPartialOrder.Multiply
        integerSemiring.Add x y |> should equal <| x + y

    [<TestCase(1, 0)>]
    [<TestCase(-3, 7)>]
    [<TestCase(-6, 6)>]
    member _.testMultiplication (x, y) =
        integerSemiring.Multiply x y |> should equal <| x * y

    [<TestCase(1, 0)>]
    [<TestCase(-3, 7)>]
    [<TestCase(-6, 6)>]
    member _.testLessOrEqual (x, y) =
        integerSemigroupWithPartialOrder.Le x y |> should equal <| (x <= y)