module QueuesTypes

open Mini

type IQueue<'T> =
    interface
        abstract member Add: 'T -> Unit
        abstract member Remove: Unit -> 'T option
    end

type Tree<'T> =
    | Empty
    | Node of Tree<'T> * 'T * Tree<'T>
