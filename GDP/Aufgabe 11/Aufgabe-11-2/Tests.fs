module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open System
open Mini
open RingBufferTypes

// ----- verwende Necklace aus Vorlesung als Referenzimplementierung
[<ReferenceEquality>]
type Necklace<'elem> = { mutable bead : 'elem; mutable next : Necklace<'elem> }

let single x =
    let rec item = { bead = x; next = item }
    in item

let swap (p : Necklace<'elem>, q : Necklace<'elem>) =
    let tmp = p.next
    p.next <- q.next
    q.next <- tmp

let append (last1 : Necklace<'elem>, last2 : Necklace<'elem>) : Necklace<'elem> =
    swap (last1, last2)
    last2

let cons (x, xs) = append (single x, xs)

let head (last : Necklace<'elem>) : 'elem =
    last.next.bead

let tail (last : Necklace<'elem>) : Necklace<'elem> =
    swap (last, last.next)
    last

let rec replicate (n : Int) (a :'a) : Necklace<'a> =  // a necklace with n + 1 beads
    if n = 0 then single a else append (replicate (n - 1) a, single a)


// ------
let create<'elem> (size: Int): Necklace<'elem option> =
    let mutable buffer = replicate (size-1) None
    buffer

let put (buffer: Necklace<'elem option> ref) (e: 'elem): unit =
    (!buffer).bead <- Some e
    buffer := (!buffer).next

let rec get (buffer: Necklace<'elem option> ref) (size: Int): 'elem option =
    if (!buffer).bead = None then (if size > 0 then get (ref (!buffer).next) (size-1) else None)
    else let res = (!buffer).bead
         (!buffer).bead <- None
         res

let r2nBuffer<'T> (r: RingBuffer<'T>): Necklace<'T option> =
    //let len = max r.buffer.Length 1 // generiere keine leeren RingBuffer (ist in TestInput behoben)
    let len = r.buffer.Length
    let n = ref (create<'T> len)
    let offset = fun (i: Int) -> ((len + !r.writePos - !r.size ) % len  + i) % len
    let rec help (i: Int) =
        if i < !r.size then 
            put n r.buffer.[offset i]
            help (i+1)
    help 0
    !n

// ------


[<StructuredFormatDisplay("{ToString}")>]
type TestInput<'T> =
    | TI of RingBuffer<'T> * string
    member this.ToString =
        let (TI (_, s)) = this
        s


type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())

    static member TestInput<'T>() =
        Arb.from<Array<'T> * Int * Int>
        |> Arb.filter (fun (buffer, size, writePos) -> size <= buffer.Length && writePos < buffer.Length && size >= 0 && writePos >= 0)
        |> Arb.convert
            (
                fun (buffer, size, writePos) ->
                    let rb = {buffer=buffer; size=ref size; writePos=ref writePos}
                    TI (rb, sprintf "%A" rb)
            ) (
                fun (TI (i, _)) -> (i.buffer, !i.size, !i.writePos)
            ) 

[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    let ex1 = fun () -> { buffer = [|0; 0; 0|]; size = ref 0; writePos = ref 0 }
    let ex2 = fun () -> { buffer = [|1; 2; 3|]; size = ref 1; writePos = ref 1 }
    let ex3 = fun () -> { buffer = [|7; 3; 6; 1; 20; 15; 17; 4; 9; 12|]; size = ref 6; writePos = ref 3 }


    // ------------------------------------------------------------------------
    // a)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) Beispiel`` (): unit =
        let capacity = 10
        let rb = RingBuffer.create<Int> capacity
        Assert.AreEqual(capacity, Array.length rb.buffer, "Kapazität des Puffers stimmt nicht.")
        Assert.AreEqual(0, !rb.size, "size stimmt nicht")
        Assert.AreEqual(0, !rb.writePos, "writePos stimmt nicht")

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) Zufall`` (): unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (n: Nat) ->
            if n > 0N then
                let capacity = int n
                let rb = RingBuffer.create<String> capacity
                Assert.AreEqual(capacity, Array.length rb.buffer, "Kapazität des Puffers stimmt nicht.")
                Assert.AreEqual(0, !rb.size, "size stimmt nicht")
                Assert.AreEqual(0, !rb.writePos, "writePos stimmt nicht")
        )


    // ------------------------------------------------------------------------
    // b)

    [<TestMethod>] [<Timeout(10000000)>]
    member this.``b) Beispiel 1`` (): unit =
        let ex = ex1()
        Assert.AreEqual(None, RingBuffer.get ex, "get liefert ein Element, obwohl keines enthalten ist.")
        Assert.AreEqual(ex1(), ex, "RingBuffer hat sich verändert, obwohl kein Objekt zurückgegeben wurde.")

    [<TestMethod>] [<Timeout(1000000)>]
    member this.``b) Beispiel 2`` (): unit =
        let ex = ex2()
        Assert.AreEqual(Some 1, RingBuffer.get ex)
        Assert.AreEqual(0, !ex.size, "Anzahl enthaltener Elemente ist nicht um 1 kleiner geworden.")

    [<TestMethod>] [<Timeout(1000000)>]
    member this.``b) Beispiel 3`` (): unit =
        let ex = ex3()
        let es = [4; 9; 12; 7; 3; 6]
        let count = ref (List.length es)
        for expected in es do
            count := !count - 1
            Assert.AreEqual(Some expected, RingBuffer.get ex)
            Assert.AreEqual(!count, !ex.size, "Anzahl enthaltener Elemente ist nicht um 1 kleiner geworden.")
        Assert.AreEqual(None, RingBuffer.get ex, "Ringpuffer sollte leer sein.")


    [<TestMethod>] [<Timeout(1000000)>]
    member this.``b) Zufall: Array von n Zufallszahlen, size=n, writePos=0. get bis size=0 soll alle Inhalte des ursprünglichen Arrays ergeben`` (): unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun (ar: Array<Int>) ->
            let rb = {buffer=ar; size=ref ar.Length; writePos=ref 0}
            let rec help (idx: Int) =
                let sizeVorher = !rb.size
                match RingBuffer.get rb with
                | None -> Assert.AreEqual(ar.Length, idx, "get liefert None obwohl noch Elemente vorhanden sind.")
                | Some e -> Assert.AreEqual(ar.[idx], e, "get sollte Array Element an Stelle "+(string idx)+" ausgeben.")
                            Assert.AreEqual(sizeVorher-1, !rb.size, "Anzahl enthaltener Elemente ist nicht um 1 kleiner geworden.")
                if idx < ar.Length then help (idx + 1) else Assert.AreEqual(None, RingBuffer.get rb, "Ringpuffer gibt Elemente zurück, obwohl None erwartet wird.")
            help 0
        )

    [<TestMethod>] [<Timeout(1000000)>]
    member this.``b) Zufall RingBuffer: ein get`` (): unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun ( TI (rb, _): TestInput<Int> ) ->
            // TestInput to Necklace
            let n = ref (r2nBuffer rb)
            Assert.AreEqual(get n (rb.buffer.Length), RingBuffer.get rb, "Hinweis: null in dieser Fehlermeldung ist None")
        )

    [<TestMethod>] [<Timeout(2000000)>]
    member this.``b) Zufall RingBuffer: size gets`` (): unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun ( TI (rb, _): TestInput<String> ) ->
            // TestInput to Necklace
            let n = ref (r2nBuffer rb)
            for i in 0..!rb.size do
                  Assert.AreEqual(get n (rb.buffer.Length), RingBuffer.get rb, "Hinweis: null in dieser Fehlermeldung ist None") |> ignore
        )


    // ------------------------------------------------------------------------
    // c)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) Beispiel 1`` (): unit =
        let ex = ex1()
        RingBuffer.put ex 30
        Assert.AreEqual(30, ex.buffer.[0], "Element nicht korrekt eingefügt.")
        Assert.AreEqual(1, !ex.size, "size wurde nicht erhöht.")
        Assert.AreEqual(1, !ex.writePos, "writePos wurde nicht verändert")

    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) Beispiel 2`` (): unit =
        let ex = ex2()
        RingBuffer.put ex 30
        Assert.AreEqual(30, ex.buffer.[1], "Element nicht korrekt eingefügt.")
        Assert.AreEqual(2, !ex.size, "size wurde nicht erhöht.")
        Assert.AreEqual(2, !ex.writePos, "writePos wurde nicht verändert")
        RingBuffer.put ex 40
        Assert.AreEqual(40, ex.buffer.[2], "Element nicht korrekt eingefügt.")
        Assert.AreEqual(3, !ex.size, "size wurde nicht erhöht.")
        Assert.AreEqual(0, !ex.writePos, "writePos wurde nicht verändert")
        RingBuffer.put ex 50
        Assert.AreEqual(50, ex.buffer.[0], "Element nicht korrekt eingefügt.")
        Assert.AreEqual(3, !ex.size, "size nicht korrekt (soll nicht Größer sein als die Kapazität des Ringpuffers).")
        Assert.AreEqual(1, !ex.writePos, "writePos wurde nicht verändert")

    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) Beispiel 3`` (): unit =
        let ex = ex3()
        RingBuffer.put ex 30
        Assert.AreEqual(30, ex.buffer.[3], "Element nicht korrekt eingefügt.")
        Assert.AreEqual(7, !ex.size, "size wurde nicht erhöht.")
        Assert.AreEqual(4, !ex.writePos, "writePos wurde nicht verändert")
        RingBuffer.put ex 40
        Assert.AreEqual(40, ex.buffer.[4], "Element nicht korrekt eingefügt.")
        Assert.AreEqual(8, !ex.size, "size wurde nicht erhöht.")
        Assert.AreEqual(5, !ex.writePos, "writePos wurde nicht verändert")
        RingBuffer.put ex 50
        Assert.AreEqual(50, ex.buffer.[5], "Element nicht korrekt eingefügt.")
        Assert.AreEqual(9, !ex.size, "size wurde nicht erhöht.")
        Assert.AreEqual(6, !ex.writePos, "writePos wurde nicht verändert")
        RingBuffer.put ex 60
        Assert.AreEqual(60, ex.buffer.[6], "Element nicht korrekt eingefügt.")
        Assert.AreEqual(10, !ex.size, "size wurde nicht erhöht.")
        Assert.AreEqual(7, !ex.writePos, "writePos wurde nicht verändert")
        RingBuffer.put ex 70
        Assert.AreEqual(70, ex.buffer.[7], "Element nicht korrekt eingefügt.")
        Assert.AreEqual(10, !ex.size, "size nicht korrekt (soll nicht Größer sein als die Kapazität des Ringpuffers).")
        Assert.AreEqual(8, !ex.writePos, "writePos wurde nicht verändert")        

    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) Zufall: (setzt voraus, dass get funktioniert)`` (): unit =
        Check.One({Config.QuickThrowOnFailure with MaxTest = 1000}, fun ( TI (rb, _): TestInput<Int>, elems: Int list ) ->
            // TestInput to Necklace
            let sizeBegin = !rb.size 
            let n = ref (r2nBuffer rb)
            for e in elems do // alle elems einfügen
                  RingBuffer.put rb e
                  put n e
            let sizeEnd = min (sizeBegin + List.length elems) rb.buffer.Length
            for i in 0..sizeEnd do // alle verfügbaren lesen
                  Assert.AreEqual(get n (rb.buffer.Length), RingBuffer.get rb, "Hinweis: null in dieser Fehlermeldung ist None") |> ignore
        )
