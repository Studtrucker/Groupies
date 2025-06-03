Imports System.Globalization

Namespace ValidationRules

    Public Class GruppeValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
            Dim bindingGroup = DirectCast(value, BindingGroup)
            If bindingGroup.Items.Count = 1 Then
                Dim Gruppe = DirectCast(bindingGroup.Items(0), Entities.Gruppe)
                Dim AusgabeTeilnehmerinfo = DirectCast(bindingGroup.GetValue(Gruppe, NameOf(Gruppe.Benennung)), String)
                Dim Benennung = DirectCast(bindingGroup.GetValue(Gruppe, NameOf(Gruppe.Alias)), String)
                Dim Sortierung = DirectCast(bindingGroup.GetValue(Gruppe, NameOf(Gruppe.Sortierung)), String)
                Dim Leistungsstand = DirectCast(bindingGroup.GetValue(Gruppe, NameOf(Gruppe.Leistungsstufe)), String)

                Dim ErrorContent As New List(Of String)
                If String.IsNullOrWhiteSpace(AusgabeTeilnehmerinfo) Then
                    ErrorContent.Add("Ausgabe Teilnehmerinfo ist eine Pflichtangabe")
                End If
                If String.IsNullOrWhiteSpace(Benennung) Then
                    ErrorContent.Add("Benennung ist eine Pflichtangabe")
                End If

                If IsNumeric(Sortierung) Then
                    If Groupies.Controller.AppController.AktuellerClub.SelectedEinteilung.Gruppenliste.ToList.Where(Function(LS) LS.Sortierung = Sortierung).Any Then
                        ErrorContent.Add("Gruppe mit dieser Sortierungszahl ist bereits vorhanden")
                    End If
                End If

                'Dim ValueListe = Controller.AppController.CurrentClub.AlleTeilnehmer.Where(Function(Tn) Tn.TeilnehmerID = Teilnehmer.TeilnehmerID)
                'Dim Teilnehmerliste = Controller.AppController.CurrentClub.AlleTeilnehmer.Except(ValueListe)
                'If Teilnehmerliste.ToList.Where(Function(Tn) Tn.Vorname = Vorname And Tn.Nachname = Nachname).Any Then
                '    ErrorContent.Add("Teilnehmer mit diesem Namen ist bereits vorhanden")
                'End If
                'If String.IsNullOrWhiteSpace(Leistungsstand) Or Leistungsstand.ToLower = "Level unbekannt" Then
                '    ErrorContent.Add("Leistungsstand muss angegeben werden")
                'End If
                If ErrorContent.Count > 0 Then
                    Return New ValidationResult(False, ErrorContent)
                End If
            End If

            Return ValidationResult.ValidResult
        End Function
    End Class
End Namespace

