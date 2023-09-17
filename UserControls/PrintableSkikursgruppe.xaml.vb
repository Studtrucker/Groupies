Imports Skischule.Entities
Imports System.Windows.Markup

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

    Public Property Skigruppen As String
        Get
            Return Skigruppenname.Text
        End Get
        Set(value As String)
            Skigruppenname.Text = value
        End Set
    End Property

End Class
