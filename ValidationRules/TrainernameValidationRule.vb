Imports Groupies.Controller.AppController

Public Class TrainernameValidationRule
    Inherits ValidationRule

    Public Overrides Function Validate(value As Object, cultureInfo As Globalization.CultureInfo) As ValidationResult
        If CurrentClub.AlleTrainer.Select(Function(Tr) Tr.Spitzname).Contains(value) Then
            Return New ValidationResult(False, $"Der Spitzname {value} darf nur einmal vorkommen")
        End If
        Return ValidationResult.ValidResult
    End Function
End Class
