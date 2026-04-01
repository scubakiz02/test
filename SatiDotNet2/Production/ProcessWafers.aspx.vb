
Partial Class Production_ProcessWafers
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub
    Sub G1update()
        Session("LoT") = Me.LotNumberTextBox.Text
        Dim NewSql As String
        NewSql = "SELECT dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out, dbo.UniqueProcesses.Complete, ISNULL(dbo.UniqueProcesses.Notes, N'') AS Notes FROM dbo.UniqueProcesses LEFT OUTER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] GROUP BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.Complete, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder, dbo.UniqueProcesses.Notes HAVING (dbo.UniqueProcesses.LotEntry = N'" & Me.LotNumberTextBox.Text & "') ORDER BY dbo.UniqueProcesses.ProcessOrder"
        SqlDataSource1.SelectCommand = NewSql
        Me.SqlDataSource1.DataBind()
        Me.GridView1.DataBind()

        If Me.RefreshButton.Visible = False Then
            Me.RefreshButton.Visible = True
        End If
    End Sub

    Protected Sub LotNumberTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LotNumberTextBox.TextChanged
        'Me.LotNumberTextBox.Enabled = False
        G1update()
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Session("SpecialPartial") = "No"
        Dim DS As New Data.DataSet
        Dim row As String
        Dim Stage As String
        row = e.CommandArgument.ToString
        Session("LotNumber") = Me.GridView1.Rows(row).Cells(0).Text
        Session("Stage") = Me.GridView1.Rows(row).Cells(2).Text
        Session("Step") = Me.GridView1.Rows(row).Cells(1).Text
        Stage = Session("Stage").ToString


        If e.CommandName = "Select" Then

            If Me.GridView1.Rows(row).Cells(5).Text = "Not Complete" Then
                If row = 0 Then
                    Session("View") = "No"
                    If Stage = "WIP 1" Or Stage = "WIP 2" Or Stage = "WIP 3" Then
                        DS = SatiCode.GetMyDataSet("SELECT Stage, LotNumber FROM dbo.T_Stage_Report WHERE (Stage = N'" & Stage & "') AND (LotNumber = N'" & Session("LotNumber").ToString & "')")
                        If DS.Tables(0).Rows.Count = 0 Then
                            Response.Redirect(Session("CheckPoint").ToString)
                        End If
                    End If
                Else

                    If Me.GridView1.Rows(row - 1).Cells(5).Text = "Not Complete" Then
                        Session("View") = "Yes"
                        If Me.GridView1.Rows(row).Cells(5).Text = "Not Complete" And Me.GridView1.Rows(row).Cells(3).Text = "N/A" Then
                            Dim GrowCount As Int16
                            Dim IG As Int16
                            GrowCount = Me.GridView1.Rows.Count
                            For IG = 0 To GrowCount - 1
                                If Me.GridView1.Rows(IG).Cells(2).Text = "CMP" Then
                                    If Not Me.GridView1.Rows(IG).Cells(3).Text = "N/A" Then
                                        Session("SpecialPartial") = "Yes" 'SpecialPartial
                                    End If
                                End If
                            Next

                            Session("View") = "NoQty"
                        End If
                    Else
                        If Not Me.GridView1.Rows(row).Cells(3).Text = "N/A" Then
                            Session("View") = "No"
                        Else
                            Session("View") = "Yes"
                            Session("View") = "NoQty"
                        End If

                        If Stage.Contains("WIP") Then 'Stage = "WIP 1" Or Stage = "WIP 2" Or Stage = "WIP 3" Then
                            DS = SatiCode.GetMyDataSet("SELECT Stage, LotNumber FROM dbo.T_Stage_Report WHERE (Stage = N'" & Stage & "') AND (LotNumber = N'" & Session("LotNumber").ToString & "')")
                            If DS.Tables(0).Rows.Count = 0 Then
                                Response.Redirect(Session("CheckPoint").ToString)
                            End If
                        End If

                        If Stage.Contains("Final Pack") Then ' check for bulk QA
                            If SatiCode.GetDiameter(Mid(Session("LotNumber").ToString, 1, 4)) = "200" Then

                                DS = SatiCode.GetMyDataSet("SELECT LotNumber, TimeStamp, [User], Note FROM dbo.T_Bulk_Final_QA_Lots WHERE (LotNumber = N'" & Session("LotNumber").ToString & "')")
                                If DS.Tables(0).Rows.Count = 0 Then
                                    Dim My_QA As String
                                    My_QA = SatiCode.Bulk_Final_QA(Session("LotNumber").ToString)
                                    If Not My_QA.Contains("Did not find any data out of Spec.") Then
                                        'pop up a window with the notes test. problem has to be fix to move on. 
                                        My_QA = My_QA.Replace("<br/>", Chr(13))
                                        Me.TextBoxInfo.Text = My_QA
                                        Me.ModalPopupExtender1.Show()
                                        Exit Sub
                                    End If

                                End If
                            End If
                        End If


                        End If
                End If
            Else
                Session("View") = "Yes"

            End If
            'Response.Redirect(Session("RunSheet").ToString)
            If CheckBoxNewWork.Checked = True Then
                Response.Redirect("~/Production/StageWork.aspx?LotNumber=" & Me.GridView1.Rows(row).Cells(0).Text & "&Stage=" & Me.GridView1.Rows(row).Cells(2).Text & "&Step=" & Me.GridView1.Rows(row).Cells(1).Text & "&View=" & Session("View").ToString)

            Else
                Response.Redirect(Session("RunSheet").ToString)
            End If

        End If

        If e.CommandName = "EditNote" Then
            
            If User.IsInRole("PC") = True Then
                Dim LotNumber As String = Me.GridView1.Rows(e.CommandArgument).Cells(0).Text
                Me.LabelLotNumber.Text = LotNumber
                Me.LabelStage.Text = Session("Stage").ToString
                Me.TextBoxComment.Text = CommentControl(LotNumber, "Find", "", Session("Stage").ToString)
                Me.Button200_ModalPopupExtender.Show()
            End If


        End If

        G1update()
    End Sub

    Protected Sub RefreshButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RefreshButton.Click
        G1update()
    End Sub

    Protected Sub ClearButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClearButton.Click
        Me.LotNumberTextBox.Enabled = True
        Me.LotNumberTextBox.Text = ""
        SqlDataSource1.SelectCommand = ""
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Function CommentControl(ByVal LotNumber As String, ByVal What As String, ByVal Comment As String, ByVal Stage As String) As String
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT LotEntry, ProcessOrder, Notes, StageName FROM dbo.UniqueProcesses WHERE (LotEntry = N'" & LotNumber & "') AND (StageName = N'" & Stage & "')"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [dbo].[UniqueProcesses] ([LotEntry], [ProcessOrder], [Notes], [StageName]) VALUES (@LotEntry, @ProcessOrder, @Notes, @StageName); SELECT LotEntry, ProcessOrder, Notes, StageName FROM dbo.UniqueProcesses WHERE (LotEntry = @LotEntry) AND (ProcessOrder = @ProcessOrder)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@LotEntry", System.Data.SqlDbType.NVarChar, 0, "LotEntry"), New System.Data.SqlClient.SqlParameter("@ProcessOrder", System.Data.SqlDbType.Int, 0, "ProcessOrder"), New System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.NVarChar, 0, "Notes"), New System.Data.SqlClient.SqlParameter("@StageName", System.Data.SqlDbType.NVarChar, 0, "StageName")})
        End With
        DA.InsertCommand = InsertCmd

        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [dbo].[UniqueProcesses] SET [LotEntry] = @LotEntry, [ProcessOrder] = @ProcessOrder, [Notes] = @Notes, [StageName] = @StageName WHERE (([LotEntry] = @Original_LotEntry) AND ([ProcessOrder] = @Original_ProcessOrder) AND ((@IsNull_Notes = 1 AND [Notes] IS NULL) OR ([Notes] = @Original_Notes)) AND ([StageName] = @Original_StageName)); SELECT LotEntry, ProcessOrder, Notes, StageName FROM dbo.UniqueProcesses WHERE (LotEntry = @LotEntry) AND (ProcessOrder = @ProcessOrder)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@LotEntry", System.Data.SqlDbType.NVarChar, 0, "LotEntry"), New System.Data.SqlClient.SqlParameter("@ProcessOrder", System.Data.SqlDbType.Int, 0, "ProcessOrder"), New System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.NVarChar, 0, "Notes"), New System.Data.SqlClient.SqlParameter("@StageName", System.Data.SqlDbType.NVarChar, 0, "StageName"), New System.Data.SqlClient.SqlParameter("@Original_LotEntry", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "LotEntry", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ProcessOrder", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ProcessOrder", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Notes", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Notes", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Notes", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Notes", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_StageName", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "StageName", System.Data.DataRowVersion.Original, Nothing)})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "UniqueProcesses", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("LotEntry", "LotEntry"), New System.Data.Common.DataColumnMapping("ProcessOrder", "ProcessOrder"), New System.Data.Common.DataColumnMapping("Notes", "Notes"), New System.Data.Common.DataColumnMapping("StageName", "StageName")})})
        DA.Fill(DS)

        Select Case What
            Case "Find"
                If Not DS.Tables(0).Rows.Count = 0 Then
                    DR = DS.Tables(0).Rows(0)
                    CommentControl = DR("Notes").ToString
                Else
                    CommentControl = ""
                End If
            Case "Edit"
                If Not DS.Tables(0).Rows.Count = 0 Then
                    DR = DS.Tables(0).Rows(0)
                    DR.AcceptChanges()
                    DR.BeginEdit()
                    DR("Notes") = Comment
                    DR.EndEdit()
                    DA.Update(DS, "UniqueProcesses")
                
                End If

        End Select
        Connection.Close()
    End Function

    Protected Sub ButtonSaveComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSaveComment.Click
        CommentControl(Me.LabelLotNumber.Text, "Edit", Me.TextBoxComment.Text, Session("Stage").ToString)
        G1update()
    End Sub

    Protected Sub ButtonClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClose.Click

    End Sub
End Class
