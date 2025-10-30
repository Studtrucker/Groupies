
Namespace Entities.Generation4

    Public Class EinteilungsspezifischeGruppe
        Inherits BaseModel

#Region "Felder"
        Private _GruppenID As Guid
        Private _EinteilungsspezifischeGruppe As Gruppe
#End Region

#Region "Eigenschaften"
        Public Property GruppenID As Guid
            Get
                Return _GruppenID
            End Get
            Set(value As Guid)
                _GruppenID = value
                OnPropertyChanged(NameOf(GruppenID))
            End Set
        End Property
        Public Property EinteilungsspezifischeGruppe As Gruppe
            Get
                Return _EinteilungsspezifischeGruppe
            End Get
            Set(value As Gruppe)
                _EinteilungsspezifischeGruppe = value
                OnPropertyChanged(NameOf(EinteilungsspezifischeGruppe))
            End Set
        End Property
#End Region

    End Class
End Namespace
