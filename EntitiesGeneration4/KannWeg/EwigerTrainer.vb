Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports Groupies.Controller.AppController


Namespace Entities


    ''' <summary>
    ''' Teilnehmer mit Angabe seines Leistungsstandes mit Hilfe einer Leistungsstufe
    ''' </summary>
    <DefaultBindingProperty("Vorname")>
    <DefaultProperty("VorUndNachname")>
    Public Class EwigerTrainer
        Inherits Trainer


#Region "Fields"

#End Region

#Region "Events"
#End Region

#Region "Konstruktor"

        Public Sub New()

        End Sub

        Public Sub New(Trainer As Trainer, Datum As Date)
            Me.Vorname = Trainer.Vorname
            Me.Nachname = Trainer.Nachname
            Me.Archivierungsdatum = Datum
        End Sub

#End Region

#Region "Eigenschaften"

        Public Property Archivierungsdatum As Date

#End Region

#Region "Funktionen und Methoden"

        Public Overrides Function ToString() As String
            Return $"{VorUndNachname} war zuletzt in {Archivierungsdatum.Month}.{Archivierungsdatum.Year} dabei"
        End Function

#End Region

    End Class
End Namespace
