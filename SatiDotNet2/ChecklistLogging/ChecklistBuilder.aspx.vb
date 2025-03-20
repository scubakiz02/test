Imports System.Text.Json
Imports SatiDotNet2.Library

Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim ChecklistBuilder As New ChecklistBuilderAspxLibrary
    Dim Security As New Security
    Dim VisiblePanels As New List(Of Panel)
    Dim ValidTextBoxes As New List(Of TextBox)
    Dim VisibleCheckBoxes As New List(Of CheckBox)
    Dim LabelInputMap As New Dictionary(Of String, String)
    Dim LabelOutOfRangeMap As New Dictionary(Of String, Boolean?)
    Dim TbxToRange As New Dictionary(Of String, String)
    Dim RangeCheck As New Dictionary(Of String, Func(Of String, Boolean)) 'a function that takes a String and returns a Boolean
    Dim AreaKeyFromDropDownList As String
    Dim TimeForNewLog As Boolean
    Dim LogDS As New Data.DataSet
    Dim LogDR As Data.DataRow
    Dim ReadOnlyMessage As String = "Read-Only Mode"
    Dim WebpageUrl As String = "/ChecklistLogging/ChecklistBuilder.aspx"
    Dim StampSelectPage As String = "/ChecklistLogging/StampSelect.aspx"
    Dim MostRecentRec As String
    Dim DS As Data.DataSet
    Dim DR As Data.DataRow
    Dim DRC As Data.DataRowCollection
    Dim AreaFromQueryString As String
    Dim LabelFromQueryString As String
    Dim CommentFromQueryString As String
    Dim EditPreviewPanel_ScrollPos As String
    Dim FormViewInsert As FormView = Nothing
    Dim Department As String
    Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))

    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        MenuAuthenication.CheckGroupsAuthenication(New String() {"FMManagerApproval", "QSHEManagerApproval"}, Server)
        Me.MaintainScrollPositionOnPostBack = True
        AreaFromQueryString = Request.QueryString("Area")
        LabelFromQueryString = Request.QueryString("Label")
        CommentFromQueryString = Request.QueryString("Comment")
        EditPreviewPanel_ScrollPos = Request.QueryString("EPP_ScrollPos")
        Dim Unit As String
        Dim FieldType As String
        Dim IntervalKey As String
        Dim Interval As String
        Dim IntervalDR As Data.DataRow
        Dim AreaIntervalSelectedValue As String = AreaIntervalDropDownList.SelectedValue
        Dim DbRange As String

        If Not IsPostBack Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PlaceholderString", "syncScrollPos('EditPreviewPanel', " & EditPreviewPanel_ScrollPos & ");", True) 'set scrollbar positioning of EditPreviewPanel and ItemsPanel control
            'AreaDropDownList_SqlDataSource.SelectCommand = "SELECT A.Area, A.[Key] FROM [ALTS].[dbo].[T_LogArea] A LEFT JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE " & If(Session("AreaIntervalKey") Is Nothing OrElse Session("AreaIntervalKey") = "All", String.Empty, " A.IntervalKey=" & Session("AreaIntervalKey") & " AND") & " OneTimeDate IS NULL OR (OneTimeDate IS NOT NULL AND ((SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key])=0 OR (SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key]) IS NULL)) ORDER BY A.Area"
            AreaDropDownList_SqlDataSource.SelectCommand = GetAreaDdlSelectCommand()

            If AreaFromQueryString IsNot Nothing Then
                RefreshIframe()
                DepartmentInterfacePanel.Enabled = True
                QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
                    {"value", AreaFromQueryString},
                    {"typeOf", "int"}
                }
                AreaFormView_SqlDataSource.SelectCommand = "Select [Key], [Area] FROM [T_LogArea] WHERE [Key]=" & AreaFromQueryString
                AreaIntervalDropDownList.SelectedValue = Session("AreaIntervalKey")
                AreaDropDownList.SelectedValue = AreaFromQueryString

                If Not Boolean.Parse(Security.GetSingleDbField("SELECT Active FROM [ALTS].[dbo].[T_LogArea] WHERE [Key]=@AreaKey", QueryConfig, "Active")) Then
                    Dim AreaDisableButton As LinkButton = AreaFormView.FindControl("AreaDisableButton")

                    If AreaDisableButton IsNot Nothing Then
                        AreaDisableButton.Text = "Enable"
                    End If
                End If

                'if here, area ddl has a selected value. Thus, remove static ListItem from AreaDropDownList
                If AreaDropDownList.Items(0).Text = "Select Checklist..." Then
                    AreaDropDownList.Items.RemoveAt(0)
                End If

                'label interface
                If LabelFromQueryString Is Nothing Then
                    LabelFromQueryString = SetLabelFromQueryString()
                End If

                LabelFormView_SqlDataSource.SelectCommand = "SELECT [Key], [Label] FROM [T_LogLabel] WHERE [AreaKey]=" & AreaFromQueryString
                LabelDropDownList_SqlDataSource.SelectCommand = "SELECT [Key], [Label] FROM [T_LogLabel] WHERE [AreaKey]=" & AreaFromQueryString & " ORDER BY LabelOrder"
                LabelDropDownList.Items.Clear()
                LabelDropDownList.DataBind()

                If LabelFromQueryString IsNot Nothing Then
                    QueryConfig.Clear()
                    QueryConfig("@LabelKey") = New Dictionary(Of String, String) From {
                        {"value", LabelFromQueryString},
                        {"typeOf", "int"}
                    }
                    FieldType = Security.GetSingleDbField("SELECT FieldType FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@LabelKey", QueryConfig, "FieldType")
                    DbRange = Security.GetSingleDbField("SELECT Range From [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@LabelKey", QueryConfig, "Range")
                    Unit = Security.GetSingleDbField("SELECT U.[Key] FROM [ALTS].[dbo].[T_LogLabel] L INNER JOIN [ALTS].[dbo].[T_LogUnit] U ON L.UnitKey=U.[Key] WHERE L.[Key]=@LabelKey", QueryConfig, "Key")

                    'enable associated functionalities
                    LabelDropDownList.Enabled = True
                    LabelOrderInterfacePanel.Enabled = True
                    UnitInterfacePanel.Enabled = True
                    FieldType_DropDownList.Enabled = True
                    RangeOrderInterfacePanel.Enabled = True

                    UnitDropDownList.SelectedValue = Unit

                    LabelDropDownList.SelectedValue = LabelFromQueryString
                    LabelFormView_SqlDataSource.SelectCommand = "SELECT [Key], [Label] FROM [T_LogLabel] WHERE [Key]=" & LabelFromQueryString
                    LabelFormView_SqlDataSource.DataBind()

                    'set field type
                    FieldType_DropDownList.SelectedValue = If(FieldType Is Nothing, "", FieldType)

                    RangeOrderMenu_onClick(New Button(), EventArgs.Empty) 'reset range order (enable all menu buttons, hide any interface within DynamicRangeBoxPanel, & enable 'Set' button in bottom right)
                    If FieldType = "STC" Then
                        RangeOrderMenu.Style("visibility") = "hidden"
                        RangeOrderMenu.Style("height") = "0" 'to remove whitespace between RangeOrderLabel & DpPanel
                        DiffPanel.Visible = True
                        DiffTextbox.Text = If(DbRange IsNot Nothing AndAlso DbRange.Contains("+/-"), DbRange.Split(" ")(1), String.Empty)
                    ElseIf FieldType = "DP" Then
                        Dim DpNums As String() = If(DbRange Is Nothing, Nothing, DbRange.Split("&"))
                        RangeOrderLabel.Text = "Pump #'s"
                        RangeOrderMenu.Style("visibility") = "hidden"
                        RangeOrderMenu.Style("height") = "0" 'to remove whitespace between RangeOrderLabel & DpPanel
                        DpPanel.Visible = True
                        Pump1TextBox.Text = If(DpNums IsNot Nothing, Trim(DpNums(0)), String.Empty)
                        Pump2TextBox.Text = If(DpNums IsNot Nothing, Trim(DpNums(1)), String.Empty)
                    Else
                        SetRangeOrder(DbRange)
                    End If
                End If

                'comment interface
                CommentFormView_SqlDataSource.SelectCommand = "SELECT [Key], [Comment] FROM [T_LogCommentList] WHERE [AreaKey]=" & AreaFromQueryString
                CommentDropDownList_SqlDataSource.SelectCommand = "SELECT [Key], [Comment] FROM [T_LogCommentList] WHERE [AreaKey]=" & AreaFromQueryString & " ORDER BY CommentOrder"
                CommentDropDownList.Items.Clear()
                CommentDropDownList.DataBind()

                If CommentFromQueryString IsNot Nothing Then
                    'enable associated functionalities
                    CommentDropDownList.Enabled = True
                    CommentOrderInterface.Enabled = True

                    'prep functionalities that were enabled
                    CommentDropDownList.SelectedValue = CommentFromQueryString
                    CommentFormView_SqlDataSource.SelectCommand = "SELECT [Key], [Comment] FROM [T_LogCommentList] WHERE [Key]=" & CommentFromQueryString
                    CommentFormView_SqlDataSource.DataBind()
                End If

                'stamp interface
                StampInterfacePanel.Enabled = True

                'Department interface
                QueryConfig.Clear()
                QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
                    {"value", AreaFromQueryString},
                    {"typeOf", "int"}
                }
                Department = Security.GetSingleDbField("SELECT D.[Key] FROM [ALTS].[dbo].[T_LogArea] A INNER JOIN [ALTS].[dbo].[T_LogDepartment] D ON A.DepartmentKey=D.[Key] WHERE A.[Key]=@AreaKey", QueryConfig, "Key")
                DepartmentDropDownList.SelectedValue = Department

                'remove static ListItem for DepartmentDropDownList once user has selected an Department
                If Department IsNot Nothing Then
                    DepartmentDropDownList.Items(0).Enabled = False
                    IntervalInterfacePanel.Enabled = True
                End If

                'interval interface
                Try 'in case selected checklist does NOT have a set interval
                    IntervalDR = Security.GetMyDataSetParamQuery("SELECT A.OneTimeDate, A.Assignee, I.[Key], I.Interval, I.DisplayOrder FROM [ALTS].[dbo].[T_LogArea] A INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE A.[Key]=@AreaKey", QueryConfig).Tables(0).Rows(0)
                    IntervalKey = IntervalDR("Key")
                    Interval = IntervalDR("Interval")
                    IntervalDropDownList.SelectedValue = IntervalKey
                Catch ex As Exception
                    IntervalKey = Nothing
                End Try

                'remove static ListItem for IntervalDropDownList once user has selected an interval
                If IntervalKey IsNot Nothing Then
                    Dim DatepickText As String = If(IsDBNull(IntervalDR("OneTimeDate")), String.Empty, Date.Parse(IntervalDR("OneTimeDate")).Date)
                    Dim Assignee As String = If(IsDBNull(IntervalDR("Assignee")), Nothing, IntervalDR("Assignee"))

                    If IntervalDropDownList.Items(0).Text = "Select Interval..." Then
                        IntervalDropDownList.Items.RemoveAt(0)
                    End If

                    If Interval = "DAILY" Then 'doing this up here, b/c the next if statement needs these changes to already be made
                        UserAssigneeButton.Visible = False

                        'reconfigure ShiftDropDownList ListItem controls
                        For Each ListItem As ListItem In ShiftDropDownList.Items
                            If ListItem.Text <> ShiftDropDownList.Items(0).Text Then
                                ListItem.Enabled = False
                            End If
                        Next

                        CreateListItem(New Dictionary(Of String, String) From {{"Parent", "ShiftDropDownList"}, {"Text", "Day Shift"}, {"Value", "Day Shift"}})
                        CreateListItem(New Dictionary(Of String, String) From {{"Parent", "ShiftDropDownList"}, {"Text", "Night Shift"}, {"Value", "Night Shift"}})
                        CreateListItem(New Dictionary(Of String, String) From {{"Parent", "ShiftDropDownList"}, {"Text", "Days (M-F)"}, {"Value", "Days (M-F)"}})
                    End If

                    'determine which ddl in AssigneeDdlPanel to display & its pre selected value
                    If Assignee Is Nothing Then
                        GenericDropDownList.Visible = True
                    ElseIf ShiftDropDownList.Items.FindByText(Assignee) IsNot Nothing Then
                        ShiftDropDownList.SelectedValue = Assignee
                        AssignToMenu_onClick(ShiftAssigneeButton, EventArgs.Empty)
                    Else 'REFACTOR!!! Find a more efficient solution to determine if User DropDownList should be shown
                        QueryConfig("@Assignee") = New Dictionary(Of String, String) From {
                            {"value", Assignee},
                            {"typeOf", "string"}
                        }
                        If Security.GetSingleDbField("SELECT COUNT(Assignee) As Assignee FROM [ALTS].[dbo].[T_LogArea] WHERE [Key]=@AreaKey AND Assignee=@Assignee", QueryConfig, "Assignee") = "1" Then
                            UsersDropDownList.DataBind()
                            UsersDropDownList.SelectedValue = Assignee
                            AssignToMenu_onClick(UserAssigneeButton, EventArgs.Empty)
                        End If
                        QueryConfig.Remove("@Assignee")
                    End If

                    If Interval = "ONE TIME ONLY" Then
                        OneTimeDatepickPanel.Visible = True
                        DatepickTextBox.Text = DatepickText

                        If String.IsNullOrEmpty(DatepickText) Then 'disable AssigneeInterfacePanel if a date is NOT set
                            AssigneeInterfacePanel.Enabled = False
                            Exit Sub
                        End If
                    ElseIf IntervalDR("DisplayOrder") > 5 Then 'DisplayOrder 5 is MONTHLY. Examples would be quarterly, bi-annual, 1 year, 2 year, etc.
                        AssigneeInterfacePanel.Visible = False
                    End If

                    'if 1 or more associated records exist in T_LogData, disable interval ddl
                    If Security.GetSingleDbField("SELECT COUNT([Key]) As NumOfLogs FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=@AreaKey", QueryConfig, "NumOfLogs") >= 1 Then
                        IntervalDropDownList.Enabled = False
                    End If
                End If

            Else 'if here, user has just opened the webpage
                'delete topmost dead record (dead = no records In T_LogData, And has been more 30 days since creation Of record In T_LogArea And (has a NULL Interval, Department, Or no associated Labels))
                Dim TopmostDeadRecordKey As String = Security.GetSingleDbField("SELECT [Key] FROM [ALTS].[dbo].[T_LogArea] A WHERE (SELECT COUNT([Key]) FROM [ALTS].[dbo].[T_LogData] D WHERE A.[Key]= D.AreaKey) = 0  And ABS(DATEDIFF(Day, GETDATE(), A.DateCreated)) > 30 And (A.IntervalKey Is NULL Or A.DepartmentKey Is NULL Or (Select COUNT([Key]) FROM [ALTS].[dbo].[T_LogLabel] L WHERE L.AreaKey=A.[Key]) = 0)", New Dictionary(Of String, Dictionary(Of String, String)), "Key")
                If TopmostDeadRecordKey IsNot Nothing Then
                    QueryConfig("@TopDeadRecord") = New Dictionary(Of String, String) From {
                        {"value", TopmostDeadRecordKey},
                        {"typeOf", "string"}
                    }
                    Security.ExecuteSqlParamQuery("DELETE FROM [ALTS].[dbo].[T_LogArea] WHERE [Key]=@TopDeadRecord", QueryConfig)
                    QueryConfig.Remove("@TopDeadRecord")
                End If
            End If

        Else
            Session("AreaIntervalKey") = AreaIntervalDropDownList.SelectedValue
        End If
    End Sub

    Protected Sub Page_PreRenderComplete(sender As Object, e As EventArgs) Handles Me.PreRenderComplete
        Dim ListItemStylesDS As Data.DataSet = Security.GetMyDataSetParamQuery("SELECT A.[Key], CASE WHEN Active=0 THEN 'background-color: lightgray; color: gray;' WHEN IntervalKey IS NULL OR DepartmentKey IS NULL OR Assignee IS NULL OR (SELECT COUNT([Key]) FROM [ALTS].[dbo].[T_LogLabel] L WHERE L.AreaKey=A.[Key]) = 0 THEN 'background-color: red; color: black;' ELSE 'color: black;' END AS ListItemStyles FROM [ALTS].[dbo].[T_LogArea] A", New Dictionary(Of String, Dictionary(Of String, String)))
        Dim ListItemStylesRC As Integer = ListItemStylesDS.Tables(0).Rows.Count - 1
        Dim ListItemStylesDR As Data.DataRow
        Dim AreaListItem As ListItem

        'write routine that gets the checklists in AreaDropDownList w/ no labels, interval, or department. Make the ForeColor of the associated ListItem control red
        For I = 0 To ListItemStylesRC
            ListItemStylesDR = ListItemStylesDS.Tables(0).Rows(I)
            AreaListItem = AreaDropDownList.Items.FindByValue(ListItemStylesDR("Key"))

            If AreaListItem IsNot Nothing Then
                AreaListItem.Attributes.Add("style", ListItemStylesDR("ListItemStyles"))
            End If
        Next

        'prevent default FormView behavior when 'Insert' linkbutton is clicked and associated TextBox is empty
        If AreaFromQueryString IsNot Nothing Then
            If FormViewInsert Is Nothing Then
                Exit Sub
            End If
        End If

        'if FormViewInsert has a value, that means that FormView control is in insert mode, and IT NEEDS TO STAY THERE
        If FormViewInsert IsNot Nothing Then
            FormViewInsert.ChangeMode(FormViewMode.Insert)
        End If

        FormViewInsert = Nothing
    End Sub

    Function GetAreaDdlSelectCommand() As String 'this query is used in several areas, but needs to use the current value in Session("AreaIntervalKey"). That is why it in a function
        Return "SELECT A.Area, A.[Key] FROM [ALTS].[dbo].[T_LogArea] A LEFT JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE " & If(Session("AreaIntervalKey") Is Nothing OrElse Session("AreaIntervalKey") = "All", String.Empty, " (A.IntervalKey=" & Session("AreaIntervalKey") & " OR (A.IntervalKey IS NULL AND DATEDIFF(DAY, A.DateCreated, GETDATE()) = 0)) AND") & " OneTimeDate IS NULL OR (OneTimeDate IS NOT NULL AND ((SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key])=0 OR (SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key]) IS NULL)) ORDER BY A.Area"
    End Function

    Sub CreateListItem(Config As Dictionary(Of String, String))
        Dim ListItem2 As New ListItem()
        ListItem2.Text = Config("Text")
        ListItem2.Value = Config("Value")
        CType(EditPreviewPanel.FindControl(Config("Parent")), DropDownList).Items.Add(ListItem2)
    End Sub

    Protected Sub DynamicButton_Click(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Dim Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    Function CallSqlFunction(Query As String) As Boolean

    End Function

    Function StripString(ByVal input As String) As String
        Return Regex.Replace(input, "[^a-zA-Z0-9]", "").ToLower()
    End Function

    Private Sub SetControlsEnabledProp(container As Control, EnabledValue As Boolean)
        For Each ctrl As Control In container.Controls
            If TypeOf ctrl Is WebControl Then
                DirectCast(ctrl, WebControl).Enabled = EnabledValue
            End If

            If EnabledValue And TypeOf ctrl Is CheckBox Then 'uncheck CheckBox elements during hourly rollover
                DirectCast(ctrl, CheckBox).Checked = False
            End If

            ' Recursively process child controls
            If ctrl.HasControls() Then
                SetControlsEnabledProp(ctrl, EnabledValue)
            End If
        Next
    End Sub


    Sub UploadToDataTable()
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = "Data Source=PWI-31\SATIDB;Initial Catalog=ALTS;Persist Security Info=True;User ID=sati;Password=laptopia"
        Connection.Open()

        Dim My_DA As New Data.SqlClient.SqlDataAdapter
        Dim My_DS As New Data.DataSet
        Dim My_DR As Data.DataRow
        '*****************************************************************
        '************************Select***********************************
        '*****************************************************************
        Dim MySelectCmd As New System.Data.SqlClient.SqlCommand
        With MySelectCmd
            .CommandText = MostRecentRec
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
            .CommandText = "UPDATE T_LogData SET [Inputs] = @Inputs, [OutOfRange] = @OutOfRange, [Date] = @Date, [Operator] = @Operator, [CompleteLog] = @CompleteLog WHERE [Key]=@DataLogKey; SELECT TOP(1) * FROM T_LogData WHERE AreaKey=" & AreaKeyFromDropDownList & " ORDER BY Date DESC;"
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
            My_DR = My_DS.Tables(0).Rows(0)
            My_DR.AcceptChanges()
            My_DR.BeginEdit()

            If Not IsDBNull(My_DR("Operator")) Then
                If My_DR("Operator") <> User.Identity.Name.ToString Then
                    Throw New Exception(ReadOnlyMessage)
                End If
            End If

            My_DR("Operator") = User.Identity.Name.ToString
            My_DR("CompleteLog") = False
            My_DR("Inputs") = JsonSerializer.Serialize(LabelInputMap)
            My_DR("OutOfRange") = JsonSerializer.Serialize(LabelOutOfRangeMap)
            My_DR("Date") = System.DateTime.Now.ToShortTimeString
            My_DR.EndEdit()
            My_DA.Update(My_DS, "T_LogData")
        Catch ex As Exception
        End Try
        Connection.Close()
    End Sub

    Function SetCommentFromQueryString() As String
        Return Security.GetSingleDbField("SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogCommentList] WHERE AreaKey=@AreaKey ORDER BY CommentOrder", QueryConfig, "Key")
    End Function

    Function SetLabelFromQueryString() As String
        Return Security.GetSingleDbField("SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=@AreaKey ORDER BY LabelOrder", QueryConfig, "Key")
    End Function

    Protected Sub AreaDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
        AreaFromQueryString = AreaDropDownList.SelectedValue
        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaFromQueryString},
            {"typeOf", "int"}
        }
        LabelFromQueryString = SetLabelFromQueryString()
        CommentFromQueryString = SetCommentFromQueryString()
        RefreshPreview()
    End Sub

    Protected Sub LabelDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaFromQueryString},
            {"typeOf", "int"}
        }
        LabelFromQueryString = sender.SelectedValue
        CommentFromQueryString = SetCommentFromQueryString()
        RefreshPreview()
    End Sub

    Protected Sub CommentDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
        CommentFromQueryString = CommentDropDownList.SelectedValue
        RefreshPreview()
    End Sub

    Protected Sub IntervalDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim IntervalDdlValue As Integer = IntervalDropDownList.SelectedValue
        Dim IntervalDdlText As String = IntervalDropDownList.SelectedItem.Text
        Dim UpdateQuery As String = "UPDATE [ALTS].[dbo].[T_LogArea] SET IntervalKey=@IntervalKey, OneTimeDate=NULL, Assignee"

        QueryConfig("@IntervalKey") = New Dictionary(Of String, String) From {
            {"value", IntervalDdlValue},
            {"typeOf", "int"}
        }
        If Security.GetSingleDbField("Select DisplayOrder FROM [ALTS].[dbo].[T_LogAreaInterval] WHERE [Key]=@IntervalKey", QueryConfig, "DisplayOrder") > 5 Then 'DisplayOrder 5 is MONTHLY. Examples would be bi-annual, 1 year, 2 year, etc.
            QueryConfig("@Assignee") = New Dictionary(Of String, String) From {
                {"value", IntervalDdlText},
                {"typeOf", "string"}
            }
            UpdateQuery += "=@Assignee"
        Else
            UpdateQuery += "=NULL" 'not using a parameterized value in this case, b/c the value in the DB will not be a true 'NULL', but rather a string that equals 'NULL'
        End If
        UpdateQuery += " WHERE [Key]=@AreaKey"

        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaFromQueryString},
            {"typeOf", "int"}
        }

        Security.ExecuteSqlParamQuery(UpdateQuery, QueryConfig)
        RefreshPreview()
    End Sub

    Protected Sub DepartmentDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaFromQueryString},
            {"typeOf", "int"}
        }
        QueryConfig("@Department") = New Dictionary(Of String, String) From {
            {"value", DepartmentDropDownList.SelectedValue},
            {"typeOf", "string"}
        }
        Security.ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogArea] SET DepartmentKey=@Department WHERE [Key]=@AreaKey", QueryConfig)
        RefreshPreview()
    End Sub

    Protected Sub UnitDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
        QueryConfig("@LabelKey") = New Dictionary(Of String, String) From {
            {"value", LabelFromQueryString},
            {"typeOf", "int"}
        }
        QueryConfig("@UnitKey") = New Dictionary(Of String, String) From {
            {"value", UnitDropDownList.SelectedValue},
            {"typeOf", "string"}
        }
        Security.ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogLabel] SET UnitKey=@UnitKey WHERE [Key]=@LabelKey", QueryConfig)
        RefreshPreview()
    End Sub


    Protected Sub EditStampsButton_OnClick(sender As Object, e As EventArgs)
        'Response.Redirect(StampSelectPage & "?" & Request.RawUrl.Split("?")(1)) 'add querystrings from current url to webpage listed within StampSelectPage
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "iframeEnabled", "iframeEnabled(true);", True)
        PreviewPanel_iframe.Attributes.Add("src", StampSelectPage & "?" & Request.RawUrl.Split("?")(1))
    End Sub

    Sub RefreshPreview()
        Response.Redirect(WebpageUrl & "?EPP_ScrollPos=" & EditPreviewPanel_HiddenField.Value & If(AreaFromQueryString IsNot Nothing, "&Area=" & AreaFromQueryString, Nothing) & If(LabelFromQueryString IsNot Nothing, "&Label=" & LabelFromQueryString, Nothing) & If(CommentFromQueryString IsNot Nothing, "&Comment=" & CommentFromQueryString, Nothing))
    End Sub

    Sub RefreshIframe()
        PreviewPanel_iframe.Attributes.Add("src", "/ChecklistLogging/Log.aspx?Area=" & AreaFromQueryString)
    End Sub

    Sub SetRangeOrder(DbRange As String)
        Dim DbRangeDelimited As String()

        If DbRange IsNot Nothing Then
            If DbRange.Contains("-") Then
                DbRangeDelimited = DbRange.Split("-")
                LowerBoundTextbox.Text = DbRangeDelimited(0)
                UpperBoundTextbox.Text = DbRangeDelimited(1)
                RangeOrderMenu_onClick(RangePickButton, EventArgs.Empty)

                'empty TextBox controls within the other DynamicRangeBoxPanel child Panel controls
                LessThanTextbox.Text = ""
                GreaterThanTextbox.Text = ""
                DiffTextbox.Text = ""
            ElseIf DbRange.Contains("<") Then
                DbRangeDelimited = DbRange.Split("<")
                LessThanTextbox.Text = DbRangeDelimited(1)
                RangeOrderMenu_onClick(LessThanPickButton, EventArgs.Empty)

                'empty TextBox controls within the other DynamicRangeBoxPanel child Panel controls
                LowerBoundTextbox.Text = ""
                UpperBoundTextbox.Text = ""
                GreaterThanTextbox.Text = ""
                DiffTextbox.Text = ""
            ElseIf DbRange.Contains(">") Then
                DbRangeDelimited = DbRange.Split(">")
                GreaterThanTextbox.Text = DbRangeDelimited(1)
                RangeOrderMenu_onClick(GreaterThanPickButton, EventArgs.Empty)

                'empty TextBox controls within the other DynamicRangeBoxPanel child Panel controls
                LowerBoundTextbox.Text = ""
                UpperBoundTextbox.Text = ""
                LessThanTextbox.Text = ""
                DiffTextbox.Text = ""
            End If
        End If
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

    Protected Sub VerifyValue_Check(sender As Object, e As EventArgs)
        Dim Button As Button
        Dim Cbx As CheckBox
        Dim CheckedStatus As Boolean

        For Each Pnl As Panel In VisiblePanels
            If Pnl Is sender.Parent Then ' if Pnl holds the textbox that triggered this event
                For Each ctrl As Control In Pnl.Controls
                    If TypeOf ctrl Is Button Then
                        Button = CType(ctrl, Button)
                    ElseIf TypeOf ctrl Is CheckBox Then
                        Cbx = CType(ctrl, CheckBox)
                    End If
                Next
            End If
        Next

        CheckedStatus = Not sender.Checked 'because view state is reset after postback, the true Checked value is the opposite of the current one
        LabelOutOfRangeMap(Button.Text) = CheckedStatus
        Cbx.Checked = CheckedStatus
        UploadToDataTable()
    End Sub

    Sub SetEnabledProps(ButtonID As String, EnabledValue As Boolean)
        If ButtonID.Contains("Area") Then
            AreaDropDownList.Enabled = EnabledValue
            LabelInterfacePanel.Enabled = EnabledValue
            StampInterfacePanel.Enabled = EnabledValue
            CommentInterfacePanel.Enabled = EnabledValue
        ElseIf ButtonID.Contains("Label") Then
            LabelDropDownList.Enabled = EnabledValue
            AreaInterfacePanel.Enabled = EnabledValue
            StampInterfacePanel.Enabled = EnabledValue
            CommentInterfacePanel.Enabled = EnabledValue
            UnitInterfacePanel.Enabled = EnabledValue
            RefreshIframe()
        ElseIf ButtonID.Contains("Comment") Then
            CommentDropDownList.Enabled = EnabledValue
            AreaInterfacePanel.Enabled = EnabledValue
            LabelInterfacePanel.Enabled = EnabledValue
            StampInterfacePanel.Enabled = EnabledValue
            RefreshIframe()
        End If

        FieldType_DropDownList.Enabled = EnabledValue
        LabelOrderInterfacePanel.Enabled = EnabledValue
        RangeOrderInterfacePanel.Enabled = EnabledValue
        CommentOrderInterface.Enabled = EnabledValue
        IntervalInterfacePanel.Enabled = EnabledValue
        DepartmentInterfacePanel.Enabled = EnabledValue
    End Sub

    Protected Sub EditButton_OnClick(sender As Object, e As EventArgs)
        RefreshIframe()

        'disable all other controls
        SetEnabledProps(sender.ID, False)

        AreaKeyFromDropDownList = AreaDropDownList.SelectedValue
        AreaFormView_SqlDataSource.SelectCommand = "Select [Key], [Area] FROM [T_LogArea] WHERE [Key]=" & AreaFromQueryString

        If LabelFromQueryString IsNot Nothing Then
            LabelFormView_SqlDataSource.SelectCommand = "Select [Key], Label From T_LogLabel WHERE [Key]=" & LabelFromQueryString
            LabelFormView_SqlDataSource.DataBind()
        End If

        If CommentFromQueryString IsNot Nothing Then
            CommentFormView_SqlDataSource.SelectCommand = "Select [Key], Comment From T_LogCommentList WHERE [Key]=" & CommentFromQueryString
            CommentFormView_SqlDataSource.DataBind()
        End If
    End Sub

    Protected Sub UpdateButton_onClick(sender As Object, e As EventArgs)
        If sender.ID.Contains("Area") Then
            AreaFormView_SqlDataSource.UpdateCommand = "UPDATE [T_LogArea] SET Area='" & SqlProofSingleQuotes(sender.Parent.FindControl("AreaTextBox").Text) & "' WHERE [Key]=" & AreaDropDownList.SelectedValue
            AreaFormView_SqlDataSource.Update()
        ElseIf sender.ID.Contains("Label") Then
            LabelFormView_SqlDataSource.UpdateCommand = "UPDATE [T_LogLabel] SET Label='" & SqlProofSingleQuotes(sender.Parent.FindControl("LabelTextBox").Text) & "' WHERE [Key]=" & LabelDropDownList.SelectedValue
            LabelFormView_SqlDataSource.Update()
        ElseIf sender.ID.Contains("Comment") Then
            CommentFormView_SqlDataSource.UpdateCommand = "UPDATE [T_LogCommentList] SET Comment='" & SqlProofSingleQuotes(sender.Parent.FindControl("CommentTextBox").Text) & "' WHERE [Key]=" & CommentDropDownList.SelectedValue
            CommentFormView_SqlDataSource.Update()
            'ElseIf sender.ID.Contains("Stamp") Then
            '    StampFormView_SqlDataSource.UpdateCommand = "UPDATE [T_LogStampList] SET Title='" & sender.Parent.FindControl("StampTextBox").Text & "' WHERE [Key]=" & StampDropDownList.SelectedValue
            '    StampFormView_SqlDataSource.Update()
        End If

        RefreshPreview()
    End Sub

    Protected Sub UpdateCancelButton_OnClick(sender As Object, e As EventArgs)
        SetEnabledProps(sender.ID, True) 'enable currently disabled FormView and DropDownList controls
        RefreshPreview()
    End Sub

    Function SqlProofSingleQuotes(Text As String) As String
        Return Text.Replace("'", "''") 'escape single quotes (') by doubling them ('')
    End Function

    Protected Sub InsertButton_onClick(sender As Object, e As EventArgs)
        Dim UserInput As String
        Dim NewLabelOrder As Integer
        Dim NewCommentOrder As Integer

        If sender.ID.Contains("Area") Then
            Dim DS As Data.DataSet = SatiCode.GetMyDataSet("SELECT [Key] FROM [ALTS].[dbo].[T_LogStampTitle]")
            Dim RC As Integer = DS.Tables(0).Rows.Count
            Dim DR As Data.DataRow
            Dim DuplicateDS As Data.DataSet = SatiCode.GetMyDataSet("SELECT Area FROM [ALTS].[dbo].[T_LogArea] A LEFT JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE I.Interval <> 'ONE TIME ONLY' OR A.IntervalKey IS NULL")
            Dim DuplicateRC As Integer = DuplicateDS.Tables(0).Rows.Count
            Dim DuplicateDR As Data.DataRow
            UserInput = SqlProofSingleQuotes(sender.Parent.FindControl("AreaTextBox").Text)

            If String.IsNullOrEmpty(UserInput) Then
                FormViewInsert = AreaFormView 'Page_PreRenderComplete will ensure FormView stays in Insert mode
                Exit Sub
            End If

            'ensure checklist name does NOT currently exist in T_LogArea
            For J = 0 To DuplicateRC - 1
                DuplicateDR = DuplicateDS.Tables(0).Rows(J)
                Dim Area As String = DuplicateDR("Area")

                If StripString(UserInput) = StripString(Area) Then
                    FormViewInsert = AreaFormView 'Page_PreRenderComplete will ensure FormView stays in Insert mode
                    AreaErrorLabel.Text = "Error: '" & UserInput & "' checklist exists"
                    Exit Sub
                End If
            Next

            QueryConfig("@UserInput") = New Dictionary(Of String, String) From {
                {"value", UserInput},
                {"typeOf", "string"}
            }
            QueryConfig("@Date") = New Dictionary(Of String, String) From {
                {"value", Today},
                {"typeOf", "string"}
            }
            Security.ExecuteSqlParamQuery("INSERT INTO [ALTS].[dbo].[T_LogArea] (Area, DateCreated, Active) OUTPUT INSERTED.[Key] VALUES (@UserInput, @Date, 1);", QueryConfig)

            QueryConfig.Remove("@Date")
            AreaFromQueryString = Security.GetSingleDbField("SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogArea] WHERE IntervalKey IS NULL AND Area=@UserInput ORDER BY [Key] DESC", QueryConfig, "Key") 'get unique key value of checklist that was just created

            'add record to FROM [ALTS].[dbo].[T_LogStampList] for each stamp that exists in [ALTS].[dbo].[T_LogStampTitle]
            For I = 0 To RC - 1
                DR = DS.Tables(0).Rows(I)
                QueryConfig.Clear()

                QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
                    {"value", AreaFromQueryString},
                    {"typeOf", "int"}
                }
                QueryConfig("@StampKey") = New Dictionary(Of String, String) From {
                    {"value", DR("Key")},
                    {"typeOf", "int"}
                }

                Security.ExecuteSqlParamQuery("INSERT INTO [ALTS].[dbo].[T_LogStampList] (AreaKey, TitleKey, Active) VALUES (@AreaKey, @StampKey, 0);", QueryConfig)
            Next

            'b/c a new checklist has been created, there are ZERO associated Labels, Comments, or Stamps
            LabelFromQueryString = Nothing
            CommentFromQueryString = Nothing

            If AreaErrorLabel.Text <> "" Then
                AreaErrorLabel.Text = ""
            End If
        ElseIf sender.ID.Contains("Label") Then
            UserInput = SqlProofSingleQuotes(sender.Parent.FindControl("LabelTextBox").Text)
            If String.IsNullOrEmpty(UserInput) Then
                FormViewInsert = LabelFormView 'Page_PreRenderComplete will ensure FormView stays in Insert mode
                Exit Sub
            End If
            QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
                {"value", AreaFromQueryString},
                {"typeOf", "int"}
            }
            NewLabelOrder = Security.GetSingleDbField("SELECT TOP(1) LabelOrder FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=@AreaKey ORDER BY [Key] DESC", QueryConfig, "LabelOrder") + 1

            QueryConfig("@UserInput") = New Dictionary(Of String, String) From {
                {"value", UserInput},
                {"typeOf", "string"}
            }
            QueryConfig("@LabelOrder") = New Dictionary(Of String, String) From {
                {"value", NewLabelOrder},
                {"typeOf", "int"}
            }
            Security.ExecuteSqlParamQuery("INSERT INTO [ALTS].[dbo].[T_LogLabel] (AreaKey, Label, LabelOrder) VALUES (@AreaKey, @UserInput, @LabelOrder);", QueryConfig)
            LabelFromQueryString = Security.GetSingleDbField("SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=@AreaKey And Label=@UserInput And LabelOrder=@LabelOrder ORDER BY [Key] DESC", QueryConfig, "Key")

        ElseIf sender.ID.Contains("Comment") Then
            UserInput = SqlProofSingleQuotes(sender.Parent.FindControl("CommentTextBox").Text)
            If String.IsNullOrEmpty(UserInput) Then
                FormViewInsert = CommentFormView 'Page_PreRenderComplete will ensure FormView stays in Insert mode
                Exit Sub
            End If

            QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
                {"value", AreaFromQueryString},
                {"typeOf", "int"}
            }
            NewCommentOrder = Security.GetSingleDbField("SELECT TOP(1) CommentOrder FROM [ALTS].[dbo].[T_LogCommentList] WHERE AreaKey=@AreaKey ORDER BY [Key] DESC", QueryConfig, "CommentOrder") + 1

            QueryConfig("@UserInput") = New Dictionary(Of String, String) From {
                {"value", UserInput},
                {"typeOf", "string"}
            }
            QueryConfig("@CommentOrder") = New Dictionary(Of String, String) From {
                {"value", NewCommentOrder},
                {"typeOf", "int"}
            }
            Security.ExecuteSqlParamQuery("INSERT INTO [ALTS].[dbo].[T_LogCommentList] (AreaKey, Comment, CommentOrder) VALUES (@AreaKey, @UserInput, @CommentOrder);", QueryConfig)
            CommentFromQueryString = Security.GetSingleDbField("SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogCommentList] WHERE AreaKey=@AreaKey And Comment=@UserInput And CommentOrder=@CommentOrder ORDER BY [Key] DESC", QueryConfig, "Key")
        End If

        RefreshPreview()
    End Sub

    Protected Sub InsertCancelButton_onClick(sender As Object, e As EventArgs)
        Response.Redirect(Request.Url.AbsoluteUri) 'redirect to current url w/querystrings 
    End Sub

    Protected Sub NewButton_onClick(sender As Object, e As EventArgs)
        SetEnabledProps(sender.ID, False)
    End Sub

    Protected Sub DisableButton_onClick(sender As Object, e As EventArgs)
        QueryConfig("@Active") = New Dictionary(Of String, String) From {
            {"value", If(sender.Text = "Disable", False, True)},
            {"typeOf", "bit"}
        }
        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaFromQueryString},
            {"typeOf", "int"}
        }
        Security.ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogArea] SET Active=@Active WHERE [Key]=@AreaKey", QueryConfig)
        RefreshPreview()
    End Sub

    Protected Sub FieldType_OnSelectedIndexChanged(sender As Object, e As EventArgs)
        Dim FieldType As String = sender.SelectedValue
        Dim UpdateQuery As String = "UPDATE [ALTS].[dbo].[T_LogLabel] Set FieldType="

        QueryConfig("@LabelKey") = New Dictionary(Of String, String) From {
            {"value", LabelDropDownList.SelectedValue},
            {"typeOf", "int"}
        }
        If String.IsNullOrEmpty(FieldType) = False Then
            QueryConfig("@FieldType") = New Dictionary(Of String, String) From {
                {"value", FieldType},
                {"typeOf", "string"}
            }
            UpdateQuery += "@FieldType"
        Else
            UpdateQuery += "NULL" 'not using a parameterized value in this case, b/c the value in the DB will not be a true 'NULL', but rather a string that equals 'NULL'
        End If

        Security.ExecuteSqlParamQuery(UpdateQuery & " WHERE [Key]=@LabelKey", QueryConfig)
        RefreshPreview()
    End Sub

    Protected Sub AreaInterval_OnSelectedIndexChanged(sender As Object, e As EventArgs)
        AreaFromQueryString = GetSingleDbField(GetAreaDdlSelectCommand().Insert(6, " TOP(1)"), "Key") 'not using Security class GetSingleDbField function, b/c of the use of GetAreaDdlSelectCommand() function in Page_Load
        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaFromQueryString},
            {"typeOf", "int"}
        }
        LabelFromQueryString = SetLabelFromQueryString()
        CommentFromQueryString = SetCommentFromQueryString()
        RefreshPreview()
    End Sub

    Protected Sub CommentOrderInterface_onClick(sender As Object, e As EventArgs)
        Dim Action As String
        Dim UpdateQuery As String
        Dim ModifyOrderConfig As New Dictionary(Of String, String)

        Select Case sender.ID
            Case "UpInOrderCommentButton"
                Action = "up"
            Case "DownInOrderCommentButton"
                Action = "down"
        End Select

        ModifyOrderConfig = ChecklistBuilder.ModifyOrder(CommentFromQueryString, Action, "Comment")
        UpdateQuery = ModifyOrderConfig("SqlQuery")
        If String.IsNullOrEmpty(UpdateQuery) = False Then
            Security.ExecuteSqlParamQuery(UpdateQuery, JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderConfig("ParameterizedValues")))
            RefreshPreview()
        End If
    End Sub
    Protected Sub LabelOrderInterface_onClick(sender As Object, e As EventArgs)
        Dim Action As String
        Dim UpdateQuery As String
        Dim ModifyOrderConfig As New Dictionary(Of String, String)

        Select Case sender.ID
            Case "UpInOrderLabelButton"
                Action = "up"
            Case "DownInOrderLabelButton"
                Action = "down"
        End Select

        ModifyOrderConfig = ChecklistBuilder.ModifyOrder(LabelFromQueryString, Action, "Label")
        UpdateQuery = ModifyOrderConfig("SqlQuery")
        If String.IsNullOrEmpty(UpdateQuery) = False Then
            Security.ExecuteSqlParamQuery(UpdateQuery, JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderConfig("ParameterizedValues")))
            RefreshPreview()
        End If
    End Sub

    Protected Sub RangeOrderMenu_onClick(sender As Object, e As EventArgs)
        'iterate through child controls within RangeOrderMenu, check if it's it the sender. If so, add. Otherwise, enable

        'iterate through child controls within RangeOrderMenu, check if it's it the sender. If so, disable. Otherwise, enable
        For Each Ctrl In RangeOrderMenu.Controls
            If TypeOf Ctrl Is Button Then
                If Ctrl.ID = sender.ID Then
                    Ctrl.Enabled = False
                Else
                    Ctrl.Enabled = True
                End If
            End If
        Next

        'iterate through child controls within DynamicRangeBoxPanel, check if it's Panel with ID of sender control custom 'InterfacePanel' attribute. If so, make visible. Otherwise, make invisible
        For Each Ctrl In DynamicRangeBoxPanel.Controls
            If TypeOf Ctrl Is Panel Then
                If Ctrl.ID = sender.Attributes("InterfacePanel") Then
                    Ctrl.Visible = True
                Else
                    Ctrl.Visible = False
                End If
            End If
        Next

        SetRangeButton.Enabled = True
        ResetRangeButton.Enabled = True
        InvalidInputLabel.Visible = False 'hide error message from user
    End Sub

    Protected Sub AssignToMenu_onClick(sender As Object, e As EventArgs)
        'iterate through child controls within RangeOrderMenu, check if it's it the sender. If so, disable. Otherwise, enable
        For Each Ctrl In AssignToMenuPanel.Controls
            If TypeOf Ctrl Is Button Then
                If Ctrl.Text = sender.Text Then
                    Ctrl.Enabled = False
                Else
                    Ctrl.Enabled = True
                End If
            End If
        Next

        'iterate through child controls within DynamicRangeBoxPanel, check if it's Panel with ID of sender control custom 'InterfacePanel' attribute. If so, make visible. Otherwise, make invisible
        For Each Ctrl In AssigneeDdlPanel.Controls
            If TypeOf Ctrl Is DropDownList Then
                Dim DropDownList As DropDownList = CType(EditPreviewPanel.FindControl(Ctrl.ID), DropDownList)

                If Ctrl.ID = sender.Attributes("Ddl") Then
                    Ctrl.Visible = True

                    If Not DropDownList.SelectedItem.Text.Contains("Assign") AndAlso DropDownList.Items(0).Text.Contains("Assign") Then
                        DropDownList.Items(0).Enabled = False
                    End If
                Else
                    Ctrl.Visible = False
                End If
            End If
        Next
    End Sub

    'Protected Sub GeneralAssigneeButton_OnClick(sender As Object, e As EventArgs)
    '    AssignToMenu_onClick(GeneralAssigneeButton, EventArgs.Empty)
    '    Assignee_SelectedIndexChanged(GenericDropDownList, EventArgs.Empty)
    'End Sub

    Protected Sub Assignee_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim Assignee As String = sender.SelectedValue
        ExecuteSqlQuery("UPDATE [ALTS].[dbo].[T_LogArea] SET Assignee=" & If(Assignee = "NULL", "NULL", "'" & Assignee & "'") & " WHERE [Key]=" & AreaFromQueryString)
        RefreshPreview()
    End Sub

    Protected Sub ResetRangeButton_onClick(sender As Object, e As EventArgs)
        ExecuteSqlQuery("UPDATE [ALTS].[dbo].[T_LogLabel] SET Range=NULL WHERE [Key]=" & LabelFromQueryString)
        RefreshPreview()
    End Sub


    Protected Sub SetRangeButton_onClick(sender As Object, e As EventArgs)
        Dim UserInput As Double
        Dim UserInput2 As Double
        Dim DbRange As String

        If RangePanel.Visible Then
            If Double.TryParse(LowerBoundTextbox.Text, UserInput) And Double.TryParse(UpperBoundTextbox.Text, UserInput2) And UserInput < UserInput2 Then
                DbRange = UserInput & "-" & UserInput2
                InvalidInputLabel.Visible = False 'hide error message from user
            Else
                InvalidInputLabel.Visible = True
                Exit Sub
            End If

        ElseIf LessThanPanel.Visible Then
            If Double.TryParse(LessThanTextbox.Text, UserInput) Then
                DbRange = "<" & UserInput
                InvalidInputLabel.Visible = False 'hide error message from user
            Else
                InvalidInputLabel.Visible = True
                Exit Sub
            End If

        ElseIf GreaterThanPanel.Visible Then
            If Double.TryParse(GreaterThanTextbox.Text, UserInput) Then
                DbRange = ">" & UserInput
                InvalidInputLabel.Visible = False 'hide error message from user
            Else
                InvalidInputLabel.Visible = True
                Exit Sub
            End If

        ElseIf DiffPanel.Visible Then
            If Double.TryParse(DiffTextbox.Text, UserInput) Then
                DbRange = "+/- " & UserInput
                InvalidInputLabel.Visible = False 'hide error message from user
            Else
                InvalidInputLabel.Visible = True
                Exit Sub
            End If

        ElseIf DpPanel.Visible Then
            If Double.TryParse(Pump1TextBox.Text, UserInput) And Double.TryParse(Pump2TextBox.Text, UserInput2) Then
                DbRange = Pump1TextBox.Text & " & " & Pump2TextBox.Text 'grabbing values from TextBox controls rather than UserInput variables to keep leading zeros user inputs
                InvalidInputLabel.Visible = False 'hide error message from user
            Else
                InvalidInputLabel.Visible = True
                Exit Sub
            End If
        End If

        ExecuteSqlQuery("UPDATE [ALTS].[dbo].[T_LogLabel] SET Range='" & DbRange & "' WHERE [Key]=" & LabelFromQueryString)
        RefreshPreview()
    End Sub

    Sub ExecuteSqlQuery(SqlQuery As String)
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim MySQLCommand As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = Session("DBConnect")
        Connection.Open()
        With MySQLCommand
            .CommandText = SqlQuery
            .Connection = Connection
        End With
        MySQLCommand.ExecuteNonQuery()
        Connection.Close()
    End Sub

    Protected Sub DatepickCalendar_OnSelectionChanged(sender As Object, e As EventArgs)
        DatepickTextBox.Text = sender.SelectedDate.Date
        ExecuteSqlQuery("UPDATE [ALTS].[dbo].[T_LogArea] SET OneTimeDate='" & DatepickTextBox.Text & "' WHERE [Key]=" & AreaFromQueryString & "; UPDATE [ALTS].[dbo].[T_LogData] SET Date='" & DatepickTextBox.Text & "' WHERE AreaKey=" & AreaFromQueryString) 'update smalldatetime fields in T_LogArea & T_LogData
        EditDatepickButton.Enabled = True
        DatepickCalendar.Visible = False
    End Sub

    Protected Sub DatepickCalendar_OnDayRender(sender As Object, e As DayRenderEventArgs)
        If e.Day.Date = Today.Date Then
            e.Cell.BackColor = System.Drawing.Color.LightGray
            e.Cell.ForeColor = System.Drawing.Color.DarkGray
        End If
    End Sub

    Protected Sub EditDatepickButton_OnClick(sender As Object, e As EventArgs)
        EditDatepickButton.Enabled = False
        DatepickCalendar.Visible = True
    End Sub

    Protected Sub UsersDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
        ExecuteSqlQuery("UPDATE [ALTS].[dbo].[T_LogArea] SET Assignee='" & sender.SelectedValue & "' WHERE [Key]=" & AreaFromQueryString)
    End Sub
End Class

