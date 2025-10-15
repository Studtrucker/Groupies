Imports System.IO
Imports System.Xml.Serialization
Imports Groupies.Interfaces
Imports System.Collections.ObjectModel


' Damit eine Property nicht im xml gespeichert wird, muss es mit dem Attribut <XmlIgnore> gekennzeichnet werden

Namespace Entities.Generation4

    Public Class Club
        Inherits BaseModel
        'Implements IModel
        Implements IClub

#Region "Fields"
        Private _DateiGeneration As String
        Private _Clubname As String
        'Private _GruppenIDListe As ObservableCollection(Of Guid)
        'Private _TeilnehmerIDListe As ObservableCollection(Of Guid)
        'Private _TrainerIDListe As ObservableCollection(Of Guid)
        'Private _EinteilungIDListe As ObservableCollection(Of Guid)
        'Private _LeistungsstufenIDListe As ObservableCollection(Of Guid)
        'Private _FaehigkeitenIDListe As ObservableCollection(Of Guid)
        Private _Gruppenliste As GruppeCollection
        Private _Teilnehmerliste As TeilnehmerCollection
        Private _Trainerliste As TrainerCollection
        Private _Einteilungsliste As EinteilungCollection
        Private _Leistungsstufenliste As LeistungsstufeCollection
        Private _Faehigkeitenliste As FaehigkeitCollection
#End Region

#Region "Konstruktor"

        ''' <summary>
        ''' Parameterloser Konstruktor für das
        ''' Einlesen von XML Dateien notwendig
        ''' </summary>
        Public Sub New()
            DateiGeneration = "4"
        End Sub

#End Region

#Region "Properties"

        ''' <summary>
        ''' Die Generation der Datei
        ''' </summary>
        ''' <returns></returns>
        Public Property DateiGeneration As String Implements IClub.DateiGeneration
            Get
                Return _DateiGeneration
            End Get
            Set(value As String)
                _DateiGeneration = value
            End Set
        End Property

        ''' <summary>
        ''' Der Clubname
        ''' </summary>
        ''' <returns></returns>
        Public Property ClubName As String Implements IClub.Name
            Get
                Return _Clubname
            End Get
            Set(value As String)
                _Clubname = value
            End Set
        End Property

        '''' <summary>
        '''' Liste der Tages-Einteilung IDs
        '''' </summary>
        'Public Property EinteilungIDListe As ObservableCollection(Of Guid)
        '    Get
        '        Return _EinteilungIDListe
        '    End Get
        '    Set(value As ObservableCollection(Of Guid))
        '        _EinteilungIDListe = value
        '    End Set
        'End Property

        ''' <summary>
        ''' Die Tages-Einteilungen
        ''' </summary>
        ''' <returns></returns>
        Public Property Einteilungsliste() As EinteilungCollection
            Get
                Return _Einteilungsliste
            End Get
            Set(value As EinteilungCollection)
                _Einteilungsliste = value
            End Set
        End Property

        '''' <summary>
        '''' Liste der LeistungsstufenIDs im aktuellen Club
        '''' </summary>
        '''' 
        '''' <returns></returns>
        'Public Property LeistungsstufenIDListe As ObservableCollection(Of Guid)
        '    Get
        '        Return _LeistungsstufenIDListe
        '    End Get
        '    Set(value As ObservableCollection(Of Guid))
        '        _LeistungsstufenIDListe = value
        '    End Set
        'End Property

        ''' <summary>
        ''' Liste aller Leistungsstufen, 
        ''' Basis für die Angabe in den Gruppen und bei den Teilnehmern
        ''' </summary>
        ''' <returns></returns>
        Public Property Leistungsstufenliste() As LeistungsstufeCollection
            Get
                Return _Leistungsstufenliste
            End Get
            Set(value As LeistungsstufeCollection)
                _Leistungsstufenliste = value
            End Set
        End Property

        '''' <summary>
        '''' Eine Liste der FaehigkeitenIDs im aktuellen Club
        '''' </summary>
        '''' <returns></returns>
        'Public Property FaehigkeitenIDListe As ObservableCollection(Of Guid)
        '    Get
        '        Return _FaehigkeitenIDListe
        '    End Get
        '    Set(value As ObservableCollection(Of Guid))
        '        _FaehigkeitenIDListe = value
        '    End Set
        'End Property


        ''' <summary>
        ''' Liste aller Fähigkeiten,
        ''' Basis für die Beschreibung der Leistungsstufen
        ''' </summary>
        ''' <returns></returns>
        Public Property Faehigkeitenliste() As FaehigkeitCollection
            Get
                Return _Faehigkeitenliste
            End Get
            Set(value As FaehigkeitCollection)
                _Faehigkeitenliste = value
            End Set
        End Property

        '''' <summary>
        '''' Eine Liste der TrainerIDs im aktuellen Club
        '''' </summary>
        '''' <returns></returns>
        'Public Property TrainerIDListe As ObservableCollection(Of Guid)
        '    Get
        '        Return _TrainerIDListe
        '    End Get
        '    Set(value As ObservableCollection(Of Guid))
        '        _TrainerIDListe = value
        '    End Set
        'End Property

        ''' <summary>
        ''' Liste aller Trainer,
        ''' Basis für die Angabe in den Gruppen.
        ''' Zusätzlich kein jede Einteilung einen Trainerpool haben.
        ''' </summary>
        ''' <returns></returns>
        Public Property Trainerliste() As TrainerCollection
            Get
                Return _Trainerliste
            End Get
            Set(value As TrainerCollection)
                _Trainerliste = value
            End Set
        End Property

        '''' <summary>
        '''' Liste der TeilnehmerIDs im aktuellen Club
        '''' </summary>
        'Public Property TeilnehmerIDListe As ObservableCollection(Of Guid)
        '    Get
        '        Return _TeilnehmerIDListe
        '    End Get
        '    Set(value As ObservableCollection(Of Guid))
        '        _TeilnehmerIDListe = value
        '    End Set
        'End Property

        ''' <summary>
        ''' Liste aller Teilnehmer.
        ''' Basis für die Mitgliederliste in den Gruppen.
        ''' Zusätzlich kann jeder Einteilung eine Teilnehmerliste zugeordnet werden.
        ''' </summary>
        ''' <returns></returns>
        Public Property Teilnehmerliste() As TeilnehmerCollection
            Get
                Return _Teilnehmerliste
            End Get
            Set(value As TeilnehmerCollection)
                _Teilnehmerliste = value
            End Set
        End Property


        '''' <summary>
        '''' Liste der TeilnehmerIDs im aktuellen Club
        '''' </summary>
        '''' <returns></returns>
        'Public Property GruppenIDListe As ObservableCollection(Of Guid)
        '    Get
        '        Return _GruppenIDListe
        '    End Get
        '    Set(value As ObservableCollection(Of Guid))
        '        _GruppenIDListe = value
        '    End Set
        'End Property


        ''' <summary>
        ''' Liste der aller Gruppen.
        ''' Basis für die Gruppen in den Einteilungen.
        ''' </summary>
        ''' <returns></returns>
        Public Property Gruppenliste() As GruppeCollection
            Get
                Return _Gruppenliste
            End Get
            Set(value As GruppeCollection)
                _Gruppenliste = value
            End Set
        End Property

#End Region

#Region "Funktionen und Methoden"

        Public Overrides Function ToString() As String
            Return ClubName
        End Function

        Public Function LadeGroupies(Datei As String) As Generation4.Club Implements IClub.LadeGroupies
            Using fs = New FileStream(Datei, FileMode.Open)
                Dim serializer = New XmlSerializer(GetType(Generation4.Club))
                Dim loadedSkiclub As Generation4.Club
                Try
                    loadedSkiclub = TryCast(serializer.Deserialize(fs), Generation4.Club)
                    Return Map2AktuelleGeneration(loadedSkiclub)
                Catch ex As InvalidDataException
                    Throw ex
                End Try
            End Using
        End Function

        Public Function Map2AktuelleGeneration(Skiclub As IClub) As Club Implements IClub.Map2AktuelleGeneration
            Return MappingGeneration4.MapSkiClub2Club(Skiclub)
        End Function

        Public Function Map2AktuelleGeneration(Skiclub As IClub, Dateiname As String) As Club Implements IClub.Map2AktuelleGeneration
            Return MappingGeneration4.MapSkiClub2Club(Skiclub, Dateiname)
        End Function

#End Region

    End Class


End Namespace
