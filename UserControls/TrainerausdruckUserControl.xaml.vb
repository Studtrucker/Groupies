Imports Groupies.Entities
Imports System.IO
Imports System.Windows.Markup
Imports CDS = Groupies.Controller.AppController
Imports Groupies.Interfaces

Namespace UserControls

    <ContentProperty("Group")>
    Public Class TrainerausdruckUserControl
        Implements IPrintableNotice

        Public Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        End Sub

        Public Sub InitPropsFromGroup(Gruppe As Gruppe) Implements IPrintableNotice.InitPropsFromGroup

            AusgabeTeilnehmerinfo = Gruppe.AusgabeTeilnehmerinfo
            AusgabeTrainerinfo = Gruppe.Benennung
            Mitgliederliste = New TeilnehmerCollection(Gruppe.Mitgliederliste.Geordnet.ToList)
            Faehigkeitenliste = New FaehigkeitCollection(Gruppe.Leistungsstufe.Faehigkeiten.ToList)
            If Gruppe.Leistungsstufe IsNot Nothing Then
                Gruppenleistungsstufe = Gruppe.Leistungsstufe.Benennung
                AnzahlGruppenmitglieder = Gruppe.Mitgliederliste.Count
                'BeschreibungLeistungsanforderung = Gruppe.Leistungsstufe.Beschreibungstext
            End If
            If Gruppe.Trainer IsNot Nothing Then
                TrainerSpitzname = Gruppe.Trainer.Spitzname
            End If

            ' For Style setting
            If Gruppe.Mitgliederliste.Count > 14 Then
                DataContext = "ZuGross"
            End If

        End Sub


        Public Property AusgabeTrainerinfo As String
            Get
                Return AusgabeTrainerinfoTextBlock.Text
            End Get
            Set(value As String)
                AusgabeTrainerinfoTextBlock.Text = value
            End Set
        End Property

        Public Property Gruppenleistungsstufe As String
            Get
                Return GruppenleistungsstufeTextBlock.Text
            End Get
            Set(value As String)
                GruppenleistungsstufeTextBlock.Text = value
            End Set
        End Property

        Public Property AnzahlGruppenmitglieder As String
            Get
                Return AnzahlGruppenmitgliederTextblock.Text
            End Get
            Set(value As String)
                AnzahlGruppenmitgliederTextblock.Text = value
            End Set
        End Property

        Public Property AusgabeTeilnehmerinfo As String
            Get
                Return AusgabeTeilnehmerinfoTextBlock.Text
            End Get
            Set(value As String)
                AusgabeTeilnehmerinfoTextBlock.Text = value
            End Set
        End Property

        Public Property TrainerSpitzname As String
            Get
                Return TrainerSpitznameTextBox.Text
            End Get
            Set(value As String)
                TrainerSpitznameTextBox.Text = value
            End Set
        End Property

        Public Property Mitgliederliste As TeilnehmerCollection
            Get
                Return MitgliederlisteDataGrid.ItemsSource
            End Get
            Set(value As TeilnehmerCollection)
                MitgliederlisteDataGrid.ItemsSource = value
            End Set
        End Property

        'Public Property BeschreibungLeistungsanforderung As String
        '    Get
        '        'Return BeschreibungLeistungsanforderungTextblock.Text
        '    End Get
        '    Set(value As String)
        '        'BeschreibungLeistungsanforderungTextblock.Text = value
        '    End Set
        'End Property

        Public Property Faehigkeitenliste As FaehigkeitCollection
            Get
                Return FaehigkeitenlisteDataGrid.ItemsSource
            End Get
            Set(value As FaehigkeitCollection)
                FaehigkeitenlisteDataGrid.ItemsSource = value
            End Set
        End Property

    End Class
End Namespace
