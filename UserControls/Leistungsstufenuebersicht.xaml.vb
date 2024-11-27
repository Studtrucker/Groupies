Imports System.ComponentModel
Imports AppCon = Groupies.Controller.AppController

Public Class Leistungsstufenuebersicht

#Region "Felder"

    Private _LeistungsstufenCollectionView As ICollectionView


#End Region

#Region "WindowEvents"

    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs)


        ' 1. CommandBindings zur CommandBindings-Property des Window
        '    hinzufügen, um die Commands mit den entsprechenden Eventhandler zu verbinden

        CommandBindings.Add(New CommandBinding(ApplicationCommands.Delete, AddressOf Handle_Delete_Execute, AddressOf Handle_Delete_CanExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.[New], AddressOf Handle_New_Execute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Close, AddressOf Handle_Close_Execute))

        _LeistungsstufenCollectionView = New ListCollectionView(AppCon.CurrentClub.Leistungsstufenliste)
        If _LeistungsstufenCollectionView.CanSort Then
            _LeistungsstufenCollectionView.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Ascending))
        End If
        DataContext = _LeistungsstufenCollectionView

    End Sub

#End Region


#Region "EventHandler CommandBindings"

    Private Sub Handle_Delete_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = False
    End Sub

    Private Sub Handle_Delete_Execute(sender As Object, e As ExecutedRoutedEventArgs)

    End Sub

    Private Sub Handle_New_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
    End Sub

    Private Sub Handle_Close_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub Handle_New_Execute(sender As Object, e As ExecutedRoutedEventArgs)

        Dim dlg = New NeueLeistungsstufeDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        If dlg.ShowDialog = True Then
            AppCon.CurrentClub.Leistungsstufenliste.Add(dlg.Leistungsstufe)
        End If
    End Sub

#End Region

End Class
