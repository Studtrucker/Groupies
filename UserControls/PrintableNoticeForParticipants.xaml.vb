Imports Skischule.Entities
Imports System.IO
Imports System.Windows.Markup

Namespace UserControls

    <ContentProperty("Skikursgruppe")>
    Partial Public Class PrintableNoticeForParticipants
        Implements IPrintableNotice

        Public Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        End Sub

        Public Sub InitPropsFromGroup(Group As Group, InstructorList As InstructorCollection) Implements IPrintableNotice.InitPropsFromGroup
            GroupPrintName = Group.GroupPrintNaming
            Members = Group.GroupMembers

            If Not Group.GroupLeader Is Nothing Then
                GroupLeaderPrintName = InstructorList.GetPrintName(Group.GroupLeader)
                If InstructorList.GetHatFoto(Group.GroupLeader) Then
                    Dim bi = New BitmapImage
                    bi.BeginInit()
                    bi.StreamSource = New MemoryStream(InstructorList.GetFoto(Group.GroupLeader))
                    bi.EndInit()
                    GroupLeaderPicture = bi
                Else
                    ' Todo: Ersatzfoto festlegen
                    GroupLeaderPicture = New BitmapImage(New Uri("/Images/icons8-ski-goggles-96.png", UriKind.Relative))
                End If
            End If

            If Group.GroupMembers.Count <= 3 Then
                DataContext = "VielZuKlein"
            ElseIf Group.GroupMembers.Count <= 6 Then
                DataContext = "ZuKlein"
            ElseIf Group.GroupMembers.Count < 12 Then
                DataContext = "ZuGross"
            End If


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
