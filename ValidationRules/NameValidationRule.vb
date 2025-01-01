Namespace ValidationRules

    Public Class NameValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As Globalization.CultureInfo) As ValidationResult
            If value = Nothing OrElse String.IsNullOrEmpty(value.ToString) Then
                Return New ValidationResult(False, "Der Name muss mindestens einen Buchstaben haben" & vbNewLine & Me.ToString)
            End If
            If value.ToString()(0).Equals(" ") Then
                Return New ValidationResult(False, "Der Name kann nicht mit Leerzeichen beginnen" & vbNewLine & Me.ToString)
            End If
            Return ValidationResult.ValidResult
        End Function
    End Class

End Namespace

