Imports Groupies.Interfaces

Namespace Fabriken

    Public Class ModusFabrik

        Public Function ErzeugeModus(Modus As Enums.ModusEnum) As Interfaces.IModus
            Select Case Modus
                Case Enums.ModusEnum.Bearbeiten
                    Return New ModusBearbeiten
                Case Enums.ModusEnum.Erstellen
                    Return New ModusErstellen
                Case Enums.ModusEnum.Anzeigen
                    Return New ModusAnzeigen
                Case Else
                    Return New Exception("Unbekannter Modus")
            End Select
        End Function

    End Class

    Public Class ModusAnzeigen
        Implements Interfaces.IModus

        Public Property Titel As String = "ansehen" Implements Interfaces.IModus.Titel

        Public Property WindowIcon As String = "pack://application:,,,/Images/icons8-view-48.png" Implements IModus.WindowIcon

        Public Property CancelButtonVisibility As Visibility = Visibility.Collapsed Implements IModus.CancelButtonVisibility

        Public Property CloseButtonVisibility As Visibility = Visibility.Visible Implements IModus.CloseButtonVisibility

        Public Property OkButtonVisibility As Visibility = Visibility.Collapsed Implements IModus.OkButtonVisibility

    End Class

    Public Class ModusErstellen
        Implements Interfaces.IModus

        Public Property Titel As String = "erstellen" Implements Interfaces.IModus.Titel

        Public Property WindowIcon As String = "pack://application:,,,/Images/icons8-create-48.png" Implements IModus.WindowIcon

        Public Property CancelButtonVisibility As Visibility = Visibility.Visible Implements IModus.CancelButtonVisibility

        Public Property CloseButtonVisibility As Visibility = Visibility.Collapsed Implements IModus.CloseButtonVisibility

        Public Property OkButtonVisibility As Visibility = Visibility.Visible Implements IModus.OkButtonVisibility

    End Class

    Public Class ModusBearbeiten
        Implements Interfaces.IModus

        Public Property Titel As String = "bearbeiten" Implements Interfaces.IModus.Titel

        Public Property WindowIcon As String = "pack://application:,,,/Images/icons8-pencil-48.png" Implements IModus.WindowIcon

        Public Property CancelButtonVisibility As Visibility = Visibility.Visible Implements IModus.CancelButtonVisibility

        Public Property CloseButtonVisibility As Visibility = Visibility.Collapsed Implements IModus.CloseButtonVisibility

        Public Property OkButtonVisibility As Visibility = Visibility.Visible Implements IModus.OkButtonVisibility

    End Class

End Namespace



