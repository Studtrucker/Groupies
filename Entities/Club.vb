Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports Groupies.Services

Namespace Entities


    Public Class Club


#Region "Fields"


#End Region

#Region "Konstruktor"

        ''' <summary>
        ''' Erstellung eines neuen Clubs
        ''' Es wird zusätzlich eine leere Teilnehmerliste und leere Trainerliste erstellt
        ''' </summary>
        Public Sub New()
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            _Instructorlist = New InstructorCollection
        End Sub

        ''' <summary>
        ''' Erstellung eines neuen Clubs unter Angabe einer Teilnehmerliste
        ''' Es wird zusätzlich eine leere Gruppenliste und leere Teilnehmerliste erstellt
        ''' </summary>
        ''' <param name="Teilnehmerliste"></param>
        <Obsolete>
        Public Sub New(Teilnehmerliste As ParticipantCollection)
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            LadeTeilnehmerliste(Teilnehmerliste)
        End Sub

        ''' <summary>
        ''' Erstellung eines neuen Clubs unter Angabe einer Trainerliste
        ''' </summary>
        ''' <param name="Instructorlist"></param>
        <Obsolete>
        Public Sub New(Instructorlist As InstructorCollection)
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            LadeTrainerliste(Instructorlist)
        End Sub

        <Obsolete>
        Public Sub New(Clubname As String, Teilnehmerliste As ParticipantCollection)
            _ClubName = Clubname
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            LadeTeilnehmerliste(Teilnehmerliste)
        End Sub

        ''' <summary>
        ''' Erstellung eines neuen Clubs unter Angabe einer Benennung und einer Trainerliste
        ''' </summary>
        ''' <param name="Benennung"></param>
        ''' <param name="Instructorlist"></param>
        <Obsolete>
        Public Sub New(Benennung As String, Instructorlist As InstructorCollection)
            _ClubName = Benennung
            _Grouplist = New GroupCollection
            _Participantlist = New ParticipantCollection
            LadeTrainerliste(Instructorlist)
        End Sub

        ''' <summary>
        ''' Erstellung eines neuen Clubs unter Angabe einer Benennung und Erstellung von 5 leeren Gruppen
        ''' </summary>
        ''' <param name="Benennung"></param>
        Public Sub New(Benennung As String)
            _ClubName = Benennung
            _Grouplist = PresetService.CreateGroups(5)
            _Participantlist = New ParticipantCollection
            _Instructorlist = New InstructorCollection
        End Sub

        Public Sub New(Benennung As String, NumberOfGroups As Integer)
            _ClubName = Benennung
            _Grouplist = PresetService.CreateGroups(NumberOfGroups)
            _Participantlist = New ParticipantCollection
            _Instructorlist = New InstructorCollection
        End Sub


#End Region

#Region "Eigenschaften"
        ''' <summary>
        ''' Der Clubname
        ''' </summary>
        ''' <returns></returns>
        Public Property ClubName As String

        ''' <summary>
        ''' Eine Liste aller Teilnehmer im Club
        ''' </summary>
        ''' <returns></returns>
        Public Property Participantlist() As ParticipantCollection

        ''' <summary>
        ''' Eine Liste aller Gruppen im Club
        ''' </summary>
        ''' <returns></returns>
        Public Property Grouplist() As GroupCollection

        ''' <summary>
        ''' Eine Liste aller Leistungsstufen
        ''' </summary>
        ''' <returns></returns>
        Public Property Levellist() As LevelCollection

        ''' <summary>
        ''' Eine Liste aller Trainer
        ''' </summary>
        ''' <returns></returns>
        Public Property Instructorlist() As InstructorCollection

        Public Property Teilnehmerliste = If(_Participantlist Is Nothing, "", _Participantlist.Select(Function(Tn) Tn.VorUndNachname & vbCrLf))

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

#Region "Funktionen und Methoden"
        Public Function AnzahlFreieTeilnehmer() As Integer
            Return Participantlist.Where(Function(TN) TN.IsNotInGroup).Count
        End Function

        Public Function AnzahlEingeteilteTeilnehmer() As Integer
            Return Participantlist.Where(Function(TN) TN.IsGroupMember).Count
        End Function

        Public Overrides Function ToString() As String
            Return ClubName
        End Function
        Public Function GetAktualisierungen() As Club
            Grouplist.ToList.ForEach(AddressOf GetAktualisierungen)
            Participantlist.ToList.ForEach(AddressOf GetAktualisierungen)
            Return Me
        End Function

        Private Sub LadeTeilnehmerliste(Teilnehmer As ParticipantCollection)
            Participantlist = Teilnehmer
            'Skikursgruppenliste = Teilnehmer.ToList.ForEach()
            ' Levelsliste = Teilnehmer.ToList.ForEach()
        End Sub

        Private Sub LadeTrainerliste(Instructors As InstructorCollection)
            Instructorlist = Instructors
            'Skikursgruppenliste = Teilnehmer.ToList.ForEach()
            ' Levelsliste = Teilnehmer.ToList.ForEach()
        End Sub
        Private Sub GetAktualisierungen(Skikurs As Gruppe)
            'Skikurs.Skilehrer = Skilehrerliste.Where(Function(x) x.SkilehrerID = Skikurs.Skilehrer.SkilehrerID).First
            'For i = 0 To Skikurs.Mitgliederliste.Count - 1
            '    Skikurs.Mitgliederliste.Item(i) = GetAktualisierungen(Skikurs.Mitgliederliste.Item(i))
            'Next
        End Sub

        Private Sub GetAktualisierungen(Mitglied As Teilnehmer)
            'Participantlist.Where(Function(x) x.ParticipantID = Mitglied.ParticipantID).First
        End Sub

#End Region

#Region "Veraltert"

#End Region

    End Class

End Namespace
