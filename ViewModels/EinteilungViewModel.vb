Imports Groupies.Entities

Public Class EinteilungViewModel
    Inherits MasterDetailViewModel(Of Einteilung)
    Implements IViewModelSpecial

#Region "Felder"
    Private _Einteilung As Einteilung
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
        OkCommand = New RelayCommand(Of Einteilung)(AddressOf OnOk, Function() IstEingabeGueltig)
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
        End Set
    End Property

    Public Property Benennung As String
        Get
            Return _Einteilung.Benennung
        End Get
        Set(value As String)
            _Einteilung.Benennung = value
        End Set
    End Property

    Public Property Sortierung As String
        Get
            Return _Einteilung.Sortierung
        End Get
        Set(value As String)
            _Einteilung.Sortierung = value
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
    Public ReadOnly Property DataGridSortingCommand As ICommand Implements IViewModelSpecial.DataGridSortingCommand
    Public ReadOnly Property UserControlLoaded As ICommand Implements IViewModelSpecial.UserControlLoaded
    Public ReadOnly Property OkCommand As ICommand

#End Region

#Region "Methoden"

    ''' <summary>
    ''' Diese Methode wird aufgerufen, wenn das UserControl geladen wird.
    ''' Hier können Sie die Logik für die Initialisierung des ViewModels implementieren.
    ''' </summary>
    ''' <param name="obj">Das geladene Objekt.</param>
    ''' <remarks>Implementierung der IViewModelSpecial-Schnittstelle.</remarks>
    Public Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        'Throw New NotImplementedException()
    End Sub

    Public Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk
        ' Hier können Sie die Logik für den OK-Button implementieren
        _Einteilung.speichern()
    End Sub

#End Region

#Region "Gültigkeitsprüfung"

#End Region

End Class
