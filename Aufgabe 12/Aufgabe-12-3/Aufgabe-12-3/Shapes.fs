module Shapes
open Mini
open ShapesTypes

// a)
let square (size: Nat): IShape =
    {
        new IShape with 
            member self.Contains (x) (y) = (x<=size && y<=size)
            member self.Rightmost = size
            member self.Topmost   = size  
    }

// b)
let rectangle (width: Nat, height: Nat): IShape =
    {
        new IShape with 
            member self.Contains (x) (y) = (x<=width && y<=height)
            member self.Rightmost = width
            member self.Topmost   = height  
    }

// c)
let union (s1: IShape, s2: IShape): IShape =
    {
        new IShape with 
            member self.Contains (x) (y) = s1.Contains (x) (y) || s2.Contains (x) (y)
            member self.Rightmost = max s1.Rightmost s2.Rightmost
            member self.Topmost   = max s1.Topmost s2.Topmost  
    }

// d)
let move (deltaX: Nat, deltaY: Nat) (s: IShape): IShape =
    {
        new IShape with 
            member self.Contains (x) (y) =  
                (x>= deltaX && y>= deltaY) && s.Contains (x- deltaX) (y - deltaY)
            member self.Rightmost = s.Rightmost + deltaX
            member self.Topmost   = s.Topmost + deltaY 
    }
