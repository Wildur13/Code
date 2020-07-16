module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck

// Machen Sie sich bitte aktuell nicht die Mühe, das hier verstehen zu wollen.
// Das können wir zu einem späteren Zeitpunkt versuchen.

let expression0 a b = if a then b else false
let expressionA (a: int) b = if a > b then false else true
let expressionB a = if a then true else false
let expressionC a (b: int) (c: int) = if a then b + 555 else c + 555
let expressionD a b c = if a then (if b then true else c) else false
let expressionE a b = a <> b || ((b <> a) = true <> b && (a = b) <> (a = false) || a && b) || not (a || not b)

let check1 (expected: 'a -> 'b) (actual: 'a -> 'b) (x: 'a): unit =
    Assert.AreEqual(expected x, actual x)

let check2 (expected: 'a -> 'b -> 'c) (actual: 'a -> 'b -> 'c) (x: 'a) (y: 'b): unit =
    Assert.AreEqual(expected x y, actual x y)

let check3 (expected: 'a -> 'b -> 'c -> 'd) (actual: 'a -> 'b -> 'c -> 'd) (x: 'a) (y: 'b) (z: 'c): unit =
    Assert.AreEqual(expected x y z, actual x y z)


[<TestClass>]
type Tests() =

    [<TestMethod>]
    member this.``Beispiel`` (): unit =
        Check.QuickThrowOnFailure (check2 expression0 Simplify.simplified0)

    [<TestMethod>]
    member this.``Aufgabenteil a)`` (): unit =
        Check.QuickThrowOnFailure (check2 expressionA Simplify.simplifiedA)

    [<TestMethod>]
    member this.``Aufgabenteil b)`` (): unit =
        Check.QuickThrowOnFailure (check1 expressionB Simplify.simplifiedB)

    [<TestMethod>]
    member this.``Aufgabenteil c)`` (): unit =
        Check.QuickThrowOnFailure (check3 expressionC Simplify.simplifiedC)

    [<TestMethod>]
    member this.``Aufgabenteil d)`` (): unit =
        Check.QuickThrowOnFailure (check3 expressionD Simplify.simplifiedD)

    [<TestMethod>]
    member this.``Aufgabenteil e)`` (): unit =
        Check.QuickThrowOnFailure (check2 expressionE Simplify.simplifiedE)
