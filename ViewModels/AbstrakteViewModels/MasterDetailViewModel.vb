Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
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
    Private ReadOnly _items As ObservableCollection(Of T)
    Private _ItemsView As ICollectionView
#End Region



#Region "Konstruktoren"
    Public Sub New()
        MyBase.New
        Items = New ObservableCollection(Of T)()
        ItemsView = New ListCollectionView(Items)

        MoveNextCommand = New RelayCommand(Of T)(Sub() OnMoveNext(), Function() CanMoveNext)
        MovePreviousCommand = New RelayCommand(Of T)(Sub() OnMovePrevious(), Function() CanMovePrevious)
        LoeschenCommand = New RelayCommand(Of T)(AddressOf OnLoeschen, Function() CanLoeschen)
        'NeuCommand = New RelayCommand(Of T)(AddressOf OnNeu)
    End Sub

#End Region

#Region "Events"

#End Region

#Region "Properties"
    Friend Property CanBearbeiten() As Boolean
        Get
            Return SelectedItem IsNot Nothing
        End Get
        Set(value As Boolean)
            OnPropertyChanged(NameOf(CanBearbeiten))
        End Set
    End Property

    Friend Property CanNeu() As Boolean
        Get
            Return True
        End Get
        Set(value As Boolean)
            OnPropertyChanged(NameOf(CanNeu))
        End Set
    End Property

    Public ReadOnly Property AktuelleAnzahlObjekte As String
        Get
            Return $"Aktuelle Anzahl in der Liste: {Items.Count}"
        End Get
    End Property

    Public Property Items As ObservableCollection(Of T)
        Get
            Return ItemsView.SourceCollection
        End Get
        Set(value As ObservableCollection(Of T))
            If Not Equals(_items, value) Then
                ItemsView = New ListCollectionView(value)
                OnPropertyChanged(NameOf(Items))
                OnPropertyChanged(NameOf(AktuelleAnzahlObjekte))
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
            OnPropertyChanged(NameOf(MoveNextCommand))
        End Set
    End Property

    Public Property CanMovePrevious As Boolean
        Get
            Return ItemsView.CurrentPosition > 0
        End Get
        Set(value As Boolean)
            OnPropertyChanged(NameOf(CanMovePrevious))
        End Set
    End Property

    Public Property CanLoeschen As Boolean
        Get
            Return ItemsView.CurrentItem IsNot Nothing AndAlso Items.Count > 1
        End Get
        Set(value As Boolean)
            OnPropertyChanged(NameOf(CanLoeschen))
        End Set
    End Property

    Public Property SelectedItem As T
        Get
            Return _selectedItem
        End Get
        Set(value As T)
            If Not Equals(_selectedItem, value) Then
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
    Public Property LoeschenCommand As RelayCommand(Of T)
    'Public Property NeuCommand As RelayCommand(Of T)


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

    Public Sub OnLoeschen()
        Items.Remove(SelectedItem)
        MoveNextCommand.RaiseCanExecuteChanged()
        MovePreviousCommand.RaiseCanExecuteChanged()
    End Sub

    Friend Overloads Sub OnNeu()
        ' Diese Methode kann in abgeleiteten Klassen überschrieben werden, um die Logik für den Neu-Button zu implementieren.
        MoveNextCommand.RaiseCanExecuteChanged()
        MovePreviousCommand.RaiseCanExecuteChanged()
    End Sub

    Friend Sub OnLoaded()
        AddHandler Items.CollectionChanged, AddressOf OnCollectionChanged
    End Sub

    Public Sub OnCollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
        OnPropertyChanged(NameOf(AktuelleAnzahlObjekte))
    End Sub
#End Region

End Class