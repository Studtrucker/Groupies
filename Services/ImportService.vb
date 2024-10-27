Imports System.Windows
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Collections.ObjectModel
Imports Microsoft.Win32
Imports Groupies.Entities
Imports System.Text
Imports Groupies.Controller
Imports Groupies

Namespace Services

    Public Module ImportService

#Region "Felder"

        Private ReadOnly _ofdDokument As New OpenFileDialog
        Public Workbook As Excel.Workbook
        Private _xlSheet As Excel.Worksheet
        Private ReadOnly _xlCell As Excel.Range
        Private ReadOnly _skischule = New Entities.Club("Club")

#End Region

#Region "Eigenschaften"

#End Region

#Region "Funktionen und Methoden"

        Public Function ImportSkiclub() As Entities.Club

            Workbook = Nothing

            _ofdDokument.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            _ofdDokument.Filter = "Excel Dateien (*.xlsx)| *.xlsx"
            _ofdDokument.FilterIndex = 1
            _ofdDokument.RestoreDirectory = True

            If _ofdDokument.ShowDialog = True Then
                Dim xlApp = New Excel.Application
                Workbook = xlApp.Workbooks.Open(_ofdDokument.FileName,, True)
                If CheckExcelFileFormatSkiclub(Workbook) Then
                    Return ReadImportExcelfileSkiclub(Workbook.ActiveSheet)
                End If
                Workbook.Close()
            End If
            Return Nothing

        End Function

        Public Function ImportParticipants() As Entities.TeilnehmerCollection
            Workbook = Nothing

            _ofdDokument.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            _ofdDokument.Filter = "Excel Dateien (*.xlsx)| *.xlsx"
            _ofdDokument.FilterIndex = 1
            _ofdDokument.RestoreDirectory = True

            If _ofdDokument.ShowDialog = True Then
                Dim xlApp = New Excel.Application
                Workbook = xlApp.Workbooks.Open(_ofdDokument.FileName,, True)
                If CheckExcelFileFormatParticipants(Workbook) Then
                    Return ReadImportExcelfileParticipants(Workbook.ActiveSheet)
                End If
                Workbook.Close()
            End If
            Return Nothing

        End Function

        Public Function ImportInstructors() As Entities.TrainerCollection
            Workbook = Nothing

            _ofdDokument.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            _ofdDokument.Filter = "Excel Dateien (*.xlsx)| *.xlsx"
            _ofdDokument.FilterIndex = 1
            _ofdDokument.RestoreDirectory = True

            If _ofdDokument.ShowDialog = True Then
                Dim xlApp = New Excel.Application
                Workbook = xlApp.Workbooks.Open(_ofdDokument.FileName,, True)
                If CheckExcelFileFormatInstructors(Workbook) Then
                    Return ReadImportExcelfileInstructors(Workbook.ActiveSheet)
                End If
                Workbook.Close()
            End If
            Return Nothing

        End Function

        Public Sub ImportTeilnehmer()

            Dim TeilnehmerzahlVorImport = AppController.CurrentClub.AlleTeilnehmer.Count

            _ofdDokument.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            _ofdDokument.Filter = "Excel Dateien (*.xlsx, *.xls)| *.xlsx; *.xls|CSV (Trennzeichen-getrennt) (*.csv)| *.csv"
            _ofdDokument.FilterIndex = 1

            _ofdDokument.RestoreDirectory = True

            Dim ImportTeilnehmerliste As List(Of DataImport.Teilnehmer)

            If _ofdDokument.ShowDialog = True Then
                Try
                    ImportTeilnehmerliste = LeseTeilnehmerAusDataset(_ofdDokument.FileName)
                Catch ex As Exception
                    MessageBox.Show($"{ex.Message}{vbNewLine}{ex.InnerException.Message}", "Fehler beim Datenimport", MessageBoxButton.OK, MessageBoxImage.Error)
                    Exit Sub
                End Try
            Else
                Exit Sub
            End If

            If ImportTeilnehmerliste Is Nothing Then
                Exit Sub
            End If

            If AppController.CurrentClub.AlleTeilnehmer Is Nothing Then
                AppController.CurrentClub.GruppenloseTeilnehmer = New TeilnehmerCollection
            End If

            'Aus der Importdatei werden bekannte Teilnehmer markiert
            For Each aktuellerTn As Teilnehmer In AppController.CurrentClub.AlleTeilnehmer
                ImportTeilnehmerliste.Where(Function(importTn) _
                                                importTn.TeilnehmerID = aktuellerTn.TeilnehmerID) _
                                                .ToList.ForEach(Sub(nTn) _
                                                                    nTn.IstBekannt = True)
            Next

            ' Aus dem aktuellen Club werden alle Teilnehmer als potentieller Archivkandidat gesetzt
            AppController.CurrentClub.AlleTeilnehmer.ToList.ForEach(Sub(Tn) Tn.Archivieren = True)
            ' Alle Archivkandidaten werden, wenn sie in der Importdatei gelistet sind, beibehalten
            For Each importTn In ImportTeilnehmerliste
                AppController.CurrentClub.AlleTeilnehmer.ToList.Where(Function(Tn) _
                                                                          Tn.TeilnehmerID = importTn.TeilnehmerID) _
                                                                          .ToList.ForEach(Sub(Tn) Tn.Archivieren = False)
            Next

            ' Alle unbekannten Teilnehmer 
            Dim UnbekannteTeilnehmer = ImportTeilnehmerliste.Where(Function(Tn) Not Tn.IstBekannt).Select(Function(Tn) New Teilnehmer(Tn.Vorname, Tn.Nachname))
            ' Alle bekannten Teilnehmer 
            Dim BekannteTeilnehmer = ImportTeilnehmerliste.Where(Function(Tn) Tn.IstBekannt).
                Select(Function(Tn) AppController.CurrentClub.AlleTeilnehmer.
                Where(Function(AlterTeilnehmer) Tn.TeilnehmerID = AlterTeilnehmer.TeilnehmerID))

            ' Alle zu archivierenden Teilnehmer 
            Dim ZuArchivierende = AppController.CurrentClub.AlleTeilnehmer.ToList.Where(Function(Tn) Tn.Archivieren = True)

            ' Teilnehmer in der ewigen Liste archivieren
            Dim Archiv = AppController.CurrentClub.AlleTeilnehmer.ToList.Where(Function(Tn) Tn.Archivieren = True).Select((Function(Tn) New EwigerTeilnehmer(Tn, Now.Date)))
            MessageBox.Show($"Anzahl Teilnehmer vor dem Aussortieren {AppController.CurrentClub.AlleTeilnehmer.Count}")
            ' Teilnehmer, die nicht dabei sind, aussortieren
            For Each ArchivTn In ZuArchivierende
                'AppController.CurrentClub.AlleTeilnehmer.Remove(AppController.CurrentClub.AlleTeilnehmer.Where(Function(Tn) Tn.TeilnehmerID = ArchivTn.TeilnehmerID).Single)
                AppController.CurrentClub.Gruppenliste.ToList.ForEach(Sub(Gr) Gr.Mitgliederliste.Remove(ArchivTn))
                AppController.CurrentClub.GruppenloseTeilnehmer.Remove(ArchivTn)
            Next

            MessageBox.Show($"Anzahl Teilnehmer nach dem Aussortieren {AppController.CurrentClub.AlleTeilnehmer.Count}; Anzahl zum Aussortieren {ZuArchivierende.Count}")

            Dim sb As New StringBuilder

            sb.Append($"Der Club hatte vor dem Datenimport {TeilnehmerzahlVorImport} Teilnehmer.{vbNewLine}")
            sb.Append($"Die Importdatei enthält {ImportTeilnehmerliste.Count} Teilnehmer, ")
            sb.Append($"von denen sind {BekannteTeilnehmer.Count} Teilnehmer bereits bekannt.{vbNewLine}")
            sb.Append($"{ZuArchivierende.Count} Teilnehmer werden archiviert und ")
            sb.Append($"{UnbekannteTeilnehmer.Count} neu hinzugefügt")
            MessageBox.Show($"{sb}", "Datenimport", MessageBoxButton.OK, MessageBoxImage.Information)


            UnbekannteTeilnehmer.ToList.ForEach(Sub(Tn) AppController.CurrentClub.GruppenloseTeilnehmer.Add(Tn))


        End Sub

#End Region

#Region "Private"

        Private Function ReadImportExcelfileSkiclub(Excelsheet As Excel.Worksheet) As Entities.Club
            Dim CurrentRow = 4
            Dim RowCount = Excelsheet.UsedRange.Rows.Count
            Dim Skikursgruppe As Gruppe
            Do Until CurrentRow > RowCount

                Dim Teilnehmer As New Teilnehmer With {
                .Vorname = Trim(Excelsheet.UsedRange(CurrentRow, 1).Value),
                .Nachname = Trim(Excelsheet.UsedRange(CurrentRow, 2).Value),
                .Leistungsstand = FindLevel(Trim(Excelsheet.UsedRange(CurrentRow, 3).Value))}

                _skischule.Participantlist.Add(Teilnehmer)
                '.MemberOfGroup = Excelsheet.UsedRange(CurrentRow, 4).Value}

                'Gibt es die Skikursgruppe aus der Excelliste schon?
                If Not String.IsNullOrEmpty(Excelsheet.UsedRange(CurrentRow, 4).Value) Then
                    Skikursgruppe = FindSkikursgruppe(Excelsheet.UsedRange(CurrentRow, 4).Value)
                    ' Skikursgruppe gefunden, aktuellen Teilnehmer hinzufügen
                    If Skikursgruppe IsNot Nothing Then
                        Skikursgruppe.Mitgliederliste.Add(Teilnehmer)
                        'Teilnehmer.MemberOfGroup = Skikursgruppe.GruppenID
                    End If
                End If
                CurrentRow += 1
            Loop
            Return _skischule
        End Function

        Private Function ReadImportExcelfileParticipants(Excelsheet As Excel.Worksheet) As Entities.TeilnehmerCollection
            Dim CurrentRow = 2
            Dim RowCount = Excelsheet.UsedRange.Rows.Count


            Dim _Participantlist = New TeilnehmerCollection

            Do Until CurrentRow > RowCount

                Dim Teilnehmer As New Teilnehmer With {
                .Vorname = Trim(Excelsheet.UsedRange(CurrentRow, 1).Value),
                .Nachname = Trim(Excelsheet.UsedRange(CurrentRow, 2).Value),
                .Leistungsstand = FindLevel(Trim(Excelsheet.UsedRange(CurrentRow, 3).Value))}
                _Participantlist.Add(Teilnehmer)

                CurrentRow += 1
            Loop
            Return _Participantlist

        End Function

        Private Function ReadImportExcelfileInstructors(Excelsheet As Excel.Worksheet) As Entities.TrainerCollection
            Dim CurrentRow = 2
            Dim RowCount = Excelsheet.UsedRange.Rows.Count


            Dim _Participantlist = New TrainerCollection

            Do Until CurrentRow > RowCount

                Dim Trainer As New Trainer(Trim(Excelsheet.UsedRange(CurrentRow, 1).Value),
                                              Trim(Excelsheet.UsedRange(CurrentRow, 2).Value),
                                              Trim(Excelsheet.UsedRange(CurrentRow, 3).Value))
                _Participantlist.Add(Trainer)

                CurrentRow += 1
            Loop
            Return _Participantlist

        End Function

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

        Private Function CheckExcelFileFormatSkiclub(Excelfile As Excel.Workbook) As Boolean

            Dim XlValid As Boolean

            ' Excel Sheet (Überschrift Zeile 1, Daten Zeile 2 bis zum Dateiende)
            _xlSheet = Excelfile.ActiveSheet

            If _xlSheet IsNot Nothing Then
                XlValid = _xlSheet.UsedRange.Columns.Count = 4

                ' Check column caption
                XlValid = XlValid And _xlSheet.Range("A1").Value = "Vorname"
                XlValid = XlValid And _xlSheet.Range("B1").Value = "Nachname"
                XlValid = XlValid And _xlSheet.Range("C1").Value = "Level"
                XlValid = XlValid And _xlSheet.Range("D1").Value = "Skigruppe"

                ' Check first data row
                XlValid = XlValid And Not String.IsNullOrEmpty(_xlSheet.Range("A2").Value)
                'XlValid = XlValid And Not String.IsNullOrEmpty(_xlSheet.Range("B2").Value)
                'XlValid = XlValid And Not String.IsNullOrEmpty(_xlSheet.Range("C2").Value)

            End If

            If Not XlValid Then MessageBox.Show("Die Datei ist nicht zum Skiclubimport geeignet")
            Dim Text = New StringBuilder
            Text.AppendLine("Die Datei ist nicht zum Skiclubimport geeignet")
            Text.AppendLine("Verwende eine Excel Datei mit den Überschriften [Vorname] in Feld A1, [Nachname] in B1, [Level] in C1 und [Skigruppe] in D1")
            Text.AppendLine("Pflichtfelder sind Vorname und Nachname")

            If Not XlValid Then MessageBox.Show(Text.ToString)

            Return XlValid

        End Function

        Private Function CheckExcelFileFormatParticipants(Excelfile As Excel.Workbook) As Boolean

            Dim XlValid As Boolean

            ' Excel Sheet (Überschrift Zeile 1, Daten Zeile 2 bis zum Dateiende)
            _xlSheet = Excelfile.ActiveSheet

            If _xlSheet IsNot Nothing Then
                XlValid = _xlSheet.UsedRange.Columns.Count = 3

                ' Check column caption
                XlValid = XlValid And _xlSheet.Range("A1").Value = "Vorname"
                XlValid = XlValid And _xlSheet.Range("B1").Value = "Nachname"
                XlValid = XlValid And _xlSheet.Range("C1").Value = "Level"

                ' Check first data row
                XlValid = XlValid And Not String.IsNullOrEmpty(_xlSheet.Range("A2").Value)

            End If

            Dim Text = New StringBuilder
            Text.AppendLine("Die Datei ist nicht zum Teilnehmerimport geeignet")
            Text.AppendLine("Verwende eine Excel Datei mit den Überschriften [Vorname] in Feld A1, [Nachname] in B1 und [Level] in C1")
            Text.AppendLine("Pflichtfelder sind Vorname und Nachname")

            If Not XlValid Then MessageBox.Show(Text.ToString)

            Return XlValid

        End Function

        Private Function CheckExcelFileFormatInstructors(Excelfile As Excel.Workbook) As Boolean

            Dim XlValid As Boolean

            ' Excel Sheet (Überschrift Zeile 1, Daten Zeile 2 bis zum Dateiende)
            _xlSheet = Excelfile.ActiveSheet

            If _xlSheet IsNot Nothing Then
                XlValid = _xlSheet.UsedRange.Columns.Count = 3

                ' Check column caption
                XlValid = XlValid And _xlSheet.Range("A1").Value = "Vorname"
                XlValid = XlValid And _xlSheet.Range("B1").Value = "Nachname"
                XlValid = XlValid And _xlSheet.Range("C1").Value = "Printname"

                ' Check first data row
                XlValid = XlValid And Not String.IsNullOrEmpty(_xlSheet.Range("A2").Value)

            End If

            Dim Text = New StringBuilder
            Text.AppendLine("Die Datei ist nicht zum Skilehrerimport geeignet")
            Text.AppendLine("Verwende eine Excel Datei mit den Überschriften [Vorname] in Feld A1, [Nachname] in B1 und [Printname] in C1")
            Text.AppendLine("Pflichtfelder sind Vorname und Printname")

            If Not XlValid Then MessageBox.Show(Text.ToString)

            Return XlValid

        End Function

#End Region

    End Module
End Namespace
