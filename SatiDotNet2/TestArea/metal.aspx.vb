
Partial Class TestArea_metal
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.GridView1.DataSource = Saticode.GetCarton200mmMetals(Me.TextBox1.Text)
        Me.GridView1.DataBind()
    End Sub
End Class
