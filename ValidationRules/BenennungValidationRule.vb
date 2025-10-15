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
                If Services.DateiService.AktuellerClub.Leistungsstufenliste.Where(Function(o) o.Benennung.ToLower = Objekt.Benennung.ToLower AndAlso o.Ident <> Objekt.Ident).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Faehigkeit) As ValidationResult

            If Objekt.Benennung IsNot Nothing Then
                If Services.DateiService.AktuellerClub.Faehigkeitenliste.Where(Function(o) o.Benennung.ToLower = Objekt.Benennung.ToLower AndAlso o.FaehigkeitID <> Objekt.FaehigkeitID).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Gruppe) As ValidationResult

            If Objekt.Benennung IsNot Nothing Then
                If Services.DateiService.AktuellerClub.Gruppenliste.Where(Function(o) o.Benennung.ToLower = Objekt.Benennung.ToLower AndAlso o.Ident <> Objekt.Ident).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Einteilung) As ValidationResult

            If Objekt.Benennung IsNot Nothing Then
                If Services.DateiService.AktuellerClub.Einteilungsliste.Where(Function(o) o.Benennung.ToLower = Objekt.Benennung.ToLower AndAlso o.Ident <> Objekt.Ident).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Trainer) As ValidationResult

            If Objekt.Spitzname IsNot Nothing Then
                If Services.DateiService.AktuellerClub.Trainerliste.Where(Function(o) o.Spitzname.ToLower = Objekt.Spitzname.ToLower AndAlso o.TrainerID <> Objekt.TrainerID).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Spitzname} wird bereits verwendet. Der Alias muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function


        Public Function GetEindeutigkeit(Objekt As Teilnehmer) As ValidationResult

            If Objekt.VorUndNachname IsNot Nothing Then
                If Services.DateiService.AktuellerClub.Teilnehmerliste.Where(Function(o) o.VorUndNachname.ToLower = Objekt.VorUndNachname.ToLower AndAlso o.Ident <> Objekt.Ident).Any() Then
                    Return New ValidationResult(False, $"{Objekt.VorUndNachname} wird bereits verwendet. Die Kombination Vor- und Nachname muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

    End Class

End Namespace

