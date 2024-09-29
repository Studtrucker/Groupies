Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports CDS = Groupies.Services.CurrentDataService


Namespace Entities

    <DefaultBindingProperty("ParticipantFirstname")>
    <DefaultProperty("ParticipantFullName")>
    Public Class Teilnehmer
        Inherits BaseModel

#Region "Events"
        Public Event ChangeGroup(Participant As Teilnehmer)
#End Region

#Region "Konstruktor"

        ''' <summary>
        ''' Erstellt einen neuen Teilnehmer
        ''' </summary>
        <Obsolete>
        Public Sub New()
            _TeilnehmerID = Guid.NewGuid()
        End Sub

        ''' <summary>
        ''' Erstellt einen neuen Teilnehmer mit Vorname und Nachname unter Angabe seines Leistungsstandes
        ''' </summary>
        ''' <param name="Vorname"></param>
        ''' <param name="Nachname"></param>
        ''' <param name="Leistungsstufe"></param>
        Public Sub New(Vorname As String, Nachname As String, Leistungsstufe As Leistungsstufe)
            _TeilnehmerID = Guid.NewGuid()
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
            _TeilnehmerID = Guid.NewGuid()
            _Vorname = Vorname
            _Nachname = Nachname
            _Leistungsstand = New Leistungsstufe()
        End Sub

#End Region

#Region "Eigenschaften"
        ''' <summary>
        ''' Eindeutige Kennzeichnung des Teilnehmers 
        ''' </summary>
        ''' <returns></returns>
        Public Property TeilnehmerID As Guid

        ''' <summary>
        ''' Der Nachname des Teilnehmers
        ''' </summary>
        ''' <returns></returns>
        Public Property Nachname As String

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
        ''' Setzt und liest den Leistungsstand des Teilnehmers
        ''' </summary>
        ''' <returns></returns>
        Public Property Leistungsstand As Leistungsstufe

#End Region

#Region "Funktionen und Methoden"
        ''' <summary>
        ''' Gibt den Vor- und Nachnamen für die Teilnehmerinformation zurück
        ''' </summary>
        ''' <returns></returns>
        Public Function AusgabeAnTeilnehmerinfo() As String
            Return VorUndNachname
        End Function

        ''' <summary>
        ''' Gibt den Vor-, Nachnamen und Leistungsstand für die Trainerinformation zurück
        ''' </summary>
        ''' <returns></returns>
        Public Function AusgabeAnTrainerinfo() As String
            If Leistungsstand Is Nothing Then
                Return $"{VorUndNachname}, Leistungsstand unbekannt"
            Else
                Return $"{VorUndNachname}, {Leistungsstand.Benennung}"
            End If
        End Function

        Public Overrides Function ToString() As String
            Return VorUndNachname
        End Function
#End Region

#Region "Veraltert"
        ''' <summary>
        ''' Gruppenmitglied in der Gruppe 
        ''' mit dem Gruppenkennzeichen
        ''' </summary>
        ''' <returns></returns>
        Public Property MemberOfGroup As Guid

        ''' <summary>
        ''' Gruppenmitglied in der Gruppe 
        ''' mit dem Gruppennamen 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property MemberOfGroup_Naming() As String
            Get
                Return CDS.Skiclub.Grouplist.Where(Function(x) x.GroupID.Equals(MemberOfGroup)).DefaultIfEmpty(New Group With {.GroupNaming = String.Empty}).Single.GroupNaming
            End Get
        End Property

        ''' <summary>
        ''' Wird Gruppenmitglied von der Gruppe
        ''' mit dem Gruppenkennzeichen
        ''' </summary>
        ''' <param name="GroupID"></param>
        Public Sub SetAsGroupMember(GroupID As Guid)
            MemberOfGroup = GroupID
            _IsGroupMember = True
        End Sub

        ''' <summary>
        ''' Wird als Gruppenmitgleid gekennzeichnet
        ''' </summary>
        Public Sub SetAsGroupMember()
            _IsGroupMember = True
        End Sub

        ''' <summary>
        ''' Wird als Gruppenmitglied entfernt
        ''' </summary>
        Public Sub RemoveFromGroup()
            MemberOfGroup = Nothing
            _IsGroupMember = False
        End Sub

        ''' <summary>
        ''' Setzt und liest die Gruppenmitgliedschaft
        ''' </summary>
        Private _IsGroupMember As Boolean
        Public Property IsGroupMember As Boolean
            Get
                Return _IsGroupMember
            End Get
            Set(value As Boolean)
                _IsGroupMember = value
            End Set
        End Property



        ''' <summary>
        ''' Liest die umgekehrte Gruppenmitgliedschaft
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsNotInGroup As Boolean
            Get
                Return Not _IsGroupMember
            End Get
        End Property

#End Region

    End Class
End Namespace
