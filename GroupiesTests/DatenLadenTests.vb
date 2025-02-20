Imports Groupies
Imports Groupies.Entities
Imports Groupies.Controller

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
        Dim Gruppen = Controller.DatenLaden.GruppenLesen()
        Assert.IsNotNull(Gruppen)
    End Sub

    <TestMethod>
    Public Sub XmlTeilnehmerLesenTest()
        Dim Teilnehmer = Controller.DatenLaden.TeilnehmerLesen()
        Assert.IsNotNull(Teilnehmer)
    End Sub


    <TestMethod>
    Public Sub XmlTrainerLesenTest()
        Dim Trainer = Controller.DatenLaden.TrainerLesen()
        Assert.IsNotNull(Trainer)
    End Sub

    <TestMethod>
    Public Sub XmlEinteilungenLesenTest()
        Dim Einteilungen = Controller.DatenLaden.EinteilungenLesen()
        Assert.IsNotNull(Einteilungen)
    End Sub


    <TestMethod>
    Public Sub LeseXMLDateiVersion2Test()
        Dim Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2.ski"
        Dim Filestream = New IO.FileStream(Pfad, IO.FileMode.Open)
        Dim Erfolg = Controller.DatenLaden.LeseXMLDateiVersion2(Filestream)
        Assert.IsTrue(Erfolg)
    End Sub

    <TestMethod>
    Public Sub LeseXMLDateiVersion2Test2()
        Dim Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion11.ski"
        Dim Filestream = New IO.FileStream(Pfad, IO.FileMode.Open)
        Dim Erfolg = Controller.DatenLaden.LeseXMLDateiVersion2(Filestream)
        Assert.IsFalse(Erfolg)
    End Sub
End Class
