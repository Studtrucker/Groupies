Imports System.ComponentModel
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Xml.Serialization
Imports Groupies.Commands
Imports Groupies.Entities
Imports CDS = Groupies.Services.CurrentDataService

Public Class Window1

#Region "Fields"
    Private _teilnehmerListCollectionView As ICollectionView
    Private _gruppenListCollectionView As ICollectionView
    Private _uebungsleiterListCollectionView As ICollectionView
#End Region

#Region "Constructor"

    Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _teilnehmerListCollectionView = New ListCollectionView(New ParticipantCollection)
        _gruppenListCollectionView = New ListCollectionView(New GroupCollection)
        _uebungsleiterListCollectionView = New ListCollectionView(New InstructorCollection)

    End Sub

#End Region

#Region "Window-Events"
    Private Sub Window1_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        ' 1. CommandBindings zur CommandBindings-Property des Window
        '    hinzufügen, um die Commands mit den entsprechenden Eventhandler zu verbinden

        setView(Groupies.Services.CurrentDataService.Skiclub)

    End Sub

#End Region

#Region "Helper-Methoden"
    Private Sub setView(Skikursliste As Skiclub)
        ' Neue ListCollectionView laden
        _teilnehmerListCollectionView = New ListCollectionView(CDS.Skiclub.Participantlist)
        ' ListCollectionView nach Teilnehmer in Gruppen filtern
        If _teilnehmerListCollectionView.CanFilter Then
            _teilnehmerListCollectionView.Filter = Function(x As Participant) x.IsNotInGroup = True
        End If
    End Sub
#End Region

    Private Sub MenuItem_Click_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub ParticipantsToDistributeDataGrid_SendByMouseDown(sender As Object, e As MouseButtonEventArgs)

    End Sub

    Private Sub ParticipantsToDistributeDataGrid_ReceiveByDrop(sender As Object, e As DragEventArgs)

    End Sub


End Class
