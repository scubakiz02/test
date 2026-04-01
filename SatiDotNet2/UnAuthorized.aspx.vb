
Partial Class UnAuthorized
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)

        Dim query As String = Request.QueryString("GroupName")
        If query <> "" Then
            GroupAuthorized.Text = query
        Else
            GroupAuthorized.Text = "NONE"
        End If
    End Sub
End Class