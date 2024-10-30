Module ExportService
    Public Sub ExportTeilnehmer()
        Dim xl = New XLSchreiber()
        xl.ExportDatenAlsXl(".xlsx", "Teilnehmer")
    End Sub

    Public Sub ExportTrainer()
        Dim xl = New XLSchreiber()
        xl.ExportDatenAlsXl(".xlsx", "Trainer")
    End Sub


End Module
