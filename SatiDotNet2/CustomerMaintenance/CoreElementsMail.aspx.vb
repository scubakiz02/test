Imports AjaxControlToolkit
Partial Class DBMaintenance_CoreElementsMail
    Inherits System.Web.UI.Page

    Protected Sub CustomerDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ChangeCustomer()
    End Sub

    Protected Sub FabDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ChangeFab()
    End Sub

    Protected Sub FabAddButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AddFabOrCustomer(True, False)
        Me.NewFabPanel.Visible = False
        ChangeCustomer()
    End Sub

    Protected Sub AddCustomerButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AddFabOrCustomer(False, True)
        Me.CustomerPanel.Visible = False
        Me.CustomerDropDownList.Items.Clear()
        Me.CustomerDropDownList.Items.Add("Select Customer...")
        Me.CustomerDropDownList.Items.Add("Add Customer...")
        Me.CustomerSqlDataSource.SelectCommand = "SELECT Business_Name, Customer_Name FROM dbo.Customer GROUP BY Business_Name, Customer_Name ORDER BY Business_Name"
        Me.CustomerDropDownList.DataBind()

    End Sub

    Sub ChangeCustomer()
        Me.CustomerPanel.Visible = False
        If Me.CustomerDropDownList.SelectedItem.Text = "Add Customer..." Then
            ReadyToAddCustomer()
        End If
        Me.FabDropDownList.Items.Clear()
        Me.FabsOnlySqlDataSource.SelectCommand = "SELECT CustomerID FROM dbo.Customer WHERE (Business_Name = N'" & Me.CustomerDropDownList.SelectedItem.Text & "') ORDER BY CustomerID"
        Me.FabDropDownList.Items.Add("Select Fab...")
        Me.FabDropDownList.Items.Add("Add Fab...")
        Me.FabDropDownList.DataBind()
        ChangeFab()

    End Sub

    Sub ChangeFab()
        Me.FabDataSource.SelectCommand = "SELECT CustomerID, Customer_Name, Business_Name, MacolaID, Supplier_Number, Transit_Days, Note1, Note2 FROM dbo.Customer WHERE (CustomerID = N'" & Me.FabDropDownList.SelectedItem.Text & "')"
        Me.GridView2.DataBind()
        Me.FabPanel.Visible = False
        Me.IDDropDownList.Items.Clear()
        Me.IDDropDownList.Items.Clear()
        If Not Me.GridView2.Rows.Count = 0 Then
            Me.IDDropDownList.Items.Add("Select ID...")
            me.IDDropDownList.Items.Add("Add ID...")
            Me.FabPanel.Visible = True
        End If
        Me.MainSqlDataSource.SelectCommand = "SELECT MainID FROM dbo.MainID WHERE (CustomerID = N'" & Me.FabDropDownList.SelectedItem.Text & "')"
        Me.IDDropDownList.DataBind()
        If Me.FabDropDownList.SelectedItem.Text = "Add Fab..." Then
            ReadyToAddFab()
        Else
            Me.NewFabPanel.Visible = False
        End If
        IDSelected(False)
    End Sub

    Sub ReadyToAddFab()
        Me.FabPanel.Visible = True
        Me.NewFabPanel.Visible = True
        Me.NewFabBusinessNameTextBox.Text = Me.CustomerDropDownList.SelectedItem.Text
        Me.NewFabBusinessNameTextBox.Enabled = False
        Me.NewFabCustomerNameTextBox.Text = Me.CustomerDropDownList.SelectedValue.ToString
        Me.NewFabCustomerNameTextBox.Enabled = False
        Me.NewFabMacolaNumberTextBox.Text = ""
        Me.NewFabNameTextBox.Text = ""
        Me.NewFabNote1TextBox.Text = ""
        Me.NewFabNote2TextBox.Text = ""
        Me.NewFabSupplierNumberTextBox.Text = ""
        Me.NewFabTransitDaysTextBox.Text = ""
    End Sub

    Sub ReadyToAddCustomer()
        Me.CustomerPanel.Visible = True
        Me.AddCustomerBusinessNameTextBox.Text = ""
        Me.AddCustomerIDTextBox.Text = ""
        Me.AddCustomerNameTextBox.Text = ""
    End Sub

    Function AddFabOrCustomer(ByVal Fab As Boolean, ByVal Customer As Boolean) As Boolean
        AddFabOrCustomer = False
        If Fab = True Then
            If Me.NewFabNameTextBox.Text = "" Then
                Exit Function
            End If
        End If
        If Customer = True Then
            If Me.AddCustomerBusinessNameTextBox.Text = "" Or Me.AddCustomerIDTextBox.Text = "" Or Me.AddCustomerNameTextBox.Text = "" Then
                Exit Function
            End If
        End If
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA_AddFab As New Data.SqlClient.SqlDataAdapter
        Dim DS_AddFab As New Data.DataSet
        Dim DR_AddFab As Data.DataRow

        Dim AddFabSelectCmd As New System.Data.SqlClient.SqlCommand
        With AddFabSelectCmd
            .CommandText = "SELECT CustomerID, Customer_Name, Business_Name, MacolaID, Supplier_Number, Transit_Days, Note1, Note2 FROM dbo.Customer WHERE (CustomerID = N'x')"
            .Connection = Connection
        End With
        DA_AddFab.SelectCommand = AddFabSelectCmd

        Dim AddFabInsertCmd As New System.Data.SqlClient.SqlCommand
        With AddFabInsertCmd
            .CommandText = "INSERT INTO [dbo].[Customer] ([CustomerID], [Customer_Name], [Business_Name], [MacolaID], [Supplier_Number], [Transit_Days], [Note1], [Note2]) VALUES (@CustomerID, @Customer_Name, @Business_Name, @MacolaID, @Supplier_Number, @Transit_Days, @Note1, @Note2)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@CustomerID", System.Data.SqlDbType.NVarChar, 0, "CustomerID"), New System.Data.SqlClient.SqlParameter("@Customer_Name", System.Data.SqlDbType.NVarChar, 0, "Customer_Name"), New System.Data.SqlClient.SqlParameter("@Business_Name", System.Data.SqlDbType.NVarChar, 0, "Business_Name"), New System.Data.SqlClient.SqlParameter("@MacolaID", System.Data.SqlDbType.NVarChar, 0, "MacolaID"), New System.Data.SqlClient.SqlParameter("@Supplier_Number", System.Data.SqlDbType.NVarChar, 0, "Supplier_Number"), New System.Data.SqlClient.SqlParameter("@Transit_Days", System.Data.SqlDbType.TinyInt, 0, "Transit_Days"), New System.Data.SqlClient.SqlParameter("@Note1", System.Data.SqlDbType.NVarChar, 0, "Note1"), New System.Data.SqlClient.SqlParameter("@Note2", System.Data.SqlDbType.NVarChar, 0, "Note2")})
        End With
        DA_AddFab.InsertCommand = AddFabInsertCmd

        DA_AddFab.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "Customer", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("CustomerID", "CustomerID"), New System.Data.Common.DataColumnMapping("Customer_Name", "Customer_Name"), New System.Data.Common.DataColumnMapping("Business_Name", "Business_Name"), New System.Data.Common.DataColumnMapping("MacolaID", "MacolaID"), New System.Data.Common.DataColumnMapping("Supplier_Number", "Supplier_Number"), New System.Data.Common.DataColumnMapping("Transit_Days", "Transit_Days"), New System.Data.Common.DataColumnMapping("Note1", "Note1"), New System.Data.Common.DataColumnMapping("Note2", "Note2")})})

        DA_AddFab.Fill(DS_AddFab) ' fill the ds with a blank query so the table is set for a insert

        DR_AddFab = DS_AddFab.Tables("Customer").NewRow
        If Customer = True Then
            DR_AddFab("CustomerID") = Me.AddCustomerIDTextBox.Text
            DR_AddFab("Customer_Name") = Me.AddCustomerNameTextBox.Text
            DR_AddFab("Business_Name") = Me.AddCustomerBusinessNameTextBox.Text
        End If

        If Fab = True Then
            DR_AddFab("CustomerID") = Me.NewFabNameTextBox.Text
            DR_AddFab("Customer_Name") = Me.NewFabCustomerNameTextBox.Text
            DR_AddFab("Business_Name") = Me.NewFabBusinessNameTextBox.Text
            DR_AddFab("MacolaID") = Me.NewFabMacolaNumberTextBox.Text
            DR_AddFab("Supplier_Number") = Me.NewFabSupplierNumberTextBox.Text
            DR_AddFab("Transit_Days") = Me.NewFabTransitDaysTextBox.Text
            DR_AddFab("Note1") = Me.NewFabNote1TextBox.Text
            DR_AddFab("Note2") = Me.NewFabNote2TextBox.Text
        End If

        DS_AddFab.Tables("Customer").Rows.Add(DR_AddFab)
        DA_AddFab.Update(DS_AddFab, "Customer")
        Connection.Close()
        Return True
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        'Server.Transfer("~/Login.aspx?ReturnUrl=~/CustomerMaintenance/CoreElementsMail.aspx")

        MenuAuthenication.CheckGroupAuthenication("CustomerEdit", Server)


        Me.FabDataSource.SelectCommand = "SELECT CustomerID, Customer_Name, Business_Name, MacolaID, Supplier_Number, Transit_Days, Note1, Note2 FROM dbo.Customer WHERE (CustomerID = N'" & Me.FabDropDownList.SelectedItem.Text & "')"
        Try
            Me.IDDefectsSqlDataSource.SelectCommand = "SELECT [Key], ID, Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE (ID = '" & Me.IDDropDownList.SelectedItem.Text & "') ORDER BY Defect"
        Catch ex As Exception
        End Try

        Try
            Me.AllDefectsNamesSqlDataSource.SelectCommand = "SELECT DefectName FROM dbo.DefectNames AS DefectNames_1 WHERE (DefectName NOT IN (SELECT Defect AS DefectName FROM dbo.T_ID_Defects WHERE (ID = '" & Me.IDDropDownList.SelectedItem.Text & "'))) AND (NOT (DefectName = N'test')) AND (NOT (DefectName = N'GFAA')) GROUP BY DefectName"
        Catch ex As Exception
        End Try
    End Sub

    Sub IDSelected(ByVal Switch As Boolean)
        If Switch = True Then
            'Title Panels
            Me.IDTitlePanel.Visible = True
            Me.SpecTiltlePanel.Visible = True
            Me.PackingTiltlePanel.Visible = True
            Me.PathTitlePanel.Visible = True
            Me.DefectsTitlePanel.Visible = True
            Me.AddressTitlePanel.Visible = True
            Me.CofAInfoTitlePanel.Visible = True
            'Content Panels
            Me.PackingContentPanel.Visible = True
            Me.SpecContentPanel.Visible = True
            Me.IDContentPanel.Visible = True
            Me.PathContentPanel.Visible = True
            Me.DefectsContentPanel.Visible = True
            Me.AddressContentPanel.Visible = True
            Me.CofAInfoContentPanel.Visible = True
            'Others
            Me.NewIDPanel.Visible = False
            Me.DefectInfoLabel.Text = ""
            IDMod(Me.IDDropDownList.SelectedItem.Text, True, False, False, "", "")

            Me.IDDefectsSqlDataSource.SelectCommand = "SELECT [Key], ID, Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE (ID = '" & Me.IDDropDownList.SelectedItem.Text & "') ORDER BY Defect"
            Me.IDDefectsGridView.DataBind()

            Me.DefectsDropDownList.Items.Clear()
            Me.DefectsDropDownList.Items.Add("Select Defect...")
            Me.AllDefectsNamesSqlDataSource.SelectCommand = "SELECT DefectName FROM dbo.DefectNames AS DefectNames_1 WHERE (DefectName NOT IN (SELECT Defect AS DefectName FROM dbo.T_ID_Defects WHERE (ID = '" & Me.IDDropDownList.SelectedItem.Text & "'))) AND (NOT (DefectName = N'test')) AND (NOT (DefectName = N'GFAA')) GROUP BY DefectName"
            Me.DefectsDropDownList.DataBind()

            Me.AddressShip1Label.Text = ""
            Me.AddressShip2Label.Text = ""
            Me.AddressShip3Label.Text = ""
            Me.AddressShip4Label.Text = ""
            Me.AddressShip5Label.Text = ""
            Me.AddressShip6Label.Text = ""
            Me.AddressBill1Label.Text = ""
            Me.AddressBill2Label.Text = ""
            Me.AddressBill3Label.Text = ""
            Me.AddressBill4Label.Text = ""
            Me.AddressBill5Label.Text = ""
            Me.AddressBill6Label.Text = ""
            Me.AddressShippingEditButton.Text = "New"
            Me.AddressBillingEditButton.Text = "New"
            Me.ShipKeyLabel.Text = ""
            Me.BillKeyLabel.Text = ""
            AddressGet()




        Else
            'Title Panels
            Me.IDTitlePanel.Visible = False
            Me.SpecTiltlePanel.Visible = False
            Me.PackingTiltlePanel.Visible = False
            Me.PathTitlePanel.Visible = False
            Me.DefectsTitlePanel.Visible = False
            Me.AddressTitlePanel.Visible = False
            Me.CofAInfoTitlePanel.Visible = False
            'Content Panels
            Me.PackingContentPanel.Visible = False
            Me.SpecContentPanel.Visible = False
            Me.IDContentPanel.Visible = False
            Me.PathContentPanel.Visible = False
            Me.DefectsContentPanel.Visible = False
            Me.AddressContentPanel.Visible = False
            Me.CofAInfoContentPanel.Visible = False
            'Others
            Me.NewIDPanel.Visible = True
            Me.DefectInfoLabel.Text = ""
        End If


    End Sub

    Function IDMod(ByVal MainID As String, ByVal Read As Boolean, ByVal Add As Boolean, ByVal Change As Boolean, ByVal What As String, ByVal Value As String) As Boolean
        '***********
        '***LIST****
        '***********
        '[In-Out]
        'CleanRm_Partial
        'Diameter
        'WAFERS_PER_CASS
        'Minimum_Per_Cass
        'Cassette
        'Retain_Rec_WL
        'RET_REJ
        'Over_Label_Qty
        'PO_On_Label
        'Yield_FrontEnd
        'Yield_FrontEnd_R
        'Yield_Polish
        'Yield_Polish_R
        'EffectiveDtd
        'ExpirationDtd
        'PackingSlip_Note
        'PackingSlip_Command
        'Operator
        'EventTime
        'Error
        'Consignment
        'Exsil_Supplied 



        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        'MainID Table
        Dim DA_MAinID As New Data.SqlClient.SqlDataAdapter
        Dim DS_MainID As New Data.DataSet
        Dim DR_MainID As Data.DataRow

        Dim MainIDSelectCmd As New System.Data.SqlClient.SqlCommand
        With MainIDSelectCmd
            .CommandText = "SELECT MainID, CustomerID, [In-Out], CleanRm_Partial, Diameter, WAFERS_PER_CASS, Minimum_Per_Cass, Cassette, Retain_Rec_WL, RET_REJ, Over_Label_Qty, PO_On_Label, Yield_FrontEnd, Yield_FrontEnd_R, Yield_Polish, Yield_Polish_R, EffectiveDtd, ExpirationDtd, PackingSlip_Note, PackingSlip_Command, Operator, EventTime, Error, Consignment, Exsil_Supplied FROM dbo.MainID WHERE (MainID = N'" & MainID & "') "
            .Connection = Connection
        End With
        DA_MAinID.SelectCommand = MainIDSelectCmd

        Dim MainIDInsertCmd As New System.Data.SqlClient.SqlCommand
        With MainIDInsertCmd
            .CommandText = "INSERT INTO [dbo].[MainID] ([MainID], [CustomerID], [In-Out], [CleanRm_Partial], [Diameter], [WAFERS_PER_CASS], [Minimum_Per_Cass], [Cassette], [Retain_Rec_WL], [RET_REJ], [Over_Label_Qty], [PO_On_Label], [Yield_FrontEnd], [Yield_FrontEnd_R], [Yield_Polish], [Yield_Polish_R], [EffectiveDtd], [ExpirationDtd], [PackingSlip_Note], [PackingSlip_Command], [Operator], [EventTime], [Error], [Consignment], [Exsil_Supplied]) VALUES (@MainID, @CustomerID, @p1, @CleanRm_Partial, @Diameter, @WAFERS_PER_CASS, @Minimum_Per_Cass, @Cassette, @Retain_Rec_WL, @RET_REJ, @Over_Label_Qty, @PO_On_Label, @Yield_FrontEnd, @Yield_FrontEnd_R, @Yield_Polish, @Yield_Polish_R, @EffectiveDtd, @ExpirationDtd, @PackingSlip_Note, @PackingSlip_Command, @Operator, @EventTime, @Error, @Consignment, @Exsil_Supplied); SELECT MainID, CustomerID, [In-Out], CleanRm_Partial, Diameter, WAFERS_PER_CASS, Minimum_Per_Cass, Cassette, Retain_Rec_WL, RET_REJ, Over_Label_Qty, PO_On_Label, Yield_FrontEnd, Yield_FrontEnd_R, Yield_Polish, Yield_Polish_R, EffectiveDtd, ExpirationDtd, PackingSlip_Note, PackingSlip_Command, Operator, EventTime, Error, Consignment, Exsil_Supplied FROM dbo.MainID WHERE (MainID = @MainID)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@CustomerID", System.Data.SqlDbType.NVarChar, 0, "CustomerID"), New System.Data.SqlClient.SqlParameter("@p1", System.Data.SqlDbType.Bit, 0, "In-Out"), New System.Data.SqlClient.SqlParameter("@CleanRm_Partial", System.Data.SqlDbType.Bit, 0, "CleanRm_Partial"), New System.Data.SqlClient.SqlParameter("@Diameter", System.Data.SqlDbType.SmallInt, 0, "Diameter"), New System.Data.SqlClient.SqlParameter("@WAFERS_PER_CASS", System.Data.SqlDbType.Int, 0, "WAFERS_PER_CASS"), New System.Data.SqlClient.SqlParameter("@Minimum_Per_Cass", System.Data.SqlDbType.Int, 0, "Minimum_Per_Cass"), New System.Data.SqlClient.SqlParameter("@Cassette", System.Data.SqlDbType.TinyInt, 0, "Cassette"), New System.Data.SqlClient.SqlParameter("@Retain_Rec_WL", System.Data.SqlDbType.Bit, 0, "Retain_Rec_WL"), New System.Data.SqlClient.SqlParameter("@RET_REJ", System.Data.SqlDbType.Bit, 0, "RET_REJ"), New System.Data.SqlClient.SqlParameter("@Over_Label_Qty", System.Data.SqlDbType.Bit, 0, "Over_Label_Qty"), New System.Data.SqlClient.SqlParameter("@PO_On_Label", System.Data.SqlDbType.Bit, 0, "PO_On_Label"), New System.Data.SqlClient.SqlParameter("@Yield_FrontEnd", System.Data.SqlDbType.Real, 0, "Yield_FrontEnd"), New System.Data.SqlClient.SqlParameter("@Yield_FrontEnd_R", System.Data.SqlDbType.Real, 0, "Yield_FrontEnd_R"), New System.Data.SqlClient.SqlParameter("@Yield_Polish", System.Data.SqlDbType.Real, 0, "Yield_Polish"), New System.Data.SqlClient.SqlParameter("@Yield_Polish_R", System.Data.SqlDbType.Real, 0, "Yield_Polish_R"), New System.Data.SqlClient.SqlParameter("@EffectiveDtd", System.Data.SqlDbType.SmallDateTime, 0, "EffectiveDtd"), New System.Data.SqlClient.SqlParameter("@ExpirationDtd", System.Data.SqlDbType.SmallDateTime, 0, "ExpirationDtd"), New System.Data.SqlClient.SqlParameter("@PackingSlip_Note", System.Data.SqlDbType.NVarChar, 0, "PackingSlip_Note"), New System.Data.SqlClient.SqlParameter("@PackingSlip_Command", System.Data.SqlDbType.NVarChar, 0, "PackingSlip_Command"), New System.Data.SqlClient.SqlParameter("@Operator", System.Data.SqlDbType.NVarChar, 0, "Operator"), New System.Data.SqlClient.SqlParameter("@EventTime", System.Data.SqlDbType.SmallDateTime, 0, "EventTime"), New System.Data.SqlClient.SqlParameter("@Error", System.Data.SqlDbType.Bit, 0, "Error"), New System.Data.SqlClient.SqlParameter("@Consignment", System.Data.SqlDbType.Bit, 0, "Consignment"), New System.Data.SqlClient.SqlParameter("@Exsil_Supplied", System.Data.SqlDbType.Bit, 0, "Exsil_Supplied")})
        End With
        DA_MAinID.InsertCommand = MainIDInsertCmd

        Dim MainIDUpdateCmd As New System.Data.SqlClient.SqlCommand
        With MainIDUpdateCmd
            .CommandText = "UPDATE [dbo].[MainID] SET [MainID] = @MainID, [CustomerID] = @CustomerID, [In-Out] = @p1, [CleanRm_Partial] = @CleanRm_Partial, [Diameter] = @Diameter, [WAFERS_PER_CASS] = @WAFERS_PER_CASS, [Minimum_Per_Cass] = @Minimum_Per_Cass, [Cassette] = @Cassette, [Retain_Rec_WL] = @Retain_Rec_WL, [RET_REJ] = @RET_REJ, [Over_Label_Qty] = @Over_Label_Qty, [PO_On_Label] = @PO_On_Label, [Yield_FrontEnd] = @Yield_FrontEnd, [Yield_FrontEnd_R] = @Yield_FrontEnd_R, [Yield_Polish] = @Yield_Polish, [Yield_Polish_R] = @Yield_Polish_R, [EffectiveDtd] = @EffectiveDtd, [ExpirationDtd] = @ExpirationDtd, [PackingSlip_Note] = @PackingSlip_Note, [PackingSlip_Command] = @PackingSlip_Command, [Operator] = @Operator, [EventTime] = @EventTime, [Error] = @Error, [Consignment] = @Consignment, [Exsil_Supplied] = @Exsil_Supplied WHERE (([MainID] = @Original_MainID)); SELECT MainID, CustomerID, [In-Out], CleanRm_Partial, Diameter, WAFERS_PER_CASS, Minimum_Per_Cass, Cassette, Retain_Rec_WL, RET_REJ, Over_Label_Qty, PO_On_Label, Yield_FrontEnd, Yield_FrontEnd_R, Yield_Polish, Yield_Polish_R, EffectiveDtd, ExpirationDtd, PackingSlip_Note, PackingSlip_Command, Operator, EventTime, Error, Consignment, Exsil_Supplied FROM dbo.MainID WHERE (MainID = @MainID)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@CustomerID", System.Data.SqlDbType.NVarChar, 0, "CustomerID"), New System.Data.SqlClient.SqlParameter("@p1", System.Data.SqlDbType.Bit, 0, "In-Out"), New System.Data.SqlClient.SqlParameter("@CleanRm_Partial", System.Data.SqlDbType.Bit, 0, "CleanRm_Partial"), New System.Data.SqlClient.SqlParameter("@Diameter", System.Data.SqlDbType.SmallInt, 0, "Diameter"), New System.Data.SqlClient.SqlParameter("@WAFERS_PER_CASS", System.Data.SqlDbType.Int, 0, "WAFERS_PER_CASS"), New System.Data.SqlClient.SqlParameter("@Minimum_Per_Cass", System.Data.SqlDbType.Int, 0, "Minimum_Per_Cass"), New System.Data.SqlClient.SqlParameter("@Cassette", System.Data.SqlDbType.TinyInt, 0, "Cassette"), New System.Data.SqlClient.SqlParameter("@Retain_Rec_WL", System.Data.SqlDbType.Bit, 0, "Retain_Rec_WL"), New System.Data.SqlClient.SqlParameter("@RET_REJ", System.Data.SqlDbType.Bit, 0, "RET_REJ"), New System.Data.SqlClient.SqlParameter("@Over_Label_Qty", System.Data.SqlDbType.Bit, 0, "Over_Label_Qty"), New System.Data.SqlClient.SqlParameter("@PO_On_Label", System.Data.SqlDbType.Bit, 0, "PO_On_Label"), New System.Data.SqlClient.SqlParameter("@Yield_FrontEnd", System.Data.SqlDbType.Real, 0, "Yield_FrontEnd"), New System.Data.SqlClient.SqlParameter("@Yield_FrontEnd_R", System.Data.SqlDbType.Real, 0, "Yield_FrontEnd_R"), New System.Data.SqlClient.SqlParameter("@Yield_Polish", System.Data.SqlDbType.Real, 0, "Yield_Polish"), New System.Data.SqlClient.SqlParameter("@Yield_Polish_R", System.Data.SqlDbType.Real, 0, "Yield_Polish_R"), New System.Data.SqlClient.SqlParameter("@EffectiveDtd", System.Data.SqlDbType.SmallDateTime, 0, "EffectiveDtd"), New System.Data.SqlClient.SqlParameter("@ExpirationDtd", System.Data.SqlDbType.SmallDateTime, 0, "ExpirationDtd"), New System.Data.SqlClient.SqlParameter("@PackingSlip_Note", System.Data.SqlDbType.NVarChar, 0, "PackingSlip_Note"), New System.Data.SqlClient.SqlParameter("@PackingSlip_Command", System.Data.SqlDbType.NVarChar, 0, "PackingSlip_Command"), New System.Data.SqlClient.SqlParameter("@Operator", System.Data.SqlDbType.NVarChar, 0, "Operator"), New System.Data.SqlClient.SqlParameter("@EventTime", System.Data.SqlDbType.SmallDateTime, 0, "EventTime"), New System.Data.SqlClient.SqlParameter("@Error", System.Data.SqlDbType.Bit, 0, "Error"), New System.Data.SqlClient.SqlParameter("@Consignment", System.Data.SqlDbType.Bit, 0, "Consignment"), New System.Data.SqlClient.SqlParameter("@Exsil_Supplied", System.Data.SqlDbType.Bit, 0, "Exsil_Supplied"), New System.Data.SqlClient.SqlParameter("@Original_MainID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MainID", System.Data.DataRowVersion.Original, Nothing)})
        End With
        DA_MAinID.UpdateCommand = MainIDUpdateCmd

        DA_MAinID.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "MainID", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("MainID", "MainID"), New System.Data.Common.DataColumnMapping("CustomerID", "CustomerID"), New System.Data.Common.DataColumnMapping("In-Out", "In-Out"), New System.Data.Common.DataColumnMapping("CleanRm_Partial", "CleanRm_Partial"), New System.Data.Common.DataColumnMapping("Diameter", "Diameter"), New System.Data.Common.DataColumnMapping("WAFERS_PER_CASS", "WAFERS_PER_CASS"), New System.Data.Common.DataColumnMapping("Minimum_Per_Cass", "Minimum_Per_Cass"), New System.Data.Common.DataColumnMapping("Cassette", "Cassette"), New System.Data.Common.DataColumnMapping("Retain_Rec_WL", "Retain_Rec_WL"), New System.Data.Common.DataColumnMapping("RET_REJ", "RET_REJ"), New System.Data.Common.DataColumnMapping("Over_Label_Qty", "Over_Label_Qty"), New System.Data.Common.DataColumnMapping("PO_On_Label", "PO_On_Label"), New System.Data.Common.DataColumnMapping("Yield_FrontEnd", "Yield_FrontEnd"), New System.Data.Common.DataColumnMapping("Yield_FrontEnd_R", "Yield_FrontEnd_R"), New System.Data.Common.DataColumnMapping("Yield_Polish", "Yield_Polish"), New System.Data.Common.DataColumnMapping("Yield_Polish_R", "Yield_Polish_R"), New System.Data.Common.DataColumnMapping("EffectiveDtd", "EffectiveDtd"), New System.Data.Common.DataColumnMapping("ExpirationDtd", "ExpirationDtd"), New System.Data.Common.DataColumnMapping("PackingSlip_Note", "PackingSlip_Note"), New System.Data.Common.DataColumnMapping("PackingSlip_Command", "PackingSlip_Command"), New System.Data.Common.DataColumnMapping("Operator", "Operator"), New System.Data.Common.DataColumnMapping("EventTime", "EventTime"), New System.Data.Common.DataColumnMapping("Error", "Error"), New System.Data.Common.DataColumnMapping("Consignment", "Consignment"), New System.Data.Common.DataColumnMapping("Exsil_Supplied", "Exsil_Supplied")})})

        DA_MAinID.Fill(DS_MainID)

        If Read = True Then
            'MainID Table
            DR_MainID = DS_MainID.Tables(0).Rows(0)
            Me.ReceivingIDCheckBox.Checked = DR_MainID("In-Out") '[In-Out]
            'CleanRm_Partial
            For i As Int16 = 0 To Me.DiameterDropDownList.Items.Count - 1
                If Me.DiameterDropDownList.Items(i).Value = DR_MainID("Diameter").ToString Then
                    Me.DiameterDropDownList.SelectedIndex = i
                End If
            Next
            Me.WafersPerCassetteDropDownList.SelectedIndex = (DR_MainID("WAFERS_PER_CASS")) - 1 'WAFERS_PER_CASS
            Me.WaferMinPerCassetteDropDownList.SelectedIndex = DR_MainID("Minimum_Per_Cass") - 1 'Minimum_Per_Cass
            Me.CassettesPerBoxDropDownList.SelectedIndex = DR_MainID("Cassette") - 1 'Cassette
            'Retain_Rec_WL
            'RET_REJ
            'Over_Label_Qty
            Me.PO_OnLabelCheckBox.Checked = DR_MainID("PO_On_Label") 'PO_On_Label
            'Yield_FrontEnd
            'Yield_FrontEnd_R
            'Yield_Polish
            'Yield_Polish_R
            'EffectiveDtd
            'ExpirationDtd
            Me.PackingSlipNoteTextBox.Text = DR_MainID("PackingSlip_Note").ToString 'PackingSlip_Note
            'PackingSlip_Command
            'Operator
            'EventTime
            'Error
            Me.ConsignmentCheckBox.Checked = DR_MainID("Consignment") 'Consignment
            Me.SuppliedCheckBox.Checked = DR_MainID("Exsil_Supplied") 'Exsil_Supplied 


        End If

        If Change = True Then
            DR_MainID = DS_MainID.Tables(0).Rows(0)
            DR_MainID.AcceptChanges()
            DR_MainID.BeginEdit()
            Select Case What
                Case "In-Out" '[In-Out]
                    DR_MainID("In-Out") = Value

                Case "CleanRm_Partial" 'CleanRm_Partial
                    DR_MainID("CleanRm_Partial") = Value

                Case "Diameter" 'Diameter
                    DR_MainID("Diameter") = Value

                Case "WAFERS_PER_CASS" 'WAFERS_PER_CASS
                    DR_MainID("WAFERS_PER_CASS") = Value

                Case "Minimum_Per_Cass" 'Minimum_Per_Cass
                    DR_MainID("Minimum_Per_Cass") = Value

                Case "Cassette" 'Cassette
                    DR_MainID("Cassette") = Value

                Case "Retain_Rec_WL" 'Retain_Rec_WL
                    DR_MainID("Retain_Rec_WL") = Value

                Case "RET_REJ" 'RET_REJ
                    DR_MainID("RET_REJ") = Value

                Case "Over_Label_Qty" 'Over_Label_Qty
                    DR_MainID("Over_Label_Qty") = Value

                Case "PO_On_Label" 'PO_On_Label
                    DR_MainID("PO_On_Label") = Value

                Case "Yield_FrontEnd" 'Yield_FrontEnd
                    DR_MainID("Yield_FrontEnd") = Value

                Case "Yield_FrontEnd_R" 'Yield_FrontEnd_R
                    DR_MainID("Yield_FrontEnd_R") = Value

                Case "Yield_Polish" 'Yield_Polish
                    DR_MainID("Yield_Polisht") = Value

                Case "Yield_Polish_R" 'Yield_Polish_R
                    DR_MainID("Yield_Polish_R") = Value

                Case "EffectiveDtd" 'EffectiveDtd
                    DR_MainID("EffectiveDtd") = Value

                Case "ExpirationDtd" 'ExpirationDtd
                    DR_MainID("ExpirationDtd") = Value

                Case "PackingSlip_Note" 'PackingSlip_Note
                    DR_MainID("PackingSlip_Note") = Value

                Case "PackingSlip_Command" 'PackingSlip_Command
                    DR_MainID("PackingSlip_Command") = Value

                Case "Operator" 'Operator
                    DR_MainID("Operator") = Value

                Case "EventTime" 'EventTime
                    DR_MainID("EventTime") = Value

                Case "Error" 'Error
                    DR_MainID("Error") = Value

                Case "Consignment" 'Consignment
                    DR_MainID("Consignment") = Value

                Case "Exsil_Supplied" 'Exsil_Supplied 
                    DR_MainID("Exsil_Supplied") = Value

                Case Else

            End Select
            DR_MainID.EndEdit()
            DA_MAinID.Update(DS_MainID, "MainID")

        End If
        '[In-Out]
        'CleanRm_Partial
        'Diameter
        'WAFERS_PER_CASS
        'Minimum_Per_Cass
        'Cassette
        'Retain_Rec_WL
        'RET_REJ
        'Over_Label_Qty
        'PO_On_Label
        'Yield_FrontEnd
        'Yield_FrontEnd_R
        'Yield_Polish
        'Yield_Polish_R
        'EffectiveDtd
        'ExpirationDtd
        'PackingSlip_Note
        'PackingSlip_Command
        'Operator
        'EventTime
        'Error
        'Consignment
        'Exsil_Supplied 

        Connection.Close()
    End Function

    Protected Sub ReceivingIDCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        IDMod(Me.IDDropDownList.SelectedItem.Text, False, False, True, "In-Out", Me.ReceivingIDCheckBox.Checked.ToString)
    End Sub

    Protected Sub SuppliedCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        IDMod(Me.IDDropDownList.SelectedItem.Text, False, False, True, "Exsil_Supplied", Me.SuppliedCheckBox.Checked.ToString)
    End Sub

    Protected Sub ConsignmentCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        IDMod(Me.IDDropDownList.SelectedItem.Text, False, False, True, "Consignment", Me.ConsignmentCheckBox.Checked.ToString)
    End Sub

    Protected Sub WafersPerCassetteDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        IDMod(Me.IDDropDownList.SelectedItem.Text, False, False, True, "WAFERS_PER_CASS", Me.WafersPerCassetteDropDownList.SelectedItem.Text)
    End Sub

    Protected Sub WaferMinPerCassetteDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        IDMod(Me.IDDropDownList.SelectedItem.Text, False, False, True, "Minimum_Per_Cass", Me.WaferMinPerCassetteDropDownList.SelectedItem.Text)
    End Sub

    Protected Sub CassettesPerBoxDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        IDMod(Me.IDDropDownList.SelectedItem.Text, False, False, True, "Cassette", Me.CassettesPerBoxDropDownList.SelectedItem.Text)
    End Sub

    Protected Sub PO_OnLabelCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        IDMod(Me.IDDropDownList.SelectedItem.Text, False, False, True, "PO_On_Label", Me.PO_OnLabelCheckBox.Checked.ToString)
    End Sub

    Protected Sub DiameterDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        IDMod(Me.IDDropDownList.SelectedItem.Text, False, False, True, "Diameter", Me.DiameterDropDownList.SelectedItem.Text)
    End Sub

    Protected Sub PackingNoteButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        IDMod(Me.IDDropDownList.SelectedItem.Text, False, False, True, "PackingSlip_Note", Me.PackingSlipNoteTextBox.Text)
    End Sub


    Protected Sub AddDefectCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.AddDefectCheckBox.Checked = True Then
            Me.DeffectAddPanel.Visible = True
        Else
            Me.DeffectAddPanel.Visible = False
        End If
    End Sub

    Protected Sub DefectAddButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not Me.DefectTypeDropDownList.SelectedItem.Text = "Select Type..." And Not Me.DefectGroupDropDownList.SelectedItem.Text = "Select Group..." Then


            Dim Connection As New Data.SqlClient.SqlConnection
            Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
            Connection.Open()

            'MainID Table
            Dim DA_AddDefect As New Data.SqlClient.SqlDataAdapter
            Dim DS_AddDefect As New Data.DataSet
            Dim DR_AddDefect As Data.DataRow

            Dim AddDefectSelectCmd As New System.Data.SqlClient.SqlCommand
            With AddDefectSelectCmd
                .CommandText = "SELECT [Key], ID, Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE (ID = '0')"
                .Connection = Connection
            End With
            DA_AddDefect.SelectCommand = AddDefectSelectCmd

            Dim AddDefectInsertCmd As New System.Data.SqlClient.SqlCommand
            With AddDefectInsertCmd
                .CommandText = "INSERT INTO [dbo].[T_ID_Defects] ([ID], [Defect], [Type], [Group]) VALUES (@ID, @Defect, @Type, @Group); SELECT [Key], ID, Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE ([Key] = SCOPE_IDENTITY())"
                .Connection = Connection
                .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.VarChar, 0, "ID"), New System.Data.SqlClient.SqlParameter("@Defect", System.Data.SqlDbType.VarChar, 0, "Defect"), New System.Data.SqlClient.SqlParameter("@Type", System.Data.SqlDbType.VarChar, 0, "Type"), New System.Data.SqlClient.SqlParameter("@Group", System.Data.SqlDbType.VarChar, 0, "Group")})
            End With
            DA_AddDefect.InsertCommand = AddDefectInsertCmd

            DA_AddDefect.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_ID_Defects", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("ID", "ID"), New System.Data.Common.DataColumnMapping("Defect", "Defect"), New System.Data.Common.DataColumnMapping("Type", "Type"), New System.Data.Common.DataColumnMapping("Group", "Group")})})

            DA_AddDefect.Fill(DS_AddDefect)
            DR_AddDefect = DS_AddDefect.Tables("T_ID_Defects").NewRow
            DR_AddDefect("ID") = Me.IDDropDownList.SelectedItem.Text
            DR_AddDefect("Defect") = Me.DefectsDropDownList.SelectedItem.Text
            DR_AddDefect("Type") = Me.DefectTypeDropDownList.SelectedItem.Text
            DR_AddDefect("Group") = Me.DefectGroupDropDownList.SelectedItem.Text
            DS_AddDefect.Tables("T_ID_Defects").Rows.Add(DR_AddDefect)
            DA_AddDefect.Update(DS_AddDefect, "T_ID_Defects")

            IDSelected(True)
            Me.DeffectAddPanel.Visible = False
            Me.AddDefectCheckBox.Checked = False

            Connection.Close()

        Else
            Me.DefectInfoLabel.Text = "Group Or Type Not Selected"
        End If


    End Sub

    Sub AddressGet()
        Me.AddressShippingEditButton.Text = "New"
        Me.AddressBillingEditButton.Text = "New"
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()
        Dim DA_AddressGet As New Data.SqlClient.SqlDataAdapter
        Dim DS_AddressGet As New Data.DataSet
        Dim DR_AddressGet As Data.DataRow
        Dim AddressGetSelectCmd As New System.Data.SqlClient.SqlCommand
        With AddressGetSelectCmd
            .CommandText = "SELECT MainID, Type, Row1, Row2, Row3, Row4, Row5, Row6, AddressKey FROM dbo.fctn_q_Customer_Address() AS fctn_q_Customer_Address_1 WHERE (MainID = N'" & Me.IDDropDownList.SelectedItem.Text & "')"
            .Connection = Connection
        End With
        DA_AddressGet.SelectCommand = AddressGetSelectCmd
        DA_AddressGet.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "fctn_q_Customer_Address", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("MainID", "MainID"), New System.Data.Common.DataColumnMapping("Type", "Type"), New System.Data.Common.DataColumnMapping("Row1", "Row1"), New System.Data.Common.DataColumnMapping("Row2", "Row2"), New System.Data.Common.DataColumnMapping("Row3", "Row3"), New System.Data.Common.DataColumnMapping("Row4", "Row4"), New System.Data.Common.DataColumnMapping("Row5", "Row5"), New System.Data.Common.DataColumnMapping("Row6", "Row6"), New System.Data.Common.DataColumnMapping("AddressKey", "AddressKey")})})

        DA_AddressGet.Fill(DS_AddressGet)
        Try
            DR_AddressGet = DS_AddressGet.Tables(0).Rows(0)

            For i As Int16 = 0 To DS_AddressGet.Tables(0).Rows.Count - 1
                DR_AddressGet = DS_AddressGet.Tables(0).Rows(i)
                Select Case DR_AddressGet("Type")
                    Case "Shipping"
                        Me.AddressShip1Label.Text = DR_AddressGet("Row1")
                        Me.AddressShip2Label.Text = DR_AddressGet("Row2")
                        Me.AddressShip3Label.Text = DR_AddressGet("Row3")
                        Me.AddressShip4Label.Text = DR_AddressGet("Row4")
                        Me.AddressShip5Label.Text = DR_AddressGet("Row5")
                        Me.AddressShip6Label.Text = DR_AddressGet("Row6")
                        Me.ShipKeyLabel.Text = DR_AddressGet("AddressKey")
                        Me.AddressShippingEditButton.Text = "Edit"
                    Case "Billing"
                        Me.AddressBill1Label.Text = DR_AddressGet("Row1")
                        Me.AddressBill2Label.Text = DR_AddressGet("Row2")
                        Me.AddressBill3Label.Text = DR_AddressGet("Row3")
                        Me.AddressBill4Label.Text = DR_AddressGet("Row4")
                        Me.AddressBill5Label.Text = DR_AddressGet("Row5")
                        Me.AddressBill6Label.Text = DR_AddressGet("Row6")
                        Me.billKeyLabel.Text = DR_AddressGet("AddressKey")
                        Me.AddressBillingEditButton.Text = "Edit"
                End Select

            Next

        Catch ex As Exception

        End Try

        Connection.Close()
    End Sub

    Protected Sub AddressShippingEditButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.AdressAddShippingPanel.Visible = True
        Select Case Me.AddressShippingEditButton.Text
            Case "Edit"
                AddressSave("Edit", "Ship")
            Case "New"

        End Select
    End Sub

    Protected Sub AddressShippingSaveButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AddressSave("Save", "Ship")
        Me.AdressAddShippingPanel.Visible = False
    End Sub

    Protected Sub AddressBillingEditButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case Me.AddressBillingEditButton.Text
            Case "Edit"

            Case "New"
                Me.AdressAddBillingPanel.Visible = True
        End Select
    End Sub

    Protected Sub ABSaveButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.AdressAddBillingPanel.Visible = False
    End Sub

    Protected Sub IDDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not Me.IDDropDownList.SelectedItem.Text = "Add ID..." Then
            IDSelected(True)
        Else
            IDSelected(False)
        End If
    End Sub

    Protected Sub IDDefectsGridView_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles IDDefectsGridView.RowDeleted
        IDSelected(True)
    End Sub

    Protected Sub AddressShippingStreetRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.AddressShippingStreetRadioButton.Checked = True Then
            Me.ASAddPOBoxLabel.Visible = False
            Me.ASAddPoBoxTextBox.Visible = False
            Me.ASAddStreetNumberLabel.Visible = True
            Me.ASAddStreetNumberTextBox.Visible = True
            Me.ASAddDirectionLabel.Visible = True
            Me.ASAddDirectionTextBox.Visible = True
            Me.ASAddStreetNameLabel.Visible = True
            Me.ASAddStreetNameTextBox.Visible = True
            Me.ASAddStreetTypeLabel.Visible = True
            Me.ASAddStreetTypeTextBox.Visible = True
        End If
    End Sub

    Protected Sub AddressShippingPOBoxRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ASAddPOBoxLabel.Visible = True
        Me.ASAddPoBoxTextBox.Visible = True
        Me.ASAddStreetNumberLabel.Visible = False
        Me.ASAddStreetNumberTextBox.Visible = False
        Me.ASAddDirectionLabel.Visible = False
        Me.ASAddDirectionTextBox.Visible = False
        Me.ASAddStreetNameLabel.Visible = False
        Me.ASAddStreetNameTextBox.Visible = False
        Me.ASAddStreetTypeLabel.Visible = False
        Me.ASAddStreetTypeTextBox.Visible = False
    End Sub

    Sub AddressSave(ByVal EditOrSave As String, ByVal ShipOrBill As String)
        Dim SelectSQL As String = ""
        Dim DBKey As String = ""
        Dim NewKey As String = ""
        Select Case ShipOrBill
            Case "Ship"
                DBKey = Me.ShipKeyLabel.Text

            Case "Bill"
                DBKey = Me.BillKeyLabel.Text

            Case Else
                Exit Sub

        End Select

        Select Case EditOrSave
            Case "Edit"
                SelectSQL = "SELECT [Key], CustomerID, Attn, Building, POBox, Street_No, STREET_ADDRESS, Street_Compass, Street_Type, CITY, State, ZIP, Country, TELEPHONE, FAX, Operator, EffectiveDtd FROM dbo.CustomerAddresses WHERE ([Key] = '" & DBKey & "')"

            Case "Save"
                SelectSQL = "SELECT [Key], CustomerID, Attn, Building, POBox, Street_No, STREET_ADDRESS, Street_Compass, Street_Type, CITY, State, ZIP, Country, TELEPHONE, FAX, Operator, EffectiveDtd FROM dbo.CustomerAddresses WHERE ([Key] = '" & DBKey & "')"

            Case Else
                Exit Sub

        End Select

        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA_Address As New Data.SqlClient.SqlDataAdapter
        Dim DS_Address As New Data.DataSet
        Dim DR_Address As Data.DataRow

        Dim AddressSelectCmd As New System.Data.SqlClient.SqlCommand
        With AddressSelectCmd
            .CommandText = SelectSQL
            .Connection = Connection
        End With
        DA_Address.SelectCommand = AddressSelectCmd

        Dim AddressInsertCmd As New System.Data.SqlClient.SqlCommand
        With AddressInsertCmd
            .CommandText = "INSERT INTO [dbo].[CustomerAddresses] ([CustomerID], [Attn], [Building], [POBox], [Street_No], [STREET_ADDRESS], [Street_Compass], [Street_Type], [CITY], [State], [ZIP], [Country], [TELEPHONE], [FAX], [Operator], [EffectiveDtd]) VALUES (@CustomerID, @Attn, @Building, @POBox, @Street_No, @STREET_ADDRESS, @Street_Compass, @Street_Type, @CITY, @State, @ZIP, @Country, @TELEPHONE, @FAX, @Operator, @EffectiveDtd); SELECT [Key], CustomerID, Attn, Building, POBox, Street_No, STREET_ADDRESS, Street_Compass, Street_Type, CITY, State, ZIP, Country, TELEPHONE, FAX, Operator, EffectiveDtd FROM dbo.CustomerAddresses WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@CustomerID", System.Data.SqlDbType.NVarChar, 0, "CustomerID"), New System.Data.SqlClient.SqlParameter("@Attn", System.Data.SqlDbType.NVarChar, 0, "Attn"), New System.Data.SqlClient.SqlParameter("@Building", System.Data.SqlDbType.NVarChar, 0, "Building"), New System.Data.SqlClient.SqlParameter("@POBox", System.Data.SqlDbType.NVarChar, 0, "POBox"), New System.Data.SqlClient.SqlParameter("@Street_No", System.Data.SqlDbType.Float, 0, "Street_No"), New System.Data.SqlClient.SqlParameter("@STREET_ADDRESS", System.Data.SqlDbType.NVarChar, 0, "STREET_ADDRESS"), New System.Data.SqlClient.SqlParameter("@Street_Compass", System.Data.SqlDbType.NVarChar, 0, "Street_Compass"), New System.Data.SqlClient.SqlParameter("@Street_Type", System.Data.SqlDbType.NVarChar, 0, "Street_Type"), New System.Data.SqlClient.SqlParameter("@CITY", System.Data.SqlDbType.NVarChar, 0, "CITY"), New System.Data.SqlClient.SqlParameter("@State", System.Data.SqlDbType.NVarChar, 0, "State"), New System.Data.SqlClient.SqlParameter("@ZIP", System.Data.SqlDbType.NVarChar, 0, "ZIP"), New System.Data.SqlClient.SqlParameter("@Country", System.Data.SqlDbType.NVarChar, 0, "Country"), New System.Data.SqlClient.SqlParameter("@TELEPHONE", System.Data.SqlDbType.NVarChar, 0, "TELEPHONE"), New System.Data.SqlClient.SqlParameter("@FAX", System.Data.SqlDbType.NVarChar, 0, "FAX"), New System.Data.SqlClient.SqlParameter("@Operator", System.Data.SqlDbType.NVarChar, 0, "Operator"), New System.Data.SqlClient.SqlParameter("@EffectiveDtd", System.Data.SqlDbType.SmallDateTime, 0, "EffectiveDtd")})
        End With
        DA_Address.InsertCommand = AddressInsertCmd

        Dim AddressUpdateCmd As New System.Data.SqlClient.SqlCommand
        With AddressUpdateCmd
            .CommandText = "UPDATE [dbo].[CustomerAddresses] SET [CustomerID] = @CustomerID, [Attn] = @Attn, [Building] = @Building, [POBox] = @POBox, [Street_No] = @Street_No, [STREET_ADDRESS] = @STREET_ADDRESS, [Street_Compass] = @Street_Compass, [Street_Type] = @Street_Type, [CITY] = @CITY, [State] = @State, [ZIP] = @ZIP, [Country] = @Country, [TELEPHONE] = @TELEPHONE, [FAX] = @FAX, [Operator] = @Operator, [EffectiveDtd] = @EffectiveDtd WHERE (([Key] = @Original_Key) AND ([CustomerID] = @Original_CustomerID) AND ((@IsNull_Attn = 1 AND [Attn] IS NULL) OR ([Attn] = @Original_Attn)) AND ((@IsNull_Building = 1 AND [Building] IS NULL) OR ([Building] = @Original_Building)) AND ((@IsNull_POBox = 1 AND [POBox] IS NULL) OR ([POBox] = @Original_POBox)) AND ((@IsNull_Street_No = 1 AND [Street_No] IS NULL) OR ([Street_No] = @Original_Street_No)) AND ((@IsNull_STREET_ADDRESS = 1 AND [STREET_ADDRESS] IS NULL) OR ([STREET_ADDRESS] = @Original_STREET_ADDRESS)) AND ((@IsNull_Street_Compass = 1 AND [Street_Compass] IS NULL) OR ([Street_Compass] = @Original_Street_Compass)) AND ((@IsNull_Street_Type = 1 AND [Street_Type] IS NULL) OR ([Street_Type] = @Original_Street_Type)) AND ((@IsNull_CITY = 1 AND [CITY] IS NULL) OR ([CITY] = @Original_CITY)) AND ((@IsNull_State = 1 AND [State] IS NULL) OR ([State] = @Original_State)) AND ((@IsNull_ZIP = 1 AND [ZIP] IS NULL) OR ([ZIP] = @Original_ZIP)) AND ((@IsNull_Country = 1 AND [Country] IS NULL) OR ([Country] = @Original_Country)) AND ((@IsNull_TELEPHONE = 1 AND [TELEPHONE] IS NULL) OR ([TELEPHONE] = @Original_TELEPHONE)) AND ((@IsNull_FAX = 1 AND [FAX] IS NULL) OR ([FAX] = @Original_FAX)) AND ([Operator] = @Original_Operator) AND ([EffectiveDtd] = @Original_EffectiveDtd)); SELECT [Key], CustomerID, Attn, Building, POBox, Street_No, STREET_ADDRESS, Street_Compass, Street_Type, CITY, State, ZIP, Country, TELEPHONE, FAX, Operator, EffectiveDtd FROM dbo.CustomerAddresses WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@CustomerID", System.Data.SqlDbType.NVarChar, 0, "CustomerID"), New System.Data.SqlClient.SqlParameter("@Attn", System.Data.SqlDbType.NVarChar, 0, "Attn"), New System.Data.SqlClient.SqlParameter("@Building", System.Data.SqlDbType.NVarChar, 0, "Building"), New System.Data.SqlClient.SqlParameter("@POBox", System.Data.SqlDbType.NVarChar, 0, "POBox"), New System.Data.SqlClient.SqlParameter("@Street_No", System.Data.SqlDbType.Float, 0, "Street_No"), New System.Data.SqlClient.SqlParameter("@STREET_ADDRESS", System.Data.SqlDbType.NVarChar, 0, "STREET_ADDRESS"), New System.Data.SqlClient.SqlParameter("@Street_Compass", System.Data.SqlDbType.NVarChar, 0, "Street_Compass"), New System.Data.SqlClient.SqlParameter("@Street_Type", System.Data.SqlDbType.NVarChar, 0, "Street_Type"), New System.Data.SqlClient.SqlParameter("@CITY", System.Data.SqlDbType.NVarChar, 0, "CITY"), New System.Data.SqlClient.SqlParameter("@State", System.Data.SqlDbType.NVarChar, 0, "State"), New System.Data.SqlClient.SqlParameter("@ZIP", System.Data.SqlDbType.NVarChar, 0, "ZIP"), New System.Data.SqlClient.SqlParameter("@Country", System.Data.SqlDbType.NVarChar, 0, "Country"), New System.Data.SqlClient.SqlParameter("@TELEPHONE", System.Data.SqlDbType.NVarChar, 0, "TELEPHONE"), New System.Data.SqlClient.SqlParameter("@FAX", System.Data.SqlDbType.NVarChar, 0, "FAX"), New System.Data.SqlClient.SqlParameter("@Operator", System.Data.SqlDbType.NVarChar, 0, "Operator"), New System.Data.SqlClient.SqlParameter("@EffectiveDtd", System.Data.SqlDbType.SmallDateTime, 0, "EffectiveDtd"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CustomerID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CustomerID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Attn", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Attn", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Attn", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Attn", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Building", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Building", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Building", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Building", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_POBox", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "POBox", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_POBox", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "POBox", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Street_No", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Street_No", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Street_No", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Street_No", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_STREET_ADDRESS", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "STREET_ADDRESS", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_STREET_ADDRESS", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "STREET_ADDRESS", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Street_Compass", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Street_Compass", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Street_Compass", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Street_Compass", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Street_Type", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Street_Type", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Street_Type", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Street_Type", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_CITY", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "CITY", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_CITY", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CITY", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_State", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "State", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_State", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "State", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_ZIP", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "ZIP", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_ZIP", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ZIP", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Country", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Country", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Country", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Country", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_TELEPHONE", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "TELEPHONE", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_TELEPHONE", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "TELEPHONE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_FAX", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "FAX", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_FAX", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "FAX", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Operator", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Operator", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_EffectiveDtd", System.Data.SqlDbType.SmallDateTime, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "EffectiveDtd", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA_Address.UpdateCommand = AddressUpdateCmd

        DA_Address.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "CustomerAddresses", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("CustomerID", "CustomerID"), New System.Data.Common.DataColumnMapping("Attn", "Attn"), New System.Data.Common.DataColumnMapping("Building", "Building"), New System.Data.Common.DataColumnMapping("POBox", "POBox"), New System.Data.Common.DataColumnMapping("Street_No", "Street_No"), New System.Data.Common.DataColumnMapping("STREET_ADDRESS", "STREET_ADDRESS"), New System.Data.Common.DataColumnMapping("Street_Compass", "Street_Compass"), New System.Data.Common.DataColumnMapping("Street_Type", "Street_Type"), New System.Data.Common.DataColumnMapping("CITY", "CITY"), New System.Data.Common.DataColumnMapping("State", "State"), New System.Data.Common.DataColumnMapping("ZIP", "ZIP"), New System.Data.Common.DataColumnMapping("Country", "Country"), New System.Data.Common.DataColumnMapping("TELEPHONE", "TELEPHONE"), New System.Data.Common.DataColumnMapping("FAX", "FAX"), New System.Data.Common.DataColumnMapping("Operator", "Operator"), New System.Data.Common.DataColumnMapping("EffectiveDtd", "EffectiveDtd")})})

        DA_Address.Fill(DS_Address)

        Select Case EditOrSave
            Case "Edit" 'Fill for edit
                DR_Address = DS_Address.Tables(0).Rows(0)
                If ShipOrBill = "Ship" Then
                    Me.ASAddAttnTextBox.Text = DR_Address("Attn").ToString
                    Me.ASAddBuildingTextBox.Text = DR_Address("Building").ToString
                    Me.ASAddPOBoxLabel.Text = DR_Address("POBox").ToString
                    Me.ASAddStreetNumberTextBox.Text = DR_Address("Street_No").ToString
                    Me.ASAddDirectionTextBox.Text = DR_Address("Street_Compass").ToString
                    Me.ASAddStreetNameTextBox.Text = DR_Address("STREET_ADDRESS").ToString
                    Me.ASAddStreetTypeTextBox.Text = DR_Address("Street_Type").ToString
                    Me.ASAddCityTextBox.Text = DR_Address("CITY").ToString
                    Me.ASAddStateTextBox.Text = DR_Address("State").ToString
                    Me.ASAddZipCodeTextBox.Text = DR_Address("ZIP").ToString
                    Me.ASAddCountryTextBox.Text = DR_Address("Country").ToString
                    Me.ASAddPhoneTextBox.Text = DR_Address("TELEPHONE").ToString
                    Me.ASAddFaxTextBox.Text = DR_Address("FAX").ToString
                End If
                If ShipOrBill = "Bill" Then
                    Me.ABAddAttnTextBox.Text = DR_Address("Attn").ToString
                    Me.ABAddBuildingTextBox.Text = DR_Address("Building").ToString
                    Me.ABAddPOBoxLabel.Text = DR_Address("POBox").ToString
                    Me.ABAddStreetNumberTextBox.Text = DR_Address("Street_No").ToString
                    Me.ABAddDirectionTextBox.Text = DR_Address("Street_Compass").ToString
                    Me.ABAddStreetNameTextBox.Text = DR_Address("STREET_ADDRESS").ToString
                    Me.ABAddStreetTypeTextBox.Text = DR_Address("Street_Type").ToString
                    Me.ABAddCityTextBox.Text = DR_Address("CITY").ToString
                    Me.ABAddStateTextBox.Text = DR_Address("State").ToString
                    Me.ABAddZipCodeTextBox.Text = DR_Address("ZIP").ToString
                    Me.ABAddCountryTextBox.Text = DR_Address("Country").ToString
                    Me.ABAddPhoneTextBox.Text = DR_Address("TELEPHONE").ToString
                    Me.ABAddFaxTextBox.Text = DR_Address("FAX").ToString
                End If

            Case "Save"
                If ShipOrBill = "Ship" Then
                    Select Case Me.AddressShippingEditButton.Text
                        Case "Edit"
                            DR_Address = DS_Address.Tables(0).Rows(0)
                            DR_Address.AcceptChanges()
                            DR_Address.BeginEdit()
                            DR_Address("Attn") = Me.ASAddAttnTextBox.Text
                            DR_Address("Building") = Me.ASAddBuildingTextBox.Text
                            If Not Me.ASAddPOBoxLabel.Text = "" Then
                                DR_Address("POBox") = Me.ASAddPOBoxLabel.Text
                            Else
                                DR_Address("Street_No") = Me.ASAddStreetNumberTextBox.Text
                                DR_Address("Street_Compass") = Me.ASAddDirectionTextBox.Text
                                DR_Address("STREET_ADDRESS") = Me.ASAddStreetNameTextBox.Text
                                DR_Address("Street_Type") = Me.ASAddStreetTypeTextBox.Text
                            End If
                            DR_Address("CITY") = Me.ASAddCityTextBox.Text
                            DR_Address("State") = Me.ASAddStateTextBox.Text
                            DR_Address("ZIP") = Me.ASAddZipCodeTextBox.Text
                            DR_Address("Country") = Me.ASAddCountryTextBox.Text
                            DR_Address("TELEPHONE") = Me.ASAddPhoneTextBox.Text
                            DR_Address("FAX") = Me.ASAddFaxTextBox.Text
                            DR_Address.EndEdit()
                            DA_Address.Update(DS_Address, "CustomerAddresses")

                        Case "New"
                            DR_Address = DS_Address.Tables("CustomerAddresses").NewRow
                            If Not Me.ASAddAttnTextBox.Text = "" Then
                                DR_Address("Attn") = Me.ASAddAttnTextBox.Text
                            End If

                            If Not Me.ASAddBuildingTextBox.Text = "" Then
                                DR_Address("Building") = Me.ASAddBuildingTextBox.Text
                            End If

                            If Not Me.ASAddPOBoxLabel.Text = "" Then
                                DR_Address("POBox") = Me.ASAddPOBoxLabel.Text
                            End If

                            If Not Me.ASAddStreetNumberTextBox.Text = "" Then
                                DR_Address("Street_No") = Me.ASAddStreetNumberTextBox.Text
                            End If

                            If Not Me.ASAddDirectionTextBox.Text = "" Then
                                DR_Address("Street_Compass") = Me.ASAddDirectionTextBox.Text
                            End If
                            If Not Me.ASAddStreetNameTextBox.Text = "" Then
                                DR_Address("STREET_ADDRESS") = Me.ASAddStreetNameTextBox.Text
                            End If

                            If Not Me.ASAddStreetTypeTextBox.Text = "" Then
                                DR_Address("Street_Type") = Me.ASAddStreetTypeTextBox.Text
                            End If

                            If Not Me.ASAddCityTextBox.Text = "" Then
                                DR_Address("CITY") = Me.ASAddCityTextBox.Text
                            End If
                            If Not Me.ASAddStateTextBox.Text = "" Then
                                DR_Address("State") = Me.ASAddStateTextBox.Text
                            End If
                            If Not Me.ASAddZipCodeTextBox.Text = "" Then
                                DR_Address("ZIP") = Me.ASAddZipCodeTextBox.Text
                            End If
                            If Not Me.ASAddCountryTextBox.Text = "" Then
                                DR_Address("Country") = Me.ASAddCountryTextBox.Text
                            End If
                            If Not Me.ASAddPhoneTextBox.Text = "" Then
                                DR_Address("TELEPHONE") = Me.ASAddPhoneTextBox.Text
                            End If
                            If Not Me.ASAddFaxTextBox.Text = "" Then
                                DR_Address("FAX") = Me.ASAddFaxTextBox.Text
                            End If
                            DR_Address("CustomerID") = Me.FabDropDownList.SelectedItem.Text
                            DR_Address("EffectiveDtd") = DateTime.Now.ToShortDateString
                            DR_Address("Operator") = User.Identity.Name.ToString 'Operator
                            DS_Address.Tables("CustomerAddresses").Rows.Add(DR_Address)
                            DA_Address.Update(DS_Address, "CustomerAddresses")
                            NewKey = DR_Address("Key")

                    End Select

                End If
        End Select

        Connection.Close()

        If Not NewKey = "" Then
            Dim DA_IDAddress As New Data.SqlClient.SqlDataAdapter
            Dim DS_IDAddress As New Data.DataSet
            Dim DR_IDAddress As Data.DataRow

            Dim IDAddressSelectCmd As New System.Data.SqlClient.SqlCommand
            With IDAddressSelectCmd
                .CommandText = "SELECT MainID, Address_Key, Address_Type, EffectiveDtd FROM dbo.MainID_Address WHERE (Address_Key = 0)"
                .Connection = Connection
            End With
            DA_IDAddress.SelectCommand = IDAddressSelectCmd

            Dim IDAddressInsertCmd As New System.Data.SqlClient.SqlCommand
            With IDAddressInsertCmd
                .CommandText = "INSERT INTO [dbo].[MainID_Address] ([MainID], [Address_Key], [Address_Type], [EffectiveDtd]) VALUES (@MainID, @Address_Key, @Address_Type, @EffectiveDtd); SELECT MainID, Address_Key, Address_Type, EffectiveDtd FROM dbo.MainID_Address WHERE (Address_Key = @Address_Key) AND (Address_Type = @Address_Type) AND (MainID = @MainID)"
                .Connection = Connection
                .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@Address_Key", System.Data.SqlDbType.Int, 0, "Address_Key"), New System.Data.SqlClient.SqlParameter("@Address_Type", System.Data.SqlDbType.TinyInt, 0, "Address_Type"), New System.Data.SqlClient.SqlParameter("@EffectiveDtd", System.Data.SqlDbType.SmallDateTime, 0, "EffectiveDtd")})
            End With
            DA_IDAddress.InsertCommand = IDAddressInsertCmd

            DA_IDAddress.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "MainID_Address", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("MainID", "MainID"), New System.Data.Common.DataColumnMapping("Address_Key", "Address_Key"), New System.Data.Common.DataColumnMapping("Address_Type", "Address_Type"), New System.Data.Common.DataColumnMapping("EffectiveDtd", "EffectiveDtd")})})

            DA_IDAddress.Fill(DS_IDAddress)
            DR_IDAddress = DS_IDAddress.Tables("MainID_Address").NewRow
            DR_IDAddress("MainID") = Me.IDDropDownList.SelectedItem.Text
            DR_IDAddress("Address_Key") = NewKey
            If ShipOrBill = "Ship" Then
                DR_IDAddress("Address_Type") = 0
            End If
            If ShipOrBill = "Bill" Then
                DR_IDAddress("Address_Type") = 1
            End If
            DR_IDAddress("EffectiveDtd") = DateTime.Now.ToShortDateString

            DS_IDAddress.Tables("MainID_Address").Rows.Add(DR_IDAddress)
            DA_IDAddress.Update(DS_IDAddress, "MainID_Address")

        End If
    End Sub

    
End Class
