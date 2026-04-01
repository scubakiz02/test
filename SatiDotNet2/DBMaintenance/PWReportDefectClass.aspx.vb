
Partial Class DBMaintenance_PWReportDefectClass
    Inherits System.Web.UI.Page

    Protected Sub ViewButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewButton.Click
        If Me.CodeTextBox.Text.ToString = "PW" Or Me.CodeTextBox.Text.ToString = "pw" Then
            Me.GridView1.Visible = True
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub
End Class
