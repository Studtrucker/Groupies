Imports Groupies.Entities
Public Module VeralterteKlassenMapping

    Private NeuerClub As Club
    Private _Gruppenliste As GruppeCollection

    Public Function MapSkiClub2Club(Skiclub As Veraltert.Skiclub) As Club

        NeuerClub = New Club
        ' Jeden Groupmember aus den Groups in die Teilnehmerliste des Clubs hängen
        Skiclub.Grouplist.ToList.ForEach(Sub(Group) MapGroup2Gruppe(Group))

        NeuerClub.Leistungsstufenliste = New LeistungsstufeCollection(Skiclub.Levellist.Select(AddressOf MapLevel2Leistungsstufe).ToList)

        Return NeuerClub

    End Function

    Private Function MapGroup2Gruppe(Group As Veraltert.Group) As Gruppe

        ' Die Gruppe mappen
        Dim Gruppe = New Gruppe(Group.GroupPrintNaming) With {
            .Benennung = Group.GroupNaming,
            .Sortierung = Group.GroupSort,
            .GruppenID = Group.GroupID,
            .Leistungsstufe = MapLevel2Leistungsstufe(Group.GroupLevel)}
        NeuerClub.Gruppenliste.Add(Gruppe)

        ' Den Trainer mappen
        Dim Trainer = MapInstructor2Trainer(Group.GroupLeader)
        NeuerClub.Trainerliste.Add(Trainer)
        NeuerClub.TrainerEinerGruppeZuweisen(Trainer, Gruppe)

        ' Die Teilnehmer mappen
        'For Each Participant In Group.GroupMembers
        '    Dim Teilnehmer = MapParticipant2Teilnehmer(Participant)
        '    NeuerClub.Teilnehmerliste.Add(Teilnehmer)
        '    NeuerClub.TeilnehmerInGruppeEinteilen(Teilnehmer, Gruppe)
        'Next

        Dim Mitglieder = Group.GroupMembers.Select(AddressOf MapParticipant2Teilnehmer).ToList
        Mitglieder.ForEach(Sub(M) NeuerClub.Teilnehmerliste.Add(M))
        Mitglieder.ForEach(Sub(M) NeuerClub.TeilnehmerInGruppeEinteilen(M, Gruppe))

        Return Gruppe

    End Function

    Private Function MapParticipant2Teilnehmer(Participant As Veraltert.Participant) As Teilnehmer
        Dim Teilnehmer = New Teilnehmer(Participant.ParticipantFirstName, Participant.ParticipantLastName) With {
                                                                             .Leistungsstand = MapLevel2Leistungsstufe(Participant.ParticipantLevel),
                                                                             .TeilnehmerID = Participant.ParticipantID}

        Return Teilnehmer

    End Function

    Private Function MapInstructor2Trainer(Instructor As Veraltert.Instructor) As Trainer
        Dim Trainer = New Trainer(Instructor.InstructorFirstName) With {
            .eMail = Instructor.eMail,
            .Foto = Instructor.InstructorPicture,
            .Nachname = Instructor.InstructorLastName,
            .Spitzname = Instructor.InstructorPrintName,
            .TrainerID = Instructor.InstructorID}

        Return Trainer

    End Function

    Private Function MapLevel2Leistungsstufe(Level As Veraltert.Level) As Leistungsstufe

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

    Private Function MapSkill2Faehigkeit(Skill As Veraltert.Skill) As Faehigkeit
        Dim Faehigkeit = New Faehigkeit(Skill.SkillNaming) With {
                                .Beschreibung = Skill.Description,
                                .FaehigkeitID = Skill.SkillID,
                                .Sortierung = Skill.SortNumber}
        Return Faehigkeit
    End Function

End Module
