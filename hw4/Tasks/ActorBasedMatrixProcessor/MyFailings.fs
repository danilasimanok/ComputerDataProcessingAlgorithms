module MyFailings

type MyFailing =
    | TypeDoesNotExist
    | IntegerParameterIsIncorrect
    | VariableDoesNotExists
    | TypesDoesNotMatch
    | ActorWasDead
    | PromiseIsNotExpected
    | UnexpectedMessage of string
    | SizeWasNotPositive of int
    | AnotherTypeRequired of string
    | BadPath of string
    | MatrixIsIncorrect
    | SizesAreIncorrect

let internal badProgrammer = "[PROGRAMMER DO NOT WORK WELL]"

let failingToString failing =
    match failing with
    | TypeDoesNotExist -> "Type does not exists."
    | IntegerParameterIsIncorrect -> "Integer parameter is incorrect."
    | VariableDoesNotExists -> "Variable does not exist."
    | TypesDoesNotMatch -> "Types does not match."
    | ActorWasDead -> "Actor working on variable was dead."
    | PromiseIsNotExpected -> badProgrammer + " function do not expect VariableInformation.Promise."
    | UnexpectedMessage msg -> badProgrammer + " actor do not expect a message " + msg + "."
    | SizeWasNotPositive size -> "Matrix size equals to " + string size + ", while it must be positive." 
    | AnotherTypeRequired msg -> badProgrammer + " actor expected Matrix<" + msg + ">."
    | BadPath msg -> "Can not perform I/O operation with " + msg + "."
    | MatrixIsIncorrect -> "File contains incorrect matrix."
    | SizesAreIncorrect -> "Matrices have different sizes."