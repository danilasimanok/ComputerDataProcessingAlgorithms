module MatrixGPU

open Brahma.OpenCL
open Brahma.FSharp.OpenCL.Core
open FsMatrix.Matrix

let pairwisedProducts mul =
    <@
        fun (r : _3D) (rows : array<array<_>>) (columns : array<array<_>>) (products : array<array<array<_>>>) ->
            let i, j, k = r.GlobalID0, r.GlobalID1, r.GlobalID2
            products.[i].[j].[k] <- mul rows.[j].[k] columns.[i].[k]
    @>

let prepareForSum neutral =
    <@
        fun (r : _2D) len (products : array<array<array<_>>>) ->
            let i, j = r.GlobalID0, r.GlobalID1
            products.[i].[j].[len] <- neutral
    @>

let summarize add =
    <@
        fun (r : _3D) (products : array<array<array<_>>>) ->
            let i, j, k = r.GlobalID0, r.GlobalID1, r.GlobalID2
            let k' = 2 * k
            products.[i].[j].[k] <- add products.[i].[j].[k'] products.[i].[j].[k' + 1]
    @>

let getResult () =
    <@
        fun (r : _2D) (products : array<array<array<_>>>) (result : array<array<_>>) ->
            let i, j = r.GlobalID0, r.GlobalID1
            result.[i].[j] <- products.[j].[i].[0]
    @>

let multiply (provider : ComputeProvider) (queue : Brahma.CommandQueue) sr a b =
    let len lists = (List.length << List.head) lists
    let even x = if x % 2 = 0 then x else x + 1

    let summarize = summarize sr.Add
    let pairwisedProducts = pairwisedProducts sr.Multiply
    let prepareForSum = prepareForSum sr.IdentityElement

    let _, pairwisedProductsPrepare, pairwisedProductsRun = provider.Compile pairwisedProducts
    let _, prepareForSumPrepare, prepareForSumRun = provider.Compile prepareForSum
    let _, summarizePrepare, summarizeRun = provider.Compile summarize
    let _, getResultPrepare, getResultRun = provider.Compile (getResult ())

    let a = toRowsList a
    let b = toColumnsList b
    let len1, len2 = len a, len b

    let rec sum n (products : array<array<array<_>>>) k (queue : Brahma.CommandQueue) =
        if k <> 1 then
            let k = even k
            let r = new _2D (n, n)
            prepareForSumPrepare r k products
            let r = new _3D (n, n, k)
            summarizePrepare r products
            let queue = queue.Add(prepareForSumRun ()).Add(summarizeRun ()) 
            sum n products (k / 2) queue
        else queue
    
    if len1 <> len2
    then None
    else
        // делаем матрицы массивами
        let toArray = Array.ofList << List.map Array.ofList
        let a, b = toArray a, toArray b
        // создаем буфер
        let create x = Array.create len1 x
        let len2 = even len2
        let products = (create << create << Array.create len2) sr.IdentityElement
        // получаем попарные произведения
        let r = new _3D (len1, len1, len1)
        pairwisedProductsPrepare r a b products
        let queue = queue.Add (pairwisedProductsRun ())
        // складываем попарные произведения
        let queue = sum len1 products len2 queue
        // создаем буфер для результата
        let result = (create << create) sr.IdentityElement
        // сохраняем результат в буфер
        let r = new _2D (len1, len1)
        getResultPrepare r products result
        let queue = queue.Add (getResultRun ())
        // заставляем ГПУ работать
        (ignore << queue.Finish) ()
        // делаем массив матрицей и возвращаем ее
        let lists = (List.ofArray << Array.map List.ofArray) result
        fromRowsList lists