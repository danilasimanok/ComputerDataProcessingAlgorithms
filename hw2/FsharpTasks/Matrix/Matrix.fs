namespace Matrix

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
    
    let transpose lists =
        let rec f lists acc res_acc new_lists =
            match lists with
            | [] -> f (List.rev new_lists) [] ((List.rev acc)::res_acc) []
            | hd :: tl ->
                match hd with
                | [] -> List.rev res_acc
                | hd' :: tl' ->
                    f tl (hd'::acc) res_acc (tl'::new_lists)
        f lists [] [] []

    let internal uncurry f (x, y) = f x y

    let internal multiplyRC sr row column = List.fold sr.Add sr.IdentityElement (List.map (uncurry sr.Multiply) (List.zip row column))

    let internal multiplyWithColumns sr columns row = List.map (multiplyRC sr row) columns

    let toRowsList matrix =
        match matrix with
        | Rows(lists) -> lists
        | Columns(lists) -> transpose lists

    let mulpiply sr a b =
        let m1 = toRowsList a
        let m2 =
            match b with
            | Rows(rows) -> transpose rows
            | Columns(columns) -> columns
        Rows(List.map (multiplyWithColumns sr m2) m1)

    type SemigroupWithPartialOrder<'a> =
        {
        Multiply: ('a -> 'a -> 'a);
        Infty: 'a;
        Le: ('a -> 'a -> bool)
        }

    (*let floyd_warshall sg rows =
        
        let min_sum =
            let min x y = if sg.Le x y then x else y
            List.fold (fun c' (x, y) -> min c' (sg.Multiply x y))
        
        let columns = transpose rows
        
        let rec f rows result =
            match rows with
            | [] -> List.rev result
            | row :: tl ->
                let zip_with_row = List.zip row
                let columns_zipped = zip_with_row columns
                let mins = List.map (uncurry min_sum) (zip_with_row (zip_with_row columns))
                f tl (mins :: result)
        
        f rows []

module FsharpMatrixConvertor =
    
    exception MatrixSizeException of string

    let toMatrix seqs =
        let rowsList = List.ofSeq >> List.map List.ofSeq <| seqs
        match List.length rowsList with
        | 0 -> raise <| MatrixSizeException "Размер матрицы должен быть ненулевым."
        | _ ->
            let length = List.length << List.head <| rowsList
            if length = 0
            then raise <| MatrixSizeException "Размер матрицы должен быть ненулевым."
            elif List.forall (fun x -> length = List.length x) rowsList
            then Matrix.Rows(rowsList)
            else raise <| MatrixSizeException "Размеры строк должны быть одинаковыми"
*)