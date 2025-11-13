Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Windows.Input
Imports Groupies.Entities.Generation4
Imports Groupies.ViewModels

Namespace ViewModels

    Public Enum SuchResultTargetType
        Gruppe
        NichtZugewiesen
    End Enum

    Public Class NavigationRequest
        Public Property Teilnehmer As Teilnehmer
        Public Property ZielGruppe As Gruppe
        Public Property ZielEinteilung As Einteilung
        Public Property TargetType As SuchResultTargetType
    End Class

    Public Class TeilnehmerSuchErgebnisViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Public ReadOnly Property Items As ObservableCollection(Of TeilnehmerSuchErgebnisItem)

        Public ReadOnly Property OpenTargetCommand As ICommand

        ' Liefert ein NavigationRequest; Aufrufer (z. B. MainViewModel) muss damit navigieren.
        Public Shared Event OpenTargetRequested As EventHandler(Of NavigationRequest)

        ' Neues Instanz-Ereignis damit die View sich selbst schließen kann.
        Public Event RequestClose As EventHandler

        Public Sub New(suchergebnis As IEnumerable(Of TeilnehmerSuchErgebnisItem))
            Me.Items = New ObservableCollection(Of TeilnehmerSuchErgebnisItem)(suchergebnis)
            OpenTargetCommand = New RelayCommand(Of Object)(AddressOf OnOpenTarget)
        End Sub

        Private Sub OnOpenTarget(parameter As Object)
            Dim item = TryCast(parameter, TeilnehmerSuchErgebnisItem)
            If item Is Nothing Then
                Dim t = TryCast(parameter, Teilnehmer)
                If t IsNot Nothing Then
                    item = Items.FirstOrDefault(Function(i) i.Teilnehmer Is t OrElse i.Teilnehmer?.Ident = t.Ident)
                End If
            End If

            If item Is Nothing Then
                Return
            End If

            Dim req As New NavigationRequest With {
                .Teilnehmer = item.Teilnehmer,
                .ZielGruppe = item.ZielGruppe,
                .ZielEinteilung = item.ZielEinteilung,
                .TargetType = item.TargetType
            }

            ' 1) Informiere Subscriber (z. B. MainViewModel) über Navigation
            RaiseEvent OpenTargetRequested(Me, req)

            ' 2) Signalisiere der View, dass sie sich schließen soll
            RaiseEvent RequestClose(Me, EventArgs.Empty)
        End Sub

        Protected Sub OnPropertyChanged(name As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub

    End Class
End Namespace