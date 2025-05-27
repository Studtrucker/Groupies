Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows.Input
Imports Groupies.Interfaces
Imports Groupies.UserControls

''' <summary>
''' Abstrakte Basisklasse für ViewModels, die in einem Fenster angezeigt werden.
''' Sie implementiert grundlegende Funktionalitäten wie Befehle und Ereignisse für das Schließen des Fensters.
''' </summary>
Public Class ViewModelWindow
    Inherits ViewModelBase

#Region "Variablen"
    Private ReadOnly _windowService As IWindowService
#End Region

#Region "Konstruktor"

    Public Sub New()
        MyBase.New()
        CancelCommand = New RelayCommand(Of Object)(AddressOf OnCancel)
        CloseCommand = New RelayCommand(Of Object)(AddressOf OnClose)
    End Sub
    Public Sub New(windowService As IWindowService)
        _windowService = windowService
        CloseCommand = New RelayCommand(Of Object)(AddressOf OnClose)
        CancelCommand = New RelayCommand(Of Object)(AddressOf OnCancel)
    End Sub


#End Region

#Region "Events"

    Public Event RequestClose As EventHandler(Of Boolean)

    Public Event Close As EventHandler

#End Region

#Region "Commands"
    Public Property CancelCommand As ICommand
    Public Property CloseCommand As ICommand

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
            If Datentyp Is Nothing OrElse Modus Is Nothing Then
                Return "Unbekannter Dialog"
            End If
            Return $"{Datentyp.DatentypText} {Modus.Titel}"
        End Get
    End Property

    Public ReadOnly Property WindowTitleIcon As String
        Get
            If Modus Is Nothing Then
                Return "Unbekannter Modus"
            End If
            Return Modus.WindowIcon
        End Get
    End Property

    Public ReadOnly Property WindowHeaderImage As String
        Get
            If Datentyp Is Nothing Then
                Return Nothing
            End If
            Return Datentyp.DatentypIcon
        End Get
    End Property

    Public ReadOnly Property CloseButtonVisibility As Visibility
        Get
            If Modus Is Nothing Then
                Return Visibility.Visible
            End If
            Return Modus.CloseButtonVisibility
        End Get
    End Property

    Public ReadOnly Property OkButtonVisibility As Visibility
        Get
            If Modus Is Nothing Then
                Return Visibility.Visible
            End If
            Return Modus.OkButtonVisibility
        End Get
    End Property

    Public ReadOnly Property CancelButtonVisibility As Visibility
        Get
            If Modus Is Nothing Then
                Return Visibility.Visible
            End If
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

    Protected Sub OnClose(obj As Object)
        ' Fenster schließen mit Close
        'RaiseEvent Close(Me, EventArgs.Empty)
        _windowService.CloseWindow()
    End Sub

#End Region

End Class
