Imports Groupies.Controller
Imports Groupies.Entities

Public Class FaehigkeitViewModel
    Inherits MasterDetailViewModel(Of Faehigkeit)
    Implements IViewModelSpecial

#Region "Felder"
    Private _Faehigkeit As Faehigkeit
#End Region

#Region "Events"
    Public Event ModelChangedEvent As EventHandler(Of Boolean) Implements IViewModelSpecial.ModelChangedEvent
#End Region

#Region "Konstruktor"
    ''' <summary>
    ''' Parameterloser Konstruktor für den FaehigkeitViewModel.
    ''' Die Instanz benötigt den Modus und ein Faehigkeit-Objekt.
    ''' Der Datentyp und das UserControl werden automatisch gesetzt.
    ''' </summary>
    Public Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen

        'OkCommand = New RelayCommand(Of Faehigkeit)(AddressOf OnOk, Function() IstEingabeGueltig)
        DataGridSortingCommand = New RelayCommand(Of DataGridSortingEventArgs)(AddressOf MyBase.OnDataGridSorting)
        NeuCommand = New RelayCommand(Of Einteilung)(AddressOf OnNeu, Function() CanNeu)
        BearbeitenCommand = New RelayCommand(Of Einteilung)(AddressOf OnBearbeiten, Function() CanBearbeiten)
    End Sub

#End Region

#Region "Properties"

    Public Property Faehigkeit As IModel Implements IViewModelSpecial.Model
        Get
            Return _Faehigkeit
        End Get
        Set(value As IModel)
            _Faehigkeit = value
        End Set
    End Property

    Public Property FaehigkeitID As Guid
        Get
            Return _Faehigkeit.FaehigkeitID
        End Get
        Set(value As Guid)
            _Faehigkeit.FaehigkeitID = value
            OnPropertyChanged(NameOf(FaehigkeitID))
            ValidateSortierung()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Sortierung As Integer
        Get
            Return _Faehigkeit.Sortierung
        End Get
        Set(value As Integer)
            _Faehigkeit.Sortierung = value
            OnPropertyChanged(NameOf(Sortierung))
            ValidateSortierung()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Beschreibung() As String
        Get
            Return _Faehigkeit.Beschreibung
        End Get
        Set(ByVal value As String)
            _Faehigkeit.Beschreibung = value
            OnPropertyChanged(NameOf(Beschreibung))
            ValidateBeschreibung()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Benennung As String
        Get
            Return _Faehigkeit.Benennung
        End Get
        Set(value As String)
            _Faehigkeit.Benennung = value
            OnPropertyChanged(NameOf(Benennung))
            ValidateBenennung()
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
    Public ReadOnly Property BearbeitenCommand As ICommand Implements IViewModelSpecial.BearbeitenCommand
    Public ReadOnly Property NeuCommand As ICommand Implements IViewModelSpecial.NeuCommand
#End Region

#Region "Methoden"

    Public ReadOnly Property OkCommand As ICommand

    Public Overloads Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        Validate()
    End Sub

    Public Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk

        ' Hier können Sie die Logik für den OK-Button implementieren
        _Faehigkeit.speichern()

    End Sub

    Public Sub OnNeu(obj As Object) 'Implements IViewModelSpecial.OnNeu
        ' Hier können Sie die Logik für den Neu-Button implementieren
        Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Faehigkeit),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)}

        mvw.AktuellesViewModel.Model = New Faehigkeit
        dialog.DataContext = mvw

        Dim result As Boolean = dialog.ShowDialog()

        If result = True Then
            ' Todo: Das Speichern muss im ViewModel erledigt werden
            Services.DateiService.AktuellerClub.AlleFaehigkeiten.Add(mvw.AktuellesViewModel.Model)
            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Faehigkeit).Benennung} wurde gespeichert")
        End If
        MoveNextCommand.RaiseCanExecuteChanged()
        MovePreviousCommand.RaiseCanExecuteChanged()
    End Sub

    Public Sub OnBearbeiten(obj As Object) 'Implements IViewModelSpecial.OnNeu


        ' Hier können Sie die Logik für den Neu-Button implementieren
        Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Faehigkeit),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Bearbeiten)}

        mvw.AktuellesViewModel.Model = New Faehigkeit(SelectedItem)
        dialog.DataContext = mvw

        Dim result As Boolean = dialog.ShowDialog()

        If result = True Then
            Dim index = Services.DateiService.AktuellerClub.AlleFaehigkeiten.IndexOf(SelectedItem)
            ' Todo: Das Speichern muss im ViewModel erledigt werden
            Services.DateiService.AktuellerClub.AlleFaehigkeiten(index) = mvw.AktuellesViewModel.Model
            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Faehigkeit).Benennung} wurde gespeichert")
        End If

    End Sub
#End Region

#Region "Validation"
    Private Sub Validate()
        If _Faehigkeit IsNot Nothing Then
            ValidateFaehigkeitID()
            ValidateBenennung()
            ValidateBeschreibung()
            ValidateSortierung()
        End If
    End Sub

    Private Sub ValidateFaehigkeitID()
        ClearErrors(NameOf(_Faehigkeit.FaehigkeitID))
        If _Faehigkeit.FaehigkeitID = Guid.Empty Then
            AddError(NameOf(_Faehigkeit.FaehigkeitID), "FaehigkeitID darf nicht leer sein.")
        End If
    End Sub

    Private Sub ValidateBenennung()
        ClearErrors(NameOf(_Faehigkeit.Benennung))
        If String.IsNullOrWhiteSpace(_Faehigkeit.Benennung) Then
            AddError(NameOf(_Faehigkeit.Benennung), "Benennung darf nicht leer sein.")
        End If
        Dim result = New ValidationRules.BenennungValidationRule().Validate(_Faehigkeit, Nothing)

        If Not result = ValidationResult.ValidResult Then
            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Faehigkeit.Benennung), result.ErrorContent.ToString())
        End If
    End Sub

    Private Sub ValidateBeschreibung()
        ClearErrors(NameOf(_Faehigkeit.Beschreibung))
        'If String.IsNullOrEmpty(_Faehigkeit.Beschreibung) Then
        '    'Fehlerbehandlung für ungültige Beschreibung
        '    MessageBox.Show("Beschreibung ist erforderlich.")
        'End If
    End Sub

    Private Sub ValidateSortierung()
        ClearErrors(NameOf(_Faehigkeit.Sortierung))

        Dim result = New ValidationRules.SortierungValidationRule().Validate(_Faehigkeit, Nothing)

        If Not result = ValidationResult.ValidResult Then
            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Faehigkeit.Sortierung), result.ErrorContent.ToString())
        End If
    End Sub

#End Region

End Class
