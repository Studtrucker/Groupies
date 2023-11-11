Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Data.SqlClient

Namespace Entities

    Public Class InstructorCollection
        Inherits ObservableCollection(Of Instructor)

        'Public ReadOnly Property SortedListDisplayables As InstructorCollection
        '    Get
        '        Dim sorted = New InstructorCollection
        '        Me.Where(Function(y) y.SaveAndShow = True).OrderBy(Of String)(Function(x) x.InstructorFullName).ToList.ForEach(Sub(i) sorted.Add(i))
        '        Return sorted
        '    End Get
        'End Property

        'Public ReadOnly Property SortedListIsAvailable As InstructorCollection
        '    Get
        '        Dim sorted = New InstructorCollection
        '        Me.Where(Function(y) y.IsAvailable).OrderBy(Of String)(Function(x) x.InstructorFullName).ToList.ForEach(Sub(i) sorted.Add(i))
        '        Return sorted
        '    End Get
        'End Property

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
