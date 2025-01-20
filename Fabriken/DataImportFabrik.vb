
Public Class DataImportFabrik

    Public Function ErzeugeImportformat(Datentyp As String, Dateityp As String) As Importformat

        Dim FabrikSpalten = New DataImportSpaltennamenFabrik
        Dim Spalten = FabrikSpalten.ErzeugeSpalten(Datentyp)

        Dim FabrikTabellenname = New DataImportTabellennameFabrik
        Dim Tabelle = FabrikTabellenname.ErzeugeTabellename(Datentyp, Dateityp)

        Return New Importformat(Tabelle, Spalten)

    End Function

End Class

Public Class Importformat
    Public Property Tabelle As String
    Public Property Spalten As List(Of String)

    Public Sub New(Tabelle As IImportTabellenname, Spalten As IImportSpaltennamen)
        _Tabelle = Tabelle.Tabellenname
        _Spalten = Spalten.Spalten
    End Sub
End Class


Public Class DataImportTabellennameFabrik
    Public Function ErzeugeTabellename(Datentyp As String, Dateityp As String) As IImportTabellenname
        Select Case Dateityp
            Case ".xls", ".xlsx"
                Select Case Datentyp
                    Case "Teilnehmer"
                        Return New TabellennameTeilnehmerXl
                    Case Else
                        Return New TabellennameTrainerXl
                End Select
            Case ".csv"
                Return New TabellennameCsv
            Case Else
                Throw New IO.FileFormatException($"Der Dateityp [{Dateityp}] ist unbekannt")
        End Select
    End Function

End Class

Public Class DataImportSpaltennamenFabrik
    Public Function ErzeugeSpalten(Datentyp As String) As IImportSpaltennamen
        Select Case Datentyp
            Case "Teilnehmer"
                Return New SpaltennamenTeilnehmer
            Case "Trainer"
                Return New SpaltennamenTrainer
            Case Else
                Throw New IO.FileLoadException($"Der Datentyp [{Datentyp}] ist unbekannt")
        End Select
    End Function

End Class


Public Interface IImportTabellenname
    ReadOnly Property Tabellenname As String

End Interface

Public Class TabellennameTeilnehmerXl
    Implements IImportTabellenname
    Public ReadOnly Property Tabellenname As String = "Teilnehmer" Implements IImportTabellenname.Tabellenname
End Class

Public Class TabellennameTrainerXl
    Implements IImportTabellenname
    Public ReadOnly Property Tabellenname As String = "Trainer" Implements IImportTabellenname.Tabellenname
End Class

Public Class TabellennameCsv
    Implements IImportTabellenname
    Public ReadOnly Property Tabellenname As String = "Table1" Implements IImportTabellenname.Tabellenname
End Class

Public Interface IImportSpaltennamen
    ReadOnly Property Spalten As List(Of String)
End Interface

Public Class SpaltennamenTeilnehmer
    Implements IImportSpaltennamen

    Sub New()
        _Spalten = New List(Of String) From {"TeilnehmerID", "Vorname", "Nachname", "Geburtsdatum", "Telefonnummer", "Leistungsstufe"}
    End Sub

    Private ReadOnly Property Spalten As List(Of String) Implements IImportSpaltennamen.Spalten

End Class

Public Class SpaltennamenTrainer
    Implements IImportSpaltennamen

    Sub New()
        _Spalten = New List(Of String) From {"TrainerID", "Vorname", "Nachname", "Spitzname", "Telefonnummer", "E-Mail"}
    End Sub

    Public ReadOnly Property Spalten As List(Of String) Implements IImportSpaltennamen.Spalten

End Class



