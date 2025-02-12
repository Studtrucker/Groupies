Imports System.Globalization

Namespace ValidationRules


    Public Class LeistungsstufeValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
            Dim bindingGroup = DirectCast(value, BindingGroup)
            If bindingGroup.Items.Count = 1 Then
                Dim Leistungsstufe = DirectCast(bindingGroup.Items(0), Entities.Leistungsstufe)
                Dim Sortierung = CType(bindingGroup.GetValue(Leistungsstufe, NameOf(Leistungsstufe.Sortierung)), String)
                Dim Benennung = DirectCast(bindingGroup.GetValue(Leistungsstufe, NameOf(Leistungsstufe.Benennung)), String)

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

                ' Zuerst das Objekt aus dem Value mit Hilfe der ID ermitteln (Rückgabe = IEnumerable(of Object))
                Dim ValueListe = Controller.AppController.AktuellerClub.Leistungsstufenliste.Where(Function(Ls) Ls.LeistungsstufeID = Leistungsstufe.LeistungsstufeID)
                ' Dieses Objekt aus der Gesamt-Objektliste entfernen
                Dim Leistungsstufenliste = Controller.AppController.AktuellerClub.Leistungsstufenliste.Except(ValueListe)
                ' Jetzt kann geprüft werden, ob es ein gleich benanntes Objekt in der Restliste gibt
                If Leistungsstufenliste.ToList.Where(Function(LS) LS.Benennung = Benennung).Any Then
                    ErrorContent.Add("Leistungsstufe mit dieser Benennung ist bereits vorhanden")
                End If
                If IsNumeric(Sortierung) Then
                    If Leistungsstufenliste.ToList.Where(Function(LS) LS.Sortierung = Sortierung).Any Then
                        ErrorContent.Add("Leistungsstufe mit dieser Sortierungszahl ist bereits vorhanden")
                    End If
                End If
                If Leistungsstufe.Faehigkeiten.Count = 0 Then
                    ErrorContent.Add("Es muß mindestens eine Fähigkeit angegeben werden")
                End If
                If ErrorContent.Count > 0 Then
                        Return New ValidationResult(False, ErrorContent)
                    End If
                End If
            Return ValidationResult.ValidResult
        End Function
    End Class

End Namespace
