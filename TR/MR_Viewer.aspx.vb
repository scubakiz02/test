
Partial Class MR_MR_Viewer
    Inherits System.Web.UI.Page
    Dim SQL As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        MenuAuthenication.CheckGroupAuthenication("EditRoles", Server)

        If Page.IsPostBack = False Then
            Me.TextBoxStartDate.Text = DateAndTime.DateAdd(DateInterval.Day, -14, CDate(DateAndTime.Now.ToShortDateString))
            Me.TextBoxEndDate.Text = DateAndTime.Now.ToShortDateString

        End If
        'Look_For_SG_Tags()
        BuildSQL()

    End Sub

    Sub Look_For_SG_Tags()
        '"SELECT T_Tool_SubGroup_Tag_Names.SG_Name, T_Tool_SubGroup_Tag_Names.SB_Tag FROM T_IT_Tools INNER JOIN T_Tool_SubGroup_Tag_Names ON T_IT_Tools.[Key] = T_Tool_SubGroup_Tag_Names.Tool_Key WHERE (T_IT_Tools.Tool = 'CMP 1')"
        Me.PanelSGT.Visible = True
        Me.SqlDataSource_SGN.SelectCommand = "SELECT T_Tool_SubGroup_Tag_Names.SG_Name, T_Tool_SubGroup_Tag_Names.SB_Tag FROM T_IT_Tools INNER JOIN T_Tool_SubGroup_Tag_Names ON T_IT_Tools.[Key] = T_Tool_SubGroup_Tag_Names.Tool_Key WHERE (T_IT_Tools.Tool = '" & Me.DropDownListTools.SelectedItem.Text & "')"
        Me.CheckBoxList_SGL.DataBind()
        If CheckBoxList_SGL.Items.Count > 0 Then

        Else
            Me.PanelSGT.Visible = False
        End If
    End Sub

    Sub BuildSQL()
        Dim Build As String
        Dim Howmany As Int16 = 0
        Dim StartDate As String = Me.TextBoxStartDate.Text
        Dim EndDate As String = Me.TextBoxEndDate.Text
        Dim SortOrder As String = " DESC"
        Dim ToolSort As String = ""
        Dim H As Boolean = False

        If Me.CheckBoxToolOnly.Checked = True Then
            ToolSort = " AND (dbo.T_IT_Tools.Tool = '" & Me.DropDownListTools.SelectedItem.Text & "')"
        End If


        'Select
        Build = "SELECT dbo.T_ITR_Tickets.MR_Key AS Ticket#, dbo.T_IT_Tools.Department, dbo.T_IT_Tools.Tool, dbo.T_ITR_Tickets.Status, dbo.T_ITR_TicketNotes.Note, dbo.T_ITR_Tickets.IssueDate FROM dbo.T_ITR_Tickets INNER JOIN dbo.T_IT_Tools ON dbo.T_ITR_Tickets.Tool = dbo.T_IT_Tools.[Key] INNER JOIN dbo.T_ITR_TicketNotes ON dbo.T_ITR_Tickets.MR_Key = dbo.T_ITR_TicketNotes.MR_Key "

        'Where
        Build = Build & "WHERE "
        '(dbo.T_ITR_Tickets.CloseDate IS NULL) AND (dbo.T_ITR_TicketNotes.NoteType = 'Org')"
        If Me.OpenTicketsRadioButton.Checked Then
            Build = Build & "(dbo.T_ITR_Tickets.CloseDate IS NULL) AND (dbo.T_ITR_TicketNotes.NoteType = 'Org')" & ToolSort ' AND (dbo.T_ITR_Tickets.IssueDate > CONVERT(DATETIME, '" & StartDate & " 00:00:00', 102) AND dbo.T_ITR_Tickets.IssueDate < CONVERT(DATETIME, '" & EndDate & " 00:00:00', 102))"
        Else
            Build = Build & "(dbo.T_ITR_Tickets.CloseDate IS NOT NULL) AND (dbo.T_ITR_TicketNotes.NoteType = 'Org') AND (dbo.T_ITR_Tickets.IssueDate > CONVERT(DATETIME, '" & StartDate & " 00:00:00', 102) AND dbo.T_ITR_Tickets.IssueDate < CONVERT(DATETIME, '" & EndDate & " 23:59:59', 102))" & ToolSort
        End If

        'GROUP BY
        Build = Build & " GROUP BY dbo.T_ITR_Tickets.MR_Key, dbo.T_IT_Tools.Department, dbo.T_IT_Tools.Tool, dbo.T_ITR_Tickets.Status, dbo.T_ITR_TicketNotes.Note, dbo.T_ITR_Tickets.IssueDate "

        'HAVING
        If Me.PanelSGT.Visible = True Then
            If Me.CheckBoxList_SGL.Items.Count > 0 Then
                For i As Int16 = 0 To Me.CheckBoxList_SGL.Items.Count - 1
                    If Me.CheckBoxList_SGL.Items(i).Selected = True Then
                        If H = False Then
                            H = True
                            Build = Build & "HAVING ("
                            Build = Build & "dbo.T_ITR_TicketNotes.Note LIKE '%< " & Me.CheckBoxList_SGL.Items(i).Value & " >%'"
                        Else
                            Build = Build & " OR dbo.T_ITR_TicketNotes.Note LIKE '%< " & Me.CheckBoxList_SGL.Items(i).Value & " >%'"
                        End If
                    End If
                Next
                If H = True Then
                    Build = Build & ") "
                End If
            End If
        End If


        Build = Build & "ORDER BY dbo.T_ITR_Tickets.IssueDate" & SortOrder

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

            Response.Redirect("OpenTickets.aspx?MR_Key=" & MR_Key & "&Open=" & Open)
        End If
    End Sub

    Protected Sub ButtonRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRefresh.Click
        BuildSQL()
    End Sub

    Protected Sub CheckBoxToolOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxToolOnly.CheckedChanged
        BuildSQL()
    End Sub

    Protected Sub DropDownListTools_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListTools.SelectedIndexChanged
        Look_For_SG_Tags()
        If Me.CheckBoxToolOnly.Checked = True Then
            BuildSQL()
        End If

    End Sub

    Protected Sub CheckBoxList_SGL_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckBoxList_SGL.SelectedIndexChanged
        BuildSQL()
    End Sub

End Class

