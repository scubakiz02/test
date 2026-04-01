
Partial Class PC_Make_FGI_PalletHolding
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1

    Protected Sub TextBoxCBIn_TextChanged(sender As Object, e As EventArgs) Handles TextBoxCBIn.TextChanged
        CheckDataInForCB()
        Me.HyperLink9.Visible = False
        CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.TextBoxCBIn.ClientID)
        Me.TextBoxCBIn.Focus()
    End Sub

    Sub CheckDataInForCB()
        If Not Me.TextBoxCBIn.Text = "" Then
            TextBoxCBIn.Text = UCase(TextBoxCBIn.Text)

            For i As Integer = 0 To ListBoxCB.Items.Count - 1
                If TextBoxCBIn.Text = ListBoxCB.Items(i).Text Then
                    Me.TextBoxCBIn.Text = ""
                    CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.TextBoxCBIn.ClientID)
                    Me.TextBoxCBIn.Focus()
                    Exit Sub
                End If
            Next


            If LabelCartonSet.Text = "Carton Type" Or LabelCartonSet.Text = "Carton Type = CB" Then
                If Mid(TextBoxCBIn.Text, 1, 2) = "CB" Then
                    Me.LabelCartonSet.Text = "Carton Type = CB"
                    Me.ListBoxCB.Items.Add(Me.TextBoxCBIn.Text)
                    Me.LabelCartonsadded.Text = LabelCartonsadded.Text + 1
                End If
            End If

            If LabelCartonSet.Text = "Carton Type" Or LabelCartonSet.Text = "Carton Type = C" Then
                If Mid(TextBoxCBIn.Text, 1, 1) = "C" Then
                    Me.LabelCartonSet.Text = "Carton Type = C"
                    Me.ListBoxCB.Items.Add(Me.TextBoxCBIn.Text)
                    Me.LabelCartonsadded.Text = LabelCartonsadded.Text + 1
                End If
            End If

            Me.TextBoxCBIn.Text = ""
            CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.TextBoxCBIn.ClientID)
            Me.TextBoxCBIn.Focus()
        End If
    End Sub

    Protected Sub ButtonMakePalletHolding_Click(sender As Object, e As EventArgs) Handles ButtonMakePalletHolding.Click
        Dim TheUser As String = User.Identity.Name.ToString
        Dim CartonType As String = Me.LabelCartonSet.Text
        If CartonType = "Carton Type = CB" Then
            EnterCB(TheUser, Get_PH_Number(TheUser, "CB"))
        End If
        If CartonType = "Carton Type = C" Then
            EnterCB(TheUser, Get_PH_Number(TheUser, "C"))
        End If
        CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.TextBoxCBIn.ClientID)
        Me.TextBoxCBIn.Focus()
    End Sub

    Function Get_PH_Number(TheUserName As String, CartonType As String) As Integer

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
            .CommandText = "SELECT PH_Key, SatiUser, Note FROM T_PH_Keys_Table WHERE (SatiUser = N'None')"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd
        '*****************************************************************
        '************************Insert***********************************
        '*****************************************************************
        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [T_PH_Keys_Table] ([SatiUser], [Note]) VALUES (@SatiUser, @Note); SELECT PH_Key, SatiUser, Note FROM T_PH_Keys_Table WHERE (PH_Key = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@SatiUser", System.Data.SqlDbType.NVarChar, 0, "SatiUser"), New System.Data.SqlClient.SqlParameter("@Note", System.Data.SqlDbType.NVarChar, 0, "Note")})
        End With
        DA.InsertCommand = InsertCmd
        '*****************************************************************
        '************************Update***********************************
        '*****************************************************************
        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [T_PH_Keys_Table] SET [SatiUser] = @SatiUser, [Note] = @Note WHERE (([PH_Key] = @Original_PH_Key) AND ((@IsNull_SatiUser = 1 AND [SatiUser] IS NULL) OR ([SatiUser] = @Original_SatiUser)) AND ((@IsNull_Note = 1 AND [Note] IS NULL) OR ([Note] = @Original_Note))); SELECT PH_Key, SatiUser, Note FROM T_PH_Keys_Table WHERE (PH_Key = @PH_Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@SatiUser", System.Data.SqlDbType.NVarChar, 0, "SatiUser"), New System.Data.SqlClient.SqlParameter("@Note", System.Data.SqlDbType.NVarChar, 0, "Note"), New System.Data.SqlClient.SqlParameter("@Original_PH_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PH_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_SatiUser", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "SatiUser", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_SatiUser", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SatiUser", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Note", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Note", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Note", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Note", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@PH_Key", System.Data.SqlDbType.Int, 4, "PH_Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_PH_Keys_Table", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("PH_Key", "PH_Key"), New System.Data.Common.DataColumnMapping("SatiUser", "SatiUser"), New System.Data.Common.DataColumnMapping("Note", "Note")})})
        DA.Fill(DS)


        DR = DS.Tables("T_PH_Keys_Table").NewRow
        DR("SatiUser") = TheUserName
        DR("Note") = CartonType

        DS.Tables("T_PH_Keys_Table").Rows.Add(DR)
        DA.Update(DS, "T_PH_Keys_Table")
        Get_PH_Number = DR("PH_Key")

        Connection.Close()

    End Function


    Sub EnterCB(TheUserName As String, PH_Key As Integer)
        Dim Flex As New FlexCel.XlsAdapter.XlsFile(True)
        Dim Path As String = "\\PWI-40\software$\LabelTemplates\PalletHolding.xls"
        Dim FileName As String = "PalletHolding_" & TheUserName & "_" & Date.Now.ToFileTime
        Dim FilePath_Name As String = "\\PWI-40\software$\LabelTemplates\LabelArchive\PalletHolding\" & FileName & ".xls"
        Dim Link_Path As String = "http://pwi-40:81/LabelTemp/PalletHolding/" & FileName & ".xls"
        Dim BoxString As String = ""
        Dim Boxtype As String = ""
        Dim PH_Info_SQL As String = "SELECT dbo.T_PH_Keys_Table.PH_Key, dbo.T_PH_Keys_Table.Note, dbo.MainID.MainID, dbo.Customer.Customer_Name FROM dbo.ShippingInventory INNER JOIN dbo.MainID INNER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID INNER JOIN dbo.LabelsMade ON dbo.MainID.MainID = LEFT(dbo.LabelsMade.Lot, 4) ON dbo.ShippingInventory.LotEntry = dbo.LabelsMade.LabelRecordNumber INNER JOIN dbo.T_PH_Table INNER JOIN dbo.T_PH_Keys_Table ON dbo.T_PH_Table.PH_Key = dbo.T_PH_Keys_Table.PH_Key ON dbo.ShippingInventory.Carton_Key = dbo.T_PH_Table.CB GROUP BY dbo.T_PH_Keys_Table.PH_Key, dbo.T_PH_Keys_Table.Note, dbo.MainID.MainID, dbo.Customer.Customer_Name HAVING (dbo.T_PH_Keys_Table.PH_Key = " & PH_Key & ")"

        Flex.Open(Path)
        Flex.Recalc(True)
        Flex.ActiveSheetByName = "Data"


        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim MyDS As New Data.DataSet
        Dim MyDR As Data.DataRow


        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        '*****************************************************************
        '************************Select***********************************
        '*****************************************************************
        Dim SelectCmd As New System.Data.SqlClient.SqlCommand
        With SelectCmd
            .CommandText = "SELECT Record_Key, PH_Key, CB, SatiUser FROM T_PH_Table WHERE (SatiUser = N'none')"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd
        '*****************************************************************
        '************************Insert***********************************
        '*****************************************************************
        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [T_PH_Table] ([PH_Key], [CB], [SatiUser]) VALUES (@PH_Key, @CB, @SatiUser); SELECT Record_Key, PH_Key, CB, SatiUser FROM T_PH_Table WHERE (Record_Key = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@PH_Key", System.Data.SqlDbType.Int, 0, "PH_Key"), New System.Data.SqlClient.SqlParameter("@CB", System.Data.SqlDbType.Int, 0, "CB"), New System.Data.SqlClient.SqlParameter("@SatiUser", System.Data.SqlDbType.NVarChar, 0, "SatiUser")})
        End With
        DA.InsertCommand = InsertCmd
        '*****************************************************************
        '************************Update***********************************
        '*****************************************************************
        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [T_PH_Table] SET [PH_Key] = @PH_Key, [CB] = @CB, [SatiUser] = @SatiUser WHERE (([Record_Key] = @Original_Record_Key) AND ([PH_Key] = @Original_PH_Key) AND ([CB] = @Original_CB) AND ((@IsNull_SatiUser = 1 AND [SatiUser] IS NULL) OR ([SatiUser] = @Original_SatiUser))); SELECT Record_Key, PH_Key, CB, SatiUser FROM T_PH_Table WHERE (Record_Key = @Record_Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@PH_Key", System.Data.SqlDbType.Int, 0, "PH_Key"), New System.Data.SqlClient.SqlParameter("@CB", System.Data.SqlDbType.Int, 0, "CB"), New System.Data.SqlClient.SqlParameter("@SatiUser", System.Data.SqlDbType.NVarChar, 0, "SatiUser"), New System.Data.SqlClient.SqlParameter("@Original_Record_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Record_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_PH_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PH_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CB", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CB", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_SatiUser", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "SatiUser", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_SatiUser", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SatiUser", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Record_Key", System.Data.SqlDbType.Int, 4, "Record_Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_PH_Table", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Record_Key", "Record_Key"), New System.Data.Common.DataColumnMapping("PH_Key", "PH_Key"), New System.Data.Common.DataColumnMapping("CB", "CB"), New System.Data.Common.DataColumnMapping("SatiUser", "SatiUser")})})
        DA.Fill(DS)


        If LabelCartonSet.Text = "Carton Type = CB" Then
            Boxtype = "New"
        End If
        If LabelCartonSet.Text = "Carton Type = C" Then
            Boxtype = "Old"
        End If

        Dim Rowindex As Integer = 10

        'loop for each CB that is for the PH
        For i = 0 To Me.ListBoxCB.Items.Count - 1
            If Not i = 0 Then
                BoxString = BoxString & ", "
            End If
            DR = DS.Tables("T_PH_Table").NewRow
            DR("PH_Key") = PH_Key 'PH_Key 

            Select Case Boxtype
                Case "New"
                    DR("CB") = Mid(ListBoxCB.Items(i).Text, 3) ' CB,
                    BoxString = BoxString & Mid(ListBoxCB.Items(i).Text, 3)
                    'SELECT TOP (100) PERCENT LEFT(dbo.LabelsMade.Lot, 4) AS LotID, dbo.T_FGI_Boxes.CartonNumber, dbo.LabelsMade.Wafers FROM dbo.LabelsMade INNER JOIN dbo.T_FGI_Boxes ON dbo.LabelsMade.LabelRecordNumber = dbo.T_FGI_Boxes.LabelsMadeKey GROUP BY LEFT(dbo.LabelsMade.Lot, 4), dbo.T_FGI_Boxes.CartonNumber, dbo.LabelsMade.Wafers HAVING (dbo.T_FGI_Boxes.CartonNumber = 212932) ORDER BY LotID
                    'MyDS = Saticode.GetMyDataSet("SELECT TOP (100) PERCENT LEFT(dbo.LabelsMade.Lot, 4) AS LotID, dbo.T_FGI_Boxes.CartonNumber, dbo.LabelsMade.Wafers FROM dbo.LabelsMade INNER JOIN dbo.T_FGI_Boxes ON dbo.LabelsMade.LabelRecordNumber = dbo.T_FGI_Boxes.LabelsMadeKey GROUP BY LEFT(dbo.LabelsMade.Lot, 4), dbo.T_FGI_Boxes.CartonNumber, dbo.LabelsMade.Wafers HAVING (dbo.T_FGI_Boxes.CartonNumber = " & Mid(ListBoxCB.Items(i).Text, 3) & ") ORDER BY LotID")
                    MyDS = Saticode.GetMyDataSet("SELECT TOP (100) PERCENT LEFT(dbo.LabelsMade.Lot, 4) AS LotID, dbo.T_FGI_Boxes.CartonNumber, SUM(dbo.LabelsMade.Wafers) AS Wafers FROM dbo.LabelsMade INNER JOIN dbo.T_FGI_Boxes ON dbo.LabelsMade.LabelRecordNumber = dbo.T_FGI_Boxes.LabelsMadeKey GROUP BY LEFT(dbo.LabelsMade.Lot, 4), dbo.T_FGI_Boxes.CartonNumber, dbo.LabelsMade.Wafers HAVING (dbo.T_FGI_Boxes.CartonNumber = " & Mid(ListBoxCB.Items(i).Text, 3) & ") ORDER BY LotID")
                    For R = 0 To MyDS.Tables(0).Rows.Count - 1
                        If R > 0 Then
                            Rowindex = Rowindex + 1
                        End If
                        MyDR = MyDS.Tables(0).Rows(R)
                        Flex.SetCellValue(i + Rowindex, 1, MyDR("LotID").ToString)
                        Flex.SetCellValue(i + Rowindex, 2, "CB" & MyDR("CartonNumber").ToString)
                        Flex.SetCellValue(i + Rowindex, 3, MyDR("Wafers"))
                    Next

                Case "Old"
                    DR("CB") = Mid(ListBoxCB.Items(i).Text, 2) ' C,
                    BoxString = BoxString & Mid(ListBoxCB.Items(i).Text, 2)
                    'SELECT TOP (100) PERCENT LEFT(dbo.LabelsMade.Lot, 4) AS LotID, SUM(dbo.LabelsMade.Wafers) AS Wafers, dbo.ShippingInventory.Carton_Key FROM dbo.LabelsMade INNER JOIN dbo.ShippingInventory ON dbo.LabelsMade.LabelRecordNumber = dbo.ShippingInventory.LotEntry GROUP BY LEFT(dbo.LabelsMade.Lot, 4), dbo.ShippingInventory.Carton_Key HAVING (dbo.ShippingInventory.Carton_Key = 212949) ORDER BY LotID
                    MyDS = Saticode.GetMyDataSet("SELECT TOP (100) PERCENT LEFT(dbo.LabelsMade.Lot, 4) AS LotID, SUM(dbo.ShippingInventory.Total_Qty) AS Wafers, dbo.ShippingInventory.Carton_Key FROM dbo.LabelsMade INNER JOIN dbo.ShippingInventory ON dbo.LabelsMade.LabelRecordNumber = dbo.ShippingInventory.LotEntry GROUP BY LEFT(dbo.LabelsMade.Lot, 4), dbo.ShippingInventory.Carton_Key HAVING (dbo.ShippingInventory.Carton_Key = " & Mid(ListBoxCB.Items(i).Text, 2) & ") ORDER BY LotID")

                    For R = 0 To MyDS.Tables(0).Rows.Count - 1
                        If R > 0 Then
                            Rowindex = Rowindex + 1
                        End If
                        MyDR = MyDS.Tables(0).Rows(R)
                        Flex.SetCellValue(i + Rowindex, 1, MyDR("LotID").ToString)
                        Flex.SetCellValue(i + Rowindex, 2, "C" & MyDR("Carton_Key").ToString)
                        Flex.SetCellValue(i + Rowindex, 3, MyDR("Wafers"))
                    Next

            End Select

            BoxString = BoxString &
            DR("SatiUser") = TheUserName ' SatiUser

            DS.Tables("T_PH_Table").Rows.Add(DR)
            DA.Update(DS, "T_PH_Table")

        Next

        'add other stuff to Sheet
        MyDS = Saticode.GetMyDataSet(PH_Info_SQL)

        Select Case MyDS.Tables(0).Rows.Count
            Case 1
                MyDR = MyDS.Tables(0).Rows(0)
                Flex.SetCellValue(1, 4, MyDR("Customer_Name")) 'Customer
                Flex.SetCellValue(2, 4, "ID# " & MyDR("MainID")) 'ID#
                Flex.SetCellValue(3, 4, "PH" & MyDR("PH_Key"))
                Flex.SetCellValue(4, 4, "Pallet FGI# " & PH_Key)

            Case > 1
                Dim M As String = ""
                For i = 0 To MyDS.Tables(0).Rows.Count - 1
                    MyDR = MyDS.Tables(0).Rows(i)
                    M = M & MyDR("MainID") & ", "
                Next
                Flex.SetCellValue(1, 4, "MIXED -- MIXED") 'Customer
                Flex.SetCellValue(2, 4, "ID# " & M) 'ID#
                Flex.SetCellValue(3, 4, "PH" & PH_Key)
                Flex.SetCellValue(4, 4, "Shelf MIX# " & PH_Key)
        End Select


        Connection.Close()
        Me.ListBoxCB.Items.Clear()
        Flex.RecalcAndVerify()
        Flex.ActiveSheetByName = "Pallet Barcode"
        Flex.RecalcAndVerify()

        Flex.Save(FilePath_Name)

        Me.HyperLink9.Visible = True
        Me.HyperLink9.NavigateUrl = Link_Path 'FilePath_Name
        CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.TextBoxCBIn.ClientID)
        Me.TextBoxCBIn.Focus()
    End Sub

    Private Sub PC_Make_FGI_PalletHolding_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)

        CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.TextBoxCBIn.ClientID)
        Me.TextBoxCBIn.Focus()
    End Sub

    Private Sub ListBoxCB_TextChanged(sender As Object, e As EventArgs) Handles ListBoxCB.TextChanged
        CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.TextBoxCBIn.ClientID)
        Me.TextBoxCBIn.Focus()
    End Sub
End Class
