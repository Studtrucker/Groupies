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

        CommandBindings.Add(New CommandBinding(Commands.SkiclubCommands.LeistungsstufeNeuErstellen, AddressOf Handle_New_Execute, AddressOf Handle_New_CanExecute))
        CommandBindings.Add(New CommandBinding(Commands.SkiclubCommands.LeistungsstufeLoeschen, AddressOf Handle_Delete_Execute, AddressOf Handle_Delete_CanExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Close, AddressOf Handle_Close_Execute))


        _LeistungsstufenCollectionView = New ListCollectionView(AppCon.CurrentClub.Leistungsstufenliste)
        If _LeistungsstufenCollectionView.CanSort Then
            _LeistungsstufenCollectionView.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Ascending))
        End If
        DataContext = _LeistungsstufenCollectionView

    End Sub

#End Region


#Region "EventHandler CommandBindings"

    Public Sub Handle_Delete_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppCon.CurrentClub.LeistungsstufeWirdNichtGenutzt(_LeistungsstufenCollectionView.CurrentItem)
    End Sub

    Public Sub Handle_Delete_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        AppCon.CurrentClub.Leistungsstufenliste.Remove(_LeistungsstufenCollectionView.CurrentItem)
    End Sub

    Public Sub Handle_New_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
    End Sub

    Public Sub Handle_Close_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        Me.Close()
    End Sub

    Public Sub Handle_New_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
    End Sub

    Public Sub Handle_New_Execute(sender As Object, e As ExecutedRoutedEventArgs)

        Dim dlg = New NeueLeistungsstufeDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        If dlg.ShowDialog = True Then
            AppCon.CurrentClub.Leistungsstufenliste.Add(dlg.Leistungsstufe)
        End If
    End Sub

#End Region

End Class
