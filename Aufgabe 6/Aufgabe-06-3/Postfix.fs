module Postfix
open Mini
open Types


let rec parse (xs: Input list): Expr option =
    match xs with 
    |[] -> None 
    |y::ys -> let r = parse ys   


let rec Digitmax (n:Nat):Nat = 
    if n=0N then 0N
    else let r= Digitmax (n/10N)
         max (n%10N) r   


let rec mult (n:Nat): Nat = 
    if n=0N then 1N 
    elif (n%10N)= 0N then 0N 
    else (n%10N)* mult (n/10N)
let rec mult(n:Nat):Nat=
    try
    match n with 
    |0N->raise Zeroexception
    |n->if n<10N then n
                 else (n%10N)*mult(n/10N)
    with 
    |zeroexception->0N    



let rec intervals (xs: Intervals list): Intervals =  
    match xs with 
    |x::xss -> let r = intervals xss 
               {left = min x.left r.left ; right = max x.right r.right} 
    |[]->   { left = 0N; right = 0N}

                    

let rec div4 (n:Nat):Nat = 
    if n=0N then 0N 
    else let r = div4 (n - 1N)
         