Imports Groupies.Controller.AppController

Namespace ValidationRules

    Public Class TrainerSpitznameValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As Globalization.CultureInfo) As ValidationResult
            If CurrentClub.AlleTrainer.Select(Function(Tr) Tr.Spitzname.ToUpper).Contains(value.ToString.ToUpper) Then
                Return New ValidationResult(False, $"Der Spitzname {value} wird bereits verwendet und darf aber nur für einen Trainer vergeben werden (Validation Rule)")
            End If
            Return ValidationResult.ValidResult
        End Function
    End Class
End Namespace
