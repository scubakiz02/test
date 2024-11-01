Imports SAR_ActionsTableAdapters
Imports SAR_NotesTableAdapters

Partial Class Reports_SAR_CreatSARMain
    Inherits System.Web.UI.Page

    Protected Sub DateDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateDropDownList.SelectedIndexChanged
        RefreshData()
    End Sub

    Protected Sub SiteDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SiteDropDownList.SelectedIndexChanged
        Me.CustomerDropDownList.ClearSelection()
        LoadMainData("Site")
    End Sub

    Protected Sub CustomerDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomerDropDownList.SelectedIndexChanged
        Me.SiteDropDownList.ClearSelection()
        LoadMainData("Customer")
    End Sub

    Protected Sub ClearFilterButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClearFilterButton.Click
        Me.CustomerDropDownList.ClearSelection()
        Me.SiteDropDownList.ClearSelection()
        LoadMainData("All")
    End Sub

    Protected Sub RefreshButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RefreshButton.Click
        RefreshData()
    End Sub

    Protected Sub MonthDataGridView_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles MonthDataGridView.DataBound
        Dim RowCount As Integer
        Dim row As Integer = 0
        Dim Bal As String
        RowCount = Me.MonthDataGridView.Rows.Count
        For row = 0 To RowCount - 1
            Bal = Me.MonthDataGridView.Rows(row).Cells(1).Text.ToString
            If Bal = "0" Then
                Me.MonthDataGridView.Rows(row).Cells(1).BackColor = Drawing.Color.LightGreen
            Else
                Me.MonthDataGridView.Rows(row).Cells(1).BackColor = Drawing.Color.MistyRose
            End If
        Next
    End Sub

    Sub LoadMainData(ByVal Filter As String)
        Dim StartDate As DateTime
        Dim PreDate As DateTime
        Dim SQLText As String = ""
        Try
            StartDate = Me.DateDropDownList.SelectedItem.Value
            PreDate = Me.DateDropDownList.Items(Me.DateDropDownList.SelectedIndex - 1).Text
            Select Case Filter
                Case "All"
                    SQLText = "SELECT fctn_SAR_Ini_Population_1.ID AS MainID, dbo.Customer.CustomerID AS Stite, dbo.Customer.Customer_Name AS Customer, fctn_SAR_Ini_Population_2.WH_Inv + fctn_SAR_Ini_Population_2.WIP + fctn_SAR_Ini_Population_2.Rework + fctn_SAR_Ini_Population_2.FGI + fctn_SAR_Ini_Population_2.CR_Par + fctn_SAR_Ini_Population_2.P_Par AS Start, fctn_SAR_Ini_Population_2.FGI AS S_FGI, fctn_SAR_Ini_Population_1.ReceivedQty AS Rec, fctn_SAR_Ini_Population_1.WH_Inv AS WH, fctn_SAR_Ini_Population_1.Icn_Adj AS WL_Adj, fctn_SAR_Ini_Population_1.WIP, fctn_SAR_Ini_Population_1.FGI, fctn_SAR_Ini_Population_1.Rework, fctn_SAR_Ini_Population_1.Recects AS Rejects, fctn_SAR_Ini_Population_1.CR_Par, fctn_SAR_Ini_Population_1.P_Par, fctn_SAR_Ini_Population_1.Split_Out, fctn_SAR_Ini_Population_1.Split_In, fctn_SAR_Ini_Population_1.ShippedQty AS Shipped, (fctn_SAR_Ini_Population_2.WH_Inv + fctn_SAR_Ini_Population_2.WIP + fctn_SAR_Ini_Population_2.Rework + fctn_SAR_Ini_Population_2.FGI + fctn_SAR_Ini_Population_2.CR_Par + fctn_SAR_Ini_Population_2.P_Par + fctn_SAR_Ini_Population_1.Split_In + fctn_SAR_Ini_Population_1.Icn_Adj + fctn_SAR_Ini_Population_1.ReceivedQty) - (fctn_SAR_Ini_Population_1.WH_Inv + fctn_SAR_Ini_Population_1.WIP + fctn_SAR_Ini_Population_1.Rework + fctn_SAR_Ini_Population_1.FGI + fctn_SAR_Ini_Population_1.CR_Par + fctn_SAR_Ini_Population_1.P_Par + fctn_SAR_Ini_Population_1.ShippedQty + fctn_SAR_Ini_Population_1.Recects + fctn_SAR_Ini_Population_1.Split_Out + fctn_SAR_Ini_Population_1.Scrapped) AS Bal, fctn_SAR_Ini_Population_1.Scrapped FROM dbo.fctn_SAR_Ini_Population('" & StartDate & "') AS fctn_SAR_Ini_Population_1 INNER JOIN dbo.MainID ON fctn_SAR_Ini_Population_1.ID = dbo.MainID.MainID INNER JOIN dbo.fctn_SAR_Ini_Population('" & PreDate & "') AS fctn_SAR_Ini_Population_2 ON dbo.MainID.MainID = fctn_SAR_Ini_Population_2.ID INNER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID"
                Case "Site"
                    Dim Site As String
                    Site = Me.SiteDropDownList.SelectedItem.Text.ToString
                    SQLText = "SELECT fctn_SAR_Ini_Population_1.ID AS MainID, dbo.Customer.CustomerID AS Stite, dbo.Customer.Customer_Name AS Customer, fctn_SAR_Ini_Population_2.WH_Inv + fctn_SAR_Ini_Population_2.WIP + fctn_SAR_Ini_Population_2.Rework + fctn_SAR_Ini_Population_2.FGI + fctn_SAR_Ini_Population_2.CR_Par + fctn_SAR_Ini_Population_2.P_Par AS Start, fctn_SAR_Ini_Population_2.FGI AS S_FGI, fctn_SAR_Ini_Population_1.ReceivedQty AS Rec, fctn_SAR_Ini_Population_1.WH_Inv AS WH, fctn_SAR_Ini_Population_1.Icn_Adj AS WL_Adj, fctn_SAR_Ini_Population_1.WIP, fctn_SAR_Ini_Population_1.FGI, fctn_SAR_Ini_Population_1.Rework, fctn_SAR_Ini_Population_1.Recects AS Rejects, fctn_SAR_Ini_Population_1.CR_Par, fctn_SAR_Ini_Population_1.P_Par, fctn_SAR_Ini_Population_1.Split_Out, fctn_SAR_Ini_Population_1.Split_In, fctn_SAR_Ini_Population_1.ShippedQty AS Shipped, (fctn_SAR_Ini_Population_2.WH_Inv + fctn_SAR_Ini_Population_2.WIP + fctn_SAR_Ini_Population_2.Rework + fctn_SAR_Ini_Population_2.FGI + fctn_SAR_Ini_Population_2.CR_Par + fctn_SAR_Ini_Population_2.P_Par + fctn_SAR_Ini_Population_1.Split_In + fctn_SAR_Ini_Population_1.Icn_Adj + fctn_SAR_Ini_Population_1.ReceivedQty) - (fctn_SAR_Ini_Population_1.WH_Inv + fctn_SAR_Ini_Population_1.WIP + fctn_SAR_Ini_Population_1.Rework + fctn_SAR_Ini_Population_1.FGI + fctn_SAR_Ini_Population_1.CR_Par + fctn_SAR_Ini_Population_1.P_Par + fctn_SAR_Ini_Population_1.ShippedQty + fctn_SAR_Ini_Population_1.Recects + fctn_SAR_Ini_Population_1.Split_Out + fctn_SAR_Ini_Population_1.Scrapped) AS Bal, fctn_SAR_Ini_Population_1.Scrapped FROM dbo.fctn_SAR_Ini_Population('" & StartDate & "') AS fctn_SAR_Ini_Population_1 INNER JOIN dbo.MainID ON fctn_SAR_Ini_Population_1.ID = dbo.MainID.MainID INNER JOIN dbo.fctn_SAR_Ini_Population('" & PreDate & "') AS fctn_SAR_Ini_Population_2 ON dbo.MainID.MainID = fctn_SAR_Ini_Population_2.ID INNER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID WHERE (dbo.Customer.CustomerID = N'" & Site & "')"
                Case "Customer"
                    Dim Customer As String
                    Customer = Me.CustomerDropDownList.SelectedItem.Text.ToString
                    SQLText = "SELECT fctn_SAR_Ini_Population_1.ID AS MainID, dbo.Customer.CustomerID AS Stite, dbo.Customer.Customer_Name AS Customer, fctn_SAR_Ini_Population_2.WH_Inv + fctn_SAR_Ini_Population_2.WIP + fctn_SAR_Ini_Population_2.Rework + fctn_SAR_Ini_Population_2.FGI + fctn_SAR_Ini_Population_2.CR_Par + fctn_SAR_Ini_Population_2.P_Par AS Start, fctn_SAR_Ini_Population_2.FGI AS S_FGI, fctn_SAR_Ini_Population_1.ReceivedQty AS Rec, fctn_SAR_Ini_Population_1.WH_Inv AS WH, fctn_SAR_Ini_Population_1.Icn_Adj AS WL_Adj, fctn_SAR_Ini_Population_1.WIP, fctn_SAR_Ini_Population_1.FGI, fctn_SAR_Ini_Population_1.Rework, fctn_SAR_Ini_Population_1.Recects AS Rejects, fctn_SAR_Ini_Population_1.CR_Par, fctn_SAR_Ini_Population_1.P_Par, fctn_SAR_Ini_Population_1.Split_Out, fctn_SAR_Ini_Population_1.Split_In, fctn_SAR_Ini_Population_1.ShippedQty AS Shipped, (fctn_SAR_Ini_Population_2.WH_Inv + fctn_SAR_Ini_Population_2.WIP + fctn_SAR_Ini_Population_2.Rework + fctn_SAR_Ini_Population_2.FGI + fctn_SAR_Ini_Population_2.CR_Par + fctn_SAR_Ini_Population_2.P_Par + fctn_SAR_Ini_Population_1.Split_In + fctn_SAR_Ini_Population_1.Icn_Adj + fctn_SAR_Ini_Population_1.ReceivedQty) - (fctn_SAR_Ini_Population_1.WH_Inv + fctn_SAR_Ini_Population_1.WIP + fctn_SAR_Ini_Population_1.Rework + fctn_SAR_Ini_Population_1.FGI + fctn_SAR_Ini_Population_1.CR_Par + fctn_SAR_Ini_Population_1.P_Par + fctn_SAR_Ini_Population_1.ShippedQty + fctn_SAR_Ini_Population_1.Recects + fctn_SAR_Ini_Population_1.Split_Out + fctn_SAR_Ini_Population_1.Scrapped) AS Bal, fctn_SAR_Ini_Population_1.Scrapped FROM dbo.fctn_SAR_Ini_Population('" & StartDate & "') AS fctn_SAR_Ini_Population_1 INNER JOIN dbo.MainID ON fctn_SAR_Ini_Population_1.ID = dbo.MainID.MainID INNER JOIN dbo.fctn_SAR_Ini_Population('" & PreDate & "') AS fctn_SAR_Ini_Population_2 ON dbo.MainID.MainID = fctn_SAR_Ini_Population_2.ID INNER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID WHERE (dbo.Customer.Customer_Name = N'" & Customer & "')"
                    '          SELECT fctn_SAR_Ini_Population_1.ID AS MainID, dbo.Customer.CustomerID AS Stite, dbo.Customer.Customer_Name AS Customer, fctn_SAR_Ini_Population_2.WH_Inv + fctn_SAR_Ini_Population_2.WIP + fctn_SAR_Ini_Population_2.Rework + fctn_SAR_Ini_Population_2.FGI + fctn_SAR_Ini_Population_2.CR_Par + fctn_SAR_Ini_Population_2.P_Par AS Start, fctn_SAR_Ini_Population_2.FGI AS S_FGI, fctn_SAR_Ini_Population_1.ReceivedQty AS Rec, fctn_SAR_Ini_Population_1.WH_Inv AS WH, fctn_SAR_Ini_Population_1.Icn_Adj AS WL_Adj, fctn_SAR_Ini_Population_1.WIP, fctn_SAR_Ini_Population_1.FGI, fctn_SAR_Ini_Population_1.Rework, fctn_SAR_Ini_Population_1.Recects AS Rejects, fctn_SAR_Ini_Population_1.CR_Par, fctn_SAR_Ini_Population_1.P_Par, fctn_SAR_Ini_Population_1.Split_Out, fctn_SAR_Ini_Population_1.Split_In, fctn_SAR_Ini_Population_1.ShippedQty AS Shipped, (fctn_SAR_Ini_Population_2.WH_Inv + fctn_SAR_Ini_Population_2.WIP + fctn_SAR_Ini_Population_2.Rework + fctn_SAR_Ini_Population_2.FGI + fctn_SAR_Ini_Population_2.CR_Par + fctn_SAR_Ini_Population_2.P_Par + fctn_SAR_Ini_Population_1.Split_In + fctn_SAR_Ini_Population_1.Icn_Adj + fctn_SAR_Ini_Population_1.ReceivedQty) - (fctn_SAR_Ini_Population_1.WH_Inv + fctn_SAR_Ini_Population_1.WIP + fctn_SAR_Ini_Population_1.Rework + fctn_SAR_Ini_Population_1.FGI + fctn_SAR_Ini_Population_1.CR_Par + fctn_SAR_Ini_Population_1.P_Par + fctn_SAR_Ini_Population_1.ShippedQty + fctn_SAR_Ini_Population_1.Recects + fctn_SAR_Ini_Population_1.Split_Out + fctn_SAR_Ini_Population_1.Scrapped) AS Bal, fctn_SAR_Ini_Population_1.Scrapped FROM dbo.fctn_SAR_Ini_Population('6/1/2006') AS fctn_SAR_Ini_Population_1 INNER JOIN dbo.MainID ON fctn_SAR_Ini_Population_1.ID = dbo.MainID.MainID INNER JOIN dbo.fctn_SAR_Ini_Population('5/1/2006') AS fctn_SAR_Ini_Population_2 ON dbo.MainID.MainID = fctn_SAR_Ini_Population_2.ID INNER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID WHERE (dbo.Customer.Customer_Name = N'blank')
            End Select
            Me.IniSqlDataSource.SelectCommand = SQLText

        Catch ex As Exception
            Me.IniSqlDataSource.SelectCommand = ""
        End Try
        Try
            DetailSQLBuild("")
        Catch ex As Exception

        End Try
    End Sub

    Sub RefreshData()
        Dim path As String = ""
        If Not Me.SiteDropDownList.SelectedItem.Text.ToString = "Select Site..." Then
            path = "Site"
        End If
        If Not Me.CustomerDropDownList.SelectedItem.Text.ToString = "Select Customer..." Then
            path = "Customer"
        End If
        Select Case path
            Case "Site"
                LoadMainData("Site")
            Case "Customer"
                LoadMainData("Customer")
            Case Else
                LoadMainData("All")
        End Select
    End Sub

    Sub Details(ByVal Switch As String, ByVal MainID As String)
        Select Case Switch
            Case "On"
                Me.EditTABLE.Visible = True
                DetailSQLBuild(MainID)
            Case "Off"
                Me.EditTABLE.Visible = False
        End Select
    End Sub

    Sub DetailSQLBuild(ByVal MainID As String)
        MainID = Me.IDLabel.Text
        Dim StartDate As DateTime
        StartDate = Me.DateDropDownList.SelectedItem.Value
        Dim ID_SQL As String
        Dim Rec_SQL As String
        Dim Ship_SQL As String
        Dim IncAdj_SQL As String
        Dim SplitOut_SQL As String
        Dim SplitIn_SQL As String
        Dim Rej_SQL As String
        Dim Scrap_SQL As String
        Dim Notes_SQL As String
        ID_SQL = "SELECT [Key], ReportKey, ID, WH, WIP, RW, FGI, Par_CR, Par_Polish FROM dbo.T_SAR_End_Inv WHERE (ReportKey = CONVERT (DATETIME, '" & StartDate & "', 102)) AND (ID = N'" & MainID & "')"
        Rec_SQL = "SELECT [Key], ReportKey, ID, Adj_Item, Track_ID, Qty, EventTime FROM dbo.T_SAR_Action WHERE (ReportKey = CONVERT (DATETIME, '" & StartDate & "', 102)) AND (ID = N'" & MainID & "') AND (Adj_Item = 'Received')"
        Ship_SQL = "SELECT [Key], ReportKey, ID, Adj_Item, Track_ID, Qty, EventTime FROM dbo.T_SAR_Action WHERE (ReportKey = CONVERT (DATETIME, '" & StartDate & "', 102)) AND (ID = N'" & MainID & "') AND (Adj_Item = 'Shipped')"
        IncAdj_SQL = "SELECT [Key], ReportKey, ID, Adj_Item, Track_ID, Qty, EventTime FROM dbo.T_SAR_Action WHERE (ReportKey = CONVERT (DATETIME, '" & StartDate & "', 102)) AND (ID = N'" & MainID & "6') AND (Adj_Item = 'WL_Adj')"
        SplitOut_SQL = "SELECT [Key], ReportKey, OutID, INID, Qty FROM dbo.T_SAR_ID_Transfer WHERE (OutID = N'" & MainID & "') AND (ReportKey = CONVERT (DATETIME, '" & StartDate & "', 102))"
        SplitIn_SQL = "SELECT [Key], ReportKey, OutID, INID, Qty FROM dbo.T_SAR_ID_Transfer WHERE (ReportKey = CONVERT (DATETIME, '" & StartDate & "', 102)) AND (INID = N'" & MainID & "')"
        Rej_SQL = "SELECT [Key], ReportKey, ID, Defect, Qty FROM dbo.T_SAR_Defects WHERE (ReportKey = CONVERT (DATETIME, '" & StartDate & "', 102)) AND (ID = N'" & MainID & "') ORDER BY Defect"
        Scrap_SQL = "SELECT [Key], ReportKey, ID, Adj_Item, Track_ID, Qty, EventTime FROM dbo.T_SAR_Action WHERE (ReportKey = CONVERT (DATETIME, '" & StartDate & "', 102)) AND (ID = N'" & MainID & "') AND (Adj_Item = 'scrap')"
        Notes_SQL = "SELECT [key], ReportKey, ID, Note FROM dbo.T_SAR_Notes WHERE (ReportKey = CONVERT (DATETIME, '" & StartDate & "', 102)) AND (ID = '" & MainID & "')"
        Me.IDDataSqlDataSource.SelectCommand = ID_SQL
        Me.RecSqlDataSource.SelectCommand = Rec_SQL
        Me.ShippedSqlDataSource.SelectCommand = Ship_SQL
        Me.IncAdjSqlDataSource.SelectCommand = IncAdj_SQL
        Me.Split_OutSqlDataSource.SelectCommand = SplitOut_SQL
        Me.Split_InSqlDataSource.SelectCommand = SplitIn_SQL
        Me.RejSqlDataSource.SelectCommand = Rej_SQL
        Me.ScrapSqlDataSource.SelectCommand = Scrap_SQL
        Me.NotesSqlDataSource.SelectCommand = Notes_SQL

    End Sub

    Protected Sub MonthDataGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles MonthDataGridView.RowCommand
        Dim Row As Integer
        Dim MainID As String
        If e.CommandName = "OpenDetail" Then
            Row = e.CommandArgument.ToString
            MainID = Me.MonthDataGridView.Rows(Row).Cells(2).Text
            Me.IDLabel.Text = MainID
            Details("On", MainID)
        End If
    End Sub

    Protected Sub IDDataGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles IDDataGridView.RowCommand
        Dim test As String = e.CommandName
        If e.CommandName = "Edit" Or e.CommandName = "Cancel" Then
            DetailSQLBuild("")
        End If
    End Sub

    Protected Sub IDDataGridView_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles IDDataGridView.RowUpdated
        DetailSQLBuild("")
        RefreshData()
    End Sub

    Protected Sub IncAdjGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles IncAdjGridView.RowCommand
        If e.CommandName = "Edit" Or e.CommandName = "Cancel" Then
            DetailSQLBuild("")
        End If
    End Sub

    Protected Sub IncAdjGridView_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles IncAdjGridView.RowUpdated
        DetailSQLBuild("")
        RefreshData()
    End Sub

    Protected Sub RecGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles RecGridView.RowCommand
        If e.CommandName = "Edit" Or e.CommandName = "Cancel" Then
            DetailSQLBuild("")
        End If
    End Sub

    Protected Sub RecGridView_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles RecGridView.RowUpdated
        DetailSQLBuild("")
        RefreshData()
    End Sub

    Protected Sub RejGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles RejGridView.RowCommand
        If e.CommandName = "Edit" Or e.CommandName = "Cancel" Then
            DetailSQLBuild("")
        End If
    End Sub

    Protected Sub RejGridView_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles RejGridView.RowUpdated
        DetailSQLBuild("")
        RefreshData()
    End Sub

    Protected Sub ShippedGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles ShippedGridView.RowCommand
        If e.CommandName = "Edit" Or e.CommandName = "Cancel" Then
            DetailSQLBuild("")
        End If
    End Sub

    Protected Sub ShippedGridView_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles ShippedGridView.RowUpdated
        DetailSQLBuild("")
        RefreshData()
    End Sub

    Protected Sub Split_InGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Split_InGridView.RowCommand
        If e.CommandName = "Edit" Or e.CommandName = "Cancel" Then
            DetailSQLBuild("")
        End If
    End Sub

    Protected Sub Split_InGridView_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles Split_InGridView.RowUpdated
        DetailSQLBuild("")
        RefreshData()
    End Sub

    Protected Sub Split_OutGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Split_OutGridView.RowCommand
        If e.CommandName = "Edit" Or e.CommandName = "Cancel" Then
            DetailSQLBuild("")
        End If
    End Sub

    Protected Sub Split_OutGridView_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles Split_OutGridView.RowUpdated
        DetailSQLBuild("")
        RefreshData()
    End Sub

    Protected Sub ScrapGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles ScrapGridView.RowCommand
        If e.CommandName = "Edit" Or e.CommandName = "Cancel" Then
            DetailSQLBuild("")
        End If
    End Sub

    Protected Sub ScrapGridView_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles ScrapGridView.RowUpdated
        DetailSQLBuild("")
        RefreshData()
    End Sub

    Protected Sub NotesGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles NotesGridView.RowCommand
        If e.CommandName = "Edit" Or e.CommandName = "Cancel" Then
            DetailSQLBuild("")
        End If
    End Sub

    Protected Sub NotesGridView_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles NotesGridView.RowDeleted
        DetailSQLBuild("")
        RefreshData()
    End Sub

    Protected Sub NotesGridView_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles NotesGridView.RowUpdated
        DetailSQLBuild("")
        RefreshData()
    End Sub

    Protected Sub ScrapAddButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ScrapAddButton.Click
        If Me.ScrapQtyTextBox.Text = "" Then
            Exit Sub
        End If
        If Me.ScrapTrackIDTextBox.Text = "" Then
            Exit Sub
        End If
        Dim SAR As New T_SAR_ActionTableAdapter
        SAR.Insert_SAR_Action(Me.DateDropDownList.SelectedItem.Value, Me.IDLabel.Text.ToString, "scrap", Me.ScrapTrackIDTextBox.Text.ToString, Me.ScrapQtyTextBox.Text.ToString, System.DateTime.Now.ToShortDateString)
        Me.ScrapQtyTextBox.Text = ""
        Me.ScrapTrackIDTextBox.Text = ""
        RefreshData()
    End Sub

    Protected Sub NotesSaveButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotesSaveButton.Click
        If Me.NotesTextBox.Text = "" Then
            Exit Sub
        End If
        Dim Sar_Note As New T_SAR_NotesTableAdapter
        Sar_Note.Insert_SAR_Notes(Me.DateDropDownList.SelectedItem.Value, Me.IDLabel.Text.ToString, Me.NotesTextBox.Text.ToString)
        Me.NotesTextBox.Text = ""
        RefreshData()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        If Roles.IsUserInRole("SarEdit") = False Then

        End If
    End Sub

End Class
