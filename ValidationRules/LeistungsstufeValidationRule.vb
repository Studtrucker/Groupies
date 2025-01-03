Imports System.Globalization

Namespace ValidationRules


    Public Class LeistungsstufeValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
            Dim bindingGroup = DirectCast(value, BindingGroup)
            If bindingGroup.Items.Count = 1 Then
                Dim Leistungsstufe = DirectCast(bindingGroup.Items(0), Entities.Leistungsstufe)
                'Dim Sortierung = CType(bindingGroup.GetValue(Leistungsstufe, NameOf(Leistungsstufe.Sortierung)), Integer)
                'Dim Benennung = DirectCast(bindingGroup.GetValue(Leistungsstufe, NameOf(Leistungsstufe.Benennung)), String)
                Dim r1 = New BenennungValidationRule
                Return r1.Validate(bindingGroup.GetValue(Leistungsstufe, NameOf(Leistungsstufe.Benennung)), cultureInfo)
                Dim r2 = New SortierungUniqueValidationRule
                Return r2.Validate(bindingGroup.GetValue(Leistungsstufe, NameOf(Leistungsstufe.Sortierung)), cultureInfo)
                Dim r3 = New IntegerValidationRule
                Return r3.Validate(bindingGroup.GetValue(Leistungsstufe, NameOf(Leistungsstufe.Sortierung)), cultureInfo)
            End If
            Return ValidationResult.ValidResult
        End Function
    End Class

End Namespace
