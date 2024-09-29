Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies
Imports Groupies.Entities

<TestClass>
Public Class GruppeTests
    <TestMethod>
    Public Sub TestKonstruktor()
        Dim g1 = New Gruppe("Enzian", "Anfänger", 1)
        Dim g2 = New Gruppe("K2", 2)
        Dim g3 = New Gruppe("Wildspitze", "Experten")
        Assert.AreEqual("Enzian", g1.Ausgabename)
        Assert.AreEqual("K2", g2.Ausgabename)
        Assert.AreEqual("Wildspitze", g3.Ausgabename)
        Assert.AreEqual("Experten", g3.Benennung)
        Assert.AreEqual(2, g2.Sortierung)
    End Sub

    <TestMethod>
    Public Sub TestMitgliedHinzufuegen()
        Dim g1 = New Gruppe("Enzian", "Anfänger", 1)
        Dim Koenner = New Leistungsstufe()
        Dim Manu = New Teilnehmer("Manuela", "Ramm", Koenner)
        Dim Willi = New Teilnehmer("Willi", "Sensmeier") With {.Leistungsstand = Koenner}
        g1.TeilnehmerHinzufuegen(Manu)
        g1.TeilnehmerHinzufuegen(Willi)
        Assert.AreEqual(2, g1.Mitglieder.Count)
        g1.TeilnehmerEntfernen(Willi)
        Assert.AreEqual(1, g1.Mitglieder.Count)
    End Sub
End Class
