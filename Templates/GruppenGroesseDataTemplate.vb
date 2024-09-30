Imports Groupies.Entities

Namespace TemplateSelectors

    Public Class GruppenGroesseDataTemplate
        Inherits DataTemplateSelector

        Public KleineGruppeDataTemplate As DataTemplate
        Public NormaleGruppeDataTemplate As DataTemplate
        Public GrosseGruppeDataTemplate As DataTemplate

        Public Overrides Function SelectTemplate(item As Object, container As DependencyObject) As DataTemplate
            If GrosseGruppeDataTemplate Is Nothing Then
                GrosseGruppeDataTemplate = TryCast(Application.Current.FindResource("GrosseGruppeDataTemplate"), DataTemplate)
            End If
            If NormaleGruppeDataTemplate Is Nothing Then
                NormaleGruppeDataTemplate = TryCast(Application.Current.FindResource("NormaleGruppeDataTemplate"), DataTemplate)
            End If
            If KleineGruppeDataTemplate Is Nothing Then
                KleineGruppeDataTemplate = TryCast(Application.Current.FindResource("KleineGruppeDataTemplate"), DataTemplate)
            End If
            Dim SK = TryCast(item, TeilnehmerCollection)
            If SK IsNot Nothing Then
                If SK.Count < 5 Then
                    Return KleineGruppeDataTemplate
                End If
                If SK.Count >= 6 And SK.Count < 10 Then
                    Return NormaleGruppeDataTemplate
                End If
                Return GrosseGruppeDataTemplate
            End If
            Return MyBase.SelectTemplate(item, container)
        End Function
    End Class

End Namespace
