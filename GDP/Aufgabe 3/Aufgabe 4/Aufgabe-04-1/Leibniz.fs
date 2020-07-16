module Leibniz
open Mini

// a)
let rec digitSum (n: Nat): Nat =
    if n = 0N then 0N
    else (n%10N)+ digitSum(n/10N)

// b)
let rec isExp2 (n: Nat): bool =
    if n = 0N then false 
    else if n = 1N then true 
    elif (n%2N = 0N) then isExp2(n/2N) else false
