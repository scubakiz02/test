
Partial Class PC_ScanPalletHolding
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub

    Protected Sub TextBoxIn_TextChanged(sender As Object, e As EventArgs) Handles TextboxIn.TextChanged
        CheckDataIn()
        Me.HyperLink9.Visible = False
    End Sub

    Sub CheckDataIn()
        If Not Me.TextboxIn.Text = "" Then
            TextboxIn.Text = UCase(TextboxIn.Text)

            For i As Integer = 0 To ListBoxCB.Items.Count - 1
                If TextboxIn.Text = ListBoxCB.Items(i).Text Then
                    Exit Sub
                End If
            Next

            If Mid(TextboxIn.Text, 1, 2) = "PH" Then
                Me.ListBoxCB.Items.Add(Me.TextboxIn.Text)
                Me.Labeladded.Text = Labeladded.Text + 1
            End If

            Me.TextboxIn.Text = ""
            CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.TextboxIn.ClientID)
            Me.TextboxIn.Focus()
        End If
    End Sub


    Protected Sub ButtonSubmit_Click(sender As Object, e As EventArgs) Handles ButtonSubmit.Click
        Dim TheUser As String = User.Identity.Name.ToString
        Dim MyScanDate As DateTime = DateAndTime.Now
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
            .CommandText = "SELECT [Key], ScanKey, PH_Key, SatiUser FROM T_PH_DayScans WHERE ([Key] = 0)"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd
        '*****************************************************************
        '************************Insert***********************************
        '*****************************************************************
        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [T_PH_DayScans] ([ScanKey], [PH_Key], [SatiUser]) VALUES (@ScanKey, @PH_Key, @SatiUser); SELECT [Key], ScanKey, PH_Key, SatiUser FROM T_PH_DayScans WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@ScanKey", System.Data.SqlDbType.SmallDateTime, 0, "ScanKey"), New System.Data.SqlClient.SqlParameter("@PH_Key", System.Data.SqlDbType.NChar, 0, "PH_Key"), New System.Data.SqlClient.SqlParameter("@SatiUser", System.Data.SqlDbType.NVarChar, 0, "SatiUser")})
        End With
        DA.InsertCommand = InsertCmd
        '*****************************************************************
        '************************Update***********************************
        '*****************************************************************
        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [T_PH_DayScans] SET [ScanKey] = @ScanKey, [PH_Key] = @PH_Key, [SatiUser] = @SatiUser WHERE (([Key] = @Original_Key) AND ([ScanKey] = @Original_ScanKey) AND ([PH_Key] = @Original_PH_Key) AND ([SatiUser] = @Original_SatiUser)); SELECT [Key], ScanKey, PH_Key, SatiUser FROM T_PH_DayScans WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@ScanKey", System.Data.SqlDbType.SmallDateTime, 0, "ScanKey"), New System.Data.SqlClient.SqlParameter("@PH_Key", System.Data.SqlDbType.NChar, 0, "PH_Key"), New System.Data.SqlClient.SqlParameter("@SatiUser", System.Data.SqlDbType.NVarChar, 0, "SatiUser"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ScanKey", System.Data.SqlDbType.SmallDateTime, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ScanKey", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_PH_Key", System.Data.SqlDbType.NChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PH_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SatiUser", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SatiUser", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_PH_DayScans", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("ScanKey", "ScanKey"), New System.Data.Common.DataColumnMapping("PH_Key", "PH_Key"), New System.Data.Common.DataColumnMapping("SatiUser", "SatiUser")})})
        DA.Fill(DS)


        'loop for each PH scan
        For i = 0 To Me.ListBoxCB.Items.Count - 1

            DR = DS.Tables("T_PH_DayScans").NewRow

            DR("PH_Key") = Mid(ListBoxCB.Items(i).Text, 3) ' CB,
            DR("ScanKey") = MyScanDate
            DR("SatiUser") = TheUser

            DS.Tables("T_PH_DayScans").Rows.Add(DR)
            DA.Update(DS, "T_PH_DayScans")

        Next

        Connection.Close()
        Me.ListBoxCB.Items.Clear()
    End Sub
End Class
