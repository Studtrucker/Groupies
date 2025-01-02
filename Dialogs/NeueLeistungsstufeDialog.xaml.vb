Imports System.Text
Imports Groupies.Entities
Imports Groupies.Commands
Imports System.Data.Linq

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

        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogOk, AddressOf HandleButtonOKExecute, AddressOf HandleButtonOKCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogCancel, AddressOf HandleButtonCancelExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.FaehigkeitNeuErstellen, AddressOf HandleFaehigkeitNeuErstellenExecute))
        ' Binding hier erstellen, da das Binding aus XAML hier noch nicht aktiv ist
        Dim b As New Binding(NameOf(LeistungsstufeView.SortierungText))
        b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        b.Path = New PropertyPath("Sortierung")
        Dim IVR As New ValidationRules.IntegerValidationRule
        Dim RVR As New ValidationRules.SortierungLeistungsstufeUniqueValidationRule
        b.ValidationRules.Add(IVR)
        b.ValidationRules.Add(RVR)
        ' Sortierung ist per default leer. Gleich hier mit einem Error markieren
        Dim be = LeistungsstufeView.SortierungText.SetBinding(TextBox.TextProperty, b)
        Dim Fehler = New ValidationError(IVR, be, "Die Sortierung ist eine Pflichtangabe", Nothing)

        Validation.MarkInvalid(be, Fehler)
    End Sub

    Private Sub HandleFaehigkeitNeuErstellenExecute(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New NeueFaehigkeitDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        If dlg.ShowDialog = True Then
            _Leistungsstufe.Faehigkeiten.Add(dlg.Faehigkeit)
        End If
    End Sub

    Private Sub HandleButtonOKCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = _Leistungsstufe.IsOk
    End Sub


    Private Sub HandleButtonOKExecute(sender As Object, e As ExecutedRoutedEventArgs)
        If ValidateInput Then
            DialogResult = True
        Else
            MessageBox.Show(GetErrors())
        End If

    End Sub

    Private Function ValidateInput() As Boolean
        Return Not Validation.GetHasError(LeistungsstufeView.SortierungText)
    End Function

    Private Function GetErrors() As String
        Dim sb As New StringBuilder
        For Each [Error] In Validation.GetErrors(LeistungsstufeView.SortierungText)
            sb.AppendLine([Error].ErrorContent.ToString)
        Next
        Return sb.ToString
    End Function

    Private Sub HandleButtonCancelExecute(sender As Object, e As ExecutedRoutedEventArgs)
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
