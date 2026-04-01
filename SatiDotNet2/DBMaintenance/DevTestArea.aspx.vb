
Partial Class DBMaintenance_DevTestArea
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        'Server.Transfer("~/Login.aspx?ReturnUrl=~/DBMaintenance/DevTestArea.aspx")

        MenuAuthenication.CheckGroupAuthenication("MasterDBAdmin", Server)
    End Sub
    Protected Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        SatiCode.FlexTest()

    End Sub

    Protected Sub ButtonTest200QA_Click(sender As Object, e As EventArgs) Handles ButtonTest200QA.Click
        Me.LabelQAFeedback.Text = SatiCode.Bulk_Final_QA(Me.TextBoxLotNumber.Text)
    End Sub
End Class
