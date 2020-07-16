module Peano
open Mini

// Wie schon beim letzten Übungsblatt verwenden wir hier den Ausdruck
// failwith "TODO" als Platzhalter für Ihren Code. Dieser Ausdruck
// bewirkt, dass jeder Aufruf der Funktion mit der Fehlermeldung TODO
// fehlschlägt. So können wir Ihnen eine Vorlage bereitstellen, die
// vom Compiler akzeptiert wird. Solange der Platzhalter nicht,
// ersetzt wurde schlagen die Testfälle liefern die Testfälle für die
// jeweilige Funktion daher die Fehlermeldung TODO.

// a)
let rec mult3 (x: Nat): Nat =
    if x = 0N then 0N
        else 3N + mult3(x - 1N)

// b)
let rec exp3 (x: Nat): Nat =
        if x = 0N then 1N
        else 3N * exp3(x - 1N)

// c)
let sqr (x:Nat): Nat = x*x*x
let rec root3 (x: Nat): Nat =
    if x = 0N then 0N
        else let r= root3(x - 1N)
             if  x<sqr(r+1N) then r else r+1N

// d)
let rec search (s: string) (f: Nat -> string) (limit: Nat): bool =
    if limit = 0N then false 
        else f (limit - 1N) = s || search s f (limit - 1N)
