Public Class DataGridSortBehavior

    Public Shared ReadOnly SortCommandProperty As DependencyProperty = DependencyProperty.RegisterAttached(
        "SortCommand", GetType(ICommand), GetType(DataGridSortBehavior), New PropertyMetadata(Nothing, AddressOf OnSortCommandChanged))

    Public Shared Sub SetSortCommand(element As DependencyObject, value As ICommand)
        element.SetValue(SortCommandProperty, value)
    End Sub

    Public Shared Function GetSortCommand(element As DependencyObject) As ICommand
        Return CType(element.GetValue(SortCommandProperty), ICommand)
    End Function

    Private Shared Sub OnSortCommandChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim dataGrid As DataGrid = TryCast(d, DataGrid)
        If dataGrid IsNot Nothing Then Return
        If e.NewValue IsNot Nothing Then
            AddHandler dataGrid.Sorting, AddressOf OnDataGridSorting
        Else
            RemoveHandler dataGrid.Sorting, AddressOf OnDataGridSorting
        End If
    End Sub

    Private Shared Sub OnDataGridSorting(sender As Object, e As DataGridSortingEventArgs)
        e.Handled = True
        Dim dataGrid = CType(sender, DataGrid)
        Dim command = GetSortCommand(dataGrid)
        If command IsNot Nothing AndAlso command.CanExecute(e.Column.SortMemberPath) Then
            command.Execute(e.Column.SortMemberPath)
        End If
    End Sub
End Class
