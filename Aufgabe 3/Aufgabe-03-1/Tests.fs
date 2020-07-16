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
    member this.``Teil a) Minimum ist eine der beiden Zahlen`` (): unit =
        Check.QuickThrowOnFailure (fun (x: Nat) (y: Nat) ->
            let result = Minimum.min x y
            Assert.IsTrue(
                result = x || result = y,
                sprintf "Das errechnete Minimum %A ist nicht eine der beiden eingegebenen Zahlen %A und %A" result x y
            )
        )

    [<TestMethod>]
    member this.``Teil a) Minimum ist kleiner gleich der beiden Zahlen`` (): unit =
        Check.QuickThrowOnFailure (fun (x: Nat) (y: Nat) ->
            let result = Minimum.min x y
            Assert.IsTrue(
                result <= x && result <= y,
                sprintf "Das errechnete Minimum %A ist nicht kleiner oder gleich der beiden eingegebenen Zahlen %A und %A" result x y
            )
        )

    [<TestMethod>]
    member this.``Teil b) Minimum ist eine der drei Zahlen`` (): unit =
        Check.QuickThrowOnFailure (fun (x: Nat) (y: Nat) (z: Nat) ->
            let result = Minimum.min3 x y z
            Assert.IsTrue(
                result = x || result = y || result = z,
                sprintf "Das errechnete Minimum %A ist nicht eine der drei eingegebenen Zahlen %A, %A und %A" result x y z
            )
        )

    [<TestMethod>]
    member this.``Teil b) Minimum ist kleiner gleich der drei Zahlen`` (): unit =
        Check.QuickThrowOnFailure (fun (x: Nat) (y: Nat) (z: Nat) ->
            let result = Minimum.min3 x y z
            Assert.IsTrue(
                result <= x && result <= y && result <= z,
                sprintf "Das errechnete Minimum %A ist nicht kleiner oder gleich der drei eingegebenen Zahlen %A, %A und %A" result x y z
            )
        )
