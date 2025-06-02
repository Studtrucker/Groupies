
Public Class DataGridScrollBehavior

    Public Shared ReadOnly AutoScrollIntoViewProperty As DependencyProperty = DependencyProperty.RegisterAttached(
        "AutoScroll", GetType(Boolean), GetType(DataGridScrollBehavior), New PropertyMetadata(False, AddressOf OnAutoScrollChanged))

    Public Shared Function GetAutoScrollIntoView(ByVal obj As DependencyObject) As Boolean
        Return CType(obj.GetValue(AutoScrollIntoViewProperty), Boolean)
    End Function

    Public Shared Sub SetAutoScrollIntoView(ByVal obj As DependencyObject, ByVal value As Boolean)
        obj.SetValue(AutoScrollIntoViewProperty, value)
    End Sub

    Private Shared Sub OnAutoScrollChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim listBox As DataGrid = TryCast(d, DataGrid)
        If listBox IsNot Nothing AndAlso CBool(e.NewValue) Then
            AddHandler listBox.SelectionChanged, Sub(sender, args)
                                                     If listBox.SelectedItem IsNot Nothing Then
                                                         listBox.ScrollIntoView(listBox.SelectedItem)
                                                     End If
                                                 End Sub
        End If
    End Sub
End Class
