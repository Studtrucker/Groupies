Imports Groupies.Entities
Public Module MappingGeneration1

    ''' <summary>
    ''' Mapped einen Skiclub aus der ersten Generation in einen Club der aktuellen Version
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Public Function MapSkiClub2Club(Skiclub As Generation1.Skiclub) As Generation3.Club

        Dim NeuerClub = New Generation3.Club
        NeuerClub.ClubName = If(Skiclub.Name, "Club")
        ' Neue Einteilung erstellen
        NeuerClub.Einteilungsliste.Add(New Einteilung With {.Benennung = "Tag1", .Sortierung = 1})

        ' Jede Group aus dem Skiclub mappen und in die Gruppenliste der ersten Einteilung des Clubs hängen
        NeuerClub.Einteilungsliste(0).Gruppenliste = New GruppeCollection(Skiclub.Grouplist.ToList.Select(AddressOf MapGroup2Gruppe))

        ' Unabhängige Leistungsstufen 
        NeuerClub.Leistungsstufenliste = New LeistungsstufeCollection(Skiclub.Levellist.Select(AddressOf MapLevel2Leistungsstufe).ToList)
        ' Gruppenlose Teilnehmer und Trainer mappen
        NeuerClub.Einteilungsliste(0).GruppenloseTeilnehmer = New TeilnehmerCollection(Skiclub.ParticipantsNotInGroup.Select(AddressOf MapParticipant2Teilnehmer))
        NeuerClub.Einteilungsliste(0).GruppenloseTrainer = New TrainerCollection(Skiclub.Instructorlist.Select(AddressOf MapInstructor2Trainer))
        ' Trainer, die bereits in Gruppen eingeteilt wurden, aus den Gruppenlosen entfernen
        NeuerClub.Einteilungsliste(0).Gruppenliste.ToList.ForEach(Sub(G) NeuerClub.Einteilungsliste(0).GruppenloseTrainer.RemoveByTrainerID(G.Trainer.TrainerID))

        Return NeuerClub

    End Function

    Private Function MapGroup2Gruppe(Group As Generation1.Group) As Gruppe

        ' Die Gruppe mappen
        Dim Gruppe = New Gruppe(Group.GroupPrintNaming) With {
            .Benennung = Group.GroupNaming,
            .Sortierung = Group.GroupSort,
            .GruppenID = Group.GroupID,
            .Leistungsstufe = MapLevel2Leistungsstufe(Group.GroupLevel)}

        ' Den Trainer mappen
        Dim Trainer = MapInstructor2Trainer(Group.GroupLeader)
        Gruppe.Trainer = MapInstructor2Trainer(Group.GroupLeader)

        ' Jeder Member aus den Groupmembers wird zum Teilnehmer gemappt und kann sofort in die Gruppe gehängt werden
        Group.GroupMembers.Select(AddressOf MapParticipant2Teilnehmer).ToList.ForEach(Sub(M) Gruppe.Mitgliederliste.Add(M))

        Return Gruppe

    End Function

    Private Function MapParticipant2Teilnehmer(Participant As Generation1.Participant) As Teilnehmer

        If Participant Is Nothing Then
            Return Nothing
        End If

        Dim Teilnehmer = New Teilnehmer(Participant.ParticipantFirstName, Participant.ParticipantLastName) With {
                                                                             .Leistungsstand = MapLevel2Leistungsstufe(Participant.ParticipantLevel),
                                                                             .TeilnehmerID = Participant.ParticipantID}

        Return Teilnehmer

    End Function

    Private Function MapInstructor2Trainer(Instructor As Generation1.Instructor) As Trainer

        If Instructor Is Nothing Then
            Return Nothing
        End If

        Dim Trainer = New Trainer(Instructor.InstructorFirstName) With {
            .EMail = Instructor.eMail,
            .Foto = Instructor.InstructorPicture,
            .Nachname = Instructor.InstructorLastName,
            .Spitzname = Instructor.InstructorPrintName,
            .TrainerID = Instructor.InstructorID}

        Return Trainer

    End Function

    Private Function MapLevel2Leistungsstufe(Level As Generation1.Level) As Leistungsstufe

        If Level Is Nothing Then
            Return Nothing
        End If

        ' Eine Collection instanziieren
        Dim FaehigkeitCollection = New FaehigkeitCollection
        ' Eine Liste aus neuen Klassen aus den veralterten Klassen erstellen
        Dim Faehigkeitliste = (From Skill In Level.LevelSkills
                               Select MapSkill2Faehigkeit(Skill)).ToList
        ' Jedes Mitglied der neuen Klasse aus der Liste der Collection hinzufügen
        Faehigkeitliste.ForEach(Sub(f) FaehigkeitCollection.Add(f))

        Dim Leistungsstufe = New Leistungsstufe(Level.LevelNaming) With {
            .Beschreibung = Level.LevelDescription,
            .LeistungsstufeID = Level.LevelID,
            .Sortierung = Level.SortNumber,
            .Faehigkeiten = FaehigkeitCollection}

        Return Leistungsstufe

    End Function

    Private Function MapSkill2Faehigkeit(Skill As Generation1.Skill) As Faehigkeit

        If Skill Is Nothing Then
            Return Nothing
        End If

        Dim Faehigkeit = New Faehigkeit(Skill.SkillNaming) With {
                                .Beschreibung = Skill.Description,
                                .FaehigkeitID = Skill.SkillID,
                                .Sortierung = Skill.SortNumber}
        Return Faehigkeit
    End Function

End Module
