Imports Groupies.Entities.Generation4

Public Class TeilnehmerEventArgs
    Inherits EventArgs
    Public Property GeaenderteTeilnehmer As IEnumerable(Of Teilnehmer)
    Public Sub New(Optional items As IEnumerable(Of Teilnehmer) = Nothing)
        Me.GeaenderteTeilnehmer = items
    End Sub
End Class
