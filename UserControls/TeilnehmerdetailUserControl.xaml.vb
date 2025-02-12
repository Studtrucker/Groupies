Imports System.ComponentModel
Imports Groupies.Entities
Imports CDS = Groupies.Controller.AppController

Namespace UserControls

    Public Class TeilnehmerdetailUserControl

        Private _levelListCollectionView As ICollectionView

        Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
            _levelListCollectionView = New ListCollectionView(New LeistungsstufeCollection)
            AddHandler Loaded, AddressOf ParticipantView_Loaded
        End Sub

        Private Sub ParticipantView_Loaded(sender As Object, e As RoutedEventArgs)
            _levelListCollectionView = New ListCollectionView(CDS.AktuellerClub.Leistungsstufenliste)
            _levelListCollectionView.SortDescriptions.Add(New SortDescription("SortNumber", ListSortDirection.Ascending))
            LeistungsstandComboBox.ItemsSource = _levelListCollectionView
        End Sub

    End Class
End Namespace
