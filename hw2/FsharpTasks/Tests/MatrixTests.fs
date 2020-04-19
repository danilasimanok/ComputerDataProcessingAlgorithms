module Tests

open NUnit.Framework
open FsUnit

open Matrix

[<TestFixture>]
type MatrixTests() =
    
    let sr = {IdentityElement = 0; Add = (+); Multiply = (*)}
    let sg = {Multiply = (+); Le = (<=)}

    let multiply = multiply sr
    let floydWarshall = floydWarshall sg

    [<Test>]
    member _.transposeTest () =
        let lists = [["a1"; "a2"; "a3"]; ["b1"; "b2"; "b3"]; ["c1"; "c2"; "c3"]]
        let expected = [["a1"; "b1"; "c1"]; ["a2"; "b2"; "c2"]; ["a3"; "b3"; "c3"]]
        (transpose <| Rows lists) |> should equal <| (Rows <| expected)

    [<Test>]
    member _.testMultiplyCommonCase () =
        let result = multiply (Rows([[1; 2; 3]])) (Columns([[4; 5; 6]]))
        result |> should equal <| (Some <| Rows [[32]])

    [<Test>]
    member _.testInvalidMultiplication () =
        let matrix = Rows [[1; 2; 3]; [4; 5; 6]]
        multiply matrix matrix |> should equal None

    [<Test>]
    member _.testMatrixCreatorCommonCase () =
        let lists = [[1; 2; 3]; [4; 5; 6]]
        let m = fromRowsList lists
        m |> should equal <| (Some <| Rows lists)

    [<Test>]
    member _.testMatricesWithoutElements () =
        let m1 = fromRowsList []
        let m2 = fromRowsList [[]]
        m1 |> should equal m2
        m1 |> should equal None

    [<Test>]
    member _.testInvalidMatrixCreation () =
        let m = fromRowsList [[1; 2; 3]; [4; 5]; [6; 7; 8]]
        m |> should equal None

    [<Test>]
    member _.testFloydWarshallCommonCase () =
        let matrix = Rows [[0; 9; 2]; [1; 0; 4]; [2; 4; 0]]
        let expected = Rows [[0; 6; 2]; [1; 0; 3]; [2; 4; 0]]
        floydWarshall matrix |> should equal <| Some expected

    [<Test>]
    member _.testFloydWarshallWithInvalidArguments () =
        Rows [[1; 2; 3]; [4; 5; 6;]]|> floydWarshall |> should equal None