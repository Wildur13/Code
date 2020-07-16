module Bool

(*

Da dies wahrscheinlich Ihre erste Programmieraufgabe ist, sind hier
einige Hinweise vorweg. Was Sie hier lesen ist ein Kommentar.
Kommentare in F# können mit einer öffnenden Klammer gefolgt von einem
Stern gestartet und mit einem Stern gefolgt von einer schließenden
Klammer wieder beendet werden. Alternativ ist es möglich, mit zwei
aufeinanderfolgenden Slash Zeichen // einen Kommentar einzuleiten,
der bis zum Ende der jeweiligen Zeile geht.
Kommentare haben keinerlei Einfluss auf das Programm. Sie sollen
anderen Menschen oder Ihnen selbst zu einem späteren Zeitpunkt dabei
helfen, den Programmcode zu verstehen.

Die "module" Zeile ganz oben erstellt ein sogenanntes Modul. Darin
werden Programmteile gebündelt. Wichtig zu wissen für Sie ist jedoch
nur, dass Sie das Modul im F# Interpreter öffnen müssen, wenn Sie
etwas aus dieser Datei verwenden wollen: open Bool;;

Da wir die Ausdrücke nicht einfach wahllos in die Datei schreiben
können, müssen wir Sie an Bezeichner binden. Tatsächlich definieren
wir hier sogar Funktionen, da die Ausdrücke von a und b abhängig
sind. Mehr dazu lernen Sie später, das braucht Sie im Moment nicht
zu interessieren.

*)

// Hier ist das Beispiel vom Übungsblatt:

let greater a b = if b then false else a

// Wir interessieren uns hier nur für den Ausdruck nach dem = Zeichen.

// Nun müssen Sie selbst derartige Ausdrücke schreiben.

// Damit diese Datei keine Compile-Fehler enthält, fügen wir immer
// da wo Sie selbst etwas ergänzen müssen folgenden Platzhalter ein:
//     failwith "TODO"


let smallerOrEqual a b = if a then b else true

let multiplication a b = if a then b else false

let equivalence a b = if a then b else if b then false else true

// Wenn Sie Ihr Programm nun testen wollen, dann können Sie entweder
// dotnet test verwenden oder Sie laden es im F# Interpreter. Führen
// Sie dazu folgendes aus: dotnet fsi Bool.fs
// Im Interpreter geben Sie nun ein: open Bool;;
// Anschließend können Sie die Definitionen oben benutzen.
// Geben Sie ein: greater true false;;
// und bestätigen Sie mit Enter. Der Ausdruck sollte zu true auswerten.
// Ihre eigenen Teile können Sie genauso ausprobieren.
// Bei Problemen oder wenn Sie Fragen besuchen Sie bitte eine der
// Sprechstunden zur Übung.
