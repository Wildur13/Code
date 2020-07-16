module Vectors
open Mini
open VectorsType

// a)
let scale (v: Vec) (n: Nat): Vec =
   {x = n*v.x; y = n*v.y; z = n*v.z}

// b)
let add (v1: Vec) (v2: Vec): Vec =
    {x = v1.x+v2.x; y = v1.y+v2.y; z = v1.z+v2.z}

// c)
let dotProduct (v1: Vec) (v2: Vec): Nat =
     v1.x*v2.x+v1.y*v2.y+v1.z*v2.z