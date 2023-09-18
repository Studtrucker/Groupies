Imports Skischule.Entities
Imports System.Windows.Markup

Namespace UserControls

    <ContentProperty("AngezeigterGruppenname")>
    Partial Public Class PrintableSkikursgruppe

        Public Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        End Sub

        Public Sub InitPropsFromFriend(Skikursgruppe As Skikursgruppe)

        End Sub

        Public Property Skilehrer As String
            Get
                Return txtSkilehrername.Text
            End Get
            Set(value As String)
                txtSkilehrername.Text = value
            End Set
        End Property

        Public Property Skilehrerfoto As ImageSource
            Get
                Return imgSkilehrerfoto.Source
            End Get
            Set(value As ImageSource)
                imgSkilehrerfoto.Source = value
            End Set
        End Property


        Public Property Skigruppenname As String
            Get
                Return txtSkigruppenname.Text
            End Get
            Set(value As String)
                txtSkigruppenname.Text = value
            End Set
        End Property

        Public Property Mitglieder As DataGrid
            Get
                Return lstMitglieder
            End Get
            Set(value As DataGrid)
                lstMitglieder = value
            End Set
        End Property

    End Class

End Namespace
