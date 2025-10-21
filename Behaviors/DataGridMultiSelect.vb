Imports System.Windows
Imports System.Windows.Controls
Imports System.Collections

Public Class DataGridMultiSelect
    Public Shared ReadOnly BindableSelectedItemsProperty As DependencyProperty =
        DependencyProperty.RegisterAttached("BindableSelectedItems",
                                            GetType(IList),
                                            GetType(DataGridMultiSelect),
                                            New PropertyMetadata(Nothing, AddressOf OnBindableSelectedItemsChanged))

    Public Shared Sub SetBindableSelectedItems(obj As DependencyObject, value As IList)
        obj.SetValue(BindableSelectedItemsProperty, value)
    End Sub

    Public Shared Function GetBindableSelectedItems(obj As DependencyObject) As IList
        Return CType(obj.GetValue(BindableSelectedItemsProperty), IList)
    End Function

    Private Shared Sub OnBindableSelectedItemsChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim dg = TryCast(d, DataGrid)

        If dg IsNot Nothing Then Return
        '    RemoveHandler dg.SelectionChanged, AddressOf OnSelectionChanged
        '    AddHandler dg.SelectionChanged, AddressOf OnSelectionChanged
        'End If

        RemoveHandler dg.SelectionChanged, AddressOf DataGrid_SelectionChanged
        AddHandler dg.SelectionChanged, AddressOf DataGrid_SelectionChanged

        ' Optional: initial sync von DataGrid -> gebundene Liste
        Dim bound = TryCast(e.NewValue, IList)
        If bound IsNot Nothing Then
            bound.Clear()
            For Each it In dg.SelectedItems
                bound.Add(it)
            Next
        End If

    End Sub

    'Private Shared Sub OnSelectionChanged(sender As Object, e As SelectionChangedEventArgs)
    '    Dim dg = TryCast(sender, DataGrid)
    '    If dg IsNot Nothing Then
    '        Dim targetList = GetBindableSelectedItems(dg)
    '        If targetList IsNot Nothing Then
    '            targetList.Clear()
    '            For Each item In dg.SelectedItems
    '                targetList.Add(item)
    '            Next
    '        End If
    '    End If
    'End Sub

    Private Shared Sub DataGrid_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim dg = DirectCast(sender, DataGrid)
        Dim bound = GetBindableSelectedItems(dg)
        If bound Is Nothing Then Return

        bound.Clear()
        For Each it In dg.SelectedItems
            bound.Add(it)
        Next
    End Sub
End Class
