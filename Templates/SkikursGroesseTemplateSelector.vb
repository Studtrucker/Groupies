Imports Skischule.Entities

Public Class SkikursGroesseTemplateSelector
    Inherits DataTemplateSelector
    Public KleineGruppe As DataTemplate
    Public NormaleGruppe As DataTemplate
    Public GrosseGruppe As DataTemplate

    Public Overrides Function SelectTemplate(item As Object, container As DependencyObject) As DataTemplate
        If GrosseGruppe Is Nothing Then
            GrosseGruppe = TryCast(Application.Current.FindResource("GrosseGruppe"), DataTemplate)
        End If
        If NormaleGruppe Is Nothing Then
            NormaleGruppe = TryCast(Application.Current.FindResource("NormaleGruppe"), DataTemplate)
        End If
        If KleineGruppe Is Nothing Then
            KleineGruppe = TryCast(Application.Current.FindResource("KleineGruppe"), DataTemplate)
        End If
        Dim SK = TryCast(item, TeilnehmerCollection)
        If SK IsNot Nothing Then
            If SK.Count < 7 Then
                Return KleineGruppe
            End If
            If SK.Count >= 7 And SK.Count < 14 Then
                Return NormaleGruppe
            End If
            Return GrosseGruppe
        End If
        Return MyBase.SelectTemplate(item, container)
    End Function
End Class
