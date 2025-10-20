Imports Groupies.Entities.Generation4

Namespace Services

    Public Class TeilnehmerService

        Public Shared Event TeilnehmerGeaendert As EventHandler(Of EventArgs)
        Public Sub New()
        End Sub

        Protected Overridable Sub OnTrainerGeaendert(e As EventArgs)
            RaiseEvent TeilnehmerGeaendert(Me, e)
        End Sub

        ''' <summary>
        ''' Teilnehmer in Gruppe einteilen und aus NichtZugewieseneTeilnehmer entfernen
        ''' </summary>
        ''' <param name="NeuerTeilnehmerIDListe"></param>
        ''' <param name="GruppenID"></param>
        ''' <param name="EinteilungID"></param>
        Public Sub TeilnehmerInGruppeEinteilen(NeuerTeilnehmerIDListe As List(Of Guid), GruppenID As Guid, EinteilungID As Guid)

            ' In Teilnehmerliste in Gruppe schreiben ...
            Dim Gruppe = DateiService.AktuellerClub.Einteilungsliste.Where(Function(EL) EL.Ident = EinteilungID).Single.Gruppenliste.Where(Function(G) G.Ident = GruppenID).Single
            For Each ID In NeuerTeilnehmerIDListe
                Gruppe.Mitgliederliste.Add(TeilnehmerLesen(ID))
                Gruppe.MitgliederIDListe.Add(ID)
            Next

            ' ... aus NichtZugewieseneTeilnehmer entfernen
            Dim NichtZugewieseneTeilnehmerListe = DateiService.AktuellerClub.Einteilungsliste.Where(Function(EL) EL.Ident = EinteilungID).Single.NichtZugewieseneTeilnehmerListe
            Dim NichtZugewieseneTeilnehmerIDListe = DateiService.AktuellerClub.Einteilungsliste.Where(Function(EL) EL.Ident = EinteilungID).Single.NichtZugewieseneTeilnehmerIDListe
            For Each ID In NeuerTeilnehmerIDListe
                NichtZugewieseneTeilnehmerListe.Remove(TeilnehmerLesen(ID))
                NichtZugewieseneTeilnehmerIDListe.Remove(ID)
            Next

            OnTrainerGeaendert(EventArgs.Empty)

        End Sub

        Private Function TeilnehmerLesen(NeuerTeilnehmerIDListe As List(Of Guid)) As List(Of Teilnehmer)

            ' Guard: keine IDs
            If NeuerTeilnehmerIDListe Is Nothing OrElse NeuerTeilnehmerIDListe.Count = 0 Then
                Return New List(Of Teilnehmer)()
            End If

            Dim alleTeilnehmer = If(DateiService.AktuellerClub, Nothing)?.Teilnehmerliste
            If alleTeilnehmer Is Nothing Then
                ' Wenn keine Teilnehmerliste vorhanden ist, die gleiche Anzahl an Nothing-Einträgen zurückgeben
                Dim empties As New List(Of Teilnehmer)(NeuerTeilnehmerIDListe.Count)
                For Each id In NeuerTeilnehmerIDListe
                    empties.Add(Nothing)
                Next
                Return empties
            End If

            ' Dictionary einmal aufbauen für O(1)-Lookups
            Dim lookup As New Dictionary(Of Guid, Teilnehmer)(alleTeilnehmer.Count)
            For Each t In alleTeilnehmer
                If Not lookup.ContainsKey(t.Ident) Then
                    lookup.Add(t.Ident, t)
                End If
            Next

            Dim result As New List(Of Teilnehmer)(NeuerTeilnehmerIDListe.Count)
            Dim tmp As Teilnehmer = Nothing
            For Each id In NeuerTeilnehmerIDListe
                If lookup.TryGetValue(id, tmp) Then
                    result.Add(tmp)
                Else
                    result.Add(Nothing)
                End If
            Next

            Return result
        End Function

        Private Function TeilnehmerLesen(ID As Guid) As Teilnehmer
            Return DateiService.AktuellerClub.Teilnehmerliste.Where(Function(T) T.Ident = ID).SingleOrDefault
        End Function

    End Class
End Namespace
