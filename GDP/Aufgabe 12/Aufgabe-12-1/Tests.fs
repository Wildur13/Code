module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open System
open Mini
open QueuesTypes


type Action<'T> =
    | Remove
    | Add of 'T

type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())

    static member Tree() =
        Arb.fromGen (
            let rec generator lo hi size =
                gen {
                    if size = 0 || lo > hi then return Empty
                    else
                        let! sizeL = Gen.choose(0, size/2)
                        let! sizeR = Gen.choose(0, size/2)
                        let! x = Gen.choose(lo, hi)
                        let! tl = generator lo (x - 1) sizeL
                        let! tr = generator (x + 1) hi sizeR
                        return Node (tl, Nat.Make x, tr)
                }
            Gen.sized (generator 0 50)
        )


// Referenzimplementierung IQueue mit Arrays (Achtung: ineffizient!)
let arrayQueue<'T> (): IQueue<'T> =
    let ar = ref (Array.zeroCreate<'T> 0)
    {
        new IQueue<'T> with
            member self.Add (elem: 'T): Unit =
                Array.Resize (ar, (!ar).Length + 1)
                (!ar).[(!ar).Length - 1] <- elem

            member self.Remove (): 'T option =
                if (!ar).Length > 0 then
                    let fst = (!ar).[0]
                    for i in 0..(!ar).Length-2 do (!ar).[i] <- (!ar).[i+1] // left shift elements
                    Array.Resize (ar, (!ar).Length - 1) // shrink by 1
                    Some fst
                else None
    }


// --------

// Referenzimplementierung Breitendurchlauf Baum ohne Warteschlange
let getLevel (root: Tree<'T>) (level: Nat): 'T list =
    let rec help (root: Tree<'T>) (targetLevel: Nat) (currentLevel: Nat) (acc: 'T List): 'T list =
        match root with
        | Empty -> acc
        | Node (left, x, right) ->
            if currentLevel = targetLevel then x :: acc
            else (help left targetLevel (currentLevel + 1N) acc) @ (help right targetLevel (currentLevel + 1N) acc)
    help root level 0N []

let rec bft' (root: Tree<'T>): 'T list =
    let rec help (root: Tree<'T>) (level: Nat) =
        let res = (getLevel root level)
        match res with
        | [] -> []
        | _ -> res @ help root (level+1N)
    help root 0N

// --------


[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore


    // Beispiel für d)
    let ex = Node (Node(Node(Empty,1,Empty),2,Node(Empty,3,Empty)) , 4, Node(Node(Empty,5,Empty),6,Node(Empty,7,Empty)))


    // ------------------------------------------------------------------------
    // a)

    [<TestMethod>] [<Timeout(2000)>]
    member this.``a) Beispiel 1: Remove bei leerer Queue`` (): Unit =
        let q = Queues.simpleQueue<Nat> ()
        Assert.AreEqual(None, q.Remove ())

    [<TestMethod>] [<Timeout(2000)>]
    member this.``a) Beispiel 2: Add und Remove`` (): Unit =
        let q = Queues.simpleQueue<Nat> ()
        q.Add 1N
        q.Add 2N
        Assert.AreEqual(Some 1N, q.Remove ())
        Assert.AreEqual(Some 2N, q.Remove ())
        Assert.AreEqual(None, q.Remove ())

    [<TestMethod>] [<Timeout(10000)>]
    member this.``a) Zufall: Add und Remove`` (): Unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (actions : Action<Int> list) ->
            let arQ = arrayQueue<Int> ()
            let q = Queues.simpleQueue<Int> ()
            for a in actions do
                match a with
                | Add elem -> arQ.Add elem; q.Add elem
                | Remove -> let expected = arQ.Remove ()
                            let actual = q.Remove ()
                            Assert.AreEqual(expected, actual)
        )


    // ------------------------------------------------------------------------
    // b)

    [<TestMethod>] [<Timeout(2000)>]
    member this.``b) Beispiel 1: Remove bei leerer Queue`` (): Unit =
        let q = Queues.advancedQueue<Nat> ()
        Assert.AreEqual(None, q.Remove ())

    [<TestMethod>] [<Timeout(2000)>]
    member this.``b) Beispiel 2: Add und Remove`` (): Unit =
        let q = Queues.advancedQueue<Nat> ()
        q.Add 1N
        q.Add 2N
        Assert.AreEqual(Some 1N, q.Remove ())
        q.Add 3N
        q.Add 4N
        Assert.AreEqual(Some 2N, q.Remove ())
        Assert.AreEqual(Some 3N, q.Remove ())
        Assert.AreEqual(Some 4N, q.Remove ())
        Assert.AreEqual(None, q.Remove ())

    [<TestMethod>] [<Timeout(10000)>]
    member this.``b) Zufall: Add und Remove`` (): Unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (actions : Action<Int> list) ->
            let arQ = arrayQueue<Int> ()
            let q = Queues.advancedQueue<Int> ()
            for a in actions do
                match a with
                | Add elem -> arQ.Add elem; q.Add elem
                | Remove -> let expected = arQ.Remove ()
                            let actual = q.Remove ()
                            Assert.AreEqual(expected, actual)
        )


    // ------------------------------------------------------------------------
    // c)

    [<TestMethod>] [<Timeout(2000)>]
    member this.``c) Beispiel 1: dequeue leere Queue`` (): Unit =
        let aQ = Queues.simpleQueue<Int> ()
        let bQ = Queues.advancedQueue<Int> ()
        Assert.AreEqual(List.empty<Int>, Queues.dequeue aQ)
        Assert.AreEqual(List.empty<Int>, Queues.dequeue bQ)

    [<TestMethod>] [<Timeout(2000)>]
    member this.``c) Beispiel 2: enqueue und dequeue`` (): Unit =
        let ex = [1;2;3;4;5]
        let aQ = Queues.simpleQueue<Int> ()
        let bQ = Queues.advancedQueue<Int> ()
        Queues.enqueue aQ ex
        Queues.enqueue bQ ex
        Assert.AreEqual(ex, Queues.dequeue aQ, "dequeue nach enqueue liefert mit simpleQueue falsches Ergebnis")
        Assert.AreEqual(ex, Queues.dequeue bQ, "dequeue nach enqueue liefert mit advancedQueue falsches Ergebnis")


    [<TestMethod>] [<Timeout(10000)>]
    member this.``c) Zufall: enqueue und dequeue`` (): Unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (actions : Action<Int> list, elems: Int list) ->
            let aQ = Queues.simpleQueue<Int> ()
            let bQ = Queues.advancedQueue<Int> ()
            let rQ = arrayQueue<Int> ()
            for a in actions do
                match a with
                | Add elem -> aQ.Add elem; bQ.Add elem; rQ.Add elem; 
                | Remove -> let expected = rQ.Remove ()
                            let actualA = aQ.Remove ()
                            let actualB = bQ.Remove ()
                            Assert.AreEqual(expected, actualA)
                            Assert.AreEqual(expected, actualB)
            Queues.enqueue aQ elems
            Queues.enqueue bQ elems
            Queues.enqueue rQ elems
            let resA = Queues.dequeue aQ
            let resB = Queues.dequeue bQ
            let resR = Queues.dequeue rQ // Ergebnis der Referenzimplementierung
            Assert.AreEqual(resR, resA, "dequeue nach enqueue liefert mit simpleQueue falsches Ergebnis")
            Assert.AreEqual(resR, resB, "dequeue nach enqueue liefert mit advancedQueue falsches Ergebnis")
            Assert.AreEqual(None, aQ.Remove (), "simpleQueue ist nach dequeue nicht leer")
            Assert.AreEqual(None, bQ.Remove (), "advancedQueue ist nach dequeue nicht leer")
        )


    // ------------------------------------------------------------------------
    // d)

    [<TestMethod>] [<Timeout(2000)>]
    member this.``d) Beispiel (setzt voraus, dass simpleQueue funktioniert)`` (): Unit =
        let q = Queues.simpleQueue<Tree<int>> ()
        q.Add ex
        let actual = Queues.bft q
        Assert.AreEqual([4;2;6;1;3;5;7], actual)

    [<TestMethod>] [<Timeout(2000)>]
    member this.``d) Beispiel (setzt voraus, dass advancedQueue funktioniert)`` (): Unit =
        let q = Queues.advancedQueue<Tree<int>> ()
        q.Add ex
        let actual = Queues.bft q
        Assert.AreEqual([4;2;6;1;3;5;7], actual)

    [<TestMethod>] [<Timeout(20000)>]
    member this.``d) Zufall (setzt voraus, dass simpleQueue funktioniert)`` (): Unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (root : Tree<Nat>) ->
            let q = Queues.simpleQueue<Tree<Nat>> ()
            q.Add root
            let actual = Queues.bft q
            let expected = bft' root
            Assert.AreEqual(expected, actual)
        )

    [<TestMethod>] [<Timeout(20000)>]
    member this.``d) Zufall (setzt voraus, dass advancedQueue funktioniert)`` (): Unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (root : Tree<Nat>) ->
            let q = Queues.advancedQueue<Tree<Nat>> ()
            q.Add root
            let actual = Queues.bft q
            let expected = bft' root
            Assert.AreEqual(expected, actual)
        )
