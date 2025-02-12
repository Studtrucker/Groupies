Namespace Entities


    Public Class Einteilung
        Inherits BaseModelTest


#Region "Felder"
        Private _GruppenloseTeilnehmer As New TeilnehmerCollection
        Private _Gruppenliste = New GruppeCollection
        Private _GruppenloseTrainer As New TrainerCollection

#End Region

#Region "Properties"

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

        ''' <summary>
        ''' Trainer ohne Gruppe
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

#End Region
    End Class

End Namespace
