
Partial Class PC_NonConformingManagment
    Inherits System.Web.UI.Page

    Sub MainUpdate()
        Me.DropDownListSolarType.AutoPostBack = False
        Me.LabelBelong.Visible = False
        IDInfo(Me.ListBoxId.SelectedItem.Text, "", "Check")
        LoadAvalibleIds(Me.ListBoxId.SelectedItem.Text)
        CheckBelong(Me.ListBoxId.SelectedItem.Text)
        SolarType(Me.ListBoxId.SelectedItem.Text, "Load")
        Me.LabelSelectedID.Text = Me.ListBoxId.SelectedItem.Text
        Me.LabelDiameter.Text = Me.DropDownListDiameter.SelectedItem.Text
        GridView1.DataBind()
        ListBoxSubId.DataBind()
        Me.DropDownListSolarType.AutoPostBack = True
    End Sub

    Sub SolarType(ByVal SelectedID As String, ByVal What As String)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand
        With SelectCmd
            .CommandText = "SELECT [Key], ID, Solar FROM dbo.T_NC_ID_Info WHERE (ID = N'" & SelectedID & "')"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [dbo].[T_NC_ID_Info] SET [ID] = @ID, [Solar] = @Solar WHERE (([Key] = @Original_Key) AND ([ID] = @Original_ID) AND ([Solar] = @Original_Solar)); SELECT [Key], ID, Solar FROM dbo.T_NC_ID_Info WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.NVarChar, 0, "ID"), New System.Data.SqlClient.SqlParameter("@Solar", System.Data.SqlDbType.NVarChar, 0, "Solar"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Solar", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Solar", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_NC_ID_Info", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("ID", "ID"), New System.Data.Common.DataColumnMapping("Solar", "Solar")})})
        DA.Fill(DS)

        Select Case What
            Case "Load"
                DR = DS.Tables(0).Rows(0)
                Select Case DR("Solar")
                    Case "C"
                        Me.DropDownListSolarType.SelectedIndex = 0
                    Case "P"
                        Me.DropDownListSolarType.SelectedIndex = 1
                    Case "N"
                        Me.DropDownListSolarType.SelectedIndex = 2
                End Select


            Case "Change"
                DR = DS.Tables(0).Rows(0)
                DR.AcceptChanges()
                DR.BeginEdit()
                DR("Solar") = Me.DropDownListSolarType.SelectedItem.Value
                DR.EndEdit()
                DA.Update(DS, "T_NC_ID_Info")
                MainUpdate()
        End Select

        Connection.Close()
    End Sub

    Protected Sub DropDownListCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListCustomer.SelectedIndexChanged
        Me.SqlDataSourceDiameter.SelectCommand = "SELECT dbo.Customer.Customer_Name, dbo.MainID.Diameter FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID GROUP BY dbo.Customer.Customer_Name, dbo.MainID.Diameter HAVING (dbo.Customer.Customer_Name = N'" & Me.DropDownListCustomer.SelectedItem.Text & "') ORDER BY dbo.MainID.Diameter"
    End Sub

    Protected Sub ButtonGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGetData.Click
        Me.SqlDataSourceIDlist.SelectCommand = "SELECT dbo.MainID.MainID, dbo.MainID.Diameter FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID WHERE (dbo.Customer.Customer_Name = N'" & Me.DropDownListCustomer.SelectedItem.Text & "') AND (dbo.MainID.Diameter = " & Me.DropDownListDiameter.SelectedItem.Text & ")"
    End Sub

    Protected Sub ListBoxId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBoxId.SelectedIndexChanged
        MainUpdate()
    End Sub

    Sub IDInfo(ByVal SelectedID As String, ByVal PackID As String, ByVal What As String)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand
        With SelectCmd
            .CommandText = "SELECT [Key], ID, PackWithID FROM dbo.T_NC_ID_Info WHERE (ID = N'" & SelectedID & "')"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [dbo].[T_NC_ID_Info] ([ID], [PackWithID]) VALUES (@ID, @PackWithID); SELECT [Key], ID, PackWithID FROM dbo.T_NC_ID_Info WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.NVarChar, 0, "ID"), New System.Data.SqlClient.SqlParameter("@PackWithID", System.Data.SqlDbType.NVarChar, 0, "PackWithID")})
        End With
        DA.InsertCommand = InsertCmd

        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [dbo].[T_NC_ID_Info] SET [ID] = @ID, [PackWithID] = @PackWithID WHERE (([Key] = @Original_Key) AND ([ID] = @Original_ID) AND ([PackWithID] = @Original_PackWithID)); SELECT [Key], ID, PackWithID FROM dbo.T_NC_ID_Info WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.NVarChar, 0, "ID"), New System.Data.SqlClient.SqlParameter("@PackWithID", System.Data.SqlDbType.NVarChar, 0, "PackWithID"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_PackWithID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PackWithID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_NC_ID_Info", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("ID", "ID"), New System.Data.Common.DataColumnMapping("PackWithID", "PackWithID")})})
        DA.Fill(DS)

        'Will check all the time
        If DS.Tables(0).Rows.Count = 0 Then
            DR = DS.Tables("T_NC_ID_Info").NewRow
            DR("ID") = SelectedID
            DR("PackWithID") = SelectedID
            DS.Tables("T_NC_ID_Info").Rows.Add(DR)
            DA.Update(DS, "T_NC_ID_Info")
        End If

        Select Case What
            Case "Check"


            Case "Change"

                DR = DS.Tables(0).Rows(0)
                DR.AcceptChanges()
                DR.BeginEdit()
                DR("ID") = SelectedID
                DR("PackWithID") = PackID
                DR.EndEdit()
                DA.Update(DS, "T_NC_ID_Info")
                MainUpdate()
        End Select

        Connection.Close()

    End Sub

    Sub LoadAvalibleIds(ByVal SelectedID As String)

        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()
        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim SelectCmd As New System.Data.SqlClient.SqlCommand
        With SelectCmd
            .CommandText = "SELECT dbo.MainID.MainID, dbo.MainID.Diameter FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID WHERE (dbo.Customer.Customer_Name = N'" & Me.DropDownListCustomer.SelectedItem.Text & "') AND (dbo.MainID.Diameter = " & Me.DropDownListDiameter.SelectedItem.Text & ")"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd
        DA.Fill(DS)


        Dim DA_Sub As New Data.SqlClient.SqlDataAdapter
        Dim DS_Sub As New Data.DataSet
        Dim Sub_SelectCmd As New System.Data.SqlClient.SqlCommand
        With Sub_SelectCmd
            .CommandText = "SELECT [ID] FROM [T_NC_ID_Info] WHERE (PackWithID = N'" & SelectedID & "')"
            .Connection = Connection
        End With
        DA_Sub.SelectCommand = Sub_SelectCmd
        DA_Sub.Fill(DS_Sub)

        Connection.Close()

        Me.ListBoxAvalibleSubId.Items.Clear()

        Dim DR As Data.DataRow
        Dim DR_Sub As Data.DataRow
        Dim SubCount As Integer = DS_Sub.Tables(0).Rows.Count - 1
        Dim FoundSub As Boolean
        For i As Integer = 0 To DS.Tables(0).Rows.Count - 1
            DR = DS.Tables(0).Rows(i)
            FoundSub = False
            For ii As Integer = 0 To SubCount
                DR_Sub = DS_Sub.Tables(0).Rows(ii)
                If DR("MainID").ToString = DR_Sub("ID").ToString Then
                    FoundSub = True
                End If
            Next
            If FoundSub = False Then
                Me.ListBoxAvalibleSubId.Items.Add(DR("MainID").ToString)
            End If

        Next


    End Sub

    Sub CheckBelong(ByVal SelectedID As String)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()
        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim SelectCmd As New System.Data.SqlClient.SqlCommand
        With SelectCmd
            .CommandText = "SELECT ID, PackWithID FROM dbo.T_NC_ID_Info WHERE (ID = N'" & SelectedID & "')"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd
        DA.Fill(DS)

        Dim DR As Data.DataRow

        DR = DS.Tables(0).Rows(0)
        If Not DR("ID") = DR("PackWithID") Then
            Me.LabelBelong.Visible = True
            Me.LabelBelong.Text = DR("ID") & " is a Sub ID of " & DR("PackwithID")
        End If
        Connection.Close()
    End Sub

    Protected Sub ButtonSubIdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSubIdAdd.Click
        Try
            IDInfo(Me.ListBoxAvalibleSubId.SelectedItem.Text, Me.LabelSelectedID.Text, "Change")
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub ButtonSubIdRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSubIdRemove.Click
        Try
            IDInfo(Me.ListBoxSubId.SelectedItem.Text, Me.ListBoxSubId.SelectedItem.Text, "Change")
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub DropDownListSolarType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListSolarType.SelectedIndexChanged
        SolarType(Me.ListBoxId.SelectedItem.Text, "Change")
    End Sub
End Class
