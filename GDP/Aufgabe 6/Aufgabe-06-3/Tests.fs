module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open Mini
open Types

type ArbitraryModifiers =
    static member Nat() =
        Arb.from<bigint>
        |> Arb.filter (fun i -> i >= 0I)
        |> Arb.convert (Nat.Make) (fun n -> n.ToBigInteger())

let isValid (xs: Input list): bool =
    let rec h (xs: Input list) (s: Nat): bool =
        match xs with
        | [] -> s = 1N
        | INum _::xs' -> h xs' (s + 1N)
        | _::xs' -> s >= 2N && h xs' (s - 1N)
    h xs 0N

let rec postorder (expr: Expr): Input list =
    match expr with
    | Num x -> [INum x]
    | Add (left, right) -> postorder left @ postorder right @ [Plus]
    | Mul (left, right) -> postorder left @ postorder right @ [Star]

[<TestClass>]
type Tests() =
    do Arb.register<ArbitraryModifiers>() |> ignore

    [<TestMethod>]
    member this.``Gültige Eingaben`` (): unit =
        Check.QuickThrowOnFailure(fun (expr: Expr) ->
            let xs = postorder expr
            match Postfix.parse xs with
            | None -> Assert.Fail(sprintf "Die Eingabe %A wurde nicht richtig eingelesen!\nExpected: %A\nActual: None" xs (Some expr))
            | r -> Assert.AreEqual(Some expr, r, sprintf "Die Eingabe %A wurde nicht richtig eingelesen!" xs)
        )

    [<TestMethod>]
    member this.``Ungültige Eingaben`` (): unit =
        Check.QuickThrowOnFailure(fun (xs: Input list) ->
            if not (isValid xs) then
                match Postfix.parse xs with
                | None -> ()
                | _ -> Assert.Fail(sprintf "Die Eingabe %A wurde nicht als ungültig erkannt!" xs)
        )
