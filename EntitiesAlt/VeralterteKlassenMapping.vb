Imports Groupies.Entities
Public Module VeralterteKlassenMapping

    Private Gruppen As GruppenCollection

    Public Function MapSkiClub2Club(Skiclub As Veraltert.Skiclub) As Club

        ' Eine Collection instanziieren
        Dim NeueGruppenCollection = New GruppenCollection
        ' Eine Liste aus neuen Klassen aus den veralterten Klassen erstellen
        Dim NeueGruppenliste = (From Group In Skiclub.Grouplist Select MapGroup2Gruppe(Group)).ToList
        ' Jedes Mitglied der neuen Klasse aus der Liste der Collection hinzufügen
        NeueGruppenliste.ForEach(Sub(Gruppe) NeueGruppenCollection.Add(Gruppe))


        Dim NeueLeistungsstufeliste = (From Level In Skiclub.Levellist
                                       Select MapLevel2Leistungsstufe(Level)).ToList

        Dim LeistungsstufeCollection = New LeistungsstufeCollection
        NeueLeistungsstufeliste.ForEach(Sub(L) LeistungsstufeCollection.Add(L))

        Dim NeueTeilnehmerliste = (From Participant In Skiclub.Participantlist
                                   Select New Teilnehmer(Participant.ParticipantFirstName, Participant.ParticipantLastName) With {
                                      .Leistungsstand = MapLevel2Leistungsstufe(Participant.ParticipantLevel),
                                      .TeilnehmerID = Participant.ParticipantID}).ToList

        Dim TeilnehmerCollection = New TeilnehmerCollection
        NeueTeilnehmerliste.ForEach(Sub(t) TeilnehmerCollection.Add(t))

        Dim NeueTrainerliste = (From Instructor In Skiclub.Instructorlist
                                Select MapInstructor2Trainer(Instructor)).ToList

        Dim TrainerCollection = New TrainerCollection
        NeueTrainerliste.ForEach(Sub(t) TrainerCollection.Add(t))

        Dim NeuerClub = New Club() With {
            .Gruppenliste = NeueGruppenCollection,
            .Leistungsstufeliste = LeistungsstufeCollection,
            .Teilnehmerliste = TeilnehmerCollection,
            .Trainerliste = TrainerCollection}

        Return NeuerClub

    End Function

    Private Function MapGroup2Gruppe(Group As Veraltert.Group) As Gruppe

        ' Eine Collection instanziieren
        Dim TeilnehmerCollection = New TeilnehmerCollection
        ' Eine Liste aus neuen Klassen aus den veralterten Klassen erstellen
        Dim Mitgliederliste = (From Member In Group.GroupMembers
                               Select MapParticipant2Teilnehmer(Member)).ToList
        ' Jedes Mitglied der neuen Klasse aus der Liste der Collection hinzufügen
        Mitgliederliste.ForEach(Sub(m) TeilnehmerCollection.Add(m))


        Dim Gruppe = New Gruppe(Group.GroupPrintNaming) With {
            .Benennung = Group.GroupNaming,
            .Sortierung = Group.GroupSort,
            .Trainer = MapInstructor2Trainer(Group.GroupLeader),
            .GruppenID = Group.GroupID,
            .Leistungsstufe = MapLevel2Leistungsstufe(Group.GroupLevel),
            .Mitglieder = TeilnehmerCollection}

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
            .InstructorPicture = Instructor.InstructorPicture,
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
