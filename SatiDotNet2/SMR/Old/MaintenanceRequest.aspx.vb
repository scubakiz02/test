Imports Class1
Partial Class MR_MaintenanceRequest
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub departmentDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ToolSqlDataSource.SelectCommand = "SELECT Tool, [Key] FROM dbo.T_Tools WHERE (Department = '" & Me.DepartmentDropDownList.SelectedItem.Text & "') ORDER BY Tool"
        Me.ToolDropDownList.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If User.Identity.IsAuthenticated = False Then
        'Server.Transfer("~/Login.aspx")
        'End If
    End Sub

    Sub Submit()
        Dim tool As String
        Dim status As String = ""
        Dim TicketNumber As String = ""
        Dim TheMail As String = ""
        Dim TheSubject As String = ""

        If Me.ToolDropDownList.SelectedValue.ToString = "" Then
            Me.infoLabel.Text = "Select a Tool"
            Exit Sub
        End If
        tool = Me.ToolDropDownList.SelectedValue.ToString

        If Me.StatusDownRadioButton.Checked = True Then
            status = "Down"
        Else
            status = "Standard"
        End If
        If Me.ProblemTextBox.Text = "" Then
            Me.infoLabel.Text = "Describe Problem"
            Exit Sub
        End If


        TicketNumber = SatiCode.MaintenanceRequestTicket("New", "0", tool, status)
        Try
            SatiCode.MaintenanceRequestNote(TicketNumber, "Org", Me.ProblemTextBox.Text)

            TheMail = "Sati.Net has received a Maintenance Request from " & User.Identity.Name.ToString & " In the " _
            & Me.DepartmentDropDownList.SelectedItem.Text.ToString & " Department. " & Chr(13) & Chr(13) _
            & "The " & Me.ToolDropDownList.SelectedItem.Text.ToString & " has the following problem. " & Chr(13) _
            & Me.ProblemTextBox.Text & Chr(13) & Chr(13) & "The Maintenance Request is under Ticket Number: " & TicketNumber

            If status = "Down" Then
                TheSubject = "Tool: " & Me.ToolDropDownList.SelectedItem.Text.ToString & " Is Down! Ticket Number: " & TicketNumber
            Else
                TheSubject = "Maintenance Request Issued. Ticket Number: " & TicketNumber
            End If
            Me.Button1.Enabled = False
            'SatiCode.SendMail_HTML(TheMail, TheSubject, "AZ.SatiMaintenanceRequest@purewafer.com", "AZ.SatiMaintenanceRequest@purewafer.com")
            'SatiCode.SendMail_MaintenRequest(TheMail, TheSubject, "New", TicketNumber)
            SatiCode.SendMail_HTML(TheMail, TheSubject, "AZ.SatiMaintenanceRequest@purewafer.com", "AZ.SatiMaintenanceRequest@purewafer.com")

            Me.infoLabel.Text = "Your Request Was Submited. Your Ticket Number is " & TicketNumber
        Catch ex As Exception
            Me.infoLabel.Text = "Error, Contact Your Sati.Net Admin"
        End Try

    End Sub


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Submit()
    End Sub
End Class
