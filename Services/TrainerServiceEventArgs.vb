Public Class TrainerServiceEventArgs
    Inherits EventArgs
    Public Property TrainerIDListe As List(Of Guid)
    Public Property EinteilungID As Nullable(Of Guid)
    Public Property GruppeID As Nullable(Of Guid)

    Public Sub New(TrainerIDListe As List(Of Guid), EinteilungID As Guid, Optional GruppeID As Nullable(Of Guid) = Nothing)
        Me.TrainerIDListe = TrainerIDListe
        Me.EinteilungID = EinteilungID
        Me.GruppeID = GruppeID
    End Sub

End Class
