Imports System.ComponentModel

Namespace Entities
    Public Class Skikursgruppe
        Implements INotifyPropertyChanged

        Private skigruppeIDFeld As Guid
        Private _gruppenname As String
        Private _angezeigterGruppenname As String
        Private _koennenstufe As Entities.Koennenstufe
        Private _skilehrer As Entities.Skilehrer

        Public Sub New()
            skigruppeIDFeld = Guid.NewGuid()
            _koennenstufe = New Entities.Koennenstufe
            _skilehrer = New Entities.Skilehrer
        End Sub

        Public Property SkigruppeID As Guid
            Get
                Return skigruppeIDFeld
            End Get
            Set(value As Guid)
                skigruppeIDFeld = value
            End Set
        End Property
        Public Property Gruppenname As String
            Get
                Return _gruppenname
            End Get
            Set(value As String)
                _gruppenname = value
                Changed("Gruppenname")
            End Set
        End Property

        Public Property AngezeigterGruppenname As String
            Get
                Return _angezeigterGruppenname
            End Get
            Set(value As String)
                _angezeigterGruppenname = value
                Changed("AngezeigterGruppenname")
            End Set
        End Property

        Public Property Koennenstufe As Entities.Koennenstufe
            Get
                Return _koennenstufe
            End Get
            Set(value As Entities.Koennenstufe)
                _koennenstufe = value
                Changed("Koennenstufe")
            End Set
        End Property

        Public Property Skilehrer As Entities.Skilehrer
            Get
                Return _skilehrer
            End Get
            Set(value As Entities.Skilehrer)
                _skilehrer = value
                Changed("Skilehrer")
            End Set
        End Property
        Private Sub Changed(propertyName As String)
            Dim handler = PropertyChangedEvent
            If handler IsNot Nothing Then
                handler(Me, New PropertyChangedEventArgs(propertyName))
            End If
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class

End Namespace
