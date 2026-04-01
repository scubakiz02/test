
Partial Class Production_ViewProductionLogData
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)

        If Not Me.IsPostBack Then
            'Me.BindLogGrid()
            'Me.BindEventGrid()
            Me.LogPanel.Visible = False
            Me.EventPanel.Visible = False
            Me.DTCBPanel.Visible = False
        End If
    End Sub
    Protected Sub DateTextBox_SelectionChanged(sender As Object, e As EventArgs) Handles DateTextBox.SelectionChanged
        If Me.ViewState("LogVisible") Then
            Me.LogPanel.Visible = False
            Me.EventPanel.Visible = False
            Me.DTCheckBox.Checked = False
        Else
            Me.ViewState("LogVisible") = True
            Me.LogPanel.Visible = True
            Me.DTCBPanel.Visible = True
        End If
        Me.BindLogGrid()
        Me.BindEventGrid()
    End Sub
    Protected Sub DateTextBox2_SelectionChanged(sender As Object, e As EventArgs) Handles DateTextBox.SelectionChanged
        If Me.ViewState("LogVisible2") Then
            Me.LogPanel.Visible = False
            Me.EventPanel.Visible = False
            Me.DTCheckBox.Checked = False
        Else
            Me.ViewState("LogVisible2") = True
            Me.LogPanel.Visible = True
            Me.DTCBPanel.Visible = True
        End If
        Me.BindLogGrid()
        Me.BindEventGrid()
    End Sub

    Protected Sub ShiftCheckList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ShiftCheckList.SelectedIndexChanged
        If Me.LogPanel.Visible = False Then
            Me.LogPanel.Visible = True
        End If

        Me.BindLogGrid()
        Me.BindEventGrid()
    End Sub

    Protected Sub DeparmentCheckList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DeparmentCheckList.SelectedIndexChanged
        If Me.LogPanel.Visible = False Then
            Me.LogPanel.Visible = True
        End If

        Me.BindLogGrid()
        Me.BindEventGrid()
    End Sub
    Protected Sub DTCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles DTCheckBox.CheckedChanged
        If DTCheckBox.Checked Then
            Me.EventPanel.Visible = True
        End If
        If Not DTCheckBox.Checked Then
            Me.EventPanel.Visible = False
        End If
    End Sub

    Private Sub BindLogGrid()
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim query As String = "SELECT ReportDate, Shift, Department, LotNumber, QtyCompleat, QtyPass, StartTime, EndTime, Op, (DATEDIFF(ss, StartTime, EndTime) * 1.0) / (60 * 60) AS Hours FROM T_ProLogLots"
        Dim where As String = " WHERE" 'WHERE (SUBSTRING(Shift, 2, 1) IN (@SCL)) AND (ReportDate BETWEEN @StartDate AND @EndDate) AND (Department IN (@Department))
        Dim order As String = " ORDER BY ReportDate, Shift, Department"

        Dim StartYear As String = Me.DateTextBox.SelectedDate.Date.Year.ToString
        Dim StartMonth As String = Me.DateTextBox.SelectedDate.Date.Month.ToString
        Dim StartDay As String = Me.DateTextBox.SelectedDate.Date.Day.ToString
        Dim SD As String = StartYear + "/" + StartMonth + "/" + StartDay

        Dim EndYear As String = Me.DateTextBox2.SelectedDate.Date.Year.ToString
        Dim EndMonth As String = Me.DateTextBox2.SelectedDate.Date.Month.ToString
        Dim EndDay As String = Me.DateTextBox2.SelectedDate.Date.Day.ToString
        Dim ED As String = EndYear + "/" + EndMonth + "/" + EndDay

        Dim ReportDate As String = " AND (ReportDate BETWEEN '" & SD & "' AND '" & ED & "')"

        Dim Shifts As String = String.Empty
        For Each item As ListItem In ShiftCheckList.Items
            Shifts += If(item.Selected, String.Format("'{0}',", item.Value), String.Empty)
        Next
        Dim Departments As String = String.Empty
        For Each item As ListItem In DeparmentCheckList.Items
            Departments += If(item.Selected, String.Format("'{0}',", item.Value), String.Empty)
        Next


        If Not String.IsNullOrEmpty(Shifts) Then
            Shifts = String.Format(" (SUBSTRING(Shift, 2, 1) IN ({0}))", Shifts.Substring(0, Shifts.Length - 1))
        End If
        If Not String.IsNullOrEmpty(Departments) Then
            Departments = String.Format(" AND (Department IN ({0}))", Departments.Substring(0, Departments.Length - 1))
        End If

        Dim FullQuery As String = query & where & Shifts & ReportDate & Departments & order
        Me.SqlDataSourceLogViewer.SelectCommand = FullQuery
        Me.LogViewer.DataBind()
    End Sub

    Private Sub BindEventGrid()
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim query As String = "SELECT ReportDate, Shift, Department, Event, StartTime, EndTime, Op, (DATEDIFF(ss, StartTime, EndTime) * 1.0) / (60 * 60) AS Hours FROM T_ProLogDT"
        Dim where As String = " WHERE" 'WHERE (ReportDate = @ReportDate) And (SUBSTRING(Shift, 2, 1) In (@Shifts)) And (Department In (@Departments))
        Dim order As String = " ORDER BY ReportDate, Shift, Department"

        Dim StartYear As String = Me.DateTextBox.SelectedDate.Date.Year.ToString
        Dim StartMonth As String = Me.DateTextBox.SelectedDate.Date.Month.ToString
        Dim StartDay As String = Me.DateTextBox.SelectedDate.Date.Day.ToString
        Dim SD As String = StartYear + "/" + StartMonth + "/" + StartDay

        Dim EndYear As String = Me.DateTextBox2.SelectedDate.Date.Year.ToString
        Dim EndMonth As String = Me.DateTextBox2.SelectedDate.Date.Month.ToString
        Dim EndDay As String = Me.DateTextBox2.SelectedDate.Date.Day.ToString
        Dim ED As String = EndYear + "/" + EndMonth + "/" + EndDay

        Dim ReportDate As String = " AND (ReportDate BETWEEN '" & SD & "' AND '" & ED & "')"

        Dim Shifts As String = String.Empty
        For Each item As ListItem In ShiftCheckList.Items
            Shifts += If(item.Selected, String.Format("'{0}',", item.Value), String.Empty)
        Next
        Dim Departments As String = String.Empty
        For Each item As ListItem In DeparmentCheckList.Items
            Departments += If(item.Selected, String.Format("'{0}',", item.Value), String.Empty)
        Next


        If Not String.IsNullOrEmpty(Shifts) Then
            Shifts = String.Format(" (SUBSTRING(Shift, 2, 1) IN ({0}))", Shifts.Substring(0, Shifts.Length - 1))
        End If
        If Not String.IsNullOrEmpty(Departments) Then
            Departments = String.Format(" AND (Department IN ({0}))", Departments.Substring(0, Departments.Length - 1))
        End If

        Dim FullQuery As String = query & where & Shifts & ReportDate & Departments & order
        Me.SqlDataSourceDTViewer.SelectCommand = FullQuery
        Me.DTViewer.DataBind()
    End Sub
End Class