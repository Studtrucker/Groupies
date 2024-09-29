Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Data.SqlClient

Namespace Entities

    Public Class InstructorCollection
        Inherits ObservableCollection(Of Trainer)
        Implements IEnumerable(Of Trainer)

        Public Function GetPrintName(Instructor As Trainer) As String
            Return First(Function(x) x.TrainerID = Instructor.TrainerID).Spitzname
        End Function

        Public Function GetHatFoto(Instructor As Trainer) As Boolean
            Return First(Function(x) x.TrainerID = Instructor.TrainerID).HasPicture
        End Function

        Public Function GetFoto(Instructor As Trainer) As Byte()
            Return First(Function(x) x.TrainerID = Instructor.TrainerID).InstructorPicture
        End Function

    End Class
End Namespace
