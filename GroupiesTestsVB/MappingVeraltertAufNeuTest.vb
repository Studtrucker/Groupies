Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Entities
Imports Groupies
Imports System.IO
Imports System.Xml.Serialization

<TestClass>
Public Class MappingVeraltertAufNeuTest

    <TestMethod>
    Public Sub TestClubOeffnen()
        Dim filename = "Z:\GroupiesReisen\Stubaital\20231118Stubai.ski"
        If File.Exists(filename) Then

            ' Datei deserialisieren
            Dim serializer = New XmlSerializer(GetType(Veraltert.Skiclub))
            Dim loadedSkiclub As Veraltert.Skiclub
            Using fs = New FileStream(filename, FileMode.Open)
                Try
                    loadedSkiclub = TryCast(serializer.Deserialize(fs), Veraltert.Skiclub)
                Catch ex As InvalidDataException
                    Debug.Print("Datei ungültig: " & ex.Message)
                    Exit Sub
                End Try
            End Using

            Dim neuerClub = VeralterteKlassenMapping.MapSkiClub2Club(loadedSkiclub)

            Assert.AreEqual(loadedSkiclub.Levellist.Count, neuerClub.Leistungsstufenliste.Count)
            Assert.AreEqual(loadedSkiclub.Participantlist.Count, neuerClub.GruppenloseTeilnehmer.Count)
            Assert.AreEqual(loadedSkiclub.Grouplist.Count, neuerClub.Gruppenliste.Count)
            Assert.AreEqual(loadedSkiclub.Instructorlist.Count, neuerClub.GruppenloseTrainer.Count)
            Assert.AreEqual(loadedSkiclub.Participantlist(0).ParticipantID, neuerClub.GruppenloseTeilnehmer(0).TeilnehmerID)
            Assert.AreEqual(loadedSkiclub.Levellist(2).LevelDescription, neuerClub.Leistungsstufenliste(2).Beschreibung)
            Assert.AreEqual(loadedSkiclub.Grouplist(7).GroupLeader.InstructorFullName, neuerClub.Gruppenliste(7).Trainer.VorUndNachname)
        End If

    End Sub
End Class
