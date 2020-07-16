module Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open TrianglesType


[<TestClass>]
type Tests() =

    [<TestMethod>]
    member this.``a) height: Beispiel1`` (): unit =
        Assert.AreEqual(3, Triangles.height {a = 2; b = -2; c = 3})

    [<TestMethod>]
    member this.``a) height: Beispiel2`` (): unit =
        Assert.AreEqual(10, Triangles.height {a = 2; b = 5;  c = 3})

    [<TestMethod>]
    member this.``a) height: Beispiel3`` (): unit =
        Assert.AreEqual(-3, Triangles.height {a = 2; b = -8; c = 3})


    [<TestMethod>]
    member this.``b) isPoint: Beispiel1`` (): unit =
        Assert.AreEqual(false, Triangles.isPoint {a = 2;  b = 5; c = 3})

    [<TestMethod>]
    member this.``b) isPoint: Beispiel2`` (): unit =
        Assert.AreEqual(true, Triangles.isPoint {a = -8; b = 5; c = 3})


    [<TestMethod>]
    member this.``c) mirrorA: Beispiel`` (): unit =
        Assert.AreEqual({a = 2; b = -5; c = -7}, Triangles.mirrorA {a = 2; b = 5; c = 3})

    [<TestMethod>]
    member this.``c) mirrorA: Identität wenn isPoint`` (): unit =
        Check.QuickThrowOnFailure(fun (t: Triangle) ->
            if Triangles.isPoint t then
                let result = Triangles.mirrorA t
                Assert.AreEqual(t, result, sprintf "Das Dreieck %A ist zu einem Punkt degradiert, daher sollte es sich durch Spiegeln nicht verändern" t)
        )

    [<TestMethod>]
    member this.``c) mirrorA: Höhe negiert`` (): unit =
        Check.QuickThrowOnFailure(fun (t: Triangle) ->
            let result = Triangles.mirrorA t
            let h1 = Triangles.height t
            let h2 = Triangles.height result
            Assert.AreEqual(-h1, h2, sprintf "Das Dreieck %A hat die Höhe %i. Das gespiegelte Dreieck %A hat die Höhe %i. Die Höhe müsste eigentlich negiert sein." t h1 result h2)
        )
