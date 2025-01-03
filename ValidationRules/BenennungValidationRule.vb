Namespace ValidationRules

    Public Class BenennungValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As Globalization.CultureInfo) As ValidationResult
            If value = Nothing OrElse String.IsNullOrEmpty(value.ToString) Then
                Return New ValidationResult(False, "Die Benennung darf nicht leer sein")
            End If
            If value.ToString.StartsWith(" ") Then
                Return New ValidationResult(False, "Der Bennenung kann nicht mit Leerzeichen beginnen")
            End If
            If value.ToString.EndsWith(" ") Then
                Return New ValidationResult(False, "Der Benennung kann nicht mit Leerzeichen enden")
            End If
            Return ValidationResult.ValidResult
        End Function
    End Class

End Namespace

