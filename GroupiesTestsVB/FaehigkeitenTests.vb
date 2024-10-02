Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies

<TestClass>
Public Class FaehigkeitenTests

    <TestMethod>
    Public Sub TestKonstruktor()
        Dim f = New Entities.Faehigkeit("Bremsen", 3) With {.Beschreibung = "Kann mit Hilfe des Pflugs an flachen Hängen stoppen"}
        Assert.AreEqual("Bremsen", f.Benennung)
        Assert.AreEqual("Kann mit Hilfe des Pflugs an flachen Hängen stoppen", f.Beschreibung)
        Assert.AreEqual($"3. Bremsen{Environment.NewLine}Kann mit Hilfe des Pflugs an flachen Hängen stoppen.", f.AusgabeAnTrainerinfo)

        Dim f1 = New Entities.Faehigkeit("Bremsen") With {.Sortierung = 3, .Beschreibung = "Kann mit Hilfe des Pflugs an flachen Hängen stoppen"}
        Assert.AreEqual("Bremsen", f1.Benennung)
        Assert.AreEqual("Kann mit Hilfe des Pflugs an flachen Hängen stoppen", f1.Beschreibung)
        Assert.AreEqual($"3. Bremsen{Environment.NewLine}Kann mit Hilfe des Pflugs an flachen Hängen stoppen.", f1.AusgabeAnTrainerinfo)

        Dim f2 = New Entities.Faehigkeit("Kurven") With {.Sortierung = 1}
        Assert.AreEqual("1. Kurven", f2.AusgabeAnTrainerinfo)

        Dim f3 = New Entities.Faehigkeit("Einfache Kurven") With {.Beschreibung = "Kann einzelne Kurven mit Hilfe des Pflugbogens fahren"}
        Assert.AreEqual($"Einfache Kurven{Environment.NewLine}Kann einzelne Kurven mit Hilfe des Pflugbogens fahren.", f3.AusgabeAnTrainerinfo)
    End Sub

End Class
