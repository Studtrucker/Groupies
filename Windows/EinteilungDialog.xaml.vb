Imports Groupies.Interfaces
Imports Groupies.Entities
Imports Groupies.Services

Public Class EinteilungDialog
    Implements Interfaces.IWindowMitModus

    Property Einteilung As Einteilung
    Public Property Modus As Interfaces.IModus Implements Interfaces.IWindowMitModus.Modus
    Public Property Dialog As Boolean Implements Interfaces.IWindowMitModus.Dialog

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Einteilung = New Einteilung
        DataContext = _Einteilung

    End Sub

    Public Sub ModusEinstellen() Implements IWindowMitModus.ModusEinstellen
        Me.Titel.Text &= Modus.Titel
    End Sub

    Private Function GetErrors()
        Dim Fehlertext = String.Empty
        DirectCast(Validation.GetErrors(Me)(0).ErrorContent, List(Of String)).ForEach(Sub(Ft As String) Fehlertext &= Ft & vbNewLine)
        Return Fehlertext.Remove(Fehlertext.Count - 2, Len(vbNewLine))
    End Function

    Private Sub NeueEinteilung()
        _Einteilung.Benennung = "Tag2"
        _Einteilung.Gruppenliste = TemplateService.StandardGruppenErstellen(5)
    End Sub

    Private Sub HandlerOkButton(sender As Object, e As RoutedEventArgs)
        NeueEinteilung()
        BindingGroup.CommitEdit()
        If Validation.GetHasError(Me) Then
            MessageBox.Show(GetErrors, "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Error)
            Dialog = False
        Else
            LeistungsstufeView.SortierungTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            LeistungsstufeView.BenennungTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            'LeistungsstufeView.FaehigkeitenDataGrid.GetBindingExpression(DataGrid.HasItemsProperty).UpdateSource()
            Dialog = True
        End If
    End Sub

    Private Sub HandlerCancelButton(sender As Object, e As RoutedEventArgs)
        BindingGroup.CancelEdit()
    End Sub

    Private Sub SchliessenButton_Click(sender As Object, e As RoutedEventArgs) Implements Interfaces.IWindowMitModus.HandleSchliessenButton
        Modus.HandleClose(Me)
    End Sub

End Class
