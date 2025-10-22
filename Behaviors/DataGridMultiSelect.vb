Imports System.Collections
Imports System.Collections.Specialized
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Threading



Namespace Behaviors

#Region "Ermöglicht die Bindung von DataGrid.SelectedItems an eine ViewModel-Collection"

    Public NotInheritable Class DataGridMultiSelect

        Private Sub New()
        End Sub

        Private Shared ReadOnly _map As New Dictionary(Of DataGrid, IList)()
        Private Shared ReadOnly _updatingFromDataGrid As New HashSet(Of DataGrid)()
        Private Shared ReadOnly _updatingFromCollection As New HashSet(Of DataGrid)()

        Public Shared ReadOnly BindableSelectedItemsProperty As DependencyProperty =
            DependencyProperty.RegisterAttached("BindableSelectedItems", GetType(IList), GetType(DataGridMultiSelect),
                                                New PropertyMetadata(Nothing, AddressOf OnBindableSelectedItemsChanged))

        Public Shared Sub SetBindableSelectedItems(d As DependencyObject, value As IList)
            d.SetValue(BindableSelectedItemsProperty, value)
        End Sub

        Public Shared Function GetBindableSelectedItems(d As DependencyObject) As IList
            Return CType(d.GetValue(BindableSelectedItemsProperty), IList)
        End Function

        Private Shared Sub OnBindableSelectedItemsChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim dg = TryCast(d, DataGrid)
            If dg Is Nothing Then Return

            ' Alte Verknüpfung entfernen
            If _map.ContainsKey(dg) Then
                RemoveHandler dg.SelectionChanged, AddressOf DataGrid_SelectionChanged
                Dim oldBound = _map(dg)
                Dim oldNotify = TryCast(oldBound, INotifyCollectionChanged)
                If oldNotify IsNot Nothing Then RemoveHandler oldNotify.CollectionChanged, AddressOf BoundCollection_CollectionChanged
                _map.Remove(dg)
            End If

            Dim bound = TryCast(e.NewValue, IList)
            If bound Is Nothing Then Return

            ' Map und Handler hinzufügen
            _map(dg) = bound
            AddHandler dg.SelectionChanged, AddressOf DataGrid_SelectionChanged
            Dim notify = TryCast(bound, INotifyCollectionChanged)
            If notify IsNot Nothing Then AddHandler notify.CollectionChanged, AddressOf BoundCollection_CollectionChanged

            ' Initiale Synchronisation: Bound -> DataGrid.SelectedItems (ohne Reentranz)
            dg.Dispatcher.BeginInvoke(Sub()
                                          Try
                                              _updatingFromCollection.Add(dg)
                                              dg.SelectedItems.Clear()
                                              For Each it In bound
                                                  dg.SelectedItems.Add(it)
                                              Next
                                          Finally
                                              _updatingFromCollection.Remove(dg)
                                          End Try
                                      End Sub, DispatcherPriority.Background)
        End Sub

        Private Shared Sub DataGrid_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
            Dim dg = DirectCast(sender, DataGrid)
            If Not _map.ContainsKey(dg) Then Return

            ' Ignore SelectionChanged events originating from inner Selectors (e.g. ComboBox inside a cell).
            ' ComboBox.SelectionChanged is a routed event and bubbles up to the DataGrid; those events carry SelectedItems
            ' that are not row objects (e.g. Leistungsstufe) and must not be treated as row selection changes.
            If e.OriginalSource IsNot Nothing Then
                If TypeOf e.OriginalSource Is System.Windows.Controls.Primitives.Selector Then
                    Return
                End If
            End If

            ' Wenn die Änderung von BoundCollection kam, ignoriere sie
            If _updatingFromCollection.Contains(dg) Then Return

            Dim bound = _map(dg)
            Dim list = TryCast(bound, IList)
            If list Is Nothing Then Return

            Try
                _updatingFromDataGrid.Add(dg)

                ' Minimaländerungen anwenden statt clear+add
                If e.RemovedItems IsNot Nothing Then
                    For Each it In e.RemovedItems
                        If list.Contains(it) Then list.Remove(it)
                    Next
                End If

                If e.AddedItems IsNot Nothing Then
                    For Each it In e.AddedItems
                        If Not list.Contains(it) Then list.Add(it)
                    Next
                End If
            Finally
                _updatingFromDataGrid.Remove(dg)
            End Try
        End Sub

        Private Shared Sub BoundCollection_CollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
            ' Finde die DataGrid(s), die an diese Collection gebunden sind
            For Each kvp In _map
                Dim dg = kvp.Key
                Dim bound = kvp.Value
                If Not Object.ReferenceEquals(bound, sender) Then Continue For

                ' Wenn die Änderung aus DataGrid selection kam, ignoriere sie
                If _updatingFromDataGrid.Contains(dg) Then Continue For

                Try
                    _updatingFromCollection.Add(dg)

                    ' Aktualisiere SelectedItems gezielt nach Action
                    Select Case e.Action
                        Case NotifyCollectionChangedAction.Add
                            If e.NewItems IsNot Nothing Then
                                dg.Dispatcher.BeginInvoke(Sub()
                                                              For Each it In e.NewItems
                                                                  If Not dg.SelectedItems.Contains(it) Then
                                                                      dg.SelectedItems.Add(it)
                                                                  End If
                                                              Next
                                                          End Sub, DispatcherPriority.Background)
                            End If

                        Case NotifyCollectionChangedAction.Remove
                            If e.OldItems IsNot Nothing Then
                                dg.Dispatcher.BeginInvoke(Sub()
                                                              For Each it In e.OldItems
                                                                  If dg.SelectedItems.Contains(it) Then
                                                                      dg.SelectedItems.Remove(it)
                                                                  End If
                                                              Next
                                                          End Sub, DispatcherPriority.Background)
                            End If

                        Case NotifyCollectionChangedAction.Replace
                            dg.Dispatcher.BeginInvoke(Sub()
                                                          If e.OldItems IsNot Nothing Then
                                                              For Each it In e.OldItems
                                                                  If dg.SelectedItems.Contains(it) Then dg.SelectedItems.Remove(it)
                                                              Next
                                                          End If
                                                          If e.NewItems IsNot Nothing Then
                                                              For Each it In e.NewItems
                                                                  If Not dg.SelectedItems.Contains(it) Then dg.SelectedItems.Add(it)
                                                              Next
                                                          End If
                                                      End Sub, DispatcherPriority.Background)

                        Case NotifyCollectionChangedAction.Reset
                            dg.Dispatcher.BeginInvoke(Sub()
                                                          dg.SelectedItems.Clear()
                                                          Dim coll = TryCast(bound, IEnumerable)
                                                          If coll IsNot Nothing Then
                                                              For Each it In coll
                                                                  dg.SelectedItems.Add(it)
                                                              Next
                                                          End If
                                                      End Sub, DispatcherPriority.Background)

                        Case Else
                            ' Move -> meist irrelevant für Auswahl
                    End Select
                Finally
                    _updatingFromCollection.Remove(dg)
                End Try
            Next
        End Sub

    End Class

#End Region


#Region "Einfachere Variante ohne differenzierte CollectionChanged-Behandlung"

    'Public Class DataGridMultiSelect
    '    Public Shared ReadOnly BindableSelectedItemsProperty As DependencyProperty =
    '        DependencyProperty.RegisterAttached("BindableSelectedItems",
    '                                            GetType(IList),
    '                                            GetType(DataGridMultiSelect),
    '                                            New PropertyMetadata(Nothing, AddressOf OnBindableSelectedItemsChanged))

    '    Public Shared Sub SetBindableSelectedItems(obj As DependencyObject, value As IList)
    '        obj.SetValue(BindableSelectedItemsProperty, value)
    '    End Sub

    '    Public Shared Function GetBindableSelectedItems(obj As DependencyObject) As IList
    '        Return CType(obj.GetValue(BindableSelectedItemsProperty), IList)
    '    End Function

    '    Private Shared Sub OnBindableSelectedItemsChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
    '        Dim dg = TryCast(d, DataGrid)

    '        If dg IsNot Nothing Then Return
    '        '    RemoveHandler dg.SelectionChanged, AddressOf OnSelectionChanged
    '        '    AddHandler dg.SelectionChanged, AddressOf OnSelectionChanged
    '        'End If

    '        RemoveHandler dg.SelectionChanged, AddressOf DataGrid_SelectionChanged
    '        AddHandler dg.SelectionChanged, AddressOf DataGrid_SelectionChanged

    '        ' Optional: initial sync von DataGrid -> gebundene Liste
    '        Dim bound = TryCast(e.NewValue, IList)
    '        If bound IsNot Nothing Then
    '            bound.Clear()
    '            For Each it In dg.SelectedItems
    '                bound.Add(it)
    '            Next
    '        End If

    '    End Sub

    '    Private Shared Sub OnSelectionChanged(sender As Object, e As SelectionChangedEventArgs)
    '        Dim dg = TryCast(sender, DataGrid)
    '        If dg IsNot Nothing Then
    '            Dim targetList = GetBindableSelectedItems(dg)
    '            If targetList IsNot Nothing Then
    '                targetList.Clear()
    '                For Each item In dg.SelectedItems
    '                    targetList.Add(item)
    '                Next
    '            End If
    '        End If
    '    End Sub

    '    Private Shared Sub DataGrid_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
    '        Dim dg = DirectCast(sender, DataGrid)
    '        Dim bound = GetBindableSelectedItems(dg)
    '        If bound Is Nothing Then Return

    '        bound.Clear()
    '        For Each it In dg.SelectedItems
    '            bound.Add(it)
    '        Next
    '    End Sub
    'End Class
#End Region

End Namespace