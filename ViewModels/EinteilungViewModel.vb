Imports Groupies.DataImport
Imports Groupies.Entities

Public Class EinteilungViewModel
    Inherits MasterDetailViewModel(Of Einteilung)
    Implements IViewModelSpecial

#Region "Felder"
    Private _Einteilung As Einteilung
#End Region

#Region "Events"
    Public Event ModelChangedEvent As EventHandler(Of Boolean) Implements IViewModelSpecial.ModelChangedEvent
#End Region

#Region "Konstruktor"
    ''' <summary>
    ''' Parameterloser Konstruktor für den EinteilungViewModel.
    ''' Die Instanz benötigt den Modus und ein Einteilungs-Objekt.
    ''' Der Datentyp und das UserControl werden automatisch gesetzt.
    ''' </summary>
    Public Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen
        UserControlLoaded = New RelayCommand(Of Einteilung)(AddressOf OnLoaded)
        DataGridSortingCommand = New RelayCommand(Of DataGridSortingEventArgs)(AddressOf MyBase.OnDataGridSorting)
    End Sub

#End Region

#Region "Properties"

    Public Property Einteilung As IModel Implements IViewModelSpecial.Model
        Get
            Return _Einteilung
        End Get
        Set(value As IModel)
            _Einteilung = value
        End Set
    End Property

    Public Property EinteilungID As Guid
        Get
            Return _Einteilung.EinteilungID
        End Get
        Set(value As Guid)
            _Einteilung.EinteilungID = value
            OnPropertyChanged(NameOf(EinteilungID))
            ValidateEinteilungID()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property


    Public Property Benennung As String
        Get
            Return _Einteilung.Benennung
        End Get
        Set(value As String)
            _Einteilung.Benennung = value
            OnPropertyChanged(NameOf(Benennung))
            ValidateBenennung()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property


    Public Property Sortierung As Integer
        Get
            Return _Einteilung.Sortierung
        End Get
        Set(value As Integer)
            _Einteilung.Sortierung = value
            OnPropertyChanged(NameOf(Sortierung))
            ValidateSortierung()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
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
    Public ReadOnly Property UserControlLoaded As ICommand Implements IViewModelSpecial.UserControlLoaded

#End Region

#Region "Methoden"

    ''' <summary>
    ''' Diese Methode wird aufgerufen, wenn das UserControl geladen wird.
    ''' Hier können Sie die Logik für die Initialisierung des ViewModels implementieren.
    ''' </summary>
    ''' <param name="obj">Das geladene Objekt.</param>
    ''' <remarks>Implementierung der IViewModelSpecial-Schnittstelle.</remarks>
    Public Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        ValidateBenennung()
        ValidateEinteilungID()
        ValidateSortierung()
    End Sub

    Public Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk
        ' Hier können Sie die Logik für den OK-Button implementieren
        _Einteilung.speichern()
    End Sub

#End Region

#Region "Validation"

    Private Sub ValidateSortierung()
        ClearErrors(NameOf(_Einteilung.Sortierung))

        Dim result = New ValidationRules.SortierungValidationRule().Validate(_Einteilung, Nothing)

        If Not result = ValidationResult.ValidResult Then
            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Einteilung.Sortierung), result.ErrorContent.ToString())
        End If
    End Sub

    Private Sub ValidateEinteilungID()
        ClearErrors(NameOf(_Einteilung.EinteilungID))
        If _Einteilung.EinteilungID = Nothing Then
            AddError(NameOf(_Einteilung.EinteilungID), "Eine EinteilungID muss eingetragen werden.")
        End If
    End Sub

    Private Sub ValidateBenennung()
        ClearErrors(NameOf(_Einteilung.Benennung))
        If String.IsNullOrWhiteSpace(_Einteilung.Benennung) Then
            AddError(NameOf(_Einteilung.Benennung), "Benennung darf nicht leer sein.")
        End If
    End Sub
#End Region

End Class
