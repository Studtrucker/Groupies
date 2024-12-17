Imports Groupies.Controller.AppController
Namespace ValidationRules

    Public Class TrainernameValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As Globalization.CultureInfo) As ValidationResult
            If CurrentClub.AlleTrainer.Select(Function(Tr) Tr.Spitzname).Contains(value) Then
                Return New ValidationResult(False, $"Der Spitzname {value} darf nur bei einem Trainer vorkommen")
            End If
            Return ValidationResult.ValidResult
        End Function
    End Class
End Namespace
