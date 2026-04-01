
Imports System.Diagnostics
Imports SatiDotNet2.Library
Imports System.Data

Partial Class MR_MR_Viewer
    Inherits System.Web.UI.Page
    Dim SQL As String = ""
    Dim Security As New Security()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        If Page.IsPostBack = False Then
            Me.TextBoxStartDate.Text = DateAndTime.DateAdd(DateInterval.Day, -14, CDate(DateAndTime.Now.ToShortDateString))
            Me.TextBoxEndDate.Text = DateAndTime.Now.ToShortDateString

        End If
        'Look_For_SG_Tags()

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'DataBind on GridView1 must occur within PreRender
        'TicketsSqlDataSource UpdateCommand does receives original values, rather than the new values, if DataBind is called in Page_Load
        BuildSQL()
    End Sub

    Sub Look_For_SG_Tags()
        '"SELECT T_Tool_SubGroup_Tag_Names.SG_Name, T_Tool_SubGroup_Tag_Names.SB_Tag FROM T_Tools INNER JOIN T_Tool_SubGroup_Tag_Names ON T_Tools.[Key] = T_Tool_SubGroup_Tag_Names.Tool_Key WHERE (T_Tools.Tool = 'CMP 1')"
        Me.PanelSGT.Visible = True
        Me.SqlDataSource_SGN.SelectCommand = "SELECT T_Tool_SubGroup_Tag_Names.SG_Name, T_Tool_SubGroup_Tag_Names.SB_Tag FROM T_Tools INNER JOIN T_Tool_SubGroup_Tag_Names ON T_Tools.[Key] = T_Tool_SubGroup_Tag_Names.Tool_Key WHERE (T_Tools.Tool = '" & Me.DropDownListTools.SelectedItem.Text & "')"
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

        TicketsSqlDataSource.SelectParameters.Clear() ' Clear any existing parameters

        If Me.CheckBoxToolOnly.Checked = True Then
            ToolSort = " AND (dbo.T_Tools.Tool = @Tool)"
            TicketsSqlDataSource.SelectParameters.Add("Tool", TypeCode.String, Me.DropDownListTools.SelectedItem.Text)
        End If

        'Select
        Build = "SELECT dbo.T_SMR_TicketNotes.[Key] AS TicketNoteID, dbo.T_SMR_Tickets.SMR_Key AS TicketID, dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.T_SMR_TicketNotes.Note, FORMAT(dbo.T_SMR_Tickets.IssueDate, 'MM/dd/yyyy') As IssueDate, dbo.T_SMR_Tickets.EstimatedHrs, FORMAT(dbo.T_SMR_Tickets.EarliestStartTime, 'MM/dd/yyyy') As EarliestStartTime, FORMAT(dbo.T_SMR_Tickets.ScheduledStartTime, 'MM/dd/yyyy') As ScheduledStartTime, dbo.T_SMR_Tickets.OrderParts FROM dbo.T_SMR_Tickets INNER JOIN dbo.T_Tools ON dbo.T_SMR_Tickets.Tool = dbo.T_Tools.[Key] INNER JOIN dbo.T_SMR_TicketNotes ON dbo.T_SMR_Tickets.SMR_Key = dbo.T_SMR_TicketNotes.SMR_Key "

        'Where
        Build = Build & "WHERE "
        '(dbo.T_SMR_Tickets.CloseDate IS NULL) AND (dbo.T_SMR_TicketNotes.NoteType = 'Org')"
        If Me.OpenTicketsRadioButton.Checked Then
            Build = Build & "(dbo.T_SMR_Tickets.CloseDate IS NULL) AND (dbo.T_SMR_TicketNotes.NoteType = 'Org')" & ToolSort ' AND (dbo.T_SMR_Tickets.IssueDate > CONVERT(DATETIME, '" & StartDate & " 00:00:00', 102) AND dbo.T_SMR_Tickets.IssueDate < CONVERT(DATETIME, '" & EndDate & " 00:00:00', 102))"
        Else
            Build = Build & "(dbo.T_SMR_Tickets.CloseDate IS NOT NULL) AND (dbo.T_SMR_TicketNotes.NoteType = 'Org') AND (dbo.T_SMR_Tickets.IssueDate > CONVERT(DATETIME, @StartDate, 102) AND dbo.T_SMR_Tickets.IssueDate < CONVERT(DATETIME, @EndDate, 102))" & ToolSort
            TicketsSqlDataSource.SelectParameters.Add("StartDate", TypeCode.String, StartDate & " 00:00:00")
            TicketsSqlDataSource.SelectParameters.Add("EndDate", TypeCode.String, EndDate & " 23:59:59")
        End If

        ''GROUP BY
        'Build = Build & " GROUP BY dbo.T_SMR_TicketNotes.[Key], dbo.T_SMR_Tickets.SMR_Key, dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.T_SMR_TicketNotes.Note, dbo.T_SMR_Tickets.IssueDate, dbo.T_SMR_Tickets.EstimatedHrs, dbo.T_SMR_Tickets.EarliestStartTime, dbo.T_SMR_Tickets.ScheduledStartTime "

        ''HAVING
        'If Me.PanelSGT.Visible = True Then
        '    If Me.CheckBoxList_SGL.Items.Count > 0 Then
        '        For i As Int16 = 0 To Me.CheckBoxList_SGL.Items.Count - 1
        '            If Me.CheckBoxList_SGL.Items(i).Selected = True Then
        '                If H = False Then
        '                    H = True
        '                    Build = Build & "HAVING ("
        '                    Build = Build & "dbo.T_SMR_TicketNotes.Note LIKE '%< " & Me.CheckBoxList_SGL.Items(i).Value & " >%'"
        '                Else
        '                    Build = Build & " OR dbo.T_SMR_TicketNotes.Note LIKE '%< " & Me.CheckBoxList_SGL.Items(i).Value & " >%'"
        '                End If
        '            End If
        '        Next
        '        If H = True Then
        '            Build = Build & ") "
        '        End If
        '    End If
        'End If



        Build = Build & "ORDER BY dbo.T_SMR_Tickets.IssueDate" & SortOrder

        SQL = Build

        TicketsSqlDataSource.SelectCommand = SQL

        GridView1.DataBind()

        'Me.TicketsSqlDataSource.SelectCommand = SQL
        'Me.GridView1.DataBind()
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
            Dim SMR_Key As String = Me.GridView1.Rows(row).Cells(1).Text
            Dim Open As String
            If Me.OpenTicketsRadioButton.Checked = True Then
                Open = "Yes"
            Else
                Open = "No"
            End If
            'Response.Redirect("~/MR/OpenTickets.aspx?SMR_Key=" & SMR_Key & "&Open=" & Open)
            'http://PWI-40:81/MR/MR_View.aspx


            'OpenNewPage(Me.UpdatePanel1, "http://PWI-40:81/MR/OpenTickets.aspx?SMR_Key=" & SMR_Key & "&Open=" & Open)

            OpenNewPage(Me.UpdatePanel1, "OpenTickets.aspx?SMR_Key=" & SMR_Key & "&Open=" & Open)

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

    Protected Sub CheckBoxToolOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxToolOnly.CheckedChanged
        BuildSQL()
    End Sub

    Protected Sub DropDownListTools_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListTools.SelectedIndexChanged
        'Look_For_SG_Tags()
        If Me.CheckBoxToolOnly.Checked = True Then
            BuildSQL()
        End If

    End Sub

    Protected Sub CheckBoxList_SGL_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckBoxList_SGL.SelectedIndexChanged
        BuildSQL()
    End Sub

    Private Function TextModeDate(DateStr As String) As String
        Try 'format expected by HTML5 <input type="date">
            Return Date.Parse(DateStr).ToString("yyyy-MM-dd")
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Row As GridViewRow = e.Row

            If e.Row.RowState.HasFlag(DataControlRowState.Edit) Then
                'row has entered edit mode
                'sql query converts EarliestStartTime & ScheduledStartTime to 'MM/dd/yyyy' format using FORMAT sql function
                'convert date to TextBox control TextMode="Date" compatible format 'yyyy-MM-dd'
                Dim EarliestStartTime_TextBox As TextBox = CType(Row.FindControl("EarliestStartTime_TextBox"), TextBox)
                Dim ScheduledStartTime_TextBox As TextBox = CType(Row.FindControl("ScheduledStartTime_TextBox"), TextBox)

                EarliestStartTime_TextBox.Text = TextModeDate(EarliestStartTime_TextBox.Text)
                ScheduledStartTime_TextBox.Text = TextModeDate(ScheduledStartTime_TextBox.Text)
            Else
                'Order Parts? column cell ddl & modal magic
                Dim OrderParts_Label As Label = CType(Row.FindControl("OrderParts_Label"), Label)

                If OrderParts_Label.Text = "True" Then
                    CType(Row.FindControl("OrderParts_ImageButton"), ImageButton).Visible = True
                End If
            End If

        End If
    End Sub
End Class

