Imports Groupies
Imports Groupies.Entities
Imports Groupies.Controller
Imports System.Windows

<TestClass>
Public Class DatenladenTests
    <TestMethod>
    Public Sub BestimmeEinteilungsbenennungTest()

        Dim Einteilungen = New Entities.EinteilungCollection From {
            New Einteilung With {.Benennung = "Montag", .Sortierung = 2},
            New Einteilung With {.Benennung = "Dienstag", .Sortierung = 3},
            New Einteilung With {.Benennung = "Sonntag", .Sortierung = 1}}

        Dim Ben = Controller.DatenLaden.BestimmeEinteilungsbenennung(Einteilungen)
        Assert.AreEqual(Ben, "Tag4")

    End Sub

    <TestMethod>
    Public Sub BestimmeEinteilungsbenennungTest2()

        Dim Einteilungen = New Entities.EinteilungCollection From {
            New Einteilung With {.Benennung = "Tag2", .Sortierung = 2},
            New Einteilung With {.Benennung = "Tag3", .Sortierung = 3},
            New Einteilung With {.Benennung = "Tag4", .Sortierung = 4}}

        Dim Ben = Controller.DatenLaden.BestimmeEinteilungsbenennung(Einteilungen)
        Assert.AreEqual(Ben, "Tag5")

    End Sub

    <TestMethod>
    Public Sub BestimmeEinteilungsbenennungTest3()

        Dim Einteilungen = New Entities.EinteilungCollection

        Dim Ben = Controller.DatenLaden.BestimmeEinteilungsbenennung(Einteilungen)
        Assert.AreEqual(Ben, "Tag1")

    End Sub

    <TestMethod>
    Public Sub BestimmeEinteilungsbenennungTest4()

        Dim Einteilungen = Nothing

        Dim Ben = Controller.DatenLaden.BestimmeEinteilungsbenennung(Einteilungen)
        Assert.AreEqual(Ben, "Tag1")

    End Sub

    <TestMethod>
    Public Sub XmlGruppenLesenTest()
        Debug.Print($"Version2 Gruppen lesen")
        Dim Gruppen = Controller.DatenLaden.GruppenLesen()
        Assert.IsNotNull(Gruppen)
    End Sub

    <TestMethod>
    Public Sub XmlTeilnehmerLesenTest()
        Debug.Print($"Version2 Teilnehmer lesen")
        Dim Teilnehmer = Controller.DatenLaden.TeilnehmerLesen()
        Assert.IsNotNull(Teilnehmer)
    End Sub


    <TestMethod>
    Public Sub XmlTrainerLesenTest()
        Debug.Print($"Version2 Trainer lesen")
        Dim Trainer = Controller.DatenLaden.TrainerLesen()
        Assert.IsNotNull(Trainer)
    End Sub

    <TestMethod>
    Public Sub XmlEinteilungenLesenTest()
        Debug.Print($"Version2 Einteilungen lesen")
        Dim Einteilungen = Controller.DatenLaden.EinteilungenLesen()
        Assert.IsNotNull(Einteilungen)
    End Sub


    <TestMethod>
    Public Sub LeseXMLDateiVersion2Test()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2.ski"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2.ski"
        End If

        Dim Filestream = New IO.FileStream(Pfad, IO.FileMode.Open)
        Dim Erfolg = Controller.DatenLaden.LeseXMLDateiVersion2(Filestream)
        Assert.IsTrue(Erfolg)
    End Sub

    <TestMethod>
    Public Sub LeseXMLDateiVersion2Test2()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion1.ski"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion1.ski"
        End If
        Dim Filestream = New IO.FileStream(Pfad, IO.FileMode.Open)
        Dim Erfolg = Controller.DatenLaden.LeseXMLDateiVersion2(Filestream)
        Assert.IsFalse(Erfolg)
    End Sub
End Class
