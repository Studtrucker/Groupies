Imports Skischule.Entities
Imports System.IO
Imports System.Windows.Markup

Namespace UserControls

    <ContentProperty("Skikursgruppe")>
    Partial Public Class PrintableSkikursgruppe

        Public Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        End Sub

        Public Sub InitPropsFromSkikursgruppe(Skikursgruppe As Skikursgruppe)
            Skigruppenname = Skikursgruppe.AngezeigterGruppenname
            Mitglieder = Skikursgruppe.Mitgliederliste

            If Skikursgruppe.Skilehrer IsNot Nothing Then
                Skilehrer = Skikursgruppe.Skilehrer.AngezeigterName
                If Skikursgruppe.Skilehrer.HatFoto Then
                    Dim bi = New BitmapImage
                    bi.BeginInit()
                    bi.StreamSource = New MemoryStream(Skikursgruppe.Skilehrer.Foto)
                    bi.EndInit()
                    Skilehrerfoto = bi
                Else
                    ' Todo: Ersatzfoto festlegen
                    Skilehrerfoto = New BitmapImage(New Uri("/Images/icons8-ski-goggles-96.png", UriKind.Relative))
                End If
            End If

        End Sub

        Public Property Skigruppenname As String
            Get
                Return txtSkigruppenname.Text
            End Get
            Set(value As String)
                txtSkigruppenname.Text = value
            End Set
        End Property

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

        Public Property Mitglieder As TeilnehmerCollection
            Get
                Return lstMitglieder.ItemsSource
            End Get
            Set(value As TeilnehmerCollection)
                lstMitglieder.ItemsSource = value
            End Set
        End Property

    End Class

End Namespace
