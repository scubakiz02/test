
Partial Class Production_RunSheets
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        If Roles.IsUserInRole("RunSheets") = False Then
    
        End If
    End Sub
End Class
