module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open System
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

    let ex = [12;9;4;17;15;20;1;6;3;7]
    let ex1 = [(12, "B"); (9, "u"); (4, "c"); (17, "k"); (15, "e"); (20, "t"); (1, "s"); (7, "o"); (3, "r"); (7, "t")]
    let ex2 = [for i in ex do yield (i, i)]
    let ex3 = [for i in ex do yield (i, fun x -> 1+x)]

    [<TestMethod>] [<Timeout(1000)>]
    member this.``Beispiel 1: sortiere (Int * String) list`` (): unit =
        let expected = List.sortBy fst ex1
        let actual = Bucketsort.bucketsort ex1 (1 + fst (List.maxBy fst ex1)) 4
        Assert.AreEqual(expected |> List.map fst, actual |> List.map fst, "Elemente sind nicht korrekt sortiert")
        Assert.AreEqual(List.filter (fun x -> fst x <> 7) expected,  List.filter (fun x -> fst x <> 7) actual, "Zuordnung der Schlüssel zu den Werten nicht korrekt")

    [<TestMethod>] [<Timeout(1000)>]
    member this.``Beispiel 2: sortiere (Int * Int) list`` (): unit =
        let expected = List.sortBy fst ex2
        let actual = Bucketsort.bucketsort ex2 (1 + fst (List.maxBy fst ex2)) 4
        Assert.AreEqual(expected, actual, "Schlüssel/Wertpaare sind nicht korrekt sortiert")

    [<TestMethod>] [<Timeout(1000)>]
    member this.``Beispiel 3: sortiere (Int * (Int -> Int)) list`` (): unit =
        Assert.AreEqual(List.sortBy fst ex3, Bucketsort.bucketsort ex3 (1 + fst (List.maxBy fst ex3)) 4)
    
    [<TestMethod>] [<Timeout(1000)>]
    member this.``Zufall 1`` (): unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (elemsN: (Nat * String) list) ->
            let elems = List.map (fun (f,s) -> (int f,s) ) elemsN
            if not( List.isEmpty elems ) then
                let n = int (Math.Sqrt (float (List.length elems)))
                Assert.AreEqual(List.sortBy fst elems |> List.map fst, Bucketsort.bucketsort elems (1 + fst (List.maxBy fst elems)) n |> List.map fst)
        )

    [<TestMethod>] [<Timeout(1000)>]
    member this.``Zufall 2`` (): unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (elemsN: (Nat * (Bool -> Int)) list) ->
            let elems = List.map (fun (f,s) -> (int f,s) ) elemsN
            if not( List.isEmpty elems ) then
                let n = int (Math.Sqrt (float (List.length elems)))
                Assert.AreEqual(List.sortBy fst elems |> List.map fst, Bucketsort.bucketsort elems (1 + fst (List.maxBy fst elems)) n |> List.map fst)
        )
