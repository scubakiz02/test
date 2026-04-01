
Partial Class PC_RemoveShipment
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim row As String
        Dim PickTicket As String
        row = e.CommandArgument.ToString
        PickTicket = Me.GridView1.Rows(row).Cells(1).Text
        If e.CommandName = "Remove" Then
            KillShipment(PickTicket)
        End If
        Me.GridView1.DataBind()

    End Sub
    Sub KillShipment(Pick As String)

    End Sub
End Class
