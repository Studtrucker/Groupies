Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports Groupies.Controller.AppController


Namespace Entities


    ''' <summary>
    ''' Teilnehmer mit Angabe seines Leistungsstandes mit Hilfe einer Leistungsstufe
    ''' </summary>
    <DefaultBindingProperty("Vorname")>
    <DefaultProperty("VorUndNachname")>
    Public Class EwigerTeilnehmer
        Inherits Teilnehmer


#Region "Fields"

#End Region

#Region "Events"
#End Region

#Region "Konstruktor"

        Public Sub New()

        End Sub

        Public Sub New(Vorname As String, Nachname As String)
            Me.Vorname = Vorname
            Me.Nachname = Nachname
        End Sub

        Public Sub New(Vorname As String, Nachname As String, Datum As Date)
            Me.Vorname = Vorname
            Me.Nachname = Nachname
            Me.ZuletztTeilgenommen = Datum
        End Sub

#End Region

#Region "Eigenschaften"

        Public Property ZuletztTeilgenommen As Date

#End Region

#Region "Funktionen und Methoden"

        Public Overrides Function ToString() As String
            Return $"{VorUndNachname} war zuletzt in {ZuletztTeilgenommen.Month}.{ZuletztTeilgenommen.Year} dabei"
        End Function

#End Region

    End Class
End Namespace
