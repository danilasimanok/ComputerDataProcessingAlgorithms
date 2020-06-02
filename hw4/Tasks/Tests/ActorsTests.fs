module ActorsTests

open Creator
open Reader
open Writer
open Multiplier
open TransitiveClosure
open NUnit.Framework
open FsUnit
open FsMatrix.Matrix

[<TestFixture>]
type ActorsTests () =
    
    let creators = createCreators ()
    let readers = createReaders ()
    let writers = createWriters ()
    let multipliers = createMultipliers ()
    let transitiveClosures = createTransitiveClosures ()
    let path = @"../../../../../TestData/"
    let read path =
        (Async.RunSynchronously << readers.BoolReader.PostAndAsyncReply) (fun r -> ReadMatrix (path, r))
    let write matrix path =
        (Async.RunSynchronously << writers.BoolWriter.PostAndAsyncReply) (fun r -> WriteMatrix (path, matrix, r))
    let m1, m2 =
        Rows [[false; true]; [true; false]], Rows [[true; true]; [true; true]]
    let mul m1 m2 =
        (Async.RunSynchronously << multipliers.BoolMultiplier.PostAndAsyncReply) (fun r -> MultiplyMatrices (m1, m2, r))

    [<Test>]
    member _.testCreator () =
        (ignore << Async.RunSynchronously << creators.BoolCreator.PostAndAsyncReply) (fun r -> CreateMatrix (2, r))
        Assert.Pass ()

    [<Test>]
    member _.testReaderCommonCase () =
        let reply = read (path + "matrix")
        reply |> should equal <| Some m1

    [<Test>]
    member _.tryToReadIncorrectTable () =
        read (path + "incorrect1") |> should equal None

    [<Test>]
    member _.tryToReadIncorrectWords () =
        read (path + "incorrect2") |> should equal None

    [<Test>]
    member _.testWriterCommonCase () =
        write m1 (path + "test") |> should equal true

    [<Test>]
    member _.testWriterExtremeCase () =
        write m1 (path + @"\s/\/") |> should equal false

    [<Test>]
    member _.testMultiplierCommonCase () =
        mul m1 m2 |> should equal <| Some m2

    [<Test>]
    member _.testMultiplierExtremeCase () =
        let m = Rows [[true]]
        mul m1 m |> should equal None

    [<Test>]
    member _.testTRC () =
        let result = (Async.RunSynchronously << transitiveClosures.BoolTransitiveClosure.PostAndAsyncReply) (fun r -> TRC (m1, r))
        result |> should equal m2

    [<OneTimeTearDown>]
    member _.tearDown () =
        let reports = [creators.BoolCreator.PostAndAsyncReply Creator.Exit; creators.IntCreator.PostAndAsyncReply Creator.Exit;
        creators.RealCreator.PostAndAsyncReply Creator.Exit; creators.ExtendedRealCreator.PostAndAsyncReply Creator.Exit;
        readers.BoolReader.PostAndAsyncReply Reader.Exit; readers.IntReader.PostAndAsyncReply Reader.Exit;
        readers.RealReader.PostAndAsyncReply Reader.Exit; readers.ExtendedRealReader.PostAndAsyncReply Reader.Exit;
        writers.BoolWriter.PostAndAsyncReply Writer.Exit; writers.IntWriter.PostAndAsyncReply Writer.Exit;
        writers.RealWriter.PostAndAsyncReply Writer.Exit; writers.ExtendedRealWriter.PostAndAsyncReply Writer.Exit;
        multipliers.BoolMultiplier.PostAndAsyncReply Multiplier.Exit; multipliers.IntMultiplier.PostAndAsyncReply Multiplier.Exit;
        multipliers.RealMultiplier.PostAndAsyncReply Multiplier.Exit; multipliers.ExtendedRealMultiplier.PostAndAsyncReply Multiplier.Exit;
        transitiveClosures.BoolTransitiveClosure.PostAndAsyncReply TransitiveClosure.Exit; transitiveClosures.IntTransitiveClosure.PostAndAsyncReply TransitiveClosure.Exit;
        transitiveClosures.RealTransitiveClosure.PostAndAsyncReply TransitiveClosure.Exit; transitiveClosures.ExtendedRealTransitiveClosure.PostAndAsyncReply TransitiveClosure.Exit]
        List.iter (ignore << Async.RunSynchronously) reports