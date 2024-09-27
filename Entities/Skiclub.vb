Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities


    Public Class Skiclub

#Region "Fields"

        Public Property Name As String

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
            readParticipantlist(Teilnehmerliste)
        End Sub

        Public Sub New(Instructorlist As InstructorCollection)
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            _Levellist = New LevelCollection
            readInstructorlist(Instructorlist)
        End Sub

        Public Sub New(Clubname As String, Teilnehmerliste As ParticipantCollection)
            _Name = Clubname
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            _Levellist = New LevelCollection
            readParticipantlist(Teilnehmerliste)
        End Sub

        Public Sub New(Clubname As String, Instructorlist As InstructorCollection)
            _Name = Clubname
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            _Levellist = New LevelCollection
            readInstructorlist(Instructorlist)
        End Sub


        Public Sub New(Clubname As String)
            _Name = Clubname
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            _Levellist = New LevelCollection
            _Instructorlist = New InstructorCollection
        End Sub

#End Region

#Region "Properties"

        'Public ReadOnly Property ParticipantsNotInAGroup As ParticipantCollection
        '    Get
        '        Dim List = New ParticipantCollection
        '        Participantlist.Where(Function(x) x.IsNotInGroup).ToList.ForEach(Sub(item) List.Add(item))
        '        Return List
        '    End Get
        'End Property

        Public ReadOnly Property InstructorsAvailable As InstructorCollection
            Get
                Dim List As New InstructorCollection
                Instructorlist.Where(Function(x) x.IsAvailable).ToList.ForEach(Sub(item) List.Add(item))
                Return List
            End Get
        End Property

#End Region

        Public Function AnzahlFreieTeilnehmer() As Integer
            Return Participantlist.Where(Function(TN) TN.IsNotInGroup).Count
        End Function

        Public Function AnzahlEingeteilteTeilnehmer() As Integer
            Return Participantlist.Where(Function(TN) TN.IsGroupMember).Count
        End Function

        Public Property Teilnehmerliste = If(Participantlist Is Nothing, "", Participantlist.Select(Function(Tn) Tn.ParticipantFullName & vbCrLf))

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
