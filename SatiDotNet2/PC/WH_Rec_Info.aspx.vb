


Partial Class PC_WH_Rec_Info
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not Request.QueryString("Sati_Key") = "" Then
                Me.LabelSatiKey.Text = Request.QueryString("Sati_Key")
            Else
                Me.LabelSatiKey.Text = "0"
            End If

            If Not Request.QueryString("Old_Key") = "" Then
                Me.LabelOldKey.Text = Request.QueryString("Old_Key")
            Else
                Me.LabelOldKey.Text = "0"
            End If
            Try
                LoadRecords()
            Catch ex As Exception

            End Try

        End If


    End Sub

    Sub LoadRecords()
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        DS = SatiCode.GetMyDataSet("SELECT Reveiving_Key, MainID, Waferlog, Action, Qty, QtySpecified, PackingSlip, Carrier, Complete, Note, StorageUnit, StorageUnitCount, Operator, EventTime FROM T_WH_Invintory WHERE (Reveiving_Key = " & Me.LabelSatiKey.Text & ")")
        DR = DS.Tables(0).Rows(0)

        TextBoxID.Text = DR("MainID")
        TextBoxWL.Text = DR("Waferlog")
        TextBoxQty.Text = DR("Qty")
        TextBoxPackingSlip.Text = DR("PackingSlip")
        TextBoxCarrier.Text = DR("Carrier")
        TextBoxNote.Text = DR("Note")

    End Sub


    Protected Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        SaveRecords_Sati()
        SaveRecords_Old()
    End Sub

    Sub SaveRecords_Sati()
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand
        With SelectCmd
            .CommandText = "SELECT Reveiving_Key, MainID, Waferlog, Qty, PackingSlip, Carrier, Note FROM T_WH_Invintory WHERE (Reveiving_Key = " & Me.LabelSatiKey.Text & ")"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [T_WH_Invintory] SET [MainID] = @MainID, [Waferlog] = @Waferlog, [Qty] = @Qty, [PackingSlip] = @PackingSlip, [Carrier] = @Carrier, [Note] = @Note WHERE (([Reveiving_Key] = @Original_Reveiving_Key) AND ([MainID] = @Original_MainID) AND ([Waferlog] = @Original_Waferlog) AND ([Qty] = @Original_Qty) AND ((@IsNull_PackingSlip = 1 AND [PackingSlip] IS NULL) OR ([PackingSlip] = @Original_PackingSlip)) AND ((@IsNull_Carrier = 1 AND [Carrier] IS NULL) OR ([Carrier] = @Original_Carrier)) AND ((@IsNull_Note = 1 AND [Note] IS NULL) OR ([Note] = @Original_Note)));SELECT Reveiving_Key, MainID, Waferlog, Qty, PackingSlip, Carrier, Note FROM T_WH_Invintory WHERE (Reveiving_Key = @Reveiving_Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@Waferlog", System.Data.SqlDbType.NVarChar, 0, "Waferlog"), New System.Data.SqlClient.SqlParameter("@Qty", System.Data.SqlDbType.Int, 0, "Qty"), New System.Data.SqlClient.SqlParameter("@PackingSlip", System.Data.SqlDbType.NVarChar, 0, "PackingSlip"), New System.Data.SqlClient.SqlParameter("@Carrier", System.Data.SqlDbType.NVarChar, 0, "Carrier"), New System.Data.SqlClient.SqlParameter("@Note", System.Data.SqlDbType.NVarChar, 0, "Note"), New System.Data.SqlClient.SqlParameter("@Original_Reveiving_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Reveiving_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_MainID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MainID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Waferlog", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Waferlog", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Qty", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Qty", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_PackingSlip", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "PackingSlip", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_PackingSlip", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PackingSlip", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Carrier", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Carrier", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Carrier", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Carrier", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Note", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Note", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Note", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Note", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Reveiving_Key", System.Data.SqlDbType.Int, 4, "Reveiving_Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_WH_Invintory", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Reveiving_Key", "Reveiving_Key"), New System.Data.Common.DataColumnMapping("MainID", "MainID"), New System.Data.Common.DataColumnMapping("Waferlog", "Waferlog"), New System.Data.Common.DataColumnMapping("Qty", "Qty"), New System.Data.Common.DataColumnMapping("PackingSlip", "PackingSlip"), New System.Data.Common.DataColumnMapping("Carrier", "Carrier"), New System.Data.Common.DataColumnMapping("Note", "Note")})})
        DA.Fill(DS)


        DR = DS.Tables(0).Rows(0)
        DR.AcceptChanges()
        DR.BeginEdit()
        DR("MainID") = TextBoxID.Text
        DR("Waferlog") = TextBoxWL.Text
        DR("Qty") = TextBoxQty.Text
        DR("PackingSlip") = TextBoxPackingSlip.Text
        DR("Carrier") = TextBoxCarrier.Text
        DR("Note") = TextBoxNote.Text
        DR.EndEdit()
        DA.Update(DS, "T_WH_Invintory")

    End Sub

    Sub SaveRecords_Old()
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand
        With SelectCmd
            .CommandText = "SELECT Reveiving_Key, MainID, Waferlog, Qty, PackingSlip, Carrier, Note FROM ReceivingLog WHERE (Reveiving_Key = " & Me.LabelOldKey.Text & ")"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [ReceivingLog] SET [MainID] = @MainID, [Waferlog] = @Waferlog, [Qty] = @Qty, [PackingSlip] = @PackingSlip, [Carrier] = @Carrier, [Note] = @Note WHERE (([Reveiving_Key] = @Original_Reveiving_Key) AND ([MainID] = @Original_MainID) AND ([Waferlog] = @Original_Waferlog) AND ([Qty] = @Original_Qty) AND ((@IsNull_PackingSlip = 1 AND [PackingSlip] IS NULL) OR ([PackingSlip] = @Original_PackingSlip)) AND ((@IsNull_Carrier = 1 AND [Carrier] IS NULL) OR ([Carrier] = @Original_Carrier)) AND ((@IsNull_Note = 1 AND [Note] IS NULL) OR ([Note] = @Original_Note))); SELECT Reveiving_Key, MainID, Waferlog, Qty, PackingSlip, Carrier, Note FROM ReceivingLog WHERE (Reveiving_Key = @Reveiving_Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@Waferlog", System.Data.SqlDbType.NVarChar, 0, "Waferlog"), New System.Data.SqlClient.SqlParameter("@Qty", System.Data.SqlDbType.Int, 0, "Qty"), New System.Data.SqlClient.SqlParameter("@PackingSlip", System.Data.SqlDbType.NVarChar, 0, "PackingSlip"), New System.Data.SqlClient.SqlParameter("@Carrier", System.Data.SqlDbType.NVarChar, 0, "Carrier"), New System.Data.SqlClient.SqlParameter("@Note", System.Data.SqlDbType.NVarChar, 0, "Note"), New System.Data.SqlClient.SqlParameter("@Original_Reveiving_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Reveiving_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_MainID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MainID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Waferlog", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Waferlog", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Qty", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Qty", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_PackingSlip", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "PackingSlip", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_PackingSlip", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PackingSlip", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Carrier", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Carrier", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Carrier", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Carrier", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Note", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Note", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Note", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Note", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Reveiving_Key", System.Data.SqlDbType.Int, 4, "Reveiving_Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "ReceivingLog", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Reveiving_Key", "Reveiving_Key"), New System.Data.Common.DataColumnMapping("MainID", "MainID"), New System.Data.Common.DataColumnMapping("Waferlog", "Waferlog"), New System.Data.Common.DataColumnMapping("Qty", "Qty"), New System.Data.Common.DataColumnMapping("PackingSlip", "PackingSlip"), New System.Data.Common.DataColumnMapping("Carrier", "Carrier"), New System.Data.Common.DataColumnMapping("Note", "Note")})})
        DA.Fill(DS)


        DR = DS.Tables(0).Rows(0)
        DR.AcceptChanges()
        DR.BeginEdit()
        DR("MainID") = TextBoxID.Text
        DR("Waferlog") = TextBoxWL.Text
        DR("Qty") = TextBoxQty.Text
        DR("PackingSlip") = TextBoxPackingSlip.Text
        DR("Carrier") = TextBoxCarrier.Text
        DR("Note") = TextBoxNote.Text
        DR.EndEdit()
        DA.Update(DS, "ReceivingLog")

    End Sub

End Class
