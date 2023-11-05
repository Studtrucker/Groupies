Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Entities

    Public Class Instructor
        Inherits BaseModel

        Public Sub New()
            _InstructorID = Guid.NewGuid()
        End Sub

        Public Property InstructorID As Guid

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Vorname ist eine Pflichtangabe")>
        Public Property InstructorFirstName As String

        Public Property InstructorLastName As String


        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Print Name ist eine Pflichtangabe")>
        Public Property InstructorPrintName As String

        Public Property InstructorPicture As Byte()


        Public ReadOnly Property InstructorFullName As String
            Get
                If _InstructorFirstName Is Nothing Then
                    Return _InstructorLastName
                ElseIf _InstructorLastName Is Nothing Then
                    Return _InstructorFirstName
                Else
                    Return String.Format("{0} {1}", _InstructorFirstName, _InstructorLastName)
                End If
            End Get
        End Property


        Public ReadOnly Property HasPicture As Boolean
            Get
                Return _InstructorPicture IsNot Nothing AndAlso InstructorPicture.Length > 0
            End Get
        End Property

        Public Property LeaderOfGroup As Guid

    End Class

End Namespace
