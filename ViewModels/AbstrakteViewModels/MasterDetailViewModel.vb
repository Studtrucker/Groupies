Imports System.Collections.ObjectModel
Imports System.ComponentModel

Public MustInherit Class MasterDetailViewModel(Of T)
    Inherits ViewModelWindow

    Public Property Items As ObservableCollection(Of T)
    Public Property ItemsView As ListCollectionView



    'Private _selectedItem As T
    Public Property SelectedItem As T
        Get
            Return ItemsView.CurrentItem
        End Get
        Set(value As T)
            If Not Equals(ItemsView.CurrentItem, value) Then
                '_selectedItem = value
                'OnPropertyChanged(NameOf(SelectedItem))
                OnPropertyChanged(ItemsView.MoveCurrentTo(value))
            End If
        End Set
    End Property

    Public Sub New()
        Items = New ObservableCollection(Of T)()

    End Sub


End Class