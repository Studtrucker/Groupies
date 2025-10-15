Imports System.ComponentModel.DataAnnotations

Namespace Entities.Generation2

    Public Class Trainer
        Implements IModel

#Region "Properties"

        Public Property TrainerID As Guid Implements IModel.Ident
        Public Property Vorname As String
        Public Property Nachname As String
        Public Property Spitzname As String
        Public Property Foto As Byte()
        Public Property VorUndNachname As String
        Public Property Archivieren As Boolean

        Public Sub speichern() Implements IModel.speichern
            Throw New NotImplementedException()
        End Sub

#End Region

    End Class

End Namespace
