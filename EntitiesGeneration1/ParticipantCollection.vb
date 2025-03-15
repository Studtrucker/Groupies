Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities.Generation1

    Public Class ParticipantCollection
        Inherits ObservableCollection(Of Participant)
        Implements IEnumerable

    End Class
End Namespace
