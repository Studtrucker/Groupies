Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Entities

    ''' <summary>
    ''' Fähigkeit zur Beschreibung von Leistungsstufen 
    ''' </summary>
    Public Class Faehigkeit
        Inherits BaseModel
        Implements IModel

#Region "Felder"
        Private _FaehigkeitID = Guid.NewGuid
        Private _Benennung As String
        Private _Beschreibung As String
        Private _Sortierung As Integer
#End Region

#Region "Konstruktor"


        ''' <summary>
        ''' Erstellt eine neue Fähigkeit
        ''' </summary>
        Public Sub New()
            _Sortierung = -1
        End Sub

        ''' <summary>
        ''' Erstellt eine neue Fähigkeit unter Angabe der Benennung
        ''' </summary>
        ''' <param name="Benennung"></param>
        Sub New(Benennung As String)
            _Sortierung = -1
            _Benennung = Benennung
        End Sub

        ''' <summary>
        ''' Erstellt eine neue Fähigkeit unter Angabe der Benennung und Sortierungskennzahl
        ''' </summary>
        ''' <param name="Benennung"></param>
        Sub New(Benennung As String, Sortierung As Integer)
            _Benennung = Benennung
            _Sortierung = Sortierung
        End Sub

        ''' <summary>
        ''' Kopierkonstruktor für tiefes Kopieren
        ''' </summary>
        ''' <param name="Origin"></param>
        Public Sub New(origin As Faehigkeit)
            _FaehigkeitID = origin.FaehigkeitID
            _Benennung = origin.Benennung
            _Beschreibung = origin.Beschreibung
            _Sortierung = origin.Sortierung
        End Sub
#End Region

#Region "Properties"
        ''' <summary>
        ''' Eindeutige Kennzeichnung der Fähigkeit
        ''' </summary>
        ''' <returns></returns>
        Public Property FaehigkeitID As Guid Implements IModel.Ident
            Get
                Return _FaehigkeitId
            End Get
            Set(value As Guid)
                _FaehigkeitID = value
            End Set
        End Property

        ''' <summary>
        ''' Benennung der Fähigkeit
        ''' </summary>
        ''' <returns></returns>

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Benennung ist eine Pflichtangabe")>
        Public Property Benennung As String
            Get
                Return _Benennung
            End Get
            Set(value As String)
                _Benennung = value
            End Set
        End Property

        ''' <summary>
        ''' Beschreibung der Fähigkeit
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Beschreibung ist ein Pflichtfeld")>
        Public Property Beschreibung As String
            Get
                Return _Beschreibung
            End Get
            Set(value As String)
                _Beschreibung = value
            End Set
        End Property

        ''' <summary>
        ''' Sortierungszahl für die Ausgabeinformationen 
        ''' </summary>
        ''' <returns></returns>
        Public Property Sortierung As Integer
            Get
                Return _Sortierung
            End Get
            Set(value As Integer)
                _Sortierung = value
            End Set
        End Property

        ''' <summary>
        ''' Gibt die Benennung und Beschreibung für die Trainerinformation zurück
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AusgabeAnTrainerInfo As String
            Get
                If Beschreibung Is Nothing Then
                    Return $"{Sortierung}. {Benennung}"
                Else
                    Return $"{Sortierung}. {Benennung}{Environment.NewLine}{Beschreibung}"
                End If
            End Get
        End Property

#End Region

#Region "Funktionen und Methoden"

        Public Overrides Function ToString() As String
            Return Benennung
        End Function

        Public Sub speichern() Implements IModel.speichern
            Throw New NotImplementedException()
        End Sub
#End Region

    End Class
End Namespace
