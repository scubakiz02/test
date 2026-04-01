Imports WaferMoverTableTableAdapters
Imports UniqueprocessesTableAdapters
Imports ActionTrackerTableAdapters


Partial Class PC_MergeLots
    Inherits System.Web.UI.Page

    Protected Sub DiaDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshGrids(1)
        RefreshGrids(2)
    End Sub

    Protected Sub FirstPassCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FirstPassCheckBox.CheckedChanged
        RefreshGrids(1)
        RefreshGrids(2)
    End Sub

    Protected Sub SecondPassCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshGrids(1)
        RefreshGrids(2)
    End Sub

    Sub RefreshGrid1()
        'SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.MainID.Diameter = 2) AND (NOT (dbo.UniqueProcesses.LotEntry LIKE N'%R%')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName
        Dim Build As Boolean = True
        Dim SQLString As String = ""
        Dim SQLLotStyleString As String = ") AND (dbo.UniqueProcesses.LotEntry LIKE N'%') "

        If Me.FirstPassCheckBox.Checked = True And Me.SecondPassCheckBox.Checked = True Then
            SQLLotStyleString = ") AND (dbo.UniqueProcesses.LotEntry LIKE N'%') "
        Else
            If Me.FirstPassCheckBox.Checked = True Then
                SQLLotStyleString = ") AND (NOT (dbo.UniqueProcesses.LotEntry LIKE N'%R%')) "
            End If
            If Me.SecondPassCheckBox.Checked = True Then
                SQLLotStyleString = ") AND (dbo.UniqueProcesses.LotEntry LIKE N'%R%') "
            End If
            If Me.FirstPassCheckBox.Checked = False And Me.SecondPassCheckBox.Checked = False Then
                SQLString = "SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.MainID.Diameter = 2) AND (NOT (dbo.UniqueProcesses.LotEntry LIKE N'%R%')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"
                Me.FromLotsSqlDataSource.SelectCommand = SQLString
                Me.GridView1.DataBind()
                Build = False
            End If
        End If

        If Build = True Then
            SQLString = "SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND "
            SQLString = SQLString & "(dbo.MainID.Diameter = " & Me.DiaDropDownList.SelectedItem.Text 'Diameter
            SQLString = SQLString & SQLLotStyleString
            SQLString = SQLString & "GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"
            'SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND '(dbo.MainID.Diameter = 200) AND (dbo.UniqueProcesses.LotEntry LIKE N'%') GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName
            Me.FromLotsSqlDataSource.SelectCommand = SQLString
        End If


    End Sub


    Protected Sub MergeCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.InfoLabel.Visible = True Then
            Me.InfoLabel.Visible = False
        End If
        UpdateQty()
        RefreshGrids(2)
    End Sub

    Sub UpdateQty()
        Dim rows As Integer
        Dim i As Integer
        Dim WaferCount As Integer
        rows = Me.GridView1.Rows.Count
        For i = 0 To rows - 1
            CType(Me.GridView1.Rows(i).Cells(7).FindControl("QtyTextBox"), TextBox).BackColor = Drawing.Color.White
            If CType(Me.GridView1.Rows(i).Cells(0).FindControl("MergeCheckBox"), CheckBox).Checked = True Then
                If CType(Me.GridView1.Rows(i).Cells(7).FindControl("QtyTextBox"), TextBox).Text = "" Then
                    CType(Me.GridView1.Rows(i).Cells(7).FindControl("QtyTextBox"), TextBox).Text = Me.GridView1.Rows(i).Cells(6).Text
                    WaferCount = WaferCount + CType(Me.GridView1.Rows(i).Cells(7).FindControl("QtyTextBox"), TextBox).Text
                    CType(Me.GridView1.Rows(i).Cells(7).FindControl("QtyTextBox"), TextBox).BackColor = Drawing.Color.LightGreen
                Else
                    If CType(Me.GridView1.Rows(i).Cells(7).FindControl("QtyTextBox"), TextBox).Text > Me.GridView1.Rows(i).Cells(6).Text Or CType(Me.GridView1.Rows(i).Cells(7).FindControl("QtyTextBox"), TextBox).Text < 0 Then
                        CType(Me.GridView1.Rows(i).Cells(7).FindControl("QtyTextBox"), TextBox).BackColor = Drawing.Color.LightCoral
                    Else
                        WaferCount = WaferCount + CType(Me.GridView1.Rows(i).Cells(7).FindControl("QtyTextBox"), TextBox).Text
                        CType(Me.GridView1.Rows(i).Cells(7).FindControl("QtyTextBox"), TextBox).BackColor = Drawing.Color.LightGreen
                    End If

                End If
            Else
                CType(Me.GridView1.Rows(i).Cells(7).FindControl("QtyTextBox"), TextBox).Text = ""
            End If
        Next
        Me.TotalQtyLabel.Text = WaferCount
    End Sub

    Protected Sub QtyTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateQty()
    End Sub
    
    Sub RefreshGrids(ByVal Grid As Int16)
        'G1
        'SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.MainID.Diameter = 2) AND (NOT (dbo.UniqueProcesses.LotEntry LIKE N'%R%'))                                                  GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0))                                                              ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName
        'G2
        'SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.MainID.Diameter = 2) AND (NOT (dbo.UniqueProcesses.LotEntry LIKE N'%R%')) AND (dbo.UniqueProcesses.LotEntry LIKE N'2386%') GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) AND (NOT (dbo.UniqueProcesses.LotEntry = N'2386-1234-1234')) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName
        'G2 With More IDs
        'SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.MainID.Diameter = 2) AND (NOT (dbo.UniqueProcesses.LotEntry LIKE N'%R%')) AND (dbo.UniqueProcesses.LotEntry LIKE N'2386%') OR (dbo.UniqueProcesses.LotEntry LIKE N'2386%') GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) AND (NOT (dbo.UniqueProcesses.LotEntry = N'2386-1234-1234')) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName
        Dim Build As Boolean = True
        Dim LotNumberString As String = ""
        Dim IDString As String = ""
        Dim SQLString As String = ""
        Dim SQLLotStyleString As String = ") AND (dbo.UniqueProcesses.LotEntry LIKE N'%') "
        Dim StageString As String = ""
        Dim Customer As Boolean = False

        If Me.FirstPassCheckBox.Checked = True And Me.SecondPassCheckBox.Checked = True Then
            SQLLotStyleString = ") AND (dbo.UniqueProcesses.LotEntry LIKE N'%') "
        Else
            If Me.FirstPassCheckBox.Checked = True Then
                SQLLotStyleString = ") AND (NOT (dbo.UniqueProcesses.LotEntry LIKE N'%R%')) "
            End If
            If Me.SecondPassCheckBox.Checked = True Then
                SQLLotStyleString = ") AND (dbo.UniqueProcesses.LotEntry LIKE N'%R%') "
            End If
            If Me.FirstPassCheckBox.Checked = False And Me.SecondPassCheckBox.Checked = False Then
                SQLString = "SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.MainID.Diameter = 2) AND (NOT (dbo.UniqueProcesses.LotEntry LIKE N'%R%')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"
                Me.FromLotsSqlDataSource.SelectCommand = SQLString
                Me.GridView1.DataBind()
                Build = False
            End If
        End If

        If Grid = 2 Then
            If Me.FirstPassCheckBox.Checked = False And Me.SecondPassCheckBox.Checked = False Then
                SQLString = "SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.MainID.Diameter = 2) AND (NOT (dbo.UniqueProcesses.LotEntry LIKE N'%R%')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"
                Me.ToLotsSqlDataSource.SelectCommand = SQLString
                Me.GridView2.DataBind()
                Build = False
            End If
            Dim First As Boolean = True
            Dim rows As Integer
            rows = Me.GridView1.Rows.Count

            If Not rows = 0 Then
                '**********************************************************************
                '********************Same ID's*****************************************
                '**********************************************************************
                If Me.SameIDRadioButton.Checked = True Then
                    'AND (NOT (dbo.UniqueProcesses.LotEntry = N'2386-1234-1234'))
                    IDString = "AND (dbo.UniqueProcesses.LotEntry LIKE N'"
                    LotNumberString = "AND (NOT (dbo.UniqueProcesses.LotEntry = N'"
                    Dim i As Integer
                    Dim GetID As String
                    Dim LastID As String = ""
                    Dim GetLotNumber As String
                    For i = 0 To rows - 1
                        If CType(Me.GridView1.Rows(i).Cells(0).FindControl("MergeCheckBox"), CheckBox).Checked = True Then
                            GetID = Left(Me.GridView1.Rows(i).Cells(1).Text, 4)
                            GetLotNumber = Me.GridView1.Rows(i).Cells(1).Text
                            If First = True Then
                                ' AND (dbo.UniqueProcesses.LotEntry LIKE N'4193%' OR dbo.UniqueProcesses.LotEntry LIKE N'4228%')

                                IDString = IDString & GetID & "%'"
                                LotNumberString = LotNumberString & GetLotNumber & "'))"
                                First = False
                                LastID = GetID
                            Else
                                'OR (dbo.UniqueProcesses.LotEntry LIKE N'2386%') 
                                If Not LastID = GetID Then
                                    IDString = IDString & " OR dbo.UniqueProcesses.LotEntry LIKE N'" & GetID & "%'"
                                End If
                                LotNumberString = LotNumberString & " And (NOT (dbo.UniqueProcesses.LotEntry = N'" & GetLotNumber & "'))"
                                LastID = GetID
                            End If
                        End If
                    Next
                    IDString = IDString & ")"
                End If

                '**********************************************************************
                '********************Same Customer*************************************
                '**********************************************************************
                If Me.SameCustomerRadioButton.Checked = True Then
                    Customer = True
                    '(MainID_1.MainID = N'3082' OR MainID_1.MainID = N'3018')
                    IDString = "AND (MainID_1.MainID = N'"
                    LotNumberString = "AND (NOT (dbo.UniqueProcesses.LotEntry = N'"
                    Dim i As Integer
                    Dim GetID As String
                    Dim LastID As String = ""
                    Dim GetLotNumber As String
                    For i = 0 To rows - 1
                        If CType(Me.GridView1.Rows(i).Cells(0).FindControl("MergeCheckBox"), CheckBox).Checked = True Then
                            GetID = Left(Me.GridView1.Rows(i).Cells(1).Text, 4)
                            GetLotNumber = Me.GridView1.Rows(i).Cells(1).Text
                            If First = True Then
                                IDString = IDString & GetID & "'"
                                LotNumberString = LotNumberString & GetLotNumber & "'))"
                                First = False
                                LastID = GetID
                            Else
                                If Not LastID = GetID Then
                                    IDString = IDString & " OR MainID_1.MainID = N'" & GetID & "'"
                                End If
                                LotNumberString = LotNumberString & " And (NOT (dbo.UniqueProcesses.LotEntry = N'" & GetLotNumber & "'))"
                                LastID = GetID
                            End If
                        End If
                    Next
                    IDString = IDString & ")"
                End If


                '**********************************************************************
                '********************All ID's******************************************
                '**********************************************************************
                If Me.AllIDsRadioButton.Checked = True Then
                    LotNumberString = "AND (NOT (dbo.UniqueProcesses.LotEntry = N'"
                    Dim i As Integer
                    Dim GetLotNumber As String
                    For i = 0 To rows - 1
                        If CType(Me.GridView1.Rows(i).Cells(0).FindControl("MergeCheckBox"), CheckBox).Checked = True Then
                            GetLotNumber = Me.GridView1.Rows(i).Cells(1).Text
                            If First = True Then
                                LotNumberString = LotNumberString & GetLotNumber & "'))"
                                First = False
                            Else
                                LotNumberString = LotNumberString & " And (NOT (dbo.UniqueProcesses.LotEntry = N'" & GetLotNumber & "'))"
                            End If
                        End If
                    Next
                End If
                '**********************************************************************
                '*********************Stage Stage***************************************
                '**********************************************************************
                If Me.SameStageCheckBox.Checked = True Then
                    Dim i As Integer
                    Dim FirstStage As Boolean = True
                    Dim LastStage As String = ""
                    rows = Me.GridView1.Rows.Count
                    Dim GetStage As String = ""
                    StageString = " AND (dbo.UniqueProcesses.StageName = N'"
                    For i = 0 To rows - 1
                        If CType(Me.GridView1.Rows(i).Cells(0).FindControl("MergeCheckBox"), CheckBox).Checked = True Then
                            GetStage = Me.GridView1.Rows(i).Cells(2).Text
                            If FirstStage = True Then
                                StageString = StageString & GetStage & "'"
                                FirstStage = False
                                LastStage = GetStage
                            Else
                                If Not LastStage = GetStage Then
                                    StageString = StageString & " OR dbo.UniqueProcesses.StageName = N'" & GetStage & "'"
                                End If
                                LastStage = GetStage
                            End If
                        End If
                    Next
                    StageString = StageString & ")"
                End If
                '**********************************************************************
                '**********************************************************************
                '**********************************************************************
            Else
                Build = False
            End If 'Make Sure we have Rows
            If First = True Then
                SQLString = "SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.MainID.Diameter = 2) AND (NOT (dbo.UniqueProcesses.LotEntry LIKE N'%R%')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"
                Me.ToLotsSqlDataSource.SelectCommand = SQLString
                Me.GridView2.DataBind()
                Build = False
            End If
        End If 'end Grid2

        If Build = True Then
            Select Case Grid
                Case 1
                    'G1
                    SQLString = "SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND "
                    SQLString = SQLString & "(dbo.MainID.Diameter = " & Me.DiaDropDownList.SelectedItem.Text 'Diameter
                    SQLString = SQLString & SQLLotStyleString
                    SQLString = SQLString & "GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"
                    'SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND '(dbo.MainID.Diameter = 200) AND (dbo.UniqueProcesses.LotEntry LIKE N'%') GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName
                    Me.FromLotsSqlDataSource.SelectCommand = SQLString
                Case 2
                    'G2
                    If Customer = False Then
                        SQLString = "SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND "
                    Else
                        SQLString = "SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.MainID AS MainID_1 INNER JOIN dbo.Customer ON MainID_1.CustomerID = dbo.Customer.CustomerID INNER JOIN dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND "
                    End If
                    SQLString = SQLString & "(dbo.MainID.Diameter = " & Me.DiaDropDownList.SelectedItem.Text 'Diameter
                    SQLString = SQLString & SQLLotStyleString & " "
                    SQLString = SQLString & IDString
                    SQLString = SQLString & " GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) "
                    SQLString = SQLString & LotNumberString & StageString
                    SQLString = SQLString & " ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"
                    Me.ToLotsSqlDataSource.SelectCommand = SQLString

            End Select

        End If

        'SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check'))                                                                                                                                                               AND (dbo.MainID.Diameter = 300) AND (dbo.UniqueProcesses.LotEntry LIKE N'%R%')                                                              GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName
        'SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.MainID AS MainID_1 INNER JOIN dbo.Customer ON MainID_1.CustomerID = dbo.Customer.CustomerID INNER JOIN dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.MainID.Diameter = 300) AND (dbo.UniqueProcesses.LotEntry LIKE N'%R%') AND (MainID_1.MainID = N'3082')                              GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) AND (NOT (dbo.UniqueProcesses.LotEntry = N'1-134-1234')) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName
        'SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.MainID AS MainID_1 INNER JOIN dbo.Customer ON MainID_1.CustomerID = dbo.Customer.CustomerID INNER JOIN dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.MainID.Diameter = 300) AND                                                (MainID_1.MainID = N'3082' OR MainID_1.MainID = N'3018') GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) AND (NOT (dbo.UniqueProcesses.LotEntry = N'1-134-1234')) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName


        'SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.MainID.Diameter = 200) AND (NOT (dbo.UniqueProcesses.LotEntry LIKE N'%R%')) AND (dbo.UniqueProcesses.LotEntry LIKE N'2386%' OR dbo.UniqueProcesses.LotEntry LIKE N'2553%') GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) AND (NOT (dbo.UniqueProcesses.LotEntry = N'1234-1234-1234')) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName
        'SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.MainID.Diameter = 200) AND      (dbo.UniqueProcesses.LotEntry LIKE N'%R%')  AND (dbo.UniqueProcesses.LotEntry LIKE N'2850%' OR (dbo.UniqueProcesses.LotEntry LIKE N'4269%') GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) AND (NOT (dbo.UniqueProcesses.LotEntry = N'2850-9925S-R175')) And (NOT (dbo.UniqueProcesses.LotEntry = N'4269-1871P-R423')) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName
    End Sub

    Protected Sub SameIDRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshGrids(2)
    End Sub

    Protected Sub SameCustomerRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshGrids(2)
    End Sub

    Protected Sub AllIDsRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshGrids(2)
    End Sub

    Protected Sub CheckBox1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshGrids(2)
    End Sub

    Protected Sub MakeMergeButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Merge() = True Then
            RefreshGrids(1)
            RefreshGrids(2)
            Me.InfoLabel.Visible = True
        End If
    End Sub

    Function Merge() As Boolean
        Merge = False
        Dim Rows1 As Integer
        Dim Rows2 As Integer
        Dim i As Integer
        Dim ToLot As String = ""
        Dim FromLot As String = ""
        Dim ToStep As String = ""
        Dim FromStep As String = ""
        Dim ToQty As String = ""
        Dim FromQty As String = ""
        Dim FromStage As String
        Dim Go As Boolean = False
        Rows1 = Me.GridView1.Rows.Count
        Rows2 = Me.GridView2.Rows.Count

        'What are we Merging into?
        For i = 0 To Rows2 - 1
            If CType(Me.GridView2.Rows(i).Cells(0).FindControl("SelectRadioButton"), RadioButton).Checked = True Then
                ToLot = Me.GridView2.Rows(i).Cells(1).Text
                ToStep = Me.GridView2.Rows(i).Cells(3).Text
                ToQty = Me.TotalQtyLabel.Text
                Go = True
            End If
        Next

        'Make Tranfer Records
        If Go = True Then
            Dim WMT As New WaferMoverTableAdapter
            Dim ActionTable As New ActionTrackerTableAdapter
            Dim CR As New UniqueProcessesTableAdapter
            For i = 0 To Rows1 - 1
                If CType(Me.GridView1.Rows(i).Cells(0).FindControl("MergeCheckBox"), CheckBox).Checked = True Then
                    If CType(Me.GridView1.Rows(i).Cells(7).FindControl("QtyTextBox"), TextBox).BackColor = Drawing.Color.LightGreen Then
                        FromLot = Me.GridView1.Rows(i).Cells(1).Text
                        FromStep = Me.GridView1.Rows(i).Cells(3).Text
                        FromQty = CType(Me.GridView1.Rows(i).Cells(7).FindControl("QtyTextBox"), TextBox).Text
                        FromStage = Me.GridView1.Rows(i).Cells(2).Text

                        'Transaction into  TOlot
                        ActionTable.InsertActionTracker(FromLot, ToLot, FromStep, ToStep, FromQty, "Merge", User.Identity.Name.ToString)
                        WMT.InsertWaferMover(ToLot, ToStep, FromQty, "0", "Log", "Merged-In", User.Identity.Name.ToString)

                        'Transaction into  Fromlot
                        ActionTable.InsertActionTracker(ToLot, FromLot, ToStep, FromStep, FromQty, "Split", User.Identity.Name.ToString)
                        WMT.InsertWaferMover(FromLot, FromStep, "0", FromQty, "Log", "SplitOut-ToMerge", User.Identity.Name.ToString)

                        If CType(Me.GridView1.Rows(i).Cells(7).FindControl("QtyTextBox"), TextBox).Text = Me.GridView1.Rows(i).Cells(6).Text Then
                            CR.UpdateMakeCompleat(FromLot, FromStep, FromStage, System.DateTime.Now.ToShortDateString, FromLot, FromStep, FromStage)
                        End If
                    End If


                End If
            Next

        End If ' End Go True
        If Go = True Then
            Return True
        End If
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("PC", Server)
    End Sub
End Class
