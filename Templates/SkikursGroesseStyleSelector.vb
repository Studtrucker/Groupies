Imports Groupies.Entities

Namespace TemplateSelectors

    Public Class SkikursGroesseStyleSelector
        Inherits StyleSelector
        Public KleineGruppeStyleSelector As Style
        Public NormaleGruppeStyleSelector As Style
        Public GrosseGruppeStyleSelector As Style

        Public Overrides Function SelectStyle(item As Object, container As DependencyObject) As Style
            If GrosseGruppeStyleSelector Is Nothing Then
                GrosseGruppeStyleSelector = TryCast(Application.Current.FindResource("GrosseGruppeStyleTemplate"), Style)
            End If
            If NormaleGruppeStyleSelector Is Nothing Then
                NormaleGruppeStyleSelector = TryCast(Application.Current.FindResource("NormaleGruppeStyleTemplate"), Style)
            End If
            If KleineGruppeStyleSelector Is Nothing Then
                KleineGruppeStyleSelector = TryCast(Application.Current.FindResource("KleineGruppeStyleTemplate"), Style)
            End If
            Dim SK = TryCast(item, TeilnehmerCollection)
            If SK IsNot Nothing Then
                If SK.Count < 6 Then
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
