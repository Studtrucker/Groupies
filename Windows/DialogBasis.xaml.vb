Imports System.IO
Imports System.Text
Imports Groupies.Entities
Imports Groupies.Interfaces

Public Class DialogBasis(Of T)
    Inherits Window
    Implements Interfaces.IGenericWindowMitModus(Of T)

#Region "Modus-Handler"

    Public Property Modus As IModus Implements IGenericWindowMitModus(Of T).Modus

    Private _Objekt As T


    Public ReadOnly Property Objekt() As T
        Get
            Return _Objekt
        End Get
    End Property

    Public Sub ModusEinstellen() Implements IGenericWindowMitModus(Of T).ModusEinstellen

        Me.Title = Modus.Titel
        ' Me.Icon = Modus.Icon
    End Sub

    Public Sub Bearbeiten(Original As T) Implements IGenericWindowMitModus(Of T).Bearbeiten
        Throw New NotImplementedException()
    End Sub

#End Region
    Private Sub HandleButtonOKExecuted(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub HandleCancelButtonExecuted(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
