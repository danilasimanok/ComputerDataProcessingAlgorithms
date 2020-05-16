module Boolean

open FsMatrix.Matrix
open System

let booleanSemiring = {IdentityElement = false; Add = (||); Multiply = (&&)}

let toWordB f = if f then "t" else "f"

let fromWordB w =
    if w = "t"
    then true
    elif w = "f"
    then false
    else raise (FormatException("Boolean could be restored only from 't' or 'f'."))