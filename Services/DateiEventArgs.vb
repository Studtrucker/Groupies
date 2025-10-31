Imports System
Public Class DateiEventArgs
    Inherits EventArgs

    ' Falls zusätzliche Daten erforderlich sind, können diese hier hinzugefügt werden
    ' Beispiel: Dateipfad

    Public Property DateiPfad As String
    Public Property Clubname As String

    Public Sub New(dateiPfad As String)
        Me.DateiPfad = dateiPfad
    End Sub
    Public Sub New(dateiPfad As String, Clubname As String)
        Me.New(dateiPfad)
        Me.Clubname = Clubname
    End Sub

End Class
