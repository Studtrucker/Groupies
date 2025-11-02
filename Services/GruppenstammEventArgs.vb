Imports Groupies.Entities.Generation4
Public Class GruppenstammEventArgs
    Inherits EventArgs

    ' Falls zusätzliche Daten erforderlich sind, können diese hier hinzugefügt werden
    ' Beispiel: Dateipfad

    Public Property ChangedGruppenstamm As Gruppenstamm

    Public Sub New(ChangedGruppenstamm As Gruppenstamm)
        Me.ChangedGruppenstamm = ChangedGruppenstamm
    End Sub

End Class
