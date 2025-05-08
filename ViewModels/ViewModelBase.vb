Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows.Input
Imports Groupies.Interfaces
Imports Groupies.UserControls


Public Class ViewModelBase
    'Implements IViewModel
    'Implements IDataErrorInfo

#Region "Events"

    'Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Event RequestClose As EventHandler(Of Boolean) 'Implements IViewModel.RequestClose

    Public Event Close As EventHandler 'Implements IViewModel.Close

#End Region

#Region "Commands"
    Public Property OkCommand As ICommand 'Implements IViewModel.OkCommand
    Public Property CancelCommand As ICommand 'Implements IViewModel.CancelCommand
    Public Property CloseCommand As ICommand 'Implements IViewModel.CloseCommand

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

    Public Property CurrentUserControl As UserControl



    Public ReadOnly Property WindowTitleText As String 'Implements IViewModel.WindowTitleText
        Get
            Return $"{Datentyp.DatentypText} {Modus.Titel}"
        End Get
    End Property

    Public ReadOnly Property WindowTitleIcon As String 'Implements IViewModel.WindowTitleIcon
        Get
            Return Modus.WindowIcon
        End Get
    End Property

    Public ReadOnly Property WindowHeaderImage As String 'Implements IViewModel.WindowHeaderImage
        Get
            Return Datentyp.DatentypIcon
        End Get
    End Property

    Public ReadOnly Property CloseButtonVisibility As Visibility 'Implements IViewModel.CloseButtonVisibility
        Get
            Return Modus.CloseButtonVisibility
        End Get
    End Property

    Public ReadOnly Property OkButtonVisibility As Visibility 'Implements IViewModel.OkButtonVisibility
        Get
            Return Modus.OkButtonVisibility
        End Get
    End Property

    Public ReadOnly Property CancelButtonVisibility As Visibility 'Implements IViewModel.CancelButtonVisibility
        Get
            Return Modus.CancelButtonVisibility
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

    'Sub OnPropertyChanged(propertyName As String)
    '    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    'End Sub

#End Region

End Class
