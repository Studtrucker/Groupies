Imports Microsoft.Office.Interop
Imports Groupies
Imports Groupies.Controller

Public Class XLSchreiber

    Private Benennungen As Importformat

    Public Sub ExportDatenAlsXl(Extension As String, Datatyp As String)
        Dim fab = New Groupies.FabrikDataImport

        Benennungen = fab.ErzeugeImportformat(Datatyp, Extension)

        NeuesXlObjekt()

    End Sub


    Public Sub NeuesXlObjekt()
        Dim oExcel As Excel.Application
        Dim oBook As Excel.Workbook
        Dim oSheet As Excel.Worksheet

        'Start a new workbook in Excel    
        oExcel = CreateObject("Excel.Application")
        oBook = oExcel.Workbooks.Add
        oExcel.visible = True

        'Add data to cells of the first worksheet in the new workbook    
        oSheet = oBook.Worksheets(1)

        oSheet.Name = Benennungen.Tabelle

        Dim c = 1, r = 1
        For Each Spalte In Benennungen.Spalten.OrderBy(Function(t) t)
            oSheet.Columns(c).rows(1).value = Spalte
            c += 1
        Next
        r = 2

        If Benennungen.Tabelle = "Trainer" Then

            AppController.CurrentClub.AlleTrainer.ToList.ForEach(Sub(Tn)
                                                                     oSheet.Columns(2).Rows(r).value = Tn.TrainerID.ToString
                                                                     oSheet.Columns(3).Rows(r).value = Tn.Vorname
                                                                     oSheet.Columns(1).Rows(r).value = Tn.Nachname
                                                                     r += 1
                                                                 End Sub)

        Else

            AppController.CurrentClub.AlleTeilnehmer.ToList.ForEach(Sub(Tn)
                                                                        oSheet.Columns(2).Rows(r).value = Tn.TeilnehmerID.ToString
                                                                        oSheet.Columns(3).Rows(r).value = Tn.Vorname
                                                                        oSheet.Columns(1).Rows(r).value = Tn.Nachname
                                                                        r += 1
                                                                    End Sub)
        End If

        oBook.SaveAs($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\{Year(Now)}{Month(Now)}{Day(Now)}{Hour(Now)}{Minute(Now)}{Second(Now)}{Benennungen.Tabelle}.xlsx")
        'oExcel.Quit
    End Sub

End Class
