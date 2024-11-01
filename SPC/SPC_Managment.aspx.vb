
Partial Class SPC_SPC_Managment
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub DropDownListDepartments_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListDepartments.SelectedIndexChanged
        Me.SqlDataSourceTools.SelectCommand = "SELECT Tool FROM T_Tools WHERE (Department = '" & Me.DropDownListDepartments.SelectedItem.Text & "') ORDER BY Tool"
        Me.DropDownListTools.DataBind()

    End Sub
    Protected Sub DropDownListTools_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListTools.SelectedIndexChanged

    End Sub
    Protected Sub ButtonNewTool_Click(sender As Object, e As EventArgs) Handles ButtonNewTool.Click
        Me.PanelNewTool.Visible = True
    End Sub
    Protected Sub ButtonCloseAddToolPanel_Click(sender As Object, e As EventArgs) Handles ButtonCloseAddToolPanel.Click
        Me.PanelNewTool.Visible = False
    End Sub
    Protected Sub ButtonAddTool_Click(sender As Object, e As EventArgs) Handles ButtonAddTool.Click
        If Not Me.DropDownListDepartments.SelectedItem.Text = "" And Not Me.DropDownListTools.SelectedItem.Text = "" Then
            EnterTool(Me.DropDownListTools.SelectedItem.Text, Me.DropDownListDepartments.SelectedItem.Text)
        End If
        Me.GridView1.DataBind()

    End Sub

    Sub EnterTool(Tool As String, Department As String)

        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = Session("SATI_SPC_DB")
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT [Key], Tool_Name, Department FROM T_SPC_Tool_Info WHERE ([Key] = 0)"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [T_SPC_Tool_Info] ([Tool_Name], [Department]) VALUES (@Tool_Name, @Department); SELECT [Key], Tool_Name, Department FROM T_SPC_Tool_Info WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Tool_Name", System.Data.SqlDbType.NVarChar, 0, "Tool_Name"), New System.Data.SqlClient.SqlParameter("@Department", System.Data.SqlDbType.NVarChar, 0, "Department")})
        End With
        DA.InsertCommand = InsertCmd

        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [T_SPC_Tool_Info] SET [Tool_Name] = @Tool_Name, [Department] = @Department WHERE (([Key] = @Original_Key) AND ([Tool_Name] = @Original_Tool_Name) AND ([Department] = @Original_Department)); SELECT [Key], Tool_Name, Department FROM T_SPC_Tool_Info WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Tool_Name", System.Data.SqlDbType.NVarChar, 0, "Tool_Name"), New System.Data.SqlClient.SqlParameter("@Department", System.Data.SqlDbType.NVarChar, 0, "Department"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Tool_Name", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Tool_Name", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Department", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Department", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_SPC_Tool_Info", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("Tool_Name", "Tool_Name"), New System.Data.Common.DataColumnMapping("Department", "Department")})})
        DA.Fill(DS)


        DR = DS.Tables("T_SPC_Tool_Info").NewRow

        DR("Tool_Name") = Tool
        DR("Department") = Department

        DS.Tables("T_SPC_Tool_Info").Rows.Add(DR)
        DA.Update(DS, "T_SPC_Tool_Info")
        Connection.Close()


    End Sub

    Sub Enter_Parameter(tool As Int16, seq As Int16)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = Session("SATI_SPC_DB")
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT [Key], Tool_Key, Seq_Flow FROM T_SPC_Parameters WHERE (Tool_Key = 0)"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [T_SPC_Parameters] ([Tool_Key], [Seq_Flow]) VALUES (@Tool_Key, @Seq_Flow); SELECT [Key], Tool_Key, Seq_Flow FROM T_SPC_Parameters WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Tool_Key", System.Data.SqlDbType.Int, 0, "Tool_Key"), New System.Data.SqlClient.SqlParameter("@Seq_Flow", System.Data.SqlDbType.Int, 0, "Seq_Flow")})
        End With
        DA.InsertCommand = InsertCmd

        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [T_SPC_Parameters] SET [Tool_Key] = @Tool_Key, [Seq_Flow] = @Seq_Flow WHERE (([Key] = @Original_Key) AND ([Tool_Key] = @Original_Tool_Key) AND ([Seq_Flow] = @Original_Seq_Flow)); SELECT [Key], Tool_Key, Seq_Flow FROM T_SPC_Parameters WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Tool_Key", System.Data.SqlDbType.Int, 0, "Tool_Key"), New System.Data.SqlClient.SqlParameter("@Seq_Flow", System.Data.SqlDbType.Int, 0, "Seq_Flow"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Tool_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Tool_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Seq_Flow", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Seq_Flow", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_SPC_Parameters", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("Tool_Key", "Tool_Key"), New System.Data.Common.DataColumnMapping("Seq_Flow", "Seq_Flow")})})
        DA.Fill(DS)


        DR = DS.Tables("T_SPC_Parameters").NewRow

        DR("Tool_Key") = tool
        DR("Seq_Flow") = seq

        DS.Tables("T_SPC_Parameters").Rows.Add(DR)
        DA.Update(DS, "T_SPC_Parameters")
        Connection.Close()


    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

        Dim row As String
        row = e.CommandArgument.ToString
        If e.CommandName = "Select" Then
            Me.LabelToolNumber.Text = Me.GridView1.Rows(row).Cells(1).Text
            Me.LabelParameterTool.Text = Me.GridView1.Rows(row).Cells(3).Text
            Me.PanelParameters.Visible = True
            Me.PanelLimits.Visible = False
            Me.GridViewParameters.SelectedIndex = -1
        End If

    End Sub

    Protected Sub ButtonAddParameter_Click(sender As Object, e As EventArgs) Handles ButtonAddParameter.Click
        Dim Tool As Int16
        Dim Seq As Int16 = 0
        Dim Last As String
        Dim My_Row As Data.DataRow
        Tool = Me.LabelToolNumber.Text
        'SELECT MAX(Seq_Flow) AS Last FROM dbo.T_SPC_Parameters WHERE (Tool_Key = 0)
        Dim My_DS As New Data.DataSet

        My_DS = SatiCode.GetMyDataSetSPCData("SELECT MAX(Seq_Flow) AS Last FROM dbo.T_SPC_Parameters WHERE (Tool_Key = " & Tool & ")")
        My_Row = My_DS.Tables(0).Rows(0)

        If Not IsDBNull(My_Row("Last")) Then
            Last = My_Row("Last")
            Seq = Last + 1
        Else
            Seq = 1
        End If

        Enter_Parameter(Tool, Seq)

        Me.GridViewParameters.DataBind()

    End Sub

    Protected Sub GridViewParameters_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewParameters.RowCommand

        Dim row As String
        row = e.CommandArgument.ToString
        If e.CommandName = "Select" Then
            Me.PanelLimits.Visible = True
            Me.LabelLimitName.Text = Me.GridViewParameters.Rows(row).Cells(4).Text
            Me.LabelParameterNumber.Text = Me.GridViewParameters.Rows(row).Cells(1).Text
        End If

    End Sub

    Protected Sub ButtonAddLimit_Click(sender As Object, e As EventArgs) Handles ButtonAddLimit.Click
        Enter_Limit(Me.LabelParameterNumber.Text)
        Me.GridViewLimits.DataBind()

    End Sub


    Sub Enter_Limit(Parameter As Int16)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = Session("SATI_SPC_DB")
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT [Key], Parameter_Key FROM T_SPC_Limits WHERE ([Key] = 0)"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [T_SPC_Limits] ([Parameter_Key]) VALUES (@Parameter_Key); SELECT [Key], Parameter_Key FROM T_SPC_Limits WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Parameter_Key", System.Data.SqlDbType.Int, 0, "Parameter_Key")})
        End With
        DA.InsertCommand = InsertCmd

        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [T_SPC_Limits] SET [Parameter_Key] = @Parameter_Key WHERE (([Key] = @Original_Key) AND ([Parameter_Key] = @Original_Parameter_Key)); SELECT [Key], Parameter_Key FROM T_SPC_Limits WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Parameter_Key", System.Data.SqlDbType.Int, 0, "Parameter_Key"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Parameter_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Parameter_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_SPC_Limits", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("Parameter_Key", "Parameter_Key")})})
        DA.Fill(DS)

        DR = DS.Tables("T_SPC_Limits").NewRow

        DR("Parameter_Key") = Parameter

        DS.Tables("T_SPC_Limits").Rows.Add(DR)
        DA.Update(DS, "T_SPC_Limits")
        Connection.Close()

    End Sub

    Private Sub SPC_SPC_Managment_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("Eng", Server)
    End Sub
End Class
