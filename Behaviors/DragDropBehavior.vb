Imports Microsoft.Xaml.Behaviors

Public Class DragDropBehavior
    Inherits Behavior(Of UIElement)

    Public Shared ReadOnly DragOverCommandProperty As DependencyProperty =
        DependencyProperty.Register("DragOverCommand", GetType(ICommand), GetType(DragDropBehavior), New PropertyMetadata(Nothing))

    Public Property DragOverCommand As ICommand
        Get
            Return CType(GetValue(DragOverCommandProperty), ICommand)
        End Get
        Set(value As ICommand)
            SetValue(DragOverCommandProperty, value)
        End Set
    End Property

    Public Shared ReadOnly DropCommandProperty As DependencyProperty =
        DependencyProperty.Register("DropCommand", GetType(ICommand), GetType(DragDropBehavior), New PropertyMetadata(Nothing))

    Public Property DropCommand As ICommand
        Get
            Return CType(GetValue(DropCommandProperty), ICommand)
        End Get
        Set(value As ICommand)
            SetValue(DropCommandProperty, value)
        End Set
    End Property


    Protected Overrides Sub OnAttached()
        AddHandler AssociatedObject.DragOver, AddressOf OnDragOver
        AddHandler AssociatedObject.Drop, AddressOf OnDrop
    End Sub

    Protected Overrides Sub OnDetaching()
        RemoveHandler AssociatedObject.DragOver, AddressOf OnDragOver
        RemoveHandler AssociatedObject.Drop, AddressOf OnDrop
    End Sub

    Private Sub OnDragOver(sender As Object, e As DragEventArgs)
        If DragOverCommand IsNot Nothing AndAlso DragOverCommand.CanExecute(e) Then
            DragOverCommand.Execute(e)
        End If
    End Sub

    Private Sub OnDrop(sender As Object, e As DragEventArgs)
        If DropCommand IsNot Nothing AndAlso DropCommand.CanExecute(e) Then
            DropCommand.Execute(e)
        End If
    End Sub


End Class
