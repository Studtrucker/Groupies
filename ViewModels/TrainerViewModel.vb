Imports System.ComponentModel
Imports Microsoft.Office.Interop.Excel
Imports Groupies.Entities
Imports Groupies.UserControls

Public Class TrainerViewModel
    Inherits ViewModelBase
    Implements IDataErrorInfo
    Implements INotifyDataErrorInfo


    Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen

    End Sub

    Public Property DatenObjekt As Trainer 'Implements IViewModel.DatenObjekt


#Region "DataErrorInfo"
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

#End Region

#Region "NotifyDataErrorInfo"

    Private _fehler As New Dictionary(Of String, List(Of String))

    Public Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged

    Private Sub ValidateVorname()
        _fehler("Vorname") = New List(Of String)
        If String.IsNullOrWhiteSpace(DatenObjekt.Nachname) Then
            _fehler("Vorname").Add("Vorname darf nicht leer sein.")
        End If
        RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs("Vorname"))
    End Sub

    Public ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
        Get
            Return _fehler.Any(Function(kv) kv.Value.Count > 0)
        End Get
    End Property

    Public Function GetErrors(propertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors
        If _fehler.ContainsKey(propertyName) Then Return _fehler(propertyName)
        Return Nothing
    End Function

#End Region
End Class

