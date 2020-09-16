module Messages

open MyFailings

type Answer =
    | Success of result : obj
    | Failure of message : MyFailing list
    | Dead

type ReplyChannel = AsyncReplyChannel<Answer>

type Command =
    | Create of size : int * r : ReplyChannel
    | Write of matrix : obj * path : string * r : ReplyChannel
    | Read of path : string * r : ReplyChannel
    | Multiply of matrix1 : obj * matrix2 : obj * r : ReplyChannel
    | ExecuteTRC of matrix : obj * r : ReplyChannel
    | Die of r : ReplyChannel

let getReplyChannel cmd =
    match cmd with
    | Create (_, r) -> r
    | Write (_, _, r) -> r
    | Read (_, r) -> r
    | Multiply (_, _, r) -> r
    | ExecuteTRC (_, r) -> r
    | Die r -> r

let cmdToStr cmd =
    match cmd with
    | Create _ -> "Create"
    | Write _ -> "Write"
    | Read _ -> "Read"
    | Multiply _ -> "Multiply"
    | ExecuteTRC _ -> "ExecuteTRC"
    | Die _ -> "Die"