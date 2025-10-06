Imports Groupies.Entities
Imports Groupies.Controller

Namespace ValidationRules

    Public Class BenennungValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As Globalization.CultureInfo) As ValidationResult

            Return GetEindeutigkeit(value)
            Return ValidationResult.ValidResult

        End Function


        Public Function GetEindeutigkeit(Objekt As Leistungsstufe) As ValidationResult

            If Objekt.Benennung IsNot Nothing Then
                If Services.DateiService.AktuellerClub.AlleLeistungsstufen.Where(Function(o) o.Benennung.ToLower = Objekt.Benennung.ToLower AndAlso o.Ident <> Objekt.Ident).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Faehigkeit) As ValidationResult

            If Objekt.Benennung IsNot Nothing Then
                If Services.DateiService.AktuellerClub.AlleFaehigkeiten.Where(Function(o) o.Benennung.ToLower = Objekt.Benennung.ToLower AndAlso o.FaehigkeitID <> Objekt.FaehigkeitID).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Gruppe) As ValidationResult

            If Objekt.Benennung IsNot Nothing Then
                If Services.DateiService.AktuellerClub.AlleGruppen.Where(Function(o) o.Benennung.ToLower = Objekt.Benennung.ToLower AndAlso o.GruppenID <> Objekt.GruppenID).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Einteilung) As ValidationResult

            If Objekt.Benennung IsNot Nothing Then
                If Services.DateiService.AktuellerClub.AlleEinteilungen.Where(Function(o) o.Benennung.ToLower = Objekt.Benennung.ToLower AndAlso o.EinteilungID <> Objekt.EinteilungID).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Trainer) As ValidationResult

            If Objekt.Spitzname IsNot Nothing Then
                If Services.DateiService.AktuellerClub.AlleTrainer.Where(Function(o) o.Spitzname.ToLower = Objekt.Spitzname.ToLower AndAlso o.TrainerID <> Objekt.TrainerID).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Spitzname} wird bereits verwendet. Der Alias muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function


        Public Function GetEindeutigkeit(Objekt As Teilnehmer) As ValidationResult

            If Objekt.VorUndNachname IsNot Nothing Then
                If Services.DateiService.AktuellerClub.AlleTeilnehmer.Where(Function(o) o.VorUndNachname.ToLower = Objekt.VorUndNachname.ToLower AndAlso o.TeilnehmerID <> Objekt.TeilnehmerID).Any() Then
                    Return New ValidationResult(False, $"{Objekt.VorUndNachname} wird bereits verwendet. Die Kombination Vor- und Nachname muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

    End Class

End Namespace

