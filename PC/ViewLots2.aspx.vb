Imports System.IO
Imports System.Runtime.InteropServices

Partial Class PC_ViewLots2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub

    Sub UpdateGrid()
        Dim Stage As String
        Stage = Me.DropDownList1.SelectedValue.ToString

        If Not Stage = "All Stages" Then
            'Me.SqlDataSource1.SelectCommand = "SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out, dbo.T_Sati_Lot_Comments.Comment, dbo.T_Sati_Lot_Comments.TheKey FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] LEFT OUTER JOIN dbo.T_Sati_Lot_Comments ON dbo.WaferMover.LotEntry = dbo.T_Sati_Lot_Comments.LotNumber WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.T_Sati_Lot_Comments.Comment, dbo.T_Sati_Lot_Comments.TheKey HAVING (dbo.UniqueProcesses.StageName = N'" & Stage & "') ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"
            Me.SqlDataSource1.SelectCommand = "SELECT TOP 100 PERCENT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out, dbo.T_Sati_Lot_Comments.Comment, dbo.T_Sati_Lot_Comments.TheKey, MAX(dbo.WaferMover.EventTime) AS LastDate FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] LEFT OUTER JOIN dbo.T_Sati_Lot_Comments ON dbo.WaferMover.LotEntry = dbo.T_Sati_Lot_Comments.LotNumber WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (dbo.UniqueProcesses.StageName = N'" & Stage & "') GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.T_Sati_Lot_Comments.Comment, dbo.T_Sati_Lot_Comments.TheKey ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"

        Else
            'Me.SqlDataSource1.SelectCommand = "SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out, dbo.T_Sati_Lot_Comments.Comment, dbo.T_Sati_Lot_Comments.TheKey FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] LEFT OUTER JOIN dbo.T_Sati_Lot_Comments ON dbo.WaferMover.LotEntry = dbo.T_Sati_Lot_Comments.LotNumber WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.T_Sati_Lot_Comments.Comment, dbo.T_Sati_Lot_Comments.TheKey ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"
            Me.SqlDataSource1.SelectCommand = "SELECT TOP 100 PERCENT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out, dbo.T_Sati_Lot_Comments.Comment, dbo.T_Sati_Lot_Comments.TheKey, MAX(dbo.WaferMover.EventTime) AS LastDate FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] LEFT OUTER JOIN dbo.T_Sati_Lot_Comments ON dbo.WaferMover.LotEntry = dbo.T_Sati_Lot_Comments.LotNumber WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.T_Sati_Lot_Comments.Comment, dbo.T_Sati_Lot_Comments.TheKey ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"

        End If

        Me.GridView1.DataBind()

        If Not Me.TextBoxLotFilter.Text = "" Then
            FilterGrid()
        End If

    End Sub



    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        UpdateGrid()
    End Sub


    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "EditComment" Then
            Dim LotNumber As String = Me.GridView1.Rows(e.CommandArgument).Cells(0).Text
            Me.LabelLotNumber.Text = LotNumber
            Me.TextBoxComment.Text = CommentControl(LotNumber, "Find", "")
            Me.Button2_ModalPopupExtender.Show()
        End If
    End Sub

    Function CommentControl(ByVal LotNumber As String, ByVal What As String, ByVal Comment As String) As String
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = Session("DBConnect")
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT TheKey, LotNumber, Comment FROM dbo.T_Sati_Lot_Comments WHERE (LotNumber = N'" & LotNumber & "')"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [dbo].[T_Sati_Lot_Comments] ([LotNumber], [Comment]) VALUES (@LotNumber, @Comment); SELECT TheKey, LotNumber, Comment FROM dbo.T_Sati_Lot_Comments WHERE (TheKey = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@LotNumber", System.Data.SqlDbType.NVarChar, 0, "LotNumber"), New System.Data.SqlClient.SqlParameter("@Comment", System.Data.SqlDbType.NVarChar, 0, "Comment")})
        End With
        DA.InsertCommand = InsertCmd

        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [dbo].[T_Sati_Lot_Comments] SET [LotNumber] = @LotNumber, [Comment] = @Comment WHERE (([TheKey] = @Original_TheKey) AND ((@IsNull_LotNumber = 1 AND [LotNumber] IS NULL) OR ([LotNumber] = @Original_LotNumber)) AND ((@IsNull_Comment = 1 AND [Comment] IS NULL) OR ([Comment] = @Original_Comment))); SELECT TheKey, LotNumber, Comment FROM dbo.T_Sati_Lot_Comments WHERE (TheKey = @TheKey)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@LotNumber", System.Data.SqlDbType.NVarChar, 0, "LotNumber"), New System.Data.SqlClient.SqlParameter("@Comment", System.Data.SqlDbType.NVarChar, 0, "Comment"), New System.Data.SqlClient.SqlParameter("@Original_TheKey", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "TheKey", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_LotNumber", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "LotNumber", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_LotNumber", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "LotNumber", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Comment", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Comment", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Comment", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Comment", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@TheKey", System.Data.SqlDbType.Int, 4, "TheKey")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_Sati_Lot_Comments", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("TheKey", "TheKey"), New System.Data.Common.DataColumnMapping("LotNumber", "LotNumber"), New System.Data.Common.DataColumnMapping("Comment", "Comment")})})
        DA.Fill(DS)

        Select Case What
            Case "Find"
                If Not DS.Tables(0).Rows.Count = 0 Then
                    DR = DS.Tables(0).Rows(0)
                    CommentControl = DR("Comment").ToString
                Else
                    CommentControl = ""
                End If
            Case "Edit"
                If Not DS.Tables(0).Rows.Count = 0 Then
                    DR = DS.Tables(0).Rows(0)
                    DR.AcceptChanges()
                    DR.BeginEdit()
                    DR("Comment") = Comment
                    DR.EndEdit()
                    DA.Update(DS, "T_Sati_Lot_Comments")
                Else
                    DR = DS.Tables("T_Sati_Lot_Comments").NewRow
                    DR("LotNumber") = LotNumber
                    DR("Comment") = Comment

                    DS.Tables("T_Sati_Lot_Comments").Rows.Add(DR)
                    DA.Update(DS, "T_Sati_Lot_Comments")
                End If

        End Select
        Connection.Close()
    End Function

    Protected Sub ButtonSaveComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSaveComment.Click
        CommentControl(Me.LabelLotNumber.Text, "Edit", Me.TextBoxComment.Text)
        UpdateGrid()
    End Sub

    Protected Sub TextBoxLotFilter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxLotFilter.TextChanged
        FilterGrid()
    End Sub

    Sub FilterGrid()
        Dim GridRows As Integer
        GridRows = Me.GridView1.Rows.Count

        For C As Integer = 0 To GridRows - 1
            If Not GridView1.Rows(C).Cells(0).Text.Contains(Me.TextBoxLotFilter.Text) Then
                Me.GridView1.Rows(C).Visible = False
            Else
                Me.GridView1.Rows(C).Visible = True
            End If
        Next
    End Sub

    Protected Sub ExportButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExportButton.Click
        ExportFLEXRemake()
    End Sub

    Sub ExportFLEXRemake()
        Dim Flex As New FlexCel.XlsAdapter.XlsFile(True)
        Dim Path As String = "\\PWI-40\software$\LabelTemplates\View Lots In Process.xls"
        Dim SheetCount As Integer = 0
        Dim SheetRow As Int16 = 2
        Dim SheetCol As Int16 = 1

        Flex.Open(Path)
        Flex.ActiveSheetByName = "LOTS"

        If GridView1.Rows.Count = 0 Then
            ExportLabel.Text = "Export Failed. No data in Grid."
            ExportLabel.ForeColor = Drawing.Color.Red
            Exit Sub
        Else
            For prim As Integer = 0 To Me.GridView1.Rows.Count - 1
                If prim Mod 2 = 0 Then
                    For seco As Integer = 1 To 7
                        Flex.SetCellFormat(SheetRow, seco, Flex.GetCellVisibleFormat(2, 2))
                    Next
                Else
                    For seco As Integer = 1 To 7
                        Flex.SetCellFormat(SheetRow, seco, Flex.GetCellVisibleFormat(3, 2))
                    Next
                End If

                If Me.GridView1.Rows(prim).Visible = True Then
                    For seco As Integer = 0 To 6
                        Flex.SetCellValue(SheetRow, SheetCol + seco, GridView1.Rows(prim).Cells(seco).Text)
                    Next

                    SheetRow = SheetRow + 1
                    SheetCol = 1

                    If prim = GridView1.Rows.Count - 1 Then
                        SaveNewExcelFile(Flex)
                    End If
                End If
            Next
        End If
    End Sub

    Sub SaveNewExcelFile(ByVal FLEX As FlexCel.XlsAdapter.XlsFile)
        Dim PathName As String = "\\PWI-40\TempImageWebFiles$\" '"\\PWI-40\software$\LabelTemplates\View Lots In Process - " & User.Identity.Name.ToString & ".xls"
        Dim FileName As String = "View Lots In Process - " & User.Identity.Name.ToString & ".xls"
        FLEX.Save(PathName & FileName)

        Me.ViewExcelFile.Visible = True
        Me.ViewExcelFile.NavigateUrl = Session("ReportFolder") & FileName

    End Sub
End Class
