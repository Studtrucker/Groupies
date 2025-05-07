Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows.Input
Imports Groupies.Interfaces


Public Class ViewModelBase
    Implements INotifyPropertyChanged
    Implements IViewModel

#Region "Events"

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Event RequestClose As EventHandler(Of Boolean)

    Public Event Close As EventHandler
    Private Event IViewModel_RequestClose As EventHandler(Of Boolean) Implements IViewModel.RequestClose
    Private Event IViewModel_Close As EventHandler Implements IViewModel.Close
#End Region

#Region "Commands"
    Public Property OkCommand As ICommand Implements IViewModel.OkCommand
    Public Property CancelCommand As ICommand Implements IViewModel.CancelCommand
    Public Property CloseCommand As ICommand Implements IViewModel.CloseCommand

#End Region

#Region "Konstruktor"

    Public Sub New()
        OkCommand = New RelayCommand(AddressOf OnOK)
        CancelCommand = New RelayCommand(AddressOf OnCancel)
        CloseCommand = New RelayCommand(AddressOf OnClose)
    End Sub

#End Region

#Region "Properties"
    Public Property Modus As IModus
    Public Property Datentyp As IDatentyp

    Public Property DatenObjekt As Object Implements IViewModel.DatenObjekt
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property WindowTitleText As String Implements IViewModel.WindowTitleText
        Get
            Return $"{Datentyp.Datentyptext} {Modus.Titel}"
        End Get
    End Property

    Public ReadOnly Property WindowTitelIcon As String Implements IViewModel.WindowTitelIcon
        Get
            Return Modus.WindowIcon
        End Get
    End Property

    Public ReadOnly Property WindowHeaderImage As String Implements IViewModel.WindowHeaderImage
        Get
            Return Datentyp.DatentypIcon
        End Get
    End Property

    Private ReadOnly Property WindowHeaderText As String Implements IViewModel.WindowHeaderText
        Get
            Return $"{Datentyp.Datentyptext} {Modus.Titel}"
        End Get
    End Property

#End Region

#Region "Methoden"

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

    Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

#End Region

End Class
