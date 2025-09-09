
Partial Class Sales_EnterFuturePO
    Inherits System.Web.UI.Page


    Private Sub Sales_EnterFuturePO_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("Sales", Server)
    End Sub

    Protected Sub ButtonAddSO_Click(sender As Object, e As EventArgs) Handles ButtonAddSO.Click
        Me.PanelEnter.Visible = True

    End Sub
    Protected Sub ButtonEnter_Click(sender As Object, e As EventArgs) Handles ButtonEnter.Click
        EnterNewSO()
        Me.GridView1.DataBind()

        Me.PanelEnter.Visible = False
    End Sub



    Sub EnterNewSO()
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()
        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SOInfoSelectCmd As New System.Data.SqlClient.SqlCommand
        With SOInfoSelectCmd
            .CommandText = "SELECT [Key], MainID, SO, PO, Qty, Note, DateStamp FROM T_SO_Future_List WHERE ([Key] = 0)"
            .Connection = Connection
        End With
        DA.SelectCommand = SOInfoSelectCmd

        Dim SOInfoInsertCmd As New System.Data.SqlClient.SqlCommand
        With SOInfoInsertCmd
            .CommandText = "INSERT INTO [T_SO_Future_List] ([MainID], [SO], [PO], [Qty], [Note], [DateStamp]) VALUES (@MainID, @SO, @PO, @Qty, @Note, @DateStamp); SELECT [Key], MainID, SO, PO, Qty, Note, DateStamp FROM T_SO_Future_List WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@SO", System.Data.SqlDbType.NVarChar, 0, "SO"), New System.Data.SqlClient.SqlParameter("@PO", System.Data.SqlDbType.NVarChar, 0, "PO"), New System.Data.SqlClient.SqlParameter("@Qty", System.Data.SqlDbType.Int, 0, "Qty"), New System.Data.SqlClient.SqlParameter("@Note", System.Data.SqlDbType.NVarChar, 0, "Note"), New System.Data.SqlClient.SqlParameter("@DateStamp", System.Data.SqlDbType.SmallDateTime, 0, "DateStamp")})
        End With
        DA.InsertCommand = SOInfoInsertCmd

        Dim SOInfoUpdateCmd As New System.Data.SqlClient.SqlCommand
        With SOInfoUpdateCmd
            .CommandText = "UPDATE [T_SO_Future_List] SET [MainID] = @MainID, [SO] = @SO, [PO] = @PO, [Qty] = @Qty, [Note] = @Note, [DateStamp] = @DateStamp WHERE (([Key] = @Original_Key) AND ((@IsNull_MainID = 1 AND [MainID] IS NULL) OR ([MainID] = @Original_MainID)) AND ((@IsNull_SO = 1 AND [SO] IS NULL) OR ([SO] = @Original_SO)) AND ((@IsNull_PO = 1 AND [PO] IS NULL) OR ([PO] = @Original_PO)) AND ((@IsNull_Qty = 1 AND [Qty] IS NULL) OR ([Qty] = @Original_Qty)) AND ((@IsNull_DateStamp = 1 AND [DateStamp] IS NULL) OR ([DateStamp] = @Original_DateStamp))); SELECT [Key], MainID, SO, PO, Qty, Note, DateStamp FROM T_SO_Future_List WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@SO", System.Data.SqlDbType.NVarChar, 0, "SO"), New System.Data.SqlClient.SqlParameter("@PO", System.Data.SqlDbType.NVarChar, 0, "PO"), New System.Data.SqlClient.SqlParameter("@Qty", System.Data.SqlDbType.Int, 0, "Qty"), New System.Data.SqlClient.SqlParameter("@Note", System.Data.SqlDbType.NVarChar, 0, "Note"), New System.Data.SqlClient.SqlParameter("@DateStamp", System.Data.SqlDbType.SmallDateTime, 0, "DateStamp"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_MainID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "MainID", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_MainID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MainID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_SO", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "SO", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_SO", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SO", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_PO", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "PO", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_PO", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PO", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Qty", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Qty", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Qty", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Qty", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_DateStamp", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "DateStamp", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_DateStamp", System.Data.SqlDbType.SmallDateTime, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateStamp", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = SOInfoUpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_SO_Future_List", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("MainID", "MainID"), New System.Data.Common.DataColumnMapping("SO", "SO"), New System.Data.Common.DataColumnMapping("PO", "PO"), New System.Data.Common.DataColumnMapping("Qty", "Qty"), New System.Data.Common.DataColumnMapping("Note", "Note"), New System.Data.Common.DataColumnMapping("DateStamp", "DateStamp")})})
        DA.Fill(DS)

        DR = DS.Tables("T_SO_Future_List").NewRow
        DR("MainID") = Me.TextBoxID.Text
        DR("SO") = Me.TextBoxSO.Text
        DR("PO") = Me.TextBoxPO.Text
        DR("Qty") = Me.TextBoxQty.Text
        DR("Note") = Me.TextBoxNote.Text
        DR("DateStamp") = DateTime.Now.ToShortDateString

        DS.Tables("T_SO_Future_List").Rows.Add(DR)
        DA.Update(DS, "T_SO_Future_List")

        Connection.Close()

    End Sub


End Class
