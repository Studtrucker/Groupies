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
#End Region

#Region "Konstruktor"

        Public Sub New()
            _Mitglieder = New TeilnehmerCollection
        End Sub

        ''' <summary>
        ''' Erstellt eine Gruppe unter Angabe des Namens für die Information
        ''' </summary>
        ''' <param name="Ausgabename"></param>
        Public Sub New(Ausgabename As String)
            _Ausgabename = Ausgabename
        End Sub

        ''' <summary>
        ''' Erstellt eine Gruppe unter Angabe des Namens für die Information und einer Sortierungszahl
        ''' </summary>
        ''' <param name="Ausgabename"></param>
        ''' <param name="Sortierung"></param>
        Public Sub New(Ausgabename As String, Sortierung As Integer)
            _Ausgabename = Ausgabename
            _Sortierung = Sortierung
        End Sub

        ''' <summary>
        ''' Erstellt eine Gruppe unter Angabe des Namens für die Information und der Benennung
        ''' </summary>
        ''' <param name="Benennung"></param>
        ''' <param name="Ausgabename"></param>
        Public Sub New(Ausgabename As String, Benennung As String)
            _Benennung = Benennung
            _Ausgabename = Ausgabename
        End Sub

        ''' <summary>
        ''' Erstellt eine Gruppe unter Angabe des Namens für die Information, der Benennung und einer Sortierungszahl
        ''' </summary>
        ''' <param name="Ausgabename"></param>
        ''' <param name="Benennung"></param>
        ''' <param name="Sortierung"></param>
        Public Sub New(Ausgabename As String, Benennung As String, Sortierung As Integer)
            _Benennung = Benennung
            _Ausgabename = Ausgabename
            _Sortierung = Sortierung
            Mitglieder = New TeilnehmerCollection
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
        ''' Der Ausgabename für die Information
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Ausgabename ist ein Pflichtfeld")>
        Public Property Ausgabename As String

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
        Public Property Mitglieder As TeilnehmerCollection

#End Region

#Region "Funktionen und Methoden"

        ''' <summary>
        ''' Fügt der Gruppe einen Teilnehmer hinzu
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        Public Sub TeilnehmerHinzufuegen(Teilnehmer As Teilnehmer)
            _Mitglieder.Add(Teilnehmer)
        End Sub

        ''' <summary>
        ''' Entfernt einen Teilnehmer aus der Gruppe
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        Public Sub TeilnehmerEntfernen(Teilnehmer As Teilnehmer)
            _Mitglieder.Remove(Teilnehmer)
        End Sub

#End Region

#Region "Veraltert"


#End Region

    End Class

End Namespace
