
Imports System.ComponentModel
Imports Groupies.Entities
Imports Groupies.Interfaces

Public Class ViewModelTrainer
    'Inherits DialogViewModelBase
    Implements IViewModel
    Implements IModus

    Property Modus As IModus
    Property Datentyp As IDatentyp

    Public Property DatenObjekt As Object Implements IViewModel.DatenObjekt

    Public Property WindowCaption As String = "Trainercaption" Implements IViewModel.WindowCaption

    Public Property WindowImage As String = "/Images/icons8-trainer-48.png" Implements IViewModel.WindowImage

    Public ReadOnly Property WindowTitle As String Implements IViewModel.WindowTitle
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
