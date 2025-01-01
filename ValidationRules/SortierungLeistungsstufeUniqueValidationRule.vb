Imports System.Globalization
Imports Groupies.Controller.AppController

Namespace ValidationRules

    Public Class SortierungLeistungsstufeUniqueValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult

            Dim vorhanden As Boolean '= CurrentClub.Leistungsstufenliste.ToList.TakeWhile(Function(Ls) Ls.Sortierung = value)

            For Each item In CurrentClub.Leistungsstufenliste
                If item.Sortierung = value Then
                    vorhanden = True
                    Exit For
                End If
            Next

            If vorhanden Then

                Return New ValidationResult(False, $"Die Sortierung {value} wird bereits verwendet {vbNewLine}({Me.ToString})")
            End If
            Return ValidationResult.ValidResult

        End Function
    End Class

End Namespace
