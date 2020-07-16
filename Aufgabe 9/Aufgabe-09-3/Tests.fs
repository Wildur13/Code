module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open Mini
open TestUtilsIO
open NimTypes
open Nim
open System.IO


[<StructuredFormatDisplay("{s}")>]
type SafeString = SS of s: string


type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())

    static member SafeString() =
        Arb.from<string>
        |> Arb.filter (not << isNull)
        |> Arb.convert (String.filter (fun c -> (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))) (id)
        |> Arb.convert (SS) (fun (SS s) -> s)


[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    let ioTimeout = 1000

    // ------------------------------------------------------------------------
    // a)

    [<TestMethod>] [<Timeout(10000)>]
    member this.``a) Beispiele ungültige natürliche Zahl`` (): unit =
        executeIOTest (
            (fun () -> Nim.queryNat "Eine Nachricht: " |> ignore),
            fun io ->
                io.timeout <- ioTimeout
                io.Expect("Eine Nachricht: ")
                io.WriteLine("abc")
                io.ExpectLine("Eingabe ist keine natuerliche Zahl!")
                io.Expect("Eine Nachricht: ")
                io.WriteLine("")
                io.ExpectLine("Eingabe ist keine natuerliche Zahl!")
                io.Expect("Eine Nachricht: ")
                io.WriteLine("-4711")
                io.ExpectLine("Eingabe ist keine natuerliche Zahl!")
                io.Expect("Eine Nachricht: ")
                io.WriteLine(".")
                io.ExpectLine("Eingabe ist keine natuerliche Zahl!")
                io.Expect("Eine Nachricht: ")
                io.WriteLine("123a+5")
                io.ExpectLine("Eingabe ist keine natuerliche Zahl!")
                io.Expect("Eine Nachricht: ")
                io.WriteLine("0") // beenden
                io.ExpectLine("Die Zahl 0 wurde eingegeben.")
        )

    [<TestMethod>] [<Timeout(10000)>]
    member this.``a) Zufall gültige natürliche Zahl`` (): unit =
        Check.QuickThrowOnFailure (fun (num: Nat) (msg: SafeString) ->
            let (SS msg) = msg
            executeIOTest (
                (fun () -> Nim.queryNat msg |> ignore),
                fun io ->
                    io.timeout <- ioTimeout
                    if msg <> "" then io.Expect(msg)
                    io.WriteLine(string (int num))
                    io.ExpectLine ("Die Zahl " + (string num) + " wurde eingegeben.")
            )
        )


    // ------------------------------------------------------------------------
    // b)

    [<TestMethod>] [<Timeout(10000)>]
    member this.``b) Beispiel 1`` (): unit =
        executeIOTest (
            (fun () -> Nim.queryMove 10N 3N A |> ignore),
            fun io ->
                io.timeout <- ioTimeout
                io.Expect("Es sind noch 10 Streichhoelzer uebrig. Spieler A ist am Zug: ")
                io.WriteLine("")
                io.ExpectLine("Eingabe ist keine natuerliche Zahl!")
                io.Expect("Es sind noch 10 Streichhoelzer uebrig. Spieler A ist am Zug: ")
                io.WriteLine("0")
                io.ExpectLine("Die Zahl 0 wurde eingegeben.")
                io.ExpectLine("Ungueltige Eingabe!")
                io.Expect("Es sind noch 10 Streichhoelzer uebrig. Spieler A ist am Zug: ")
                io.WriteLine("4")
                io.ExpectLine("Die Zahl 4 wurde eingegeben.")
                io.ExpectLine("Ungueltige Eingabe!")
                io.Expect("Es sind noch 10 Streichhoelzer uebrig. Spieler A ist am Zug: ")
                io.WriteLine("3")
                io.ExpectLine("Die Zahl 3 wurde eingegeben.")
        )

    member this.``b) Beispiel 2`` (): unit =
        executeIOTest (
            (fun () -> Nim.queryMove 2N 3N A |> ignore),
            fun io ->
                io.timeout <- ioTimeout
                io.Expect("Es sind noch 2 Streichhoelzer uebrig. Spieler A ist am Zug: ")
                io.WriteLine("3")
                io.ExpectLine("Die Zahl 3 wurde eingegeben.")
        )

    [<TestMethod>] [<Timeout(10000)>]
    member this.``b) Zufall`` (): unit =
        Check.QuickThrowOnFailure (fun (n: Nat) (k: Nat) (p: Player) ->
            let k = k + 1N
            let n = k + 1N
            executeIOTest (
               (fun () -> Nim.queryMove n k p |> ignore),
                fun io ->
                    io.timeout <- ioTimeout
                    io.Expect("Es sind noch "+(string n)+" Streichhoelzer uebrig. Spieler " + ( string p ) + " ist am Zug: ")
                    io.WriteLine(int (k + 1N) |> string)
                    io.ExpectLine ("Die Zahl " + (string (k + 1N)) + " wurde eingegeben.")
                    io.ExpectLine ("Ungueltige Eingabe!")
                    io.Expect("Es sind noch "+(string n)+" Streichhoelzer uebrig. Spieler " + ( string p ) + " ist am Zug: ")
                    io.WriteLine(string 0)
                    io.ExpectLine ("Die Zahl 0 wurde eingegeben.")
                    io.ExpectLine ("Ungueltige Eingabe!")
                    io.Expect("Es sind noch "+(string n)+" Streichhoelzer uebrig. Spieler " + ( string p ) + " ist am Zug: ")
                    io.WriteLine(int k |> string)
                    io.ExpectLine ("Die Zahl " + (string k) + " wurde eingegeben.")
            )
        )


    // ------------------------------------------------------------------------
    // c)

    [<TestMethod>] [<Timeout(10000)>]
    member this.``c) Beispiele`` (): unit =
        executeIOTest (
            (fun () -> Nim.nim 10N 4N B |> ignore),
            fun io ->
                io.timeout <- ioTimeout
                io.Expect("Es sind noch 10 Streichhoelzer uebrig. Spieler B ist am Zug: ")
                io.WriteLine("3")
                io.ExpectLine("Die Zahl 3 wurde eingegeben.")
                io.Expect("Es sind noch 7 Streichhoelzer uebrig. Spieler A ist am Zug: ")
                io.WriteLine("4")
                io.ExpectLine("Die Zahl 4 wurde eingegeben.")
                io.Expect("Es sind noch 3 Streichhoelzer uebrig. Spieler B ist am Zug: ")
                io.WriteLine("2")
                io.ExpectLine("Die Zahl 2 wurde eingegeben.")
                io.Expect("Es sind noch 1 Streichhoelzer uebrig. Spieler A ist am Zug: ")
                io.WriteLine("1")
                io.ExpectLine("Die Zahl 1 wurde eingegeben.")
                io.ExpectLine("Spieler B gewinnt das Spiel!")
        )


    // ------------------------------------------------------------------------
    // d)

    [<TestMethod>] [<Timeout(10000)>]
    member this.``d) Beispiele`` (): unit =
        executeIOTest (
            (fun () -> Nim.main ()),
            fun io ->
                io.timeout <- ioTimeout
                io.ExpectLine("Willkommen zu Nim")
                io.Expect("Wie viele Streichhoelzer sollen es sein? ")
                io.WriteLine("10")
                io.ExpectLine("Die Zahl 10 wurde eingegeben.")
                io.Expect("Es sind noch 10 Streichhoelzer uebrig. Spieler A ist am Zug: ")
                io.WriteLine("3")
                io.ExpectLine("Die Zahl 3 wurde eingegeben.")
                io.Expect("Es sind noch 7 Streichhoelzer uebrig. Spieler B ist am Zug: ")
                io.WriteLine("2")
                io.ExpectLine("Die Zahl 2 wurde eingegeben.")
                io.Expect("Es sind noch 5 Streichhoelzer uebrig. Spieler A ist am Zug: ")
                io.WriteLine("1")
                io.ExpectLine("Die Zahl 1 wurde eingegeben.")
                io.Expect("Es sind noch 4 Streichhoelzer uebrig. Spieler B ist am Zug: ")
                io.WriteLine("2")
                io.ExpectLine("Die Zahl 2 wurde eingegeben.")
                io.Expect("Es sind noch 2 Streichhoelzer uebrig. Spieler A ist am Zug: ")
                io.WriteLine("3")
                io.ExpectLine("Die Zahl 3 wurde eingegeben.")
                io.ExpectLine("Spieler B gewinnt das Spiel!")
        )
