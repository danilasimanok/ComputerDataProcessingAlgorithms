module Tests

open NUnit.Framework
open FsUnit
open System.IO
open System
open Main

[<TestFixture>]
type MatrixGeneratorTests() =
    
    let path = "matrices"
    let range = {1 .. 4}
    let sep = string Path.DirectorySeparatorChar

    [<Test>]
    member _.testCreation () =
        (ignore << main) [|"--t"; "BOOLEAN"; "--s"; "3"; "--p"; path; "--c"; "4"|]
        let path1 = path + sep + "boolean" + sep + "3" + sep + "matrix"
        Seq.iter (fun x -> File.Exists (path1 + string x) |> should equal true) range
        Directory.Delete (path, true)

    [<Test>]
    member _.testCreationExtremeCase () =
        use sw = new StringWriter ()
        Console.SetError sw
        (ignore << main) [|"--t"; "BOOLEAN"; "--s"; "3"; path; "--c"; "4"|]
        sw.ToString().Length |> should be (greaterThan 0)