
Partial Class PC_FGICartonDetail
    Inherits System.Web.UI.Page

    Protected Sub CartonTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        SetData()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        SetData()
    End Sub

    Protected Sub CheckBox1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        FilerColumns()
    End Sub

    Protected Sub CheckBox2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        FilerColumns()
    End Sub

    Protected Sub CheckBox3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        FilerColumns()
    End Sub

    Protected Sub CheckBox4_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        FilerColumns()
    End Sub

    Sub SetData()
        Dim DataInput As String
        DataInput = Me.CartonTextBox.Text
        'If DataInput.Contains("WB") Or DataInput.Contains("CB") Then
        'DataInput = Mid(DataInput, 3)
        'End If
        Try

            If DataInput.Contains("CB") Then
                DataInput = Mid(DataInput, 3)
                Me.CartonDetailSqlDataSource.SelectCommand = "SELECT TOP 100 PERCENT dbo.T_FGI_Boxes.CartonNumber, dbo.T_FGI_Boxes.BoxInvNumber AS WaferBoxNumber, dbo.LabelsMade.Lot, dbo.T7_InstanceInfo.Slot, dbo.T7_WaferActionTracking.T7, dbo.MainIDSpec.thk_grp AS SpecThick, dbo.T7_GeoData.CenterThick, dbo.MainIDSpec.res_grp AS SpecRes, dbo.T7_GeoData.CenterRes, dbo.MainIDSpec.WTYPE_DOPE AS SpecType, dbo.T7_GeoData.Type, dbo.PROCESS_INFO.BOW AS SpecBow, dbo.T7_GeoData.Bow, dbo.PROCESS_INFO.WARP AS SpecWarp, dbo.T7_GeoData.TotWarp AS Warp, dbo.PROCESS_INFO.FINAL_TTV AS SpecTTV, dbo.T7_GeoData.TTV, dbo.CofA_Info.First_Bin, dbo.PROCESS_INFO.PARTICLE_SPEC_1 AS First_BinSpec, dbo.CofA_Info.Second_Bin, dbo.PROCESS_INFO.PARTICLE_SPEC_2 AS Second_BinSpec, dbo.CofA_Info.Third_Bin, dbo.PROCESS_INFO.PARTICLE_SPEC_3 AS Third_BinBin, dbo.CofA_Info.Forth_Bin, dbo.PROCESS_INFO.PARTICLE_SPEC_4 AS Forth_BinSpec, dbo.T7_ParticalData.SP1BinCnt1 AS Bin1, dbo.T7_ParticalData.SP1BinCnt2 AS Bin2, dbo.T7_ParticalData.SP1BinCnt3 AS Bin3, dbo.T7_ParticalData.SP1BinCnt4 AS Bin4, dbo.T7_ParticalData.SP1BinCnt5 AS Bin5, dbo.T7_ParticalData.SP1BinCnt6 AS Bin6, dbo.T7_ParticalData.SP1BinCnt7 AS Bin7, dbo.T7_ParticalData.SP1BinCnt8 AS Bin8 FROM dbo.T_FGI_Boxes LEFT OUTER JOIN dbo.T7_ParticalData INNER JOIN dbo.T7_GeoData INNER JOIN dbo.T7_WaferActionTracking INNER JOIN dbo.T7_InstanceInfo ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key ON dbo.T7_GeoData.Geo_Key = dbo.T7_WaferActionTracking.PostGeo_Key ON dbo.T7_ParticalData.Partical_Key = dbo.T7_WaferActionTracking.Partical_Key ON dbo.T_FGI_Boxes.InstanceKey = dbo.T7_InstanceInfo.InstanceID LEFT OUTER JOIN dbo.MainID_MainIDSpec INNER JOIN dbo.LabelsMade INNER JOIN dbo.MainIDSpec ON dbo.LabelsMade.RecordNumber = dbo.MainIDSpec.RecordNumber ON dbo.MainID_MainIDSpec.WaferSpec_Key = dbo.MainIDSpec.RecordNumber INNER JOIN dbo.CofA_Info ON dbo.MainID_MainIDSpec.MainID = dbo.CofA_Info.ID_NUMBER INNER JOIN dbo.PROCESS_INFO ON dbo.MainID_MainIDSpec.MainID = dbo.PROCESS_INFO.ID_NUMBER ON dbo.T_FGI_Boxes.LabelsMadeKey = dbo.LabelsMade.LabelRecordNumber WHERE (dbo.T_FGI_Boxes.CartonNumber = '" & DataInput & "') ORDER BY dbo.T7_InstanceInfo.Slot"
            End If

            If DataInput.Contains("WB") Then
                DataInput = Mid(DataInput, 3)
                Me.CartonDetailSqlDataSource.SelectCommand = "SELECT TOP 100 PERCENT dbo.T_FGI_Boxes.CartonNumber, dbo.T_FGI_Boxes.BoxInvNumber AS WaferBoxNumber, dbo.LabelsMade.Lot, dbo.T7_InstanceInfo.Slot, dbo.T7_WaferActionTracking.T7, dbo.MainIDSpec.thk_grp AS SpecThick, dbo.T7_GeoData.CenterThick, dbo.MainIDSpec.res_grp AS SpecRes, dbo.T7_GeoData.CenterRes, dbo.MainIDSpec.WTYPE_DOPE AS SpecType, dbo.T7_GeoData.Type, dbo.PROCESS_INFO.BOW AS SpecBow, dbo.T7_GeoData.Bow, dbo.PROCESS_INFO.WARP AS SpecWarp, dbo.T7_GeoData.TotWarp AS Warp, dbo.PROCESS_INFO.FINAL_TTV AS SpecTTV, dbo.T7_GeoData.TTV, dbo.CofA_Info.First_Bin, dbo.PROCESS_INFO.PARTICLE_SPEC_1 AS First_BinSpec, dbo.CofA_Info.Second_Bin, dbo.PROCESS_INFO.PARTICLE_SPEC_2 AS Second_BinSpec, dbo.CofA_Info.Third_Bin, dbo.PROCESS_INFO.PARTICLE_SPEC_3 AS Third_BinBin, dbo.CofA_Info.Forth_Bin, dbo.PROCESS_INFO.PARTICLE_SPEC_4 AS Forth_BinSpec, dbo.T7_ParticalData.SP1BinCnt1 AS Bin1, dbo.T7_ParticalData.SP1BinCnt2 AS Bin2, dbo.T7_ParticalData.SP1BinCnt3 AS Bin3, dbo.T7_ParticalData.SP1BinCnt4 AS Bin4, dbo.T7_ParticalData.SP1BinCnt5 AS Bin5, dbo.T7_ParticalData.SP1BinCnt6 AS Bin6, dbo.T7_ParticalData.SP1BinCnt7 AS Bin7, dbo.T7_ParticalData.SP1BinCnt8 AS Bin8 FROM dbo.T_FGI_Boxes LEFT OUTER JOIN dbo.T7_ParticalData INNER JOIN dbo.T7_GeoData INNER JOIN dbo.T7_WaferActionTracking INNER JOIN dbo.T7_InstanceInfo ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key ON dbo.T7_GeoData.Geo_Key = dbo.T7_WaferActionTracking.PostGeo_Key ON dbo.T7_ParticalData.Partical_Key = dbo.T7_WaferActionTracking.Partical_Key ON dbo.T_FGI_Boxes.InstanceKey = dbo.T7_InstanceInfo.InstanceID LEFT OUTER JOIN dbo.MainID_MainIDSpec INNER JOIN dbo.LabelsMade INNER JOIN dbo.MainIDSpec ON dbo.LabelsMade.RecordNumber = dbo.MainIDSpec.RecordNumber ON dbo.MainID_MainIDSpec.WaferSpec_Key = dbo.MainIDSpec.RecordNumber INNER JOIN dbo.CofA_Info ON dbo.MainID_MainIDSpec.MainID = dbo.CofA_Info.ID_NUMBER INNER JOIN dbo.PROCESS_INFO ON dbo.MainID_MainIDSpec.MainID = dbo.PROCESS_INFO.ID_NUMBER ON dbo.T_FGI_Boxes.LabelsMadeKey = dbo.LabelsMade.LabelRecordNumber WHERE (dbo.T_FGI_Boxes.BoxInvNumber = " & DataInput & ") ORDER BY dbo.T7_InstanceInfo.Slot"
            End If

            Me.GridView1.DataBind()
            FilerColumns()
            If Me.GridView1.Rows.Count > 0 Then
                Me.InfoLabel.Text = "Records Found"
            Else
                If Not DataInput = "" Then
                    Me.InfoLabel.Text = "No Record Found For " & Me.CartonTextBox.Text
                End If
            End If
        Catch ex As Exception
            Me.InfoLabel.Text = "SATI.Net Finds A Record Problem With That Data!"
        End Try


    End Sub

    Sub FilerColumns()
        If Me.InfoLabel.Text = "Records Found" Then
            Dim i As Int16
            If Me.CheckBox1.Checked = True Then
                For i = 4 To 15
                    If Me.GridView1.Columns(i).Visible = False Then
                        Me.GridView1.Columns(i).Visible = True
                    End If
                Next
            Else
                For i = 4 To 15
                    If Me.GridView1.Columns(i).Visible = True Then
                        Me.GridView1.Columns(i).Visible = False
                    End If
                Next
            End If
            If Me.CheckBox2.Checked = True Then
                For i = 16 To 31
                    If Me.GridView1.Columns(i).Visible = False Then
                        Me.GridView1.Columns(i).Visible = True
                    End If
                Next
            Else
                For i = 16 To 31
                    If Me.GridView1.Columns(i).Visible = True Then
                        Me.GridView1.Columns(i).Visible = False
                    End If
                Next
            End If
            If Me.CheckBox3.Checked = True Then
                If Me.GridView1.Columns(2).Visible = False Then
                    Me.GridView1.Columns(2).Visible = True
                End If
            Else
                If Me.GridView1.Columns(2).Visible = True Then
                    Me.GridView1.Columns(2).Visible = False
                End If
            End If
            If Me.CheckBox4.Checked = True Then
                If Me.GridView1.Columns(3).Visible = False Then
                    Me.GridView1.Columns(3).Visible = True
                End If
            Else
                If Me.GridView1.Columns(3).Visible = True Then
                    Me.GridView1.Columns(3).Visible = False
                End If
            End If
        End If
    End Sub


    Sub Summary()

    End Sub
End Class
