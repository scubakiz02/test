Imports System.Text.Json
Imports System.Web.Services
Imports SatiDotNet2.Library
Imports System.Data
Imports System.IO
Imports System.Text.RegularExpressions

Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim Security As New Security
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
    Dim MostRecentRecKey As Integer
    Dim ItemsPanel_ScrollPos As String
    Dim DS As Data.DataSet
    Dim DR As Data.DataRow
    Dim DRC As Data.DataRowCollection
    Dim JsFunctionCalls As String
    Dim DBConnections As String
    Dim DateFieldType As String
    Dim STC_TbxOverlays As String
    Dim DP_TbxOverlay As String
    Dim LogAspx As New LogAspxLibrary
    Dim AcceptedFormats As String() = {"tif", "tiff", "jpg", "jpeg", "png", "gif", "bmp"}
    Dim FormatToContentType As New Dictionary(Of String, String) From
     {
        {"jpg", "jpeg"},
        {"svg", "svg%2Bxml"}
     } '%2B is URL encoding for '+'
    Dim ContentTypeToFormat As New Dictionary(Of String, String) From
     {
        {"svg%2Bxml", "svg"}
     } '%2B is URL encoding for '+'
    Dim uploadDirectory As String
    Dim VirtualDirectory As String
    Dim Directory As String
    Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
    Private Format As New Format()
    Private PhaseController As PhaseController

    Public Delegate Sub DeleteNoteDelegate(ID As String)
    <WebMethod()>
    Public Shared Function DeleteNoteValue(T_LogOperatorCommentsKey As String) As String
        Try 'in case code-behind throws an error
            Dim Delete_Note As DeleteNoteDelegate = HttpContext.Current.Session("DeleteNote")
            Delete_Note(T_LogOperatorCommentsKey)
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    Public Delegate Sub UpdateNoteDelegate(ID As String, Value As String)
    <WebMethod()>
    Public Shared Function UpdateNoteValue(T_LogOperatorCommentsKey As String, Comment As String) As String
        Try 'in case code-behind throws an error
            Dim Update_Note As UpdateNoteDelegate = HttpContext.Current.Session("UpdateNote")
            Update_Note(T_LogOperatorCommentsKey, Comment)
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    Public Delegate Function ModifyInputDelegate(ID As String, Value As String) As Boolean
    <WebMethod()>
    Public Shared Function DbWrite(SenderID As String, SenderValue As String) As String
        Try 'in case code-behind throws an error
            Dim ModifyInput As ModifyInputDelegate = HttpContext.Current.Session("ModifyInput")
            Return ModifyInput(SenderID, SenderValue)  'Return a response back to the JavaScript function
        Catch ex As Exception
        End Try

        Return False
    End Function

    Public Delegate Function ValidDateDelegate(UserInput As String) As String
    <WebMethod()>
    Public Shared Function ValidDate(UserInput As String) As String
        Try 'in case code-behind throws an error
            Dim Valid_Date As ValidDateDelegate = HttpContext.Current.Session("ValidDate")
            Return Valid_Date(UserInput)  'Return a response back to the JavaScript function
        Catch ex As Exception
        End Try

        Return "*Format Error: MM/YY*"
    End Function

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        ClientScript.RegisterStartupScript(Me.GetType(), "SetHoverEffect", "syncScrollPos('ItemsPanel', " & ItemsPanel_ScrollPos & "); setFooterAtBottom(); " & JsFunctionCalls, True)
        ClientScript.RegisterStartupScript(Me.GetType(), "SetDBConnections", DBConnections + STC_TbxOverlays + DP_TbxOverlay + DateFieldType, True)

        If Session("DisplayError") Then
            MessageUserLabel.Text = "Error: red or yellow logs present. Add a comment to proceed."
        End If
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
        Dim PhaseConfig As Dictionary(Of Integer, Dictionary(Of String, String))
        Dim CurrPhaseOrder As Integer

        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        'MenuAuthenication.CheckGroupAuthenication("Office", Server)
        Me.MaintainScrollPositionOnPostBack = True
        AreaFromQueryString = Request.QueryString("Area")
        KeyFromQueryString = Request.QueryString("Key")
        ItemsPanel_ScrollPos = Request.QueryString("IP_ScrollPos")
        Page.MaintainScrollPositionOnPostBack = True

        If KeyFromQueryString IsNot Nothing Then 'if this is true, displaying Log.aspx for operator to fill out
            Dim ModifyInputDelegate As ModifyInputDelegate = AddressOf ModifyInput
            Dim UpdateNoteDelegate As UpdateNoteDelegate = AddressOf UpdateNote
            Dim DeleteNoteDelegate As DeleteNoteDelegate = AddressOf DeleteNote
            Dim ValidDateDelegate As ValidDateDelegate = AddressOf LogAspx.ValidDate

            Session("ModifyInput") = ModifyInputDelegate
            Session("UpdateNote") = UpdateNoteDelegate
            Session("DeleteNote") = DeleteNoteDelegate
            Session("ValidDate") = ValidDateDelegate

            ScriptManager.GetCurrent(Me.Page).EnablePageMethods = True

            QueryConfig("@T_LogDataKey") = New Dictionary(Of String, String) From {
                {"value", KeyFromQueryString},
                {"typeOf", "int"}
            }

            MostRecentRecKey = Security.GetSingleDbField("SELECT Top(1) * FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=@T_LogDataKey ORDER BY Date DESC", QueryConfig, "Key")

            'get info needed to build checklist (labels, ranges, units, checklist name, etc.)
            'DS = Security.GetMyDataSetParamQuery("SELECT TOP (100) A.[Key] As AreaKey, Area, I.SqlFunc, I.SqlFunc2ndArg, L.Label As Label, L.[Key] As LabelKey, L.Range As Range, L.FieldType, U.Unit, D.Date From [ALTS].[dbo].[T_LogArea] A INNER JOIN [ALTS].[dbo].[T_LogData] D ON A.[Key]=D.AreaKey INNER JOIN [ALTS].[dbo].[T_LogLabel] L ON A.[Key]=L.AreaKey LEFT JOIN [ALTS].[dbo].[T_LogUnit] U ON L.UnitKey=U.[Key] INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE D.[Key]=@T_LogDataKey ORDER BY L.LabelOrder", QueryConfig)
            DS = Security.GetMyDataSetParamQuery("SELECT TOP (100) A.[Key] As AreaKey, Area, I.SqlFunc, I.SqlFunc2ndArg, L.Label As Label, L.[Key] As LabelKey, L.Range As Range, L.FieldType, U.Unit, D.Date From [ALTS].[dbo].[T_LogArea] A INNER JOIN [ALTS].[dbo].[T_LogData] D ON A.[Key]=D.AreaKey INNER JOIN [ALTS].[dbo].[T_LogLabel] L ON A.[Key]=L.AreaKey LEFT JOIN [ALTS].[dbo].[T_LogPhase] P ON L.PhaseKey=P.[Key] LEFT JOIN [ALTS].[dbo].[T_LogUnit] U ON L.UnitKey=U.[Key] INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE D.[Key]=@T_LogDataKey ORDER BY P.PhaseOrder, L.LabelOrder", QueryConfig)
            RC = DS.Tables(0).Rows.Count

            'CommentSqlDataSource.SelectCommand = "SELECT OpComments.[Key], OpComments.Comment FROM [ALTS].[dbo].[T_LogOperatorComments] OpComments WHERE OpComments.CommentKey=(Select Top(1) [Key] FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=" & KeyFromQueryString & " ORDER BY Date DESC)"
            CommentSqlDataSource.SelectCommand = "SELECT OpComments.[Key], OpComments.Comment FROM [ALTS].[dbo].[T_LogOperatorComments] OpComments WHERE OpComments.CommentKey=(Select Top(1) [Key] FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=@T_LogDataKey ORDER BY Date DESC)"
            CommentSqlDataSource.SelectParameters.Clear()
            CommentSqlDataSource.SelectParameters.Add("T_LogDataKey", KeyFromQueryString)
            CommentGridView.DataBind()
            If CommentGridView.Rows.Count > 0 Then
                CommentGridView.Visible = True
            End If

            ClientScript.RegisterStartupScript(Me.GetType(), "callFunction", "textboxFocus(" & Security.GetSingleDbField("SELECT Top(1) * FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=@T_LogDataKey ORDER BY Date DESC", QueryConfig, "KeyOfLastLabel") & ");", True) 'set focus on current textbox noted in DB

            DR1 = DS.Tables(0).Rows(0)

            AreaFromQueryString = DR1("AreaKey")
            TitleLabel.Text = DR1("Area")

            'get data related to log instance (derived from querystring 'Key' and its value)
            QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
                {"value", AreaFromQueryString},
                {"typeOf", "int"}
            }
            QueryConfig("@SqlFunc2ndArg") = New Dictionary(Of String, String) From {
                {"value", DR1("SqlFunc2ndArg")},
                {"typeOf", "string"}
            }
            QueryConfig("@Date") = New Dictionary(Of String, String) From {
                {"value", DR1("Date")},
                {"typeOf", "string"}
            }
            LogDS = Security.GetMyDataSetParamQuery("SELECT Top(1) * From [ALTS].[dbo].[T_LogData] D CROSS APPLY (SELECT * FROM " & DR1("SqlFunc") & "(@AreaKey, @SqlFunc2ndArg, @Date)) DA WHERE [Key]=@T_LogDataKey ORDER BY Date DESC", QueryConfig)
            LogDR = LogDS.Tables(0).Rows(0)
            TimeForNewLog = LogDR("TimeForNewLog")

            DateLabel.Text = LogDR("DatePeriod")

            Session("LabelInputMap") = LogAspx.GetInputs(LogDR) 'LabelInputMap is a session state variable so WebMethod (static) function has access to it
            LabelOutOfRangeMap = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Boolean?))(LogDR("OutOfRange"))

            'disable controls if log is complete
            If LogDR("CompleteLog") Then
                SetControlsEnabledProp(ItemsPanel, False)
                HeaderPanel.Enabled = True
                WrongFormButton.Enabled = False
                DoneButton.Enabled = False
                UndoDoneButton.Enabled = True
            End If

            'update T_LogData Inputs field to the new format (Dictionary(Of Integer, Dictionary(Of String, String)) in case T_LogData Inputs field format is the old format (Dictionary(Of String, String))
            QueryConfig.Clear()
            QueryConfig("@T_LogDataKey") = New Dictionary(Of String, String) From {
                {"value", KeyFromQueryString},
                {"typeOf", "int"}
            }
            QueryConfig("@Inputs") = New Dictionary(Of String, String) From {
                {"value", JsonSerializer.Serialize(Session("LabelInputMap"))},
                {"typeOf", "string"}
            }
            Security.ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogData] SET Inputs=@Inputs WHERE [Key]=@T_LogDataKey", QueryConfig)
            QueryConfig.Remove("@Inputs") 'b/c QueryConfig uses @T_LogDataKey but NOT @Inputs for PhotoDS DataSet variable below

            PhotoDS = Security.GetMyDataSetParamQuery("SELECT * FROM [ALTS].[dbo].[T_LogDataPhotos] WHERE DataKey=@T_LogDataKey", QueryConfig)
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

            DR = Security.GetMyDataSetParamQuery("SELECT A.[Key], I.SqlFunc2ndArg, D.Date, A.Area, I.SqlFunc FROM [ALTS].[dbo].[T_LogData] D INNER JOIN [ALTS].[dbo].[T_LogArea] A ON D.AreaKey=A.[Key] INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE D.[Key]=@T_LogDataKey", QueryConfig).Tables(0).Rows(0)

            QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
                {"value", DR("Key")},
                {"typeOf", "int"}
            }
            QueryConfig("@SqlFunc2ndArg") = New Dictionary(Of String, String) From {
                {"value", DR("SqlFunc2ndArg")},
                {"typeOf", "float"}
            }
            QueryConfig("@Date") = New Dictionary(Of String, String) From {
                {"value", DR("Date")},
                {"typeOf", "string"}
            }
            Directory = Security.StripIllegalFileSysChars(DR("Area"), Security.GetSingleDbField("Select DatePeriod FROM " & DR("SqlFunc") & "(@AreaKey, @SqlFunc2ndArg, @Date)", QueryConfig, "DatePeriod"))
            uploadDirectory = Path.Combine(Session("SUP_IO"), Directory).Replace("\", "/")
            VirtualDirectory = Path.Combine(Session("SUP_VD"), Directory).Replace("\", "/")
        ElseIf AreaFromQueryString IsNot Nothing Then  'if this is true, displaying webpage in iframe within ChecklistBuilder.aspx
            Dim TempDataKey As Integer

            QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
                {"value", AreaFromQueryString},
                {"typeOf", "int"}
            }

            TempDataKey = Security.GetSingleDbField("SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=@AreaKey", QueryConfig, "Key")

            'get info needed to build the checklist (name, labels, units, ranges, etc.)
            'DS = Security.GetMyDataSetParamQuery("SELECT TOP (100) Area, L.Label As Label, L.[Key] As LabelKey, L.Range As Range, L.FieldType, U.Unit From [ALTS].[dbo].[T_LogArea] A INNER JOIN [ALTS].[dbo].[T_LogLabel] L ON A.[Key]=L.AreaKey LEFT JOIN [ALTS].[dbo].[T_LogUnit] U ON L.UnitKey=U.[Key] WHERE A.[Key]=@AreaKey ORDER BY L.LabelOrder", QueryConfig)
            DS = Security.GetMyDataSetParamQuery("SELECT TOP (100) Area, L.Label As Label, L.[Key] As LabelKey, L.Range As Range, L.FieldType, U.Unit From [ALTS].[dbo].[T_LogArea] A INNER JOIN [ALTS].[dbo].[T_LogLabel] L ON A.[Key]=L.AreaKey LEFT JOIN [ALTS].[dbo].[T_LogPhase] P ON L.PhaseKey=P.[Key] LEFT JOIN [ALTS].[dbo].[T_LogUnit] U ON L.UnitKey=U.[Key] WHERE A.[Key]=@AreaKey ORDER BY P.PhaseOrder, L.LabelOrder", QueryConfig)
            RC = DS.Tables(0).Rows.Count

            'reset session state LabelInputMap variable to ensure logic surrounding FieldType case statement below still works
            Session("LabelInputMap") = New Dictionary(Of Integer, Dictionary(Of String, String))
            For J = 0 To RC - 1
                DR = DS.Tables(0).Rows(J)
                Session("LabelInputMap")(DR("LabelKey")) = New Dictionary(Of String, String) From {
                    {"Date", String.Empty},
                    {"Operator", String.Empty},
                    {"Value", String.Empty}
                }
            Next

            TitleLabel.Text = Security.GetSingleDbField("SELECT Area FROM [ALTS].[dbo].[T_LogArea] WHERE [Key]=@AreaKey", QueryConfig, "Area")

            Try 'using a try catch block in case DS has no records, which mean the user is building a new checklist
                DR1 = DS.Tables(0).Rows(0)
            Catch ex As Exception
                DateLabel.Text = "Date"
                HeaderPanel.Enabled = False
                FooterPanel.Enabled = False
                FooterPanel.Attributes.Add("class", "disabled")
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
            FooterPanel.Enabled = False
            FooterPanel.Attributes.Add("class", "disabled")
        End If

        QueryConfig.Clear()

        PhaseController = New PhaseController(AreaFromQueryString, Session("LabelInputMap"))
        PhaseConfig = PhaseController.GetPhases()
        CurrPhaseOrder = PhaseController.GetPhase()

        For I = 0 To RC - 1
            Dim myPanel As Panel = CType(UpdatePanel.FindControl("Panel" & I), Panel)
            Dim Cbx As CheckBox
            DR = DS.Tables(0).Rows(I)
            LabelKey = DR("LabelKey")
            'Range = If(IsDBNull(DR("Range")), String.Empty, DR("Range"))
            Range = LogAspx.GetRange(Request.QueryString("Key"), LogDR, DR)
            Unit = If(IsDBNull(DR("Unit")), String.Empty, DR("Unit"))

            If PhaseConfig IsNot Nothing AndAlso PhaseConfig.ContainsKey(LabelKey) Then
                Dim Phase As String = PhaseConfig(LabelKey)("Phase")
                Dim PhasePanelID As String = Phase.Replace(" ", "-") & "_Panel"
                Dim PhasePanel As Panel = ItemsPanel.FindControl(PhasePanelID)

                If PhasePanel Is Nothing Then
                    Dim PhaseLabel As New Label
                    PhasePanel = New Panel()

                    PhaseLabel.Text = Phase
                    'NOTE: for some odd reason, calc(var(--UFontSize) * 1) is a bigger font-size than var(--UFontSize) * 1
                    PhaseLabel.Attributes.Add("style", "font-size: calc(var(--UFontSize) * 1); font-weight: bolder;")
                    ItemsPanel.Controls.Add(PhaseLabel)

                    PhasePanel.ID = PhasePanelID
                    PhasePanel.Attributes.Add("style", "display: grid; grid-template-columns: 49% 49%; justify-content: space-between; gap: var(--UWhitespace);")

                    ItemsPanel.Controls.Add(PhasePanel)
                End If

                PhasePanel.Controls.Add(myPanel)
                ItemsPanel.Attributes("style") = "display: flex; flex-direction: column; gap: var(--UWhitespace); overflow: auto;"

                If PhaseConfig(LabelKey)("PhaseOrder") > CurrPhaseOrder AndAlso Request.QueryString("Key") IsNot Nothing Then
                    PhasePanel.Enabled = False
                End If
            End If

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

                If TypeOf ctrl Is Label AndAlso Request.QueryString("Area") IsNot Nothing Then 'this means user is in ChecklistBuilder.aspx
                    Dim CtrlAsLabel As Label = DirectCast(ctrl, Label)

                    If CtrlAsLabel.Attributes("ColorBlindMessage") Then
                        'to simulate correct width for textbox control in iframe
                        CtrlAsLabel.Text = "*Error: Not a number*"
                        CtrlAsLabel.Style("visibility") = "hidden"
                    End If
                End If


                If TypeOf ctrl Is TextBox Then
                    myTextBox = CType(ctrl, TextBox)

                    If Request.QueryString("Key") IsNot Nothing Then 'this means user is logging inputs
                        Dim TbxId As String = "TextBox_" & LabelKey
                        Dim LabelKeyInput As New Dictionary(Of String, String)
                        myTextBox.ID = TbxId
                        DBConnections += "SetDBConnection('" & TbxId & "'); "
                        TbxToRange(LabelKey) = Range

                        Try 'if error occurs, T_LogData Inputs & OutOfRange values are NOT up to date
                            LabelKeyInput = Session("LabelInputMap")(LabelKey)
                            myTextBox.Text = LabelKeyInput("Value")
                        Catch ex As Exception
                            Dim My_DS2 As New Data.DataSet
                            Dim RC2 As Integer
                            Dim My_DR2 As Data.DataRow
                            Dim MapKey As Integer

                            QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
                                {"value", AreaFromQueryString},
                                {"typeOf", "int"}
                            }
                            My_DS2 = Security.GetMyDataSetParamQuery("SELECT [Key] FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=@AreaKey", QueryConfig)
                            RC2 = My_DS2.Tables(0).Rows.Count

                            For J = 0 To RC2 - 1
                                My_DR2 = My_DS2.Tables(0).Rows(J)
                                MapKey = My_DR2("Key")

                                If Not Session("LabelInputMap").ContainsKey(MapKey) Then
                                    'Session("LabelInputMap").Add(MapKey, "")
                                    Session("LabelInputMap").Add(MapKey, New Dictionary(Of String, String) From {
                                        {"Date", String.Empty},
                                        {"Operator", String.Empty},
                                        {"Value", String.Empty}
                                    })
                                End If

                                If Not LabelOutOfRangeMap.ContainsKey(MapKey) Then
                                    LabelOutOfRangeMap.Add(MapKey, Nothing)
                                End If
                            Next

                            QueryConfig.Remove("@AreaKey")
                            QueryConfig("@Inputs") = New Dictionary(Of String, String) From {
                                {"value", JsonSerializer.Serialize(Session("LabelInputMap"))},
                                {"typeOf", "string"}
                            }
                            QueryConfig("@OutOfRange") = New Dictionary(Of String, String) From {
                                {"value", JsonSerializer.Serialize(LabelOutOfRangeMap)},
                                {"typeOf", "string"}
                            }
                            QueryConfig("@T_LogDataKey") = New Dictionary(Of String, String) From {
                                {"value", KeyFromQueryString},
                                {"typeOf", "int"}
                            }

                            Security.ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogData] SET Inputs=@Inputs, OutOfRange=@OutOfRange WHERE [Key]=@T_LogDataKey", QueryConfig)

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
                    Dim FieldType As String = If(IsDBNull(DR("FieldType")), Nothing, DR("FieldType")) 'using ternary operator in case field value is NULL

                    If Request.QueryString("Area") IsNot Nothing Then 'this means user is in ChecklistBuilder.aspx
                        MalleableCtrl.Enabled = False
                    End If

                    If FieldType IsNot Nothing AndAlso MalleableCtrl.Attributes(FieldType) IsNot Nothing Then 'if FieldType is null, it is a standard textbox
                        MalleableCtrl.Attributes(FieldType) = True
                        MalleableCtrl.Visible = True
                        myTextBox.Style("display") = "none"

                        Dim InputCtrl As Control = ctrl.Controls(1)
                        Dim InputCtrlID As String
                        Dim Value As String = Session("LabelInputMap")(LabelKey)("Value")

                        Select Case FieldType
                            Case "Checkbox"
                                Dim CheckBox As CheckBox = DirectCast(ctrl.Controls(1), CheckBox)
                                InputCtrlID = "CheckBox_" & LabelKey
                                Dim Checked As String = If(Value = "1", "1", "") 'to prevent empty strings when checkbox is NOT checked
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
                                myTextBox.Text = Value
                                DirectCast(InputCtrl, TextBox).Text = Value
                            Case "Date"
                                Dim jsConfig As New Dictionary(Of String, String)

                                InputCtrlID = "Date_" & LabelKey
                                myTextBox.Text = Value
                                DirectCast(InputCtrl, TextBox).Text = Value

                                jsConfig("id") = InputCtrlID
                                jsConfig("validBackColor") = "#F5F5F5"
                                jsConfig("invalidBackColor") = "Red"

                                'DateFieldType += "DateFieldType('" & InputCtrlID & "');"
                                DateFieldType += "DateFieldType(" & JsonSerializer.Serialize(jsConfig) & ");"
                            Case "STC"
                                Dim BathTextBox As TextBox = DirectCast(ctrl.Controls(3), TextBox)
                                Dim IRGunTextBox As TextBox = DirectCast(ctrl.Controls(7), TextBox)
                                Dim BathTextBoxID As String = "BathTemp_" & LabelKey
                                Dim IRGunTextBoxID As String = "IrGunTemp_" & LabelKey
                                Dim UnderlyingTextBoxText As String = Value
                                Dim Temps As String() = UnderlyingTextBoxText.Split("/")

                                BathTextBox.ID = BathTextBoxID
                                IRGunTextBox.ID = IRGunTextBoxID

                                myTextBox.Text = UnderlyingTextBoxText

                                BathTextBox.Text = Temps(0)
                                IRGunTextBox.Text = If(Temps.Count > 1, Temps(1), String.Empty)

                                STC_TbxOverlays += "STC_TbxOverlay('" & BathTextBoxID & "'); STC_TbxOverlay('" & IRGunTextBoxID & "'); "

                                Continue For 'to avoid SetDBConnection being called on InputCtrl control
                            Case "DP"
                                Dim Dp1Box As CheckBox = DirectCast(ctrl.Controls(3), CheckBox)
                                Dim Dp2Box As CheckBox = DirectCast(ctrl.Controls(7), CheckBox)
                                Dim Dp1BoxID As String = "Dp1_" & LabelKey
                                Dim Dp2BoxID As String = "Dp2_" & LabelKey
                                Dim UnderlyingTextBoxText As String = Value
                                Dim Temps As String() = UnderlyingTextBoxText.Split("/")
                                Dim DpNums As String() = Range.Split("&")

                                Dp1Box.ID = Dp1BoxID
                                Dp2Box.ID = Dp2BoxID

                                myTextBox.Text = UnderlyingTextBoxText

                                Dp1Box.Checked = If(String.IsNullOrEmpty(Temps(0)) OrElse Temps(0) = 0, False, True)
                                Dp2Box.Checked = If(Temps.Count > 1 AndAlso Not String.IsNullOrEmpty(Temps(1)) AndAlso Temps(1) = 1, True, False)

                                Dp1Box.Text = Trim(DpNums(0))
                                Dp2Box.Text = Trim(DpNums(1))

                                DP_TbxOverlay += "DP_TbxOverlay('" & Dp1BoxID & "'); DP_TbxOverlay('" & Dp2BoxID & "'); "

                                Continue For 'to avoid SetDBConnection being called on InputCtrl control
                        End Select

                        InputCtrl.ID = InputCtrlID
                        DBConnections += "SetDBConnection('" & InputCtrlID & "'); "
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
        QueryConfig("@StampKey") = New Dictionary(Of String, String) From {
            {"value", sender.ID.Split("_")(1)},
            {"typeOf", "int"}
        }
        QueryConfig("@T_LogDataKey") = New Dictionary(Of String, String) From {
            {"value", MostRecentRecKey},
            {"typeOf", "int"}
        }
        QueryConfig("@User") = New Dictionary(Of String, String) From {
            {"value", User.Identity.Name.ToString},
            {"typeOf", "string"}
        }
        QueryConfig("@Date") = New Dictionary(Of String, String) From {
            {"value", System.DateTime.Now},
            {"typeOf", "string"}
        }
        Security.ExecuteSqlParamQuery("INSERT INTO [ALTS].[dbo].[T_LogStamp] (Active, StampKey, DataRecordKey, StampedBy, Date) VALUES (1, @StampKey, @T_LogDataKey, @User, @Date)", QueryConfig)
        sender.Text = User.Identity.Name.ToString
        sender.Enabled = False
        SetScrollPos()
    End Sub

    Protected Sub StcTextBox_onTextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim LabelKey As String = sender.ID.Split("_")(1)
        Dim BathTempTextBox As TextBox = DirectCast(sender.Parent.Parent.FindControl("BathTemp_") & LabelKey, TextBox)
        Dim IrGunTextBox As TextBox = DirectCast(sender.Parent.Parent.FindControl("IrGunTemp_") & LabelKey, TextBox)
        Dim DbValue As String = BathTempTextBox.Text & "/" & IrGunTextBox.Text
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

    Sub UploadToDataTable(LogOperator As String)
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
            .CommandText = "SELECT Top(1) * FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=@T_LogDataKey ORDER BY Date DESC" 'same query used to get MostRecRecordKey
            .Connection = Connection
            .Parameters.Add("@T_LogDataKey", SqlDbType.Int).Value = KeyFromQueryString
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
            My_DR("Operator") = LogOperator
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

    Public Function FindOverlayControl(Attribute As String, Parent As Control) As Control
        Dim ChildAsWebControl As WebControl

        For Each child As Control In Parent.Controls
            ChildAsWebControl = TryCast(child, WebControl)

            If ChildAsWebControl IsNot Nothing AndAlso ChildAsWebControl.Attributes(Attribute) IsNot Nothing Then
                Return child.Controls(1) 'at this point, child would be the parent panel control of the field type control
            End If

            If child.HasControls() Then
                FindOverlayControl(Attribute, child)
            End If
        Next
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
        Dim FieldType As String
        Dim Pnl As Panel = FindPnl(ControlID)
        Dim LabelKey As Integer
        Dim Diff As Boolean = False

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

            If TypeOf ctrl Is Label Then
                Dim CtrlAsLabel As Label = DirectCast(ctrl, Label)

                If CtrlAsLabel.Attributes("ColorBlindMessage") IsNot Nothing AndAlso FieldType = "Text" Then
                    CtrlAsLabel.Visible = False
                End If
            End If

            If TypeOf ctrl Is TextBox Then
                TextBox = CType(ctrl, TextBox)
                If UserInput Is Nothing Then UserInput = SqlProofSingleQuotes(TextBox.Text)
                LabelKey = TextBox.ID.Split("_")(1)
                QueryConfig("@LabelKey") = New Dictionary(Of String, String) From {
                    {"value", LabelKey},
                    {"typeOf", "int"}
                }
                FieldType = Security.GetSingleDbField("SELECT FieldType FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@LabelKey", QueryConfig, "FieldType")
                Range = TbxToRange(LabelKey)

                If FieldType IsNot Nothing Then
                    Select Case FieldType 'search for cases where the input would be valid
                        Case "Checkbox"
                            If UserInput = "1" Then Exit For
                        Case "HOA"
                            If Not UserInput.Contains("...") Then Exit For
                        Case "Text"
                            If Not String.IsNullOrEmpty(UserInput) Then Exit For
                        Case "Date"
                            Dim Res As String = LogAspx.ValidDate(UserInput)

                            If String.IsNullOrEmpty(Res) = False Then
                                SetPanelBackColor(System.Drawing.Color.Red, Res, Pnl)
                                DirectCast(FindOverlayControl(FieldType, Pnl), WebControl).BackColor = System.Drawing.Color.Red
                                Valid = False
                                Continue For
                            Else
                                Exit For
                            End If
                        Case "STC"
                            Dim Temps As String() = UserInput.Split("/")
                            Dim BackPanelColor As System.Drawing.Color
                            Dim Temp1 As Decimal
                            Dim Temp2 As Decimal

                            Try 'in case user types in invlaid characters
                                Temp1 = Decimal.Parse(Temps(0))
                                Temp2 = Decimal.Parse(Temps(1))
                            Catch ex As Exception
                                Exit Select
                            End Try

                            If Math.Abs(Temp1 - Temp2) > Decimal.Parse(Range.Split(" ")(1)) Then
                                BackPanelColor = System.Drawing.ColorTranslator.FromHtml("#E6E600")
                                SetPanelBackColor(BackPanelColor, "*CAUTION: OUT OF SPEC*", Pnl)
                            Else
                                BackPanelColor = System.Drawing.ColorTranslator.FromHtml("#F5F5F5")
                                SetPanelBackColor(BackPanelColor, "", Pnl)
                            End If

                            DirectCast(FindOverlayControl(FieldType, Pnl), WebControl).BackColor = BackPanelColor
                            Continue For
                        Case "DP"
                            Dim DPs As String() = UserInput.Split("/")
                            Dim BackPanelColor As System.Drawing.Color
                            Dim DP1 As Decimal
                            Dim DP2 As Decimal

                            Try 'in case user types in invlaid characters
                                DP1 = Decimal.Parse(DPs(0))
                                DP2 = Decimal.Parse(DPs(1))
                            Catch ex As Exception
                                Exit Select
                            End Try

                            If DP1 = 1 OrElse DP2 = 1 Then
                                BackPanelColor = System.Drawing.ColorTranslator.FromHtml("#F5F5F5")
                                SetPanelBackColor(BackPanelColor, "", Pnl)
                                Continue For
                            End If
                    End Select

                    'if here, input is NOT valid
                    SetPanelBackColor(System.Drawing.Color.Red, "", Pnl)
                    DirectCast(FindOverlayControl(FieldType, Pnl), WebControl).BackColor = System.Drawing.Color.Red
                    Valid = False
                    Continue For

                Else 'use range to validate input
                    InRange = True

                    'validate user input before considering the range
                    If Not Range.Contains("+/-") And Not Decimal.TryParse(UserInput, UserInputDec) Then 'check if value is a number
                        SetPanelBackColor(System.Drawing.Color.Red, "*ERROR: NOT A NUMBER*", Pnl)
                        Valid = False
                        Continue For
                    End If

                    'validate user input using the range
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
                        If FieldType Is Nothing Then 'make sure FieldType is 'Number'
                            SetPanelBackColor(System.Drawing.ColorTranslator.FromHtml("#E6E600"), "*CAUTION: OUT OF RANGE*", Pnl)
                            Continue For
                        End If
                    End If

                End If

                'if here, value is valid and in range
                SetPanelBackColor(System.Drawing.ColorTranslator.FromHtml("#F5F5F5"), "", Pnl)
            End If
        Next
        Session("LabelInputMap")(LabelKey)("Value") = UserInput

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

        UploadToDataTable(User.Identity.Name.ToString)
        Return All_InputsAreValid
    End Function

    'Protected Sub LogAreasDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    Response.Redirect(WebpageUrl & "?Area=" & LogAreasDropDownList.SelectedValue)
    'End Sub

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
        UploadToDataTable(User.Identity.Name.ToString)
        Session("DisplayError") = False
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

    Function ModifyInput(ID As String, Value As String) As Boolean
        Dim LabelKey As String = ID.Split("_")(1)
        Dim InputOfInterest As Dictionary(Of String, String) = Session("LabelInputMap")(LabelKey)
        Dim SatiUser As String = User.Identity.Name.ToString()
        Dim PrevValue As String = InputOfInterest("Value")

        Session("DisplayError") = False

        If Value = PrevValue Then Return False 'value has NOT changed, so do NOT modify input

        InputOfInterest("Operator") = SatiUser
        InputOfInterest("Date") = Format.DateField(System.DateTime.Now.ToString())

        ValidateInput(ID, Value)
        UploadToDataTable(SatiUser)

        Try 'in case user in on last input, in which case sql will return 'There is no row at position 0.'
            QueryConfig("@LabelKey") = New Dictionary(Of String, String) From {
                {"value", LabelKey},
                {"typeOf", "int"}
            }

            If String.IsNullOrEmpty(Value) OrElse (Value.Contains("/") AndAlso ID.Contains("Date") = False) Then ' field value went from not empty to empty OR STC fieldtype (js handles cursor focus)
                QueryConfig("@T_LogDataKey") = New Dictionary(Of String, String) From {
                    {"value", KeyFromQueryString},
                    {"typeOf", "int"}
                }
                Security.ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogData] SET KeyOfLastLabel=@LabelKey WHERE [Key]=@T_LogDataKey", QueryConfig)
            Else
                Dim NextLabelKey As Integer = Security.GetSingleDbField("SELECT TOP(1) [Key], AreaKey, Label, LabelOrder FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=(SELECT AreaKey FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@LabelKey) AND LabelOrder > (SELECT LabelOrder FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@LabelKey) ORDER BY LabelOrder", QueryConfig, "Key")

                QueryConfig("@LabelKey")("value") = NextLabelKey
                QueryConfig("@DataKey") = New Dictionary(Of String, String) From {
                    {"value", MostRecentRecKey},
                    {"typeOf", "int"}
                }
                Security.ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogData] SET KeyOfLastLabel=@LabelKey WHERE [Key]=@DataKey", QueryConfig) 'update KeyOfLastLabel field in DB
            End If
        Catch ex As Exception

        End Try

        Return True
    End Function

    Sub UpdateNote(ID As String, Comment As String)
        QueryConfig("@T_LogOperatorCommentsKey") = New Dictionary(Of String, String) From {
            {"value", ID},
            {"typeOf", "int"}
        }
        QueryConfig("@Comment") = New Dictionary(Of String, String) From {
            {"value", Comment},
            {"typeOf", "string"}
        }
        Security.ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogOperatorComments] SET Comment=@Comment WHERE [Key]=@T_LogOperatorCommentsKey", QueryConfig)
    End Sub

    Sub DeleteNote(ID As String)
        QueryConfig("@T_LogOperatorCommentsKey") = New Dictionary(Of String, String) From {
            {"value", ID},
            {"typeOf", "int"}
        }
        Security.ExecuteSqlParamQuery("DELETE FROM [ALTS].[dbo].[T_LogOperatorComments] WHERE [Key]=@T_LogOperatorCommentsKey", QueryConfig)
    End Sub

    Sub Update_All_InputsValid_Field()
        Dim All_InputsValid As Boolean = True

        For Each Pnl As Panel In VisiblePanels
            If Not ValidateInput(Pnl.ID, Nothing) Then
                All_InputsValid = False
            End If
        Next

        Try 'in case There is no row at position 0
            QueryConfig("@T_LogDataKey") = New Dictionary(Of String, String) From {
                {"value", MostRecentRecKey},
                {"typeOf", "int"}
            }
            If Not All_InputsValid Then
                Security.ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogData] Set All_InputsValid=0 WHERE [Key]=@T_LogDataKey", QueryConfig)
            Else
                Security.ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogData] Set All_InputsValid=1 WHERE [Key]=@T_LogDataKey", QueryConfig)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub DoneButton_Click(sender As Object, e As EventArgs)
        ''if here, all fields are valid, because 'Exit Sub' statement has NOT been run
        UploadToDataTable(User.Identity.Name.ToString)
        MarkAsDone()
    End Sub

    Protected Sub UndoDoneButton_Click(sender As Object, e As EventArgs)
        'undo done & inactivate stamps on log
        QueryConfig("@T_LogDataKey") = New Dictionary(Of String, String) From {
            {"value", KeyFromQueryString},
            {"typeOf", "int"}
        }
        Security.ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogStamp] SET Active=0 WHERE DataRecordKey=@T_LogDataKey And Active=1; UPDATE [ALTS].[dbo].[T_LogData] SET CompleteLog=0, Ranges=NULL WHERE [Key]=@T_LogDataKey;", QueryConfig)
        SetScrollPos()
    End Sub

    Sub MarkAsDone()
        Dim LabelRangeMap As New Dictionary(Of Integer, String)
        'update Ranges field (constuct & stringify Dictionary, then run update query)
        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaFromQueryString},
            {"typeOf", "int"}
        }
        DS = Security.GetMyDataSetParamQuery("SELECT [Key], Range FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=@AreaKey", QueryConfig)
        DRC = DS.Tables(0).Rows

        For I = 0 To DRC.Count - 1
            DR = DRC(I)
            LabelRangeMap.Add(DR("Key"), If(IsDBNull(DR("Range")), Nothing, DR("Range")))
        Next

        QueryConfig.Remove("@AreaKey")
        QueryConfig("@Ranges") = New Dictionary(Of String, String) From {
            {"value", JsonSerializer.Serialize(LabelRangeMap)},
            {"typeOf", "string"}
        }
        QueryConfig("@T_LogDataKey") = New Dictionary(Of String, String) From {
            {"value", KeyFromQueryString},
            {"typeOf", "int"}
        }
        Security.ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogData] SET Ranges=@Ranges, CompleteLog=1 WHERE [Key]=@T_LogDataKey", QueryConfig) 'record to 'Ranges' field in T_LogData
        Update_All_InputsValid_Field()
        Response.Redirect("~/ChecklistLogging/StatusBoard.aspx")
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

        QueryConfig("@T_LogDataKey") = New Dictionary(Of String, String) From {
            {"value", MostRecentRecKey},
            {"typeOf", "int"}
        }
        QueryConfig("@Note") = New Dictionary(Of String, String) From {
            {"value", TextBoxText},
            {"typeOf", "string"}
        }
        Security.ExecuteSqlParamQuery("INSERT INTO [ALTS].[dbo].[T_LogOperatorComments] VALUES (@T_LogDataKey, @Note)", QueryConfig)
        SetScrollPos()
    End Sub

    Protected Sub MarkAsDoneCheckBox_OnCheckedChanged(sender As Object, e As EventArgs)
        If sender.Checked Then
            MarkAsDone()
            MarkAsDoneCheckBox.Visible = False
        End If
    End Sub

    Sub hold()
        '<asp:Button ID="Button1" runat="server" Text="Auto Batch Stock (Polish)" Height="125px" Width="258px" LabelTip="Master drive fault: n4 outer pin ring, n2 lower plate, n3 inner pin ring. Audible grinding noise heard emanating from outer pin ring gearbox/motor assembly area. Grinding most audible in second half of brush cycle when spin direction changes. " BackColor="#33CC33"/>
        '<asp:Button ID="Button2" runat="server" Text="Auto Batch Stock / 3500 (DSP)" Height="50px" Width="300px" BackColor="#FFFF66"/>
        '<asp:Button ID="Button3" runat="server" Text="Button" Height="112px" Width="644px" BackColor="Red"/>
        '<asp:Button ID="Button4" runat="server" Text="Button" OnClick="myclick" CommandArgument="themrnumber"/>
    End Sub

    Sub BuildDynamicAsp()
        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaFromQueryString},
            {"typeOf", "int"}
        }

        If Request.QueryString("Key") IsNot Nothing Then 'if true, user is filling out a log sheet
            QueryConfig("@T_LogDataKey") = New Dictionary(Of String, String) From {
                {"value", MostRecentRecKey},
                {"typeOf", "int"}
            }
            DS = Security.GetMyDataSetParamQuery("Select T.Title, L.[Key] As ID, S.[Key] As StampedRecordKey, S2.StampedBy As StampedBy, T.RoleID FROM [ALTS].[dbo].[T_LogStamp] S RIGHT JOIN [ALTS].[dbo].[T_LogStampList] L On L.[Key]=S.StampKey AND S.Active=1 AND DataRecordKey=@T_LogDataKey INNER JOIN [ALTS].[dbo].[T_LogStampTitle] T On L.TitleKey=T.[Key] LEFT JOIN [ALTS].[dbo].[T_LogStamp] S2 On S.[Key]=S2.[Key] AND S.Active=1 WHERE AreaKey=@AreaKey AND L.Active=1", QueryConfig)
            QueryConfig.Remove("@T_LogDataKey")
        Else 'user is in ChecklistBuilder.aspx editing or creating a checklist
            DS = Security.GetMyDataSetParamQuery("SELECT Stamped.Title, Stamp.[Key] As ID FROM [ALTS].[dbo].[T_LogStampList] Stamp INNER JOIN [ALTS].[dbo].[T_LogStampTitle] Stamped ON Stamp.TitleKey=Stamped.[Key] WHERE Active=1 AND AreaKey=@AreaKey", QueryConfig)
        End If

        DRC = DS.Tables(0).Rows

        'dynamically create Stamp related controls
        For I = 0 To DRC.Count - 1
            DR = DRC(I)

            Dim Panel As New Panel()
            Dim Button As New Button()
            Dim Label As New Label()
            Dim StampBorderColor As String = "red"
            Dim ButtonText As String = "Stamp"

            Panel.Controls.Add(Label)
            Panel.Controls.Add(Button)
            StampPanel.Controls.Add(Panel)

            Panel.Attributes.Add("style", "display: flex; flex-direction: column;")
            Label.Text = DR("Title") & ":"
            Button.ID = "StampList_" & DR("ID")

            If Request.QueryString("Key") IsNot Nothing AndAlso IsDBNull(DR("StampedBy")) = False Then
                ButtonText = DR("StampedBy")
                StampBorderColor = "#33CC33"
            End If

            Button.Enabled = False
            Button.Text = ButtonText
            Panel.Attributes.Add("style", "padding: var(--UWhitespace); border: 5px solid " & StampBorderColor) 'control in 'Panel' variable is the parent of the 'Button' control

            If Request.QueryString("Key") IsNot Nothing Then 'if true, user is filling out a log sheet
                Dim UserContainsRole As Boolean
                AddHandler Button.Click, AddressOf Stamp_OnClick

                'if log is complete, stamp does NOT exist, AND user has the associated role to stamp, enable button
                If Button.Text = "Stamp" Then
                    QueryConfig.Clear()
                    QueryConfig("@RoleId") = New Dictionary(Of String, String) From {
                        {"value", DR("RoleID")},
                        {"typeOf", "string"}
                    }
                    QueryConfig("@LogStampListKey") = New Dictionary(Of String, String) From {
                        {"value", Button.ID.Split("_")(1)},
                        {"typeOf", "int"}
                    }
                    QueryConfig("@User") = New Dictionary(Of String, String) From {
                        {"value", User.Identity.Name.ToString},
                        {"typeOf", "string"}
                    }
                    UserContainsRole = Security.GetSingleDbField("SELECT COUNT(RoleName) As ContainsRole FROM [SatiUsers].[dbo].aspnet_UsersInRoles INNER JOIN [SatiUsers].[dbo].aspnet_Users On [SatiUsers].[dbo].aspnet_UsersInRoles.UserId = [SatiUsers].[dbo].aspnet_Users.UserId INNER JOIN [SatiUsers].[dbo].aspnet_Roles On [SatiUsers].[dbo].aspnet_UsersInRoles.RoleId = [SatiUsers].[dbo].aspnet_Roles.RoleId INNER JOIN [ALTS].[dbo].[T_LogStampList] On [SatiUsers].[dbo].aspnet_Roles.RoleId=@RoleId WHERE [ALTS].[dbo].[T_LogStampList].[Key]=@LogStampListKey And [SatiUsers].[dbo].aspnet_Users.UserName=@User", QueryConfig, "ContainsRole")

                    If UserContainsRole AndAlso LogDR("CompleteLog") Then
                        Button.Enabled = True
                    End If
                End If
            End If
        Next

        QueryConfig.Clear()
        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaFromQueryString},
            {"typeOf", "int"}
        }

        'dynamically create Comment related controls
        DS = Security.GetMyDataSetParamQuery("SELECT Comment FROM [ALTS].[dbo].[T_LogCommentList] WHERE AreaKey=@AreaKey ORDER BY CommentOrder", QueryConfig)
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

    Protected Sub UploadFile(sender As Object, e As EventArgs)
        Dim fileNameDelimited As String()
        Dim Format As String
        Dim TestFile As String
        Dim match As Match
        Dim FileFormat As String
        Dim fileName As String

        If Not Uploader.HasFile Then
            '    ErrorMessage.Text = "CHOOSE A FILE BEFORE UPLOADING"
            Exit Sub
        End If

        fileName = IO.Path.GetFileName(Uploader.FileName) 'using session state variable because global variables do NOT retain values assinged within this function
        TestFile = fileName
        fileNameDelimited = fileName.Split(".")
        Format = fileNameDelimited(fileNameDelimited.Count - 1)
        match = Regex.Match(fileName, "[% < > : / \ | ? *]")
        FileFormat = fileName.Split(".")(1)

        'Check for format other than an image
        If Not AcceptedFormats.Contains(FileFormat) Then
            'AcceptedFormat = False
        End If

        'REMOVES CHARACTERS THAT ARENT ALLOWED IN FILE NAMES
        Do While match.Success
            Dim key As String = match.Value
            TestFile = TestFile.Replace(key, String.Empty)
            match = match.NextMatch()
        Loop
        fileName = TestFile

        If Not System.IO.File.Exists(uploadDirectory) Then
            System.IO.Directory.CreateDirectory(uploadDirectory)
        End If

        Session("FileUploadDirectory") = Path.Combine(uploadDirectory, fileName)
        Uploader.PostedFile.SaveAs(Session("FileUploadDirectory"))

        'variables declared in UploadFile do NOT hold their value, so I tied them to the session
        Session("ContentType") = If(FormatToContentType.ContainsKey(Format), FormatToContentType(Format), Format)

        PreviewPanel_iframe.Visible = True
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "iframeEnabled", "iframeEnabled(true);", True)
        PreviewPanel_iframe.Attributes.Add("src", "/ChecklistLogging/AddPhoto.aspx" & "?" & Request.RawUrl.Split("?")(1) & "&DataKey=" & KeyFromQueryString & "&fileName=" & fileName)
    End Sub


    Protected Sub ResetLog_OnClick(sender As Object, e As EventArgs)
        Dim LabelKeys As List(Of Integer) = LabelOutOfRangeMap.Keys.ToList()

        'reset Inputs, OutOfRange, and Operator field values in DB
        For i As Integer = 0 To LabelKeys.Count - 1
            Dim LabelKey As Integer = LabelKeys(i)
            'Session("LabelInputMap")(LabelKey) = String.Empty
            Session("LabelInputMap")(LabelKey) = New Dictionary(Of String, String) From {
                {"Date", String.Empty},
                {"Operator", String.Empty},
                {"Value", String.Empty}
            }
            LabelOutOfRangeMap(LabelKey) = Nothing
        Next
        UploadToDataTable(Nothing)

        'delete associated photos & notes for the log instance
        QueryConfig("@T_LogDataKey") = New Dictionary(Of String, String) From {
            {"value", KeyFromQueryString},
            {"typeOf", "int"}
        }
        Security.ExecuteSqlParamQuery("DELETE FROM [ALTS].[dbo].[T_LogOperatorComments] WHERE CommentKey=@T_LogDataKey; DELETE FROM [ALTS].[dbo].[T_LogDataPhotos] WHERE DataKey=@T_LogDataKey", QueryConfig)

        Response.Redirect("~/ChecklistLogging/StatusBoard.aspx")
    End Sub
End Class

