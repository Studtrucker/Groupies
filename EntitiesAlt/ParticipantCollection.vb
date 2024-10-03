Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities.Veraltert

    Public Class ParticipantCollection
        Inherits ObservableCollection(Of Participant)
        Implements IEnumerable

    End Class
End Namespace
