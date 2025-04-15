Imports Groupies.Interfaces

Namespace Fabriken

    Public Class ModusFabrik

        Public Function ErzeugeModus(Modus As Enums.ModusEnum) As Interfaces.IModus
            If Modus = Enums.ModusEnum.Bearbeiten Then
                Return New ModusBearbeiten
            ElseIf Modus = Enums.ModusEnum.Erstellen Then
                Return New ModusErstellen
            Else
                Return New Exception("Unbekannter Modus")
            End If
        End Function

    End Class

    Public Class ModusErstellen
        Implements Interfaces.IModus

        Public Property Titel As String = " erstellen" Implements Interfaces.IModus.Titel

    End Class

    Public Class ModusBearbeiten
        Implements Interfaces.IModus

        Public Property Titel As String = " bearbeiten" Implements Interfaces.IModus.Titel

    End Class

End Namespace

Namespace Interfaces

    Public Interface IModus
        Property Titel As String
    End Interface
End Namespace


Namespace Enums

    Public Enum ModusEnum
        Erstellen
        Bearbeiten
    End Enum

End Namespace