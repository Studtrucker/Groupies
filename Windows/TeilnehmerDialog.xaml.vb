Imports Groupies.Controller
Imports System.ComponentModel
Imports Groupies.Entities


Public Class TeilnehmerDialog
    Implements Interfaces.IWindowMitModus

    Public Property Modus As Interfaces.IModus Implements Interfaces.IWindowMitModus.Modus
    Public ReadOnly Property Teilnehmer() As Teilnehmer
    Private ReadOnly _LeistungsstufenListCollectionView As ICollectionView

#Region "Konstruktor"

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Teilnehmer = New Teilnehmer
        DataContext = _Teilnehmer

        ' ListCollectionView für die Combobox erstellen
        _LeistungsstufenListCollectionView = New CollectionView(AppController.AktuellerClub.LeistungsstufenTextliste)
        LeistungsstandComboBox.ItemsSource = _LeistungsstufenListCollectionView

    End Sub

#End Region

#Region "Events"
    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        VornameTextBox.Focus()
    End Sub

    Private Sub HandleWindowClosing(sender As Object, e As CancelEventArgs)
        If DialogResult = True Then
            BindingGroup.CommitEdit()
            If Validation.GetHasError(Me) Then
                MessageBox.Show(GetErrors, "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Error)
                e.Cancel = True
            Else
                TeilnehmerIDTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                VornameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                NachnameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                GeburtstagDatePicker.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource()
                LeistungsstandComboBox.GetBindingExpression(ComboBox.SelectedValueProperty).UpdateSource()
                TelefonTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
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

    Public Sub ModusEinstellen() Implements Interfaces.IWindowMitModus.ModusEinstellen
        Me.Titel.Text &= Modus.Titel
    End Sub

    Public Sub Bearbeiten(Teilnehmer As Teilnehmer)
        _Teilnehmer = Teilnehmer
        DataContext = _Teilnehmer
    End Sub

    Public Sub Bearbeiten(Teilnehmer As BaseModel) Implements Interfaces.IWindowMitModus.Bearbeiten
        _Teilnehmer = Teilnehmer
        DataContext = _Teilnehmer
    End Sub

#End Region

End Class
