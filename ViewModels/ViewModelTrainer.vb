Imports System.ComponentModel
Imports Microsoft.Office.Interop.Excel
Imports Groupies.Entities

Public Class ViewModelTrainer
    Inherits ViewModelBase
    Implements IDataErrorInfo

    Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen

    End Sub

    Public Property Vorname As String
    Default Public ReadOnly Property Item(columnName As String) As String Implements IDataErrorInfo.Item
        Get
            If columnName = NameOf(Trainer.Vorname) AndAlso String.IsNullOrWhiteSpace(Vorname) Then
                Return "Vorname darf nicht leer sein."
            End If
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property [Error] As String Implements IDataErrorInfo.Error
        Get
            Return Nothing ' Wird meist ignoriert
        End Get
    End Property
End Class
