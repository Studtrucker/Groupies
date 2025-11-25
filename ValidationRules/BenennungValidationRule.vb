Imports Groupies.Entities.Generation4
Imports Groupies.ServiceproviderNamespace

Namespace ValidationRules

    Public Class BenennungValidationRule
        Inherits ValidationRule
        Public Overrides Function Validate(value As Object, cultureInfo As Globalization.CultureInfo) As ValidationResult

            Return GetEindeutigkeit(value)
            Return ValidationResult.ValidResult

        End Function


        Public Function GetEindeutigkeit(Objekt As Leistungsstufe) As ValidationResult

            If Objekt.Benennung IsNot Nothing Then
                Dim DS = Services.ServiceProvider.DateiService
                If DS.AktuellerClub.Leistungsstufenliste.Where(Function(o) o.Benennung.ToLower = Objekt.Benennung.ToLower AndAlso o.Ident <> Objekt.Ident).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Faehigkeit) As ValidationResult

            If Objekt.Benennung IsNot Nothing Then
                Dim DS = Services.ServiceProvider.DateiService
                If DS.AktuellerClub.Faehigkeitenliste.Where(Function(o) o.Benennung.ToLower = Objekt.Benennung.ToLower AndAlso o.FaehigkeitID <> Objekt.FaehigkeitID).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Gruppenstamm) As ValidationResult

            If Objekt.Benennung IsNot Nothing Then
                Dim DS = Services.ServiceProvider.DateiService
                If DS.AktuellerClub.Gruppenstammliste.Where(Function(o) o.Benennung.ToLower = Objekt.Benennung.ToLower AndAlso o.Ident <> Objekt.Ident).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Einteilung) As ValidationResult

            If Objekt.Benennung IsNot Nothing Then
                Dim DS = Services.ServiceProvider.DateiService
                If DS.AktuellerClub.Einteilungsliste.Where(Function(o) o.Benennung.ToLower = Objekt.Benennung.ToLower AndAlso o.Ident <> Objekt.Ident).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Trainer) As ValidationResult

            If Objekt.Alias IsNot Nothing Then
                Dim DS = Services.ServiceProvider.DateiService
                If DS.AktuellerClub.Trainerliste.Where(Function(o) o.Alias.ToLower = Objekt.Alias.ToLower AndAlso o.TrainerID <> Objekt.TrainerID).Any() Then
                    Return New ValidationResult(False, $"{Objekt.Alias} wird bereits verwendet. Der Alias muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function


        Public Function GetEindeutigkeit(Objekt As Teilnehmer) As ValidationResult

            If Objekt.VorUndNachname IsNot Nothing Then
                Dim DS = Groupies.Services.ServiceProvider.DateiService
                If DS.AktuellerClub.Teilnehmerliste.Where(Function(o) o.VorUndNachname.ToLower = Objekt.VorUndNachname.ToLower AndAlso o.Ident <> Objekt.Ident).Any() Then
                    Return New ValidationResult(False, $"{Objekt.VorUndNachname} wird bereits verwendet. Die Kombination Vor- und Nachname muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

    End Class

End Namespace

