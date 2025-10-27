Imports System.ComponentModel
Imports Groupies.Entities.Generation4
Imports Groupies.ViewModels

Public Class TeilnehmerSuchErgebnis

    Public Sub New(Suchergebnis As BindingList(Of Teilnehmer))
        InitializeComponent()
        Me.DataContext = New TeilnehmerSuchErgebnisViewModel(Suchergebnis)
    End Sub
    Public Sub New()
        InitializeComponent()
    End Sub

End Class
