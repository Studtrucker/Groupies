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
    Private _items As ObservableCollection(Of T)
#End Region

#Region "Konstruktoren"
    Public Sub New()
        MyBase.New
        Items = New ObservableCollection(Of T)()
        'ItemsView = New CollectionView(Items)
        MoveNextCommand = New RelayCommand(Of T)(Sub() OnMoveNext(), Function() CanMoveNext())
        MovePreviousCommand = New RelayCommand(Of T)(Sub() OnMovePrevious(), Function() CanMovePrevious)
    End Sub

#End Region

#Region "Events"
#End Region

#Region "Properties"
    Public Property Items As ObservableCollection(Of T)
        Get
            Return _items
        End Get
        Set(value As ObservableCollection(Of T))
            If Not Equals(_items, value) Then
                _items = value
                SelectedItem = If(value.FirstOrDefault(), Nothing)
                'ItemsView = New CollectionView(_items)
                OnPropertyChanged(NameOf(Items))
            End If
        End Set
    End Property

    'Public Property ItemsView As ICollectionView
    Public Property MoveNextCommand As RelayCommand(Of T)
    Public Property MovePreviousCommand As RelayCommand(Of T)

    Public Property CanMoveNext() As Boolean
        Get
            'Return ItemsView.CurrentPosition < Items.Count - 1
            Return Items.IndexOf(SelectedItem) < Items.Count - 1
        End Get
        Set(value As Boolean)
            OnPropertyChanged(NameOf(CanMoveNext))
        End Set
    End Property

    Public Property CanMovePrevious As Boolean
        Get
            'Return ItemsView.CurrentPosition > 0
            Return Items.IndexOf(SelectedItem) > 0
        End Get
        Set(value As Boolean)
            OnPropertyChanged(NameOf(CanMovePrevious))
        End Set
    End Property

    Public Property SelectedItem As T
        Get
            'Return ItemsView.CurrentItem
            Return _selectedItem
        End Get
        Set(value As T)
            If Not Equals(_selectedItem, value) Then
                _selectedItem = value
                'ItemsView.MoveCurrentTo(value)
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
        'ItemsView.MoveCurrentToPrevious()
        'SelectedItem = ItemsView.CurrentItem
        SelectedItem = Items(Items.IndexOf(SelectedItem) - 1)
    End Sub
    Private Sub OnMoveNext()
        'ItemsView.MoveCurrentToNext()
        'SelectedItem = ItemsView.CurrentItem
        SelectedItem = Items(Items.IndexOf(SelectedItem) + 1)
    End Sub

    Public Sub SortBy(propertyName As String, direction As ListSortDirection)
        'ItemsView.SortDescriptions.Clear()
        'ItemsView.SortDescriptions.Add(New SortDescription(propertyName, direction))
        'ItemsView.Refresh()
    End Sub
#End Region

End Class