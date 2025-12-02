Imports System.ComponentModel
Imports System.Windows.Markup
Imports Groupies.Interfaces
Imports Groupies.UserControls
Imports Groupies.ViewModels
Imports Groupies.Entities.Generation4

Public Class DruckService

    ''' <summary>
    ''' Erstellt ein FixedDocument für den Ausdruck einer Einteilung.
    ''' Hinweis: Diese Methode verändert das übergebene <paramref name="Einteilung"/> nicht mehr.
    ''' </summary>
    Public Shared Function PrintoutInfo(einteilung As Einteilung, printversion As Printversion, pageSize As Size, pageMargin As Thickness) As FixedDocument
        If einteilung Is Nothing Then Throw New ArgumentNullException(NameOf(einteilung))
        If einteilung.Gruppenliste Is Nothing OrElse einteilung.Gruppenliste.Count = 0 Then
            Throw New ArgumentException("Einteilung enthält keine Gruppen.", NameOf(einteilung))
        End If

        ' Seitenparameter validieren
        Dim availablePageHeight As Double = Math.Max(1.0, pageSize.Height - pageMargin.Top - pageMargin.Bottom)
        Dim availablePageWidth As Double = Math.Max(1.0, pageSize.Width - pageMargin.Left - pageMargin.Right)

        Const printFriendHeight As Double = 1000
        Const printFriendWidth As Double = 730

        Dim rowsPerPage As Integer = Math.Max(1, CInt(Math.Floor(availablePageHeight / printFriendHeight)))
        Dim columnsPerPage As Integer = Math.Max(1, CInt(Math.Floor(availablePageWidth / printFriendWidth)))
        Dim participantsPerPage As Integer = rowsPerPage * columnsPerPage

        Dim vMarginBetweenFriends As Double = 0
        If rowsPerPage > 1 Then
            Dim vLeftOverSpace As Double = availablePageHeight - (printFriendHeight * rowsPerPage)
            vMarginBetweenFriends = vLeftOverSpace / (rowsPerPage - 1)
        End If

        Dim hMarginBetweenFriends As Double = 0
        If columnsPerPage > 1 Then
            Dim hLeftOverSpace As Double = availablePageWidth - (printFriendWidth * columnsPerPage)
            hMarginBetweenFriends = hLeftOverSpace / (columnsPerPage - 1)
        End If

        ' Sortierte Liste statt ListCollectionView/MoveCurrentToPosition (leichter und schneller)
        Dim groups = einteilung.Gruppenliste.OrderBy(Function(g) g.Benennung).ToList()

        Dim doc As New FixedDocument()
        'With { .DocumentPaginator = New FixedDocument().DocumentPaginator}
        doc.DocumentPaginator.PageSize = pageSize

        Dim page As FixedPage = Nothing

        For i As Integer = 0 To groups.Count - 1
            Dim skikursgruppe As Gruppe = groups(i)

            ' Neue Seite anlegen, wenn nötig
            If i Mod participantsPerPage = 0 Then
                page = New FixedPage()
                Dim content As New PageContent()
                DirectCast(content, IAddChild).AddChild(page)
                doc.Pages.Add(content)
            End If

            Dim printable As IPrintableNotice = If(printversion = Printversion.TeilnehmerInfo,
                                                  CType(New TeilnehmerAusdruckUserControl(), IPrintableNotice),
                                                  CType(New TrainerausdruckUserControl(), IPrintableNotice))

            Dim uc As UserControl = DirectCast(printable, UserControl)
            uc.Height = printFriendHeight
            uc.Width = printFriendWidth

            ' Keine Seiteneffekte: Name der Einteilung darf nicht verändert werden; Abbruch statt InputBox
            If String.IsNullOrWhiteSpace(einteilung.Benennung) Then
                Throw New ArgumentException("Einteilung.Benennung darf nicht leer sein. Bitte vor dem Druck benennen.", NameOf(einteilung))
            End If

            printable.InitPropsFromGroup(skikursgruppe, einteilung.Benennung)

            Dim posOnPage As Integer = i Mod participantsPerPage
            Dim currentRow As Integer = posOnPage \ columnsPerPage   ' ganze Zahl-Division
            Dim currentColumn As Integer = posOnPage Mod columnsPerPage

            FixedPage.SetTop(uc, pageMargin.Top + ((uc.Height + vMarginBetweenFriends) * currentRow))
            FixedPage.SetLeft(uc, pageMargin.Left + ((uc.Width + hMarginBetweenFriends) * currentColumn))

            page.Children.Add(uc)
        Next

        Return doc
    End Function

End Class
