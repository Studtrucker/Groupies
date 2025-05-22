Imports System.Collections.ObjectModel
Imports System.ComponentModel

Public MustInherit Class MasterDetailViewModel(Of T)
    Inherits ViewModelWindow

    Public Property Items As ObservableCollection(Of T)
    Private _selectedItem As T
    Public Property SelectedItem As T
        Get
            Return _selectedItem
        End Get
        Set(value As T)
            If Not Equals(_selectedItem, value) Then
                _selectedItem = value
                OnPropertyChanged(NameOf(SelectedItem))
            End If
        End Set
    End Property

    Public Sub New()
        Items = New ObservableCollection(Of T)()
    End Sub

End Class