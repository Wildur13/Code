module PriorityQueueHeap
open Mini

// Implementierung mit HEAPS

// Die nachfolgenden Zeilen bitte so stehen lassen!
type Entry<'a> = {priority: Nat; elem: 'a}
type PHeap<'a> = | Empty | Node of (Entry<'a> * PHeap<'a> * PHeap<'a>)

// merge Funktion vom letzten Übungsblatt, adaptiert an den Typ PHeap
// Darf hier natürlich gerne verwendet werden!
let rec merge<'a> (root1: PHeap<'a>) (root2: PHeap<'a>): PHeap<'a> =
    match (root1, root2) with
    | (Empty, t) | (t, Empty) -> t
    | (Node (e1, l1, r1), Node (e2, l2, r2)) ->
        let join x l r t = Node (x, merge t r, l)
        if e1.priority <= e2.priority then join e1 l1 r1 root2
        else join e2 l2 r2 root1


// Nutze PHeap für die Prioritätswarteschlange
type PQueue<'a> = PHeap<'a>
let empty: PQueue<'a> = Empty


// Ab hier die Lösung eingeben:

let isEmpty<'a> (q: PQueue<'a>): bool =
    failwith "TODO"

let rec insert<'a> (q: PQueue<'a>) (e: Entry<'a>): PQueue<'a> =
    failwith "TODO"

let rec extractMin<'a> (q: PQueue<'a>): (Entry<'a> * PQueue<'a>) option =
    failwith "TODO"
