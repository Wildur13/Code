module PriorityQueueUnsortedList
open Mini

// Implementierung mit UNSORTIERTEN Listen

// Die nachfolgenden Zeilen bitte so stehen lassen!
type Entry<'a> = {priority: Nat; elem: 'a}
type PQueue<'a> = Entry<'a> list
let empty: PQueue<'a> = []


// Ab hier die Lösung eingeben:

let isEmpty<'a> (q: PQueue<'a>): bool =
    failwith "TODO"

let rec insert<'a> (q: PQueue<'a>) (e: Entry<'a>): PQueue<'a> =
    failwith "TODO"

let rec extractMin<'a> (q: PQueue<'a>): (Entry<'a> * PQueue<'a>) option =
    failwith "TODO"
