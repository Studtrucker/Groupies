Namespace Entities


    Public Class Einteilung
        Inherits BaseModel


#Region "Felder"

        Private _Gruppenliste = New GruppeCollection
        Private _GruppenloseTeilnehmer As New TeilnehmerCollection
        Private _GruppenloseTrainer As New TrainerCollection

        Private ReadOnly _EingeteilteTrainer As New TrainerCollection
        Private ReadOnly _AlleTrainer As New TrainerCollection
        Private ReadOnly _EingeteilteTeilnehmer As New TeilnehmerCollection
        Private ReadOnly _AlleTeilnehmer As New TeilnehmerCollection

#End Region

#Region "Properties"

        ''' <summary>
        ''' Die Einteilungen können beispielsweise
        ''' mit den Tagen benannt werden
        ''' </summary>
        ''' <returns></returns>
        Public Property Sortierung() As Integer

        ''' <summary>
        ''' Die Einteilungen können beispielsweise
        ''' mit den Tagen benannt werden
        ''' </summary>
        ''' <returns></returns>
        Public Property Benennung() As String

        ''' <summary>
        ''' Eine Liste aller Gruppen
        ''' </summary>
        ''' <returns></returns>
        Public Property Gruppenliste() As GruppeCollection
            Get
                Return _Gruppenliste
            End Get
            Set(value As GruppeCollection)
                _Gruppenliste = value
            End Set
        End Property


        ''' <summary>
        ''' Teilnehmer ohne Gruppe
        ''' Alle neuen Teilnehmer werden hier eingetragen
        ''' </summary>
        ''' <returns></returns>
        Public Property GruppenloseTeilnehmer() As TeilnehmerCollection
            Get
                Return _GruppenloseTeilnehmer
            End Get
            Set(value As TeilnehmerCollection)
                _GruppenloseTeilnehmer = value
            End Set
        End Property

        Private _SelectedGruppenloserTeilnehmer As Teilnehmer
        ''' <summary>
        ''' Der aktuell ausgewählte gruppenlose Teilnehmer
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectedGruppenloserTeilnehmer As Teilnehmer
            Get
                Return _SelectedGruppenloserTeilnehmer
            End Get
            Set(value As Teilnehmer)
                _SelectedGruppenloserTeilnehmer = value
            End Set
        End Property

        ''' <summary>
        ''' Trainer ohne Gruppe
        ''' Alle neuen Trainer werden hier eingetragen
        ''' </summary>
        ''' <returns></returns>
        Public Property GruppenloseTrainer() As TrainerCollection
            Get
                Return _GruppenloseTrainer
            End Get
            Set(value As TrainerCollection)
                _GruppenloseTrainer = value
            End Set
        End Property

        Private _SelectedGruppenloserTrainer As Trainer
        ''' <summary>
        ''' Der aktuell ausgewählte Trainer
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectedGruppenloserTrainer As Trainer
            Get
                Return _SelectedGruppenloserTrainer
            End Get
            Set(value As Trainer)
                _SelectedGruppenloserTrainer = value
            End Set
        End Property

        ''' <summary>
        ''' Trainer, die bereits in Gruppen eingeteilt wurden
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property EingeteilteTrainer() As TrainerCollection
            ' = Gruppenliste.ToList.Select(Function(Gr) Gr.Trainer))
            Get
                _EingeteilteTrainer.Clear()
                Gruppenliste.ToList.Where(Function(Gr) Gr.Trainer IsNot Nothing).ToList.ForEach(Sub(Gr) _EingeteilteTrainer.Add(Gr.Trainer))
                Return _EingeteilteTrainer
            End Get
        End Property


        ''' <summary>
        ''' Liste mit allen Trainern, Eingeteilte und die ohne Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AlleTrainer() As TrainerCollection
            Get
                _AlleTrainer.Clear()
                EingeteilteTrainer.ToList.ForEach(Sub(T) _AlleTrainer.Add(T))
                GruppenloseTrainer.ToList.ForEach(Sub(T) _AlleTrainer.Add(T))
                Return _AlleTrainer
            End Get
        End Property

        ''' <summary>
        '''  Teilnehmer, die bereits in Gruppen eingeteilt wurden 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property EingeteilteTeilnehmer As TeilnehmerCollection
            Get
                _EingeteilteTeilnehmer.Clear()
                Gruppenliste.ToList.ForEach(Sub(G) G.Mitgliederliste.ToList.ForEach(Sub(M) _EingeteilteTeilnehmer.Add(M)))
                Return _EingeteilteTeilnehmer
            End Get
        End Property


        ''' <summary>
        ''' Liste mit allen Teilnehmern, Eingeteilte und die ohne Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AlleTeilnehmer As TeilnehmerCollection
            Get
                _AlleTeilnehmer.Clear()
                EingeteilteTeilnehmer.ToList.ForEach(Sub(M) _AlleTeilnehmer.Add(M))
                GruppenloseTeilnehmer.ToList.ForEach(Sub(M) _AlleTeilnehmer.Add(M))
                Return _AlleTeilnehmer
            End Get
        End Property
#End Region

#Region "Methoden und Funktionen"

        ''' <summary>
        ''' Trainer dieser Einteilung hinzufügen
        ''' </summary>
        ''' <param name="Trainer"></param>
        Public Sub TrainerHinzufuegen(Trainer As Trainer)
            GruppenloseTrainer.Add(Trainer)
        End Sub

        ''' <summary>
        ''' Trainer aus dieser Einteilung entfernen
        ''' </summary>
        ''' <param name="Trainer"></param>
        Public Sub TrainerEntfernen(Trainer As Trainer)
            GruppenloseTrainer.Remove(Trainer)
        End Sub


        ''' <summary>
        ''' Teilnehmer dieser Einteilung hinzufügen
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        Public Sub TeilnehmerHinzufuegen(Teilnehmer As Teilnehmer)
            GruppenloseTeilnehmer.Add(Teilnehmer)
        End Sub

        ''' <summary>
        ''' Teilnehmer aus dieser Einteilung entfernen
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        Public Sub TeilnehmerEntfernen(Teilnehmer As Teilnehmer)
            GruppenloseTeilnehmer.Remove(Teilnehmer)
        End Sub




        ''' <summary>
        ''' Der Teilnehmer wird aus der angegebenen Gruppe als Mitglied entfernt
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        ''' <param name="Gruppe"></param>
        Public Sub TeilnehmerAusGruppeEntfernen(Teilnehmer As Teilnehmer, Gruppe As Gruppe)
            Gruppe.Mitgliederliste.Remove(Teilnehmer)
            GruppenloseTeilnehmer.Add(Teilnehmer)
        End Sub

        ''' <summary>
        ''' Die angegebene Gruppe bekommt den Teilnehmer als Mitglied
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        ''' <param name="Gruppe"></param>
        Public Sub TeilnehmerInGruppeEinteilen(Teilnehmer As Teilnehmer, Gruppe As Gruppe)
            Gruppe.Mitgliederliste.Add(Teilnehmer)
            GruppenloseTeilnehmer.Remove(Teilnehmer)
        End Sub

        ''' <summary>
        ''' Der Trainer wird der angegebenen Gruppe zugewiesen
        ''' </summary>
        ''' <param name="Trainer"></param>
        ''' <param name="Gruppe"></param>
        Public Sub TrainerEinerGruppeZuweisen(Trainer As Trainer, Gruppe As Gruppe)
            Gruppe.Trainer = Trainer
            GruppenloseTrainer.Remove(Trainer)
        End Sub

        ''' <summary>
        ''' Der Trainer wird aus der angegebenen Gruppe entfernt
        ''' </summary>
        ''' <param name="Gruppe"></param>
        Public Sub TrainerAusGruppeEntfernen(Gruppe As Gruppe)
            GruppenloseTrainer.Add(Gruppe.Trainer)
            Gruppe.Trainer = Nothing
        End Sub

#End Region

    End Class

End Namespace
