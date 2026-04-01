Imports DBCharTableAdapters
Imports UniqueprocessesTableAdapters
Imports WaferMoverTableTableAdapters
Imports ReworkINVTableAdapters

Partial Class MakeReworkLot
    Inherits System.Web.UI.Page

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim Row As String
        If e.CommandName = "MakeLot" Then
            Row = e.CommandArgument.ToString
            Dim LotQty As String
            LotQty = CType(Me.GridView1.Rows(Row).Cells(2).FindControl("Textbox1"), TextBox).Text
            If QtyCheck(Me.GridView1.Rows(Row).Cells(1).Text, LotQty) = True Then
                MakeReworkLot(Me.GridView1.Rows(Row).Cells(0).Text, LotQty, "SE")
            End If
        End If
    End Sub
    Protected Sub GridView2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView2.RowCommand
        Dim Row As String
        If e.CommandName = "MakeLot" Then
            Row = e.CommandArgument.ToString
            Dim LotQty As String
            LotQty = CType(Me.GridView2.Rows(Row).Cells(2).FindControl("Textbox2"), TextBox).Text
            If QtyCheck(Me.GridView2.Rows(Row).Cells(1).Text, LotQty) = True Then
                MakeReworkLot(Me.GridView2.Rows(Row).Cells(0).Text, LotQty, "Lap")
            End If
        End If
    End Sub
    Protected Sub GridView3_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView3.RowCommand
        Dim Row As String
        If e.CommandName = "MakeLot" Then
            Row = e.CommandArgument.ToString
            Dim LotQty As String
            LotQty = CType(Me.GridView3.Rows(Row).Cells(2).FindControl("Textbox3"), TextBox).Text
            If QtyCheck(Me.GridView3.Rows(Row).Cells(1).Text, LotQty) = True Then
                MakeReworkLot(Me.GridView3.Rows(Row).Cells(0).Text, LotQty, "Polish")
            End If
        End If
    End Sub
    Sub MakeReworkLot(ByVal MainID As String, ByVal QTY As Integer, ByVal Area As String)
        Dim SatiUser As String = User.Identity.Name.ToString
        Dim LotModChr As String = ""
        Dim PathType As String = ""
        Dim InvType As String = ""
        Select Case Area
            Case Is = "SE"
                LotModChr = "S"
                PathType = "SERework"
                InvType = "-6"
            Case Is = "Lap"
                LotModChr = "L"
                PathType = "LapRework"
                InvType = "-4"
            Case Is = "Polish"
                LotModChr = "P"
                PathType = "PolishRework"
                InvType = "-5"
        End Select

        'Get the Path
        Dim MyDataSet As New Data.DataSet
        Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim SQL As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        SQL.CommandText = "SELECT dbo.CannedPaths.ProcessOrder AS Step, dbo.CannedPaths.StageName AS Stage FROM dbo.CannedPathInfo INNER JOIN dbo.CannedPaths ON dbo.CannedPathInfo.PathName = dbo.CannedPaths.PathName WHERE (dbo.CannedPathInfo.MainID = '" & MainID & "') AND (dbo.CannedPathInfo.PathType = '" & PathType & "')"
        MyAdapter.SelectCommand = SQL
        MyAdapter.SelectCommand.Connection = Connection
        Connection.Open()
        MyAdapter.Fill(MyDataSet)
        Dim ReworkPathRow As Data.DataRow = MyDataSet.Tables(0).Rows(0)

        'check for canned path
        Dim RowCount As Int16 = ReworkPathRow.Table.Rows.Count
        If RowCount = 0 Then
            Me.InfoTextBox.Text = "Canned Path Not Found"
            Exit Sub
        End If

        'make lot number
        Dim FullRun As String = GetRunNumber() + 1
        Dim FullWL As String = GetWLNumber() + 1
        Dim NewRun As String = FullRun & LotModChr
        Dim NewWL As String = "R" & Mid(FullWL, 2)
        Dim LotNumber As String = MainID & "-" & NewRun & "-" & NewWL

        'Update Run & WL
        RecordRunNumber(FullRun)
        RecordWLNumber(FullWL)

        'enter canned path'Enter in UniqueProcesses
        Dim i As Int16 = 0
        Dim UniqueTable As New UniqueProcessesTableAdapter
        Dim FirstStage As String = ""
        For i = 0 To RowCount - 1
            If i = 0 Then
                FirstStage = ReworkPathRow("Stage")
            End If
            ReworkPathRow = MyDataSet.Tables(0).Rows(i)
            UniqueTable.InsertQuery(LotNumber, ReworkPathRow("Step"), ReworkPathRow("Stage"), System.DateTime.Now.ToShortDateString)
        Next
        Connection.Close()

        'Mark the first stage "Compleat"
        UniqueTable.UpdateMakeCompleat(LotNumber, "1", FirstStage, System.DateTime.Now.ToShortDateString, LotNumber, "1", FirstStage)

        'Enter in Wafer Mover
        Dim WaferMoverTable As New WaferMoverTableAdapter
        WaferMoverTable.InsertWaferMover(LotNumber, "1", QTY, QTY, "LotStart", "Processing", SatiUser)
        WaferMoverTable.InsertWaferMover(LotNumber, "2", QTY, "0", "LotStart", "Results", SatiUser)
        'make a record that will help witht the old ALTs Reports and Old Month End Wip inv value reports
        WaferMoverTable.InsertWaferMover(LotNumber, InvType, "0", QTY, "LotStart", "Processing", SatiUser)

        'Remove from Inentory
        DebitInv(MainID, QTY, InvType, SatiUser, LotNumber)

        'Enter Lot Number in InfoBox
        Me.InfoTextBox.Text = "Your Lot Number is " & LotNumber & " , For a Qty of " & QTY

    End Sub

    Function GetRunNumber() As String
        Dim RunNumberTable As New DB_CharacteristicsTableAdapter
        Return RunNumberTable.GetRunNumber
    End Function

    Sub RecordRunNumber(ByVal Run As String)
        Dim RunNumberTable As New DB_CharacteristicsTableAdapter
        RunNumberTable.UpdateRunNumber("RunNum", Run, "RunNum", Run - 1)
    End Sub

    Function GetWLNumber() As String
        Dim MyDataSet As New Data.DataSet
        Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim SQL As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        SQL.CommandText = "SELECT FieldName, Value FROM dbo.DB_Characteristics WHERE (FieldName = N'RecWaferlog')"
        MyAdapter.SelectCommand = SQL
        MyAdapter.SelectCommand.Connection = Connection
        Connection.Open()
        MyAdapter.Fill(MyDataSet)
        Dim ReworkWL As Data.DataRow = MyDataSet.Tables(0).Rows(0)
        Return ReworkWL("Value").ToString
        Connection.Close()
    End Function

    Sub RecordWLNumber(ByVal WL As String)
        Dim WLTable As New DB_CharacteristicsTableAdapter
        WLTable.UpdateWaferLog("RecWaferlog", WL, "RecWaferlog", WL - 1)
    End Sub

    Sub DebitInv(ByVal MainID As String, ByVal Qty As String, ByVal InvType As String, ByVal SatiUser As String, ByVal Lot As String)
        'Enter the Defects in the Defect Table
        Dim ReworkTable As New T_Rework_InvintoryTableAdapter
        ReworkTable.InsertReworkInv(InvType, MainID, "-" & Qty, "Debit", Date.Now, SatiUser, Lot, "INV")
    End Sub

    Function QtyCheck(ByVal InvQty As String, ByVal LotQty As String) As Boolean
        If LotQty > InvQty Or LotQty < 1 Then
            Me.InfoTextBox.Text = "Bad Qty"
            Return False
        Else
            Return True
        End If

    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("PC", Server)
    End Sub
End Class
