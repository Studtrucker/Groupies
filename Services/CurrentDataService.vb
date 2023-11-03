Imports Skischule.Entities

Namespace DataService
    Module CurrentDataService

        Public Property Skiclub As Skiclub

        Public ReadOnly Property InstructorWithoutGroupList As InstructorCollection
            Get

                Return Skiclub.Instructorlist
            End Get
        End Property

    End Module


End Namespace
