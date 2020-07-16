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

    // ------------------------------------------------------------------------
    // a)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) splitIntoRuns Beispiel 1`` (): unit =
        let expected: Nat list list = []
        Assert.AreEqual(expected, Mergesort.splitIntoRuns<Nat> [])

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) splitIntoRuns Beispiel 2`` (): unit =
        Assert.AreEqual([[1N; 2N; 3N; 3N; 4N; 5N]], Mergesort.splitIntoRuns [1N; 2N; 3N; 3N; 4N; 5N])

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) splitIntoRuns Beispiel 3`` (): unit =
        Assert.AreEqual([[3]; [1;4]; [1;5;9]; [2;6]; [5]; [3;5;9]], Mergesort.splitIntoRuns [3;1;4;1;5;9;2;6;5;3;5;9])

    [<TestMethod>] [<Timeout(10000)>]
    member this.``a) splitIntoRuns Zufall`` (): unit =
        Check.QuickThrowOnFailure (fun (xs: Nat list) ->
            let runs = Mergesort.splitIntoRuns xs
            Assert.AreEqual(xs, List.concat runs, sprintf "List.concat %A ergibt nicht die Eingabeliste" runs)
            if List.contains runs [] then
                Assert.Fail (sprintf "Ergebnis %A darf keine leere Liste enthalten!" runs)
            let rec check runs =
                match runs with
                | xs::(y::ys)::zss ->
                    if (List.last xs) <= y then
                        Assert.Fail (sprintf "Ergebnis %A nicht optimal: Teilliste %A sollte nächsten Wert %A noch enthalten!" runs xs y)
                    check ((y::ys)::zss)
                | _ -> ()
            check runs
        )


    // ------------------------------------------------------------------------
    // b)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``b) merge Beispiel`` (): unit =
        Assert.AreEqual(
            [0N; 1N; 2N; 2N; 3N; 4N; 5N],
            Mergesort.merge [1N; 3N; 4N] [0N; 2N; 2N; 5N]
        )

    [<TestMethod>] [<Timeout(10000)>]
    member this.``b) merge Zufall`` (): unit =
        Check.QuickThrowOnFailure (fun (xs: Nat list) (ys: Nat list) ->
            let xs = List.sort xs
            let ys = List.sort ys
            let result = Mergesort.merge xs ys
            Assert.AreEqual(
                List.sort (xs @ ys),
                result,
                sprintf "merge %A %A liefert falsches Ergebnis %A" xs ys result
            )
        )


    // ------------------------------------------------------------------------
    // c)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) mergePass Beispiel 1`` (): unit =
        let expected: Nat list list = []
        Assert.AreEqual(
            expected,
            Mergesort.mergePass<Nat> []
        )

    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) mergePass Beispiel 2`` (): unit =
        Assert.AreEqual(
            [[1; 3; 4]; [1; 2; 5; 6; 9]; [3; 5; 5; 9]],
            Mergesort.mergePass [[3]; [1;4]; [1;5;9]; [2;6]; [5]; [3;5;9]]
        )


    // ------------------------------------------------------------------------
    // d)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``d) mergePasses Beispiel 1`` (): unit =
        let expected: Nat list = []
        Assert.AreEqual(
            expected,
            Mergesort.mergePasses<Nat> []
        )

    [<TestMethod>] [<Timeout(1000)>]
    member this.``d) mergePasses Beispiel 2`` (): unit =
        Assert.AreEqual(
            [1; 1; 2; 3; 3; 4; 5; 5; 5; 6; 9; 9],
            Mergesort.mergePasses [[3]; [1;4]; [1;5;9]; [2;6]; [5]; [3;5;9]]
        )


    // ------------------------------------------------------------------------
    // e)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``e) mergesort Beispiel`` (): unit =
        Assert.AreEqual(
            [1; 1; 2; 3; 3; 4; 5; 5; 5; 6; 9; 9],
            Mergesort.mergesort [3; 1; 4; 1; 5; 9; 2; 6; 5; 3; 5; 9]
        )

    [<TestMethod>] [<Timeout(10000)>]
    member this.``e) mergesort Zufall`` (): unit =
        Check.QuickThrowOnFailure (fun (xs: Nat list) ->
            Assert.AreEqual(List.sort xs, Mergesort.mergesort xs)
        )
