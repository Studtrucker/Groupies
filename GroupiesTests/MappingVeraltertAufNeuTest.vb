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
            Assert.AreEqual(84, neuerClub.Einteilungsliste(0).EingeteilteTeilnehmer.Count)
            Assert.AreEqual(0, neuerClub.Einteilungsliste(0).GruppenloseTeilnehmer.Count)
            Assert.AreEqual(10, neuerClub.Einteilungsliste(0).Gruppenliste.Count)
            Assert.AreEqual(0, neuerClub.Einteilungsliste(0).GruppenloseTrainer.Count)
            Assert.AreEqual(loadedSkiclub.Participantlist.OrderBy(Function(p) p.ParticipantID)(0).ParticipantID, neuerClub.Einteilungsliste(0).AlleTeilnehmer.OrderBy(Function(T) T.TeilnehmerID)(0).TeilnehmerID)
            Assert.AreEqual(loadedSkiclub.Levellist.Count, neuerClub.Leistungsstufenliste.Count)
            Assert.AreEqual(loadedSkiclub.Grouplist.OrderBy(Function(GL) GL.GroupNaming)(7).GroupLeader.InstructorFullName, neuerClub.Einteilungsliste(0).Gruppenliste.OrderBy(Function(Gl) Gl.Benennung)(7).Trainer.VorUndNachname)
        End If

    End Sub
End Class
