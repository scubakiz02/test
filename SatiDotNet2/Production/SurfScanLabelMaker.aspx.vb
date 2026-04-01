'Imports Class1

Partial Class Production_SurfScanLabelMaker
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1


    Sub GoSPxSQL(ByVal tool As String, ByVal Station As String, ByVal Printer As String)
        Me.InfoTextBox.Text = ""
        Dim IDPass As String = 0

        'ClearBoxes()

        '****
        Dim i As Integer = 0
        Dim Slot As String
        Dim Qty As String
        Dim LastSlot As Int16 = 26
        Dim IKey As Integer
        Dim ParticalKey As Integer
        Dim T_IKey As String
        Dim T_Slot As String
        Dim TheTime As Date
        Dim TheLotNumber As String = ""
        Dim LastT7 As String = ""
        Dim T7Ver As Boolean = False
        Dim last4 As String
        Dim MailStringBuild As String
        MailStringBuild = ""
        '****************************
        '********Connections*********
        '****************************
        Dim AutoDataConnection As New Data.SqlClient.SqlConnection
        AutoDataConnection.ConnectionString = ConfigurationManager.ConnectionStrings("AutoDataConnectionString").ConnectionString
        AutoDataConnection.Open()

        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        '*****************************
        Select Case tool
            Case "SP1"
                'Update SP1 Data
                Dim SP1DataCollector As New System.Data.SqlClient.SqlCommand
                With SP1DataCollector
                    .CommandText = "exsil_user.[SP1DataCollector_SP11Only]"
                    .CommandType = System.Data.CommandType.StoredProcedure
                    .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, False, CType(0, Byte), CType(0, Byte), "", System.Data.DataRowVersion.Current, Nothing)})
                    .Connection = AutoDataConnection
                End With
                SP1DataCollector.ExecuteNonQuery()
                AutoDataConnection.Close()
            Case "SP2"
                'Update SP12 Data
                Dim SP1DataCollector As New System.Data.SqlClient.SqlCommand
                With SP1DataCollector
                    .CommandText = "exsil_user.[SP1DataCollector_SP12Only]"
                    .CommandType = System.Data.CommandType.StoredProcedure
                    .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, False, CType(0, Byte), CType(0, Byte), "", System.Data.DataRowVersion.Current, Nothing)})
                    .Connection = AutoDataConnection
                End With
                SP1DataCollector.ExecuteNonQuery()
                AutoDataConnection.Close()
            Case ""
                'look if sp2 data collection is on 5/16/11
                ' xml files for sp2 "\\sp2-s0132\c$\SP2DataDump\XML Files\"

                'Dim filesnames() As String
                'filesnames = System.IO.Directory.GetFiles("\\sp2-s0132\c$\SP2DataDump\XML Files\")
                'If Not filesnames.Count = 0 Then
                'Me.InfoTextBox.Text = "SP2 Data Colection has files to read. Check SP2 Data Collection."
                'Exit Sub
                'End If
        End Select

        '*********************************************
        '******Sutup DA's and DS's********************
        '*********************************************

        '****************************************************************************************************
        'SP1 data *******************************************************************************************
        '****************************************************************************************************
        Dim DA_SP1 As New System.Data.SqlClient.SqlDataAdapter
        Dim DS_SP1 As New System.Data.DataSet
        Dim DR_SP1 As System.Data.DataRow
        Dim SP1RowCount As Integer

        Dim SP1SelectCmd As New System.Data.SqlClient.SqlCommand
        With SP1SelectCmd
            .CommandText = "SELECT TOP 25 Entry, Machine, CreationDate, SPSessionName, SPRecipeName, SessionDate, ID#, RUN#, Wafer_log, Comment1, Comment2, ChannelID, SourceSlotID, DispositionName, SumAllDefects, FailedLimit, AreaCnt, TotalArea, ScratchCnt, ScratchTotalLength, ScratchAveLength, ScratchMaxLength, ClusterAreaCnt, LPDECnt, LPDSCnt, PosCnt, NegCnt, WaferPosAvgDensity, WaferPosMean, WaferPosStdDev, WaferNegAvgDensity, WaferNegMean, WaferNegStdDev, BinCnt1, BinCnt2, BinCnt3, BinCnt4, BinCnt5, BinCnt6, BinCnt7, BinCnt8, BinCnt18, RangeMin, RangeMax, TotalNCDefectsCount, LPDNBinCntInSize1, LPDNBinCntInSize2, LPDNBinCntInSize3, LPDNBinCntInSize4, LPDNBinCntInSize5, LPDNBinCntInSize6, LPDNBinCntInSize7, LPDNBinCntInSize8, LPDNBinCntInSize18, SOD1, SOD2, SOD3, SOD4, SOD5, SOD6, SOD7, SOD8, SOD18, Average, Peak, Median, StdDeviation, Thruput, WaferDia, EdgeExclusion, DestinationStationID, DestinationSlotID, WaferIdLabel, Comment, Map, RFID FROM dbo.SP1_Data WHERE (DestinationStationID = " & Station & ") AND (Machine = N'" & tool & "') ORDER BY CreationDate DESC"
            .Connection = AutoDataConnection
        End With
        DA_SP1.SelectCommand = SP1SelectCmd

        '****************************************************************************************************
        'Transulated Instance *******************************************************************************
        '****************************************************************************************************
        Dim DA_T_Instance As New Data.SqlClient.SqlDataAdapter
        Dim DS_T_Instance As New Data.DataSet
        Dim DR_T_Instance As Data.DataRow

        Dim T_InstanceSelectCmd As New System.Data.SqlClient.SqlCommand
        T_InstanceSelectCmd.Connection = Connection
        DA_T_Instance.SelectCommand = T_InstanceSelectCmd

        '****************************************************************************************************
        'Instance table for recording the new istance *******************************************************
        '****************************************************************************************************
        Dim DA_Instance As New Data.SqlClient.SqlDataAdapter
        Dim DS_Instance As New Data.DataSet
        Dim DR_Instance As Data.DataRow

        Dim InstanceSelectCmd As New System.Data.SqlClient.SqlCommand
        With InstanceSelectCmd
            .CommandText = "SELECT I_Key, InstanceID, WAT_Key, Slot, Note FROM dbo.T7_InstanceInfo Where (InstanceID = 0)"
            .Connection = Connection
        End With
        DA_Instance.SelectCommand = InstanceSelectCmd

        Dim InstanceInsertCmd As New System.Data.SqlClient.SqlCommand
        With InstanceInsertCmd
            .CommandText = "INSERT INTO [dbo].[T7_InstanceInfo] ([InstanceID], [WAT_Key], [Slot], [Note]) VALUES (@InstanceID, @WAT_Key, @Slot, @Note); SELECT I_Key, InstanceID, WAT_Key, Slot, Note FROM dbo.T7_InstanceInfo WHERE (I_Key = SCOPE_IDENTITY())"
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@InstanceID", System.Data.SqlDbType.Int, 0, "InstanceID"), New System.Data.SqlClient.SqlParameter("@WAT_Key", System.Data.SqlDbType.Int, 0, "WAT_Key"), New System.Data.SqlClient.SqlParameter("@Slot", System.Data.SqlDbType.Int, 0, "Slot"), New System.Data.SqlClient.SqlParameter("@Note", System.Data.SqlDbType.VarChar, 0, "Note")})
            .Connection = Connection
        End With
        DA_Instance.InsertCommand = InstanceInsertCmd

        DA_Instance.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T7_InstanceInfo", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("I_Key", "I_Key"), New System.Data.Common.DataColumnMapping("InstanceID", "InstanceID"), New System.Data.Common.DataColumnMapping("WAT_Key", "WAT_Key"), New System.Data.Common.DataColumnMapping("Slot", "Slot"), New System.Data.Common.DataColumnMapping("Note", "Note")})})
        DA_Instance.Fill(DS_Instance) ' fill the ds with a blank query so the table is set for a insert

        '****************************************************************************************************
        'Partical data **************************************************************************************
        '****************************************************************************************************
        Dim DA_Partical As New Data.SqlClient.SqlDataAdapter
        Dim DS_Partical As New Data.DataSet
        Dim DR_Partical As Data.DataRow

        Dim ParticalSelectCmd As New System.Data.SqlClient.SqlCommand
        With ParticalSelectCmd
            .Connection = Connection
            .CommandText = "SELECT Partical_Key, Tool, Recipe, ID, Run, WL, UserID, RecordDate, SumAllDefects, AreaCnt, TotalArea, ScratchCnt, ScratchTotalLength, SP1BinCnt1, SP1BinCnt2, SP1BinCnt3, SP1BinCnt4, SP1BinCnt5, SP1BinCnt6, SP1BinCnt7, SP1BinCnt8, SP1BinCnt18, SP1LPDNBinCntInSize1, SP1LPDNBinCntInSize2, SP1LPDNBinCntInSize3, SP1LPDNBinCntInSize4, SP1LPDNBinCntInSize5, SP1LPDNBinCntInSize6, SP1LPDNBinCntInSize7, SP1LPDNBinCntInSize8, SP1LPDNBinCntInSize18, SP1SOD1, SP1SOD2, SP1SOD3, SP1SOD4, SP1SOD5, SP1SOD6, SP1SOD7, SP1SOD8, SP1SOD18, Average, Peak, Median, StdDeviation, EdgeExclusion, DispositionName, Map, RFID, ClusterAreaCnt FROM dbo.T7_ParticalData WHERE (Partical_Key = 0)"
        End With
        DA_Partical.SelectCommand = ParticalSelectCmd

        Dim ParticalInsertCmd As New System.Data.SqlClient.SqlCommand
        With ParticalInsertCmd
            .CommandText = "INSERT INTO [T7_ParticalData] ([Tool], [Recipe], [ID], [Run], [WL], [UserID], [RecordDate], [SumAllDefects], [AreaCnt], [TotalArea], [ScratchCnt], [ScratchTotalLength], [SP1BinCnt1], [SP1BinCnt2], [SP1BinCnt3], [SP1BinCnt4], [SP1BinCnt5], [SP1BinCnt6], [SP1BinCnt7], [SP1BinCnt8], [SP1BinCnt18], [SP1LPDNBinCntInSize1], [SP1LPDNBinCntInSize2], [SP1LPDNBinCntInSize3], [SP1LPDNBinCntInSize4], [SP1LPDNBinCntInSize5], [SP1LPDNBinCntInSize6], [SP1LPDNBinCntInSize7], [SP1LPDNBinCntInSize8], [SP1LPDNBinCntInSize18], [SP1SOD1], [SP1SOD2], [SP1SOD3], [SP1SOD4], [SP1SOD5], [SP1SOD6], [SP1SOD7], [SP1SOD8], [SP1SOD18], [Average], [Peak], [Median], [StdDeviation], [EdgeExclusion], [DispositionName], [Map], [RFID], [ClusterAreaCnt]) VALUES (@Tool, @Recipe, @ID, @Run, @WL, @UserID, @RecordDate, @SumAllDefects, @AreaCnt, @TotalArea, @ScratchCnt, @ScratchTotalLength, @SP1BinCnt1, @SP1BinCnt2, @SP1BinCnt3, @SP1BinCnt4, @SP1BinCnt5, @SP1BinCnt6, @SP1BinCnt7, @SP1BinCnt8, @SP1BinCnt18, @SP1LPDNBinCntInSize1, @SP1LPDNBinCntInSize2, @SP1LPDNBinCntInSize3, @SP1LPDNBinCntInSize4, @SP1LPDNBinCntInSize5, @SP1LPDNBinCntInSize6, @SP1LPDNBinCntInSize7, @SP1LPDNBinCntInSize8, @SP1LPDNBinCntInSize18, @SP1SOD1, @SP1SOD2, @SP1SOD3, @SP1SOD4, @SP1SOD5, @SP1SOD6, @SP1SOD7, @SP1SOD8, @SP1SOD18, @Average, @Peak, @Median, @StdDeviation, @EdgeExclusion, @DispositionName, @Map, @RFID, @ClusterAreaCnt); SELECT Partical_Key, Tool, Recipe, ID, Run, WL, UserID, RecordDate, SumAllDefects, AreaCnt, TotalArea, ScratchCnt, ScratchTotalLength, SP1BinCnt1, SP1BinCnt2, SP1BinCnt3, SP1BinCnt4, SP1BinCnt5, SP1BinCnt6, SP1BinCnt7, SP1BinCnt8, SP1BinCnt18, SP1LPDNBinCntInSize1, SP1LPDNBinCntInSize2, SP1LPDNBinCntInSize3, SP1LPDNBinCntInSize4, SP1LPDNBinCntInSize5, SP1LPDNBinCntInSize6, SP1LPDNBinCntInSize7, SP1LPDNBinCntInSize8, SP1LPDNBinCntInSize18, SP1SOD1, SP1SOD2, SP1SOD3, SP1SOD4, SP1SOD5, SP1SOD6, SP1SOD7, SP1SOD8, SP1SOD18, Average, Peak, Median, StdDeviation, EdgeExclusion, DispositionName, Map, RFID, ClusterAreaCnt FROM T7_ParticalData WHERE (Partical_Key = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Tool", System.Data.SqlDbType.NVarChar, 0, "Tool"), New System.Data.SqlClient.SqlParameter("@Recipe", System.Data.SqlDbType.NVarChar, 0, "Recipe"), New System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.NVarChar, 0, "ID"), New System.Data.SqlClient.SqlParameter("@Run", System.Data.SqlDbType.NVarChar, 0, "Run"), New System.Data.SqlClient.SqlParameter("@WL", System.Data.SqlDbType.NVarChar, 0, "WL"), New System.Data.SqlClient.SqlParameter("@UserID", System.Data.SqlDbType.NVarChar, 0, "UserID"), New System.Data.SqlClient.SqlParameter("@RecordDate", System.Data.SqlDbType.SmallDateTime, 0, "RecordDate"), New System.Data.SqlClient.SqlParameter("@SumAllDefects", System.Data.SqlDbType.Real, 0, "SumAllDefects"), New System.Data.SqlClient.SqlParameter("@AreaCnt", System.Data.SqlDbType.Real, 0, "AreaCnt"), New System.Data.SqlClient.SqlParameter("@TotalArea", System.Data.SqlDbType.Real, 0, "TotalArea"), New System.Data.SqlClient.SqlParameter("@ScratchCnt", System.Data.SqlDbType.Real, 0, "ScratchCnt"), New System.Data.SqlClient.SqlParameter("@ScratchTotalLength", System.Data.SqlDbType.Real, 0, "ScratchTotalLength"), New System.Data.SqlClient.SqlParameter("@SP1BinCnt1", System.Data.SqlDbType.Real, 0, "SP1BinCnt1"), New System.Data.SqlClient.SqlParameter("@SP1BinCnt2", System.Data.SqlDbType.Real, 0, "SP1BinCnt2"), New System.Data.SqlClient.SqlParameter("@SP1BinCnt3", System.Data.SqlDbType.Real, 0, "SP1BinCnt3"), New System.Data.SqlClient.SqlParameter("@SP1BinCnt4", System.Data.SqlDbType.Real, 0, "SP1BinCnt4"), New System.Data.SqlClient.SqlParameter("@SP1BinCnt5", System.Data.SqlDbType.Real, 0, "SP1BinCnt5"), New System.Data.SqlClient.SqlParameter("@SP1BinCnt6", System.Data.SqlDbType.Real, 0, "SP1BinCnt6"), New System.Data.SqlClient.SqlParameter("@SP1BinCnt7", System.Data.SqlDbType.Real, 0, "SP1BinCnt7"), New System.Data.SqlClient.SqlParameter("@SP1BinCnt8", System.Data.SqlDbType.Real, 0, "SP1BinCnt8"), New System.Data.SqlClient.SqlParameter("@SP1BinCnt18", System.Data.SqlDbType.Real, 0, "SP1BinCnt18"), New System.Data.SqlClient.SqlParameter("@SP1LPDNBinCntInSize1", System.Data.SqlDbType.Real, 0, "SP1LPDNBinCntInSize1"), New System.Data.SqlClient.SqlParameter("@SP1LPDNBinCntInSize2", System.Data.SqlDbType.Real, 0, "SP1LPDNBinCntInSize2"), New System.Data.SqlClient.SqlParameter("@SP1LPDNBinCntInSize3", System.Data.SqlDbType.Real, 0, "SP1LPDNBinCntInSize3"), New System.Data.SqlClient.SqlParameter("@SP1LPDNBinCntInSize4", System.Data.SqlDbType.Real, 0, "SP1LPDNBinCntInSize4"), New System.Data.SqlClient.SqlParameter("@SP1LPDNBinCntInSize5", System.Data.SqlDbType.Real, 0, "SP1LPDNBinCntInSize5"), New System.Data.SqlClient.SqlParameter("@SP1LPDNBinCntInSize6", System.Data.SqlDbType.Real, 0, "SP1LPDNBinCntInSize6"), New System.Data.SqlClient.SqlParameter("@SP1LPDNBinCntInSize7", System.Data.SqlDbType.Real, 0, "SP1LPDNBinCntInSize7"), New System.Data.SqlClient.SqlParameter("@SP1LPDNBinCntInSize8", System.Data.SqlDbType.Real, 0, "SP1LPDNBinCntInSize8"), New System.Data.SqlClient.SqlParameter("@SP1LPDNBinCntInSize18", System.Data.SqlDbType.Real, 0, "SP1LPDNBinCntInSize18"), New System.Data.SqlClient.SqlParameter("@SP1SOD1", System.Data.SqlDbType.Real, 0, "SP1SOD1"), New System.Data.SqlClient.SqlParameter("@SP1SOD2", System.Data.SqlDbType.Real, 0, "SP1SOD2"), New System.Data.SqlClient.SqlParameter("@SP1SOD3", System.Data.SqlDbType.Real, 0, "SP1SOD3"), New System.Data.SqlClient.SqlParameter("@SP1SOD4", System.Data.SqlDbType.Real, 0, "SP1SOD4"), New System.Data.SqlClient.SqlParameter("@SP1SOD5", System.Data.SqlDbType.Real, 0, "SP1SOD5"), New System.Data.SqlClient.SqlParameter("@SP1SOD6", System.Data.SqlDbType.Real, 0, "SP1SOD6"), New System.Data.SqlClient.SqlParameter("@SP1SOD7", System.Data.SqlDbType.Real, 0, "SP1SOD7"), New System.Data.SqlClient.SqlParameter("@SP1SOD8", System.Data.SqlDbType.Real, 0, "SP1SOD8"), New System.Data.SqlClient.SqlParameter("@SP1SOD18", System.Data.SqlDbType.Real, 0, "SP1SOD18"), New System.Data.SqlClient.SqlParameter("@Average", System.Data.SqlDbType.Real, 0, "Average"), New System.Data.SqlClient.SqlParameter("@Peak", System.Data.SqlDbType.Real, 0, "Peak"), New System.Data.SqlClient.SqlParameter("@Median", System.Data.SqlDbType.Real, 0, "Median"), New System.Data.SqlClient.SqlParameter("@StdDeviation", System.Data.SqlDbType.Real, 0, "StdDeviation"), New System.Data.SqlClient.SqlParameter("@EdgeExclusion", System.Data.SqlDbType.Real, 0, "EdgeExclusion"), New System.Data.SqlClient.SqlParameter("@DispositionName", System.Data.SqlDbType.VarChar, 0, "DispositionName"), New System.Data.SqlClient.SqlParameter("@Map", System.Data.SqlDbType.NVarChar, 0, "Map"), New System.Data.SqlClient.SqlParameter("@RFID", System.Data.SqlDbType.NVarChar, 0, "RFID"), New System.Data.SqlClient.SqlParameter("@ClusterAreaCnt", System.Data.SqlDbType.Real, 0, "ClusterAreaCnt")})
        End With
        DA_Partical.InsertCommand = ParticalInsertCmd
        DA_Partical.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T7_ParticalData", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Partical_Key", "Partical_Key"), New System.Data.Common.DataColumnMapping("Tool", "Tool"), New System.Data.Common.DataColumnMapping("Recipe", "Recipe"), New System.Data.Common.DataColumnMapping("ID", "ID"), New System.Data.Common.DataColumnMapping("Run", "Run"), New System.Data.Common.DataColumnMapping("WL", "WL"), New System.Data.Common.DataColumnMapping("UserID", "UserID"), New System.Data.Common.DataColumnMapping("RecordDate", "RecordDate"), New System.Data.Common.DataColumnMapping("SumAllDefects", "SumAllDefects"), New System.Data.Common.DataColumnMapping("AreaCnt", "AreaCnt"), New System.Data.Common.DataColumnMapping("TotalArea", "TotalArea"), New System.Data.Common.DataColumnMapping("ScratchCnt", "ScratchCnt"), New System.Data.Common.DataColumnMapping("ScratchTotalLength", "ScratchTotalLength"), New System.Data.Common.DataColumnMapping("SP1BinCnt1", "SP1BinCnt1"), New System.Data.Common.DataColumnMapping("SP1BinCnt2", "SP1BinCnt2"), New System.Data.Common.DataColumnMapping("SP1BinCnt3", "SP1BinCnt3"), New System.Data.Common.DataColumnMapping("SP1BinCnt4", "SP1BinCnt4"), New System.Data.Common.DataColumnMapping("SP1BinCnt5", "SP1BinCnt5"), New System.Data.Common.DataColumnMapping("SP1BinCnt6", "SP1BinCnt6"), New System.Data.Common.DataColumnMapping("SP1BinCnt7", "SP1BinCnt7"), New System.Data.Common.DataColumnMapping("SP1BinCnt8", "SP1BinCnt8"), New System.Data.Common.DataColumnMapping("SP1BinCnt18", "SP1BinCnt18"), New System.Data.Common.DataColumnMapping("SP1LPDNBinCntInSize1", "SP1LPDNBinCntInSize1"), New System.Data.Common.DataColumnMapping("SP1LPDNBinCntInSize2", "SP1LPDNBinCntInSize2"), New System.Data.Common.DataColumnMapping("SP1LPDNBinCntInSize3", "SP1LPDNBinCntInSize3"), New System.Data.Common.DataColumnMapping("SP1LPDNBinCntInSize4", "SP1LPDNBinCntInSize4"), New System.Data.Common.DataColumnMapping("SP1LPDNBinCntInSize5", "SP1LPDNBinCntInSize5"), New System.Data.Common.DataColumnMapping("SP1LPDNBinCntInSize6", "SP1LPDNBinCntInSize6"), New System.Data.Common.DataColumnMapping("SP1LPDNBinCntInSize7", "SP1LPDNBinCntInSize7"), New System.Data.Common.DataColumnMapping("SP1LPDNBinCntInSize8", "SP1LPDNBinCntInSize8"), New System.Data.Common.DataColumnMapping("SP1LPDNBinCntInSize18", "SP1LPDNBinCntInSize18"), New System.Data.Common.DataColumnMapping("SP1SOD1", "SP1SOD1"), New System.Data.Common.DataColumnMapping("SP1SOD2", "SP1SOD2"), New System.Data.Common.DataColumnMapping("SP1SOD3", "SP1SOD3"), New System.Data.Common.DataColumnMapping("SP1SOD4", "SP1SOD4"), New System.Data.Common.DataColumnMapping("SP1SOD5", "SP1SOD5"), New System.Data.Common.DataColumnMapping("SP1SOD6", "SP1SOD6"), New System.Data.Common.DataColumnMapping("SP1SOD7", "SP1SOD7"), New System.Data.Common.DataColumnMapping("SP1SOD8", "SP1SOD8"), New System.Data.Common.DataColumnMapping("SP1SOD18", "SP1SOD18"), New System.Data.Common.DataColumnMapping("Average", "Average"), New System.Data.Common.DataColumnMapping("Peak", "Peak"), New System.Data.Common.DataColumnMapping("Median", "Median"), New System.Data.Common.DataColumnMapping("StdDeviation", "StdDeviation"), New System.Data.Common.DataColumnMapping("EdgeExclusion", "EdgeExclusion"), New System.Data.Common.DataColumnMapping("DispositionName", "DispositionName"), New System.Data.Common.DataColumnMapping("Map", "Map"), New System.Data.Common.DataColumnMapping("RFID", "RFID"), New System.Data.Common.DataColumnMapping("ClusterAreaCnt", "ClusterAreaCnt")})})
        DA_Partical.Fill(DS_Partical) ' fill the ds with a blank query so the table is set for a insert

        '****************************************************************************************************
        'Wat ************************************************************************************************
        '****************************************************************************************************
        Dim DA_WatP As New Data.SqlClient.SqlDataAdapter
        Dim DS_WatP As New Data.DataSet
        Dim DR_WatP As Data.DataRow

        Dim WatPSelectCmd As New System.Data.SqlClient.SqlCommand
        WatPSelectCmd.Connection = Connection
        DA_WatP.SelectCommand = WatPSelectCmd

        Dim WatPUpdateCmd As New System.Data.SqlClient.SqlCommand
        With WatPUpdateCmd
            .CommandText = "UPDATE dbo.T7_WaferActionTracking SET T7 = @T7, Active = @Active, Partical_Key = @Partical_Key WHERE (WAT_Key = @Original_WAT_Key) AND (@IsNull_T7 = 1) AND (T7 IS NULL) AND (@IsNull_Active = 1) AND (Active IS NULL) AND (@IsNull_Partical_Key = 1) AND (Partical_Key IS NULL) OR (WAT_Key = @Original_WAT_Key) AND (T7 = @Original_T7) AND (@IsNull_Active = 1) AND (Active IS NULL) AND (@IsNull_Partical_Key = 1) AND (Partical_Key IS NULL) OR (WAT_Key = @Original_WAT_Key) AND (@IsNull_T7 = 1) AND (T7 IS NULL) AND (Active = @Original_Active) AND (@IsNull_Partical_Key = 1) AND (Partical_Key IS NULL) OR (WAT_Key = @Original_WAT_Key) AND (T7 = @Original_T7) AND (Active = @Original_Active) AND (@IsNull_Partical_Key = 1) AND (Partical_Key IS NULL) OR (WAT_Key = @Original_WAT_Key) AND (@IsNull_T7 = 1) AND (T7 IS NULL) AND (@IsNull_Active = 1) AND (Active IS NULL) AND (Partical_Key = @Original_Partical_Key) OR (WAT_Key = @Original_WAT_Key) AND (T7 = @Original_T7) AND (@IsNull_Active = 1) AND (Active IS NULL) AND (Partical_Key= @Original_Partical_Key) OR (WAT_Key = @Original_WAT_Key) AND (@IsNull_T7 = 1) AND (T7 IS NULL) AND (Active = @Original_Active) AND (Partical_Key = @Original_Partical_Key) OR (WAT_Key = @Original_WAT_Key) AND (T7 = @Original_T7) AND (Active = @Original_Active) AND (Partical_Key = @Original_Partical_Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@T7", System.Data.SqlDbType.NVarChar, 0, "T7"), New System.Data.SqlClient.SqlParameter("@Active", System.Data.SqlDbType.NVarChar, 0, "Active"), New System.Data.SqlClient.SqlParameter("@Partical_Key", System.Data.SqlDbType.Int, 0, "Partical_Key"), New System.Data.SqlClient.SqlParameter("@Original_WAT_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "WAT_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_T7", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "T7", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_T7", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "T7", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Active", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Active", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Active", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Active", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Partical_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Partical_Key", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Partical_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Partical_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@WAT_Key", System.Data.SqlDbType.Int, 4, "WAT_Key")})
        End With

        DA_WatP.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T7_WaferActionTracking", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("WAT_Key", "WAT_Key"), New System.Data.Common.DataColumnMapping("T7", "T7"), New System.Data.Common.DataColumnMapping("Active", "Active"), New System.Data.Common.DataColumnMapping("Partical_Key", "Partical_Key")})})
        DA_WatP.UpdateCommand = WatPUpdateCmd

        '****************************************************************************************************
        'I Key***********************************************************************************************
        '****************************************************************************************************
        Dim DA_DBChr As New Data.SqlClient.SqlDataAdapter
        Dim DS_DBChr As New Data.DataSet
        Dim DR_DBChr As Data.DataRow

        Dim DBChrSelectCmd As New System.Data.SqlClient.SqlCommand
        With DBChrSelectCmd
            .CommandText = "SELECT FieldName, Value, Characteristic, EffectiveDtd FROM dbo.DB_Characteristics WHERE (FieldName = N'InstanceID')"
            .Connection = Connection
        End With
        DA_DBChr.SelectCommand = DBChrSelectCmd

        Dim DBChrUpdateCmd As New System.Data.SqlClient.SqlCommand
        With DBChrUpdateCmd
            .CommandText = "UPDATE [dbo].[DB_Characteristics] SET [FieldName] = @FieldName, [Value] = @Value, [Characteristic] = @Characteristic, [EffectiveDtd] = @EffectiveDtd WHERE (([FieldName] = @Original_FieldName) AND ([Value] = @Original_Value) AND ([Characteristic] = @Original_Characteristic) AND ([EffectiveDtd] = @Original_EffectiveDtd)); SELECT FieldName, Value, Characteristic, EffectiveDtd FROM dbo.DB_Characteristics WHERE (Characteristic = @Characteristic) AND (EffectiveDtd = @EffectiveDtd) AND (FieldName = @FieldName) AND (Value = @Value)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@FieldName", System.Data.SqlDbType.NVarChar, 0, "FieldName"), New System.Data.SqlClient.SqlParameter("@Value", System.Data.SqlDbType.NVarChar, 0, "Value"), New System.Data.SqlClient.SqlParameter("@Characteristic", System.Data.SqlDbType.NVarChar, 0, "Characteristic"), New System.Data.SqlClient.SqlParameter("@EffectiveDtd", System.Data.SqlDbType.SmallDateTime, 0, "EffectiveDtd"), New System.Data.SqlClient.SqlParameter("@Original_FieldName", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "FieldName", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Value", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Value", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Characteristic", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Characteristic", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_EffectiveDtd", System.Data.SqlDbType.SmallDateTime, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "EffectiveDtd", System.Data.DataRowVersion.Original, Nothing)})
        End With
        DA_DBChr.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "DB_Characteristics", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("FieldName", "FieldName"), New System.Data.Common.DataColumnMapping("Value", "Value"), New System.Data.Common.DataColumnMapping("Characteristic", "Characteristic"), New System.Data.Common.DataColumnMapping("EffectiveDtd", "EffectiveDtd")})})
        DA_DBChr.UpdateCommand = DBChrUpdateCmd

        '****************************************************************************************************
        'Get Instance number and Update BDCharactristicas Table**********************************************
        '****************************************************************************************************
        DS_DBChr.Clear()
        DA_DBChr.Fill(DS_DBChr)

        DR_DBChr = DS_DBChr.Tables(0).Rows(0)
        DR_DBChr.AcceptChanges()
        DR_DBChr.BeginEdit()
        IKey = DR_DBChr("Value") + 1
        DR_DBChr("Value") = IKey
        DR_DBChr.EndEdit()
        DA_DBChr.Update(DS_DBChr, "DB_Characteristics")

        DA_SP1.Fill(DS_SP1)
        SP1RowCount = DS_SP1.Tables(0).Rows.Count
        DR_SP1 = DS_SP1.Tables(0).Rows(0)
        Qty = DR_SP1("DestinationSlotID")
        MailStringBuild = MailStringBuild + "Tool: " & Me.ToolListBox.SelectedItem.Text & Chr(13)
        MailStringBuild = MailStringBuild + "Station: " & Me.StationListBox.SelectedItem.Text & Chr(13)

        Dim KickOut As Boolean = False
        Dim KickOutI As Int16 = 0


        Dim RFID_Info As Boolean = True 'Collect RFID info
        Dim SPxSlot1CreationDate As DateTime
        Dim MyRFID As String = ""

        For i = 0 To SP1RowCount - 1

            DR_SP1 = DS_SP1.Tables(0).Rows(i)
            Slot = DR_SP1("DestinationSlotID")
            If Slot = 1 Then
                'CreationDate
                SPxSlot1CreationDate = DR_SP1("CreationDate")

            End If

            If Slot < LastSlot Then

                LastSlot = Slot
                T_IKey = DR_SP1("Comment2")
                T_Slot = DR_SP1("SourceSlotID")
                TheTime = DateTime.Now.ToShortDateString

                DR_Partical = DS_Partical.Tables("T7_ParticalData").NewRow

                DR_Partical("Tool") = tool
                DR_Partical("Recipe") = DR_SP1("SPRecipeName")
                DR_Partical("ID") = DR_SP1("ID#")
                DR_Partical("Run") = DR_SP1("RUN#")
                DR_Partical("WL") = DR_SP1("Wafer_log")
                DR_Partical("UserID") = DR_SP1("Comment1")
                DR_Partical("RecordDate") = DR_SP1("CreationDate")
                DR_Partical("SumAllDefects") = DR_SP1("SumAllDefects")
                DR_Partical("AreaCnt") = DR_SP1("AreaCnt")
                DR_Partical("TotalArea") = DR_SP1("TotalArea")
                DR_Partical("ScratchCnt") = DR_SP1("ScratchCnt")
                DR_Partical("ScratchTotalLength") = DR_SP1("ScratchTotalLength")
                DR_Partical("SP1BinCnt1") = DR_SP1("BinCnt1")
                DR_Partical("SP1BinCnt2") = DR_SP1("BinCnt2")
                DR_Partical("SP1BinCnt3") = DR_SP1("BinCnt3")
                DR_Partical("SP1BinCnt4") = DR_SP1("BinCnt4")
                DR_Partical("SP1BinCnt5") = DR_SP1("BinCnt5")
                DR_Partical("SP1BinCnt6") = DR_SP1("BinCnt6")
                DR_Partical("SP1BinCnt7") = DR_SP1("BinCnt7")
                DR_Partical("SP1BinCnt8") = DR_SP1("BinCnt8")
                DR_Partical("SP1BinCnt18") = DR_SP1("BinCnt18")
                DR_Partical("SP1LPDNBinCntInSize1") = DR_SP1("LPDNBinCntInSize1")
                DR_Partical("SP1LPDNBinCntInSize2") = DR_SP1("LPDNBinCntInSize2")
                DR_Partical("SP1LPDNBinCntInSize3") = DR_SP1("LPDNBinCntInSize3")
                DR_Partical("SP1LPDNBinCntInSize4") = DR_SP1("LPDNBinCntInSize4")
                DR_Partical("SP1LPDNBinCntInSize5") = DR_SP1("LPDNBinCntInSize5")
                DR_Partical("SP1LPDNBinCntInSize6") = DR_SP1("LPDNBinCntInSize6")
                DR_Partical("SP1LPDNBinCntInSize7") = DR_SP1("LPDNBinCntInSize7")
                DR_Partical("SP1LPDNBinCntInSize8") = DR_SP1("LPDNBinCntInSize8")
                DR_Partical("SP1LPDNBinCntInSize18") = DR_SP1("LPDNBinCntInSize18")
                DR_Partical("SP1SOD1") = DR_SP1("SOD1")
                DR_Partical("SP1SOD2") = DR_SP1("SOD2")
                DR_Partical("SP1SOD3") = DR_SP1("SOD3")
                DR_Partical("SP1SOD4") = DR_SP1("SOD4")
                DR_Partical("SP1SOD5") = DR_SP1("SOD5")
                DR_Partical("SP1SOD6") = DR_SP1("SOD6")
                DR_Partical("SP1SOD7") = DR_SP1("SOD7")
                DR_Partical("SP1SOD8") = DR_SP1("SOD8")
                DR_Partical("SP1SOD18") = DR_SP1("SOD18")
                DR_Partical("Average") = DR_SP1("Average")
                DR_Partical("Peak") = DR_SP1("Peak")
                DR_Partical("Median") = DR_SP1("Median")
                DR_Partical("StdDeviation") = DR_SP1("StdDeviation")
                DR_Partical("EdgeExclusion") = DR_SP1("EdgeExclusion")
                DR_Partical("DispositionName") = DR_SP1("DispositionName")
                DR_Partical("Map") = DR_SP1("Map")
                DR_Partical("RFID") = DR_SP1("RFID")
                DR_Partical("ClusterAreaCnt") = DR_SP1("ClusterAreaCnt")

                IDPass = DR_SP1("ID#")
                TheLotNumber = DR_SP1("ID#") & "-" & DR_SP1("RUN#") & "-" & DR_SP1("Wafer_log")
                DS_Partical.Tables("T7_ParticalData").Rows.Add(DR_Partical)
                DA_Partical.Update(DS_Partical, "T7_ParticalData")

                ParticalKey = DR_Partical("Partical_Key")

                '*******************************************************
                'Get The Translated Instance WAT Key *******************
                '*******************************************************
                T_InstanceSelectCmd.CommandText = "SELECT WAT_Key FROM dbo.T7_InstanceInfo WHERE (InstanceID = " & T_IKey & ") AND (Slot = " & T_Slot & ")"
                DS_T_Instance.Clear()
                DA_T_Instance.Fill(DS_T_Instance)
                DR_T_Instance = DS_T_Instance.Tables(0).Rows(0)

                '*******************************************************
                'Record the slot Instance*******************************
                '*******************************************************
                DR_Instance = DS_Instance.Tables("T7_InstanceInfo").NewRow
                DR_Instance("InstanceID") = IKey
                DR_Instance("Wat_Key") = DR_T_Instance("Wat_Key")
                DR_Instance("Slot") = Slot
                DR_Instance("Note") = tool
                DS_Instance.Tables("T7_InstanceInfo").Rows.Add(DR_Instance)
                DA_Instance.Update(DS_Instance, "T7_InstanceInfo")

                '*******************************************************
                'Update the WAT Table with the Partical Key*************
                '*******************************************************
                WatPSelectCmd.CommandText = "SELECT WAT_Key, T7, Active, Partical_Key FROM dbo.T7_WaferActionTracking WHERE (Wat_Key = " & DR_T_Instance("Wat_Key") & ") ORDER BY WAT_Key DESC"
                DS_WatP.Clear()
                DA_WatP.Fill(DS_WatP)
                DR_WatP = DS_WatP.Tables(0).Rows(0)
                DR_WatP.AcceptChanges()
                DR_WatP.BeginEdit()
                DR_WatP("Partical_Key") = ParticalKey
                DR_WatP.EndEdit()
                DA_WatP.Update(DS_WatP, "T7_WaferActionTracking")
                If T7Ver = False Then
                    LastT7 = DR_WatP("T7")
                    'Me.InfoTextBox.Text = LastT7
                    last4 = Me.M12LaserMarkTextBox.Text
                    If TestT7(LastT7, last4) = False Then
                        Connection.Close()
                        AutoDataConnection.Close()
                        Me.InfoTextBox.Text = "Scribe not correct"
                        Exit Sub
                    End If
                    MailStringBuild = MailStringBuild + "Instence: " & IKey & Chr(13)
                    MailStringBuild = MailStringBuild + "Wafer Count: " & Slot & Chr(13)
                    MailStringBuild = MailStringBuild + "T7 by Slot " & Chr(13)

                    T7Ver = True
                End If
            Else
                KickOut = True
                KickOutI = i
                Exit For
            End If
            MailStringBuild = MailStringBuild + "Slot: " & Slot & " T7: " & DR_WatP("T7") & Chr(13)

            If Not DR_SP1("RFID").ToString.Contains("Error") Or DR_SP1("RFID").ToString.Contains("No") Then
                MyRFID = DR_SP1("RFID").ToString
            End If

        Next

        Connection.Close()
        AutoDataConnection.Close()

        Dim LabelID As String

        LabelID = SatiCode.IDForSP1Reciver(IDPass, Station) 'get the id for labeling

        If KickOut = True Then
            DR_SP1 = DS_SP1.Tables(0).Rows(KickOutI - 1)
        End If

        DR_SP1 = DS_SP1.Tables(0).Rows(0) 'set row to get the last slot in cassetes Run# and WL#
        TheLotNumber = LabelID & "-" & DR_SP1("RUN#") & "-" & DR_SP1("Wafer_log")

        Dim MakeLabel As Boolean = True

        '************Check to make sure Lot Number is REAL*******************
        If SatiCode.IsLotNumberReal(DR_SP1("ID#") & "-" & DR_SP1("RUN#") & "-" & DR_SP1("Wafer_log")) = False Then
            MakeLabel = False
            Me.InfoTextBox.Text = "The Lot Number Is Not Valid..... You Will Need To Make A Instance And Rerun With The Correct Lot Number."
            SatiCode.SendMail_To_From("Sati Just Blocked A Label From Being Made In The Cleanroom. The Lot Number Was Not Valid. The Information Entered Was Incorect. Invalid Lot Number = " & TheLotNumber & " Tool = " & tool & " Tool User = " & DR_SP1("Comment1"), "Label Print Block", "az.SatiAlert@purewafer.com", "Sati@purewafer.com")
            Exit Sub
        End If
        '********************************************************************

        '*****************Check Instance For T7 Scribe Validation *******************************
        Dim CheckT7Requirement As String = SatiCode.Check_For_T7_Requierments(LabelID, IKey)
        If Not CheckT7Requirement = "Good" Then
            Me.InfoTextBox.Text = CheckT7Requirement & ". Rerun under Instance Number " & IKey & " and remove bad wafer."
            MakeLabel = False
        End If

        '****************************************************************************************


        Dim CustomerName As String = SatiCode.GetCustomerID(LabelID)

        '**********************************************************************
        '*************** Intel Check For Dupes ********************************
        '**********************************************************************

        CustomerName = UCase(CustomerName)
        Dim Whitelist As Boolean = False
        If CustomerName.Contains("INTEL") Then
            'If Not CustomerName = ("INTEL-CHINA") Then
            If LabelID = "6610" Then
                Whitelist = True
            End If
            Dim DupeCheck As String = SatiCode.CheckSeqDupe("I", IKey, Whitelist)
            If Not DupeCheck = "" Then
                Me.InfoTextBox.Text = DupeCheck
                MakeLabel = False
            End If
            'End If
        End If

        Dim File As String

        '*********************************************************************
        '**************** RFID Check *****************************************
        '*********************************************************************
        If SatiCode.IS_RFID_Enable(LabelID) = True Then

            If MyRFID = "" Then
                RFID_Info = False
            End If



            If CustomerName.Contains("Global") Or CustomerName.Contains("Global") Then

                'code to check the RFID. look for all number or no alfa?

            End If









            If SatiCode.Check_RFID_Used_Last20Days(MyRFID, SPxSlot1CreationDate) = True Then
                MakeLabel = False
                SatiCode.MakeLabel(False, "ReRun", "PWC", LabelID, TheLotNumber, Qty, 1, IKey, Printer, "", 0, "", "", New Data.DataSet, "WB", "", User.Identity.Name.ToString, False, 0)
                SatiCode.SendMail_To_From("Sati just blocked a label from being made due to an RFID tag that has been used within the last 20 days. RFID# " & MyRFID, "RFID Lock", "az.SatiAlert@purewafer.com", "Sati@purewafer.com")
                Me.InfoTextBox.Text = "Re Run Label Was Made"
            End If

            If RFID_Info = False Then
                'print instance rerun lable
                MakeLabel = False
                SatiCode.MakeLabel(False, "ReRun", "PWC", LabelID, TheLotNumber, Qty, 1, IKey, Printer, "", 0, "", "", New Data.DataSet, "WB", "", User.Identity.Name.ToString, False, 0)
                Me.InfoTextBox.Text = "Re Run Label Was Made"
            End If

        End If


        '**********************************************************************
        '***************IF this ID needs to write to RFID Pill*****************
        '**********************************************************************
        If LabelID = "3265" Then
            'F7FRxxxxxxxx
            '1st = F
            '2nd = 7 for Fab 7
            '3rd = F for Mirial KT-3004 A4
            '4th = R for Purewafer
            '5-12 = Instance number, Padding 0's on left

            Dim RFIDString As String = "0"

            Select Case IKey.ToString.Length
                Case 6
                    RFIDString = "00" & IKey.ToString
                Case 7
                    RFIDString = "0" & IKey.ToString
            End Select

            RFIDString = "F7FA" & RFIDString

            If SatiCode.RFID_WriteTable("Make", IKey, tool, Station, RFIDString) = "Data Record Made" Then
                'We need to wait up to 30 seconds for RFID Read/Writers to compleat
                For ImGoingToWait As Int16 = 0 To 5
                    Threading.Thread.Sleep(5000)

                    Select Case SatiCode.RFID_WriteTable("Look", IKey, tool, Station, RFIDString)
                        Case "Wrote Tag"
                            'Print label for infopad
                            SatiCode.MakeLabel(False, "InfoPad", "", LabelID, TheLotNumber, Qty, 1, 0, Printer, "", 0, "", "", New Data.DataSet, "WB", RFIDString, User.Identity.Name.ToString, False, 0)
                            Exit For

                        Case "No Record Found"
                            Me.InfoTextBox.Text = "No Record Found In The RFID DB Table"
                    End Select

                    ImGoingToWait = ImGoingToWait + 1
                Next

            Else
                Me.InfoTextBox.Text = "Could Not Write To RFID DB Table"
            End If
        End If




        If MakeLabel = True Then

            File = SatiCode.MakeLabel(False, "WB", "PWC", LabelID, TheLotNumber, Qty, 1, IKey, Printer, "", 0, "", "", New Data.DataSet, "WB", MyRFID, User.Identity.Name.ToString, False, 0)
            'MailStringBuild = MailStringBuild + "Wafer Box Number Used was: " & File & Chr(13)

            'SatiCode.SendMail(MailStringBuild, "SPx Label Made", "Info")
            Me.InfoTextBox.Text = "Label Was Made" & Chr(13) & File
        End If


    End Sub

    Protected Sub ToolListBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Ready4Ver()
    End Sub

    Sub Ready4Ver()
        If Me.ToolListBox.SelectedIndex = -1 Then
            Me.Button1.Visible = False
            'Me.VerButton.Visible = False
            Exit Sub
        End If
        If Me.StationListBox.SelectedIndex = -1 Then
            'Me.VerButton.Visible = False
            Me.Button1.Visible = False
            Exit Sub
        End If
        Me.Button1.Visible = True
        'Me.VerButton.Visible = True

    End Sub

    Protected Sub StationListBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Ready4Ver()
    End Sub

    Protected Sub VerButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        VerTestCode()
    End Sub

    Function TestT7(ByVal T7 As String, ByVal M12 As String) As Boolean
        'If Button1.Visible = False Then ' override for testing
        T7 = UCase(T7)
            M12 = UCase(M12)
            If T7 = M12 Then
                Return True
            End If
            If T7 = Left(M12, 10) Then
                Return True
            End If
        'Else
        'Return True ' override for testing
        'End If


    End Function

    Sub VerTestCode()
        Me.InfoTextBox.Text = ""
        Dim last4 As String
        last4 = Me.M12LaserMarkTextBox.Text
        If last4.Length < 10 Then
            Me.InfoTextBox.Text = "Try Re-Typing The Laser Scribe"
            Exit Sub
        End If
        If TestT7("1367E086SE", Me.M12LaserMarkTextBox.Text) = False Then
            Me.InfoTextBox.Text = "Scribe not correct"
        Else
            Me.InfoTextBox.Text = "GOOD Pass"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub


    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim Printer As String
        Me.InfoTextBox.Text = ""
        Dim last4 As String
        last4 = Me.M12LaserMarkTextBox.Text
        If last4.Length < 10 Then
            Me.InfoTextBox.Text = "Try Re-Typing The Laser Scribe"
            Exit Sub
        End If
        If Not Me.PrinterDropDownList.SelectedItem.Text = "Select Printer..." Then
            Printer = "\\PWI-40\" & Me.PrinterDropDownList.SelectedItem.Text
        Else
            Exit Sub
        End If

        GoSPxSQL(Me.ToolListBox.SelectedValue, Me.StationListBox.SelectedValue, Printer)
    End Sub


End Class
