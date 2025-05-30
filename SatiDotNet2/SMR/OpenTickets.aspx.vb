Imports Class1
Imports SatiDotNet2.Library

Partial Class SMR_OpenTickets
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim Security As New Security
    Dim SMR_Key As String
    Dim Closed As Boolean
    Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        MenuAuthenication.CheckGroupAuthenication("Maintenance", Server)

        If Not Request.QueryString("SMR_Key") = "" Then
            SMR_Key = Request.QueryString("SMR_Key")
        Else
            SMR_Key = "4058" '"4058"
        End If

        QueryConfig("@SMR_Key") = New Dictionary(Of String, String) From {
            {"value", SMR_Key},
            {"typeOf", "int"}
        }

        If Me.IsPostBack = False Then
            Dim ds As Data.DataSet
            Dim DR As Data.DataRow
            Dim nullcheck As Boolean

            ds = Security.GetMyDataSetParamQuery("SELECT CloseDate FROM dbo.T_SMR_Tickets WHERE SMR_Key=@SMR_Key", QueryConfig) 'SELECT CloseDate FROM dbo.T_SMR_Tickets WHERE (SMR_Key = 53868)
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

        '/SMR/OpenTickets.aspx?SMR_Key=54066&Open=Yes

        'BuildSQL()
    End Sub

    Protected Sub SubmitButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Note As String
        Note = Me.NewNoteTextBox.Text

        If Note.Length > 200 Then
            Me.SubmitLabel.Text = "Sorry, The Note needs to be less then 200 Char. You're At " & Note.Length
        Else
            SatiCode.ScheduledMaintenanceRequestNote(SMR_Key, "Tech", Me.NewNoteTextBox.Text)
            Me.NewNoteTextBox.Text = ""
            Me.SubmitLabel.Text = ""
            BuildSQL()
        End If

    End Sub

    Sub BuildSQL()
        Dim buildTicket As String
        Dim MyDS As Data.DataSet
        Dim MyRow As Data.DataRow
        MyDS = Security.GetMyDataSetParamQuery("SELECT dbo.T_SMR_Tickets.SMR_Key AS Ticket#, dbo.T_Tools.Tool, dbo.T_SMR_TicketNotes.Note, dbo.T_SMR_Tickets.IssueDate, dbo.T_SMR_Tickets.IssueUser, dbo.T_SMR_Tickets.CloseDate, dbo.T_SMR_Tickets.CloseUser, dbo.T_SMR_Tickets.ReportOK FROM dbo.T_SMR_Tickets INNER JOIN dbo.T_Tools ON dbo.T_SMR_Tickets.Tool = dbo.T_Tools.[Key] INNER JOIN dbo.T_SMR_TicketNotes ON dbo.T_SMR_Tickets.SMR_Key = dbo.T_SMR_TicketNotes.SMR_Key WHERE (dbo.T_SMR_TicketNotes.NoteType = 'Org') AND (dbo.T_SMR_Tickets.SMR_Key=@SMR_Key)", QueryConfig)
        MyRow = MyDS.Tables(0).Rows(0)
        Me.LabelSMRNumber.Text = MyRow("Ticket#").ToString
        Me.LabelTool.Text = MyRow("Tool").ToString

        Me.LabelIssueDate.Text = MyRow("IssueDate").ToString
        Me.LabelClosedDate.Text = MyRow("CloseDate").ToString
        Me.LabelIssueUser.Text = MyRow("IssueUser").ToString
        Me.LabelCloseUser.Text = MyRow("CloseUser").ToString
        Me.TextBoxUserNote.Text = MyRow("Note").ToString
        Me.CheckBoxReport.Checked = MyRow("ReportOK")

        NotesSqlDataSource.SelectCommand = "SELECT NoteDate, SatiUser, Note FROM dbo.T_SMR_TicketNotes WHERE (SMR_Key=@SMR_Key) AND (NoteType = 'Tech')"
        NotesSqlDataSource.SelectParameters.Clear()
        NotesSqlDataSource.SelectParameters.Add("SMR_Key", SMR_Key)
        GridView1.DataBind()
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
            SatiCode.ScheduledMaintenanceRequestTicket("Close", SMR_Key, 0)
        Else
            Me.infoLabel.Text = "You need add at least one Field Note!"
        End If

        BuildSQL()
    End Sub


    Protected Sub CheckBoxReport_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxReport.CheckedChanged
        Dim SMR As Integer = Me.LabelSMRNumber.Text
        Dim UpdateQueryConfig As New Dictionary(Of String, Dictionary(Of String, String))(QueryConfig)
        Dim MyVal As Boolean

        If Me.CheckBoxReport.Checked = True Then
            MyVal = True
        Else
            MyVal = False
        End If

        'UpdateQueryConfig inherits '@SMR_Key' parameterized value from QueryConfig
        UpdateQueryConfig("@SMR_Key")("value") = SMR
        UpdateQueryConfig("@ReportOk") = New Dictionary(Of String, String) From {
            {"value", MyVal},
            {"typeOf", "bit"}
        }

        'Security.ExecuteSqlParamQuery("UPDATE dbo.T_SMR_Tickets SET ReportOK=@ReportOK WHERE SMR_Key=@SMR_Key", UpdateQueryConfig) 'UPDATE dbo.T_SMR_Tickets SET ReportOK = 1 WHERE SMR_Key = 4057
        BuildSQL()
    End Sub

    'Protected Sub DropDownListStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListStatus.SelectedIndexChanged
    '    SatiCode.MaintenanceRequestTicket("ModStatus", SMR_Key, 0, Me.DropDownListStatus.SelectedValue.ToString)
    '    SatiCode.SendMail_HTML(Me.LabelTool.Text & " Type changed to " & Me.DropDownListStatus.SelectedValue.ToString & " ticket. User: " & User.Identity.Name.ToString, "SMR " & Me.LabelSMRNumber.Text & " Change", "Tim.Hughes@purewafer.com", "")
    '    BuildSQL()
    'End Sub

End Class
