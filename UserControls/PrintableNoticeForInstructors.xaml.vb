Imports Groupies.Entities
Imports System.IO
Imports System.Windows.Markup
Imports CDS = Groupies.Controller.AppController
Imports Groupies.Interfaces

Namespace UserControls

    <ContentProperty("Group")>
    Public Class PrintableNoticeForInstructors
        Implements IPrintableNotice

        Public Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        End Sub

        Public Sub InitPropsFromGroup(Group As Gruppe) Implements IPrintableNotice.InitPropsFromGroup

            GroupPrintName = Group.Ausgabename
            GroupNaming = Group.Benennung
            Members = Group.Mitglieder.ParticipantCollectionOrdered
            If Group.Leistungsstufe IsNot Nothing Then
                GroupLevelNaming = Group.Leistungsstufe.Benennung
            End If
            If Group.Trainer IsNot Nothing Then
                GroupLeaderPrintName = Group.Trainer.Spitzname
            End If

            ' For Style setting
            If Group.Mitglieder.Count > 14 Then
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

        Public Property Members As TeilnehmerCollection
            Get
                Return MembersDataGrid.ItemsSource
            End Get
            Set(value As TeilnehmerCollection)
                MembersDataGrid.ItemsSource = value
            End Set
        End Property

    End Class
End Namespace
