module Tests

open NUnit.Framework
open FsUnit

open Matrix.Matrix

[<Test>]
let Test1 () =
    [1;2;3] |> should equal [1;2;3]

let sr = {IdentityElement = 0; Add = (fun x y -> x + y); Multiply = (fun x y -> x * y)}

[<Test>]
let testMultRC () =
    let result = multiplyRC sr [1; 2; 3] [4; 5; 6]
    result |> should equal 32

[<Test>]
let testCommonCase () =
    let result = mulpiply sr (Rows([[1; 2; 3]])) (Columns([[4; 5; 6]]))
    result |> should equal (Rows([[32]]))