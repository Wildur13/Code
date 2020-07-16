module Search

open Mini
open SearchTypes

// a)
let rec height<'T> (root: PTree<'T>): Nat =
    failwith "TODO"

let rec size<'T> (root: PTree<'T>): Nat =
    failwith "TODO"

// b)
let rec contains<'T when 'T: comparison> (elem: 'T) (root: PTree<'T>): bool =
    failwith "TODO"

// c)
let rec insert<'T when 'T: comparison> (elem: 'T) (root: PTree<'T>): unit =
    failwith "TODO"

// d)
let rec delete<'T when 'T: comparison> (elem: 'T) (root: PTree<'T>): unit =
    failwith "TODO"