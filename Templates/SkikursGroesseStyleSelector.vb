Imports Skischule.Entities

Public Class SkikursGroesseStyleSelector
    Inherits StyleSelector

    Public KleineGruppe As Style
    Public NormaleGruppe As Style
    Public GrosseGruppe As Style

    Public Overrides Function SelectStyle(item As Object, container As DependencyObject) As Style
        If GrosseGruppe Is Nothing Then
            GrosseGruppe = TryCast(Application.Current.FindResource("GrosseGruppeStyle"), Style)
        End If
        If NormaleGruppe Is Nothing Then
            NormaleGruppe = TryCast(Application.Current.FindResource("NormaleGruppeStyle"), Style)
        End If
        If KleineGruppe Is Nothing Then
            KleineGruppe = TryCast(Application.Current.FindResource("KleineGruppeStyle"), Style)
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
        Return MyBase.SelectStyle(item, container)
    End Function
End Class
