Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Xml.Serialization

Namespace Entities.Generation4


    ''' <summary>
    ''' Einteilungsspezifische Gruppendaten
    ''' </summary>
    Public Class GruppeVariable
        Inherits BaseModel
        Implements IModel

#Region "Felder"
        Private _Ident As Guid
        Private _MitgliederIDListe As ObservableCollection(Of Guid)
        Private _Mitgliederliste As TeilnehmerCollection
        Private _TrainerID As Guid
        Private _Trainer As Trainer
        Private _GruppenstammID As Guid
        Private _Gruppenstamm As Gruppenstamm
#End Region

#Region "Konstruktor"

        Public Sub New()
            Ident = Guid.NewGuid()
            MitgliederIDListe = New ObservableCollection(Of Guid)
            Mitgliederliste = New TeilnehmerCollection
        End Sub

        ''' <summary>
        ''' Kopierkonstruktor für tiefes Kopieren
        ''' </summary>
        ''' <param name="OriginGruppe"></param>
        Public Sub New(OriginGruppe As Gruppe)
            Ident = Guid.NewGuid()
            Trainer = OriginGruppe.Trainer
            TrainerID = OriginGruppe.TrainerID
            MitgliederIDListe = OriginGruppe.MitgliederIDListe
            Mitgliederliste = New TeilnehmerCollection()
            For Each m In OriginGruppe.Mitgliederliste
                If m IsNot Nothing Then
                    Mitgliederliste.Add(New Teilnehmer(m))
                End If
            Next
        End Sub

#End Region

#Region "Properties"
        ''' <summary>
        ''' Eindeutige Gruppenkennung
        ''' </summary>
        ''' <returns></returns>
        Public Property Ident As Guid Implements IModel.Ident
            Get
                Return _Ident
            End Get
            Set(value As Guid)
                _Ident = value
            End Set
        End Property

        ''' <summary>
        ''' Der FK für den Trainer der Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public Property TrainerID As Guid
            Get
                Return _TrainerID
            End Get
            Set(value As Guid)
                _TrainerID = value
                OnPropertyChanged(NameOf(TrainerID))
            End Set
        End Property

        ''' <summary>
        ''' Der Trainer der Gruppe
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public Property Trainer As Trainer
            Get
                Return _Trainer
            End Get
            Set(value As Trainer)
                _Trainer = value
                OnPropertyChanged(NameOf(Trainer))
            End Set
        End Property

        ''' <summary>
        ''' Liste der GruppenmitgliederIDs
        ''' </summary>
        ''' <returns></returns>
        Public Property MitgliederIDListe As ObservableCollection(Of Guid)
            Get
                Return _MitgliederIDListe
            End Get
            Set(value As ObservableCollection(Of Guid))
                _MitgliederIDListe = value
            End Set
        End Property

        ''' <summary>
        ''' Liste der Gruppenmitglieder
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public Property Mitgliederliste As TeilnehmerCollection
            Get
                Return _Mitgliederliste
            End Get
            Set(value As TeilnehmerCollection)
                _Mitgliederliste = value
            End Set
        End Property

        ''' <summary>
        ''' Gruppenstamm ID 
        ''' </summary>
        ''' <returns></returns>
        Public Property GruppenstammID As Guid
            Get
                Return _GruppenstammID
            End Get
            Set(value As Guid)
                _GruppenstammID = value
            End Set
        End Property

        ''' <summary>
        ''' Gruppenstammdaten
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public Property Gruppenstamm As Gruppenstamm
            Get
                Return _Gruppenstamm
            End Get
            Set(value As Gruppenstamm)
                _Gruppenstamm = value
            End Set
        End Property

#End Region

#Region "Funktionen und Methoden"

        Public Overrides Function ToString() As String
            Return Gruppenstamm.Benennung
        End Function

        Public Sub speichern() Implements IModel.speichern
            MessageBox.Show("Gruppe speichern")
        End Sub
#End Region

    End Class

End Namespace
