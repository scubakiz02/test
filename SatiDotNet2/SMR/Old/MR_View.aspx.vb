
Partial Class MR_MR_View
    Inherits System.Web.UI.Page
    Dim SQL As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        If Page.IsPostBack = False Then
            Me.TextBoxStartDate.Text = DateAndTime.DateAdd(DateInterval.Day, -14, CDate(DateAndTime.Now.ToShortDateString))
            Me.TextBoxEndDate.Text = DateAndTime.Now.ToShortDateString
        End If

        BuildSQL()

    End Sub

    Sub BuildSQL()
        Dim Build As String
        Dim Howmany As Int16 = 0
        Dim StartDate As String = Me.TextBoxStartDate.Text
        Dim EndDate As String = Me.TextBoxEndDate.Text
        Dim SortOrder As String = ""
        Dim ToolSort As String = ""

        If CheckBoxSort.Checked = True Then
            SortOrder = " DESC"
        End If

        If Me.CheckBoxToolOnly.Checked = True Then
            ToolSort = " AND (dbo.T_Tools.Tool = '" & Me.DropDownListTools.SelectedItem.Text & "')"
        End If



        'Select
        Build = "SELECT dbo.T_MR_Tickets.MR_Key AS Ticket#, dbo.T_Tools.Tool, dbo.T_MR_Tickets.Status, dbo.T_MR_TicketNotes.Note, dbo.T_MR_Tickets.IssueDate FROM dbo.T_MR_Tickets INNER JOIN dbo.T_Tools ON dbo.T_MR_Tickets.Tool = dbo.T_Tools.[Key] INNER JOIN dbo.T_MR_TicketNotes ON dbo.T_MR_Tickets.MR_Key = dbo.T_MR_TicketNotes.MR_Key "




        'Where
        Build = Build & "WHERE "
        '(dbo.T_MR_Tickets.CloseDate IS NULL) AND (dbo.T_MR_TicketNotes.NoteType = 'Org')"
        If Me.OpenTicketsRadioButton.Checked Then
            Build = Build & "(dbo.T_MR_Tickets.CloseDate IS NULL) AND (dbo.T_MR_TicketNotes.NoteType = 'Org')" & ToolSort ' AND (dbo.T_MR_Tickets.IssueDate > CONVERT(DATETIME, '" & StartDate & " 00:00:00', 102) AND dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & EndDate & " 00:00:00', 102))"
        Else
            Build = Build & "(dbo.T_MR_Tickets.CloseDate IS NOT NULL) AND (dbo.T_MR_TicketNotes.NoteType = 'Org') AND (dbo.T_MR_Tickets.IssueDate > CONVERT(DATETIME, '" & StartDate & " 00:00:00', 102) AND dbo.T_MR_Tickets.IssueDate < CONVERT(DATETIME, '" & EndDate & " 23:59:59', 102))" & ToolSort
        End If

        'GROUP BY
        Build = Build & " GROUP BY dbo.T_MR_Tickets.MR_Key, dbo.T_Tools.Tool, dbo.T_MR_Tickets.Status, dbo.T_MR_TicketNotes.Note, dbo.T_MR_Tickets.IssueDate "


        'HAVING
        Build = Build & "HAVING "
        If Me.DownCheckBox.Checked = True Then
            If Howmany = 0 Then
                Build = Build & "(dbo.T_MR_Tickets.Status = 'Down')"
            Else
                Build = Build & " OR (dbo.T_MR_Tickets.Status = 'Down')"
            End If
            Howmany = Howmany + 1
        End If

        If Me.StandardCheckBox.Checked = True Then
            If Howmany = 0 Then
                Build = Build & "(dbo.T_MR_Tickets.Status = 'Standard')"
            Else
                Build = Build & " OR (dbo.T_MR_Tickets.Status = 'Standard')"
            End If
            Howmany = Howmany + 1
        End If

        If Me.ScheduledCheckBox.Checked = True Then
            If Howmany = 0 Then
                Build = Build & "(dbo.T_MR_Tickets.Status = 'Shedule')"
            Else
                Build = Build & " OR (dbo.T_MR_Tickets.Status = 'Shedule')"
            End If
            Howmany = Howmany + 1
        End If
        If Howmany = 0 Then
            Build = Build & "(dbo.T_MR_Tickets.Status = '')"
        End If

        Build = Build & "ORDER BY dbo.T_MR_Tickets.IssueDate" & SortOrder

        SQL = Build
        Me.TicketsSqlDataSource.SelectCommand = SQL
        Me.GridView1.DataBind()
    End Sub

    Protected Sub OpenTicketsRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        BuildSQL()
    End Sub

    Protected Sub ClosedRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        BuildSQL()
    End Sub

    Protected Sub DownCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        BuildSQL()
    End Sub

    Protected Sub StandardCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        BuildSQL()
    End Sub

    Protected Sub ScheduledCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        BuildSQL()
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "Select" Then
            Dim row As String = e.CommandArgument.ToString
            Dim MR_Key As String = Me.GridView1.Rows(row).Cells(1).Text
            Dim Open As String
            If Me.OpenTicketsRadioButton.Checked = True Then
                Open = "Yes"
            Else
                Open = "No"
            End If
            'Response.Redirect("~/MR/OpenTickets.aspx?MR_Key=" & MR_Key & "&Open=" & Open)
            'http://PWI-40:81/MR/MR_View.aspx

            OpenNewPage(Me.UpdatePanel1, "http://PWI-40:81/MR/OpenTickets.aspx?MR_Key=" & MR_Key & "&Open=" & Open)
        End If
    End Sub

    Sub OpenNewPage(ByVal MyUpdatePanel As UpdatePanel, ByVal TheWebPage As String)
        Dim docGuid As String = Guid.NewGuid().ToString()
        Dim sb As StringBuilder = New StringBuilder("")
        Dim strRoot As String
        strRoot = Request.Url.GetLeftPart(UriPartial.Authority)
        sb.Append("window.open('" & TheWebPage & "');")
        ScriptManager.RegisterClientScriptBlock(MyUpdatePanel, MyUpdatePanel.GetType(), "NewClientScript", sb.ToString(), True)
    End Sub

    Protected Sub ButtonRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRefresh.Click
        BuildSQL()
    End Sub

    Protected Sub CheckBoxSort_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxSort.CheckedChanged
        BuildSQL()
    End Sub

    Protected Sub CheckBoxToolOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxToolOnly.CheckedChanged
        BuildSQL()
    End Sub

    Protected Sub DropDownListTools_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListTools.SelectedIndexChanged
        If Me.CheckBoxToolOnly.Checked = True Then
            BuildSQL()
        End If

    End Sub
End Class
