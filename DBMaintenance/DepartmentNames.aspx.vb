Imports Class1
Partial Class DBMaintenance_DepartmentNames
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    
    Protected Sub AddCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.AddCheckBox.Checked = True Then
            Me.AddPanel.Visible = True
            Me.DeptTextBox.Text = ""
        Else
            Me.AddPanel.Visible = False
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not Me.DeptTextBox.Text = "" Then
            SatiCode.Deparments("Add", Me.DeptTextBox.Text)
            Me.GridView2.DataBind()
            Me.AddPanel.Visible = False
            me.AddCheckBox.Checked = False
        Else
            Me.infoLabel.Visible = True
            Me.infoLabel.Text = "Enter Department Name"
        End If


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub
End Class
