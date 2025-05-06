Imports Groupies.Interfaces

Namespace Fabriken

    Public Class DatentypFabrik

        Public Function ErzeugeDatentyp(Datentyp As Enums.DatentypEnum) As Interfaces.IDatentyp
            If Datentyp = Enums.DatentypEnum.Teilnehmer Then
                Return New DatentypTeilnehmer
            ElseIf Datentyp = Enums.DatentypEnum.Trainer Then
                Return New DatentypTrainer
            Else
                Return New Exception("Unbekannter Datentyp")
            End If
        End Function

    End Class

    Public Class DatentypTrainer
        Implements Interfaces.IDatentyp

        Public Property Titel As String = "Trainer" Implements Interfaces.IDatentyp.Titel
        Public Property IconString As String = "pack://application:,,,/Images/icons8-trainer-48.png" Implements IDatentyp.CaptionbildString

        Public ReadOnly Property DatentypControl As UserControl Implements IDatentyp.DatentypControl
            Get
                Return New UserControls.TrainerUserControl
            End Get
        End Property

    End Class

    Public Class DatentypTeilnehmer
        Implements Interfaces.IDatentyp
        Public Property Titel As String = "Teilnehmer" Implements Interfaces.IDatentyp.Titel
        Public Property IconString As String = "pack://application:,,,/Images/icons8-participant-48.png" Implements IDatentyp.CaptionbildString
        Public ReadOnly Property DatentypControl As UserControl Implements IDatentyp.DatentypControl
            Get
                Return New UserControls.TeilnehmerUserControl
            End Get
        End Property

    End Class

End Namespace
