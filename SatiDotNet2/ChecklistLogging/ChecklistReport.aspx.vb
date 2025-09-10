Imports System.Text.Json
Imports SatiDotNet2.Library
Imports System.Security.Cryptography
Imports FlexCel.Core
Imports System.Text


Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim CurrUser As New SatiUser(User.Identity.Name.ToString())
    Dim ChecklistBuilder As New MaintPM
    Dim Department As String = CurrUser.GetDepartment()
    Dim DepartmentKey As String = CurrUser.GetDepartmentKey()
    Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
    Dim GroupFromQueryString As String
    Dim AreaFromQueryString As String
    Dim LabelsFromQs As String
    Dim StartDateFromQueryString As String
    Dim EndDateFromQueryString As String
    Dim PageIdxFromQueryString As String
    Dim AdminFromQueryString As String
    Private ViewFiltersFromQs As String
    Private Shared Security As New Security()
    Private Shared Format As New Format()
    Private SatiCode As New Class1()
    Private MaintPM As New MaintPM()
    Private PhaseController As New PhaseController()
    Private QsKeys As New List(Of String) From {"Group", "AreasToInclude", "LabelsToInclude", "StartDate", "EndDate", "PageIdx", "Admin", "ViewFilters"}
    Private _Report As New Report()
    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        'to ensure each user of this webpage gets their own class objects
        If Session("Report") Is Nothing Then
            Session("Report") = New Report(New Dictionary(Of String, String) From {
                {"GroupKey", 0},
                {"AreaKey", 0}
            })
        End If

        If Session("AspWebpage") Is Nothing Then
            Session("AspWebpage") = New AspWebpage("/ChecklistLogging/ChecklistReport.aspx", QsKeys)
        End If

        ' MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        Me.MaintainScrollPositionOnPostBack = True
        GroupFromQueryString = Request.QueryString("Group")
        AreaFromQueryString = Request.QueryString("AreasToInclude")
        LabelsFromQs = Request.QueryString("LabelsToInclude")
        PageIdxFromQueryString = Request.QueryString("PageIdx")
        StartDateFromQueryString = Request.QueryString("StartDate")
        EndDateFromQueryString = Request.QueryString("EndDate")
        ViewFiltersFromQs = Request.QueryString("ViewFilters")

        If StartDateFromQueryString IsNot Nothing AndAlso EndDateFromQueryString IsNot Nothing Then
            GroupDropDownList.Enabled = True
        End If

        Try
            AdminFromQueryString = Request.QueryString("Admin")

            If AdminFromQueryString Then
                MenuAuthenication.CheckGroupsAuthenication({"admin", "FMManagerApproval"}, Server)
                ReportGridView.Columns(ReportGridView.Columns.Count - 1).Visible = True 'make CommandField visible
            End If
        Catch ex As Exception
            AdminFromQueryString = Nothing
        End Try
        Session("AspWebpage").SetUrl("Admin", AdminFromQueryString) 'always set Admin querystring

        If GroupFromQueryString IsNot Nothing Then
            If GroupFromQueryString > 0 Then
                ExportButton.Enabled = True
            End If
        End If

        'set Visible property for ViewFilters_Panel and Checked property for ViewFilters_CheckBox
        'if "ViewFilters" does NOT exist, set properties mentioned above to true, meaning checkbox is checked by p
        'if the above does not occur, a double click on ViewFilters_CheckBox will be required on initial load of webpage for proper functionality
        Dim ViewFilters As Boolean = True
        If ViewFiltersFromQs IsNot Nothing Then ViewFilters = ViewFiltersFromQs
        ViewFilters_Panel.Visible = ViewFilters
        ViewFilters_CheckBox.Checked = ViewFilters
    End Sub

    Private Function TextBoxDateFormat(DateStr As String) As String
        Try 'format expected by HTML5 <input type="date">
            Return Date.Parse(DateStr).ToString("yyyy-MM-dd")
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        StartDate_TextBox.Text = TextBoxDateFormat(StartDateFromQueryString)
        EndDate_TextBox.Text = TextBoxDateFormat(EndDateFromQueryString)
        ReportGridView.PageIndex = PageIdxFromQueryString

        If GroupFromQueryString IsNot Nothing Then
            SetGridViewSrc()

            If GroupDropDownList.Items(0).Text = "Select Group..." Then
                GroupDropDownList.Items.RemoveAt(0)
            End If
        End If

        'set CheckBox overlay where FieldType is 'CheckBox'
        For Each Row As GridViewRow In ReportGridView.Rows
            If Row.RowType = DataControlRowType.DataRow Then
                FieldTypeOverlay(Row, Sub(CheckBox As CheckBox)
                                          If CheckBox IsNot Nothing Then 'if true, FieldType is CheckBox. However, control is not the one we're looking for
                                              Dim ReportValue_CheckBox As CheckBox = CType(Row.FindControl("ReportValue_CheckBox"), CheckBox)
                                              Dim ReportValue_Label As Label = CType(Row.FindControl("ReportValue_Label"), Label)

                                              If ReportValue_Label IsNot Nothing Then 'do not know why, but this logic doesn't work properly w/o this condition
                                                  ReportValue_Label.Visible = False

                                                  ReportValue_CheckBox.Visible = True
                                                  ReportValue_CheckBox.Checked = If(ReportValue_Label.Text = "1", True, False)
                                              End If
                                          End If
                                      End Sub)
            End If
        Next

        ClientScript.RegisterStartupScript(Me.GetType(), "GridViewCols", "ColWidths(" & JsonSerializer.Serialize(Of Dictionary(Of String, String))(Session("Report").GetMaxFieldVals()) & ");", True)

        'build AreaCheckBoxList children dynamically using DS variable
        If GroupFromQueryString IsNot Nothing AndAlso GroupFromQueryString <> 0 Then 'if AreaFromQueryString is nothing or 0, then it area ddl is at 'All'
            Dim AreasDS As New Data.DataSet
            Dim AreasList As New List(Of Integer)
            Dim Cbx As ListItem

            FilterChecklists_Button.Enabled = True

            QueryConfig("@GroupKey") = New Dictionary(Of String, String) From {
                {"value", GroupFromQueryString},
                {"typeOf", "int"}
            }
            AreasDS = Security.GetMyDataSetParamQuery("SELECT A.[Key] As AreaKey, A.Area FROM [ALTS].[dbo].[T_LogArea] A WHERE GroupKey=@GroupKey AND A.Status='live' ORDER BY A.Area", QueryConfig)

            Try 'in case AreaFromQueryString is null
                AreasList = JsonSerializer.Deserialize(Of List(Of Integer))(AreaFromQueryString)
            Catch ex As Exception
                AreasList = Nothing
            End Try

            AreaCheckBoxList.Items.Clear()
            For Each AreaDR As Data.DataRow In AreasDS.Tables(0).Rows
                Dim AreaKey As String = AreaDR("AreaKey")
                Dim Area As String = AreaDR("Area")

                Cbx = New ListItem(Area, AreaKey)
                AreaCheckBoxList.Items.Add(Cbx)

                'if AreasList is nothing, that means user has NOT interacted with AreaCheckBoxList
                'that means all Areas are included in the DataSet for the GroupKey
                If AreasList Is Nothing OrElse AreasList.Contains(AreaKey) Then
                    Cbx.Selected = True
                End If
            Next
            If AreasList Is Nothing OrElse AreasList.Count = AreasDS.Tables(0).Rows.Count Then CheckAllChecklists_CheckBox.Checked = True

            'build LabelCbxList children dynamically using DS variable
            Try 'in case AreaFromQueryString is nothing
                Dim LabelsHash As Dictionary(Of Integer, String) = Session("Report").GetLabels()
                Dim LabelsToInclude As List(Of Integer)

                If LabelsFromQs Is Nothing Then
                    LabelsToInclude = LabelsHash.Keys.ToList()
                Else
                    LabelsToInclude = JsonSerializer.Deserialize(Of List(Of Integer))(LabelsFromQs)
                End If

                'condition below ensures 1 checklist is being selected for reporting
                If AreasList.Count = 1 Then
                    Dim DataIsOrderedByDate As Boolean = Session("Report").OrderedByDate()

                    FilterLabels_Button.Enabled = True
                    If DataIsOrderedByDate Then
                        OrderByDateRB.Checked = True
                    Else
                        OrderByInputRB.Checked = True
                    End If

                    LabelCbxList.Items.Clear()
                    For Each kvp As KeyValuePair(Of Integer, String) In LabelsHash
                        Dim LabelKey As Integer = kvp.Key
                        Dim Label As String = kvp.Value
                        Dim Cbx1 As New ListItem(Label, LabelKey)

                        LabelCbxList.Items.Add(Cbx1)

                        If LabelsToInclude.Contains(LabelKey) Then
                            Cbx1.Selected = True
                        End If
                    Next

                    If LabelsToInclude.Count = LabelsHash.Count Then CheckAll_CheckBox.Checked = True
                End If
            Catch ex As Exception

            End Try
        End If


    End Sub

    Private Sub Page_PreRenderComplete(sender As Object, e As EventArgs) Handles Me.PreRenderComplete
        GroupDropDownList.SelectedValue = GroupFromQueryString

        For Each ListItem As ListItem In GroupDropDownList.Items
            If ListItem.Text = "All" Then Exit Sub
        Next

        GroupDropDownList.Items.Add(New ListItem("All", "0"))
    End Sub

    'Public Sub ConfigureAreas()
    '    Dim AreasToExclude As New List(Of Integer)
    '    Dim AreasToInclude As List(Of Integer)
    '    Dim AreasDS As New Data.DataSet

    '    If GroupFromQueryString IsNot Nothing Then
    '        QueryConfig("@GroupKey") = New Dictionary(Of String, String) From {
    '            {"value", GroupFromQueryString},
    '            {"typeOf", "int"}
    '        }
    '        AreasDS = Security.GetMyDataSetParamQuery("SELECT A.[Key] As AreaKey, A.Area FROM [ALTS].[dbo].[T_LogArea] A WHERE GroupKey=@GroupKey ORDER BY A.Area", QueryConfig)
    '        For Each AreasDR As Data.DataRow In AreasDS.Tables(0).Rows
    '            Dim AreaKey As Integer = AreasDR("AreaKey")

    '            If AreasToExclude.Contains(AreaKey) = False Then AreasToExclude.Add(AreaKey)
    '        Next

    '        'find out elements to add/remove from AreasToInclude List data structure
    '        AreasToInclude = New List(Of Integer)(AreasToExclude)
    '        For Each ListItem As ListItem In AreaCheckBoxList.Items
    '            Dim Value As String = ListItem.Value

    '            If ListItem.Selected = False Then
    '                AreasToInclude.Remove(Value)
    '            Else
    '                AreasToExclude.Remove(Value)
    '            End If
    '        Next

    '        Session("AspWebpage").SetUrl("AreasToInclude", JsonSerializer.Serialize(Of List(Of Integer))(AreasToInclude))
    '        Session("Report").SetAreas(AreasToInclude)
    '    End If

    'End Sub

    Protected Sub UpdateLabelsButton_OnClick(sender As Object, e As EventArgs)
        Dim LabelsToInclude As New List(Of Integer)
        Dim LabelsToIncludeStringified As String = String.Empty

        For Each LabelCbx As ListItem In LabelCbxList.Items
            Dim LabelKey As Integer = LabelCbx.Value

            If LabelCbx.Selected Then 'if selected property is true, it means it's checked
                LabelsToInclude.Add(LabelKey)
            ElseIf LabelCbx.Selected = False Then
                LabelsToInclude.Remove(LabelKey)
            End If
        Next

        If OrderByDateRB.Checked Then
            Session("Report").OrderDSByDate()
        Else
            Session("Report").UndoOrderDSByDate()
        End If

        LabelsToIncludeStringified = JsonSerializer.Serialize(Of List(Of Integer))(LabelsToInclude)
        Session("AspWebpage").SetUrl("LabelsToInclude", LabelsToIncludeStringified)
        Session("Report").SetLabels(LabelsToInclude)

        RefreshPreview() 'to have LabelsToInclude in querystring take effect
    End Sub

    Protected Sub UpdateChecklistsButton_OnClick(sender As Object, e As EventArgs)
        SetPmAndChecklists()

        LabelCbxList.Items.Clear()
        Session("AspWebpage").SetUrl("LabelsToInclude", Nothing)

        RefreshPreview() 'to see changes in AreasToInclude and LabelsToInclude querystring keys
    End Sub

    Private Sub SetPmAndChecklists()
        Dim AreasToInclude As New List(Of Integer) 'b/c interacting with Session("AreasToInclude") directly tends to cause issues

        For Each AreaCheckBox As ListItem In AreaCheckBoxList.Items
            Dim AreaKey As Integer = AreaCheckBox.Value

            If AreaCheckBox.Selected Then 'if selected property is true, it means it's checked
                AreasToInclude.Add(AreaKey)
            ElseIf AreaCheckBox.Selected = False Then
                AreasToInclude.Remove(AreaKey)
            End If
        Next

        Session("AreasToInclude") = New List(Of Integer)(AreasToInclude)
        Session("AspWebpage").SetUrl("AreasToInclude", JsonSerializer.Serialize(Of List(Of Integer))(AreasToInclude))
        Session("Report").SetAreas(Session("AreasToInclude"))
    End Sub

    Protected Sub SetGridViewSrc()
        Dim DS As Data.DataSet = Session("Report").GetDS()

        ReportGridView.DataSource = DS.Tables(0)
        ReportGridView.DataBind()
    End Sub

    Protected Sub ReportGridView_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles ReportGridView.PageIndexChanging
        'PageIdxFromQueryString = e.NewPageIndex
        Session("AspWebpage").SetUrl("PageIdx", e.NewPageIndex)
        'SetGridViewSrc()
        RefreshPreview()
    End Sub

    Protected Sub GroupDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim SelectedValue As String = GroupDropDownList.SelectedValue

        Session("AspWebpage").SetUrl("AreasToInclude", Nothing)
        Session("AspWebpage").SetUrl("LabelsToInclude", Nothing)

        Session("AspWebpage").SetUrl("Group", SelectedValue)
        Session("Report").SetGroup(SelectedValue)

        LabelCbxList.Items.Clear()

        RefreshPreview()
    End Sub

    Sub RefreshPreview()
        Response.Redirect(Session("AspWebpage").GetUrl())
    End Sub

    Protected Function PickDate(QsDate As String, PickedDate As Date) As Boolean
        If QsDate IsNot Nothing AndAlso PickedDate = QsDate Then
            Return True
        Else
            Return False
        End If
    End Function

    Protected Sub ReportGridView_RowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub

    Private Sub ReportGridView_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles ReportGridView.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow AndAlso e.Row.RowState.HasFlag(DataControlRowState.Edit) Then
            Dim Row As GridViewRow = e.Row
            Dim ReportLabelKey_Label As Label = CType(Row.FindControl("ReportLabelKey_Label"), Label)
            Dim ReportValue_TextBox As TextBox = CType(Row.FindControl("ReportValue_TextBox"), TextBox)
            Dim ReportDate_TextBox As TextBox = CType(Row.FindControl("ReportDate_TextBox"), TextBox)
            Dim ReportOperator_DropDownList As DropDownList = CType(e.Row.FindControl("ReportOperator_DropDownList"), DropDownList)

            'dynamically assign dataset to ReportOperator_DropDownList
            ReportOperator_DropDownList.DataSource = Session("Report").GetOperators().Tables(0)
            ReportOperator_DropDownList.DataTextField = "Operator"
            ReportOperator_DropDownList.DataValueField = "Operator"
            ReportOperator_DropDownList.DataBind()
            ReportOperator_DropDownList.SelectedValue = CType(Row.FindControl("ReportOperatorHidden_Label"), Label).Text
            ReportOperator_DropDownList.Items.Insert(0, New ListItem("Select Operator...", String.Empty))

            If Session("EditModeValues") IsNot Nothing Then
                Dim EditModeValues As Dictionary(Of String, String) = Session("EditModeValues")

                'doing this b/c EditTemplate controls returns to DB field value after postback
                ReportDate_TextBox.Text = EditModeValues("Date")
                ReportValue_TextBox.Text = EditModeValues("Value")
                ReportOperator_DropDownList.SelectedValue = EditModeValues("Operator")

                Row.FindControl("InvalidReportDate_Label").Visible = True
            End If

            'incorporate Checkbox asp overlay if needed
            FieldTypeOverlay(Row, Sub(CheckBox As CheckBox)
                                      CheckBox.Checked = If(ReportValue_TextBox.Text = "1", True, False)
                                      ReportValue_TextBox.Visible = False
                                      CType(Row.FindControl("CheckBox_Panel"), Panel).Visible = True
                                  End Sub)
        End If
    End Sub

    Private Sub ReportGridView_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles ReportGridView.RowUpdating
        Dim Row As GridViewRow = ReportGridView.Rows(Convert.ToInt32(ReportGridView.EditIndex))
        Dim StartDate_Label As Label = CType(Row.FindControl("StartDate_Label"), Label)
        Dim ReportLabelKey_Label As Label = CType(Row.FindControl("ReportLabelKey_Label"), Label)
        Dim ReportValue_TextBox As TextBox = CType(Row.FindControl("ReportValue_TextBox"), TextBox)
        Dim ReportDate_TextBox As TextBox = CType(Row.FindControl("ReportDate_TextBox"), TextBox)
        Dim ReportOperator_DropDownList As DropDownList = CType(Row.FindControl("ReportOperator_DropDownList"), DropDownList)
        Dim InputDate As String = ReportDate_TextBox.Text
        Dim InputValue As String = ReportValue_TextBox.Text
        Dim InputOperator As String = ReportOperator_DropDownList.SelectedValue
        Dim Config As New Dictionary(Of String, String) From {
            {"LabelKey", ReportLabelKey_Label.Text},
            {"Date", StartDate_Label.Text}
        }
        Dim Mods As New Dictionary(Of String, String) From {
            {"Value", InputValue},
            {"Date", InputDate},
            {"Operator", InputOperator}
        }

        'the callback subroutine below will only run when the fieldtype is checkbox
        FieldTypeOverlay(Row, Sub(CheckBox As CheckBox)
                                  InputValue = If(CheckBox.Checked, 1, 0)
                                  Mods("Value") = InputValue
                              End Sub)

        If Format.ValidLogDate(InputDate) Then
            Session.Remove("EditModeValues")
            Session("Report").Override(Config, Mods, True)

            ReportGridView.EditIndex = -1
            SetGridViewSrc()
        Else
            Session("EditModeValues") = New Dictionary(Of String, String) From {
                {"Date", InputDate},
                {"Value", InputValue},
                {"Operator", InputOperator}
            }
        End If
    End Sub

    Private Sub FieldTypeOverlay(Row As GridViewRow, Callback As Action(Of CheckBox))
        Dim ReportLabelKey_Label As Label = CType(Row.FindControl("ReportLabelKey_Label"), Label)
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim FieldType As String

        QueryConfig("@LabelKey") = New Dictionary(Of String, String) From {
            {"value", ReportLabelKey_Label.Text},
            {"typeOf", "int"}
        }
        FieldType = Security.GetSingleDbField("SELECT FieldType FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@LabelKey", QueryConfig, "FieldType")

        If FieldType IsNot Nothing Then
            Dim DbValueCtrl As Label = CType(Row.FindControl("ReportValue_Label"), Label)
            Dim DbValue As String

            Try 'in case GridView is in edit mode (Label control will NOT be visible)
                DbValue = DbValueCtrl.Text
            Catch ex As Exception
                DbValue = CType(Row.FindControl("ReportValue_TextBox"), TextBox).Text
            End Try

            Select Case FieldType
                Case "Checkbox"
                    Callback(CType(Row.FindControl("ReportValue_CheckBox"), CheckBox))
            End Select
        End If
    End Sub

    Private Sub ReportGridView_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles ReportGridView.RowEditing
        ReportGridView.EditIndex = e.NewEditIndex
        SetGridViewSrc()
    End Sub

    Private Sub ReportGridView_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles ReportGridView.RowCancelingEdit
        Session.Remove("EditModeValues")
        ReportGridView.EditIndex = -1
        SetGridViewSrc()
    End Sub

    Private Sub GenerateActiveSheet(FlexObj As FlexCel.XlsAdapter.XlsFile, SheetName As String)
        Dim NewSheetCount As Integer = FlexObj.SheetCount + 1

        FlexObj.InsertAndCopySheets(1, NewSheetCount, 1)
        FlexObj.ActiveSheet = NewSheetCount
        FlexObj.SheetName = SheetName
    End Sub

    Private Function GetExcelFormatIds(FlexObj As FlexCel.XlsAdapter.XlsFile) As Dictionary(Of String, Integer)
        Dim ExcelFormatToFormatIdHash As New Dictionary(Of String, Integer)

        'A1 Cell format
        Dim HeaderFormat As TFlxFormat = FlexObj.GetDefaultFormat
        HeaderFormat.Font.Size20 = 16 * 20  ' FlexCel uses 1/20 pt units
        HeaderFormat.HAlignment = THFlxAlignment.center 'horizontal alignment
        ExcelFormatToFormatIdHash("A1") = FlexObj.AddFormat(HeaderFormat)

        'default format
        ExcelFormatToFormatIdHash("default") = 0 'default cell format within excel

        'bold format
        Dim BoldFormat As TFlxFormat = FlexObj.GetDefaultFormat
        BoldFormat.Font.Style = TFlxFontStyles.Bold
        ExcelFormatToFormatIdHash("bold") = FlexObj.AddFormat(BoldFormat)

        Return ExcelFormatToFormatIdHash
    End Function

    Protected Sub ExportButton_OnClick2(sender As Object, e As EventArgs) Handles ExportButton.Click
        Dim Flex As New FlexCel.XlsAdapter.XlsFile(True)
        Dim ExportDataHash As Dictionary(Of String, List(Of String())) = _Report.GetExcelData(Session("Report"))
        Dim DS_Final As Data.DataSet = Session("Report").GetDS()
        Dim DataIsOrderedByDate As Boolean = Session("Report").OrderedByDate()
        Dim Path As String = "\\PWI-40\software$\LabelTemplates\Sati_ChecklistReport.v2.xls"

        Flex.Open(Path) 'create spreadsheet
        Flex.ActiveSheetByName = "ChecklistReport"

        'write data onto spreadsheet
        Dim ExcelFormatIdsHash As Dictionary(Of String, Integer) = GetExcelFormatIds(Flex)
        For Each kvp As KeyValuePair(Of String, List(Of String())) In ExportDataHash
            Dim ActiveSheetName As String = kvp.Key
            Dim ActiveSheetMatrix As List(Of String()) = kvp.Value

            GenerateActiveSheet(Flex, ActiveSheetName)

            Dim ExcelFormat As String
            Dim SpreadsheetRowIdx As Integer
            For RowIdx As Integer = 0 To ActiveSheetMatrix.Count - 1  ' Number of rows
                Dim NumOfCols As Integer = 6
                ExcelFormat = ActiveSheetMatrix(RowIdx)(NumOfCols - 1)
                SpreadsheetRowIdx = 1 + RowIdx '(1 based indexing)

                For ColIdx As Integer = 0 To NumOfCols - 2 ' Number of columns (exclude the last column, which is the fontstyle value)
                    Dim CellValue As String = ActiveSheetMatrix(RowIdx)(ColIdx)
                    Dim SpreadsheetColIdx As Integer = 1 + ColIdx '(1 based indexing)
                    Dim ExcelFormatId As Integer = ExcelFormatIdsHash(ExcelFormat)

                    Flex.SetCellFormat(SpreadsheetRowIdx, SpreadsheetColIdx, ExcelFormatId)
                    Flex.SetCellValue(SpreadsheetRowIdx, SpreadsheetColIdx, CellValue)


                    Dim CurrColWidth As Integer = Flex.GetColWidth(SpreadsheetColIdx)
                    Dim CellValueWithPadding As Integer = (CellValue.Length + 5) * 256 '256 is a factor to scale the width in Excel units
                    Flex.SetColWidth(SpreadsheetColIdx, Math.Max(CurrColWidth, CellValueWithPadding))
                Next
            Next
        Next

        'set active sheet first to remove active sheet 1, which is a placeholder to start writing data
        Flex.ActiveSheet = 1
        Flex.DeleteSheet(1)
        SaveToFileExplorer(Flex)
    End Sub

    Private Sub SaveToFileExplorer(FlexObj As FlexCel.XlsAdapter.XlsFile)
        Dim SaveDir As String
        Dim FileName As String

        SaveDir = "\\PWI-40\SATI_Upload_Pics$\$ChecklistReports\"
        FileName = GenerateSpreadsheetName(Session("AreasToInclude"), User.Identity.Name.ToString())
        FlexObj.Save(SaveDir & FileName)

        If SendMailCheckBox.Checked Then
            SatiCode.SendMailWithFile("Checklist Report From " & User.Identity.Name.ToString, "SATI.Net Checklist Report", EmailUserNameTextBox.Text & "@purewafer.com", SaveDir & FileName)
        End If
    End Sub

    Private Function StripIllegalFileSystemChars(Str As String) As String
        Dim IllegalCharMatches As Match = Regex.Match(Str, "[% < > : / \ | ? * ""]")
        Dim Res As String = Str

        Do While IllegalCharMatches.Success
            Dim Key As String = IllegalCharMatches.Value
            Res = Res.Replace(Key, String.Empty)
            IllegalCharMatches = IllegalCharMatches.NextMatch()
        Loop

        Return Res
    End Function

    Private Function CombineStrings(StringsToCombine As List(Of String)) As String
        Dim LongestStringToCombine As Integer
        Dim CombineRes As New StringBuilder()

        'weave elements from StringsToCombine into a single string
        LongestStringToCombine = StringsToCombine.Max(Function(s) s.Length)
        For i = 0 To LongestStringToCombine - 1
            For Each Str As String In StringsToCombine
                If i < Str.Length Then
                    CombineRes.Append(Str(i))
                End If
            Next
        Next
        Return CombineRes.ToString()
    End Function

    Private Function HashString(Str As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim Bytes = Encoding.UTF8.GetBytes(Str)
            Dim Hash = sha256.ComputeHash(Bytes)
            Return BitConverter.ToString(Hash).Replace("-", "").ToLowerInvariant()
        End Using
    End Function

    Private Function GenerateSpreadsheetName(ReportAreaKeys As List(Of Integer), SatiUsername As String) As String
        Dim CombinedString As String = String.Empty
        Dim HashedCombinedString As String
        Dim Route As String
        Dim Res As String

        If ReportAreaKeys Is Nothing Then
            SetPmAndChecklists()
            ReportAreaKeys = Session("AreasToInclude")
        End If

        'combine all strings that are going to be weaved into a List data structure
        For Each ReportAreaKey As String In ReportAreaKeys
            Dim PmOrChecklistName As String = MaintPM.GetPmOrChecklistName(ReportAreaKey)
            CombinedString += PmOrChecklistName
        Next
        CombinedString += SatiUsername
        CombinedString += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") 'include seconds and milliseconds. This in combination with sati username ensures there is never a match

        HashedCombinedString = HashString(CombinedString)

        'If 1 pm/checklist is in the report, initialize Route with Pm/Checklist Name
        'Otherwise, initialize Route with Pm/Checklist Group
        If ReportAreaKeys.Count = 1 Then
            Route = MaintPM.GetPmOrChecklistName(ReportAreaKeys(0))
        Else
            Route = MaintPM.GetGroup(ReportAreaKeys(0))
        End If

        Res = (StripIllegalFileSystemChars(Route) & "--" & HashedCombinedString).Replace(" ", String.Empty) & ".xls"
        Return Res
    End Function

    Private Sub ReportGridView_PreRender(sender As Object, e As EventArgs) Handles ReportGridView.PreRender

    End Sub

    Protected Sub ViewFilters_OnCheckedChanged(sender As Object, e As EventArgs)
        Session("AspWebpage").SetUrl("ViewFilters", Not sender.Checked) 'applying opposite of sender.Checked, b/c of when this line is executed relative to asp.net page lifecycle
        RefreshPreview()
    End Sub

    Protected Sub ResetGrid_OnClick(sender As Object, e As EventArgs) Handles ResetGridButton.Click
        Session("Report") = Nothing

        For Each QsKey As String In QsKeys
            Session("AspWebpage").SetUrl(QsKey, Nothing)
        Next

        RefreshPreview()
    End Sub

    Protected Sub DatepickTextBox_OnTextChanged(sender As Object, e As EventArgs)
        Dim StartDate As String = MMDDYYYYFormat(StartDate_TextBox.Text)
        Dim EndDate As String = MMDDYYYYFormat(EndDate_TextBox.Text)

        Session("Report").SetDateRange(StartDate, EndDate)
        Session("AspWebpage").SetUrl("StartDate", StartDate)
        Session("AspWebpage").SetUrl("EndDate", EndDate)

        RefreshPreview()
    End Sub

    Private Function MMDDYYYYFormat(DateStr As String) As String
        Try 'format expected by HTML5 <input type="date">
            Return Date.Parse(DateStr).ToString("MM/dd/yyyy")
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class

