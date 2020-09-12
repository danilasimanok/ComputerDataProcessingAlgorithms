module Railway

type Result<'ContentType, 'ErrorMessageType> =
    | Success of 'ContentType
    | Failure of 'ErrorMessageType list

let bind f =
    fun x ->
        match x with
        | Success x -> f x
        | Failure msgs -> Failure msgs

let map f =
    fun x ->
        match x with
        | Success x -> (Success << f) x
        | Failure msgs -> Failure msgs

let tee f =
    fun x ->
        f x
        x

let noExns f exnsToErrs =
    fun x ->
        try
            (Success << f) x
        with e ->
            (Failure << exnsToErrs) e

let internal combine (f : 'a -> Result<'a, 'b>) (g : 'a -> Result<'a, 'b>) =
    fun x ->
        match f x, g x with
        | Success _, Success _ -> Success x
        | Success _, Failure msgs -> Failure msgs
        | Failure msgs, Success _ -> Failure msgs
        | Failure msgs1, Failure msgs2 -> (Failure << List.append msgs1) msgs2

let combineChecks checks = List.reduce combine checks

let couple a b =
    match a, b with
    | Success a, Success b -> Success (a, b)
    | Success _, Failure msgs -> Failure msgs
    | Failure msgs, Success _ -> Failure msgs
    | Failure msgs1, Failure msgs2 -> (Failure << List.append msgs1) msgs2

let triple a b c =
    match couple a (couple b c) with
    | Success (a, (b, c)) -> Success (a, b, c)
    | Failure msgs -> Failure msgs

let quadruple a b c d =
    match couple a (triple b c d) with
    | Success (a, (b, c, d)) -> Success (a, b, c, d)
    | Failure msgs -> Failure msgs