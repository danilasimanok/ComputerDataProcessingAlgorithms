module Reader

    open FsMatrix.Matrix
    open FsMatrix.MatrixIO
    open Types
    open Boolean
    open Integer
    open Real
    open ExtendedReal
    
    type ReaderMessage<'a> =
        | ReadMatrix of string * AsyncReplyChannel<Matrix<'a> option>
        | Exit of AsyncReplyChannel<bool>

    type Readers =
        {
        BoolReader : MailboxProcessor<ReaderMessage<bool>>;
        IntReader : MailboxProcessor<ReaderMessage<int>>;
        RealReader : MailboxProcessor<ReaderMessage<float>>;
        ExtendedRealReader : MailboxProcessor<ReaderMessage<ExtendedReal>>;
        }

    let createReaders () =
        let processMail (f : string -> 'a) (inbox : MailboxProcessor<ReaderMessage<'a>>) =
            let rec innerLoop () = async {
                let! message = inbox.Receive ()
                match message with
                | Exit r-> r.Reply true
                | ReadMatrix (path, r) ->
                    try
                        (r.Reply << fromRowsList << readRowsList f) path
                    with _ -> r.Reply None
                    return! innerLoop ()
            }
            innerLoop ()
        let boolReader = MailboxProcessor<ReaderMessage<bool>>.Start (processMail fromWordB)
        let intReader = MailboxProcessor<ReaderMessage<int>>.Start (processMail fromWordI)
        let realReader = MailboxProcessor<ReaderMessage<float>>.Start (processMail fromWordR)
        let extendedRealReader = MailboxProcessor<ReaderMessage<ExtendedReal>>.Start (processMail fromWordER)
        {BoolReader = boolReader; IntReader = intReader; RealReader = realReader; ExtendedRealReader = extendedRealReader}