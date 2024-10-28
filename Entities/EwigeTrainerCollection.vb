Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports Groupies.DataImport

Namespace Entities

    Public Class EwigeTrainerCollection
        Inherits ObservableCollection(Of EwigerTrainer)
        Implements IEnumerable(Of EwigerTrainer)

#Region "Konstruktor"
        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Trainerliste As IEnumerable(Of EwigerTrainer))
            MyBase.New
            Trainerliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

        Public Sub New(Trainer As Trainer, Datum As Date)
            Add(New EwigerTrainer(Trainer, Datum))
        End Sub

        Public Sub New(Trainer As Trainer, Datum As Date, LetzteGruppenmitgliedschaft As Guid)
            Add(New EwigerTrainer(Trainer, Datum))
        End Sub

#End Region

#Region "Porperties IEnumerable(String) Rückgabe"


#End Region

#Region "Porperties IEnumerable(Teilnehmer) Rückgabe"

#End Region

#Region "Funktionen und Methoden"

        Public Overloads Sub Add(Trainer As EwigerTrainer)
            If Me.Where(Function(Tn) Tn.TrainerID = Trainer.TrainerID).Count = 1 Then
                Me.Where(Function(Tn) Tn.TrainerID = Trainer.TrainerID).Single.ZuletztTeilgenommen = Trainer.ZuletztTeilgenommen
            Else
                MyBase.Add(Trainer)
            End If

        End Sub

        Public Overloads Sub Add(Trainer As Trainer, Datum As Date)
            If Me.Where(Function(Tn) Tn.TrainerID = Trainer.TrainerID).Count = 1 Then
                Me.Where(Function(Tn) Tn.TrainerID = Trainer.TrainerID).Single.ZuletztTeilgenommen = Datum
            Else
                MyBase.Add(New EwigerTrainer(Trainer, Datum))
            End If

        End Sub

        Public Overloads Sub Add(Trainer As Trainer, Datum As Date, GruppenID As Guid)
            If Me.Where(Function(Tn) Tn.TrainerID = Trainer.TrainerID).Count = 1 Then
                Me.Where(Function(Tn) Tn.TrainerID = Trainer.TrainerID).Single.ZuletztTeilgenommen = Datum
            Else
                MyBase.Add(New EwigerTrainer(Trainer, Datum))
            End If

        End Sub

#End Region

    End Class
End Namespace
