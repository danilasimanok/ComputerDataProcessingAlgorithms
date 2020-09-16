module Main

open Argu
open Types
open Random
open FsMatrix.MatrixIO
open System.IO
open Boolean
open Integer
open Real
open ExtendedReal

type Arguments =
    | [<Mandatory>] T of elementType : ElementType
    | [<Mandatory>] S of size : int
    | [<Mandatory>] P of path : string
    | [<Mandatory>] C of count : int
    with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | T _ -> "specify type of matrix elements. (REAL | EXTENDED_REAL | BOOLEAN | INTEGER)"
            | S _ -> "specify size of matrix."
            | P _ -> "specify where matrix should be created."
            | C _ -> "specify count of matrices to create."

let parser = ArgumentParser.Create<Arguments>(programName = "MatrixProcessor")

let toString et =
    match et with
    | REAL -> "real"
    | EXTENDED_REAL -> "extended_real"
    | BOOLEAN -> "boolean"
    | INTEGER -> "integer"

[<EntryPoint>]
let main argv =
    try
        let results = parser.ParseCommandLine argv
        let elementType, size, path, count =
            results.GetResult T, results.GetResult S, results.GetResult P, results.GetResult C
        let range = {1 .. count}
        let path = Path.Combine [|path; toString elementType; string size|]
        (ignore << Directory.CreateDirectory) path
        let path = Path.Combine (path, "matrix")
        let action x =
            match elementType with
            | REAL -> writeRowsList toWordR (randomLists randomReal size) (path + string x)
            | EXTENDED_REAL -> writeRowsList toWordER (randomLists randomExtendedReal size) (path + string x)
            | BOOLEAN -> writeRowsList toWordB (randomLists randomBoolean size) (path + string x)
            | INTEGER -> writeRowsList toWordI (randomLists randomInteger size) (path + string x)
        Seq.iter action range
    with e -> eprintfn "%s" e.Message
    0