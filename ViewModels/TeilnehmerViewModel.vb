Imports System.ComponentModel
Imports Groupies.Controller
Imports Groupies.Entities

Public Class TeilnehmerViewModel
    Inherits MasterDetailViewModel(Of Teilnehmer)
    Implements IViewModelSpecial

#Region "Felder"
    Private _Teilnehmer As Teilnehmer
#End Region

#Region "Events"
    Public Event ModelChangedEvent As EventHandler(Of Boolean) Implements IViewModelSpecial.ModelChangedEvent
#End Region

#Region "Konstruktor"


    ''' <summary>
    ''' Parameterloser Konstruktor für den TeilnehmerViewModel.
    ''' Die Instanz benötigt den Modus und ein Teilnehmer-Objekt.
    ''' Der Datentyp und das UserControl werden automatisch gesetzt.
    ''' </summary>
    Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen

        Dim DropDown = New ListCollectionView(AppController.AktuellerClub.AlleLeistungsstufen)
        DropDown.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Ascending))
        LeistungsstufenListCollectionView = DropDown
        DataGridSortingCommand = New RelayCommand(Of DataGridSortingEventArgs)(AddressOf MyBase.OnDataGridSorting)
    End Sub

#End Region

#Region "Properties"
    Public Property LeistungsstufenListCollectionView As ICollectionView

    Public Property Model As IModel Implements IViewModelSpecial.Model
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
            ValidateTeilnehmerID()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
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
            RaiseEvent ModelChangedEvent(Me, Not HasErrors)
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
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Geburtsdatum As Date
        Get
            Return _Teilnehmer.Geburtsdatum
        End Get
        Set(value As Date)
            _Teilnehmer.Geburtsdatum = value
            OnPropertyChanged(NameOf(Geburtsdatum))
            OnPropertyChanged(NameOf(Alter))
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Telefonnummer As String
        Get
            Return _Teilnehmer.Telefonnummer
        End Get
        Set(value As String)
            _Teilnehmer.Telefonnummer = value
            OnPropertyChanged(NameOf(Telefonnummer))
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public ReadOnly Property Alter As String
        Get
            Return $"Alter: {_Teilnehmer.Alter} Jahre"
        End Get
    End Property

    Public Property Leistungsstand As Leistungsstufe
        Get
            Return _Teilnehmer.Leistungsstand
        End Get
        Set(value As Leistungsstufe)
            _Teilnehmer.Leistungsstand = value
            OnPropertyChanged(NameOf(Leistungsstand))
            ValidateLeistungsstand()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Private Overloads Property Daten As IEnumerable(Of IModel) Implements IViewModelSpecial.Daten
        Get
            Return Items
        End Get
        Set(value As IEnumerable(Of IModel))
            Items = value
            OnPropertyChanged(NameOf(Daten))
        End Set
    End Property

    Public ReadOnly Property AktuelleAnzahlObjekte As String
        Get
            Return $"Aktuelle Anzahl der Teilnehmer: {Items.ToList.Count}"
        End Get
    End Property


    Public Overloads ReadOnly Property IstEingabeGueltig As Boolean Implements IViewModelSpecial.IstEingabeGueltig
        Get
            Return MyBase.IstEingabeGueltig
        End Get
    End Property
#End Region

#Region "Command-Properties"

    Public ReadOnly Property DataGridSortingCommand As ICommand Implements IViewModelSpecial.DataGridSortingCommand

    Public ReadOnly Property UserControlLoaded As ICommand Implements IViewModelSpecial.UserControlLoaded

#End Region

#Region "Methoden"

    Public Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk
        ' Hier können Sie die Logik für den OK-Button implementieren
        _Teilnehmer.speichern()
    End Sub

    Private Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        ValidateVorname()
        ValidateNachname()
        ValidateLeistungsstand()
        ValidateTeilnehmerID()
    End Sub

#End Region

#Region "Validation"

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
        If _Teilnehmer.Leistungsstand Is Nothing OrElse _Teilnehmer.Leistungsstand.Sortierung = -1 Then
            AddError(NameOf(_Teilnehmer.Leistungsstand), "Leistungsstand muss ausgewählt werden.")
        End If
    End Sub
    Private Sub ValidateTeilnehmerID()
        ClearErrors(NameOf(_Teilnehmer.TeilnehmerID))
        If _Teilnehmer.TeilnehmerID = Nothing Then
            AddError(NameOf(_Teilnehmer.TeilnehmerID), "Eine TeilnehmerID muss eingetragen werden.")
        End If
    End Sub
#End Region

End Class

