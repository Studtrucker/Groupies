Imports Groupies.Commands
Imports Groupies.Entities
Imports System.Text

Public Class LeistungsstufeDialog
    Implements Interfaces.IWindowMitModus

    Public Property Modus As Interfaces.IModus
    Public Property Dialog As Boolean Implements Interfaces.IWindowMitModus.Dialog
    Public ReadOnly Property Leistungsstufe() As Leistungsstufe

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Leistungsstufe = New Leistungsstufe()
        DataContext = _Leistungsstufe

    End Sub

    Private Sub HandleFaehigkeitNeuErstellenExecute(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New NeueFaehigkeitDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        If dlg.ShowDialog = True Then
            _Leistungsstufe.Faehigkeiten.Add(dlg.Faehigkeit)
        End If
    End Sub

    Private Function ValidateInput() As Boolean
        If Validation.GetHasError(Me) Then
            Return False
        End If
        Return True
    End Function

    Private Function GetErrors()
        Dim Fehlertext = String.Empty
        DirectCast(Validation.GetErrors(Me)(0).ErrorContent, List(Of String)).ForEach(Sub(Ft As String) Fehlertext &= Ft & vbNewLine)
        Return Fehlertext.Remove(Fehlertext.Count - 2, Len(vbNewLine))
    End Function


    Private Sub HandlerOkButton(sender As Object, e As RoutedEventArgs)
        BindingGroup.CommitEdit()
        If Validation.GetHasError(Me) Then
            MessageBox.Show(GetErrors, "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Error)
            Dialog = False
        Else
            LeistungsstufeView.SortierungTextbox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            LeistungsstufeView.BenennungTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            'LeistungsstufeView.FaehigkeitenDataGrid.GetBindingExpression(DataGrid.HasItemsProperty).UpdateSource()
            Dialog = True
        End If
    End Sub

    Private Sub HandlerCancelButton(sender As Object, e As RoutedEventArgs)
        BindingGroup.CancelEdit()
    End Sub

    Private Sub SchliessenButton_Click(sender As Object, e As RoutedEventArgs) Implements Interfaces.IWindowMitModus.HandlerSchliessenButton
        Modus.HandleClose(Me)
    End Sub

    Public Sub ModusEinstellen() Implements Interfaces.IWindowMitModus.ModusEinstellen
        Me.Titel.Text &= Modus.Titel
    End Sub

End Class
