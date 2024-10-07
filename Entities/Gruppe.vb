Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports PropertyChanged

Namespace Entities


    ''' <summary>
    ''' Gruppe mit Angabe seiner Leistungsstufe
    ''' </summary>
    <DefaultProperty("GruppenName")>
    Public Class Gruppe
        Inherits BaseModel

#Region "Felder"
        Private _GruppenID = Guid.NewGuid()
        Private _Mitgliederliste = New TeilnehmerCollection
#End Region

#Region "Konstruktor"

        Public Sub New()
        End Sub

        ''' <summary>
        ''' Erstellt eine Gruppe unter Angabe des Namens für die Information
        ''' </summary>
        ''' <param name="Ausgabename"></param>
        Public Sub New(Ausgabename As String)
            _AusgabeTeilnehmerinfo = Ausgabename
        End Sub

        ''' <summary>
        ''' Erstellt eine Gruppe unter Angabe des Namens für die Information und einer Sortierungszahl
        ''' </summary>
        ''' <param name="Ausgabename"></param>
        ''' <param name="Sortierung"></param>
        Public Sub New(Ausgabename As String, Sortierung As Integer)
            _AusgabeTeilnehmerinfo = Ausgabename
            _Sortierung = Sortierung
        End Sub

        ''' <summary>
        ''' Erstellt eine Gruppe unter Angabe des Namens für die Information und der Benennung
        ''' </summary>
        ''' <param name="Benennung"></param>
        ''' <param name="Ausgabename"></param>
        Public Sub New(Ausgabename As String, Benennung As String)
            _Benennung = Benennung
            _AusgabeTeilnehmerinfo = Ausgabename
        End Sub

        ''' <summary>
        ''' Erstellt eine Gruppe unter Angabe des Namens für die Information, der Benennung und einer Sortierungszahl
        ''' </summary>
        ''' <param name="Ausgabename"></param>
        ''' <param name="Benennung"></param>
        ''' <param name="Sortierung"></param>
        Public Sub New(Ausgabename As String, Benennung As String, Sortierung As Integer)
            _Benennung = Benennung
            _AusgabeTeilnehmerinfo = Ausgabename
            _Sortierung = Sortierung
            Mitgliederliste = New TeilnehmerCollection
        End Sub

#End Region

#Region "Eigenschaft"
        ''' <summary>
        ''' Eindeutige Gruppenkennung
        ''' </summary>
        ''' <returns></returns>
        Public Property GruppenID As Guid
            Get
                Return _GruppenID
            End Get
            Set(value As Guid)
                _GruppenID = value
            End Set
        End Property

        ''' <summary>
        ''' Die interne Benennung der Gruppe
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Gruppenbennung ist ein Pflichtfeld")>
        Public Property Benennung As String

        ''' <summary>
        ''' Der Ausgabename für die Teilnehmerinformation
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Ausgabename ist ein Pflichtfeld")>
        Public Property AusgabeTeilnehmerinfo As String

        ''' <summary>
        ''' Der Ausgabename für die Trainerinformation
        ''' TeilnehmerinfoName und die Benennung
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AusgabeTrainerinfo As String = $"{AusgabeTeilnehmerinfo} {Benennung}"


        ''' <summary>
        ''' Sortierungszahl für die Ausgabeinformationen 
        ''' </summary>
        ''' <returns></returns>
        Public Property Sortierung As Integer?

        ''' <summary>
        ''' Die Leistungsstufe der Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public Property Leistungsstufe As Leistungsstufe

        ''' <summary>
        ''' Der Trainer der Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public Property Trainer As Trainer

        ''' <summary>
        ''' Liste der Gruppenmitglieder
        ''' </summary>
        ''' <returns></returns>
        Public Property Mitgliederliste As TeilnehmerCollection
            Get
                Return _Mitgliederliste
            End Get
            Set(value As TeilnehmerCollection)
                _Mitgliederliste = value
            End Set
        End Property

#End Region

#Region "Funktionen und Methoden"

        Public Overrides Function ToString() As String
            Return AusgabeTeilnehmerinfo
        End Function
#End Region

    End Class

End Namespace
