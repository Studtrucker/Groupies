Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports Groupies.Controller.AppController


Namespace Entities


    ''' <summary>
    ''' Teilnehmer mit Angabe seines Leistungsstandes mit Hilfe einer Leistungsstufe
    ''' </summary>
    <DefaultBindingProperty("Vorname")>
    <DefaultProperty("VorUndNachname")>
    Public Class Teilnehmer
        Inherits BaseModel


#Region "Fields"
        Private _Leistungsstand = New Leistungsstufe("Level unbekannt", -1)
        Private _TeilnehmerID = Guid.NewGuid()

#End Region

#Region "Events"
        Public Event ChangeGroup(Participant As Teilnehmer)
#End Region

#Region "Konstruktor"

        ''' <summary>
        ''' Erstellt einen neuen Teilnehmer
        ''' </summary>
        Public Sub New()
            Geburtsdatum = DateAndTime.Now.ToLongDateString
        End Sub

        ''' <summary>
        ''' Erstellt einen neuen Teilnehmer mit Vorname und Nachname unter Angabe seines Leistungsstandes
        ''' </summary>
        ''' <param name="Vorname"></param>
        ''' <param name="Nachname"></param>
        ''' <param name="Leistungsstufe"></param>
        Public Sub New(Vorname As String, Nachname As String, Leistungsstufe As Leistungsstufe)
            _Vorname = Vorname
            _Nachname = Nachname
            _Leistungsstand = Leistungsstufe
        End Sub

        ''' <summary>
        ''' Erstellt einen neuen Teilnehmer mit Vorname und Nachname
        ''' </summary>
        ''' <param name="Vorname"></param>
        ''' <param name="Nachname"></param>
        Public Sub New(Vorname As String, Nachname As String)
            _Vorname = Vorname
            _Nachname = Nachname
        End Sub

        ''' <summary>
        ''' Erstellt einen neuen Teilnehmer mit Vorname
        ''' </summary>
        ''' <param name="Vorname"></param>
        Public Sub New(Vorname As String)
            _Vorname = Vorname
        End Sub

#End Region

#Region "Eigenschaften"
        ''' <summary>
        ''' Eindeutige Kennzeichnung des Teilnehmers 
        ''' </summary>
        ''' <returns></returns>
        Public Property TeilnehmerID As Guid
            Get
                Return _TeilnehmerID
            End Get
            Set(value As Guid)
                _TeilnehmerID = value
            End Set
        End Property

        ''' <summary>
        ''' Der Nachname des Teilnehmers
        ''' </summary>
        ''' <returns></returns>
        Public Property Nachname As String

        ''' <summary>
        ''' Das Geburtsdatum des Teilnehmers
        ''' </summary>
        ''' <returns></returns>
        Public Property Geburtsdatum As Date

        ''' <summary>
        ''' Berechnet das Alter und  
        ''' gibt es auf dem Trainerausdruck 
        ''' und in der Gruppeneinteilung aus
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Alter As Long
            Get
                Dim nMonate As Integer
                Dim nJahre As Integer
                If Geburtsdatum = "0001-01-01" Then
                    nJahre = 0
                Else
                    nJahre = Math.Floor(DateDiff(DateInterval.Month, Geburtsdatum, DateTime.Now) / 12)
                    nMonate = DatePart(DateInterval.Month, Geburtsdatum)
                    If nMonate = DatePart(DateInterval.Month, DateTime.Now) Then
                        If DatePart(DateInterval.Day, Geburtsdatum) > DatePart(DateInterval.Day, DateTime.Now) Then
                            nJahre += -1
                        End If
                    End If
                End If
                Return nJahre
            End Get
        End Property

        ''' <summary>
        ''' Der Vorname des Teilnehmers
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Vorname ist eine Pflichtangabe")>
        Public Property Vorname As String

        ''' <summary>
        ''' Der Vor- und Nachname
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property VorUndNachname As String
            Get
                If _Vorname Is Nothing Then
                    Return _Nachname
                ElseIf _Nachname Is Nothing Then
                    Return _Vorname
                Else
                    Return String.Format("{0} {1}", _Vorname, _Nachname)
                End If
            End Get
        End Property


        ''' <summary>
        ''' Gibt die Telefonnummer 
        ''' auf dem Trainerausdruck aus
        ''' </summary>
        ''' <returns></returns>
        Public Property Telefonnummer As String

        ''' <summary>
        ''' Setzt und liest den Leistungsstand des Teilnehmers
        ''' </summary>
        ''' <returns></returns>
        Public Property Leistungsstand As Leistungsstufe
            Get
                Return _Leistungsstand
            End Get
            Set(value As Leistungsstufe)
                _Leistungsstand = value
            End Set
        End Property

        ''' <summary>
        ''' Gibt den Vor- und Nachnamen für die Teilnehmerinformation zurück
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AusgabeTeilnehmerinfo As String
            Get
                Return VorUndNachname
            End Get
        End Property


        ''' <summary>
        ''' Gibt den Vor-, Nachnamen und Leistungsstand für die Trainerinformation zurück
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AusgabeTrainerinfo As String
            Get
                Return GetAusgabeTrainerInfo()
            End Get
        End Property

        Public Property Archivieren As Boolean

#End Region

#Region "Funktionen und Methoden"

        Private Function GetAusgabeTrainerInfo() As String
            If Leistungsstand Is Nothing Then
                Return $"{VorUndNachname}, {Telefonnummer}, Level unbekannt"
            Else
                Return $"{VorUndNachname}, {Telefonnummer}, {Leistungsstand.Benennung}"
            End If
        End Function

        Public Overrides Function ToString() As String
            Return VorUndNachname
        End Function

#End Region

    End Class
End Namespace
