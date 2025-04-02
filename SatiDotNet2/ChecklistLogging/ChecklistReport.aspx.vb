Imports System.Text.Json
Imports SatiDotNet2.Library
Imports System.Web.Services

Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim CurrUser As New SatiUser(User.Identity.Name.ToString())
    Dim ChecklistBuilder As New ChecklistBuilderAspxLibrary
    Dim Department As String = CurrUser.GetDepartment()
    Dim DepartmentKey As String = CurrUser.GetDepartmentKey()
    Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
    Dim GroupFromQueryString As String
    Dim AreaFromQueryString As String
    Dim StartDateFromQueryString As String
    Dim EndDateFromQueryString As String
    Dim PageIdxFromQueryString As String

    Public Delegate Sub SetQsDatesDelegate(StartDate As String, EndDate As String)
    <WebMethod()>
    Public Shared Function SetQueryStringDates(UserInput As String, StartDate As String, EndDate As String) As Dictionary(Of String, String)
        Dim DateInRange As String = HttpContext.Current.Session("GroupReport").DateInRange(UserInput)
        Dim QsDates As SetQsDatesDelegate = HttpContext.Current.Session("SetQsDates")
        Dim Res As New Dictionary(Of String, String)

        If String.IsNullOrEmpty(DateInRange) Then
            QsDates(StartDate, EndDate)
            Res("Url") = HttpContext.Current.Session("AspWebpage").GetUrl()
        End If

        Res("DateInRange") = DateInRange

        Return Res
    End Function

    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim SetQsDatesDelegate As SetQsDatesDelegate = AddressOf SetQsDates
        Dim SqlSelectCommand As String = "SELECT A.Area, A.[Key] FROM [ALTS].[dbo].[T_LogArea] A ORDER BY A.Area"

        Session("SetQsDates") = SetQsDatesDelegate

        'to ensure each user of this webpage gets their own class objects
        If Session("GroupReport") Is Nothing Then
            Session("GroupReport") = New GroupReport(New Dictionary(Of String, String) From {
                {"GroupKey", 0},
                {"AreaKey", 0}
            })
        End If

        If Session("AspWebpage") Is Nothing Then
            Session("AspWebpage") = New AspWebpage("/ChecklistLogging/ChecklistReport.aspx", New List(Of String) From {"Group", "Area", "StartDate", "EndDate", "PageIdx"})
        End If
        'to ensure each user of this webpage gets their own class objects

        ' MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        Me.MaintainScrollPositionOnPostBack = True
        GroupFromQueryString = Request.QueryString("Group")
        AreaFromQueryString = Request.QueryString("Area")
        PageIdxFromQueryString = Request.QueryString("PageIdx")
        StartDateFromQueryString = Request.QueryString("StartDate")
        EndDateFromQueryString = Request.QueryString("EndDate")

        If StartDateFromQueryString IsNot Nothing Then
            StartDateCalendar.VisibleDate = StartDateFromQueryString
        End If
        If EndDateFromQueryString IsNot Nothing Then
            EndDateCalendar.VisibleDate = EndDateFromQueryString
        End If

        If GroupFromQueryString IsNot Nothing Then
            SqlSelectCommand = SqlSelectCommand.Replace("ORDER BY", "WHERE A.GroupKey=@GroupKey OR (@GroupKey=0 AND A.GroupKey IS NOT NULL) ORDER BY")
            AreaDropDownList_SqlDataSource.SelectParameters.Clear()
            AreaDropDownList_SqlDataSource.SelectParameters.Add("GroupKey", GroupFromQueryString)
        End If
        AreaDropDownList_SqlDataSource.SelectCommand = SqlSelectCommand
    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        GroupDropDownList.SelectedValue = GroupFromQueryString
        AreaDropDownList.SelectedValue = AreaFromQueryString
        StartDate_TextBox.Text = StartDateFromQueryString
        EndDate_TextBox.Text = EndDateFromQueryString
        ReportGridView.PageIndex = PageIdxFromQueryString

        ReportGridView.DataSource = Session("GroupReport").GetDS().Tables(0)
        ReportGridView.DataBind()

        ClientScript.RegisterStartupScript(Me.GetType(), "GridViewCols", "ColWidths(" & JsonSerializer.Serialize(Of Dictionary(Of String, String))(Session("GroupReport").GetMaxFieldVals()) & ");", True)
    End Sub

    Protected Sub SetGridViewSrc()
        ReportGridView.DataSource = Session("GroupReport").GetDS().Tables(0)
        ReportGridView.DataBind()
    End Sub

    Protected Sub ReportGridView_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles ReportGridView.PageIndexChanging
        'PageIdxFromQueryString = e.NewPageIndex
        Session("AspWebpage").SetUrl("PageIdx", e.NewPageIndex)
        'SetGridViewSrc()
        RefreshPreview()
    End Sub

    Protected Sub AreaDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim SelectedValue As String = AreaDropDownList.SelectedValue

        Session("AspWebpage").SetUrl("Area", SelectedValue)
        Session("GroupReport").SetArea(SelectedValue)
        RefreshPreview()
    End Sub

    Protected Sub GroupDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim SelectedValue As String = GroupDropDownList.SelectedValue

        Session("AspWebpage").SetUrl("Group", SelectedValue)
        Session("GroupReport").SetGroup(SelectedValue)
        RefreshPreview()
    End Sub

    Sub RefreshPreview()
        Response.Redirect(Session("AspWebpage").GetUrl())
    End Sub

    Protected Sub DatepickCalendar_OnDayRender(sender As Object, e As DayRenderEventArgs)
        If e.Day.Date < Date.Parse("03/16/2025") OrElse e.Day.Date > Today.Date Then '03/16/2025 is the date of the first entries in the DB
            e.Cell.Text = e.Day.Date.Day.ToString() 'disable date click event
            e.Cell.ForeColor = System.Drawing.Color.Red
        ElseIf (sender Is StartDateCalendar AndAlso PickDate(StartDateFromQueryString, e.Day.Date)) OrElse (sender Is EndDateCalendar AndAlso PickDate(EndDateFromQueryString, e.Day.Date)) Then
            e.Cell.ForeColor = System.Drawing.Color.Gray
            e.Cell.BackColor = System.Drawing.Color.LightGray
        End If
    End Sub

    Protected Function PickDate(QsDate As String, PickedDate As Date) As Boolean
        If QsDate IsNot Nothing AndAlso PickedDate = QsDate Then
            Return True
        Else
            Return False
        End If
    End Function

    Protected Sub Calendar_OnSelectionChanged(sender As Object, e As EventArgs)
        Dim Calendar_TextBox As TextBox
        Dim SelectedDate As Date = sender.SelectedDate.Date
        Dim StartDate As Date
        Dim EndDate As Date

        If sender Is StartDateCalendar Then
            Calendar_TextBox = StartDate_TextBox
            StartDate = SelectedDate
            EndDate = If(EndDateFromQueryString Is Nothing, Today.Date, Date.Parse(EndDateFromQueryString))
        Else
            Calendar_TextBox = EndDate_TextBox
            StartDate = StartDateFromQueryString
            EndDate = SelectedDate
        End If

        Calendar_TextBox.Text = SelectedDate

        SetQsDates(StartDate, EndDate)

        RefreshPreview()
    End Sub

    Protected Sub SetQsDates(StartDate As String, EndDate As String)
        Session("AspWebpage").SetUrl("StartDate", StartDate)
        Session("AspWebpage").SetUrl("EndDate", EndDate)
        Session("GroupReport").SetDateRange(StartDate, EndDate)
    End Sub
End Class

