Imports System.Collections.ObjectModel
Imports System.Xml.Serialization
Imports Groupies.DataImport

Namespace Entities.Generation3


    Public Class Einteilung
        Implements IModel


#Region "Properties"

        Public Property Ident As Guid Implements IModel.Ident
        Public Property Sortierung() As Integer
        Public Property Benennung() As String
        Public Property GruppenIDListe() As ObservableCollection(Of Guid)
        Public Property Gruppenliste() As GruppeCollection
        Public Property NichtZugewieseneTeilnehmerIDListe() As ObservableCollection(Of Guid)
        Public Property NichtZugewieseneTeilnehmerListe() As TeilnehmerCollection
        Public Property VerfuegbareTrainerIDListe() As ObservableCollection(Of Guid)
        Public Property VerfuegbareTrainerListe() As TrainerCollection

        Public Sub speichern() Implements IModel.speichern
            Throw New NotImplementedException()
        End Sub

#End Region

    End Class

End Namespace
