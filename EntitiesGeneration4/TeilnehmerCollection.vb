Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Text

Namespace Entities.Generation4

    Public Class TeilnehmerCollection
        Inherits ObservableCollection(Of Teilnehmer)
        Implements IEnumerable(Of Teilnehmer)

#Region "Konstruktor"
        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Teilnehmerliste As IEnumerable(Of Teilnehmer))
            MyBase.New
            Teilnehmerliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

#End Region

#Region "Porperties IEnumerable(String) Rückgabe"

        ''' <summary>
        ''' Die Teilnehmer mit VorUndNachname
        '''  Geordnet:
        '''  Nachname aufsteigend
        '''  Vorname aufsteigend
        ''' </summary>
        ''' <returns></returns>
        Public Property TeilnehmernameOrderByNachname As IEnumerable(Of String) =
            OrderBy(Function(Tn) Tn.Nachname) _
            .ThenBy(Function(Tn) Tn.Vorname) _
            .Select(Function(Tn) Tn.VorUndNachname)

        Public ReadOnly Property TeilnehmerOrderByNachname As TeilnehmerCollection
            Get
                Return New TeilnehmerCollection(Me.OrderBy(Of String)(Function(x) x.Vorname).OrderBy(Of String)(Function(x) x.Nachname))
            End Get
        End Property


#End Region

#Region "Porperties IEnumerable(Teilnehmer) Rückgabe"


#End Region

#Region "Funktionen und Methoden"

#End Region

#Region "Beispiel Gruppierung"

#End Region

    End Class
End Namespace
