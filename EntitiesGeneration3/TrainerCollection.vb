Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Data.SqlClient

Namespace Entities.Generation3

    Public Class TrainerCollection
        Inherits ObservableCollection(Of Trainer)
        Implements IEnumerable(Of Trainer)

    End Class
End Namespace
