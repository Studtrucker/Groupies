Imports Groupies.Interfaces
Imports System.ComponentModel
Imports System.Windows.Markup

Public MustInherit Class Dialog(Of T)
    Inherits Window
    Implements Interfaces.IGenericWindowMitModus(Of T)


    Private _Objekt As T


    Public ReadOnly Property Objekt() As T
        Get
            Return _Objekt
        End Get
    End Property



#Region "Konstruktor"

    Public Sub New()
        InitializeComponent()
    End Sub


#End Region

#Region "Modus"
    Public Sub ModusEinstellen() Implements IGenericWindowMitModus(Of T).ModusEinstellen
        Me.Title &= Modus.Titel
        Me.Icon = New BitmapImage(New Uri(Modus.IconString, UriKind.Absolute))
    End Sub

    Public Sub Bearbeiten(Original As T) Implements IGenericWindowMitModus(Of T).Bearbeiten
        DataContext = Original
    End Sub

    Public Sub Erstellen() Implements IGenericWindowMitModus(Of T).Erstellen
        'DataContext = New T
    End Sub
    Public Sub Ansehen(Original As T) Implements IGenericWindowMitModus(Of T).Ansehen
        DataContext = Original
    End Sub

    Public Property Modus As IModus Implements IGenericWindowMitModus(Of T).Modus

#End Region

#Region "Fehlerbehandlung"

    Private Function GetErrors() As String
        Dim Fehlertext = String.Empty
        DirectCast(Validation.GetErrors(Me)(0).ErrorContent, List(Of String)).ForEach(Sub(Ft As String) Fehlertext &= Ft & vbNewLine)
        Return Fehlertext.Remove(Fehlertext.Count - 2, Len(vbNewLine))
    End Function

#End Region

#Region "Events"
    Private Sub HandleWindowClosing(sender As Object, e As CancelEventArgs)
        If DialogResult = True Then
            BindingGroup.CommitEdit()
            If Validation.GetHasError(Me) Then
                MessageBox.Show(GetErrors, "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Error)
                e.Cancel = True
            Else
                'TeilnehmerIDTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                'VornameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                'NachnameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                'GeburtstagDatePicker.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource()
                'LeistungsstandComboBox.GetBindingExpression(ComboBox.SelectedValueProperty).UpdateSource()
                'TelefonTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            End If
        Else
            BindingGroup.CancelEdit()
        End If
    End Sub
    Private Sub HandleButtonOKExecuted(sender As Object, e As RoutedEventArgs)
        DialogResult = True
    End Sub

    Private Sub HandleCancelButtonExecuted(sender As Object, e As RoutedEventArgs)
        DialogResult = False
    End Sub

    Public Sub Connect(connectionId As Integer, target As Object) Implements IComponentConnector.Connect
        Throw New NotImplementedException()
    End Sub

    Public Sub InitializeComponent() Implements IComponentConnector.InitializeComponent
        Throw New NotImplementedException()
    End Sub

#End Region


End Class
