Imports System.ComponentModel.DataAnnotations
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
            _Levellist.Add(New Level(False) With {.LevelNaming = String.Empty, .SortNumber = "000"})
            _Instructorlist = New InstructorCollection
            _Instructorlist.Add(New Instructor(False, True) With {
                                .InstructorFirstName = String.Empty,
                                .InstructorLastName = String.Empty,
                                .InstructorPrintName = String.Empty})
        End Sub

        Public Sub New(Teilnehmerliste As ParticipantCollection)
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            _Levellist = New LevelCollection
            readParticipantlist(Teilnehmerliste)
        End Sub

        Public Sub New(Instructorlist As InstructorCollection)
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            _Levellist = New LevelCollection
            readInstructorlist(Instructorlist)
        End Sub

#End Region

        Public ReadOnly Property ParticipantsToDistribute As ParticipantCollection
            Get
                Dim List = New ParticipantCollection
                Participantlist.Where(Function(x) x.MemberOfGroup.Equals(Nothing)).ToList.ForEach(Sub(item) List.Add(item))
                Return List
            End Get
        End Property

        Private Sub readParticipantlist(Teilnehmer As ParticipantCollection)
            Participantlist = Teilnehmer
            'Skikursgruppenliste = Teilnehmer.ToList.ForEach()
            ' Levelsliste = Teilnehmer.ToList.ForEach()
        End Sub
        Private Sub readInstructorlist(Instructors As InstructorCollection)
            Instructorlist = Instructors
            'Skikursgruppenliste = Teilnehmer.ToList.ForEach()
            ' Levelsliste = Teilnehmer.ToList.ForEach()
        End Sub

        Public Function GetAktualisierungen() As Skiclub
            Grouplist.ToList.ForEach(AddressOf GetAktualisierungen)
            Participantlist.ToList.ForEach(AddressOf GetAktualisierungen)
            Return Me
        End Function

        Private Sub GetAktualisierungen(Skikurs As Group)
            'Skikurs.Skilehrer = Skilehrerliste.Where(Function(x) x.SkilehrerID = Skikurs.Skilehrer.SkilehrerID).First
            'For i = 0 To Skikurs.Mitgliederliste.Count - 1
            '    Skikurs.Mitgliederliste.Item(i) = GetAktualisierungen(Skikurs.Mitgliederliste.Item(i))
            'Next
        End Sub

        Private Sub GetAktualisierungen(Mitglied As Participant)
            'Participantlist.Where(Function(x) x.ParticipantID = Mitglied.ParticipantID).First
        End Sub


    End Class

End Namespace
