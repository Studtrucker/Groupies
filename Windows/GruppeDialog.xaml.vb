Imports System.ComponentModel
Imports System.Text
Imports Groupies.Entities
Imports Groupies.Commands
Imports Groupies.Interfaces

Public Class GruppeDialog
    Implements Interfaces.IWindowMitModus

    Public ReadOnly Property Group() As Gruppe

    Public Property Modus As IModus Implements IWindowMitModus.Modus
    Public Property Dialog As Boolean Implements IWindowMitModus.Dialog


    Private ReadOnly _levelListCollectionView As ICollectionView
    Private ReadOnly _instructorListCollectionView As ICollectionView

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Group = New Gruppe
        DataContext = _Group

        _levelListCollectionView = New ListCollectionView(Controller.AppController.AktuellerClub.LeistungsstufenTextliste.ToList)
        _instructorListCollectionView = New ListCollectionView(Controller.AppController.AktuellerClub.GruppenloseTrainer)

        LeistungsstufeCombobox.ItemsSource = _levelListCollectionView

    End Sub

    Private Function GetErrors() As String
        Dim Fehlertext = String.Empty
        DirectCast(Validation.GetErrors(Me)(0).ErrorContent, List(Of String)).ForEach(Sub(Ft As String) Fehlertext &= Ft & vbNewLine)
        Return Fehlertext.Remove(Fehlertext.Count - 2, Len(vbNewLine))
    End Function

    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogOk, AddressOf HandleButtonOKExecuted, AddressOf HandleButtonOKCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogCancel, AddressOf HandleButtonCancelExecuted))

        AusgabenameTextBox.Focus()

    End Sub

    Private Sub HandleButtonOKCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
    End Sub

    Private Sub HandleButtonOKExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = True
    End Sub
    Private Sub HandleOkButton(sender As Object, e As RoutedEventArgs)
        BindingGroup.CommitEdit()
        If Validation.GetHasError(Me) Then
            MessageBox.Show(GetErrors, "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Error)
            Dialog = False
        Else
            AusgabenameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            BenennungTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            SortierungTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            Dialog = True
        End If
    End Sub

    Private Sub HandleButtonCancelExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        BindingGroup.CancelEdit()
    End Sub

    Public Sub ModusEinstellen() Implements IWindowMitModus.ModusEinstellen
        'Me.Titel.Text &= Modus.Titel
    End Sub

    Public Sub HandleSchliessenButton(sender As Object, e As RoutedEventArgs) Implements IWindowMitModus.HandleSchliessenButton
        Modus.HandleClose(Me)
    End Sub
End Class
