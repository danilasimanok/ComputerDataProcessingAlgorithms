module SemigroupsTests

open NUnit.Framework
open FsUnit
open FsCheck.NUnit
open Semigroups
open Types

[<TestFixture>]
type BoolTests () =
    
    [<Property>]
    member _.testMul (x : bool, y : bool) =
        booleanSemigroup.Multiply x y = (x && y)

    [<Test>]
    member _.testLe () =
        booleanSemigroup.Le false false |> should equal true
        booleanSemigroup.Le false true |> should equal false
        booleanSemigroup.Le true false |> should equal true
        booleanSemigroup.Le true true |> should equal true

[<TestFixture>]
type IntTests () =
    
    [<Property>]
    member _.testMul (x : int, y : int) =
        integerSemigroup.Multiply x y = x + y

    [<Property>]
    member _.testLE (x : int, y : int) =
        integerSemigroup.Le x y = (x <= y)

[<TestFixture>]
type RealTests () =
    
    [<Property>]
    member _.testMul (x : float, y : float) =
        realSemigroup.Multiply x y |> should equal (x + y)

    [<Property>]
    member _.testLe (x : float, y : float) =
        realSemigroup.Le x y = (x <= y)

type ExtendedRealTests () =
    
    let x, y, inf, ind = Real 1.0, Real 3.14, Infinity, Indeterminacy

    member _.testMul () =
        extendedRealSemigroup.Multiply x y |> should equal <| Real 4.14
        extendedRealSemigroup.Multiply ind x |> should equal Indeterminacy
        extendedRealSemigroup.Multiply inf y |> should equal Infinity
        extendedRealSemigroup.Multiply inf ind |> should equal Indeterminacy

    member _.testLe () =
        extendedRealSemigroup.Le x y |> should equal true
        extendedRealSemigroup.Le y x |> should equal false
        extendedRealSemigroup.Le x inf |> should equal true
        extendedRealSemigroup.Le inf y |> should equal false
        extendedRealSemigroup.Le ind x |> should equal false
        extendedRealSemigroup.Le ind inf |> should equal false