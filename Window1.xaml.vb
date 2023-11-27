Public Class Window1
    Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Groupies.Services.CurrentDataService.Skiclub = New Entities.Skiclub
        Groupies.Services.CurrentDataService.Skiclub.Levellist = Groupies.Services.CreateLevels()

        LevelView.DataContext = Groupies.Services.CurrentDataService.Skiclub.Levellist(4)

    End Sub

End Class
