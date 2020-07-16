module NatInfType
open Mini

[<NoComparison>]
type NatInf =
    | Infty          // unendlich
    | Finite of Nat  // endliche natürliche Zahl
