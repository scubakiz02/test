
Imports System.Text.Json
Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim TimeForNewLog As Boolean
    Dim LogStatus As String
    Dim StripeColor As String
    Dim CurrLogDate As String
    Dim SqlFunc As String
    Dim LogDS As New Data.DataSet
    Dim LogDR As Data.DataRow

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Dim DS As New Data.DataSet
        Dim RC As Integer = 0
        Dim AreaKey As Integer
        Dim AreaDS As Data.DataSet
        Dim AreaRC As Integer
        Dim SqlFuncDR As Data.DataRow
        Dim TodaysDate As Date = Date.Parse(System.DateTime.Now)

        'update session state variables if querystring values exist
        If Request.QueryString.Count > 0 Then
            'if where waa NOT passed to querystring OR midnight rollover occurs
            If Request.QueryString("WHERE") Is Nothing OrElse (DateDiff(DateInterval.Day, Date.Parse(Session("WhereFromQueryString")), TodaysDate) = 1 AndAlso TodaysDate.Hour = 0) Then
                Session("WhereFromQueryString") = TodaysDate.Date
            Else 'If Request.QueryString("WHERE") IsNot Nothing Then
                Session("WhereFromQueryString") = Request.QueryString("WHERE")
            End If

            Session("DepartmentFromQueryString") = Request.QueryString("Department")
            Session("ViewFromQueryString") = Request.QueryString("View")

            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path)) 'redirect the user to the URL without query strings
        End If

        WhereLabel.Text = Session("WhereFromQueryString")

        If Not String.IsNullOrEmpty(Session("ViewFromQueryString")) Then
            'iterate through child controls within DepartmentMenu, check if it's it the sender. If so, disable. Otherwise, enable
            For Each Ctrl In ViewMenu.Controls
                If TypeOf Ctrl Is Button Then
                    If Ctrl.ID.Contains(Session("ViewFromQueryString")) Then
                        Ctrl.Enabled = False
                    Else
                        Ctrl.Enabled = True
                    End If
                End If
            Next

            If Session("ViewFromQueryString") = "Full" Then
                AdminPanel.Visible = True
            End If
        Else
            Session("ViewFromQueryString") = "Focus"
            ViewMenu_onClick(FocusViewButton, EventArgs.Empty) 'maintenance by default
        End If

        If Not String.IsNullOrEmpty(Session("DepartmentFromQueryString")) Then
            'iterate through child controls within DepartmentMenu, check if it's it the sender. If so, disable. Otherwise, enable
            For Each Ctrl In DepartmentMenu.Controls
                If TypeOf Ctrl Is Button Then
                    If Ctrl.ID.Contains(Session("DepartmentFromQueryString")) Then
                        Ctrl.Enabled = False
                    Else
                        Ctrl.Enabled = True
                    End If
                End If
            Next
        Else
            Session("ViewFromQueryString") = "Maintenance"
            DepartmentMenu_onClick(AllButton, EventArgs.Empty) 'all by default
        End If

        'build button controls for checklists that have a department, interval, assignee, & at least 1 input
        AreaDS = SatiCode.GetMyDataSet("SELECT A.[Key] FROM [ALTS].[dbo].[T_LogArea] A INNER JOIN [ALTS].[dbo].[T_LogDepartment] D ON A.DepartmentKey=D.[Key] INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE A.Active=1 AND" & If(Session("DepartmentFromQueryString") <> "All", " D.Department='" & Session("DepartmentFromQueryString") & "' AND", String.Empty) & " (SELECT COUNT([Key]) FROM [ALTS].[dbo].[T_LogLabel] L WHERE L.AreaKey=A.[Key]) > 0 AND A.Assignee IS NOT NULL ORDER BY I.DisplayOrder, A.Area")
        AreaRC = AreaDS.Tables(0).Rows.Count

        For I = 0 To AreaRC - 1
            AreaKey = AreaDS.Tables(0).Rows(I)("Key")
            SqlFuncDR = SatiCode.GetMyDataSet("SELECT I.SqlFunc, I.SqlFunc2ndArg FROM [ALTS].[dbo].[T_LogArea] A INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE A.[Key]=" & AreaKey).Tables(0).Rows(0)
            SqlFunc = SqlFuncDR("SqlFunc")
            Dim DailyOrWeeklyChecklist As Boolean = If(SqlFunc = "[ALTS].[dbo].[T_Log_DailyChecklistInfo]" OrElse SqlFunc = "[ALTS].[dbo].[T_Log_WeeklyChecklistInfo]", True, False)

            'If Date.Parse(Session("WhereFromQueryString")).Date <> Today.Date AndAlso DailyOrWeeklyChecklist Then 'do NOT display daily or weekly checklists during time travel
            '    TimeTravelMessageLabel.Visible = True
            '    Continue For
            'End If

            LogDS = SatiCode.GetMyDataSet("Select  * FROM " & SqlFunc & "(" & AreaKey & ", " & SqlFuncDR("SqlFunc2ndArg") & ", '" & Session("WhereFromQueryString") & "')")
            LogDR = LogDS.Tables(0).Rows(0)
            TimeForNewLog = LogDR("TimeForNewLog")
            CurrLogDate = LogDR("CurrLogDate")

            If TimeForNewLog Then
                CreateRecord(AreaKey)
            End If

            Build(AreaKey)
        Next
    End Sub


    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        'MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        'MenuAuthenication.CheckGroupAuthenication("Office", Server)
    End Sub

    Sub MaybeCreateRecord(AreaKey As Integer, CalendarDate As Date)
    End Sub

    Sub CreateRecord(AreaKey As Integer)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = "Data Source=PWI-31\SATIDB;Initial Catalog=ALTS;Persist Security Info=True;User ID=sati;Password=laptopia"
        Connection.Open()

        Dim My_DA As New Data.SqlClient.SqlDataAdapter
        Dim My_DS As New Data.DataSet
        Dim My_DR As Data.DataRow
        Dim My_DS2 As New Data.DataSet
        Dim RC As Integer
        Dim My_DR2 As Data.DataRow
        Dim InputsMap As New Dictionary(Of Integer, String)
        Dim OutOfRangeMap As New Dictionary(Of Integer, String)
        Dim MapKey As String
        Dim Key As String
        '*****************************************************************
        '************************Select***********************************
        '*****************************************************************
        Dim MySelectCmd As New System.Data.SqlClient.SqlCommand
        With MySelectCmd
            .CommandText = "SELECT Top(1) * FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=" & AreaKey & " ORDER BY Date DESC"
            .Connection = Connection
        End With
        My_DA.SelectCommand = MySelectCmd

        '*****************************************************************
        '************************Insert***********************************
        '*****************************************************************
        Dim MyInsertCmd As New System.Data.SqlClient.SqlCommand
        With MyInsertCmd
            .CommandText = "INSERT INTO T_LogData (AreaKey, Inputs, OutOfRange, Date, Operator, Shift, CompleteLog, ManagerStamp1, ManagerStamp2, ManagerStamp3, ToolNumber, Active) VALUES (@AreaKey, @Inputs, @OutOfRange, @Date, @Operator, @Shift, @CompleteLog, NULL, NULL, NULL, NULL, 'False')"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@AreaKey", System.Data.SqlDbType.Int, 0, "AreaKey"), New System.Data.SqlClient.SqlParameter("@Inputs", System.Data.SqlDbType.VarChar, 0, "Inputs"), New System.Data.SqlClient.SqlParameter("@OutOfRange", System.Data.SqlDbType.VarChar, 0, "OutOfRange"), New System.Data.SqlClient.SqlParameter("@Date", System.Data.SqlDbType.SmallDateTime, 0, "Date"), New System.Data.SqlClient.SqlParameter("@Operator", System.Data.SqlDbType.VarChar, 0, "Operator"), New System.Data.SqlClient.SqlParameter("@Shift", System.Data.SqlDbType.VarChar, 0, "Shift"), New System.Data.SqlClient.SqlParameter("@CompleteLog", System.Data.SqlDbType.Bit, 0, "CompleteLog")})
        End With
        My_DA.InsertCommand = MyInsertCmd

        '*****************************************************************
        '************************Update***********************************
        '*****************************************************************
        Dim MyUpdateCmd As New System.Data.SqlClient.SqlCommand
        With MyUpdateCmd
            .CommandText = "UPDATE T_LogData SET [Inputs] = @Inputs, [OutOfRange] = @OutOfRange, [Date] = @Date, [Operator] = @Operator, [CompleteLog] = @CompleteLog WHERE [Key]=@DataLogKey; SELECT TOP(1) * FROM T_LogData WHERE AreaKey=" & AreaKey & " ORDER BY Date DESC;"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Inputs", System.Data.SqlDbType.VarChar, 0, "Inputs"), New System.Data.SqlClient.SqlParameter("@OutOfRange", System.Data.SqlDbType.VarChar, 0, "OutOfRange"), New System.Data.SqlClient.SqlParameter("@Date", System.Data.SqlDbType.SmallDateTime, 0, "Date"), New System.Data.SqlClient.SqlParameter("@Operator", System.Data.SqlDbType.VarChar, 0, "Operator"), New System.Data.SqlClient.SqlParameter("@CompleteLog", System.Data.SqlDbType.Bit, 0, "CompleteLog"), New System.Data.SqlClient.SqlParameter("@DataLogKey", System.Data.SqlDbType.Int, 0, "Key")})
        End With
        My_DA.UpdateCommand = MyUpdateCmd

        '*****************************************************************
        '************************Delete***********************************
        '*****************************************************************
        'Dim MyDeleteCmd As New System.Data.SqlClient.SqlCommand
        'With MyDeleteCmd
        '    .CommandText = "DELETE FROM [aspnet_UsersInRoles] WHERE (([UserId] = @Original_UserId) AND ([RoleId] = @Original_RoleId))"
        '    .Connection = Connection
        '    .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Original_UserId", System.Data.SqlDbType.UniqueIdentifier, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "UserId", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_RoleId", System.Data.SqlDbType.UniqueIdentifier, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "RoleId", System.Data.DataRowVersion.Original, Nothing)})
        'End With
        'My_DA.DeleteCommand = MyDeleteCmd

        '*****************************************************************
        '************************Genral***********************************
        '*****************************************************************
        My_DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_LogData", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("AreaKey", "AreaKey"), New System.Data.Common.DataColumnMapping("Inputs", "Inputs"), New System.Data.Common.DataColumnMapping("OutOfRange", "OutOfRange"), New System.Data.Common.DataColumnMapping("Date", "Date"), New System.Data.Common.DataColumnMapping("Operator", "Operator"), New System.Data.Common.DataColumnMapping("Shift", "Shift"), New System.Data.Common.DataColumnMapping("CompleteLog", "CompleteLog")})}) 'the fields that are dynamically generated
        My_DA.Fill(My_DS)

        'in case of db upload failure, closing code below in a try catch block
        Try
            My_DR = My_DS.Tables("T_LogData").NewRow
            My_DR("AreaKey") = AreaKey

            My_DS2 = SatiCode.GetMyDataSet("SELECT [Key] FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=" & AreaKey)
            RC = My_DS2.Tables(0).Rows.Count

            For I = 0 To RC - 1
                My_DR2 = My_DS2.Tables(0).Rows(I)
                MapKey = My_DR2("Key")
                InputsMap.Add(MapKey, "")
                OutOfRangeMap.Add(MapKey, Nothing)
            Next

            My_DR("Inputs") = JsonSerializer.Serialize(InputsMap)
            My_DR("OutOfRange") = JsonSerializer.Serialize(OutOfRangeMap)

            My_DR("Date") = CurrLogDate
            My_DR("Operator") = Nothing
            My_DR("CompleteLog") = False
            My_DR("Shift") = SatiCode.GetMyDataSet("SELECT Shift FROM [ALTS].[dbo].[T_Log_GetShift]()").Tables(0).Rows(0)("Shift")
            My_DS.Tables("T_LogData").Rows.Add(My_DR)
            My_DA.Update(My_DS, "T_LogData")
        Catch ex As Exception
            Dim CatchErr As String = ex.Message.ToString()
            Dim Placeholder As String = "Yay"
        End Try

        Connection.Close()
    End Sub

    Function GetSingleDbField(SqlQuery As String, Field As String) As String
        Dim Res As String

        'using try catch block in case 'There is no row at position 0.', which means there are no associated record in Table
        Try
            Res = If(IsDBNull(SatiCode.GetMyDataSet(SqlQuery).Tables(0).Rows(0)(Field)), Nothing, SatiCode.GetMyDataSet(SqlQuery).Tables(0).Rows(0)(Field)) 'using ternary operator as a workaround to Null DB field values, which in that case the function will return Nothing
        Catch ex As Exception
            Res = Nothing
        End Try

        Return Res
    End Function

    Sub SetButtonBackground(DR As Data.DataRow)
        If Not IsDBNull(DR("Operator")) Then
            LogStatus = DR("LogStatus")
            StripeColor = DR("StripeColor")
        Else
            LogStatus = "pink"
            StripeColor = "pink"
        End If
    End Sub

    Sub Build(AreaKey As Integer)
        Dim I As Integer = 0
        Dim II As Integer = 0
        Dim RC As Integer = 0
        Dim Department As String = ""
        Dim SB As New StringBuilder
        Dim PastIssuesSB As New StringBuilder
        Dim TempNote As String
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim DR_Look As Data.DataRow
        Dim CountNotes As Integer
        Dim DateDiffInterval As String = "Day"
        Dim SubSectionId As String
        Dim Assignee As String

        'build controls for CurrentLogsPanel dynamically
        DS = SatiCode.GetMyDataSet("SELECT A.Area, I.Interval, A.Assignee, D.[Key], D.AreaKey, D.Operator, Sql.LogStatus, Sql.StripeColor, MAX(D.Date) AS MaxDate FROM [ALTS].[dbo].[T_LogData] D INNER JOIN [ALTS].[dbo].[T_LogArea] A ON D.AreaKey = A.[Key] INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] CROSS APPLY [ALTS].[dbo].[T_Log_ChecklistRecordInfo]((SELECT [Key] FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=" & AreaKey & " AND CAST(Date As Date) = '" & CurrLogDate & "'), 1, '" & CurrLogDate & "') Sql WHERE CAST(D.Date As Date) = '" & CurrLogDate & "' AND AreaKey=" & AreaKey & " GROUP BY A.Area, I.Interval, A.Assignee, D.[Key], D.AreaKey, D.Operator, Sql.LogStatus, Sql.StripeColor")
        RC = DS.Tables(0).Rows.Count

        For I = 0 To RC - 1
            Dim CurrentLogsButton As New Button()
            DR = DS.Tables(0).Rows(I)

            Assignee = If(IsDBNull(DR("Assignee")), Nothing, DR("Assignee").ToString())

            SetButtonBackground(DR)
            CurrentLogsButton.ID = DR("Key")
            CurrentLogsButton.Text = DR("Area")

            AddHandler CurrentLogsButton.Click, AddressOf RedirectToLogAspx
            CurrentLogsButton.Attributes.Add("style", "background: repeating-linear-gradient(60deg, " + LogStatus + ", " + LogStatus + " 10px, " + StripeColor + ", " + StripeColor + " 20px); ")
            CurrentLogsButton.CssClass = "ChecklistButton"

            Select Case DR("Interval")
                Case "ONE TIME ONLY"
                    If Date.Parse(Session("WhereFromQueryString")).Date = LogDR("CurrLogDate").Date Then 'check if date of One TIme Task matches date in querystring
                        SubSectionId += "OneTime"
                    Else
                        Continue For
                    End If
                Case "DAILY"
                    SubSectionId += "Daily"
                Case "WEEKLY"
                    SubSectionId += "Weekly"
                Case "MONTHLY"
                    SubSectionId += "Monthly"
                Case Else ' Interval > Monthly. Ex: 1 year, 2 years, 5 years, etc.
                    If Not LogDR("BuildInStatusBoard") Then 'check value of BuildInStatusBoard from sql function
                        Continue For
                    Else
                        SpecialLogsPanel.Visible = True
                    End If
            End Select

            Select Case Assignee
                Case "Day Shift"
                    SubSectionId += "DayShift"
                Case "Night Shift"
                    SubSectionId += "NightShift"
                Case "Days (M-F)"
                    SubSectionId += "MFShift"
                Case "D1"
                    SubSectionId += "D1"
                Case "N1"
                    SubSectionId += "N1"
                Case "D2"
                    SubSectionId += "D2"
                Case "N2"
                    SubSectionId += "N2"
                Case "QUARTERLY"
                    SubSectionId += "Quarterly"
                Case "BIANNUAL"
                    SubSectionId += "BiAnnual"
                Case "1 YEAR"
                    SubSectionId += "OneYear"
                Case "2 YEARS"
                    SubSectionId += "TwoYear"
                Case "3 YEARS"
                    SubSectionId += "ThreeYear"
                Case "4 YEARS"
                    SubSectionId += "FourYear"
                Case "5 YEARS"
                    SubSectionId += "FiveYear"
                Case Else 'User
                    SubSectionId += "Users"
                    CurrentLogsButton.Text = DR("Assignee") & " - " & DR("Area")
            End Select

            If SubSectionId IsNot Nothing Then
                CType(CurrentLogsPanel.FindControl(SubSectionId & "Panel"), Panel).Controls.Add(CurrentLogsButton)
                CType(CurrentLogsPanel.FindControl(SubSectionId & "NoneLabel"), Label).Visible = False
            End If
        Next

        'build controls for PastIssuesPanel dynamically
        'DS = SatiCode.GetMyDataSet("SELECT D.[Key], D.Operator, A.Area, Sql.LogStatus, Sql.StripeColor, Sql.NumOfStamps, Sql.NumOfNeededStamps FROM [ALTS].[dbo].[T_LogData] D INNER JOIN [ALTS].[dbo].[T_LogArea] A ON D.AreaKey=A.[Key] CROSS APPLY [ALTS].[dbo].[T_Log_ChecklistRecordInfo](D.[Key], 1, (SELECT Date FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=D.[Key])) Sql WHERE (D.[All_InputsValid] <> 1 OR D.CompleteLog <> 1) AND D.[Key] <> (SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=" & AreaKey & " ORDER BY DATE DESC) AND D.Date < '" & Session("WhereFromQueryString") & "' AND AreaKey=" & AreaKey & " ORDER BY Date ASC")
        DS = SatiCode.GetMyDataSet("SELECT D.[Key], D.Date, D.Operator, A.Area, A.Assignee, Sql.LogStatus, Sql.StripeColor, Sql.NumOfStamps, Sql.NumOfNeededStamps FROM [ALTS].[dbo].[T_LogData] D INNER JOIN [ALTS].[dbo].[T_LogArea] A ON D.AreaKey=A.[Key] CROSS APPLY [ALTS].[dbo].[T_Log_ChecklistRecordInfo](D.[Key], 1, (SELECT Date FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=D.[Key])) Sql WHERE AreaKey=" & AreaKey & " AND (D.[Key] <> (SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=" & AreaKey & " AND CAST(D.Date As Date) < '" & CurrLogDate & "' ORDER BY DATE DESC) OR (A.OneTimeDate IS NOT NULL AND '" & Session("WhereFromQueryString") & "' > D.Date)) AND ((D.[All_InputsValid] <> 1 OR D.CompleteLog <> 1) OR Sql.NumOfStamps < Sql.NumOfNeededStamps) ORDER BY Date ASC")
        RC = DS.Tables(0).Rows.Count

        For I = 0 To RC - 1
            Dim PastIssuesButton As New Button()
            DR = DS.Tables(0).Rows(I)

            SetButtonBackground(DR)
            PastIssuesButton.ID = DR("Key")
            PastIssuesButton.Text = DR("Area")
            SetButtonText(PastIssuesButton, DR)

            AddHandler PastIssuesButton.Click, AddressOf RedirectToLogAspx
            PastIssuesButton.Attributes.Add("style", "max-width: 100%; background: repeating-linear-gradient(60deg, " + LogStatus + ", " + LogStatus + " 10px, " + StripeColor + ", " + StripeColor + " 20px); ")
            PastIssuesButton.CssClass = "ChecklistButton"
            PastIssuesPanel.Controls.Add(PastIssuesButton)
        Next

    End Sub

    Sub SetButtonText(Button As Button, DR As Data.DataRow)
        If Not IsDBNull(DR("Assignee")) Then
            Button.Text = DR("Assignee") & " - " & DR("Area")
            ' Button.ForeColor = System.Drawing.Color.DarkBlue
        Else
            Button.Text = DR("Area")
        End If

    End Sub


    Protected Sub TimeTravelCalendar_OnSelectionChanged(sender As Object, e As EventArgs)
        Response.Redirect(Request.FilePath.ToString & "?WHERE=" & TimeTravelCalendar.SelectedDate & "&Department=" & Session("DepartmentFromQueryString") & "&View=" & Request.QueryString("View"))
    End Sub

    Protected Sub TimeTravelCalendar_OnDayRender(sender As Object, e As DayRenderEventArgs)
        'Dim FirstDate As String = GetSingleDbField("SELECT Min(Date) As FirstDate FROM [ALTS].[dbo].[T_LogData]", "FirstDate")
        'Dim LastDate As String = GetSingleDbField("SELECT Max(Date) As LastDate FROM [ALTS].[dbo].[T_LogData]", "LastDate")
        'Dim CalendarDate As Date = e.Day.Date

        'If FirstDate Is Nothing OrElse LastDate Is Nothing Then
        '    Exit Sub
        'End If

        'If Date.Parse(LastDate).Date < Today.Date Then
        '    LastDate = Today.ToString()
        'End If

        'If CalendarDate < Date.Parse(FirstDate).Date OrElse CalendarDate > Date.Parse(LastDate).Date Then
        '    e.Day.IsSelectable = False
        '    e.Cell.BackColor = System.Drawing.Color.LightGray
        '    e.Cell.ForeColor = System.Drawing.Color.DarkGray
        'End If

        If e.Day.Date = Today.Date Then
            e.Cell.BackColor = System.Drawing.Color.LightGray
            e.Cell.ForeColor = System.Drawing.Color.DarkGray
        End If
    End Sub

    Protected Sub RedirectToLogAspx(sender As Object, e As EventArgs)
        'Response.Redirect("/ChecklistLogging/Log.aspx?Key=" & sender.ID & If(Date.Parse(Session("WhereFromQueryString")).Date <> Today.Date, "&WHERE=" & Request.QueryString("WHERE"), String.Empty) & "&Department=" & Request.QueryString("Department") & "&View=" & Request.QueryString("View"))
        Response.Redirect("/ChecklistLogging/Log.aspx?Key=" & sender.ID)
    End Sub

    Protected Sub DepartmentMenu_onClick(sender As Object, e As EventArgs)
        Response.Redirect(Request.FilePath.ToString & "?" & If(Date.Parse(Session("WhereFromQueryString")).Date <> Today.Date, "&WHERE=" & Request.QueryString("WHERE") & "&", String.Empty) & "Department=" & sender.Text & "&View=" & Request.QueryString("View"))
    End Sub

    Protected Sub ViewMenu_onClick(sender As Object, e As EventArgs)
        Response.Redirect(Request.FilePath.ToString & "?" & If(Date.Parse(Session("WhereFromQueryString")).Date <> Today.Date, "&WHERE=" & Request.QueryString("WHERE") & "&", String.Empty) & "Department=" & Request.QueryString("Department") & "&View=" & sender.Text)
    End Sub

    Protected Sub PageRefresh_OnTick(sender As Object, e As EventArgs)
        Response.Redirect(Request.Url.ToString & "?Department=" & Session("DepartmentFromQueryString") & "&View=" & Session("ViewFromQueryString") & "&WHERE=" & Session("WhereFromQueryString"))
    End Sub

End Class
