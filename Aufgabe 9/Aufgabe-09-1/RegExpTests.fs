module RegExpTests

open Microsoft.VisualStudio.TestTools.UnitTesting
open System.Text.RegularExpressions
open FsCheck
open Mini
open RegExpTypes


type ArbitraryModifiers =
    static member Nat() =
        Arb.fromGenShrink (
            Gen.choose(0, 100),
            fun n -> seq {
                if n > 0 then yield n - 1
            }
        )
        |> Arb.convert (Nat.Make) (int)


[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    // ------------------------------------------------------------------------
    // c)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) accept Beispiel 1`` (): unit =
        Assert.IsTrue(RegExp.accept [])

    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) accept Beispiel 2`` (): unit =
        Assert.IsTrue(RegExp.accept [C])

    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) accept Beispiel 3`` (): unit =
        Assert.IsTrue(RegExp.accept [A;C])

    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) accept Beispiel 4`` (): unit =
        Assert.IsTrue(RegExp.accept [A;C;A;C;B;B;B;B;C;C])


    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) accept Gegenbeispiel 1`` (): unit =
        Assert.IsTrue(not (RegExp.accept [B]))

    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) accept Gegenbeispiel 2`` (): unit =
        Assert.IsTrue(not (RegExp.accept [A;C;A;B;B;B]))

    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) accept Gegenbeispiel 3`` (): unit =
        Assert.IsTrue(not (RegExp.accept [A;C;A]))


    [<TestMethod>] [<Timeout(10000)>]
    member this.``c) accept Zufall`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 100}, fun (input: Alphabet list) ->
            let rec toString (acc: String) (xs: Alphabet list): String =
                match xs with
                | [] -> acc
                | A::rest -> toString (acc + "a") rest
                | B::rest -> toString (acc + "b") rest
                | C::rest -> toString (acc + "c") rest
            let inputStr = toString "" input
            let m = Regex.Match(inputStr, "((a|b*)c)*")
            Assert.AreEqual(m.Success && m.Value = inputStr, RegExp.accept input)
        )
