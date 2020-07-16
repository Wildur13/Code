module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck

// Machen Sie sich bitte aktuell nicht die Mühe, das hier verstehen zu wollen.
// Das können wir zu einem späteren Zeitpunkt versuchen.

let b2i (x: bool): int = if x then 1 else 0
let i2b (x: int): bool = x > 0

let checkb (op: int -> int -> bool) (impl: bool -> bool -> bool) (a: bool) (b: bool): unit =
    let expected: bool = op (b2i a) (b2i b)
    let actual: bool = impl a b
    Assert.AreEqual(expected, actual)

let checki (op: int -> int -> int) (impl: bool -> bool -> bool) (a: bool) (b: bool): unit =
    let expected: bool = op (b2i a) (b2i b) |> i2b
    let actual: bool = impl a b
    Assert.AreEqual(expected, actual)


[<TestClass>]
type Tests() =

    [<TestMethod>]
    member this.``greater`` (): unit =
        Check.QuickThrowOnFailure (checkb (>) Bool.greater)

    [<TestMethod>]
    member this.``smallerOrEqual`` (): unit =
        Check.QuickThrowOnFailure (checkb (<=) Bool.smallerOrEqual)

    [<TestMethod>]
    member this.``multiplication`` (): unit =
        Check.QuickThrowOnFailure (checki (*) Bool.multiplication)

    [<TestMethod>]
    member this.``equivalence`` (): unit =
        Check.QuickThrowOnFailure (checkb (=) Bool.equivalence)
