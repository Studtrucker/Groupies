Imports System.ComponentModel
Imports System.IO
Imports System.Text
Imports Groupies.Entities
Imports CDS = Groupies.Services.CurrentDataService

Public Class InstructorsWindow
#Region "Fields"
    Private _instructorListCollectionView As ICollectionView
#End Region

#Region "Constructor"
    Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _instructorListCollectionView = New ListCollectionView(New TrainerCollection)

    End Sub
#End Region

#Region "Window Events"
    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If CDS.Club IsNot Nothing AndAlso CDS.Club.Trainerliste IsNot Nothing Then
            setView(CDS.Club.Trainerliste)
        End If
    End Sub
#End Region

#Region "Helper Methoden"
    Private Sub setView(Instructorlist As TrainerCollection)
        _instructorListCollectionView = New ListCollectionView(Instructorlist)
        If _instructorListCollectionView.CanSort Then
            _instructorListCollectionView.SortDescriptions.Add(New SortDescription("InstructorVorname", ListSortDirection.Ascending))
        End If
        DataContext = _instructorListCollectionView
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
                    DirectCast(_instructorListCollectionView.CurrentItem, Trainer).InstructorPicture = buffer
                    '                    _Instructor.InstructorPicture = buffer
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
End Class
