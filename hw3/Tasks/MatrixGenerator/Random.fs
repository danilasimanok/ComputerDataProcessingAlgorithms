module Random

open System
open Types

let internal random = new Random ()

let internal randomNonNegativeReal () = random.NextDouble () * Double.MaxValue

let randomBoolean () = random.Next () % 2 = 0

let randomInteger () = random.Next (Int32.MinValue, Int32.MaxValue)

let randomReal () =
    if randomBoolean ()
    then randomNonNegativeReal ()
    else - randomNonNegativeReal ()

let randomExtendedReal () =
    let r = randomReal ()
    if r = 0.0 && randomBoolean ()
    then Infinity
    else Real r

let randomLists f n =
    List.init n (fun _ -> List.init n (fun _ -> f ()))