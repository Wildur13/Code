module Santa
open Mini
open SantaTypes

// Beispiele vom Übungsblatt (zur Verwendung im Interpreter)
let lisa  = {name="Lisa" ; location = {x = 1N; y = 2N}}
let alice = {name="Alice"; location = {x = 2N; y = 5N}}
let harry = {name="Harry"; location = {x = 5N; y = 3N}}
let bob   = {name="Bob"  ; location = {x = 4N; y = 4N}}
let santasList = [lisa; alice; harry; bob]

// a)
let distance (a: Person) (b: Person): Nat =
    failwith "TODO"

// b)
let rec pathlength (p: Person list): Nat =
    failwith "TODO"

// c)
let rec prepend (elem: 'a) (xss: 'a list list): 'a list list =
    failwith "TODO"

// d)
let rec insert (elem: 'a) (xs: 'a list): 'a list list =
    failwith "TODO"

// e)
let rec permute (ls : 'a list): 'a list list =
    failwith "TODO"

// f)
let shortestPath (p: Person list): (Person list * Nat) =
    failwith "TODO"
