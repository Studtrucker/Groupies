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

        ''' <summary>
        ''' Anzahl der Teilnehmer, die keiner Gruppe zugeteilt sind
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AnzahlFreieTeilnehmer As Integer
            Get
                Return (From t In Teilnehmerliste Where Not t.IstGruppenmitglied).Count
            End Get
        End Property


        ''' <summary>
        ''' Anzahl der Teilnehmer, die bereits einer Gruppe zugeteilt sind
        ''' </summary>
        ''' <returns></returns>

        Public ReadOnly Property AnzahlEingeteilteTeilnehmer As Integer
            Get
                Return (From t In Teilnehmerliste Where t.IstGruppenmitglied).Count
            End Get
        End Property

#End Region

#Region "Funktionen und Methoden"

        Public Overrides Function ToString() As String
            Return ClubName
        End Function

#End Region


    End Class

End Namespace
