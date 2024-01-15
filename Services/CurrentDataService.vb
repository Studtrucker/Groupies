Imports System.Collections.ObjectModel
Imports Groupies.Entities

Namespace Services

    Module CurrentDataService

        Public Property Skiclub As Skiclub
        Public Property SkiclubFileName As String

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
            Skiclub = New Entities.Skiclub With {.Levellist = CreateDefaultService.CreateLevels()}
            If MessageBoxResult.Yes = MessageBox.Show("Neuen Skiclub erstellt, gleich neue Gruppen hinzufügen?", "Achtung", MessageBoxButton.YesNo) Then
                Dim dlg = New CountOfGroupsDialog
                If dlg.ShowDialog Then
                    Skiclub.Grouplist = CreateDefaultService.CreateGroups(dlg.Count.Text)
                End If
            End If
        End Sub

    End Module

End Namespace
