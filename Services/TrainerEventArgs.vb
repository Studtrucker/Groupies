Imports Groupies.Entities.Generation4
Public Class TrainerEventArgs
    Inherits EventArgs
    Public Property Trainer As Trainer
    Public Property Gruppe As Gruppe
    Public Property Einteilung As Einteilung

    Public Sub New(Trainer As Trainer, Optional Gruppe As Gruppe = Nothing, Optional Einteilung As Einteilung = Nothing)
        Me.Trainer = Trainer
        Me.Einteilung = Einteilung
        Me.Gruppe = Gruppe
    End Sub

    Public Sub New()
        Me.Trainer = Nothing
    End Sub

    Public Shared Shadows ReadOnly Property Empty As TrainerEventArgs = New TrainerEventArgs()

End Class
