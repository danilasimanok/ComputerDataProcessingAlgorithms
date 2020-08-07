module ExtendedRealTests

open NUnit.Framework
open FsUnit
open ExtendedReal
open System

let xs = Indeterminacy :: Infinity :: List.map Real [1.65555555; -3.14; 0.0]
let inds = List.map (fun _ -> Indeterminacy) xs

[<TestFixture>]
type ExtendedRealTests () =

    [<Test>]
    member _.testSerialization () =
        (toWordER << Real) 1.0 |> should equal "1"

    [<Test>]
    member _.testDeserialization () =
        fromWordER "1.0" |> should equal (Real 1.0)
        fromWordER "?" |> should equal Indeterminacy
        fromWordER "oo" |> should equal Infinity

    [<Test>]
    member _.testDeserializationExtreneCase () =
        (fun () -> (ignore << fromWordER) "ooo") |> should throw typeof<FormatException>

    [<TestCase(1.65555555)>]
    [<TestCase(-3.14)>]
    [<TestCase(0.0)>]
    member _.testAdditionOppositeElement x =
        let x = Real x
        extendedRealSemiring.Add x (reversed x) =! extendedRealSemiring.IdentityElement |> should equal true

    [<Test>]
    member _.testAdditionOppositeElementExtremeCases () =
        extendedRealSemiring.Add Infinity (reversed Infinity) =! Infinity |> should equal true
        let ind = extendedRealSemiring.Add Indeterminacy (reversed Indeterminacy)
        ind =! Indeterminacy |> should equal false
        ind |> should equal Indeterminacy
    
    [<TestCase(1.65555555)>]
    [<TestCase(-3.14)>]
    [<TestCase(0.0)>]
    member _.testAdditionIdentityElement x =
        let x = Real x
        extendedRealSemiring.Add x extendedRealSemiring.IdentityElement =! extendedRealSemiring.Add extendedRealSemiring.IdentityElement x |> should equal true

    [<Test>]
    member _.testAdditionIdentityElementExtremeCases () =
        extendedRealSemiring.Add Infinity extendedRealSemiring.IdentityElement =! extendedRealSemiring.Add extendedRealSemiring.IdentityElement Infinity |> should equal true
        extendedRealSemiring.Add Indeterminacy extendedRealSemiring.IdentityElement |> should equal Indeterminacy
        extendedRealSemiring.Add extendedRealSemiring.IdentityElement Indeterminacy |> should equal Indeterminacy

    [<TestCase(1.01010101, 0.0)>]
    [<TestCase(-3.14, 7.77)>]
    [<TestCase(-6.5, 6.5)>]
    member _.testAddition (x, y) =
        let x', y' = Real x, Real y
        extendedRealSemiring.Add x' y' =! Real (x + y) |> should equal true

    [<Test>]
    member _.testAdditionWithInfty () =
        List.map (extendedRealSemiring.Add Infinity) xs |> should equal <| Indeterminacy :: List.init (List.length xs - 1) (fun _ -> Infinity)

    member _.testAdditionWithInd () =
        List.map (extendedRealSemiring.Add Indeterminacy) xs |> should equal <| inds

    [<TestCase(1.01010101, 0.0)>]
    [<TestCase(-3.14, 7.77)>]
    [<TestCase(-6.5, 6.5)>]
    member _.testMultiplication (x, y) =
        let x', y' = Real x, Real y
        extendedRealSemiring.Multiply x' y' =! Real(x * y) |> should equal true

    [<Test>]
    member _.testMultiplicationWithInfty () =
        extendedRealSemiring.Multiply (Real 1.2278) Infinity =! Infinity |> should equal true
        extendedRealSemiring.Multiply Infinity (Real 1.2278) =! Infinity |> should equal true
        extendedRealSemiring.Multiply Infinity Infinity =! Infinity |> should equal true

    [<Test>]
    member _.testMultiplicationWithInd () =
        List.map (extendedRealSemiring.Multiply Indeterminacy) xs |> should equal inds