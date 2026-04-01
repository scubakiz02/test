
Partial Class Production_StageWork
    Inherits System.Web.UI.Page
    Dim MyLotNumber As String
    Dim MyID As String = ""
    Dim MyStage As String
    Dim MyStep As String
    Dim View As String
    Dim Saticode As New Class1

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            MyLotNumber = Request.QueryString("LotNumber")
            MyID = Mid(MyLotNumber, 1, MyLotNumber.IndexOf("-"))
            MyStage = Request.QueryString("Stage")
            MyStep = Request.QueryString("Step")
            View = Request.QueryString("View")
            If Not Page.IsPostBack Then
                LoadMain()
            End If

            '?LotNumber=2152-2016-3842&Stage=Strip%20Standard&Step=2&View=No
        Catch ex As Exception

        End Try

    End Sub

    Sub LoadMain()
        Dim DS As New Data.DataSet
        Dim dr As Data.DataRow

        Me.LabelLotNumber.Text = MyLotNumber
        Me.LabelStage.Text = MyStage

        DS = Saticode.GetMyDataSet("SELECT SUM(InQty) AS [In], SUM(OutQty) AS Out, SUM(InQty) - SUM(OutQty) AS [left] FROM dbo.WaferMover GROUP BY LotEntry, [Order] HAVING (LotEntry = N'" & MyLotNumber & "') AND ([Order] = " & MyStep & ")")
        If DS.Tables(0).Rows.Count > 0 Then
            dr = DS.Tables(0).Rows(0)
            Me.LabelTotalIn.Text = dr("In").ToString
            Me.LabelTotalOut.Text = dr("Out").ToString
            Me.LabelTotalRemaining.Text = dr("left").ToString
        End If

        DS.Clear()

        DS = Saticode.GetMyDataSet("SELECT SUM(dbo.DefectTracking.Qty) AS DefQty FROM dbo.WaferMover INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.WaferMover.LotEntry = N'" & MyLotNumber & "') AND (dbo.WaferMover.[Order] = " & MyStep & ")")
        If DS.Tables(0).Rows.Count > 0 Then
            dr = DS.Tables(0).Rows(0)
            Me.LabelTotalDefect.Text = dr("DefQty").ToString
            If Me.LabelTotalDefect.Text = "" Then
                Me.LabelTotalDefect.Text = 0
            End If
        Else
            Me.LabelTotalDefect.Text = "0"
        End If

        If Not MyID = "" Then
            GetDefectlist()
            PopDefects()
        End If

    End Sub

    Sub GetDefectlist()
        Me.SqlDataSourceDefectList.SelectCommand = "SELECT TOP (100) PERCENT dbo.T_ID_Defects.Defect, dbo.T_ID_Defects.Type, dbo.T_ID_Defects.[Group] FROM dbo.T_ID_Defects INNER JOIN dbo.DefectDefs ON dbo.T_ID_Defects.Defect = dbo.DefectDefs.DefectName WHERE (dbo.T_ID_Defects.ID = '" & MyID & "') AND (dbo.DefectDefs.StageName = N'" & MyStage & "') GROUP BY dbo.T_ID_Defects.Defect, dbo.T_ID_Defects.Type, dbo.T_ID_Defects.[Group] ORDER BY dbo.T_ID_Defects.Defect"
        Me.GridViewDefects.DataBind()
    End Sub

    Sub PopDefects()
        'SELECT TOP (100) PERCENT SUM(ISNULL(dbo.DefectTracking.Qty, 0)) AS total, dbo.DefectTracking.DefectName FROM dbo.WaferMover INNER JOIN dbo.UniqueProcesses ON dbo.WaferMover.LotEntry = dbo.UniqueProcesses.LotEntry INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.UniqueProcesses.LotEntry = N'2152-2016-3842') GROUP BY dbo.DefectTracking.DefectName, dbo.UniqueProcesses.StageName HAVING (dbo.UniqueProcesses.StageName = N'Strip Standard')
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        DS = Saticode.GetMyDataSet("SELECT TOP (100) PERCENT SUM(ISNULL(dbo.DefectTracking.Qty, 0)) AS total, dbo.DefectTracking.DefectName FROM dbo.WaferMover INNER JOIN dbo.UniqueProcesses ON dbo.WaferMover.LotEntry = dbo.UniqueProcesses.LotEntry INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.UniqueProcesses.LotEntry = N'" & MyLotNumber & "') GROUP BY dbo.DefectTracking.DefectName, dbo.UniqueProcesses.StageName HAVING (dbo.UniqueProcesses.StageName = N'" & MyStage & "')")

        If Not DS.Tables(0).Rows.Count = 0 Then
            For i As Int16 = 0 To DS.Tables(0).Rows.Count - 1
                DR = DS.Tables(0).Rows(i)
                For G As Int16 = 0 To Me.GridViewDefects.Rows.Count - 1
                    If Me.GridViewDefects.Rows(G).Cells(0).Text = DR("DefectName").ToString Then
                        CType(Me.GridViewDefects.Rows(G).Cells(3).FindControl("TextBoxDefectQty"), TextBox).Text = DR("total").ToString
                        Exit For
                    End If
                Next
            Next
        End If
    End Sub

    Sub AdjDefects()
        Dim numtest As Integer
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim got As Boolean = False
        Dim DT As New Data.DataTable
        Dim What As New ArrayList
        Dim bean As Integer = 0

        DT.Columns.Add("Defect", Type.GetType("System.String"))
        DT.Columns.Add("Group", Type.GetType("System.String"))
        DT.Columns.Add("Qty", Type.GetType("System.String"))

        DS = Saticode.GetMyDataSet("SELECT TOP (100) PERCENT SUM(ISNULL(dbo.DefectTracking.Qty, 0)) AS total, dbo.DefectTracking.DefectName FROM dbo.WaferMover INNER JOIN dbo.UniqueProcesses ON dbo.WaferMover.LotEntry = dbo.UniqueProcesses.LotEntry INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.UniqueProcesses.LotEntry = N'" & MyLotNumber & "') GROUP BY dbo.DefectTracking.DefectName, dbo.UniqueProcesses.StageName HAVING (dbo.UniqueProcesses.StageName = N'" & MyStage & "')")

        For G As Int16 = 0 To Me.GridViewDefects.Rows.Count - 1
            got = False
            If Not DS.Tables(0).Rows.Count = 0 Then
                For i As Int16 = 0 To DS.Tables(0).Rows.Count - 1
                    DR = DS.Tables(0).Rows(i)
                    If Me.GridViewDefects.Rows(G).Cells(0).Text = DR("DefectName").ToString Then
                        got = True
                        If Not CType(Me.GridViewDefects.Rows(G).Cells(3).FindControl("TextBoxDefectQty"), TextBox).Text = DR("total").ToString Then
                            DT.Rows.Add()
                            DT.Rows(DT.Rows.Count - 1)("Defect") = Me.GridViewDefects.Rows(G).Cells(0).Text
                            DT.Rows(DT.Rows.Count - 1)("Group") = Me.GridViewDefects.Rows(G).Cells(2).Text
                            DT.Rows(DT.Rows.Count - 1)("Qty") = CType(Me.GridViewDefects.Rows(G).Cells(3).FindControl("TextBoxDefectQty"), TextBox).Text - DR("total").ToString

                            Exit For
                        End If
                    End If
                Next
            End If

            If got = False Then
                If Integer.TryParse(CType(Me.GridViewDefects.Rows(G).Cells(3).FindControl("TextBoxDefectQty"), TextBox).Text, numtest) Then
                    If CType(Me.GridViewDefects.Rows(G).Cells(3).FindControl("TextBoxDefectQty"), TextBox).Text > 0 Then
                        DT.Rows.Add()
                        DT.Rows(DT.Rows.Count - 1)("Defect") = Me.GridViewDefects.Rows(G).Cells(0).Text
                        DT.Rows(DT.Rows.Count - 1)("Group") = Me.GridViewDefects.Rows(G).Cells(2).Text
                        DT.Rows(DT.Rows.Count - 1)("Qty") = CType(Me.GridViewDefects.Rows(G).Cells(3).FindControl("TextBoxDefectQty"), TextBox).Text
                        'Make_Defect_Record(Me.GridViewDefects.Rows(G).Cells(0).Text, Me.GridViewDefects.Rows(G).Cells(2).Text, CType(Me.GridViewDefects.Rows(G).Cells(3).FindControl("TextBoxDefectQty"), TextBox).Text)
                    End If
                End If
            End If
        Next

        If DT.Rows.Count > 0 Then
            'pop up

            Me.GridViewDefect_OK.DataSource = DT
            Me.GridViewDefect_OK.DataBind()


            ' if ok then loop the array and call the function


        End If




        LoadMain()




    End Sub

    Sub ChangeDefectQty(NewQty As Integer, OldQty As Integer, Defect_Name As String, Group As String)
        Make_Defect_Record(Defect_Name, Group, NewQty - OldQty)
    End Sub

    Function Make_Defect_Record(Defect_Name As String, Group As String, qty As Integer) As String
        Dim SatiUser As String = User.Identity.Name.ToString
        Dim DefectLocation As String = ""
        Dim RecordNumber As Int64

        Select Case Group
            Case "StripEtch"
                DefectLocation = "-6"
            Case "Polish"
                DefectLocation = "-5"
            Case "Reject"
                DefectLocation = "-2"
            Case "Lap"
                DefectLocation = "-4"
            Case "T7"
                DefectLocation = "-11"
        End Select

        'enter into wafermover the main entry
        RecordNumber = Saticode.ModWafermover(MyLotNumber, "New", MyStep, 0, qty)
        Saticode.ModWafermover(MyLotNumber, "New", DefectLocation, qty, 0)

        'enter into the defect table
        Saticode.ModDefectTracking("Add", RecordNumber, Defect_Name, DefectLocation, qty)

    End Function

    Protected Sub ButtonAddDefect_Click(sender As Object, e As EventArgs) Handles ButtonAddDefect.Click
        AdjDefects()
    End Sub
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.NotesBox.Text = CType(Me.GridViewDefects.Rows(1).Cells(3).FindControl("TextBoxDefectQty"), TextBox).Text
    End Sub

    Protected Sub ButtonDefectOK_Click(sender As Object, e As EventArgs) Handles ButtonDefectOK.Click

    End Sub
End Class
