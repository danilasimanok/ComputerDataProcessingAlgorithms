module Types

type ElementType =
    | REAL
    | EXTENDED_REAL
    | BOOLEAN
    | INTEGER

type ExtendedReal =
    | Real of float
    | Infinity
    | Indeterminacy

let toWord r =
    match r with
    | Real r -> string r
    | Infinity -> "oo"
    | Indeterminacy -> "?"