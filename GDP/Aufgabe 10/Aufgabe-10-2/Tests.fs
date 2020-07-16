module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open System.Text.RegularExpressions
open FsCheck
open Mini
open BibTypes


[<StructuredFormatDisplay("{s}")>]
type SafeString = SS of s: string

type SafeBuch = SB of sb: Buch


let sample(): Buch list = 
    let bib = [
          {titel="Algorithms"; exemplare=[ref Verfuegbar; ref (Dauerleihe "AG Softwaretechnik")]; warteliste=ref []}
        ; {titel="Grundlagen der Programmierung"; exemplare=[ref Verfuegbar; ref Verfuegbar]; warteliste=ref []}
        ; {titel="F# for Beginners"; exemplare=[ref (Dauerleihe "AG Softwaretechnik"); ref (Dauerleihe "AG Softwaretechnik")]; warteliste=ref []}
        ; {titel="F# for Experts"; exemplare=[ref (Dauerleihe "AG Softwaretechnik"); ref (NormaleLeihe "Lisa Lista"); ref Verfuegbar]; warteliste=ref []}
        ; {titel="Introduction to Python"; exemplare=[ref Verfuegbar; ref (NormaleLeihe "Harry Hacker")]; warteliste=ref []}
        ]
    bib

let mixcase (s: String): String =
    let cs = [for x in s -> x]
    let rec m (s: char list) (p: int): char list =
        match s with
        | [] -> []
        | c::cs -> if p=0 then (System.Char.ToLower c) :: (m cs 1)
                   else (System.Char.ToUpper c) :: (m cs 0)
    let mixed = m cs 0
    let sb = System.Text.StringBuilder(mixed.Length)
    mixed |> List.iter (sb.Append >> ignore)
    sb.ToString()



type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())

    static member SafeString() =
        Arb.from<string>
        |> Arb.filter (not << isNull)
        |> Arb.convert (String.filter (fun c -> (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))) (id)
        |> Arb.convert (SS) (fun (SS s) -> s)

    static member SafeBuch() =
        Arb.from<Buch>
        |> Arb.filter (fun b -> not (isNull b.titel))
        |> Arb.filter (fun b -> String.length b.titel > 3)
        |> Arb.convert (fun x -> {titel=(String.filter (fun c -> (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) x.titel); exemplare=x.exemplare; warteliste=x.warteliste}) (id)
        |> Arb.convert (SB) (fun (SB sb) -> sb)


[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    // ------------------------------------------------------------------------
    // a)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) Beispiel: erfolgreiche Suche nach komplettem Titel`` (): unit =
        // jedes Buch in der Bibliothek muss unter seinem vollständigen Titel auffindbar sein
        let bib = sample() // Neue Instanz
        List.map (fun (buch: Buch) ->
            Assert.IsTrue(List.length (Bib.sucheTitel bib buch.titel) = 1, "Im Beispiel sind Buchtitel eindeutig, daher soll bei der Suche nach einem vollständigen Buchtitel genau ein Buch gefunden werden.")
            Assert.AreEqual([buch], Bib.sucheTitel bib buch.titel, "Es wurde nicht das korrekte Buch gefunden.")
        ) bib |> ignore

    [<TestMethod>] [<Timeout(10000)>]
    member this.``a) Zufall: erfolgreiche Suche in zufälliger Bibliothek`` (): unit =
        // jedes Buch in der Bibliothek muss unter seinem vollständigen Titel auffindbar sein
        Check.One({Config.QuickThrowOnFailure with EndSize = 100}, fun (rbib: SafeBuch list) ->
            let rbib = List.map (fun (SB sb) -> sb) rbib
            List.map (fun (buch: Buch) ->
                if buch.titel <> "" then // leere Suche gibt alle Bücher aus
                    // verwende contains, falls ein Buchtitel substring eines anderen Buchtitels ist
                    Assert.IsTrue(List.contains buch (Bib.sucheTitel rbib buch.titel))
                else
                    Assert.AreEqual(List.length rbib, List.length (Bib.sucheTitel rbib buch.titel))
            ) rbib |> ignore
        )

    [<TestMethod>] [<Timeout(10000)>]
    member this.``a) Zufall: Suche nach leerem String in Bibliothek gibt gesamte Bibliothek zurück`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 100}, fun (rbib: SafeBuch list) ->
            let rbib = List.map (fun (SB sb) -> sb) rbib
            Assert.AreEqual(List.length rbib, List.length (Bib.sucheTitel rbib ""))
        )

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) Beispiel: erfolglose Suche`` (): unit =
        let bib = sample() // Neue Instanz
        List.map (fun (titel: String) ->
            Assert.AreEqual(List.empty<Buch>, Bib.sucheTitel bib titel, "Buch gefunden, obwohl es nicht in der Bibliothek ist.")
        ) [" Algorithms"; "BAlgorithms"; "F+"; "Beginnners"; "Python "] |> ignore

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) Beispiel: erfolgreiche Suche nach Teilstring`` (): unit =
        let bib = sample() // Neue Instanz
        Assert.AreEqual(2, List.length (Bib.sucheTitel bib "for"))

    [<TestMethod>] [<Timeout(10000)>]
    member this.``a) Zufall: erfolgreiche Suche nach Teilstring`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 100}, fun (rbib: SafeBuch list) ->
            let rbib = List.map (fun (SB sb) -> sb) rbib
            let fbs = rbib |> List.filter (fun b -> String.length b.titel > 3)
                           |> List.map (fun b -> b.titel.[0..2])
            if not (List.isEmpty fbs) then
                fbs |> List.map (fun b -> Assert.IsTrue(List.length (Bib.sucheTitel rbib b) > 0)) |> ignore
        )

    [<TestMethod>] [<Timeout(1000)>]
    member this.``a) Beispiel: erfolgreiche Suche - Groß-/Kleinschreibung soll ignoriert werden`` (): unit =
        let bib = sample() // Neue Instanz
        List.map (fun (buch: Buch) ->
            Assert.AreEqual([buch], Bib.sucheTitel bib (mixcase buch.titel), "Groß-/Kleinschreibung wird nicht ignoriert.")
        ) bib |> ignore


    [<TestMethod>] [<Timeout(10000)>]
    member this.``a) Zufall: erfolgreiche Suche - Groß-/Kleinschreibung soll ignoriert werden`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 100}, fun (rb: SafeBuch) ->
            let (SB rb) = rb
            Assert.AreEqual([rb], Bib.sucheTitel [rb] (mixcase rb.titel), "Groß-/Kleinschreibung wird nicht ignoriert.")
        )

    // ------------------------------------------------------------------------
    // b)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``b) Beispiel: 1. Buch nicht vorhanden`` (): unit =
        let bib = sample()
        Assert.AreEqual(None, Bib.leiheBuch bib "Python" "Harry Hacker")
        Assert.AreEqual(sample(), bib, "Zustand der Bibliothek wurde verändert, obwohl das Buch nicht auf der Liste der Bibliothek steht.")
    

    [<TestMethod>] [<Timeout(10000)>]
    member this.``b) Zufall: 1. Buch nicht vorhanden`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 100}, fun (rb: SafeBuch list) ->
            let rb = List.map (fun (SB sb) -> sb) rb
            Assert.AreEqual(None, Bib.leiheBuch rb "123" "Harry Hacker") // (da nach def. oben keine Zahlen in Titel erlaubt)
        )


    [<TestMethod>] [<Timeout(1000)>]
    member this.``b) Beispiel: 2. Exemplar verfügbar`` (): unit =
        let p1 = "Bob"
        let p2 = "Eve"
        let p3 = "Alice"
        let v1 = ref Verfuegbar
        let ws = ref []
        let mkbuch = fun() -> {titel="Algorithms"; exemplare=[ref (Dauerleihe "AG Softwaretechnik"); ref (NormaleLeihe "Lisa Lista"); v1]; warteliste=ws}
        let bib = [mkbuch()]

        Assert.AreEqual(Some ErfolgreichAusgeliehen, Bib.leiheBuch bib "Algorithms" p1, "Rückgabe ist nicht ErfolgreichAusgeliehen, obwohl ein Exemplar verfügbar ist")
        Assert.AreEqual(NormaleLeihe p1, !v1, "Statusänderung des Exemplars nach NormaleLeihe Bob war nicht erfolgreich.")
        Assert.AreEqual([mkbuch()], bib, "Es wurde mehr als nur der Zustand des ausgeliehenen Buches verändert.")


    [<TestMethod>] [<Timeout(1000)>]
    member this.``b) Beispiel: 3. Alle Exemplare in Dauerleihe`` (): unit =
        let bib = sample()
        Assert.AreEqual(Some NichtVerfuegbar, Bib.leiheBuch bib "F# for Beginners" "Eve")
        Assert.AreEqual(sample(), bib, "Zustand wurde verändert, obwohl kein Exemplar verfügbar.")


    [<TestMethod>] [<Timeout(1000)>]
    member this.``b) Beispiel: 4. Warteliste`` (): unit =
        let p1 = "Bob"
        let p2 = "Eve"
        let p3 = "Alice"
        let v1 = ref (NormaleLeihe p1)
        let ws = ref []
        let buch = {titel="F# for Experts"; exemplare=[ref (Dauerleihe "AG Softwaretechnik"); ref (NormaleLeihe "Lisa Lista"); v1]; warteliste=ws}
        let bib = [buch]

        // Warteliste: Eintrag 1
        Assert.AreEqual(Some Warteliste, Bib.leiheBuch bib "F# for Experts" p2)
        Assert.AreEqual([p2], !buch.warteliste)

        // Warteliste: Eintrag 2
        Assert.AreEqual(Some Warteliste, Bib.leiheBuch bib "F# for Experts" p3)
        Assert.AreEqual([p2;p3], !buch.warteliste, "Warteliste hat nicht die richtige Reihenfolge.")

    // ------------------------------------------------------------------------
    // c)

    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) Beispiel: 1. Rückgabe nicht möglich wenn ein Buch nicht in der Bibliothek gelistet ist`` (): unit =
        let bib = sample()
        Assert.AreEqual(false, Bib.rueckgabe bib "Python" "Harry Hacker")
        Assert.AreEqual(sample(), bib, "Zustand wurde verändert, obwohl kein Exemplar zurückgegeben wurde.")

    [<TestMethod>] [<Timeout(10000)>]
    member this.``c) Zufall: 1. Rückgabe nicht möglich wenn ein Buch nicht in der Bibliothek gelistet ist`` (): unit =
        Check.One({Config.QuickThrowOnFailure with EndSize = 100}, fun (rb: SafeBuch list) ->
            let rb = List.map (fun (SB sb) -> sb) rb
            Assert.AreEqual(false, Bib.rueckgabe rb "123" "Harry Hacker") // (da nach def. oben keine Zahlen in Titel erlaubt)
        )


    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) Beispiel: 2. Rückgabe ohne Warteliste`` (): unit =
        let v1 = ref (NormaleLeihe "Lisa Lista")
        let ws = ref []
        let mkbuch = fun() -> {titel="F# for Experts"; exemplare=[ref (Dauerleihe "AG Softwaretechnik"); v1; ref Verfuegbar]; warteliste=ws}
        let buch = mkbuch()
        let bib = [buch]

        Assert.AreEqual(true, Bib.rueckgabe bib "F# for Experts" "Lisa Lista", "Rückgabewert nicht korrekt")
        Assert.AreEqual(Verfuegbar, !v1, "Exemplar ist nicht in den Status Verfuegbar gewechselt.")
        Assert.AreEqual([mkbuch()], bib, "Es hat sich mehr am Zustand verändert, als nur die Rückgabe des Buches.")


    [<TestMethod>] [<Timeout(1000)>]
    member this.``c) Beispiel: 3. Rückgabe mit Warteliste`` (): unit =
        let v1 = ref (NormaleLeihe "Lisa Lista")
        let v2 = ref (NormaleLeihe "Harry Hacker")
        let ws = ref ["Alice"; "Bob"]
        let mkbuch = fun() -> {titel="F# for Experts"; exemplare=[ref (Dauerleihe "AG Softwaretechnik"); v1; v2]; warteliste=ws}
        let buch = mkbuch()
        let bib = [buch]

        Assert.AreEqual(true, Bib.rueckgabe bib "F# for Experts" "Lisa Lista", "Rückgabewert nicht korrekt")
        Assert.AreEqual(NormaleLeihe "Alice", !v1, "Das Buch wurde nach der Rückgabe nicht an die erste Person auf der Warteliste weiterverliehen.")
        Assert.AreEqual(["Bob"], !ws, "Die Warteliste ist nach dem Weiterverleihen nicht korrekt.")
