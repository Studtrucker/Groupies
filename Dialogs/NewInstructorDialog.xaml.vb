Imports System.IO
Imports System.Text
Imports Skiclub.Entities
Imports Skiclub.Commands
Imports Microsoft.Win32

Public Class NewInstructorDialog

    Public ReadOnly Property Instructor() As Instructor

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Instructor = New Instructor
        DataContext = _Instructor

    End Sub
    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogOk, AddressOf HandleButtonOKExecuted, AddressOf HandleButtonOKCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogCancel, AddressOf HandleButtonCancelExecuted))

        PrintNameField.Focus()
    End Sub

    Private Sub HandleButtonOKCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = Instructor.IsOk
    End Sub

    Private Sub HandleButtonOKExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = True
    End Sub

    Private Sub HandleButtonCancelExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = False
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
                    _Instructor.InstructorPicture = buffer
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


End Class
