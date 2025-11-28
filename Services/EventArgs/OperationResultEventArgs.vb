Imports System

Namespace Services
    ''' <summary>
    ''' Generalisierte EventArgs, die Erfolg/Zustand, Meldung, Exception und optionales Payload transportieren.
    ''' </summary>
    Public Class OperationResultEventArgs
        Inherits EventArgs

        Public Sub New()
        End Sub

        Public Sub New(success As Boolean, message As String, Optional ex As Exception = Nothing, Optional payload As Object = Nothing)
            Me.Success = success
            Me.Message = message
            Me.Exception = ex
            Me.Payload = payload
        End Sub

        Public Property Success As Boolean
        Public Property Message As String
        Public Property Exception As Exception
        ''' <summary>
        ''' Optional: Beliebige Nutzdaten (z. B. geladene Domain-Objekte).
        ''' </summary>
        Public Property Payload As Object
    End Class
End Namespace