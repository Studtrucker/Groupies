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
        Private _Foto As Byte()
        Private _TrainerID As Guid
        Private _Alias As String
        Private _Vorname As String
        Private _Nachname As String
        Private _Telefonnummer As String
        Private _EMail As String
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
        ''' <param name="vorname">Der Vorname des Trainers</param>
        ''' <param name="nachname">Der Nachname des Trainers</param>
        ''' <param name="trainerAlias">Der Alias des Trainers</param>
        Public Sub New(vorname As String, nachname As String, trainerAlias As String)
            Me.New()
            _Vorname = vorname
            _Nachname = nachname
            _Alias = trainerAlias
        End Sub

        ''' <summary>
        ''' Trainer mit Angabe von Vorname, Nachname
        ''' </summary>
        ''' <param name="vorname">Der Vorname des Trainers</param>
        ''' <param name="nachname">Der Nachname des Trainers</param>
        Public Sub New(vorname As String, nachname As String)
            Me.New()
            _Vorname = vorname
            _Nachname = nachname
        End Sub

        ''' <summary>
        ''' Trainer mit Angabe von Vorname
        ''' </summary>
        ''' <param name="vorname">Der Vorname des Trainers</param>
        Public Sub New(vorname As String)
            Me.New()
            _Vorname = vorname
        End Sub

        ''' <summary>
        ''' Kopierkonstruktor für tiefes Kopieren
        ''' </summary>
        ''' <param name="origin">Das zu kopierende Trainer-Objekt</param>
        Public Sub New(origin As Trainer)
            If origin Is Nothing Then
                Throw New ArgumentNullException(NameOf(origin), "Das zu kopierende Trainer-Objekt darf nicht Nothing sein.")
            End If

            TrainerID = Guid.NewGuid() ' Neue ID für Kopie
            _Vorname = origin.Vorname
            _Nachname = origin.Nachname
            _Alias = origin.Alias
            _Telefonnummer = origin.Telefonnummer
            _EMail = origin.EMail

            ' Foto-Array kopieren (defensive copy)
            If origin.Foto IsNot Nothing Then
                _Foto = New Byte(origin.Foto.Length - 1) {}
                Array.Copy(origin.Foto, _Foto, origin.Foto.Length)
            End If
        End Sub

#End Region

#Region "Properties"

        ''' <summary>
        ''' Eindeutige Kennzeichnung
        ''' </summary>
        Public Property TrainerID As Guid Implements IModel.Ident
            Get
                Return _TrainerID
            End Get
            Set(value As Guid)
                If _TrainerID <> value Then
                    _TrainerID = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Vorname des Trainers
        ''' </summary>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Vorname ist eine Pflichtangabe")>
        Public Property Vorname As String
            Get
                Return _Vorname
            End Get
            Set(value As String)
                If _Vorname <> value Then
                    _Vorname = value
                    OnPropertyChanged()
                    OnPropertyChanged(NameOf(VorNachname))
                    OnPropertyChanged(NameOf(VorNachnameAlias))
                End If
            End Set
        End Property

        ''' <summary>
        ''' Nachname des Trainers
        ''' </summary>
        Public Property Nachname As String
            Get
                Return _Nachname
            End Get
            Set(value As String)
                If _Nachname <> value Then
                    _Nachname = value
                    OnPropertyChanged()
                    OnPropertyChanged(NameOf(VorNachname))
                    OnPropertyChanged(NameOf(VorNachnameAlias))
                End If
            End Set
        End Property

        ''' <summary>
        ''' Alias des Trainers
        ''' </summary>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Alias ist eine Pflichtangabe")>
        Public Property [Alias] As String
            Get
                Return _Alias
            End Get
            Set(value As String)
                If _Alias <> value Then
                    _Alias = value
                    OnPropertyChanged()
                    OnPropertyChanged(NameOf(VorNachnameAlias))
                End If
            End Set
        End Property

        ''' <summary>
        ''' Foto des Trainers
        ''' </summary>
        Public Property Foto As Byte()
            Get
                Return _Foto
            End Get
            Set(value As Byte())
                If Not Object.ReferenceEquals(_Foto, value) Then
                    _Foto = value
                    OnPropertyChanged()
                    OnPropertyChanged(NameOf(HatFoto))
                    OnPropertyChanged(NameOf(HatKeinFoto))
                End If
            End Set
        End Property

        ''' <summary>
        ''' Telefonnummer des Trainers
        ''' </summary>
        Public Property Telefonnummer As String
            Get
                Return _Telefonnummer
            End Get
            Set(value As String)
                If _Telefonnummer <> value Then
                    _Telefonnummer = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gültige E-Mail-Adresse des Trainers
        ''' </summary>
        <EmailAddress(ErrorMessage:="Gültige e-Mail Adresse")>
        Public Property EMail As String
            Get
                Return _EMail
            End Get
            Set(value As String)
                If _EMail <> value Then
                    _EMail = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Flag, ob der Trainer in einem Import enthalten ist
        ''' </summary>
        <XmlIgnore>
        Public Property IstImImport As Boolean

        ''' <summary>
        ''' Flag, ob der Trainer nicht in einem Import enthalten ist
        ''' </summary>
        <XmlIgnore>
        Public ReadOnly Property IstNichtImImport As Boolean
            Get
                Return Not IstImImport
            End Get
        End Property

        ''' <summary>
        ''' Flag, ob der Trainer in diesem Club neu ist
        ''' </summary>
        <XmlIgnore>
        Public Property IstNeuImClub As Boolean

        ''' <summary>
        ''' Flag, ob der Trainer ein Rückkehrer ist
        ''' </summary>
        <XmlIgnore>
        Public Property IstRueckkehrer As Boolean

        ''' <summary>
        ''' Ausgabe des Trainernamens auf der Information (Alias oder Vor-/Nachname)
        ''' </summary>
        <XmlIgnore>
        Public ReadOnly Property VorNachnameAlias As String
            Get
                Return If(String.IsNullOrWhiteSpace(_Alias), VorNachname, _Alias)
            End Get
        End Property

        ''' <summary>
        ''' Zusammengefasster Vor- und Nachname. Ist der Nachname unbekannt, wird nur der Vorname geliefert.
        ''' </summary>
        <XmlIgnore>
        Public ReadOnly Property VorNachname As String
            Get
                If String.IsNullOrWhiteSpace(_Nachname) Then
                    Return If(_Vorname, String.Empty)
                Else
                    Return $"{_Vorname} {_Nachname}".Trim()
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gibt an, ob der Trainer ein Foto hat
        ''' </summary>
        <XmlIgnore>
        Public ReadOnly Property HatFoto As Boolean
            Get
                Return _Foto IsNot Nothing AndAlso _Foto.Length > 0
            End Get
        End Property

        ''' <summary>
        ''' Gibt an, ob der Trainer kein Foto hat
        ''' </summary>
        <XmlIgnore>
        Public ReadOnly Property HatKeinFoto As Boolean
            Get
                Return Not HatFoto
            End Get
        End Property

#End Region

#Region "Funktionen und Methoden"

        ''' <summary>
        ''' Gibt den Trainer-Namen (Alias oder Vor-/Nachname) zurück
        ''' </summary>
        Public Overrides Function ToString() As String
            Return VorNachnameAlias
        End Function

        ''' <summary>
        ''' Speichert die Änderungen des Trainers
        ''' </summary>
        Public Sub speichern() Implements IModel.speichern
            ' TODO: Implementierung durch TrainerService
            MessageBox.Show($"Trainer '{VorNachnameAlias}' speichern")
        End Sub

#End Region

    End Class

End Namespace
