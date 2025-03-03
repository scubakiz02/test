Imports System.Web.Services

Public Class MR_MRT
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub departmentDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ToolSqlDataSource.SelectCommand = "SELECT Tool, [Key] FROM dbo.T_Tools WHERE (Department = '" & Me.DepartmentDropDownList.SelectedItem.Text & "') ORDER BY Tool"
        Me.ToolDropDownList.DataBind()
        Look_For_SG_Tags()

        EvalProblemDesc(ProblemTextBox.Text)
    End Sub

    Protected Sub ToolDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolDropDownList.SelectedIndexChanged
        Look_For_SG_Tags()
    End Sub
    Protected Sub DropDownListTicketType_OnSelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolDropDownList.SelectedIndexChanged
        EvalProblemDesc(ProblemTextBox.Text)
    End Sub

    Sub Look_For_SG_Tags()
        '"SELECT T_Tool_SubGroup_Tag_Names.SG_Name, T_Tool_SubGroup_Tag_Names.SB_Tag FROM T_Tools INNER JOIN T_Tool_SubGroup_Tag_Names ON T_Tools.[Key] = T_Tool_SubGroup_Tag_Names.Tool_Key WHERE (T_Tools.Tool = 'CMP 1')"
        Me.PanelSGT.Visible = True
        Me.SqlDataSource_SGN.SelectCommand = "SELECT T_Tool_SubGroup_Tag_Names.SG_Name, T_Tool_SubGroup_Tag_Names.SB_Tag FROM T_Tools INNER JOIN T_Tool_SubGroup_Tag_Names ON T_Tools.[Key] = T_Tool_SubGroup_Tag_Names.Tool_Key WHERE (T_Tools.Tool = '" & Me.ToolDropDownList.SelectedItem.Text & "')"
        Me.CheckBoxList_SGL.DataBind()
        If CheckBoxList_SGL.Items.Count > 0 Then

        Else
            Me.PanelSGT.Visible = False
        End If
    End Sub

    <WebMethod()>
    Public Shared Function EvaluateProblemTextBox(ProblemTextBoxText As String) As String
        Dim EvalProblemDesc As EvalProblemDescDelegate = HttpContext.Current.Session("EvalProblemDesc")
        Return EvalProblemDesc(ProblemTextBoxText)
    End Function

    Public Delegate Function EvalProblemDescDelegate(ProblemTextBoxText As String) As String
    Function EvalProblemDesc(ProblemTextBoxText As String) As String
        Dim infoLabelText As String

        infoLabelText = RequirementCheck()
        If infoLabelText = "" AndAlso String.IsNullOrEmpty(ProblemTextBoxText) Then infoLabelText = "Describe Problem"

        Me.infoLabel.Text = infoLabelText
        If infoLabelText = "" Then
            Me.Button1.Enabled = True
        Else
            Me.Button1.Enabled = False 'in case all requirements were present but are NOT anymore
        End If

        Return infoLabelText
    End Function

    Function RequirementCheck() As String
        Try
            If Me.ToolDropDownList.SelectedValue.ToString = "" Then
                Throw New Exception("Select a Tool")
            End If

            If Me.DropDownListTicketType.SelectedValue = "Select..." Then
                Throw New Exception("Select Ticket Type")
            End If

            'If Me.ProblemTextBox.Text = "" Then
            '    Throw New Exception("Describe Problem")
            'End If

            If Me.DropDownListTicketType.SelectedValue.ToString = "Down" Then
                ' check to see if a "down" tickit is alread on this tool
                If DownTicket(Me.ToolDropDownList.SelectedValue.ToString) = True Then
                    Throw New Exception("SATI.Net already has a Down ticket for this tool. ")
                End If
            End If

            Return ""
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try

    End Function

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
        Dim status As String = ""
        Dim TicketNumber As String = ""
        Dim TheMail As String = ""
        Dim TheSubject As String = ""
        Dim TAGs As String = " "

        tool = Me.ToolDropDownList.SelectedValue.ToString
        status = Me.DropDownListTicketType.SelectedValue.ToString

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

        'TicketNumber = SatiCode.MaintenanceRequestTicket("New", "0", tool, status)



        Try
            'SatiCode.MaintenanceRequestNote(TicketNumber, "Org", Me.ProblemTextBox.Text & TAGs)

            'TheMail = "Sati.Net has received a Maintenance Request from " & User.Identity.Name.ToString & " In the " _
            '& Me.DepartmentDropDownList.SelectedItem.Text.ToString & " Department. " & Chr(13) & Chr(13) _
            '& "The " & Me.ToolDropDownList.SelectedItem.Text.ToString & " has the following problem. " & Chr(13) _
            '& Me.ProblemTextBox.Text & TAGs & Chr(13) & Chr(13) & "The Maintenance Request is under Ticket Number: " & TicketNumber

            'If status = "Down" Then
            '    TheSubject = "Tool: " & Me.ToolDropDownList.SelectedItem.Text.ToString & " Is Down! Ticket Number: " & TicketNumber
            'Else
            '    TheSubject = "Maintenance Request Issued. Ticket Number: " & TicketNumber
            'End If
            'Me.Button1.Enabled = False
            ''SatiCode.SendMail(TheMail, TheSubject, "MaintenanceRequest")
            ''SatiCode.SendMail_MaintenRequest(TheMail, TheSubject, "New", TicketNumber)
            'SatiCode.SendMail_HTML(TheMail, TheSubject, "AZ.SatiMaintenanceRequest@purewafer.com", "Sati@purewafer.com")

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

        Dim EvalProblemDescDelegate As EvalProblemDescDelegate = AddressOf EvalProblemDesc
        Session("EvalProblemDesc") = EvalProblemDescDelegate
    End Sub
End Class
