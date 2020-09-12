module ConcreteActors

open Controller
open Actors
open Boolean
open Integer
open Real
open ExtendedReal
open Random
open FsMatrix.Matrix

let internal getActors t randomElement cast toWord fromWord sr sg =
    {
    Creator = MailboxProcessor.Start (creator randomElement);
    Writer =  MailboxProcessor.Start (writer t cast toWord);
    Reader =  MailboxProcessor.Start (reader fromWord);
    Multiplier =  MailboxProcessor.Start (multiplier t cast sr);
    TRCExecutor =  MailboxProcessor.Start (trcExecutor t cast sg);
    }

let boolActors () = getActors "bool" randomBoolean (fun m -> m :?> Matrix<bool>) toWordB fromWordB booleanSemiring booleanSemigroupWithPartialOrder

let intActors () = getActors "int" randomInteger (fun m -> m :?> Matrix<int>) toWordI fromWordI integerSemiring integerSemigroupWithPartialOrder

let realActors () = getActors "real" randomReal (fun m -> m :?> Matrix<float>) toWordR fromWordR realSemiring realSemigroupWithPartialOrder

let extendedRealActors () = getActors "extended real" randomExtendedReal (fun m -> m :?> Matrix<ExtendedReal>) toWordER fromWordER extendedRealSemiring extendedRealSemigroupWithPartialOrder