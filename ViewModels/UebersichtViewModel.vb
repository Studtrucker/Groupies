Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports Groupies.Fabriken
Imports Groupies.Interfaces

Public Class UebersichtViewModel

    Public Sub New()
        CloseCommand = New RelayCommand(Of Object)(AddressOf OnClose)
        AnsehenCommand = New RelayCommand(Of Object)(AddressOf OnAnsehen)
        NeuCommand = New RelayCommand(Of Object)(AddressOf OnNeu)
        BearbeitenCommand = New RelayCommand(Of Object)(AddressOf OnBearbeiten)
        LoeschenCommand = New RelayCommand(Of Object)(AddressOf OnLoeschen)
        VorCommand = New RelayCommand(Of Object)(AddressOf OnVor)
        ZurueckCommand = New RelayCommand(Of Object)(AddressOf OnZurueck)

    End Sub

#Region "Events"

    Public Event Close As EventHandler
    Public Event Ansehen As EventHandler
    Public Event Neu As EventHandler
    Public Event Bearbeiten As EventHandler
    Public Event Loeschen As EventHandler
    Public Event Zurueck As EventHandler
    Public Event Vor As EventHandler
#End Region

#Region "Commands"
    Public Property CloseCommand As ICommand
    Public Property AnsehenCommand As ICommand
    Public Property NeuCommand As ICommand
    Public Property BearbeitenCommand As ICommand
    Public Property LoeschenCommand As ICommand
    Public Property VorCommand As ICommand
    Public Property ZurueckCommand As ICommand
#End Region

#Region "Properties"
    Public ReadOnly Property WindowHeaderImage As String
        Get
            Return Datentyp.DatentypIcon
        End Get
    End Property
    Public ReadOnly Property WindowTitleIcon As String
        Get
            Return "pack://application:,,,/Images/icons8-view-48.png"
        End Get
    End Property

    Public Property WindowHeaderText As String


    Public ReadOnly Property WindowTitleText As String
        Get
            Return $"{Datentyp.DatentypText}übersicht"
        End Get
    End Property

    Public ReadOnly Property CurrentListUserControl As UserControl
        Get
            Return Datentyp.DatentypListUserControl
        End Get
    End Property

    Public ReadOnly Property CurrentDetailUserControl As UserControl
        Get
            Return Datentyp.DatentypDetailUserControl
        End Get
    End Property
    Public Property Datentyp As IDatentyp

#End Region


#Region "Methoden"
    Private Sub OnClose()
        RaiseEvent Close(Me, EventArgs.Empty)
    End Sub

    Private Sub OnAnsehen()
        RaiseEvent Ansehen(Me, EventArgs.Empty)
    End Sub

    Private Sub OnNeu()
        RaiseEvent Neu(Me, EventArgs.Empty)
    End Sub

    Private Sub OnBearbeiten()
        RaiseEvent Bearbeiten(Me, EventArgs.Empty)
    End Sub

    Private Sub OnLoeschen()
        RaiseEvent Loeschen(Me, EventArgs.Empty)
    End Sub

    Private Sub OnVor()
        RaiseEvent Vor(Me, EventArgs.Empty)
    End Sub

    Private Sub OnZurueck()
        RaiseEvent Zurueck(Me, EventArgs.Empty)
    End Sub

#End Region

End Class
