module Actors

open Messages
open MyFailings
open FsMatrix
open Random
open Railway
open GeneralActor

let creator randomElement =
    let f msg =
        match msg with
        | Create (size, r) ->
            let result =
                if size > 0
                then (Result.Success << Option.get << Matrix.fromRowsList << randomLists randomElement) size
                else Result.Failure [SizeWasNotPositive size]
            Some (result, r)
        | _ -> None
    creativeActor f

let writer t cast toWord =
    let f cast toWord msg =
        match msg with
        | Write (matrix, path, r) ->
            let write = 
                let write (matrix, path) =
                    MatrixIO.writeRowsList toWord (Matrix.toRowsList matrix) path
                let exnToErr _ = [BadPath path]
                noExns write exnToErr
            let matrix, path = cast matrix, Success path
            let writing = (bind write) (couple matrix path)
            Some (writing, r)
        | _ -> None
    processingActor t cast f toWord

let reader fromWord =
    let f msg =
        let getValue opt =
            match opt with
            | Some matrix -> Success matrix
            | None -> Failure [MatrixIsIncorrect]
        match msg with
        | Read (path, r) ->
            let read =
                let read () = 
                    (Matrix.fromRowsList << MatrixIO.readRowsList fromWord) path
                let exnToErr _ = [BadPath path]
                noExns read exnToErr
            let result = (bind getValue << read) ()
            Some (result, r)
        | _ -> None
    creativeActor f

let multiplier t cast sr =
    let f cast sr msg =
        let multiply (matrix1, matrix2) =
            match Matrix.multiply sr matrix1 matrix2 with
            | Some matrix -> Success matrix
            | None -> Failure [SizesAreIncorrect]
        match msg with
        | Multiply (matrix1, matrix2, r) ->
            let matrix1, matrix2 = cast matrix1, cast matrix2
            let result = (bind multiply) (couple matrix1 matrix2)
            Some (result, r)
        | _ -> None
    processingActor t cast f sr

let trcExecutor t cast sg =
    let f cast sg msg =
        let trc matrix =
            // У нас только квадратные матрицы.
            (Matrix.floydWarshall sg matrix).Value
        match msg with
        | ExecuteTRC (matrix, r) ->
            let result = (map trc << cast) matrix
            Some (result, r)
        | _ -> None
    processingActor t cast f sg