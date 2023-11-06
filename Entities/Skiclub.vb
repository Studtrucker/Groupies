Imports System.ComponentModel.DataAnnotations
Imports Skischule.Entities
Imports System.Collections.ObjectModel

Namespace Entities


    Public Class Skiclub

#Region "Fields"

        Public Property Participantlist() As ParticipantCollection
        Public Property Grouplist() As GroupCollection
        Public Property Levellist() As LevelCollection
        Public Property Instructorlist() As InstructorCollection

#End Region

#Region "Constructor"
        Public Sub New()
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            _Levellist = New LevelCollection
            _Instructorlist = New InstructorCollection
        End Sub

        Public Sub New(Teilnehmerliste As ParticipantCollection)
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            _Levellist = New LevelCollection
            readTeilnehmerliste(Teilnehmerliste)
        End Sub

#End Region

        Public ReadOnly Property ParticipantsToDistribute As ParticipantCollection
            Get
                Dim List = New ParticipantCollection
                Participantlist.Where(Function(x) x.MemberOfGroup.Equals(Nothing)).ToList.ForEach(Sub(item) List.Add(item))
                Return List
            End Get
        End Property

        Public Function GetInstructorsWithoutGroup() As InstructorCollection
            Dim List = New InstructorCollection
            Instructorlist.Where(Function(x) x.LeaderOfGroup.Equals(Nothing)).ToList.ForEach(Sub(y) List.Add(y))
            Return List
        End Function

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
