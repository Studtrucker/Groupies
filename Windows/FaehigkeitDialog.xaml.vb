Imports System.Text
Imports Groupies.Entities
Imports Groupies.Commands
'Imports Microsoft.Office.Interop.Excel
Imports Groupies.Interfaces

Public Class FaehigkeitDialog
    'Implements Interfaces.IWindowMitModus

    Public ReadOnly Property Faehigkeit() As Faehigkeit

    'Public Property Modus As IModus Implements Interfaces.IWindowMitModus.Modus

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Faehigkeit = New Faehigkeit()
        DataContext = _Faehigkeit

    End Sub

    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogOk, AddressOf HandleButtonOKExecuted, AddressOf HandleButtonOKCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogCancel, AddressOf HandleButtonCancelExecuted))
        SortierungTextBox.Focus()

    End Sub

    Private Sub HandleButtonOKCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
    End Sub

    Private Sub HandleButtonOKExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = True
    End Sub

    Private Sub HandleButtonCancelExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = False
    End Sub

    Private Function GetErrors() As String
        Dim Fehlertext = String.Empty
        DirectCast(Validation.GetErrors(Me)(0).ErrorContent, List(Of String)).ForEach(Sub(Ft As String) Fehlertext &= Ft & vbNewLine)
        Return Fehlertext.Remove(Fehlertext.Count - 2, Len(vbNewLine))
    End Function

    Private Sub HandleOkButton(sender As Object, e As RoutedEventArgs)
        BindingGroup.CommitEdit()
        If Validation.GetHasError(Me) Then
            MessageBox.Show(GetErrors, "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Error)
            DialogResult = False
        Else
            SortierungTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            BeschreibungTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            BenennungTextBox.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource()
            DialogResult = True
        End If
    End Sub

    Private Sub HandleCancelButton(sender As Object, e As RoutedEventArgs)
        BindingGroup.CancelEdit()
    End Sub


    'Private Sub ModusEinstellen() Implements Interfaces.IWindowMitModus.ModusEinstellen
    '    'Me.Titel.Text &= Modus.Titel
    'End Sub

End Class
