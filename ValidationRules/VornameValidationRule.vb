Namespace ValidationRules

    Public Class VornameValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As Globalization.CultureInfo) As ValidationResult
            If value = Nothing OrElse String.IsNullOrEmpty(value.ToString) Then
                Return New ValidationResult(False, "Vorname muss mindestens einen Buchstaben haben")
            End If
            If value.ToString()(0).Equals(" ") Then
                Return New ValidationResult(False, "Vorname kann nicht mit Leerzeichen beginnen")
            End If
            Return ValidationResult.ValidResult
        End Function
    End Class

End Namespace

