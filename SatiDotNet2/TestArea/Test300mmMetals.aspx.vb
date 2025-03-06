
Partial Class TestArea_Test300mmMetals
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Protected Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        SatiCode.GetCarton300mmMetals(Me.TextBox1.Text)
        Me.SqlDataSource1.SelectCommand = "SELECT T_FGI_Boxes.InstanceKey, [GFAAS Data].[Date/Time], [GFAAS Data].Source, [GFAAS Data].[Test Type], [GFAAS Data].Idenyification, [GFAAS Data].Location, [GFAAS Data].Ca, [GFAAS Data].Ma, [GFAAS Data].Ni, [GFAAS Data].Zn, [GFAAS Data].Al, [GFAAS Data].Fe, [GFAAS Data].Cr, [GFAAS Data].Cu, [GFAAS Data].Na, [GFAAS Data].K, [GFAAS Data].Co, [GFAAS Data].Mn, [GFAAS Data].Mo, [GFAAS Data].W, [GFAAS Data].Ti FROM LabelsMade INNER JOIN T_FGI_Boxes ON LabelsMade.LabelRecordNumber = T_FGI_Boxes.LabelsMadeKey INNER JOIN [GFAAS Data] ON LabelsMade.Lot = [GFAAS Data].Idenyification WHERE (T_FGI_Boxes.InstanceKey = " & Me.TextBox1.Text & ")"
        Me.GridView1.DataBind()

    End Sub
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TextBoxRandom.Text = SatiCode.GetRandomNumber(Me.TextBoxU.Text, Me.TextBoxL.Text)
    End Sub
End Class
