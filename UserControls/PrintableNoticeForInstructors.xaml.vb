Imports Skischule.Entities
Imports System.IO
Imports System.Windows.Markup
Imports CDS = Skischule.DataService.CurrentDataService


Namespace UserControls

    <ContentProperty("Group")>
    Public Class PrintableNoticeForInstructors
        Implements IPrintableNotice

        Public Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        End Sub

        Public Sub InitPropsFromGroup(Group As Group) Implements IPrintableNotice.InitPropsFromGroup

            GroupPrintName = Group.GroupPrintNaming
            GroupNaming = Group.GroupNaming
            Members = Group.GroupMembers
            If Not Group.GroupLevel Is Nothing Then
                GroupLevelNaming = Group.GroupLevel.LevelNaming
            End If
            If Not Group.GroupLeader Is Nothing Then
                GroupLeaderPrintName = Group.GroupLeader.InstructorPrintName
            End If

            ' For Style setting
            If Group.GroupMembers.Count > 14 Then
                DataContext = "ZuGross"
            End If

        End Sub


        Public Property GroupNaming As String
            Get
                Return GroupNamingTextBlock.Text
            End Get
            Set(value As String)
                GroupNamingTextBlock.Text = value
            End Set
        End Property

        Public Property GroupLevelNaming As String
            Get
                Return GroupLevelNamingTextBlock.Text
            End Get
            Set(value As String)
                GroupLevelNamingTextBlock.Text = value
            End Set
        End Property

        Public Property GroupPrintName As String
            Get
                Return GroupPrintNameTextBlock.Text
            End Get
            Set(value As String)
                GroupPrintNameTextBlock.Text = value
            End Set
        End Property

        Public Property GroupLeaderPrintName As String
            Get
                Return GroupLeaderPrintNameTextBox.Text
            End Get
            Set(value As String)
                GroupLeaderPrintNameTextBox.Text = value
            End Set
        End Property

        Public Property Members As ParticipantCollection
            Get
                Return MembersDataGrid.ItemsSource
            End Get
            Set(value As ParticipantCollection)
                MembersDataGrid.ItemsSource = value
            End Set
        End Property

    End Class
End Namespace
