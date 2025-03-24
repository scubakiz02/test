
Partial Class Testing
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim ChartIndex As Integer = 1
        Me.UpdatePanel1.FindControl(("Panel" & ChartIndex)).Visible = True
        'Me.Panel1.Visible = True
    End Sub
End Class
