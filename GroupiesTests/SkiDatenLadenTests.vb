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

        Dim Ben = SkiDatenLaden.BestimmeEinteilungsbenennung(Einteilungen)
        Assert.AreEqual(Ben, "Tag4")

    End Sub

    <TestMethod>
    Public Sub BestimmeEinteilungsbenennungTest2()

        Dim Einteilungen = New Entities.EinteilungCollection From {
            New Einteilung With {.Benennung = "Tag2", .Sortierung = 2},
            New Einteilung With {.Benennung = "Tag3", .Sortierung = 3},
            New Einteilung With {.Benennung = "Tag4", .Sortierung = 4}}

        Dim Ben = SkiDatenLaden.BestimmeEinteilungsbenennung(Einteilungen)
        Assert.AreEqual(Ben, "Tag5")

    End Sub

    <TestMethod>
    Public Sub BestimmeEinteilungsbenennungTest3()

        Dim Einteilungen = New Entities.EinteilungCollection

        Dim Ben = SkiDatenLaden.BestimmeEinteilungsbenennung(Einteilungen)
        Assert.AreEqual(Ben, "Tag1")

    End Sub

    <TestMethod>
    Public Sub BestimmeEinteilungsbenennungTest4()

        Dim Einteilungen = Nothing

        Dim Ben = SkiDatenLaden.BestimmeEinteilungsbenennung(Einteilungen)
        Assert.AreEqual(Ben, "Tag1")

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
        Dim Einteilungsliste1 = (SkiDatenLaden.EinteilungenLesen(Pfad1))
        Dim Einteilungsliste = (SkiDatenLaden.EinteilungenLesen(Pfad))

        Dim Club = SkiDatenLaden.SkiDateiLesen(Pfad)

        Einteilungsliste1.ToList.ForEach(Sub(T) Club.Einteilungsliste.Add(T))

        Assert.AreEqual(2, Club.Einteilungsliste.Count)
    End Sub

End Class
