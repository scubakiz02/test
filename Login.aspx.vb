
Partial Class SatiUsers_Login
    Inherits System.Web.UI.Page

    Protected Sub Login1_LoginError(ByVal sender As Object, ByVal e As System.EventArgs) Handles Login1.LoginError

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckLogInAuthenication(Page, User, Server)
    End Sub

End Class
