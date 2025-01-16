Imports Groupies.Controller
Imports System.ComponentModel
Imports Groupies.Entities

Public Class TeilnehmerDialog
    Implements Interfaces.IWindowMitModus
    Public Property Dialog As Boolean Implements Interfaces.IWindowMitModus.Dialog
    Public Property Modus As Interfaces.IModus Implements Interfaces.IWindowMitModus.Modus
    Public Property Teilnehmer() As Teilnehmer
    Private ReadOnly _LeistungsstufenListCollectionView As ICollectionView

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Teilnehmer = New Teilnehmer
        DataContext = _Teilnehmer

        ' ListCollectionView für die Combobox erstellen
        _LeistungsstufenListCollectionView = New CollectionView(AppController.CurrentClub.LeistungsstufenTextliste)
        LeistungsstandComboBox.ItemsSource = _LeistungsstufenListCollectionView

    End Sub

    Public Sub New(Teilnehmer As Teilnehmer)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        DataContext = Teilnehmer

        ' ListCollectionView für die Combobox erstellen
        _LeistungsstufenListCollectionView = New CollectionView(AppController.CurrentClub.Leistungsstufenliste.Select(Function(LS) LS.Benennung))
        LeistungsstandComboBox.ItemsSource = _LeistungsstufenListCollectionView

    End Sub
    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        VornameTextBox.Focus()
    End Sub

    Public Sub ModusEinstellen() Implements Interfaces.IWindowMitModus.ModusEinstellen
        Me.Titel.Text &= Modus.Titel
    End Sub


    Private Function GetErrors() As String
        Dim Fehlertext = String.Empty
        DirectCast(Validation.GetErrors(Me)(0).ErrorContent, List(Of String)).ForEach(Sub(Ft As String) Fehlertext &= Ft & vbNewLine)
        Return Fehlertext.Remove(Fehlertext.Count - 2, Len(vbNewLine))
    End Function

    Private Function ValidateInput() As Boolean
        If Validation.GetHasError(Me) Then
            Return False
        End If
        Return True
    End Function

    Private Sub HandleOkButton(sender As Object, e As RoutedEventArgs)
        BindingGroup.CommitEdit()
        If Validation.GetHasError(Me) Then
            MessageBox.Show(GetErrors, "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Error)
            Dialog = False
        Else
            VornameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            NachnameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            GeburtstagDatePicker.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource()
            LeistungsstandComboBox.GetBindingExpression(ComboBox.SelectedValueProperty).UpdateSource()
            Dialog = True
        End If
    End Sub

    Private Sub HandleCancelButton(sender As Object, e As RoutedEventArgs)
        BindingGroup.CancelEdit()
    End Sub

    Private Sub SchliessenButton_Click(sender As Object, e As RoutedEventArgs) Implements Interfaces.IWindowMitModus.HandleSchliessenButton
        Modus.HandleClose(Me)
    End Sub

End Class
