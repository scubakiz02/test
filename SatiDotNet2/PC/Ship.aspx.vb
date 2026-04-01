
Partial Class PC_Ship
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub

    Protected Sub TextBoxScanIn_TextChanged(sender As Object, e As EventArgs) Handles TextBoxScanIn.TextChanged
        Dim TheScan As String = TextBoxScanIn.Text
        UCase(TheScan)

        If Left(TheScan, 2) = "CB" Then
            CB_Run(Mid(TheScan, 3))
        End If

        If Left(TheScan, 1) = "C" Then
            C_Run(Mid(TheScan, 2))
        End If


        Me.TextBoxScanInfo.Rows = Me.TextBoxScanInfo.Rows + 1
        Me.TextBoxScanInfo.Text = TextBoxScanIn.Text & Chr(13) & Me.TextBoxScanInfo.Text
        TextBoxScanIn.Text = ""

        CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.TextBoxScanIn.ClientID)

    End Sub

    Sub C_Run(Scan As String)



    End Sub

    Sub CB_Run(Scan As String)
        Dim DS_MyBox As New Data.DataSet
        Dim DR_MyBox As Data.DataRow

        Dim MyBoxRowCount As Int16
        Dim Qty As Integer
        Dim CartonCount As Int16
        Dim TheCustomer As String
        Dim Whitelist As Boolean = False


        'First time setup
        If LabelSRN.Text = "0" Then
            Dim DS_Setup As New Data.DataSet
            Dim DR_Setup As Data.DataRow
            DS_Setup = SatiCode.GetMyDataSet("SELECT dbo.T_FGI_Boxes.CartonNumber, dbo.LabelsMade.Lot, dbo.LabelsMade.Wafers AS Qty, dbo.MainIDSpec.PART_NUMBER AS Part, dbo.MainIDSpec.PART_REV_NUMBER, dbo.MainIDSpec.SPEC_NUMBER AS Spec, dbo.MainIDSpec.SPEC_REV_NUMBER AS SpecRev, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number AS PO, dbo.MainID.PO_On_Label, dbo.T_FGI_Boxes.InstanceKey AS Ikey, LEFT(dbo.LabelsMade.Lot, 4) AS LotID, dbo.MainID.CustomerID, dbo.MainID.Diameter, dbo.MainID.CrossFabShip FROM dbo.MainIDSpec INNER JOIN dbo.LabelsMade ON dbo.MainIDSpec.RecordNumber = dbo.LabelsMade.RecordNumber INNER JOIN dbo.T_FGI_Boxes ON dbo.LabelsMade.LabelRecordNumber = dbo.T_FGI_Boxes.LabelsMadeKey INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO INNER JOIN dbo.MainID_MainIDSpec ON dbo.MainIDSpec.RecordNumber = dbo.MainID_MainIDSpec.WaferSpec_Key INNER JOIN dbo.MainID ON dbo.MainID_MainIDSpec.MainID = dbo.MainID.MainID WHERE (dbo.T_FGI_Boxes.CartonNumber = " & Scan & ")")
            If DS_Setup.Tables(0).Rows.Count = 1 Then
                DR_Setup = DS_Setup.Tables(0).Rows(0)
                Me.LabelID.Text = DR_Setup("LotID")
                Me.LabelCustomerID.Text = DR_Setup("CustomerID")
                Me.LabelDiameter.Text = DR_Setup("Diameter")
                Me.LabelSO.Text = DR_Setup("SO")
                Me.LabelPO.Text = DR_Setup("PO")

                Me.LabelPOonLabel.Text = DR_Setup("PO_On_Label")

                Me.LabelCrossFab.Text = DR_Setup("CrossFabShip")

                Me.LabelPart.Text = DR_Setup("Part")
                Me.LabelPartRev.Text = DR_Setup("PART_REV_NUMBER")
                Me.LabelSpec.Text = DR_Setup("Spec")
                Me.LabelSpecRev.Text = DR_Setup("SpecRev")

                If DR_Setup("Diameter") = 300 Then
                    Me.LabelBulk.Text = "True"
                Else
                    Me.LabelBulk.Text = "False"
                End If

                Me.LabelSRN.Text = SRN("Get")
                SRN("Write")

            End If
        End If

        'Get Box Data and start checks
        DS_MyBox = SatiCode.GetMyDataSet("SELECT dbo.T_FGI_Boxes.CartonNumber, dbo.LabelsMade.Lot, dbo.LabelsMade.Wafers AS Qty, dbo.MainIDSpec.PART_NUMBER AS Part, dbo.MainIDSpec.PART_REV_NUMBER, dbo.MainIDSpec.SPEC_NUMBER AS Spec, dbo.MainIDSpec.SPEC_REV_NUMBER AS SpecRev, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number AS PO, dbo.T_FGI_Boxes.InstanceKey AS Ikey, LEFT(dbo.LabelsMade.Lot, 4) AS LotID, dbo.MainID.CustomerID, dbo.Customer.Customer_Name FROM dbo.MainIDSpec INNER JOIN dbo.LabelsMade ON dbo.MainIDSpec.RecordNumber = dbo.LabelsMade.RecordNumber INNER JOIN dbo.T_FGI_Boxes ON dbo.LabelsMade.LabelRecordNumber = dbo.T_FGI_Boxes.LabelsMadeKey INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO INNER JOIN dbo.MainID_MainIDSpec ON dbo.MainIDSpec.RecordNumber = dbo.MainID_MainIDSpec.WaferSpec_Key INNER JOIN dbo.MainID ON dbo.MainID_MainIDSpec.MainID = dbo.MainID.MainID INNER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID WHERE (dbo.T_FGI_Boxes.CartonNumber = " & Scan & ")")
        MyBoxRowCount = DS_MyBox.Tables(0).Rows.Count

        If MyBoxRowCount = 0 Then
            Me.TextBoxScanInfo.Rows = Me.TextBoxScanInfo.Rows + 1
            Me.TextBoxScanInfo.Text = TextBoxScanIn.Text & Chr(13) & "Carton " & Scan & " Not Found. Carton Not Added!!"
            Exit Sub
        End If

        For i As Int16 = 0 To MyBoxRowCount - 1
            DR_MyBox = DS_MyBox.Tables(0).Rows(i)

            'Check ID
            If Not Me.LabelID.Text = DR_MyBox("LotID").ToString Then
                Me.TextBoxScanInfo.Rows = Me.TextBoxScanInfo.Rows + 1
                Me.TextBoxScanInfo.Text = TextBoxScanIn.Text & Chr(13) & " Wrong ID. Carton Not Added!!"
                Exit Sub
            End If

            'Check PO and SO information
            If Me.LabelPOonLabel.Text = "True" Then
                'SO
                If Not Me.LabelSO.Text = DR_MyBox("SO").ToString Then
                    Me.TextBoxScanInfo.Rows = Me.TextBoxScanInfo.Rows + 1
                    Me.TextBoxScanInfo.Text = TextBoxScanIn.Text & Chr(13) & " Wrong SO. Carton Not Added!!"
                    Exit Sub
                End If
                'PO
                If Not Me.LabelPO.Text = DR_MyBox("PO").ToString Then
                    Me.TextBoxScanInfo.Rows = Me.TextBoxScanInfo.Rows + 1
                    Me.TextBoxScanInfo.Text = TextBoxScanIn.Text & Chr(13) & " Wrong PO. Carton Not Added!!"
                    Exit Sub
                End If
            End If

            'Spec
            If Not Me.LabelSpec.Text = DR_MyBox("Spec").ToString Then
                Me.TextBoxScanInfo.Rows = Me.TextBoxScanInfo.Rows + 1
                Me.TextBoxScanInfo.Text = TextBoxScanIn.Text & Chr(13) & " Wrong Spec. Carton Not Added!!"
                Exit Sub
            End If

            'Spec Rev
            If Not Me.LabelSpecRev.Text = DR_MyBox("SpecRev").ToString Then
                Me.TextBoxScanInfo.Rows = Me.TextBoxScanInfo.Rows + 1
                Me.TextBoxScanInfo.Text = TextBoxScanIn.Text & Chr(13) & " Wrong Spec Rev. Carton Not Added!!"
                Exit Sub
            End If

            'Part Number
            If Not Me.LabelPart.Text = DR_MyBox("Part").ToString Then
                Me.TextBoxScanInfo.Rows = Me.TextBoxScanInfo.Rows + 1
                Me.TextBoxScanInfo.Text = TextBoxScanIn.Text & Chr(13) & " Wrong Part Number. Carton Not Added!!"
                Exit Sub
            End If

            'Part Number Rev
            If Not Me.LabelPartRev.Text = DR_MyBox("PART_REV_NUMBER").ToString Then
                Me.TextBoxScanInfo.Rows = Me.TextBoxScanInfo.Rows + 1
                Me.TextBoxScanInfo.Text = TextBoxScanIn.Text & Chr(13) & " Wrong Part Number Rev. Carton Not Added!!"
                Exit Sub
            End If


            '********************************************
            'Special Checks *****************************
            '********************************************
            TheCustomer = DR_MyBox("CustomerID").ToString
            TheCustomer = UCase(TheCustomer)

            'Check to make sure the FGI box was verified
            If TheCustomer.Contains("IBM") Or TheCustomer.Contains("GF") Then
                If SatiCode.FGI_RFID_Checked(Scan) = False Then
                    Me.TextBoxScanInfo.Rows = Me.TextBoxScanInfo.Rows + 1
                    Me.TextBoxScanInfo.Text = TextBoxScanIn.Text & Chr(13) & " RFID Was Not Verified. Carton Not Added!!"
                    Exit Sub
                End If
                'check for 3212 scribes
            End If

            'Dupe check
            If Me.LabelID.Text = "6610" Then
                Whitelist = True
            End If
            If TheCustomer.Contains("INTEL") Then
                'If Not TheCustomer = ("INTEL-CHINA") Then
                Dim DupeCheck As String = ""
                    DupeCheck = SatiCode.CheckSeqDupe("I", DR_MyBox("Ikey").ToString, Whitelist)
                    If Not DupeCheck = "" Then
                        Me.TextBoxScanInfo.Rows = Me.TextBoxScanInfo.Rows + 1
                        Me.TextBoxScanInfo.Text = TextBoxScanIn.Text & Chr(13) & "Dupe Check Function Returned " & DupeCheck & " Carton Not Added"
                        Exit Sub
                    End If
                'End If
            End If

            'Check for T7 requirements
            Dim CheckT7Requirement As String = SatiCode.Check_For_T7_Requierments(DR_MyBox("LotID").ToString, DR_MyBox("Ikey").ToString)
            If Not CheckT7Requirement = "Good" Then
                Me.TextBoxScanInfo.Rows = Me.TextBoxScanInfo.Rows + 1
                Me.TextBoxScanInfo.Text = TextBoxScanIn.Text & Chr(13) & CheckT7Requirement & ". Rerun under Instance Number " & DR_MyBox("Ikey").ToString & " and remove bad wafer. Carton Not Added"
                Exit Sub
            End If

            'Missing data
            Dim DataScan As String = ""
            If TheCustomer.Contains("MICRON") Or TheCustomer.Contains("IBM") Or TheCustomer.Contains("GF") Then

                DataScan = SatiCode.CBFullDataRecordCheck(Scan, "All")

                If Not DataScan = "No Problems" Then
                    SatiCode.CB_CheckAndFix_Geo(Scan) 'fixes any pre data

                    'If SatiCode.CB_CheckAndFix_Geo(Scan) = 0 Then 'fixes any pre data
                    'Warn = Warn + 1
                    'Me.WarnLabel.Visible = True
                    'Else
                    'DataScan = SatiCode.CBFullDataRecordCheck(Scan, "All")
                    'If Not DataScan = "No Problems" Then
                    'Warn = Warn + 1
                    'Me.WarnLabel.Visible = True
                    'End If
                    'End If
                End If
            End If


            Me.LabelWaferQty.Text = Me.LabelWaferQty.Text + MyBoxRowCount

        Next


        'Add the Carton to the shipment
        SRN_Add(Scan)










    End Sub



    Private Sub TextBoxScanInfo_TextChanged(sender As Object, e As EventArgs) Handles TextBoxScanInfo.TextChanged

    End Sub


    Function SRN(CMD As String) As Integer

        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim MySelectCmd As New System.Data.SqlClient.SqlCommand
        With MySelectCmd
            .CommandText = "SELECT SRN, EventTime, ShipmentNumber, Diameter, CustomerID, MainID, PO, SO, Part, Spec FROM T_Sati_SRN WHERE (SRN = " & Me.LabelSRN.Text & ")"
            .Connection = Connection
        End With
        DA.SelectCommand = MySelectCmd

        Dim MyInsertCmd As New System.Data.SqlClient.SqlCommand
        With MyInsertCmd
            .CommandText = "INSERT INTO [T_Sati_SRN] ([EventTime], [ShipmentNumber], [Diameter], [CustomerID], [MainID], [PO], [SO], [Part], [Spec]) VALUES (@EventTime, @ShipmentNumber, @Diameter, @CustomerID, @MainID, @PO, @SO, @Part, @Spec); SELECT SRN, EventTime, ShipmentNumber, Diameter, CustomerID, MainID, PO, SO, Part, Spec FROM T_Sati_SRN WHERE (SRN = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@EventTime", System.Data.SqlDbType.SmallDateTime, 0, "EventTime"), New System.Data.SqlClient.SqlParameter("@ShipmentNumber", System.Data.SqlDbType.NVarChar, 0, "ShipmentNumber"), New System.Data.SqlClient.SqlParameter("@Diameter", System.Data.SqlDbType.Int, 0, "Diameter"), New System.Data.SqlClient.SqlParameter("@CustomerID", System.Data.SqlDbType.NVarChar, 0, "CustomerID"), New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@PO", System.Data.SqlDbType.NVarChar, 0, "PO"), New System.Data.SqlClient.SqlParameter("@SO", System.Data.SqlDbType.NVarChar, 0, "SO"), New System.Data.SqlClient.SqlParameter("@Part", System.Data.SqlDbType.NVarChar, 0, "Part"), New System.Data.SqlClient.SqlParameter("@Spec", System.Data.SqlDbType.NVarChar, 0, "Spec")})
        End With
        DA.InsertCommand = MyInsertCmd

        Dim MyUpdateCmd As New System.Data.SqlClient.SqlCommand
        With MyUpdateCmd
            .CommandText = "UPDATE [T_Sati_SRN] SET [EventTime] = @EventTime, [ShipmentNumber] = @ShipmentNumber, [Diameter] = @Diameter, [CustomerID] = @CustomerID, [MainID] = @MainID, [PO] = @PO, [SO] = @SO, [Part] = @Part, [Spec] = @Spec WHERE (([SRN] = @Original_SRN) AND ([EventTime] = @Original_EventTime) AND ((@IsNull_ShipmentNumber = 1 AND [ShipmentNumber] IS NULL) OR ([ShipmentNumber] = @Original_ShipmentNumber)) AND ((@IsNull_Diameter = 1 AND [Diameter] IS NULL) OR ([Diameter] = @Original_Diameter)) AND ((@IsNull_CustomerID = 1 AND [CustomerID] IS NULL) OR ([CustomerID] = @Original_CustomerID)) AND ((@IsNull_MainID = 1 AND [MainID] IS NULL) OR ([MainID] = @Original_MainID)) AND ((@IsNull_PO = 1 AND [PO] IS NULL) OR ([PO] = @Original_PO)) AND ((@IsNull_SO = 1 AND [SO] IS NULL) OR ([SO] = @Original_SO)) AND ((@IsNull_Part = 1 AND [Part] IS NULL) OR ([Part] = @Original_Part)) AND ((@IsNull_Spec = 1 AND [Spec] IS NULL) OR ([Spec] = @Original_Spec))); SELECT SRN, EventTime, ShipmentNumber, Diameter, CustomerID, MainID, PO, SO, Part, Spec FROM T_Sati_SRN WHERE (SRN = @SRN)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@EventTime", System.Data.SqlDbType.SmallDateTime, 0, "EventTime"), New System.Data.SqlClient.SqlParameter("@ShipmentNumber", System.Data.SqlDbType.NVarChar, 0, "ShipmentNumber"), New System.Data.SqlClient.SqlParameter("@Diameter", System.Data.SqlDbType.Int, 0, "Diameter"), New System.Data.SqlClient.SqlParameter("@CustomerID", System.Data.SqlDbType.NVarChar, 0, "CustomerID"), New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@PO", System.Data.SqlDbType.NVarChar, 0, "PO"), New System.Data.SqlClient.SqlParameter("@SO", System.Data.SqlDbType.NVarChar, 0, "SO"), New System.Data.SqlClient.SqlParameter("@Part", System.Data.SqlDbType.NVarChar, 0, "Part"), New System.Data.SqlClient.SqlParameter("@Spec", System.Data.SqlDbType.NVarChar, 0, "Spec"), New System.Data.SqlClient.SqlParameter("@Original_SRN", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SRN", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_EventTime", System.Data.SqlDbType.SmallDateTime, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "EventTime", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_ShipmentNumber", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "ShipmentNumber", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_ShipmentNumber", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ShipmentNumber", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Diameter", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Diameter", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Diameter", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Diameter", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_CustomerID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "CustomerID", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_CustomerID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CustomerID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_MainID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "MainID", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_MainID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MainID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_PO", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "PO", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_PO", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PO", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_SO", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "SO", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_SO", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SO", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Part", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Part", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Part", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Part", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Spec", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Spec", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Spec", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Spec", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@SRN", System.Data.SqlDbType.Int, 4, "SRN")})
        End With
        DA.UpdateCommand = MyUpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_Sati_SRN", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("SRN", "SRN"), New System.Data.Common.DataColumnMapping("EventTime", "EventTime"), New System.Data.Common.DataColumnMapping("ShipmentNumber", "ShipmentNumber"), New System.Data.Common.DataColumnMapping("Diameter", "Diameter"), New System.Data.Common.DataColumnMapping("CustomerID", "CustomerID"), New System.Data.Common.DataColumnMapping("MainID", "MainID"), New System.Data.Common.DataColumnMapping("PO", "PO"), New System.Data.Common.DataColumnMapping("SO", "SO"), New System.Data.Common.DataColumnMapping("Part", "Part"), New System.Data.Common.DataColumnMapping("Spec", "Spec")})})
        DA.Fill(DS)

        Select Case CMD
            Case "Get"

                DR = DS.Tables("T_Sati_SRN").NewRow 'InstanceNumber, SP2, SP2Station, RFID_String, Info
                DR("EventTime") = DateTime.Now.ToShortDateString

                DS.Tables("T_Sati_SRN").Rows.Add(DR)
                DA.Update(DS, "T_Sati_SRN")
                SRN = DR("SRN")

            Case "Write"

                DR = DS.Tables(0).Rows(0)
                DR.AcceptChanges()
                DR.BeginEdit()
                DR("Diameter") = Me.LabelDiameter.Text
                DR("CustomerID") = Me.LabelCustomerID.Text
                DR("MainID") = Me.LabelID.Text
                DR("PO") = Me.LabelPO.Text
                DR("SO") = Me.LabelSO.Text
                DR("Part") = Me.LabelPart.Text
                DR("Spec") = Me.LabelSpec.Text

                DR.EndEdit()
                DA.Update(DS, "T_Sati_SRN")

            Case "ShipNum"

                DR = DS.Tables(0).Rows(0)
                DR.AcceptChanges()
                DR.BeginEdit()
                DR("ShipmentNumber") = Me.LabelShipNum.Text

                DR.EndEdit()
                DA.Update(DS, "T_Sati_SRN")


        End Select
        Connection.Close()

    End Function

    Sub SRN_Add(Carton As String)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim MySelectCmd As New System.Data.SqlClient.SqlCommand
        With MySelectCmd
            .CommandText = "SELECT [Key], SRN, CartonNumber FROM T_Sati_SRN_Items WHERE ([Key] = 0)"
            .Connection = Connection
        End With
        DA.SelectCommand = MySelectCmd

        Dim MyInsertCmd As New System.Data.SqlClient.SqlCommand
        With MyInsertCmd
            .CommandText = "INSERT INTO [T_Sati_SRN_Items] ([SRN], [CartonNumber]) VALUES (@SRN, @CartonNumber); SELECT [Key], SRN, CartonNumber FROM T_Sati_SRN_Items WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@SRN", System.Data.SqlDbType.Int, 0, "SRN"), New System.Data.SqlClient.SqlParameter("@CartonNumber", System.Data.SqlDbType.Int, 0, "CartonNumber")})
        End With
        DA.InsertCommand = MyInsertCmd

        Dim MyUpdateCmd As New System.Data.SqlClient.SqlCommand
        With MyUpdateCmd
            .CommandText = "UPDATE [T_Sati_SRN_Items] SET [SRN] = @SRN, [CartonNumber] = @CartonNumber WHERE (([Key] = @Original_Key) AND ([SRN] = @Original_SRN) AND ([CartonNumber] = @Original_CartonNumber)); SELECT [Key], SRN, CartonNumber FROM T_Sati_SRN_Items WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@SRN", System.Data.SqlDbType.Int, 0, "SRN"), New System.Data.SqlClient.SqlParameter("@CartonNumber", System.Data.SqlDbType.Int, 0, "CartonNumber"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SRN", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SRN", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CartonNumber", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CartonNumber", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = MyUpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_Sati_SRN_Items", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("SRN", "SRN"), New System.Data.Common.DataColumnMapping("CartonNumber", "CartonNumber")})})
        DA.Fill(DS)



        DR = DS.Tables("T_Sati_SRN_Items").NewRow
        DR("SRN") = Me.LabelSRN.Text
        DR("CartonNumber") = Carton

        DS.Tables("T_Sati_SRN_Items").Rows.Add(DR)
        DA.Update(DS, "T_Sati_SRN_Items")

        Connection.Close()

    End Sub


    Protected Sub ButtonLoadSRN_Page_Click(sender As Object, e As EventArgs) Handles ButtonLoadSRN_Page.Click
        Response.Redirect(Session("SRN").ToString & "?SRN=" & Me.LabelSRN.Text)
    End Sub

    Protected Sub ButtonMakeSRN_Click(sender As Object, e As EventArgs) Handles ButtonMakeSRN.Click
        Me.PanelMakeSRN_ModalPopupExtender.Show()
    End Sub

    Protected Sub CarrierDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CarrierDropDownList.SelectedIndexChanged
        MakeshippmentButtom_OkToView()
    End Sub

    Protected Sub FreightDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FreightDropDownList.SelectedIndexChanged
        MakeshippmentButtom_OkToView()
    End Sub

    Protected Sub FreightAccountDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FreightAccountDropDownList.SelectedIndexChanged
        MakeshippmentButtom_OkToView()
    End Sub

    Sub MakeshippmentButtom_OkToView()
        If Not CarrierDropDownList.SelectedValue.ToString = "Select..." Then
            If Not FreightDropDownList.SelectedValue.ToString = "Select..." Then
                If Not FreightAccountDropDownList.SelectedValue.ToString = "Select..." Then
                    Me.ButtonMakeShipmentSRN.Visible = True
                End If
            End If
        End If
        Me.PanelMakeSRN_ModalPopupExtender.Show()
    End Sub

    Protected Sub ButtonMakeShipmentSRN_Click(sender As Object, e As EventArgs) Handles ButtonMakeShipmentSRN.Click

        'Once the shippment was made succesfully then show other buttons
        Me.ButtonLoadSRN_Page.Visible = True
        Me.ButtonPrint.Visible = True
        Me.ButtonMakeSRN.Visible = False

    End Sub
End Class
