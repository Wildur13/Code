module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open Mini
open ShapesTypes

type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())

    static member Shape() =
        Arb.fromGenShrink (
            let rec generator size =
                gen {
                    let choices =
                        [0; 0; 0; 1; 1; 2; 2]
                        @ if size = 0 then [] else [3]
                    let! choice = Gen.elements choices
                    match choice with
                    | 0 ->
                        let! d = Arb.from<SquareData> |> Arb.toGen
                        return Square d
                    | 1 ->
                        let! d = Arb.from<RectangleData> |> Arb.toGen
                        return Rectangle d
                    | 2 ->
                        let! m = Arb.from<Movement> |> Arb.toGen
                        let! s = generator (size / 2)
                        return Move (m, s)
                    | 3 ->
                        let! s1 = generator (size / 2)
                        let! s2 = generator (size / 2)
                        return Or (s1, s2)
                    | _ -> return Square {size = 0N}
                }
            Gen.sized generator
            ,
            fun (s: Shape) -> seq {
                match s with
                | Or (s1, s2) ->
                    yield s1
                    yield s2
                | Move (m, s) ->
                    yield s
                | _ -> ()
            }
        )


[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    [<TestMethod>]
    member this.``contains smiley`` (): unit =
        let smiley = Shapes.smiley()
        let smileyCoords =
            seq {
                for x in [1N..5N] @ [18N..22N] do
                    for y in [9N..13N] do
                        yield (x, y)
                for x in [11N..12N] do
                    for y in [6N..11N] do
                        yield (x, y)
                for x in [2N..4N] @ [19N..21N] do
                    for y in [3N..5N] do
                        yield (x, y)
                for x in [3N..20N] do
                    for y in [1N..3N] do
                        yield (x, y)
            }
            |> Set.ofSeq
        for x in [0N..100N] do
            for y in [0N..100N] do
                Assert.AreEqual(
                    Set.contains (x, y) smileyCoords,
                    Shapes.contains smiley (x, y),
                    sprintf "Ihre contains Funktion gibt für Ihren smiley Asdruck an Koordinate %A ein falsches Ergebnis zurück!" (x, y)
                )

    [<TestMethod>]
    member this.``rightmost smiley`` (): unit =
        Assert.AreEqual(22N, Shapes.rightmost <| Shapes.smiley())

    [<TestMethod>]
    member this.``topmost smiley`` (): unit =
        Assert.AreEqual(13N, Shapes.topmost <| Shapes.smiley())

    [<TestMethod>] [<Timeout(20000)>]
    member this.``contains rightmost topmost Plausibilität`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 20}, fun (s: Shape) ->
            let rm = Shapes.rightmost s
            let tm = Shapes.topmost s
            // Suche Punkt auf senkrechter Linie bei x=rightmost
            Assert.IsTrue(
                List.exists (fun y -> Shapes.contains s (rm, y)) [0N..tm],
                sprintf "rightmost s ist %A, topmost s ist %A, aber es gibt kein y in [0N..%A] mit contains s (%A, y)" rm tm tm rm
            )
            // Suche Punkt auf waagerechter Linie bei y=topmost
            Assert.IsTrue(
                List.exists (fun x -> Shapes.contains s (x, tm)) [0N..rm],
                sprintf "rightmost s ist %A, topmost s ist %A, aber es gibt kein x in [0N..%A] mit contains s (x, %A)" rm tm rm tm
            )
            // Suche Punkte rechts von rightmost
            for x in [rm+1N..rm+10N] do
                for y in [0N..tm+10N] do
                    Assert.IsFalse(
                        Shapes.contains s (x, y),
                        sprintf "rightost s ist %A, aber contains s %A ist trotzdem true" rm (x, y)
                    )
            // Suche Punkte oberhalb von topmost
            for y in [tm+1N..tm+10N] do
                for x in [0N..rm+10N] do
                    Assert.IsFalse(
                        Shapes.contains s (x, y),
                        sprintf "topmost s ist %A, aber contains s %A ist trotzdem true" tm (x, y)
                    )
        )
