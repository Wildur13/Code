module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open System
open Mini
open SearchTypes

type NTree<'T> =
    | NEmpty
    | NNode of NTree<'T> * 'T * NTree<'T>

[<StructuredFormatDisplay("{ToString}")>]
type TestInput =
    | TI of NTree<Nat> * Nat list * Nat // tree, list is inorder, height
    member this.ToString =
        let (TI (t, _, _)) = this
        sprintf "%A" t


type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())

    static member TestInput() =
        Arb.fromGen (
            let rec generator lo hi size =
                gen {
                    if size = 0 || lo > hi then return TI (NEmpty, [], 0N)
                    else
                        let! sizeL = Gen.choose(0, size/2)
                        let! sizeR = Gen.choose(0, size/2)
                        let! x = Gen.choose(lo, hi)
                        let! TI (tl, ll, hl) = generator lo (x - 1) sizeL
                        let! TI (tr, lr, hr) = generator (x + 1) hi sizeR
                        return TI (NNode (tl, Nat.Make x, tr), ll @ [Nat.Make x] @ lr, 1N + max hl hr)
                }
            Gen.sized (generator 0 50)
        )


let rec PTree2NTree<'T> (ptree: PTree<'T>): NTree<'T> =
    match !ptree with
    | Empty -> NEmpty
    | Node (l, x, r) -> NNode ((PTree2NTree l), x, (PTree2NTree r))

let rec NTree2PTree<'T> (ntree: NTree<'T>): PTree<'T> =
    match ntree with
    | NEmpty -> ref Empty
    | NNode (l, x, r) -> ref (Node (NTree2PTree l, x, NTree2PTree r))

[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    let ex1: PTree<Nat> = ref Empty
    let ex2 = ref (Node ((ref Empty), 2N, (ref Empty)))
    let ex3 = ref (
                Node (
                      ref ( Node ((ref Empty), 1N, (ref Empty)) )
                    , 3N
                    , ref ( Node ((ref Empty), 5N, (ref Empty)) )
                )
              )
    let ex4 = ref (
                Node (
                      ref ( Node ((ref Empty), 2N, (ref Empty)) )
                    , 5N
                    , ref Empty 
                )
              )
            
    // ------------------------------------------------------------------------
    // a)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) size Beispiel 1`` (): unit =
        Assert.AreEqual(0N, Search.size ex1)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) size Beispiel 2`` (): unit =
        Assert.AreEqual(1N, Search.size ex2)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) size Beispiel 3`` (): unit =
        Assert.AreEqual(3N, Search.size ex3)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) size Beispiel 4`` (): unit =
        Assert.AreEqual(2N, Search.size ex4)

    [<TestMethod>] [<Timeout(10000)>]
    member this.``a) size Zufall`` (): unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (TI (t, l, _)) ->
            Assert.AreEqual(List.length l |> Nat.Make, Search.size <| NTree2PTree t)
        )

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) height Beispiel 1`` (): unit =
        Assert.AreEqual(0N, Search.height ex1)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) height Beispiel 2`` (): unit =
        Assert.AreEqual(1N, Search.height ex2)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) height Beispiel 4`` (): unit =
        Assert.AreEqual(2N, Search.height ex4)

    [<TestMethod>] [<Timeout(10000)>]
    member this.``a) height Zufall`` (): unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (TI (t, _, h)) ->
            Assert.AreEqual(h, Search.height <| NTree2PTree t)
        )

    // ------------------------------------------------------------------------
    // b)


    [<TestMethod>] [<Timeout(1000)>]
    member this.``b) contains Beispiel 1`` (): unit =
        Assert.AreEqual(true, Search.contains 5N ex3)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``b) contains Beispiel 2`` (): unit =
        Assert.AreEqual(false, Search.contains 7N ex3)

    [<TestMethod>] [<Timeout(10000)>]
    member this.``b) contains Zufall`` (): unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (x: Nat) (TI (t, l, _)) ->
            Assert.AreEqual(List.contains x l, Search.contains x <| NTree2PTree t)
        )


    // ------------------------------------------------------------------------
    // c)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) insert Beispiel 1`` (): unit =
        let a: PTree<Nat> = ref Empty
        Search.insert 1N a
        Assert.AreEqual(NNode (NEmpty, 1N, NEmpty), PTree2NTree a)

    [<TestMethod>] [<Timeout(30000)>]
    member this.``c) insert Zufall bereits enthalten`` (): unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (x: Nat) (TI (t, l, _)) ->
            if List.contains x l then
                let t1 = NTree2PTree t
                let t2 = NTree2PTree t
                Search.insert x t2
                Assert.AreEqual(t1, t2, "Der Suchbaum darf durch das Einfüen eines bereits enthaltenen Elements nicht verändert werden!")
        )

    [<TestMethod>] [<Timeout(30000)>]
    member this.``c) insert Zufall noch nicht enthalten`` (): unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 300}, fun (x: Nat) (TI (t, l, _)) ->
            if not <| List.contains x l then
                let t = NTree2PTree t
                Search.insert x t
                Assert.AreEqual(
                    (List.length l |> Nat.Make) + 1N,
                    Search.size t,
                    sprintf "Größe nach dem Einfügen ist falsch!\n%A" t
                )
                for i in 0N .. 15N do
                    Assert.AreEqual(
                        i = x || List.contains i l,
                        Search.contains i t,
                        sprintf "Nach dem Einfügen ist contains %A t falsch!\n%A" i t
                    )
        )

    // ------------------------------------------------------------------------
    // d)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``d) delete Beispiel 1`` (): unit =
        let a = NNode (NNode (NNode (NEmpty,1N,NEmpty),2N,NNode (NEmpty,3N,NEmpty)),5N,NNode (NNode (NEmpty,6N, NEmpty) ,7N ,NNode (NEmpty ,8N ,NEmpty )))
        let b = NNode (NNode (NNode (NEmpty,1N,NEmpty),2N,NNode (NEmpty,3N,NEmpty)),6N,NNode (NEmpty,7N,NNode (NEmpty,8N,NEmpty)))
        let x = NTree2PTree a
        Search.delete 5N x
        let c = PTree2NTree x

        //let a = ref (Node ((ref Empty), 2N, (ref Empty)))
        //Search.delete 2N a
        //let e : NTree<Nat> = NEmpty
        Assert.AreEqual(b, c)

    [<TestMethod>] [<Timeout(10000)>]
    member this.``d) delete Zufall nicht enthalten`` (): unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (x: Nat) (TI (t, l, _)) ->
            if not <| List.contains x l then
                let t1 = NTree2PTree t
                let t2 = NTree2PTree t
                Search.delete x t2
                Assert.AreEqual(t1, t2, "Der Suchbaum darf durch das Löschen eines nicht enthaltenen Elements nicht verändert werden!")
        )

    [<TestMethod>] [<Timeout(30000)>]
    member this.``d) delete Zufall enthalten`` (): unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 300}, fun (x: Nat) (TI (t, l, _)) ->
            if List.contains x l then
                let t = NTree2PTree t
                Search.delete x t
                Assert.AreEqual(
                    (List.length l |> Nat.Make) - 1N,
                    Search.size t,
                    sprintf "Größe nach dem Löschen ist falsch!\n%A" t
                )
                for i in 0N .. 15N do
                    Assert.AreEqual(
                        i <> x && List.contains i l,
                        Search.contains i t,
                        sprintf "Nach dem Löschen ist contains %A t falsch!\n%A" i t
                    )
        )
