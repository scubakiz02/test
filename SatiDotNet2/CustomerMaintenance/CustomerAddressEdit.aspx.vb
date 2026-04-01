
Partial Class CustomerMaintenance_CustomerAddressEdit
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub

    Protected Sub CustomerDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ChangeCustomer()
    End Sub

    Sub ChangeCustomer()
        Me.FabDropDownList.Items.Clear()
        Me.FabsOnlySqlDataSource.SelectCommand = "SELECT CustomerID FROM dbo.Customer WHERE (Business_Name = N'" & Me.CustomerDropDownList.SelectedItem.Text & "') ORDER BY CustomerID"
        Me.FabDropDownList.Items.Add("Select Fab...")
        Me.FabDropDownList.DataBind()
        ChangeFab()

    End Sub

    Protected Sub FabDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ChangeFab()
    End Sub

    Sub ChangeFab()
        Me.IDDropDownList.Items.Clear()
        Me.IDDropDownList.Items.Clear()
        Me.MainSqlDataSource.SelectCommand = "SELECT MainID FROM dbo.MainID WHERE (CustomerID = N'" & Me.FabDropDownList.SelectedItem.Text & "')"
        Me.IDDropDownList.Items.Add("Select ID...")
        Me.IDDropDownList.DataBind()
        IDSelected(False)
    End Sub

    Protected Sub IDDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not Me.IDDropDownList.SelectedItem.Text = "Select ID..." Then
            IDSelected(True)
        Else
            IDSelected(False)
        End If
    End Sub

    Sub IDSelected(ByVal Switch As Boolean)
        If Switch = True Then
            Me.AddressContentPanel.Visible = True
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
            Me.AddressContentPanel.Visible = False
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
                Select Case DR_AddressGet("Type").ToString
                    Case "Shipping"
                        Me.AddressShip1Label.Text = DR_AddressGet("Row1")
                        Me.AddressShip2Label.Text = DR_AddressGet("Row2")
                        Me.AddressShip3Label.Text = DR_AddressGet("Row3")
                        Me.AddressShip4Label.Text = DR_AddressGet("Row4")
                        Me.AddressShip5Label.Text = DR_AddressGet("Row5")
                        Me.AddressShip6Label.Text = DR_AddressGet("Row6")
                        Me.ShipKeyLabel.Text = DR_AddressGet("AddressKey")
                        Me.AddressShippingEditButton.Text = "Edit"
                        Me.AdressAddShippingPanel.Visible = False
                    Case "Billing"
                        Me.AddressBill1Label.Text = DR_AddressGet("Row1")
                        Me.AddressBill2Label.Text = DR_AddressGet("Row2")
                        Me.AddressBill3Label.Text = DR_AddressGet("Row3")
                        Me.AddressBill4Label.Text = DR_AddressGet("Row4")
                        Me.AddressBill5Label.Text = DR_AddressGet("Row5")
                        Me.AddressBill6Label.Text = DR_AddressGet("Row6")
                        Me.BillKeyLabel.Text = DR_AddressGet("AddressKey")
                        Me.AddressBillingEditButton.Text = "Edit"
                        Me.AdressAddBillingPanel.Visible = False

                End Select

            Next

        Catch ex As Exception

        End Try

        Connection.Close()
    End Sub

    Protected Sub AddressShippingStreetRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.AddressShippingStreetRadioButton.Checked = True Then
            AddressLayout("Ship", "Street")
        End If
    End Sub

    Protected Sub AddressShippingPOBoxRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.AddressShippingPOBoxRadioButton.Checked = True Then
            AddressLayout("Ship", "PO")
        End If
    End Sub

    Protected Sub AddressBillingStreetRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.AddressBillingStreetRadioButton.Checked = True Then
            AddressLayout("Bill", "Street")
        End If
    End Sub

    Protected Sub AddressBillingPOBoxRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.AddressBillingPOBoxRadioButton.Checked = True Then
            AddressLayout("Bill", "PO")
        End If
    End Sub

    Sub AddressLayout(ByVal ShipOrBill As String, ByVal StreetOrPO As String)
        Select Case ShipOrBill
            Case "Ship"
                Select Case StreetOrPO
                    Case "Street"
                        Me.ShipPOLabel.Visible = False
                        Me.ASAddPoBoxTextBox.Visible = False
                        Me.ASAddStreetNumberLabel.Visible = True
                        Me.ASAddStreetNumberTextBox.Visible = True
                        Me.ASAddDirectionLabel.Visible = True
                        Me.ASAddDirectionTextBox.Visible = True
                        Me.ASAddStreetNameLabel.Visible = True
                        Me.ASAddStreetNameTextBox.Visible = True
                        Me.ASAddStreetTypeLabel.Visible = True
                        Me.ASAddStreetTypeTextBox.Visible = True

                    Case "PO"
                        Me.ShipPOLabel.Visible = True
                        Me.ASAddPoBoxTextBox.Visible = True
                        Me.ASAddStreetNumberLabel.Visible = False
                        Me.ASAddStreetNumberTextBox.Visible = False
                        Me.ASAddDirectionLabel.Visible = False
                        Me.ASAddDirectionTextBox.Visible = False
                        Me.ASAddStreetNameLabel.Visible = False
                        Me.ASAddStreetNameTextBox.Visible = False
                        Me.ASAddStreetTypeLabel.Visible = False
                        Me.ASAddStreetTypeTextBox.Visible = False
                End Select
            Case "Bill"
                Select Case StreetOrPO
                    Case "Street"
                        Me.ABAddPOBoxLabel.Visible = False
                        Me.ABAddPoBoxTextBox.Visible = False
                        Me.ABAddStreetNumberLabel.Visible = True
                        Me.ABAddStreetNumberTextBox.Visible = True
                        Me.ABAddDirectionLabel.Visible = True
                        Me.ABAddDirectionTextBox.Visible = True
                        Me.ABAddStreetNameLabel.Visible = True
                        Me.ABAddStreetNameTextBox.Visible = True
                        Me.ABAddStreetTypeLabel.Visible = True
                        Me.ABAddStreetTypeTextBox.Visible = True

                    Case "PO"
                        Me.ABAddPOBoxLabel.Visible = True
                        Me.ABAddPoBoxTextBox.Visible = True
                        Me.ABAddStreetNumberLabel.Visible = False
                        Me.ABAddStreetNumberTextBox.Visible = False
                        Me.ABAddDirectionLabel.Visible = False
                        Me.ABAddDirectionTextBox.Visible = False
                        Me.ABAddStreetNameLabel.Visible = False
                        Me.ABAddStreetNameTextBox.Visible = False
                        Me.ABAddStreetTypeLabel.Visible = False
                        Me.ABAddStreetTypeTextBox.Visible = False
                End Select
        End Select

    End Sub

    Sub AddressSave(ByVal EditOrSave As String, ByVal ShipOrBill As String)
        Dim SelectSQL As String = ""
        Dim DBKey As String = ""
        Dim NewKey As String = ""
        Dim MyNull As Object
        MyNull = System.DBNull.Value
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
                    Me.ASAddPoBoxTextBox.Text = DR_Address("POBox").ToString
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
                    If DR_Address("POBox").ToString = "" Then
                        AddressLayout("Ship", "Street")
                    Else
                        AddressLayout("Ship", "PO")
                    End If
                End If
                If ShipOrBill = "Bill" Then
                    Me.ABAddAttnTextBox.Text = DR_Address("Attn").ToString
                    Me.ABAddBuildingTextBox.Text = DR_Address("Building").ToString
                    Me.ABAddPoBoxTextBox.Text = DR_Address("POBox").ToString
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
                    If DR_Address("POBox").ToString = "" Then
                        AddressLayout("Bill", "Street")
                    Else
                        AddressLayout("Bill", "PO")
                    End If
                End If

            Case "Save"
                If ShipOrBill = "Ship" Then
                    Select Case Me.AddressShippingEditButton.Text
                        Case "Edit"
                            DR_Address = DS_Address.Tables(0).Rows(0)
                            DR_Address.AcceptChanges()
                            DR_Address.BeginEdit()

                            'Attn
                            If Not Me.ASAddAttnTextBox.Text = "" Then
                                DR_Address("Attn") = Me.ASAddAttnTextBox.Text
                            Else
                                DR_Address("Attn") = MyNull
                            End If

                            'Building
                            DR_Address("Building") = Me.ASAddBuildingTextBox.Text

                            'PO Box
                            If Not Me.ASAddPoBoxTextBox.Text = "" Then
                                DR_Address("POBox") = Me.ASAddPoBoxTextBox.Text
                            Else
                                If Not Me.ASAddStreetNumberTextBox.Text = "" Then
                                    DR_Address("Street_No") = Me.ASAddStreetNumberTextBox.Text
                                Else
                                    DR_Address("Street_No") = MyNull
                                End If
                                DR_Address("Street_Compass") = Me.ASAddDirectionTextBox.Text
                                DR_Address("STREET_ADDRESS") = Me.ASAddStreetNameTextBox.Text
                                DR_Address("Street_Type") = Me.ASAddStreetTypeTextBox.Text
                                DR_Address("POBox") = MyNull
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

                            If Not Me.ASAddPoBoxTextBox.Text = "" Then
                                DR_Address("POBox") = Me.ASAddPoBoxTextBox.Text
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
                'Start Bill****************************************************************************************
                If ShipOrBill = "Bill" Then
                    Select Case Me.AddressBillingEditButton.Text
                        Case "Edit"
                            DR_Address = DS_Address.Tables(0).Rows(0)
                            DR_Address.AcceptChanges()
                            DR_Address.BeginEdit()

                            'Attn
                            If Not Me.ABAddAttnTextBox.Text = "" Then
                                DR_Address("Attn") = Me.ABAddAttnTextBox.Text
                            Else
                                DR_Address("Attn") = MyNull
                            End If

                            'Building
                            DR_Address("Building") = Me.ABAddBuildingTextBox.Text

                            'PO Box
                            If Not Me.ABAddPoBoxTextBox.Text = "" Then
                                DR_Address("POBox") = Me.ABAddPoBoxTextBox.Text
                            Else
                                If Not Me.ABAddStreetNumberTextBox.Text = "" Then
                                    DR_Address("Street_No") = Me.ABAddStreetNumberTextBox.Text
                                Else
                                    DR_Address("Street_No") = MyNull
                                End If
                                DR_Address("Street_Compass") = Me.ABAddDirectionTextBox.Text
                                DR_Address("STREET_ADDRESS") = Me.ABAddStreetNameTextBox.Text
                                DR_Address("Street_Type") = Me.ABAddStreetTypeTextBox.Text
                                DR_Address("POBox") = MyNull
                            End If

                            DR_Address("CITY") = Me.ABAddCityTextBox.Text
                            DR_Address("State") = Me.ABAddStateTextBox.Text
                            DR_Address("ZIP") = Me.ABAddZipCodeTextBox.Text
                            DR_Address("Country") = Me.ABAddCountryTextBox.Text
                            DR_Address("TELEPHONE") = Me.ABAddPhoneTextBox.Text
                            DR_Address("FAX") = Me.ABAddFaxTextBox.Text
                            DR_Address.EndEdit()
                            DA_Address.Update(DS_Address, "CustomerAddresses")

                        Case "New"
                            DR_Address = DS_Address.Tables("CustomerAddresses").NewRow
                            If Not Me.ABAddAttnTextBox.Text = "" Then
                                DR_Address("Attn") = Me.ABAddAttnTextBox.Text
                            End If

                            If Not Me.ABAddBuildingTextBox.Text = "" Then
                                DR_Address("Building") = Me.ABAddBuildingTextBox.Text
                            End If

                            If Not Me.ABAddPoBoxTextBox.Text = "" Then
                                DR_Address("POBox") = Me.ABAddPoBoxTextBox.Text
                            End If

                            If Not Me.ABAddStreetNumberTextBox.Text = "" Then
                                DR_Address("Street_No") = Me.ABAddStreetNumberTextBox.Text
                            End If

                            If Not Me.ABAddDirectionTextBox.Text = "" Then
                                DR_Address("Street_Compass") = Me.ABAddDirectionTextBox.Text
                            End If
                            If Not Me.ABAddStreetNameTextBox.Text = "" Then
                                DR_Address("STREET_ADDRESS") = Me.ABAddStreetNameTextBox.Text
                            End If

                            If Not Me.ABAddStreetTypeTextBox.Text = "" Then
                                DR_Address("Street_Type") = Me.ABAddStreetTypeTextBox.Text
                            End If

                            If Not Me.ABAddCityTextBox.Text = "" Then
                                DR_Address("CITY") = Me.ABAddCityTextBox.Text
                            End If
                            If Not Me.ABAddStateTextBox.Text = "" Then
                                DR_Address("State") = Me.ABAddStateTextBox.Text
                            End If
                            If Not Me.ABAddZipCodeTextBox.Text = "" Then
                                DR_Address("ZIP") = Me.ABAddZipCodeTextBox.Text
                            End If
                            If Not Me.ABAddCountryTextBox.Text = "" Then
                                DR_Address("Country") = Me.ABAddCountryTextBox.Text
                            End If
                            If Not Me.ABAddPhoneTextBox.Text = "" Then
                                DR_Address("TELEPHONE") = Me.ABAddPhoneTextBox.Text
                            End If
                            If Not Me.ABAddFaxTextBox.Text = "" Then
                                DR_Address("FAX") = Me.ABAddFaxTextBox.Text
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
        'End Bill***********************************************************************

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
        IDSelected(True)
        Me.AdressAddShippingPanel.Visible = False

    End Sub

    Protected Sub AddressBillingEditButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.AdressAddBillingPanel.Visible = True
        Select Case Me.AddressBillingEditButton.Text
            Case "Edit"
                AddressSave("Edit", "Bill")
            Case "New"
                Me.AdressAddBillingPanel.Visible = True
        End Select
    End Sub

    Protected Sub ABSaveButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.AdressAddBillingPanel.Visible = False
    End Sub

   

End Class
