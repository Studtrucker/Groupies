Imports Groupies.Entities
Imports System.IO
Imports System.Windows.Markup
Imports CDS = Groupies.Controller.AppController
Imports Groupies.Interfaces

Namespace UserControls

    <ContentProperty("Skikursgruppe")>
    Partial Public Class TeilnehmerAusdruckUserControl
        Implements IPrintableNotice

        Public Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        End Sub

        Public Sub InitPropsFromGroup(Gruppe As Gruppe) Implements IPrintableNotice.InitPropsFromGroup

            AusgabeTeilnehmerinfo = Gruppe.AusgabeTeilnehmerinfo
            Mitgliederliste = New TeilnehmerCollection(Gruppe.Mitgliederliste.Geordnet.ToList)

            If Gruppe.Trainer IsNot Nothing Then
                Trainer = Gruppe.Trainer.AusgabeTeilnehmerInfo
                If Gruppe.Trainer.HatFoto Then
                    Dim bi = New BitmapImage
                    bi.BeginInit()
                    bi.StreamSource = New MemoryStream(Gruppe.Trainer.Foto)
                    bi.EndInit()
                    TrainerFoto = bi
                Else
                    ' Todo: Ersatzfoto festlegen
                    TrainerFoto = New BitmapImage(New Uri("/Images/icons8-ski-goggles-96.png", UriKind.Relative))
                End If
            End If

            If Gruppe.Mitgliederliste.Count > 14 Then
                DataContext = "ZuGross"
            End If

            Dim x = New Groupies.TemplateSelectors.SkikursGroesseStyleSelector
            lstMitgliederliste.Style = x.SelectStyle(Mitgliederliste, lstMitgliederliste)

        End Sub

        Public Property AusgabeTeilnehmerinfo As String
            Get
                Return txtAusgabeTeilnehmerinfo.Text
            End Get
            Set(value As String)
                txtAusgabeTeilnehmerinfo.Text = value
            End Set
        End Property

        Public Property Trainer As String
            Get
                Return txtTrainer.Text
            End Get
            Set(value As String)
                txtTrainer.Text = value
            End Set
        End Property

        Public Property TrainerFoto As ImageSource
            Get
                Return imgTrainerFoto.Source
            End Get
            Set(value As ImageSource)
                imgTrainerFoto.Source = value
            End Set
        End Property

        Public Property Mitgliederliste As TeilnehmerCollection
            Get
                Return lstMitgliederliste.ItemsSource
            End Get
            Set(value As TeilnehmerCollection)
                lstMitgliederliste.ItemsSource = value
            End Set
        End Property

    End Class

End Namespace
