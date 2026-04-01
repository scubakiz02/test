Imports DBCharTableAdapters
Imports CannedPathTableAdapters
Imports UniqueprocessesTableAdapters
Imports WaferMoverTableTableAdapters
Imports WH_InvintoryTableAdapters
Imports Class1
Partial Class PC_MakeLotPart2
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        If Page.IsPostBack = False Then
            Me.IDTextBox.Text = Session("ID").ToString
            Me.WLTextBox.Text = Session("WL").ToString
            Me.QtyTextBox.Text = Session("QTY").ToString
        End If
    End Sub
    Sub QtyCheck()

    End Sub

    Sub MakeLot()
        Dim WL As Integer = Me.WLTextBox.Text
        Dim ID As String = Me.IDTextBox.Text
        Dim Qty As Integer = Me.QtyTextBox.Text

        'Get Canned path
        Dim Path As New Get_PathTableAdapter
        Path.GetData(ID)

        'check for canned path
        Dim RowCount As Int16 = Path.GetData(ID).Rows.Count
        If RowCount = 0 Then
            Me.InfoTextBox.Text = "Canned Path Not Found"
            Exit Sub
        End If

        'Get Qty
        If Me.CheckBox1.Checked = True Then
            If Me.SQtyTextBox.Text >= 0 Then
                qty = Me.SQtyTextBox.Text
            Else
                Me.InfoTextBox.Text = "You have to enter a Special Qty >0"
                Exit Sub
            End If
        End If

        'Check Qty
        If Qty > Me.QtyTextBox.Text Then
            Me.InfoTextBox.Text = "Qty to High"
            Exit Sub
        End If

        'make lot number
        Dim RunNumberTable As New DB_CharacteristicsTableAdapter
        Dim Run As String = RunNumberTable.GetRunNumber + 1
        Dim LotNumber As String = ID & "-" & Right(Run, 4) & "-" & WL

        'enter canned path
        Dim i As Int16 = 0
        Dim PathRow As Data.DataRow
        Dim UniqueTable As New UniqueProcessesTableAdapter
        Dim FirstStage As String = ""
        For i = 0 To RowCount - 1
            PathRow = Path.GetData(ID).Rows(i)
            If PathRow("ProcessOrder") = "1" Then
                FirstStage = PathRow("StageName")
            End If
            UniqueTable.InsertQuery(LotNumber, PathRow("ProcessOrder"), PathRow("StageName"), System.DateTime.Now.ToShortDateString)
        Next

        'Mark the first stage "Compleat"
        UniqueTable.UpdateMakeCompleat(LotNumber, "1", FirstStage, System.DateTime.Now.ToShortDateString, LotNumber, "1", FirstStage)

        'Enter in Wafer Mover
        Dim WaferMoverTable As New WaferMoverTableAdapter
        WaferMoverTable.InsertWaferMover(LotNumber, "1", "0", Qty, "LotStart", "Processing", User.Identity.Name.ToString)
        WaferMoverTable.InsertWaferMover(LotNumber, "2", Qty, "0", "LotStart", "Results", User.Identity.Name.ToString)

        'record runnumber
        RunNumberTable.UpdateRunNumber("RunNum", Run, "RunNum", Run - 1)

        'Remove from WHInv
        If Session("InvType").ToString = "Normal" Then
            Dim WH_Inv As New T_WH_InvintoryTableAdapter
            WH_Inv.Insert_WH_inv_Lot(ID, WL, "Made Lot", "-" & Qty, LotNumber, User.Identity.Name.ToString, System.DateTime.Now.ToShortDateString)
            If Qty = Me.QtyTextBox.Text Then 'Check If Wafer Log Needs To Be Clossed Out
                WH_Inv.InsertTransaction(ID, WL, "WaferLog Complete", "0", System.DateTime.Now.ToShortDateString, User.Identity.Name.ToString, System.DateTime.Now.ToShortDateString)
            End If
        Else
            Saticode.ReceivatoryWafers("Made Lot", Session("ID").ToString, WL, "-" & Qty, "", "", LotNumber, "", 0, User.Identity.Name.ToString)
            If Qty = Me.QtyTextBox.Text Then 'Check If Wafer Log Needs To Be Clossed Out
                Saticode.ReceivatoryWafers("WaferLog Complete", Session("ID").ToString, WL, "0", "", "", "", "", 0, User.Identity.Name.ToString)
            End If
        End If

        'Update Form
        Me.InfoTextBox.Text = "Lot# " & LotNumber & ", Qty of " & Qty & ", in " & FirstStage
        Me.Button2.Visible = True


    End Sub


    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Response.Redirect(Session("MakeLotPart1").ToString)
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Button1.Visible = False
        Me.Button2.Visible = True
        MakeLot()

    End Sub
End Class
