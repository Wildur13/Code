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

    static member IShape() =
        Arb.fromGenShrink (
            let rec generator size =
                gen {
                    let choices =
                        [0; 0; 0; 1; 1; 2; 2]
                        @ if size = 0 then [] else [3]
                    let! choice = Gen.elements choices
                    match choice with
                    | 0 ->
                        let! d = Arb.from<Nat> |> Arb.toGen
                        return Shapes.square d
                    | 1 ->
                        let! width = Arb.from<Nat> |> Arb.toGen
                        let! height = Arb.from<Nat> |> Arb.toGen
                        return Shapes.rectangle (width, height)
                    | 2 ->
                        let! dx = Arb.from<Nat> |> Arb.toGen
                        let! dy = Arb.from<Nat> |> Arb.toGen
                        let! s = generator (size / 2)
                        return Shapes.move (dx, dy) s
                    | 3 ->
                        let! s1 = generator (size / 2)
                        let! s2 = generator (size / 2)
                        return Shapes.union (s1, s2)
                    | _ -> return Shapes.square 0N
                }
            Gen.sized generator
             ,
            fun (s: IShape) -> seq {
                match s with
                | _ -> ()
            }
        )


[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore


    // Wir verwenden als Beispiel wieder den Smiley von Übungsblatt 5, Aufgabe 4
    let smiley(): IShape =
        let leftEye = Shapes.move (1N, 9N) (Shapes.square 4N)
        let leftMouth = Shapes.move (2N, 3N) (Shapes.square 2N)
        let left = Shapes.union (leftEye, leftMouth)
        let right = Shapes.move (17N, 0N) left
        let nose = Shapes.move (11N, 6N) (Shapes.rectangle (1N, 5N))
        let mouth = Shapes.move (3N, 1N) (Shapes.rectangle (17N, 2N))
        Shapes.union (Shapes.union (left, right), Shapes.union (nose, mouth))


    [<TestMethod>] [<Timeout(20000)>]
    member this.``a) Beispiel: square`` (): Unit =
        let s = Shapes.square 3N
        Assert.AreEqual(3N, s.Rightmost)
        Assert.AreEqual(3N, s.Topmost)
        Assert.IsTrue(s.Contains 0N 0N, "(0,0) soll enthalten sein")
        Assert.IsTrue(s.Contains 3N 3N, "(3,3) soll enthalten sein")
        Assert.IsTrue(s.Contains 3N 1N, "(3,1) soll enthalten sein")
        Assert.IsFalse(s.Contains 3N 4N, "(3,4) soll nicht enthalten sein")

    [<TestMethod>] [<Timeout(20000)>]
    member this.``b) Beispiel: rectangle`` (): Unit =
        let s = Shapes.rectangle (4N, 5N)
        Assert.AreEqual(4N, s.Rightmost)
        Assert.AreEqual(5N, s.Topmost)
        Assert.IsTrue(s.Contains 0N 0N, "(0,0) soll enthalten sein")
        Assert.IsTrue(s.Contains 4N 5N, "(4,5) soll enthalten sein")
        Assert.IsTrue(s.Contains 3N 2N, "(3,2) soll enthalten sein")
        Assert.IsFalse(s.Contains 5N 5N, "(5,5) soll nicht enthalten sein")

    [<TestMethod>] [<Timeout(20000)>]
    member this.``c) Beispiel: union`` (): Unit =
        let s = Shapes.union ((Shapes.square 3N), (Shapes.rectangle (4N, 1N)))
        Assert.AreEqual(4N, s.Rightmost)
        Assert.AreEqual(3N, s.Topmost)
        Assert.IsTrue(s.Contains 4N 1N, "(4,1) soll enthalten sein")
        Assert.IsTrue(s.Contains 3N 2N, "(3,2) soll enthalten sein")
        Assert.IsTrue(s.Contains 4N 0N, "(4,0) soll enthalten sein")
        Assert.IsFalse(s.Contains 4N 2N, "(4,2) soll nicht enthalten sein")
        Assert.IsFalse(s.Contains 3N 4N, "(3,4) soll nicht enthalten sein")

    [<TestMethod>] [<Timeout(20000)>]
    member this.``d) Beispiel: move`` (): Unit =
        let s = Shapes.move (1N, 9N) (Shapes.square 4N)
        Assert.AreEqual(5N, s.Rightmost)
        Assert.AreEqual(13N, s.Topmost)
        Assert.IsTrue(s.Contains 1N 9N, "(1,9) soll enthalten sein")
        Assert.IsTrue(s.Contains 5N 13N, "(5,13) soll enthalten sein")
        Assert.IsTrue(s.Contains 5N 9N, "(5,9) soll enthalten sein")
        Assert.IsTrue(s.Contains 1N 13N, "(1,13) soll enthalten sein")
        Assert.IsFalse(s.Contains 4N 4N, "(4,4) soll nicht enthalten sein")
        Assert.IsFalse(s.Contains 1N 14N, "(1,14) soll nicht enthalten sein")
        Assert.IsFalse(s.Contains 0N 0N, "(0,0) soll nicht enthalten sein")


    [<TestMethod>] [<Timeout(20000)>]
    member this.``alle) Contains smiley`` (): Unit =
        let s = smiley()
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
                    s.Contains x y,
                    sprintf "Ihre Contains Funktion gibt für die Smiley Form (s. Übungsblatt 5, Aufgabe 4) an Koordinate %A ein falsches Ergebnis zurück!" (x, y)
                )

    [<TestMethod>] [<Timeout(20000)>]
    member this.``alle) Rightmost smiley`` (): Unit =
        let s = smiley()
        Assert.AreEqual(22N, s.Rightmost)

    [<TestMethod>] [<Timeout(20000)>]
    member this.``alle) Topmost smiley`` (): Unit =
        let s = smiley()
        Assert.AreEqual(13N, s.Topmost)

    [<TestMethod>] [<Timeout(20000)>]
    member this.``alle) Contains Rightmost Topmost Plausibilität`` (): Unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 20}, fun (s: IShape) ->
            let rm = s.Rightmost
            let tm = s.Topmost
            // Suche Punkt auf senkrechter Linie bei x=rightmost
            Assert.IsTrue(
                List.exists (fun y -> s.Contains rm y) [0N..tm],
                sprintf "s.Rightmost ist %A, s.Topmost ist %A, aber es gibt kein y in [0N..%A] mit s.Contains (%A, y)" rm tm tm rm
            )
            // Suche Punkt auf waagerechter Linie bei y=topmost
            Assert.IsTrue(
                List.exists (fun x -> s.Contains x tm) [0N..rm],
                sprintf "s.Rightmost s ist %A, s.Topmost ist %A, aber es gibt kein x in [0N..%A] mit s.Contains (x, %A)" rm tm rm tm
            )
            // Suche Punkte rechts von rightmost
            for x in [rm+1N..rm+10N] do
                for y in [0N..tm+10N] do
                    Assert.IsFalse(
                        s.Contains x y,
                        sprintf "s.Rightmost ist %A, aber s.Contains %A ist trotzdem true" rm (x, y)
                    )
            // Suche Punkte oberhalb von topmost
            for y in [tm+1N..tm+10N] do
                for x in [0N..rm+10N] do
                    Assert.IsFalse(
                        s.Contains x y,
                        sprintf "s.Topmost ist %A, aber s.Contains %A ist trotzdem true" tm (x, y)
                    )
        )
