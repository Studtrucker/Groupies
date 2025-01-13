Public Class ModusFabrik

    Public Function ErzeugeModus(Modus As ModusEnum) As IModus
        If Modus = ModusEnum.Bearbeiten Then
            Return New ModusBearbeiten
        ElseIf Modus = ModusEnum.Erstellen Then
            Return New ModusErstellen
        Else
            Return New Exception("Unbekannter Modus")
        End If
    End Function

End Class

Public Interface IModus
    Property Titel As String
    Sub HandleClose(Window As IWindowMitModus)

End Interface

Public Class ModusErstellen
    Implements IModus

    Public Property Titel As String = " erstellen" Implements IModus.Titel

    Public Sub HandleClose(Window As IWindowMitModus) Implements IModus.HandleClose
        DirectCast(Window, Window).DialogResult = Window.Dialog
    End Sub

End Class

Public Class ModusBearbeiten
    Implements IModus

    Public Property Titel As String = " bearbeiten" Implements IModus.Titel

    Public Sub HandleClose(Window As IWindowMitModus) Implements IModus.HandleClose
        DirectCast(Window, Window).Close()
    End Sub
End Class


Public Enum ModusEnum
    Erstellen
    Bearbeiten
End Enum