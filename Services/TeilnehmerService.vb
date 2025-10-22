Imports Groupies.Entities.Generation4

Namespace Services

    Public Class TeilnehmerService

        Public Shared Event TeilnehmerGeaendert As EventHandler(Of EventArgs)

        Public Sub New()
        End Sub

        Protected Overridable Sub OnTeilnehmerGeaendert(e As EventArgs)
            RaiseEvent TeilnehmerGeaendert(Me, e)
        End Sub

        ''' <summary>
        ''' Eine Liste von Teilnehmern in eine Gruppe einteilen und aus der Liste nicht zugewiesene Teilnehmer entfernen
        ''' </summary>
        ''' <param name="NeueTeilnehmerListe"></param>
        ''' <param name="Gruppe"></param>
        ''' <param name="Einteilung"></param>
        Public Sub TeilnehmerInGruppeEinteilen(NeueTeilnehmerListe As List(Of Teilnehmer), Gruppe As Gruppe, Einteilung As Einteilung)

            ' In Teilnehmerliste in Gruppe schreiben ...
            For Each Item In NeueTeilnehmerListe
                Gruppe.Mitgliederliste.Add(Item)
                Gruppe.MitgliederIDListe.Add(Item.Ident)
            Next

            ' ... aus NichtZugewieseneTeilnehmer entfernen
            For Each Teilnehmer In NeueTeilnehmerListe
                Einteilung.NichtZugewieseneTeilnehmerIDListe.Remove(Teilnehmer.Ident)
                Einteilung.NichtZugewieseneTeilnehmerListe.Remove(Einteilung.NichtZugewieseneTeilnehmerListe.Where(Function(T) T.Ident = Teilnehmer.Ident).Single)
            Next

            OnTeilnehmerGeaendert(EventArgs.Empty)

        End Sub

        ''' <summary>
        ''' Eine Liste von Teilnehmern aus einer Gruppe eintfernen und in die Liste nicht zugewiesene Teilnehmer hinzufügen.
        ''' </summary>
        ''' <param name="TeilnehmerListe"></param>
        ''' <param name="Gruppe"></param>
        ''' <param name="Einteilung"></param>
        Public Sub TeilnehmerAusGruppeEntfernen(TeilnehmerListe As List(Of Teilnehmer), Gruppe As Gruppe, Einteilung As Einteilung)

            ' Teilnehmerliste aus Gruppe entfernen ...
            For Each Tn In TeilnehmerListe
                Gruppe.Mitgliederliste.Remove(Gruppe.Mitgliederliste.Where(Function(M) M.Ident = Tn.Ident).Single)
                Gruppe.MitgliederIDListe.Remove(Tn.Ident)
            Next

            ' ... in NichtZugewieseneTeilnehmer schreiben
            For Each Tn In TeilnehmerListe
                Einteilung.NichtZugewieseneTeilnehmerListe.Add(Tn)
                Einteilung.NichtZugewieseneTeilnehmerIDListe.Add(Tn.Ident)
            Next

            OnTeilnehmerGeaendert(EventArgs.Empty)

        End Sub

        ''' <summary>
        ''' Eine Liste von Teilnehmern wird aus der Einteilung entfernt
        ''' </summary>
        ''' <param name="TeilnehmerListe"></param>
        ''' <param name="Einteilung"></param>
        Public Sub TeilnehmerEinteilungHinzufuegen(TeilnehmerListe As List(Of Teilnehmer), Gruppe As Gruppe, Einteilung As Einteilung)

            For Each Tn In TeilnehmerListe
                Einteilung.NichtZugewieseneTeilnehmerIDListe.Add(Tn.Ident)
                Einteilung.NichtZugewieseneTeilnehmerListe.Add(Tn)
            Next

            OnTeilnehmerGeaendert(EventArgs.Empty)

        End Sub

        ''' <summary>
        ''' Eine Liste von Teilnehmern wird aus der Einteilung entfernt
        ''' </summary>
        ''' <param name="TeilnehmerListe"></param>
        ''' <param name="Einteilung"></param>
        Public Sub TeilnehmerAusEinteilungEntfernen(TeilnehmerListe As List(Of Teilnehmer), Einteilung As Einteilung)

            For Each Tn In TeilnehmerListe
                Einteilung.NichtZugewieseneTeilnehmerIDListe.Remove(Tn.Ident)
                Einteilung.NichtZugewieseneTeilnehmerListe.Remove(Einteilung.NichtZugewieseneTeilnehmerListe.Where(Function(NZT) NZT.Ident = Tn.Ident).Single)
            Next

            OnTeilnehmerGeaendert(EventArgs.Empty)

        End Sub

    End Class
End Namespace
