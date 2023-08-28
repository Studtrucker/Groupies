Imports Skikurs.Entities
Class Application

    ' Ereignisse auf Anwendungsebene wie Startup, Exit und DispatcherUnhandledException
    ' können in dieser Datei verarbeitet werden.


    Private Sub Skikurs_Startup(sender As Object, e As StartupEventArgs)

        Dim wnd As New MainWindow
        BasicObjects.Skischule.initialisiereFixeListen()


        'BasicObjects.erstelleKoennenstufen()
    End Sub

End Class
