Imports UniqueprocessesTableAdapters
Imports StageReportTableAdapters
Imports ReworkINVTableAdapters

Partial Class PC_CheckPoint
    Inherits System.Web.UI.Page
    Dim OkToCheck As Boolean = False

    Dim PreStep As String
    Dim CStep As String

    Dim LotType As String
    Dim Lot As String
    Dim LotID As String
    Dim LotWL As String

    Dim StartQty As Integer
    Dim EndQty As Integer
    Dim SplitQty As Integer
    Dim MergedQty As Integer
    Dim InQty As Integer
    Dim OutQty As Integer
    Dim RejectQty As Integer
    Dim ReworkQty As Integer
    Dim CV As Integer




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        Me.LotNumLabel.Text = Session("Lotnumber").ToString

        Me.StageLabel.Text = Session("Stage").ToString
        CStep = Session("Step").ToString
        Lot = Session("Lotnumber").ToString
        LotID = Mid(Lot, 1, 4)
        Me.IDLabel.Text = LotID
        LotWL = Mid(Lot, Lot.LastIndexOf("-") + 2)
        Me.WLLabel.Text = LotWL
        Dim DS_Path As New Data.DataSet
        Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim SQL As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        SQL.CommandText = "SELECT LotEntry, ProcessOrder, StageName, Complete FROM dbo.UniqueProcesses WHERE (LotEntry = N'" & Session("LotNumber").ToString & "') ORDER BY ProcessOrder"

        MyAdapter.SelectCommand = SQL
        MyAdapter.SelectCommand.Connection = Connection

        Connection.Open()
        MyAdapter.Fill(DS_Path)
        Me.StatLabel.Text = DS_Path.Tables(0).Rows.Count

        'look to see if able to check in
        Dim DS_PathCount As Integer = DS_Path.Tables(0).Rows.Count
        Dim PathRow As Data.DataRow
        Dim i As Integer
        Dim RecNumber As Integer
        Dim LotStep As Integer = Session("Step").ToString
        OkToCheck = False
        Do
            PathRow = DS_Path.Tables(0).Rows(i)
            If PathRow("Complete") Is System.DBNull.Value Then
                RecNumber = PathRow("ProcessOrder")
                Exit Do
            End If
            i = i + 1
        Loop
        If RecNumber >= LotStep Then
            OkToCheck = True
            Me.StatLabel.Text = "Ready For Check In"
            Me.StatLabel.BackColor = Drawing.Color.Green
        Else
            Me.StatLabel.Text = "Not Ready for Check In"
            Me.CheckInButton.Visible = False
            Me.StatLabel.BackColor = Drawing.Color.Red
        End If



        Connection.Close()
    End Sub

    Protected Sub CheckinButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckInButton.Click
        'Get All The Values
        GetPathInfo()
        GetStartEndQty()
        GetSplitQty()
        GetMergedQty()
        GetRejectQty()
        GetReworkQty()

        'Calulate In Qty
        InQty = StartQty - SplitQty
        InQty = InQty + MergedQty
        Me.InQtyLabel.Text = InQty

        'Calulate Out Qty
        OutQty = EndQty
        Me.OutQtyLabel.Text = EndQty

        'Calculate Check Value
        CV = OutQty + RejectQty
        CV = CV + ReworkQty
        CV = CV - InQty
        Me.CheckValueLabel.Text = CV
        If CV = 0 Then
            Me.CheckValueLabel.BackColor = Drawing.Color.Green
            Me.ContinueButton.Visible = True
            Me.CheckInButton.Visible = False
        Else
            Me.CheckValueLabel.BackColor = Drawing.Color.Red
        End If
        'Response.Redirect("CheckPoint.aspx")
    End Sub
    Sub GetPathInfo()
        Dim LotTypeLook As String = Lot
        'Get Lot Type
        If LotTypeLook.Contains("R") Then
            If LotTypeLook.Contains("P") Then
                LotType = "P"
            Else
                LotType = "R"
            End If
        Else
            LotType = "F"
        End If
        Me.LotTypeLabel.Text = LotType

        'Dim DS_Path As New Data.DataSet
        Dim MyDataSet As New Data.DataSet
        Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim SQL As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        SQL.CommandText = "SELECT LotEntry, ProcessOrder, StageName FROM dbo.UniqueProcesses WHERE (LotEntry = N'" & Lot & "')"
        MyAdapter.SelectCommand = SQL
        MyAdapter.SelectCommand.Connection = Connection
        Connection.Open()
        MyAdapter.Fill(MyDataSet)
        Dim PathRow As Data.DataRow
        Dim i As Integer
        Dim CheckPoint As String = Session("Stage").ToString
        Dim Precheckpoint As String

        Do
            PathRow = MyDataSet.Tables(0).Rows(i)
            If PathRow("StageName") = CheckPoint Then
                Do
                    i = i - 1
                    PathRow = MyDataSet.Tables(0).Rows(i)
                    Precheckpoint = PathRow("StageName").ToString
                    If Precheckpoint.Contains("WIP") Or i = 0 Then
                        PreStep = PathRow("ProcessOrder")
                        Session("PreStep") = PreStep
                        Exit Do
                    End If
                Loop

                Exit Do
            End If
            i = i + 1
        Loop
        Connection.Close()

    End Sub

    Sub GetStartEndQty()
        Dim MyDataSet As New Data.DataSet
        Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim SQL As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        SQL.CommandText = "SELECT dbo.WaferMover.LotEntry, dbo.UniqueProcesses.ProcessOrder, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS IQ, SUM(dbo.WaferMover.OutQty) AS OQ FROM dbo.WaferMover INNER JOIN dbo.UniqueProcesses ON dbo.WaferMover.LotEntry = dbo.UniqueProcesses.LotEntry AND dbo.WaferMover.[Order] = dbo.UniqueProcesses.ProcessOrder GROUP BY dbo.WaferMover.LotEntry, dbo.UniqueProcesses.ProcessOrder, dbo.UniqueProcesses.StageName HAVING (dbo.WaferMover.LotEntry = N'" & Lot & "')"
        MyAdapter.SelectCommand = SQL
        MyAdapter.SelectCommand.Connection = Connection
        Connection.Open()
        MyAdapter.Fill(MyDataSet)
        Dim DRow As Data.DataRow
        Dim i As Integer = 0
        Do
            DRow = MyDataSet.Tables(0).Rows(i)
            If DRow("ProcessOrder").ToString = PreStep Then
                StartQty = DRow("OQ").ToString
                Me.StartQtyLabel.Text = StartQty
                Exit Do
            End If
            i = i + 1
        Loop
        i = 0
        Do
            DRow = MyDataSet.Tables(0).Rows(i)
            If DRow("ProcessOrder").ToString = CStep Then
                EndQty = DRow("IQ").ToString
                Me.EndQtyLabel.Text = EndQty
                Exit Do
            End If
            i = i + 1
        Loop
        Connection.Close()

    End Sub
    Sub GetSplitQty()
        'Dim DS_Path As New Data.DataSet
        Dim MyDataSet As New Data.DataSet
        Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim SQL As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        SQL.CommandText = "SELECT SUM(Qty) AS SplitQty FROM dbo.ActionTracker WHERE (ParentLotNum = N'" & Lot & "') AND (Action LIKE N'split%') AND (P_Order < " & CStep + 1 & " AND P_Order > " & PreStep & ")"
        'SELECT SUM(Qty) AS SplitQty FROM dbo.ActionTracker WHERE (ParentLotNum = N'1713-6496-3069') AND (Action LIKE N'split%') AND (P_Order < 7) AND (P_Order > 2)
        MyAdapter.SelectCommand = SQL
        MyAdapter.SelectCommand.Connection = Connection
        Connection.Open()
        MyAdapter.Fill(MyDataSet)
        Dim SRow As Data.DataRow
        If MyDataSet.Tables(0).Rows.Count = 0 Then
            SplitQty = 0
            Me.SplitQtyLabel.Text = SplitQty
            Connection.Close()
            Exit Sub
        End If
        SRow = MyDataSet.Tables(0).Rows(0)
        If SRow("SplitQty").ToString = "" Then
            SplitQty = 0
            Me.SplitQtyLabel.Text = SplitQty
            Connection.Close()
            Exit Sub
        End If

        SplitQty = SRow("SplitQty").ToString
        Me.SplitQtyLabel.Text = SplitQty
        Connection.Close()
    End Sub
    Sub GetMergedQty()
        Dim MyDataSet As New Data.DataSet
        Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim SQL As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        SQL.CommandText = "SELECT SUM(Qty) AS SplitQty FROM dbo.ActionTracker WHERE (ParentLotNum = N'" & Lot & "') AND (Action LIKE N'Merge%') AND (P_Order < " & CStep + 1 & ") AND (P_Order > " & PreStep & ")"
        MyAdapter.SelectCommand = SQL
        MyAdapter.SelectCommand.Connection = Connection
        Connection.Open()
        MyAdapter.Fill(MyDataSet)
        Dim SRow As Data.DataRow
        If MyDataSet.Tables(0).Rows.Count = 0 Then
            MergedQty = 0
            Me.MergeQtyLabel.Text = MergedQty
            Connection.Close()
            Exit Sub
        End If
        SRow = MyDataSet.Tables(0).Rows(0)
        If SRow("SplitQty").ToString = "" Then
            MergedQty = 0
            Me.MergeQtyLabel.Text = MergedQty
            Connection.Close()
            Exit Sub
        End If

        MergedQty = SRow("SplitQty").ToString
        Me.MergeQtyLabel.Text = MergedQty
        Connection.Close()

    End Sub

    Sub GetRejectQty()
        Dim MyDataSet As New Data.DataSet
        Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim SQL As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        'SQL.CommandText = "SELECT SUM(dbo.DefectTracking.Qty) AS RejectQty FROM dbo.WaferMover INNER JOIN dbo.UniqueProcesses ON dbo.WaferMover.LotEntry = dbo.UniqueProcesses.LotEntry AND dbo.WaferMover.[Order] = dbo.UniqueProcesses.ProcessOrder INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.DefectTracking.Location = '-1' OR dbo.DefectTracking.Location = '-2') AND (dbo.UniqueProcesses.ProcessOrder > " & PreStep & ") AND (dbo.UniqueProcesses.ProcessOrder < " & CStep + 1 & ") GROUP BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName HAVING (dbo.UniqueProcesses.LotEntry = N'" & Lot & "') AND (NOT (SUM(dbo.DefectTracking.Qty) = 0))"
        SQL.CommandText = "SELECT SUM(dbo.DefectTracking.Qty) AS RejectQty FROM dbo.WaferMover INNER JOIN dbo.UniqueProcesses ON dbo.WaferMover.LotEntry = dbo.UniqueProcesses.LotEntry AND dbo.WaferMover.[Order] = dbo.UniqueProcesses.ProcessOrder INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.DefectTracking.Location = '-1' OR dbo.DefectTracking.Location = '-2') AND (dbo.UniqueProcesses.ProcessOrder > " & PreStep & ") AND (dbo.UniqueProcesses.ProcessOrder < " & CStep + 1 & ") GROUP BY dbo.UniqueProcesses.LotEntry HAVING (dbo.UniqueProcesses.LotEntry = N'" & Lot & "') AND (NOT (SUM(dbo.DefectTracking.Qty) = 0))"
        MyAdapter.SelectCommand = SQL
        MyAdapter.SelectCommand.Connection = Connection
        Connection.Open()
        MyAdapter.Fill(MyDataSet)
        Dim SRow As Data.DataRow
        If MyDataSet.Tables(0).Rows.Count = 0 Then
            RejectQty = 0
            Me.RejectQtyLabel.Text = RejectQty
            Connection.Close()
            Exit Sub
        End If
        SRow = MyDataSet.Tables(0).Rows(0)
        If SRow("RejectQty").ToString = "" Then
            RejectQty = 0
            Me.RejectQtyLabel.Text = RejectQty
            Connection.Close()
            Exit Sub
        End If

        RejectQty = SRow("RejectQty").ToString
        Me.RejectQtyLabel.Text = RejectQty
        Connection.Close()

    End Sub

    Sub GetReworkQty()

        Dim MyDataSet As New Data.DataSet
        Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim SQL As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        'SQL.CommandText = "SELECT SUM(dbo.DefectTracking.Qty) AS ReworkQty FROM dbo.WaferMover INNER JOIN dbo.UniqueProcesses ON dbo.WaferMover.LotEntry = dbo.UniqueProcesses.LotEntry AND dbo.WaferMover.[Order] = dbo.UniqueProcesses.ProcessOrder INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.DefectTracking.Location < - 2) AND (dbo.UniqueProcesses.ProcessOrder > " & PreStep & ") AND (dbo.UniqueProcesses.ProcessOrder < " & CStep & ") GROUP BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName HAVING (dbo.UniqueProcesses.LotEntry = N'" & Lot & "') AND (NOT (SUM(dbo.DefectTracking.Qty) = 0))"
        SQL.CommandText = "SELECT SUM(dbo.DefectTracking.Qty) AS ReworkQty FROM dbo.WaferMover INNER JOIN dbo.UniqueProcesses ON dbo.WaferMover.LotEntry = dbo.UniqueProcesses.LotEntry AND dbo.WaferMover.[Order] = dbo.UniqueProcesses.ProcessOrder INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.DefectTracking.Location < - 2) AND (dbo.UniqueProcesses.ProcessOrder > " & PreStep & ") AND (dbo.UniqueProcesses.ProcessOrder < " & CStep & ") GROUP BY dbo.UniqueProcesses.LotEntry HAVING (dbo.UniqueProcesses.LotEntry = N'" & Lot & "') AND (NOT (SUM(dbo.DefectTracking.Qty) = 0))"
        MyAdapter.SelectCommand = SQL
        MyAdapter.SelectCommand.Connection = Connection
        Connection.Open()
        MyAdapter.Fill(MyDataSet)
        Dim SRow As Data.DataRow
        If MyDataSet.Tables(0).Rows.Count = 0 Then
            ReworkQty = 0
            Me.ReworkQtyLabel.Text = ReworkQty
            Connection.Close()
            Exit Sub
        End If
        SRow = MyDataSet.Tables(0).Rows(0)
        If SRow("ReworkQty").ToString = "" Then
            ReworkQty = 0
            Me.ReworkQtyLabel.Text = ReworkQty
            Connection.Close()
            Exit Sub
        End If

        ReworkQty = SRow("ReworkQty").ToString
        Me.ReworkQtyLabel.Text = ReworkQty
        Connection.Close()
    End Sub

    Protected Sub ContinueButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContinueButton.Click

        'Enter Reworks in to Rework Inv
        '
        Dim MyDataSet As New Data.DataSet
        Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim SQL As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        SQL.CommandText = "SELECT SUM(dbo.DefectTracking.Qty) AS ReworkQty, dbo.DefectTracking.Location FROM dbo.WaferMover INNER JOIN dbo.UniqueProcesses ON dbo.WaferMover.LotEntry = dbo.UniqueProcesses.LotEntry AND dbo.WaferMover.[Order] = dbo.UniqueProcesses.ProcessOrder INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.DefectTracking.Location < - 2) AND (dbo.UniqueProcesses.ProcessOrder > " & Session("PreStep").ToString & ") AND (dbo.UniqueProcesses.ProcessOrder < " & CStep & ") GROUP BY dbo.UniqueProcesses.LotEntry, dbo.DefectTracking.Location HAVING (NOT (SUM(dbo.DefectTracking.Qty) = 0)) AND (dbo.UniqueProcesses.LotEntry = N'" & Lot & "')"
        MyAdapter.SelectCommand = SQL
        MyAdapter.SelectCommand.Connection = Connection
        Connection.Open()
        MyAdapter.Fill(MyDataSet)
        Dim i As Integer
        Dim SRow As Data.DataRow
        Dim RowCount As Integer
        RowCount = MyDataSet.Tables(0).Rows.Count
        If RowCount = 0 Then
            Connection.Close()
            'Exit Sub
        End If
        i = 0
        Dim ReworkTable As New T_Rework_InvintoryTableAdapter
        For i = 0 To RowCount - 1
            SRow = MyDataSet.Tables(0).Rows(i)
            ReworkTable.InsertReworkInv(SRow("Location").ToString, Me.IDLabel.Text, SRow("ReworkQty").ToString, "Added", Date.Now, User.Identity.Name.ToString, Lot, Session("Stage").ToString)
        Next
        Connection.Close()

        Dim ReportStage As New T_Stage_ReportTableAdapter
        ReportStage.InsertStageReport(Me.LotTypeLabel.Text, Session("Stage").ToString, Me.IDLabel.Text, Me.WLLabel.Text, Me.LotNumLabel.Text, Me.InQtyLabel.Text, Me.OutQtyLabel.Text, Me.RejectQtyLabel.Text, Me.ReworkQtyLabel.Text, Date.Now, User.Identity.Name.ToString)
        Me.ContinueButton.Visible = False
        'Response.Redirect("/SaitDotNet/Production/ProcessWafers.aspx")
        Response.Redirect("~/Production/ProcessWafers.aspx")
        'Dim CR As New UniqueProcessesTableAdapter
        'CR.UpdateMakeCompleat(Session("LotNumber").ToString, Session("Step").ToString, Session("Stage").ToString, System.DateTime.Now.ToShortTimeString, Session("LotNumber").ToString, Session("Step").ToString, Session("Stage").ToString)

    End Sub


End Class
