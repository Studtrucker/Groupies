Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

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

#Region "Porperties String Rückgabe"
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
            OrderByDescending(Function(o) o.Leistungsstand.Sortierung) _
            .ThenBy(Function(o) o.Nachname) _
            .ThenBy(Function(o) o.Vorname) _
            .Select(Function(Tn) Tn.AusgabeTrainerinfo)



#End Region

#Region "Porperties IEnumerable Rückgabe"
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
        Public Property TeilnehmerGeordnet As IEnumerable(Of Teilnehmer) =
            OrderByDescending(Function(Tn) Tn.Nachname) _
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
            OrderByDescending(Function(o) o.Leistungsstand.Sortierung) _
            .ThenBy(Function(o) o.Nachname) _
            .ThenBy(Function(o) o.Vorname)

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
