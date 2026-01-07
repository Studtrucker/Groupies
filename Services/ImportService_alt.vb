Imports System.Collections.ObjectModel
Imports System.Security.Cryptography
Imports System.Text
Imports System.Windows
Imports Groupies
Imports Groupies.Controller
Imports Groupies.Entities.Generation4
Imports Microsoft.Win32
Imports Excel = Microsoft.Office.Interop.Excel

Namespace Services

    Public Module ImportService_alt

#Region "Felder"

        Private ReadOnly _ofdDokument As New OpenFileDialog
        'Public Workbook As Excel.Workbook
        'Private _xlSheet As Excel.Worksheet
        'Private ReadOnly _xlCell As Excel.Range
        Private ReadOnly _skischule = New Club() With {.ClubName = "Skiclub"}

#End Region

#Region "Eigenschaften"

#End Region

#Region "Funktionen und Methoden"

        Public Function ImportTrainerUndTeilnehmerdaten() As Club

            Dim Pfad = StarteOpenFileDialog()
            If Pfad Is Nothing Then Exit Function

            ImportTrainer(Pfad)
            ImportTeilnehmer(Pfad)

            Return Nothing

        End Function



        Public Sub ImportTrainer()
            Dim Pfad = StarteOpenFileDialog()
            If Pfad Is Nothing Then Exit Sub
            ImportTrainer(Pfad)
        End Sub

        Public Sub ImportTrainer(Pfad As String)


            'Dim TraineranzahlVorImport = AppController.AktuellerClub.SelectedEinteilung.EinteilungAlleTrainer.Count
            Dim ImportTrainerliste As List(Of DataImport.Trainer)

            Try
                ImportTrainerliste = LeseTrainerAusDatasetKI(Pfad)
            Catch ex As Exception
                MessageBox.Show($"{ex.Message}{vbNewLine}{ex.InnerException.Message}", "Fehler beim Datenimport", MessageBoxButton.OK, MessageBoxImage.Error)
                Exit Sub
            End Try

            Dim club = ServiceProvider.DateiService.AktuellerClub

            ' ALle Trainer mit gültiger ID durchgehen
            For Each ImportedTrainer In ImportTrainerliste
                If ImportedTrainer.TrainerID <> Guid.Empty Then
                    ' Prüfen, ob der Trainer im Club bereits existiert
                    Dim ExistingTrainer As Trainer = club.Trainerliste.FirstOrDefault(Function(Tn) Tn.TrainerID = ImportedTrainer.TrainerID)
                    If ExistingTrainer IsNot Nothing Then
                        ' Trainer existiert bereits, ggf. Daten aktualisieren
                        ExistingTrainer.Vorname = ImportedTrainer.Vorname
                        ExistingTrainer.Nachname = ImportedTrainer.Nachname
                        ExistingTrainer.Alias = ImportedTrainer.Spitzname
                        ExistingTrainer.Telefonnummer = ImportedTrainer.Telefonnummer
                        ExistingTrainer.EMail = ImportedTrainer.eMail
                        ExistingTrainer.IstImImport = True
                        Continue For
                    Else
                        ' Trainer existiert bereits, ist aber nicht in der Trainerliste - also Rückkehrer
                        Dim Rueckkehrer = New Trainer()
                        Rueckkehrer.TrainerID = ImportedTrainer.TrainerID
                        Rueckkehrer.Vorname = ImportedTrainer.Vorname
                        Rueckkehrer.Nachname = ImportedTrainer.Nachname
                        Rueckkehrer.Alias = ImportedTrainer.Spitzname
                        Rueckkehrer.Telefonnummer = ImportedTrainer.Telefonnummer
                        Rueckkehrer.EMail = ImportedTrainer.eMail
                        Rueckkehrer.Foto = Nothing
                        Rueckkehrer.IstImImport = True
                        Rueckkehrer.IstRueckkehrer = True
                        Rueckkehrer.IstNeuImClub = True
                    End If
                Else
                    Dim BekannterTrainer = club.Trainerliste.FirstOrDefault(Function(Tr) Tr.Vorname = ImportedTrainer.Vorname AndAlso Tr.Nachname = ImportedTrainer.Nachname)
                    If BekannterTrainer IsNot Nothing Then
                        ' Trainer mit gleichem Namen gefunden, aber andere ID - ggf. Daten aktualisieren
                        BekannterTrainer.Vorname = ImportedTrainer.Vorname
                        BekannterTrainer.Nachname = ImportedTrainer.Nachname
                        BekannterTrainer.Alias = ImportedTrainer.Spitzname
                        BekannterTrainer.Telefonnummer = ImportedTrainer.Telefonnummer
                        BekannterTrainer.EMail = ImportedTrainer.eMail
                        BekannterTrainer.IstImImport = True
                        Continue For
                    Else
                        ' Neuer Trainer, der noch nicht im Club ist
                        Dim NewTrainer As New Trainer With {
                            .TrainerID = Guid.NewGuid,
                            .Vorname = ImportedTrainer.Vorname,
                            .Nachname = ImportedTrainer.Nachname,
                            .Alias = ImportedTrainer.Spitzname,
                            .Telefonnummer = ImportedTrainer.Telefonnummer,
                            .EMail = ImportedTrainer.eMail,
                            .Foto = Nothing,
                            .IstNeuImClub = True,
                            .IstImImport = True}
                        club.Trainerliste.Add(NewTrainer)
                    End If
                End If
            Next

            If club.Trainerliste.Where(Function(Tr) Not Tr.IstImImport).Count > 0 Then
                Dim Ts As New TrainerService
                Dim x = club.Trainerliste.Count - club.Trainerliste.Where(Function(Tr) Tr.IstImImport).Count
                If MessageBox.Show($"Alle Trainer wurden gelesen{vbNewLine}Können die {x} bestehende Trainer gelöscht werden?", "Datenimport", vbYesNo) = vbYes Then
                    ' Trainer löschen, die nicht im Import enthalten sind
                    club.Trainerliste.Where(Function(Tr) Tr.IstNichtImImport).ToList.ForEach(Sub(Tr) Ts.TrainerLoeschen(Tr))
                End If
                Dim y = club.Trainerliste.Where(Function(Tr) Tr.IstNeuImClub)
                For Each e In club.Einteilungsliste
                    ' Neue Trainer den Einteilungen hinzufügen
                    Ts.TrainerEinteilungHinzufuegen(y.ToList, e)
                Next
            End If

        End Sub

        Public Sub ImportTrainerKi()

            Dim Pfad = StarteOpenFileDialog()
            If Pfad Is Nothing Then Exit Sub

            Dim ImportTrainerliste As List(Of DataImport.Trainer)
            Try
                ImportTrainerliste = LeseTrainerAusDatasetKI(Pfad)
            Catch ex As Exception
                MessageBox.Show(ex.ToString(), "Fehler beim Datenimport", MessageBoxButton.OK, MessageBoxImage.Error)
                Exit Sub
            End Try

            Dim club = ServiceProvider.DateiService.AktuellerClub
            If club Is Nothing Then
                MessageBox.Show("Kein Club geladen. Import abgebrochen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning)
                Exit Sub
            End If

            Dim ts As New TrainerService()

            ' Sicherstellen, dass Listen existieren
            If club.Trainerliste Is Nothing Then club.Trainerliste = New TrainerCollection()
            If club.Einteilungsliste Is Nothing Then club.Einteilungsliste = New EinteilungCollection()

            ' Reset Import-Flags
            For Each t In club.Trainerliste
                t.IstImImport = False
                t.IstNeuImClub = False
                t.IstRueckkehrer = False
            Next

            ' Indizes / Lookup-Strukturen zur schnellen Suche
            Dim byId As New Dictionary(Of Guid, Trainer)
            Dim byName As New Dictionary(Of String, Trainer)(StringComparer.OrdinalIgnoreCase)

            For Each t In club.Trainerliste
                If t.TrainerID <> Guid.Empty AndAlso Not byId.ContainsKey(t.TrainerID) Then
                    byId.Add(t.TrainerID, t)
                End If
                Dim nameKey = (t.Vorname & "|" & t.Nachname).Trim().ToLowerInvariant()
                If Not byName.ContainsKey(nameKey) Then byName.Add(nameKey, t)
            Next

            Dim neuTrainer As New List(Of Trainer)()

            ' Verarbeite importierte Trainer
            For Each imported In ImportTrainerliste
                If imported Is Nothing Then Continue For

                If imported.TrainerID <> Guid.Empty Then
                    Dim id = imported.TrainerID
                    Dim exist As Trainer = Nothing
                    If byId.TryGetValue(id, exist) Then
                        ' Update existierender Trainer
                        exist.Vorname = imported.Vorname
                        exist.Nachname = imported.Nachname
                        exist.Alias = imported.Spitzname
                        exist.Telefonnummer = imported.Telefonnummer
                        exist.EMail = imported.eMail
                        exist.IstImImport = True
                    Else
                        ' Rückkehrer: Trainer-ID vorhanden, aber nicht in Liste -> hinzufügen
                        Dim rueck As New Trainer With {
                    .TrainerID = id,
                    .Vorname = imported.Vorname,
                    .Nachname = imported.Nachname,
                    .Alias = imported.Spitzname,
                    .Telefonnummer = imported.Telefonnummer,
                    .EMail = imported.eMail,
                    .IstImImport = True,
                    .IstRueckkehrer = True,
                    .IstNeuImClub = True
                }
                        club.Trainerliste.Add(rueck)
                        neuTrainer.Add(rueck)
                        ' Update Lookups
                        If id <> Guid.Empty Then
                            Try
                                byId.Add(id, rueck)
                            Catch
                            End Try
                        End If
                        Dim key = (rueck.Vorname & "|" & rueck.Nachname).Trim().ToLowerInvariant()
                        If Not byName.ContainsKey(key) Then byName.Add(key, rueck)
                    End If
                Else
                    ' Kein ID in Import: versuche Match per Name
                    Dim nameKey = (imported.Vorname & "|" & imported.Nachname).Trim().ToLowerInvariant()
                    Dim known As Trainer = Nothing
                    If byName.TryGetValue(nameKey, known) Then
                        known.Vorname = imported.Vorname
                        known.Nachname = imported.Nachname
                        known.Alias = imported.Spitzname
                        known.Telefonnummer = imported.Telefonnummer
                        known.EMail = imported.eMail
                        known.IstImImport = True
                    Else
                        ' Neuer Trainer
                        Dim neu As New Trainer With {
                    .TrainerID = Guid.NewGuid(),
                    .Vorname = imported.Vorname,
                    .Nachname = imported.Nachname,
                    .Alias = imported.Spitzname,
                    .Telefonnummer = imported.Telefonnummer,
                    .EMail = imported.eMail,
                    .IstNeuImClub = True,
                    .IstImImport = True
                }
                        club.Trainerliste.Add(neu)
                        neuTrainer.Add(neu)
                        ' Update Lookups
                        byId(neu.TrainerID) = neu
                        byName((neu.Vorname & "|" & neu.Nachname).Trim().ToLowerInvariant()) = neu
                    End If
                End If
            Next

            ' Trainer löschen, die nicht im Import vorkamen
            Dim nichtImImport = club.Trainerliste.Where(Function(t) Not t.IstImImport).ToList()
            If nichtImImport.Count > 0 Then
                Dim x = nichtImImport.Count
                Dim res = MessageBox.Show($"Alle Trainer wurden gelesen.{vbNewLine}Können die {x} bestehenden Trainer gelöscht werden?", "Datenimport", MessageBoxButton.YesNo, MessageBoxImage.Question)
                If res = MessageBoxResult.Yes Then
                    For Each t In nichtImImport
                        Try
                            ts.TrainerLoeschen(t)
                        Catch ex As Exception
                            ' Fehler beim Löschen protokollieren, aber weitermachen
                            Debug.WriteLine($"Fehler beim Löschen von Trainer {t.TrainerID}: {ex}")
                        End Try
                    Next
                End If
            End If

            ' Neue Trainer allen Einteilungen hinzufügen (falls gewünscht)
            If neuTrainer.Count > 0 AndAlso club.Einteilungsliste IsNot Nothing Then
                For Each e In club.Einteilungsliste
                    Try
                        ts.TrainerEinteilungHinzufuegen(neuTrainer, e)
                    Catch ex As Exception
                        Debug.WriteLine($"Fehler beim Hinzufügen neuer Trainer zu Einteilung {e?.Benennung}: {ex}")
                    End Try
                Next
            End If

        End Sub

        Public Sub ImportTeilnehmer()
            Dim Pfad = StarteOpenFileDialog()
            If Pfad Is Nothing Then Exit Sub
        End Sub

        Public Sub ImportTeilnehmer(Pfad As String)

            'Dim TeilnehmerzahlVorImport = AppController.AktuellerClub.SelectedEinteilung.EinteilungAlleTeilnehmer.Count
            Dim ImportTeilnehmerliste As List(Of DataImport.Teilnehmer)

            Try
                ImportTeilnehmerliste = LeseTeilnehmerAusDatasetKI(Pfad)
            Catch ex As Exception
                MessageBox.Show($"{ex.Message}{vbNewLine}{ex.InnerException.Message}", "Fehler beim Datenimport", MessageBoxButton.OK, MessageBoxImage.Error)
                Exit Sub
            End Try

            Dim LSListe = If(ServiceProvider.DateiService.AktuellerClub.Leistungsstufenliste, New LeistungsstufeCollection())

            Dim Teilnehmerliste = ServiceProvider.DateiService.AktuellerClub.Teilnehmerliste
            ' ALle Teilnehmer mit gültiger ID durchgehen
            For Each ImportedTeilnehmer In ImportTeilnehmerliste
                If ImportedTeilnehmer.TeilnehmerID <> Guid.Empty Then
                    ' Prüfen, ob der Teilnehmer im Club bereits existiert
                    Dim ExistingTeilnehmer As Teilnehmer = Teilnehmerliste.FirstOrDefault(Function(Tn) Tn.Ident = ImportedTeilnehmer.TeilnehmerID)
                    If ExistingTeilnehmer IsNot Nothing Then
                        ' Teilnehmer existiert bereits, ggf. Daten aktualisieren
                        ExistingTeilnehmer.Vorname = ImportedTeilnehmer.Vorname
                        ExistingTeilnehmer.Nachname = ImportedTeilnehmer.Nachname
                        ExistingTeilnehmer.Geburtsdatum = ImportedTeilnehmer.Geburtsdatum
                        ExistingTeilnehmer.Telefonnummer = ImportedTeilnehmer.Telefonnummer
                        ' Hier wird keine neue Leistungsstufe angelegt
                        ExistingTeilnehmer.IstImImport = True
                        Continue For
                    Else
                        ' Teilnehmer existiert bereits, ist aber nicht in der Teilnehmerliste - also Rückkehrer
                        Dim Rueckkehrer = New Teilnehmer()
                        Rueckkehrer.Ident = ImportedTeilnehmer.TeilnehmerID
                        Rueckkehrer.Vorname = ImportedTeilnehmer.Vorname
                        Rueckkehrer.Nachname = ImportedTeilnehmer.Nachname
                        Rueckkehrer.Telefonnummer = ImportedTeilnehmer.Telefonnummer
                        Rueckkehrer.Geburtsdatum = ImportedTeilnehmer.Geburtsdatum
                        Rueckkehrer.Leistungsstufe = LSListe.FirstOrDefault(Function(ls) ls.Benennung = ImportedTeilnehmer.Leistungsstand)
                        Rueckkehrer.LeistungsstufeID = If(Rueckkehrer.Leistungsstufe Is Nothing, Guid.Empty, Rueckkehrer.Leistungsstufe.Ident)
                        Rueckkehrer.IstImImport = True
                        Rueckkehrer.IstRueckkehrer = True
                        Rueckkehrer.IstNeuImClub = True
                    End If
                Else
                    ' Ohne ID: per Name suchen
                    Dim BekannterTeilnehmer = Teilnehmerliste.FirstOrDefault(Function(Tr) Tr.Vorname = ImportedTeilnehmer.Vorname AndAlso Tr.Nachname = ImportedTeilnehmer.Nachname)
                    If BekannterTeilnehmer IsNot Nothing Then
                        ' Trainer mit gleichem Namen gefunden, aber andere ID - ggf. Daten aktualisieren
                        BekannterTeilnehmer.Vorname = ImportedTeilnehmer.Vorname
                        BekannterTeilnehmer.Nachname = ImportedTeilnehmer.Nachname
                        BekannterTeilnehmer.Telefonnummer = ImportedTeilnehmer.Telefonnummer
                        BekannterTeilnehmer.Geburtsdatum = ImportedTeilnehmer.Geburtsdatum
                        ' Hier wird keine neue Leistungsstufe angelegt
                        BekannterTeilnehmer.IstImImport = True
                        Continue For
                    Else
                        ' Neuer Teilnehmer, der noch nicht im Club ist
                        Dim NeuerTeilnehmer As New Teilnehmer With {
                            .Ident = Guid.NewGuid,
                            .Vorname = ImportedTeilnehmer.Vorname,
                            .Nachname = ImportedTeilnehmer.Nachname,
                            .Geburtsdatum = ImportedTeilnehmer.Geburtsdatum,
                            .Telefonnummer = ImportedTeilnehmer.Telefonnummer,
                            .Leistungsstufe = LSListe.FirstOrDefault(Function(ls) ls.Benennung = ImportedTeilnehmer.Leistungsstand),
                            .LeistungsstufeID = If(.Leistungsstufe Is Nothing, Guid.Empty, .Leistungsstufe.Ident),
                            .IstImImport = True,
                            .IstNeuImClub = True}
                        Teilnehmerliste.Add(NeuerTeilnehmer)
                    End If
                End If
            Next

            Dim TS = New TeilnehmerService
            ' Teilnehmer löschen, die nicht im Import vorkamen
            Dim nichtImImport = Teilnehmerliste.Where(Function(t) Not t.IstImImport).ToList()
            If nichtImImport.Count > 0 Then
                Dim x = nichtImImport.Count
                Dim res = MessageBox.Show($"Alle Teilnehmer wurden gelesen.{vbNewLine}Können die {x} bestehenden Teilnehmer gelöscht werden?", "Datenimport", MessageBoxButton.YesNo, MessageBoxImage.Question)
                If res = MessageBoxResult.Yes Then
                    For Each t In nichtImImport
                        Try
                            TS.TeilnehmerLoeschen(t)
                        Catch ex As Exception
                            ' Fehler beim Löschen protokollieren, aber weitermachen
                            Debug.WriteLine($"Fehler beim Löschen von Trainer {t.Ident}: {ex}")
                        End Try
                    Next
                End If
            End If

            ' Neue Teilnehmer in allen Einteilungen hinzufügen (falls gewünscht)
            Dim y = Teilnehmerliste.Where(Function(Tn) Tn.IstNeuImClub).ToList
            For Each e In ServiceProvider.DateiService.AktuellerClub.Einteilungsliste
                ' Neue Trainer den Einteilungen hinzufügen
                TS.TeilnehmerEinteilungHinzufuegen(y, e)
            Next

        End Sub

#End Region

#Region "Private"


        Public Function StarteOpenFileDialog() As String
            Return StarteOpenFileDialog(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
        End Function

        Public Function StarteOpenFileDialog(InitialDirectory As String, Filename As String) As String
            _ofdDokument.InitialDirectory = InitialDirectory
            _ofdDokument.FileName = Filename
            _ofdDokument.Filter = "Excel Dateien (*.xlsx, *.xls)| *.xlsx; *.xls|CSV (Trennzeichen-getrennt) (*.csv)| *.csv"
            _ofdDokument.FilterIndex = 1
            _ofdDokument.RestoreDirectory = True

            Return OpenFileDialog()

        End Function

        Public Function StarteOpenFileDialog(InitialDirectory As String) As String

            _ofdDokument.InitialDirectory = InitialDirectory
            _ofdDokument.Filter = "Excel Dateien (*.xlsx, *.xls)| *.xlsx; *.xls|CSV (Trennzeichen-getrennt) (*.csv)| *.csv"
            _ofdDokument.FilterIndex = 1
            _ofdDokument.RestoreDirectory = True

            Return OpenFileDialog()

        End Function

        Private Function OpenFileDialog() As String


            If _ofdDokument.ShowDialog = True Then
                Try
                    Return _ofdDokument.FileName
                Catch ex As Exception
                    MessageBox.Show($"{ex.Message}{vbNewLine}{ex.InnerException.Message}", "Fehler beim Datenimport", MessageBoxButton.OK, MessageBoxImage.Error)
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If

        End Function


        'Private Function ReadImportExcelfileSkiclub(Excelsheet As Excel.Worksheet) As Club
        '    Dim CurrentRow = 4
        '    Dim RowCount = Excelsheet.UsedRange.Rows.Count
        '    Dim Skikursgruppe As Gruppe
        '    Do Until CurrentRow > RowCount

        '        Dim Teilnehmer As New Teilnehmer With {
        '        .Vorname = Trim(Excelsheet.UsedRange(CurrentRow, 1).Value),
        '        .Nachname = Trim(Excelsheet.UsedRange(CurrentRow, 2).Value),
        '        .Leistungsstufe = FindLevel(Trim(Excelsheet.UsedRange(CurrentRow, 3).Value))}

        '        _skischule.Participantlist.Add(Teilnehmer)
        '        '.MemberOfGroup = Excelsheet.UsedRange(CurrentRow, 4).Value}

        '        'Gibt es die Skikursgruppe aus der Excelliste schon?
        '        If Not String.IsNullOrEmpty(Excelsheet.UsedRange(CurrentRow, 4).Value) Then
        '            Skikursgruppe = FindSkikursgruppe(Excelsheet.UsedRange(CurrentRow, 4).Value)
        '            ' Skikursgruppe gefunden, aktuellen Teilnehmer hinzufügen
        '            If Skikursgruppe Is Nothing Then
        '            Else
        '                Skikursgruppe.Mitgliederliste.Add(Teilnehmer)
        '                'Teilnehmer.MemberOfGroup = Skikursgruppe.GruppenID
        '            End If
        '        End If
        '        CurrentRow += 1
        '    Loop
        '    Return _skischule
        'End Function


        Private Function FindLevel(Benennung As String) As Leistungsstufe

            If String.IsNullOrEmpty(Benennung) Then
                Return Nothing
            End If

            Dim Level = _skischule.Levellist.FirstOrDefault(Function(k) k.LevelNaming = Benennung)
            If Level Is Nothing Then
                Level = New Leistungsstufe(Benennung)
                _skischule.Levellist.Add(Level)
            End If

            Return Level
        End Function

        Private Function FindSkikursgruppe(Naming As String) As Gruppe
            Dim Group = _skischule.Grouplist.FirstOrDefault(Function(s) s.GroupNaming = Naming)
            If Group Is Nothing Then
                Group = New Gruppe With {.Benennung = Naming}
                _skischule.Grouplist.Add(Group)
            End If
            Return Group
        End Function

        'Private Function CheckExcelFileFormatSkiclub(Excelfile As Excel.Workbook) As Boolean

        '    Dim XlValid As Boolean

        '    ' Excel Sheet (Überschrift Zeile 1, Daten Zeile 2 bis zum Dateiende)
        '    _xlSheet = Excelfile.ActiveSheet

        '    If _xlSheet IsNot Nothing Then
        '        XlValid = _xlSheet.UsedRange.Columns.Count = 4

        '        ' Check column caption
        '        XlValid = XlValid And _xlSheet.Range("A1").Value = "Vorname"
        '        XlValid = XlValid And _xlSheet.Range("B1").Value = "Nachname"
        '        XlValid = XlValid And _xlSheet.Range("C1").Value = "Level"
        '        XlValid = XlValid And _xlSheet.Range("D1").Value = "Skigruppe"

        '        ' Check first data row
        '        XlValid = XlValid And Not String.IsNullOrEmpty(_xlSheet.Range("A2").Value)
        '        'XlValid = XlValid And Not String.IsNullOrEmpty(_xlSheet.Range("B2").Value)
        '        'XlValid = XlValid And Not String.IsNullOrEmpty(_xlSheet.Range("C2").Value)

        '    End If

        '    If Not XlValid Then MessageBox.Show("Die Datei ist nicht zum Skiclubimport geeignet")
        '    Dim Text = New StringBuilder
        '    Text.AppendLine("Die Datei ist nicht zum Skiclubimport geeignet")
        '    Text.AppendLine("Verwende eine Excel Datei mit den Überschriften [Vorname] in Feld A1, [Nachname] in B1, [Level] in C1 und [Skigruppe] in D1")
        '    Text.AppendLine("Pflichtfelder sind Vorname und Nachname")

        '    If Not XlValid Then MessageBox.Show(Text.ToString)

        '    Return XlValid

        'End Function
#End Region

    End Module

End Namespace
