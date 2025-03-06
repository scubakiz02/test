Imports DBCharTableAdapters
Imports UniqueprocessesTableAdapters
Imports WaferMoverTableTableAdapters
Imports ReworkINVTableAdapters
Imports ActionTrackerTableAdapters

Partial Class PC_MakeReworkLots
    Inherits System.Web.UI.Page

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ChangeData()
    End Sub

    Protected Sub DropDownList2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ChangeData()
    End Sub

    Sub ChangeData()
        'Selected Rework Type
        Dim SQLString As String = ""
        Dim CustomerName As String
        Select Case Me.DropDownList1.SelectedValue.ToString
            Case ""
                CustomerName = ""
            Case Else
                CustomerName = Me.DropDownList1.SelectedValue.ToString

        End Select

        Dim ReworkType As String = ""

        ReworkType = FindReworkType(Me.DropDownList2.SelectedValue)

        SQLString = "SELECT dbo.MainID.CustomerID, dbo.MainID.Diameter, dbo.MainID.MainID, SUM(dbo.T_Rework_Invintory.Qty) AS Qty FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID INNER JOIN dbo.T_Rework_Invintory ON dbo.MainID.MainID = dbo.T_Rework_Invintory.ID WHERE (dbo.Customer.Customer_Name = N'" & CustomerName & "') GROUP BY dbo.MainID.MainID, dbo.T_Rework_Invintory.Type, dbo.MainID.CustomerID, dbo.MainID.Diameter HAVING (dbo.T_Rework_Invintory.Type = N'" & ReworkType & "') ORDER BY dbo.MainID.CustomerID, dbo.MainID.Diameter DESC, dbo.MainID.MainID"

        Me.IDsSqlDataSource.SelectCommand = SQLString
        Me.IDDropDownList.Items.Clear()
        Me.IDDropDownList.Items.Add("Select ID...")

        Me.IDDropDownList.DataBind()
    End Sub

    Function FindReworkType(ByVal Code As String) As String
        FindReworkType = ""
        Select Case Code
            Case "P"
                FindReworkType = "-5"
            Case "SE"
                FindReworkType = "-6"
            Case "L"
                FindReworkType = "-4"
            Case "T7"
                FindReworkType = "-11"
        End Select
    End Function

    Protected Sub SelectQtyTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        AddQty()
    End Sub


    Sub AddQty()
        Dim ErrorCheck As Boolean = False
        Dim i As Int16
        Dim Rows As Integer
        Dim CV As Integer = 0
        Dim TQ As Integer = 0
        Dim Qty As String = ""
        Dim AV_Qty As String = ""
        Dim LastSize As String = ""

        Rows = Me.GridView1.Rows.Count
        For i = 0 To Rows - 1
            CType(Me.GridView1.Rows(i).Cells(4).FindControl("SelectQtyTextBox"), TextBox).BackColor = Drawing.Color.White
            Me.GridView1.Rows(i).Cells(1).BackColor = Drawing.Color.White
            Qty = CType(Me.GridView1.Rows(i).Cells(4).FindControl("SelectQtyTextBox"), TextBox).Text
            AV_Qty = Me.GridView1.Rows(i).Cells(3).Text
            If Qty = "" Then
                Qty = 0
            End If
            If Not CType(AV_Qty, Integer) < CType(Qty, Integer) Then
                If Not Qty = 0 Then
                    If LastSize = "" Then
                        LastSize = Me.GridView1.Rows(i).Cells(1).Text
                    End If
                    If Me.GridView1.Rows(i).Cells(1).Text = LastSize Then
                        CV = CInt(Qty)
                        TQ = TQ + CV
                        CType(Me.GridView1.Rows(i).Cells(4).FindControl("SelectQtyTextBox"), TextBox).BackColor = Drawing.Color.LightGreen
                        LastSize = Me.GridView1.Rows(i).Cells(1).Text
                        If LastSize = "300" And Me.DropDownList2.SelectedValue = "P" Then
                            Me.Panel300mm.Visible = True
                        Else
                            Me.Panel300mm.Visible = False
                        End If
                    Else
                        Me.GridView1.Rows(i).Cells(1).BackColor = Drawing.Color.LightCoral
                        ErrorCheck = True
                    End If
                End If
            Else
                CType(Me.GridView1.Rows(i).Cells(4).FindControl("SelectQtyTextBox"), TextBox).BackColor = Drawing.Color.LightCoral
                ErrorCheck = True
            End If

        Next
        Me.QtyLabel.Text = TQ
        If TQ = 0 Then
            Me.QtyLabel.BackColor = Drawing.Color.LightCoral
        Else
            Me.QtyLabel.BackColor = Drawing.Color.LightGreen
        End If

        If Panel300mm.Visible = True Then
            Me.CMPRadioButton.Checked = False
            Me.DSPRadioButton.Checked = False
            Me.Panel300mm.BackColor = Drawing.Color.LightCoral
        End If

        If ErrorCheck = False Then
            MakeIDlist(True, LastSize)
        Else
            MakeIDlist(False, "")
        End If
    End Sub
    Sub MakeIDlist(ByVal Fill As Boolean, ByVal Size As String)
        If Fill = True Then
            Dim SqlString As String

            ' AND (dbo.MainID.Diameter = 200)
            '            SELECT dbo.MainID.CustomerID, dbo.MainID.Diameter, dbo.MainID.MainID FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID WHERE (dbo.Customer.Customer_Name = N'Blank')                                           GROUP BY dbo.MainID.MainID, dbo.MainID.CustomerID, dbo.MainID.Diameter HAVING (dbo.MainID.Diameter = 200)          ORDER BY dbo.MainID.CustomerID, dbo.MainID.Diameter DESC, dbo.MainID.MainID
            SqlString = "SELECT dbo.MainID.CustomerID, dbo.MainID.Diameter, dbo.MainID.MainID FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID WHERE (dbo.Customer.Customer_Name = N'" & Me.DropDownList1.SelectedValue.ToString & "') GROUP BY dbo.MainID.MainID, dbo.MainID.CustomerID, dbo.MainID.Diameter HAVING (dbo.MainID.Diameter = " & Size & ") ORDER BY dbo.MainID.CustomerID, dbo.MainID.Diameter DESC, dbo.MainID.MainID"

            Me.IDbyDieSqlDataSource.SelectCommand = SqlString
            Me.IDDropDownList.Items.Clear()
            Me.IDDropDownList.Items.Add("Select ID...")

            Me.IDDropDownList.DataBind()
        Else
            Me.IDDropDownList.Items.Clear()
        End If


    End Sub
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If RunCheck() = True Then
            MakeReworkLot(Me.IDDropDownList.SelectedItem.Text, Me.QtyLabel.Text, Me.DropDownList2.SelectedItem.Value)
            ChangeData()
            Me.IDDropDownList.BackColor = Drawing.Color.LightCoral
            Me.QtyLabel.Text = "0"
            Me.QtyLabel.BackColor = Drawing.Color.LightCoral
        End If
    End Sub
    Function RunCheck() As Boolean

        Dim i As Int16
        Dim Rows As Integer
        Dim CV As Integer = 0
        Dim TQ As Integer = 0
        Dim Qty As String = ""
        Dim AV_Qty As String = ""
        Dim LastSize As String = ""
        Rows = Me.GridView1.Rows.Count
        For i = 0 To Rows - 1
            If CType(Me.GridView1.Rows(i).Cells(4).FindControl("SelectQtyTextBox"), TextBox).BackColor = Drawing.Color.LightCoral Then
                Return False
            End If
            If Me.GridView1.Rows(i).Cells(1).BackColor = Drawing.Color.LightCoral Then
                Return False
            End If
        Next

        If Me.IDDropDownList.BackColor = Drawing.Color.LightCoral Then
            Return False
        End If

        If Me.Panel300mm.Visible = True Then
            If Me.Panel300mm.BackColor = Drawing.Color.LightCoral Then
                Return False
            End If
        End If
        If Me.QtyLabel.Text = "0" Then
            Return False
        End If
        Return True
    End Function

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
            Case Is = "L"
                LotModChr = "L"
                PathType = "LapRework"
                InvType = "-4"
            Case Is = "P"
                LotModChr = "P"
                PathType = "PolishRework"
                InvType = "-5"

                If Me.Panel300mm.Visible = True Then
                    If Me.DSPRadioButton.Checked = True Then
                        PathType = "DSPRework"
                    End If
                    If Me.CMPRadioButton.Checked = True Then
                        PathType = "CMPRework"
                    End If
                End If
            Case Is = "T7"
                InvType = "-11"
        End Select

        'Get the Path
        Dim MyDataSet As New Data.DataSet
        Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim SQL As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = Session("DBConnect")
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
        Dim NewWL As String = "R" & Right(FullWL, 3) 'Dim NewWL As String = "R" & Mid(FullWL, 2)
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
        i = 0
        Dim Rows As Integer
        Dim DebID As String
        Dim DebQty As Integer
        Rows = Me.GridView1.Rows.Count
        For i = 0 To Rows - 1
            If CType(Me.GridView1.Rows(i).Cells(4).FindControl("SelectQtyTextBox"), TextBox).BackColor = Drawing.Color.LightGreen Then
                DebID = Me.GridView1.Rows(i).Cells(2).Text
                DebQty = CType(Me.GridView1.Rows(i).Cells(4).FindControl("SelectQtyTextBox"), TextBox).Text
                If Not MainID = DebID Then
                    SplitIDs(DebID, MainID, DebQty)
                End If
                DebitInv(DebID, DebQty, InvType, SatiUser, LotNumber)
            End If
        Next

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
        Connection.ConnectionString = Session("DBConnect")
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

    Protected Sub DSPRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Panel300mm.BackColor = Drawing.Color.LightGreen
    End Sub

    Protected Sub CMPRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Panel300mm.BackColor = Drawing.Color.LightGreen
    End Sub

    Protected Sub IDDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.IDDropDownList.SelectedItem.Text = "Select ID..." Then
            Me.IDDropDownList.BackColor = Drawing.Color.LightCoral
        Else
            Me.IDDropDownList.BackColor = Drawing.Color.LightGreen
        End If
    End Sub

    Sub SplitIDs(ByVal FromID As String, ByVal ToID As String, ByVal Qty As Integer)

        'Insert in Action Tracker
        Dim ActionTable As New ActionTrackerTableAdapter
        ActionTable.InsertActionTracker(FromID & "-0000-0000", ToID & "-0000-0000", 1, 1, Qty, "ReworkTranferIn", User.Identity.Name.ToString)
        ActionTable.InsertActionTracker(ToID & "-0000-0000", FromID & "-0000-0000", 1, 1, Qty, "ReworkTranferOut", User.Identity.Name.ToString)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("PC", Server)
    End Sub
End Class
