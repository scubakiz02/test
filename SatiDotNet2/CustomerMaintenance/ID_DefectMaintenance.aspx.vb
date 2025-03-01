
Partial Class DBMaintenance_ID_DefectMaintenance
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        'Server.Transfer("~/Login.aspx?ReturnUrl=~/CustomerMaintenance/ID_DefectMaintenance.aspx")

        MenuAuthenication.CheckGroupAuthenication("CustomerEdit", Server)
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "" Then
            Dim Row As String = e.CommandArgument.ToString
            Me.IDLabel.Text = Me.GridView1.Rows(Row).Cells(2).Text
            G2update()
        End If


    End Sub

    Protected Sub GridView2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView2.RowCommand
        G2update()
    End Sub


    Protected Sub GridView2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView2.SelectedIndexChanged

    End Sub
    Sub G2update()
        Dim NewSQL As String
        NewSQL = "SELECT [Key], ID, Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE (ID = '" & Me.IDLabel.Text & "')"
        'NewSQL = "SELECT [Key], ID, Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE (ID = '" & Me.IDLabel.Text & "')"
        SqlDataSource2.SelectCommand = NewSQL
    End Sub
End Class
