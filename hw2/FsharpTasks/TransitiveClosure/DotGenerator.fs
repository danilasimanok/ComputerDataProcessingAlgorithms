module DotGenerator

open Matrix

let generateDot origin result =
    
    let origin, result = toRowsList origin, toRowsList result

    let rec inner origin result orow rrow i j strlist =
        match orow with
        | [] ->
            match origin with
            | [] -> strlist
            | hd :: tl -> inner tl (List.tail result) hd (List.head result) (i + 1) 0 strlist
        | hd :: tl ->
            let hd_strlist = (string i) + " -> " + (string j) + " [" + (if hd then "" else "color=red") + "];"
            let next = inner origin result tl (List.tail rrow) i (j + 1)
            if List.head rrow
            then next (hd_strlist :: strlist)
            else next strlist

    let verticesCount = List.length origin

    let rec addVertices i strs =
        if i < verticesCount
        then addVertices (i + 1) (((string i) + " ;") :: strs)
        else strs

    let strs = (inner origin result [] [] -1 0 << addVertices 0) ["digraph G {"]

    (String.concat "\n" << List.rev) ("}" :: strs)