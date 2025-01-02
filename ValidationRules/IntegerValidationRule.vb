Imports System.Globalization
Imports System.Windows.Controls

Namespace ValidationRules

    Public Class IntegerValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
            If String.IsNullOrWhiteSpace(value) Then
                Return New ValidationResult(False, $"Die Sortierung ist eine Pflichtangabe{vbNewLine}({Me.ToString})")
            End If
            Dim result As Integer
            If Int32.TryParse(value, result) Then
                Return ValidationResult.ValidResult
            Else
                Return New ValidationResult(False, $"Die Sortierung muss eine Zahl sein{vbNewLine}({Me.ToString})")
            End If

        End Function
    End Class

End Namespace
