
Partial Class PC_NonConformingPacking
    Inherits System.Web.UI.Page
    Dim saticode As New Class1

    Protected Sub TextBoxID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxID.TextChanged
        Get_ID_Info(Me.TextBoxID.Text)
    End Sub

    Sub Get_ID_Info(ByVal SelectedID As String)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()
        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim SelectCmd As New System.Data.SqlClient.SqlCommand
        With SelectCmd
            .CommandText = "SELECT [Key], ID, PackWithID, PackingNote, Sell, PWI_Percent, Solar, TimeStamp FROM dbo.T_NC_ID_Info WHERE (ID = N'" & SelectedID & "')"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd
        DA.Fill(DS)


        If DS.Tables(0).Rows.Count = 0 Then
            Me.LabelPackWith.Text = "0000"
            Me.TextBoxPackingNote.Text = "Id not setup in system!"
            Exit Sub
        End If

        DR = DS.Tables(0).Rows(0)

        Me.LabelPackWith.Text = DR("PackWithID")

        If Not DR("ID") = DR("PackWithID") Then

            With SelectCmd
                .CommandText = "SELECT [Key], ID, PackWithID, PackingNote, Sell, PWI_Percent, Solar, TimeStamp FROM dbo.T_NC_ID_Info WHERE (ID = N'" & DR("PackWithID") & "')"
                .Connection = Connection
            End With
            DA.SelectCommand = SelectCmd
            DS.Clear()
            DA.Fill(DS)
            DR = DS.Tables(0).Rows(0)
        End If



        Me.TextBoxPackingNote.Text = DR("PackingNote").ToString
        Connection.Close()
        GetInvList(Me.LabelPackWith.Text)
    End Sub

    Sub GetInvList(ByVal SelectedID As String)

        Me.SqlDataSourceOpenInv.SelectCommand = "SELECT dbo.T_NC_Box.NC_Inv_Box, dbo.T_NC_Box.MainID, dbo.T_NC_Box.Type, SUM(ISNULL(dbo.T_NC_Box_Qty.Qty, 0)) AS Total FROM dbo.T_NC_Box LEFT OUTER JOIN dbo.T_NC_Box_Qty ON dbo.T_NC_Box.NC_Inv_Box = dbo.T_NC_Box_Qty.NC_Inv_Box WHERE (dbo.T_NC_Box.[Open] = 1) GROUP BY dbo.T_NC_Box.MainID, dbo.T_NC_Box.Type, dbo.T_NC_Box.NC_Inv_Box HAVING (dbo.T_NC_Box.MainID = N'" & SelectedID & "') ORDER BY dbo.T_NC_Box.Type"
        Me.GridViewBoxes.DataBind()

    End Sub

    Protected Sub ButtonNewBox_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonNewBox.Click
        Dim NewInvNumber As String

        NewInvNumber = NC_Box("New", Me.DropDownListType.SelectedItem.Text, Me.LabelPackWith.Text, 0, 0, "")


        GetInvList(Me.LabelPackWith.Text)
    End Sub

    Sub NC_Box_Add(ByVal InvNumber As Integer, ByVal Qty As Integer, ByVal User As String)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand
        With SelectCmd
            .CommandText = "SELECT [Key], NC_Inv_Box, Qty, [User] FROM dbo.T_NC_Box_Qty WHERE ([Key] = 0)"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [dbo].[T_NC_Box_Qty] ([NC_Inv_Box], [Qty], [User]) VALUES (@NC_Inv_Box, @Qty, @User); SELECT [Key], NC_Inv_Box, Qty, [User] FROM dbo.T_NC_Box_Qty WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@NC_Inv_Box", System.Data.SqlDbType.Int, 0, "NC_Inv_Box"), New System.Data.SqlClient.SqlParameter("@Qty", System.Data.SqlDbType.Int, 0, "Qty"), New System.Data.SqlClient.SqlParameter("@User", System.Data.SqlDbType.NVarChar, 0, "User")})
        End With
        DA.InsertCommand = InsertCmd

        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [dbo].[T_NC_Box_Qty] SET [NC_Inv_Box] = @NC_Inv_Box, [Qty] = @Qty, [User] = @User WHERE (([Key] = @Original_Key) AND ([NC_Inv_Box] = @Original_NC_Inv_Box) AND ([Qty] = @Original_Qty) AND ([User] = @Original_User)); SELECT [Key], NC_Inv_Box, Qty, [User] FROM dbo.T_NC_Box_Qty WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@NC_Inv_Box", System.Data.SqlDbType.Int, 0, "NC_Inv_Box"), New System.Data.SqlClient.SqlParameter("@Qty", System.Data.SqlDbType.Int, 0, "Qty"), New System.Data.SqlClient.SqlParameter("@User", System.Data.SqlDbType.NVarChar, 0, "User"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_NC_Inv_Box", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "NC_Inv_Box", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Qty", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Qty", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_User", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "User", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_NC_Box_Qty", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("NC_Inv_Box", "NC_Inv_Box"), New System.Data.Common.DataColumnMapping("Qty", "Qty"), New System.Data.Common.DataColumnMapping("User", "User")})})
        DA.Fill(DS)

        DR = DS.Tables("T_NC_Box_Qty").NewRow
        DR("NC_Inv_Box") = InvNumber
        DR("Qty") = Qty
        DR("User") = User

        DS.Tables("T_NC_Box_Qty").Rows.Add(DR)
        DA.Update(DS, "T_NC_Box_Qty")


        Connection.Close()
    End Sub

    Function NC_Box(ByVal What As String, ByVal Type As String, ByVal SelectedID As String, ByVal InvNumber As Integer, ByVal SpecialKey As Integer, ByVal CloseQty As String) As String
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand
        With SelectCmd
            .CommandText = "SELECT NC_Inv_Box, MainID, Type, RequestSpecialKey, Note, [Open] FROM dbo.T_NC_Box WHERE (NC_Inv_Box = " & InvNumber & ")"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [dbo].[T_NC_Box] ([MainID], [Type], [RequestSpecialKey], [Note], [Open]) VALUES (@MainID, @Type, @RequestSpecialKey, @Note, @Open); SELECT NC_Inv_Box, MainID, Type, RequestSpecialKey, Note, [Open] FROM dbo.T_NC_Box WHERE (NC_Inv_Box = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@Type", System.Data.SqlDbType.NVarChar, 0, "Type"), New System.Data.SqlClient.SqlParameter("@RequestSpecialKey", System.Data.SqlDbType.Int, 0, "RequestSpecialKey"), New System.Data.SqlClient.SqlParameter("@Note", System.Data.SqlDbType.NVarChar, 0, "Note"), New System.Data.SqlClient.SqlParameter("@Open", System.Data.SqlDbType.Bit, 0, "Open")})
        End With
        DA.InsertCommand = InsertCmd

        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [dbo].[T_NC_Box] SET [MainID] = @MainID, [Type] = @Type, [RequestSpecialKey] = @RequestSpecialKey, [Note] = @Note, [Open] = @Open WHERE (([NC_Inv_Box] = @Original_NC_Inv_Box) AND ([MainID] = @Original_MainID) AND ([Type] = @Original_Type) AND ([RequestSpecialKey] = @Original_RequestSpecialKey) AND ((@IsNull_Note = 1 AND [Note] IS NULL) OR ([Note] = @Original_Note)) AND ([Open] = @Original_Open)); SELECT NC_Inv_Box, MainID, Type, RequestSpecialKey, Note, [Open] FROM dbo.T_NC_Box WHERE (NC_Inv_Box = @NC_Inv_Box)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MainID", System.Data.SqlDbType.NVarChar, 0, "MainID"), New System.Data.SqlClient.SqlParameter("@Type", System.Data.SqlDbType.NVarChar, 0, "Type"), New System.Data.SqlClient.SqlParameter("@RequestSpecialKey", System.Data.SqlDbType.Int, 0, "RequestSpecialKey"), New System.Data.SqlClient.SqlParameter("@Note", System.Data.SqlDbType.NVarChar, 0, "Note"), New System.Data.SqlClient.SqlParameter("@Open", System.Data.SqlDbType.Bit, 0, "Open"), New System.Data.SqlClient.SqlParameter("@Original_NC_Inv_Box", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "NC_Inv_Box", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_MainID", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MainID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Type", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Type", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_RequestSpecialKey", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "RequestSpecialKey", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Note", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Note", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Note", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Note", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Open", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Open", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@NC_Inv_Box", System.Data.SqlDbType.Int, 4, "NC_Inv_Box")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_NC_Box", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("NC_Inv_Box", "NC_Inv_Box"), New System.Data.Common.DataColumnMapping("MainID", "MainID"), New System.Data.Common.DataColumnMapping("Type", "Type"), New System.Data.Common.DataColumnMapping("RequestSpecialKey", "RequestSpecialKey"), New System.Data.Common.DataColumnMapping("Note", "Note"), New System.Data.Common.DataColumnMapping("Open", "Open")})})
        DA.Fill(DS)


        Select Case What
            Case "New"
                'Make New box
                DR = DS.Tables("T_NC_Box").NewRow
                DR("MainID") = SelectedID
                DR("Type") = Type
                DR("Open") = 1
                DR("RequestSpecialKey") = SpecialKey
                DS.Tables("T_NC_Box").Rows.Add(DR)
                DA.Update(DS, "T_NC_Box")

                NC_Box = DR("NC_Inv_Box")

                saticode.MakeLabel(False, "NC_Box", Type, SelectedID, "", 0, 0, DR("NC_Inv_Box"), "\\PWI-40\" & Me.DropDownListPrinterlist.SelectedItem.Text, "", 1, "", "", New Data.DataSet, "", "", "", False, 0)

            Case "NewNoLabel"
                'Make New box
                DR = DS.Tables("T_NC_Box").NewRow
                DR("MainID") = SelectedID
                DR("Type") = Type
                DR("Open") = 1
                DR("RequestSpecialKey") = SpecialKey
                DS.Tables("T_NC_Box").Rows.Add(DR)
                DA.Update(DS, "T_NC_Box")

                NC_Box = DR("NC_Inv_Box")

            Case "Close"
                'Set box inv to open = 0
                DR = DS.Tables(0).Rows(0)
                DR.AcceptChanges()
                DR.BeginEdit()
                DR("Open") = 0
                DR.EndEdit()
                DA.Update(DS, "T_NC_Box")

                saticode.MakeLabel(False, "NC_Box", Type, SelectedID, "", CloseQty, 0, InvNumber, "\\PWI-40\" & Me.DropDownListPrinterlist.SelectedItem.Text, "", 1, "", "", New Data.DataSet, "", "", "", False, 0)
        End Select

        Connection.Close()


    End Function

    Protected Sub GridViewBoxes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewBoxes.RowCommand

        Dim row As String
        Dim InvNumber As Integer
        Dim Qty As String
        row = e.CommandArgument.ToString
        InvNumber = Me.GridViewBoxes.Rows(row).Cells(2).Text

        Qty = CType(Me.GridViewBoxes.Rows(row).Cells(5).FindControl("TextBoxQty"), TextBox).Text
        If Qty = "" Then
            Qty = 0
        End If

        Select Case e.CommandName
            Case "AddToBox"
                If Not Qty = 0 Then
                    Try
                        NC_Box_Add(InvNumber, CType(Qty, Integer), User.Identity.Name.ToString)
                    Catch ex As Exception

                    End Try

                End If

            Case "CloseBox"
                NC_Box("Close", Me.GridViewBoxes.Rows(row).Cells(1).Text, Me.GridViewBoxes.Rows(row).Cells(3).Text, InvNumber, 0, Me.GridViewBoxes.Rows(row).Cells(4).Text)
        End Select

        GetInvList(Me.LabelPackWith.Text)

    End Sub

    Protected Sub TextBoxQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim row As String


    End Sub

    Protected Sub ButtonNewBoxNoPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonNewBoxNoPrint.Click
        Dim NewInvNumber As String

        NewInvNumber = NC_Box("NewNoLabel", Me.DropDownListType.SelectedItem.Text, Me.LabelPackWith.Text, 0, 0, "")


        GetInvList(Me.LabelPackWith.Text)
    End Sub
End Class
