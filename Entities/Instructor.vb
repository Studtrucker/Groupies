Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Entities

    Public Class Instructor
        Inherits BaseModel

        Public Sub New()
            _InstructorID = Guid.NewGuid()
        End Sub

        Public Property InstructorID As Guid


        Public Property Firstname As String


        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Name ist eine Pflichtangabe")>
        Public Property Name As String


        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Print Name ist eine Pflichtangabe")>
        Public Property PrintName As String

        Public Property Picture As Byte()


        Public ReadOnly Property Fullname As String
            Get
                If _Firstname Is Nothing Then
                    Return _Name
                ElseIf _Name Is Nothing Then
                    Return _Firstname
                Else
                    Return String.Format("{0} {1}", _Firstname, _Name)
                End If
            End Get
        End Property


        Public ReadOnly Property HasPicture As Boolean
            Get
                Return _Picture IsNot Nothing AndAlso Picture.Length > 0
            End Get
        End Property

    End Class

End Namespace
