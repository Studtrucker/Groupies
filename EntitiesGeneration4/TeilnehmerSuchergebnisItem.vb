Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Windows.Input
Imports Groupies.Entities.Generation4
Imports Groupies.ViewModels

Namespace Entities.Generation4
    Public Class TeilnehmerSuchErgebnisItem
        Public Sub New(teilnehmer As Teilnehmer, fundortText As String, zielGruppe As Gruppe, zielEinteilung As Einteilung, targetType As SuchResultTargetType)
            Me.Teilnehmer = teilnehmer
            Me.FundortText = fundortText
            Me.ZielGruppe = zielGruppe
            Me.ZielEinteilung = zielEinteilung
            Me.TargetType = targetType
        End Sub

        Public Property Teilnehmer As Teilnehmer
        Public Property FundortText As String
        Public Property ZielGruppe As Gruppe
        Public Property ZielEinteilung As Einteilung
        Public Property TargetType As SuchResultTargetType
    End Class
End Namespace
