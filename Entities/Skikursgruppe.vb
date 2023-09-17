Imports System.ComponentModel

Namespace Entities
    Public Class Skikursgruppe
        Implements INotifyPropertyChanged

        Private skikursgruppenIDFeld As Guid
        Private _gruppenname As String
        Private _angezeigterGruppenname As String
        Private _gruppenlevel As Entities.Level
        Private _skilehrer As Entities.Uebungsleiter
        Private _mitgliederliste As Entities.TeilnehmerCollection

        Public Sub New()
            skikursgruppenIDFeld = Guid.NewGuid()
            _mitgliederliste = New TeilnehmerCollection
        End Sub

        Public Property SkikursgruppenID As Guid
            Get
                Return skikursgruppenIDFeld
            End Get
            Set(value As Guid)
                skikursgruppenIDFeld = value
            End Set
        End Property
        Public Property Gruppenname As String
            Get
                Return _gruppenname
            End Get
            Set(value As String)
                _gruppenname = value
                Changed("Gruppenname")
            End Set
        End Property

        Public Property AngezeigterGruppenname As String
            Get
                Return _angezeigterGruppenname
            End Get
            Set(value As String)
                _angezeigterGruppenname = value
                Changed("AngezeigterGruppenname")
            End Set
        End Property

        Public Property Gruppenlevel As Entities.Level
            Get
                Return _gruppenlevel
            End Get
            Set(value As Entities.Level)
                _gruppenlevel = value
                Changed("Gruppenlevel")
            End Set
        End Property

        Public Property Skilehrer As Entities.Uebungsleiter
            Get
                Return _skilehrer
            End Get
            Set(value As Entities.Uebungsleiter)
                _skilehrer = value
                Changed("Skilehrer")
            End Set
        End Property

        Public Property Mitgliederliste As TeilnehmerCollection
            Get
                Return _mitgliederliste
            End Get
            Set(value As Entities.TeilnehmerCollection)
                _mitgliederliste = value
                Changed("Mitgliederliste")
            End Set
        End Property

        Public Sub AddMitglied(Teilnehmer As Teilnehmer)
            _mitgliederliste.Add(Teilnehmer)
            Changed("Mitgliederliste")
        End Sub

        Public Sub AddMitglieder(Teilnehmerliste As TeilnehmerCollection)
            Teilnehmerliste.ToList.ForEach(Sub(x) _mitgliederliste.Add(x))
            Changed("Mitgliederliste")
        End Sub

        Public Sub RemoveMitglieder(Teilnehmerliste As TeilnehmerCollection)
            'Todo: Skikursgruppe.Mitglieder entfernen            Mitgliederliste.Remove(Teilnehmerliste)
            Changed("Mitgliederliste")
        End Sub

        Public ReadOnly Property AnzahlMitglieder As Integer
            Get
                Return Mitgliederliste.Count
            End Get
        End Property

        Private Sub Changed(propertyName As String)
            Dim handler = PropertyChangedEvent
            If handler IsNot Nothing Then
                handler(Me, New PropertyChangedEventArgs(propertyName))
            End If
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class

End Namespace
