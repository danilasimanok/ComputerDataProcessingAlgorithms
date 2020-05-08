namespace FsMatrix

module Matrix = 
    
    type Matrix<'a> = 
        | Rows of 'a list list
        | Columns of 'a list list

    type Semiring<'a> =
        {
        IdentityElement: 'a;
        Add: ('a -> 'a -> 'a);
        Multiply: ('a -> 'a -> 'a)
        }
    
    let internal transposeLists lists =
        let rec f lists acc res_acc new_lists =
            match lists with
            | [] -> f (List.rev new_lists) [] ((List.rev acc)::res_acc) []
            | hd :: tl ->
                match hd with
                | [] -> List.rev res_acc
                | hd' :: tl' ->
                    f tl (hd'::acc) res_acc (tl'::new_lists)
        f lists [] [] []

    let transpose matrix =
        match matrix with
        | Rows lists -> (Rows << transposeLists) lists
        | Columns lists -> (Columns << transposeLists) lists

    let toRowsList matrix =
        match matrix with
        | Rows lists -> lists
        | Columns lists -> transposeLists lists

    let toColumnsList matrix =
        match matrix with
        | Rows lists -> transposeLists lists
        | Columns lists -> lists

    let internal len lists = (List.length << List.head) lists

    let multiply sr a b =
        let m1 = toRowsList a
        let m2 = toColumnsList b
        let multiply (x, y) = sr.Multiply x y
        let sizesAreCorrect = len m1 = len m2

        let multiplyRC row =
            List.fold sr.Add sr.IdentityElement << List.map multiply << List.zip row

        let multiplyWithColumns row = List.map (multiplyRC row) m2

        if sizesAreCorrect
        then (Some << Rows << List.map multiplyWithColumns) m1
        else None

    let fromRowsList list =
        
        let checkLengths length = List.forall ((=) length << List.length)

        match list with
        | [] -> None
        | hd :: _ ->
            let length = List.length hd
            if length = 0 || not (checkLengths length list)
            then None
            else (Some << Rows) list

    type SemigroupWithPartialOrder<'a> =
        {
        Multiply: ('a -> 'a -> 'a);
        Le: ('a -> 'a -> bool)
        }

    let floydWarshall sg matrix =
        
        let rows = toRowsList matrix
        let columns = toColumnsList matrix
        let min state (x, y) =
            let sum = sg.Multiply x y
            if sg.Le sum state
            then sum
            else state

        let inner columns row =
            let zip_with_row list = List.zip row list
            let state_pairs_list = (zip_with_row << List.map zip_with_row) columns
            let process_pair (state, pairs) = List.fold min state pairs

            List.map process_pair state_pairs_list

        if len rows = len columns
        then (Some << Rows << List.map (inner columns)) rows
        else None