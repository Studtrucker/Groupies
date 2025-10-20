Imports System
Public Class DateiEventArgs
    Inherits EventArgs

    ' Falls zusätzliche Daten erforderlich sind, können diese hier hinzugefügt werden
    ' Beispiel: Dateipfad

    Public Property DateiPfad As String
    Public Sub New(dateiPfad As String)
        Me.DateiPfad = dateiPfad
    End Sub
End Class
