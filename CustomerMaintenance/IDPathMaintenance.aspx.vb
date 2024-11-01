
Partial Class DBMaintenance_IDPathMaintenance
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        'Server.Transfer("~/Login.aspx?ReturnUrl=~/CustomerMaintenance/IDPathMaintenance.aspx")

        MenuAuthenication.CheckGroupAuthenication("CustomerEdit", Server)
    End Sub


    Protected Sub GridView2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView2.RowCommand
        If e.CommandName = "Select" Then
            Dim Row As String
            Row = e.CommandArgument
            Session("PathName") = Me.GridView2.Rows(Row).Cells(1).Text
            Session("ID") = Me.GridView2.Rows(Row).Cells(0).Text
            Me.PathLabel.Text = Session("PathName").ToString
            Me.IDLabel.Text = Session("ID").ToString
            'MsgBox("test")
            ReView()
        End If
    End Sub

    Sub ReView()
        SqlDataSource2.SelectCommand = "SELECT PathName, ProcessOrder, StageName FROM dbo.CannedPaths WHERE (PathName = N'" & Session("PathName").ToString & "')"
        SqlDataSource1.SelectCommand = "SELECT ID FROM dbo.WI_Rev WHERE (PathName = N'" & Session("PathName").ToString & "')"
    End Sub

End Class
