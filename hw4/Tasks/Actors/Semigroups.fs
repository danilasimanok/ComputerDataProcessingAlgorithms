module Semigroups

open FsMatrix.Matrix
open Types
open ExtendedReal

let booleanSemigroup = {Multiply = (&&); Le = fun x y -> x || not y}

let integerSemigroup = {Multiply = (+); Le = (<=)}

let realSemigroup : SemigroupWithPartialOrder<float> = {Multiply = (+); Le = (<=)}

let internal le x y =
    match x, y with
    | Indeterminacy, _ -> false
    | _, Indeterminacy -> true
    | Infinity, _ -> false
    | _, Infinity -> true
    | Real x, Real y -> x <= y

let extendedRealSemigroup = {Multiply = extendedRealSemiring.Add; Le = le}