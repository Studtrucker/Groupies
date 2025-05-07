Imports System.ComponentModel
Imports System.Text
Imports Groupies.Entities
Imports Groupies.Commands

Public Class GruppeDialog

    'Implements Interfaces.IWindowMitModus

    'Public Property Modus As Interfaces.IModus Implements Interfaces.IWindowMitModus.Modus

    Public ReadOnly Property Gruppe() As Gruppe

    Private ReadOnly _levelListCollectionView As ICollectionView
    Private ReadOnly _instructorListCollectionView As ICollectionView

#Region "Konstruktor"

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Gruppe = New Gruppe
        DataContext = _Gruppe

        _levelListCollectionView = New ListCollectionView(Controller.AppController.AktuellerClub.LeistungsstufenTextliste.ToList)
        _instructorListCollectionView = New ListCollectionView(Controller.AppController.AktuellerClub.SelectedEinteilung.GruppenloseTrainer)

        LeistungsstufeCombobox.ItemsSource = _levelListCollectionView

    End Sub

#End Region

#Region "Events"
    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs)
        AusgabenameTextBox.Focus()
    End Sub

    Private Sub HandleWindowClosing(sender As Object, e As ComponentModel.CancelEventArgs)
        If DialogResult = True Then
            BindingGroup.CommitEdit()
            If Validation.GetHasError(Me) Then
                MessageBox.Show(GetErrors, "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Error)
                e.Cancel = True
            Else
                AusgabenameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                BenennungTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                SortierungTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            End If
        Else
            BindingGroup.CancelEdit()
        End If
    End Sub

    Private Sub HandleCancelButtonExecuted(sender As Object, e As RoutedEventArgs)
        DialogResult = False
    End Sub

    Private Sub HandleButtonOKExecuted(sender As Object, e As RoutedEventArgs)
        DialogResult = True
    End Sub

#End Region

#Region "Formular-spezifische Handler"

#End Region

#Region "Fehlerbehandlung"

    Private Function GetErrors() As String
        Dim Fehlertext = String.Empty
        DirectCast(Validation.GetErrors(Me)(0).ErrorContent, List(Of String)).ForEach(Sub(Ft As String) Fehlertext &= Ft & vbNewLine)
        Return Fehlertext.Remove(Fehlertext.Count - 2, Len(vbNewLine))
    End Function

#End Region

#Region "Modus-Handler"

    'Public Sub ModusEinstellen() Implements Interfaces.IWindowMitModus.ModusEinstellen
    '    Me.Titel.Text &= Modus.Titel
    'End Sub

#End Region



    Private Sub HandleButtonOKCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
    End Sub

    Private Sub HandleButtonOKExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = True
    End Sub
    Private Sub HandleOkButton(sender As Object, e As RoutedEventArgs)

    End Sub


End Class
