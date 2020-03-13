namespace Utils

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

    (* Не забудь реализовать. *)
    let columnsToRows columns = [[]]

    (* Не забудь реализовать. *)
    let rowsToColumns rows = [[]]

    let multiplyRC sr row column = List.fold sr.Add (List.map sr.Multiply (List.zip row column)) sr.IdentityElement

    let multiplyWithColumns sr columns row = List.map (multiplyRC sr row) columns

    let mulpiply sr a b =
        let m1 =
            match a with
            | Rows(rows) -> rows
            | Columns(columns) -> columnsToRows columns
        let m2 =
            match b with
            | Rows(rows) -> rowsToColumns rows
            | Columns(columns) -> columns
        List.map (multiplyWithColumns sr m2) m1