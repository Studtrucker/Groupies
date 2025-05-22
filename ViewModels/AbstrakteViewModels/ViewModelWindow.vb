Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows.Input
Imports Groupies.Interfaces
Imports Groupies.UserControls


Public MustInherit Class ViewModelWindow
    Inherits ViewModelBase

#Region "Konstruktor"

    Public Sub New()
        CancelCommand = New RelayCommand(Of Object)(AddressOf OnCancel)
        CloseCommand = New RelayCommand(Of Object)(AddressOf OnClose)
        VorCommand = New RelayCommand(Of Object)(AddressOf OnVor, AddressOf OnVorCanExecuted)
        ZurueckCommand = New RelayCommand(Of Object)(AddressOf OnZurueck, AddressOf OnZurueckCanExecuted)
    End Sub

    Private Function OnZurueckCanExecuted(obj As Object) As Boolean
        Return True
    End Function

    Private Function OnVorCanExecuted(obj As Object) As Boolean
        Return True
    End Function

    Private Sub OnZurueck(obj As Object)
        MessageBox.Show("Zurück")
    End Sub

    Private Sub OnVor(obj As Object)
        MessageBox.Show("Vor")
    End Sub

#End Region

#Region "Events"

    Public Event RequestClose As EventHandler(Of Boolean)

    Public Event Close As EventHandler

#End Region

#Region "Commands"
    Public Property OkCommand As ICommand
    Public Property CancelCommand As ICommand
    Public Property CloseCommand As ICommand
    Public Property VorCommand As ICommand
    Public Property ZurueckCommand As ICommand

#End Region

#Region "Properties"

    ''' <summary>
    ''' Der Modus (Erstellen, Bearbeiten, Löschen)
    ''' stellt die Erscheinung und die Funktionalität des Dialogs dar.
    ''' </summary>
    ''' <remarks></remarks>
    Public Property Modus As IModus

    ''' <summary>
    ''' Der Datentyp (Trainer, Teilnehmer, Gruppe)
    ''' stellt die Datenstruktur des Dialogs dar.
    ''' </summary>
    ''' <remarks></remarks>
    Public Property Datentyp As IDatentyp

    ''' <summary>
    ''' Der aktuelle UserControl, der im Dialog angezeigt wird.
    ''' </summary>
    Public Property CurrentUserControl As UserControl


    Public ReadOnly Property WindowTitleText As String
        Get
            Return $"{Datentyp.DatentypText} {Modus.Titel}"
        End Get
    End Property

    Public ReadOnly Property WindowTitleIcon As String
        Get
            Return Modus.WindowIcon
        End Get
    End Property

    Public ReadOnly Property WindowHeaderImage As String
        Get
            Return Datentyp.DatentypIcon
        End Get
    End Property

    Public ReadOnly Property CloseButtonVisibility As Visibility
        Get
            Return Modus.CloseButtonVisibility
        End Get
    End Property

    Public ReadOnly Property OkButtonVisibility As Visibility
        Get
            Return Modus.OkButtonVisibility
        End Get
    End Property

    Public ReadOnly Property CancelButtonVisibility As Visibility
        Get
            Return Modus.CancelButtonVisibility
        End Get
    End Property

#End Region

#Region "Methoden"

    Public Overridable Sub OnOk(obj As Object)
        ' Businesslogik erfolgreich → Dialog schließen mit OK
        RaiseEvent RequestClose(Me, True)
    End Sub

    Public Sub OnCancel(obj As Object)
        ' Businesslogik nicht erfolgreich → Dialog schließen mit Cancel
        RaiseEvent RequestClose(Me, False)
    End Sub

    Private Sub OnClose(obj As Object)
        ' Fenster schließen mit Close
        RaiseEvent Close(Me, EventArgs.Empty)
    End Sub

#End Region

End Class
