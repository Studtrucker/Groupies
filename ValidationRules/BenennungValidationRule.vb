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
                If AppController.AktuellerClub.AlleLeistungsstufen.ToList.Select(Function(Ls) $"{Ls.Benennung.ToLower}").Contains(Objekt.Benennung.ToLower) Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Faehigkeit) As ValidationResult

            If Objekt.Benennung IsNot Nothing Then
                If AppController.AktuellerClub.AlleFaehigkeiten.ToList.Select(Function(O) $"{O.Benennung.ToLower}").Contains(Objekt.Benennung.ToLower) Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Gruppe) As ValidationResult

            If Objekt.Benennung IsNot Nothing Then
                If AppController.AktuellerClub.AlleGruppen.ToList.Select(Function(O) $"{O.Benennung.ToLower}").Contains(Objekt.Benennung.ToLower) Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Einteilung) As ValidationResult

            If Objekt.Benennung IsNot Nothing Then
                If AppController.AktuellerClub.Einteilungsliste.ToList.Select(Function(O) $"{O.Benennung.ToLower}").Contains(Objekt.Benennung.ToLower) Then
                    Return New ValidationResult(False, $"{Objekt.Benennung} wird bereits verwendet. Die Benennung muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Objekt As Trainer) As ValidationResult

            If Objekt.Spitzname IsNot Nothing Then
                If AppController.AktuellerClub.AlleTrainer.ToList.Select(Function(O) $"{O.Spitzname.ToLower}").Contains(Objekt.Spitzname.ToLower) Then
                    Return New ValidationResult(False, $"{Objekt.Spitzname} wird bereits verwendet. Der Alias muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function


        Public Function GetEindeutigkeit(Objekt As Teilnehmer) As ValidationResult

            If Objekt.VorUndNachname IsNot Nothing Then
                If AppController.AktuellerClub.AlleTeilnehmer.ToList.Select(Function(O) $"{O.VorUndNachname.ToLower}").Contains(Objekt.VorUndNachname.ToLower) Then
                    Return New ValidationResult(False, $"{Objekt.VorUndNachname} wird bereits verwendet. Die Kombination Vor- und Nachname muss aber eindeutig sein.")
                End If
            End If

            Return ValidationResult.ValidResult

        End Function

    End Class

End Namespace

