
Imports System.Drawing

Partial Class Reports_WipX_Report
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub

    Protected Sub ButtonRun_Click(sender As Object, e As EventArgs) Handles ButtonRun.Click
        GetMyData()
    End Sub

    Sub GetMyData()
        'check to make sure dates are enterd
        If IsDate(Me.TextBoxDateStart.Text) = False Then
            Me.SqlDataSource1.SelectCommand = ""
            Me.SqlDataSource1.DataBind()
            Me.TextBoxDateStart.BackColor = Drawing.Color.LightCoral
        Else
            Me.TextBoxDateStart.BackColor = Drawing.Color.LightGreen
        End If
        If IsDate(Me.TextBoxDateEnd.Text) = False Then
            Me.SqlDataSource1.SelectCommand = ""
            Me.SqlDataSource1.DataBind()
            Me.TextBoxDateEnd.BackColor = Drawing.Color.LightCoral
        Else
            Me.TextBoxDateEnd.BackColor = Drawing.Color.LightGreen
        End If

        'Build SQL
        Me.SqlDataSource1.SelectCommand = BuildSQL()
        Me.SqlDataSource1.DataBind()
    End Sub

    Function BuildSQL() As String
        Dim MySQLString As String = ""
        MySQLString = "SELECT TOP (100) PERCENT dbo.MainID.Diameter, dbo.T_Stage_Report.LotType, dbo.T_Stage_Report.ID, SUM(dbo.T_Stage_Report.[In]) AS Start, SUM(dbo.T_Stage_Report.Out) AS [End], SUM(dbo.T_Stage_Report.Rejects) AS Rejects, SUM(dbo.T_Stage_Report.Rework) AS Rework, COUNT(dbo.T_Stage_Report.LotNumber) AS [lot count], dbo.T_Stage_Report.Stage, dbo.MainID.Exsil_Supplied FROM dbo.T_Stage_Report INNER JOIN dbo.MainID ON dbo.T_Stage_Report.ID = dbo.MainID.MainID "
        MySQLString = MySQLString & "WHERE (dbo.T_Stage_Report.Date > CONVERT(DATETIME, '" & Me.TextBoxDateStart.Text & " 00:00:00', 102)) AND (dbo.T_Stage_Report.Date < CONVERT(DATETIME, '" & Me.TextBoxDateEnd.Text & " 23:59:59', 102)) "
        MySQLString = MySQLString & "GROUP BY dbo.MainID.Diameter, dbo.T_Stage_Report.LotType, dbo.T_Stage_Report.ID, dbo.T_Stage_Report.Stage, dbo.MainID.Exsil_Supplied "

        'Wip Stage
        If RadioButtonWip3.Checked = True Then
            MySQLString = MySQLString & "HAVING (dbo.T_Stage_Report.Stage = N'WIP 3') "
        End If
        If RadioButtonWip25.Checked = True Then
            MySQLString = MySQLString & "HAVING (dbo.T_Stage_Report.Stage = N'WIP 2.5') "
        End If
        If RadioButtonWip2.Checked = True Then
            MySQLString = MySQLString & "HAVING (dbo.T_Stage_Report.Stage = N'WIP 2') "
        End If
        If RadioButtonWip1.Checked = True Then
            MySQLString = MySQLString & "HAVING (dbo.T_Stage_Report.Stage = N'WIP 1') "
        End If

        'Diameter
        If Me.RadioButtonDiameter300.Checked = True Then
            MySQLString = MySQLString & "AND (dbo.MainID.Diameter = 300)"
        End If
        If Me.RadioButtonDiameter200.Checked = True Then
            MySQLString = MySQLString & "AND (dbo.MainID.Diameter = 200)"
        End If
        If Me.RadioButtonDiameterOther.Checked = True Then
            MySQLString = MySQLString & "AND (dbo.MainID.Diameter < 200)"
        End If

        'First pass or not
        If Me.RadioButtonPassFirst.Checked = True Then
            MySQLString = MySQLString & "AND (dbo.T_Stage_Report.LotType = N'F')"
        End If
        If Me.RadioButtonPassNotFirst.Checked = True Then
            MySQLString = MySQLString & "AND (NOT (dbo.T_Stage_Report.LotType = N'F'))"
        End If

        'Supplied or not
        If Me.RadioButtonTypeSupplied.Checked = True Then
            MySQLString = MySQLString & "AND (dbo.MainID.Exsil_Supplied = 1)"
        End If
        If Me.RadioButtonTypeReclaim.Checked = True Then
            MySQLString = MySQLString & "AND (dbo.MainID.Exsil_Supplied = 0)"
        End If

        MySQLString = MySQLString & "ORDER BY dbo.MainID.Diameter, dbo.T_Stage_Report.ID"
        Return MySQLString

    End Function

    Function BuildSQLAdvanced() As String
        Dim MTO As Boolean = False 'More Then One
        Dim DP As String = ""
        Dim t As Integer = 0

        Dim MySQLString As String = ""
        MySQLString = "SELECT TOP (100) PERCENT dbo.MainID.Diameter, dbo.T_Stage_Report.LotType, dbo.T_Stage_Report.ID, SUM(dbo.T_Stage_Report.[In]) AS Start, SUM(dbo.T_Stage_Report.Out) AS [End], SUM(dbo.T_Stage_Report.Rejects) AS Rejects, SUM(dbo.T_Stage_Report.Rework) AS Rework, COUNT(dbo.T_Stage_Report.LotNumber) AS [lot count], dbo.T_Stage_Report.Stage, dbo.MainID.Exsil_Supplied FROM dbo.T_Stage_Report INNER JOIN dbo.MainID ON dbo.T_Stage_Report.ID = dbo.MainID.MainID "


        'Build the WHERE

        MySQLString = MySQLString & "WHERE (DATEPART(yyyy, dbo.T_Stage_Report.Date) = " & Me.DropDownListYear.SelectedItem.Text & ") AND ("


        'if Month is selected
        If RadioButtonMM.Checked = True Then
            DP = "mm"
        End If

        'if Quarter is selected
        If RadioButtonQQ.Checked = True Then
            DP = "qq"
        End If

        'if workweek is selected
        If RadioButtonWW.Checked = True Then
            DP = "ww"
        End If

        'so how many months were selected
        For t = 0 To Me.CheckBoxListRangeSelected.Items.Count - 1
            If Me.CheckBoxListRangeSelected.Items(t).Selected = True Then
                If MTO = False Then
                    MySQLString = MySQLString & "DATEPART(" & DP & ", dbo.T_Stage_Report.Date) = " & Me.CheckBoxListRangeSelected.Items(t).Text
                    MTO = True
                Else
                    MySQLString = MySQLString & " Or DATEPART(" & DP & ", dbo.T_Stage_Report.Date) = " & Me.CheckBoxListRangeSelected.Items(t).Text
                End If
            End If
        Next
        MySQLString = MySQLString & ") "


        MySQLString = MySQLString & "GROUP BY dbo.MainID.Diameter, dbo.T_Stage_Report.LotType, dbo.T_Stage_Report.ID, dbo.T_Stage_Report.Stage, dbo.MainID.Exsil_Supplied "

        'Wip Stage
        If RadioButtonWippy3.Checked = True Then
            MySQLString = MySQLString & "HAVING (dbo.T_Stage_Report.Stage = N'WIP 3') "
        End If
        If RadioButtonWippy25.Checked = True Then
            MySQLString = MySQLString & "HAVING (dbo.T_Stage_Report.Stage = N'WIP 2.5') "
        End If
        If RadioButtonWippy2.Checked = True Then
            MySQLString = MySQLString & "HAVING (dbo.T_Stage_Report.Stage = N'WIP 2') "
        End If
        If RadioButtonWippy1.Checked = True Then
            MySQLString = MySQLString & "HAVING (dbo.T_Stage_Report.Stage = N'WIP 1') "
        End If

        'First pass or not
        If Me.RadioButtonPassyFirst.Checked = True Then
            MySQLString = MySQLString & "AND (dbo.T_Stage_Report.LotType = N'F')"
        End If
        If Me.RadioButtonPassyNotFirst.Checked = True Then
            MySQLString = MySQLString & "AND (NOT (dbo.T_Stage_Report.LotType = N'F'))"
        End If

        MTO = False
        'Select the IDs
        For t = 0 To Me.CheckBoxListFabIDs.Items.Count - 1
            If Me.CheckBoxListFabIDs.Items(t).Selected = True Then
                If MTO = False Then
                    MySQLString = MySQLString & " AND (dbo.T_Stage_Report.ID = N'" & Me.CheckBoxListFabIDs.Items(t).Text & "' "
                    MTO = True
                Else
                    MySQLString = MySQLString & " Or  dbo.T_Stage_Report.ID = N'" & Me.CheckBoxListFabIDs.Items(t).Text & "' "
                End If
            End If
        Next
        MySQLString = MySQLString & ") "



        MySQLString = MySQLString & "ORDER BY dbo.MainID.Diameter, dbo.T_Stage_Report.ID"
        Return MySQLString

    End Function


    Sub UpdateView()
        Dim Flex As New FlexCel.XlsAdapter.XlsFile(True)

        Dim Path As String
        Dim StartRow As Int16 = 4
        Dim ReportName As String = "\\PWI-40\TempImageWebFiles$\InvWIP3Report " & User.Identity.Name.ToString & ".xls"
        Dim StartCount As Integer
        Dim EndCount As Integer
        Dim Y As Double
        Dim SS As Integer = 0
        Dim SE As Integer = 0
        Dim SRJ As Integer = 0
        Dim SRW As Integer = 0
        Dim SL As Integer = 0
        Dim days As Long
        Dim row As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)
        Dim cell As New TableHeaderCell()
        Dim ReportText As String
        Dim AvgDay As Double


        Me.HyperLinkReport.Visible = False

        Try
            Path = "\\PWI-40\software$\LabelTemplates\Sati_WIP3_Report.xls"
            Flex.Open(Path)
            Flex.ActiveSheetByName = "WIP3"

            For i As Integer = 0 To GridView1.Rows.Count - 1
                StartCount = Me.GridView1.Rows(i).Cells(3).Text
                EndCount = Me.GridView1.Rows(i).Cells(4).Text
                Y = EndCount / StartCount
                Me.GridView1.Rows(i).Cells(10).Text = FormatPercent(Y)

                SS = SS + Me.GridView1.Rows(i).Cells(3).Text
                SE = SE + Me.GridView1.Rows(i).Cells(4).Text
                SRJ = SRJ + Me.GridView1.Rows(i).Cells(5).Text
                SRW = SRW + Me.GridView1.Rows(i).Cells(6).Text
                SL = SL + Me.GridView1.Rows(i).Cells(7).Text
                'SL = SL + CType(Me.GridView1.Rows(i).Cells(7).FindControl("label1"), Label).Text

                Flex.SetCellValue(i + StartRow, 1, Int(GridView1.Rows(i).Cells(0).Text)) 'Dia
                Flex.SetCellValue(i + StartRow, 2, GridView1.Rows(i).Cells(1).Text) 'Type
                Flex.SetCellValue(i + StartRow, 3, Int(GridView1.Rows(i).Cells(2).Text)) 'ID
                Flex.SetCellValue(i + StartRow, 4, Int(GridView1.Rows(i).Cells(3).Text)) 'Start 
                Flex.SetCellValue(i + StartRow, 5, Int(GridView1.Rows(i).Cells(4).Text)) 'End
                Flex.SetCellValue(i + StartRow, 6, Int(GridView1.Rows(i).Cells(5).Text)) 'Reject
                Flex.SetCellValue(i + StartRow, 7, Int(GridView1.Rows(i).Cells(6).Text)) 'Rework
                Flex.SetCellValue(i + StartRow, 8, Int(GridView1.Rows(i).Cells(7).Text)) 'Lot Count
                'Flex.SetCellValue(i + StartRow, 8, Int(CType(Me.GridView1.Rows(i).Cells(7).FindControl("label1"), Label).Text)) 'Lot Count
                Flex.SetCellValue(i + StartRow, 9, GridView1.Rows(i).Cells(8).Text) 'Stage
                Flex.SetCellValue(i + StartRow, 10, GridView1.Rows(i).Cells(9).Text) 'Supplied
                Flex.SetCellValue(i + StartRow, 11, CType(Y, Double)) 'Yield
            Next


            Me.GridView1.ShowFooter = True
            Me.GridView1.FooterRow.Cells(3).Text = Format(SS, "n0")
            Me.GridView1.FooterRow.Cells(4).Text = Format(SE, "n0")
            Me.GridView1.FooterRow.Cells(5).Text = Format(SRJ, "n0")
            Me.GridView1.FooterRow.Cells(6).Text = Format(SRW, "n0")
            Me.GridView1.FooterRow.Cells(7).Text = Format(SL, "n0")
            Me.GridView1.FooterRow.Cells(10).Text = FormatPercent(SE / SS)


            Try
                days = DateDiff(DateInterval.Day, Date.Parse(Me.TextBoxDateStart.Text), Date.Parse(Me.TextBoxDateEnd.Text)) + 1
            Catch ex As Exception
                days = 0
            End Try

            cell.Text = days & " Day Summary"
            cell.ColumnSpan = 3
            row.Controls.Add(cell)
            Flex.SetCellValue(2, 1, days & " Day Summary")


            cell = New TableHeaderCell()
            cell.Text = Format(SS, "n0")
            row.Controls.Add(cell)
            Flex.SetCellValue(2, 4, SS)

            cell = New TableHeaderCell()
            cell.Text = Format(SE, "n0")
            row.Controls.Add(cell)
            Flex.SetCellValue(2, 5, SE)

            cell = New TableHeaderCell()
            cell.Text = Format(SRJ, "n0")
            row.Controls.Add(cell)
            Flex.SetCellValue(2, 6, SRJ)

            cell = New TableHeaderCell()
            cell.Text = Format(SRW, "n0")
            row.Controls.Add(cell)
            Flex.SetCellValue(2, 7, SRW)

            cell = New TableHeaderCell()
            cell.Text = Format(SL, "n0")
            row.Controls.Add(cell)
            Flex.SetCellValue(2, 8, SL)

            AvgDay = Format(SE / days, "n2")
            cell = New TableHeaderCell()
            cell.Text = Format(SE / days, "n2") & " Avg out/day"
            cell.ColumnSpan = 2
            row.Controls.Add(cell)
            Flex.SetCellValue(2, 9, AvgDay & " Avg out/day")

            cell = New TableHeaderCell()
            cell.Text = FormatPercent(SE / SS)
            row.Controls.Add(cell)
            Flex.SetCellValue(2, 11, SE / SS)

            row.BackColor = ColorTranslator.FromHtml("#3AC0F2")
            GridView1.HeaderRow.Parent.Controls.AddAt(0, row)


            ReportText = "Wip3 Report. From " & TextBoxDateStart.Text & " To " & TextBoxDateEnd.Text
            Flex.SetCellValue(1, 1, ReportText)



            Flex.Save(ReportName) 'LabelTemplates\LabelArchive\InvReportHold

            Me.HyperLinkReport.Visible = True
            Me.HyperLinkReport.NavigateUrl = ReportName

        Catch ex As Exception

        End Try

    End Sub

    Private Sub GridView1_DataBound(sender As Object, e As EventArgs) Handles GridView1.DataBound
        UpdateView()

    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim row As String
        Dim MySQL As String

        row = e.CommandArgument.ToString
        row = row

        If e.CommandName = "Lots" Then
            Me.PanelLotView_ModalPopupExtender.Show()

            Dim ds As New Data.DataSet
            Dim DR As Data.DataRow
            ds = SatiCode.GetMyDataSet(BuildSQL)

            DR = ds.Tables(0).Rows(row)

            MySQL = "SELECT TOP (100) PERCENT LotType AS Type, LotNumber, [In] AS Start, Out AS [End], Rejects, Rework, Date FROM T_Stage_Report "
            MySQL = MySQL & "WHERE (Date < CONVERT(DATETIME, '" & Me.TextBoxDateEnd.Text & " 23:59:59', 102)) AND (Date > CONVERT(DATETIME, '" & Me.TextBoxDateStart.Text & " 00:00:00', 102)) "

            'Wip Stage
            If RadioButtonWip3.Checked = True Then
                MySQL = MySQL & "AND (Stage = N'WIP 3') "
            End If
            If RadioButtonWip25.Checked = True Then
                MySQL = MySQL & "AND (Stage = N'WIP 2.5') "
            End If
            If RadioButtonWip2.Checked = True Then
                MySQL = MySQL & "AND (Stage = N'WIP 2') "
            End If
            If RadioButtonWip1.Checked = True Then
                MySQL = MySQL & "AND (Stage = N'WIP 1') "
            End If


            'MySQL = MySQL & "AND (Stage = N'WIP 3') "

            MySQL = MySQL & "AND (ID = N'" & DR("ID") & "') "
            If DR("LotType") = "F" Then
                MySQL = MySQL & "AND (LotType = N'f') "
            Else
                MySQL = MySQL & "AND (NOT (LotType = N'F')) "
            End If
            MySQL = MySQL & "ORDER BY Date"

            Me.SqlDataSource2.SelectCommand = MySQL
            Me.GridView2.DataBind()

        End If

        GetMyData()
    End Sub

    Private Sub GridView2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView2.RowCommand
        Dim row As String

        row = e.CommandArgument.ToString
        row = row

        If e.CommandName = "LotNumber" Then
            OpenNewPage(Me.UpdatePanel1, "http://pwi-40:82/LotDetailReport.aspx?LotNumber=" & Me.GridView2.Rows(row).Cells(1).Text)
        End If

        GetMyData()
        Me.PanelLotView_ModalPopupExtender.Show()
    End Sub

    Sub OpenNewPage(ByVal MyUpdatePanel As UpdatePanel, ByVal TheWebPage As String)
        Dim docGuid As String = Guid.NewGuid().ToString()
        Dim sb As StringBuilder = New StringBuilder("")
        Dim strRoot As String
        strRoot = Request.Url.GetLeftPart(UriPartial.Authority)
        sb.Append("window.open('" & TheWebPage & "');")
        ScriptManager.RegisterClientScriptBlock(MyUpdatePanel, MyUpdatePanel.GetType(), "NewClientScript", sb.ToString(), True)
    End Sub



    Protected Sub RadioButtonQQ_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonQQ.CheckedChanged
        If RadioButtonQQ.Checked = True Then
            DataRangeView()
        End If
    End Sub
    Protected Sub RadioButtonMM_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonMM.CheckedChanged
        If RadioButtonMM.Checked = True Then
            DataRangeView()
        End If
    End Sub
    Protected Sub RadioButtonWW_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonWW.CheckedChanged
        If RadioButtonWW.Checked = True Then
            DataRangeView()
        End If
    End Sub

    Sub DataRangeView()
        If RadioButtonQQ.Checked = True Or RadioButtonMM.Checked = True Or RadioButtonWW.Checked = True Then


            Dim DS As New Data.DataSet
            Dim DR As Data.DataRow
            Dim TheYear As String = Me.DropDownListYear.SelectedItem.Text

            'clear by default add code

            If RadioButtonQQ.Checked Then 'Select TOP(100) PERCENT DATEPART(QQ, Date) As QQ FROM dbo.T_Stage_Report GROUP BY DATEPART(yyyy, Date), DATEPART(QQ, Date) HAVING (DATEPART(yyyy, Date) = 2019) ORDER BY DATEPART(QQ, Date)
                DS = SatiCode.GetMyDataSet("Select TOP(100) PERCENT DATEPART(QQ, Date) As n FROM dbo.T_Stage_Report GROUP BY DATEPART(yyyy, Date), DATEPART(QQ, Date) HAVING (DATEPART(yyyy, Date) = " & TheYear & ") ORDER BY DATEPART(QQ, Date)")


            End If

            If RadioButtonMM.Checked Then 'SELECT TOP (100) PERCENT DATEPART(mm, Date) AS MM FROM  dbo.T_Stage_Report GROUP BY DATEPART(yyyy, Date), DATEPART(mm, Date) HAVING (DATEPART(yyyy, Date) = 2019) ORDER BY DATEPART(mm, Date)
                DS = SatiCode.GetMyDataSet("SELECT TOP (100) PERCENT DATEPART(mm, Date) AS n FROM  dbo.T_Stage_Report GROUP BY DATEPART(yyyy, Date), DATEPART(mm, Date) HAVING (DATEPART(yyyy, Date) = " & TheYear & ") ORDER BY DATEPART(mm, Date)")


            End If

            If RadioButtonWW.Checked Then 'Select Case TOP(100) PERCENT DATEPART(ww, Date) As WW FROM dbo.T_Stage_Report GROUP BY DATEPART(yyyy, Date), DATEPART(ww, Date) HAVING (DATEPART(yyyy, Date) = 2019) ORDER BY WW
                DS = SatiCode.GetMyDataSet("Select TOP(100) PERCENT DATEPART(ww, Date) As n FROM dbo.T_Stage_Report GROUP BY DATEPART(yyyy, Date), DATEPART(ww, Date) HAVING (DATEPART(yyyy, Date) = " & TheYear & ") ORDER BY DATEPART(ww, Date)")


            End If

            Me.CheckBoxListRangeSelected.Items.Clear()

            If DS.Tables(0).Rows.Count > 0 Then
                For I As Integer = 0 To DS.Tables(0).Rows.Count - 1
                    DR = DS.Tables(0).Rows(I)
                    Me.CheckBoxListRangeSelected.Items.Add(DR("n"))

                Next
            End If
        End If
        Me.PanelAdvancedView_ModalPopupExtender.Show()
    End Sub

    Protected Sub DropDownListYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListYear.SelectedIndexChanged
        DataRangeView()
        FabFind()
        Me.PanelAdvancedView_ModalPopupExtender.Show()
    End Sub

    Sub FabFind()
        'SELECT TOP (100) PERCENT dbo.MainID.CustomerID FROM dbo.T_Stage_Report INNER JOIN dbo.MainID ON dbo.T_Stage_Report.ID = dbo.MainID.MainID GROUP BY DATEPART(yyyy, dbo.T_Stage_Report.Date), dbo.MainID.CustomerID HAVING (DATEPART(yyyy, dbo.T_Stage_Report.Date) = 2019) ORDER BY dbo.MainID.CustomerID
        Dim DS As Data.DataSet
        Dim DR As Data.DataRow
        DS = SatiCode.GetMyDataSet("SELECT TOP (100) PERCENT dbo.MainID.CustomerID FROM dbo.T_Stage_Report INNER JOIN dbo.MainID ON dbo.T_Stage_Report.ID = dbo.MainID.MainID GROUP BY DATEPART(yyyy, dbo.T_Stage_Report.Date), dbo.MainID.CustomerID HAVING (DATEPART(yyyy, dbo.T_Stage_Report.Date) = " & Me.DropDownListYear.SelectedItem.Text & ") ORDER BY dbo.MainID.CustomerID")
        Me.DropDownListFabs.Items.Clear()
        If DS.Tables(0).Rows.Count > 0 Then
            Me.DropDownListFabs.Items.Add("Select ID...")
            For I As Integer = 0 To DS.Tables(0).Rows.Count - 1
                DR = DS.Tables(0).Rows(I)
                Me.DropDownListFabs.Items.Add(DR("CustomerID"))
            Next
        End If
        Me.PanelAdvancedView_ModalPopupExtender.Show()
    End Sub

    Protected Sub DropDownListFabs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListFabs.SelectedIndexChanged
        Me.CheckBoxListFabIDs.Items.Clear()

        If Not DropDownListFabs.SelectedItem.Text = "Select ID..." Then
            Dim DS As New Data.DataSet
            Dim DR As Data.DataRow
            'SELECT TOP (100) PERCENT dbo.MainID.MainID FROM dbo.T_Stage_Report INNER JOIN dbo.MainID ON dbo.T_Stage_Report.ID = dbo.MainID.MainID WHERE (dbo.MainID.CustomerID = N'Intel-AZ') GROUP BY DATEPART(yyyy, dbo.T_Stage_Report.Date), dbo.MainID.MainID HAVING (DATEPART(yyyy, dbo.T_Stage_Report.Date) = 2019) ORDER BY dbo.MainID.MainID
            DS = SatiCode.GetMyDataSet("SELECT TOP (100) PERCENT dbo.MainID.MainID FROM dbo.T_Stage_Report INNER JOIN dbo.MainID ON dbo.T_Stage_Report.ID = dbo.MainID.MainID WHERE (dbo.MainID.CustomerID = N'" & Me.DropDownListFabs.SelectedItem.Text & "') GROUP BY DATEPART(yyyy, dbo.T_Stage_Report.Date), dbo.MainID.MainID HAVING (DATEPART(yyyy, dbo.T_Stage_Report.Date) = " & Me.DropDownListYear.SelectedItem.Text & ") ORDER BY dbo.MainID.MainID")
            If DS.Tables(0).Rows.Count >= 0 Then
                For I As Integer = 0 To DS.Tables(0).Rows.Count - 1
                    DR = DS.Tables(0).Rows(I)
                    Me.CheckBoxListFabIDs.Items.Add(DR("MainID"))
                Next
            End If
        End If
        Me.PanelAdvancedView_ModalPopupExtender.Show()
    End Sub
    Protected Sub ButtonAdvanced_Click(sender As Object, e As EventArgs) Handles ButtonAdvanced.Click
        'Build SQL
        Me.SqlDataSource1.SelectCommand = BuildSQLAdvanced()
        Me.SqlDataSource1.DataBind()
    End Sub
    Protected Sub ButtonShowTheAdanced_Click(sender As Object, e As EventArgs) Handles ButtonShowTheAdanced.Click
        Me.PanelAdvancedView_ModalPopupExtender.Show()
    End Sub
End Class
