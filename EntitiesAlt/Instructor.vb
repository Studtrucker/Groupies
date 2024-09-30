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

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Vorname ist eine Pflichtangabe")>
        Public Property InstructorFirstName As String

        Public Property InstructorLastName As String


        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Print Name ist eine Pflichtangabe")>
        Public Property InstructorPrintName As String

        Public Property InstructorPicture As Byte()

        <DataAnnotations.EmailAddress(ErrorMessage:="Gültige e-Mail Adresse")>
        Public Property eMail() As String

        Public Property InstructorFullName As String
            Get
                Return GetFullName()
            End Get
            Set(value As String)
                SetFullName(value)
            End Set
        End Property

        Private Function GetFullName() As String
            If _InstructorFirstName Is Nothing Then
                Return _InstructorLastName
            ElseIf _InstructorLastName Is Nothing Then
                Return _InstructorFirstName
            Else
                Return String.Format("{0} {1}", _InstructorFirstName, _InstructorLastName)
            End If
        End Function

        Private Sub SetFullName(Value As String)
            If Value = GetFullName() Then

            End If
        End Sub


        Public ReadOnly Property HasPicture As Boolean
            Get
                Return _InstructorPicture IsNot Nothing AndAlso InstructorPicture.Length > 0
            End Get
        End Property

        Private _IsAvailable As Boolean
        Public Property IsAvailable As Boolean
            Get
                Return _IsAvailable
            End Get
            Set(value As Boolean)
                _IsAvailable = value
            End Set
        End Property

        Public ReadOnly Property IsAssigned As Boolean
            Get
                Return Not _IsAvailable
            End Get
        End Property

        Public Property SaveOrDisplay As Boolean

        Public Overrides Function ToString() As String
            Return InstructorFullName
        End Function

    End Class

End Namespace
