Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports Groupies.Services

Namespace Entities


    Public Class Club


#Region "Fields"

        Public Property ClubName As String

        Public Property Participantlist() As ParticipantCollection
        Public Property Grouplist() As GroupCollection
        Public Property Levellist() As LevelCollection
        Public Property Instructorlist() As InstructorCollection

#End Region

#Region "Constructor"
        Public Sub New()
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            _Instructorlist = New InstructorCollection
        End Sub

        Public Sub New(Teilnehmerliste As ParticipantCollection)
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            readParticipantlist(Teilnehmerliste)
        End Sub

        Public Sub New(Instructorlist As InstructorCollection)
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            readInstructorlist(Instructorlist)
        End Sub

        Public Sub New(Clubname As String, Teilnehmerliste As ParticipantCollection)
            _ClubName = Clubname
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            readParticipantlist(Teilnehmerliste)
        End Sub

        Public Sub New(Clubname As String, Instructorlist As InstructorCollection)
            _ClubName = Clubname
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            readInstructorlist(Instructorlist)
        End Sub


        Public Sub New(Clubname As String)
            _ClubName = Clubname
            _Grouplist = PresetService.CreateGroups(5)
            _Participantlist = New ParticipantCollection
            _Instructorlist = New InstructorCollection
        End Sub

        Public Sub New(Clubname As String, NumberOfGroups As Integer)
            _ClubName = Clubname
            _Grouplist = PresetService.CreateGroups(NumberOfGroups)
            _Participantlist = New ParticipantCollection
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

        Public Overrides Function ToString() As String
            Return ClubName
        End Function

        Public Property Teilnehmerliste = If(_Participantlist Is Nothing, "", _Participantlist.Select(Function(Tn) Tn.VorUndNachname & vbCrLf))

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

        Public Function GetAktualisierungen() As Club
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

        Private Sub GetAktualisierungen(Mitglied As Teilnehmer)
            'Participantlist.Where(Function(x) x.ParticipantID = Mitglied.ParticipantID).First
        End Sub

    End Class

End Namespace
