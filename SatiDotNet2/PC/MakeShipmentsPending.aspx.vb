
Partial Class PC_MakeShipmentsPending
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub ButtonSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSubmit.Click
        SatiCode.PendingShipmentAdd("Add", Me.TextBoxPickTicket.Text, Me.TextBoxComment.Text, User.Identity.Name.ToString)
        Me.TextBoxComment.Text = ""
        Me.TextBoxPickTicket.Text = ""
        CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.TextBoxComment.ClientID)


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("PC", Server)
    End Sub
End Class
