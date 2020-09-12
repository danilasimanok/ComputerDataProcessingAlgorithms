module ExtendedReal

open FsMatrix.Matrix
open Real

type ExtendedReal =
    | Indeterminacy
    | Infinity
    | Real of y : float

let internal add x y =
    match x, y with
    | Indeterminacy, _ -> Indeterminacy
    | _, Indeterminacy -> Indeterminacy
    | Infinity, _ -> Infinity
    | _, Infinity -> Infinity
    | Real x, Real y -> Real (x + y)

let inline reversed x =
    match x with
    | Real x -> Real -x
    | _ -> x

let internal infMulReal x = if x =! 0.0 then Indeterminacy else Infinity

let internal multiply x y =
    match x, y with
    | Indeterminacy, _ -> Indeterminacy
    | _, Indeterminacy -> Indeterminacy
    | Infinity, Infinity -> Infinity
    | Infinity, Real x -> infMulReal x
    | Real x, Infinity -> infMulReal x
    | Real x, Real y -> Real (x * y)

let inline (=!) x y =
    match x, y with
    | Indeterminacy, _ -> false
    | _, Indeterminacy -> false
    | Infinity, Infinity -> true
    | Infinity, _ -> false
    | _, Infinity -> false
    | Real x, Real y -> x =! y

let internal le x y =
    match x, y with
    | Indeterminacy, _ -> false
    | _, Indeterminacy -> false
    | Infinity, Infinity -> true
    | Infinity, _ -> false
    | _, Infinity -> true
    | Real x, Real y -> x <= y

let extendedRealSemigroupWithPartialOrder = {Multiply = add; Le = le}

let extendedRealSemiring = {IdentityElement = Real 0.0; Add = add; Multiply = multiply}

let toWordER x =
    match x with
    | Indeterminacy -> "?"
    | Infinity -> "oo"
    | Real x -> string x

let fromWordER w =
    if w = "?"
    then Indeterminacy
    elif w = "oo"
    then Infinity
    else (Real << float) w