Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports Groupies.Entities


''' <summary>
''' Funktionalität für Master-Detail-Ansichten in ViewModels.
''' Diese Klasse ermöglicht die Anzeige und Navigation durch eine Liste von Elementen,
''' wobei jedes Element im Detail angezeigt werden kann.
''' </summary>
''' <typeparam name="T"></typeparam>
Public MustInherit Class MasterDetailViewModel(Of T)
    Inherits ViewModelBase


#Region "Felder"
    Private _selectedItem As T
    Private _items As ObservableCollection(Of T)
    Private _ItemsView As ICollectionView
#End Region

#Region "Konstruktoren"
    Public Sub New()
        MyBase.New
        Items = New ObservableCollection(Of T)()
        ItemsView = New ListCollectionView(Items)

        MoveNextCommand = New RelayCommand(Of T)(Sub() OnMoveNext(), Function() CanMoveNext())
        MovePreviousCommand = New RelayCommand(Of T)(Sub() OnMovePrevious(), Function() CanMovePrevious)
    End Sub

#End Region

#Region "Events"

#End Region

#Region "Properties"
    Public Property Items As ObservableCollection(Of T)
        Get
            'Return _items
            Return ItemsView.SourceCollection
        End Get
        Set(value As ObservableCollection(Of T))
            If Not Equals(_items, value) Then
                ItemsView = New ListCollectionView(value)
                OnPropertyChanged(NameOf(Items))
            End If
        End Set
    End Property


    Public Property ItemsView As ICollectionView
        Get
            Return _ItemsView
        End Get
        Set(value As ICollectionView)
            _ItemsView = value
            OnPropertyChanged(NameOf(ItemsView))
        End Set
    End Property



    Public Property CanMoveNext() As Boolean
        Get
            Return ItemsView.CurrentPosition + 1 < Items.Count
        End Get
        Set(value As Boolean)
            OnPropertyChanged(NameOf(CanMoveNext))
        End Set
    End Property

    Public Property CanMovePrevious As Boolean
        Get
            Return ItemsView.CurrentPosition > 0
            'Return Items.IndexOf(SelectedItem) > 0
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
                ' _SelectedItem muss aktualisiert werden, damit
                _selectedItem = value
                ItemsView.MoveCurrentTo(value)
                OnPropertyChanged(NameOf(SelectedItem))
                MoveNextCommand.RaiseCanExecuteChanged()
                MovePreviousCommand.RaiseCanExecuteChanged()
            End If
        End Set
    End Property

#End Region

#Region "Commands"
    Public Property MoveNextCommand As RelayCommand(Of T)
    Public Property MovePreviousCommand As RelayCommand(Of T)

#End Region

#Region "Methoden"

    Friend Sub OnDataGridSorting(e As DataGridSortingEventArgs)
        If e Is Nothing OrElse e.Column Is Nothing Then
            Return
        End If
        Dim direction = If(e.Column.SortDirection = ListSortDirection.Ascending, ListSortDirection.Descending, ListSortDirection.Ascending)
        SortBy(e.Column.SortMemberPath, direction)
        e.Column.SortDirection = direction
        MoveNextCommand.RaiseCanExecuteChanged()
        MovePreviousCommand.RaiseCanExecuteChanged()
        e.Handled = True
    End Sub

    Private Sub SortBy(propertyName As String, direction As ListSortDirection)
        ItemsView.SortDescriptions.Clear()
        ItemsView.SortDescriptions.Add(New SortDescription(propertyName, direction))
        ItemsView.Refresh()
    End Sub

    Private Sub OnMovePrevious()
        ItemsView.MoveCurrentToPrevious()
    End Sub
    Private Sub OnMoveNext()
        ItemsView.MoveCurrentToNext()
    End Sub

#End Region

End Class