Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Entities
Imports Groupies
Imports System.IO
Imports System.Xml.Serialization

<TestClass>
Public Class MappingVeraltertAufNeuTest

    <TestMethod>
    Public Sub TestClubOeffnen()

        Dim filename
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            filename = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiClub.ski"
        Else
            filename = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\TestdateiClub.ski"
        End If

        If File.Exists(filename) Then

            ' Datei deserialisieren
            Dim serializer = New XmlSerializer(GetType(Generation1.Skiclub))
            Dim loadedSkiclub As Generation1.Skiclub
            Using fs = New FileStream(filename, FileMode.Open)
                Try
                    loadedSkiclub = TryCast(serializer.Deserialize(fs), Generation1.Skiclub)
                Catch ex As InvalidDataException
                    Debug.Print("Datei ungültig: " & ex.Message)
                    Exit Sub
                End Try
            End Using

            Dim neuerClub = MappingGeneration1.MapSkiClub2Club(loadedSkiclub)

            Assert.AreEqual(loadedSkiclub.Levellist.Count, neuerClub.AlleLeistungsstufen.Count)
            Assert.AreEqual(84, neuerClub.AlleEinteilungen(0).EingeteilteTeilnehmer.Count)
            Assert.AreEqual(0, neuerClub.AlleEinteilungen(0).GruppenloseTeilnehmer.Count)
            Assert.AreEqual(10, neuerClub.AlleEinteilungen(0).EinteilungAlleGruppen.Count)
            Assert.AreEqual(0, neuerClub.AlleEinteilungen(0).GruppenloseTrainer.Count)
            Assert.AreEqual(loadedSkiclub.Participantlist.OrderBy(Function(p) p.ParticipantID)(0).ParticipantID, neuerClub.AlleEinteilungen(0).EinteilungAlleTeilnehmer.OrderBy(Function(T) T.TeilnehmerID)(0).TeilnehmerID)
            Assert.AreEqual(loadedSkiclub.Levellist.Count, neuerClub.AlleLeistungsstufen.Count)
            Assert.AreEqual(loadedSkiclub.Grouplist.OrderBy(Function(GL) GL.GroupNaming)(7).GroupLeader.InstructorFullName, neuerClub.AlleEinteilungen(0).EinteilungAlleGruppen.OrderBy(Function(Gl) Gl.Benennung)(7).Trainer.VorUndNachname)
        End If

    End Sub
End Class
