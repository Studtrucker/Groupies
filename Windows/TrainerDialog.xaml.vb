Imports System.IO
Imports System.Text
Imports Groupies.Entities
Imports Groupies.Commands
Imports Microsoft.Win32
Imports Groupies.Interfaces

Public Class TrainerDialog
    Implements Interfaces.IWindowMitModus

    Public ReadOnly Property Trainer() As Trainer
    Public Property Modus As Interfaces.IModus

    Public Property Dialog As Boolean Implements IWindowMitModus.Dialog


    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Trainer = New Trainer(String.Empty)
        DataContext = _Trainer

    End Sub

    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        SpitznameTextBox.Focus()
    End Sub

    Public Sub Bearbeiten(Trainer As Trainer)
        _Trainer = Trainer
        DataContext = _Trainer
    End Sub

    Private Sub HandleButtonOKExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = True
    End Sub


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

    Public Sub ModusEinstellen() Implements IWindowMitModus.ModusEinstellen
        Me.Titel.Text &= Modus.Titel
    End Sub
    Private Function GetErrors() As String
        Dim Fehlertext = String.Empty
        DirectCast(Validation.GetErrors(Me)(0).ErrorContent, List(Of String)).ForEach(Sub(Ft As String) Fehlertext &= Ft & vbNewLine)
        Return Fehlertext.Remove(Fehlertext.Count - 2, Len(vbNewLine))
    End Function


    Private Sub HandleOkButton(sender As Object, e As RoutedEventArgs)
        BindingGroup.CommitEdit()
        If Validation.GetHasError(Me) Then
            MessageBox.Show(GetErrors, "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Error)
            Dialog = False
        Else
            SpitznameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            VornameTextbox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            NachnameTextbox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            eMailTextbox.GetBindingExpression(TextBox.TextProperty).UpdateSource()
            Dialog = True
        End If
    End Sub

    Private Sub HandleCancelButton(sender As Object, e As RoutedEventArgs)
        BindingGroup.CancelEdit()
    End Sub

    Private Sub SchliessenButton_Click(sender As Object, e As RoutedEventArgs)
        Modus.HandleClose(Me)
    End Sub

    Public Sub HandleSchliessenButton(sender As Object, e As RoutedEventArgs) Implements IWindowMitModus.HandleSchliessenButton
        Throw New NotImplementedException()
    End Sub
End Class
