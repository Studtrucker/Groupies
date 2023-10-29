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

        Public Sub InitPropsFromSkikursgruppe(Skikurs As Group, Skilehrer As InstructorCollection)
            Skigruppenname = Skikurs.GroupPrintName
            Mitglieder = Skikurs.Groupmembers

            If Not Skikurs.Groupleader Is Nothing Then
                Skilehrername = Skilehrer.GetPrintName(Skikurs.Groupleader)
                If Skilehrer.GetHatFoto(Skikurs.Groupleader) Then
                    Dim bi = New BitmapImage
                    bi.BeginInit()
                    bi.StreamSource = New MemoryStream(Skilehrer.GetFoto(Skikurs.Groupleader))
                    bi.EndInit()
                    Skilehrerfoto = bi
                Else
                    ' Todo: Ersatzfoto festlegen
                    Skilehrerfoto = New BitmapImage(New Uri("/Images/icons8-ski-goggles-96.png", UriKind.Relative))
                End If
            End If

            If Skikurs.Groupmembers.Count <= 3 Then
                DataContext = "VielZuKlein"
            ElseIf Skikurs.Groupmembers.Count <= 6 Then
                DataContext = "ZuKlein"
            ElseIf Skikurs.Groupmembers.Count < 12 Then
                DataContext = "ZuGross"
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

        Public Property Skilehrername As String
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

        Public Property Mitglieder As ParticipantCollection
            Get
                Return lstMitglieder.ItemsSource
            End Get
            Set(value As ParticipantCollection)
                lstMitglieder.ItemsSource = value
            End Set
        End Property

    End Class

End Namespace
