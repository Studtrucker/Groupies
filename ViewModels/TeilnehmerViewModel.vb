Imports System.ComponentModel
Imports Groupies.Controller.AppController
Imports Groupies.Entities
Imports Groupies.UserControls

Public Class TeilnehmerViewModel
    Inherits MasterDetailViewModel(Of Teilnehmer)
    Implements IViewModelSpecial

#Region "Konstruktor"


    ''' <summary>
    ''' Parameterloser Konstruktor für den TeilnehmerViewModel.
    ''' Die Instanz benötigt den Modus und ein Teilnehmer-Objekt.
    ''' Der Datentyp und das UserControl werden automatisch gesetzt.
    ''' </summary>
    Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen

        UserControlLoaded = New RelayCommand(Of Teilnehmer)(AddressOf OnLoaded)
        OkCommand = New RelayCommand(Of Teilnehmer)(AddressOf OnOk, Function() IstEingabeGueltig)

    End Sub

#End Region

#Region "Methoden"

    Public Overrides Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk

        ' Hier können Sie die Logik für den OK-Button implementieren
        _Teilnehmer.speichern()

        MyBase.OnOk(Me)

    End Sub

    Private Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        ValidateVorname()
        ValidateNachname()
        ValidateLeistungsstand()
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property OkCommand As ICommand

    Public Property UserControlLoaded As ICommand Implements IViewModelSpecial.UserControlLoaded

    Public Property LeistungsstufenListCollectionView As ICollectionView = New CollectionView(AktuellerClub.LeistungsstufenTextliste)


    Private _Teilnehmer As Teilnehmer
    Public Property Teilnehmer As IModel Implements IViewModelSpecial.Model
        Get
            Return _Teilnehmer
        End Get
        Set(value As IModel)
            _Teilnehmer = value
        End Set
    End Property

    Public Property TeilnehmerID As Guid
        Get
            Return _Teilnehmer.TeilnehmerID
        End Get
        Set(value As Guid)
            _Teilnehmer.TeilnehmerID = value
            OnPropertyChanged(NameOf(TeilnehmerID))
        End Set
    End Property

    Public Property Vorname As String
        Get
            Return _Teilnehmer.Vorname
        End Get
        Set(value As String)
            _Teilnehmer.Vorname = value
            OnPropertyChanged(NameOf(Vorname))
            ValidateVorname()
        End Set
    End Property

    Public Property Nachname As String
        Get
            Return _Teilnehmer.Nachname
        End Get
        Set(value As String)
            _Teilnehmer.Nachname = value
            OnPropertyChanged(NameOf(Nachname))
            ValidateNachname()
        End Set
    End Property

    Public Property Geburtsdatum As Date
        Get
            Return _Teilnehmer.Geburtsdatum
        End Get
        Set(value As Date)
            _Teilnehmer.Geburtsdatum = value
            OnPropertyChanged(NameOf(Geburtsdatum))
        End Set
    End Property

    Public Property Telefonnummer As String
        Get
            Return _Teilnehmer.Telefonnummer
        End Get
        Set(value As String)
            _Teilnehmer.Telefonnummer = value
            OnPropertyChanged(NameOf(Telefonnummer))
        End Set
    End Property

    Public Property Leistungsstand As Leistungsstufe
        Get
            Return _Teilnehmer.Leistungsstand
        End Get
        Set(value As Leistungsstufe)
            _Teilnehmer.Leistungsstand = value
            OnPropertyChanged(NameOf(Leistungsstand))
            ValidateLeistungsstand()
        End Set
    End Property


    Public Property HandleUserControlLoaded As RelayCommand(Of Object)

    Private Property Datenliste As IEnumerable(Of IModel) Implements IViewModelSpecial.Datenliste
        Get
            Return Items
        End Get
        Set(value As IEnumerable(Of IModel))
            Items = value
        End Set
    End Property

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

