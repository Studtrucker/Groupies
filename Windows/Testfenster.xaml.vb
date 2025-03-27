Imports System.ComponentModel
Imports Microsoft.Win32
Imports Groupies.Entities.Generation3
Imports Groupies.Entities
Imports Groupies.Controller

Public Class Testfenster
    Dim Club As Club

    Dim _Einteilungen As ICollectionView
    Dim _Gruppen As ICollectionView


    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        Groupies.Services.SkiDateienService.OpenSkiDatei()

        DataContext = AppController.AktuellerClub

    End Sub

    Private Sub Testfenster_Loaded(sender As Object, e As RoutedEventArgs)

        Dim Model As New Club
        Model.ClubName = "Skiclub Meerbusch"

        Dim e1 = New Entities.Einteilung With {.Benennung = "Tag1"}
        Dim e2 = New Entities.Einteilung With {.Benennung = "Tag2"}
        Dim e3 = New Entities.Einteilung With {.Benennung = "Tag3"}

        e1.Gruppenliste.Add(New Entities.Gruppe With {.Benennung = "Gruppe11", .Trainer = New Entities.Trainer("Andreas")})
        e1.Gruppenliste.Add(New Entities.Gruppe With {.Benennung = "Gruppe12", .Trainer = New Entities.Trainer("Ralf")})
        e1.Gruppenliste.Add(New Entities.Gruppe With {.Benennung = "Gruppe13", .Trainer = New Entities.Trainer("Sandra")})

        e2.Gruppenliste.Add(New Entities.Gruppe With {.Benennung = "Gruppe21", .Trainer = New Entities.Trainer("Sandra")})
        e2.Gruppenliste.Add(New Entities.Gruppe With {.Benennung = "Gruppe22", .Trainer = New Entities.Trainer("Andreas")})
        e2.Gruppenliste.Add(New Entities.Gruppe With {.Benennung = "Gruppe23", .Trainer = New Entities.Trainer("Ralf")})

        e3.Gruppenliste.Add(New Entities.Gruppe With {.Benennung = "Gruppe31", .Trainer = New Entities.Trainer("Ralf")})
        e3.Gruppenliste.Add(New Entities.Gruppe With {.Benennung = "Gruppe32", .Trainer = New Entities.Trainer("Sandra")})
        e3.Gruppenliste.Add(New Entities.Gruppe With {.Benennung = "Gruppe33", .Trainer = New Entities.Trainer("Andreas")})

        Model.Einteilungsliste = New Entities.EinteilungCollection From {e1, e2, e3}

        DataContext = Model

    End Sub

End Class
