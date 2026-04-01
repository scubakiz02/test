
Partial Class MR_OpenMRQuickView
    Inherits System.Web.UI.Page

    Private Sub MR_OpenMRQuickView_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
        'MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        'menuAuthenication.CheckGroupAuthenication("Maintenance", Server)
    End Sub
End Class
