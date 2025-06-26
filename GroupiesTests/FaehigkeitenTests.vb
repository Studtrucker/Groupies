Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Entities

<TestClass>
Public Class FaehigkeitenTests



    <TestMethod>
    Public Sub TestKonstruktor1()

        Dim f = New Faehigkeit() With {.Benennung = "Kurven", .Beschreibung = "Kurven fahren", .Sortierung = 1}
        Assert.AreEqual($"1. Kurven{Environment.NewLine}Kurven fahren", f.AusgabeAnTrainerInfo)

    End Sub

    <TestMethod>
    Public Sub TestKonstruktor2()

        Dim f1 = New Faehigkeit("Pflug") With {.Beschreibung = "Pflug fahren"}
        Assert.AreEqual("Pflug", f1.Benennung)
        Assert.AreEqual("Pflug fahren", f1.Beschreibung)
        Assert.AreEqual(0, f1.Sortierung)
        Assert.AreEqual($"{f1.Sortierung}. Pflug{Environment.NewLine}Pflug fahren", f1.AusgabeAnTrainerInfo)

    End Sub

    <TestMethod>
    Public Sub TestKonstruktor3()
        Dim f = New Faehigkeit("Bremsen", 3) With {.Beschreibung = "Kann stoppen"}
        Assert.AreEqual("Bremsen", f.Benennung)
        Assert.AreEqual("Kann stoppen", f.Beschreibung)
        Assert.AreEqual($"3. Bremsen{Environment.NewLine}Kann stoppen", f.AusgabeAnTrainerInfo)
    End Sub

    <TestMethod>
    Public Sub TestSortierungSammlung()

        Dim f = New Faehigkeit("Bremsen", 3) With {
            .Beschreibung = "Kann mit Hilfe des Pflugs an flachen Hängen stoppen"}
        Dim f1 = New Faehigkeit("Pflug") With {
            .Sortierung = 2,
            .Beschreibung = "Kann mit Hilfe des Pflugs an flachen Hängen Kurven fahren"}


        Dim f2 = New Faehigkeit("Kurven") With {
            .Sortierung = 1}
        Dim f3 = New Faehigkeit("Einfache Kurven") With {
            .Beschreibung = "Kann einzelne Kurven mit Hilfe des Pflugbogens fahren"}

        Dim fcol = New FaehigkeitCollection From {f, f1, f2, f3}
        CollectionAssert.AreEqual(New List(Of String) From {
                                  $"0. Einfache Kurven{Environment.NewLine}Kann einzelne Kurven mit Hilfe des Pflugbogens fahren",
                                  $"1. Kurven",
                                  $"2. Pflug{Environment.NewLine}Kann mit Hilfe des Pflugs an flachen Hängen Kurven fahren",
                                  $"3. Bremsen{Environment.NewLine}Kann mit Hilfe des Pflugs an flachen Hängen stoppen"},
                                  fcol.Sortieren.ToList)

    End Sub

    <TestMethod>
    Public Sub TestSortierung()

        Richtungen.Add(-1, "Nord")
        Richtungen.Add(-2, "Ost")
        Richtungen.Add(-3, "NordOst")
        Richtungen.Add(-4, "Nord")
        Richtungen.Add(-5, "Hoch")
        Richtungen.Add(-6, "Rein")
        Richtungen.Add(1, "Süd")
        Richtungen.Add(2, "West")
        Richtungen.Add(3, "SüdWest")
        Richtungen.Add(4, "Süd")
        Richtungen.Add(5, "Runter")
        Richtungen.Add(6, "Raus")

        Debug.Print("RichtungslisteOhne")
        For Each item In RichtungslisteOhne
            Debug.Print(item)
        Next
        Debug.Print(Environment.NewLine)
        Debug.Print("RichtungslisteWert")
        For Each item In RichtungslisteWert
            Debug.Print(item)
        Next
        Debug.Print(Environment.NewLine)
        Debug.Print("RichtungslisteOriginal")
        For Each item In Richtungsliste
            Debug.Print(item)
        Next
        Debug.Print(Environment.NewLine)
        Debug.Print("RichtungslisteAbsWertWert")
        For Each item In RichtungslisteAbsWertWert
            Debug.Print(item)
        Next
    End Sub

    Private ReadOnly Richtungen = New Dictionary(Of Integer, String)()

    Property RichtungslisteWert As IEnumerable(Of String) = Richtungen _
        .OrderBy(Function(SchlüsselWertPaar) SchlüsselWertPaar.Key) _
        .Select(Function(SchlüsselWertPaar) $"{SchlüsselWertPaar.Value} ist {SchlüsselWertPaar.Key}")
    Property RichtungslisteAbsWertWert As IEnumerable(Of String) = Richtungen _
        .OrderBy(Function(SchlüsselWertPaar) Math.Abs(SchlüsselWertPaar.Key)) _
        .OrderBy(Function(SchlüsselWertPaar) SchlüsselWertPaar.Key) _
        .Select(Function(SchlüsselWertPaar) $"{SchlüsselWertPaar.Value} ist {SchlüsselWertPaar.Key}")

    Property RichtungslisteOhne As IEnumerable(Of String) = Richtungen _
        .Select(Function(SchlüsselWertPaar) $"{SchlüsselWertPaar.Value} ist {SchlüsselWertPaar.Key}")
    Property Richtungsliste As IEnumerable(Of String) = Richtungen _
        .OrderBy(Function(SchlüsselWertPaar) SchlüsselWertPaar.Key) _
        .OrderBy(Function(SchlüsselWertPaar) Math.Abs(SchlüsselWertPaar.Key)) _
        .Select(Function(SchlüsselWertPaar) $"{SchlüsselWertPaar.Value} ist {SchlüsselWertPaar.Key}")

End Class
