Imports System
Imports System.Linq
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media
Imports Microsoft.Xaml.Behaviors

Namespace Behaviors

    Public Class DataGridRowDoubleClickBehavior
        Inherits Behavior(Of DataGrid)

        Public Property Command As ICommand
            Get
                Return CType(GetValue(CommandProperty), ICommand)
            End Get
            Set(value As ICommand)
                SetValue(CommandProperty, value)
            End Set
        End Property

        Public Shared ReadOnly CommandProperty As DependencyProperty =
            DependencyProperty.Register(NameOf(Command), GetType(ICommand), GetType(DataGridRowDoubleClickBehavior), New PropertyMetadata(Nothing))

        Public Property CommandParameter As Object
            Get
                Return GetValue(CommandParameterProperty)
            End Get
            Set(value As Object)
                SetValue(CommandParameterProperty, value)
            End Set
        End Property

        Public Shared ReadOnly CommandParameterProperty As DependencyProperty =
            DependencyProperty.Register(NameOf(CommandParameter), GetType(Object), GetType(DataGridRowDoubleClickBehavior), New PropertyMetadata(Nothing))

        Protected Overrides Sub OnAttached()
            MyBase.OnAttached()
            If AssociatedObject IsNot Nothing Then
                AddHandler AssociatedObject.MouseDoubleClick, AddressOf OnMouseDoubleClick
            End If
        End Sub

        Protected Overrides Sub OnDetaching()
            If AssociatedObject IsNot Nothing Then
                RemoveHandler AssociatedObject.MouseDoubleClick, AddressOf OnMouseDoubleClick
            End If
            MyBase.OnDetaching()
        End Sub

        Private Sub OnMouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
            Try
                Dim dep As DependencyObject = CType(e.OriginalSource, DependencyObject)
                Dim row As DataGridRow = Nothing

                While dep IsNot Nothing AndAlso row Is Nothing
                    row = TryCast(dep, DataGridRow)
                    dep = If(row Is Nothing, VisualTreeHelper.GetParent(dep), Nothing)
                End While

                If row Is Nothing Then
                    Return
                End If

                Dim parameter = Me.CommandParameter
                If parameter Is Nothing Then
                    ' Standard: SelectedItems (falls mehrere) oder die einzelne Zeile
                    If AssociatedObject.SelectedItems IsNot Nothing AndAlso AssociatedObject.SelectedItems.Count > 0 Then
                        parameter = AssociatedObject.SelectedItems.Cast(Of Object)().ToList()
                    Else
                        parameter = row.Item
                    End If
                End If

                Dim cmd = Me.Command
                If cmd IsNot Nothing AndAlso cmd.CanExecute(parameter) Then
                    cmd.Execute(parameter)
                    e.Handled = True
                End If
            Catch
                ' still swallow exceptions to avoid breaking UI; optionally log in debug
            End Try
        End Sub

    End Class

End Namespace