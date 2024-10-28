Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports System.Linq

Namespace Entities

    ''' <summary>
    ''' Trainer, verantwortlich für eine Gruppe
    ''' </summary>
    Public Class Trainer
        Inherits BaseModel


        'Todo:Standardfoto festlegen
        'Private _bi As BitmapImage = New BitmapImage(New Uri("/Images/icons8-ski-goggles-96.png", UriKind.Relative))
        Private _Foto As Byte()
        Private _TrainerID = Guid.NewGuid()

#Region "Konstruktor"

        ''' <summary>
        ''' Erstellt einen neuen Teilnehmer
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Trainer mit Angabe von Vorname, Nachname und Spitzname
        ''' </summary>
        ''' <param name="Vorname"></param>
        ''' <param name="Nachname"></param>
        ''' <param name="Spitzname"></param>
        Public Sub New(Vorname As String, Nachname As String, Spitzname As String)
            _Spitzname = Spitzname
            _Vorname = Vorname
            _Nachname = Nachname
        End Sub

        ''' <summary>
        ''' Trainer mit Angabe von Vorname, Nachname
        ''' </summary>
        ''' <param name="Vorname"></param>
        ''' <param name="Nachname"></param>
        Public Sub New(Vorname As String, Nachname As String)
            _Vorname = Vorname
            _Nachname = Nachname
        End Sub

        ''' <summary>
        ''' Trainer mit Angabe von Vorname
        ''' </summary>
        ''' <param name="Vorname"></param>
        Public Sub New(Vorname As String)
            _Vorname = Vorname
        End Sub

#End Region

#Region "Eigenschaften"

        ''' <summary>
        ''' Eindeutige Kennzeichnung
        ''' </summary>
        ''' <returns></returns>
        Public Property TrainerID As Guid
            Get
                Return _TrainerID
            End Get
            Set(value As Guid)
                _TrainerID = value
            End Set
        End Property

        ''' <summary>
        ''' Vorname des Trainers
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Vorname ist eine Pflichtangabe")>
        Public Property Vorname As String

        ''' <summary>
        ''' Nachname des Trainers
        ''' </summary>
        ''' <returns></returns>
        Public Property Nachname As String

        ''' <summary>
        ''' Spitzname des Trainers
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Print Name ist eine Pflichtangabe")>
        Public Property Spitzname As String

        ''' <summary>
        ''' Foto des Trainers
        ''' </summary>
        ''' <returns></returns>
        Public Property Foto As Byte()
            Get
                Return _Foto
            End Get
            Set(value As Byte())
                _Foto = value
            End Set
        End Property

        ''' <summary>
        '''  Gültige eMailadresse des Trainers
        ''' </summary>
        ''' <returns></returns>
        <DataAnnotations.EmailAddress(ErrorMessage:="Gültige e-Mail Adresse")>
        Public Property eMail() As String

        ''' <summary>
        ''' Ausgabe von Vor- und Nachname
        ''' </summary>
        ''' <returns></returns>
        Public Property VorUndNachname As String
            Get
                If Vorname Is Nothing Then
                    Return Nachname
                ElseIf Nachname Is Nothing Then
                    Return Vorname
                Else
                    Return String.Format("{0} {1}", Vorname, Nachname)
                End If
            End Get
            Set(value As String)

            End Set
        End Property

        ''' <summary>
        ''' Ausgabe des Trainernamens auf der Information
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AusgabeTeilnehmerInfo As String
            Get
                If _Spitzname Is Nothing And Nachname Is Nothing Then
                    Return Vorname
                ElseIf _Spitzname Is Nothing Then
                    Return $"{Vorname} {Nachname}"
                Else
                    Return Spitzname
                End If
            End Get
        End Property

        Public ReadOnly Property IstEinerGruppeZugewiesen As Boolean

        Public ReadOnly Property HatFoto As Boolean
            Get
                Return _Foto IsNot Nothing AndAlso Foto.Length > 0
            End Get
        End Property

        Public Property Archivieren As Boolean

#End Region

#Region "Funktionen und Methoden"

        Public Overrides Function ToString() As String
            Return VorUndNachname
        End Function

#End Region


    End Class

End Namespace
