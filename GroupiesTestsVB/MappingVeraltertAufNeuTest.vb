Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Entities
Imports Groupies
Imports System.IO
Imports System.Xml.Serialization

<TestClass>
Public Class MappingVeraltertAufNeuTest

    <TestMethod>
    Public Sub TestClubOeffnen()
        Dim filename = "C:\Users\studtan\OneDrive\Dokumente\Reisen\20231124Stubai.ski"
        If File.Exists(filename) Then

            Dim loadedSkiclub = AppController.DateiEinlesen(filename)
            Dim neuerClub = VeralterteKlassenMapping.MapSkiClub2Club(loadedSkiclub)

            Assert.AreEqual(loadedSkiclub.Levellist.Count, neuerClub.Leistungsstufeliste.Count)
            Assert.AreEqual(loadedSkiclub.Participantlist.Count, neuerClub.Teilnehmerliste.Count)
            Assert.AreEqual(loadedSkiclub.Grouplist.Count, neuerClub.Gruppenliste.Count)
            Assert.AreEqual(loadedSkiclub.Instructorlist.Count, neuerClub.Trainerliste.Count)
            Assert.AreEqual(loadedSkiclub.Participantlist(0).ParticipantID, neuerClub.Teilnehmerliste(0).TeilnehmerID)
            Assert.AreEqual(loadedSkiclub.Levellist(2).LevelDescription, neuerClub.Leistungsstufeliste(2).Beschreibung)
            Assert.AreEqual(loadedSkiclub.Grouplist(7).GroupLeader.InstructorFullName, neuerClub.Gruppenliste(7).Trainer.VorUndNachname)
        End If

    End Sub
End Class
