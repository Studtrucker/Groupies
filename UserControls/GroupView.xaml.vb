Imports System.ComponentModel
Imports Groupies.Commands
Imports Groupies.Entities
Imports PropertyChanged
Imports CDS = Groupies.Controller.AppController
Namespace UserControls

    Public Class GroupView

        Public Property Group As Gruppe
        Private _levelListCollectionView As ICollectionView
        Private _instructorListCollectionView As ICollectionView
        Private _groupmemberListCollectionView As ICollectionView

        Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

        End Sub


        Private Sub GroupView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

            If CDS.CurrentClub IsNot Nothing AndAlso CDS.CurrentClub.Leistungsstufenliste IsNot Nothing Then

                _levelListCollectionView = New ListCollectionView(CDS.CurrentClub.Leistungsstufenliste.Leistungsstufen)
                If _levelListCollectionView.CanSort Then
                    _levelListCollectionView.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Ascending))
                End If
                Gruppenleistungsstufe.ItemsSource = _levelListCollectionView

                '    '*****************************************************************************************************
                '    '* Darf nicht gefiltert werden, denn sonst werden die einegteilten Instructoren nicht mehr angezeigt *
                '    '* If _instructorListCollectionView.CanFilter Then                                                   *
                '    '*     _instructorListCollectionView.Filter = Function(f As Instructor) f.IsAvailable                *
                '    '* End If                                                                                            *
                '    '*****************************************************************************************************
                '    _instructorListCollectionView = New ListCollectionView(CDS.Skiclub.Instructorlist)
                '    If _instructorListCollectionView.CanSort Then
                '        _instructorListCollectionView.SortDescriptions.Add(New SortDescription("InstructorFirstName", ListSortDirection.Ascending))
                '        _instructorListCollectionView.SortDescriptions.Add(New SortDescription("InstructorLastName", ListSortDirection.Ascending))
                '    End If
                '    'GroupLeaderCombobox.ItemsSource = _instructorListCollectionView
            End If


            CommandBindings.Add(New CommandBinding(SkiclubCommands.TeilnehmerAusGruppeEntfernen, AddressOf Handle_TeilnehmerAusGruppeEntfernen_Execute, AddressOf Handle_TeilnehmerAusGruppeEntfernen_CanExecute))
            CommandBindings.Add(New CommandBinding(SkiclubCommands.GruppentrainerEntfernen, AddressOf Handle_GruppentrainerEntfernen_Execute, AddressOf Handle_GruppentrainerEntfernen_CanExecute))

        End Sub


        Private Sub Handle_TeilnehmerAusGruppeEntfernen_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
            ' Todo: Regel einbauen: Die aktuelle Gruppe muß mehr als 0 Mitglieder haben und mindestens ein Mitglied markiert sein
            e.CanExecute = False
        End Sub

        Private Sub Handle_TeilnehmerAusGruppeEntfernen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
            MessageBox.Show("GroupView Teilnehmer raus")
        End Sub

        Private Sub Handle_GruppentrainerEntfernen_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
            ' Todo: Regel einbauen: Der Gruppe muss einen Trainer haben
            e.CanExecute = True
        End Sub

        Private Sub Handle_GruppentrainerEntfernen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
            MessageBox.Show("GroupView Trainer raus")
        End Sub

        Private Sub SetView()

            '_groupmemberListCollectionView = New ListCollectionView(DirectCast(DataContext, Group).GroupMembers)
            'If _groupmemberListCollectionView.CanSort Then
            '    _groupmemberListCollectionView.SortDescriptions.Clear()
            '    _groupmemberListCollectionView.SortDescriptions.Add(New SortDescription("ParticipantFullName", ListSortDirection.Ascending))
            'End If
            '_groupmemberListCollectionView.MoveCurrentToFirst()
            'GroupMembersDataGrid.ItemsSource = _groupmemberListCollectionView

        End Sub

        Private Sub MenuItemDeleteGroupMember_Click(sender As Object, e As RoutedEventArgs)
            For i = GroupMembersDataGrid.SelectedItems.Count - 1 To 0 Step -1
                CDS.CurrentClub.TeilnehmerAusGruppeEntfernen(GroupMembersDataGrid.SelectedItems.Item(i), DirectCast(DataContext, ICollectionView).CurrentItem)
            Next
            SetView()
        End Sub


        Private Sub MenuItem_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
            ' Teilnehmer werden aus der Gruppe entfernt und in die Gruppe GruppenloseTeilnehmer eingetragen
            For i = GroupMembersDataGrid.SelectedItems.Count - 1 To 0 Step -1
                CDS.CurrentClub.TeilnehmerAusGruppeEntfernen(GroupMembersDataGrid.SelectedItems.Item(i), DirectCast(DataContext, ICollectionView).CurrentItem)
            Next
        End Sub

        Private Sub GroupLeaderTextblock_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
            ' Binding an das Objekt und nicht an die Eigenschaft
            If DirectCast(DirectCast(DataContext, ICollectionView).CurrentItem, Gruppe).Trainer IsNot Nothing Then
                CDS.CurrentClub.TrainerAusGruppeEntfernen(DirectCast(DataContext, ICollectionView).CurrentItem)
            End If
        End Sub

    End Class
End Namespace
