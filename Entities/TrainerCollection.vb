Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Data.SqlClient

Namespace Entities

    Public Class TrainerCollection
        Inherits ObservableCollection(Of Trainer)
        Implements IEnumerable(Of Trainer)




        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Trainerliste As IEnumerable(Of Trainer))
            Trainerliste.ToList.ForEach(Sub(T) Add(T))
        End Sub


        Public ReadOnly Property VerfuegbareTrainer = From t In Me Where t.IstEinerGruppeZugewiesen = False

        Public ReadOnly Property GeordnetNachnameVorname = From t In Me Order By t.Nachname, t.Vorname

    End Class
End Namespace
