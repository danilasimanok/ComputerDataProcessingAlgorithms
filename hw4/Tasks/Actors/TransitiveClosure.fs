module TransitiveClosure

open FsMatrix.Matrix
open Types
open Semigroups

type TransitiveClosureMessage<'a> =
    | TRC of Matrix<'a> * AsyncReplyChannel<Matrix<'a>>
    | Exit of AsyncReplyChannel<bool>

type TransitiveClosures =
    {
    BoolTransitiveClosure : MailboxProcessor<TransitiveClosureMessage<bool>>;
    IntTransitiveClosure : MailboxProcessor<TransitiveClosureMessage<int>>;
    RealTransitiveClosure : MailboxProcessor<TransitiveClosureMessage<float>>;
    ExtendedRealTransitiveClosure : MailboxProcessor<TransitiveClosureMessage<ExtendedReal>>;
    }

let createTransitiveClosures () =
    let processMail (sg : SemigroupWithPartialOrder<'a>) (inbox : MailboxProcessor<TransitiveClosureMessage<'a>>) =
        let rec innerLoop () = async {
            let! message = inbox.Receive ()
            match message with
            | TRC (m, r) ->
                r.Reply (floydWarshall sg m).Value
                return! innerLoop ()
            | Exit r -> r.Reply true
        }
        innerLoop ()
    let boolTRC = MailboxProcessor<TransitiveClosureMessage<bool>>.Start (processMail booleanSemigroup)
    let intTRC = MailboxProcessor<TransitiveClosureMessage<int>>.Start (processMail integerSemigroup)
    let realTRC = MailboxProcessor<TransitiveClosureMessage<float>>.Start (processMail realSemigroup)
    let extendedRealTRC = MailboxProcessor<TransitiveClosureMessage<ExtendedReal>>.Start (processMail extendedRealSemigroup)
    {BoolTransitiveClosure = boolTRC; IntTransitiveClosure = intTRC; RealTransitiveClosure = realTRC; ExtendedRealTransitiveClosure = extendedRealTRC}