Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Data.SqlClient

Namespace Entities

    Public Class InstructorCollection
        Inherits ObservableCollection(Of Instructor)

        'Public Sub New()
        '    Me.Add(New Instructor(False, True) With {
        '           .InstructorFirstName = String.Empty,
        '           .InstructorLastName = String.Empty,
        '           .InstructorPrintName = String.Empty})
        'End Sub

        Public ReadOnly Property SortedListIsAvailable As ObservableCollection(Of Instructor)
            Get
                Return New ObservableCollection(Of Instructor)(Me.Where(Function(y) y.IsAvailable).OrderBy(Of String)(Function(x) x.InstructorFullName))
            End Get
        End Property
        Public ReadOnly Property SortedListDivided As ObservableCollection(Of Instructor)
            Get
                Return New ObservableCollection(Of Instructor)(Me.Where(Function(y) y.IsDivided).OrderBy(Of String)(Function(x) x.InstructorFullName))
            End Get
        End Property

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
