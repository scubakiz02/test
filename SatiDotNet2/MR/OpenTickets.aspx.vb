Imports Class1

Partial Class MR_OpenTickets
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim MR_Key As String
    Dim Closed As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        MenuAuthenication.CheckGroupAuthenication("Maintenance", Server)



        If Not Request.QueryString("MR_Key") = "" Then
            MR_Key = Request.QueryString("MR_Key")
        Else
            MR_Key = "4058" '"4058"
        End If

        If Me.IsPostBack = False Then
            Dim ds As Data.DataSet
            Dim DR As Data.DataRow
            Dim nullcheck As Boolean

            ds = SatiCode.GetMyDataSet("SELECT CloseDate FROM dbo.T_MR_Tickets WHERE (MR_Key = " & MR_Key & ")") 'SELECT CloseDate FROM dbo.T_MR_Tickets WHERE (MR_Key = 53868)
            DR = ds.Tables(0).Rows(0)
            nullcheck = IsDBNull(DR("CloseDate"))
            If nullcheck = True Or DR("CloseDate").ToString = "" Then
                Me.CloseButton.Visible = True
                Closed = False
            End If

            If Request.QueryString("Open") = "Yes" Then

            End If

            BuildSQL()
        End If

        '/MR/OpenTickets.aspx?MR_Key=54066&Open=Yes

        'BuildSQL()
    End Sub

    Protected Sub SubmitButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Note As String
        Note = Me.NewNoteTextBox.Text

        If Note.Length > 200 Then
            Me.SubmitLabel.Text = "Sorry, The Note needs to be less then 200 Char. Your At " & Note.Length
        Else
            SatiCode.MaintenanceRequestNote(MR_Key, "Tech", Me.NewNoteTextBox.Text)
            Me.NewNoteTextBox.Text = ""
            Me.SubmitLabel.Text = ""
            BuildSQL()
        End If

    End Sub

    Sub BuildSQL()

        Dim buildTicket As String
        Dim MyDS As Data.DataSet
        Dim MyRow As Data.DataRow
        MyDS = SatiCode.GetMyDataSet("SELECT dbo.T_MR_Tickets.MR_Key AS Ticket#, dbo.T_Tools.Tool, dbo.T_MR_Tickets.Status, dbo.T_MR_TicketNotes.Note, dbo.T_MR_Tickets.IssueDate, dbo.T_MR_Tickets.IssueUser, dbo.T_MR_Tickets.CloseDate, dbo.T_MR_Tickets.CloseUser, dbo.T_MR_Tickets.ReportOK FROM dbo.T_MR_Tickets INNER JOIN dbo.T_Tools ON dbo.T_MR_Tickets.Tool = dbo.T_Tools.[Key] INNER JOIN dbo.T_MR_TicketNotes ON dbo.T_MR_Tickets.MR_Key = dbo.T_MR_TicketNotes.MR_Key WHERE (dbo.T_MR_TicketNotes.NoteType = 'Org') AND (dbo.T_MR_Tickets.MR_Key = '" & MR_Key & "')")
        MyRow = MyDS.Tables(0).Rows(0)
        Me.LabelMRNumber.Text = MyRow("Ticket#").ToString
        Me.LabelTool.Text = MyRow("Tool").ToString
        Me.LabelStatus.Text = MyRow("Status").ToString

        'Me.DropDownListStatus.SelectedValue = MyRow("Status").ToString

        Me.DropDownListStatus.SelectedIndex = Me.DropDownListStatus.Items.IndexOf(Me.DropDownListStatus.Items.FindByValue(MyRow("Status").ToString))

        Me.LabelIssueDate.Text = MyRow("IssueDate").ToString
        Me.LabelClosedDate.Text = MyRow("CloseDate").ToString
        Me.LabelIssueUser.Text = MyRow("IssueUser").ToString
        Me.LabelCloseUser.Text = MyRow("CloseUser").ToString
        Me.TextBoxUserNote.Text = MyRow("Note").ToString
        Me.CheckBoxReport.Checked = MyRow("ReportOK")

        Dim BuildNotes As String
        BuildNotes = "SELECT NoteDate, SatiUser, Note FROM dbo.T_MR_TicketNotes WHERE (MR_Key = '" & MR_Key & "') AND (NoteType = 'Tech')"
        Me.NotesSqlDataSource.SelectCommand = BuildNotes
        Me.GridView1.DataBind()



    End Sub

    Protected Sub CloseButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CloseButton.Click

        If Closed = True Then
            Exit Sub
        End If

        If Me.GridView1.Rows.Count > 0 Then
            Closed = True
            Me.CloseButton.Visible = False

            Me.infoLabel.Text = ""
            'BuildSQL()
            SatiCode.MaintenanceRequestTicket("Close", MR_Key, 0, "")
        Else
            Me.infoLabel.Text = "You need add at least one Feild Note!"
        End If

        BuildSQL()
    End Sub


    Protected Sub CheckBoxReport_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxReport.CheckedChanged
        Dim MR As Integer = Me.LabelMRNumber.Text
        Dim MyVal As Int16
        If Me.CheckBoxReport.Checked = True Then
            MyVal = 0
        Else
            MyVal = 1
        End If

        SatiCode.GetMyDataSet("UPDATE dbo.T_MR_Tickets SET ReportOK = " & MyVal & " WHERE MR_Key = " & MR) 'UPDATE dbo.T_MR_Tickets SET ReportOK = 1 WHERE MR_Key = 4057
        BuildSQL()
    End Sub

    Protected Sub DropDownListStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListStatus.SelectedIndexChanged
        SatiCode.MaintenanceRequestTicket("ModStatus", MR_Key, 0, Me.DropDownListStatus.SelectedValue.ToString)
        SatiCode.SendMail_HTML(Me.LabelTool.Text & " Type changed to " & Me.DropDownListStatus.SelectedValue.ToString & " ticket. User: " & User.Identity.Name.ToString, "MR " & Me.LabelMRNumber.Text & " Change", "Tim.Hughes@purewafer.com", "")
        BuildSQL()
    End Sub

End Class
