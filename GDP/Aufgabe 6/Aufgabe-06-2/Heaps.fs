module Heaps
open Mini
open HeapType

// Beispiele vom Übungsblatt (zur Verwendung im Interpreter):
let ex1 = Node(2N, Node(6N,Empty,Empty), Node(4N,Empty,Empty))
let ex2 = Node(3N, Node(7N,Empty,Empty), Node(5N,Empty,Empty))
let ex3 = Node(1N, Node(1N,Empty,Empty), Empty)
let inv1 = Node(3N, Node(2N,Empty,Empty), Empty)
let inv2 = Node(3N, Node(5N, Node(4N,Empty,Empty), Empty), Empty)


// a)
let rec size<'T> (root: Heap<'T>): Nat =
    match root with 
    |Empty -> 0N
    |Node (w,l,r)-> 1N+ size l + size r

let rec height<'T> (root: Heap<'T>): Nat =
    match root with 
    |Empty -> 0N
    |Node (w,l,r)-> 1N + max (height l) (height r)

// b)
let rec isHeap<'T when 'T: comparison> (root: Heap<'T>): bool =
    match root with 
    | Empty -> true 
    | Node (x, Node (y, _, _), _) | Node (x, _, Node (y, _, _)) when x > y -> false 
    | Node (_, left, right) -> isHeap left && isHeap right


// c)
let head<'T > (root: Heap<'T>): 'T =
   match root with 
   |Empty -> failwith ""
   |Node(w,l,r)-> w

// d)
let rec merge<'T when 'T: comparison> (root1: Heap<'T>) (root2: Heap<'T>): Heap<'T> =
    match root1,root2 with 
    |Empty,Empty -> Empty
    |Empty,Node (x,l1,r1)|Node (x,l1,r1),Empty -> Node (x,l1,r1)
    |Node (x,l,r) , Node (y,l1,r1) -> let p = min x y 
                                      if p = x then Node(x, merge r root2, l)
                                      else Node (y,merge root1 r1, l1)

// e)
let tail<'T when 'T: comparison> (root: Heap<'T>): Heap<'T> =
    match root with 
    |Empty-> Empty
    |Node (x,l,r) -> merge l r

// f)
let insert<'T when 'T: comparison> (root: Heap<'T>) (x: 'T): Heap<'T> =
    merge root (Node (x,Empty,Empty)) 
// g)
let rec ofList<'T when 'T: comparison> (xs: 'T list): Heap<'T> =
    match xs with 
    |[]-> Empty 
    |x::xss -> let h= ofList xss
               merge h (Node(x,Empty,Empty)) 

let rec toList<'T when 'T: comparison> (root: Heap<'T>): 'T list =
    match root with 
    |Empty -> []
    |Node (x,l,r) -> x:: toList (merge l r)

// h)
let heapsort<'T when 'T: comparison> (xs: 'T list): 'T list =
    toList (ofList xs)
