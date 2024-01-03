Imports System.ComponentModel
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Xml.Serialization
Imports Groupies.Commands
Imports CDS = Groupies.Services.CurrentDataService

Public Class Window1

    'Private _levelListCollectionView As ICollectionView

    'Sub New()

    '    ' Dieser Aufruf ist für den Designer erforderlich.
    '    InitializeComponent()

    '    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    '    'Services.Service.LoadLastSkischule()
    '    'If Groupies.Services.CurrentDataService.Skiclub Is Nothing Then
    '    'Services.Service.OpenSkischule("2023StubaiTest.ski")
    '    'End If
    '    _levelListCollectionView = New ListCollectionView(Groupies.Services.CurrentDataService.Skiclub.Levellist)
    '    _levelListCollectionView.SortDescriptions.Add(New SortDescription("SortNumber", ListSortDirection.Ascending))
    '    DataContext = _levelListCollectionView

    'End Sub

    'Sub New()

    '    ' Dieser Aufruf ist für den Designer erforderlich.
    '    InitializeComponent()

    '    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    '    _skillListCollectionView = New ListCollectionView(New SkillCollection())

    'End Sub

    '' Zur Ausführung des Handles HandleNewSkillExecuted erstellen, kann auf dem Mutter Window ein RoutedEvent registriert werden.
    '' Siehe auch EventTrigger (= Ereignisauslöser) Kapitel 11 »Styles, Trigger und Templates«

    'Private Sub Window1_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
    '    CommandBindings.Add(New CommandBinding(SkiclubCommands.NeuerSkill, AddressOf HandleNewSkillExecuted))
    'End Sub

    'Private Sub HandleNewSkillCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
    '    e.CanExecute = True
    'End Sub

    'Private Sub HandleNewSkillExecuted(sender As Object, e As ExecutedRoutedEventArgs)
    '    Dim dlg = New NewSkillDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}
    '    If dlg.ShowDialog = True Then
    '        CDS.Skiclub.Levellist(_levelListCollectionView.CurrentItem).LevelSkills.Add(dlg.Skill)
    '        'skillListCollectionView.MoveCurrentTo(dlg.Skill)
    '        'SkillsDataGrid.ScrollIntoView(dlg.Skill)
    '    End If
    'End Sub
End Class
