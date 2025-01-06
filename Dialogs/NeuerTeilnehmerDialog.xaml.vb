Imports Groupies.Entities
Imports System.ComponentModel
Imports DS = Groupies.Services
Imports Groupies.Commands
Imports Groupies.Controller
Imports System.Text
Imports Groupies.UserControls

Public Class NeuerTeilnehmerDialog
    Public ReadOnly Property Teilnehmer() As Teilnehmer
    Private ReadOnly _instructorListCollectionView As ICollectionView
    Private ReadOnly _levelListCollectionView As ICollectionView

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Teilnehmer = New Teilnehmer
        DataContext = _Teilnehmer

        _levelListCollectionView = New CollectionView(AppController.CurrentClub.Leistungsstufenliste)
        _instructorListCollectionView = New CollectionView(AppController.CurrentClub.Gruppenliste)

        ParticipantLevelComboBox.ItemsSource = _levelListCollectionView

    End Sub

    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogOk, AddressOf HandleButtonOKExecuted, AddressOf HandleButtonOKCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogCancel, AddressOf HandleButtonCancelExecuted))

        VornameText.Focus()

    End Sub

    Private Sub HandleButtonOKCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = Teilnehmer.IsOk
    End Sub

    Private Sub HandleButtonOKExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        MessageBox.Show(Validation.GetValidationAdornerSiteFor(NachnameText).InvalidateProperty).
        BindingGroup.CommitEdit()
        MessageBox.Show(GetErrors)
    End Sub

    Private Sub HandleButtonCancelExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        BindingGroup.CancelEdit()
        DialogResult = False
    End Sub
    Private Function GetErrors()
        Dim sb As New StringBuilder
        sb.AppendLine(GetErrorsVorname)
        sb.AppendLine(GetErrorsNachname)
        Return sb.ToString
    End Function
    Private Function GetErrorsVorname() As String
        Dim sb As New StringBuilder
        For Each [Error] In Validation.GetErrors(VornameText)
            sb.AppendLine([Error].ErrorContent.ToString)
        Next
        Return sb.ToString
    End Function

    Private Function GetErrorsNachname() As String
        Dim sb As New StringBuilder
        For Each [Error] In Validation.GetErrors(NachnameText)
            sb.AppendLine([Error].ErrorContent.ToString)
        Next
        Return sb.ToString
    End Function

    Private Function ValidateInput() As Boolean
        If Validation.GetHasError(VornameText) OrElse Validation.GetHasError(NachnameText) Then
            Return False
        End If
        Return True
    End Function
End Class
