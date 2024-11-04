Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Text

Namespace Entities

    Public Class TeilnehmerCollection
        Inherits ObservableCollection(Of Teilnehmer)
        Implements IEnumerable(Of Teilnehmer)

#Region "Konstruktor"
        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Teilnehmerliste As IEnumerable(Of Teilnehmer))
            MyBase.New
            Teilnehmerliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

#End Region

#Region "Porperties IEnumerable(String) Rückgabe"
        ''' <summary>
        ''' Die Teilnehmer mit VorUndNachname ungeordnet
        ''' </summary>
        ''' <returns></returns>
        Public Property Teilnehmerinfo As IEnumerable(Of String) = Me.Select(Function(Tn) Tn.VorUndNachname)

        ''' <summary>
        ''' Die Teilnehmer mit VorUndNachname
        '''  Geordnet:
        '''  Nachname aufsteigend
        '''  Vorname aufsteigend
        ''' </summary>
        ''' <returns></returns>
        Public Property TeilnehmerinfoGeordnet As IEnumerable(Of String) =
            OrderBy(Function(Tn) Tn.Nachname) _
            .ThenBy(Function(Tn) Tn.Vorname) _
            .Select(Function(Tn) Tn.AusgabeTeilnehmerinfo)

        ''' <summary>
        '''  Die Teilnehmer mit VorUndNachname, Leistungsstufe
        '''  Geordnet:
        '''  Leistungsstand absteigend
        '''  Nachname aufsteigend
        '''  Vorname aufsteigend
        ''' </summary>
        ''' <returns></returns>
        Public Property TrainerinfoGeordnet As IEnumerable(Of String) =
            OrderByDescending(Function(Tr) Tr.Leistungsstand.Sortierung) _
            .ThenBy(Function(Tr) Tr.Nachname) _
            .ThenBy(Function(Tr) Tr.Vorname) _
            .Select(Function(Tn) Tn.AusgabeTrainerinfo)

        'Public Overloads Sub Add(Teilnehmer As Teilnehmer, Vergleichsliste As TeilnehmerCollection)
        '    Fehler.Clear()
        '    PruefeSortierungVorhanden(Leistungsstufe.Sortierung, Vergleichsliste)
        '    PruefeBenennungVorhanden(Leistungsstufe.Benennung, Vergleichsliste)
        '    If Fehler.Count > 0 Then
        '        Dim FText As New StringBuilder
        '        FText.Append($"Leistungsstufe kann nicht erstellt werden.{Environment.NewLine}")
        '        Fehler.ForEach(Sub(F) FText.Append($"{F}{Environment.NewLine}"))
        '        Throw New GroupiesException("Leistungsstufe kann nicht erstellt werden", New GroupiesException($"{FText}"))
        '    End If
        '    Add(Leistungsstufe)
        'End Sub

        'Private Sub PruefeSortierungVorhanden(Kennzahl As Integer?, AktuelleListe As LeistungsstufeCollection)
        '    If AktuelleListe.Where(Function(Ls) Ls.Sortierung = Kennzahl).Count > 0 Then
        '        Fehler.Add($"Sortierung {Kennzahl} ist schon vorhanden")
        '    End If
        'End Sub
        'Private Sub PruefeBenennungVorhanden(Benennung As String, AktuelleListe As LeistungsstufeCollection)
        '    If AktuelleListe.Where(Function(Ls) Ls.Benennung = Benennung).Count > 0 Then
        '        Fehler.Add($"Benennung {Benennung} ist schon vorhanden")
        '    End If
        'End Sub

#End Region

#Region "Porperties IEnumerable(Teilnehmer) Rückgabe"
        ''' <summary>
        ''' Die Teilnehmer ungeordnet
        ''' </summary>
        ''' <returns></returns>
        Public Property Teilnehmer As IEnumerable(Of Teilnehmer) = Me

        ''' <summary>
        ''' Die Teilnehmer mit VorUndNachname
        '''  Geordnet:
        '''  Nachname aufsteigend
        '''  Vorname aufsteigend
        ''' </summary>
        ''' <returns></returns>
        Public Property Geordnet As IEnumerable(Of Teilnehmer) =
            OrderBy(Function(Tn) Tn.Nachname) _
            .ThenBy(Function(Tn) Tn.Vorname)

        ''' <summary>
        '''  Die Teilnehmer mit VorUndNachname, Leistungsstufe
        '''  Geordnet:
        '''  Leistungsstand absteigend
        '''  Nachname aufsteigend
        '''  Vorname aufsteigend
        ''' </summary>
        ''' <returns></returns>
        Public Property TeilnehmerMitLeistungsstufeGeordnet As IEnumerable(Of Teilnehmer) =
            OrderByDescending(Function(Tn) Tn.Leistungsstand.Sortierung) _
            .ThenBy(Function(Tn) Tn.Nachname) _
            .ThenBy(Function(Tn) Tn.Vorname)

#End Region

#Region "Funktionen und Methoden"

        'Public Overloads Sub Add(Teilnehmer As Teilnehmer)
        '    Add(Teilnehmer)
        'End Sub

        'Public Overloads Sub Remove(Teilnehmer As Teilnehmer)
        '    Remove(Teilnehmer)
        'End Sub

#End Region


#Region "Beispiel Gruppierung"

        Public Property GruppeLeistungNachnameVorname =
            OrderBy(Function(TN) TN.Nachname) _
            .ThenBy(Function(TN) TN.Vorname) _
            .GroupBy(Function(TN) TN.Leistungsstand.Sortierung) _
            .OrderByDescending(Function(TN) TN.Key) _
            .Select(Function(TNG) TNG.ToList.Select(Function(Tt) Tt))

#End Region

    End Class
End Namespace
