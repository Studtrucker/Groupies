Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows.Input
Imports Groupies.Controller
Imports Groupies.Interfaces
Imports Groupies.UserControls
Imports Microsoft.Office.Interop.Excel

''' <summary>
''' Abstrakte Basisklasse für ViewModels, die in einem Fenster angezeigt werden.
''' Sie implementiert grundlegende Funktionalitäten wie Befehle und Ereignisse für das Schließen des Fensters.
''' </summary>
Public Class ViewModelWindow
    Inherits ViewModelBase

#Region "Variablen"
    Private ReadOnly _windowService As IWindowService
    Private _Datentyp As IDatentyp
    Private _AktuellesViewModel As IViewModelSpecial
#End Region

#Region "Konstruktor"

    Public Sub New()
        MyBase.New()
        OkCommand = New RelayCommand(Of Object)(AddressOf OnOk, AddressOf CanOK)
        CancelCommand = New RelayCommand(Of Object)(AddressOf OnCancel)
        CloseCommand = New RelayCommand(Of Object)(AddressOf OnClose)
        LoadedCommand = New RelayCommand(Of Object)(AddressOf OnLoaded)
    End Sub

    Public Sub New(windowService As IWindowService)
        MyBase.New()
        _windowService = windowService
        OkCommand = New RelayCommand(Of Object)(AddressOf OnOk, AddressOf CanOk)
        CancelCommand = New RelayCommand(Of Object)(AddressOf OnCancel)
        CloseCommand = New RelayCommand(Of Object)(AddressOf OnClose)
        LoadedCommand = New RelayCommand(Of Object)(AddressOf OnLoaded)
    End Sub


#End Region

#Region "Events"

    Public Event RequestClose As EventHandler(Of Boolean)

    Public Event Close As EventHandler

#End Region

#Region "Commands"
    Public Property CancelCommand As ICommand
    Public Property CloseCommand As ICommand
    Public Property OkCommand As ICommand
    Public Property LoadedCommand As ICommand

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
        Get
            Return _Datentyp
        End Get
        Set(value As IDatentyp)
            _Datentyp = value
            _AktuellesViewModel = _Datentyp.AktuellesUebersichtViewModel
        End Set
    End Property

    Public ReadOnly Property WindowTitleIcon As String
        Get
            If Modus IsNot Nothing Then
                Return Modus.WindowIcon
            Else
                Return "Unbekannter Modus"
            End If
        End Get
    End Property


    Public ReadOnly Property WindowTitleText As String
        Get
            If Datentyp IsNot Nothing Then
                Return Datentyp.DatentypenText
            Else
                Return "Übersicht"
            End If
        End Get
    End Property

    Public ReadOnly Property WindowHeaderImage As String
        Get
            If Datentyp IsNot Nothing Then
                Return Datentyp.DatentypIcon
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public ReadOnly Property DetailWindowHeaderText As String
        Get
            If Datentyp IsNot Nothing Then
                Return $"{Datentyp.DatentypText} {Modus.Titel}"
            Else
                Return "Detail"
            End If
        End Get
    End Property

    Public ReadOnly Property UebersichtWindowHeaderText As String
        Get
            If Datentyp IsNot Nothing Then
                Return $"Übersicht {Datentyp.DatentypenText}"
            Else
                Return "Übersicht"
            End If
        End Get
    End Property

    ''' <summary>
    ''' Das aktuelle ViewModel, das verwendet wird
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property AktuellesViewModel As IViewModelSpecial
        Get
            Return _AktuellesViewModel
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

    Private Function CanOk(param As Object) As Boolean
        Return AktuellesViewModel.IstEingabeGueltig
    End Function

    Public Overridable Sub OnOk(obj As Object)
        ' Businesslogik erfolgreich → Dialog schließen mit OK
        _windowService.DialogResult = True
    End Sub

    Public Sub OnCancel(obj As Object)
        ' Businesslogik nicht erfolgreich 
        _windowService.DialogResult = False
    End Sub

    Protected Sub OnClose(obj As Object)
        ' Fenster schließen mit Close
        _windowService.CloseWindow()
    End Sub

    Protected Sub OnLoaded(obj As Object)
        AddHandler _AktuellesViewModel.ModelChangedEvent, AddressOf OnModelChanged

        ' Fenster wurde geladen
        _windowService.SizeToContent = SizeToContent.WidthAndHeight
        AktuellesViewModel.OnLoaded(obj)
        CType(OkCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
    End Sub

    Private Sub OnModelChanged(sender As Object, e As Boolean)
        ' Objekt wurde geändert, hier können Sie Logik hinzufügen, die auf Änderungen reagiert
        ' Zum Beispiel: Aktualisieren der Ansicht oder Validierung
        CType(OkCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
    End Sub

#End Region

End Class
