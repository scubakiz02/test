Imports System.Windows.Forms.DataVisualization.Charting

Partial Class SPC_SPC_View
    Inherits System.Web.UI.Page

    Dim SatiCode As New Class1
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub
    Protected Sub DropDownListTool_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListTool.SelectedIndexChanged
        Try
            For Clear As Int16 = 1 To 16
                Me.UpdatePanel1.FindControl("Panel" & Clear).Visible = False
            Next
        Catch ex As Exception

        End Try

        Dim ICP As Boolean = False

        If Me.DropDownListTool.SelectedItem.Text = "Select..." Then
            'clear stuff

        Else
            Dim Records As String = "1000"
            Dim MySQL As String = "SELECT TOP (100) PERCENT dbo.T_SPC_Tool_Info.Tool_Name, dbo.T_SPC_DataPoints.TimeStamp, dbo.T_SPC_DataPoints.DSC, dbo.T_SPC_DataPoints.Op, dbo.T_SPC_DataPoints.Seq, dbo.T_SPC_DataPoints.Name, dbo.T_SPC_DataPoints.Para, dbo.T_SPC_DataPoints.LCL, dbo.T_SPC_DataPoints.Value, dbo.T_SPC_DataPoints.UCL, dbo.T_SPC_DataPoints.DataPoints FROM dbo.T_SPC_DataPoints INNER JOIN dbo.T_SPC_Tool_Info ON dbo.T_SPC_DataPoints.Tool_Key = dbo.T_SPC_Tool_Info.[Key] WHERE (dbo.T_SPC_Tool_Info.Tool_Name = N'" & Me.DropDownListTool.SelectedItem.Text & "') AND (dbo.T_SPC_DataPoints.DSC IN (SELECT TOP (" & Records & ") T_SPC_DataPoints_1.DSC FROM dbo.T_SPC_Tool_Info AS T_SPC_Tool_Info_1 INNER JOIN dbo.T_SPC_DataPoints AS T_SPC_DataPoints_1 ON T_SPC_Tool_Info_1.[Key] = T_SPC_DataPoints_1.Tool_Key GROUP BY T_SPC_Tool_Info_1.Tool_Name, T_SPC_DataPoints_1.DSC, T_SPC_DataPoints_1.TimeStamp HAVING (T_SPC_Tool_Info_1.Tool_Name = N'" & Me.DropDownListTool.SelectedItem.Text & "') ORDER BY T_SPC_DataPoints_1.TimeStamp DESC)) ORDER BY dbo.T_SPC_DataPoints.Seq, dbo.T_SPC_DataPoints.Para, dbo.T_SPC_DataPoints.TimeStamp DESC"
            Dim DS As New Data.DataSet
            Dim DR As Data.DataRow

            If Me.DropDownListTool.SelectedItem.Text = "ICP-MS 8900" Then
                ICP = True
            End If

            ' MySQL = "SELECT TOP (100) PERCENT dbo.T_SPC_Tool_Info.Tool_Name, dbo.T_SPC_DataPoints.TimeStamp, dbo.T_SPC_DataPoints.DSC, dbo.T_SPC_DataPoints.Op, dbo.T_SPC_DataPoints.Seq, dbo.T_SPC_DataPoints.Name, dbo.T_SPC_DataPoints.Para, dbo.T_SPC_DataPoints.LCL, dbo.T_SPC_DataPoints.Value, dbo.T_SPC_DataPoints.UCL, dbo.T_SPC_DataPoints.DataPoints FROM dbo.T_SPC_DataPoints INNER JOIN dbo.T_SPC_Tool_Info ON dbo.T_SPC_DataPoints.Tool_Key = dbo.T_SPC_Tool_Info.[Key] WHERE (dbo.T_SPC_Tool_Info.Tool_Name = N'" & Me.DropDownListTool.SelectedItem.Text & "') AND (dbo.T_SPC_DataPoints.DSC IN (SELECT TOP (" & Records & ") T_SPC_DataPoints_1.DSC FROM dbo.T_SPC_Tool_Info AS T_SPC_Tool_Info_1 INNER JOIN dbo.T_SPC_DataPoints AS T_SPC_DataPoints_1 ON T_SPC_Tool_Info_1.[Key] = T_SPC_DataPoints_1.Tool_Key GROUP BY T_SPC_Tool_Info_1.Tool_Name, T_SPC_DataPoints_1.DSC, T_SPC_DataPoints_1.TimeStamp HAVING (T_SPC_Tool_Info_1.Tool_Name = N'" & Me.DropDownListTool.SelectedItem.Text & "') ORDER BY T_SPC_DataPoints_1.TimeStamp DESC)) ORDER BY dbo.T_SPC_DataPoints.Seq, dbo.T_SPC_DataPoints.Para, dbo.T_SPC_DataPoints.TimeStamp DESC"

            DS = SatiCode.GetMyDataSetSPCData(MySQL)


            If ICP = False Then

                If DS.Tables(0).Rows.Count > 0 Then
                    DR = DS.Tables(0).Rows(0)
                    Dim Name As String = DR("Name")
                    Dim Para As String = DR("Para")
                    Dim U As Double = DR("UCL")
                    Dim L As Double = DR("LCL")
                    Me.Panel1.Visible = True


                    Dim ChartIndex As Int16 = 1
                    Dim ChartIndexSub As String = ""
                    Dim DS_New As New Data.DataSet
                    Dim DR_New As Data.DataRow

                    DS_New.Tables.Add("MyData")
                    DS_New.Tables("MyData").Columns.Add("DSC")
                    DS_New.Tables("MyData").Columns.Add("LCL")
                    DS_New.Tables("MyData").Columns.Add("Value")
                    DS_New.Tables("MyData").Columns.Add("UCL")


                    If DR("Para") = "AVG" Then
                        ChartIndexSub = "A"
                    Else
                        ChartIndexSub = "B"
                    End If
                    CType(Me.UpdatePanel1.FindControl("Label" & ChartIndex), Label).Text = Name

                    'Me.Chart1A.Series("SeriesLCL").MarkerSize = 10
                    'Me.Chart1A.Series("SeriesLCL").BorderWidth = 10


                    '*********** Dive *****************
                    '*********** Dive *****************
                    '*********** Dive *****************
                    For I As Integer = 0 To DS.Tables(0).Rows.Count - 1
                        DR = DS.Tables(0).Rows(I)

                        If Not Para = DR("Para") Then
                            'AVG or StDev

                            Try

                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("LCL").XValueMember = "DSC"
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("LCL").YValueMembers = "LCL"
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("LCL").BorderWidth = 5

                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("Value").XValueMember = "DSC"
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("Value").YValueMembers = "Value"
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("Value").BorderWidth = 5

                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("UCL").XValueMember = "DSC"
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("UCL").YValueMembers = "UCL"
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("UCL").BorderWidth = 5



                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisY.Minimum = MinMax(DS_New, "L")
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisY.Maximum = MinMax(DS_New, "U")
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisX.MajorTickMark.Interval = 1
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisX.MajorGrid.Interval = 1
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisX.Interval = 1


                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).DataSource = DS_New
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).DataBind()
                            Catch ex As Exception

                            End Try

                            Para = DR("Para")
                            If DR("Para") = "AVG" Then
                                ChartIndexSub = "A"
                            Else
                                ChartIndexSub = "B"
                            End If

                            DS_New.Tables(0).Rows.Clear()
                            U = DR("UCL")
                            L = DR("LCL")
                        End If

                        If Not Name = DR("Name") Then
                            'new panel
                            Name = DR("Name")

                            ChartIndex = ChartIndex + 1
                            Try
                                Me.UpdatePanel1.FindControl("Panel" & ChartIndex).Visible = True
                                CType(Me.UpdatePanel1.FindControl("Label" & ChartIndex), Label).Text = Name
                                'Label1.Text = ""
                            Catch ex As Exception

                            End Try

                        End If
                        DR_New = DS_New.Tables("MyData").NewRow
                        DR_New("DSC") = DR("DSC")
                        DR_New("LCL") = DR("LCL")
                        DR_New("Value") = DR("Value")
                        DR_New("UCL") = DR("UCL")
                        DS_New.Tables("MyData").Rows.Add(DR_New)


                    Next

                    'do the last
                    If DR("Para") = "AVG" Then
                        ChartIndexSub = "A"
                    Else
                        ChartIndexSub = "B"
                    End If
                    Try
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("LCL").XValueMember = "DSC"
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("LCL").YValueMembers = "LCL"
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("LCL").BorderWidth = 5

                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("Value").XValueMember = "DSC"
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("Value").YValueMembers = "Value"
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("Value").BorderWidth = 5

                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("UCL").XValueMember = "DSC"
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("UCL").YValueMembers = "UCL"
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("UCL").BorderWidth = 5



                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisY.Minimum = MinMax(DS_New, "L")
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisY.Maximum = MinMax(DS_New, "U")
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisX.MajorTickMark.Interval = 1
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisX.MajorGrid.Interval = 1
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisX.Interval = 1

                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).DataSource = DS_New
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).DataBind()
                    Catch ex As Exception

                    End Try

                End If


            Else

                If DS.Tables(0).Rows.Count > 0 Then
                    DR = DS.Tables(0).Rows(0)
                    Dim Name As String = DR("Name")
                    Dim Para As String = DR("Para")
                    Dim U As Double = DR("UCL")
                    Dim L As Double = DR("LCL")
                    Me.Panel1.Visible = True


                    Dim ChartIndex As Int16 = 1
                    Dim ChartIndexSub As String = ""
                    Dim DS_New As New Data.DataSet
                    Dim DR_New As Data.DataRow

                    DS_New.Tables.Add("MyData")
                    DS_New.Tables("MyData").Columns.Add("DSC")
                    DS_New.Tables("MyData").Columns.Add("LCL")
                    DS_New.Tables("MyData").Columns.Add("Value")
                    DS_New.Tables("MyData").Columns.Add("UCL")


                    If DR("Para") = "AVG" Then
                        ChartIndexSub = "A"
                    Else
                        ChartIndexSub = "B"
                    End If
                    CType(Me.UpdatePanel1.FindControl("Label" & ChartIndex), Label).Text = Name

                    'Me.Chart1A.Series("SeriesLCL").MarkerSize = 10
                    'Me.Chart1A.Series("SeriesLCL").BorderWidth = 10


                    '*********** Dive *****************
                    '*********** Dive *****************
                    '*********** Dive *****************
                    For I As Integer = 0 To DS.Tables(0).Rows.Count - 1
                        DR = DS.Tables(0).Rows(I)

                        If Not Name = DR("Name") Then
                            'AVG or StDev

                            Try

                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("LCL").XValueMember = "DSC"
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("LCL").YValueMembers = "LCL"
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("LCL").BorderWidth = 5

                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("Value").XValueMember = "DSC"
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("Value").YValueMembers = "Value"
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("Value").BorderWidth = 5

                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("UCL").XValueMember = "DSC"
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("UCL").YValueMembers = "UCL"
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("UCL").BorderWidth = 5



                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisY.Minimum = MinMax(DS_New, "L")
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisY.Maximum = MinMax(DS_New, "U")
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisX.MajorTickMark.Interval = 1
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisX.MajorGrid.Interval = 1
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisX.Interval = 1


                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).DataSource = DS_New
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).DataBind()

                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & "B"), DataVisualization.Charting.Chart).Visible = False
                                CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Titles(0).Text = "QC"

                            Catch ex As Exception

                            End Try

                            Para = DR("Para")
                            If DR("Para") = "AVG" Then
                                ChartIndexSub = "A"
                            Else
                                ChartIndexSub = "B"
                            End If

                            DS_New.Tables(0).Rows.Clear()
                            U = DR("UCL")
                            L = DR("LCL")
                        End If

                        If Not Name = DR("Name") Then
                            'new panel
                            Name = DR("Name")

                            ChartIndex = ChartIndex + 1
                            Try
                                Me.UpdatePanel1.FindControl("Panel" & ChartIndex).Visible = True
                                CType(Me.UpdatePanel1.FindControl("Label" & ChartIndex), Label).Text = Name
                                'Label1.Text = ""
                            Catch ex As Exception

                            End Try

                        End If
                        DR_New = DS_New.Tables("MyData").NewRow
                        DR_New("DSC") = DR("DSC")
                        DR_New("LCL") = DR("LCL")
                        DR_New("Value") = DR("Value")
                        DR_New("UCL") = DR("UCL")
                        DS_New.Tables("MyData").Rows.Add(DR_New)


                    Next

                    'do the last
                    'If DR("Name") = "Name" Then
                    '    ChartIndexSub = "A"
                    'Else
                    '    ChartIndexSub = "B"
                    'End If
                    Try
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("LCL").XValueMember = "DSC"
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("LCL").YValueMembers = "LCL"
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("LCL").BorderWidth = 5

                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("Value").XValueMember = "DSC"
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("Value").YValueMembers = "Value"
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("Value").BorderWidth = 5

                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("UCL").XValueMember = "DSC"
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("UCL").YValueMembers = "UCL"
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Series("UCL").BorderWidth = 5



                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisY.Minimum = MinMax(DS_New, "L")
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisY.Maximum = MinMax(DS_New, "U")
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisX.MajorTickMark.Interval = 1
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisX.MajorGrid.Interval = 1
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).ChartAreas(0).AxisX.Interval = 1

                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).DataSource = DS_New
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).DataBind()

                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & "B"), DataVisualization.Charting.Chart).Visible = False
                        CType(Me.UpdatePanel1.FindControl("Chart" & ChartIndex & ChartIndexSub), DataVisualization.Charting.Chart).Titles(0).Text = "QC"
                    Catch ex As Exception

                    End Try

                End If


            End If



            End If






        ' End If
    End Sub

    Function MinMax(ds As Data.DataSet, UorL As String) As Double
        Dim U As Double
        Dim L As Double

        Dim dr As Data.DataRow
        dr = ds.Tables(0).Rows(0)
        U = dr("UCL")
        L = dr("LCL")

        For i As Int16 = 0 To ds.Tables(0).Rows.Count - 1
            dr = ds.Tables(0).Rows(i)
            If dr("Value") > U Then
                U = dr("Value")
            End If
            If dr("Value") < L Then
                L = dr("Value")
            End If
        Next

        Select Case UorL
            Case "U"
                Return U
            Case "L"
                Return L
            Case Else
                Return 555
        End Select

    End Function


End Class
