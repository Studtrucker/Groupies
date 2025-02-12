Imports System.Globalization

Namespace ValidationRules

    Public Class TrainerValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
            Dim bindingGroup = DirectCast(value, BindingGroup)
            If bindingGroup.Items.Count = 1 Then
                Dim Trainer = DirectCast(bindingGroup.Items(0), Entities.Trainer)
                Dim Vorname = DirectCast(bindingGroup.GetValue(Trainer, NameOf(Trainer.Vorname)), String)
                Dim Spitzname = DirectCast(bindingGroup.GetValue(Trainer, NameOf(Trainer.Spitzname)), String)

                Dim ErrorContent As New List(Of String)
                If String.IsNullOrWhiteSpace(Vorname) Then
                    ErrorContent.Add("Vorname ist eine Pflichtangabe")
                End If
                If String.IsNullOrWhiteSpace(Spitzname) Then
                    ErrorContent.Add("Spitzname ist eine Pflichtangabe")
                End If

                Dim ValueListe = Controller.AppController.AktuellerClub.AlleTrainer.Where(Function(Tr) Tr.TrainerID = Trainer.TrainerID)
                Dim Trainerliste = Controller.AppController.AktuellerClub.AlleTrainer.Except(ValueListe)
                If Trainerliste.ToList.Where(Function(Tn) Tn.Spitzname = Spitzname).Any Then
                    ErrorContent.Add("Trainer mit diesem Spitzname ist bereits vorhanden")
                End If
                If Trainerliste.ToList.Where(Function(Tn) Tn.Vorname = Vorname).Any Then
                    'ErrorContent.Add("Trainer mit diesem Vorname ist bereits vorhanden")
                End If
                If ErrorContent.Count > 0 Then
                    Return New ValidationResult(False, ErrorContent)
                End If
            End If

            Return ValidationResult.ValidResult
        End Function
    End Class
End Namespace
