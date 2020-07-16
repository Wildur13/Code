module BibTypes
open Mini

type Ausleihstatus =
    | Dauerleihe of String // Name der Arbeitsgruppe, an die das Buch ausgeliehen ist
    | NormaleLeihe of String // Name der Person, an die das Buch ausgeliehen ist
    | Verfuegbar

type Buch =
    { titel: String                     // Eindeutiger Titel
      exemplare: Ausleihstatus ref list // Exemplarliste hat feste Länge
      warteliste: String list ref }     // Liste von Personen

type AusleihenErgebnis =
    | ErfolgreichAusgeliehen
    | Warteliste      // Person auf die Warteliste gesetzt
    | NichtVerfuegbar // Alle Exemplare in Dauerleihe, daher keine Warteliste