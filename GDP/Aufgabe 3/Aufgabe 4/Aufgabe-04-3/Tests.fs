module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open Mini

let testConfig = {Config.QuickThrowOnFailure with MaxTest = 300; StartSize = 0; EndSize = 12; QuietOnSuccess = true}
let seeds = [(1591000511,296671646); (1893282681,296671730); (1551215629,296671507); (180184836,296671692)]

// Darstellung des Höhenprofils als 2D Matrix
[<StructuredFormatDisplay("{ToString}")>]
type Heights =
    | Heights of matrix: Nat[,]

    // Matrix in Funktion transformieren
    member self.ToFunction(): Nat * Nat -> Nat =
        let (Heights matrix) = self
        let len1 = Array2D.length1 matrix |> Nat.Make
        let len2 = Array2D.length2 matrix |> Nat.Make
        fun (x, y) ->
            if len1 > 0N && len2 > 0N then
                if x < len1 then
                    if y < len2 then
                        matrix.[int x, int y]
                    else
                        y + matrix.[int x, int len2 - 1] + 1N - len2
                else x + y + matrix.[int len1 - 1, int len2 - 1] + 2N - len1 - len2
            else x + y

    // F# Code für die Funktion generieren
    member self.ToString =
        let (Heights matrix) = self
        let sb = System.Text.StringBuilder()
        let out(s: string): unit = sb.Append(s) |> ignore
        out("fun (x, y) ->")
        let len1 = Array2D.length1 matrix |> Nat.Make
        let len2 = Array2D.length2 matrix |> Nat.Make
        if len1 > 0N && len2 > 0N then
            out("\n")
            for x in 0..int len1 - 1 do
                out("    ")
                if x > 0 then out("else ")
                out(sprintf "if x = %iN then\n" x)
                for y in 0..int len2 - 1 do
                    out("        ")
                    if y > 0 then out("else ")
                    out(sprintf "if y = %iN then %A\n" y matrix.[x, y])
                out(sprintf "        else y + %A\n" (matrix.[x, int len2 - 1] + 1N - len2))
            out(sprintf "    else x + y + %A" (matrix.[int len1 - 1, int len2 - 1] + 2N - len1 - len2))
        else out(" x + y")
        sb.ToString()


type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())

    static member Heights() =
        Arb.fromGenShrink (
            // Generator für eine gültige 2D Höhenprofilmatrix
            Gen.sized <| fun size ->
                gen {
                    let size = max 0 size
                    let matrix: Nat[,] = Array2D.create size size 0N
                    for x in 0 .. size - 1 do
                        for y in 0 .. size - 1 do
                            let mutable bound = 0N
                            if x > 0 then bound <- max bound (matrix.[x-1,y] + 1N)
                            if y > 0 then bound <- max bound (matrix.[x,y-1] + 1N)
                            let! h = Arb.from<Nat> |> Arb.toGen |> Gen.filter (fun x -> x >= bound)
                            matrix.[x,y] <- h
                    return Heights matrix
                }
            ,

            // Gegebene Höhenprofilmatrix verkleinern
            fun (Heights matrix) -> seq {
                let len1 = Array2D.length1 matrix
                let len2 = Array2D.length2 matrix
                if len1 > 1 then // letzte Spalte entfernen
                    yield Heights (Array2D.init (len1 - 1) len2 (fun x y -> matrix.[x, y]))
                if len2 > 1 then // letzte Zeile entfernen
                    yield Heights (Array2D.init len1 (len2 - 1) (fun x y -> matrix.[x, y]))
                if len1 = 1 && len2 = 1 then // ganz leere Matrix
                    yield Heights (Array2D.create 0 0 0N)
                if len1 > 0 && len2 > 0 && matrix.[0, 0] > 0N then // Zahlen eins kleiner
                    yield Heights (matrix |> Array2D.map (fun h -> h - 1N))
            }
        )


let limitCalls (n: Nat) (f: 'a -> 'b) (error: string): 'a -> 'b =
    let mutable counter = n
    fun x ->
        if counter = 0N then failwith error
        counter <- counter - 1N
        f x

let check (limit: Nat -> Nat * string): unit =
    for seed in seeds do
        Check.One ({testConfig with Replay = Some <| Random.StdGen seed}, fun (heights: Heights) (x: Nat) (y: Nat) ->
            let heights = heights.ToFunction()
            let n = heights (x, y)
            let (limit, limitError) = limit n
            let error: string = sprintf "\nGesucht ist die Höhe %A, zu finden zum Beispiel an Position %A.\n" n (x, y)
            let result =
                Cartography.findHeight
                    (limitCalls limit heights (error + limitError))
                    n
            let resultheight = heights result
            Assert.AreEqual (
                n,
                resultheight,
                error + sprintf "Zurückgegeben wird aber Position %A mit Höhe %A." result resultheight
            )
        )

[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    // [<TestMethod>]
    // member this.``Testgenerator ist korrekt`` (): unit =
    //     for seed in seeds do
    //         Check.One ({testConfig with Replay = Some <| Random.StdGen seed}, fun (heights: Heights) (x: Nat) (y: Nat) (z: Nat) ->
    //             let heights = heights.ToFunction()
    //             Assert.IsTrue(
    //                 x >= z || heights (x, y) < heights (z, y),
    //                 sprintf "Violated: %A < %A => heights (%A, %A) < heights (%A, %A)" x z x y z y
    //             )
    //             Assert.IsTrue(
    //                 y >= z || heights (x, y) < heights (x, z),
    //                 sprintf "Violated: %A < %A => heights (%A, %A) < heights (%A, %A)" y z x y x z
    //             )
    //         )

    [<TestMethod>]
    member this.``Pflicht: maximal n*n*n+1000 Aufrufe`` (): unit =
        check (
            fun n ->
                let limit = n*n*n+1000N
                let limitError = sprintf "Ihre Funktion hat nach %s Aufrufen von heights aber immer noch keinen Wert zurück gegeben. Möglicherweise liegt eine Endlos-Rekursion vor." <| string limit
                (limit, limitError)
        )

    [<TestMethod>]
    member this.``Optional1: maximal (n+1)^2 Aufrufe`` (): unit =
        check (
            fun n ->
                let limit = (n+1N)*(n+1N)
                let limitError = sprintf "Ihre Funktion hat nach %s Aufrufen von heights immer noch keinen Wert zurück gegeben." <| string limit
                (limit, limitError)
        )

    [<TestMethod>]
    member this.``Optional2: maximal 2*n+1 Aufrufe`` (): unit =
        check (
            fun n ->
                let limit = 2N*n+1N
                let limitError = sprintf "Ihre Funktion hat nach %s Aufrufen von heights immer noch keinen Wert zurück gegeben." <| string limit
                (limit, limitError)
        )

    [<TestMethod>]
    member this.``Optional3: maximal 2*n Aufrufe`` (): unit =
        check (
            fun n ->
                let limit = 2N*n
                let limitError = sprintf "Ihre Funktion hat nach %s Aufrufen von heights immer noch keinen Wert zurück gegeben." <| string limit
                (limit, limitError)
        )
