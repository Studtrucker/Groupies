Imports System.Windows.Forms
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Collections.ObjectModel
Imports Microsoft.Win32
Imports Groupies.Entities
Imports System.Text

Namespace Services

    Public Module ImportService

#Region "Fields"

        Private ReadOnly _ofdDokument As New Forms.OpenFileDialog
        Public Workbook As Excel.Workbook
        Private _xlSheet As Excel.Worksheet
        Private ReadOnly _xlCell As Excel.Range
        Private ReadOnly _skischule = New Entities.Skiclub

#End Region

#Region "Public Functions"

        Public Function ImportSkiclub() As Entities.Skiclub

            Workbook = Nothing

            _ofdDokument.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            _ofdDokument.Filter = "Excel Dateien (*.xlsx)| *.xlsx"
            _ofdDokument.FilterIndex = 1
            _ofdDokument.RestoreDirectory = True

            If _ofdDokument.ShowDialog = DialogResult.OK Then
                Dim xlApp = New Excel.Application
                Workbook = xlApp.Workbooks.Open(_ofdDokument.FileName,, True)
                If CheckExcelFileFormatSkiclub(Workbook) Then
                    Return ReadImportExcelfileSkiclub(Workbook.ActiveSheet)
                End If
                Workbook.Close()
            End If
            Return Nothing

        End Function

        Public Function ImportParticipants() As Entities.ParticipantCollection
            Workbook = Nothing

            _ofdDokument.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            _ofdDokument.Filter = "Excel Dateien (*.xlsx)| *.xlsx"
            _ofdDokument.FilterIndex = 1
            _ofdDokument.RestoreDirectory = True

            If _ofdDokument.ShowDialog = DialogResult.OK Then
                Dim xlApp = New Excel.Application
                Workbook = xlApp.Workbooks.Open(_ofdDokument.FileName,, True)
                If CheckExcelFileFormatParticipants(Workbook) Then
                    Return ReadImportExcelfileParticipants(Workbook.ActiveSheet)
                End If
                Workbook.Close()
            End If
            Return Nothing

        End Function

        Public Function ImportInstructors() As Entities.InstructorCollection
            Workbook = Nothing

            _ofdDokument.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            _ofdDokument.Filter = "Excel Dateien (*.xlsx)| *.xlsx"
            _ofdDokument.FilterIndex = 1
            _ofdDokument.RestoreDirectory = True

            If _ofdDokument.ShowDialog = DialogResult.OK Then
                Dim xlApp = New Excel.Application
                Workbook = xlApp.Workbooks.Open(_ofdDokument.FileName,, True)
                If CheckExcelFileFormatInstructors(Workbook) Then
                    Return ReadImportExcelfileInstructors(Workbook.ActiveSheet)
                End If
                Workbook.Close()
            End If
            Return Nothing

        End Function

#End Region

#Region "Private"

        Private Function ReadImportExcelfileSkiclub(Excelsheet As Excel.Worksheet) As Entities.Skiclub
            Dim CurrentRow = 4
            Dim RowCount = Excelsheet.UsedRange.Rows.Count
            Dim Skikursgruppe As Group
            Do Until CurrentRow > RowCount

                Dim Teilnehmer As New Participant With {
                .ParticipantFirstName = Trim(Excelsheet.UsedRange(CurrentRow, 1).Value),
                .ParticipantLastName = Trim(Excelsheet.UsedRange(CurrentRow, 2).Value),
                .ParticipantLevel = FindLevel(Trim(Excelsheet.UsedRange(CurrentRow, 3).Value)),
                .MemberOfGroup = Excelsheet.UsedRange(CurrentRow, 4).Value}
                _skischule.Participantlist.Add(Teilnehmer)

                'Gibt es die Skikursgruppe aus der Excelliste schon?
                If Not String.IsNullOrEmpty(Excelsheet.UsedRange(CurrentRow, 4).Value) Then
                    Skikursgruppe = FindSkikursgruppe(Excelsheet.UsedRange(CurrentRow, 4).Value)
                    ' Skikursgruppe gefunden, aktuellen Teilnehmer hinzufügen
                    If Skikursgruppe IsNot Nothing Then
                        Skikursgruppe.AddMember(Teilnehmer)
                        Teilnehmer.MemberOfGroup = Skikursgruppe.GroupID
                    End If
                End If
                CurrentRow += 1
            Loop
            Return _skischule
        End Function

        Private Function ReadImportExcelfileParticipants(Excelsheet As Excel.Worksheet) As Entities.ParticipantCollection
            Dim CurrentRow = 2
            Dim RowCount = Excelsheet.UsedRange.Rows.Count


            Dim _Participantlist = New ParticipantCollection

            Do Until CurrentRow > RowCount

                Dim Teilnehmer As New Participant With {
                .ParticipantFirstName = Trim(Excelsheet.UsedRange(CurrentRow, 1).Value),
                .ParticipantLastName = Trim(Excelsheet.UsedRange(CurrentRow, 2).Value),
                .ParticipantLevel = FindLevel(Trim(Excelsheet.UsedRange(CurrentRow, 3).Value))}
                _Participantlist.Add(Teilnehmer)

                CurrentRow += 1
            Loop
            Return _Participantlist

        End Function

        Private Function ReadImportExcelfileInstructors(Excelsheet As Excel.Worksheet) As Entities.InstructorCollection
            Dim CurrentRow = 2
            Dim RowCount = Excelsheet.UsedRange.Rows.Count


            Dim _Participantlist = New InstructorCollection

            Do Until CurrentRow > RowCount

                Dim Teilnehmer As New Instructor(True, True) With {
                .InstructorFirstName = Trim(Excelsheet.UsedRange(CurrentRow, 1).Value),
                .InstructorLastName = Trim(Excelsheet.UsedRange(CurrentRow, 2).Value),
                .InstructorPrintName = Trim(Excelsheet.UsedRange(CurrentRow, 3).Value)}
                _Participantlist.Add(Teilnehmer)

                CurrentRow += 1
            Loop
            Return _Participantlist

        End Function

        Private Function FindLevel(Benennung As String) As Level

            Dim Level = _skischule.Levellist.FirstOrDefault(Function(k) k.LevelNaming = Benennung)
            If Level Is Nothing Then
                Level = New Level(True) With {.LevelNaming = Benennung}
                _skischule.Levellist.Add(Level)
            End If

            Return Level
        End Function

        Private Function FindSkikursgruppe(Naming As String) As Group
            Dim Group = _skischule.Grouplist.FirstOrDefault(Function(s) s.GroupNaming = Naming)
            If Group Is Nothing Then
                Group = New Group With {.GroupNaming = Naming}
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
