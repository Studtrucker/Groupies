Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies
Imports Groupies.Entities

<TestClass>
Public Class GruppeTests

    <TestMethod>
    Public Sub TestKonstruktor()
        Dim g1 = New Gruppe("Enzian", 1)
        Dim g2 = New Gruppe("K2", 2)
        Dim g3 = New Gruppe("Wildspitze", "Experten")
        Assert.AreEqual("Enzian", g1.Benennung)
        Assert.AreEqual("K2", g2.Benennung)
        Assert.AreEqual("Wildspitze", g3.Benennung)
        Assert.AreEqual("Experten", g3.Benennung)
        Assert.AreEqual(2, g2.Sortierung)
    End Sub

    <TestMethod>
    Public Sub TestMitgliedHinzufuegen()
        Dim g1 = New Gruppe("Enzian", 1)
        Dim Koenner = New Leistungsstufe()
        Dim Manu = New Teilnehmer("Manuela", "Ramm", Koenner)
        Dim Willi = New Teilnehmer("Willi", "Sensmeier") With {.Leistungsstand = Koenner}
        g1.Mitgliederliste.Add(Manu)
        g1.Mitgliederliste.Add(Willi)
        Assert.AreEqual(2, g1.Mitgliederliste.Count)
        g1.Mitgliederliste.Remove(Willi)
        Assert.AreEqual(1, g1.Mitgliederliste.Count)
    End Sub
End Class
