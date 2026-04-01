
Partial Class CheckSum
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        LabelCS.Text = SatiCode.CheckSum(TextBoxT7.Text)
    End Sub
End Class
