Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports Groupies.Entities

Public Class MainViewModel
    Implements INotifyPropertyChanged

    Private Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Property ClubName As String

    Public Property Einteilungen As Entities.EinteilungCollection

    Private Shared _SelectedEinteilung As Einteilung

    Public Property SelectedEinteilung As Einteilung
        Get
            Return _SelectedEinteilung
        End Get
        Set(value As Einteilung)
            _SelectedEinteilung = value
            OnPropertyChanged()
            SelectedGruppe = Nothing
        End Set
    End Property

    Private Shared _SelectedGruppe As Gruppe
    Public Property SelectedGruppe As Gruppe
        Get
            Return _SelectedGruppe
        End Get
        Set(value As Gruppe)
            _SelectedGruppe = value
            OnPropertyChanged()
        End Set
    End Property

    Protected Overridable Sub OnPropertyChanged(<CallerMemberName> Optional propertyName As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

End Class
