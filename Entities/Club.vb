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
            _Gruppenliste = New GruppenCollection
            _Teilnehmerliste = New TeilnehmerCollection
            _Trainerliste = New TrainerCollection
        End Sub

        ''' <summary>
        ''' Erstellung eines neuen Clubs unter Angabe einer Teilnehmerliste
        ''' Es wird zusätzlich eine leere Gruppenliste und leere Teilnehmerliste erstellt
        ''' </summary>
        ''' <param name="Teilnehmerliste"></param>
        <Obsolete>
        Public Sub New(Teilnehmerliste As TeilnehmerCollection)
            _Gruppenliste = New GruppenCollection
            _Teilnehmerliste = New TeilnehmerCollection
            LadeTeilnehmerliste(Teilnehmerliste)
        End Sub

        ''' <summary>
        ''' Erstellung eines neuen Clubs unter Angabe einer Trainerliste
        ''' </summary>
        ''' <param name="Trainerliste"></param>
        <Obsolete>
        Public Sub New(Trainerliste As TrainerCollection)
            _Gruppenliste = New GruppenCollection
            _Teilnehmerliste = New TeilnehmerCollection
            LadeTrainerliste(Trainerliste)
        End Sub

        <Obsolete>
        Public Sub New(Clubname As String, Teilnehmerliste As TeilnehmerCollection)
            _ClubName = Clubname
            _Gruppenliste = New GruppenCollection
            _Teilnehmerliste = New TeilnehmerCollection
            LadeTeilnehmerliste(Teilnehmerliste)
        End Sub

        ''' <summary>
        ''' Erstellung eines neuen Clubs unter Angabe einer Benennung und einer Trainerliste
        ''' </summary>
        ''' <param name="Benennung"></param>
        ''' <param name="Trainerliste"></param>
        <Obsolete>
        Public Sub New(Benennung As String, Trainerliste As TrainerCollection)
            _ClubName = Benennung
            _Gruppenliste = New GruppenCollection
            _Teilnehmerliste = New TeilnehmerCollection
            LadeTrainerliste(Trainerliste)
        End Sub

        ''' <summary>
        ''' Erstellung eines neuen Clubs unter Angabe einer Benennung und Erstellung von 5 leeren Gruppen
        ''' </summary>
        ''' <param name="Benennung"></param>
        Public Sub New(Benennung As String)
            _ClubName = Benennung
            _Gruppenliste = PresetService.CreateGroups(5)
            _Teilnehmerliste = New TeilnehmerCollection
            _Trainerliste = New TrainerCollection
        End Sub

        Public Sub New(Benennung As String, NumberOfGroups As Integer)
            _ClubName = Benennung
            _Gruppenliste = PresetService.CreateGroups(NumberOfGroups)
            _Teilnehmerliste = New TeilnehmerCollection
            _Trainerliste = New TrainerCollection
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
        Public Property Teilnehmerliste() As TeilnehmerCollection

        ''' <summary>
        ''' Eine Liste aller Gruppen im Club
        ''' </summary>
        ''' <returns></returns>
        Public Property Gruppenliste() As GruppenCollection

        ''' <summary>
        ''' Eine Liste aller Leistungsstufen
        ''' </summary>
        ''' <returns></returns>
        Public Property Leistungsstufeliste() As LeistungsstufeCollection

        ''' <summary>
        ''' Eine Liste aller Trainer
        ''' </summary>
        ''' <returns></returns>
        Public Property Trainerliste() As TrainerCollection

        Public Property Participantliste = If(_Teilnehmerliste Is Nothing, "", _Teilnehmerliste.Select(Function(Tn) Tn.VorUndNachname & vbCrLf))

        'Public ReadOnly Property ParticipantsNotInAGroup As ParticipantCollection
        '    Get
        '        Dim List = New ParticipantCollection
        '        Participantlist.Where(Function(x) x.IsNotInGroup).ToList.ForEach(Sub(item) List.Add(item))
        '        Return List
        '    End Get
        'End Property

        <Obsolete>
        Public ReadOnly Property InstructorsAvailable As TrainerCollection
            Get
                Dim List As New TrainerCollection
                Trainerliste.Where(Function(x) x.IsAvailable).ToList.ForEach(Sub(item) List.Add(item))
                Return List
            End Get
        End Property

#End Region

#Region "Funktionen und Methoden"
        Public Function AnzahlFreieTeilnehmer() As Integer
            Return Teilnehmerliste.Where(Function(TN) TN.IsNotInGroup).Count
        End Function

        Public Function AnzahlEingeteilteTeilnehmer() As Integer
            Return Teilnehmerliste.Where(Function(TN) TN.IsGroupMember).Count
        End Function

        Public Overrides Function ToString() As String
            Return ClubName
        End Function

        Public Function GetAktualisierungen() As Club
            Gruppenliste.ToList.ForEach(AddressOf GetAktualisierungen)
            Teilnehmerliste.ToList.ForEach(AddressOf GetAktualisierungen)
            Return Me
        End Function

        Private Sub LadeTeilnehmerliste(Teilnehmer As TeilnehmerCollection)
            _Teilnehmerliste = Teilnehmer
            'Skikursgruppenliste = Teilnehmer.ToList.ForEach()
            ' Levelsliste = Teilnehmer.ToList.ForEach()
        End Sub

        Private Sub LadeTrainerliste(Instructors As TrainerCollection)
            _Trainerliste = Instructors
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
