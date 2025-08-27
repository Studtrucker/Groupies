Imports System.Globalization
Imports Groupies.Controller
Imports Groupies.Entities

Namespace ValidationRules

    Public Class SortierungValidationRule
        Inherits ValidationRule

        Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult

            If value.Sortierung < 0 Then
                Return New ValidationResult(False, "Sortierung darf nicht negativ sein.")
            End If
            If String.IsNullOrWhiteSpace(value.Sortierung) Then
                Return New ValidationResult(False, "Sortierung muss eingetragen sein.")
            End If
            Return GetEindeutigkeit(value)

            Return ValidationResult.ValidResult

        End Function

        Public Function GetEindeutigkeit(Gruppe As Gruppe) As ValidationResult
            If Services.DateiService.AktuellerClub.AlleGruppen.Where(Function(Gr) Gr.Sortierung = Gruppe.Sortierung AndAlso Gr.GruppenID <> Gruppe.GruppenID).Any() Then
                Return New ValidationResult(False, "Die Sortierung muss eindeutig sein.")
            End If
            Return ValidationResult.ValidResult
        End Function

        Public Function GetEindeutigkeit(Faehigkeit As Faehigkeit) As ValidationResult
            If Services.DateiService.AktuellerClub.AlleFaehigkeiten.Where(Function(Fk) Fk.Sortierung = Faehigkeit.Sortierung AndAlso Fk.FaehigkeitID <> Faehigkeit.FaehigkeitID).Any() Then
                Return New ValidationResult(False, "Die Sortierung muss eindeutig sein.")
            End If
            Return ValidationResult.ValidResult
        End Function

        Public Function GetEindeutigkeit(Leistungsstufe As Leistungsstufe) As ValidationResult
            If Services.DateiService.AktuellerClub.AlleLeistungsstufen.Where(Function(Ls) Ls.Sortierung = Leistungsstufe.Sortierung AndAlso Ls.LeistungsstufeID <> Leistungsstufe.LeistungsstufeID).Any() Then
                Return New ValidationResult(False, "Die Sortierung muss eindeutig sein.")
            End If
            Return ValidationResult.ValidResult
        End Function

        Public Function GetEindeutigkeit(Einteilung As Einteilung) As ValidationResult
            If Services.DateiService.AktuellerClub.AlleEinteilungen.Where(Function(Ls) Ls.Sortierung = Einteilung.Sortierung AndAlso Ls.EinteilungID <> Einteilung.EinteilungID).Any() Then
                Return New ValidationResult(False, "Die Sortierung muss eindeutig sein.")
            End If
            Return ValidationResult.ValidResult
        End Function

    End Class

End Namespace
