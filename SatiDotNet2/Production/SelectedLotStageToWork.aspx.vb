Imports WaferMoverTableTableAdapters
Imports DefectTrackingTableAdapters
Imports UniqueprocessesTableAdapters
Imports ActionTrackerTableAdapters
Imports DBCharTableAdapters
Imports CannedPathTableAdapters
'Imports Class1
'Imports System.Windows.Forms
Imports System.Net.Mail

Partial Class Production_SelectedLotStageToWork
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Sub ReloadMe()
        Response.Redirect("SelectedLotStageToWork.aspx")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        UpdateMe()
    End Sub

    Protected Sub GridView4_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView4.RowCommand
        'Enter Defect Code
        If e.CommandName = "Add" Then

            Dim row As String = e.CommandArgument.ToString
            Dim Defect As String = Me.GridView4.Rows(row).Cells(0).Text
            Dim Type As String = Me.GridView4.Rows(row).Cells(1).Text
            Dim Group As String = Me.GridView4.Rows(row).Cells(2).Text
            Dim Qty As String = Me.TextBox1.Text
            Dim SatiUser As String = User.Identity.Name.ToString
            Dim WMT As New WaferMoverTableAdapter
            Dim DT As New DefectTrackingTableAdapter
            Dim DefectLocation As String = ""

            If QtyCheck(Qty) = False Then
                Exit Sub
            End If

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
            If DefectLocation = "" Then
                Me.InfoTextBox.Text = "Defect Type Was Not Clear To SATI. Not Entering Any Transaction!"
                Me.TextBox1.Text = ""
                Exit Sub
            End If

            'Enter the wafers as a Qty Out
            WMT.InsertWaferMover(Session("LotNumber").ToString, Session("Step").ToString, "0", Qty, "Log", "Results", SatiUser)

            'Need to get that last movment Record Key for the defect table
            Dim DS_D As New Data.DataSet
            Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
            Dim Connection As New Data.SqlClient.SqlConnection
            Dim SQL As New Data.SqlClient.SqlCommand
            Connection.ConnectionString = Session("DBConnect")
            SQL.CommandText = "SELECT MovementEntry AS MR FROM dbo.WaferMover WHERE (LotEntry = N'" & Session("LotNumber").ToString & "') AND ([Order] = " & Session("Step").ToString & ") AND (InQty = 0) AND (OutQty = " & Qty & ") AND (LotStatus = N'Log') ORDER BY MovementEntry DESC"
            MyAdapter.SelectCommand = SQL
            MyAdapter.SelectCommand.Connection = Connection
            Connection.Open()
            MyAdapter.Fill(DS_D)
            Dim DRow As Data.DataRow
            DRow = DS_D.Tables(0).Rows(0)
            Dim NewMR As String
            NewMR = DRow("MR")
            Connection.Close()

            'Enter the wafers as a Defect and the Defect Location
            WMT.InsertWaferMover(Session("LotNumber").ToString, DefectLocation, Qty, "0", "Log", "Results", SatiUser)

            'Enter the Defects in the Defect Table
            DT.InsertDefects(NewMR, Defect, DefectLocation, Qty, SatiUser)

            ReloadMe()

        End If

        Me.TextBox1.Text = ""
    End Sub

    Protected Sub EnterGoodWafersButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EnterGoodWafersButton.Click
        'Dim WMT As New WaferMoverTableAdapter
        Dim satiuser As String = User.Identity.Name.ToString
        'If QtyCheck(Qty) = False Then
        'Exit Sub
        'End If
        'WMT.InsertWaferMover(Session("LotNumber").ToString, Session("Step").ToString, "0", Qty, "Log", "Results", User)
        'WMT.InsertWaferMover(Session("LotNumber").ToString, Session("Step").ToString + 1, Qty, "0", "Log", "Results", User)
        Dim Qty As String = Me.EnterQtyTextBox.Text
        EnterGoodWafers(Session("LotNumber").ToString, Session("Step").ToString, Qty, satiuser)
        ReloadMe()
    End Sub
    Protected Sub EnterPartialButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EnterPartialButton.Click
        'Button for multy partials
        Dim G6Rows As Integer
        Dim partialQty As String
        Dim PartialID As String
        G6Rows = Me.GridView6.Rows.Count - 1
        Dim i As Integer = 0
        For i = 0 To G6Rows
            PartialID = Me.GridView6.Rows(i).Cells(0).Text.ToString
            partialQty = CType(Me.GridView6.Rows(i).Cells(1).FindControl("PartialTextBox"), TextBox).Text
            If partialQty >= 0 Then
                MakePartial(PartialID, partialQty)
            End If
        Next
        ReloadMe()
    End Sub

    Sub EnterGoodWafers(ByVal lotnumber As String, ByVal PStep As String, ByVal qty As String, ByVal SatiUser As String)
        Dim WMT As New WaferMoverTableAdapter
        If QtyCheck(qty) = False Then
            Exit Sub
        End If
        WMT.InsertWaferMover(lotnumber, PStep, "0", qty, "Log", "Results", SatiUser)
        WMT.InsertWaferMover(lotnumber, PStep + 1, qty, "0", "Log", "Results", SatiUser)
        ReloadMe()
    End Sub
    Function QtyCheck(ByVal qty As String) As Boolean
        'need a int verification
        If qty = "" Then
            qty = 0
        End If
        Dim QtyLeft As String
        Dim DQty As Integer
        Dim LQty As Integer
        QtyLeft = Me.LeftQtyLabel.Text
        DQty = qty
        LQty = QtyLeft
        If qty = 0 Then
            Me.InfoTextBox.Text = "Not a Valid Qty"
            Return False
        End If
        If DQty > LQty Then
            Me.InfoTextBox.Text = "There are only " & Me.LeftQtyLabel.Text & " Wafers left to work with"
            Return False
        Else
            Return True
        End If

    End Function
    Function CCQtyCheck(ByVal qty As String) As Boolean
        'need a int verification
        If qty = "" Then
            qty = 0
        End If
        If qty = 0 Then
            Me.InfoTextBox.Text = "Not a Valid Qty"
            Return False
        Else
            Return True
        End If

    End Function

    Protected Sub RecButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RecButton.Click
        Dim Saticode As New Class1
        If Me.LeftQtyLabel.Text = "0" Then
            Dim CR As New UniqueProcessesTableAdapter
            CR.UpdateMakeCompleat(Session("LotNumber").ToString, Session("Step").ToString, Session("Stage").ToString, System.DateTime.Now.ToShortDateString, Session("LotNumber").ToString, Session("Step").ToString, Session("Stage").ToString)
            Session("View") = "Yes"

            'Saticode.SendMail("Lot# " & Session("LotNumber").ToString & Chr(13) & "Completed Stage: " & Session("Stage").ToString, "Lot Completed Stage")

            'SendRecComplete()

            ReloadMe()

        End If
    End Sub

    Sub SendRecComplete()
        Dim To_MA As New MailAddress("SatiInfo@purewaferinc.com")
        Dim From_MA As New MailAddress("SatiInfo@Purewaferinc.com", "SATI.Net")
        Dim Mail As New MailMessage(From_MA, To_MA)
        Dim SMTP_C As New SmtpClient("localhost")
        SMTP_C.Host = Session("EmailServerIP").ToString
        SMTP_C.Port = Session("EmailServerPort").ToString

        Dim Yeild As Double
        Yeild = Me.GoodQtyLabel.Text / Me.InQtyLabel.Text
        Yeild = Yeild * 100
        Yeild = Decimal.Round(CType(Yeild, Decimal), 2)
        Mail.Subject = "SATI Info Lot Completed Stage"
        Mail.Body = "Lot# " & Session("LotNumber").ToString & Chr(13) & "Completed Stage: " & Session("Stage").ToString & Chr(13) & "In Qty: " & Me.InQtyLabel.Text & Chr(13) & "Defect Qty: " & Me.DefectQtyLabel.Text & Chr(13) & "Passed Qty: " & Me.GoodQtyLabel.Text & Chr(13) & "Yield: " & Yeild & "%"

        SMTP_C.Send(Mail)
    End Sub

    Protected Sub SplitButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitButton.Click
        Dim SplitQty As String
        SplitQty = Me.QtyToSplitTextBox.Text

        If QtyCheck(SplitQty) = True Then
            Dim NewID As String
            NewID = Me.SplitIDDropDownList.SelectedValue

            Dim Main As Boolean

            If NewID = Mid(Session("LotNumber").ToString, 1, 4) Then
                Main = True
            Else
                Main = False
            End If


            'Get Canned path

            If Main = True Then
                '*****************************************************
                Dim DS_D As New Data.DataSet
                Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
                Dim Connection As New Data.SqlClient.SqlConnection
                Dim SQL As New Data.SqlClient.SqlCommand
                Connection.ConnectionString = Session("DBConnect")
                SQL.CommandText = "SELECT LotEntry, ProcessOrder, StageName FROM dbo.UniqueProcesses WHERE (LotEntry = N'" & Session("LotNumber").ToString & "')"
                MyAdapter.SelectCommand = SQL
                MyAdapter.SelectCommand.Connection = Connection
                Connection.Open()
                MyAdapter.Fill(DS_D)

                Dim RowCount As Int16 = DS_D.Tables(0).Rows.Count
                If RowCount = 0 Then
                    Me.InfoTextBox.Text = "Canned Path Not Found"
                    Exit Sub
                End If

                '********************************************************
                Dim RunNumberTable As New DB_CharacteristicsTableAdapter
                Dim Run As String = RunNumberTable.GetRunNumber + 1
                Dim NewLotNumber As String
                Dim WL As String
                Dim LotN As String = Session("LotNumber").ToString

                WL = Mid(Session("LotNumber").ToString, LotN.LastIndexOf("-") + 2)

                Dim RunNumber4Recording As Integer
                RunNumber4Recording = Run

                'see if it is a rework lot
                Dim LotLength As Integer = Session("LotNumber").ToString.Length
                If LotLength = 15 Then
                    Run = Run & Mid(Session("LotNumber").ToString, LotN.LastIndexOf("-"), 1)
                End If
                NewLotNumber = NewID & "-" & Run & "-" & WL

                'enter canned path
                Dim i As Int16 = 0
                Dim PathRow As Data.DataRow
                Dim UniqueTable As New UniqueProcessesTableAdapter

                'Check to see if you can make a parellel Split
                Dim SplitOK As Boolean = False
                Dim StepInsert As Int16
                For i = 0 To RowCount - 1
                    PathRow = DS_D.Tables(0).Rows(i)
                    If PathRow("StageName") = Session("Stage").ToString Then
                        StepInsert = PathRow("ProcessOrder")
                        SplitOK = True
                    End If
                Next


                If SplitOK = True Then
                    RowCount = DS_D.Tables(0).Rows.Count
                    Dim newstep As Int16 = 1
                    UniqueTable.InsertQuery(NewLotNumber, newstep, "WIP", System.DateTime.Now.ToShortDateString)

                    For i = 0 To RowCount - 1
                        PathRow = DS_D.Tables(0).Rows(i)
                        If PathRow("ProcessOrder") >= StepInsert Then
                            newstep = newstep + 1
                            UniqueTable.InsertQuery(NewLotNumber, newstep, PathRow("StageName"), System.DateTime.Now.ToShortDateString)
                        End If
                    Next

                    'Mark the stage "Compleat"
                    UniqueTable.UpdateMakeCompleat(NewLotNumber, "1", "WIP", System.DateTime.Now.ToShortDateString, NewLotNumber, "1", "WIP")

                    'Enter in Wafer Mover
                    Dim WaferMoverTable As New WaferMoverTableAdapter
                    WaferMoverTable.InsertWaferMover(NewLotNumber, "1", "0", SplitQty, "LotStart", "Processing", User.Identity.Name.ToString)
                    WaferMoverTable.InsertWaferMover(NewLotNumber, "2", SplitQty, "0", "LotStart", "Results", User.Identity.Name.ToString)
                    WaferMoverTable.InsertWaferMover(Session("LotNumber").ToString, Session("Step").ToString, "-" & SplitQty, 0, "Log", "PSplit", User.Identity.Name.ToString)

                    'Insert in Action Tracker
                    Dim ActionTable As New ActionTrackerTableAdapter
                    ActionTable.InsertActionTracker(NewLotNumber, Session("LotNumber").ToString, 2, Session("Step").ToString, SplitQty, "Split", User.Identity.Name.ToString)
                    '****

                    RunNumberTable.UpdateRunNumber("RunNum", RunNumber4Recording, "RunNum", RunNumber4Recording - 1)
                Else
                    Me.InfoTextBox.Text = "Can Not Make Split. No Mirroring Stage."
                End If
                Connection.Close()
            End If ' end main = true

            If Main = False Then
                Dim Path As New Get_PathTableAdapter
                Path.GetData(NewID)

                'check for canned path
                Dim RowCount As Int16 = Path.GetData(NewID).Rows.Count
                If RowCount = 0 Then
                    Me.InfoTextBox.Text = "Canned Path Not Found"
                    Exit Sub
                End If

                Dim RunNumberTable As New DB_CharacteristicsTableAdapter
                Dim Run As String = RunNumberTable.GetRunNumber + 1
                Dim NewLotNumber As String
                Dim WL As String
                Dim LotN As String = Session("LotNumber").ToString

                WL = Mid(Session("LotNumber").ToString, LotN.LastIndexOf("-") + 2)

                Dim RunNumber4Recording As Integer
                RunNumber4Recording = Run

                'see if it is a rework lot
                Dim LotLength As Integer = Session("LotNumber").ToString.Length
                If LotLength = 15 Then
                    Run = Run & Mid(Session("LotNumber").ToString, LotN.LastIndexOf("-"), 1)
                End If
                NewLotNumber = NewID & "-" & Run & "-" & WL

                'enter canned path
                Dim i As Int16 = 0
                Dim PathRow As Data.DataRow
                Dim UniqueTable As New UniqueProcessesTableAdapter

                'Check to see if you can make a parellel Split
                Dim SplitOK As Boolean = False
                Dim StepInsert As Int16
                For i = 0 To RowCount - 1
                    PathRow = Path.GetData(NewID).Rows(i)
                    If PathRow("StageName") = Session("Stage").ToString Then
                        StepInsert = PathRow("ProcessOrder")
                        SplitOK = True
                    End If
                Next


                If SplitOK = True Then
                    RowCount = Path.GetData(NewID).Rows.Count
                    Dim newstep As Int16 = 1
                    UniqueTable.InsertQuery(NewLotNumber, newstep, "WIP", System.DateTime.Now.ToShortDateString)
                    For i = 0 To RowCount - 1
                        PathRow = Path.GetData(NewID).Rows(i)
                        If PathRow("ProcessOrder") >= StepInsert Then
                            newstep = newstep + 1
                            UniqueTable.InsertQuery(NewLotNumber, newstep, PathRow("StageName"), System.DateTime.Now.ToShortDateString)
                        End If
                    Next

                    'Mark the stage "Compleat"
                    UniqueTable.UpdateMakeCompleat(NewLotNumber, "1", "WIP", System.DateTime.Now.ToShortDateString, NewLotNumber, "1", "WIP")

                    'Enter in Wafer Mover
                    Dim WaferMoverTable As New WaferMoverTableAdapter
                    WaferMoverTable.InsertWaferMover(NewLotNumber, "1", "0", SplitQty, "LotStart", "Processing", User.Identity.Name.ToString)
                    WaferMoverTable.InsertWaferMover(NewLotNumber, "2", SplitQty, "0", "LotStart", "Results", User.Identity.Name.ToString)
                    WaferMoverTable.InsertWaferMover(Session("LotNumber").ToString, Session("Step").ToString, "-" & SplitQty, 0, "Log", "PSplit", User.Identity.Name.ToString)

                    'Insert in Action Tracker
                    Dim ActionTable As New ActionTrackerTableAdapter
                    ActionTable.InsertActionTracker(NewLotNumber, Session("LotNumber").ToString, 2, Session("Step").ToString, SplitQty, "Split", User.Identity.Name.ToString)
                    '****


                    RunNumberTable.UpdateRunNumber("RunNum", RunNumber4Recording, "RunNum", RunNumber4Recording - 1)
                Else
                    Me.InfoTextBox.Text = "Can Not Make Split. No Mirroring Stage."
                End If


            End If ' end main = false

        End If
        ReloadMe()
    End Sub
    Sub SetForm()
        Select Case Session("View").ToString
            Case "No" '***********************************************************************************************
                Me.ModeLabel.Text = "Page Is In Active Mode....."

                If Me.LeftQtyLabel.Text = "0" Then
                    Me.RecButton.Visible = True
                Else
                    Me.EnterGoodWafersButton.Visible = True
                    Me.GridView4.Visible = True
                    Dim MainID As String
                    MainID = Session("ID").ToString
                    Dim CurrentStage As String = Session("Stage").ToString

                    Select Case CurrentStage

                        Case Is = "Incoming Visual"
                            If Session("Step").ToString = 2 Then
                                Me.CCButton.Visible = True
                            End If

                        Case Is = "Inc Cen Thk"
                            If Session("Step").ToString = 2 Then
                                Me.CCButton.Visible = True
                            End If

                        Case Is = "Incoming T7 Read"
                            If Session("Step").ToString = 2 Then
                                Me.CCButton.Visible = True
                            End If

                        Case Is = "DSP"
                            'Me.PartialEnterButton.Visible = True
                            'Me.PartialQtyTextBox.Visible = True

                        Case Is = "CMP"
                            ViewAddPartials()
                            Me.PartialsSqlDataSource.SelectCommand = "SELECT dbo.Q_Sati_Inv_PolishPartials.LotNumber AS lotEntry, dbo.Q_Sati_Inv_PolishPartials.Qty AS [In] FROM dbo.Q_Sati_Inv_PolishPartials INNER JOIN dbo.TransferID_ByStage ON dbo.Q_Sati_Inv_PolishPartials.ID = dbo.TransferID_ByStage.[To] WHERE (dbo.TransferID_ByStage.StageName = N'CMP' OR dbo.TransferID_ByStage.StageName = N'all') GROUP BY dbo.TransferID_ByStage.[From], dbo.Q_Sati_Inv_PolishPartials.LotNumber, dbo.Q_Sati_Inv_PolishPartials.Qty HAVING (dbo.TransferID_ByStage.[From] = N'" & MainID & "')"
                            Me.PartialEnterButton.Visible = True
                            Me.PartialQtyTextBox.Visible = True

                        Case Is = "Polish"
                            Me.PartialEnterButton.Visible = True
                            Me.PartialQtyTextBox.Visible = True
                            ' ViewAddPartials()
                            'Me.PartialsSqlDataSource.SelectCommand = "SELECT LotEntry, SUM(InQty) AS [In] FROM dbo.WaferMover WHERE ([Order] = 0) GROUP BY LotEntry HAVING (LotEntry LIKE N'%-xxxx') AND (LotEntry LIKE N'" & MainID & "-%') AND (SUM(OutQty) = 0) ORDER BY LotEntry"

                        Case Is = "Laser Inspection"
                            ViewMakeSplitPartials()
                            ViewAddPartials()
                            'SELECT dbo.Q_Sati_Inv_CleanroomPartials.LotNumber AS lotEntry, dbo.Q_Sati_Inv_CleanroomPartials.Qty AS [In] FROM dbo.Q_Sati_Inv_CleanroomPartials INNER JOIN dbo.TransferID_ByStage ON dbo.Q_Sati_Inv_CleanroomPartials.ID = dbo.TransferID_ByStage.[To] WHERE (dbo.TransferID_ByStage.StageName = N'Laser Inspection' OR dbo.TransferID_ByStage.StageName = N'all') AND (dbo.TransferID_ByStage.[From] = N'2828') GROUP BY dbo.Q_Sati_Inv_CleanroomPartials.LotNumber, dbo.Q_Sati_Inv_CleanroomPartials.Qty
                            Me.PartialsSqlDataSource.SelectCommand = "SELECT dbo.Q_Sati_Inv_CleanroomPartials.LotNumber AS lotEntry, dbo.Q_Sati_Inv_CleanroomPartials.Qty AS [In] FROM dbo.Q_Sati_Inv_CleanroomPartials INNER JOIN dbo.TransferID_ByStage ON dbo.Q_Sati_Inv_CleanroomPartials.ID = dbo.TransferID_ByStage.[To] WHERE (dbo.TransferID_ByStage.StageName = N'Laser Inspection' OR dbo.TransferID_ByStage.StageName = N'all') AND (dbo.TransferID_ByStage.[From] = N'" & MainID & "') GROUP BY dbo.Q_Sati_Inv_CleanroomPartials.LotNumber, dbo.Q_Sati_Inv_CleanroomPartials.Qty"
                            Me.MakeLabelPanel.Visible = True


                        Case Is = "Cleanroom"
                            ViewMakeSplitPartials()
                            ViewAddPartials()
                            'SELECT dbo.Q_Sati_Inv_CleanroomPartials.LotNumber AS lotEntry, dbo.Q_Sati_Inv_CleanroomPartials.Qty AS [In] FROM dbo.Q_Sati_Inv_CleanroomPartials INNER JOIN dbo.TransferID_ByStage ON dbo.Q_Sati_Inv_CleanroomPartials.ID = dbo.TransferID_ByStage.[To] WHERE (dbo.TransferID_ByStage.StageName = N'Laser Inspection' OR dbo.TransferID_ByStage.StageName = N'all') AND (dbo.TransferID_ByStage.[From] = N'2828') GROUP BY dbo.Q_Sati_Inv_CleanroomPartials.LotNumber, dbo.Q_Sati_Inv_CleanroomPartials.Qty
                            Me.PartialsSqlDataSource.SelectCommand = "SELECT dbo.Q_Sati_Inv_CleanroomPartials.LotNumber AS lotEntry, dbo.Q_Sati_Inv_CleanroomPartials.Qty AS [In] FROM dbo.Q_Sati_Inv_CleanroomPartials INNER JOIN dbo.TransferID_ByStage ON dbo.Q_Sati_Inv_CleanroomPartials.ID = dbo.TransferID_ByStage.[To] WHERE (dbo.TransferID_ByStage.StageName = N'Laser Inspection' OR dbo.TransferID_ByStage.StageName = N'all') AND (dbo.TransferID_ByStage.[From] = N'" & MainID & "') GROUP BY dbo.Q_Sati_Inv_CleanroomPartials.LotNumber, dbo.Q_Sati_Inv_CleanroomPartials.Qty"
                            Me.MakeLabelPanel.Visible = True


                        Case Is = "Presort"
                            ViewAddPartials()
                            'SELECT dbo.Q_Sati_Inv_PolishPartials.LotNumber AS lotEntry, dbo.Q_Sati_Inv_PolishPartials.Qty AS [In] FROM dbo.Q_Sati_Inv_PolishPartials INNER JOIN dbo.TransferID_ByStage ON dbo.Q_Sati_Inv_PolishPartials.ID = dbo.TransferID_ByStage.[To] WHERE (dbo.TransferID_ByStage.StageName = N'Presort' OR dbo.TransferID_ByStage.StageName = N'all') GROUP BY dbo.TransferID_ByStage.[From], dbo.Q_Sati_Inv_PolishPartials.LotNumber, dbo.Q_Sati_Inv_PolishPartials.Qty HAVING (dbo.TransferID_ByStage.[From] = N'2785')
                            Me.PartialsSqlDataSource.SelectCommand = "SELECT dbo.Q_Sati_Inv_PolishPartials.LotNumber AS lotEntry, dbo.Q_Sati_Inv_PolishPartials.Qty AS [In] FROM dbo.Q_Sati_Inv_PolishPartials INNER JOIN dbo.TransferID_ByStage ON dbo.Q_Sati_Inv_PolishPartials.ID = dbo.TransferID_ByStage.[To] WHERE (dbo.TransferID_ByStage.StageName = N'Presort' OR dbo.TransferID_ByStage.StageName = N'all') GROUP BY dbo.TransferID_ByStage.[From], dbo.Q_Sati_Inv_PolishPartials.LotNumber, dbo.Q_Sati_Inv_PolishPartials.Qty HAVING (dbo.TransferID_ByStage.[From] = N'" & MainID & "')"

                        Case Is = "WIP 2"
                            ViewAddPartials()
                            Me.PartialsSqlDataSource.SelectCommand = "SELECT LotEntry, SUM(InQty) AS [In] FROM dbo.WaferMover WHERE ([Order] = 0) GROUP BY LotEntry HAVING (LotEntry LIKE N'%-xxxx') AND (LotEntry LIKE N'" & MainID & "-%') AND (SUM(OutQty) = 0) ORDER BY LotEntry"
                    End Select
                End If

            Case "Yes" '*************************************************************************************************************
                Me.ModeLabel.Text = "Page Is In View Mode....."
                Me.SplitButton.Visible = False
                Me.RecButton.Visible = False

            Case "NoQty"
                Me.ModeLabel.Text = "Page Is In View Mode....."
                Me.SplitButton.Visible = False
                Me.RecButton.Visible = False

                If Session("Stage").ToString = "Laser Inspection" Then
                    If Session("SpecialPartial").ToString = "Yes" Then
                        Me.ModeLabel.Text = "Page Is In Partial Mode Only....."
                        ViewMakeSplitPartials()
                        ViewAddPartials()
                        'SELECT dbo.Q_Sati_Inv_CleanroomPartials.LotNumber AS lotEntry, dbo.Q_Sati_Inv_CleanroomPartials.Qty AS [In] FROM dbo.Q_Sati_Inv_CleanroomPartials INNER JOIN dbo.TransferID_ByStage ON dbo.Q_Sati_Inv_CleanroomPartials.ID = dbo.TransferID_ByStage.[To] WHERE (dbo.TransferID_ByStage.StageName = N'Laser Inspection' OR dbo.TransferID_ByStage.StageName = N'all') AND (dbo.TransferID_ByStage.[From] = N'2828') GROUP BY dbo.Q_Sati_Inv_CleanroomPartials.LotNumber, dbo.Q_Sati_Inv_CleanroomPartials.Qty
                        Me.PartialsSqlDataSource.SelectCommand = "SELECT dbo.Q_Sati_Inv_CleanroomPartials.LotNumber AS lotEntry, dbo.Q_Sati_Inv_CleanroomPartials.Qty AS [In] FROM dbo.Q_Sati_Inv_CleanroomPartials INNER JOIN dbo.TransferID_ByStage ON dbo.Q_Sati_Inv_CleanroomPartials.ID = dbo.TransferID_ByStage.[To] WHERE (dbo.TransferID_ByStage.StageName = N'Laser Inspection' OR dbo.TransferID_ByStage.StageName = N'all') AND (dbo.TransferID_ByStage.[From] = N'" & Session("ID").ToString & "') GROUP BY dbo.Q_Sati_Inv_CleanroomPartials.LotNumber, dbo.Q_Sati_Inv_CleanroomPartials.Qty"

                    End If
                End If
        End Select

        If Session("Stage").ToString = "Laser Inspection" Then
            Me.MakeLabelPanel.Visible = True
        End If

    End Sub
    Sub UpdateMe()
        Me.LotLabel.Text = Session("LotNumber").ToString
        Session("ID") = Mid(Session("LotNumber").ToString, 1, 4)
        Me.StageLabel.Text = Session("Stage").ToString
        If Not Session("View").ToString = "NoQty" Then
            Dim out As String
            Dim defQty As String

            'Get In Out And Left Qty's
            'Me.InOutSqlDataSource.SelectCommand = "SELECT SUM(InQty) AS [In], SUM(OutQty) AS Out, SUM(InQty) - SUM(OutQty) AS [left] FROM dbo.WaferMover GROUP BY LotEntry, [Order] HAVING (LotEntry = N'" & Session("LotNumber").ToString & "') AND ([Order] = " & Session("Step").ToString & ")"
            Dim MyDataSet As New Data.DataSet
            Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
            Dim Connection As New Data.SqlClient.SqlConnection
            Dim SQL As New Data.SqlClient.SqlCommand
            Connection.ConnectionString = Session("DBConnect")
            SQL.CommandText = "SELECT SUM(InQty) AS [In], SUM(OutQty) AS Out, SUM(InQty) - SUM(OutQty) AS [left] FROM dbo.WaferMover GROUP BY LotEntry, [Order] HAVING (LotEntry = N'" & Session("LotNumber").ToString & "') AND ([Order] = " & Session("Step").ToString & ")"
            MyAdapter.SelectCommand = SQL
            MyAdapter.SelectCommand.Connection = Connection
            Connection.Open()
            MyAdapter.Fill(MyDataSet)
            Dim QtyRow As Data.DataRow = MyDataSet.Tables(0).Rows(0)
            Me.InQtyLabel.Text = QtyRow("In").ToString
            Me.OutQtyLabel.Text = QtyRow("Out").ToString
            out = QtyRow("Out").ToString
            Me.LeftQtyLabel.Text = QtyRow("left").ToString
            Connection.Close()

            'Get Defect Qty
            'Me.DefectSumSqlDataSource.SelectCommand = "SELECT SUM(dbo.DefectTracking.Qty) AS Expr1 FROM dbo.WaferMover INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.WaferMover.LotEntry = N'" & Session("LotNumber").ToString & "') AND (dbo.WaferMover.[Order] = " & Session("Step").ToString & ")"
            SQL.CommandText = ""
            MyAdapter.Dispose()
            MyDataSet.Clear()
            SQL.CommandText = "SELECT SUM(dbo.DefectTracking.Qty) AS DefQty FROM dbo.WaferMover INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.WaferMover.LotEntry = N'" & Session("LotNumber").ToString & "') AND (dbo.WaferMover.[Order] = " & Session("Step").ToString & ")"
            MyAdapter.SelectCommand = SQL
            MyAdapter.SelectCommand.Connection = Connection
            Connection.Open()
            MyAdapter.Fill(MyDataSet)
            If MyDataSet.Tables(0).Rows.Count = 0 Then
                Me.DefectQtyLabel.Text = "0"
                defQty = 0
            Else
                QtyRow = MyDataSet.Tables(0).Rows(0)
                Me.DefectQtyLabel.Text = QtyRow("DefQty").ToString
                defQty = QtyRow("DefQty").ToString
                If Me.DefectQtyLabel.Text = "" Then
                    Me.DefectQtyLabel.Text = "0"
                    defQty = 0
                End If
            End If
            Connection.Close()

            Me.GoodQtyLabel.Text = out - defQty
        Else
            Me.DefectQtyLabel.Text = "0"
            Me.GoodQtyLabel.Text = "0"
            Me.InQtyLabel.Text = "0"
            Me.OutQtyLabel.Text = "0"
            Me.LeftQtyLabel.Text = "0"
        End If
        Me.DefectSqlDataSource.SelectCommand = "SELECT dbo.DefectTracking.DefectName, SUM(dbo.DefectTracking.Qty), dbo.T_ID_Defects.[Group] FROM dbo.WaferMover INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry INNER JOIN dbo.T_ID_Defects ON dbo.DefectTracking.DefectName = dbo.T_ID_Defects.Defect WHERE (dbo.WaferMover.LotEntry = N'" & Session("LotNumber").ToString & "') AND (dbo.WaferMover.[Order] = " & Session("Step").ToString & ") AND (dbo.T_ID_Defects.ID = '" & Session("ID").ToString & "') GROUP BY dbo.DefectTracking.DefectName, dbo.T_ID_Defects.[Group]"
        Me.SplitSqlDataSource.SelectCommand = "SELECT ChildLotNum, Qty FROM dbo.ActionTracker WHERE (ParentLotNum = N'" & Session("LotNumber").ToString & "') AND (P_Order = " & Session("Step").ToString & ") AND (Action LIKE N'Split%')"
        Me.MergedSqlDataSource.SelectCommand = "SELECT ChildLotNum, Qty FROM dbo.ActionTracker WHERE (ParentLotNum = N'" & Session("LotNumber").ToString & "') AND (P_Order = " & Session("Step").ToString & ") AND (Action LIKE N'Merge%')"
        Me.DefectsForStageSqlDataSource.SelectCommand = "SELECT dbo.T_ID_Defects.Defect, dbo.T_ID_Defects.Type, dbo.T_ID_Defects.[Group] FROM dbo.T_ID_Defects INNER JOIN dbo.DefectDefs ON dbo.T_ID_Defects.Defect = dbo.DefectDefs.DefectName WHERE (dbo.T_ID_Defects.ID = '" & Session("ID").ToString & "') AND (dbo.DefectDefs.StageName = N'" & Session("Stage").ToString & "') ORDER BY dbo.T_ID_Defects.Defect"
        Me.MovmentSqlDataSource.SelectCommand = "SELECT MIN(DISTINCT MovementEntry) AS Expr1 FROM dbo.WaferMover WHERE (LotEntry = N'" & Session("LotNumber").ToString & "') AND ([Order] = " & Session("Step").ToString & ")"
        Me.SplitIDSqlDataSource.SelectCommand = "SELECT [To] FROM dbo.TransferID_ByStage WHERE (StageName = N'" & Session("Stage").ToString & "' OR StageName = N'All') AND ([From] = N'" & Session("ID").ToString & "')"

        If Session("View") = "Yes" Or Session("View") = "NoQty" Then
            Me.GridView4.Visible = False
        End If
        SetForm()



    End Sub

   
    Protected Sub CCButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CCButton.Click
        If Me.CCButton.Text = "Make Count Adj" Then
            Me.CCButton.Text = "Enter Qty"
            Me.CCTextBox.Visible = True
            Exit Sub
        End If
        If Me.CCButton.Text = "Enter Qty" Then
            Dim CC_Qty As Integer
            If Me.CCTextBox.Text = "" Then
                Me.InfoTextBox.Text = "No Qty Entered For CC"
                Exit Sub
            End If
            CC_Qty = Me.CCTextBox.Text

            If CCQtyCheck(CC_Qty) = True Then
                Dim WaferMoverTable As New WaferMoverTableAdapter
                WaferMoverTable.InsertWaferMover(Session("LotNumber").ToString, Session("Step").ToString, CC_Qty, 0, "CC", "Inc CC", User.Identity.Name.ToString)
                WaferMoverTable.InsertWaferMover(Session("LotNumber").ToString, Session("Step").ToString - 1, 0, CC_Qty, "CC", "Inc CC", User.Identity.Name.ToString)
                ReloadMe()
            End If
        End If
    End Sub

    Protected Sub PartialEnterButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PartialEnterButton.Click
        
        MakePartial(Session("ID").ToString, Me.PartialQtyTextBox.Text)
        ReloadMe()
    End Sub
    Sub MakePartial(ByVal Partialid As String, ByVal PartialQty As String)
        Dim PStage As String = Session("Stage").ToString
        Dim WL_Chr As String = ""
        Select Case PStage
            'Case Is = "DSP"
            '   WL_Chr = "XXXX"
            Case Is = "CMP"
                WL_Chr = "XXXX"
            Case Is = "Polish"
                WL_Chr = "XXXX"
            Case Is = "Laser Inspection"
                WL_Chr = "ZZZZ"
        End Select
        If QtyCheck(PartialQty) = True Then
            Dim PartialLotNumber As String
            PartialLotNumber = Partialid & Mid(Session("LotNumber").ToString, 5, Session("LotNumber").ToString.LastIndexOf("-") - 3) & WL_Chr
            Dim ActionTable As New ActionTrackerTableAdapter
            ActionTable.InsertActionTracker(PartialLotNumber, Session("LotNumber").ToString, 2, Session("Step").ToString, PartialQty, "Split-Partial", User.Identity.Name.ToString)
            Dim WaferMoverTable As New WaferMoverTableAdapter
            'This take the qty off the parent lot
            WaferMoverTable.InsertWaferMover(Session("LotNumber").ToString, Session("Step").ToString, "-" & PartialQty, 0, "Log", "Split-Partial", User.Identity.Name.ToString)
            'Make the Record for the new XXXX lot
            WaferMoverTable.InsertWaferMover(PartialLotNumber, 0, PartialQty, 0, "Split-Partial", "Split-Partial", User.Identity.Name.ToString)
        End If
    End Sub
    Sub MergePartial(ByVal PartiallotNumber As String, ByVal PartialQty As String, ByVal SatiUser As String)
        'Merge-Partial
        Dim ActionTable As New ActionTrackerTableAdapter
        ActionTable.InsertActionTracker(PartiallotNumber, Session("LotNumber").ToString, 0, Session("Step").ToString, PartialQty, "Merge-Partial", SatiUser)

        Dim WaferMoverTable As New WaferMoverTableAdapter
        'This Adds the qty To the parent lot
        WaferMoverTable.InsertWaferMover(Session("LotNumber").ToString, Session("Step").ToString, PartialQty, 0, "Log", "Merge-Partial", SatiUser)
        'Make the Record to deduct
        WaferMoverTable.InsertWaferMover(PartiallotNumber, 0, 0, PartialQty, "Merge-Partial", "Merge-Partial", SatiUser)
        ReloadMe()

    End Sub

    Protected Sub GridView5_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView5.RowCommand
        If e.CommandName = "Merge" Then
            Dim row As String = e.CommandArgument.ToString
            Dim PartiallotNumber As String
            Dim PartialQty As String

            PartiallotNumber = Me.GridView5.Rows(row).Cells(0).Text
            PartialQty = Me.GridView5.Rows(row).Cells(1).Text
            MergePartial(PartiallotNumber, PartialQty, User.Identity.Name.ToString)
        End If
    End Sub

    Protected Sub AddPartialButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddPartialButton.Click
        Dim row As String
        row = Me.GridView5.Rows.Count
        Dim PartiallotNumber As String
        Dim PartialQty As String
        Dim i As Integer = 0
        Dim Checked As Boolean
        For i = 0 To row - 1
            Checked = CType(Me.GridView5.Rows(i).Cells(2).FindControl("CheckBox1"), CheckBox).Checked
            If Checked = True Then
                PartiallotNumber = Me.GridView5.Rows(i).Cells(0).Text
                PartialQty = Me.GridView5.Rows(i).Cells(1).Text
                MergePartial(PartiallotNumber, PartialQty, User.Identity.Name.ToString)
            End If
        Next

    End Sub
    Sub ViewAddPartials()
        Me.GridView5.Visible = True
        Me.AddPartialLabel.Visible = True
        Me.AddPartialButton.Visible = True
    End Sub

    Sub ViewMakeSplitPartials()
        Me.GridView6.Visible = True ' grid for partials
        Me.PartialLabel1.Visible = True
        Me.EnterPartialButton.Visible = True
    End Sub

    Protected Sub MakeLabelDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Button1.Text = "Make " & Me.MakeLabelDropDownList.SelectedItem.Text & " Zebra"
    End Sub

   

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim LabelID As String = Me.MakeLabelDropDownList.SelectedItem.Text
        Dim Thelotnumber As String
        Dim LabelFeedBack As String = ""
        Dim PrinterName As String = ""

        PrinterName = "\\PWI-40\" & Me.LabelPrinterDropDownList.SelectedItem.Text

        If Not LabelID = "Select ID" Then
            Thelotnumber = LabelID & Mid(Me.LotLabel.Text, Me.LotLabel.Text.IndexOf("-") + 1)
            LabelFeedBack = SatiCode.MakeLabel(False, "WB", "PWC", LabelID, Thelotnumber, Me.LabelQtyDropDownList.SelectedItem.Text, 1, 0, PrinterName, "", 0, "", "", New Data.DataSet, "W", "", User.Identity.Name.ToString, False, 0)
            Me.InfoTextBox.Text = LabelFeedBack
        Else
            Me.InfoTextBox.Text = "Select ID For Label"
        End If
    End Sub
End Class
