
Partial Class DBMaintenance_MR_ToolSub_Group
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Panel2.Visible = True
        Me.SqlDataSourceTools.SelectCommand = "SELECT [Key], Tool FROM T_Tools ORDER BY Tool"
        Me.DropDownListTools.DataBind()
        Me.LabelInfo.Text = ""
        Me.TextBox_SGN.Text = ""
        Me.TextBox_TAG.Text = ""

    End Sub

    Protected Sub Button_Close_Click(sender As Object, e As EventArgs) Handles Button_Close.Click
        Me.Panel2.Visible = False
    End Sub

    Protected Sub Button_add_Click(sender As Object, e As EventArgs) Handles Button_add.Click
        Dim ToolID As Int16
        Dim SGN As String
        Dim Tag As String
        If Not Me.TextBox_SGN.Text = "" And Not Me.TextBox_TAG.Text = "" Then
            Me.LabelInfo.Text = ""
            ToolID = Me.DropDownListTools.SelectedValue
            SGN = Me.TextBox_SGN.Text
            Tag = Me.TextBox_TAG.Text
            Try
                Add_Record(ToolID, SGN, Tag, 0)
            Catch ex As Exception
                Me.LabelInfo.Text = "Error Adding Record!"
            End Try
        Else
            Me.LabelInfo.Text = "Please Enter Information in Both Feilds"
        End If
    End Sub

    Sub Add_Record(ToolID As Int16, SGN As String, Tag As String, Delete_Record_Number As Integer)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT [Key], Tool_Key, SG_Name, SB_Tag FROM T_Tool_SubGroup_Tag_Names WHERE ([Key] = " & Delete_Record_Number & ")"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [T_Tool_SubGroup_Tag_Names] ([Tool_Key], [SG_Name], [SB_Tag]) VALUES (@Tool_Key, @SG_Name, @SB_Tag); Select [Key], Tool_Key, SG_Name, SB_Tag FROM T_Tool_SubGroup_Tag_Names WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Tool_Key", System.Data.SqlDbType.Int, 0, "Tool_Key"), New System.Data.SqlClient.SqlParameter("@SG_Name", System.Data.SqlDbType.NVarChar, 0, "SG_Name"), New System.Data.SqlClient.SqlParameter("@SB_Tag", System.Data.SqlDbType.NVarChar, 0, "SB_Tag")})
        End With
        DA.InsertCommand = InsertCmd

        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [T_Tool_SubGroup_Tag_Names] Set [Tool_Key] = @Tool_Key, [SG_Name] = @SG_Name, [SB_Tag] = @SB_Tag WHERE (([Key] = @Original_Key) And ([Tool_Key] = @Original_Tool_Key) And ([SG_Name] = @Original_SG_Name) And ([SB_Tag] = @Original_SB_Tag)); Select [Key], Tool_Key, SG_Name, SB_Tag FROM T_Tool_SubGroup_Tag_Names WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Tool_Key", System.Data.SqlDbType.Int, 0, "Tool_Key"), New System.Data.SqlClient.SqlParameter("@SG_Name", System.Data.SqlDbType.NVarChar, 0, "SG_Name"), New System.Data.SqlClient.SqlParameter("@SB_Tag", System.Data.SqlDbType.NVarChar, 0, "SB_Tag"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Tool_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Tool_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SG_Name", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SG_Name", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SB_Tag", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SB_Tag", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        Dim DeleteCmd As New System.Data.SqlClient.SqlCommand
        With DeleteCmd
            .CommandText = "DELETE FROM [T_Tool_SubGroup_Tag_Names] WHERE (([Key] = @Original_Key) And ([Tool_Key] = @Original_Tool_Key) And ([SG_Name] = @Original_SG_Name) And ([SB_Tag] = @Original_SB_Tag))"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Tool_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Tool_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SG_Name", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SG_Name", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SB_Tag", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SB_Tag", System.Data.DataRowVersion.Original, Nothing)})
        End With
        DA.DeleteCommand = DeleteCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_Tool_SubGroup_Tag_Names", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("Tool_Key", "Tool_Key"), New System.Data.Common.DataColumnMapping("SG_Name", "SG_Name"), New System.Data.Common.DataColumnMapping("SB_Tag", "SB_Tag")})})
        DA.Fill(DS)

        If Delete_Record_Number = 0 Then
            DR = DS.Tables("T_Tool_SubGroup_Tag_Names").NewRow
            DR("Tool_Key") = ToolID
            DR("SG_Name") = SGN
            DR("SB_Tag") = Tag
            DS.Tables("T_Tool_SubGroup_Tag_Names").Rows.Add(DR)
        Else
            DR = DS.Tables(0).Rows(0)
            DR.Delete()
        End If
        DA.Update(DS, "T_Tool_SubGroup_Tag_Names")
        Connection.Close()

        Me.GridView1.DataBind()
        Me.LabelInfo.Text = "Record Added!"
    End Sub


End Class
