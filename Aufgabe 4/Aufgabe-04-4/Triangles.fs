module Triangles
open Mini
open TrianglesType

// a)
let height (t: Triangle): int =
   t.a+t.b+t.c

// b)
let isPoint (t: Triangle): bool =
    (t.a+t.b+t.c)=0

// c)
let mirrorA (t: Triangle): Triangle =
    {a=t.a; b = -(t.a+t.c); c = -(t.a+t.b)}
