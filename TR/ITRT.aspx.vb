
Partial Class MR_MRT
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub departmentDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ToolSqlDataSource.SelectCommand = "SELECT Tool, [Key] FROM dbo.T_IT_Tools WHERE (Department = '" & Me.DepartmentDropDownList.SelectedItem.Text & "') ORDER BY Tool"
        Me.ToolDropDownList.DataBind()
        FillWebpageDropDownList()
        Look_For_SG_Tags()
    End Sub

    Protected Sub ToolDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolDropDownList.SelectedIndexChanged
        FillWebpageDropDownList()
        Look_For_SG_Tags()
    End Sub

    Sub Look_For_SG_Tags()
        '"SELECT T_Tool_SubGroup_Tag_Names.SG_Name, T_Tool_SubGroup_Tag_Names.SB_Tag FROM T_Tools INNER JOIN T_Tool_SubGroup_Tag_Names ON T_Tools.[Key] = T_Tool_SubGroup_Tag_Names.Tool_Key WHERE (T_Tools.Tool = 'CMP 1')"
        Me.PanelSGT.Visible = True
        Me.SqlDataSource_SGN.SelectCommand = "SELECT T_Tool_SubGroup_Tag_Names.SG_Name, T_Tool_SubGroup_Tag_Names.SB_Tag FROM T_IT_Tools INNER JOIN T_Tool_SubGroup_Tag_Names ON T_IT_Tools.[Key] = T_Tool_SubGroup_Tag_Names.Tool_Key WHERE (T_IT_Tools.Tool = '" & Me.ToolDropDownList.SelectedItem.Text & "')"
        Me.CheckBoxList_SGL.DataBind()
        If CheckBoxList_SGL.Items.Count > 0 Then

        Else
            Me.PanelSGT.Visible = False
        End If
    End Sub

    Sub FillWebpageDropDownList()
        If DepartmentDropDownList.SelectedItem.ToString() = "Sati" Then
            WebpagePanel.Visible = "True"
            Me.WebpageSqlDataSource.SelectCommand = "SELECT Tool, [Key] FROM dbo.T_IT_Webpages WHERE (Department = '" & Me.ToolDropDownList.SelectedItem.Text & "') ORDER BY Tool"
            Me.WebpageDropDownList.DataBind()
        Else
            WebpagePanel.Visible = "False"
        End If
    End Sub

    Function DownTicket(tool As String) As Boolean
        Dim ds As Data.DataSet

        ds = SatiCode.GetMyDataSet("SELECT dbo.T_Tools.Tool, dbo.T_MR_Tickets.MR_Key AS Ticket, dbo.T_MR_Tickets.Status FROM dbo.T_MR_Tickets INNER JOIN dbo.T_Tools ON dbo.T_MR_Tickets.Tool = dbo.T_Tools.[Key] INNER JOIN dbo.T_MR_TicketNotes ON dbo.T_MR_Tickets.MR_Key = dbo.T_MR_TicketNotes.MR_Key WHERE (dbo.T_MR_Tickets.CloseDate IS NULL) AND (dbo.T_Tools.[Key] = " & tool & ") GROUP BY dbo.T_Tools.Tool, dbo.T_MR_Tickets.MR_Key, dbo.T_MR_Tickets.Status HAVING (dbo.T_MR_Tickets.Status = 'Down')")

        If ds.Tables(0).Rows.Count = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Sub Submit()
        Dim tool As String
        Dim webpage As String
        Dim status As String = ""
        Dim TicketNumber As String = ""
        Dim SB As New StringBuilder
        Dim TheSubject As String = ""
        Dim TAGs As String = " "

        If WebpagePanel.Visible = True Then
            If WebpageDropDownList.SelectedValue.ToString = "" Then
                Me.infoLabel.Text = "Select a Webpage"
                Exit Sub
            Else
                webpage = WebpageDropDownList.SelectedItem.Text
            End If
        End If

        If Me.ToolDropDownList.SelectedValue.ToString = "" Then
            Me.infoLabel.Text = "Select a Tool"
            Exit Sub
        End If
        tool = Me.ToolDropDownList.SelectedValue.ToString

        If Me.DropDownListTicketType.SelectedValue = "Select..." Then
            Me.infoLabel.Text = "Select Ticket Type"
            Exit Sub
        Else
            status = Me.DropDownListTicketType.SelectedValue.ToString
        End If

        If Me.ProblemTextBox.Text = "" Then
            Me.infoLabel.Text = "Describe Problem"
            Exit Sub
        End If

        '******add the sub grouping tags***
        If Me.PanelSGT.Visible = True Then
            If Me.CheckBoxList_SGL.Items.Count > 0 Then
                For i As Int16 = 0 To Me.CheckBoxList_SGL.Items.Count - 1
                    If Me.CheckBoxList_SGL.Items(i).Selected = True Then
                        TAGs = TAGs & "< " & Me.CheckBoxList_SGL.Items(i).Value & " >"
                    End If
                Next
            End If
        End If

        If status = "Down" Then
            ' check to see if a "down" tickit is alread on this tool
            If DownTicket(tool) = True Then
                Me.infoLabel.Text = "SATI.Net already has a Down ticket for this tool. "

                Exit Sub
            End If

        End If


        TicketNumber = SatiCode.ITRequestTicket("New", "0", tool, status)

        Try
            SatiCode.ITRequestNote(TicketNumber, "Org", If(webpage Is Nothing, "", webpage & " Webpage: ") & Me.ProblemTextBox.Text & TAGs)

            SB.Append(<h1 style="color: #0000FF">IT Request</h1>)
            SB.Append(<br/>)

            SB.Append("<font size=5>Ticket:</font> &nbsp; <font size=5 color=red><b>" & TicketNumber.ToString & "</b></font>")
            SB.Append(<br/>)
            SB.Append("<font size=5>Department:</font> &nbsp; <font size=5 color=red><b>" & DepartmentDropDownList.SelectedItem.Text.ToString & "</b></font>")
            SB.Append(<br/>)

            If webpage Is Nothing Then
                SB.Append("<font size=5>Tool:</font> &nbsp; <font size=5 color=red><b>" & ToolDropDownList.SelectedItem.Text.ToString & "</b></font>")
            Else
                SB.Append("<font size=5>Webpage:</font> &nbsp; <font size=5 color=red><b>" & ToolDropDownList.SelectedItem.Text.ToString & " -> " & webpage & "</b></font>")
            End If

            SB.Append(<br/>)
            SB.Append("<font size=5>From: &nbsp; <font size=5 color=red><b>" & User.Identity.Name.ToString & "</b></font>")
            SB.Append(<br/>)
            SB.Append("<font size=5>Problem:</font> &nbsp; <font size=5 color=red><b>" & Me.ProblemTextBox.Text & TAGs & Chr(13) & Chr(13) & "</b></font>")
            SB.Append(<br/>)

            If status = "Down" Then
                TheSubject = "Tool: " & Me.ToolDropDownList.SelectedItem.Text.ToString & " Is Down! Ticket Number: " & TicketNumber
            Else
                TheSubject = "IT Request Issued. Ticket Number: " & TicketNumber
            End If
            Me.Button1.Enabled = False

            SatiCode.SendMail_HTML(SB.ToString, TheSubject, "szymon.tyburek@purewafer.com", "Sati@purewafer.com")

            Me.infoLabel.Text = "Your Request Was Submited. Your Ticket Number is " & TicketNumber
        Catch ex As Exception
            Me.infoLabel.Text = "Error, Contact Your Sati.Net Admin"
        End Try

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Button1.Enabled = False
        Submit()
    End Sub

    Private Sub MR_MRT_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)

        If User.Identity.IsAuthenticated = False Then
            Server.Transfer("~/Login.aspx")
        End If
    End Sub
End Class
