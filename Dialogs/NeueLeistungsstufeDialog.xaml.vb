Imports System.Text
Imports Groupies.Entities
Imports Groupies.Commands

Public Class NeueLeistungsstufeDialog

    Public ReadOnly Property Leistungsstufe() As Leistungsstufe

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Leistungsstufe = New Leistungsstufe()
        DataContext = _Leistungsstufe

    End Sub

    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogOk, AddressOf HandleButtonOKExecuted, AddressOf HandleButtonOKCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogCancel, AddressOf HandleButtonCancelExecuted))
        NamingField.Focus()

    End Sub

    Private Sub HandleButtonOKCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = Not Leistungsstufe.HasErrors
    End Sub


    Private Sub HandleButtonOKExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = True
    End Sub

    Private Sub HandleButtonCancelExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = False
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim text = New StringBuilder
        Dim x = Controller.AppController.CurrentClub.Leistungsstufenliste.ToList.OrderByDescending(Function(Ls) Ls.Sortierung).ToList.Select(Function(Ls) $"{Ls.Sortierung} {Ls.Benennung}{Environment.NewLine}")
        x.ToList.ForEach(Sub(Ls) text.Append(Ls))
        text.Remove(text.Length - Environment.NewLine.Length, Environment.NewLine.Length)
        MessageBox.Show($"{text}", "Aktuelle Leistungsstufen", MessageBoxButton.OK, MessageBoxImage.Information)
    End Sub
End Class
