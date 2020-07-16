module Nats
open Mini
open NatsType

// a)
let rec contains (x: Nat) (xs: Nats): bool =
    match xs with 
    |Nil -> false
    |Cons(a,b) -> if x = a then true else contains x b 

// b)
let rec maximum (xs: Nats): Nat =
    match xs with 
    |Nil -> 0N
    |Cons(a,b) -> let maxi = maximum b 
                  if a > maxi then a else maxi     

// c)
let rec take (n: Nat) (xs: Nats): Nats =
    if n = 0N then Nil
    else 
        match xs with
        |Nil -> Nil
        |Cons(a,b) -> Cons(a, take (n - 1N) b)

// d)
let rec skip (n: Nat) (xs: Nats): Nats =
    if n = 0N then xs
    else 
        match xs with
        |Nil -> Nil
        |Cons(a,b) -> skip(n - 1N) b

// e)
let rec concat (xs: Nats) (ys: Nats): Nats =
    failwith "TODO"

// f)
let rec mirror (xs: Nats): Nats =
    failwith "TODO"

// g)
let rec digits (x: Nat): Nats =
    failwith "TODO"
