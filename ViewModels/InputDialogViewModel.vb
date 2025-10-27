Imports System.ComponentModel

Namespace ViewModels

    Public Class InputDialogViewModel
        Implements INotifyPropertyChanged

        Private _responseText As String
        Public Property ResponseText As String
            Get
                Return _responseText
            End Get
            Set(value As String)
                If _responseText IsNot value Then
                    _responseText = value
                    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ResponseText)))
                End If
            End Set
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace
