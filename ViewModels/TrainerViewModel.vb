Imports System.ComponentModel
Imports Microsoft.Office.Interop.Excel
Imports Groupies.Entities
Imports Groupies.UserControls

Public Class TrainerViewModel
    Inherits ViewModelBase
    'Implements IViewModel
    Implements IDataErrorInfo

    Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen

    End Sub

    Public Property DatenObjekt As Trainer 'Implements IViewModel.DatenObjekt

    Default Public ReadOnly Property Item(columnName As String) As String Implements IDataErrorInfo.Item
        Get
            If columnName = NameOf(DatenObjekt.Vorname) AndAlso String.IsNullOrWhiteSpace(DatenObjekt.Vorname) Then
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
