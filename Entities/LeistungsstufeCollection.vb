Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Text

Namespace Entities

    Public Class LeistungsstufeCollection
        Inherits ObservableCollection(Of Leistungsstufe)
        Implements IEnumerable(Of Leistungsstufe)

        Private Fehler As New List(Of String)

        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Leistungsstufenliste As IEnumerable(Of Leistungsstufe))
            MyBase.New
            Leistungsstufenliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

        Public Overloads Sub Add(Leistungsstufe As Leistungsstufe, Vergleichsliste As LeistungsstufeCollection)
            Fehler.Clear()
            PruefeSortierungVorhanden(Leistungsstufe.Sortierung, Vergleichsliste)
            PruefeBenennungVorhanden(Leistungsstufe.Benennung, Vergleichsliste)
            If Fehler.Count > 0 Then
                Dim FText As New StringBuilder
                FText.Append($"Leistungsstufe kann nicht erstellt werden.{Environment.NewLine}")
                Fehler.ForEach(Sub(F) FText.Append($"{F}{Environment.NewLine}"))
                Throw New GroupiesException("Leistungsstufe kann nicht erstellt werden", New GroupiesException($"{FText}"))
            End If
            Add(Leistungsstufe)
        End Sub

        Private Sub PruefeSortierungVorhanden(Kennzahl As Integer?, AktuelleListe As LeistungsstufeCollection)
            If AktuelleListe.Where(Function(Ls) Ls.Sortierung = Kennzahl).Count > 0 Then
                Fehler.Add($"Sortierung {Kennzahl} ist schon vorhanden")
            End If
        End Sub
        Private Sub PruefeBenennungVorhanden(Benennung As String, AktuelleListe As LeistungsstufeCollection)
            If AktuelleListe.Where(Function(Ls) Ls.Benennung = Benennung).Count > 0 Then
                Fehler.Add($"Benennung {Benennung} ist schon vorhanden")
            End If
        End Sub

    End Class
End Namespace
