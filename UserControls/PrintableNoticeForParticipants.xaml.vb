Imports Groupies.Entities
Imports System.IO
Imports System.Windows.Markup
Imports CDS = Groupies.Services.CurrentDataService
Imports Groupies.Interfaces

Namespace UserControls

    <ContentProperty("Skikursgruppe")>
    Partial Public Class PrintableNoticeForParticipants
        Implements IPrintableNotice

        Public Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        End Sub

        Public Sub InitPropsFromGroup(Group As Group) Implements IPrintableNotice.InitPropsFromGroup

            GroupPrintName = Group.GroupPrintNaming
            Members = Group.GroupMembers.ParticipantCollectionOrdered

            If Group.GroupLeader IsNot Nothing Then
                GroupLeaderPrintName = CDS.Skiclub.Instructorlist.GetPrintName(Group.GroupLeader)
                If CDS.Skiclub.Instructorlist.GetHatFoto(Group.GroupLeader) Then
                    Dim bi = New BitmapImage
                    bi.BeginInit()
                    bi.StreamSource = New MemoryStream(CDS.Skiclub.Instructorlist.GetFoto(Group.GroupLeader))
                    bi.EndInit()
                    GroupLeaderPicture = bi
                Else
                    ' Todo: Ersatzfoto festlegen
                    GroupLeaderPicture = New BitmapImage(New Uri("/Images/icons8-ski-goggles-96.png", UriKind.Relative))
                End If
            End If

            If Group.GroupMembers.Count > 14 Then
                DataContext = "ZuGross"
            End If

            Dim x = New Groupies.TemplateSelectors.SkikursGroesseStyleSelector
            lstMitglieder.Style = x.SelectStyle(Members, lstMitglieder)

        End Sub

        Public Property GroupPrintName As String
            Get
                Return txtSkigruppenname.Text
            End Get
            Set(value As String)
                txtSkigruppenname.Text = value
            End Set
        End Property

        Public Property GroupLeaderPrintName As String
            Get
                Return txtSkilehrername.Text
            End Get
            Set(value As String)
                txtSkilehrername.Text = value
            End Set
        End Property

        Public Property GroupLeaderPicture As ImageSource
            Get
                Return imgSkilehrerfoto.Source
            End Get
            Set(value As ImageSource)
                imgSkilehrerfoto.Source = value
            End Set
        End Property

        Public Property Members As ParticipantCollection
            Get
                Return lstMitglieder.ItemsSource
            End Get
            Set(value As ParticipantCollection)
                lstMitglieder.ItemsSource = value
            End Set
        End Property

    End Class

End Namespace
