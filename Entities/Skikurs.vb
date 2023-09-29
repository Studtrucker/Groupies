Imports System.ComponentModel

Namespace Entities
    Public Class Skikurs
        Implements INotifyPropertyChanged

        Private skikursIDFeld As Guid
        Private _kurs As String
        Private _kursprintName As String
        Private _kursleiterID As Guid
        Private _kurslevelID As Guid
        Private _mitgliederliste As Entities.TeilnehmerCollection

        Public Sub New()
            skikursIDFeld = Guid.NewGuid()
            _mitgliederliste = New TeilnehmerCollection
        End Sub

        Public Property SkikursID As Guid
            Get
                Return skikursIDFeld
            End Get
            Set(value As Guid)
                skikursIDFeld = value
            End Set
        End Property

        Public Property Kurs As String
            Get
                Return _kurs
            End Get
            Set(value As String)
                _kurs = value
                Changed("Kurs")
            End Set
        End Property

        Public Property KursPrintName As String
            Get
                Return _kursprintName
            End Get
            Set(value As String)
                _kursprintName = value
                Changed("KursPrintName")
            End Set
        End Property

        Public Property KurslevelID As Guid
            Get
                Return _kurslevelID
            End Get
            Set(value As Guid)
                _kurslevelID = value
                Changed("KurslevelID")
            End Set
        End Property

        Public Property KursleiterID As Guid
            Get
                Return _kursleiterID
            End Get
            Set(value As Guid)
                _kursleiterID = value
                Changed("KursleiterID")
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

        Public Sub RemoveMitglieder(Teilnehmer As Teilnehmer)
            _mitgliederliste.Remove(Teilnehmer)
            Changed("Mitgliederliste")
        End Sub

        Public Sub RemoveMitglieder(Teilnehmerliste As TeilnehmerCollection)
            Teilnehmerliste.ToList.ForEach(Sub(x) _mitgliederliste.Remove(x))
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
