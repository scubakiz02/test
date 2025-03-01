
Partial Class Reports_T7Detail
    Inherits System.Web.UI.Page

    Protected Sub T7TextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles T7TextBox.TextChanged
        ClearMe()
        ChangeData(Me.T7TextBox.Text)
    End Sub

    Sub ClearMe()

        Me.RecDateLabel.Text = ""
        Me.ShipDateLabel.Text = ""

        Me.PreGeoDateLabel.Text = ""
        Me.PreGeoToolLabel.Text = ""
        Me.PreGeoThickLabel.Text = ""
        Me.PreGeoResLabel.Text = ""
        Me.PreGeoTypeLabel.Text = ""

        Me.PostGeoDateLabel.Text = ""
        Me.PostGeoToolLabel.Text = ""
        Me.PostGeoThickLabel.Text = ""
        Me.PostGeoResLabel.Text = ""
        Me.PostGeoTypeLabel.Text = ""

        Me.SurfDateLabel.Text = ""
        Me.SurfToolLabel.Text = ""
        Me.SurfScrachLabel.Text = ""
        'Me.SurfAreaLabel.Text = ""
        Me.CMPLabel.Text = ""

        Me.Surf1LPDLabel.Text = ""
        Me.Surf1LPDNLabel.Text = ""
        Me.Surf1SizeLabel.Text = ""
        Me.Surf1SODLabel.Text = ""

        Me.Surf2LPDLabel.Text = ""
        Me.Surf2LPDNLabel.Text = ""
        Me.Surf2SizeLabel.Text = ""
        Me.Surf2SODLabel.Text = ""

        Me.Surf3LPDLabel.Text = ""
        Me.Surf3LPDNLabel.Text = ""
        Me.Surf3SizeLabel.Text = ""
        Me.Surf3SODLabel.Text = ""

        Me.Surf4LPDLabel.Text = ""
        Me.Surf4LPDNLabel.Text = ""
        Me.Surf4SizeLabel.Text = ""
        Me.Surf4SODLabel.Text = ""

        Me.ShipCartonLabel.Text = ""
        Me.ShipDateLabel.Text = ""
        Me.ShipLotLabel.Text = ""
        Me.ShipNumberLabel.Text = ""
        Me.ShipPartLabel.Text = ""
        Me.ShipSlotLabel.Text = ""
        Me.ShipSpecLabel.Text = ""
        Me.ShipSpecRevLabel.Text = ""
        Me.ShipWaferBoxLabel.Text = ""

        Me.InfoLabel.Visible = False
    End Sub
    Sub ChangeData(ByVal T7 As String)

        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = Session("DBConnect")
        Connection.Open()
        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim SelectCmd As New System.Data.SqlClient.SqlCommand
        With SelectCmd
            '.CommandText = "SELECT dbo.T7_WaferActionTracking.T7, dbo.T_WH_Invintory.EventTime AS RecDate, dbo.T7_WaferActionTracking.StartDate AS ProdStart, dbo.T7_GeoData.RecordDate AS PreGeoDate, dbo.T7_GeoData.Tool AS PreGeoTool, dbo.T7_GeoData.CenterThick AS PreGeoThick, dbo.T7_GeoData.CenterRes AS PreGeoRes, dbo.T7_GeoData.Type AS PreGeoType, T7_GeoData_1.RecordDate AS PostGeoDate, T7_GeoData_1.Tool AS PostGeoTool, T7_GeoData_1.CenterThick AS PostGeoThick, T7_GeoData_1.CenterRes AS PostGeoRes, T7_GeoData_1.Type AS PostGeoType, dbo.T7_ParticalData.RecordDate AS SurfDate, dbo.T7_ParticalData.Tool AS SurfTool, dbo.T7_ParticalData.ID AS SurfID, dbo.T7_ParticalData.Run AS SurfRun, dbo.T7_ParticalData.WL AS SurfWL, dbo.T7_ParticalData.SP1BinCnt1, dbo.T7_ParticalData.SP1BinCnt2, dbo.T7_ParticalData.SP1BinCnt3, dbo.T7_ParticalData.SP1BinCnt4, dbo.T7_ParticalData.SP1BinCnt5, dbo.T7_ParticalData.SP1BinCnt6, dbo.T7_ParticalData.SP1BinCnt7, dbo.T7_ParticalData.SP1BinCnt8, dbo.T7_ParticalData.SP1LPDNBinCntInSize1, dbo.T7_ParticalData.SP1LPDNBinCntInSize2, dbo.T7_ParticalData.SP1LPDNBinCntInSize3, dbo.T7_ParticalData.SP1LPDNBinCntInSize4, dbo.T7_ParticalData.SP1LPDNBinCntInSize5, dbo.T7_ParticalData.SP1LPDNBinCntInSize6, dbo.T7_ParticalData.SP1LPDNBinCntInSize7, dbo.T7_ParticalData.SP1LPDNBinCntInSize8, dbo.T7_ParticalData.SP1SOD1, dbo.T7_ParticalData.SP1SOD2, dbo.T7_ParticalData.SP1SOD3, dbo.T7_ParticalData.SP1SOD4, dbo.T7_ParticalData.SP1SOD5, dbo.T7_ParticalData.SP1SOD6, dbo.T7_ParticalData.SP1SOD7, dbo.T7_ParticalData.SP1SOD8, dbo.T7_ParticalData.ScratchCnt, dbo.CofA_Info.LPD_G1, dbo.CofA_Info.First_Bin, dbo.CofA_Info.LPD_G2, dbo.CofA_Info.Second_Bin, dbo.CofA_Info.LPD_G3, dbo.CofA_Info.Third_Bin, dbo.CofA_Info.LPD_G4, dbo.CofA_Info.Forth_Bin, dbo.T7_ParticalData.AreaCnt, dbo.T7_ParticalData.UserID FROM dbo.T7_WaferActionTracking INNER JOIN dbo.T_WH_Invintory ON dbo.T7_WaferActionTracking.WL = dbo.T_WH_Invintory.Waferlog INNER JOIN dbo.T7_GeoData ON dbo.T7_WaferActionTracking.PreGeo_Key = dbo.T7_GeoData.Geo_Key INNER JOIN dbo.T7_GeoData AS T7_GeoData_1 ON dbo.T7_WaferActionTracking.PostGeo_Key = T7_GeoData_1.Geo_Key INNER JOIN dbo.T7_ParticalData ON dbo.T7_WaferActionTracking.Partical_Key = dbo.T7_ParticalData.Partical_Key INNER JOIN dbo.CofA_Info ON dbo.T7_ParticalData.ID = dbo.CofA_Info.ID_NUMBER WHERE (dbo.T7_WaferActionTracking.T7 = N'" & T7 & "') AND (dbo.T7_WaferActionTracking.Active = N'Yes') AND (dbo.T_WH_Invintory.Action = N'StartWL')"
            .CommandText = "SELECT dbo.T7_WaferActionTracking.T7, dbo.T_WH_Invintory.EventTime AS RecDate, dbo.T7_WaferActionTracking.StartDate AS ProdStart, dbo.T7_GeoData.RecordDate AS PreGeoDate, dbo.T7_GeoData.Tool AS PreGeoTool, dbo.T7_GeoData.CenterThick AS PreGeoThick, dbo.T7_GeoData.CenterRes AS PreGeoRes, dbo.T7_GeoData.Type AS PreGeoType, T7_GeoData_1.RecordDate AS PostGeoDate, T7_GeoData_1.Tool AS PostGeoTool, T7_GeoData_1.CenterThick AS PostGeoThick, T7_GeoData_1.CenterRes AS PostGeoRes, T7_GeoData_1.Type AS PostGeoType, dbo.T7_ParticalData.RecordDate AS SurfDate, dbo.T7_ParticalData.Tool AS SurfTool, dbo.T7_ParticalData.ID AS SurfID, dbo.T7_ParticalData.Run AS SurfRun, dbo.T7_ParticalData.WL AS SurfWL, dbo.T7_ParticalData.SP1BinCnt1, dbo.T7_ParticalData.SP1BinCnt2, dbo.T7_ParticalData.SP1BinCnt3, dbo.T7_ParticalData.SP1BinCnt4, dbo.T7_ParticalData.SP1BinCnt5, dbo.T7_ParticalData.SP1BinCnt6, dbo.T7_ParticalData.SP1BinCnt7, dbo.T7_ParticalData.SP1BinCnt8, dbo.T7_ParticalData.SP1LPDNBinCntInSize1, dbo.T7_ParticalData.SP1LPDNBinCntInSize2, dbo.T7_ParticalData.SP1LPDNBinCntInSize3, dbo.T7_ParticalData.SP1LPDNBinCntInSize4, dbo.T7_ParticalData.SP1LPDNBinCntInSize5, dbo.T7_ParticalData.SP1LPDNBinCntInSize6, dbo.T7_ParticalData.SP1LPDNBinCntInSize7, dbo.T7_ParticalData.SP1LPDNBinCntInSize8, dbo.T7_ParticalData.SP1SOD1, dbo.T7_ParticalData.SP1SOD2, dbo.T7_ParticalData.SP1SOD3, dbo.T7_ParticalData.SP1SOD4, dbo.T7_ParticalData.SP1SOD5, dbo.T7_ParticalData.SP1SOD6, dbo.T7_ParticalData.SP1SOD7, dbo.T7_ParticalData.SP1SOD8, dbo.T7_ParticalData.ScratchCnt, dbo.CofA_Info.LPD_G1, dbo.CofA_Info.First_Bin, dbo.CofA_Info.LPD_G2, dbo.CofA_Info.Second_Bin, dbo.CofA_Info.LPD_G3, dbo.CofA_Info.Third_Bin, dbo.CofA_Info.LPD_G4, dbo.CofA_Info.Forth_Bin, dbo.T7_ParticalData.AreaCnt, dbo.T_FGI_Boxes.BoxInvNumber AS WaferBox, dbo.T7_InstanceInfo.Slot, dbo.T_FGI_Boxes.CartonNumber, dbo.ShippingInventory.PickTicket AS ShipmentNumber, dbo.ShippingInventory.Confirmed AS ShipDate, dbo.LabelsMade.Lot, dbo.MainIDSpec.PART_NUMBER, dbo.MainIDSpec.SPEC_NUMBER, dbo.MainIDSpec.SPEC_REV_NUMBER, dbo.T7_ParticalData.UserID, dbo.T7_ParticalData.Map FROM dbo.T_FGI_Boxes INNER JOIN dbo.T7_InstanceInfo ON dbo.T_FGI_Boxes.InstanceKey = dbo.T7_InstanceInfo.InstanceID INNER JOIN dbo.ShippingInventory ON dbo.T_FGI_Boxes.CartonNumber = dbo.ShippingInventory.Carton_Key INNER JOIN dbo.LabelsMade ON dbo.T_FGI_Boxes.LabelsMadeKey = dbo.LabelsMade.LabelRecordNumber INNER JOIN dbo.MainIDSpec ON dbo.LabelsMade.RecordNumber = dbo.MainIDSpec.RecordNumber RIGHT OUTER JOIN dbo.T7_WaferActionTracking INNER JOIN dbo.T_WH_Invintory ON dbo.T7_WaferActionTracking.WL = dbo.T_WH_Invintory.Waferlog INNER JOIN dbo.T7_GeoData ON dbo.T7_WaferActionTracking.PreGeo_Key = dbo.T7_GeoData.Geo_Key INNER JOIN dbo.T7_GeoData AS T7_GeoData_1 ON dbo.T7_WaferActionTracking.PostGeo_Key = T7_GeoData_1.Geo_Key INNER JOIN dbo.T7_ParticalData ON dbo.T7_WaferActionTracking.Partical_Key = dbo.T7_ParticalData.Partical_Key INNER JOIN dbo.CofA_Info ON dbo.T7_ParticalData.ID = dbo.CofA_Info.ID_NUMBER ON dbo.T7_InstanceInfo.WAT_Key = dbo.T7_WaferActionTracking.WAT_Key WHERE (dbo.T7_WaferActionTracking.T7 = N'" & T7 & "') AND (dbo.T7_WaferActionTracking.Active = N'Yes') AND (dbo.T_WH_Invintory.Action = N'StartWL') ORDER BY dbo.ShippingInventory.Confirmed DESC"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd
        DA.Fill(DS)

        Connection.Close()

        If DS.Tables(0).Rows.Count > 0 Then
            DR = DS.Tables(0).Rows(0)
            'T7
            Me.RecDateLabel.Text = CType(DR("RecDate").ToString, DateTime).Date.ToShortDateString
            'ProdStart

            'Pre Geo
            Me.PreGeoDateLabel.Text = CType(DR("PreGeoDate").ToString, DateTime).Date.ToShortDateString
            Me.PreGeoToolLabel.Text = DR("PreGeoTool").ToString
            Me.PreGeoThickLabel.Text = DR("PreGeoThick").ToString
            Me.PreGeoResLabel.Text = DR("PreGeoRes").ToString
            Me.PreGeoTypeLabel.Text = DR("PreGeoType").ToString

            'Post Geo
            Me.PostGeoDateLabel.Text = CType(DR("PostGeoDate").ToString, DateTime).Date.ToShortDateString
            Me.PostGeoToolLabel.Text = DR("PostGeoTool").ToString
            Me.PostGeoThickLabel.Text = DR("PostGeoThick").ToString
            Me.PostGeoResLabel.Text = DR("PostGeoRes").ToString
            Me.PostGeoTypeLabel.Text = DR("PostGeoType").ToString

            'SurfScan
            Me.SurfDateLabel.Text = CType(DR("SurfDate").ToString, DateTime).Date.ToShortDateString
            Me.SurfToolLabel.Text = DR("SurfTool").ToString
            'SurfID
            'SurfRun
            'SurfWL
            Me.SurfScrachLabel.Text = DR("ScratchCnt").ToString
            'Me.SurfAreaLabel.Text = DR("AreaCnt").ToString

            'UserID
            If DR("UserID").ToString.Contains("-1") Then
                Me.CMPLabel.Text = "1"
            End If
            If DR("UserID").ToString.Contains("-2") Then
                Me.CMPLabel.Text = "2"
            End If

            Me.Surf1SizeLabel.Text = DR("LPD_G1").ToString
            Me.Surf2SizeLabel.Text = DR("LPD_G2").ToString
            Me.Surf3SizeLabel.Text = DR("LPD_G3").ToString
            Me.Surf4SizeLabel.Text = DR("LPD_G4").ToString

            'First_Bin
            Me.Surf1LPDLabel.Text = DR("SP1BinCnt" & DR("First_Bin").ToString).ToString
            Me.Surf1LPDNLabel.Text = DR("SP1LPDNBinCntInSize" & DR("First_Bin").ToString).ToString
            Me.Surf1SODLabel.Text = DR("SP1SOD" & DR("First_Bin").ToString).ToString

            'Second_Bin
            Me.Surf2LPDLabel.Text = DR("SP1BinCnt" & DR("Second_Bin").ToString).ToString
            Me.Surf2LPDNLabel.Text = DR("SP1LPDNBinCntInSize" & DR("Second_Bin").ToString).ToString
            Me.Surf2SODLabel.Text = DR("SP1SOD" & DR("Second_Bin").ToString).ToString

            'Third_Bin
            Me.Surf3LPDLabel.Text = DR("SP1BinCnt" & DR("Third_Bin").ToString).ToString
            Me.Surf3LPDNLabel.Text = DR("SP1LPDNBinCntInSize" & DR("Third_Bin").ToString).ToString
            Me.Surf3SODLabel.Text = DR("SP1SOD" & DR("Third_Bin").ToString).ToString

            'Forth_Bin
            Me.Surf4LPDLabel.Text = DR("SP1BinCnt" & DR("Forth_Bin").ToString).ToString
            Me.Surf4LPDNLabel.Text = DR("SP1LPDNBinCntInSize" & DR("Forth_Bin").ToString).ToString
            Me.Surf4SODLabel.Text = DR("SP1SOD" & DR("Forth_Bin").ToString).ToString

            'SP1BinCnt1, SP1LPDNBinCntInSize1, SP1SOD1
            'SP1BinCnt2, SP1LPDNBinCntInSize2, SP1SOD2
            'SP1BinCnt3, SP1LPDNBinCntInSize3, SP1SOD3
            'SP1BinCnt4, SP1LPDNBinCntInSize4, SP1SOD4
            'SP1BinCnt5, SP1LPDNBinCntInSize5, SP1SOD5
            'SP1BinCnt6, SP1LPDNBinCntInSize6, SP1SOD6
            'SP1BinCnt7, SP1LPDNBinCntInSize7, SP1SOD7
            'SP1BinCnt8, SP1LPDNBinCntInSize8, SP1SOD8


            Me.ShipWaferBoxLabel.Text = DR("WaferBox").ToString
            Me.ShipSlotLabel.Text = DR("Slot").ToString
            Me.ShipCartonLabel.Text = DR("CartonNumber").ToString
            Me.ShipNumberLabel.Text = DR("ShipmentNumber").ToString
            Try
                Me.ShipDateLabel.Text = CType(DR("ShipDate").ToString, DateTime).Date.ToShortDateString
            Catch ex As Exception
                Me.ShipDateLabel.Text = "Not Shipped"
            End Try

            Me.ShipLotLabel.Text = DR("Lot").ToString
            Me.ShipPartLabel.Text = DR("PART_NUMBER").ToString
            Me.ShipSpecLabel.Text = DR("SPEC_NUMBER").ToString
            Me.ShipSpecRevLabel.Text = DR("SPEC_REV_NUMBER").ToString


            Me.MapButton.CommandName = DR("Map").ToString()

        Else
            Me.InfoLabel.Visible = True
        End If

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)

        ClearMe()
    End Sub

    Protected Sub MapButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MapButton.Click
        Try
            Dim MapFile As String
            MapFile = Me.MapButton.CommandName
            If MapFile = "" Then
                Me.Button3_ModalPopupExtender.Show()
                Exit Sub
            End If

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
            Me.Button2_ModalPopupExtender.Show()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ButtonClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClose.Click

    End Sub

    Protected Sub MapCloseButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MapCloseButton.Click

    End Sub
End Class
