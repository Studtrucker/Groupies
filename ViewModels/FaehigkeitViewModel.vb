Imports Groupies.Entities

Public Class FaehigkeitViewModel
    Inherits MasterDetailViewModel(Of Faehigkeit)
    Implements IViewModelSpecial

#Region "Felder"
    Private _Faehigkeit As Entities.Faehigkeit
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

        OkCommand = New RelayCommand(Of Faehigkeit)(AddressOf OnOk, Function() IstEingabeGueltig)
        UserControlLoaded = New RelayCommand(Of Faehigkeit)(AddressOf OnLoaded)
        DataGridSortingCommand = New RelayCommand(Of DataGridSortingEventArgs)(AddressOf MyBase.OnDataGridSorting)
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

    Public Property Sortierung As Integer
        Get
            Return _Faehigkeit.Sortierung
        End Get
        Set(value As Integer)
            _Faehigkeit.Sortierung = value
            OnPropertyChanged(NameOf(Sortierung))
            ValidateSortierung()
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

#End Region

#Region "Command-Properties"

    Public ReadOnly Property UserControlLoaded As ICommand Implements IViewModelSpecial.UserControlLoaded
    Public ReadOnly Property DataGridSortingCommand As ICommand Implements IViewModelSpecial.DataGridSortingCommand

#End Region

#Region "Methoden"

    Public ReadOnly Property OkCommand As ICommand

    Public Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        Validate()
    End Sub

    Public Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk

        ' Hier können Sie die Logik für den OK-Button implementieren
        _Faehigkeit.speichern()

    End Sub

#End Region

#Region "Gültigkeitsprüfung"
    Private Sub Validate()
        ValidateBenennung()
        ValidateBeschreibung()
        ValidateSortierung()
    End Sub

    Private Sub ValidateBenennung()
        ClearErrors(NameOf(_Faehigkeit.Benennung))
        If String.IsNullOrWhiteSpace(_Faehigkeit.Benennung) Then
            AddError(NameOf(_Faehigkeit.Benennung), "Benennung darf nicht leer sein.")
        End If
    End Sub

    Private Sub ValidateBeschreibung()
        ClearErrors(NameOf(_Faehigkeit.Beschreibung))
        If String.IsNullOrEmpty(_Faehigkeit.Beschreibung) Then
            ' Fehlerbehandlung für ungültige Beschreibung
            ' Beispiel: MessageBox.Show("Beschreibung ist erforderlich.")
        End If
    End Sub

    Private Sub ValidateSortierung()
        ClearErrors(NameOf(_Faehigkeit.Sortierung))
        If _Faehigkeit.Sortierung = 0 Then
            ' Fehlerbehandlung für ungültige Sortierung
            AddError(NameOf(_Faehigkeit.Sortierung), "Sortierung darf nicht 0 sein.")
            ' Beispiel: MessageBox.Show("Sortierung muss größer oder gleich 0 sein.")
        End If

        'todo: Es geprüft werden, ob die Sortierung bereits vorhanden ist!
    End Sub

#End Region

End Class
