Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports Groupies.Entities
Imports Groupies.Services
Imports PropertyChanged


''' <summary>
''' Funktionalität für Master-Detail-Ansichten in ViewModels.
''' Diese Klasse ermöglicht die Anzeige und Navigation durch eine Liste von Elementen,
''' wobei jedes Element im Detail angezeigt werden kann.
''' </summary>
''' <typeparam name="T"></typeparam>
Public MustInherit Class MasterDetailViewModel(Of T)
    Inherits ViewModelBase


#Region "Felder"
    Private ReadOnly _items As ObservableCollection(Of T)
    Private _selectedItem As T
    Private _ItemsView As ICollectionView
    Private _AlleEinteilungenCV As ICollectionView
#End Region

#Region "Konstruktoren"
    Public Sub New()
        MyBase.New
        Items = New ObservableCollection(Of T)()
        ItemsView = New ListCollectionView(Items)

        MoveNextCommand = New RelayCommand(Of T)(Sub(t) OnMoveNext(t), Function() CanMoveNext)
        MovePreviousCommand = New RelayCommand(Of T)(Sub(t) OnMovePrevious(t), Function() CanMovePrevious)
        LoeschenCommand = New RelayCommand(Of T)(AddressOf OnLoeschen, Function() CanLoeschen)
        AddHandler LeistungsstufenService.LeistungsstufeBearbeitet, AddressOf OnLeistungsstufeBearbeitet
        ' Sicher und kompatibel: DefaultView verwenden; DateiService bzw. Club kann beim Konstruktoraufruf noch Nothing sein
        Try
            If ServiceProvider.DateiService.AktuellerClub IsNot Nothing AndAlso ServiceProvider.DateiService.AktuellerClub.Einteilungsliste IsNot Nothing Then
                AlleEinteilungenCV = CollectionViewSource.GetDefaultView(ServiceProvider.DateiService.AktuellerClub.Einteilungsliste)
            Else
                ' leere View statt null → Bindings funktionieren zur Laufzeit sicher
                AlleEinteilungenCV = CollectionViewSource.GetDefaultView(New ObservableCollection(Of Object)())
            End If
        Catch ex As Exception
            AlleEinteilungenCV = CollectionViewSource.GetDefaultView(New ObservableCollection(Of Object)())
        End Try
    End Sub

    Private Sub OnLeistungsstufeBearbeitet(sender As Object, e As EventArgs)
        OnPropertyChanged(NameOf(Items))
        ItemsView.Refresh()
    End Sub

#End Region

#Region "Events"

#End Region

#Region "Properties"

    Public Property AlleEinteilungenCV As ICollectionView
        Get
            Return _AlleEinteilungenCV
        End Get
        Set(value As ICollectionView)
            _AlleEinteilungenCV = value
            OnPropertyChanged(NameOf(AlleEinteilungenCV))
        End Set
    End Property

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

    Private Sub OnMovePrevious(obj As Object)
        ItemsView.MoveCurrentToPrevious()
    End Sub

    Private Sub OnMoveNext(obj As Object)
        ItemsView.MoveCurrentToNext()
    End Sub

    Public Sub OnLoeschen(obj As Object)
        'Items.Remove(SelectedItem)
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

#Region "Hilfsmethoden"
    Friend Sub ConfigureItemsView(Of TModel)(ParamArray sortPropertyNames() As String)
        Try
            Dim view As ICollectionView = Nothing
            If ItemsView IsNot Nothing Then
                view = ItemsView
            ElseIf Items IsNot Nothing Then
                view = CollectionViewSource.GetDefaultView(Items)
                ' Falls die Basisklasse eine ItemsView-Property hat, versuchen wir, sie zu setzen
                Try
                    ItemsView = view
                Catch
                    ' ignore - ItemsView möglicherweise schreibgeschützt in Basisklasse
                End Try
            Else
                Return
            End If

            If view Is Nothing Then Return

            If view.CanSort Then
                view.SortDescriptions.Clear()
                ' Sortiere nach übergebenen Property-Namen (ascending)
                For Each propName In sortPropertyNames
                    If Not String.IsNullOrWhiteSpace(propName) Then
                        view.SortDescriptions.Add(New SortDescription(propName, ListSortDirection.Ascending))
                    End If
                Next
            End If

            ' ggf. Refresh, damit UI sofort aktualisiert wird
            If TypeOf view Is CollectionView Then
                DirectCast(view, CollectionView).Refresh()
            Else
                view.Refresh()
            End If
        Catch ex As Exception
            ' Optional: Logging
        End Try
    End Sub
#End Region

End Class