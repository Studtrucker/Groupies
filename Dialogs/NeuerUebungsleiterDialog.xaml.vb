Imports System.IO
Imports System.Text
Imports Skischule.Entities
Imports Microsoft.Win32

Public Class NeuerUebungsleiterDialog

    Public ReadOnly Property Skilehrer() As Instructor

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Skilehrer = New Instructor
        DataContext = _Skilehrer

    End Sub


    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        txtAngezeigterName.Focus()
    End Sub

    Private Sub HandleButtonOKClick(sender As Object, e As RoutedEventArgs)
        If ValidateInput() Then
            DialogResult = True
        Else
            MessageBox.Show(GetErrors)
        End If
    End Sub

    Private Function ValidateInput() As Boolean
        Return Not Validation.GetHasError(txtAngezeigterName)
    End Function

    Private Function GetErrors() As String
        Dim sb = New StringBuilder

        For Each [Error] In Validation.GetErrors(txtAngezeigterName)
            sb.AppendLine([Error].ErrorContent.ToString)
        Next

        Return sb.ToString

    End Function

    Private Sub HandleButtonCancelClick(sender As Object, e As RoutedEventArgs)
        DialogResult = False

    End Sub

    Private Sub HandleDrop(sender As Object, e As DragEventArgs)
        Dim sb As StringBuilder = New StringBuilder()
        Dim filepath As String() = TryCast(e.Data.GetData(DataFormats.FileDrop, True), String())
        Dim validPictureFile = False

        If filepath.Length > 0 Then
            Dim extension As String = Path.GetExtension(filepath(0)).ToLower()

            If ImageTypes.AllImageTypes.Contains(extension) Then
                Using filestream = New FileStream(filepath(0), FileMode.Open)
                    Dim buffer = New Byte(filestream.Length - 1) {}
                    filestream.Read(buffer, 0, filestream.Length)
                    _Skilehrer.Foto = buffer
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

        If sb.ToString().Length > 0 Then
            txtErrorsAndWarnings.Text = sb.ToString()
        Else
            txtErrorsAndWarnings.Text = String.Empty
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


End Class
