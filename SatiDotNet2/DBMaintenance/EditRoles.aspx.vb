

Partial Class DBMaintenance_EditRoles
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        '?ReturnUrl=~/DBMaintenance/EditRoles.aspx")

        MenuAuthenication.CheckGroupAuthenication("EditRoles", Server)
    End Sub

    Protected Sub UsersDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        modSQL()
        LockUser("Look", Me.UsersDropDownList.SelectedValue)
    End Sub

    Sub modSQL()
        Dim TheUser As String
        TheUser = Me.UsersDropDownList.SelectedItem.Text
        Me.NewRolesSqlDataSource.SelectCommand = "SELECT TOP 100 PERCENT RoleName, RoleId FROM dbo.aspnet_Roles AS aspnet_Roles_1 WHERE (NOT (RoleName IN (SELECT dbo.aspnet_Roles.RoleName FROM dbo.aspnet_Users INNER JOIN dbo.aspnet_UsersInRoles ON dbo.aspnet_Users.UserId = dbo.aspnet_UsersInRoles.UserId INNER JOIN dbo.aspnet_Roles ON dbo.aspnet_UsersInRoles.RoleId = dbo.aspnet_Roles.RoleId WHERE (dbo.aspnet_Users.UserName = N'" & TheUser & "')))) ORDER BY RoleName"
        Me.RolesDropDownList.DataBind()

        Me.UsersRolesSqlDataSource.SelectCommand = "SELECT dbo.aspnet_Roles.RoleName, dbo.aspnet_Roles.RoleId FROM dbo.aspnet_UsersInRoles INNER JOIN dbo.aspnet_Users ON dbo.aspnet_UsersInRoles.UserId = dbo.aspnet_Users.UserId INNER JOIN dbo.aspnet_Roles ON dbo.aspnet_UsersInRoles.RoleId = dbo.aspnet_Roles.RoleId WHERE (dbo.aspnet_Users.UserName = N'" & TheUser & "') ORDER BY dbo.aspnet_Roles.RoleName"
        Me.UsersRolesListBox.DataBind()
    End Sub

    Protected Sub AddButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        EditRole("Add", Me.UsersDropDownList.SelectedItem.Value, Me.RolesDropDownList.SelectedItem.Value)
        modSQL()
    End Sub

    Sub EditRole(ByVal Action As String, ByVal UserID As String, ByVal RoleID As String)
        'SELECT UserId, RoleId FROM aspnet_UsersInRoles WHERE (UserId LIKE '0')

        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = "Data Source=PWI-31\SATIDB;Initial Catalog=SatiUsers;Persist Security Info=True;User ID=sati;Password=laptopia"
        Connection.Open()

        Dim My_DA As New Data.SqlClient.SqlDataAdapter
        Dim My_DS As New Data.DataSet
        Dim My_DR As Data.DataRow
        '*****************************************************************
        '************************Select***********************************
        '*****************************************************************
        Dim MySelectCmd As New System.Data.SqlClient.SqlCommand
        With MySelectCmd
            .CommandText = "SELECT UserId, RoleId FROM aspnet_UsersInRoles WHERE (UserId = '" & UserID & "')"
            .Connection = Connection
        End With
        My_DA.SelectCommand = MySelectCmd

        '*****************************************************************
        '************************Insert***********************************
        '*****************************************************************
        Dim MyInsertCmd As New System.Data.SqlClient.SqlCommand
        With MyInsertCmd
            .CommandText = "INSERT INTO [aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (@UserId, @RoleId); SELECT UserId, RoleId FROM aspnet_UsersInRoles WHERE (RoleId = @RoleId) AND (UserId = @UserId)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@UserId", System.Data.SqlDbType.UniqueIdentifier, 0, "UserId"), New System.Data.SqlClient.SqlParameter("@RoleId", System.Data.SqlDbType.UniqueIdentifier, 0, "RoleId")})
        End With
        My_DA.InsertCommand = MyInsertCmd

        '*****************************************************************
        '************************Update***********************************
        '*****************************************************************
        Dim MyUpdateCmd As New System.Data.SqlClient.SqlCommand
        With MyUpdateCmd
            .CommandText = "UPDATE [aspnet_UsersInRoles] SET [UserId] = @UserId, [RoleId] = @RoleId WHERE (([UserId] = @Original_UserId) AND ([RoleId] = @Original_RoleId)); SELECT UserId, RoleId FROM aspnet_UsersInRoles WHERE (RoleId = @RoleId) AND (UserId = @UserId)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@UserId", System.Data.SqlDbType.UniqueIdentifier, 0, "UserId"), New System.Data.SqlClient.SqlParameter("@RoleId", System.Data.SqlDbType.UniqueIdentifier, 0, "RoleId"), New System.Data.SqlClient.SqlParameter("@Original_UserId", System.Data.SqlDbType.UniqueIdentifier, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "UserId", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_RoleId", System.Data.SqlDbType.UniqueIdentifier, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "RoleId", System.Data.DataRowVersion.Original, Nothing)})
        End With
        My_DA.UpdateCommand = MyUpdateCmd

        '*****************************************************************
        '************************Delete***********************************
        '*****************************************************************
        Dim MyDeleteCmd As New System.Data.SqlClient.SqlCommand
        With MyDeleteCmd
            .CommandText = "DELETE FROM [aspnet_UsersInRoles] WHERE (([UserId] = @Original_UserId) AND ([RoleId] = @Original_RoleId))"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Original_UserId", System.Data.SqlDbType.UniqueIdentifier, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "UserId", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_RoleId", System.Data.SqlDbType.UniqueIdentifier, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "RoleId", System.Data.DataRowVersion.Original, Nothing)})
        End With
        My_DA.DeleteCommand = MyDeleteCmd

        '*****************************************************************
        '************************Genral***********************************
        '*****************************************************************
        My_DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "aspnet_UsersInRoles", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("UserId", "UserId"), New System.Data.Common.DataColumnMapping("RoleId", "RoleId")})})
        My_DA.Fill(My_DS)

        Select Case Action
            Case "Add"
                My_DR = My_DS.Tables("aspnet_UsersInRoles").NewRow
                My_DR("UserId") = UserID
                My_DR("RoleId") = RoleID
                My_DS.Tables("aspnet_UsersInRoles").Rows.Add(My_DR)
                My_DA.Update(My_DS, "aspnet_UsersInRoles")

            Case "Remove"
                Dim TheRow As Int16 = 0
                For TheRow = 0 To My_DS.Tables(0).Rows.Count - 1
                    My_DR = My_DS.Tables(0).Rows(TheRow)
                    If My_DR("RoleId").ToString = RoleID Then
                        My_DR.Delete()
                        My_DA.Update(My_DS, "aspnet_UsersInRoles")
                        Exit For
                    End If
                Next
        End Select
        Connection.Close()
    End Sub

    Protected Sub RemoveButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        EditRole("Remove", Me.UsersDropDownList.SelectedValue, Me.UsersRolesListBox.SelectedValue)
        modSQL()
    End Sub


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not Me.UsersDropDownList.SelectedValue = "Select User..." Then
            If Me.Button1.Text = "UnLock User" Then
                LockUser("UnLock", Me.UsersDropDownList.SelectedValue)
            Else
                LockUser("Lock", Me.UsersDropDownList.SelectedValue)
            End If
        End If

    End Sub


    Protected Sub ButtonDeleteUser_Click(sender As Object, e As EventArgs) Handles ButtonDeleteUser.Click
        If Not Me.UsersDropDownList.SelectedValue = "Select User..." Then
            Dim TheItem As Integer = Me.UsersDropDownList.SelectedIndex

            Membership.DeleteUser(Me.UsersDropDownList.SelectedItem.Text)
            Me.UsersDropDownList.Items.RemoveAt(Me.UsersDropDownList.SelectedIndex)

        End If
    End Sub


    Sub LockUser(ByVal Action As String, ByVal UserID As String)
        'SELECT UserId, RoleId FROM aspnet_UsersInRoles WHERE (UserId LIKE '0')

        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = "Data Source=PWI-31\SATIDB;Initial Catalog=SatiUsers;Persist Security Info=True;User ID=sati;Password=laptopia"
        Connection.Open()

        Dim My_DA As New Data.SqlClient.SqlDataAdapter
        Dim My_DS As New Data.DataSet
        Dim My_DR As Data.DataRow
        '*****************************************************************
        '************************Select***********************************
        '*****************************************************************
        Dim MySelectCmd As New System.Data.SqlClient.SqlCommand
        With MySelectCmd
            .CommandText = "SELECT UserId, IsApproved FROM aspnet_Membership WHERE (UserId = '" & UserID & "')"
            .Connection = Connection
        End With
        My_DA.SelectCommand = MySelectCmd

        '*****************************************************************
        '************************Update***********************************
        '*****************************************************************
        Dim MyUpdateCmd As New System.Data.SqlClient.SqlCommand
        With MyUpdateCmd
            .CommandText = "UPDATE [aspnet_Membership] SET [UserId] = @UserId, [IsApproved] = @IsApproved WHERE (([UserId] = @Original_UserId) AND ([IsApproved] = @Original_IsApproved)); SELECT UserId, IsApproved FROM aspnet_Membership WHERE (UserId = @UserId)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@UserId", System.Data.SqlDbType.UniqueIdentifier, 0, "UserId"), New System.Data.SqlClient.SqlParameter("@IsApproved", System.Data.SqlDbType.Bit, 0, "IsApproved"), New System.Data.SqlClient.SqlParameter("@Original_UserId", System.Data.SqlDbType.UniqueIdentifier, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "UserId", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_IsApproved", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "IsApproved", System.Data.DataRowVersion.Original, Nothing)})
        End With
        My_DA.UpdateCommand = MyUpdateCmd

        '*****************************************************************
        '************************Genral***********************************
        '*****************************************************************
        My_DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "aspnet_Membership", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("UserId", "UserId"), New System.Data.Common.DataColumnMapping("IsApproved", "IsApproved")})})
        My_DA.Fill(My_DS)

        Select Case Action
            Case "Lock"
                My_DR = My_DS.Tables(0).Rows(0)
                My_DR.AcceptChanges()
                My_DR.BeginEdit()
                My_DR("IsApproved") = 0
                My_DR.EndEdit()
                My_DA.Update(My_DS, "aspnet_Membership")
                Me.Button1.Text = "UnLock User"
                Me.Button1.BackColor = Drawing.Color.Coral

            Case "UnLock"
                My_DR = My_DS.Tables(0).Rows(0)
                My_DR.AcceptChanges()
                My_DR.BeginEdit()
                My_DR("IsApproved") = 1
                My_DR.EndEdit()
                My_DA.Update(My_DS, "aspnet_Membership")
                Me.Button1.Text = "Lock User"
                Me.Button1.BackColor = Drawing.Color.LawnGreen

            Case "Look"
                My_DR = My_DS.Tables(0).Rows(0)
                If My_DR("IsApproved") = 0 Then
                    Me.Button1.Text = "UnLock User"
                    Me.Button1.BackColor = Drawing.Color.Coral
                Else
                    Me.Button1.Text = "Lock User"
                    Me.Button1.BackColor = Drawing.Color.LawnGreen
                End If

        End Select
        Connection.Close()
    End Sub

End Class
