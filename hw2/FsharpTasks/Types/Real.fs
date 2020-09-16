module Real

open FsMatrix.Matrix

let realSemigroupWithPartialOrder : SemigroupWithPartialOrder<float> = {Multiply = (+); Le = (<=)}

let realSemiring = {IdentityElement = 0.0; Add = (+); Multiply = (*)}

let toWordR = string

let fromWordR (w : string) = float w

let epsilon = 0.00001

let inline (=!) x y =
    abs (x - y) < epsilon