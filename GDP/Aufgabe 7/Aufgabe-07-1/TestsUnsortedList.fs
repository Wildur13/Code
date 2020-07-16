module TestsUnsortedList

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open Mini

let empty = PriorityQueueUnsortedList.empty
let isEmpty = PriorityQueueUnsortedList.isEmpty
let insert = PriorityQueueUnsortedList.insert
let extractMin = PriorityQueueUnsortedList.extractMin

type Action<'a> =
    | Insert of Nat * 'a
    | IsEmpty
    | ExtractMin

type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())

[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    [<TestMethod>]
    member this.``UnsortedList: isEmpty empty`` (): unit =
        Assert.IsTrue(isEmpty empty)

    [<TestMethod>]
    member this.``UnsortedList: isEmpty (insert empty {priority=5N; elem="foo"})`` (): unit =
        Assert.IsFalse(isEmpty (insert empty {priority=5N; elem="foo"}))

    [<TestMethod>]
    member this.``UnsortedList: extractMin empty)`` (): unit =
        match extractMin empty with
        | None -> ()
        | result -> Assert.Fail(sprintf "extractMin empty muss None zurückgeben, liefert aber %A" result)

    [<TestMethod>]
    member this.``UnsortedList: Zufallstest`` (): unit =
        Check.QuickThrowOnFailure(fun (actions: Action<int> list) ->
            let mutable q = empty
            let mutable l = []
            for a in actions do
                match a with
                | Insert (p, x) ->
                    l <- (p,x)::l
                    q <- insert q {priority = p; elem = x}
                | IsEmpty -> Assert.AreEqual(List.isEmpty l, isEmpty q, "isEmpty liefert falsches Ergebnis")
                | ExtractMin ->
                    if List.isEmpty l then
                        match extractMin q with
                        | Some _ -> Assert.Fail("extractMin liefert Ergebnis obwohl die Warteschlange leer ist")
                        | _ -> ()
                    else
                        match extractMin q with
                        | None -> Assert.Fail("extractMin liefert None obwohl die Warteschlange nicht leer ist")
                        | Some ({priority = p; elem = x}, q') ->
                            if l |> List.map fst |> List.min < p then
                                Assert.Fail("extractMin liefert nicht die kleinste enthaltene Priorität")
                            match List.partition (fun z -> z = (p,x)) l with
                            | ([], _) -> Assert.Fail("extractMin liefert ein nicht enthaltenes Priorität-Element-Paar")
                            | (_::l', l'') -> l <- l'@l''; q <- q'
        )
