module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open Mini
open VectorsType

type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())


let iter2 (f: string -> Nat -> Nat -> unit) (v1: Vec) (v2: Vec): unit =
    f "x" v1.x v2.x
    f "y" v1.y v2.y
    f "z" v1.z v2.z

let iter3 (f: string -> Nat -> Nat -> Nat -> unit) (v1: Vec) (v2: Vec) (v3: Vec): unit =
    f "x" v1.x v2.x v3.x
    f "y" v1.y v2.y v3.y
    f "z" v1.z v2.z v3.z


[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    [<TestMethod>]
    member this.``a) scale`` (): unit =
        Check.QuickThrowOnFailure(fun (v: Vec) (n: Nat) ->
            let result = Vectors.scale v n
            (v, result) ||> iter2 (
                fun l vi ri -> Assert.AreEqual(n*vi, ri, sprintf "%s ist falsch!" l)
            )
        )

    [<TestMethod>]
    member this.``b) add`` (): unit =
        Check.QuickThrowOnFailure(fun (v1: Vec) (v2: Vec) ->
            let result = Vectors.add v1 v2
            (v1, v2, result) |||> iter3 (
                fun l v1i v2i ri -> Assert.AreEqual(v1i + v2i, ri, sprintf "%s ist falsch!" l)
            )
        )

    [<TestMethod>]
    member this.``c) dotProduct`` (): unit =
        Check.QuickThrowOnFailure(fun (v1: Vec) (v2: Vec) ->
            let mutable expected = 0N
            (v1, v2) ||> iter2 (
                fun _l v1i v2i -> expected <- expected + v1i * v2i
            )
            Assert.AreEqual (expected, Vectors.dotProduct v1 v2)
        )
