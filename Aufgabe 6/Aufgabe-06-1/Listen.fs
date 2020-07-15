module Listen
open Mini

// a)
let rec append<'a> (xs: 'a list) (ys: 'a list): 'a list =
    match xs with
    |[]-> ys 
    |x::xss -> x:: append xss ys 

// b)
let rec concat<'a> (xss: 'a list list): 'a list =
    match xss with 
    |[]-> []
    |y::ys -> y@ concat ys 

// c)
let rec map<'a, 'b> (f: 'a -> 'b) (xs: 'a list): 'b list =
    match xs with 
    |[]-> []
    |y::ys -> f y :: map f ys 

// d)
let rec filter<'a> (pred: 'a -> bool) (xs: 'a list): 'a list =
    match xs with 
    |[]-> []
    |y::ys -> if pred y then y:: filter pred ys else filter pred ys 

// e)
let rec collect<'a, 'b> (f: 'a -> 'b list) (xs: 'a list): 'b list =
    match xs with 
    |[]-> []
    |y::ys -> f y @ collect f ys 
