Imports System.Collections.ObjectModel
Imports System.ComponentModel


''' <summary>
''' Funktionalität für Master-Detail-Ansichten in ViewModels.
''' Diese Klasse ermöglicht die Anzeige und Navigation durch eine Liste von Elementen,
''' wobei jedes Element im Detail angezeigt werden kann.
''' </summary>
''' <typeparam name="T"></typeparam>
Public MustInherit Class MasterDetailViewModel(Of T)
    Inherits ViewModelWindow


#Region "Felder"
    Private _selectedItem As T
#End Region

#Region "Konstruktoren"
    Public Sub New()
        MyBase.New
        Items = New ObservableCollection(Of T)()
        MoveNextCommand = New RelayCommand(Of T)(Sub() OnMoveNext(), Function() CanMoveNext())
        MovePreviousCommand = New RelayCommand(Of T)(Sub() OnMovePrevious(), Function() CanMovePrevious)
    End Sub

#End Region

#Region "Properties"
    Public Property Items As ObservableCollection(Of T)
    Public Property MoveNextCommand As RelayCommand(Of T)
    Public Property MovePreviousCommand As RelayCommand(Of T)

    Public Property CanMoveNext() As Boolean
        Get
            Return Items.IndexOf(SelectedItem) < Items.Count - 1
        End Get
        Set(value As Boolean)
            OnPropertyChanged(NameOf(CanMoveNext))
        End Set
    End Property

    Public Property CanMovePrevious As Boolean
        Get
            Return Items.IndexOf(SelectedItem) > 0
        End Get
        Set(value As Boolean)
            OnPropertyChanged(NameOf(CanMovePrevious))
        End Set
    End Property

    Public Property SelectedItem As T
        Get
            Return _selectedItem
        End Get
        Set(value As T)
            If Not Equals(_selectedItem, value) Then
                _selectedItem = value
                OnPropertyChanged(NameOf(SelectedItem))
                MoveNextCommand.RaiseCanExecuteChanged()
                MovePreviousCommand.RaiseCanExecuteChanged()
            End If
        End Set
    End Property

#End Region

#Region "Commands"

#End Region

#Region "Methoden"
    Private Sub OnMovePrevious()
        SelectedItem = Items(Items.IndexOf(SelectedItem) - 1)

    End Sub
    Private Sub OnMoveNext()
        SelectedItem = Items(Items.IndexOf(SelectedItem) + 1)
    End Sub

#End Region

End Class