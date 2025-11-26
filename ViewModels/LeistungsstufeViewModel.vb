Imports System.ComponentModel
Imports Groupies.Controller
Imports Groupies.DataImport
Imports Groupies.Entities.Generation4
Imports Groupies.Services

Public Class LeistungsstufeViewModel
    Inherits MasterDetailViewModel(Of Leistungsstufe)
    Implements IViewModelSpecial

#Region "Felder"
    Private _Leistungsstufe As Leistungsstufe
#End Region

#Region "Events"
    Public Event ModelChangedEvent As EventHandler(Of Boolean) Implements IViewModelSpecial.ModelChangedEvent
#End Region

#Region "Konstruktor"

    ''' <summary>
    ''' Parameterloser Konstruktor für den LeistungsstufeViewModel.
    ''' Die Instanz benötigt den Modus und ein Leistungsstufe-Objekt.
    ''' Der Datentyp und das UserControl werden automatisch gesetzt.
    ''' </summary>
    Public Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen
        OkCommand = New RelayCommand(Of Leistungsstufe)(AddressOf OnOk, Function() IstEingabeGueltig)
        DataGridSortingCommand = New RelayCommand(Of DataGridSortingEventArgs)(AddressOf MyBase.OnDataGridSorting)
        AuswahlFaehigkeiten = New ListCollectionView(ServiceProvider.DateiService.AktuellerClub.Faehigkeitenliste)
        NeuCommand = New RelayCommand(Of Einteilung)(AddressOf OnNeu, Function() CanNeu)
        BearbeitenCommand = New RelayCommand(Of Einteilung)(AddressOf OnBearbeiten, Function() CanBearbeiten)
        LoeschenCommand = New RelayCommand(Of Leistungsstufe)(AddressOf OnLoeschen, Function() CanLoeschen)
        AddHandler LeistungsstufenService.LeistungsstufeBearbeitet, AddressOf OnLeistungsstufeBearbeitet
        ConfigureItemsView(Of Leistungsstufe)(NameOf(_Leistungsstufe.Sortierung), NameOf(_Leistungsstufe.Benennung))

    End Sub

    Private Sub OnLeistungsstufeBearbeitet(sender As Object, e As EventArgs)
        OnPropertyChanged(NameOf(Items))
    End Sub


#End Region

#Region "Properties"

    Public Property AuswahlFaehigkeiten As ICollectionView

    Public Property Leistungsstufe As IModel Implements IViewModelSpecial.Model
        Get
            Return _Leistungsstufe
        End Get
        Set(value As IModel)
            _Leistungsstufe = DirectCast(value, Leistungsstufe)
        End Set
    End Property

    Public Property LeistungsstufeID() As Guid
        Get
            Return _Leistungsstufe.Ident
        End Get
        Set(ByVal value As Guid)
            _Leistungsstufe.Ident = value
            OnPropertyChanged(NameOf(LeistungsstufeID))
            ValidateLeitungsstufeID()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Sortierung() As Integer
        Get
            Return _Leistungsstufe.Sortierung
        End Get
        Set(ByVal value As Integer)
            _Leistungsstufe.Sortierung = value
            OnPropertyChanged(NameOf(Sortierung))
            ValidateSortierung()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Beschreibung() As String
        Get
            Return _Leistungsstufe.Beschreibung
        End Get
        Set(ByVal value As String)
            _Leistungsstufe.Beschreibung = value
            OnPropertyChanged(NameOf(Beschreibung))
            ValidateBeschreibung()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Benennung() As String
        Get
            Return _Leistungsstufe.Benennung
        End Get
        Set(ByVal value As String)
            _Leistungsstufe.Benennung = value
            OnPropertyChanged(NameOf(Benennung))
            ValidateBenennung()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Faehigkeiten() As FaehigkeitCollection
        Get
            Return _Leistungsstufe.Faehigkeiten
        End Get
        Set(ByVal value As FaehigkeitCollection)
            _Leistungsstufe.Faehigkeiten = value
            OnPropertyChanged(NameOf(Faehigkeiten))
            ValidateFaehigkeiten()
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
            OnPropertyChanged(NameOf(Items))
            ConfigureItemsView(Of Leistungsstufe)(NameOf(_Leistungsstufe.Sortierung), NameOf(_Leistungsstufe.Benennung))
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
    Public ReadOnly Property BearbeitenCommand As ICommand Implements IViewModelSpecial.BearbeitenCommand
    Public ReadOnly Property NeuCommand As ICommand Implements IViewModelSpecial.NeuCommand
#End Region

#Region "Methoden"

    Public Overloads Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        If _Leistungsstufe IsNot Nothing Then
            Validate()
        End If
    End Sub

    Public Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk

        ' Hier können Sie die Logik für den OK-Button implementieren
        _Leistungsstufe.speichern()

    End Sub

    Public Overloads Property SelectedItem As Leistungsstufe
        Get
            Return MyBase.SelectedItem
        End Get
        Set(value As Leistungsstufe)
            MyBase.SelectedItem = value
            OnPropertyChanged(NameOf(SelectedItem))
            OnPropertyChanged(NameOf(CanBearbeiten))
            CType(BearbeitenCommand, RelayCommand(Of Einteilung)).RaiseCanExecuteChanged()

        End Set
    End Property

    Public Overloads Sub OnNeu(obj As Object) 'Implements IViewModelSpecial.OnNeu
        ' Hier können Sie die Logik für den Neu-Button implementieren
        Dim LS As New LeistungsstufenService
        LS.LeistungsstufeErstellen()

        MoveNextCommand.RaiseCanExecuteChanged()
        MovePreviousCommand.RaiseCanExecuteChanged()
    End Sub

    Public Sub OnBearbeiten(obj As Object) 'Implements IViewModelSpecial.OnNeu

        Dim LS As New LeistungsstufenService
        LS.LeistungsstufeBearbeiten(DirectCast(SelectedItem, Leistungsstufe))
        MoveNextCommand.RaiseCanExecuteChanged()
        MovePreviousCommand.RaiseCanExecuteChanged()
    End Sub

    Public Overloads Sub OnLoeschen(obj As Object)
        Dim LS As New LeistungsstufenService
        LS.LeistungsstufeLoeschen(DirectCast(SelectedItem, Leistungsstufe))

        MoveNextCommand.RaiseCanExecuteChanged()
        MovePreviousCommand.RaiseCanExecuteChanged()
    End Sub

#End Region

#Region "Validation"
    Private Sub Validate()
        ValidateLeitungsstufeID()
        ValidateSortierung()
        ValidateBeschreibung()
        ValidateFaehigkeiten()
        ValidateBenennung()
    End Sub

    Private Sub ValidateLeitungsstufeID()
        ClearErrors(NameOf(_Leistungsstufe.Ident))
        If _Leistungsstufe.Ident = Guid.Empty Then
            AddError(NameOf(_Leistungsstufe.Ident), "Leistungsstufe ID darf nicht leer sein.")
        End If
    End Sub

    Private Sub ValidateBenennung()
        ClearErrors(NameOf(_Leistungsstufe.Benennung))

        If String.IsNullOrWhiteSpace(_Leistungsstufe.Benennung) Then
            AddError(NameOf(_Leistungsstufe.Benennung), "Benennung darf nicht leer sein.")
        End If

        Dim result = New ValidationRules.BenennungValidationRule().Validate(_Leistungsstufe, Nothing)

        If Not result = ValidationResult.ValidResult Then
            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Leistungsstufe.Benennung), result.ErrorContent.ToString())
        End If
    End Sub

    Private Sub ValidateFaehigkeiten()
        ClearErrors(NameOf(_Leistungsstufe.Faehigkeiten))
        If _Leistungsstufe.Faehigkeiten Is Nothing OrElse _Leistungsstufe.Faehigkeiten.Count = 0 Then
            'AddError(NameOf(_Leistungsstufe.Faehigkeiten), "Es müssen mindestens 1 Fähigkeit zugeordnet werden.")
        End If
    End Sub

    Private Sub ValidateBeschreibung()
        ClearErrors(NameOf(_Leistungsstufe.Beschreibung))
        If String.IsNullOrWhiteSpace(_Leistungsstufe.Beschreibung) Then
            'AddError(NameOf(_Leistungsstufe.Beschreibung), "Beschreibung darf nicht leer sein.")
        End If
    End Sub

    Private Sub ValidateSortierung()
        ClearErrors(NameOf(_Leistungsstufe.Sortierung))

        Dim result = New ValidationRules.SortierungValidationRule().Validate(_Leistungsstufe, Nothing)

        If Not result = ValidationResult.ValidResult Then
            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Leistungsstufe.Sortierung), result.ErrorContent.ToString())
        End If

    End Sub


#End Region

End Class
