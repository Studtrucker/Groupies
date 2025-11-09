Imports System.Reflection
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input

Namespace Behaviors

    Public Class ContextMenuBehaviors
        Public Shared ReadOnly EnableRequeryForCommandsProperty As DependencyProperty =
            DependencyProperty.RegisterAttached("EnableRequeryForCommands", GetType(Boolean), GetType(ContextMenuBehaviors),
                                                New PropertyMetadata(False, AddressOf OnEnableRequeryForCommandsChanged))

        Public Shared Sub SetEnableRequeryForCommands(d As DependencyObject, value As Boolean)
            d.SetValue(EnableRequeryForCommandsProperty, value)
        End Sub

        Public Shared Function GetEnableRequeryForCommands(d As DependencyObject) As Boolean
            Return CBool(d.GetValue(EnableRequeryForCommandsProperty))
        End Function

        Private Shared Sub OnEnableRequeryForCommandsChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim cm = TryCast(d, ContextMenu)
            If cm Is Nothing Then Return

            If CBool(e.NewValue) Then
                AddHandler cm.Opened, AddressOf ContextMenu_Opened
            Else
                RemoveHandler cm.Opened, AddressOf ContextMenu_Opened
            End If
        End Sub

        Private Shared Sub ContextMenu_Opened(sender As Object, e As RoutedEventArgs)
            Dim cm = TryCast(sender, ContextMenu)
            If cm Is Nothing Then Return

            For Each itemObject In GetMenuItemsRecursive(cm)
                Dim mi = TryCast(itemObject, MenuItem)
                If mi Is Nothing Then Continue For
                Dim cmd = mi.Command
                If cmd Is Nothing Then Continue For

                ' Versuche RelayCommand.RaiseCanExecuteChanged per Reflection, ansonsten Fallback
                Dim meth = cmd.GetType().GetMethod("RaiseCanExecuteChanged", BindingFlags.Instance Or BindingFlags.Public)
                If meth IsNot Nothing Then
                    Try
                        meth.Invoke(cmd, Nothing)
                    Catch
                        CommandManager.InvalidateRequerySuggested()
                    End Try
                Else
                    CommandManager.InvalidateRequerySuggested()
                End If
            Next
        End Sub

        Private Shared Iterator Function GetMenuItemsRecursive(parent As ItemsControl) As IEnumerable(Of Object)
            For Each i In parent.Items
                Yield i
                Dim mi = TryCast(i, MenuItem)
                If mi IsNot Nothing AndAlso mi.HasItems Then
                    For Each subi In GetMenuItemsRecursive(mi)
                        Yield subi
                    Next
                End If
            Next
        End Function

    End Class

End Namespace