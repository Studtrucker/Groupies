Imports Groupies.UserControls
Public Class NewDialog

    Sub New(Entity As EntityType)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.


        DockPanel.Children.Add(New SkillView)

    End Sub

End Class

Public Enum EntityType
    Group
    Instructor
    Participant
    Level
    Skill
End Enum
