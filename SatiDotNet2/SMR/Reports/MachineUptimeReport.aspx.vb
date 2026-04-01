Partial Class MR_Reports_MachineUptimeReport
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim TheSQL As String = ""



    Protected Sub CalendarFrom_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CalendarFrom.SelectionChanged
        Me.LabelFromDate.Text = Me.CalendarFrom.SelectedDate.ToShortDateString.ToString
        SeeLabel()
    End Sub

    Protected Sub CalendarTo_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CalendarTo.SelectionChanged
        Me.LabelToDate.Text = Me.CalendarTo.SelectedDate.ToShortDateString.ToString
        SeeLabel()
    End Sub

    Sub SeeLabel()
        If Not Me.LabelFromDate.Text = "" And Not Me.LabelToDate.Text = "" Then
            Me.ButtonRun.Visible = True
            Me.ButtonRun.Text = "Run Report From: " & Me.LabelFromDate.Text & " To: " & Me.LabelToDate.Text
        End If
    End Sub

    Protected Sub ButtonRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRun.Click
        MakeReport()
    End Sub

    Function MakeReport() As String
        Dim S_Date As String = Me.LabelFromDate.Text
        Dim E_Date As String = Me.LabelToDate.Text
        Dim MySQL As String = "SELECT TOP 100 PERCENT dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.T_MR_Tickets.MR_Key, dbo.T_MR_Tickets.IssueDate, dbo.T_MR_Tickets.CloseDate, ISNULL(DATEDIFF(mi, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) THEN CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, dbo.T_MR_Tickets.CloseDate), DATEDIFF(mi, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) THEN CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, { fn NOW() })) AS MinutesDown, ISNULL(DATEDIFF(hh, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) THEN CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, dbo.T_MR_Tickets.CloseDate), DATEDIFF(hh, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) THEN CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, { fn NOW() })) AS HoursDown, ISNULL(DATEDIFF(DD, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) THEN CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, dbo.T_MR_Tickets.CloseDate), DATEDIFF(DD, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) THEN CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, { fn NOW() })) AS DaysDown, dbo.T_MR_Tickets.Status, dbo.T_MR_Tickets.ReportOK FROM dbo.T_MR_GroupLists INNER JOIN dbo.T_Tools ON dbo.T_MR_GroupLists.ToolKey = dbo.T_Tools.[Key] INNER JOIN dbo.T_MR_Tickets ON dbo.T_Tools.[Key] = dbo.T_MR_Tickets.Tool WHERE (dbo.T_MR_GroupLists.ListName = 'TerryUpTimeReport') AND (dbo.T_MR_Tickets.IssueDate > CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102)) AND (dbo.T_MR_Tickets.CloseDate < CONVERT(DATETIME, '" & E_Date & " 00:00:00', 102) OR dbo.T_MR_Tickets.CloseDate IS NULL) AND (dbo.T_MR_Tickets.Status = 'Down') OR (dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102)) AND (dbo.T_MR_Tickets.CloseDate > CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102)) AND (dbo.T_MR_Tickets.Status = 'down') AND (dbo.T_MR_Tickets.ReportOK = 1) ORDER BY dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.T_MR_Tickets.IssueDate"
        Me.Labelsql.Text = MySQL

        'SELECT TOP 100 PERCENT dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.T_MR_Tickets.MR_Key, dbo.T_MR_Tickets.IssueDate, dbo.T_MR_Tickets.CloseDate, ISNULL(DATEDIFF(mi, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '2015-03-01 00:00:00', 102) THEN CONVERT(DATETIME, '2015-03-01 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, dbo.T_MR_Tickets.CloseDate), DATEDIFF(mi, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '2015-03-01 00:00:00', 102) THEN CONVERT(DATETIME, '2015-03-01 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, { fn NOW() })) AS MinutesDown, ISNULL(DATEDIFF(hh, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '2015-03-01 00:00:00', 102) THEN CONVERT(DATETIME, '2015-03-01 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, dbo.T_MR_Tickets.CloseDate), DATEDIFF(hh, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '2015-03-01 00:00:00', 102) THEN CONVERT(DATETIME, '2015-03-01 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, { fn NOW() })) AS HoursDown, ISNULL(DATEDIFF(DD, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '2015-03-01 00:00:00', 102) THEN CONVERT(DATETIME, '2015-03-01 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, dbo.T_MR_Tickets.CloseDate), DATEDIFF(DD, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '2015-03-01 00:00:00', 102) THEN CONVERT(DATETIME, '2015-03-01 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, { fn NOW() })) AS DaysDown, dbo.T_MR_Tickets.Status, dbo.T_MR_Tickets.ReportOK FROM dbo.T_MR_GroupLists INNER JOIN dbo.T_Tools ON dbo.T_MR_GroupLists.ToolKey = dbo.T_Tools.[Key] INNER JOIN dbo.T_MR_Tickets ON dbo.T_Tools.[Key] = dbo.T_MR_Tickets.Tool WHERE (dbo.T_MR_GroupLists.ListName = 'TerryUpTimeReport') AND (dbo.T_MR_Tickets.IssueDate > CONVERT(DATETIME, '2015-03-01 00:00:00', 102)) AND (dbo.T_MR_Tickets.CloseDate < CONVERT(DATETIME, '2015-04-01 00:00:00', 102) OR dbo.T_MR_Tickets.CloseDate IS NULL) OR (dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '2015-03-01 00:00:00', 102)) AND (dbo.T_MR_Tickets.CloseDate > CONVERT(DATETIME, '2015-03-01 00:00:00', 102)) ORDER BY dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.T_MR_Tickets.IssueDate
        Me.SqlDataSource_MR_By_Date_Range.SelectCommand = MySQL
        Me.GridView1.DataBind()

    End Function

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "ViewMR" Then
            Dim row As String = e.CommandArgument.ToString
            Dim WatKey As String = Me.GridView1.Rows(row).Cells(3).Text
            OpenNewPage(Me.UpdatePanel1, "http://PWI-40:81/MR/OpenTickets.aspx?MR_Key=" & WatKey) '& "&Open=Yes")
        End If
    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub

    Protected Sub SqlDataSource_MR_By_Date_Range_Updating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceCommandEventArgs) Handles SqlDataSource_MR_By_Date_Range.Updating
        e.Command.Parameters("@C_Date").Value = e.Command.Parameters("@CloseDate").Value
        e.Command.Parameters("@R_OK").Value = e.Command.Parameters("@ReportOK").Value
        e.Command.Parameters.Remove(e.Command.Parameters("@CloseDate"))
        e.Command.Parameters.Remove(e.Command.Parameters("@ReportOK"))
        '@CloseDate
        '@ReportOK
    End Sub

    Sub OpenNewPage(ByVal MyUpdatePanel As UpdatePanel, ByVal TheWebPage As String)
        Dim docGuid As String = Guid.NewGuid().ToString()
        Dim sb As StringBuilder = New StringBuilder("")
        Dim strRoot As String
        strRoot = Request.Url.GetLeftPart(UriPartial.Authority)
        sb.Append("window.open('" & TheWebPage & "');")
        ScriptManager.RegisterClientScriptBlock(MyUpdatePanel, MyUpdatePanel.GetType(), "NewClientScript", sb.ToString(), True)
    End Sub


    Protected Sub ButonExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButonExport.Click
        Dim MySQL As String = Me.Labelsql.Text
        'MySQL = "SELECT TOP 100 PERCENT dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.T_MR_Tickets.MR_Key, dbo.T_MR_Tickets.IssueDate, dbo.T_MR_Tickets.CloseDate, ISNULL(DATEDIFF(mi, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) THEN CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, dbo.T_MR_Tickets.CloseDate), DATEDIFF(mi, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) THEN CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, { fn NOW() })) AS MinutesDown, ISNULL(DATEDIFF(hh, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) THEN CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, dbo.T_MR_Tickets.CloseDate), DATEDIFF(hh, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) THEN CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, { fn NOW() })) AS HoursDown, ISNULL(DATEDIFF(DD, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) THEN CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, dbo.T_MR_Tickets.CloseDate), DATEDIFF(DD, CASE WHEN dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) THEN CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102) ELSE dbo.T_MR_Tickets.IssueDate END, { fn NOW() })) AS DaysDown, dbo.T_MR_Tickets.Status, dbo.T_MR_Tickets.ReportOK FROM dbo.T_MR_GroupLists INNER JOIN dbo.T_Tools ON dbo.T_MR_GroupLists.ToolKey = dbo.T_Tools.[Key] INNER JOIN dbo.T_MR_Tickets ON dbo.T_Tools.[Key] = dbo.T_MR_Tickets.Tool WHERE (dbo.T_MR_GroupLists.ListName = 'TerryUpTimeReport') AND (dbo.T_MR_Tickets.IssueDate > CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102)) AND (dbo.T_MR_Tickets.CloseDate < CONVERT(DATETIME, '" & E_Date & " 00:00:00', 102) OR dbo.T_MR_Tickets.CloseDate IS NULL) AND (dbo.T_MR_Tickets.Status = 'Down') AND (dbo.T_MR_Tickets.ReportOK = 1) OR (dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102)) AND (dbo.T_MR_Tickets.CloseDate > CONVERT(DATETIME, '" & S_Date & " 00:00:00', 102)) ORDER BY dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.T_MR_Tickets.IssueDate"
        SatiCode.ExportMRReport(MySQL)

    End Sub
End Class
