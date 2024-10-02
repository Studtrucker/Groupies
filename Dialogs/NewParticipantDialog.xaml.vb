Imports System.Text
Imports System.Windows.Forms
Imports Groupies.Entities
Imports System.ComponentModel
Imports DS = Groupies.Services
Imports Groupies.Commands

Public Class NewParticipantDialog
    Public ReadOnly Property Teilnehmer() As Teilnehmer
    Private ReadOnly _instructorListCollectionView As ICollectionView
    Private ReadOnly _levelListCollectionView As ICollectionView

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Teilnehmer = New Teilnehmer
        DataContext = _Teilnehmer

        _levelListCollectionView = New CollectionView(DS.Club.Leistungsstufenliste)
        _instructorListCollectionView = New CollectionView(DS.Club.Gruppenliste)

        ParticipantLevelComboBox.ItemsSource = _levelListCollectionView
        'MemberOfGroupComboBox.ItemsSource = _instructorListCollectionView

    End Sub


    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogOk, AddressOf HandleButtonOKExecuted, AddressOf HandleButtonOKCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogCancel, AddressOf HandleButtonCancelExecuted))

        FirstNameField.Focus()

    End Sub

    Private Sub HandleButtonOKCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = Teilnehmer.IsOk
    End Sub

    Private Sub HandleButtonOKExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = True
    End Sub

    Private Sub HandleButtonCancelExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = False
    End Sub


End Class
