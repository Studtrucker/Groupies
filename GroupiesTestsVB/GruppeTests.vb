Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies

<TestClass>
Public Class GruppeTests
    <TestMethod>
    Public Sub TestKonstruktor()
        Dim g1 = New Entities.Gruppe("Enzian", "Anfänger", 1)
        Dim g2 = New Entities.Gruppe("K2", 2)
        Dim g3 = New Entities.Gruppe("Wildspitze", "Experten")
        Assert.AreEqual("Enzian", g1.Ausgabename)
        Assert.AreEqual("K2", g2.Ausgabename)
        Assert.AreEqual("Wildspitze", g3.Ausgabename)
        Assert.AreEqual("Experten", g3.Benennung)
        Assert.AreEqual(2, g2.Sortierung)
    End Sub
End Class
