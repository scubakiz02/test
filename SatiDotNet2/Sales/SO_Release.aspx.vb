
Partial Class Sales_SO_Release
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim row As String = e.CommandArgument.ToString

        If e.CommandName = "Release" Then
            CheckAndExpireMainIDSO(Me.GridView1.Rows(row).Cells(1).Text, Me.GridView1.Rows(row).Cells(2).Text)
            ActivateSO(Me.GridView1.Rows(row).Cells(1).Text, Me.GridView1.Rows(row).Cells(2).Text, Me.GridView1.Rows(row).Cells(3).Text, Me.GridView1.Rows(row).Cells(4).Text)


        End If
    End Sub


    Sub CheckAndExpireMainIDSO(MainID As String, ReplaceSO As String)

        'SELECT dbo.SO_LineItems.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.ExpirationDtd FROM dbo.SO_Info INNER JOIN dbo.SO_LineItems ON dbo.SO_Info.SO = dbo.SO_LineItems.SO WHERE (dbo.SO_LineItems.MainID = N'3143') AND (dbo.SO_Info.ExpirationDtd > { fn NOW() } OR dbo.SO_Info.ExpirationDtd IS NULL)
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        DS = Saticode.GetMyDataSet("SELECT dbo.SO_LineItems.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.ExpirationDtd FROM dbo.SO_Info INNER JOIN dbo.SO_LineItems ON dbo.SO_Info.SO = dbo.SO_LineItems.SO WHERE (dbo.SO_LineItems.MainID = N'" & MainID & "') AND (dbo.SO_Info.ExpirationDtd > { fn NOW() } OR dbo.SO_Info.ExpirationDtd IS NULL)")

        If Not DS.Tables(0).Rows.Count = 0 Then
            DR = DS.Tables(0).Rows(0)
            Saticode.ModSO("Expire", "", DR("MainID").ToString, DR("SO").ToString, "", "", "", "", DateTime.Now.ToShortDateString)

        End If

    End Sub


    Sub ActivateSO(MainID As String, SO As String, PO As String, Qty As String)

        Saticode.ModSO("Add", "", MainID, SO, PO, Qty, "", DateTime.Now.ToShortDateString, "")
        Remove(MainID, SO)
        GridView1.DataBind()

    End Sub


    Sub Remove(TheMainID As String, SO As String)

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
            .CommandText = "SELECT [Key], MainID, SO FROM T_SO_Future_List WHERE (MainID = N'" & TheMainID & "') AND (SO = N'" & SO & "')"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        '*****************************************************************
        '************************Insert***********************************
        '*****************************************************************
        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [T_SO_Future_List] ([MainID], [SO]) VALUES (@MainID, @SO); SELECT [Key], MainID, SO FROM T_SO_Future_List WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@SO", System.Data.SqlDbType.NVarChar, 0, "SO")})
        End With
        DA.InsertCommand = InsertCmd

        '*****************************************************************
        '************************Update***********************************
        '*****************************************************************
        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [T_SO_Future_List] SET [MainID] = @MainID, [SO] = @SO WHERE (([Key] = @Original_Key) AND ((@IsNull_MainID = 1 AND [MainID] IS NULL) OR ([MainID] = @Original_MainID)) AND ((@IsNull_SO = 1 AND [SO] IS NULL) OR ([SO] = @Original_SO))); SELECT [Key], MainID, SO FROM T_SO_Future_List WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@SO", System.Data.SqlDbType.NVarChar, 0, "SO"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_MainID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "MainID", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_MainID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MainID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_SO", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "SO", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_SO", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SO", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        '*****************************************************************
        '************************Delete***********************************
        '*****************************************************************
        Dim DeleteCmd As New System.Data.SqlClient.SqlCommand
        With DeleteCmd
            .CommandText = "DELETE FROM [T_SO_Future_List] WHERE (([Key] = @Original_Key) AND ((@IsNull_MainID = 1 AND [MainID] IS NULL) OR ([MainID] = @Original_MainID)) AND ((@IsNull_SO = 1 AND [SO] IS NULL) OR ([SO] = @Original_SO)))"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_MainID", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "MainID", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_MainID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MainID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_SO", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "SO", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_SO", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SO", System.Data.DataRowVersion.Original, Nothing)})
        End With
        DA.DeleteCommand = DeleteCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_SO_Future_List", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("MainID", "MainID"), New System.Data.Common.DataColumnMapping("SO", "SO")})})
        DA.Fill(DS)


        Dim i As Int16
                i = DS.Tables(0).Rows.Count - 1
                For Row As Int16 = 0 To i
                    DR = DS.Tables(0).Rows(Row)
                    DR.Delete()
                Next
        DA.Update(DS, "T_SO_Future_List")

        Connection.Close()



    End Sub

    Private Sub Sales_SO_Release_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("Office", Server)
    End Sub
End Class
