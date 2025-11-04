Imports System.ComponentModel.DataAnnotations
Imports System.Xml.Serialization


Namespace Entities.Generation4

    ''' <summary>
    ''' Trainer, verantwortlich für eine Gruppe
    ''' </summary>
    Public Class Trainer
        Inherits BaseModel
        Implements IModel


#Region "Felder"
        'Todo:Standardfoto festlegen
        Private _Foto As Byte()
        Private _TrainerID As Guid
        Private _Alias As String
        Private _Vorname As String
        Private _Nachname As String
#End Region

#Region "Events"


#End Region

#Region "Konstruktor"

        ''' <summary>
        ''' Erstellt einen neuen Trainer
        ''' </summary>
        Public Sub New()
            TrainerID = Guid.NewGuid()
        End Sub

        ''' <summary>
        ''' Trainer mit Angabe von Vorname, Nachname und Alias
        ''' </summary>
        ''' <param name="Vorname"></param>
        ''' <param name="Nachname"></param>
        ''' <param name="[Alias]"></param>
        Public Sub New(Vorname As String, Nachname As String, [Alias] As String)
            Me.New()
            _Alias = [Alias]
            _Vorname = Vorname
            _Nachname = Nachname
        End Sub

        ''' <summary>
        ''' Trainer mit Angabe von Vorname, Nachname
        ''' </summary>
        ''' <param name="Vorname"></param>
        ''' <param name="Nachname"></param>
        Public Sub New(Vorname As String, Nachname As String)
            Me.New()
            _Vorname = Vorname
            _Nachname = Nachname
        End Sub

        ''' <summary>
        ''' Trainer mit Angabe von Vorname
        ''' </summary>
        ''' <param name="Vorname"></param>
        Public Sub New(Vorname As String)
            Me.New()
            _Vorname = Vorname
        End Sub


        ''' <summary>
        ''' Kopierkonstruktor für tiefes Kopieren
        ''' </summary>
        ''' <param name="Origin"></param>
        Public Sub New(origin As Trainer)
            _Vorname = origin.Vorname
            _Foto = origin.Foto
            _Nachname = origin.Nachname
            _Alias = origin.Alias
            _TrainerID = origin.TrainerID
            _Telefonnummer = origin.Telefonnummer
            _EMail = origin.EMail
        End Sub

#End Region

#Region "Properties"

        ''' <summary>
        ''' Eindeutige Kennzeichnung
        ''' </summary>
        ''' <returns></returns>
        Public Property TrainerID As Guid Implements IModel.Ident
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
            Get
                Return _Vorname
            End Get
            Set(value As String)
                _Vorname = value
            End Set
        End Property


        ''' <summary>
        ''' Nachname des Trainers
        ''' </summary>
        ''' <returns></returns>
        Public Property Nachname As String
            Get
                Return _Nachname
            End Get
            Set(value As String)
                _Nachname = value
            End Set
        End Property

        '
        ''' <summary>
        ''' Alias des Trainers
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Alias ist eine Pflichtangabe")>
        Public Property [Alias] As String
            Get
                Return _Alias
            End Get
            Set(value As String)
                _Alias = value
            End Set
        End Property


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
        ''' Telefonnummer des Trainers
        ''' </summary>
        ''' <returns></returns>
        Public Property Telefonnummer As String

        ''' <summary>
        ''' Flag, ob der Trainer in einem Import enthalten ist
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public Property IstImImport As Boolean

        ''' <summary>
        ''' Flag, ob der Trainer in einem Import enthalten ist
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public ReadOnly Property IstNichtImImport As Boolean
            Get
                Return Not IstImImport
            End Get
        End Property

        ''' <summary>
        ''' Flag, ob der Trainer in diesem Club neu ist
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public Property IstNeuImClub As Boolean

        ''' <summary>
        ''' Flag, ob der Trainer bekannt war
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public Property IstRueckkehrer As Boolean


        ''' <summary>
        '''  Gültige eMailadresse des Trainers
        ''' </summary>
        ''' <returns></returns>
        <EmailAddress(ErrorMessage:="Gültige e-Mail Adresse")>
        Public Property EMail As String

        ''' <summary>
        ''' Ausgabe des Trainernamens auf der Information
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property VorNachnameAlias As String
            Get
                If _Alias Is Nothing Then
                    Return VorNachname
                Else
                    Return [Alias]
                End If
            End Get
        End Property

        ''' <summary>
        ''' Zusammengefasster Vor- und Nachname. Ist der Nachname unbekannt, wird nur der Vorname geliefert.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property VorNachname As String
            Get
                If Nachname Is Nothing Then
                    Return Vorname
                Else
                    Return $"{Vorname} {Nachname}"
                End If
            End Get
        End Property

        Public ReadOnly Property HatFoto As Boolean
            Get
                Return _Foto IsNot Nothing AndAlso Foto.Length > 0
            End Get
        End Property

        Public ReadOnly Property HatKeinFoto As Boolean
            Get
                Return Not HatFoto
            End Get
        End Property

#End Region

#Region "Funktionen und Methoden"

        Public Overrides Function ToString() As String
            Return VorNachnameAlias
        End Function

        Public Sub speichern() Implements IModel.speichern
            MessageBox.Show("Speichern")
        End Sub

#End Region

    End Class

End Namespace
