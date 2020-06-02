module Creator

open FsMatrix.Matrix
open Types
open Random

type CreatorMessage<'a> =
    | CreateMatrix of int * AsyncReplyChannel<Matrix<'a>>
    | Exit of AsyncReplyChannel<bool>

type Creators =
    {
    BoolCreator : MailboxProcessor<CreatorMessage<bool>>;
    IntCreator : MailboxProcessor<CreatorMessage<int>>;
    RealCreator : MailboxProcessor<CreatorMessage<float>>;
    ExtendedRealCreator : MailboxProcessor<CreatorMessage<ExtendedReal>>;
    }

let createCreators () =
    let processMail (f : unit -> 'a) (inbox : MailboxProcessor<CreatorMessage<'a>>) =
        let rec innerLoop () = async {
            let! message = inbox.Receive ()
            match message with
            | CreateMatrix (n, r) ->
                let matrix = (fromRowsList << randomLists f) n
                r.Reply matrix.Value
                return! innerLoop ()
            | Exit r -> r.Reply true
        }
        innerLoop ()
    let boolCreator = MailboxProcessor.Start (processMail randomBoolean)
    let intCreator = MailboxProcessor.Start (processMail randomInteger)
    let realCreator = MailboxProcessor.Start (processMail randomReal)
    let extendedRealCreator = MailboxProcessor.Start (processMail randomExtendedReal)
    {BoolCreator = boolCreator; IntCreator = intCreator; RealCreator = realCreator; ExtendedRealCreator = extendedRealCreator}