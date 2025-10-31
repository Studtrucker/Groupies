Imports Groupies.Interfaces
Imports Groupies.Entities.Generation4

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
                Case Enums.DatentypEnum.Gruppenstamm
                    Return New GruppenstammDatentyp
                Case Enums.DatentypEnum.Faehigkeit
                    Return New FaehigkeitDatentyp
                Case Enums.DatentypEnum.Leistungsstufe
                    Return New LeistungsstufeDatentyp
                Case Enums.DatentypEnum.Einteilung
                    Return New EinteilungDatentyp
                Case Else
                    Return New Exception("Unbekannter Datentyp")
            End Select
        End Function

        Public Function ErzeugeDatentyp(Datenobjekt As IModel) As Interfaces.IDatentyp
            Select Case Datenobjekt.GetType
                Case GetType(Trainer)
                    Return New TrainerDatentyp
                Case GetType(Teilnehmer)
                    Return New TeilnehmerDatentyp
                Case GetType(Gruppe)
                    Return New GruppeDatentyp
                Case GetType(Faehigkeit)
                    Return New FaehigkeitDatentyp
                Case GetType(Leistungsstufe)
                    Return New LeistungsstufeDatentyp
                Case GetType(Einteilung)
                    Return New EinteilungDatentyp
                Case Else
                    Return New Exception("Unbekannter Datentyp")
            End Select
        End Function

    End Class

    Public Class TrainerDatentyp
        Implements Interfaces.IDatentyp

        Public Property DatentypText As String = "Trainer" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypenText As String = "Trainer" Implements Interfaces.IDatentyp.DatentypenText

        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-trainer-48.png" Implements IDatentyp.DatentypIcon

        Public ReadOnly Property DatentypDetailUserControl As UserControl Implements IDatentyp.DatentypDetailUserControl
            Get
                Return New UserControls.TrainerUserControl
            End Get
        End Property

        Public ReadOnly Property AktuellesUebersichtViewModel As IViewModelSpecial Implements IDatentyp.AktuellesUebersichtViewModel
            Get
                Return New TrainerViewModel
            End Get
        End Property

        Public ReadOnly Property AktuellesDetailViewModel As IViewModelSpecial Implements IDatentyp.AktuellesDetailViewModel
            Get
                Throw New NotImplementedException()
            End Get
        End Property

    End Class

    Public Class TeilnehmerDatentyp
        Implements Interfaces.IDatentyp

        Public Property DatentypText As String = "Teilnehmer" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypenText As String = "Teilnehmer" Implements Interfaces.IDatentyp.DatentypenText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-person-48.png" Implements IDatentyp.DatentypIcon
        Public ReadOnly Property DatentypDetailUserControl As UserControl Implements IDatentyp.DatentypDetailUserControl
            Get
                Return New UserControls.TeilnehmerUserControl
            End Get
        End Property

        Public ReadOnly Property AktuellesUebersichtViewModel As IViewModelSpecial Implements IDatentyp.AktuellesUebersichtViewModel
            Get
                Return New TeilnehmerViewModel
            End Get
        End Property

        Public ReadOnly Property AktuellesDetailViewModel As IViewModelSpecial Implements IDatentyp.AktuellesDetailViewModel
            Get
                Throw New NotImplementedException()
            End Get
        End Property

    End Class

    Public Class GruppeDatentyp
        Implements Interfaces.IDatentyp

        Public Property DatentypText As String = "Gruppe" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypenText As String = "Gruppen" Implements Interfaces.IDatentyp.DatentypenText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-konferenz-vordergrund-ausgewaehlte-48.png" Implements IDatentyp.DatentypIcon
        Public ReadOnly Property DatentypDetailUserControl As UserControl Implements IDatentyp.DatentypDetailUserControl
            Get

                Return New UserControls.GruppeUserControl
            End Get
        End Property

        Public ReadOnly Property AktuellesUebersichtViewModel As IViewModelSpecial Implements IDatentyp.AktuellesUebersichtViewModel
            Get
                Return New GruppeViewModel
            End Get
        End Property

        Public ReadOnly Property AktuellesDetailViewModel As IViewModelSpecial Implements IDatentyp.AktuellesDetailViewModel
            Get
                Throw New NotImplementedException()
            End Get
        End Property

    End Class

    Public Class GruppenstammDatentyp
        Implements Interfaces.IDatentyp

        Public Property DatentypText As String = "Gruppenstammdaten" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypenText As String = "Gruppenstammdaten" Implements Interfaces.IDatentyp.DatentypenText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-konferenz-vordergrund-ausgewaehlte-48.png" Implements IDatentyp.DatentypIcon
        Public ReadOnly Property DatentypDetailUserControl As UserControl Implements IDatentyp.DatentypDetailUserControl
            Get

                Return New UserControls.GruppenstammUserControl
            End Get
        End Property

        Public ReadOnly Property AktuellesUebersichtViewModel As IViewModelSpecial Implements IDatentyp.AktuellesUebersichtViewModel
            Get
                Return New GruppenstammViewModel
            End Get
        End Property

        Public ReadOnly Property AktuellesDetailViewModel As IViewModelSpecial Implements IDatentyp.AktuellesDetailViewModel
            Get
                Throw New NotImplementedException()
            End Get
        End Property

    End Class

    Public Class FaehigkeitDatentyp
        Implements Interfaces.IDatentyp

        Public Property DatentypText As String = "Fähigkeit" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypenText As String = "Fähigkeiten" Implements Interfaces.IDatentyp.DatentypenText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-trophaee-48.png" Implements IDatentyp.DatentypIcon
        Public ReadOnly Property DatentypDetailUserControl As UserControl Implements IDatentyp.DatentypDetailUserControl
            Get
                Return New UserControls.FaehigkeitUserControl
            End Get
        End Property

        Public ReadOnly Property AktuellesUebersichtViewModel As IViewModelSpecial Implements IDatentyp.AktuellesUebersichtViewModel
            Get
                Return New FaehigkeitViewModel
            End Get
        End Property

        Public ReadOnly Property AktuellesDetailViewModel As IViewModelSpecial Implements IDatentyp.AktuellesDetailViewModel
            Get
                Throw New NotImplementedException()
            End Get
        End Property

    End Class

    Public Class LeistungsstufeDatentyp
        Implements Interfaces.IDatentyp

        Public Property DatentypText As String = "Leistungsstufe" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypenText As String = "Leistungsstufen" Implements Interfaces.IDatentyp.DatentypenText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-treppe-rauf-48.png" Implements IDatentyp.DatentypIcon

        Public ReadOnly Property DatentypDetailUserControl As UserControl Implements IDatentyp.DatentypDetailUserControl
            Get
                Return New UserControls.LeistungsstufeUserControl
            End Get
        End Property

        Public ReadOnly Property AktuellesUebersichtViewModel As IViewModelSpecial Implements IDatentyp.AktuellesUebersichtViewModel
            Get
                Return New LeistungsstufeViewModel
            End Get
        End Property

        Public ReadOnly Property AktuellesDetailViewModel As IViewModelSpecial Implements IDatentyp.AktuellesDetailViewModel
            Get
                Throw New NotImplementedException()
            End Get
        End Property

    End Class

    Public Class EinteilungDatentyp
        Implements Interfaces.IDatentyp

        Public Property DatentypText As String = "Einteilung" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypenText As String = "Einteilungen" Implements Interfaces.IDatentyp.DatentypenText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-diversity-48.png" Implements IDatentyp.DatentypIcon
        Public ReadOnly Property DatentypDetailUserControl As UserControl Implements IDatentyp.DatentypDetailUserControl
            Get
                Return New UserControls.EinteilungUserControl
            End Get
        End Property

        Public ReadOnly Property AktuellesUebersichtViewModel As IViewModelSpecial Implements IDatentyp.AktuellesUebersichtViewModel
            Get
                Return New EinteilungViewModel
            End Get
        End Property

        Public ReadOnly Property AktuellesDetailViewModel As IViewModelSpecial Implements IDatentyp.AktuellesDetailViewModel
            Get
                Return New EinteilungViewModel
            End Get
        End Property

    End Class

End Namespace
