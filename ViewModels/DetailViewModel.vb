Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports Groupies.Fabriken
Imports Groupies.Interfaces

Public Class DetailViewModel
    Inherits ViewModelBase

    Public Property ObjektTypen = New ObservableCollection(Of String) From {"Trainer", "Teilnehmer", "Gruppe"}
    Private _ausgewaehlterTyp As String
    Public Property AusgewaehlterTyp() As String
        Get
            Return _ausgewaehlterTyp
        End Get
        Set(ByVal value As String)
            If _ausgewaehlterTyp <> value Then
                _ausgewaehlterTyp = value
                OnPropertyChanged(NameOf(AusgewaehlterTyp))
                AktualisiereViewModel()
            End If
            _ausgewaehlterTyp = value
        End Set
    End Property

    Private _AktuellesViewModel As Object
    Public Property AktuellesViewModel As Object
        Get
            Return _AktuellesViewModel
        End Get
        Set(value As Object)
            _AktuellesViewModel = value
            OnPropertyChanged(NameOf(AktuellesViewModel))
        End Set
    End Property

    Public Sub AktualisiereViewModel()
        Select Case AusgewaehlterTyp
            Case "Teilnehmer"
                AktuellesViewModel = New TeilnehmerViewModel()
            Case "Gruppe"
                AktuellesViewModel = New GruppeViewModel()
            Case "Trainer"
                AktuellesViewModel = New TrainerViewModel()
        End Select
    End Sub

    Private _Data As IModel

    Public Sub New()

        AusgewaehlterTyp = ObjektTypen(0)

        ' Die Commands werden hier initialisiert

        CloseCommand = New RelayCommand(Of Object)(AddressOf OnClose)
        AnsehenCommand = New RelayCommand(Of Object)(AddressOf OnAnsehen)
        NeuCommand = New RelayCommand(Of Object)(AddressOf OnNeu)
        BearbeitenCommand = New RelayCommand(Of Object)(AddressOf OnBearbeiten)
        LoeschenCommand = New RelayCommand(Of Object)(AddressOf OnLoeschen)

    End Sub


#Region "Events"

    Public Event Close As EventHandler
    Public Event Ansehen As EventHandler
    Public Event Neu As EventHandler
    Public Event Bearbeiten As EventHandler
    Public Event Loeschen As EventHandler

#End Region

#Region "Commands"
    Public Property CloseCommand As ICommand
    Public Property AnsehenCommand As ICommand
    Public Property NeuCommand As ICommand
    Public Property BearbeitenCommand As ICommand
    Public Property LoeschenCommand As ICommand

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

    'Public ReadOnly Property CurrentListUserControl As UserControl
    '    Get
    '        Return Datentyp.DatentypListUserControl
    '    End Get
    'End Property

    Public ReadOnly Property CurrentDetailUserControl As UserControl
        Get
            Return Datentyp.DatentypDetailUserControl
        End Get
    End Property

    Public Property Datentyp As IDatentyp

    Property Data As IModel
        Get
            Return _Data
        End Get
        Set(value As IModel)
            _Data = value
        End Set
    End Property


#End Region


#Region "Methoden"


    'Public Sub loadData()
    '    _Datenliste = Datentyp.getAll()
    '    _dataCV = CollectionViewSource.GetDefaultView(_Datenliste)
    '    _dataCV.MoveCurrentToFirst()
    'End Sub

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


#End Region

End Class
