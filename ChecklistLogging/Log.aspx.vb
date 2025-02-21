Imports System.Text.Json
Imports System.Web.Services

Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim VisiblePanels As New List(Of Panel)
    Dim ValidTextBoxes As New List(Of TextBox)
    Dim VisibleCheckBoxes As New List(Of CheckBox)
    Dim LabelOutOfRangeMap As New Dictionary(Of Integer, Boolean?)
    Dim TbxToRange As New Dictionary(Of Integer, String)
    Dim AreaFromQueryString As String
    Dim KeyFromQueryString As String
    Dim TimeForNewLog As Boolean
    Dim LogDS As New Data.DataSet
    Dim LogDR As Data.DataRow
    Dim ReadOnlyMessage As String = "Read-Only Mode"
    Dim WebpageUrl As String = "/ChecklistLogging/Log.aspx"
    Dim MostRecentRec As String
    Dim ItemsPanel_ScrollPos As String
    Dim DS As Data.DataSet
    Dim DR As Data.DataRow
    Dim DRC As Data.DataRowCollection
    Dim JsFunctionCalls As String
    Dim DBConnections As String

    <WebMethod()>
    Public Shared Function DbWrite(SenderID As String, SenderValue As String) As String
        Dim ModifyInput As ModifyInputDelegate = HttpContext.Current.Session("ModifyInput")
        Return ModifyInput(SenderID, SenderValue)  'Return a response back to the JavaScript function
    End Function

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        ClientScript.RegisterStartupScript(Me.GetType(), "SetHoverEffect", "syncScrollPos('ItemsPanel', " & ItemsPanel_ScrollPos & "); setFooterAtBottom(); " & JsFunctionCalls, True)
        ClientScript.RegisterStartupScript(Me.GetType(), "SetDBConnections", DBConnections, True)
    End Sub

    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim I As Integer = 0
        Dim II As Integer = 0
        Dim RC As Integer = 0
        Dim Area As String = ""
        Dim SB As New StringBuilder
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim DR1 As Data.DataRow
        Dim myButton As Button
        Dim myTextBox As TextBox
        Dim LabelKey As Integer
        Dim Range As String
        Dim Unit As String
        Dim PhotoDS As Data.DataSet
        Dim PhotoDR As Data.DataRow
        Dim PhotoRC As Integer
        Dim ImageUrl As String

        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        'MenuAuthenication.CheckGroupAuthenication("Office", Server)
        Me.MaintainScrollPositionOnPostBack = True
        AreaFromQueryString = Request.QueryString("Area")
        KeyFromQueryString = Request.QueryString("Key")
        ItemsPanel_ScrollPos = Request.QueryString("IP_ScrollPos")
        Page.MaintainScrollPositionOnPostBack = True

        If KeyFromQueryString IsNot Nothing Then 'if this is true, displaying Log.aspx for operator to fill out
            Dim ModifyInputDelegate As ModifyInputDelegate = AddressOf ModifyInput
            Session("ModifyInput") = ModifyInputDelegate

            ScriptManager.GetCurrent(Me.Page).EnablePageMethods = True

            DS = SatiCode.GetMyDataSet("SELECT TOP (100) A.[Key] As AreaKey, Area, I.SqlFunc, I.SqlFunc2ndArg, L.Label As Label, L.[Key] As LabelKey, L.Range As Range, L.TbxOverlay, L.CheckboxOverTextbox, U.Unit From [ALTS].[dbo].[T_LogArea] A INNER JOIN [ALTS].[dbo].[T_LogData] D ON A.[Key]=D.AreaKey INNER JOIN [ALTS].[dbo].[T_LogLabel] L ON A.[Key]=L.AreaKey LEFT JOIN [ALTS].[dbo].[T_LogUnit] U ON L.UnitKey=U.[Key] INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE D.[Key]=" & KeyFromQueryString & " ORDER BY L.LabelOrder")
            RC = DS.Tables(0).Rows.Count

            MostRecentRec = "SELECT Top(1) * FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=" & KeyFromQueryString & " ORDER BY Date DESC"

            CommentSqlDataSource.SelectCommand = "SELECT OpComments.[Key], OpComments.Comment FROM [ALTS].[dbo].[T_LogOperatorComments] OpComments WHERE OpComments.CommentKey=(Select Top(1) [Key] FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=" & KeyFromQueryString & " ORDER BY Date DESC)"
            CommentGridView.DataBind()
            If CommentGridView.Rows.Count > 0 Then
                CommentGridView.Visible = True
            End If

            ShiftSqlDataSource.SelectCommand = "SELECT Top(1) Shift FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=" & KeyFromQueryString & " ORDER BY Date Desc"
            ShiftSqlDataSource.DataBind()

            ClientScript.RegisterStartupScript(Me.GetType(), "callFunction", "textboxFocus(" & SatiCode.GetMyDataSet(MostRecentRec).Tables(0).Rows(0)("KeyOfLastLabel") & ");", True) 'set focus on current textbox noted in DB

            DR1 = DS.Tables(0).Rows(0)

            AreaFromQueryString = DR1("AreaKey")
            TitleLabel.Text = DR1("Area")

            'LogDS = SatiCode.GetMyDataSet("SELECT  * FROM " & DR1("SqlFunc") & "(" & AreaFromQueryString & ", 1, '" & System.DateTime.Now.ToString() & "')")
            LogDS = SatiCode.GetMyDataSet("SELECT Top(1) * From [ALTS].[dbo].[T_LogData] D CROSS APPLY (SELECT * FROM " & DR1("SqlFunc") & "(" & AreaFromQueryString & ", " & DR1("SqlFunc2ndArg") & ", '" & GetSingleDbField("SELECT Date FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=" & KeyFromQueryString, "Date") & "')) DA WHERE [Key]=" & KeyFromQueryString & " ORDER BY Date DESC")
            LogDR = LogDS.Tables(0).Rows(0)
            TimeForNewLog = LogDR("TimeForNewLog")

            DateLabel.Text = LogDR("DatePeriod")

            Session("LabelInputMap") = JsonSerializer.Deserialize(Of Dictionary(Of Integer, String))(LogDR("Inputs")) 'LabelInputMap is a session state variable so WebMethod (static) function has access to it
            LabelOutOfRangeMap = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Boolean?))(LogDR("OutOfRange"))

            If LogDR("CompleteLog") Then 'disable controls if log is complete
                SetControlsEnabledProp(ItemsPanel, False)
                HeaderPanel.Enabled = True
                AddCommentPanel.Enabled = False
                DoneButton.Enabled = False
            End If

            PhotoDS = SatiCode.GetMyDataSet("SELECT * FROM [ALTS].[dbo].[T_LogDataPhotos] WHERE DataKey=" & KeyFromQueryString)
            PhotoRC = PhotoDS.Tables(0).Rows.Count

            For I = 0 To PhotoRC - 1
                PhotoDR = PhotoDS.Tables(0).Rows(I)

                Dim Panel As New Panel()
                Dim Image As New Image()
                Dim LinkButton As New LinkButton()
                Dim FileName As String = PhotoDR("FileName")

                Panel.Attributes.Add("style", "position: relative")

                Image.Attributes.Add("style", "display: none; background: white; border: 2px solid black; border-radius: var(--UWhitespace); padding: var(--UWhitespace); max-width: 50vw; max-height: 50vh;")
                ImageUrl = PhotoDR("PhotoFilePath")

                LinkButton.ID = FileName & "_LinkButton"
                LinkButton.Text = PhotoDR("PhotoTitle")

                Panel.Controls.Add(Image)
                Panel.Controls.Add(LinkButton)
                ImageHoverLinkPanel.Controls.Add(Panel)

                JsFunctionCalls += "SetHoverEffect('" & LinkButton.ID & "', '" & ImageUrl.Replace("\", "\\") & "'); "
            Next

        ElseIf AreaFromQueryString IsNot Nothing Then  'if this is true, displaying webpage in iframe within ChecklistBuilder.aspx
            DS = SatiCode.GetMyDataSet("SELECT TOP (100) Area, L.Label As Label, L.[Key] As LabelKey, L.Range As Range, L.TbxOverlay, L.CheckboxOverTextbox, U.Unit From [ALTS].[dbo].[T_LogArea] A INNER JOIN [ALTS].[dbo].[T_LogLabel] L ON A.[Key]=L.AreaKey LEFT JOIN [ALTS].[dbo].[T_LogUnit] U ON L.UnitKey=U.[Key] WHERE A.[Key]=" & AreaFromQueryString & " ORDER BY L.LabelOrder")
            RC = DS.Tables(0).Rows.Count

            TitleLabel.Text = GetSingleDbField("SELECT Area FROM [ALTS].[dbo].[T_LogArea] WHERE [Key]=" & AreaFromQueryString, "Area")

            Try 'using a try catch block in case DS has no records, which mean the user is building a new checklist
                DR1 = DS.Tables(0).Rows(0)
            Catch ex As Exception
                DateLabel.Text = "Date"
                HeaderPanel.Enabled = False
                AddCommentPanel.Enabled = False
                BuildDynamicAsp()
                Exit Sub
            End Try

            'refresh Panel controls to prevent overlapping inputs from another checklist
            For Each control As Control In ItemsPanel.Controls
                If TypeOf control Is Panel Then
                    If control.ID.Contains("Panel") Then 'Ex: Panel0, Panel1, Panel2, etc.
                        CType(control, Panel).Visible = False
                    End If
                End If
            Next

            DateLabel.Text = "Date"
            HeaderPanel.Enabled = False
            AddCommentPanel.Enabled = False
        End If

        For I = 0 To RC - 1
            Dim myPanel As Panel = CType(UpdatePanel.FindControl("Panel" & I), Panel)
            Dim Cbx As CheckBox
            DR = DS.Tables(0).Rows(I)
            LabelKey = DR("LabelKey")
            Range = If(IsDBNull(DR("Range")), String.Empty, DR("Range"))
            Unit = If(IsDBNull(DR("Unit")), String.Empty, DR("Unit"))

            myPanel.Visible = True

            If Request.QueryString("Key") IsNot Nothing Then 'this means user is logging inputs
                'When looping through parent Panel, the TextBox control is reached before the CheckBox control. This snippet is a quick workaround
                For Each ctrl As Control In myPanel.Controls
                    If TypeOf ctrl Is CheckBox Then
                        Cbx = CType(ctrl, CheckBox)
                    End If
                Next
            End If

            For Each ctrl As Control In myPanel.Controls
                If TypeOf ctrl Is Button Then
                    myButton = CType(ctrl, Button)
                    myButton.Text = DR("Label") & If(String.IsNullOrEmpty(Range), String.Empty, " | " & Range & If(String.IsNullOrEmpty(Unit), String.Empty, " " & Unit))
                End If

                If TypeOf ctrl Is TextBox Then
                    myTextBox = CType(ctrl, TextBox)

                    If Request.QueryString("Key") IsNot Nothing Then 'this means user is logging inputs
                        Dim TbxId As String = "TextBox_" & LabelKey
                        myTextBox.ID = TbxId
                        DBConnections += "SetDBConnection('" & TbxId & "'); "
                        TbxToRange(LabelKey) = Range

                        Try 'if error occurs, T_LogData Inputs & OutOfRange values are NOT up to date
                            myTextBox.Text = Session("LabelInputMap")(LabelKey)
                        Catch ex As Exception
                            Dim My_DS2 As New Data.DataSet
                            Dim RC2 As Integer
                            Dim My_DR2 As Data.DataRow
                            Dim MapKey As Integer

                            My_DS2 = SatiCode.GetMyDataSet("SELECT [Key] FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=" & AreaFromQueryString)
                            RC2 = My_DS2.Tables(0).Rows.Count

                            For J = 0 To RC2 - 1
                                My_DR2 = My_DS2.Tables(0).Rows(J)
                                MapKey = My_DR2("Key")

                                If Not Session("LabelInputMap").ContainsKey(MapKey) Then
                                    Session("LabelInputMap").Add(MapKey, "")
                                End If

                                If Not LabelOutOfRangeMap.ContainsKey(MapKey) Then
                                    LabelOutOfRangeMap.Add(MapKey, Nothing)
                                End If
                            Next

                            ExecuteSqlQuery("UPDATE [ALTS].[dbo].[T_LogData] SET Inputs='" & JsonSerializer.Serialize(Session("LabelInputMap")) & "', OutOfRange='" & JsonSerializer.Serialize(LabelOutOfRangeMap) & "' WHERE [Key]=" & KeyFromQueryString)

                            Response.Redirect(Request.Url.ToString())
                        End Try

                        If LabelOutOfRangeMap(LabelKey) IsNot Nothing Then
                            Cbx.Checked = LabelOutOfRangeMap(LabelKey)
                        End If
                    Else 'user is editing or building a checklist
                        myTextBox.Enabled = False
                    End If
                End If

                If TypeOf ctrl Is Panel Then
                    Dim MalleableCtrl As WebControl = DirectCast(ctrl, WebControl)
                    Dim TbxOverlay As String = If(IsDBNull(DR("TbxOverlay")), Nothing, DR("TbxOverlay")) 'using ternary operator in case field value is NULL

                    If TbxOverlay IsNot Nothing AndAlso MalleableCtrl.Attributes(TbxOverlay) IsNot Nothing Then 'if TbxOverlay is null, it is a standard textbox
                        MalleableCtrl.Attributes(TbxOverlay) = True
                        MalleableCtrl.Visible = True
                        myTextBox.Visible = False

                        If Request.QueryString("Key") IsNot Nothing Then 'this means user is logging inputs
                            Dim InputCtrl As Control = ctrl.Controls(1)
                            Dim InputCtrlID As String

                            Select Case TbxOverlay
                                Case "Checkbox"
                                    Dim CheckBox As CheckBox = DirectCast(ctrl.Controls(1), CheckBox)
                                    InputCtrlID = "CheckBox_" & LabelKey
                                    Dim Checked As String = If(Session("LabelInputMap")(LabelKey) = "1", "1", "0") 'to prevent empty strings when checkbox is NOT checked
                                    CheckBox.Checked = If(Checked = "1", True, False)
                                    myTextBox.Text = Checked
                                Case "HOA"
                                    Dim DDL As DropDownList = DirectCast(ctrl.Controls(1), DropDownList)
                                    InputCtrlID = "DDL_" & LabelKey
                                    Dim HOAValue As String = myTextBox.Text

                                    If HOAValue.Contains("...") OrElse String.IsNullOrEmpty(HOAValue) Then 'if db write has NOT occured, then HOAValue will be an empty string
                                        DDL.SelectedIndex = 0
                                        myTextBox.Text = DDL.SelectedItem.Text
                                    Else
                                        DDL.SelectedValue = HOAValue
                                        DDL.Items(0).Enabled = False
                                    End If
                                Case "Text"
                                    InputCtrlID = "Text_" & LabelKey
                                    myTextBox.Text = Session("LabelInputMap")(LabelKey)
                                    DirectCast(InputCtrl, TextBox).Text = Session("LabelInputMap")(LabelKey)
                            End Select

                            InputCtrl.ID = InputCtrlID
                            DBConnections += "SetDBConnection('" & InputCtrlID & "'); "

                        Else
                            MalleableCtrl.Enabled = False
                        End If
                    End If
                End If
            Next
            VisiblePanels.Add(myPanel)

            If Request.QueryString("Key") IsNot Nothing Then 'this means user is logging inputs
                ValidateInput(myPanel.ID, myTextBox.Text)
            End If
        Next

        BuildDynamicAsp()
    End Sub


    Protected Sub Stamp_OnClick(sender As Object, e As EventArgs)
        ExecuteSqlQuery("INSERT INTO [ALTS].[dbo].[T_LogStamp] (StampKey, DataRecordKey, StampedBy, Date) VALUES (" & sender.ID.Split("_")(1) & ", " & SatiCode.GetMyDataSet(MostRecentRec).Tables(0).Rows(0)("Key") & ", '" & User.Identity.Name.ToString & "', '" & System.DateTime.Now & "')")
        sender.Text = User.Identity.Name.ToString
        sender.Enabled = False
    End Sub

    Protected Sub DynamicButton_Click(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Dim Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    Function CallSqlFunction(Query As String) As Boolean

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
            '.CommandText = "UPDATE T_LogData SET [Inputs] = @Inputs, [OutOfRange] = @OutOfRange, [Date] = @Date, [Operator] = @Operator, [CompleteLog] = @CompleteLog WHERE [Key]=@DataLogKey; SELECT TOP(1) * FROM T_LogData WHERE [Key]=" & KeyFromQueryString & " ORDER BY Date DESC;"
            .CommandText = "UPDATE T_LogData SET [Inputs] = @Inputs, [OutOfRange] = @OutOfRange, [Operator] = @Operator, [CompleteLog] = @CompleteLog WHERE [Key]=@DataLogKey; SELECT TOP(1) * FROM T_LogData WHERE [Key]=" & KeyFromQueryString & " ORDER BY Date DESC;"
            .Connection = Connection
            '.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Inputs", System.Data.SqlDbType.VarChar, 0, "Inputs"), New System.Data.SqlClient.SqlParameter("@OutOfRange", System.Data.SqlDbType.VarChar, 0, "OutOfRange"), New System.Data.SqlClient.SqlParameter("@Date", System.Data.SqlDbType.SmallDateTime, 0, "Date"), New System.Data.SqlClient.SqlParameter("@Operator", System.Data.SqlDbType.VarChar, 0, "Operator"), New System.Data.SqlClient.SqlParameter("@CompleteLog", System.Data.SqlDbType.Bit, 0, "CompleteLog"), New System.Data.SqlClient.SqlParameter("@DataLogKey", System.Data.SqlDbType.Int, 0, "Key")})
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Inputs", System.Data.SqlDbType.VarChar, 0, "Inputs"), New System.Data.SqlClient.SqlParameter("@OutOfRange", System.Data.SqlDbType.VarChar, 0, "OutOfRange"), New System.Data.SqlClient.SqlParameter("@Operator", System.Data.SqlDbType.VarChar, 0, "Operator"), New System.Data.SqlClient.SqlParameter("@CompleteLog", System.Data.SqlDbType.Bit, 0, "CompleteLog"), New System.Data.SqlClient.SqlParameter("@DataLogKey", System.Data.SqlDbType.Int, 0, "Key")})
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
            My_DR("Operator") = User.Identity.Name.ToString
            My_DR("CompleteLog") = False
            My_DR("Inputs") = JsonSerializer.Serialize(Session("LabelInputMap"))
            My_DR("OutOfRange") = JsonSerializer.Serialize(LabelOutOfRangeMap)
            'My_DR("Date") = System.DateTime.Now.ToShortTimeString
            My_DR.EndEdit()
            My_DA.Update(My_DS, "T_LogData")
        Catch ex As Exception
            MessageUserLabel.Text = ex.Message
        End Try
        Connection.Close()
    End Sub

    Public Function RecursiveFind(ByVal parentControl As Control, ByVal controlID As String) As Control
        ' Check if the parentControl is null
        If parentControl Is Nothing Then
            Return Nothing
        End If

        ' Check if the current control matches the target control ID
        If parentControl.ID = controlID Then
            Return parentControl
        End If

        ' Recursively search through child controls
        For Each child As Control In parentControl.Controls
            Dim foundControl As Control = RecursiveFind(child, controlID)
            If foundControl IsNot Nothing Then
                Return foundControl
            End If
        Next

        ' Return Nothing if no matching control is found
        Return Nothing
    End Function

    Public Function FindPnl(ChildControlID As String) As Panel
        For Each Pnl As Panel In VisiblePanels
            If RecursiveFind(Pnl, ChildControlID) IsNot Nothing Then
                Return Pnl
            End If
        Next

        Return Nothing
    End Function


    Function ValidateInput(ControlID As String, Value As String) As Boolean
        Dim Button As Button
        Dim TextBox As TextBox
        Dim UserInput As String = If(Value Is Nothing, Nothing, SqlProofSingleQuotes(Value))
        Dim UserInputDec As Decimal
        Dim InRange As Boolean
        Dim Range As String
        Dim LowerBound As Decimal
        Dim UpperBound As Decimal
        Dim DelimitArr() As String
        Dim Valid As Boolean = True
        Dim TbxOverlay As String
        Dim Pnl As Panel = FindPnl(ControlID)
        Dim LabelKey As Integer

        'TO DO: incorporate this block of code into the next for loop, to avoid looping through panel controls twice.
        'Note: When looping through parent Panel, the TextBox control is reached before the CheckBox control. This snippet was a quick workaround
        Dim Cbx As CheckBox

        For Each ctrl As Control In Pnl.Controls
            If TypeOf ctrl Is CheckBox Then
                Cbx = CType(ctrl, CheckBox)
            End If
        Next
        'TO DO: incorporate this block of code into the next for loop, to avoid looping through panel controls twice.
        'Note: When looping through parent Panel, the TextBox control is reached before the CheckBox control. This snippet was a quick workaround

        For Each ctrl As Control In Pnl.Controls
            If TypeOf ctrl Is Button Then
                Button = CType(ctrl, Button)
            End If

            If TypeOf ctrl Is TextBox Then
                TextBox = CType(ctrl, TextBox)
                If UserInput Is Nothing Then UserInput = SqlProofSingleQuotes(TextBox.Text)
                LabelKey = TextBox.ID.Split("_")(1)
                TbxOverlay = GetSingleDbField("SELECT TbxOverlay FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=" & LabelKey, "TbxOverlay")

                If TbxOverlay IsNot Nothing Then
                    Select Case TbxOverlay 'search for cases where the input would be valid
                        Case "Checkbox"
                            If UserInput = "1" Then Exit For
                        Case "HOA"
                            If Not UserInput.Contains("...") Then Exit For
                        Case "Text"
                            If Not String.IsNullOrEmpty(UserInput) Then Exit For
                    End Select

                    'if here, input is NOT valid
                    SetPanelBackColor(System.Drawing.Color.Red, "", Pnl)
                    Valid = False
                    Exit For
                ElseIf Not Decimal.TryParse(UserInput, UserInputDec) Then 'check if value is valid
                    SetPanelBackColor(System.Drawing.Color.Red, "*ERROR: NOT A NUMBER*", Pnl)
                    Valid = False
                    Exit For
                ElseIf TbxToRange.ContainsKey(LabelKey) Then 'check if a range exists
                    InRange = True
                    Range = TbxToRange(LabelKey)

                    'decipher if it's a numerical OR Greater Than(>)/Less Than(<) range
                    If Range.Contains("-") Then
                        DelimitArr = Range.Split("-")
                        UserInputDec = Decimal.Parse(UserInput)

                        Decimal.TryParse(Trim(DelimitArr(0)), LowerBound)
                        Decimal.TryParse(Trim(DelimitArr(1)), UpperBound)

                        If UserInputDec < LowerBound Or UserInputDec > UpperBound Then
                            InRange = False
                        End If
                    ElseIf Range.Contains("<") Then
                        If UserInputDec >= Decimal.Parse(Trim(Range.Replace("<", ""))) Then
                            InRange = False
                        End If
                    ElseIf Range.Contains(">") Then
                        If UserInputDec <= Decimal.Parse(Trim(Range.Replace(">", ""))) Then
                            InRange = False
                        End If
                    End If

                    If Not InRange Then
                        If TbxOverlay Is Nothing Then 'make sure it is NOT a checkbox field
                            SetPanelBackColor(System.Drawing.ColorTranslator.FromHtml("#E6E600"), "*CAUTION: OUT OF RANGE*", Pnl)
                            Exit For
                        End If
                    End If
                End If
                'if here, value is valid and in range
                SetPanelBackColor(System.Drawing.ColorTranslator.FromHtml("#F5F5F5"), "", Pnl)
            End If
        Next
        Session("LabelInputMap")(LabelKey) = UserInput

        If Cbx.Visible Then 'cbx to verify if value is out of range
            LabelOutOfRangeMap(LabelKey) = Cbx.Checked
        Else
            LabelOutOfRangeMap(LabelKey) = Nothing
        End If

        MessageUserLabel.Text = "" 'this Label element could potentially have text set from previous failed attempts at completing the log

        Return Valid
    End Function

    'Sub ValidateInputsAndUploadToDataTable(Callback As Action(Of Panel))
    '    For Each Pnl As Panel In VisiblePanels
    '        Callback(Pnl)
    '        ValidateInput(Pnl.ID, Nothing)
    '    Next

    '    UploadToDataTable()
    'End Sub

    Function ValidateInputsAndUploadToDataTable() As Boolean
        Dim All_InputsAreValid As Boolean = True

        For Each Pnl As Panel In VisiblePanels
            If Not ValidateInput(Pnl.ID, Nothing) Then 'if a singular input is NOT valid
                All_InputsAreValid = False
            End If
        Next

        UploadToDataTable()
        Return All_InputsAreValid
    End Function

    Protected Sub DbUploadTimer_Tick(sender As Object, e As EventArgs)
        If TimeForNewLog Then
            SetControlsEnabledProp(UpdatePanel, False)
            MessageUserLabel.Text = "Visit Status Board To Access Current Log"
        Else
            If Not IsDBNull(GetSingleDbField("SELECT Top(1) * FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=" & Request.QueryString("Key") & " ORDER BY Date DESC", "Operator")) Then 'only IF Operator is NOT null
                Update_All_InputsValid_Field()
            End If
        End If

    End Sub

    'Protected Sub LogAreasDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    Response.Redirect(WebpageUrl & "?Area=" & LogAreasDropDownList.SelectedValue)
    'End Sub

    Protected Sub UpdateButton_onClick(sender As Object, e As EventArgs)
        ShiftSqlDataSource.UpdateCommand = "UPDATE [ALTS].[dbo].[T_LogData] SET [Shift] ='" & sender.Parent.FindControl("ShiftDropDownList").SelectedValue & "' WHERE [Key]=(SELECT Top(1) [Key] FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=" & KeyFromQueryString & " ORDER BY Date Desc)"
        ShiftSqlDataSource.Update()
    End Sub

    Protected Sub VerifyValue_Check(sender As Object, e As EventArgs)
        Dim Button As Button
        Dim Cbx As CheckBox
        Dim Tbx As TextBox
        Dim CheckedStatus As Boolean

        For Each Pnl As Panel In VisiblePanels
            If Pnl Is sender.Parent Then ' if Pnl holds the textbox that triggered this event
                For Each ctrl As Control In Pnl.Controls
                    If TypeOf ctrl Is Button Then
                        Button = CType(ctrl, Button)
                    ElseIf TypeOf ctrl Is CheckBox Then
                        Cbx = CType(ctrl, CheckBox)
                    ElseIf TypeOf ctrl Is TextBox Then
                        Tbx = CType(ctrl, TextBox)
                    End If
                Next
            End If
        Next

        CheckedStatus = Not sender.Checked 'because view state is reset after postback, the true Checked value is the opposite of the current one
        LabelOutOfRangeMap(Tbx.ID.Split("_")(1)) = CheckedStatus
        Cbx.Checked = CheckedStatus
        UploadToDataTable()
        SetScrollPos()
    End Sub

    Protected Sub SetPanelBackColor(Color As System.Drawing.Color, Message As String, Pnl As Panel)
        Dim BackColor As System.Drawing.Color = Color
        Dim MalleableCtrl As WebControl

        Pnl.BackColor = BackColor
        For Each Ctrl As Control In Pnl.Controls
            If TypeOf Ctrl Is WebControl Then
                MalleableCtrl = DirectCast(Ctrl, WebControl)
                MalleableCtrl.BackColor = BackColor

                If MalleableCtrl.Attributes("ColorBlindMessage") IsNot Nothing Then
                    DirectCast(MalleableCtrl, ITextControl).Text = Message
                End If

                If TypeOf Ctrl Is CheckBox Then
                    If Message.Contains("CAUTION") Then
                        Ctrl.Visible = True
                        VisibleCheckBoxes.Add(Ctrl)
                    Else
                        Ctrl.Visible = False
                    End If
                End If
            End If

        Next
    End Sub

    Sub SetScrollPos()
        Response.Redirect(Request.Path & "?Key=" & KeyFromQueryString & If(Request.QueryString("WHERE") IsNot Nothing, "&WHERE=" & Request.QueryString("WHERE"), String.Empty) & If(Request.QueryString("Department") IsNot Nothing, "&Department=" & Request.QueryString("Department"), String.Empty) & If(Request.QueryString("View") IsNot Nothing, "&View=" & Request.QueryString("View"), String.Empty) & "&IP_ScrollPos=" & ItemsPanel_HiddenField.Value, False) 'trigger postback AFTER this code has ran
    End Sub

    Public Delegate Function ModifyInputDelegate(ID As String, Value As String) As Boolean
    Function ModifyInput(ID As String, Value As String) As Boolean
        Dim LabelKey As String = ID.Split("_")(1)

        If Session("LabelInputMap")(LabelKey) = Value Then Return False 'value has NOT changed, so do NOT modify input

        ValidateInput(ID, Value)
        UploadToDataTable()

        Try 'in case user in on last input, in which case sql will return 'There is no row at position 0.'
            ExecuteSqlQuery("UPDATE [ALTS].[dbo].[T_LogData] SET KeyOfLastLabel=" & SatiCode.GetMyDataSet("SELECT TOP(1) [Key], AreaKey, Label, LabelOrder FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=(SELECT AreaKey FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]= " & LabelKey & ") AND LabelOrder > (SELECT LabelOrder FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]= " & LabelKey & ") ORDER BY LabelOrder").Tables(0).Rows(0)("Key") & " WHERE [Key]=" & SatiCode.GetMyDataSet(MostRecentRec).Tables(0).Rows(0)("Key")) 'update KeyOfLastLabel field in DB
        Catch ex As Exception

        End Try

        Return True
    End Function

    Sub Update_All_InputsValid_Field()
        Dim All_InputsValid As Boolean = True

        For Each Pnl As Panel In VisiblePanels
            If Not ValidateInput(Pnl.ID, Nothing) Then
                All_InputsValid = False
            End If
        Next

        Try 'in case There is no row at position 0
            If Not All_InputsValid Then
                ExecuteSqlQuery("UPDATE [ALTS].[dbo].[T_LogData] Set All_InputsValid=0 WHERE [Key]=" & SatiCode.GetMyDataSet(MostRecentRec).Tables(0).Rows(0)("Key"))
            Else
                ExecuteSqlQuery("UPDATE [ALTS].[dbo].[T_LogData] Set All_InputsValid=1 WHERE [Key]=" & SatiCode.GetMyDataSet(MostRecentRec).Tables(0).Rows(0)("Key"))
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub DoneButton_Click(sender As Object, e As EventArgs)
        Dim All_InputsAreValid As Boolean = ValidateInputsAndUploadToDataTable()
        Dim NumOfNotes As Integer = SatiCode.GetMyDataSet("Select Count([Key]) As NumOfNotes FROM [ALTS].[dbo].[T_LogOperatorComments] WHERE CommentKey=" & SatiCode.GetMyDataSet(MostRecentRec).Tables(0).Rows(0)("Key")).Tables(0).Rows(0)("NumOfNotes")

        'ensure all inputs are filled with valid data
        For Each kvp As KeyValuePair(Of Integer, String) In Session("LabelInputMap")
            Dim Value As String = kvp.Value

            If NumOfNotes = 0 Then
                If Value = "" OrElse Not All_InputsAreValid Then
                    MessageUserLabel.Text = "Error: Incomplete or invalid logs. Add a comment to proceed."
                    Exit Sub
                End If
            ElseIf Not All_InputsAreValid Then
                'display verify interface
                DoneButton.Enabled = False
                MarkAsDoneCheckBox.Visible = True
                Return
            End If
        Next

        'if here, all fields are valid, because 'Exit Sub' statement has NOT been run
        MarkAsDone()
    End Sub

    Sub MarkAsDone()
        Dim LabelRangeMap As New Dictionary(Of Integer, String)

        'update Ranges field (constuct & stringify Dictionary, then run update query)
        DS = SatiCode.GetMyDataSet("SELECT [Key], Range FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=" & GetSingleDbField("SELECT AreaKey FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=" & KeyFromQueryString, "AreaKey"))
        DRC = DS.Tables(0).Rows

        For I = 0 To DRC.Count - 1
            DR = DRC(I)
            LabelRangeMap.Add(DR("Key"), If(IsDBNull(DR("Range")), Nothing, DR("Range")))
        Next

        ExecuteSqlQuery("UPDATE [ALTS].[dbo].[T_LogData] SET Ranges='" & JsonSerializer.Serialize(LabelRangeMap) & "', CompleteLog=1, Date='" & System.DateTime.Now.ToString() & "' WHERE [Key]=" & KeyFromQueryString) 'record to 'Ranges' field in T_LogData
        Update_All_InputsValid_Field()
        Response.Redirect(Request.Url.ToString(), False) 'trigger postback AFTER this code has ran, to make the form readonly and dynamically create stamps
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

    Function SqlProofSingleQuotes(Text As String) As String
        Return Text.Replace("'", "''") 'escape single quotes (') by doubling them ('')
    End Function

    Protected Sub AddCommentButton_Click(sender As Object, e As EventArgs)
        Dim TextBoxText As String = SqlProofSingleQuotes(CommentTextBox.Text)
        If (TextBoxText = "") Then
            'send message to user
            Exit Sub
        End If

        ExecuteSqlQuery("INSERT INTO [ALTS].[dbo].[T_LogOperatorComments] VALUES (" & SatiCode.GetMyDataSet(MostRecentRec).Tables(0).Rows(0)("Key") & ", '" & TextBoxText & "')")
        'CommentTextBox.Text = ""
        'CommentGridView.DataBind()
        SetScrollPos()
    End Sub

    Protected Sub MarkAsDoneCheckBox_OnCheckedChanged(sender As Object, e As EventArgs)
        If sender.Checked Then
            MarkAsDone()
            MarkAsDoneCheckBox.Visible = False
        End If
    End Sub

    Sub hold()
        '<asp:Button ID = "Button1" runat="server" Text="Auto Batch Stock (Polish)" Height="125px" Width="258px" LabelTip="Master drive fault: n4 outer pin ring, n2 lower plate, n3 inner pin ring. Audible grinding noise heard emanating from outer pin ring gearbox/motor assembly area. Grinding most audible in second half of brush cycle when spin direction changes. " BackColor="#33CC33" />
        '<asp:Button ID = "Button2" runat="server" Text="Auto Batch Stock / 3500 (DSP)" Height="50px" Width="300px" BackColor="#FFFF66" />
        '<asp:Button ID = "Button3" runat="server" Text="Button" Height="112px" Width="644px" BackColor="Red" />
        '<asp:Button ID="Button4" runat="server" Text="Button" OnClick="myclick" CommandArgument="themrnumber" />
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

    Sub BuildDynamicAsp()
        If Request.QueryString("Key") IsNot Nothing Then 'if true, user is filling out a log sheet
            DS = SatiCode.GetMyDataSet("Select T.Title, L.[Key] As ID, S.[Key] As StampedRecordKey, S2.StampedBy As StampedBy, T.RoleID FROM [ALTS].[dbo].[T_LogStamp] S RIGHT JOIN [ALTS].[dbo].[T_LogStampList] L On L.[Key]=S.StampKey And DataRecordKey=" & SatiCode.GetMyDataSet(MostRecentRec).Tables(0).Rows(0)("Key") & " INNER JOIN [ALTS].[dbo].[T_LogStampTitle] T On L.TitleKey=T.[Key] LEFT JOIN [ALTS].[dbo].[T_LogStamp] S2 On S.[Key]=S2.[Key] WHERE AreaKey=" & AreaFromQueryString & " AND Active=1")
        Else 'user is in ChecklistBuilder.aspx editing or creating a checklist
            DS = SatiCode.GetMyDataSet("SELECT Stamped.Title, Stamp.[Key] As ID FROM [ALTS].[dbo].[T_LogStampList] Stamp INNER JOIN [ALTS].[dbo].[T_LogStampTitle] Stamped ON Stamp.TitleKey=Stamped.[Key] WHERE Active=1 AND AreaKey=" & AreaFromQueryString)
        End If

        DRC = DS.Tables(0).Rows

        'dynamically create Stamp related controls
        For I = 0 To DRC.Count - 1
            DR = DRC(I)

            Dim Panel As New Panel()
            Dim Button As New Button()
            Dim Label As New Label()

            Panel.Attributes.Add("style", "display: flex; flex-direction: column;")

            Label.Text = DR("Title") & ":"

            Button.ID = "StampList_" & DR("ID")

            If Request.QueryString("Key") IsNot Nothing Then 'if true, user is filling out a log sheet
                Button.Text = If(IsDBNull(DR("StampedBy")), "Stamp", DR("StampedBy"))
            Else
                Button.Text = "Stamp"
            End If

            Button.Enabled = False

            Panel.Controls.Add(Label)
            Panel.Controls.Add(Button)
            StampPanel.Controls.Add(Panel)

            If Request.QueryString("Key") IsNot Nothing Then 'if true, user is filling out a log sheet
                AddHandler Button.Click, AddressOf Stamp_OnClick

                'if log is complete, stamp does NOT exist, AND user has the associated role to stamp, enable button
                If Button.Text = "Stamp" AndAlso GetSingleDbField("SELECT COUNT(RoleName) As ContainsRole FROM [SatiUsers].[dbo].aspnet_UsersInRoles INNER JOIN [SatiUsers].[dbo].aspnet_Users On [SatiUsers].[dbo].aspnet_UsersInRoles.UserId = [SatiUsers].[dbo].aspnet_Users.UserId INNER JOIN [SatiUsers].[dbo].aspnet_Roles On [SatiUsers].[dbo].aspnet_UsersInRoles.RoleId = [SatiUsers].[dbo].aspnet_Roles.RoleId INNER JOIN [ALTS].[dbo].[T_LogStampList] On [SatiUsers].[dbo].aspnet_Roles.RoleId='" & DR("RoleID") & "' WHERE [ALTS].[dbo].[T_LogStampList].[Key]=" & Button.ID.Split("_")(1) & " And [SatiUsers].[dbo].aspnet_Users.UserName = '" & User.Identity.Name.ToString & "'", "ContainsRole") Then
                    If LogDR("CompleteLog") Then
                        Button.Enabled = True
                    Else
                        'MessageUserLabel.Text = "Log must be complete before stamping"
                    End If
                End If
            End If

        Next

        'dynamically create Comment related controls
        DS = SatiCode.GetMyDataSet("SELECT Comment FROM [ALTS].[dbo].[T_LogCommentList] WHERE AreaKey=" & AreaFromQueryString & "ORDER BY CommentOrder")
        DRC = DS.Tables(0).Rows

        For I = 0 To DRC.Count - 1
            DR = DRC(I)
            Dim Label As New Label()

            Label.Text = DR("Comment")
            CommentPanel.Controls.Add(Label)
        Next

    End Sub
    Protected Sub BackToStatusBoard_OnClick(sender As Object, e As EventArgs)
        Dim Where As String = Request.QueryString("WHERE")
        Dim Department As String = Request.QueryString("Department")
        Dim View As String = Request.QueryString("View")
        Dim StatusBoardUrl As String = "/ChecklistLogging/StatusBoard.aspx"

        If Department IsNot Nothing AndAlso View IsNot Nothing Then 'user accessed Log.aspx from StatusBoard.aspx
            StatusBoardUrl += "?" & "Department=" & Request.QueryString("Department") & "&View=" & Request.QueryString("View")

            If Where IsNot Nothing Then 'user is viewing log not associated with today's date
                StatusBoardUrl += "&WHERE=" & Where
            End If
        End If

        Response.Redirect(StatusBoardUrl)
    End Sub

    Protected Sub AddPhotoButton_OnClick(sender As Object, e As EventArgs)
        PreviewPanel_iframe.Visible = True
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "iframeEnabled", "iframeEnabled(true);", True)
        PreviewPanel_iframe.Attributes.Add("src", "/ChecklistLogging/AddPhoto.aspx" & "?" & Request.RawUrl.Split("?")(1) & "&DataKey=" & KeyFromQueryString)
    End Sub

End Class

