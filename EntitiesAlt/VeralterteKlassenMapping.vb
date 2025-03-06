Imports Groupies.Entities
Imports Groupies.Entities.Veraltert
Public Module VeralterteKlassenMapping

    Private NeuerClub As Club
    Private _Gruppenliste As GruppeCollection

    Public Function MapSkiClub2Club(Skiclub As Veraltert.Skiclub) As Club

        NeuerClub = New Club
        ' Jede Group dem Skiclub mappen und in die Gruppenliste des Clubs hängen
        NeuerClub.SelectedEinteilung.Gruppenliste = New GruppeCollection(Skiclub.Grouplist.ToList.Select(AddressOf MapGroup2Gruppe))
        NeuerClub.Leistungsstufenliste = New LeistungsstufeCollection(Skiclub.Levellist.Select(AddressOf MapLevel2Leistungsstufe).ToList)
        NeuerClub.SelectedEinteilung.GruppenloseTeilnehmer = New TeilnehmerCollection(Skiclub.ParticipantsNotInGroup.Select(AddressOf MapParticipant2Teilnehmer))
        NeuerClub.SelectedEinteilung.GruppenloseTrainer = New TrainerCollection(Skiclub.Instructorlist.Select(AddressOf MapInstructor2Trainer))
        For Each item In NeuerClub.SelectedEinteilung.EingeteilteTrainer
            If item IsNot Nothing Then
                NeuerClub.SelectedEinteilung.GruppenloseTrainer.RemoveByTrainerID(item.TrainerID)
            End If
        Next
        Return NeuerClub

    End Function

    Private Function MapGroup2Gruppe(Group As Veraltert.Group) As Gruppe

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

    Private Function MapParticipant2Teilnehmer(Participant As Veraltert.Participant) As Teilnehmer

        If Participant Is Nothing Then
            Return Nothing
        End If

        Dim Teilnehmer = New Teilnehmer(Participant.ParticipantFirstName, Participant.ParticipantLastName) With {
                                                                             .Leistungsstand = MapLevel2Leistungsstufe(Participant.ParticipantLevel),
                                                                             .TeilnehmerID = Participant.ParticipantID}

        Return Teilnehmer

    End Function

    Private Function MapInstructor2Trainer(Instructor As Veraltert.Instructor) As Trainer

        If Instructor Is Nothing Then
            Return Nothing
        End If

        Dim Trainer = New Trainer(Instructor.InstructorFirstName) With {
            .eMail = Instructor.eMail,
            .Foto = Instructor.InstructorPicture,
            .Nachname = Instructor.InstructorLastName,
            .Spitzname = Instructor.InstructorPrintName,
            .TrainerID = Instructor.InstructorID}

        Return Trainer

    End Function

    Private Function MapLevel2Leistungsstufe(Level As Veraltert.Level) As Leistungsstufe

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

    Private Function MapSkill2Faehigkeit(Skill As Veraltert.Skill) As Faehigkeit

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
