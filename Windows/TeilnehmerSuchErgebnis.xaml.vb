Imports System.ComponentModel
Imports Groupies.Entities.Generation4
Imports Groupies.ViewModels

Public Class TeilnehmerSuchErgebnis

    Public Sub New(Suchergebnis As BindingList(Of Teilnehmer))
        InitializeComponent()
        Me.DataContext = New TeilnehmerSuchErgebnisViewModel(Suchergebnis)
    End Sub
    Public Sub New()
        InitializeComponent()
        AddHandler Me.Loaded, AddressOf OnLoaded
        AddHandler Me.Unloaded, AddressOf OnUnloaded
    End Sub

    Private Sub OnLoaded(sender As Object, e As RoutedEventArgs)
        Dim vm = TryCast(Me.DataContext, ViewModels.TeilnehmerSuchErgebnisViewModel)
        If vm IsNot Nothing Then
            AddHandler vm.RequestClose, AddressOf Vm_RequestClose
        End If
    End Sub

    Private Sub OnUnloaded(sender As Object, e As RoutedEventArgs)
        Dim vm = TryCast(Me.DataContext, ViewModels.TeilnehmerSuchErgebnisViewModel)
        If vm IsNot Nothing Then
            RemoveHandler vm.RequestClose, AddressOf Vm_RequestClose
        End If
    End Sub

    Private Sub Vm_RequestClose(sender As Object, e As EventArgs)
        ' Sicherstellen, dass im UI-Thread geschlossen wird
        If Not Me.Dispatcher.CheckAccess() Then
            Me.Dispatcher.Invoke(Sub() Me.Close())
        Else
            Me.Close()
        End If
    End Sub

End Class
