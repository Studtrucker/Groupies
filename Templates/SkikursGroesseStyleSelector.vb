Imports Groupies.Entities

Namespace TemplateSelectors

    Public Class SkikursGroesseStyleSelector
        Inherits StyleSelector
        Public KleineGruppeStyleSelector As Style
        Public NormaleGruppeStyleSelector As Style
        Public GrosseGruppeStyleSelector As Style

        Public Overrides Function SelectStyle(item As Object, container As DependencyObject) As Style
            If GrosseGruppeStyleSelector Is Nothing Then
                GrosseGruppeStyleSelector = TryCast(Application.Current.FindResource("GrosseGruppeStyleSelector"), Style)
            End If
            If NormaleGruppeStyleSelector Is Nothing Then
                NormaleGruppeStyleSelector = TryCast(Application.Current.FindResource("NormaleGruppeStyleSelector"), Style)
            End If
            If KleineGruppeStyleSelector Is Nothing Then
                KleineGruppeStyleSelector = TryCast(Application.Current.FindResource("KleineGruppeStyleSelector"), Style)
            End If
            Dim SK = TryCast(item, ParticipantCollection)
            If SK IsNot Nothing Then
                If SK.Count < 5 Then
                    Return KleineGruppeStyleSelector
                End If
                If SK.Count >= 6 And SK.Count < 11 Then
                    Return NormaleGruppeStyleSelector
                End If
                Return GrosseGruppeStyleSelector
            End If
            Return MyBase.SelectStyle(item, container)
        End Function
    End Class
End Namespace
