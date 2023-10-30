Imports System.ComponentModel.DataAnnotations
Imports Skischule.Entities

Namespace Entities


    Public Class Skiclub

#Region "Fields"

        Public Property Participantlist() As ParticipantCollection
        Public Property Grouplist() As GroupCollection
        Public Property Levellist() As LevelCollection
        Public Property Skilehrerliste() As InstructorCollection

#End Region

#Region "Constructor"
        Public Sub New()
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            _Levellist = New LevelCollection
            _Skilehrerliste = New InstructorCollection
        End Sub

        Public Sub New(Teilnehmerliste As ParticipantCollection)
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            _Levellist = New LevelCollection
            readTeilnehmerliste(Teilnehmerliste)
        End Sub

#End Region

        Private Sub readTeilnehmerliste(Teilnehmer As ParticipantCollection)
            Participantlist = Teilnehmer
            'Skikursgruppenliste = Teilnehmer.ToList.ForEach()
            ' Levelsliste = Teilnehmer.ToList.ForEach()
        End Sub

        Public Function GetAktualisierungen() As Skiclub
            Grouplist.ToList.ForEach(AddressOf GetAktualisierungen)
            Return Me
        End Function

        Private Sub GetAktualisierungen(Skikurs As Group)
            'Skikurs.Skilehrer = Skilehrerliste.Where(Function(x) x.SkilehrerID = Skikurs.Skilehrer.SkilehrerID).First
            'For i = 0 To Skikurs.Mitgliederliste.Count - 1
            '    Skikurs.Mitgliederliste.Item(i) = GetAktualisierungen(Skikurs.Mitgliederliste.Item(i))
            'Next
        End Sub

        Private Function GetAktualisierungen(Mitglied As Participant) As Participant
            Return Participantlist.Where(Function(x) x.ParticipantID = Mitglied.ParticipantID).First
        End Function


    End Class

End Namespace
