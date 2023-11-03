Imports System.ComponentModel
Imports System.Text
Imports Skischule.Entities

Public Class NewGroupDialog
    Public ReadOnly Property Group() As Group
    Private _levelListCollectionView As ICollectionView
    Private _instructorListCollectionView As ICollectionView

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Group = New Group
        DataContext = _Group

        _levelListCollectionView = New ListCollectionView(DataService.CurrentDataService.Skiclub.Levellist)
        _instructorListCollectionView = New ListCollectionView(DataService.CurrentDataService.Skiclub.Instructorlist)

        GroupLeaderCombobox.ItemsSource = _instructorListCollectionView
        GroupLevelCombobox.ItemsSource = _levelListCollectionView

    End Sub


    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogOk, AddressOf HandleButtonOKExecuted, AddressOf HandleButtonOKCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogCancel, AddressOf HandleButtonCancelExecuted))

        GroupPrintNameField.Focus()

    End Sub

    Private Sub HandleButtonOKCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = Group.IsOk
    End Sub

    Private Sub HandleButtonOKExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = True
    End Sub

    Private Sub HandleButtonCancelExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = False
    End Sub


End Class
