module NatInf
open Mini
open NatInfType

// a)
let add (x: NatInf) (y: NatInf): NatInf =
    match x,y with 
    |(Infty,Infty) -> Infty
    |(Finite x,Infty)|(Infty,Finite x)-> Infty
    |(Finite a, Finite b)->Finite(a+b)

// b)
let minimum (x: NatInf) (y: NatInf): NatInf =
     match x,y with 
    |(Infty,Infty) -> Infty
    |(Finite x,Infty)|(Infty,Finite x)-> Finite x
    |(Finite a, Finite b)->if (a<b) then Finite a else Finite b

// c)
let maximum (x: NatInf) (y: NatInf): NatInf =
    match x,y with 
    |(Infty,Infty) -> Infty
    |(Finite x,Infty)|(Infty,Finite x)-> Infty
    |(Finite a, Finite b)->if (a<b) then Finite b else Finite a
