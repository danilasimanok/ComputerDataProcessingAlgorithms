open Matrix.Matrix
open Matrix.MatrixIO
open Matrix.Maybe


let sr = {IdentityElement = 0; Add = (+); Multiply = (*)}

let maybe = new MaybeBuilder()

let readMatrix src = maybe {
    let! rows = readRowsList int src
    let! matrix = fromRowsList rows
    return matrix
    }

[<EntryPoint>]
let main argv =
    let argsErr () = printfn "Not enough arguments."
    match argv with
    | [||] -> argsErr()
    | [|_|] -> argsErr()
    | [|_ ; _|] -> argsErr()
    | [|m1src ; m2src; dst|] -> 
        let result = maybe {
            let! matrix1 = readMatrix m1src
            let! matrix2 = readMatrix m2src
            let! result = multiply sr matrix1 matrix2
            return result
        }
        let success =
            match result with
            | None -> false
            | Some matrix -> writeRowsList string (toRowsList matrix) dst
        if success
        then ()
        else printfn "Something went wrong."
    | _ -> printfn "Too much arguments."
    0
