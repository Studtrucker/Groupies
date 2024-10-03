Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies
Imports System.IO
Imports Groupies.Entities
Imports Groupies.Controller

Namespace GroupiesTestsVB
    <TestClass>
    Public Class AppControllerTests

        <TestMethod>
        Sub TestNeuenSkiclubErstellen()

            Dim numberOfGroups = 9
            Assert.AreEqual($"Stubaital2024 wurde mit {numberOfGroups} Gruppen erfolgreich erstellt.", AppController.NeuenClubErstellen("Stubaital2024", numberOfGroups))
            Assert.AreEqual("Stubaital2024", AppController.CurrentClub.ClubName)
            Assert.AreEqual(9, AppController.CurrentClub.Gruppenliste.Count)
            Assert.AreEqual(0, AppController.CurrentClub.Teilnehmerliste.Count)
            Assert.AreEqual(5, AppController.StandardLeistungsstufen.Count)
            Assert.AreEqual(0, AppController.CurrentClub.Trainerliste.Count)

            Dim Studti As New Teilnehmer("Andreas", "Studtrucker")
            Dim Manuela As New Teilnehmer("Manuela", "Ramm")

            AppController.CurrentClub.Teilnehmerliste = New TeilnehmerCollection From {Studti, Manuela}
            Assert.AreEqual(2, AppController.CurrentClub.Teilnehmerliste.Count)
            Assert.AreEqual(2, AppController.CurrentClub.FreieTeilnehmer.Count)
            Assert.AreEqual(0, AppController.CurrentClub.EingeteilteTeilnehmer.Count)

            'Assert.AreEqual(String.Format("Manuela Ramm{0}Andreas Studtrucker{0}", vbCrLf), AppController.CurrentClub.Teilnehmerliste)

        End Sub



        <TestMethod>
        Sub TestLoadJson()
            Dim filename As String
            filename = "Test"
            ' File erst erstellen
            File.Create(String.Format("{0}.json", filename)).Close()
            Assert.AreEqual(String.Format("Die Datei {0} wurde geladen.", String.Format("{0}.json", filename)), AppController.LoadFromJson(filename))
            ' File wieder löschen
            File.Delete(String.Format("{0}.json", filename))
        End Sub

        <TestMethod>
        Sub TestLoadXML()
            Dim filename As String
            filename = "test"
            ' File erst erstellen
            File.Create(String.Format("{0}.xml", filename)).Close()
            Assert.AreEqual(String.Format("Die Datei {0} wurde geladen.", String.Format("{0}.xml", filename)), AppController.LoadFromXML(filename))
            ' File wieder löschen
            File.Delete(String.Format("{0}.xml", filename))
        End Sub

        <TestMethod>
        Public Sub TestInvalidFilenames()
            Assert.AreEqual("Bitte geben Sie einen Dateinamen ohne Schrägstriche oder Leerzeichen ein.", AppController.LoadFromJson("invalid\\filename"))
            Assert.AreEqual("Bitte geben Sie einen Dateinamen ohne Schrägstriche oder Leerzeichen ein.", AppController.LoadFromJson("invalid/filename"))
            Assert.AreEqual("Bitte geben Sie einen Dateinamen ohne Schrägstriche oder Leerzeichen ein.", AppController.LoadFromJson("invalid filename"))
        End Sub

        <TestMethod>
        Public Sub TestNotExistingFile()
            Dim filename As String
            filename = "tester"
            Assert.AreEqual(String.Format("Die Datei {0} im Ordner {1} existiert nicht.", filename & ".json", Environment.CurrentDirectory), AppController.LoadFromJson(filename))
            Assert.AreEqual(String.Format("Die Datei {0} im Ordner {1} existiert nicht.", filename & ".xml", Environment.CurrentDirectory), AppController.LoadFromXML(filename))
        End Sub

        <TestMethod>
        Public Sub TestDateiEinlesen()
            AppController.XMLDateiEinlesen("Z:\GroupiesReisen\Stubaital\20231118Stubai.ski")

        End Sub



    End Class

End Namespace

