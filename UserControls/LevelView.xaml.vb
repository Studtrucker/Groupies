Imports Groupies.Commands

Namespace UserControls
    Public Class LevelView
        Public Property Level

        Private Sub LevelView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            CommandBindings.Add(New CommandBinding(SkiclubCommands.NewSkill, AddressOf HandleNewSkillExecuted))
            CommandBindings.Add(New CommandBinding(SkiclubCommands.DeleteSkill, AddressOf HandleDeleteSkillExecuted, AddressOf HandleHandleDeleteSkillExecutedCanExecute))

        End Sub

        Private Sub HandleHandleDeleteSkillExecutedCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
            ' e.CanExecute = SkillsDataGrid.SelectedItems.Count > 0
            e.CanExecute = True
        End Sub

        Private Sub HandleDeleteSkillExecuted(sender As Object, e As ExecutedRoutedEventArgs)
            Throw New NotImplementedException()

        End Sub

        Private Sub HandleNewSkillExecuted(sender As Object, e As ExecutedRoutedEventArgs)
            Dim List = FindName("_levelListCollectionView")
        End Sub

        Private Sub SkillsDataGrid_AddingNewItem(sender As Object, e As AddingNewItemEventArgs)

        End Sub
    End Class
End Namespace
