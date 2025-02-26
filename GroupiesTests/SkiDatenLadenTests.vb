Imports Groupies
Imports Groupies.Entities
Imports Groupies.Controller
Imports System.Windows

<TestClass>
Public Class SkiDatenLadenTests
    <TestMethod>
    Public Sub BestimmeEinteilungsbenennungTest()

        Dim Einteilungen = New Entities.EinteilungCollection From {
            New Einteilung With {.Benennung = "Montag", .Sortierung = 2},
            New Einteilung With {.Benennung = "Dienstag", .Sortierung = 3},
            New Einteilung With {.Benennung = "Sonntag", .Sortierung = 1}}

        Dim Club = New Club With {.Einteilungsliste = Einteilungen}
        Club.Einteilungsliste.AddEinteilung(New Einteilung)
        Assert.AreEqual("Sonntag", Club.Einteilungsliste.OrderBy(Function(e) e.Sortierung)(0).Benennung)
        Assert.AreEqual("Montag", Club.Einteilungsliste.OrderBy(Function(e) e.Sortierung)(1).Benennung)
        Assert.AreEqual("Dienstag", Club.Einteilungsliste.OrderBy(Function(e) e.Sortierung)(2).Benennung)
        Assert.AreEqual("Tag4", Club.Einteilungsliste.OrderBy(Function(e) e.Sortierung)(3).Benennung)

    End Sub

    <TestMethod>
    Public Sub BestimmeEinteilungsbenennungTest2()

        Dim Einteilungen = New Entities.EinteilungCollection From {
            New Einteilung With {.Benennung = "Tag2", .Sortierung = 2},
            New Einteilung With {.Benennung = "Tag3", .Sortierung = 3},
            New Einteilung With {.Benennung = "Tag4", .Sortierung = 4}}

        Dim Club = New Club With {.Einteilungsliste = Einteilungen}
        Club.Einteilungsliste.AddEinteilung(New Einteilung)

        Assert.AreEqual("Tag5", Club.Einteilungsliste.OrderBy(Function(e) e.Sortierung)(3).Benennung)

    End Sub

    <TestMethod>
    Public Sub BestimmeEinteilungsbenennungTest3()

        Dim Einteilungen = New Entities.EinteilungCollection

        Dim Club = New Club With {.Einteilungsliste = Einteilungen}
        Club.Einteilungsliste.AddEinteilung(New Einteilung)

        Assert.AreEqual("Tag1", Club.Einteilungsliste.OrderBy(Function(e) e.Sortierung).ElementAt(0).Benennung)

    End Sub

    <TestMethod>
    Public Sub BestimmeEinteilungsbenennungTest4()

        Dim Club = New Club
        Club.Einteilungsliste.AddEinteilung(New Einteilung)

        Assert.AreEqual("Tag1", Club.Einteilungsliste.OrderBy(Function(e) e.Sortierung)(0).Benennung)

    End Sub

    <TestMethod>
    Public Sub SkiGruppenLesenTest()
        Debug.Print($"Version2 Gruppen lesen")
        Dim Gruppen = SkiDatenLaden.GruppenLesen()
        Assert.IsNotNull(Gruppen)
    End Sub

    <TestMethod>
    Public Sub SkiTeilnehmerLesenTest()
        Debug.Print($"Version2 Teilnehmer lesen")
        Dim Teilnehmer = SkiDatenLaden.TeilnehmerLesen()
        Assert.IsNotNull(Teilnehmer)
    End Sub


    <TestMethod>
    Public Sub SkiTrainerLesenTest()
        Debug.Print($"Version2 Trainer lesen")
        Dim Trainer = SkiDatenLaden.TrainerLesen()
        Assert.IsNotNull(Trainer)
    End Sub

    <TestMethod>
    Public Sub SkiEinteilungenLesenTest()
        Debug.Print($"Version2 Einteilungen lesen")
        Dim Einteilungen = SkiDatenLaden.EinteilungenLesen()
        Assert.IsNotNull(Einteilungen)
    End Sub

    <TestMethod>
    Public Sub ZweiEinteilungenLesenTest()

        Dim Pfad1, Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2.ski"
            Pfad1 = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2Tag2.ski"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2.ski"
            Pfad1 = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2Tag2.ski"
        End If

        Dim Club = SkiDatenLaden.SkiDateiLesen(Pfad)

        Dim Einteilungsliste1 As EinteilungCollection = SkiDatenLaden.EinteilungenLesen(Pfad1)

        Einteilungsliste1.ToList.ForEach(Sub(T) Club.Einteilungsliste.AddEinteilung(T))

        Assert.AreEqual(2, Club.Einteilungsliste.Count)
        Assert.AreEqual(1, Club.Einteilungsliste(0).Sortierung)
        Assert.AreEqual(2, Club.Einteilungsliste(1).Sortierung)
        Assert.AreEqual("Tag1", Club.Einteilungsliste(0).Benennung)
        Assert.AreEqual("Tag2", Club.Einteilungsliste(1).Benennung)
    End Sub

    <TestMethod>
    Public Sub LadenDateiVersion1_In_neueStruktur()
        Dim Pfad

        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion1.ski"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2.ski"
        End If

        Dim Club = SkiDatenLaden.SkiDateiLesen(Pfad)

        Assert.AreEqual(1, Club.Einteilungsliste.Count)
        Assert.AreEqual(1, Club.Einteilungsliste(0).Sortierung)
        Assert.AreEqual("Tag1", Club.Einteilungsliste(0).Benennung)
    End Sub

    <TestMethod>
    Public Sub LadenDateiVersion1_In_neueStruktur_ZusaetzlicheEinteilung()
        Dim Pfad, Pfad1

        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion1.ski"
            Pfad1 = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2Tag2.ski"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2.ski"
            Pfad1 = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2Tag2.ski"
        End If

        Dim Club = SkiDatenLaden.SkiDateiLesen(Pfad)

        Dim Einteilungsliste1 As EinteilungCollection = SkiDatenLaden.EinteilungenLesen(Pfad1)
        Einteilungsliste1.ToList.ForEach(Sub(T) Club.Einteilungsliste.AddEinteilung(T))

        Assert.AreEqual(2, Club.Einteilungsliste.Count)
        Assert.AreEqual(1, Club.Einteilungsliste(0).Sortierung)
        Assert.AreEqual(2, Club.Einteilungsliste(1).Sortierung)
        Assert.AreEqual("Tag1", Club.Einteilungsliste(0).Benennung)
        Assert.AreEqual("Tag2", Club.Einteilungsliste(1).Benennung)
    End Sub


    <TestMethod>
    Public Sub OpenSkidateiTest()
        Dim Pfad, Pfad1

        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion1.ski"
            Pfad1 = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2.ski"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2.ski"
            Pfad1 = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2.ski"
        End If

        Dim c1 = SkiDatenLaden.OpenSkiDatei(Pfad)
        Assert.IsNotNull(c1)
        Dim c2 = SkiDatenLaden.OpenSkiDatei(Pfad1)
        Assert.IsNotNull(c2)
    End Sub
End Class
