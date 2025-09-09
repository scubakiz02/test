
Partial Class CustomerMaintenance_CustomersIDsSpecs
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)

        If Not Me.IsPostBack Then
            Me.SqlDataSourceCustomerSelected.SelectCommand = ""
            Me.DataBind()
        End If
    End Sub

    Sub ClearPage()
        Me.SqlDataSourceCustomerSelected.SelectCommand = ""
        Me.PanelCustomerSelected.Visible = False
        Me.PanelCustomerNew.Visible = False
        Me.LabelWorkingFab.Text = ""
        Me.PanelSelectedFab.Visible = False
        Me.PanelSpecCurrent.Visible = False


    End Sub

    Sub SelectedCustomer()
        Select Case Me.DropDownListCustomersList.SelectedItem.Text
            Case "Select One..."
                ClearPage()
            Case "New Customer..."
                ClearPage()
                Me.PanelCustomerNew.Visible = True

            Case Else
                Me.PanelCustomerNew.Visible = False
                Me.PanelCustomerSelected.Visible = True
                Me.SqlDataSourceCustomerSelected.SelectCommand = "SELECT CustomerID, Customer_Name, Business_Name, MacolaID, Supplier_Number, Transit_Days, Cross_Location, Note1, Note2, PackingSlip_Note, Operator, EventTime FROM dbo.Customer WHERE (Customer_Name = N'" & Me.DropDownListCustomersList.SelectedItem.Text & "') ORDER BY CustomerID"
                Me.GridViewSelectedCustomer.DataBind()
        End Select

    End Sub

    Sub RefreshCustomerList()
        Me.DropDownListCustomersList.Items.Clear()
        Me.DropDownListCustomersList.Items.Add("Select One...")
        Me.DropDownListCustomersList.Items.Add("New Customer...")
        Me.DropDownListCustomersList.DataBind()
    End Sub

    Protected Sub DropDownListCustomersList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListCustomersList.SelectedIndexChanged
        ClearPage()
        SelectedCustomer()
    End Sub

    Protected Sub GridViewSelectedCustomer_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewSelectedCustomer.RowCommand
        If e.CommandName = "GetFab" Then
            Me.PanelSelectedFab.Visible = True
            Dim row As String
            Dim Fab As String
            row = e.CommandArgument.ToString
            Fab = Me.GridViewSelectedCustomer.Rows(row).Cells(1).Text
            Me.LabelWorkingFab.Text = Fab
            Me.SqlDataSourceFabIDs.SelectCommand = "SELECT MainID, CustomerID FROM dbo.MainID WHERE (CustomerID = N'" & Fab & "') ORDER BY MainID"
            Me.DropDownListIDs.Items.Clear()
            Me.DropDownListIDs.Items.Add("Select One...")
            Me.DropDownListIDs.Items.Add("New ID...")
            Me.DropDownListIDs.DataBind()

        Else
            SelectedCustomer()
        End If

    End Sub

    Protected Sub DetailsViewNewCustomer_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewInsertedEventArgs) Handles DetailsViewNewCustomer.ItemInserted
        RefreshCustomerList()
        ClearPage()
    End Sub

   
    Protected Sub DropDownListIDs_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListIDs.SelectedIndexChanged
        SelectedID()

    End Sub

    Sub SelectedID()
        'Me.FormViewSelectedID.Visible = True
        Select Case Me.DropDownListIDs.SelectedItem.Text

            Case "Select One..."
                Me.SqlDataSourceSelectedID.SelectCommand = ""
                Me.PanelSpecCurrent.Visible = False
                Me.PanelBar.Visible = False
            Case "New ID..."
                Me.PanelIDDetail.Visible = True
                If Not Me.FormViewSelectedID.CurrentMode = FormViewMode.Insert Then
                    Me.FormViewSelectedID.ChangeMode(FormViewMode.Insert)
                    Me.FormViewSelectedID.DefaultMode = FormViewMode.Insert
                    Me.SqlDataSourceSelectedID.SelectCommand = "SELECT MainID, CustomerID, [In-Out] AS column1, Diameter, WAFERS_PER_CASS, Minimum_Per_Cass, Cassette, PO_On_Label, EffectiveDtd, PackingSlip_Note, Operator, EventTime, Exsil_Supplied, Consignment, CrossFabShip, RFID_Enable FROM dbo.MainID WHERE (MainID = N'')"
                    Me.FormViewSelectedID.DataBind()
                    CType(Me.FormViewSelectedID.FindControl("FabTextBox"), TextBox).Text = Me.LabelWorkingFab.Text

                    CType(Me.FormViewSelectedID.FindControl("WPC_Textbox"), TextBox).Text = "25"
                    CType(Me.FormViewSelectedID.FindControl("MPC_Textbox"), TextBox).Text = "25"
                    CType(Me.FormViewSelectedID.FindControl("CPB_Textbox"), TextBox).Text = "2"
                    CType(Me.FormViewSelectedID.FindControl("ET_Textbox"), TextBox).Text = DateTime.Now.ToShortDateString
                    CType(Me.FormViewSelectedID.FindControl("ED_Textbox"), TextBox).Text = DateTime.Now.ToShortDateString
                    CType(Me.FormViewSelectedID.FindControl("UserTextbox"), TextBox).Text = User.Identity.Name.ToString
                End If
                'Me.PanelSpecCurrent.Visible = False
                Me.PanelBar.Visible = False
            Case Else
                SetView()
                'Me.PanelNewSpec.Visible = False
                Me.PanelBar.Visible = True
        End Select
    End Sub

    Sub LoadAddress()
        Dim dS As New Data.DataSet
        Dim DR As Data.DataRow

        dS = Saticode.GetAddress("Shipping", Me.DropDownListIDs.SelectedItem.Text, "")
        If Not dS.Tables(0).Rows.Count = 0 Then
            DR = dS.Tables(0).Rows(0)
            Me.LabelShippingLine1.Text = DR("Row1").ToString
            Me.LabelShippingLine2.Text = DR("Row2").ToString
            Me.LabelShippingLine3.Text = DR("Row3").ToString
            Me.LabelShippingLine4.Text = DR("Row4").ToString
            Me.LabelShippingLine5.Text = DR("Row5").ToString
            Me.LabelShippingLine6.Text = DR("Row6").ToString
        Else
            Me.LabelShippingLine1.Text = "None"
            Me.LabelShippingLine2.Text = "None"
            Me.LabelShippingLine3.Text = "None"
            Me.LabelShippingLine4.Text = "None"
            Me.LabelShippingLine5.Text = "None"
            Me.LabelShippingLine6.Text = "None"
        End If

        dS = Saticode.GetAddress("Billing", Me.DropDownListIDs.SelectedItem.Text, "")
        If Not dS.Tables(0).Rows.Count = 0 Then
            DR = dS.Tables(0).Rows(0)
            Me.LabelBillingLine1.Text = DR("Row1").ToString
            Me.LabelBillingLine2.Text = DR("Row2").ToString
            Me.LabelBillingLine3.Text = DR("Row3").ToString
            Me.LabelBillingLine4.Text = DR("Row4").ToString
            Me.LabelBillingLine5.Text = DR("Row5").ToString
            Me.LabelBillingLine6.Text = DR("Row6").ToString
        Else
            Me.LabelBillingLine1.Text = "None"
            Me.LabelBillingLine2.Text = "None"
            Me.LabelBillingLine3.Text = "None"
            Me.LabelBillingLine4.Text = "None"
            Me.LabelBillingLine5.Text = "None"
            Me.LabelBillingLine6.Text = "None"
        End If
    End Sub

    Sub SetView()
        If Me.PanelBar.Visible = True Then



        Else


        End If



        'Address *********************************************************************************************
        If Me.CheckBoxAddress.Checked Then
            'SELECT dbo.MainID_Address.Address_Key FROM dbo.MainID_Address INNER JOIN dbo.MainID ON dbo.MainID_Address.MainID = dbo.MainID.MainID WHERE (dbo.MainID.CustomerID = N'') AND (dbo.MainID_Address.Address_Type = 0) GROUP BY dbo.MainID_Address.Address_Key
            Me.PanelAddress.Visible = True

            LoadAddress()

            Me.PanelAddressChange.Visible = False
            
        Else
            Me.PanelAddressChange.Visible = False
            Me.PanelAddress.Visible = False
        End If


        'Defects *********************************************************************************************
        If Me.CheckBoxDefects.Checked Then
            Me.PanelDefects.Visible = True
            Me.SqlDataSourceCurrentDefects.SelectCommand = "SELECT [Key], Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE (ID = '" & Me.DropDownListIDs.SelectedItem.Text & "')"
            Me.GridViewDefects.DataBind()
            Me.SqlDataSourceAvailableDetects.SelectCommand = "SELECT Defect FROM dbo.T_ID_Defects GROUP BY Defect HAVING (NOT (Defect IN (SELECT Defect FROM dbo.T_ID_Defects AS T_ID_Defects_1 WHERE (ID = '" & Me.DropDownListIDs.SelectedItem.Text & "'))))"
            Me.DropDownListAvailableDefects.DataBind()

        Else
            Me.PanelDefects.Visible = False
        End If


        'Labels ***********************************************************************************
        If Me.CheckBoxLabels.Checked Then
            Me.PanelLabels.Visible = True

        Else
            Me.PanelLabels.Visible = False
        End If




        'Paths *************************************************************************************
        If Me.CheckBoxPaths.Checked Then
            Me.PanelPaths.Visible = True
            Me.DropDownListMainPath.SelectedValue = Saticode.CurrentPath(Me.DropDownListIDs.SelectedItem.Text, "Main")
            Me.DropDownListLapPath.SelectedValue = Saticode.CurrentPath(Me.DropDownListIDs.SelectedItem.Text, "LAP")
            Me.DropDownListCMPPath.SelectedValue = Saticode.CurrentPath(Me.DropDownListIDs.SelectedItem.Text, "CMP")
            Me.DropDownListDSPPath.SelectedValue = Saticode.CurrentPath(Me.DropDownListIDs.SelectedItem.Text, "DSP")
            Me.DropDownListPolishPath.SelectedValue = Saticode.CurrentPath(Me.DropDownListIDs.SelectedItem.Text, "POLISH")
            Me.DropDownListStripEtchPath.SelectedValue = Saticode.CurrentPath(Me.DropDownListIDs.SelectedItem.Text, "SE")
            Me.LabelPathsaved.Text = ""
        Else
            Me.PanelPaths.Visible = False
        End If

        'Customer Spec *******************************************************************************
        If Me.CheckCusomerSpec.Checked Then
            Me.PanelSpecCurrent.Visible = True
            Me.SqlDataSourceIDCurrentSpec.SelectCommand = "SELECT dbo.MainID_MainIDSpec.MainID, dbo.MainIDSpec.RecordNumber, dbo.MainIDSpec.PART_NUMBER, dbo.MainIDSpec.PART_REV_NUMBER, dbo.MainIDSpec.SPEC_NUMBER, dbo.MainIDSpec.SPEC_REV_NUMBER, dbo.MainIDSpec.thk_grp, dbo.MainIDSpec.res_grp, dbo.MainIDSpec.ORTN, dbo.MainIDSpec.WTYPE_DOPE, dbo.MainIDSpec.DOPE, dbo.MainIDSpec.SAMPLE_STANDARD, dbo.MainID_MainIDSpec.Label_Comments, dbo.MainID_MainIDSpec.Label_Comments2, dbo.MainID_MainIDSpec.Label_Comments3, dbo.MainID_MainIDSpec.EffectiveDtd, dbo.MainID_MainIDSpec.ExpirationDtd FROM dbo.MainIDSpec INNER JOIN dbo.MainID_MainIDSpec ON dbo.MainIDSpec.RecordNumber = dbo.MainID_MainIDSpec.WaferSpec_Key WHERE (dbo.MainID_MainIDSpec.MainID = N'" & Me.DropDownListIDs.SelectedItem.Text & "') AND (dbo.MainID_MainIDSpec.EffectiveDtd <= { fn NOW() }) AND (dbo.MainID_MainIDSpec.ExpirationDtd IS NULL OR dbo.MainID_MainIDSpec.ExpirationDtd >= { fn NOW() })"
            Me.DetailsViewIDCurrentSpec.DataBind()
        Else
            Me.PanelSpecCurrent.Visible = False
        End If

        'ID Details ********************************************************************************
        If Me.CheckIDDetail.Checked Then
            Me.PanelIDDetail.Visible = True
            If Not Me.FormViewSelectedID.CurrentMode = FormViewMode.Edit Then
                Me.FormViewSelectedID.ChangeMode(FormViewMode.ReadOnly)
                Me.FormViewSelectedID.DefaultMode = FormViewMode.ReadOnly
                Me.SqlDataSourceSelectedID.SelectCommand = "SELECT MainID, CustomerID, [In-Out] AS column1, Diameter, WAFERS_PER_CASS, Minimum_Per_Cass, Cassette, PO_On_Label, EffectiveDtd, PackingSlip_Note, Operator, EventTime, Exsil_Supplied, Consignment, CrossFabShip, RFID_Enable FROM dbo.MainID WHERE (MainID = N'" & Me.DropDownListIDs.SelectedItem.Text & "')"
                Me.FormViewSelectedID.DataBind()
                RefreshCrossIDShip()


            Else
                Me.SqlDataSourceSelectedID.SelectCommand = "SELECT MainID, CustomerID, [In-Out] AS column1, Diameter, WAFERS_PER_CASS, Minimum_Per_Cass, Cassette, PO_On_Label, EffectiveDtd, PackingSlip_Note, Operator, EventTime, Exsil_Supplied, Consignment, CrossFabShip, RFID_Enable FROM dbo.MainID WHERE (MainID = N'" & Me.DropDownListIDs.SelectedItem.Text & "')"
            End If
        Else
            Me.PanelIDDetail.Visible = False
        End If

    End Sub

    Sub RefreshCrossIDShip()
        Me.SqlDataSourceCrossFabShipID.SelectCommand = "SELECT [Key], MainID, TranID, Fab FROM T_Sati_CrossFabIDList WHERE (MainID = N'" & Me.DropDownListIDs.SelectedItem.Text & "') ORDER BY TranID"
        Me.GridViewCrossFabShipID.DataBind()

    End Sub


    Protected Sub FormViewSelectedID_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewCommandEventArgs) Handles FormViewSelectedID.ItemCommand
        If Not e.CommandName = "" Then
            SelectedID()
        End If

    End Sub

    Protected Sub FormViewSelectedID_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewInsertedEventArgs) Handles FormViewSelectedID.ItemInserted
        Dim fab As String
        fab = Me.LabelWorkingFab.Text
        Me.SqlDataSourceFabIDs.SelectCommand = "SELECT MainID, CustomerID FROM dbo.MainID WHERE (CustomerID = N'" & fab & "') ORDER BY MainID"
        Me.DropDownListIDs.Items.Clear()
        Me.DropDownListIDs.Items.Add("Select One...")
        Me.DropDownListIDs.Items.Add("New ID...")
        Me.DropDownListIDs.DataBind()

    End Sub


    Protected Sub FormViewSelectedID_ModeChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewModeEventArgs) Handles FormViewSelectedID.ModeChanging
        SelectedID()
        Me.PanelSpecCurrent.Visible = False
    End Sub


    Protected Sub ButtonNewSpec_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonNewSpec.Click
        EditSpec()
    End Sub

    Protected Sub ButtonSaveSpec_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSaveSpec.Click
        SaveNewSpec()
        Me.PanelNewSpec.Visible = False
        Me.PanelSpecCurrent.Visible = True
        Me.SqlDataSourceIDCurrentSpec.SelectCommand = "SELECT dbo.MainID_MainIDSpec.MainID, dbo.MainIDSpec.RecordNumber, dbo.MainIDSpec.PART_NUMBER, dbo.MainIDSpec.PART_REV_NUMBER, dbo.MainIDSpec.SPEC_NUMBER, dbo.MainIDSpec.SPEC_REV_NUMBER, dbo.MainIDSpec.thk_grp, dbo.MainIDSpec.res_grp, dbo.MainIDSpec.ORTN, dbo.MainIDSpec.WTYPE_DOPE, dbo.MainIDSpec.DOPE, dbo.MainIDSpec.SAMPLE_STANDARD, dbo.MainID_MainIDSpec.Label_Comments, dbo.MainID_MainIDSpec.Label_Comments2, dbo.MainID_MainIDSpec.Label_Comments3, dbo.MainID_MainIDSpec.EffectiveDtd, dbo.MainID_MainIDSpec.ExpirationDtd FROM dbo.MainIDSpec INNER JOIN dbo.MainID_MainIDSpec ON dbo.MainIDSpec.RecordNumber = dbo.MainID_MainIDSpec.WaferSpec_Key WHERE (dbo.MainID_MainIDSpec.MainID = N'" & Me.DropDownListIDs.SelectedItem.Text & "') AND (dbo.MainID_MainIDSpec.EffectiveDtd <= { fn NOW() }) AND (dbo.MainID_MainIDSpec.ExpirationDtd IS NULL OR dbo.MainID_MainIDSpec.ExpirationDtd >= { fn NOW() })"
        Me.DetailsViewIDCurrentSpec.DataBind()
    End Sub

    Sub EditSpec()
        Me.PanelNewSpec.Visible = True

        Dim DS As Data.DataSet

        DS = Saticode.CurrentSpec(Me.DropDownListIDs.SelectedItem.Text)

        If DS.Tables(0).Rows.Count > 0 Then
            Me.ButtonSaveSpec.Text = "Save New Spec"
            Dim DR As Data.DataRow
            DR = DS.Tables(0).Rows(0)
            'MainID
            Me.LabelCurrentSpec.Text = DR("RecordNumber").ToString
            Me.TextBoxPart.Text = DR("PART_NUMBER").ToString
            Me.TextBoxPartRev.Text = DR("PART_REV_NUMBER").ToString
            Me.TextBoxSpec.Text = DR("SPEC_NUMBER").ToString
            Me.TextBoxSpecRev.Text = DR("SPEC_REV_NUMBER").ToString
            Me.TextBoxThick.Text = DR("thk_grp").ToString
            Me.TextBoxRes.Text = DR("res_grp").ToString
            Me.DropDownListOrtn.SelectedValue = DR("Ortn").ToString
            Me.DropDownListType.SelectedValue = DR("WTYPE_DOPE").ToString
            Me.DropDownListDopeType.SelectedValue = DR("DOPE").ToString    'DOPE
            Me.DropDownListSS.SelectedValue = DR("SAMPLE_STANDARD").ToString
            Me.TextBoxComment1.Text = DR("Label_Comments").ToString
            Me.TextBoxComment2.Text = DR("Label_Comments2").ToString
            Me.TextBoxComment3.Text = DR("Label_Comments3").ToString
            'EffectiveDtd
            'ExpirationDtd 
        Else
            Me.ButtonSaveSpec.Text = "Create New Spec"
            Me.LabelCurrentSpec.Text = ""
            Me.TextBoxPart.Text = ""
            Me.TextBoxPartRev.Text = ""
            Me.TextBoxSpec.Text = ""
            Me.TextBoxSpecRev.Text = ""
            Me.TextBoxThick.Text = "0 - 0 µm"
            Me.TextBoxRes.Text = "0 - 0 Ohm-cm."
            'Me.DropDownListOrtn.SelectedValue = DR("Ortn").ToString
            'Me.DropDownListType.SelectedValue = DR("WTYPE_DOPE").ToString
            'DOPE
            'Me.DropDownListSS.SelectedValue = DR("SAMPLE_STANDARD").ToString
            Me.TextBoxComment1.Text = ""
            Me.TextBoxComment2.Text = ""
            Me.TextBoxComment3.Text = ""
            'EffectiveDtd
            'ExpirationDtd 
        End If
    End Sub

    Protected Sub ButtonCancelSpec_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancelSpec.Click
        Me.PanelNewSpec.Visible = False
    End Sub

    Sub SaveNewSpec()
       
        Dim CurrentSpec As String
        CurrentSpec = Me.LabelCurrentSpec.Text
        If CurrentSpec = "" Then
            CurrentSpec = "0"
        End If
        Dim NewSN As String
        NewSN = Record_A_Spec()
        'MainID 

        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        '*****************************************************************
        '************************Select***********************************
        '*****************************************************************
        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT MainID, WaferSpec_Key, Label_Verify, Label_Comments, Label_Comments2, Label_Comments3, EffectiveDtd, ExpirationDtd, EventTime FROM dbo.MainID_MainIDSpec WHERE (WaferSpec_Key = " & CurrentSpec & ")"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        '*****************************************************************
        '************************Insert***********************************
        '*****************************************************************
        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [dbo].[MainID_MainIDSpec] ([MainID], [WaferSpec_Key], [Label_Verify], [Label_Comments], [Label_Comments2], [Label_Comments3], [EffectiveDtd], [ExpirationDtd], [EventTime]) VALUES (@MainID, @WaferSpec_Key, @Label_Verify, @Label_Comments, @Label_Comments2, @Label_Comments3, @EffectiveDtd, @ExpirationDtd, @EventTime); SELECT MainID, WaferSpec_Key, Label_Verify, Label_Comments, Label_Comments2, Label_Comments3, EffectiveDtd, ExpirationDtd, EventTime FROM dbo.MainID_MainIDSpec WHERE (MainID = @MainID) AND (WaferSpec_Key = @WaferSpec_Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@WaferSpec_Key", System.Data.SqlDbType.Int, 0, "WaferSpec_Key"), New System.Data.SqlClient.SqlParameter("@Label_Verify", System.Data.SqlDbType.Bit, 0, "Label_Verify"), New System.Data.SqlClient.SqlParameter("@Label_Comments", System.Data.SqlDbType.NVarChar, 0, "Label_Comments"), New System.Data.SqlClient.SqlParameter("@Label_Comments2", System.Data.SqlDbType.NVarChar, 0, "Label_Comments2"), New System.Data.SqlClient.SqlParameter("@Label_Comments3", System.Data.SqlDbType.NVarChar, 0, "Label_Comments3"), New System.Data.SqlClient.SqlParameter("@EffectiveDtd", System.Data.SqlDbType.SmallDateTime, 0, "EffectiveDtd"), New System.Data.SqlClient.SqlParameter("@ExpirationDtd", System.Data.SqlDbType.SmallDateTime, 0, "ExpirationDtd"), New System.Data.SqlClient.SqlParameter("@EventTime", System.Data.SqlDbType.SmallDateTime, 0, "EventTime")})
        End With
        DA.InsertCommand = InsertCmd

        '*****************************************************************
        '************************Update***********************************
        '*****************************************************************
        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [dbo].[MainID_MainIDSpec] SET [MainID] = @MainID, [WaferSpec_Key] = @WaferSpec_Key, [Label_Verify] = @Label_Verify, [Label_Comments] = @Label_Comments, [Label_Comments2] = @Label_Comments2, [Label_Comments3] = @Label_Comments3, [EffectiveDtd] = @EffectiveDtd, [ExpirationDtd] = @ExpirationDtd, [EventTime] = @EventTime WHERE (([MainID] = @Original_MainID) AND ([WaferSpec_Key] = @Original_WaferSpec_Key) AND ([Label_Verify] = @Original_Label_Verify) AND ((@IsNull_Label_Comments = 1 AND [Label_Comments] IS NULL) OR ([Label_Comments] = @Original_Label_Comments)) AND ((@IsNull_Label_Comments2 = 1 AND [Label_Comments2] IS NULL) OR ([Label_Comments2] = @Original_Label_Comments2)) AND ((@IsNull_Label_Comments3 = 1 AND [Label_Comments3] IS NULL) OR ([Label_Comments3] = @Original_Label_Comments3)) AND ([EffectiveDtd] = @Original_EffectiveDtd) AND ((@IsNull_ExpirationDtd = 1 AND [ExpirationDtd] IS NULL) OR ([ExpirationDtd] = @Original_ExpirationDtd)) AND ((@IsNull_EventTime = 1 AND [EventTime] IS NULL) OR ([EventTime] = @Original_EventTime))); SELECT MainID, WaferSpec_Key, Label_Verify, Label_Comments, Label_Comments2, Label_Comments3, EffectiveDtd, ExpirationDtd, EventTime FROM dbo.MainID_MainIDSpec WHERE (MainID = @MainID) AND (WaferSpec_Key = @WaferSpec_Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@WaferSpec_Key", System.Data.SqlDbType.Int, 0, "WaferSpec_Key"), New System.Data.SqlClient.SqlParameter("@Label_Verify", System.Data.SqlDbType.Bit, 0, "Label_Verify"), New System.Data.SqlClient.SqlParameter("@Label_Comments", System.Data.SqlDbType.NVarChar, 0, "Label_Comments"), New System.Data.SqlClient.SqlParameter("@Label_Comments2", System.Data.SqlDbType.NVarChar, 0, "Label_Comments2"), New System.Data.SqlClient.SqlParameter("@Label_Comments3", System.Data.SqlDbType.NVarChar, 0, "Label_Comments3"), New System.Data.SqlClient.SqlParameter("@EffectiveDtd", System.Data.SqlDbType.SmallDateTime, 0, "EffectiveDtd"), New System.Data.SqlClient.SqlParameter("@ExpirationDtd", System.Data.SqlDbType.SmallDateTime, 0, "ExpirationDtd"), New System.Data.SqlClient.SqlParameter("@EventTime", System.Data.SqlDbType.SmallDateTime, 0, "EventTime"), New System.Data.SqlClient.SqlParameter("@Original_MainID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MainID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_WaferSpec_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "WaferSpec_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Label_Verify", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Label_Verify", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Label_Comments", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Label_Comments", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Label_Comments", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Label_Comments", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Label_Comments2", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Label_Comments2", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Label_Comments2", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Label_Comments2", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Label_Comments3", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Label_Comments3", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Label_Comments3", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Label_Comments3", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_EffectiveDtd", System.Data.SqlDbType.SmallDateTime, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "EffectiveDtd", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_ExpirationDtd", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "ExpirationDtd", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_ExpirationDtd", System.Data.SqlDbType.SmallDateTime, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ExpirationDtd", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_EventTime", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "EventTime", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_EventTime", System.Data.SqlDbType.SmallDateTime, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "EventTime", System.Data.DataRowVersion.Original, Nothing)})
        End With
        DA.UpdateCommand = UpdateCmd


        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "MainID_MainIDSpec", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("MainID", "MainID"), New System.Data.Common.DataColumnMapping("WaferSpec_Key", "WaferSpec_Key"), New System.Data.Common.DataColumnMapping("Label_Verify", "Label_Verify"), New System.Data.Common.DataColumnMapping("Label_Comments", "Label_Comments"), New System.Data.Common.DataColumnMapping("Label_Comments2", "Label_Comments2"), New System.Data.Common.DataColumnMapping("Label_Comments3", "Label_Comments3"), New System.Data.Common.DataColumnMapping("EffectiveDtd", "EffectiveDtd"), New System.Data.Common.DataColumnMapping("ExpirationDtd", "ExpirationDtd"), New System.Data.Common.DataColumnMapping("EventTime", "EventTime")})})
        DA.Fill(DS)

        If Me.ButtonSaveSpec.Text = "Save New Spec" Then
            DR = DS.Tables(0).Rows(0)
            DR.AcceptChanges()
            DR.BeginEdit()
            DR("ExpirationDtd") = DateTime.Now.ToShortDateString
            DR.EndEdit()
            DA.Update(DS, "MainID_MainIDSpec")

        End If

        DR = DS.Tables("MainID_MainIDSpec").NewRow

        DR("MainID") = Me.DropDownListIDs.SelectedItem.Text
        DR("WaferSpec_Key") = NewSN
        DR("Label_Verify") = True
        DR("Label_Comments") = Me.TextBoxComment1.Text
        DR("Label_Comments2") = Me.TextBoxComment2.Text
        DR("Label_Comments3") = Me.TextBoxComment3.Text
        DR("EffectiveDtd") = DateTime.Now.ToShortDateString

        DS.Tables("MainID_MainIDSpec").Rows.Add(DR)
        DA.Update(DS, "MainID_MainIDSpec")


        Connection.Close()
    End Sub

    Function Record_A_Spec() As String
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        '*****************************************************************
        '************************Select***********************************
        '*****************************************************************
        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT RecordNumber, PART_NUMBER, PART_REV_NUMBER, SPEC_NUMBER, SPEC_REV_NUMBER, thk_grp, res_grp, ORTN, WTYPE_DOPE, DOPE, SAMPLE_STANDARD, EffectiveDtd, EventTime FROM dbo.MainIDSpec WHERE (RecordNumber = 0)"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        '*****************************************************************
        '************************Insert***********************************
        '*****************************************************************
        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [dbo].[MainIDSpec] ([PART_NUMBER], [PART_REV_NUMBER], [SPEC_NUMBER], [SPEC_REV_NUMBER], [thk_grp], [res_grp], [ORTN], [WTYPE_DOPE], [DOPE], [SAMPLE_STANDARD], [EffectiveDtd], [EventTime]) VALUES (@PART_NUMBER, @PART_REV_NUMBER, @SPEC_NUMBER, @SPEC_REV_NUMBER, @thk_grp, @res_grp, @ORTN, @WTYPE_DOPE, @DOPE, @SAMPLE_STANDARD, @EffectiveDtd, @EventTime); SELECT RecordNumber, PART_NUMBER, PART_REV_NUMBER, SPEC_NUMBER, SPEC_REV_NUMBER, thk_grp, res_grp, ORTN, WTYPE_DOPE, DOPE, SAMPLE_STANDARD, EffectiveDtd, EventTime FROM dbo.MainIDSpec WHERE (RecordNumber = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@PART_NUMBER", System.Data.SqlDbType.NVarChar, 0, "PART_NUMBER"), New System.Data.SqlClient.SqlParameter("@PART_REV_NUMBER", System.Data.SqlDbType.NVarChar, 0, "PART_REV_NUMBER"), New System.Data.SqlClient.SqlParameter("@SPEC_NUMBER", System.Data.SqlDbType.NVarChar, 0, "SPEC_NUMBER"), New System.Data.SqlClient.SqlParameter("@SPEC_REV_NUMBER", System.Data.SqlDbType.NVarChar, 0, "SPEC_REV_NUMBER"), New System.Data.SqlClient.SqlParameter("@thk_grp", System.Data.SqlDbType.NVarChar, 0, "thk_grp"), New System.Data.SqlClient.SqlParameter("@res_grp", System.Data.SqlDbType.NVarChar, 0, "res_grp"), New System.Data.SqlClient.SqlParameter("@ORTN", System.Data.SqlDbType.NVarChar, 0, "ORTN"), New System.Data.SqlClient.SqlParameter("@WTYPE_DOPE", System.Data.SqlDbType.NVarChar, 0, "WTYPE_DOPE"), New System.Data.SqlClient.SqlParameter("@DOPE", System.Data.SqlDbType.NVarChar, 0, "DOPE"), New System.Data.SqlClient.SqlParameter("@SAMPLE_STANDARD", System.Data.SqlDbType.NVarChar, 0, "SAMPLE_STANDARD"), New System.Data.SqlClient.SqlParameter("@EffectiveDtd", System.Data.SqlDbType.SmallDateTime, 0, "EffectiveDtd"), New System.Data.SqlClient.SqlParameter("@EventTime", System.Data.SqlDbType.SmallDateTime, 0, "EventTime")})
        End With
        DA.InsertCommand = InsertCmd

        '*****************************************************************
        '************************Update***********************************
        '*****************************************************************
        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [dbo].[MainIDSpec] SET [PART_NUMBER] = @PART_NUMBER, [PART_REV_NUMBER] = @PART_REV_NUMBER, [SPEC_NUMBER] = @SPEC_NUMBER, [SPEC_REV_NUMBER] = @SPEC_REV_NUMBER, [thk_grp] = @thk_grp, [res_grp] = @res_grp, [ORTN] = @ORTN, [WTYPE_DOPE] = @WTYPE_DOPE, [DOPE] = @DOPE, [SAMPLE_STANDARD] = @SAMPLE_STANDARD, [EffectiveDtd] = @EffectiveDtd, [EventTime] = @EventTime WHERE (([RecordNumber] = @Original_RecordNumber) AND ((@IsNull_PART_NUMBER = 1 ND [PART_NUMBER] IS NULL) OR ([PART_NUMBER] = @Original_PART_NUMBER)) AND ((@IsNull_PART_REV_NUMBER = 1 AND [PART_REV_NUMBER] IS NULL) OR ([PART_REV_NUMBER] = @Original_PART_REV_NUMBER)) AND ((@IsNull_SPEC_NUMBER = 1 AND [SPEC_NUMBER] IS NULL) OR ([SPEC_NUMBER] = @Original_SPEC_NUMBER)) AND ((@IsNull_SPEC_REV_NUMBER = 1 AND [SPEC_REV_NUMBER] IS NULL) OR ([SPEC_REV_NUMBER] = @Original_SPEC_REV_NUMBER)) AND ((@IsNull_thk_grp = 1 AND [thk_grp] IS NULL) OR ([thk_grp] = @Original_thk_grp)) AND ((@IsNull_res_grp = 1 AND [res_grp] IS NULL) OR ([res_grp] = @Original_res_grp)) AND ((@IsNull_ORTN = 1 AND [ORTN] IS NULL) OR ([ORTN] = @Original_ORTN)) AND ((@IsNull_WTYPE_DOPE = 1 AND [WTYPE_DOPE] IS NULL) OR ([WTYPE_DOPE] = @Original_WTYPE_DOPE)) AND ((@IsNull_DOPE = 1 AND [DOPE] IS NULL) OR ([DOPE] = @Original_DOPE)) AND ((@IsNull_SAMPLE_STANDARD = 1 AND [SAMPLE_STANDARD] IS NULL) OR ([SAMPLE_STANDARD] = @Original_SAMPLE_STANDARD)) AND ([EffectiveDtd] = @Original_EffectiveDtd) AND ([EventTime] = @Original_EventTime)); SELECT RecordNumber, PART_NUMBER, PART_REV_NUMBER, SPEC_NUMBER, SPEC_REV_NUMBER, thk_grp, res_grp, ORTN, WTYPE_DOPE, DOPE, SAMPLE_STANDARD, EffectiveDtd, EventTime FROM dbo.MainIDSpec WHERE (RecordNumber = @RecordNumber)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@PART_NUMBER", System.Data.SqlDbType.NVarChar, 0, "PART_NUMBER"), New System.Data.SqlClient.SqlParameter("@PART_REV_NUMBER", System.Data.SqlDbType.NVarChar, 0, "PART_REV_NUMBER"), New System.Data.SqlClient.SqlParameter("@SPEC_NUMBER", System.Data.SqlDbType.NVarChar, 0, "SPEC_NUMBER"), New System.Data.SqlClient.SqlParameter("@SPEC_REV_NUMBER", System.Data.SqlDbType.NVarChar, 0, "SPEC_REV_NUMBER"), New System.Data.SqlClient.SqlParameter("@thk_grp", System.Data.SqlDbType.NVarChar, 0, "thk_grp"), New System.Data.SqlClient.SqlParameter("@res_grp", System.Data.SqlDbType.NVarChar, 0, "res_grp"), New System.Data.SqlClient.SqlParameter("@ORTN", System.Data.SqlDbType.NVarChar, 0, "ORTN"), New System.Data.SqlClient.SqlParameter("@WTYPE_DOPE", System.Data.SqlDbType.NVarChar, 0, "WTYPE_DOPE"), New System.Data.SqlClient.SqlParameter("@DOPE", System.Data.SqlDbType.NVarChar, 0, "DOPE"), New System.Data.SqlClient.SqlParameter("@SAMPLE_STANDARD", System.Data.SqlDbType.NVarChar, 0, "SAMPLE_STANDARD"), New System.Data.SqlClient.SqlParameter("@EffectiveDtd", System.Data.SqlDbType.SmallDateTime, 0, "EffectiveDtd"), New System.Data.SqlClient.SqlParameter("@EventTime", System.Data.SqlDbType.SmallDateTime, 0, "EventTime"), New System.Data.SqlClient.SqlParameter("@Original_RecordNumber", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "RecordNumber", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_PART_NUMBER", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "PART_NUMBER", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_PART_NUMBER", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PART_NUMBER", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_PART_REV_NUMBER", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "PART_REV_NUMBER", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_PART_REV_NUMBER", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PART_REV_NUMBER", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_SPEC_NUMBER", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "SPEC_NUMBER", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_SPEC_NUMBER", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SPEC_NUMBER", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_SPEC_REV_NUMBER", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "SPEC_REV_NUMBER", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_SPEC_REV_NUMBER", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SPEC_REV_NUMBER", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_thk_grp", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "thk_grp", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_thk_grp", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "thk_grp", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_res_grp", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "res_grp", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_res_grp", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "res_grp", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_ORTN", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "ORTN", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_ORTN", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ORTN", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_WTYPE_DOPE", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "WTYPE_DOPE", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_WTYPE_DOPE", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "WTYPE_DOPE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_DOPE", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "DOPE", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_DOPE", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DOPE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_SAMPLE_STANDARD", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "SAMPLE_STANDARD", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_SAMPLE_STANDARD", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SAMPLE_STANDARD", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_EffectiveDtd", System.Data.SqlDbType.SmallDateTime, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "EffectiveDtd", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_EventTime", System.Data.SqlDbType.SmallDateTime, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "EventTime", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@RecordNumber", System.Data.SqlDbType.Int, 4, "RecordNumber")})
        End With
        DA.UpdateCommand = UpdateCmd


        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "MainIDSpec", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("RecordNumber", "RecordNumber"), New System.Data.Common.DataColumnMapping("PART_NUMBER", "PART_NUMBER"), New System.Data.Common.DataColumnMapping("PART_REV_NUMBER", "PART_REV_NUMBER"), New System.Data.Common.DataColumnMapping("SPEC_NUMBER", "SPEC_NUMBER"), New System.Data.Common.DataColumnMapping("SPEC_REV_NUMBER", "SPEC_REV_NUMBER"), New System.Data.Common.DataColumnMapping("thk_grp", "thk_grp"), New System.Data.Common.DataColumnMapping("res_grp", "res_grp"), New System.Data.Common.DataColumnMapping("ORTN", "ORTN"), New System.Data.Common.DataColumnMapping("WTYPE_DOPE", "WTYPE_DOPE"), New System.Data.Common.DataColumnMapping("DOPE", "DOPE"), New System.Data.Common.DataColumnMapping("SAMPLE_STANDARD", "SAMPLE_STANDARD"), New System.Data.Common.DataColumnMapping("EffectiveDtd", "EffectiveDtd"), New System.Data.Common.DataColumnMapping("EventTime", "EventTime")})})
        DA.Fill(DS)


        DR = DS.Tables("MainIDSpec").NewRow
        'DR("RecordNumber") = ""
        DR("PART_NUMBER") = UCase(Me.TextBoxPart.Text)
        DR("PART_REV_NUMBER") = UCase(Me.TextBoxPartRev.Text)
        DR("SPEC_NUMBER") = UCase(Me.TextBoxSpec.Text)
        DR("SPEC_REV_NUMBER") = UCase(Me.TextBoxSpecRev.Text)
        DR("thk_grp") = Me.TextBoxThick.Text
        DR("res_grp") = Me.TextBoxRes.Text
        DR("ORTN") = Me.DropDownListOrtn.SelectedValue.ToString
        DR("WTYPE_DOPE") = Me.DropDownListType.SelectedValue.ToString
        DR("DOPE") = Me.DropDownListDopeType.SelectedValue.ToString
        DR("SAMPLE_STANDARD") = Me.DropDownListSS.SelectedValue.ToString
        DR("EffectiveDtd") = DateTime.Now.ToShortDateString
        DR("EventTime") = DateTime.Now.ToShortDateString

        DS.Tables("MainIDSpec").Rows.Add(DR)
        DA.Update(DS, "MainIDSpec")

        Dim NewSpecNumber As String
        NewSpecNumber = CType(DR("RecordNumber").ToString, String)
        
        Connection.Close()
        Record_A_Spec = NewSpecNumber
    End Function

    Protected Sub CheckIDDetail_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckIDDetail.CheckedChanged
        SetView()
    End Sub

    Protected Sub CheckCusomerSpec_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckCusomerSpec.CheckedChanged
        SetView()
    End Sub

    Protected Sub CheckBoxAddress_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxAddress.CheckedChanged
        SetView()
    End Sub

    Protected Sub CheckBoxLabels_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxLabels.CheckedChanged
        SetView()
    End Sub

    Protected Sub CheckBoxPaths_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxPaths.CheckedChanged
        SetView()
    End Sub

    Protected Sub CheckBoxDefects_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxDefects.CheckedChanged
        SetView()
    End Sub

    Protected Sub ButtonViewMainPath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonViewMainPath.Click
        Me.PanelViewSelctedPath.Visible = True
        Me.SqlDataSourceViewPath.SelectCommand = "SELECT ProcessOrder AS Step, StageName FROM dbo.CannedPaths WHERE (PathName = N'" & Me.DropDownListMainPath.SelectedItem.Text & " ') ORDER BY ProcessOrder"
    End Sub

    Protected Sub ButtonViewLapPath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonViewLapPath.Click
        Me.PanelViewSelctedPath.Visible = True
        Me.SqlDataSourceViewPath.SelectCommand = "SELECT ProcessOrder AS Step, StageName FROM dbo.CannedPaths WHERE (PathName = N'" & Me.DropDownListLapPath.SelectedItem.Text & " ') ORDER BY ProcessOrder"
    End Sub

    Protected Sub ButtonViewStripEtchPath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonViewStripEtchPath.Click
        Me.PanelViewSelctedPath.Visible = True
        Me.SqlDataSourceViewPath.SelectCommand = "SELECT ProcessOrder AS Step, StageName FROM dbo.CannedPaths WHERE (PathName = N'" & Me.DropDownListStripEtchPath.SelectedItem.Text & " ') ORDER BY ProcessOrder"
    End Sub

    Protected Sub ButtonViewPolishPath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonViewPolishPath.Click
        Me.PanelViewSelctedPath.Visible = True
        Me.SqlDataSourceViewPath.SelectCommand = "SELECT ProcessOrder AS Step, StageName FROM dbo.CannedPaths WHERE (PathName = N'" & Me.DropDownListPolishPath.SelectedItem.Text & " ') ORDER BY ProcessOrder"
    End Sub

    Protected Sub ButtonViewCMPPath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonViewCMPPath.Click
        Me.PanelViewSelctedPath.Visible = True
        Me.SqlDataSourceViewPath.SelectCommand = "SELECT ProcessOrder AS Step, StageName FROM dbo.CannedPaths WHERE (PathName = N'" & Me.DropDownListCMPPath.SelectedItem.Text & " ') ORDER BY ProcessOrder"
    End Sub

    Protected Sub ButtonViewDSPPath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonViewDSPPath.Click
        Me.PanelViewSelctedPath.Visible = True
        Me.SqlDataSourceViewPath.SelectCommand = "SELECT ProcessOrder AS Step, StageName FROM dbo.CannedPaths WHERE (PathName = N'" & Me.DropDownListDSPPath.SelectedItem.Text & " ') ORDER BY ProcessOrder"
    End Sub

    Protected Sub ButtonSavePaths_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSavePaths.Click
        Saticode.SetNewPath(Me.DropDownListIDs.SelectedItem.Text, "MAIN", Me.DropDownListMainPath.SelectedItem.Text, User.Identity.Name.ToString)
        Saticode.SetNewPath(Me.DropDownListIDs.SelectedItem.Text, "LAP", Me.DropDownListLapPath.SelectedItem.Text, User.Identity.Name.ToString)
        Saticode.SetNewPath(Me.DropDownListIDs.SelectedItem.Text, "SE", Me.DropDownListStripEtchPath.SelectedItem.Text, User.Identity.Name.ToString)
        Saticode.SetNewPath(Me.DropDownListIDs.SelectedItem.Text, "POLISH", Me.DropDownListPolishPath.SelectedItem.Text, User.Identity.Name.ToString)
        Saticode.SetNewPath(Me.DropDownListIDs.SelectedItem.Text, "CMP", Me.DropDownListCMPPath.SelectedItem.Text, User.Identity.Name.ToString)
        Saticode.SetNewPath(Me.DropDownListIDs.SelectedItem.Text, "DSP", Me.DropDownListDSPPath.SelectedItem.Text, User.Identity.Name.ToString)
        Me.LabelPathsaved.Text = "Saved"
    End Sub

    Protected Sub GridViewDefects_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewDefects.RowCommand
        Me.SqlDataSourceCurrentDefects.SelectCommand = "SELECT [Key], Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE (ID = '" & Me.DropDownListIDs.SelectedItem.Text & "')"
        Me.GridViewDefects.DataBind()
    End Sub

    Protected Sub ButtonCloneDefects_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCloneDefects.Click
        ModDefects(Me.DropDownListIDs.SelectedItem.Text, "Clone", "", "", "", Me.DropDownListDefectCloneIDList.SelectedItem.Text)
        SetView()
    End Sub

    Protected Sub ButtonAddDefect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddDefect.Click
        ModDefects(Me.DropDownListIDs.SelectedItem.Text, "Add", Me.DropDownListAvailableDefects.SelectedItem.Text, Me.DropDownListDefectType.SelectedItem.Text, Me.DropDownListDefectGroup.SelectedItem.Text, "")
        SetView()
    End Sub

    Sub ModDefects(ByVal MainID As String, ByVal Add_Clone As String, ByVal AddDefect As String, ByVal AddType As String, ByVal AddGroup As String, ByVal CloneID As String)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT [Key], ID, Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE (ID = '" & MainID & "')"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [dbo].[T_ID_Defects] ([ID], [Defect], [Type], [Group]) VALUES (@ID, @Defect, @Type, @Group); SELECT [Key], ID, Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.VarChar, 0, "ID"), New System.Data.SqlClient.SqlParameter("@Defect", System.Data.SqlDbType.VarChar, 0, "Defect"), New System.Data.SqlClient.SqlParameter("@Type", System.Data.SqlDbType.VarChar, 0, "Type"), New System.Data.SqlClient.SqlParameter("@Group", System.Data.SqlDbType.VarChar, 0, "Group")})
        End With
        DA.InsertCommand = InsertCmd

        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [dbo].[T_ID_Defects] SET [ID] = @ID, [Defect] = @Defect, [Type] = @Type, [Group] = @Group WHERE (([Key] = @Original_Key) AND ([ID] = @Original_ID) AND ([Defect] = @Original_Defect) AND ([Type] = @Original_Type) AND ([Group] = @Original_Group)); SELECT [Key], ID, Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.VarChar, 0, "ID"), New System.Data.SqlClient.SqlParameter("@Defect", System.Data.SqlDbType.VarChar, 0, "Defect"), New System.Data.SqlClient.SqlParameter("@Type", System.Data.SqlDbType.VarChar, 0, "Type"), New System.Data.SqlClient.SqlParameter("@Group", System.Data.SqlDbType.VarChar, 0, "Group"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ID", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Defect", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Defect", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Type", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Type", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Group", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Group", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        Dim DeleteCmd As New System.Data.SqlClient.SqlCommand
        With DeleteCmd
            .CommandText = "DELETE FROM [dbo].[T_ID_Defects] WHERE (([Key] = @Original_Key) AND ([ID] = @Original_ID) AND ([Defect] = @Original_Defect) AND ([Type] = @Original_Type) AND ([Group] = @Original_Group))"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ID", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Defect", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Defect", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Type", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Type", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Group", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Group", System.Data.DataRowVersion.Original, Nothing)})
        End With
        DA.DeleteCommand = DeleteCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_ID_Defects", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("ID", "ID"), New System.Data.Common.DataColumnMapping("Defect", "Defect"), New System.Data.Common.DataColumnMapping("Type", "Type"), New System.Data.Common.DataColumnMapping("Group", "Group")})})
        DA.Fill(DS)

        Select Case Add_Clone
            Case "Clone"

                Dim Kill As Integer
                Kill = DS.Tables(0).Rows.Count - 1
                For Row As Integer = 0 To Kill
                    DR = DS.Tables(0).Rows(Row)
                    DR.Delete()
                Next
                DA.Update(DS, "T_ID_Defects")


                Dim DA_Clone As New Data.SqlClient.SqlDataAdapter
                Dim DS_Clone As New Data.DataSet
                Dim DR_Clone As Data.DataRow
                Dim SelectCmd_Clone As New System.Data.SqlClient.SqlCommand
                With SelectCmd_Clone
                    .CommandText = "SELECT [Key], ID, Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE (ID = '" & CloneID & "')"
                    .Connection = Connection
                End With
                DA_Clone.SelectCommand = SelectCmd_Clone
                DA_Clone.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_ID_Defects", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("ID", "ID"), New System.Data.Common.DataColumnMapping("Defect", "Defect"), New System.Data.Common.DataColumnMapping("Type", "Type"), New System.Data.Common.DataColumnMapping("Group", "Group")})})
                DA_Clone.Fill(DS_Clone)

                Dim CloneRows As Integer
                CloneRows = DS_Clone.Tables(0).Rows.Count - 1
                For NewRow As Integer = 0 To CloneRows
                    DR_Clone = DS_Clone.Tables(0).Rows(NewRow)
                    DR = DS.Tables("T_ID_Defects").NewRow
                    DR("ID") = MainID
                    DR("Defect") = DR_Clone("Defect")
                    DR("Type") = DR_Clone("Type")
                    DR("Group") = DR_Clone("Group")
                    DS.Tables("T_ID_Defects").Rows.Add(DR)
                    DA.Update(DS, "T_ID_Defects")
                Next

            Case "Add"
                DR = DS.Tables("T_ID_Defects").NewRow

                DR("ID") = MainID
                DR("Defect") = AddDefect
                DR("Type") = AddType
                DR("Group") = AddGroup

                DS.Tables("T_ID_Defects").Rows.Add(DR)
                DA.Update(DS, "T_ID_Defects")
        End Select
        Connection.Close()
    End Sub

    Protected Sub ListBoxShippingKeys_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBoxShippingKeys.SelectedIndexChanged
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        DS = Saticode.GetAddress("", "", Me.ListBoxShippingKeys.SelectedItem.Text)
        If Not DS.Tables(0).Rows.Count = 0 Then
            DR = DS.Tables(0).Rows(0)
            Me.TextBoxShippingLine1.Text = DR("Row1")
            Me.TextBoxShippingLine2.Text = DR("Row2")
            Me.TextBoxShippingLine3.Text = DR("Row3")
            Me.TextBoxShippingLine4.Text = DR("Row4")
            Me.TextBoxShippingLine5.Text = DR("Row5")
            Me.TextBoxShippingLine6.Text = DR("Row6")
            Me.LabelCurrentShipKey.Text = Me.ListBoxShippingKeys.SelectedItem.Text
        End If
        Me.LabelBillSaved.Text = ""
        Me.LabelShipSave.Text = ""
    End Sub

    Protected Sub ListBoxBillingKeys_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBoxBillingKeys.SelectedIndexChanged
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        DS = Saticode.GetAddress("", "", Me.ListBoxBillingKeys.SelectedItem.Text)
        If Not DS.Tables(0).Rows.Count = 0 Then
            DR = DS.Tables(0).Rows(0)
            Me.TextBoxBillingLine1.Text = DR("Row1")
            Me.TextBoxBillingLine2.Text = DR("Row2")
            Me.TextBoxBillingLine3.Text = DR("Row3")
            Me.TextBoxBillingLine4.Text = DR("Row4")
            Me.TextBoxBillingLine5.Text = DR("Row5")
            Me.TextBoxBillingLine6.Text = DR("Row6")
            Me.LabelCurrentBillKey.Text = Me.ListBoxBillingKeys.SelectedItem.Text
        End If
        Me.LabelBillSaved.Text = ""
        Me.LabelShipSave.Text = ""
    End Sub

    Protected Sub ButtonChangeAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonChangeAddress.Click
        Me.PanelAddressChange.Visible = True
        Me.SqlDataSourceShippingKeys.SelectCommand = "SELECT dbo.MainID_Address.Address_Key FROM dbo.MainID_Address INNER JOIN dbo.MainID ON dbo.MainID_Address.MainID = dbo.MainID.MainID WHERE (dbo.MainID.CustomerID = N'" & Me.LabelWorkingFab.Text & "') AND (dbo.MainID_Address.Address_Type = 0) GROUP BY dbo.MainID_Address.Address_Key"
        Me.ListBoxShippingKeys.DataBind()
        Me.SqlDataSourceBillingKeys.SelectCommand = "SELECT dbo.MainID_Address.Address_Key FROM dbo.MainID_Address INNER JOIN dbo.MainID ON dbo.MainID_Address.MainID = dbo.MainID.MainID WHERE (dbo.MainID.CustomerID = N'" & Me.LabelWorkingFab.Text & "') AND (dbo.MainID_Address.Address_Type = 1) GROUP BY dbo.MainID_Address.Address_Key"
        Me.ListBoxBillingKeys.DataBind()

        Me.TextBoxShippingLine1.Text = ""
        Me.TextBoxShippingLine2.Text = ""
        Me.TextBoxShippingLine3.Text = ""
        Me.TextBoxShippingLine4.Text = ""
        Me.TextBoxShippingLine5.Text = ""
        Me.TextBoxShippingLine6.Text = ""

        Me.TextBoxBillingLine1.Text = ""
        Me.TextBoxBillingLine2.Text = ""
        Me.TextBoxBillingLine3.Text = ""
        Me.TextBoxBillingLine4.Text = ""
        Me.TextBoxBillingLine5.Text = ""
        Me.TextBoxBillingLine6.Text = ""
        Me.LabelBillSaved.Text = ""
        Me.LabelShipSave.Text = ""
        Me.RadioButtonReUseBillingAddress.Checked = True
        Me.RadioButtonReUseShippingAddress.Checked = True

    End Sub

   
    Protected Sub ButtonSaveShippingAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSaveShippingAddress.Click
        If Me.RadioButtonReUseShippingAddress.Checked = True Then
            Saticode.UpdateAddressTable(Me.DropDownListIDs.SelectedItem.Text, "Ship", Me.LabelCurrentShipKey.Text)
        Else
            Dim NewKey As String
            NewKey = Saticode.AddAddressLineFive(Me.LabelWorkingFab.Text, Me.TextBoxShippingLine2.Text, Me.TextBoxShippingLine3.Text, Me.TextBoxShippingLine4.Text, Me.TextBoxShippingLine5.Text, Me.TextBoxShippingLine6.Text, User.Identity.Name.ToString)
            Saticode.UpdateAddressTable(Me.DropDownListIDs.SelectedItem.Text, "Ship", NewKey)
        End If
        LoadAddress()
        Me.LabelShipSave.Text = "Saved"
    End Sub

    Protected Sub ButtonSaveBillingAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSaveBillingAddress.Click
        If RadioButtonReUseBillingAddress.Checked = True Then
            Saticode.UpdateAddressTable(Me.DropDownListIDs.SelectedItem.Text, "Bill", Me.LabelCurrentBillKey.Text)
        Else
            Dim NewKey As String
            NewKey = Saticode.AddAddressLineFive(Me.LabelWorkingFab.Text, Me.TextBoxBillingLine2.Text, Me.TextBoxBillingLine3.Text, Me.TextBoxBillingLine4.Text, Me.TextBoxBillingLine5.Text, Me.TextBoxBillingLine6.Text, User.Identity.Name.ToString)
            Saticode.UpdateAddressTable(Me.DropDownListIDs.SelectedItem.Text, "Bill", NewKey)
        End If
        LoadAddress()
        Me.LabelBillSaved.Text = "Saved"
    End Sub

    Protected Sub ButtonCloseAddressPanel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCloseAddressPanel.Click
        Me.PanelAddressChange.Visible = False
    End Sub

    Private Sub GridViewCrossFabShipID_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridViewCrossFabShipID.RowCommand

        Dim MyRecord As String = Me.GridViewCrossFabShipID.Rows(e.CommandArgument.ToString).Cells(0).Text

        If e.CommandName = "RemoveRecord" Then
            Saticode.DeleteMyAltsRecords("Delete FROM dbo.T_Sati_CrossFabIDList WHERE ([Key] = " & MyRecord & ")")
        End If
        RefreshCrossIDShip()

    End Sub

    Protected Sub ButtonAddTransferShipID_Click(sender As Object, e As EventArgs) Handles ButtonAddTransferShipID.Click
        Me.PanelAddTransferShipID.Visible = True
        Me.Label_FromShipTransferID.Text = Me.DropDownListIDs.SelectedItem.Text

        'SELECT CustomerID FROM Customer WHERE (Customer_Name = N'Customers') AND (NOT (CustomerID = N'NotTheSelectedCustomer'))
        'SELECT CustomerID FROM dbo.Customer WHERE (Customer_Name = N'INTEL') AND (NOT (CustomerID = N'Intel-AZ'))
        Me.SqlDataSource_TransferFabs.SelectCommand = "SELECT CustomerID FROM dbo.Customer WHERE (Customer_Name = N'" & Me.DropDownListCustomersList.SelectedItem.Text & "') AND (NOT (CustomerID = N'" & Me.LabelWorkingFab.Text & "'))"
        Me.DropDownList_ToShipTransferFab.DataBind()

        'SELECT MainID FROM dbo.MainID WHERE (CustomerID = N'Intel-NM')
        Me.SqlDataSource_TransferIDList.SelectCommand = "SELECT MainID FROM dbo.MainID WHERE (CustomerID = N'" & Me.DropDownList_ToShipTransferFab.SelectedItem.Text & "')"
        Me.DropDownList_ToShipTransferID.DataBind()

    End Sub

    Protected Sub Button_CloseAddTransferShipID_Panel_Click(sender As Object, e As EventArgs) Handles Button_CloseAddTransferShipID_Panel.Click
        Me.PanelAddTransferShipID.Visible = False
    End Sub

    Protected Sub DropDownList_ToShipTransferFab_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_ToShipTransferFab.SelectedIndexChanged

        Me.SqlDataSource_TransferIDList.SelectCommand = "SELECT MainID FROM dbo.MainID WHERE (CustomerID = N'" & Me.DropDownList_ToShipTransferFab.SelectedItem.Text & "')"
        Me.DropDownList_ToShipTransferID.DataBind()

    End Sub

    Protected Sub Button_SaveTransferShipID_Click(sender As Object, e As EventArgs) Handles Button_SaveTransferShipID.Click

        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        '*****************************************************************
        '************************Select***********************************
        '*****************************************************************
        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT [Key], MainID, TranID, Fab, UserName FROM T_Sati_CrossFabIDList WHERE ([Key] = 0)"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        '*****************************************************************
        '************************Insert***********************************
        '*****************************************************************
        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [T_Sati_CrossFabIDList] ([MainID], [TranID], [Fab], [UserName]) VALUES (@MainID, @TranID, @Fab, @UserName); SELECT [Key], MainID, TranID, Fab, UserName FROM T_Sati_CrossFabIDList WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@TranID", System.Data.SqlDbType.NVarChar, 0, "TranID"), New System.Data.SqlClient.SqlParameter("@Fab", System.Data.SqlDbType.NVarChar, 0, "Fab"), New System.Data.SqlClient.SqlParameter("@UserName", System.Data.SqlDbType.NVarChar, 0, "UserName")})
        End With
        DA.InsertCommand = InsertCmd

        '*****************************************************************
        '************************Update***********************************
        '*****************************************************************
        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [T_Sati_CrossFabIDList] SET [MainID] = @MainID, [TranID] = @TranID, [Fab] = @Fab, [UserName] = @UserName WHERE (([Key] = @Original_Key) AND ([MainID] = @Original_MainID) AND ([TranID] = @Original_TranID) AND ([Fab] = @Original_Fab) AND ([UserName] = @Original_UserName)); SELECT [Key], MainID, TranID, Fab, UserName FROM T_Sati_CrossFabIDList WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@TranID", System.Data.SqlDbType.NVarChar, 0, "TranID"), New System.Data.SqlClient.SqlParameter("@Fab", System.Data.SqlDbType.NVarChar, 0, "Fab"), New System.Data.SqlClient.SqlParameter("@UserName", System.Data.SqlDbType.NVarChar, 0, "UserName"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_MainID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MainID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_TranID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "TranID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Fab", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Fab", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_UserName", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "UserName", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_Sati_CrossFabIDList", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("MainID", "MainID"), New System.Data.Common.DataColumnMapping("TranID", "TranID"), New System.Data.Common.DataColumnMapping("Fab", "Fab"), New System.Data.Common.DataColumnMapping("UserName", "UserName")})})
        DA.Fill(DS)

        DR = DS.Tables("T_Sati_CrossFabIDList").NewRow

        DR("MainID") = Me.DropDownListIDs.SelectedItem.Text
        DR("TranID") = Me.DropDownList_ToShipTransferID.SelectedItem.Text
        DR("Fab") = Me.DropDownList_ToShipTransferFab.SelectedItem.Text
        DR("UserName") = User.Identity.Name.ToString

        DS.Tables("T_Sati_CrossFabIDList").Rows.Add(DR)
        DA.Update(DS, "T_Sati_CrossFabIDList")

        Connection.Close()

        RefreshCrossIDShip()

    End Sub
End Class
