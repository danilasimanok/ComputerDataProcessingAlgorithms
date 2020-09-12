module Integer

open FsMatrix.Matrix

let integerSemigroupWithPartialOrder = {Multiply = (+); Le = (<=)}

let integerSemiring = {IdentityElement = 0; Add = (+); Multiply = (*)}

let toWordI = string

let fromWordI (w : string) = int w