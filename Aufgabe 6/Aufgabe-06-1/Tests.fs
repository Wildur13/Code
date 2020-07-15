module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open Mini

type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())


[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    [<TestMethod>]
    member this.``a) append<Nat>`` (): unit =
        Check.QuickThrowOnFailure(fun (xs: Nat list) (ys: Nat list) ->
            Assert.AreEqual(
                List.append xs ys,
                Listen.append<Nat> xs ys
            )
        )

    [<TestMethod>]
    member this.``a) append<int>`` (): unit =
        Check.QuickThrowOnFailure(fun (xs: int list) (ys: int list) ->
            Assert.AreEqual(
                List.append xs ys,
                Listen.append<int> xs ys
            )
        )

    [<TestMethod>]
    member this.``b) concat<Nat>`` (): unit =
        Check.QuickThrowOnFailure(fun (xss: Nat list list) ->
            Assert.AreEqual(
                List.concat xss,
                Listen.concat<Nat> xss
            )
        )

    [<TestMethod>]
    member this.``b) concat<int>`` (): unit =
        Check.QuickThrowOnFailure(fun (xss: int list list) ->
            Assert.AreEqual(
                List.concat xss,
                Listen.concat<int> xss
            )
        )

    [<TestMethod>]
    member this.``c) map<Nat, Nat> (fun x -> 2N * x)`` (): unit =
        Check.QuickThrowOnFailure(fun (xs: Nat list) ->
            let f x = 2N * x
            Assert.AreEqual(
                List.map f xs,
                Listen.map<Nat, Nat> f xs
            )
        )

    [<TestMethod>]
    member this.``c) map<Nat, Nat> (fun x -> x + 10N)`` (): unit =
        Check.QuickThrowOnFailure(fun (xs: Nat list) ->
            let f x = x + 10N
            Assert.AreEqual(
                List.map f xs,
                Listen.map<Nat, Nat> f xs
            )
        )

    [<TestMethod>]
    member this.``c) map<int, string> (string)`` (): unit =
        Check.QuickThrowOnFailure(fun (xs: int list) ->
            Assert.AreEqual(
                List.map string xs,
                Listen.map<int, string> string xs
            )
        )

    [<TestMethod>]
    member this.``d) filter<Nat> (fun x -> x % 2N = 0N)`` (): unit =
        Check.QuickThrowOnFailure(fun (xs: Nat list) ->
            let pred x = x % 2N = 0N
            Assert.AreEqual(
                List.filter pred xs,
                Listen.filter<Nat> pred xs
            )
        )

    [<TestMethod>]
    member this.``d) filter<Nat> (fun x -> x <= 2N)`` (): unit =
        Check.QuickThrowOnFailure(fun (xs: Nat list) ->
            let pred x = x <= 2N
            Assert.AreEqual(
                List.filter pred xs,
                Listen.filter<Nat> pred xs
            )
        )

    [<TestMethod>]
    member this.``d) filter<int> (fun x -> x < 0)`` (): unit =
        Check.QuickThrowOnFailure(fun (xs: int list) ->
            let pred x = x < 0
            Assert.AreEqual(
                List.filter pred xs,
                Listen.filter<int> pred xs
            )
        )

    [<TestMethod>]
    member this.``e) collect<Nat, Nat> (fun x -> [x; x])`` (): unit =
        Check.QuickThrowOnFailure(fun (xs: Nat list) ->
            let f x = [x; x]
            Assert.AreEqual(
                List.collect f xs,
                Listen.collect<Nat, Nat> f xs
            )
        )

    [<TestMethod>]
    member this.``e) collect<int, char> (string >> Seq.toList)`` (): unit =
        Check.QuickThrowOnFailure(fun (xs: int list) ->
            let f = string >> Seq.toList
            Assert.AreEqual(
                List.collect f xs,
                Listen.collect<int, char> f xs
            )
        )
