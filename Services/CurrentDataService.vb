Imports System.Collections.ObjectModel
Imports System.Windows.Shell
Imports Groupies.Entities

Namespace Services

    ' Hier werden alle Daten der aktuellen Groupies Datei gesammelt
    Module CurrentDataService

        Public Property Skiclub As Club

        Public ReadOnly Property availableInstructors As InstructorCollection
            Get
                Return InstructorsAvailable()
            End Get
        End Property

        Private Function InstructorsAvailable() As InstructorCollection
            Dim available = New InstructorCollection
            Skiclub.Instructorlist.Where(Function(y) y.IsAvailable).ToList.ForEach(Sub(i) available.Add(i))
            Return available
        End Function

        Public Sub CreateNewSkiclub()
            Skiclub = New Entities.Club With {.Levellist = PresetService.CreateLevels()}
            If MessageBoxResult.Yes = MessageBox.Show("Neuen Skiclub erstellt, gleich neue Gruppen hinzufügen?", "Achtung", MessageBoxButton.YesNo) Then
                Dim dlg = New CountOfGroupsDialog
                If dlg.ShowDialog Then
                    Skiclub.Grouplist = PresetService.CreateGroups(dlg.Count.Text)
                End If
            End If
        End Sub

    End Module

End Namespace
