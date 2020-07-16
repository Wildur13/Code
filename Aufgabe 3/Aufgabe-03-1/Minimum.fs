module Minimum
open Mini

// Wie schon beim letzten Übungsblatt verwenden wir hier den Ausdruck
// failwith "TODO" als Platzhalter für Ihren Code. Dieser Ausdruck
// bewirkt, dass jeder Aufruf der Funktion mit der Fehlermeldung TODO
// fehlschlägt. So können wir Ihnen eine Vorlage bereitstellen, die
// vom Compiler akzeptiert wird. Solange der Platzhalter nicht,
// ersetzt wurde schlagen die Testfälle liefern die Testfälle für die
// jeweilige Funktion daher die Fehlermeldung TODO.

// a)
let min (x: Nat) (y: Nat): Nat = if x<y then x else y

// b)
let min3 (x: Nat) (y: Nat) (z: Nat): Nat = if x<y then if x<z then x else z
                                                elif y<z then y else z
