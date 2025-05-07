
Imports System.ComponentModel
Imports Groupies.Entities
Imports Groupies.Interfaces

Public Class ViewModelTrainer
    Implements IViewModel

    Implements IModus

    Public Property Modus As IModus

    Public Property DatenObjekt As Object Implements IViewModel.DatenObjekt

    Public Property WindowHeaderText As String = "Trainer" Implements IViewModel.WindowHeaderText

    Public Property WindowHeaderImage As String = "/Images/icons8-trainer-48.png" Implements IViewModel.WindowHeaderImage

    Public ReadOnly Property WindowTitleText As String Implements IViewModel.WindowTitleText
        Get
            Return $"Trainer {Modus.Titel}"
        End Get
    End Property

    Public Event RequestClose As EventHandler(Of Boolean) Implements IViewModel.RequestClose

    Public Event Close As EventHandler Implements IViewModel.Close
    Public Property OkCommand As ICommand Implements IViewModel.OkCommand
    Public Property CancelCommand As ICommand Implements IViewModel.CancelCommand
    Public Property CloseCommand As ICommand Implements IViewModel.CloseCommand

    Public ReadOnly Property Titel As String Implements IModus.Titel
        Get
            Return Modus.Titel
        End Get
    End Property

    Public Property CloseButtonVisibility As Visibility Implements IModus.CloseButtonVisibility
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Visibility)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property OkButtonVisibility As Visibility Implements IModus.OkButtonVisibility
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Visibility)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property CancelButtonVisibility As Visibility Implements IModus.CancelButtonVisibility
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Visibility)
            Throw New NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property WindowIcon As String Implements IModus.WindowIcon
        Get
            Return Modus.WindowIcon
        End Get
    End Property

    Private ReadOnly Property IViewModel_WindowIcon As String Implements IViewModel.WindowTitelIcon
        Get
            Return WindowIcon
        End Get
    End Property

    Public Sub New()
        OkCommand = New RelayCommand(AddressOf OnOK)
        CancelCommand = New RelayCommand(AddressOf OnCancel)
        CloseCommand = New RelayCommand(AddressOf OnClose)
    End Sub

    Private Sub OnOK(obj As Object)
        ' Businesslogik erfolgreich → Dialog schließen mit OK
        RaiseEvent RequestClose(Me, True)
    End Sub
    Private Sub OnCancel(obj As Object)
        ' Businesslogik nicht erfolgreich → Dialog schließen mit Cancel
        RaiseEvent RequestClose(Me, False)
    End Sub
    Private Sub OnClose(obj As Object)
        ' Businesslogik nicht erfolgreich → Dialog schließen mit Cancel
        RaiseEvent Close(Me, EventArgs.Empty)
    End Sub

End Class
