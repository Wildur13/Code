module Types
open Mini

type Input =
    | INum of Nat
    | Plus  // +
    | Star  // *

type Expr =
    | Num of Nat
    | Add of Expr * Expr
    | Mul of Expr * Expr
