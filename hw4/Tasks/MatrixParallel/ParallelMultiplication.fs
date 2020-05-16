module MatrixParallel

open FsMatrix.Matrix
open Array.Parallel

let internal len lists = (List.length << List.head) lists

let multiply (sr : Semiring<'a>) a b =
    let m1 = toRowsList a
    let m2 = toColumnsList b
    let multiply (x, y) = sr.Multiply x y
    let sizesAreCorrect = len m1 = len m2
    
    let multiplyRC row =
        List.fold sr.Add sr.IdentityElement << List.map multiply << List.zip row
    
    let multiplyWithColumns row = (List.ofArray << map (multiplyRC row) << Array.ofList) m2
    
    if sizesAreCorrect
    then (Some << Rows << List.map multiplyWithColumns) m1
    else None