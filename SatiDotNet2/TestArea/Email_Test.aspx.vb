
Partial Class TestArea_Email_Test
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SatiCode.SendMail365("This is a Test Message", "Test Subject", Me.TextBoxEmailAddress.Text, "tim.hughes@purewafer.com")
    End Sub


    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'Roles.CreateRole("SJAdmin")
    End Sub
End Class
