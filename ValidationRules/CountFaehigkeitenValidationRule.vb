Namespace ValidationRules

    Public Class CountFaehigkeitenValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As Globalization.CultureInfo) As ValidationResult
            Dim Fahigkeiten = TryCast(value, ItemCollection)
            If Fahigkeiten.Count <= 1 Then
                Return New ValidationResult(False, "Die Leistungsstufe muss mindestens eine Fähigkeit beinhalten")
            End If
            Return ValidationResult.ValidResult

        End Function
    End Class
End Namespace
