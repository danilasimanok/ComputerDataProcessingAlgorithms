module Multiplier

open FsMatrix.Matrix
open Types
open Boolean
open Integer
open Real
open ExtendedReal

type MultiplierMessage<'a> =
    | MultiplyMatrices of Matrix<'a> * Matrix<'a> * AsyncReplyChannel<Matrix<'a> option>
    | Exit of AsyncReplyChannel<bool>

type Multipliers =
    {
    BoolMultiplier : MailboxProcessor<MultiplierMessage<bool>>;
    IntMultiplier : MailboxProcessor<MultiplierMessage<int>>;
    RealMultiplier : MailboxProcessor<MultiplierMessage<float>>;
    ExtendedRealMultiplier : MailboxProcessor<MultiplierMessage<ExtendedReal>>;
    }

let createMultipliers () =
    let processMail (sr : Semiring<'a>) (inbox : MailboxProcessor<MultiplierMessage<'a>>) =
        let rec innerLoop () = async {
            let! message = inbox.Receive ()
            match message with
            | MultiplyMatrices (m1, m2, r) ->
                r.Reply (multiply sr m1 m2)
                return! innerLoop ()
            | Exit r -> r.Reply true
        }
        innerLoop ()
    let boolMultiplier = MailboxProcessor<MultiplierMessage<bool>>.Start (processMail booleanSemiring)
    let intMultiplier = MailboxProcessor<MultiplierMessage<int>>.Start (processMail integerSemiring)
    let realMultiplier = MailboxProcessor<MultiplierMessage<float>>.Start (processMail realSemiring)
    let extendedRealMultiplier = MailboxProcessor<MultiplierMessage<ExtendedReal>>.Start (processMail extendedRealSemiring)
    {BoolMultiplier = boolMultiplier; IntMultiplier = intMultiplier; RealMultiplier = realMultiplier; ExtendedRealMultiplier = extendedRealMultiplier}