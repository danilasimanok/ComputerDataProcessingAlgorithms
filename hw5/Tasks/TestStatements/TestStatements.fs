module TestStatements

open OpenCL.Net
open Brahma.OpenCL
open FsMatrix.Matrix
open Integer

let inline getProvider platformName =
    try  ComputeProvider.Create(platformName, DeviceType.Default)
    with 
    | ex -> failwith ex.Message

let inline commandQueue provider = new CommandQueue(provider, provider.Devices |> Seq.head)

let a = Rows [[1; 2]; [3; 4]]
let b = Columns [[5; 7]; [6; 8]]
let commonCaseExpected = multiply integerSemiring a b
let c = Rows [[1]]

let inline testMultiplication platformName a b expected =
    let provider = getProvider platformName
    let mutable commandQueue = commandQueue provider
    let got = MatrixGPU.multiply provider commandQueue integerSemiring a b
    commandQueue.Dispose ()
    provider.CloseAllBuffers ()
    provider.Dispose ()
    expected = got

let fstStatement platformName =
    testMultiplication platformName a b commonCaseExpected

let sndStatement platformName =
    testMultiplication platformName a c None