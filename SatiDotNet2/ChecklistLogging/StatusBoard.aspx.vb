
Imports System.Configuration
Imports System.Text.Encodings.Web
Imports System.Text.Json
Imports System.Threading.Tasks
Imports System.Web.Services
Imports Microsoft.PowerBI.Api.V2.Models
Imports SatiDotNet2.Library


Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim LogAspx As New LogAspxLibrary
    Dim Security As New Security
    Dim LogStatus As String
    Dim StripeColor As String
    Dim SqlFunc As String
    Dim LogDS As New Data.DataSet
    Dim LogDR As Data.DataRow
    Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
    Dim SqlFuncDR As Data.DataRow

    Private Shared StampIndicator As New StampIndicator()
    Private _ActivePm As New ActivePm()

    Private Sub PageInit(sender As Object, e As EventArgs) Handles Me.Init
        Dim DS As New Data.DataSet
        Dim RC As Integer = 0
        Dim AreaKey As Integer
        Dim AreaDS As Data.DataSet
        Dim AreaRC As Integer
        Dim TodaysDate As Date = Date.Parse(System.DateTime.Now)
        Dim StampIndicatorLabels As New Dictionary(Of String, String)

        'check if intitial entry of webpage does NOT contain querystring. if so, redirect to ChecklistLoggingMainMaint.aspx
        If Request.QueryString.Count = 0 AndAlso (Session("WhereFromQueryString") Is Nothing OrElse Session("DepartmentFromQueryString") Is Nothing OrElse Session("ViewFromQueryString") Is Nothing) Then
            'reinitialize the session state variables then refresh the page
            'this process is critical in ensuring 24/7 uptime of status board
            Session("WhereFromQueryString") = TodaysDate.Date
            Session("DepartmentFromQueryString") = "Maintenance"
            Session("ViewFromQueryString") = "Focus"
            Response.Redirect(Request.Url.ToString(), False)
            Exit Sub
        ElseIf Request.QueryString.Count > 0 Then
            Dim QsDepartment As String = Request.QueryString("Department")
            Dim QsView As String = Request.QueryString("View")

            If Request.QueryString("WHERE") Is Nothing Then
                Session("WhereFromQueryString") = TodaysDate.Date
            Else 'If Request.QueryString("WHERE") IsNot Nothing Then
                Session("WhereFromQueryString") = Request.QueryString("WHERE")
            End If

            Session("DepartmentFromQueryString") = If(QsDepartment Is Nothing, Session("DepartmentFromQueryString"), QsDepartment)
            Session("ViewFromQueryString") = If(QsView Is Nothing, Session("ViewFromQueryString"), QsView)

            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path), False) 'redirect the user to the URL without query strings
            Exit Sub
        Else
            'MenuAuthentication hierarchy based on querystrings user loaded the page with
            'MenuAuthenication.CheckGroupAuthenication("EditRoles", Server)
            Dim RequiredRoles As String() = Security.GetStatusBoardRole(Session("ViewFromQueryString"), Session("DepartmentFromQueryString"), Date.Parse(Session("WhereFromQueryString")))
            If RequiredRoles.Contains(Nothing) = False Then
                MenuAuthenication.CheckGroupsAuthenication(RequiredRoles, Server)
            End If

            WhereLabel.Text = Session("WhereFromQueryString")

            If Session("ViewFromQueryString") = "Full" Then
                AdminPanel.Visible = True
            End If

            'BuildCurrentLogs button controls for checklists that have a department, interval, assignee, & at least 1 input
            QueryConfig("@Department") = New Dictionary(Of String, String) From {
                {"value", Session("DepartmentFromQueryString")},
                {"typeOf", "string"}
            }
            AreaDS = Security.GetMyDataSetParamQuery("SELECT A.[Key] FROM [ALTS].[dbo].[T_LogArea] A INNER JOIN [ALTS].[dbo].[T_LogDepartment] D ON A.DepartmentKey=D.[Key] INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE A.Status='live' AND A.Active=1 AND" & If(Session("DepartmentFromQueryString") <> "All", " D.Department=@Department AND", String.Empty) & " (SELECT COUNT([Key]) FROM [ALTS].[dbo].[T_LogLabel] L WHERE L.AreaKey=A.[Key]) > 0 AND A.Assignee IS NOT NULL ORDER BY [Key], I.DisplayOrder, A.Area", QueryConfig)
            AreaRC = AreaDS.Tables(0).Rows.Count

            For I = 0 To AreaRC - 1
                QueryConfig.Clear()
                AreaKey = AreaDS.Tables(0).Rows(I)("Key")

                'There's a constraint on ALTS Database T_LogData Table AreaKey and Date columns
                'This constraint ensures every record has a unique AreaKey + Date
                'That is why CreateRecord() function can be called every time and no duplicated logs will be created
                CreateRecord(AreaKey)
                BuildCurrentLogs(AreaKey)
            Next
        End If
    End Sub

    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        'the method below is static. It only needs to be called once to start the timer that will keep all clients connected to this hub alive
        'can be called anywhere, so I chose status board Page_Load event
        SseStatusBoardHub.StartPing()
    End Sub

    Sub MaybeCreateRecord(AreaKey As Integer, CalendarDate As Date)
    End Sub

    Sub CreateRecord(AreaKey As Integer)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
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

            QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
                    {"value", AreaKey},
                    {"typeOf", "int"}
                }
            My_DS2 = Security.GetMyDataSetParamQuery("SELECT [Key] FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=@AreaKey", QueryConfig)
            RC = My_DS2.Tables(0).Rows.Count

            For I = 0 To RC - 1
                My_DR2 = My_DS2.Tables(0).Rows(I)
                MapKey = My_DR2("Key")
                InputsMap.Add(MapKey, "")
                OutOfRangeMap.Add(MapKey, Nothing)
            Next

            My_DR("OutOfRange") = JsonSerializer.Serialize(OutOfRangeMap)
            My_DR("Date") = _ActivePm.GetCurrLogDate(AreaKey, Session("WhereFromQueryString"))
            My_DR("Operator") = Nothing
            My_DR("CompleteLog") = False
            My_DR("Shift") = Security.GetSingleDbField("SELECT Shift FROM [ALTS].[dbo].[T_Log_GetShift]()", New Dictionary(Of String, Dictionary(Of String, String)), "Shift")
            My_DR("Inputs") = JsonSerializer.Serialize(InputsMap) 'old format (date & operator are NOT recorded for each input)
            My_DR("Inputs") = JsonSerializer.Serialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(LogAspx.GetInputs(My_DR)) 'new format (date & operator are recorded for each input)
            My_DS.Tables("T_LogData").Rows.Add(My_DR)
            My_DA.Update(My_DS, "T_LogData")
        Catch ex As Exception
            Dim CatchErr As String = ex.Message.ToString()
            Dim Placeholder As String = "Yay"
        End Try

        Connection.Close()
    End Sub

    Sub BuildCurrentLogs(AreaKey As Integer)
        Dim I As Integer = 0
        Dim II As Integer = 0
        Dim RC As Integer = 0
        Dim Department As String = ""
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim DateDiffInterval As String = "Day"
        Dim Assignee As String
        Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@AreaKey", Security.GetParamVarHash(AreaKey, "int")},
            {"@CurrLogDate", Security.GetParamVarHash(_ActivePm.GetCurrLogDate(AreaKey, Session("WhereFromQueryString")), "string")}
        }

        DS = Security.GetMyDataSetParamQuery("SELECT A.Area, I.Interval, A.Assignee, D.[Key], D.AreaKey, D.Inputs, D.Operator, D.Date, MAX(D.Date) AS MaxDate " &
            "From [ALTS].[dbo].[T_LogData] D " &
            "INNER JOIN [ALTS].[dbo].[T_LogArea] A ON D.AreaKey = A.[Key] " &
            "INNER Join [ALTS].[dbo].[T_LogAreaInterval] I On A.IntervalKey=I.[Key] " &
            "WHERE " &
            "CAST(D.Date As Date) = @CurrLogDate " &
            "And AreaKey = @AreaKey " &
            "GROUP BY A.Area, I.Interval, A.Assignee, D.[Key], D.AreaKey, D.Inputs, D.Operator, D.Date " &
            "ORDER BY [Key] DESC", SqlConfig)

        RC = DS.Tables(0).Rows.Count
        For I = 0 To RC - 1
            If I > 0 Then Exit For 'in case duplicates exist

            Dim Panel As Panel
            Dim CurrentLogsButton As Button
            Dim BuildLogExport As Tuple(Of Panel, Button)

            DR = DS.Tables(0).Rows(I)
            Assignee = If(IsDBNull(DR("Assignee")), Nothing, DR("Assignee").ToString())

            BuildLogExport = BuildLog(DR)
            Panel = BuildLogExport.Item1
            CurrentLogsButton = BuildLogExport.Item2

            Dim StatusBoardDateAt As String = Session("WhereFromQueryString")
            Dim SubSectionId As String = _ActivePm.GetParentId(DR("Key"), StatusBoardDateAt)
            If SubSectionId <> String.Empty Then
                Dim SubSectionPanel As Panel = CType(CurrentLogsPanel.FindControl(SubSectionId), Panel)
                SubSectionPanel.Controls.Add(Panel)

                Dim SubSectionCssClass As String = SubSectionPanel.CssClass
                Dim HasLogsClass As String = " has-logs"
                If SubSectionCssClass.Contains(HasLogsClass) = False Then
                    SubSectionPanel.CssClass += HasLogsClass
                End If
            End If
        Next
    End Sub

    Public Function BuildLog(DR As Data.DataRow) As Tuple(Of Panel, Button)
        Dim Panel As New Panel()
        Dim SubPanel As New Panel()
        Dim LogButton As New Button()
        Dim IconPanel As New Panel()
        Dim LogState As String = _ActivePm.GetLogConfig(DR("Key"))("logState")
        Dim Datakey As String = DR("Key")

        Panel.CssClass = "button-and-stamps-container"
        Panel.ID = "log-" & Datakey
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Script1-" & Datakey, "applyBackcolorClass(document.getElementById('ctl00_ContentPlaceHolder1_" & Panel.ClientID & "'), '" & LogState & "');", True)

        SubPanel.Attributes.Add("style", "display: flex")

        LogButton.Attributes.Add("style", "width: 100%; border: none; cursor: pointer;")
        LogButton.CssClass &= " ChecklistButton"
        LogButton.ID = Datakey
        LogButton.Text = DR("Area")
        LogButton.OnClientClick = "newTab('Log.aspx?Key=" & DR("Key") & "'); return false;"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Script2-" & Datakey, "applyBackcolorClass(document.getElementById('ctl00_ContentPlaceHolder1_" & LogButton.ClientID & "'), '" & LogState & "'); ", True)

        IconPanel.CssClass &= " icon-panel"
        IconPanel.ID = "IconPanel_" & Datakey

        If LogState = "submitted" Then
            StampIndicator.CreateStampHtml(IconPanel, Datakey)
        End If

        Panel.Controls.Add(SubPanel)

        SubPanel.Controls.Add(LogButton)
        SubPanel.Controls.Add(IconPanel)

        Return Tuple.Create(Panel, LogButton)
    End Function

    Sub SetButtonText(Button As Button, DR As Data.DataRow)
        If Not IsDBNull(DR("Assignee")) Then
            Button.Text = DR("Assignee") & " - " & DR("Area")
            ' Button.ForeColor = System.Drawing.Color.DarkBlue
        Else
            Button.Text = DR("Area")
        End If

    End Sub
End Class
