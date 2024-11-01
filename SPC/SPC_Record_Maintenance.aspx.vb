
Partial Class SPC_SPC_Record_Maintenance
    Inherits System.Web.UI.Page

    Private Sub SPC_SPC_Record_Maintenance_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("Eng", Server)
    End Sub
End Class
