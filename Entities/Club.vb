Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports Groupies.Services

Namespace Entities


    Public Class Club


#Region "Fields"

        Private _Gruppenliste = New GruppeCollection
        Private _Teilnehmerliste = New TeilnehmerCollection
        Private _Trainerliste = New TrainerCollection

#End Region

#Region "Konstruktor"

        ''' <summary>
        ''' Parameterloser Konstruktor für das
        ''' Einlesen von XML Dateien notwendig
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Erstellung eines neuen Clubs
        ''' Gruppenliste, Teilnehmerliste und Trainerliste werden instanziiert
        ''' </summary>
        Public Sub New(Clubname As String)
            _ClubName = Clubname
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
            Get
                Return _Teilnehmerliste
            End Get
            Set(value As TeilnehmerCollection)
                _Teilnehmerliste = value
            End Set
        End Property

        ''' <summary>
        ''' Eine Liste aller Trainer im Club
        ''' </summary>
        ''' <returns></returns>
        Public Property Trainerliste() As TrainerCollection
            Get
                Return _Trainerliste
            End Get
            Set(value As TrainerCollection)
                _Trainerliste = value
            End Set
        End Property

        ''' <summary>
        ''' Eine Liste aller Gruppen im Club
        ''' </summary>
        ''' <returns></returns>
        Public Property Gruppenliste() As GruppeCollection
            Get
                Return _Gruppenliste
            End Get
            Set(value As GruppeCollection)
                _Gruppenliste = value
            End Set
        End Property

        ''' <summary>
        ''' Eine Liste der verwendeten Leistungsstufen
        ''' </summary>
        ''' <returns></returns>
        Public Property Leistungsstufenliste() As LeistungsstufeCollection

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
            Return Teilnehmerliste.Where(Function(TN) TN.IstGruppenmitglied = False).Count
        End Function

        Public Function AnzahlEingeteilteTeilnehmer() As Integer
            Return Teilnehmerliste.Where(Function(TN) TN.IstGruppenmitglied).Count
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
