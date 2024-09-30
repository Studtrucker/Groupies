Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Data.SqlClient

Namespace Entities.Veraltert

    Public Class InstructorCollection
        Inherits ObservableCollection(Of Instructor)
        Implements IEnumerable(Of Instructor)

        Public Function GetPrintName(Instructor As Instructor) As String
            Return First(Function(x) x.InstructorID = Instructor.InstructorID).InstructorPrintName
        End Function

        Public Function GetHatFoto(Instructor As Instructor) As Boolean
            Return First(Function(x) x.InstructorID = Instructor.InstructorID).HasPicture
        End Function

        Public Function GetFoto(Instructor As Instructor) As Byte()
            Return First(Function(x) x.InstructorID = Instructor.InstructorID).InstructorPicture
        End Function

    End Class
End Namespace
