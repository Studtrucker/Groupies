Imports Groupies.Entities
Public Interface IClub

    ''' <summary>
    ''' Der Clubname
    ''' </summary>
    ''' <returns></returns>
    Property ClubName As String

    ''' <summary>
    ''' Leistungsstufen-Vorlage
    ''' zur Verwendung in den Gruppen 
    ''' und bei den Teilnehmern
    ''' </summary>
    ''' <returns></returns>
    Property LeistungsstufenTemplate As LeistungsstufeCollection

    ''' <summary>
    ''' Fähigkeiten-Vorlage 
    ''' zur Verwendung in den Leistungsstufen
    ''' </summary>
    ''' <returns></returns>
    Property FaehigkeitenTemplate As FaehigkeitCollection


End Interface
