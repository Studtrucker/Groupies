Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Data.SqlClient

Namespace Entities.Veraltert

    Public Class InstructorCollection
        Inherits ObservableCollection(Of Instructor)
        Implements IEnumerable(Of Instructor)


    End Class
End Namespace
