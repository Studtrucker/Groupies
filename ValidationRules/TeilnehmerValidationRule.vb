Imports System.Globalization

Namespace ValidationRules

    Public Class TeilnehmerValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
            Dim bindingGroup = DirectCast(value, BindingGroup)
            If bindingGroup.Items.Count = 1 Then
                Dim Teilnehmer = DirectCast(bindingGroup.Items(0), Entities.Teilnehmer)
                Dim Vorname = DirectCast(bindingGroup.GetValue(Teilnehmer, NameOf(Teilnehmer.Vorname)), String)
                Dim Nachname = DirectCast(bindingGroup.GetValue(Teilnehmer, NameOf(Teilnehmer.Nachname)), String)
                If String.IsNullOrWhiteSpace(Vorname) Then
                    Return New ValidationResult(False, "Der Vorname ist eine Pflichtangabe")
                End If
                If String.IsNullOrWhiteSpace(Nachname) Then
                    Return New ValidationResult(False, "Der Nachname ist eine Pflichtangabe")
                End If
            End If
            Return ValidationResult.ValidResult
        End Function
    End Class
End Namespace
