Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Xml.Serialization
Imports Groupies.Entities
Imports Microsoft.Win32

Namespace ViewModels

    ''' <summary>
    ''' ViewModel für die Hauptansicht der Anwendung.
    ''' Es verwaltet die Einteilungen, Gruppen, Trainer und Teilnehmer.
    ''' </summary>
    Public Class MainViewModel
        Inherits BaseModel

#Region "Konstruktor"

        Public Sub New()

        End Sub
#End Region


#Region "Properties"

        Public Property WindowTitleIcon As String = "pack://application:,,,/Images/icons8-ski-resort-48.png"

        Public Property WindowTitleText As String = "Groupies - Ski Club Management - Neues MainWindow"

        Public Property Einteilungsliste As EinteilungCollection


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

        Public Property GruppenloseTrainer As TrainerCollection

        Public Property GruppenloseTeilnehmer As TeilnehmerCollection

#End Region

#Region "Command Properties"
        Public Property WindowLoadedCommand As ICommand
        Public Property WindowClosedCommand As ICommand
        Public Property WindowClosingCommand As ICommand
        Public Property ApplicationNewCommand As ICommand
        Public Property ApplicationOpenCommand As ICommand
        Public Property ApplicationSaveCommand As ICommand
        Public Property ApplicationSaveAsCommand As ICommand
        Public Property ApplicationPrintCommand As ICommand
        Public Property ApplicationCloseCommand As ICommand

        Public Property EinteilungErstellenCommand As ICommand
        Public Property EinteilungsuebersichtAnzeigenCommand As ICommand

        Public Property GruppeErstellenCommand As ICommand
        Public Property GruppeLoeschenCommand As ICommand
        Public Property GruppenuebersichtAnzeigenCommand As ICommand

        Public Property LeistungsstufeErstellenCommand As ICommand
        Public Property LeistungsstufenuebersichtAnzeigenCommand As ICommand

        Public Property FaehigkeitErstellenCommand As ICommand
        Public Property FaehigkeitenuebersichtAnzeigenCommand As ICommand

        Public Property TeilnehmerErstellenCommand As ICommand
        Public Property TeilnehmerEinteilenCommand As ICommand
        Public Property TeilnehmerBearbeitenCommand As ICommand
        Public Property TeilnehmerLoeschenCommand As ICommand
        Public Property TeilnehmeruebersichtAnzeigenCommand As ICommand


        Public Property TrainerErstellenCommand As ICommand
        Public Property TrainerEinteilenCommand As ICommand
        Public Property TrainerLoeschenCommand As ICommand
        Public Property TrainerBearbeitenCommand As ICommand
        Public Property TraineruebersichtAnzeigenCommand As ICommand




#End Region

#Region "Functions"

        Public Function KopiereListeMitNeuenObjekten(Of T)(originalList As List(Of T), copyConstructor As Func(Of T, T)) As List(Of T)
            Dim copiedList As New List(Of T)
            For Each item In originalList
                copiedList.Add(copyConstructor(item))
            Next
            Return copiedList
        End Function


        '''' <summary>
        '''' Lädt eine XML-Datei und erstellt daraus einen Club
        '''' </summary>
        'Public Sub DateiLaden()
        '    Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}

        '    If dlg.ShowDialog = True Then
        '        Club = Controller.SkiDatenLaden.SkiDateiLesen(dlg.FileName)
        '        Dim Einteilungen = Controller.SkiDatenLaden.EinteilungenLesen(Club)

        '        If Club IsNot Nothing Then
        '            Einteilungen.ToList.ForEach(Sub(E) Club.AddEinteilung(E))
        '        End If
        '    End If
        'End Sub

        '''' <summary>
        '''' Lädt aus einer XML-Datei eine Einteilung 
        '''' </summary>
        'Public Sub EinteilungAusDateiLaden()

        '    Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}
        '    If dlg.ShowDialog = True Then
        '        Dim lokalerClub = Controller.SkiDatenLaden.SkiDateiLesen(dlg.FileName)

        '        If lokalerClub IsNot Nothing Then
        '            Dim Einteilungsname = BestimmeEinteilungsbenennung()
        '            Trainerliste = lokalerClub.AlleTrainer
        '            If Club.Einteilungsliste IsNot Nothing AndAlso Club.Einteilungsliste.Count > 0 Then
        '            Else
        '                Einteilungsliste = New EinteilungCollection From {New Einteilung With {.Benennung = Einteilungsname, .Gruppenliste = Club.Gruppenliste}}
        '            End If
        '        End If
        '    End If
        'End Sub

        'Public Function BestimmeEinteilungsbenennung(Einteilungsliste As EinteilungCollection) As String
        '    Dim Einteilungsname = String.Empty
        '    Dim Zaehler As Integer
        '    If Club.Einteilungsliste.Count > 0 Then
        '        Einteilungsname = Club.Einteilungsliste.OrderByDescending(Function(e) e.Sortierung).First.Benennung
        '        Zaehler = Val(Einteilungsname.Last) + 1
        '        Einteilungsname &= Zaehler
        '    Else
        '        Einteilungsname = "Tag1"
        '    End If
        '    Return Einteilungsname
        'End Function

        'Public Function BestimmeEinteilungsbenennung() As String
        '    Dim Einteilungsname = String.Empty
        '    Dim Zaehler As Integer
        '    If Einteilungsliste.Count > 0 Then
        '        Einteilungsname = Club.Einteilungsliste.OrderByDescending(Function(e) e.Sortierung).First.Benennung
        '        Zaehler = Val(Einteilungsname.Last) + 1
        '        Einteilungsname &= Zaehler
        '    Else
        '        Einteilungsname = "Tag1"
        '    End If
        '    Return Einteilungsname
        'End Function

#End Region

    End Class
End Namespace
