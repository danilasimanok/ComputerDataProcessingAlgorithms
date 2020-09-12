module ActorsTests

open NUnit.Framework
open ConcreteActors
open Messages
open FsUnit
open MyFailings
open FsMatrix.Matrix
open System.IO

[<TestFixture>]
type actorsTests () =
    
    let boolActors = boolActors ()
    let awaitCreator = Async.RunSynchronously << boolActors.Creator.PostAndAsyncReply
    let awaitWriter = Async.RunSynchronously << boolActors.Writer.PostAndAsyncReply
    let awaitReader = Async.RunSynchronously << boolActors.Reader.PostAndAsyncReply
    let awaitMultiplier = Async.RunSynchronously << boolActors.Multiplier.PostAndAsyncReply
    let path = @"../../../../../TestData/"
    let wrongPath = path + @"dont/exist"
    let m1 = Rows [[false; true]; [true; false]]
    let m2 = Rows [[true; false]; [false; true]]
    let m3 = Rows [[true; true]; [true; true]]
    let checkSuccess answer = answer |> should be (ofCase <@Success@>)
    let checkAbsense path = File.Exists path |> should equal false
    let checkBadPath answer = answer |> should equal <| Failure [BadPath wrongPath]

    [<Test>]
    member _.testCreationCommonCase () =
        let answer = awaitCreator (fun r -> Create (4, r))
        checkSuccess answer

    [<Test>]
    member _.testCreationExtremeCase () =
        let answer = awaitCreator (fun r -> Create (-3, r))
        answer |> should equal <| Failure [SizeWasNotPositive -3]

    // поведение одинаково для всех акторов
    [<Test>]
    member _.testUnexpectedMessage () =
        let answer = awaitCreator (fun r -> Read ("", r))
        answer |> should equal <| Failure [UnexpectedMessage "Read"]

    (*
    // поведение одинаково для всех акторов
    [<Test>]
    member _.testDeath () =
        let answer = awaitCreator (fun r -> Die r)
        answer |> should equal Dead
    *)

    [<Test>]
    member _.testWritingCommonCase () =
        let path = path + "test"
        let answer = awaitWriter (fun r -> Write (m1, path, r))
        checkSuccess answer
        File.Exists path |> should equal true

    [<Test>]
    member _.testWritingExtremeCase () =
        let answer = awaitWriter (fun r -> Write (m1, wrongPath, r))
        checkBadPath answer
        checkAbsense wrongPath

    // поведение одинаково для всех обрабатывающих акторов
    [<Test>]
    member _.testWrongArgument () =
        let path = path + "unit"
        let answer = awaitWriter (fun r -> Write ((), path, r))
        answer |> should equal <| Failure [AnotherTypeRequired "bool"]
        checkAbsense path

    [<Test>]
    member _.testReadingCommonCase () =
        let path = path + "matrix"
        let answer = awaitReader (fun r -> Read (path, r))
        let expected = Success m1
        answer |> should equal expected

    [<Test>]
    member _.testReadingIncorrectMatrix () =
        let path = path + "incorrect"
        let answer = awaitReader (fun r -> Read (path, r))
        answer |> should equal <| Failure [MatrixIsIncorrect]

    [<Test>]
    member _.testReadingExtremeCase () =
        let answer = awaitReader (fun r -> Read (wrongPath, r))
        checkBadPath answer

    [<Test>]
    member _.testMultiplyingCommonCase () =
        let answer = awaitMultiplier (fun r -> Multiply (m1, m1, r))
        let expected = Success m2
        answer |> should equal expected

    [<Test>]
    member _.testMultiplyingExtremeCase () =
        let m = Rows [[true]]
        let answer = awaitMultiplier (fun r -> Multiply (m, m1, r))
        answer |> should equal <| Failure [SizesAreIncorrect]

    // у TRCExecutor не может возникнуть ошибки
    [<Test>]
    member _.testTRCExecutor () =
        let answer = (Async.RunSynchronously << boolActors.TRCExecutor.PostAndAsyncReply) (fun r -> ExecuteTRC (m1, r))
        let expected = Success m3
        answer |> should equal expected