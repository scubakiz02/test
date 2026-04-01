Imports Class1
Partial Class MR_Tools
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub CheckBox1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.CheckBox1.Checked = True Then
            Me.NewToolPanel.Visible = True
            Me.NewToolOnlineDateDateCalendar.SelectedDate = DateTime.Now.ToShortDateString
        Else
            Me.NewToolPanel.Visible = False
            Me.NewToolNameTextBox.Text = ""
            Me.NewToolIDTextBox.Text = ""
            Me.InfoLabel.Visible = False
            Me.GridView1.DataBind()
        End If
    End Sub

    Protected Sub AddToolButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not Me.NewToolNameTextBox.Text = "" Then
            If Not Me.NewToolDeptDropDownList.SelectedItem.Text = "Select One..." Then
                If Me.NewToolIDTextBox.Text = "" Then
                    SatiCode.Tools("Add", Me.NewToolNameTextBox.Text, Me.NewToolDeptDropDownList.SelectedItem.Text, "Not Set", Me.NewToolOnlineDateDateCalendar.SelectedDate)
                Else
                    SatiCode.Tools("Add", Me.NewToolNameTextBox.Text, Me.NewToolDeptDropDownList.SelectedItem.Text, Me.NewToolIDTextBox.Text, Me.NewToolOnlineDateDateCalendar.SelectedDate)
                End If
                Me.CheckBox1.Checked = False
                Me.GridView1.DataBind()
                Me.NewToolPanel.Visible = False
            Else
                Me.InfoLabel.Visible = True
                Me.InfoLabel.Text = "Select Department"
            End If
        Else
            Me.InfoLabel.Visible = True
            Me.InfoLabel.Text = "Enter Tool Name"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("DBMaintenance", Server)
    End Sub
End Class
