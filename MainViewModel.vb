Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Xml.Serialization
Imports Groupies.Entities
Imports Microsoft.Win32

Public Class MainViewModel
    Inherits BaseModel


#Region "Properties"

    Public Property Club As Club
    Public Property Einteilungsliste As EinteilungCollection
    Public Property Trainerliste As TrainerCollection
    Public Property Teilnehmerliste As TeilnehmerCollection

    Private _SelectedEinteilung As Einteilung

    Public Property SelectedEinteilung As Einteilung
        Get
            Return _SelectedEinteilung
        End Get
        Set(value As Einteilung)
            _SelectedEinteilung = value
            SelectedGruppe = Nothing
        End Set
    End Property

    Private _SelectedGruppe As Gruppe
    Public Property SelectedGruppe As Gruppe
        Get
            Return _SelectedGruppe
        End Get
        Set(value As Gruppe)
            _SelectedGruppe = value
        End Set
    End Property

#End Region

#Region "Functions"


    ''' <summary>
    ''' Lädt eine XML-Datei und erstellt daraus einen Club
    ''' </summary>
    Public Sub DateiLaden()
        Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}

        If dlg.ShowDialog = True Then
            Club = Controller.SkiDatenLaden.SkiDateiLesen(dlg.FileName)

            If Club IsNot Nothing Then
                Dim Einteilungsname = BestimmeEinteilungsbenennung(Club.Einteilungsliste)
                Trainerliste = Club.AlleTrainer
                If Club.Einteilungsliste IsNot Nothing AndAlso Club.Einteilungsliste.Count > 0 Then
                Else
                    Einteilungsliste = New EinteilungCollection From {New Einteilung With {.Benennung = Einteilungsname, .Gruppenliste = Club.Gruppenliste}}
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Lädt aus einer XML-Datei eine Einteilung 
    ''' </summary>
    Public Sub EinteilungAusDateiLaden()

        Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}
        If dlg.ShowDialog = True Then
            Dim lokalerClub = Controller.SkiDatenLaden.SkiDateiLesen(dlg.FileName)

            If lokalerClub IsNot Nothing Then
                Dim Einteilungsname = BestimmeEinteilungsbenennung()
                Trainerliste = lokalerClub.AlleTrainer
                If Club.Einteilungsliste IsNot Nothing AndAlso Club.Einteilungsliste.Count > 0 Then
                Else
                    Einteilungsliste = New EinteilungCollection From {New Einteilung With {.Benennung = Einteilungsname, .Gruppenliste = Club.Gruppenliste}}
                End If
            End If
        End If
    End Sub

    Public Function BestimmeEinteilungsbenennung(Einteilungsliste As EinteilungCollection) As String
        Dim Einteilungsname = String.Empty
        Dim Zaehler As Integer
        If Club.Einteilungsliste.Count > 0 Then
            Einteilungsname = Club.Einteilungsliste.OrderByDescending(Function(e) e.Sortierung).First.Benennung
            Zaehler = Val(Einteilungsname.Last) + 1
            Einteilungsname &= Zaehler
        Else
            Einteilungsname = "Tag1"
        End If
        Return Einteilungsname
    End Function

    Public Function BestimmeEinteilungsbenennung() As String
        Dim Einteilungsname = String.Empty
        Dim Zaehler As Integer
        If Einteilungsliste.Count > 0 Then
            Einteilungsname = Club.Einteilungsliste.OrderByDescending(Function(e) e.Sortierung).First.Benennung
            Zaehler = Val(Einteilungsname.Last) + 1
            Einteilungsname &= Zaehler
        Else
            Einteilungsname = "Tag1"
        End If
        Return Einteilungsname
    End Function

#End Region

End Class
