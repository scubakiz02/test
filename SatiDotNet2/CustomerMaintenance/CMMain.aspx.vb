
Partial Class CustomerMaintenance_CMMain
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        'Server.Transfer("~/Login.aspx?ReturnUrl=~/CustomerMaintenance/CMMain.aspx")

        MenuAuthenication.CheckGroupAuthenication("CustomerEdit", Server)
    End Sub
End Class
