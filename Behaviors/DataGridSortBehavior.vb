Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports Microsoft.Xaml.Behaviors

Public Class DataGridSortBehavior
    Inherits Behavior(Of DataGrid)

    Public Shared ReadOnly SortCommandProperty As DependencyProperty =
        DependencyProperty.Register("SortCommand", GetType(ICommand), GetType(DataGridSortBehavior), New PropertyMetadata(Nothing))

    Public Property SortCommand As ICommand
        Get
            Return CType(GetValue(SortCommandProperty), ICommand)
        End Get
        Set(value As ICommand)
            SetValue(SortCommandProperty, value)
        End Set
    End Property

    Protected Overrides Sub OnAttached()
        MyBase.OnAttached()
        AddHandler AssociatedObject.Sorting, AddressOf OnSorting
    End Sub

    Protected Overrides Sub OnDetaching()
        RemoveHandler AssociatedObject.Sorting, AddressOf OnSorting
        MyBase.OnDetaching()
    End Sub

    Private Sub OnSorting(sender As Object, e As DataGridSortingEventArgs)
        If SortCommand IsNot Nothing AndAlso SortCommand.CanExecute(e) Then
            SortCommand.Execute(e)
            e.Handled = True
        End If
    End Sub
End Class
