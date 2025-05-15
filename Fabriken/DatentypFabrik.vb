Imports Groupies.Interfaces

Namespace Fabriken

    Public Class DatentypFabrik

        Public Function ErzeugeDatentyp(Datentyp As Enums.DatentypEnum) As Interfaces.IDatentyp
            Select Case Datentyp
                Case Enums.DatentypEnum.Teilnehmer
                    Return New DatentypTeilnehmer
                Case Enums.DatentypEnum.Trainer
                    Return New DatentypTrainer
                Case Enums.DatentypEnum.Gruppe
                    Return New DatentypGruppe
                Case Enums.DatentypEnum.Faehigkeit
                    Return New DatentypFaehigkeit
                Case Enums.DatentypEnum.Leistungsstufe
                    Return New DatentypLeistungsstufe
                Case Else
                    Return New Exception("Unbekannter Datentyp")
            End Select
        End Function

        Public Function ErzeugeDatentyp(Datenobjekt As IModel) As Interfaces.IDatentyp
            Select Case Datenobjekt.GetType
                Case GetType(Entities.Trainer)
                    Return New DatentypTrainer
                Case GetType(Entities.Teilnehmer)
                    Return New DatentypTeilnehmer
                Case GetType(Entities.Gruppe)
                    Return New DatentypGruppe
                Case GetType(Entities.Faehigkeit)
                    Return New DatentypFaehigkeit
                Case GetType(Entities.Leistungsstufe)
                    Return New DatentypLeistungsstufe
                Case Else
                    Return New Exception("Unbekannter Datentyp")
            End Select
        End Function

    End Class

    Public Class DatentypTrainer
        Implements Interfaces.IDatentyp

        Public Property DatentypText As String = "Trainer" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-trainer-48.png" Implements IDatentyp.DatentypIcon

        Public ReadOnly Property DatentypUserControl As UserControl Implements IDatentyp.DatentypUserControl
            Get
                Return New UserControls.TrainerUserControl
            End Get
        End Property

    End Class

    Public Class DatentypTeilnehmer
        Implements Interfaces.IDatentyp
        Public Property DatentypText As String = "Teilnehmer" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-person-48.png" Implements IDatentyp.DatentypIcon
        Public ReadOnly Property DatentypUserControl As UserControl Implements IDatentyp.DatentypUserControl
            Get
                Return New UserControls.TeilnehmerUserControl
            End Get
        End Property

    End Class

    Public Class DatentypGruppe
        Implements Interfaces.IDatentyp
        Public Property DatentypText As String = "Gruppe" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-konferenz-vordergrund-ausgewaehlte-48.png" Implements IDatentyp.DatentypIcon
        Public ReadOnly Property DatentypUserControl As UserControl Implements IDatentyp.DatentypUserControl
            Get
                Return New UserControls.GruppeUserControl
            End Get
        End Property

    End Class

    Public Class DatentypFaehigkeit
        Implements Interfaces.IDatentyp
        Public Property DatentypText As String = "Fähigkeit" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-trophaee-48.png" Implements IDatentyp.DatentypIcon
        Public ReadOnly Property DatentypUserControl As UserControl Implements IDatentyp.DatentypUserControl
            Get
                Return New UserControls.FaehigkeitUserControl
            End Get
        End Property

    End Class

    Public Class DatentypLeistungsstufe
        Implements Interfaces.IDatentyp
        Public Property DatentypText As String = "Leistungsstufe" Implements Interfaces.IDatentyp.DatentypText
        Public Property DatentypIcon As String = "pack://application:,,,/Images/icons8-treppe-rauf-48.png" Implements IDatentyp.DatentypIcon
        Public ReadOnly Property DatentypUserControl As UserControl Implements IDatentyp.DatentypUserControl
            Get
                Return New UserControls.LeistungsstufeUserControl
            End Get
        End Property

    End Class

End Namespace
