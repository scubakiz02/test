Imports System.IO

Partial Class Reports_SurfScan
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1
    Function findPassedBin() As Int16
        If Me.RadioButtonPassBins2.Checked = True Then
            Return 2
            Exit Function
        End If

        If Me.RadioButtonPassBins3.Checked = True Then
            Return 3
            Exit Function
        End If

        If Me.RadioButtonPassBinBoth2_3.Checked = True Then
            Return 23
            Exit Function
        End If
    End Function

    Sub UpdateData()
        Dim BuildSQL As String = ""
        Dim Tool As String = ""
        Dim Multi As Boolean = False
        Dim Surfscan As String = ""
        Dim DBtable As String

        If CheckBoxArchive.Checked = True Then
            DBtable = "dbo.Archive_SP1_Data "
        Else
            DBtable = "dbo.SP1_Data "
        End If

        If Me.SPxRadioButton.Checked = True Then
            Surfscan = "SPx"
        Else
            Surfscan = "Tencor"
        End If


        If Me.CheckBoxDaily.Checked = True Then
            '(SPSessionName LIKE N'%Daily%')
            BuildSQL = "SELECT TOP (100) PERCENT SessionDate, Machine, Comment1, Comment2, SPSessionName, ID# + N'-' + RUN# + N'-' + Wafer_log AS Lot, COUNT(DestinationStationID) AS [Wafers In], SUM(CASE WHEN DestinationStationID = 2 THEN 1 ELSE 0 END) AS Passed, SUM(CASE WHEN DispositionName = 'Rejected' THEN 1 WHEN DispositionName = 'Overload' THEN 1 ELSE 0 END) AS Rejects, SUM(CASE WHEN DispositionName = 'Overload' THEN 1 ELSE 0 END) AS OverLoads, SUM(CASE WHEN DestinationStationID = 2 THEN 1 ELSE 0 END) AS Bin2, SUM(CASE WHEN DestinationStationID = 3 THEN 1 ELSE 0 END) AS Bin3 FROM " & DBtable & "GROUP BY Machine, SPSessionName, SessionDate, ID# + N'-' + RUN# + N'-' + Wafer_log, Comment1, Comment2 HAVING (Machine = N'SP1' OR Machine = N'SP2' OR Machine = N'SP1-3' OR Machine = N'SP2-S0132' OR Machine = N'SP3-2110224') AND (SessionDate > CONVERT(DATETIME, '" & DateAdd("d", -90, DateTime.Now.ToShortDateString) & " 00:00:00', 102)) AND (SessionDate < CONVERT(DATETIME, '" & DateTime.Now.ToShortDateString & " 23:59:59', 102)) AND (SPSessionName LIKE N'%Daily%') ORDER BY SessionDate DESC"
            Me.Sp1SqlDataSource.SelectCommand = BuildSQL
            Me.GridView1.DataBind()
            FooterSum()
            Exit Sub
        End If


        BuildSQL = "SELECT "


        If Not Me.RadioButtonSelectDate.Checked Then
            BuildSQL = BuildSQL & "TOP "
        End If

        If Me.RadioButton10.Checked = True Then
            BuildSQL = BuildSQL & "10"
        End If
        If Me.RadioButton25.Checked = True Then
            BuildSQL = BuildSQL & "25"
        End If
        If Me.RadioButton50.Checked = True Then
            BuildSQL = BuildSQL & "50"
        End If
        If Me.RadioButton75.Checked = True Then
            BuildSQL = BuildSQL & "75"
        End If


        If Surfscan = "SPx" Then
            'BuildSQL = BuildSQL & " SessionDate, Machine, Comment1, Comment2, SPSessionName, ID# + N'-' + RUN# + N'-' + Wafer_log AS Lot, COUNT(DestinationStationID) AS [Wafers In], SUM(CASE WHEN DispositionName = 'Rejected' THEN 0 WHEN DispositionName = 'Overload' THEN 0 WHEN DispositionName = 'RW' THEN 0 WHEN DispositionName = 'Rerun' THEN 0 ELSE 1 END) AS Passed, SUM(CASE WHEN DispositionName = 'Rejected' THEN 1 WHEN DispositionName = 'Overload' THEN 1 ELSE 0 END) AS Rejects, SUM(CASE WHEN DispositionName = 'Overload' THEN 1 ELSE 0 END) AS OverLoads, SUM(CASE WHEN DestinationStationID = 2 THEN 1 ELSE 0 END) AS Bin2, SUM(CASE WHEN DestinationStationID = 3 THEN 1 ELSE 0 END) AS Bin3 "
            BuildSQL = BuildSQL & " SessionDate, Machine, Comment1, Comment2, SPSessionName, ID# + N'-' + RUN# + N'-' + Wafer_log AS Lot, COUNT(DestinationStationID) AS [Wafers In], SUM("
            Select Case findPassedBin()
                Case 2
                    BuildSQL = BuildSQL & "CASE WHEN DestinationStationID = 2 THEN 1 ELSE 0 END"
                Case 3
                    BuildSQL = BuildSQL & "CASE WHEN DestinationStationID = 3 THEN 1 ELSE 0 END"
                Case 23
                    BuildSQL = BuildSQL & "CASE WHEN DestinationStationID = 2 THEN 1 WHEN DestinationStationID = 3 THEN 1 ELSE 0 END"
                Case Else
                    BuildSQL = BuildSQL & "CASE WHEN DispositionName = 'Rejected' THEN 0 WHEN DispositionName = 'Overload' THEN 0 WHEN DispositionName = 'RW' THEN 0 WHEN DispositionName = 'Rerun' THEN 0 ELSE 1 END"
            End Select

            BuildSQL = BuildSQL & ") AS Passed, SUM(CASE WHEN DispositionName = 'Rejected' THEN 1 WHEN DispositionName = 'Overload' THEN 1 ELSE 0 END) AS Rejects, SUM(CASE WHEN DispositionName = 'Overload' THEN 1 ELSE 0 END) AS OverLoads, SUM(CASE WHEN DestinationStationID = 2 THEN 1 ELSE 0 END) AS Bin2, SUM(CASE WHEN DestinationStationID = 3 THEN 1 ELSE 0 END) AS Bin3 "

            BuildSQL = BuildSQL & "FROM " & DBtable '"FROM dbo.SP1_Data "



            Dim MultiWhere As Boolean = False

            'Slot*******************
            If S1CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'1') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'1') "
                End If
            End If

            If S2CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'2') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'2') "
                End If
            End If

            If S3CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'3') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'3') "
                End If
            End If

            If S4CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'4') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'4') "
                End If
            End If

            If S5CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'5') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'5') "
                End If
            End If

            If S6CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'6') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'6') "
                End If
            End If

            If S7CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'7') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'7') "
                End If
            End If

            If S8CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'8') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'8') "
                End If
            End If

            If S9CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'9') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'9') "
                End If
            End If

            If S10CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'10') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'10') "
                End If
            End If

            If S11CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'11') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'11') "
                End If
            End If

            If S12CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'12') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'12') "
                End If
            End If

            If S13CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'13') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'13') "
                End If
            End If

            If S14CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'14') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'14') "
                End If
            End If

            If S15CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'15') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'15') "
                End If
            End If

            If S16CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'16') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'16') "
                End If
            End If

            If S17CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'17') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'17') "
                End If
            End If

            If S18CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'18') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'18') "
                End If
            End If

            If S19CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'19') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'19') "
                End If
            End If

            If S20CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'20') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'20') "
                End If
            End If

            If S21CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'21') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'21') "
                End If
            End If

            If S22CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'22') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'22') "
                End If
            End If

            If S23CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'23') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'23') "
                End If
            End If

            If S24CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'24') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'24') "
                End If
            End If

            If S25CheckBox.Checked = True Then
                If MultiWhere = False Then
                    BuildSQL = BuildSQL & "WHERE (SourceSlotID = N'25') "
                    MultiWhere = True
                Else
                    BuildSQL = BuildSQL & "OR (SourceSlotID = N'25') "
                End If
            End If

            ' End Slot*****************************************************






            BuildSQL = BuildSQL & "GROUP BY Machine, SPSessionName, SessionDate, ID#, RUN#, Wafer_log, ID# + N'-' + RUN# + N'-' + Wafer_log, Comment1, Comment2, WaferDia "
            BuildSQL = BuildSQL & "HAVING (Machine = N'"

            '(Machine = N'Sp1' OR Machine = N 'SP2')
            ' Tools To Check
            'SP1
            'SP1-2
            'SP2
            'SP2-2

            If Me.SP1CheckBox.Checked = True Then
                BuildSQL = BuildSQL & "SP1"
                Multi = True
            End If

            If Me.SP12CheckBox.Checked = True Then
                If Multi = True Then
                    BuildSQL = BuildSQL & "' OR Machine = N'SP2"
                Else
                    BuildSQL = BuildSQL & "SP2"
                    Multi = True
                End If

            End If

            If Me.SP13CheckBox.Checked = True Then
                If Multi = True Then
                    BuildSQL = BuildSQL & "' OR Machine = N'SP1-3"
                Else
                    BuildSQL = BuildSQL & "SP1-3"
                    Multi = True
                End If
            End If

            If Me.SP2CheckBox.Checked = True Then
                If Multi = True Then
                    BuildSQL = BuildSQL & "' OR Machine = N'SP2-S0132"
                Else
                    BuildSQL = BuildSQL & "SP2-S0132"
                    Multi = True
                End If
            End If

            '**************SP22 Add ************
            '***********************************
            'SP2-2080166R
            If Me.SP3CheckBox.Checked = True Then
                If Multi = True Then
                    BuildSQL = BuildSQL & "' OR Machine = N'SP3-2110224"
                Else
                    BuildSQL = BuildSQL & "SP3-2110224"
                    Multi = True
                End If
            End If
            '***********************************
            '***********************************


            BuildSQL = BuildSQL & "') AND (NOT (ID# = N'move')) AND (NOT (RUN# = N'99999')) AND (NOT (Wafer_log = N'99999'))"


            If Me.CMPCheckBox.Checked = True Then

                Select Case Me.DropDownListCMP.SelectedValue
                    Case "CMP 1"
                        BuildSQL = BuildSQL & "AND (Comment1 LIKE N'%-1') "

                    Case "CMP 2"
                        BuildSQL = BuildSQL & "AND (Comment1 LIKE N'%-2') "

                    Case "CMP 3"
                        BuildSQL = BuildSQL & "AND (Comment1 LIKE N'%-3') "

                    Case "CMP 4L"
                        BuildSQL = BuildSQL & "AND (Comment1 LIKE N'%-4L') "

                    Case "CMP 4R"
                        BuildSQL = BuildSQL & "AND (Comment1 LIKE N'%-4R') "

                    Case "CMP 5"
                        BuildSQL = BuildSQL & "AND (Comment1 LIKE N'%-5') "


                End Select

                'If Me.CMP1RadioButton.Checked = True Then
                '    BuildSQL = BuildSQL & "AND (Comment1 LIKE N'%-1') "
                'End If

                'If Me.CMP2RadioButton.Checked = True Then
                '    BuildSQL = BuildSQL & "AND (Comment1 LIKE N'%-2') "
                'End If

                'If Me.CMP3RadioButton.Checked = True Then
                '    BuildSQL = BuildSQL & "AND (Comment1 LIKE N'%-3') "
                'End If

            End If


            If Me.Comment1ActivateCheckBox.Checked = True Then
                If Me.Comment1FindRadioButton.Checked = True Then
                    BuildSQL = BuildSQL & " AND (Comment1 = N'" & Me.Comment1TextBox.Text & "')"
                Else
                    BuildSQL = BuildSQL & " AND (NOT (Comment1 = N'" & Me.Comment1TextBox.Text & "'))"
                End If
            End If

            If Me.Comment2ActivateCheckBox.Checked = True Then
                If Me.Comment2FindRadioButton.Checked = True Then
                    BuildSQL = BuildSQL & " AND (Comment2 = N'" & Me.Comment2TextBox.Text & "')"
                Else
                    BuildSQL = BuildSQL & " AND (NOT (Comment2 = N'" & Me.Comment2TextBox.Text & "'))"
                End If
            End If


        Else
            BuildSQL = BuildSQL & " MAX(EventTime) AS [Run Time], MACHINE AS Tencor, RECIPE, OPERATOR, ID# + N'-' + RUN# + N'-' + WAFER_LOG AS Lot, COUNT(WAFER#) AS Wafers, SUM(CASE WHEN Sort = 'Pass' THEN 1 ELSE 0 END) AS Passed, SUM(CASE WHEN Sort = 'FAIL' THEN 1 ELSE 0 END) AS Reject FROM dbo.Tencor_Data "
            BuildSQL = BuildSQL & "WHERE (MACHINE = N'"

            If Me.TencorCheckBox.Checked = True Then
                BuildSQL = BuildSQL & "Tencor"
                Multi = True
            End If
            If Me.Tencor3CheckBox.Checked = True Then
                If Multi = True Then
                    BuildSQL = BuildSQL & "' OR Machine = N'Tencor 3"
                Else
                    BuildSQL = BuildSQL & "Tencor 3"
                    Multi = True
                End If

            End If
            If Me.Tencor4CheckBox.Checked = True Then
                If Multi = True Then
                    BuildSQL = BuildSQL & "' OR Machine = N'Tencor 4"
                Else
                    BuildSQL = BuildSQL & "Tencor 4"
                End If
            End If
            BuildSQL = BuildSQL & "')"
        End If

        If Me.AdvancedPanel.Visible = True Then

            If Me.IDActivateCheckBox.Checked = True Then
                If Me.IDFindRadioButton.Checked = True Then
                    BuildSQL = BuildSQL & " AND (ID# = N'" & Me.IDTextBox.Text & "') "
                Else
                    BuildSQL = BuildSQL & " AND (NOT (ID# = N'" & Me.IDTextBox.Text & "'))"
                End If
            End If

            If Me.RunActivateCheckBox.Checked = True Then
                If Me.RunFindRadioButton.Checked = True Then
                    BuildSQL = BuildSQL & " AND (RUN# = N'" & Me.RunTextBox.Text & "')"
                Else
                    BuildSQL = BuildSQL & " AND (NOT (RUN# = N'" & Me.RunTextBox.Text & "'))"
                End If
            End If

            If Me.WLActivateCheckBox.Checked = True Then
                If Me.WLFindRadioButton.Checked = True Then
                    BuildSQL = BuildSQL & " AND (Wafer_log = N'" & Me.WLTextBox.Text & "')"
                Else
                    BuildSQL = BuildSQL & " AND (NOT (Wafer_log = N'" & Me.WLTextBox.Text & "'))"
                End If
            End If

            If Me.SessionActivateCheckBox.Checked = True Then
                If Me.SessionFindRadioButton.Checked = True Then
                    BuildSQL = BuildSQL & " AND (SPSessionName = N'" & Me.SessionTextBox.Text & "')"
                Else
                    BuildSQL = BuildSQL & " AND (NOT (SPSessionName = N'" & Me.SessionTextBox.Text & "'))"
                End If
            End If

            If Me.CheckBoxRemoveDaily.Checked = True Then
                BuildSQL = BuildSQL & " AND (NOT (SPSessionName LIKE N'%Daily%'))"
            End If

        End If


        If Me.DropDownListDiameter.SelectedValue = "200mm" Then
            BuildSQL = BuildSQL & " AND (WaferDia = 200000)"
        End If

        If Me.DropDownListDiameter.SelectedValue = "300mm" Then
            BuildSQL = BuildSQL & " AND (WaferDia = 300000)"
        End If

        If Surfscan = "SPx" Then

            If Me.RadioButtonSelectDate.Checked Then ' if date range
                BuildSQL = BuildSQL & " AND (SessionDate > CONVERT(DATETIME, '" & Me.TextBoxStartDate.Text & " 00:00:00', 102) AND SessionDate < CONVERT(DATETIME, '" & Me.TextBoxEndDate.Text & " 23:59:59', 102))"
            End If

            BuildSQL = BuildSQL & " ORDER BY SessionDate DESC"
            Me.Sp1SqlDataSource.SelectCommand = BuildSQL
            Me.GridView1.DataBind()
            FooterSum()
        Else
            BuildSQL = BuildSQL & " GROUP BY RECIPE, OPERATOR, ID# + N'-' + RUN# + N'-' + WAFER_LOG, MACHINE ORDER BY MAX(EventTime) DESC"
            Me.TencorSqlDataSource.SelectCommand = BuildSQL
            Me.GridView2.DataBind()
            FooterSum()
        End If


    End Sub


    Sub FooterSum()
        If Me.SPxRadioButton.Checked = True Then
            Dim Rows As Integer = Me.GridView1.Rows.Count
            If Rows = 0 Then
                Exit Sub
            End If

            Dim QtyIn As Integer = 0
            Dim Passed As Integer = 0
            Dim Percentpassed As Double = 0.0
            Dim Rejects As Integer = 0
            Dim OverLoad As Integer = 0
            Dim Rework As Integer = 0
            Dim ReRuns As Integer = 0

            Dim i As Integer = 0
            If Me.FooterSumCheckBox.Checked = True Then
                For i = 0 To Rows - 1
                    QtyIn = QtyIn + CType(Me.GridView1.Rows(i).Cells(5).Text, Integer)
                    Passed = Passed + CType(Me.GridView1.Rows(i).Cells(6).Text, Integer)
                    Percentpassed = Percentpassed + CType(Me.GridView1.Rows(i).Cells(6).Text, Integer) / CType(Me.GridView1.Rows(i).Cells(5).Text, Integer)
                    Rejects = Rejects + CType(Me.GridView1.Rows(i).Cells(8).Text, Integer)
                    OverLoad = OverLoad + CType(Me.GridView1.Rows(i).Cells(9).Text, Integer)
                    Rework = Rework + CType(Me.GridView1.Rows(i).Cells(10).Text, Integer)
                    ReRuns = ReRuns + CType(Me.GridView1.Rows(i).Cells(11).Text, Integer)
                Next
                Me.GridView1.FooterRow.Cells(5).Text = QtyIn
                Me.GridView1.FooterRow.Cells(6).Text = Passed
                'Me.GridView1.FooterRow.Cells(7).Text = Format(Percentpassed / Rows, "0.0%")
                Me.GridView1.FooterRow.Cells(7).Text = Format(Passed / QtyIn, "0.0%")
                Me.GridView1.FooterRow.Cells(8).Text = Rejects
                Me.GridView1.FooterRow.Cells(9).Text = OverLoad
                Me.GridView1.FooterRow.Cells(10).Text = Rework
                Me.GridView1.FooterRow.Cells(11).Text = ReRuns
            Else
                Me.GridView1.FooterRow.Cells(5).Text = "QtyIn"
                Me.GridView1.FooterRow.Cells(6).Text = "Passed"
                Me.GridView1.FooterRow.Cells(7).Text = "%Passed"
                Me.GridView1.FooterRow.Cells(8).Text = "Rejects"
                Me.GridView1.FooterRow.Cells(9).Text = "OverLoads"
                Me.GridView1.FooterRow.Cells(10).Text = "Rework"
                Me.GridView1.FooterRow.Cells(11).Text = "ReRuns"
            End If
        Else
            Dim Rows As Integer = Me.GridView2.Rows.Count
            If Rows = 0 Then
                Exit Sub
            End If

            Dim Wafers As Integer = 0
            Dim Passed As Integer = 0
            Dim Percentpassed As Integer = 0
            Dim Rejects As Integer = 0

            Dim i As Integer = 0
            If Me.FooterSumCheckBox.Checked = True Then
                For i = 0 To Rows - 1
                    Wafers = Wafers + CType(Me.GridView2.Rows(i).Cells(6).Text, Integer)
                    Passed = Passed + CType(Me.GridView2.Rows(i).Cells(7).Text, Integer)
                    Rejects = Rejects + CType(Me.GridView2.Rows(i).Cells(8).Text, Integer)
                    
                Next
                Me.GridView2.FooterRow.Cells(6).Text = Wafers
                Me.GridView2.FooterRow.Cells(7).Text = Passed
                Me.GridView2.FooterRow.Cells(8).Text = Rejects
                Me.GridView2.FooterRow.Cells(9).Text = Format(Passed / Wafers, "0.0%")

            Else
                Me.GridView2.FooterRow.Cells(6).Text = "Wafers"
                Me.GridView2.FooterRow.Cells(7).Text = "Passed"
                Me.GridView2.FooterRow.Cells(8).Text = "Rejects"
                Me.GridView2.FooterRow.Cells(9).Text = "%Passed"

            End If
        End If

    End Sub

    Protected Sub AdvancedCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.AdvancedCheckBox.Checked = True Then
            Me.AdvancedPanel.Visible = True
        Else
            Me.AdvancedPanel.Visible = False
            UpdateData()
        End If
    End Sub

    Sub ChangeToolSet()
        If Me.SPxRadioButton.Checked = True Then

            Me.SP1CheckBox.Visible = True
            Me.SP12CheckBox.Visible = True
            Me.SP13CheckBox.Visible = True '
            Me.SP2CheckBox.Visible = True
            Me.SP3CheckBox.Visible = True
            Me.GridView1.Visible = True '

            Me.TencorCheckBox.Visible = False
            Me.Tencor3CheckBox.Visible = False
            Me.Tencor4CheckBox.Visible = False
            Me.GridView2.Visible = False

        Else

            Me.SP1CheckBox.Visible = False
            Me.SP12CheckBox.Visible = False
            Me.SP13CheckBox.Visible = False '
            Me.SP2CheckBox.Visible = False
            Me.SP3CheckBox.Visible = False
            Me.GridView1.Visible = False

            Me.TencorCheckBox.Visible = True
            Me.Tencor3CheckBox.Visible = True
            Me.Tencor4CheckBox.Visible = True
            Me.GridView2.Visible = True

        End If
    End Sub

    Protected Sub SP1CheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateData()
    End Sub

    Protected Sub SP12CheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateData()
    End Sub

    Protected Sub SP2CheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateData()
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateData()
    End Sub

    Protected Sub LotFindRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateData()
    End Sub

    Protected Sub IDTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.IDActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub IDNotRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateData()
    End Sub

    Protected Sub IDActivateCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateData()
    End Sub

    Protected Sub RunTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.RunActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub RunFindRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.RunActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub RunNotRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.RunActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub RunActivateCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateData()
    End Sub

    Protected Sub WLTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.WLActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub WLFindRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.WLActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub WLNotRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.WLActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub WLActivateCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateData()
    End Sub

    Protected Sub SessionTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.SessionActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub SessionFindRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.SessionActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub SessionNotRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.SessionActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub SessionActivateCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateData()
    End Sub

    Protected Sub SP1UpdateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Saticode.UpdateSPxTool("SP1")
        UpdateData()
    End Sub

    Protected Sub SP12UpdateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Saticode.UpdateSPxTool("SP2")
        UpdateData()
    End Sub

    Protected Sub FooterSumCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FooterSumCheckBox.CheckedChanged
        FooterSum()
    End Sub

    Protected Sub TencorRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ChangeToolSet()
    End Sub

    Protected Sub SPxRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ChangeToolSet()
    End Sub

    Protected Sub TencorCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateData()
    End Sub

    Protected Sub Tencor3CheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateData()
    End Sub

    Protected Sub Tencor4CheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateData()
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "Session" Then
            Dim row As String = e.CommandArgument.ToString
            getbucket(row)
        End If

    End Sub

    Sub getbucket(ByVal Row As String)
        Me.HiddenField_Row.Value = Row
        Dim SessionDate As String = Me.GridView1.Rows(Row).Cells(1).Text
        Dim Comment2 As String = Me.GridView1.Rows(Row).Cells(2).Text
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Try
            DS = Saticode.GetMyDataSet("SELECT Scan, Timestamp FROM dbo.T_Spec_Scan_Log WHERE (Scan = N'" & Me.GridView1.Rows(Row).Cells(2).Text & "')")
            DR = DS.Tables(0).Rows(0)
            'Spec Time xxxxxx , 
            Me.LabelSpecTime.Text = "Spec Load Time " & DR("Timestamp") & " , "
        Catch ex As Exception

        End Try
        'AND (Comment2 = N'370046')

        Me.LabelSessionDate.Text = "Surf Session Time: " & SessionDate & "  "
        Me.LabelMapSessionName.Text = "Session Name: " & SessionDate & "  "
        'Me.SPxDetailSqlDataSource.SelectCommand = "SELECT SourceSlotID AS [From], DestinationStationID AS [To], DestinationSlotID AS [To Slot], DispositionName AS Class, BinCnt1, BinCnt2, BinCnt3, BinCnt4, BinCnt5, BinCnt6, BinCnt7, BinCnt8, ScratchCnt AS SC, ClusterAreaCnt AS CAC, Map FROM dbo.SP1_Data WHERE (SessionDate = CONVERT (DATETIME, '" & SessionDate & "', 102)) AND (Comment2 = N'" & Comment2 & "') ORDER BY SourceSlotID"
        Me.SPxDetailSqlDataSource.SelectCommand = "SELECT SourceSlotID AS [From], DestinationStationID AS [To], DestinationSlotID AS [To Slot], DispositionName AS Class, SOD1, SOD2, SOD3, SOD4, SOD5, SOD6, SOD7, SOD8, ScratchCnt AS SC, ClusterAreaCnt AS CAC, Map FROM dbo.SP1_Data WHERE (SessionDate = CONVERT (DATETIME, '" & SessionDate & "', 102)) AND (Comment2 = N'" & Comment2 & "') ORDER BY SourceSlotID"
        Me.GridView3.DataBind()
        Me.SPxModalPopupExtender.Show()

        'SELECT ID_NUMBER, LPD_G1, LPD_G2, LPD_G3, LPD_G4, First_Bin, Second_Bin, Third_Bin, Forth_Bin FROM dbo.CofA_Info WHERE (ID_NUMBER = (SELECT ID# FROM AutoData.dbo.SP1_Data WHERE (SessionDate = CONVERT(DATETIME, '2021-12-02 11:11:42', 102)) AND (Comment2 = N'487751') GROUP BY ID#))

        DS.Clear()

        DS = Saticode.GetMyDataSet("SELECT ID_NUMBER, LPD_G1, LPD_G2, LPD_G3, LPD_G4, First_Bin, Second_Bin, Third_Bin, Forth_Bin FROM dbo.CofA_Info WHERE (ID_NUMBER = (SELECT ID# FROM AutoData.dbo.SP1_Data WHERE (SessionDate = CONVERT(DATETIME, '" & SessionDate & "', 102)) AND (Comment2 = N'" & Comment2 & "') GROUP BY ID#))")


        If Not DS.Tables(0).Rows.Count = 0 Then
            DR = DS.Tables(0).Rows(0)
            Try
                Me.GridView3.HeaderRow.Cells(3 + DR("First_Bin")).Text = DR("LPD_G1")
                Me.GridView3.HeaderRow.Cells(3 + DR("First_Bin")).ForeColor = Drawing.Color.Aqua

                Me.GridView3.HeaderRow.Cells(3 + DR("Second_Bin")).Text = DR("LPD_G2")
                Me.GridView3.HeaderRow.Cells(3 + DR("Second_Bin")).ForeColor = Drawing.Color.Aqua

                Me.GridView3.HeaderRow.Cells(3 + DR("Third_Bin")).Text = DR("LPD_G3")
                Me.GridView3.HeaderRow.Cells(3 + DR("Third_Bin")).ForeColor = Drawing.Color.Aqua

                Me.GridView3.HeaderRow.Cells(3 + DR("Forth_Bin")).Text = DR("LPD_G4")
                Me.GridView3.HeaderRow.Cells(3 + DR("Forth_Bin")).ForeColor = Drawing.Color.Aqua
            Catch ex As Exception

            End Try



        End If





    End Sub

    Protected Sub GridView2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView2.RowCommand
        If e.CommandName = "Session" Then
            Dim row As String = e.CommandArgument.ToString
            Dim RECIPE As String = Me.GridView2.Rows(row).Cells(3).Text
            Dim OP As String = Me.GridView2.Rows(row).Cells(4).Text
            Dim Lot As String = Me.GridView2.Rows(row).Cells(5).Text
            Dim mainID As String = Left(Lot, Lot.IndexOf("-"))
            Dim RUN As String = Mid(Lot, (Lot.IndexOf("-") + 2), Lot.LastIndexOf("-") - (Lot.IndexOf("-") + 1))
            Dim WAFER_LOG As String = Right(Lot, Lot.Length - (Lot.LastIndexOf("-") + 1))
            Dim MACHINE As String = Me.GridView2.Rows(row).Cells(2).Text

            Me.TencorDetailSqlDataSource.SelectCommand = "SELECT EventTime, WAFER# AS Slot, SORT, LPD_COUNT AS [LPD Count], [130_BIN] AS [130], [160_BIN] AS [160], [200_BIN] AS [200], [250_BIN] AS [250], [300_BIN] AS [300], [500_BIN] AS [500], [1000_BIN] AS [1000], SCRATCH_COUNT AS SC, AREA_COUNT AS AC FROM dbo.Tencor_Data WHERE (RECIPE = N'" & RECIPE & "') AND (OPERATOR = N'" & OP & "') AND (ID# = N'" & mainID & "') AND (RUN# = N'" & RUN & "') AND (WAFER_LOG = N'" & WAFER_LOG & "') AND (MACHINE = N'" & MACHINE & "')"

            Me.TencorModalPopupExtender.Show()
            Me.GridView4.DataBind()
        End If
    End Sub

    Protected Sub CMPCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateData()
    End Sub

    Protected Sub CMP1RadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.CMPCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub CMP2RadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.CMPCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub CMP3RadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.CMPCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub GridView3_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView3.RowCommand
        If e.CommandName = "Map" Then
            Dim MapFile As String = e.CommandArgument.ToString

            '*********************************************************
            '*********************************************************
            If MapFile.Contains("Z:\") Then
                MapFile = Session("SP2Files") & Mid(MapFile, 3)
            Else
                MapFile = Session("SatiMapsDir") & MapFile
            End If
            '*********************************************************
            '*********************************************************

            Me.MapImage.ImageUrl = MapFile
            Me.MapImage.DataBind()
            Me.MapModalPopupExtender.Show()
        End If

        If e.CommandName = "NewMap" Then
            'Dim row As String = e.CommandArgument.ToString
            Me.MapRowLabel.Text = e.CommandArgument.ToString
            LookMap(Me.MapRowLabel.Text, "C")
            
            Me.MapModalPopupExtender.Show()
        End If
    End Sub

    Protected Sub MapCloseButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.SPxModalPopupExtender.Show()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)

        Me.TencorDetailSqlDataSource.SelectCommand = ""
        Me.TencorSqlDataSource.SelectCommand = ""
        Me.SPxDetailSqlDataSource.SelectCommand = ""
        Me.Sp1SqlDataSource.SelectCommand = ""
    End Sub

    Sub LookMap(ByVal Row As Int16, ByVal Action As String)


        Dim MapFile As String = ""
        Select Case Action
            Case "B"
                If Row > 0 Then
                    MapFile = CType(Me.GridView3.Rows(Row - 1).Cells(14).FindControl("MapfileLabel"), Label).Text
                    Me.MapRowLabel.Text = Row - 1
                    Me.CSlotLabel.Text = "Looking @ Slot: " & Me.GridView3.Rows(Row - 1).Cells(0).Text
                End If
            Case "C"
                MapFile = CType(Me.GridView3.Rows(Row).Cells(14).FindControl("MapfileLabel"), Label).Text
                Me.MapRowLabel.Text = Row
                Me.CSlotLabel.Text = "Looking @ Slot: " & Me.GridView3.Rows(Row).Cells(0).Text
            Case "N"
                Dim RowCount As Int16 = Me.GridView3.Rows.Count
                If Row < RowCount - 1 Then
                    MapFile = CType(Me.GridView3.Rows(Row).Cells(14).FindControl("MapfileLabel"), Label).Text
                    Me.MapRowLabel.Text = Row + 1
                    Me.CSlotLabel.Text = "Looking @ Slot: " & Me.GridView3.Rows(Row).Cells(0).Text
                End If
        End Select

        '*********************************************************
        '*********************************************************
        If MapFile.Contains("Z:\") Then
            MapFile = Session("SP2Files") & Mid(MapFile, 3)
        Else
            MapFile = Session("SatiMapsDir") & MapFile
        End If
        '*********************************************************
        '*********************************************************


        'Me.CSlotLabel.Text = "Looking @ Slot: " & Me.GridView3.Rows(Row).Cells(0).Text 'me.MapRowLabel.Text + 1
       
        Me.MapImage.ImageUrl = MapFile
        Me.MapImage.DataBind()

    End Sub

    Protected Sub BackMapButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        LookMap(Me.MapRowLabel.Text, "B")
        Me.MapModalPopupExtender.Show()
    End Sub



    Protected Sub ButtonMapBackSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonMapBackSession.Click
        If Not HiddenField_Row.Value = "0" Then
            getbucket(Me.HiddenField_Row.Value - 1)
            LookMap(Me.MapRowLabel.Text, "C")
            Me.MapModalPopupExtender.Show()
        End If
    End Sub

    Protected Sub ButtonMapNextSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonMapNextSession.Click
        If Not HiddenField_Row.Value = Me.GridView1.Rows.Count - 1 Then
            getbucket(Me.HiddenField_Row.Value + 1)
            LookMap(Me.MapRowLabel.Text, "C")
            Me.MapModalPopupExtender.Show()
        End If
    End Sub


    Protected Sub Comment1TextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Comment1TextBox.TextChanged
        If Me.Comment1ActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub Comment2TextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Comment2TextBox.TextChanged
        If Me.Comment2ActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub Comment1FindRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Comment1FindRadioButton.CheckedChanged
        If Me.Comment1ActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub Comment1NotRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Comment1NotRadioButton.CheckedChanged
        If Me.Comment1ActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub Comment1ActivateCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Comment1ActivateCheckBox.CheckedChanged
        UpdateData()
    End Sub

    Protected Sub Comment2FindRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Comment2FindRadioButton.CheckedChanged
        If Me.Comment2ActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub Comment2NotRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Comment2NotRadioButton.CheckedChanged
        If Me.Comment2ActivateCheckBox.Checked = True Then
            UpdateData()
        End If
    End Sub

    Protected Sub Comment2ActivateCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Comment2ActivateCheckBox.CheckedChanged
        UpdateData()
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub ButtonAllSlots_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAllSlots.Click
        Me.S1CheckBox.Checked = True
        Me.S2CheckBox.Checked = True
        Me.S3CheckBox.Checked = True
        Me.S4CheckBox.Checked = True
        Me.S5CheckBox.Checked = True
        Me.S6CheckBox.Checked = True
        Me.S7CheckBox.Checked = True
        Me.S8CheckBox.Checked = True
        Me.S9CheckBox.Checked = True
        Me.S10CheckBox.Checked = True
        Me.S11CheckBox.Checked = True
        Me.S12CheckBox.Checked = True
        Me.S13CheckBox.Checked = True
        Me.S14CheckBox.Checked = True
        Me.S15CheckBox.Checked = True
        Me.S16CheckBox.Checked = True
        Me.S17CheckBox.Checked = True
        Me.S18CheckBox.Checked = True
        Me.S19CheckBox.Checked = True
        Me.S20CheckBox.Checked = True
        Me.S21CheckBox.Checked = True
        Me.S22CheckBox.Checked = True
        Me.S23CheckBox.Checked = True
        Me.S24CheckBox.Checked = True
        Me.S25CheckBox.Checked = True

    End Sub

    Protected Sub ButtonNoSlots_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonNoSlots.Click
        Me.S1CheckBox.Checked = False
        Me.S2CheckBox.Checked = False
        Me.S3CheckBox.Checked = False
        Me.S4CheckBox.Checked = False
        Me.S5CheckBox.Checked = False
        Me.S6CheckBox.Checked = False
        Me.S7CheckBox.Checked = False
        Me.S8CheckBox.Checked = False
        Me.S9CheckBox.Checked = False
        Me.S10CheckBox.Checked = False
        Me.S11CheckBox.Checked = False
        Me.S12CheckBox.Checked = False
        Me.S13CheckBox.Checked = False
        Me.S14CheckBox.Checked = False
        Me.S15CheckBox.Checked = False
        Me.S16CheckBox.Checked = False
        Me.S17CheckBox.Checked = False
        Me.S18CheckBox.Checked = False
        Me.S19CheckBox.Checked = False
        Me.S20CheckBox.Checked = False
        Me.S21CheckBox.Checked = False
        Me.S22CheckBox.Checked = False
        Me.S23CheckBox.Checked = False
        Me.S24CheckBox.Checked = False
        Me.S25CheckBox.Checked = False
    End Sub

    Protected Sub ButtonRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRefresh.Click
        UpdateData()
    End Sub

    Protected Sub ButtonBackSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonBackSession.Click
        If Not HiddenField_Row.Value = "0" Then
            getbucket(Me.HiddenField_Row.Value - 1)
        End If
    End Sub

    Protected Sub ButtonNextSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonNextSession.Click
        If Not HiddenField_Row.Value = Me.GridView1.Rows.Count - 1 Then
            getbucket(Me.HiddenField_Row.Value + 1)
        End If
    End Sub


    Protected Sub SP2CheckBox_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles SP2CheckBox.CheckedChanged
        UpdateData()
    End Sub

    Protected Sub RadioButtonPassBins2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonPassBins2.CheckedChanged
        UpdateData()
    End Sub

    Protected Sub RadioButtonPassBins3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonPassBins3.CheckedChanged
        UpdateData()
    End Sub

    Protected Sub RadioButtonPassBinBoth2_3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonPassBinBoth2_3.CheckedChanged
        UpdateData()
    End Sub

    Protected Sub SP3CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles SP3CheckBox.CheckedChanged
        UpdateData()
    End Sub
    Protected Sub ButtonNextMap_Click(sender As Object, e As EventArgs) Handles ButtonNextMap.Click
        'LookMap(Me.MapRowLabel.Text, "N")
        Dim MapFile As String = ""
        MapFile = CType(Me.GridView3.Rows(Me.MapRowLabel.Text).Cells(14).FindControl("MapfileLabel"), Label).Text
        Me.CSlotLabel.Text = "Looking @ Slot: " & Me.GridView3.Rows(Me.MapRowLabel.Text).Cells(0).Text
        Me.MapRowLabel.Text = Me.MapRowLabel.Text + 1

        '*********************************************************
        '*********************************************************
        If MapFile.Contains("Z:\") Then
            MapFile = Session("SP2Files") & Mid(MapFile, 3)
        Else
            MapFile = Session("SatiMapsDir") & MapFile
        End If
        '*********************************************************
        '*********************************************************

        Me.MapImage.ImageUrl = MapFile
        Me.MapImage.DataBind()
        Me.MapModalPopupExtender.Show()
    End Sub
    Protected Sub SP13CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles SP13CheckBox.CheckedChanged
        UpdateData()
    End Sub
    Protected Sub SP13UpdateButton_Click(sender As Object, e As EventArgs) Handles SP13UpdateButton.Click
        Saticode.UpdateSPxTool("SP13")
    End Sub

    Protected Sub DropDownListCMP_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListCMP.SelectedIndexChanged
        UpdateData()
    End Sub

    Protected Sub RadioButton10_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        DateSelectionShow(False)
        UpdateData()
    End Sub

    Protected Sub RadioButton25_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        DateSelectionShow(False)
        UpdateData()
    End Sub

    Protected Sub RadioButton50_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        DateSelectionShow(False)
        UpdateData()
    End Sub

    Protected Sub RadioButton75_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        DateSelectionShow(False)
        UpdateData()
    End Sub

    Protected Sub RadioButtonSelectDate_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonSelectDate.CheckedChanged
        DateSelectionShow(True)
        EvalDates()
    End Sub

    Function DateSelectionShow(Show As Boolean)
        If Show = True Then
            Me.LabelStart.Visible = True
            Me.TextBoxStartDate.Visible = True
            Me.LabelEnd.Visible = True
            Me.TextBoxEndDate.Visible = True
        Else
            Me.LabelStart.Visible = False
            Me.TextBoxStartDate.Visible = False
            Me.LabelEnd.Visible = False
            Me.TextBoxEndDate.Visible = False
        End If
    End Function

    Sub EvalDates()
        If Not TextBoxStartDate.Text = "" And Not Me.TextBoxEndDate.Text = "" Then
            UpdateData()
        End If
    End Sub

    Protected Sub TextBoxStartDate_TextChanged(sender As Object, e As EventArgs) Handles TextBoxStartDate.TextChanged
        EvalDates()
    End Sub

    Protected Sub TextBoxEndDate_TextChanged(sender As Object, e As EventArgs) Handles TextBoxEndDate.TextChanged
        EvalDates()
    End Sub

    Protected Sub CheckBoxDaily_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxDaily.CheckedChanged
        If Me.CheckBoxDaily.Checked = True Then
            Me.CheckBoxDaily.BackColor = Drawing.Color.LawnGreen

        Else
            Me.CheckBoxDaily.BackColor = Drawing.Color.White

        End If

        UpdateData()




    End Sub

    Protected Sub CheckBoxRemoveDaily_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxRemoveDaily.CheckedChanged
        UpdateData()
    End Sub

    Protected Sub CheckBoxArchive_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxArchive.CheckedChanged
        UpdateData()
    End Sub

    Private Sub DropDownListDiameter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListDiameter.SelectedIndexChanged
        UpdateData()
    End Sub

    Protected Sub ButtonExport_Click(sender As Object, e As EventArgs) Handles ButtonExport.Click
        If TextBoxE.Text = "" Then
            Exit Sub
        Else
            If GridView1.Rows.Count > 1 Then
                ExportData()
            End If
        End If
    End Sub

    Sub ExportData()

        Dim FILE_NAME As String = "\\PWI-40\TempImageWebFiles$\" & "BinFallExport " & User.Identity.Name.ToString & ".csv"
        Using sw As StreamWriter = File.CreateText(FILE_NAME)
            For I As Integer = 1 To Me.GridView1.HeaderRow.Cells.Count - 1
                sw.Write(Me.GridView1.HeaderRow.Cells(I).Text & ",")
            Next
            sw.WriteLine()
            For R As Integer = 0 To Me.GridView1.Rows.Count - 1
                For D As Integer = 1 To Me.GridView1.Columns.Count - 1
                    If Not Me.GridView1.Rows(R).Cells(D).Text = "" Then
                        sw.Write(Me.GridView1.Rows(R).Cells(D).Text & ",")
                    Else
                        Try
                            sw.Write(CType(Me.GridView1.Rows(R).Cells(D).FindControl("Label1"), Label).Text & ",")
                        Catch ex As Exception
                            sw.Write(Me.GridView1.Rows(R).Cells(D).Text & ",")
                        End Try
                    End If
                    'sw.Write(Me.GridView1.Rows(R).Cells(D).Text & ",")

                Next
                sw.WriteLine()
            Next
            For I = 1 To Me.GridView1.FooterRow.Cells.Count - 1
                sw.Write(Me.GridView1.FooterRow.Cells(I).Text & ",")
            Next


            sw.Close()
        End Using

        Saticode.SendMailWithFile("Bin Fall Report", "Sati.Net Export", Me.TextBoxE.Text & "@purewafer.com", FILE_NAME)

    End Sub

End Class
