Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Entities.Veraltert

    Public Class Instructor
        Inherits BaseModel

        Public Sub New()
        End Sub

        Public Sub New(SaveMe As Boolean, IAmAvailable As Boolean)
            _InstructorID = Guid.NewGuid()
            SaveOrDisplay = SaveMe
            IsAvailable = IAmAvailable
        End Sub

        Public Property InstructorID As Guid

        Public Property InstructorFirstName As String

        Public Property InstructorLastName As String


        Public Property InstructorPrintName As String

        Public Property InstructorPicture As Byte()

        Public Property eMail() As String

        Public Property InstructorFullName As String

        Public Property IsAvailable As Boolean

        Public Property SaveOrDisplay As Boolean


    End Class

End Namespace
