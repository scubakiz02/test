
Partial Class Reports_KPI
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim LotType As String = "F"
        Dim ReportName As String = ""

        If Me.RadioButtonF.Checked = True Then
            LotType = "1"
        End If
        If Me.RadioButtonR.Checked = True Then
            LotType = "R"
        End If
        If Me.RadioButtonB.Checked = True Then
            LotType = "B"
        End If

        ReportName = SatiCode.KPI_Data(Me.DropDownList1.SelectedItem.Text, LotType, Me.CalendarStart.SelectedDate.ToShortDateString.ToString, Me.CalendarEnd.SelectedDate.ToShortDateString.ToString, Me.CheckBoxDia.Checked, Me.CheckBoxSP2.Checked)

        If Not ReportName = "" Then
            Me.HyperLinkReport.Visible = True
            Me.HyperLinkReport.NavigateUrl = ReportName
        End If

    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        
    End Sub
End Class
