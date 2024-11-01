
Partial Class TestArea_Email_Test
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SatiCode.GetCarton300mmMetals(Me.TextBoxEmailAddress.Text)
        GridView1.DataBind()
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myScript", "runAfterLoad();", True)
    End Sub
End Class
