module Search
open Mini
open Option

let rec search (f: Nat -> Nat) (lo: Nat) (hi: Nat) (x: Nat): Option =
    if lo >= hi then NotFound
    else
        let m = lo + (hi - lo)/2N
        let r = f m
        if r = x then Found m
        else if r > x then search f lo m x
        else search f (m + 1N) hi x