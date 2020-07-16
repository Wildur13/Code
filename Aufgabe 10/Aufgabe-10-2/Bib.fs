module Bib

open Mini
open BibTypes

// a)
let sucheTitel (bib: Buch list) (titel: String): Buch list =
    failwith "TODO"

// b)
let rec leiheBuch (bib: Buch list) (titel: String) (person: String): AusleihenErgebnis option =
    failwith "TODO"

// c)
let rec rueckgabe (bib: Buch list) (titel: String) (person: String): bool =
    failwith "TODO"