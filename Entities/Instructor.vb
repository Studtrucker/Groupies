Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Entities

    Public Class Instructor
        Inherits BaseModel

        Public Sub New()
            _UebungsleiterID = Guid.NewGuid()
        End Sub

        Public Property UebungsleiterID As Guid


        Public Property Vorname As String


        Public Property Name As String


        Public Property PrintName As String


        Public Property Foto As Byte()


        Public ReadOnly Property VollerName As String
            Get
                If _vorname Is Nothing Then
                    Return _name
                ElseIf _name Is Nothing Then
                    Return _vorname
                Else
                    Return String.Format("{0} {1}", _vorname, _name)
                End If
            End Get
        End Property

        Public ReadOnly Property HatFoto As Boolean
            Get
                Return _foto IsNot Nothing AndAlso Foto.Length > 0
            End Get
        End Property

    End Class

End Namespace
