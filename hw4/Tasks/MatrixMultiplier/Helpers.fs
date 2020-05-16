module Helpers

open FsMatrix.Matrix
open FsMatrix.MatrixIO

type Operations<'a> =
    {
    ReadMatrix : string -> Matrix<'a> option;
    WriteMatrix : Matrix<'a> -> string -> unit;
    Semiring : Semiring<'a>;
    }

let helpers f g sr =
    {ReadMatrix = fromRowsList << (readRowsList f); WriteMatrix = toRowsList >> (writeRowsList g); Semiring = sr}