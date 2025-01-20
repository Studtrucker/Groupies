Imports System.Globalization

Namespace ValidationRules

    Public Class GruppeValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
            Dim bindingGroup = DirectCast(value, BindingGroup)
            If bindingGroup.Items.Count = 1 Then
                'Dim Teilnehmer = DirectCast(bindingGroup.Items(0), Entities.Teilnehmer)
                'Dim Vorname = DirectCast(bindingGroup.GetValue(Teilnehmer, NameOf(Teilnehmer.Vorname)), String)
                'Dim Nachname = DirectCast(bindingGroup.GetValue(Teilnehmer, NameOf(Teilnehmer.Nachname)), String)
                'Dim Leistungsstand = DirectCast(bindingGroup.GetValue(Teilnehmer, NameOf(Teilnehmer.Leistungsstand)), String)

                Dim ErrorContent As New List(Of String)
                'If String.IsNullOrWhiteSpace(Vorname) Then
                '    ErrorContent.Add("Vorname ist eine Pflichtangabe")
                'End If
                'If String.IsNullOrWhiteSpace(Nachname) Then
                '    ErrorContent.Add("Nachname ist eine Pflichtangabe")
                'End If

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

