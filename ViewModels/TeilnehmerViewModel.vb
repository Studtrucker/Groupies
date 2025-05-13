Imports System.ComponentModel
Imports Groupies.Controller.AppController
Imports Groupies.Entities

Public Class TeilnehmerViewModel
    Inherits ViewModelBase
    Implements IViewModelSpecial

#Region "Konstruktor"
    Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen
        LeistungsstufenListCollectionView = New CollectionView(AktuellerClub.LeistungsstufenTextliste)

        OkCommand = New RelayCommand(AddressOf OnOk, Function() IstEingabeGueltig)

    End Sub

#End Region

#Region "Methoden"

    Public Overrides Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk

        ' Hier können Sie die Logik für den OK-Button implementieren
        _Teilnehmer.speichern()

        MyBase.OnOk(Me)

    End Sub

#End Region

#Region "Properties"

    Private _Teilnehmer As Teilnehmer
    Public Property Teilnehmer As IModel Implements IViewModelSpecial.Model
        Get
            Return _Teilnehmer
        End Get
        Set(value As IModel)
            '_Teilnehmer = DirectCast(value, Teilnehmer)
            _Teilnehmer = value
            'ValidateLeistungsstand()
            'AddError(NameOf(_Teilnehmer.Vorname), "Muss Name haben")
            'ValidateVorname()
        End Set
    End Property

    Public Property TeilnehmerID As Guid
        Get
            Return _Teilnehmer.TeilnehmerID
        End Get
        Set(value As Guid)
            _Teilnehmer.TeilnehmerID = value
            OnPropertyChanged()
        End Set
    End Property

    Public Property Vorname As String
        Get
            Return _Teilnehmer.Vorname
        End Get
        Set(value As String)
            _Teilnehmer.Vorname = value
            OnPropertyChanged()
            ValidateVorname()
        End Set
    End Property

    Public Property Nachname As String
        Get
            Return _Teilnehmer.Nachname
        End Get
        Set(value As String)
            _Teilnehmer.Nachname = value
            OnPropertyChanged()
            ValidateNachname()
        End Set
    End Property

    Public Property Geburtsdatum As Date
        Get
            Return _Teilnehmer.Geburtsdatum
        End Get
        Set(value As Date)
            _Teilnehmer.Geburtsdatum = value
            OnPropertyChanged()
        End Set
    End Property

    Public Property Telefonnummer As String
        Get
            Return _Teilnehmer.Telefonnummer
        End Get
        Set(value As String)
            _Teilnehmer.Telefonnummer = value
            OnPropertyChanged()
        End Set
    End Property

    Public Property Leistungsstand As Leistungsstufe
        Get
            Return _Teilnehmer.Leistungsstand
        End Get
        Set(value As Leistungsstufe)
            _Teilnehmer.Leistungsstand = value
            OnPropertyChanged()
            ValidateLeistungsstand()
        End Set
    End Property

    Public Property LeistungsstufenListCollectionView As ICollectionView

    Public Property HandleUserControlLoaded As RelayCommand

#End Region

#Region "Gültigkeitsprüfung"
    Private Sub ValidateVorname()
        ClearErrors(NameOf(_Teilnehmer.Vorname))
        If String.IsNullOrWhiteSpace(_Teilnehmer.Vorname) Then
            AddError(NameOf(_Teilnehmer.Vorname), "Vorname darf nicht leer sein.")
        End If
    End Sub

    Private Sub ValidateNachname()
        ClearErrors(NameOf(_Teilnehmer.Nachname))
        If String.IsNullOrWhiteSpace(_Teilnehmer.Nachname) Then
            AddError(NameOf(_Teilnehmer.Nachname), "Nachname darf nicht leer sein.")
        End If
    End Sub

    Private Sub ValidateLeistungsstand()
        ClearErrors(NameOf(_Teilnehmer.Leistungsstand))
        If _Teilnehmer.Leistungsstand Is Nothing Then
            AddError(NameOf(_Teilnehmer.Leistungsstand), "Leistungsstand muss ausgewählt werden.")
        End If
    End Sub
#End Region

End Class

