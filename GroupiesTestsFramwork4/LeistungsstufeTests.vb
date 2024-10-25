Imports Groupies
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass>
Public Class LeistungsstufeTests
    <TestMethod>
    Public Sub TestKonstruktor()

        Dim l1 = New Entities.Leistungsstufe("Anfänger") With {.Sortierung = 1}
        Assert.AreEqual("Anfänger", l1.ToString)
        Assert.AreEqual(1, l1.Sortierung)
        Assert.AreEqual(0, l1.Faehigkeiten.Count)

        Dim l2 = New Entities.Leistungsstufe("Anfänger", "4") With {.Sortierung = "003"}
        Assert.AreEqual("Anfänger", l2.ToString)
        Assert.AreEqual(3, l2.Sortierung)

        Dim l3 = New Entities.Leistungsstufe("Experte", "005") With {.Beschreibung = "Fährt sicher auf Schwarzen Pisten und im Gelände"}
        Assert.AreEqual("Experte", l3.ToString)
        Assert.AreEqual(5, l3.Sortierung)
        Assert.AreEqual("Fährt sicher auf Schwarzen Pisten und im Gelände", l3.Beschreibung)

    End Sub
End Class
