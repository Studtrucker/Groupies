Imports System.Globalization
Imports Groupies.Controller
Imports Groupies.Entities.Generation4
Imports Groupies.Services

Namespace ValidationRules

    Public Class SortierungValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult

            If value.Sortierung < 1 Then
                Return New ValidationResult(False, "Sortierung muss größer Null sein.")
            End If
            If String.IsNullOrWhiteSpace(value.Sortierung) Then
                Return New ValidationResult(False, "Sortierung muss eingetragen sein.")
            End If
            Return GetEindeutigkeit(value)

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Gruppe As Gruppenstamm) As ValidationResult
            If ServiceProvider.DateiService.AktuellerClub.Gruppenstammliste.Where(Function(Gr) Gr.Sortierung = Gruppe.Sortierung AndAlso Gr.Ident <> Gruppe.Ident).Any() Then
                Return New ValidationResult(False, "Die Sortierung muss eindeutig sein.")
            End If
            Return ValidationResult.ValidResult
        End Function

        Public Function GetEindeutigkeit(Faehigkeit As Faehigkeit) As ValidationResult
            If ServiceProvider.DateiService.AktuellerClub.Faehigkeitenliste.Where(Function(Fk) Fk.Sortierung = Faehigkeit.Sortierung AndAlso Fk.FaehigkeitID <> Faehigkeit.FaehigkeitID).Any() Then
                Return New ValidationResult(False, "Die Sortierung muss eindeutig sein.")
            End If
            Return ValidationResult.ValidResult
        End Function

        Public Function GetEindeutigkeit(Leistungsstufe As Leistungsstufe) As ValidationResult
            If ServiceProvider.DateiService.AktuellerClub.Leistungsstufenliste.Where(Function(Ls) Ls.Sortierung = Leistungsstufe.Sortierung AndAlso Ls.Ident <> Leistungsstufe.Ident).Any() Then
                Return New ValidationResult(False, "Die Sortierung muss eindeutig sein.")
            End If
            Return ValidationResult.ValidResult
        End Function

        Public Function GetEindeutigkeit(Einteilung As Einteilung) As ValidationResult
            If ServiceProvider.DateiService.AktuellerClub.Einteilungsliste.Where(Function(Ls) Ls.Sortierung = Einteilung.Sortierung AndAlso Ls.Ident <> Einteilung.Ident).Any() Then
                Return New ValidationResult(False, "Die Sortierung muss eindeutig sein.")
            End If
            Return ValidationResult.ValidResult
        End Function

    End Class

End Namespace
