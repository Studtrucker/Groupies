Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

    Public Class TeilnehmerCollection
        Inherits ObservableCollection(Of Teilnehmer)
        Implements IEnumerable


        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Teilnehmerliste As IEnumerable(Of Teilnehmer))
            MyBase.New
            Teilnehmerliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

        Public ReadOnly Property ParticipantCollectionOrdered As TeilnehmerCollection
            Get
                Dim Ordered = New TeilnehmerCollection
                Me.OrderBy(Of String)(Function(x) x.VorUndNachname).ToList.ForEach(Sub(x) Ordered.Add(x))
                Return Ordered
            End Get
        End Property




        ''' <summary>
        ''' Die Teilnehmerliste geordnet nach Nachname, Vorname
        ''' </summary>
        ''' <returns></returns>
        Public Property GeordnetNachnameVorname = From t In Me
                                                  Order By
                                                      t.Nachname,
                                                      t.Vorname
        ''' <summary>
        ''' Die Teilnehmerliste geordnet nach 
        ''' der absteigenden Sortierkennzahl 
        ''' aus der Teilnehmer Leistungsstufe, 
        ''' dann aufsteigend Nachname, Vorname 
        ''' </summary>
        ''' <returns></returns>
        Public Property GeordnetLeistungsstufeNachnameVorname = From t In Me
                                                                Order By
                                                                    t.Leistungsstand.Sortierung Descending,
                                                                    t.Nachname,
                                                                    t.Vorname

    End Class
End Namespace
