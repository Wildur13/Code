module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open Mini

[<StructuredFormatDisplay("{s}")>]
type SafeString = SS of s: string


type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())

    static member SafeString() =
        Arb.from<string>
        |> Arb.filter (not << isNull)
        |> Arb.convert (String.filter (fun c -> c >= 'a' && c <= 'd')) (id)
        |> Arb.filter (fun s -> s.Length > 0)
        |> Arb.convert (SS) (fun (SS s) -> s)


[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    [<TestMethod>]
    member this.``mult3`` (): unit =
        Check.QuickThrowOnFailure (fun (x: Nat) ->
            Assert.AreEqual(3N*x, Peano.mult3 x)
        )

    [<TestMethod>]
    member this.``exp3`` (): unit =
        Check.QuickThrowOnFailure (fun (x: Nat) ->
            Assert.AreEqual(Nat.Make (3I ** (int x)), Peano.exp3 x)
        )

    [<TestMethod>]
    member this.``root3: Obere Schranke`` (): unit =
        Check.QuickThrowOnFailure (fun (x: Nat) ->
            let result = Peano.root3 x
            let result3 = Nat.Make (result.ToBigInteger() ** 3)
            Assert.IsTrue(
                result3 <= x,
                sprintf "Das Ergebnis %A kubiert (%A) ist nicht kleiner oder gleich der Eingabe %A" result result3 x
            )
        )

    [<TestMethod>]
    member this.``root3: Untere Schranke`` (): unit =
        Check.QuickThrowOnFailure (fun (x: Nat) ->
            let result = Peano.root3 x
            let result1 = result + 1N
            let result13 = Nat.Make (result1.ToBigInteger() ** 3)
            Assert.IsTrue(
                result13 > x,
                sprintf "Das Ergebnis %A ist zu klein, denn %A kubiert (%A) ist immer noch kleiner oder gleich der Eingabe %A" result result1 result13 x
            )
        )

    [<TestMethod>]
    member this.``search`` (): unit =
        Check.One ({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (s: SafeString) (f: SafeString[]) (limit: Nat) (placeholder: SafeString) ->
            let (SS s) = s
            let f = Array.map (fun (SS s) -> s) f
            let (SS placeholder) = placeholder
            let limit' = min (Nat.Make f.Length) limit
            let result = Peano.search s (fun (n: Nat) -> if n < limit' then f.[int n] else placeholder) limit
            let expected = limit > limit' && s = placeholder || (Array.sub f 0 (int limit') |> Array.contains s)
            Assert.AreEqual(
                expected,
                result,
                sprintf "\n\nlet s = %A\nlet f n =\n    %s%A\nlet limit = %A\n\n"
                    s
                    (f |> Array.mapi (fun i x -> sprintf "if n = %iN then %A\n    else " i x) |> String.concat "")
                    placeholder
                    limit
            )
        )
