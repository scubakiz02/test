
Partial Class TestArea_TestWhiteBlackListScribes
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        LabelReply.Text = ""
        LabelReply.Text = SatiCode.CheckScibes_FindChr(Me.TextBox11.Text, Me.TextBox1.Text, Me.TextBox2.Text, Me.TextBox3.Text, Me.TextBox4.Text, Me.TextBox5.Text, Me.TextBox6.Text, Me.TextBox7.Text, Me.TextBox8.Text, Me.TextBox9.Text, Me.TextBox10.Text, Me.CheckBox1.Checked)
    End Sub

End Class
