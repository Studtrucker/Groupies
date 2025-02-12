Imports System.Globalization

Namespace ValidationRules


    Public Class EinteilungValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
            Dim bindingGroup = DirectCast(value, BindingGroup)
            If bindingGroup.Items.Count = 1 Then
                Dim Einteilung = DirectCast(bindingGroup.Items(0), Entities.Einteilung)
                Dim Benennung = DirectCast(bindingGroup.GetValue(Einteilung, NameOf(Einteilung.Benennung)), String)

                Dim ErrorContent As New List(Of String)
                If String.IsNullOrWhiteSpace(Benennung) Then
                    ErrorContent.Add("Benennung ist eine Pflichtangabe")
                End If

                '' Zuerst das Objekt aus dem Value mit Hilfe der ID ermitteln (Rückgabe = IEnumerable(of Object))
                'Dim ValueListe = Controller.AppController.AktuellerClub.Leistungsstufenliste.Where(Function(Ls) Ls.LeistungsstufeID = Leistungsstufe.LeistungsstufeID)
                '' Dieses Objekt aus der Gesamt-Objektliste entfernen
                'Dim Leistungsstufenliste = Controller.AppController.AktuellerClub.Leistungsstufenliste.Except(ValueListe)
                '' Jetzt kann geprüft werden, ob es ein gleich benanntes Objekt in der Restliste gibt
                'If Leistungsstufenliste.ToList.Where(Function(LS) LS.Benennung = Benennung).Any Then
                '    ErrorContent.Add("Eine Einteilung mit dieser Benennung ist bereits vorhanden")
                'End If
                If ErrorContent.Count > 0 Then
                    Return New ValidationResult(False, ErrorContent)
                End If
            End If
            Return ValidationResult.ValidResult
        End Function
    End Class

End Namespace
