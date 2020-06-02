module Writer

open FsMatrix.Matrix
open FsMatrix.MatrixIO
open Types
open Boolean
open Integer
open Real
open ExtendedReal

type WriterMessage<'a> =
    | WriteMatrix of string * Matrix<'a> * AsyncReplyChannel<bool>
    | Exit of AsyncReplyChannel<bool>

type Writers =
    {
    BoolWriter : MailboxProcessor<WriterMessage<bool>>;
    IntWriter : MailboxProcessor<WriterMessage<int>>;
    RealWriter : MailboxProcessor<WriterMessage<float>>;
    ExtendedRealWriter : MailboxProcessor<WriterMessage<ExtendedReal>>;
    }

let createWriters () =
    let processMail (f : 'a -> string) (inbox : MailboxProcessor<WriterMessage<'a>>) =
        let rec innerLoop () = async {
            let! message = inbox.Receive ()
            match message with
            | Exit r-> r.Reply true
            | WriteMatrix (path, matrix, r) ->
                try
                    writeRowsList f (toRowsList matrix) path
                    r.Reply true
                with _ -> r.Reply false
                return! innerLoop ()
        }
        innerLoop ()
    let boolWriter = MailboxProcessor<WriterMessage<bool>>.Start (processMail toWordB)
    let intWriter = MailboxProcessor<WriterMessage<int>>.Start (processMail toWordI)
    let realWriter = MailboxProcessor<WriterMessage<float>>.Start (processMail toWordR)
    let extendedRealWriter = MailboxProcessor<WriterMessage<ExtendedReal>>.Start (processMail toWordER)
    {BoolWriter = boolWriter; IntWriter = intWriter; RealWriter = realWriter; ExtendedRealWriter = extendedRealWriter}