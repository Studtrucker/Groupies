Imports System.Globalization
Imports Groupies.Controller.AppController

Namespace ValidationRules

    Public Class SortierungUniqueValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult

            For Each item In CurrentClub.Leistungsstufenliste
                If item.Sortierung = value Then
                    Return New ValidationResult(False, $"Die Sortierung {value} wird bereits verwendet")
                End If
            Next

            Return ValidationResult.ValidResult

        End Function
    End Class

End Namespace
