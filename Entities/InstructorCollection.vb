Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Data.SqlClient

Namespace Entities

    Public Class InstructorCollection
        Inherits ObservableCollection(Of Instructor)

        Public Function GetPrintName(Instructor As Instructor) As String
            Return First(Function(x) x.InstructorID = Instructor.InstructorID).PrintName
        End Function

        Public Function GetHatFoto(Instructor As Instructor) As Boolean
            Return First(Function(x) x.InstructorID = Instructor.InstructorID).HasPicture
        End Function

        Public Function GetFoto(Instructor As Instructor) As Byte()
            Return First(Function(x) x.InstructorID = Instructor.InstructorID).Picture
        End Function

    End Class
End Namespace
