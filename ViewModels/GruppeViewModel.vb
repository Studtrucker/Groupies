Imports System.ComponentModel
Imports Groupies.Controller
Imports Groupies.DataImport
Imports Groupies.Entities

Public Class GruppeViewModel
    Inherits MasterDetailViewModel(Of Gruppe)
    Implements IViewModelSpecial

#Region "Felder"
    Private _Gruppe As Gruppe
#End Region

#Region "Events"
    Public Event ModelChangedEvent As EventHandler(Of Boolean) Implements IViewModelSpecial.ModelChangedEvent
#End Region

#Region "Konstruktor"

    ''' <summary>
    ''' Parameterloser Konstruktor für den GruppeViewModel.
    ''' Die Instanz benötigt den Modus und ein Gruppen-Objekt.
    ''' Der Datentyp und das UserControl werden automatisch gesetzt.
    ''' </summary>
    Public Sub New()
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

    Public Property Gruppe As IModel Implements IViewModelSpecial.Model
        Get
            Return _Gruppe
        End Get
        Set(value As IModel)
            _Gruppe = value
        End Set
    End Property

    Public Property GruppenID As Guid
        Get
            Return _Gruppe.GruppenID
        End Get
        Set(value As Guid)
            _Gruppe.GruppenID = value
            OnPropertyChanged(NameOf(GruppenID))
            ValidateGruppenID()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Benennung As String
        Get
            Return _Gruppe.Benennung
        End Get
        Set(value As String)
            _Gruppe.Benennung = value
            OnPropertyChanged(NameOf(Benennung))
            ValidateBenennung()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property


    Public Property Sortierung As Integer
        Get
            Return _Gruppe.Sortierung
        End Get
        Set(value As Integer)
            _Gruppe.Sortierung = value
            OnPropertyChanged(NameOf(Sortierung))
            ValidateSortierung()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Leistungsstufe As Leistungsstufe
        Get
            Return _Gruppe.Leistungsstufe
        End Get
        Set(value As Leistungsstufe)
            _Gruppe.Leistungsstufe = value
            OnPropertyChanged(NameOf(Leistungsstufe))
            ValidateLeistungsstufe()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Mitgliederliste As TeilnehmerCollection
        Get
            Return _Gruppe.Mitgliederliste
        End Get
        Set(value As TeilnehmerCollection)
            _Gruppe.Mitgliederliste = value
            OnPropertyChanged(NameOf(Mitgliederliste))
        End Set
    End Property

    Private Overloads Property Daten As IEnumerable(Of IModel) Implements IViewModelSpecial.Daten
        Get
            Return Items
        End Get
        Set(value As IEnumerable(Of IModel))
            Items = value
        End Set
    End Property

    Public Overloads ReadOnly Property IstEingabeGueltig As Boolean Implements IViewModelSpecial.IstEingabeGueltig
        Get
            Return MyBase.IstEingabeGueltig
        End Get
    End Property
#End Region

#Region "Command-Properties"
    Public ReadOnly Property DataGridSortingCommand As ICommand Implements IViewModelSpecial.DataGridSortingCommand
    Public ReadOnly Property OkCommand As ICommand
    Public Property TeilnehmerAusGruppeEntfernen As ICommand
    Public Property UserControlLoaded As ICommand Implements IViewModelSpecial.UserControlLoaded

#End Region

#Region "Methoden"

    Public Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk

        ' Hier können Sie die Logik für den OK-Button implementieren
        _Gruppe.speichern()

    End Sub


    Public Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        ValidateGruppenID()
        ValidateBenennung()
        ValidateSortierung()
        ValidateLeistungsstufe()
    End Sub


#End Region

#Region "Validation"
    Private Sub ValidateGruppenID()
        ClearErrors(NameOf(_Gruppe.GruppenID))
        If _Gruppe.GruppenID = Nothing Then
            AddError(NameOf(_Gruppe.GruppenID), "Eine GruppenID muss eingetragen werden.")
        End If
    End Sub


    Private Sub ValidateLeistungsstufe()
        ClearErrors(NameOf(_Gruppe.Leistungsstufe))
        If _Gruppe.Leistungsstufe Is Nothing OrElse _Gruppe.Leistungsstufe.Sortierung = -1 Then
            AddError(NameOf(_Gruppe.Leistungsstufe), "Leistungsstufe muss ausgewählt werden.")
        End If
    End Sub

    Private Sub ValidateSortierung()
        ClearErrors(NameOf(_Gruppe.Sortierung))

        Dim result = New ValidationRules.SortierungValidationRule().Validate(_Gruppe, Nothing)

        If Not result = ValidationResult.ValidResult Then
            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Gruppe.Sortierung), result.ErrorContent.ToString())
        End If

    End Sub

    Private Sub ValidateBenennung()
        ClearErrors(NameOf(_Gruppe.Benennung))
        If String.IsNullOrWhiteSpace(_Gruppe.Benennung) Then
            AddError(NameOf(_Gruppe.Benennung), "Benennung darf nicht leer sein.")
        End If
        Dim result = New ValidationRules.BenennungValidationRule().Validate(_Gruppe, Nothing)

        If Not result = ValidationResult.ValidResult Then
            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Gruppe.Benennung), result.ErrorContent.ToString())
        End If
    End Sub

#End Region

End Class

