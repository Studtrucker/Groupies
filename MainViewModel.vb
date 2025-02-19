Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Xml.Serialization
Imports Groupies.Entities

Public Class MainViewModel
    Inherits BaseModel


#Region "Properties"

    Public Property Club As Club
    Public Property Einteilungsliste As EinteilungCollection
    Public Property Trainerliste As TrainerCollection
    Public Property Teilnehmerliste As TeilnehmerCollection

    Private _SelectedEinteilung As Einteilung

    Public Property SelectedEinteilung As Einteilung
        Get
            Return _SelectedEinteilung
        End Get
        Set(value As Einteilung)
            _SelectedEinteilung = value
            SelectedGruppe = Nothing
        End Set
    End Property

    Private _SelectedGruppe As Gruppe
    Public Property SelectedGruppe As Gruppe
        Get
            Return _SelectedGruppe
        End Get
        Set(value As Gruppe)
            _SelectedGruppe = value
        End Set
    End Property

#End Region

#Region "Functions"
    ''' <summary>
    ''' Eine gespeicherte Datei wird eingelesen 
    ''' </summary>
    ''' <param name="Datei"></param>
    Public Sub XMLDateiEinlesen(Datei As String)
        If File.Exists(Datei) Then
            Try
                Dim DateiGelesen
                Using fs = New FileStream(Datei, FileMode.Open)
                    ' Versuche XML mit Struktur Groupies 2 zu lesen
                    DateiGelesen = LeseXMLDateiVersion2(fs)
                End Using
                ' Versuche XML mit Struktur Groupies 1 zu lesen
                Using fs = New FileStream(Datei, FileMode.Open)
                    If Not DateiGelesen Then
                        LeseXMLDateiVersion1(fs)
                    End If
                End Using

                Me.Trainerliste = Club.AlleTrainer
                If Club.Einteilungsliste IsNot Nothing AndAlso Club.Einteilungsliste.Count > 0 Then
                Else
                    Me.Einteilungsliste = New EinteilungCollection From {New Einteilung With {.Benennung = "Tag1", .Gruppenliste = Club.Gruppenliste}}
                End If

            Catch ex As InvalidDataException
                Debug.Print("Datei ungültig: " & ex.Message)
                Exit Sub
            End Try


        End If
    End Sub

    Private Function LeseXMLDateiVersion1(Filestream As FileStream) As Boolean
        Dim serializer = New XmlSerializer(GetType(Veraltert.Skiclub))
        Dim loadedSkiclub As Veraltert.Skiclub
        Try
            loadedSkiclub = TryCast(serializer.Deserialize(Filestream), Veraltert.Skiclub)
            Club = VeralterteKlassenMapping.MapSkiClub2Club(loadedSkiclub)
            Return True
        Catch ex As InvalidDataException
            Throw ex
            Return False
        End Try
    End Function

    Private Function LeseXMLDateiVersion2(Filestream As FileStream) As Boolean
        Dim serializer = New XmlSerializer(GetType(Club))
        Try
            Club = TryCast(serializer.Deserialize(Filestream), Club)
            Return True
        Catch ex As InvalidOperationException
            Return False
        Catch ex As InvalidDataException
            Throw ex
            Return False
        End Try
    End Function
#End Region

End Class
