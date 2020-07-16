module ShapesTypes
open Mini

// Beschreibung der Form eines Quadrats
type SquareData = {
    size: Nat   // Kantenlänge
}

// Beschreibung der Form eines Rechtecks
type RectangleData = {
    width: Nat  // Breite
    height: Nat // Höhe
}

// Verschieben einer Form
type Movement = {
    deltaX: Nat // nach rechts
    deltaY: Nat // nach oben
}

type Shape =
    | Square of SquareData
    | Rectangle of RectangleData
    | Or of Shape * Shape
    | Move of Movement * Shape
