Imports Groupies.Entities.Generation4
Public Class TrainerServiceEventArgs
    Inherits EventArgs
    Public Property TrainerIDListe As List(Of Guid)
    Public Property EinteilungID As Nullable(Of Guid)
    Public Property GruppeID As Nullable(Of Guid)
    Public Property NeueTrainerdaten As Trainer

    Public Sub New(TrainerIDListe As List(Of Guid), Optional EinteilungID As Nullable(Of Guid) = Nothing, Optional GruppeID As Nullable(Of Guid) = Nothing, Optional NeueTrainerdaten As Trainer = Nothing)
        Me.TrainerIDListe = TrainerIDListe
        Me.EinteilungID = EinteilungID
        Me.GruppeID = GruppeID
        Me.NeueTrainerdaten = NeueTrainerdaten
    End Sub

End Class
