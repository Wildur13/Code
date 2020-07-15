module Bucketsort

open Mini

let bucketsort<'T> (elems: (Int * 'T) list) (upper: Int) (n: Int): (Int * 'T) list =
    let arr = [| for i in 0..(n - 1) ->  []|]
    for i in 0..(n- 1) do 
        arr.[i] <- elems|> List.filter ( fun x -> ((fst x)*n)/ upper = i)|> List.sortBy fst
    List.concat [for i in arr -> i]      