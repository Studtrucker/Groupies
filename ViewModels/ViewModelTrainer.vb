
Imports System.ComponentModel
Imports Groupies.Entities

Public Class ViewModelTrainer
    Inherits DialogViewModelBase

    Private _trainer As Trainer

    Public Property OKCommand As ICommand

    Public Event RequestClose As EventHandler(Of Boolean)

    Public Sub New()
        OKCommand = New RelayCommand(AddressOf OnOK)
    End Sub

    Private Sub OnOK(obj As Object)
        ' Businesslogik erfolgreich → Dialog schließen mit OK
        RaiseEvent RequestClose(Me, True)
    End Sub


End Class
