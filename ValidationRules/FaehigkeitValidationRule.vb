Namespace ValidationRules

    Public Class FaehigkeitValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As Globalization.CultureInfo) As ValidationResult
            Dim bindingGroup = DirectCast(value, BindingGroup)
            If bindingGroup.Items.Count = 1 Then
                Dim Faehigkeit = DirectCast(bindingGroup.Items(0), Entities.Faehigkeit)
                Dim Sortierung = DirectCast(bindingGroup.GetValue(Faehigkeit, NameOf(Faehigkeit.Sortierung)), String)
                Dim Benennung = DirectCast(bindingGroup.GetValue(Faehigkeit, NameOf(Faehigkeit.Benennung)), String)

                Dim ErrorContent As New List(Of String)
                If String.IsNullOrWhiteSpace(Sortierung) Then
                    ErrorContent.Add("Sortierung ist eine Pflichtangabe")
                End If
                If Not IsNumeric(Sortierung) Then
                    ErrorContent.Add("Sortierung muß eine Zahl sein")
                End If
                If String.IsNullOrWhiteSpace(Benennung) Then
                    ErrorContent.Add("Benennung ist eine Pflichtangabe")
                End If

                Dim ValueListe = Controller.AppController.AktuellerClub.AlleFaehigkeiten.Where(Function(Fa) Fa.FaehigkeitID = Faehigkeit.FaehigkeitID)
                Dim Faehigkeitenliste = Controller.AppController.AktuellerClub.AlleFaehigkeiten.Except(ValueListe)
                If Faehigkeitenliste.ToList.Where(Function(Fa) Fa.Benennung = Benennung).Any Then
                    ErrorContent.Add("Fähigkeit mit dieser Benennung ist bereits vorhanden")
                End If
                If Faehigkeitenliste.ToList.Where(Function(Fa) Fa.Sortierung = Sortierung).Any Then
                    ErrorContent.Add("Fähigkeit mit dieser Sortierungszahl ist bereits vorhanden")
                End If
                If ErrorContent.Count > 0 Then
                    Return New ValidationResult(False, ErrorContent)
                End If
            End If

            Return ValidationResult.ValidResult
        End Function
    End Class
End Namespace
