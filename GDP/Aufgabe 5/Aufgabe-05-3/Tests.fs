module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open Mini
open NatsType

type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())

let rec toList (xs: Nats): Nat list =
    match xs with
    | Nil -> []
    | Cons (x, ys) -> x::toList ys

let rec fromList (xs: Nat list): Nats =
    match xs with
    | [] -> Nil
    | x::ys -> Cons (x, fromList ys)


[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    [<TestMethod>]
    member this.``a) contains`` (): unit =
        Check.QuickThrowOnFailure(fun (x: Nat) (xs: Nats) ->
            Assert.AreEqual(
                List.contains x (toList xs),
                Nats.contains x xs
            )
        )

    [<TestMethod>]
    member this.``b) maximum`` (): unit =
        Check.QuickThrowOnFailure(fun (xs: Nats) ->
            Assert.AreEqual(
                List.max <| 0N::toList xs,
                Nats.maximum xs
            )
        )

    [<TestMethod>]
    member this.``c) take`` (): unit =
        Check.QuickThrowOnFailure(fun (n: Nat) (xs: Nats) ->
            let xs' = toList xs
            let n' = min (int n) (xs'.Length)
            Assert.AreEqual(
                List.take n' xs' |> fromList,
                Nats.take n xs
            )
        )

    [<TestMethod>]
    member this.``d) skip`` (): unit =
        Check.QuickThrowOnFailure(fun (n: Nat) (xs: Nats) ->
            let xs' = toList xs
            let n' = min (int n) (xs'.Length)
            Assert.AreEqual(
                List.skip n' xs' |> fromList,
                Nats.skip n xs
            )
        )

    [<TestMethod>]
    member this.``e) concat`` (): unit =
        Check.QuickThrowOnFailure(fun (xs: Nats) (ys: Nats) ->
            Assert.AreEqual(
                (toList xs) @ (toList ys) |> fromList,
                Nats.concat xs ys
            )
        )

    [<TestMethod>]
    member this.``f) mirror`` (): unit =
        Check.QuickThrowOnFailure(fun (xs: Nats) ->
            Assert.AreEqual(
                List.rev (toList xs) |> fromList,
                Nats.mirror xs
            )
        )

    [<TestMethod>]
    member this.``g) digits`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 10000}, fun (x: Nat) ->
            let expected =
                if x = 0N then Nil
                else x.ToString() |> Seq.map (fun c -> int c - int '0' |> Nat.Make) |> Seq.rev |> Seq.toList |> fromList
            Assert.AreEqual(
                expected,
                Nats.digits x
            )
        )
