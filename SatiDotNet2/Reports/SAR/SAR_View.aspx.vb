
Partial Class Reports_SAR_SAR_View
    Inherits System.Web.UI.Page
    Dim Sati As New Class1

    Sub loadData()
        Dim Customer As String = Session("Customer")
        Dim ID_String As String = Session("SAR_IDs")
        Dim StartPeriod As String = ""
        Dim EndPeriod As String = ""
        Me.CustomerLabel.Text = Customer
        Me.IDLabel.Text = ID_String

        Dim Flex As New FlexCel.XlsAdapter.XlsFile(True)
        Dim Path As String = "\\PWI-40\software$\LabelTemplates\SAR_Template.xls"
        Dim Export As Boolean = False
        Dim R As Integer
        If Session("Export") = "Yes" Then
            Flex.Open(Path)
            Flex.Recalc(True)
            Flex.ActiveSheetByName = "Data"
            Export = True
            Flex.SetCellValue(3, 1, Customer)
            Flex.SetCellValue(3, 2, ID_String)
        End If


        'Build SQL ID String
        Dim Blank As String = "0000"
        Dim ID_Count As Integer = Session("SAR_ID_Count")
        Dim Main_SQL As String
        Dim i As Integer = 0

        'build ID String
        For i = 1 To 30 - ID_Count
            ID_String = ID_String & ", " & Blank
        Next

        Main_SQL = "SELECT TOP 24 ReportKey, EndInv, RecQty, IncAdjQty, ShipQty, RejQty, SplitOutQty, MergedInQty, ScrapQty FROM dbo.fctn_SAR_Ini_PopulationByID(" & ID_String & ") AS fctn_SAR_Ini_PopulationByID_1 ORDER BY ReportKey DESC"

        'Get Dataset
        Dim MyDataSet As New Data.DataSet
        Dim MyAdapter As New Data.SqlClient.SqlDataAdapter
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim SQL As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString

        'main
        SQL.CommandText = Main_SQL
        MyAdapter.SelectCommand = SQL
        MyAdapter.SelectCommand.Connection = Connection
        Connection.Open()
        MyAdapter.Fill(MyDataSet)


        Dim RowCount As Integer = MyDataSet.Tables(0).Rows.Count
        Dim Main_DR As Data.DataRow
        Main_DR = MyDataSet.Tables(0).Rows(0)
        Me.TitleLabel.Text = Me.TitleLabel.Text & " " & Main_DR("ReportKey")
        Dim c As Integer = 1
        Dim Rec As Integer = RowCount '- 1

        If Export = True Then
            For R = 0 To 11
                Main_DR = MyDataSet.Tables(0).Rows(R)
                Flex.SetCellValue(3 + R, 19, Main_DR("ReportKey"))
                Flex.SetCellValue(3 + R, 20, Main_DR("EndInv"))
                Flex.SetCellValue(3 + R, 21, Main_DR("RecQty"))
                Flex.SetCellValue(3 + R, 22, Main_DR("IncAdjQty"))
                Flex.SetCellValue(3 + R, 23, Main_DR("ShipQty"))
                Flex.SetCellValue(3 + R, 24, Main_DR("RejQty"))
                Flex.SetCellValue(3 + R, 25, Main_DR("SplitOutQty"))
                Flex.SetCellValue(3 + R, 26, Main_DR("MergedInQty"))
                Flex.SetCellValue(3 + R, 27, Main_DR("ScrapQty"))

                Flex.SetCellValue(17, 20 + R, Main_DR("ReportKey"))
                Flex.SetCellValue(18, 20 + R, Main_DR("EndInv"))
                Flex.SetCellValue(19, 20 + R, Main_DR("RecQty"))
                Flex.SetCellValue(20, 20 + R, Main_DR("IncAdjQty"))
                Flex.SetCellValue(21, 20 + R, Main_DR("ShipQty"))
                Flex.SetCellValue(22, 20 + R, Main_DR("RejQty"))
                Flex.SetCellValue(23, 20 + R, Main_DR("SplitOutQty"))
                Flex.SetCellValue(24, 20 + R, Main_DR("MergedInQty"))
                Flex.SetCellValue(25, 20 + R, Main_DR("ScrapQty"))

            Next
        End If


        '*********************************
        'month 14 and 15 for Q sum rolling
        Select Case Rec
            Case 15
                Rec = Rec - 2
            Case 14
                Rec = Rec - 1
            Case 13
                'Rec = Rec - 1
        End Select
        '*********************************

        ' end for 13th month for start
        'Main_DR = MyDataSet.Tables(0).Rows(Rec) '- 1)
        Main_DR = MyDataSet.Tables(0).Rows(12)
        Me.TABLE1.Rows(2).Cells(1).InnerText = Main_DR("EndInv")

        Dim Q As String
        Dim H As String
        Dim Y As Boolean = False
        Dim Q_Sum_Rec As Integer = 0
        Dim IncAdj As Integer = 0
        Dim Q_Sum_ship As Integer = 0
        Dim Q_Sum_Rej As Integer = 0
        Dim Q_Sum_Mer As Integer = 0
        Dim Q_Sum_Split As Integer = 0
        Dim Q_Sum_Scrap As Integer = 0
        Dim H_Sum_Rec As Integer = 0
        Dim H_IncAdj As Integer = 0
        Dim H_Sum_ship As Integer = 0
        Dim H_Sum_Rej As Integer = 0
        Dim H_Sum_Mer As Integer = 0
        Dim H_Sum_Split As Integer = 0
        Dim H_Sum_Scrap As Integer = 0
        Dim Y_Sum_Rec As Integer = 0
        Dim Y_IncAdj As Integer = 0
        Dim Y_Sum_ship As Integer = 0
        Dim Y_Sum_Rej As Integer = 0
        Dim Y_Sum_Mer As Integer = 0
        Dim Y_Sum_Split As Integer = 0
        Dim Y_Sum_Scrap As Integer = 0
        Dim Q_i As Integer
        Dim H_i As Integer
        Dim Y_i As Integer
        Dim Yeild As Double

        'For i = 1 To RowCount - 1
        For i = 1 To 12
            'Main_DR = MyDataSet.Tables(0).Rows(Rec - i) '-1
            Main_DR = MyDataSet.Tables(0).Rows(12 - i) '-1
            Q = ""
            H = ""
            Y = False
            If i = 1 Then
                StartPeriod = Main_DR("ReportKey")
            End If
            'If i = RowCount - 1 Then
            If i = 12 Then
                EndPeriod = Main_DR("ReportKey")
            End If

            Select Case Mid(Main_DR("ReportKey").ToString, Main_DR("ReportKey").ToString.IndexOf("-") + 1, 3)
                Case "-03"
                    Q = "Q1"
                Case "-06"
                    Q = "Q2"
                    H = "H1"
                Case "-09"
                    Q = "Q3"
                Case "-12"
                    Q = "Q4"
                    H = "H2"
                    Y = True
            End Select

            Me.TABLE1.Rows(1).Cells(c).InnerText = Main_DR("ReportKey")
            Me.TABLE1.Rows(3).Cells(c).InnerText = Main_DR("RecQty")
            Me.TABLE1.Rows(4).Cells(c).InnerText = Main_DR("IncAdjQty")
            Me.TABLE1.Rows(5).Cells(c).InnerText = Main_DR("ShipQty")
            Me.TABLE1.Rows(6).Cells(c).InnerText = Main_DR("RejQty")
            Me.TABLE1.Rows(7).Cells(c).InnerText = Main_DR("MergedInQty")
            Me.TABLE1.Rows(8).Cells(c).InnerText = Main_DR("SplitOutQty")
            Me.TABLE1.Rows(9).Cells(c).InnerText = Main_DR("ScrapQty")
            Me.TABLE1.Rows(10).Cells(c).InnerText = Main_DR("EndInv")
            Yeild = Main_DR("ShipQty") / (Main_DR("RejQty") + Main_DR("ShipQty"))
            Try
                Yeild = Yeild * 100
                Yeild = Decimal.Round(CType(Yeild, Decimal), 2)
                Me.TABLE1.Rows(11).Cells(c).InnerText = Unit.Percentage(Yeild).ToString
            Catch ex As Exception
                Me.TABLE1.Rows(11).Cells(c).InnerText = "N/A"
            End Try

            If Not Q = "" Then
                c = c + 1
                'mark Q
                Me.TABLE1.Rows(1).Cells(c).InnerText = Q
                Me.TABLE1.Rows(1).Cells(c).BgColor = "#E0E0E0"
                'mark H
                If Not H = "" Then
                    Me.TABLE1.Rows(1).Cells(c + 1).InnerText = H
                    Me.TABLE1.Rows(1).Cells(c + 1).BgColor = "#E0E0E0"
                End If

                If Y = True Then
                    Me.TABLE1.Rows(1).Cells(c + 2).InnerText = "Year"
                    Me.TABLE1.Rows(1).Cells(c + 2).BgColor = "#E0E0E0"
                End If

                If 12 - i > 0 Then

                    If Not H = "" Then
                        If Y = True Then
                            Me.TABLE1.Rows(2).Cells(c + 3).InnerText = Main_DR("EndInv")
                        Else
                            Me.TABLE1.Rows(2).Cells(c + 2).InnerText = Main_DR("EndInv")
                        End If
                    Else
                        Me.TABLE1.Rows(2).Cells(c + 1).InnerText = Main_DR("EndInv")
                    End If
                End If

                Q_Sum_Rec = 0
                IncAdj = 0
                Q_Sum_ship = 0
                Q_Sum_Rej = 0
                Q_Sum_Mer = 0
                Q_Sum_Split = 0
                Q_Sum_Scrap = 0
                For Q_i = 0 To 2
                    Main_DR = MyDataSet.Tables(0).Rows((12 - i) + Q_i)
                    Q_Sum_Rec = Q_Sum_Rec + Main_DR("RecQty")
                    IncAdj = IncAdj + Main_DR("IncAdjQty")
                    Q_Sum_ship = Q_Sum_ship + Main_DR("ShipQty")
                    Q_Sum_Rej = Q_Sum_Rej + Main_DR("RejQty")
                    Q_Sum_Mer = Q_Sum_Mer + Main_DR("MergedInQty")
                    Q_Sum_Split = Q_Sum_Split + Main_DR("SplitOutQty")
                    Q_Sum_Scrap = Q_Sum_Scrap + Main_DR("ScrapQty")
                    If Q_i = 0 Then
                        Me.TABLE1.Rows(10).Cells(c).InnerText = Main_DR("EndInv")
                        Me.TABLE1.Rows(10).Cells(c).BgColor = "#E0E0E0"
                    End If
                Next

                If Not H = "" Then
                    H_Sum_Rec = 0
                    H_IncAdj = 0
                    H_Sum_ship = 0
                    H_Sum_Rej = 0
                    H_Sum_Mer = 0
                    H_Sum_Split = 0
                    H_Sum_Scrap = 0
                    For H_i = 0 To 5
                        Main_DR = MyDataSet.Tables(0).Rows((12 - i) + H_i)
                        H_Sum_Rec = H_Sum_Rec + Main_DR("RecQty")
                        H_IncAdj = H_IncAdj + Main_DR("IncAdjQty")
                        H_Sum_ship = H_Sum_ship + Main_DR("ShipQty")
                        H_Sum_Rej = H_Sum_Rej + Main_DR("RejQty")
                        H_Sum_Mer = H_Sum_Mer + Main_DR("MergedInQty")
                        H_Sum_Split = H_Sum_Split + Main_DR("SplitOutQty")
                        H_Sum_Scrap = H_Sum_Scrap + Main_DR("ScrapQty")
                        If H_i = 0 Then
                            Me.TABLE1.Rows(10).Cells(c + 1).InnerText = Main_DR("EndInv")
                            Me.TABLE1.Rows(10).Cells(c + 1).BgColor = "#E0E0E0"
                        End If
                    Next
                    Me.TABLE1.Rows(3).Cells(c + 1).InnerText = H_Sum_Rec
                    Me.TABLE1.Rows(3).Cells(c + 1).BgColor = "#E0E0E0"

                    Me.TABLE1.Rows(4).Cells(c + 1).InnerText = H_IncAdj
                    Me.TABLE1.Rows(4).Cells(c + 1).BgColor = "#E0E0E0"

                    Me.TABLE1.Rows(5).Cells(c + 1).InnerText = H_Sum_ship
                    Me.TABLE1.Rows(5).Cells(c + 1).BgColor = "#E0E0E0"

                    Me.TABLE1.Rows(6).Cells(c + 1).InnerText = H_Sum_Rej
                    Me.TABLE1.Rows(6).Cells(c + 1).BgColor = "#E0E0E0"

                    Me.TABLE1.Rows(7).Cells(c + 1).InnerText = H_Sum_Mer
                    Me.TABLE1.Rows(7).Cells(c + 1).BgColor = "#E0E0E0"

                    Me.TABLE1.Rows(8).Cells(c + 1).InnerText = H_Sum_Split
                    Me.TABLE1.Rows(8).Cells(c + 1).BgColor = "#E0E0E0"

                    Me.TABLE1.Rows(9).Cells(c + 1).InnerText = H_Sum_Scrap
                    Me.TABLE1.Rows(9).Cells(c + 1).BgColor = "#E0E0E0"

                    'Main_DR = MyDataSet.Tables(0).Rows((Rec - i) + (H_i))
                    Main_DR = MyDataSet.Tables(0).Rows((12 - i) + (H_i))
                    Me.TABLE1.Rows(2).Cells(c + 1).InnerText = Main_DR("EndInv")
                    Me.TABLE1.Rows(2).Cells(c + 1).BgColor = "#E0E0E0"

                    Yeild = H_Sum_ship / (H_Sum_Rej + H_Sum_ship)
                    Me.TABLE1.Rows(11).Cells(c + 1).BgColor = "#E0E0E0"
                    Try
                        Yeild = Yeild * 100
                        Yeild = Decimal.Round(CType(Yeild, Decimal), 2)
                        Me.TABLE1.Rows(11).Cells(c + 1).InnerText = Unit.Percentage(Yeild).ToString
                    Catch ex As Exception
                        Me.TABLE1.Rows(11).Cells(c + 1).InnerText = "N/A"
                    End Try
                End If

                If Y = True Then
                    Y_Sum_Rec = 0
                    Y_IncAdj = 0
                    Y_Sum_ship = 0
                    Y_Sum_Rej = 0
                    Y_Sum_Mer = 0
                    Y_Sum_Split = 0
                    Y_Sum_Scrap = 0
                    For Y_i = 0 To 11
                        Main_DR = MyDataSet.Tables(0).Rows((12 - i) + Y_i)
                        Y_Sum_Rec = Y_Sum_Rec + Main_DR("RecQty")
                        Y_IncAdj = Y_IncAdj + Main_DR("IncAdjQty")
                        Y_Sum_ship = Y_Sum_ship + Main_DR("ShipQty")
                        Y_Sum_Rej = Y_Sum_Rej + Main_DR("RejQty")
                        Y_Sum_Mer = Y_Sum_Mer + Main_DR("MergedInQty")
                        Y_Sum_Split = Y_Sum_Split + Main_DR("SplitOutQty")
                        Y_Sum_Scrap = Y_Sum_Scrap + Main_DR("ScrapQty")
                        If Y_i = 0 Then
                            Me.TABLE1.Rows(10).Cells(c + 2).InnerText = Main_DR("EndInv")
                            Me.TABLE1.Rows(10).Cells(c + 2).BgColor = "#E0E0E0"
                        End If
                    Next
                    Me.TABLE1.Rows(3).Cells(c + 2).InnerText = Y_Sum_Rec
                    Me.TABLE1.Rows(3).Cells(c + 2).BgColor = "#E0E0E0"

                    Me.TABLE1.Rows(4).Cells(c + 2).InnerText = Y_IncAdj
                    Me.TABLE1.Rows(4).Cells(c + 2).BgColor = "#E0E0E0"

                    Me.TABLE1.Rows(5).Cells(c + 2).InnerText = Y_Sum_ship
                    Me.TABLE1.Rows(5).Cells(c + 2).BgColor = "#E0E0E0"

                    Me.TABLE1.Rows(6).Cells(c + 2).InnerText = Y_Sum_Rej
                    Me.TABLE1.Rows(6).Cells(c + 2).BgColor = "#E0E0E0"

                    Me.TABLE1.Rows(7).Cells(c + 2).InnerText = Y_Sum_Mer
                    Me.TABLE1.Rows(7).Cells(c + 2).BgColor = "#E0E0E0"

                    Me.TABLE1.Rows(8).Cells(c + 2).InnerText = Y_Sum_Split
                    Me.TABLE1.Rows(8).Cells(c + 2).BgColor = "#E0E0E0"

                    Me.TABLE1.Rows(9).Cells(c + 2).InnerText = Y_Sum_Scrap
                    Me.TABLE1.Rows(9).Cells(c + 2).BgColor = "#E0E0E0"

                    Main_DR = MyDataSet.Tables(0).Rows((12 - i) + (Y_i))
                    Me.TABLE1.Rows(2).Cells(c + 2).InnerText = Main_DR("EndInv")
                    Me.TABLE1.Rows(2).Cells(c + 2).BgColor = "#E0E0E0"

                    Yeild = Y_Sum_ship / (Y_Sum_Rej + Y_Sum_ship)
                    Me.TABLE1.Rows(11).Cells(c + 2).BgColor = "#E0E0E0"
                    Try
                        Yeild = Yeild * 100
                        Yeild = Decimal.Round(CType(Yeild, Decimal), 2)
                        Me.TABLE1.Rows(11).Cells(c + 2).InnerText = Unit.Percentage(Yeild).ToString
                    Catch ex As Exception
                        Me.TABLE1.Rows(11).Cells(c + 2).InnerText = "N/A"
                    End Try
                End If

                Me.TABLE1.Rows(3).Cells(c).InnerText = Q_Sum_Rec
                Me.TABLE1.Rows(3).Cells(c).BgColor = "#E0E0E0"

                Me.TABLE1.Rows(4).Cells(c).InnerText = IncAdj
                Me.TABLE1.Rows(4).Cells(c).BgColor = "#E0E0E0"

                Me.TABLE1.Rows(5).Cells(c).InnerText = Q_Sum_ship
                Me.TABLE1.Rows(5).Cells(c).BgColor = "#E0E0E0"

                Me.TABLE1.Rows(6).Cells(c).InnerText = Q_Sum_Rej
                Me.TABLE1.Rows(6).Cells(c).BgColor = "#E0E0E0"

                Me.TABLE1.Rows(7).Cells(c).InnerText = Q_Sum_Mer
                Me.TABLE1.Rows(7).Cells(c).BgColor = "#E0E0E0"

                Me.TABLE1.Rows(8).Cells(c).InnerText = Q_Sum_Split
                Me.TABLE1.Rows(8).Cells(c).BgColor = "#E0E0E0"

                Me.TABLE1.Rows(9).Cells(c).InnerText = Q_Sum_Scrap
                Me.TABLE1.Rows(9).Cells(c).BgColor = "#E0E0E0"

                'Main_DR = MyDataSet.Tables(0).Rows((Rec - i) + (Q_i))
                Main_DR = MyDataSet.Tables(0).Rows((12 - i) + (Q_i))
                Me.TABLE1.Rows(2).Cells(c).InnerText = Main_DR("EndInv")
                Me.TABLE1.Rows(2).Cells(c).BgColor = "#E0E0E0"

                Yeild = Q_Sum_ship / (Q_Sum_Rej + Q_Sum_ship)
                Me.TABLE1.Rows(11).Cells(c).BgColor = "#E0E0E0"
                Try
                    Yeild = Yeild * 100
                    Yeild = Decimal.Round(CType(Yeild, Decimal), 2)
                    Me.TABLE1.Rows(11).Cells(c).InnerText = Unit.Percentage(Yeild).ToString

                Catch ex As Exception
                    Me.TABLE1.Rows(11).Cells(c).InnerText = "N/A"
                End Try

            Else
                'If Not i = RowCount - 1 Then
                If Not i = 12 Then
                    'Main_DR = MyDataSet.Tables(0).Rows(Rec - i)
                    Main_DR = MyDataSet.Tables(0).Rows(12 - i)
                    Me.TABLE1.Rows(2).Cells(c + 1).InnerText = Main_DR("EndInv")
                End If

            End If
            If Y = True Then
                c = c + 1
            End If
            If Not H = "" Then
                c = c + 1
            End If
            c = c + 1

        Next
        Connection.Close()
        SQL.CommandText = ""
        MyAdapter.Dispose()
        MyDataSet.Clear()

        '*************************************************************************************************************************
        Dim DefectGroup_SQL As String
        Dim MyDataSetDefectGroup As New Data.DataSet
        Dim MyAdapterDefectGroup As New Data.SqlClient.SqlDataAdapter
        Dim SQLDefectGroup As New Data.SqlClient.SqlCommand
        'get dates
        Dim StartDate As String
        Dim EndDate As String
        StartDate = Mid(StartPeriod, StartPeriod.IndexOf("-") + 2, 2) & "/1/" & Left(StartPeriod, 4)
        EndDate = Mid(EndPeriod, EndPeriod.IndexOf("-") + 2, 2) & "/1/" & Left(EndPeriod, 4)

        'defects grouped
        'build sql
        DefectGroup_SQL = "SELECT Defect FROM dbo.fctn_SAR_Ini_PopulationByIDs_DefectsGroup('" & StartDate & "', '" & EndDate & "', " & ID_String & ") AS fctn_SAR_Ini_PopulationByIDs_DefectsGroup_1"

        SQL.CommandText = DefectGroup_SQL
        MyAdapterDefectGroup.SelectCommand = SQL
        MyAdapterDefectGroup.SelectCommand.Connection = Connection
        Connection.Open()
        MyAdapterDefectGroup.Fill(MyDataSetDefectGroup)

        Dim ii As Integer
        Dim RowCountGroup As Integer = MyDataSetDefectGroup.Tables(0).Rows.Count
        Dim DefectGroup_DR As Data.DataRow


        If Export = True Then
            For R = 0 To RowCountGroup - 1
                DefectGroup_DR = MyDataSetDefectGroup.Tables(0).Rows(R)
                Flex.SetCellValue(2, 29 + R, DefectGroup_DR("Defect"))
                Flex.SetCellValue(27 + R, 19, DefectGroup_DR("Defect"))
            Next

            'intel report defect grouping

            Flex.SetCellValue(2, 29 + RowCount + 1, "")
            Flex.SetCellValue(2, 29 + RowCount + 2, "") 'Incoming
            Flex.SetCellValue(2, 29 + RowCount + 3, "") 'Thickness
            Flex.SetCellValue(2, 29 + RowCount + 4, "") 'Warp
            Flex.SetCellValue(2, 29 + RowCount + 5, "") 'Resale (Re + Thickness)
            Flex.SetCellValue(2, 29 + RowCount + 6, "") 'Failed to strip
            Flex.SetCellValue(2, 29 + RowCount + 7, "") 'Resistivity
            Flex.SetCellValue(2, 29 + RowCount + 8, "") 'Type - N
            Flex.SetCellValue(2, 29 + RowCount + 9, "")
            Flex.SetCellValue(2, 29 + RowCount + 10, "") 'Process
            Flex.SetCellValue(2, 29 + RowCount + 11, "") 'Crack and Edge chips
            Flex.SetCellValue(2, 29 + RowCount + 12, "") 'Particle / Scratches (After reworked)
            Flex.SetCellValue(2, 29 + RowCount + 13, "") 'COP
            Flex.SetCellValue(2, 29 + RowCount + 14, "") 'Backside
            Flex.SetCellValue(2, 29 + RowCount + 15, "") 'TTV
            Flex.SetCellValue(2, 29 + RowCount + 16, "") 'Broken
            Flex.SetCellValue(2, 29 + RowCount + 17, "") 'Other



        End If

        Try
            DefectGroup_DR = MyDataSetDefectGroup.Tables(0).Rows(0)
            i = 0
            For i = 0 To RowCountGroup - 1
                DefectGroup_DR = MyDataSetDefectGroup.Tables(0).Rows(i)
                Me.TABLE1.Rows(i + 13).Cells(0).InnerText = DefectGroup_DR("Defect")
            Next
            i = 0
            For i = (RowCountGroup - 1) + 14 To 37
                Me.TABLE1.Rows.RemoveAt((RowCountGroup - 1) + 14)
            Next
            Connection.Close()
            SQL.CommandText = ""
            MyAdapterDefectGroup.Dispose()
            MyDataSetDefectGroup.Clear()


            '************************************************************************************************************************************
            ''defects qty
            Dim DefectQty_SQL As String
            Dim MyDataSetDefectQty As New Data.DataSet
            Dim MyAdapterDefectQty As New Data.SqlClient.SqlDataAdapter
            Dim SQLDefectQty As New Data.SqlClient.SqlCommand

            'build Sql
            DefectQty_SQL = "SELECT ReportKey, Defect, Qty FROM dbo.fctn_SAR_Ini_PopulationByIDs_Defects('" & StartDate & "', '" & EndDate & "', " & ID_String & ") AS fctn_SAR_Ini_PopulationByIDs_Defects_1"

            SQL.CommandText = DefectQty_SQL
            MyAdapterDefectQty.SelectCommand = SQL
            MyAdapterDefectQty.SelectCommand.Connection = Connection
            Connection.Open()
            MyAdapterDefectQty.Fill(MyDataSetDefectQty)

            Dim RowCountDefect As Integer = MyDataSetDefectQty.Tables(0).Rows.Count
            Dim Defect_DR As Data.DataRow


            If Export = True Then
                For R = 0 To RowCountDefect - 1
                    Defect_DR = MyDataSetDefectQty.Tables(0).Rows(R)

                    For RRR As Integer = 0 To 11
                        If Flex.GetCellValue(3 + RRR, 19).ToString = Defect_DR("ReportKey") Then

                            For RR As Integer = 0 To RowCountGroup - 1
                                If Flex.GetCellValue(2, 29 + RR).ToString = Defect_DR("Defect") Then
                                    Flex.SetCellValue(3 + RRR, 29 + RR, Defect_DR("Qty"))
                                    Flex.SetCellValue(27 + RR, 20 + RRR, Defect_DR("Qty"))
                                End If
                            Next
                        End If
                    Next
                Next

            End If


            Defect_DR = MyDataSetDefectQty.Tables(0).Rows(0)
            Dim Col As Integer
            Dim drop As Boolean = False
            i = 0
            For i = 0 To RowCountDefect - 1
                Defect_DR = MyDataSetDefectQty.Tables(0).Rows(i)
                drop = False
                For Col = 1 To 19 '16
                    If Me.TABLE1.Rows(1).Cells(Col).InnerText = Defect_DR("ReportKey") Then
                        For ii = 0 To RowCountGroup - 1
                            If Me.TABLE1.Rows(ii + 13).Cells(0).InnerText = Defect_DR("Defect") Then
                                Me.TABLE1.Rows(ii + 13).Cells(Col).InnerText = Defect_DR("Qty")
                                drop = True
                            End If
                            If drop = True Then
                                Exit For
                            End If
                        Next
                    End If
                    If drop = True Then
                        Exit For
                    End If
                Next
            Next
            Connection.Close()
            SQL.CommandText = ""
            MyAdapterDefectQty.Dispose()
            MyDataSetDefectQty.Clear()

        Catch ex As Exception
            i = 0
            For i = (RowCountGroup - 1) + 14 To 37
                Me.TABLE1.Rows.RemoveAt((RowCountGroup - 1) + 14)
            Next
            Connection.Close()
            SQL.CommandText = ""
            MyAdapterDefectGroup.Dispose()
            MyDataSetDefectGroup.Clear()
        End Try

        Me.ShippedSqlDataSource.SelectCommand = "SELECT TOP 15 eventtime AS Date, PackingSlip AS [Packing Slip], Qty, PartNumber AS [Part Number] FROM dbo.fctn_SAR_Ini_PopulationByIDs_Shipped(" & ID_String & ") AS fctn_SAR_Ini_PopulationByIDs_Shipped_1"
        Me.RecSqlDataSource.SelectCommand = "SELECT TOP 15 eventtime, Qty FROM dbo.fctn_SAR_Ini_PopulationByIDs_Received(" & ID_String & ") AS fctn_SAR_Ini_PopulationByIDs_Received_1"
        Me.EndSqlDataSource.SelectCommand = "SELECT SUM(WH + WIP + FGI) AS [End Inv], WH AS [WH Inv], WIP AS [WIP Inv], FGI FROM dbo.fctn_SAR_Ini_PopulationByIDs_End(" & ID_String & ") AS fctn_SAR_Ini_PopulationByIDs_End_1 GROUP BY WH, WIP, FGI"

        If Export = True Then
            Dim DR_E As Data.DataRow
            Dim DataSet_E As New Data.DataSet
            Dim Adapter_E As New Data.SqlClient.SqlDataAdapter
            Dim SqlCommand_E As New Data.SqlClient.SqlCommand
            Dim RC_E As Integer

            Connection.Open()

            SqlCommand_E.CommandText = Me.ShippedSqlDataSource.SelectCommand.ToString

            Adapter_E.SelectCommand = SqlCommand_E
            Adapter_E.SelectCommand.Connection = Connection
            Adapter_E.Fill(DataSet_E)
            RC_E = DataSet_E.Tables(0).Rows.Count

            For R = 0 To RC_E - 1
                DR_E = DataSet_E.Tables(0).Rows(R)
                Flex.SetCellValue(3 + R, 9, DR_E("Date"))
                Flex.SetCellValue(3 + R, 10, DR_E("Packing Slip"))
                Flex.SetCellValue(3 + R, 11, DR_E("Qty"))
                Flex.SetCellValue(3 + R, 12, DR_E("Part Number"))
            Next

            DataSet_E.Clear()
            SqlCommand_E.CommandText = Me.RecSqlDataSource.SelectCommand.ToString

            Adapter_E.SelectCommand = SqlCommand_E
            Adapter_E.SelectCommand.Connection = Connection
            Adapter_E.Fill(DataSet_E)
            RC_E = DataSet_E.Tables(0).Rows.Count

            For R = 0 To RC_E - 1
                DR_E = DataSet_E.Tables(0).Rows(R)
                Flex.SetCellValue(3 + R, 6, DR_E("eventtime"))
                Flex.SetCellValue(3 + R, 7, DR_E("Qty"))
            Next

            DataSet_E.Clear()
            SqlCommand_E.CommandText = Me.EndSqlDataSource.SelectCommand.ToString

            Adapter_E.SelectCommand = SqlCommand_E
            Adapter_E.SelectCommand.Connection = Connection
            Adapter_E.Fill(DataSet_E)
            RC_E = DataSet_E.Tables(0).Rows.Count

            For R = 0 To RC_E - 1
                DR_E = DataSet_E.Tables(0).Rows(R)
                Flex.SetCellValue(3 + R, 14, DR_E("End Inv"))
                Flex.SetCellValue(3 + R, 15, DR_E("WH Inv"))
                Flex.SetCellValue(3 + R, 16, DR_E("WIP Inv"))
                Flex.SetCellValue(3 + R, 17, DR_E("FGI"))
            Next
            Connection.Close()

        End If

        'Make zeros in the defects area


        Dim data As String
        For i = 13 To (RowCountGroup - 1) + 13
            For ii = 1 To c - 1
                If Me.TABLE1.Rows(1).Cells(ii).InnerText.ToString.Contains("-") Then
                    data = ""
                    data = Me.TABLE1.Rows(i).Cells(ii).InnerText.ToString()
                    If data.Contains(" ") Then
                        Me.TABLE1.Rows(i).Cells(ii).InnerText = "0"
                    End If
                End If
            Next
        Next
        ' get Size
        Dim Size_SQL As String
        Dim MyDataSetSize As New Data.DataSet
        Dim MyAdapterSize As New Data.SqlClient.SqlDataAdapter
        Dim SQLSize As New Data.SqlClient.SqlCommand

        'build Sql
        Size_SQL = "SELECT dia FROM dbo.fctn_SAR_Ini_PopulationSize(" & ID_String & ") AS fctn_SAR_Ini_PopulationSize_1"

        SQL.CommandText = Size_SQL
        MyAdapterSize.SelectCommand = SQL
        MyAdapterSize.SelectCommand.Connection = Connection
        Connection.Open()
        MyAdapterSize.Fill(MyDataSetSize)

        Dim RowCountSize As Integer = MyDataSetSize.Tables(0).Rows.Count
        Dim Size_DR As Data.DataRow
        Size_DR = MyDataSetSize.Tables(0).Rows(0)
        i = 0
        Me.SizeLabel.Text = ""
        For i = 1 To RowCountSize
            Size_DR = MyDataSetSize.Tables(0).Rows(i - 1)
            If i = 1 Then
                Me.SizeLabel.Text = Size_DR("dia") & "mm"
            Else
                Me.SizeLabel.Text = Me.SizeLabel.Text & ", " & Size_DR("dia") & "mm"
            End If
        Next

        If Export = True Then
            For R = 0 To RowCountSize - 1
                Size_DR = MyDataSetSize.Tables(0).Rows(R)
                Flex.SetCellValue(3 + R, 3, Size_DR("dia"))
            Next
        End If

        Connection.Close()
        SQL.CommandText = ""
        MyAdapterSize.Dispose()
        MyDataSetSize.Clear()


        ' get Part
        Dim Part_SQL As String
        Dim MyDataSetPart As New Data.DataSet
        Dim MyAdapterPart As New Data.SqlClient.SqlDataAdapter
        Dim SQLPart As New Data.SqlClient.SqlCommand

        'build Sql
        Part_SQL = "SELECT Part FROM dbo.fctn_SAR_Ini_PopulationPartNumber('" & DateTime.Now & "', " & ID_String & ") AS fctn_SAR_Ini_PopulationPartNumber_1"

        SQL.CommandText = Part_SQL
        MyAdapterPart.SelectCommand = SQL
        MyAdapterPart.SelectCommand.Connection = Connection
        Connection.Open()
        MyAdapterPart.Fill(MyDataSetPart)

        Dim RowCountPart As Integer = MyDataSetPart.Tables(0).Rows.Count
        Dim Part_DR As Data.DataRow
        Part_DR = MyDataSetPart.Tables(0).Rows(0)
        i = 0
        Me.PartLabel.Text = ""
        For i = 1 To RowCountPart
            Part_DR = MyDataSetPart.Tables(0).Rows(i - 1)
            If i = 1 Then
                Me.PartLabel.Text = Part_DR("Part")
            Else
                Me.PartLabel.Text = Me.PartLabel.Text & ", " & Part_DR("Part")
            End If
        Next

        If Export = True Then
            For R = 0 To RowCountPart - 1
                Part_DR = MyDataSetPart.Tables(0).Rows(R)
                Flex.SetCellValue(3 + R, 4, Part_DR("Part"))
            Next
        End If

        Connection.Close()
        SQL.CommandText = ""
        MyAdapterPart.Dispose()
        MyDataSetPart.Clear()

        If Export = True Then
            Dim SavePath As String = "\\PWI-40\software$\LabelTemplates\LabelArchive\SARsCompile\" & "Test" & ".xls"

            Flex.Save(SavePath)
            'R:\LabelTemplates\LabelArchive\SARsCompile
            'Sati.SendMailWithFile("SARs Report", "Report", Session("EmailAddress"), SavePath)

        End If



    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        loadData()
    End Sub

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad

    End Sub
End Class
