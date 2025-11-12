Imports Groupies.Entities.Generation4
Imports System.IO
Imports System.Windows.Markup
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
            Benennung = Gruppe.Benennung
            'AusgabeTrainerinfo = Gruppe.Alias

            Mitgliederliste = New TeilnehmerCollection(Gruppe.Mitgliederliste.Geordnet.ToList)
            If Gruppe.Leistungsstufe IsNot Nothing Then
                If Gruppe.Leistungsstufe IsNot Nothing Then
                    Gruppenleistungsstufe = Gruppe.Leistungsstufe.Benennung
                    AnzahlGruppenmitglieder = Gruppe.Mitgliederliste.Count
                    'BeschreibungLeistungsanforderung = Gruppe.Leistungsstufe.Beschreibungstext
                    If Gruppe.Leistungsstufe.Faehigkeiten IsNot Nothing Then Faehigkeitenliste = New FaehigkeitCollection(Gruppe.Leistungsstufe.Faehigkeiten.ToList)
                End If
            End If
            If Gruppe.Trainer IsNot Nothing Then
                TrainerSpitzname = Gruppe.Trainer.Alias
                TrainerTelefon = Gruppe.Trainer.Telefonnummer
            End If

            ' For Style setting
            If Gruppe.Mitgliederliste.Count > 14 Then
                DataContext = "ZuGross"
            End If

        End Sub

        Public Sub InitPropsFromGroup(Gruppe As Gruppe, BenennungGruppeneinteilung As String) Implements IPrintableNotice.InitPropsFromGroup

            Me.BenennungGruppeneinteilung = BenennungGruppeneinteilung
            InitPropsFromGroup(Gruppe)

        End Sub

        Public Property BenennungGruppeneinteilung As String
            Get
                Return BenennungGruppeneinteilungTextblock.Text
            End Get
            Set(value As String)
                BenennungGruppeneinteilungTextblock.Text = value
            End Set
        End Property

        Public Property Benennung As String
            Get
                Return BenennungTextBlock.Text
            End Get
            Set(value As String)
                BenennungTextBlock.Text = value
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

        Public Property TrainerSpitzname As String
            Get
                Return TrainerSpitznameTextBox.Text
            End Get
            Set(value As String)
                TrainerSpitznameTextBox.Text = value
            End Set
        End Property

        Public Property TrainerTelefon As String
            Get
                Return TrainerTelefonTextBox.Text
            End Get
            Set(value As String)
                TrainerTelefonTextBox.Text = value
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
