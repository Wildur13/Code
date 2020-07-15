module HeapType

type Heap<'T> =
    | Empty
    | Node of 'T * Heap<'T> * Heap<'T>
