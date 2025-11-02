Imports System.ComponentModel
Imports Groupies.Services
Imports Groupies.Controller
Imports Groupies.DataImport
Imports Groupies.Entities.Generation4


Public Class GruppenstammViewModel

    Inherits MasterDetailViewModel(Of Gruppenstamm)
    Implements IViewModelSpecial

#Region "Felder"
    Private _Gruppenstamm As Gruppenstamm
    Private _leistungsstufenListe As LeistungsstufeCollection
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
        'Dim DropDown = New ListCollectionView(DateiService.AktuellerClub.Leistungsstufenliste.Sortieren)
        'DropDown.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Ascending))
        'LeistungsstufenListCollectionView = DropDown
        DataGridSortingCommand = New RelayCommand(Of DataGridSortingEventArgs)(AddressOf MyBase.OnDataGridSorting)
        NeuCommand = New RelayCommand(Of Einteilung)(AddressOf OnNeu, Function() CanNeu)
        BearbeitenCommand = New RelayCommand(Of Einteilung)(AddressOf OnBearbeiten, Function() CanBearbeiten)
        LoeschenCommand = New RelayCommand(Of Gruppenstamm)(AddressOf OnLoeschen, Function() CanLoeschen)
        'AddHandler Me.PropertyChanged, AddressOf OnOwnPropertyChanged

    End Sub
    'Private Sub OnOwnPropertyChanged(sender As Object, e As PropertyChangedEventArgs)
    '    If e.PropertyName = NameOf(SelectedItem) Then
    '        If _gruppeTransferToCommand IsNot Nothing Then _gruppeTransferToCommand.RaiseCanExecuteChanged()
    '    End If
    'End Sub


#End Region

#Region "Properties"

    Public Property LeistungsstufenListCollectionView As ICollectionView

    Public Property Gruppenstamm As IModel Implements IViewModelSpecial.Model
        Get
            Return _Gruppenstamm
        End Get
        Set(value As IModel)
            _Gruppenstamm = value
        End Set
    End Property

    Public Property Ident As Guid
        Get
            Return _Gruppenstamm.Ident
        End Get
        Set(value As Guid)
            _Gruppenstamm.Ident = value
            OnPropertyChanged(NameOf(Ident))
            ValidateGruppenID()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Benennung As String
        Get
            Return _Gruppenstamm.Benennung
        End Get
        Set(value As String)
            _Gruppenstamm.Benennung = value
            OnPropertyChanged(NameOf(Benennung))
            ValidateBenennung()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property


    Public Property Sortierung As Integer
        Get
            Return _Gruppenstamm.Sortierung
        End Get
        Set(value As Integer)
            _Gruppenstamm.Sortierung = value
            OnPropertyChanged(NameOf(Sortierung))
            ValidateSortierung()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property LeistungsstufeID As Guid
        Get
            Return _Gruppenstamm.LeistungsstufeID
        End Get
        Set(value As Guid)
            _Gruppenstamm.LeistungsstufeID = value
            _Gruppenstamm.Leistungsstufe = DateiService.AktuellerClub.Leistungsstufenliste.FirstOrDefault(Function(L) L.Ident = value)
            OnPropertyChanged(NameOf(LeistungsstufeID))
            ValidateLeistungsstufe()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Leistungsstufe As Leistungsstufe
        Get
            Return _Gruppenstamm.Leistungsstufe
        End Get
        Set(value As Leistungsstufe)
            _Gruppenstamm.Leistungsstufe = value
            OnPropertyChanged(NameOf(Leistungsstufe))
            ValidateLeistungsstufe()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    ''' <summary>
    ''' DropDownliste für die Combobobox
    ''' </summary>
    ''' <returns></returns>
    Public Property LeistungsstufenListe As LeistungsstufeCollection
        Get
            Return _leistungsstufenListe
        End Get
        Set(value As LeistungsstufeCollection)
            _leistungsstufenListe = value
            OnPropertyChanged(NameOf(LeistungsstufenListe))
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
    Public ReadOnly Property OkCommand As ICommand
    Public ReadOnly Property BearbeitenCommand As ICommand Implements IViewModelSpecial.BearbeitenCommand
    Public ReadOnly Property NeuCommand As ICommand Implements IViewModelSpecial.NeuCommand

#End Region

#Region "Methoden"

    Public Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk
        ' Hier können Sie die Logik für den OK-Button implementieren
        _Gruppenstamm.speichern()
    End Sub


    Public Overloads Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded

        Me.LeistungsstufenListe = DateiService.AktuellerClub.Leistungsstufenliste.Sortieren

        If _Gruppenstamm IsNot Nothing Then
            ValidateGruppenID()
            ValidateBenennung()
            ValidateSortierung()
            ValidateLeistungsstufe()
        End If
    End Sub

    Public Overloads Sub OnNeu(obj As Object) 'Implements IViewModelSpecial.OnNeu
        Dim GS As New GruppenstammService
        GS.GruppenstammErstellen()

        MoveNextCommand.RaiseCanExecuteChanged()
        MovePreviousCommand.RaiseCanExecuteChanged()
    End Sub

    Public Sub OnBearbeiten(obj As Object) 'Implements IViewModelSpecial.OnNeu

        Dim GS As New GruppenstammService
        GS.GruppenstammBearbeiten(DirectCast(SelectedItem, Gruppenstamm))

        MoveNextCommand.RaiseCanExecuteChanged()
        MovePreviousCommand.RaiseCanExecuteChanged()

    End Sub

    Public Overloads Sub OnLoeschen(obj As Object)

        Dim GS As New GruppenstammService
        GS.GruppenstammLoeschen(DirectCast(SelectedItem, Gruppenstamm))

        MoveNextCommand.RaiseCanExecuteChanged()
        MovePreviousCommand.RaiseCanExecuteChanged()
    End Sub

#End Region

#Region "Validation"
    Private Sub ValidateGruppenID()
        ClearErrors(NameOf(_Gruppenstamm.Ident))
        If _Gruppenstamm.Ident = Guid.Empty Then
            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Gruppenstamm.Ident), "Eine GruppenID muss eingetragen werden.")
        End If
    End Sub


    Private Sub ValidateLeistungsstufe()
        ClearErrors(NameOf(_Gruppenstamm.LeistungsstufeID))
        If _Gruppenstamm.LeistungsstufeID = Guid.Empty OrElse _Gruppenstamm.Leistungsstufe.Sortierung < 1 Then
            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Gruppenstamm.LeistungsstufeID), "Leistungsstufe muss ausgewählt werden.")
        End If
    End Sub

    Private Sub ValidateSortierung()
        ClearErrors(NameOf(_Gruppenstamm.Sortierung))

        Dim result = New ValidationRules.SortierungValidationRule().Validate(_Gruppenstamm, Nothing)

        If Not result = ValidationResult.ValidResult Then
            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Gruppenstamm.Sortierung), result.ErrorContent.ToString())
        End If

    End Sub

    Private Sub ValidateBenennung()
        ClearErrors(NameOf(_Gruppenstamm.Benennung))
        If String.IsNullOrWhiteSpace(_Gruppenstamm.Benennung) Then
            AddError(NameOf(_Gruppenstamm.Benennung), "Benennung darf nicht leer sein.")
        End If
        Dim result = New ValidationRules.BenennungValidationRule().Validate(_Gruppenstamm, Nothing)

        If Not result = ValidationResult.ValidResult Then
            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Gruppenstamm.Benennung), result.ErrorContent.ToString())
        End If
    End Sub

#End Region

End Class
