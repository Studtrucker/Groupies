Imports Groupies.Interfaces
Imports Groupies.Entities
Imports Groupies.Services

Public Class EinteilungDialog
    Implements Interfaces.IWindowMitModus

    Property Einteilung As Einteilung
    Public Property Modus As Interfaces.IModus Implements Interfaces.IWindowMitModus.Modus

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Einteilung = New Einteilung
        DataContext = _Einteilung

    End Sub

    Public Sub KopiereAktuelleGruppen(OriginGruppen As GruppeCollection)
        Dim KopierteListe As New GruppeCollection
        KopierteListe.AddRange(Controller.AppController.KopiereListeMitNeuenObjekten(Of Gruppe)(OriginGruppen.ToList, Function(t) New Gruppe(t)))
        Einteilung.Gruppenliste = KopierteListe
    End Sub

    Private Function GetErrors()
        Dim Fehlertext = String.Empty
        DirectCast(Validation.GetErrors(Me)(0).ErrorContent, List(Of String)).ForEach(Sub(Ft As String) Fehlertext &= Ft & vbNewLine)
        Return Fehlertext.Remove(Fehlertext.Count - 2, Len(vbNewLine))
    End Function

    Private Sub HandlerOkButton(sender As Object, e As RoutedEventArgs)
        BindingGroup.CommitEdit()
        If Validation.GetHasError(Me) Then
            MessageBox.Show(GetErrors, "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Error)
            DialogResult = False
        Else
            EinteilungUserControl.SortierungTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            EinteilungUserControl.BenennungTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            'LeistungsstufeView.FaehigkeitenDataGrid.GetBindingExpression(DataGrid.HasItemsProperty).UpdateSource()
            DialogResult = True
        End If
    End Sub

    Private Sub HandlerCancelButton(sender As Object, e As RoutedEventArgs)
        BindingGroup.CancelEdit()
    End Sub

    Public Sub ModusEinstellen() Implements Interfaces.IWindowMitModus.ModusEinstellen
        Me.Titel.Text &= Modus.Titel
    End Sub

    Private Sub Bearbeiten(Einteilung As BaseModel) Implements Interfaces.IWindowMitModus.Bearbeiten
        Throw New NotImplementedException()
    End Sub

End Class
