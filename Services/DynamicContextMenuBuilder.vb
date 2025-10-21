Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Threading

Namespace Services
    ''' <summary>
    ''' Erzeugt dynamisch ein ContextMenu aus einer Collection von Objekten vom Typ T.
    ''' Benötigt einen Mapper, der aus einem Objekt vom Typ T ein MenuItem erstellt.
    ''' </summary>
    Public Class DynamicContextMenuBuilder(Of T)

        Private ReadOnly _itemToMenuItem As Func(Of T, MenuItem)

        Public Sub New(itemToMenuItem As Func(Of T, MenuItem))
            If itemToMenuItem Is Nothing Then
                Throw New ArgumentNullException(NameOf(itemToMenuItem))
            End If
            _itemToMenuItem = itemToMenuItem
        End Sub

        ''' <summary>
        ''' Baut ein ContextMenu basierend auf der übergebenen Auflistung.
        ''' Ist die Aufrufumgebung nicht der UI-Thread, wird auf den Dispatcher gewechselt.
        ''' </summary>
        Public Function Build(items As IEnumerable(Of T)) As ContextMenu
            Dim factory = Function() As ContextMenu
                              Dim ctx = New ContextMenu()
                              If items Is Nothing Then
                                  Return ctx
                              End If
                              For Each it In items
                                  Dim mi = _itemToMenuItem(it)
                                  If mi IsNot Nothing Then
                                      ctx.Items.Add(mi)
                                  End If
                              Next
                              Return ctx
                          End Function

            Dim disp = If(Application.Current?.Dispatcher, Dispatcher.CurrentDispatcher)
            If disp.CheckAccess() Then
                Return factory()
            Else
                Return CType(disp.Invoke(factory), ContextMenu)
            End If
        End Function

        ''' <summary>
        ''' Hängt an ein FrameworkElement ein ContextMenu, das beim Öffnen aus <paramref name="itemsProvider"/> neu aufgebaut wird.
        ''' </summary>
        ''' <param name="element">Das UI-Element, dem das ContextMenu zugewiesen wird.</param>
        ''' <param name="itemsProvider">Funktion, die die aktuellen Items liefert.</param>
        Public Sub AttachTo(element As FrameworkElement, itemsProvider As Func(Of IEnumerable(Of T)))
            If element Is Nothing Then Throw New ArgumentNullException(NameOf(element))
            If itemsProvider Is Nothing Then Throw New ArgumentNullException(NameOf(itemsProvider))

            ' Neues ContextMenu erstellen und Opening-Handler registrieren
            Dim ctx As New ContextMenu()
            AddHandler ctx.Opened, Sub(sender, e)
                                       Try
                                           Dim items = itemsProvider() ' aktuelle Daten
                                           Dim newMenu = Build(items)
                                           ' Replace items
                                           ctx.Items.Clear()
                                           For Each mi As MenuItem In newMenu.Items.OfType(Of MenuItem)()
                                               ctx.Items.Add(mi)
                                           Next
                                       Catch ex As Exception
                                           ' Fehlerbehandlung minimal (keine UI-Blockade)
                                           ' Optional: Logging hinzufügen
                                       End Try
                                   End Sub

            element.ContextMenu = ctx
        End Sub


        Public Sub Anwenden()
            Dim lvEinteilungen As New ListView
            ' Beispiel: ContextMenu für Einteilung erstellen und an ein ListView anhängen
            Dim builder = New Services.DynamicContextMenuBuilder(Of Entities.Generation4.Einteilung)(
                Function(e)
                    Dim mi = New MenuItem With {.Header = e.Benennung}
                    ' Beispiel-Command: öffnet Details (RelayCommand im Projekt vorausgesetzt)
                    mi.Command = New RelayCommand(Of Object)(Sub(o) MessageBox.Show($"Einteilung: {e.Benennung}"))
                    Return mi
                End Function)

            ' An ein FrameworkElement (z.B. ListView namens lvEinteilungen) anhängen:
            builder.AttachTo(lvEinteilungen, Function() DateiService.AktuellerClub?.Einteilungsliste)
        End Sub
        ' Hinweise / Empfehlungen
        ' •	Verwende in der Factory keine direkten UI-Operationen, die das MenuItem später beim Klick verändern; stattdessen Command-Handler mit den benötigten Referenzen übergeben (z. B. RelayCommand).
        ' •	Wenn du zusätzliche Menuelemente (z. B. "Neu", "Löschen" oder Trennlinien) an alle Menüs anhängen willst, füge diese in der Opened-Lambda vor oder nach dem Kopieren hinzu.
        ' •	Bei umfangreichen Menüs kann das Erzeugen beim Öffnen spürbar werden — falls nötig, cachest du vorher erzeugte Menüs und refreshst nur bei Änderungen.
        ' Möchtest du, dass ich ein konkretes Beispiel für Einteilung mit deinen existierenden Commands (.Command-Implementierungen / RelayCommand) erstelle und direkt In deine View (z. B. GruppendetailUserControl) integriere?
    End Class

End Namespace
