Imports Groupies.Interfaces

Namespace Fabriken

    Public Class DatentypFabrik

        Public Function ErzeugeDatentyp(Datentyp As Enums.DatentypEnum) As Interfaces.IDatentyp
            Select Case Datentyp
                Case Enums.DatentypEnum.Teilnehmer
                    Return New TeilnehmerDatentyp
                Case Enums.DatentypEnum.Trainer
                    Return New TrainerDatentyp
                Case Enums.DatentypEnum.Gruppe
                    Return New GruppeDatentyp
                Case Enums.DatentypEnum.Faehigkeit
                    Return New FaehigkeitDatentyp
                Case Enums.DatentypEnum.Leistungsstufe
                    Return New LeistungsstufeDatentyp
                Case Else
                    Return New Exception("Unbekannter Datentyp")
            End Select
        End Function

        Public Function ErzeugeDatentyp(Datenobjekt As IModel) As Interfaces.IDatentyp
            Select Case Datenobjekt.GetType
                Case GetType(Entities.Trainer)
                    Return New TrainerDatentyp
                Case GetType(Entities.Teilnehmer)
                    Return New TeilnehmerDatentyp
                Case GetType(Entities.Gruppe)
                    Return New GruppeDatentyp
                Case GetType(Entities.Faehigkeit)
                    Return New FaehigkeitDatentyp
                Case GetType(Entities.Leistungsstufe)
                    Return New LeistungsstufeDatentyp
                Case Else
                    Return New Exception("Unbekannter Datentyp")
            End Select
        End Function

    End Class

    Public Class TrainerDatentyp
        Implements Interfaces.IDatentyp

        Public Property DatentypText As String = "Trainer" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-trainer-48.png" Implements IDatentyp.DatentypIcon

        Public ReadOnly Property DatentypDetailUserControl As UserControl Implements IDatentyp.DatentypDetailUserControl
            Get
                Return New UserControls.TrainerUserControl
            End Get
        End Property

        Public ReadOnly Property DatentypListUserControl As UserControl Implements IDatentyp.DatentypListUserControl
            Get
                Return New UserControls.TrainerlisteUserControl
            End Get
        End Property
    End Class

    Public Class TeilnehmerDatentyp
        Implements Interfaces.IDatentyp

        Public Property DatentypText As String = "Teilnehmer" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-person-48.png" Implements IDatentyp.DatentypIcon
        Public ReadOnly Property DatentypDetailUserControl As UserControl Implements IDatentyp.DatentypDetailUserControl
            Get
                Return New UserControls.TeilnehmerUserControl
            End Get
        End Property

        Public ReadOnly Property DatentypListUserControl As UserControl Implements IDatentyp.DatentypListUserControl
            Get
                Return New UserControls.TeilnehmerlisteUserControl
            End Get
        End Property
    End Class

    Public Class GruppeDatentyp
        Implements Interfaces.IDatentyp
        Public Property DatentypText As String = "Gruppe" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-konferenz-vordergrund-ausgewaehlte-48.png" Implements IDatentyp.DatentypIcon
        Public ReadOnly Property DatentypDetailUserControl As UserControl Implements IDatentyp.DatentypDetailUserControl
            Get
                Return New UserControls.GruppeUserControl
            End Get
        End Property
        Public ReadOnly Property DatentypListUserControl As UserControl Implements IDatentyp.DatentypListUserControl
            Get
                Return New UserControls.GruppenlisteUserControl
            End Get
        End Property
    End Class

    Public Class FaehigkeitDatentyp
        Implements Interfaces.IDatentyp
        Public Property DatentypText As String = "Fähigkeit" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-trophaee-48.png" Implements IDatentyp.DatentypIcon
        Public ReadOnly Property DatentypDetailUserControl As UserControl Implements IDatentyp.DatentypDetailUserControl
            Get
                Return New UserControls.FaehigkeitUserControl
            End Get
        End Property
        Public ReadOnly Property DatentypListUserControl As UserControl Implements IDatentyp.DatentypListUserControl
            Get
                Return New UserControls.FaehigkeitenlisteUserControl
            End Get
        End Property
    End Class

    Public Class LeistungsstufeDatentyp
        Implements Interfaces.IDatentyp
        Public Property DatentypText As String = "Leistungsstufe" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-treppe-rauf-48.png" Implements IDatentyp.DatentypIcon
        Public ReadOnly Property DatentypDetailUserControl As UserControl Implements IDatentyp.DatentypDetailUserControl
            Get
                Return New UserControls.LeistungsstufeUserControl
            End Get
        End Property
        Public ReadOnly Property DatentypListUserControl As UserControl Implements IDatentyp.DatentypListUserControl
            Get
                Return New UserControls.LeistungsstufenlisteUserControl
            End Get
        End Property
    End Class

End Namespace
