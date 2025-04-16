Imports Groupies.Controller
Imports System.ComponentModel
Imports Groupies.Entities
Imports Groupies.Interfaces

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

    'Public Sub New(Teilnehmer As Teilnehmer)

    '    ' Dieser Aufruf ist für den Designer erforderlich.
    '    InitializeComponent()

    '    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    '    DataContext = Teilnehmer

    '    ' ListCollectionView für die Combobox erstellen
    '    _LeistungsstufenListCollectionView = New CollectionView(AppController.AktuellerClub.Leistungsstufenliste.Select(Function(LS) LS.Benennung))
    '    LeistungsstandComboBox.ItemsSource = _LeistungsstufenListCollectionView

    'End Sub
#End Region

#Region "Events"
    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        VornameTextBox.Focus()
    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        If DialogResult = True Then
            BindingGroup.CommitEdit()
            If Validation.GetHasError(Me) Then
                MessageBox.Show(GetErrors, "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Error)
                e.Cancel = True
            Else
                'SpitznameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                VornameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                NachnameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                'eMailTextbox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                TelefonTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                'FotoImage.GetBindingExpression(Image.SourceProperty).UpdateSource()
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

    Public Sub Bearbeiten(Of T)(Original As T) Implements IWindowMitModus.Bearbeiten
        If TypeOf Original Is Teilnehmer Then
            _Teilnehmer = Original
            DataContext = _Teilnehmer
        Else
            Throw New InvalidCastException("Das übergebene Objekt ist kein Teilnehmer.")
        End If
    End Sub

#End Region


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
            DialogResult = False
        Else
            TeilnehmerIDTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            VornameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            NachnameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            GeburtstagDatePicker.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource()
            LeistungsstandComboBox.GetBindingExpression(ComboBox.SelectedValueProperty).UpdateSource()
            TelefonTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            DialogResult = True
        End If
    End Sub

    Private Sub HandleCancelButton(sender As Object, e As RoutedEventArgs)
        BindingGroup.CancelEdit()
    End Sub

End Class
