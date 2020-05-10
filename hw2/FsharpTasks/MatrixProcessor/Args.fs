module Args

open Argu

type Task =
    | MUL
    | APSP
    | TRC

type Argument = 
    | [<Mandatory>] Task of task : Task
    | [<MainCommand; ExactlyOnce; Last>] Matrices of paths : string list
    with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Task _ -> "specify what is to be done. (TRC | APSP | MUL)"
            | Matrices _ -> "specify paths to matrices."