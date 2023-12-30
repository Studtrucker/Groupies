Imports System.ComponentModel
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Xml.Serialization

Public Class Window1

    Private _levelListCollectionView As ICollectionView

    Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _levelListCollectionView = New ListCollectionView(Groupies.Services.CurrentDataService.Skiclub.Levellist)
        _levelListCollectionView.SortDescriptions.Add(New SortDescription("SortNumber", ListSortDirection.Ascending))
        DataContext = _levelListCollectionView

    End Sub


End Class
