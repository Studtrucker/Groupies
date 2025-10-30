Imports System.Collections.ObjectModel
Imports System.Linq
Imports Groupies.Entities.Generation4
Imports Groupies.Services

Namespace Services

    Public NotInheritable Class CopyCommandFactory

        Private Sub New()
        End Sub

        ''' <summary>
        ''' Erzeugt ein RelayCommand(Of Einteilung) zum Verschieben einer Gruppe in eine Ziel-Einteilung.
        ''' CanExecute prüft jetzt:
        '''  - target ist ungleich Nothing
        '''  - source (Selected Gruppe) ist ungleich Nothing
        '''  - source ist in einer Einteilung vorhanden
        '''  - Ziel ist nicht die gleiche Einteilung wie Quelle
        '''  - Ziel enthält die Gruppe nicht bereits
        ''' </summary>
        Public Shared Function CreateGruppeCopyCommand(getSource As Func(Of Gruppe), Optional onCopied As Action = Nothing) As RelayCommand(Of Einteilung)
            Return New RelayCommand(Of Einteilung)(
                Sub(target)
                    Dim source = getSource()
                    If source Is Nothing OrElse target Is Nothing Then Return

                    Dim club = DateiService.AktuellerClub
                    If club Is Nothing Then Return

                    Dim sourceE = club.Einteilungsliste.FirstOrDefault(Function(e) e.Gruppenliste IsNot Nothing AndAlso e.Gruppenliste.Contains(source))
                    If sourceE Is Nothing OrElse sourceE.Ident = target.Ident Then Return

                    ' Hinzufügen zum Ziel
                    If target.Gruppenliste Is Nothing Then target.Gruppenliste = New GruppeCollection()
                    target.Gruppenliste.Add(source)
                    If target.GruppenIDListe Is Nothing Then target.GruppenIDListe = New ObservableCollection(Of Guid)
                    If Not target.GruppenIDListe.Contains(source.Ident) Then target.GruppenIDListe.Add(source.Ident)

                    onCopied?.Invoke()
                End Sub,
                Function(target)
                    If target Is Nothing Then Return False
                    Dim source = getSource()
                    If source Is Nothing Then Return False

                    Dim club = DateiService.AktuellerClub
                    If club Is Nothing OrElse club.Einteilungsliste Is Nothing Then Return False

                    Dim sourceE = club.Einteilungsliste.FirstOrDefault(Function(e) e.Gruppenliste IsNot Nothing AndAlso e.Gruppenliste.Contains(source))
                    If sourceE Is Nothing Then Return False

                    ' Quelle und Ziel dürfen nicht gleich sein
                    If sourceE.Ident = target.Ident Then Return False

                    ' Ziel darf die Gruppe noch nicht enthalten
                    If target.Gruppenliste IsNot Nothing AndAlso target.Gruppenliste.Any(Function(g) g.Ident = source.Ident) Then
                        Return False
                    End If

                    Return True
                End Function)
        End Function

        ''' <summary>
        ''' Erzeugt ein RelayCommand(Of Einteilung) zum Verschieben ganzer Einteilungsinhalte in eine Ziel-Einteilung.
        ''' CanExecute prüft nun zusätzlich, ob das Ziel bereits IDs (Gruppen/Teilnehmer/Trainer) enthält und verhindert dann den Transfer.
        ''' </summary>
        Public Shared Function CreateEinteilungCopyCommand(getSource As Func(Of Einteilung), Optional onCopied As Action = Nothing) As RelayCommand(Of Einteilung)
            Return New RelayCommand(Of Einteilung)(
                Sub(target)
                    Dim source = getSource()
                    If source Is Nothing OrElse target Is Nothing Then Return

                    Dim club = DateiService.AktuellerClub
                    If club Is Nothing Then Return

                    If source.Ident = target.Ident Then Return

                    ' Verschiebe alle Gruppen
                    source.Gruppenliste?.ToList.ForEach(Sub(g)
                                                            If target.Gruppenliste Is Nothing Then target.Gruppenliste = New GruppeCollection()
                                                            target.Gruppenliste.Add(g)
                                                            If target.GruppenIDListe Is Nothing Then target.GruppenIDListe = New ObservableCollection(Of Guid)
                                                            If Not target.GruppenIDListe.Contains(g.Ident) Then target.GruppenIDListe.Add(g.Ident)
                                                        End Sub)

                    ' Verschiebe gruppenlose Teilnehmer
                    source.NichtZugewieseneTeilnehmerListe?.ToList.ForEach(Sub(t)
                                                                               If target.NichtZugewieseneTeilnehmerListe Is Nothing Then
                                                                                   target.NichtZugewieseneTeilnehmerListe = New TeilnehmerCollection
                                                                                   target.NichtZugewieseneTeilnehmerIDListe = New ObservableCollection(Of Guid)
                                                                               End If
                                                                               target.NichtZugewieseneTeilnehmerListe.Add(t)
                                                                               If Not target.NichtZugewieseneTeilnehmerIDListe.Contains(t.Ident) Then target.NichtZugewieseneTeilnehmerIDListe.Add(t.Ident)
                                                                           End Sub)

                    ' Verschiebe verfügbare Trainer
                    source.VerfuegbareTrainerListe?.ToList.ForEach(Sub(t)
                                                                       If target.VerfuegbareTrainerListe Is Nothing Then
                                                                           target.VerfuegbareTrainerListe = New TrainerCollection
                                                                           target.VerfuegbareTrainerIDListe = New ObservableCollection(Of Guid)
                                                                       End If
                                                                       target.VerfuegbareTrainerListe.Add(t)
                                                                       If Not target.VerfuegbareTrainerIDListe.Contains(t.TrainerID) Then target.VerfuegbareTrainerIDListe.Add(t.TrainerID)
                                                                   End Sub)

                    onCopied?.Invoke()
                End Sub,
                Function(target)
                    If target Is Nothing Then Return False
                    Dim source = getSource()
                    If source Is Nothing Then Return False
                    If source.Ident = target.Ident Then Return False

                    ' Verhindere Transfer, wenn Ziel schon IDs enthält (heuristische Prüfung wie in VM gewünscht)
                    If (target.GruppenIDListe IsNot Nothing AndAlso target.GruppenIDListe.Count > 0) OrElse
                       (target.NichtZugewieseneTeilnehmerIDListe IsNot Nothing AndAlso target.NichtZugewieseneTeilnehmerIDListe.Count > 0) OrElse
                       (target.VerfuegbareTrainerIDListe IsNot Nothing AndAlso target.VerfuegbareTrainerIDListe.Count > 0) Then
                        Return False
                    End If

                    Return True
                End Function)
        End Function

    End Class
End Namespace