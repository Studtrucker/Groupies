Imports Groupies.Entities
Public Module MappingGeneration1

    ''' <summary>
    ''' Mapped einen Skiclub aus der ersten Generation in einen Club der aktuellen Version
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Public Function MapSkiClub2Club(Skiclub As Generation1.Skiclub) As Generation4.Club

        Dim NeuerClub = New Generation4.Club With {
            .ClubName = If(Skiclub.Name, "Club"),
            .AlleTrainer = GetAlleTrainer(Skiclub),
            .AlleTeilnehmer = GetAlleTeilnehmer(Skiclub),
            .AlleLeistungsstufen = GetAlleLeistungsstufenVonTeilnehmern(Skiclub),
            .AlleFaehigkeiten = GetAlleFaehigkeiten(Skiclub),
            .AlleGruppen = GetAlleGruppen(Skiclub)}

        NeuerClub.Einteilungsliste.Add(New Einteilung With {.Benennung = "Tag 1", .Sortierung = 1})

        ' Erste Einteilung füllen
        Skiclub.Grouplist.ToList.ForEach(Sub(Gl) NeuerClub.Einteilungsliste(0).Gruppenliste.Add(MapGroup2Gruppe(Gl)))
        NeuerClub.Einteilungsliste(0).GruppenloseTrainer = GetGruppenloseTrainer(Skiclub)
        NeuerClub.Einteilungsliste(0).GruppenloseTeilnehmer = GetGruppenloseTeilnehmer(Skiclub)

        Return NeuerClub

    End Function

    ''' <summary>
    ''' Gruppen werden aus dem Skiclub extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleGruppen(Skiclub As Generation1.Skiclub) As GruppeCollection
        Dim Gruppen = New GruppeCollection
        ' Gruppen aus dem Skiclub entnehmen und in die Collection einfügen
        Skiclub.Grouplist.ToList.ForEach(Sub(g) Gruppen.Add(MapGroup2Gruppe(g)))

        Return Gruppen

    End Function

    ''' <summary>
    ''' Faehigkeiten werden aus den Leistungsstufen der Teilnehmern extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleFaehigkeiten(Skiclub As Generation1.Skiclub) As FaehigkeitCollection

        Dim Faehigkeiten = New FaehigkeitCollection
        ' Fähigkeiten aus den Leistungsstufen der Teilnehmer entnehmen und in die Collection einfügen
        Skiclub.Grouplist.ToList.ForEach(Sub(g) g.GroupMembers.ToList.ForEach(Sub(M) M.ParticipantLevel.LevelSkills.ToList.ForEach(Sub(f) Faehigkeiten.Add(MapSkill2Faehigkeit(f)))))
        Skiclub.ParticipantsNotInGroup.ToList.ForEach(Sub(T) T.ParticipantLevel.LevelSkills.ToList.ForEach(Sub(f) Faehigkeiten.Add(MapSkill2Faehigkeit(f))))
        ' Leere Fähigkeit hinzufügen für Dropdowns
        Faehigkeiten.Add(New Faehigkeit With {.FaehigkeitID = Guid.Empty, .Benennung = String.Empty, .Sortierung = -1})
        ' Entferne doppelte Fähigkeiten
        Faehigkeiten = New FaehigkeitCollection(Faehigkeiten.GroupBy(Of Guid)(Function(f) f.FaehigkeitID).Select(Function(Gruppe) Gruppe.First).ToList)

        Return Faehigkeiten

    End Function

    ''' <summary>
    ''' Teilnehmer werden aus den Gruppen und der gruppenlose Teilnehmer-Liste extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleTeilnehmer(Skiclub As Generation1.Skiclub) As TeilnehmerCollection

        Dim Teilnehmer = New TeilnehmerCollection

        Skiclub.Grouplist.ToList.ForEach(Sub(g) g.GroupMembers.ToList.ForEach(Sub(T) Teilnehmer.Add(MapParticipant2Teilnehmer(T))))
        Skiclub.ParticipantsNotInGroup.ToList.ForEach(Sub(T) Teilnehmer.Add(MapParticipant2Teilnehmer(T)))

        Return Teilnehmer

    End Function

    ''' <summary>
    ''' Die gruppenlose Teilnehmer-Liste wird erstellt
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' 
    Private Function GetGruppenloseTeilnehmer(Skiclub As Generation1.Skiclub) As TeilnehmerCollection

        Dim Teilnehmer = New TeilnehmerCollection

        Skiclub.ParticipantsNotInGroup.ToList.ForEach(Sub(T) Teilnehmer.Add(MapParticipant2Teilnehmer(T)))

        'Entferne leere Trainer-Objekte 
        Teilnehmer.Where(Function(T) T Is Nothing).ToList.ForEach(Function(O) Teilnehmer.Remove(O))

        ' Entferne doppelte Trainer
        Teilnehmer = New TeilnehmerCollection(Teilnehmer.GroupBy(Of Guid)(Function(LS) LS.TeilnehmerID).Select(Function(T) T.First).ToList)

        Return Teilnehmer

    End Function


    ''' <summary>
    ''' Trainer werden aus den Gruppen extrahiert
    ''' Die gruppenlose Trainer-Liste wird extrahiert: alle Trainer ausser die eingeteilten Trainer.
    ''' </summary>
    ''' <param name="Skiclub"></param>
    Private Function GetAlleTrainer(Skiclub As Generation1.Skiclub) As TrainerCollection

        Dim Trainer = New TrainerCollection

        Skiclub.Grouplist.ToList.ForEach(Sub(g) Trainer.Add(MapInstructor2Trainer(g.GroupLeader)))

        'Entferne leere Trainer-Objekte 
        Trainer.Where(Function(T) T Is Nothing).ToList.ForEach(Function(O) Trainer.Remove(O))

        ' Entferne doppelte Trainer
        Trainer = New TrainerCollection(Trainer.GroupBy(Of Guid)(Function(LS) LS.TrainerID).Select(Function(T) T.First).ToList)

        Return Trainer

    End Function

    ''' <summary>
    ''' Die gruppenlose Trainer-Liste wird extrahiert: alle Trainer ausser die eingeteilten Trainer.
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetGruppenloseTrainer(Skiclub As Generation1.Skiclub) As TrainerCollection

        Dim AlleTrainer = New Generation1.InstructorCollection
        Dim EingeteilteTrainer = New Generation1.InstructorCollection

        Skiclub.Instructorlist.ToList.ForEach(Sub(T) AlleTrainer.Add(T))
        Skiclub.Grouplist.ToList.ForEach(Sub(g) EingeteilteTrainer.Add(g.GroupLeader))

        Dim GefilterteTrainer = AlleTrainer.Where(Function(T) Not EingeteilteTrainer.Any(Function(ET) ET.InstructorID = T.InstructorID))

        Dim NeueTrainer = New TrainerCollection
        GefilterteTrainer.ToList.ForEach(Sub(T) NeueTrainer.Add(MapInstructor2Trainer(T)))

        Return NeueTrainer

    End Function

    ''' <summary>
    ''' Leistungsstufen werden aus den Teilnehmern extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleLeistungsstufenVonTeilnehmern(Skiclub As Generation1.Skiclub) As LeistungsstufeCollection
        ' Eigene Collection initialisieren
        Dim Leistungsstufen = New LeistungsstufeCollection From {
            New Leistungsstufe With {.LeistungsstufeID = Guid.Empty, .Benennung = String.Empty, .Sortierung = -1}
        }

        ' Leistungsstufen aus den Teilnehmern entnehmen und in die Collection einfügen
        Skiclub.Grouplist.ToList.ForEach(Sub(Gl) Gl.GroupMembers.ToList.ForEach(Sub(M) Leistungsstufen.Add(MapLevel2Leistungsstufe(M.ParticipantLevel))))
        Skiclub.ParticipantsNotInGroup.ToList.ForEach(Sub(T) Leistungsstufen.Add(MapLevel2Leistungsstufe(T.ParticipantLevel)))

        ' Entferne doppelte Leistungsstufen über ID
        Leistungsstufen = New LeistungsstufeCollection(Leistungsstufen.GroupBy(Of Guid)(Function(LS) LS.LeistungsstufeID).Select(Function(Gruppe) Gruppe.First).ToList)

        Return Leistungsstufen

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
