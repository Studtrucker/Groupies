Imports System.IO
Imports System.Text
Imports Groupies.Entities
Imports Groupies.Commands
Imports Microsoft.Win32
Imports Groupies.Interfaces

Public Class TrainerDialog

    Implements Interfaces.IWindowMitModus

    Public Property Modus As Interfaces.IModus Implements Interfaces.IWindowMitModus.Modus

    Public ReadOnly Property Trainer() As Trainer

#Region "Konstruktor"
    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Trainer = New Trainer(String.Empty)
        DataContext = _Trainer



    End Sub

#End Region

#Region "Events"
    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        SpitznameTextBox.Focus()
    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        If DialogResult = True Then
            BindingGroup.CommitEdit()
            If Validation.GetHasError(Me) Then
                MessageBox.Show(GetErrors, "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Error)
                e.Cancel = True
            Else
                SpitznameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                VornameTextbox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                NachnameTextbox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                eMailTextbox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                TelefonTextbox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
                FotoImage.GetBindingExpression(Image.SourceProperty).UpdateSource()
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

    Private Sub HandleDrop(sender As Object, e As DragEventArgs)

        Dim sb = New StringBuilder()

        Dim filepath As String() = TryCast(e.Data.GetData(DataFormats.FileDrop, True), String())
        Dim validPictureFile = False

        If filepath.Length > 0 Then
            Dim extension As String = Path.GetExtension(filepath(0)).ToLower()

            If ImageTypes.AllImageTypes.Contains(extension) Then
                Using filestream = New FileStream(filepath(0), FileMode.Open)
                    Dim buffer = New Byte(filestream.Length - 1) {}
                    filestream.Read(buffer, 0, filestream.Length)
                    _Trainer.Foto = buffer
                    validPictureFile = True
                End Using
            Else
                sb.AppendLine("- Es werden nur die folgenden Dateiformate")
                sb.Append("  unterstützt: ")

                For Each fileformat As String In ImageTypes.AllImageTypes
                    sb.Append(fileformat)
                    sb.Append(", ")
                Next
                ' Das letzte ", " entfernen und Zeilenumbruch einfügen
                sb.Remove(sb.Length - 2, 1)
                sb.AppendLine()
            End If
        End If

        If filepath.Length > 1 AndAlso validPictureFile Then
            sb.AppendLine("- Sie haben mehr als eine Datei gedroppt,")
            sb.AppendLine("  es wird nur die erste verwendet")
        End If


    End Sub

    Private Sub HandleDragOver(sender As Object, e As DragEventArgs)

        e.Effects = DragDropEffects.None
        Dim filepath As String() = TryCast(e.Data.GetData(DataFormats.FileDrop, True), String())

        If filepath.Length > 0 Then
            Dim extension As String = Path.GetExtension(filepath(0)).ToLower()

            If ImageTypes.AllImageTypes.Contains(extension) Then
                e.Effects = DragDropEffects.Copy
            End If
        End If

        e.Handled = True
    End Sub

#End Region

#Region "Fehlerbehandlung"

    Private Function GetErrors() As String
        Dim Fehlertext = String.Empty
        DirectCast(Validation.GetErrors(Me)(0).ErrorContent, List(Of String)).ForEach(Sub(Ft As String) Fehlertext &= Ft & vbNewLine)
        Return Fehlertext.Remove(Fehlertext.Count - 2, Len(vbNewLine))
    End Function
#End Region



#Region "Modus-Handler"
    Public Sub Bearbeiten(Of T)(Trainer As Trainer)
        _Trainer = Trainer
        DataContext = _Trainer
    End Sub
    Public Sub Bearbeiten(Of T)(Original As T) Implements IWindowMitModus.Bearbeiten
        Throw New NotImplementedException
    End Sub


    Public Sub ModusEinstellen() Implements IWindowMitModus.ModusEinstellen
        Me.Titel.Text &= Modus.Titel
    End Sub



#End Region


End Class
