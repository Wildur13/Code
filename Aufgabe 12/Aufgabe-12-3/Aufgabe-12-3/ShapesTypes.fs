module ShapesTypes
open Mini

type IShape =
    interface
        abstract member Contains: Nat -> Nat -> Bool
        abstract member Rightmost: Nat
        abstract member Topmost: Nat
    end
