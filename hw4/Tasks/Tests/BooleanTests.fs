module BooleanTests

open NUnit.Framework
open FsUnit
open Boolean
open System

[<TestFixture>]
type BooleanTests () =
    
    [<Test>]
    member _.testSerialization () =
        toWordB true |> should equal "t"
        toWordB false |> should equal "f"

    [<Test>]
    member _.testDeserialization () =
        fromWordB "t" |> should equal true
        fromWordB "f" |> should equal false

    [<Test>]
    member _.testDeserializationExtreneCase () =
        (fun () -> (ignore << fromWordB) "lol") |> should throw typeof<FormatException>

    [<Test>]
    member _.testAddition () =
        booleanSemiring.Add true true |> should equal true
        booleanSemiring.Add true false |> should equal true
        booleanSemiring.Add false true |> should equal true
        booleanSemiring.Add false false |> should equal false

    [<Test>]
    member _.testMultiplication () =
        booleanSemiring.Multiply true true |> should equal true
        booleanSemiring.Multiply true false |> should equal false
        booleanSemiring.Multiply false true |> should equal false
        booleanSemiring.Multiply false false |> should equal false