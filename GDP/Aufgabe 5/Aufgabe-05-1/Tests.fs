module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open Mini
open NatInfType

type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())


[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    [<TestMethod>]
    member this.``a) Finite + Finite`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 10000}, fun (x: Nat) (y: Nat) ->
            let fx = Finite x
            let fy = Finite y
            Assert.AreEqual(Finite (x + y), NatInf.add fx fy, sprintf "Ergebnis von add (%A) (%A) ist falsch" fx fy)
        )

    [<TestMethod>]
    member this.``a) Infty + y`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 10000}, fun (x: NatInf) ->
            Assert.AreEqual(Infty, NatInf.add Infty x, sprintf "Ergebnis von add (Infty) (%A) ist falsch" x)
        )

    [<TestMethod>]
    member this.``a) x + Infty`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 10000}, fun (x: NatInf) ->
            Assert.AreEqual(Infty, NatInf.add x Infty, sprintf "Ergebnis von add (%A) (Infty) ist falsch" x)
        )


    [<TestMethod>]
    member this.``b) min Finite Finite`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 10000}, fun (x: Nat) (y: Nat) ->
            let fx = Finite x
            let fy = Finite y
            Assert.AreEqual(Finite (min x y), NatInf.minimum fx fy, sprintf "Ergebnis von minimum (%A) (%A) ist falsch" fx fy)
        )

    [<TestMethod>]
    member this.``b) min Infty y`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 10000}, fun (x: NatInf) ->
            Assert.AreEqual(x, NatInf.minimum Infty x, sprintf "Ergebnis von minimum (Infty) (%A) ist falsch" x)
        )

    [<TestMethod>]
    member this.``b) minimum x Infty`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 10000}, fun (x: NatInf) ->
            Assert.AreEqual(x, NatInf.minimum x Infty, sprintf "Ergebnis von minimum (%A) (Infty) ist falsch" x)
        )


    [<TestMethod>]
    member this.``c) maximum Finite Finite`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 10000}, fun (x: Nat) (y: Nat) ->
            let fx = Finite x
            let fy = Finite y
            Assert.AreEqual(Finite (max x y), NatInf.maximum fx fy, sprintf "Ergebnis von maximum (%A) (%A) ist falsch" fx fy)
        )

    [<TestMethod>]
    member this.``c) maximum Infty y`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 10000}, fun (x: NatInf) ->
            Assert.AreEqual(Infty, NatInf.maximum Infty x, sprintf "Ergebnis von maximum (Infty) (%A) ist falsch" x)
        )

    [<TestMethod>]
    member this.``c) maximum x Infty`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 10000}, fun (x: NatInf) ->
            Assert.AreEqual(Infty, NatInf.maximum x Infty, sprintf "Ergebnis von maximum (%A) (Infty) ist falsch" x)
        )
