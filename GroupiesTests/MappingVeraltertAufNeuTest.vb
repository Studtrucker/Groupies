Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Entities
Imports Groupies
Imports System.IO
Imports System.Xml.Serialization

<TestClass>
Public Class MappingVeraltertAufNeuTest

    <TestMethod>
    Public Sub TestClubOeffnen()
        Dim filename = "\\ds214play\BackupAndreas\GroupiesReisen\Stubaital\20231118Stubai.ski"
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
            Assert.AreEqual(84, neuerClub.EingeteilteTeilnehmer.Count)
            Assert.AreEqual(0, neuerClub.GruppenloseTeilnehmer.Count)
            Assert.AreEqual(10, neuerClub.Gruppenliste.Count)
            Assert.AreEqual(0, neuerClub.GruppenloseTrainer.Count)
            Assert.AreEqual(loadedSkiclub.Participantlist.OrderBy(Function(p) p.ParticipantID)(0).ParticipantID, neuerClub.AlleTeilnehmer.OrderBy(Function(T) T.TeilnehmerID)(0).TeilnehmerID)
            Assert.AreEqual(loadedSkiclub.Levellist.Count, neuerClub.Leistungsstufenliste.Count)
            Assert.AreEqual(loadedSkiclub.Grouplist.OrderBy(Function(GL) GL.GroupNaming)(7).GroupLeader.InstructorFullName, neuerClub.Gruppenliste.OrderBy(Function(Gl) Gl.Benennung)(7).Trainer.VorUndNachname)
        End If

    End Sub
End Class
