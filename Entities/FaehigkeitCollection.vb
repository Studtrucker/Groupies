Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

    Public Class FaehigkeitCollection
        Inherits ObservableCollection(Of Faehigkeit)

        Property GeordnetNachSortierung As IEnumerable(Of String) = From f In Me
                                                                    Order By f.Sortierung
                                                                    Select $"{f.Benennung}{Environment.NewLine}"

        ReadOnly Property GeordnetNachSortierung1 As String
            Get
                Dim x = (From t In Me Order By t.Sortierung
                         Select $"{t.Sortierung}. {t.Benennung}{Environment.NewLine}{t.Beschreibung}").ToList
                Dim Text As String = ""
                x.ForEach(Function(a) Text = Text & a)
                Return Text
            End Get
        End Property

    End Class

End Namespace
