module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open Mini
open Option


[<StructuredFormatDisplay("{ToString}")>]
type TestInput =
    {
        a: Nat[]
        shift: Nat
    }
    member self.Lo: Nat = self.shift
    member self.Hi: Nat = self.shift + Nat.Make self.a.Length
    member self.ToFunction: Nat -> Nat =
        fun n ->
            if self.Lo >= self.Hi then failwith "f darf nicht aufgerufen werden weil lo >= hi ist"
            if n < self.Lo then failwith "f wurde mit zu kleinem n aufgerufen"
            if n >= self.Hi then failwith "f wurde mit zu großem n aufgerufen"
            self.a.[(int) (n - self.Lo)]
    member self.ToString =
        let sb = System.Text.StringBuilder()
        let out(s: string): unit = sb.Append(s) |> ignore
        if self.Lo >= self.Hi then
            out("fun n -> failwith \"f darf nicht aufgerufen werden weil lo >= hi ist\"")
        else
            out("fun n ->")
            if self.Lo > 0N then
                out(sprintf "\n    if n < %A then failwith \"f wurde mit zu kleinem n aufgerufen\"" self.Lo)
            for i in 0..self.a.Length-1 do
                out("\n    ")
                if i > 0 || self.Lo > 0N then out("else ")
                out(sprintf "if n = %A then %A" (self.Lo + Nat.Make i) self.a.[i])
            out("\n    else failwith \"f wurde mit zu großem n aufgerufen\"")
        out(sprintf "\n%A" self.Lo)
        out(sprintf "\n%A" self.Hi)
        sb.ToString()

type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())
    static member TestInput() =
        Arb.from<(Nat[] * Nat)>
        |> Arb.filter (fun (a, _) -> a = Array.sort a)
        |> Arb.convert (fun (a, shift) -> {a=a;shift=shift}) (fun f -> (f.a, f.shift))


[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    [<TestMethod>]
    member this.``Richtiges Ergebnis`` (): unit =
        Check.QuickThrowOnFailure(fun (i: TestInput) (x: Nat) ->
            let f = i.ToFunction
            let result = Search.search f i.Lo i.Hi x
            match result with
            | Found n ->
                Assert.IsTrue(
                    n >= i.Lo && n < i.Hi,
                    sprintf "Das Ergebnis %A ist nicht im gesuchten Intervall!" n
                )
                Assert.AreEqual(
                    x,
                    f n,
                    sprintf "Das Ergebnis ist %A, aber f %A ist nicht das gesuchte x=%A" result n x
                )
            | NotFound ->
                if i.Hi > i.Lo then
                    for n in i.Lo..i.Hi-1N do
                        Assert.IsFalse(
                            f n = x,
                            sprintf "Das Ergebnis ist %A, aber x=%A kann an der Stelle n=%A gefunden werden" result x n
                        )
        )

    [<TestMethod>]
    member this.``Laufzeitschranke`` (): unit =
        Check.QuickThrowOnFailure(fun (i: TestInput) (x: Nat) ->
            let f = i.ToFunction
            let mutable counter = 0N
            let f n =
                counter <- counter + 1N
                f n
            Search.search f i.Lo i.Hi x |> ignore
            let log2 (x: Nat): Nat = log (float x) / log 2.0 |> ceil |> (int) |> Nat.Make
            let bound = log2 (i.Hi - i.Lo + 1N)
            Assert.IsTrue(
                counter <= bound,
                sprintf "Sie haben %A Aufrufe von f gebraucht, erlaubt waren nur %A" counter bound
            )
        )
