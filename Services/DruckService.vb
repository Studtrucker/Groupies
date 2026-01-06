Imports System.ComponentModel
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Markup
Imports Groupies.Entities.Generation4
Imports Groupies.Interfaces
Imports Groupies.UserControls
Imports Groupies.ViewModels



Namespace Services

    Public Enum Printversion
        TrainerInfo
        TeilnehmerInfo
    End Enum

    ''' <summary>
    ''' Service für alle Druck-Operationen der Anwendung.
    ''' Verwaltet die Erstellung von Druckdokumenten für Trainer- und Teilnehmerinformationen.
    ''' </summary>
    Public Class DruckService

        Private ReadOnly _msgService As IViewMessageService

        Public Sub New(Optional msgService As IViewMessageService = Nothing)
            _msgService = If(msgService, New DefaultViewMessageService())
        End Sub

        ''' <summary>
        ''' Zeigt den Druckdialog an und druckt die Einteilungsinformationen.
        ''' </summary>
        ''' <param name="einteilung">Die zu druckende Einteilung.</param>
        ''' <param name="printversion">Die Druckversion (Trainer oder Teilnehmer).</param>
        ''' <returns>True, wenn erfolgreich gedruckt wurde.</returns>
        Public Function DruckeEinteilungsInfo(einteilung As Einteilung, printversion As Printversion) As Boolean
            If einteilung Is Nothing Then
                _msgService.ShowWarning("Keine Einteilung zum Drucken ausgewählt.", "Warnung")
                Return False
            End If

            If einteilung.Gruppenliste Is Nothing OrElse einteilung.Gruppenliste.Count = 0 Then
                _msgService.ShowWarning("Die Einteilung enthält keine Gruppen zum Drucken.", "Warnung")
                Return False
            End If

            Try
                Dim dlg = New PrintDialog()
                If dlg.ShowDialog() = True Then
                    Dim printArea = New Size(dlg.PrintableAreaWidth, dlg.PrintableAreaHeight)
                    Dim pageMargin = New Thickness(30, 30, 30, 60)

                    Dim doc = ErstelleDruckdokument(einteilung, printversion, printArea, pageMargin)
                    dlg.PrintDocument(doc.DocumentPaginator, $"{einteilung.Benennung} - {printversion}")

                    Return True
                End If
            Catch ex As Exception
                _msgService.ShowError($"Fehler beim Drucken: {ex.Message}", "Druckfehler")
                Return False
            End Try

            Return False
        End Function

        ''' <summary>
        ''' Erstellt ein FixedDocument für den Druck der Einteilungsinformationen.
        ''' </summary>
        ''' <param name="einteilung">Die Einteilung mit den zu druckenden Gruppen.</param>
        ''' <param name="printversion">Die Druckversion (Trainer oder Teilnehmer).</param>
        ''' <param name="pageSize">Die Seitengröße für den Druck.</param>
        ''' <param name="pageMargin">Die Seitenränder.</param>
        ''' <returns>Ein FixedDocument bereit zum Drucken.</returns>
        Public Function ErstelleDruckdokument(einteilung As Einteilung,
                                              printversion As Printversion,
                                              pageSize As Size,
                                              pageMargin As Thickness) As FixedDocument
            If einteilung Is Nothing Then
                Throw New ArgumentNullException(NameOf(einteilung), "Einteilung darf nicht Nothing sein.")
            End If

            If einteilung.Gruppenliste Is Nothing OrElse einteilung.Gruppenliste.Count = 0 Then
                Throw New InvalidOperationException("Die Einteilung enthält keine Gruppen.")
            End If

            ' Sicherstellen, dass die Einteilung benannt ist
            If String.IsNullOrWhiteSpace(einteilung.Benennung) Then
                einteilung.Benennung = InputBox("Bitte diese Einteilung benennen", "Einteilung benennen", "Neue Einteilung")
                If String.IsNullOrWhiteSpace(einteilung.Benennung) Then
                    Throw New InvalidOperationException("Die Einteilung muss benannt werden.")
                End If
            End If

            ' Layout-Berechnungen
            Dim layoutInfo = BerechneLayoutParameter(pageSize, pageMargin)

            ' FixedDocument erstellen
            Dim doc = New FixedDocument()
            doc.DocumentPaginator.PageSize = pageSize

            ' Gruppen sortieren
            Dim sortedGroupView = New ListCollectionView(einteilung.Gruppenliste)
            sortedGroupView.SortDescriptions.Add(New SortDescription(NameOf(Gruppe.Benennung), ListSortDirection.Ascending))

            ' Seiten erstellen
            Dim currentPage As FixedPage = Nothing
            For i As Integer = 0 To sortedGroupView.Count - 1
                sortedGroupView.MoveCurrentToPosition(i)
                Dim gruppe = CType(sortedGroupView.CurrentItem, Gruppe)

                ' Neue Seite bei Bedarf
                If i Mod layoutInfo.ItemsPerPage = 0 Then
                    currentPage = New FixedPage With {
                        .Width = pageSize.Width,
                        .Height = pageSize.Height
                    }
                    Dim content = New PageContent()
                    TryCast(content, IAddChild).AddChild(currentPage)
                    doc.Pages.Add(content)
                End If

                ' UserControl für Gruppe erstellen
                Dim printControl = ErstelleDruckControl(gruppe, einteilung.Benennung, printversion, layoutInfo)

                ' Position berechnen und Control hinzufügen
                Dim currentRow As Integer = (i Mod layoutInfo.ItemsPerPage) \ layoutInfo.ColumnsPerPage
                Dim currentColumn As Integer = i Mod layoutInfo.ColumnsPerPage

                FixedPage.SetTop(printControl, pageMargin.Top + ((layoutInfo.PrintHeight + layoutInfo.VerticalMargin) * currentRow))
                FixedPage.SetLeft(printControl, pageMargin.Left + ((layoutInfo.PrintWidth + layoutInfo.HorizontalMargin) * currentColumn))

                currentPage.Children.Add(printControl)
            Next

            Return doc
        End Function

        ''' <summary>
        ''' Berechnet die Layout-Parameter für den Druck.
        ''' </summary>
        Private Function BerechneLayoutParameter(pageSize As Size, pageMargin As Thickness) As DruckLayoutInfo
            Const PRINT_HEIGHT As Double = 1000
            Const PRINT_WIDTH As Double = 730

            Dim availableHeight As Double = pageSize.Height - pageMargin.Top - pageMargin.Bottom
            Dim availableWidth As Double = pageSize.Width - pageMargin.Left - pageMargin.Right

            Dim rowsPerPage As Integer = CInt(Math.Floor(availableHeight / PRINT_HEIGHT))
            Dim columnsPerPage As Integer = CInt(Math.Floor(availableWidth / PRINT_WIDTH))

            If rowsPerPage = 0 Then rowsPerPage = 1
            If columnsPerPage = 0 Then columnsPerPage = 1

            Dim vMargin As Double = 0
            If rowsPerPage > 1 Then
                Dim vLeftOverSpace As Double = availableHeight - (PRINT_HEIGHT * rowsPerPage)
                vMargin = vLeftOverSpace / (rowsPerPage - 1)
            End If

            Dim hMargin As Double = 0
            If columnsPerPage > 1 Then
                Dim hLeftOverSpace As Double = availableWidth - (PRINT_WIDTH * columnsPerPage)
                hMargin = hLeftOverSpace / (columnsPerPage - 1)
            End If

            Return New DruckLayoutInfo With {
                .PrintHeight = PRINT_HEIGHT,
                .PrintWidth = PRINT_WIDTH,
                .RowsPerPage = rowsPerPage,
                .ColumnsPerPage = columnsPerPage,
                .ItemsPerPage = rowsPerPage * columnsPerPage,
                .VerticalMargin = vMargin,
                .HorizontalMargin = hMargin
            }
        End Function

        ''' <summary>
        ''' Erstellt das passende UserControl für den Druck.
        ''' </summary>
        Private Function ErstelleDruckControl(gruppe As Gruppe,
                                              einteilungName As String,
                                              printversion As Printversion,
                                              layoutInfo As DruckLayoutInfo) As IPrintableNotice
            Dim printControl As IPrintableNotice

            If printversion = printversion.TeilnehmerInfo Then
                printControl = New TeilnehmerAusdruckUserControl()
            Else
                printControl = New TrainerausdruckUserControl()
            End If

            ' Größe setzen
            DirectCast(printControl, UserControl).Height = layoutInfo.PrintHeight
            DirectCast(printControl, UserControl).Width = layoutInfo.PrintWidth

            ' Daten initialisieren
            printControl.InitPropsFromGroup(gruppe, einteilungName)

            Return printControl
        End Function

        ''' <summary>
        ''' Prüft, ob die Voraussetzungen für den Druck erfüllt sind.
        ''' </summary>
        Public Function KannDrucken(einteilung As Einteilung) As Boolean
            Return einteilung IsNot Nothing AndAlso
                   einteilung.Gruppenliste IsNot Nothing AndAlso
                   einteilung.Gruppenliste.Count > 0
        End Function

    End Class

    ''' <summary>
    ''' Hilfsklasse für Layout-Informationen beim Druck.
    ''' </summary>
    Friend Class DruckLayoutInfo
        Public Property PrintHeight As Double
        Public Property PrintWidth As Double
        Public Property RowsPerPage As Integer
        Public Property ColumnsPerPage As Integer
        Public Property ItemsPerPage As Integer
        Public Property VerticalMargin As Double
        Public Property HorizontalMargin As Double
    End Class

End Namespace