module Simplify

// Beachten Sie bitte auch die Hinweise, die wir als Kommentar in der
// Datei Bool.fs aus der Vorlage für Aufgabe 2 angegeben haben.

// Auch hier verpacken wir die Ausdrücke wieder in Funktionsdefinitionen
// und verwenden failwith "TODO" als Platzhalter für Ihren Ausdruck.


// Das angegebene Beispiel:
let expression0 a b = if a then b else false
let simplified0 a b = a && b

// a)
let expressionA a b = if a > b then false else true
let simplifiedA a b = a<=b

// b)
let expressionB a = if a then true else false
let simplifiedB a = a

// c)
let expressionC a b c = if a then b + 555 else c + 555
let simplifiedC a b c =( if a then b else c)+555

// d)
let expressionD a b c = if a then (if b then true else c) else false
let simplifiedD a b c = a && (b||c)

// e)
let expressionE a b = a <> b || ((b <> a) = true <> b && (a = b) <> (a = false) || a && b) || not (a || not b)
let simplifiedE a b = a||b
