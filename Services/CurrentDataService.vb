Imports System.Collections.ObjectModel
Imports Groupies.Entities

Namespace Services

    Module CurrentDataService

        Public Property Skiclub As Skiclub

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

    End Module

End Namespace
