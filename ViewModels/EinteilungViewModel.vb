Imports System.ComponentModel
Imports Groupies.Controller
Imports Groupies.DataImport
Imports Groupies.Entities.Generation4
Imports Groupies.Services

Public Class EinteilungViewModel
    Inherits MasterDetailViewModel(Of Einteilung)
    Implements IViewModelSpecial

    ' Testkommentar

#Region "Felder"
    Private _Einteilung As Einteilung
    Private _einteilungCopyToCommand As RelayCommand(Of Einteilung)
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
        DataGridSortingCommand = New RelayCommand(Of DataGridSortingEventArgs)(AddressOf MyBase.OnDataGridSorting)
        NeuCommand = New RelayCommand(Of Einteilung)(AddressOf OnNeu, Function() CanNeu)
        BearbeitenCommand = New RelayCommand(Of Einteilung)(AddressOf OnBearbeiten, Function() CanBearbeiten)
        LoeschenCommand = New RelayCommand(Of Einteilung)(AddressOf OnLoeschen, Function() CanLoeschen())
        AddHandler Me.PropertyChanged, AddressOf OnOwnPropertyChanged
        ConfigureItemsView(Of Einteilung)(NameOf(_Einteilung.Sortierung), NameOf(_Einteilung.Benennung))
    End Sub

    Private Sub OnOwnPropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        If e.PropertyName = NameOf(SelectedItem) Then
            If _einteilungCopyToCommand IsNot Nothing Then _einteilungCopyToCommand.RaiseCanExecuteChanged()
        End If
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

    Public Property Ident As Guid
        Get
            Return _Einteilung.Ident
        End Get
        Set(value As Guid)
            _Einteilung.Ident = value
            OnPropertyChanged(NameOf(Ident))
            ValidateIdent()
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

    ''' <summary>
    ''' Command, das in Views (ContextMenu "Copy to") gebunden werden kann.
    ''' Erzeugt über TransferCommandFactory Execute/CanExecute zentral.
    ''' </summary>
    Public ReadOnly Property EinteilungCopyToCommand As RelayCommand(Of Einteilung)
        Get
            If _einteilungCopyToCommand Is Nothing Then
                _einteilungCopyToCommand = CopyCommandFactory.CreateEinteilungCopyCommand(Function() SelectedItem,
                Sub(target As Einteilung)
                    If target Is Nothing Then Return
                    ' Optional: setze Selection oder UI-Refresh entsprechend diesem ViewModel
                    SelectedItem = target
                    ' ggf. zusätzliche Aktionen, z.B. RaiseTransferCommandsCanExecute() wenn vorhanden
                End Sub)
            End If
            Return _einteilungCopyToCommand
        End Get
    End Property

    Private Overloads Property Daten As IEnumerable(Of IModel) Implements IViewModelSpecial.Daten
        Get
            Return Items
        End Get
        Set(value As IEnumerable(Of IModel))
            Items = value
            OnPropertyChanged(NameOf(Daten))
            OnPropertyChanged(NameOf(Items))
            ConfigureItemsView(Of Einteilung)(NameOf(_Einteilung.Sortierung), NameOf(_Einteilung.Benennung))
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

    ''' <summary>
    ''' Diese Methode wird aufgerufen, wenn das UserControl geladen wird.
    ''' Hier können Sie die Logik für die Initialisierung des ViewModels implementieren.
    ''' </summary>
    ''' <param name="obj">Das geladene Objekt.</param>
    ''' <remarks>Implementierung der IViewModelSpecial-Schnittstelle.</remarks>
    Public Overloads Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        If _Einteilung IsNot Nothing Then
            ValidateBenennung()
            ValidateIdent()
            ValidateSortierung()
        End If
    End Sub

    Public Overloads Sub OnNeu(obj As Object) 'Implements IViewModelSpecial.OnNeu

        Dim ES As New EinteilungService
        ES.EinteilungErstellen()

        MyBase.OnNeu()
    End Sub

    Public Sub OnBearbeiten(obj As Object) 'Implements IViewModelSpecial.OnNeu
        Dim ES As New EinteilungService
        ES.EinteilungBearbeiten(SelectedItem)
    End Sub

    Public Overloads Sub OnLoeschen(obj As Object)
        Dim einteilungToDelete = DirectCast(SelectedItem, Einteilung)
        Dim TS As New EinteilungService()
        TS.EinteilungLoeschen(einteilungToDelete)
    End Sub

    Public Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk
        ' Hier können Sie die Logik für den OK-Button implementieren
        _Einteilung.speichern()
    End Sub

    Private Overloads Property CanLoeschen() As Boolean
        Get
            ' Anzahl der Einteilungen mit Gruppenliste ermitteln
            Dim AnzahlEinteilungenMitGruppen As Integer = ServiceProvider.DateiService.AktuellerClub.Einteilungsliste.Where(Function(e) e.Gruppenliste IsNot Nothing).Where(Function(e) e.Gruppenliste.Count > 0).Count
            Dim ItemToDeleteHatGruppen As Boolean = SelectedItem IsNot Nothing AndAlso SelectedItem.Gruppenliste IsNot Nothing AndAlso SelectedItem.Gruppenliste.Count > 0
            If ItemToDeleteHatGruppen Then
                Return ServiceProvider.DateiService.AktuellerClub.Einteilungsliste.Count > 1 AndAlso AnzahlEinteilungenMitGruppen > 1
            ElseIf AnzahlEinteilungenMitGruppen = 0 Then
                Return False
            Else
                Return True
            End If
        End Get
        Set(value As Boolean)
            OnPropertyChanged(NameOf(CanLoeschen))
        End Set
    End Property

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

    Private Sub ValidateIdent()
        ClearErrors(NameOf(_Einteilung.Ident))
        If _Einteilung.Ident = Nothing Then
            AddError(NameOf(_Einteilung.Ident), "Eine Ident muss eingetragen werden.")
        End If
    End Sub

    Private Sub ValidateBenennung()
        ClearErrors(NameOf(_Einteilung.Benennung))
        If String.IsNullOrWhiteSpace(_Einteilung.Benennung) Then
            AddError(NameOf(_Einteilung.Benennung), "Benennung darf nicht leer sein.")
        End If
        Dim result = New ValidationRules.BenennungValidationRule().Validate(_Einteilung, Nothing)

        If Not result = ValidationResult.ValidResult Then
            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Einteilung.Benennung), result.ErrorContent.ToString())
        End If
    End Sub
#End Region

End Class
