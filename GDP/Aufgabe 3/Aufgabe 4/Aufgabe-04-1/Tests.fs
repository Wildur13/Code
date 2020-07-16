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
    member this.``a) digitSum`` (): unit =
        Check.QuickThrowOnFailure (fun (n: Nat) ->
            let expected = n.ToString() |> Seq.sumBy (fun c -> int c - int '0') |> Nat.Make
            Assert.AreEqual(expected, Leibniz.digitSum n)
        )

    [<TestMethod>]
    member this.``b) isExp2`` (): unit =
        Check.QuickThrowOnFailure (fun (n: Nat) ->
            let expected = List.exists (fun m -> 2I ** (int m) |> Nat.Make = n) [0N..n]
            Assert.AreEqual(expected, Leibniz.isExp2 n)
        )
