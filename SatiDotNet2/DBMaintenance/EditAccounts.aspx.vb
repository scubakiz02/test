

Partial Class DBMaintenance_EditRoles
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim InterfaceToButtons As New Dictionary(Of DropDownList, Button)
    Dim PanelControls As New Dictionary(Of Panel, Dictionary(Of String, Control))

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        '?ReturnUrl=~/DBMaintenance/EditRoles.aspx")

        MenuAuthenication.CheckGroupAuthenication("EditRoles", Server)
        InterfaceToButtons(ActiveUsersDropDownList) = LockUserButton
        InterfaceToButtons(LockedUsersDropDownList) = UnlockUsersButton

        With PanelControls
            .Add(InactiveUsersPanel, New Dictionary(Of String, Control) From {{"ListBox", InactiveUsersListBox}, {"DropDownList", InactiveUsersDropDownList}})
            .Add(ActiveUsersPanel, New Dictionary(Of String, Control) From {{"ListBox", ActiveUsersRolesListBox}, {"DropDownList", ActiveUsersDropDownList}})
            .Add(LockedUsersPanel, New Dictionary(Of String, Control) From {{"ListBox", LockedUsersRolesListBox}, {"DropDownList", LockedUsersDropDownList}})
        End With
        '
        If Not Page.IsPostBack Then
            'Set IsAnonymous field of rows with a LastActivityDate at minimum 1 year ago
            SatiCode.DeleteMyAltsRecords("UPDATE [SatiUsers].[dbo].[aspnet_Users] SET IsAnonymous=1 WHERE IsAnonymous=0 AND LastActivityDate < DATEADD(YEAR,-1,CAST(GETDATE() AS DATE))")
        End If
    End Sub

    Sub displayPanel(Panel As Panel)
        Dim ListItem As New ListItem
        Dim ListBox As ListBox
        Dim DropDownList As DropDownList

        'make the associated panel visible
        For Each InterfacePanel As Panel In PanelControls.Keys
            If InterfacePanel.ID = Panel.ID Then
                InterfacePanel.Visible = True
            Else
                InterfacePanel.Visible = False
            End If
        Next

        'refresh contols in associated panel
        ListBox = PanelControls(Panel)("ListBox")
        ListBox.Items.Clear()

        DropDownList = PanelControls(Panel)("DropDownList")
        DropDownList.Items.Clear()

        ListItem.Selected = True
        ListItem.Text = "Select User..."
        DropDownList.Items.Add(ListItem)

        DropDownList.DataBind()
    End Sub

    Protected Sub RB_StatusChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim selectedRadio As RadioButton = CType(sender, RadioButton)

        If selectedRadio.ID = "InactiveUsersRB" Then
            displayPanel(InactiveUsersPanel)
        ElseIf selectedRadio.ID = "ActiveUsersRB" Then
            displayPanel(ActiveUsersPanel)
            LockUserButton.BackColor = Drawing.Color.LightGray
            LockUserButton.ForeColor = Drawing.Color.Gray
        ElseIf selectedRadio.ID = "LockedUsersRB" Then
            displayPanel(LockedUsersPanel)
            UnlockUsersButton.BackColor = Drawing.Color.LightGray
            UnlockUsersButton.ForeColor = Drawing.Color.Gray
        End If
    End Sub


    Protected Sub ActiveUsersDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        modSQL(ActiveUsersDropDownList)
        LockUser("Look", ActiveUsersDropDownList)
    End Sub

    Protected Sub InactiveUsersDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        modSQL(InactiveUsersDropDownList)
    End Sub

    Protected Sub LockedUsersDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        modSQL(LockedUsersDropDownList)
        LockUser("Look", LockedUsersDropDownList)
    End Sub

    Sub modSQL(DropDownList As DropDownList)
        Dim TheUser As String
        TheUser = DropDownList.SelectedItem.Text
        Me.NewRolesSqlDataSource.SelectCommand = "SELECT TOP 100 PERCENT RoleName, RoleId FROM dbo.aspnet_Roles AS aspnet_Roles_1 WHERE (NOT (RoleName IN (SELECT dbo.aspnet_Roles.RoleName FROM dbo.aspnet_Users INNER JOIN dbo.aspnet_UsersInRoles ON dbo.aspnet_Users.UserId = dbo.aspnet_UsersInRoles.UserId INNER JOIN dbo.aspnet_Roles ON dbo.aspnet_UsersInRoles.RoleId = dbo.aspnet_Roles.RoleId WHERE (dbo.aspnet_Users.UserName = N'" & TheUser & "')))) ORDER BY RoleName"
        Me.RolesDropDownList.DataBind()

        Me.UsersRolesSqlDataSource.SelectCommand = "SELECT dbo.aspnet_Roles.RoleName, dbo.aspnet_Roles.RoleId FROM dbo.aspnet_UsersInRoles INNER JOIN dbo.aspnet_Users ON dbo.aspnet_UsersInRoles.UserId = dbo.aspnet_Users.UserId INNER JOIN dbo.aspnet_Roles ON dbo.aspnet_UsersInRoles.RoleId = dbo.aspnet_Roles.RoleId WHERE (dbo.aspnet_Users.UserName = N'" & TheUser & "') ORDER BY dbo.aspnet_Roles.RoleName"
        Me.ActiveUsersRolesListBox.DataBind()
    End Sub

    Protected Sub AddButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        EditRole("Add", Me.ActiveUsersDropDownList.SelectedItem.Value, Me.RolesDropDownList.SelectedItem.Value)
        modSQL(ActiveUsersDropDownList)
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
        EditRole("Remove", Me.ActiveUsersDropDownList.SelectedValue, Me.ActiveUsersRolesListBox.SelectedValue)
        modSQL(ActiveUsersDropDownList)
    End Sub


    Protected Sub LockUserButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'ensure a user has been selected and the Lock button is NOT grayed out
        If Not Me.ActiveUsersDropDownList.SelectedValue = "Select User..." And LockUserButton.ForeColor = Drawing.Color.Black Then
            LockUser("Lock", Me.ActiveUsersDropDownList)
        End If
    End Sub

    Protected Sub UnlockUsersButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'ensure a user has been selected and the UnLock button is NOT grayed out
        If Not Me.LockedUsersDropDownList.SelectedValue = "Select User..." And UnlockUsersButton.ForeColor = Drawing.Color.Black Then
            LockUser("UnLock", LockedUsersDropDownList)
            LockedUsersDropDownList.Items.RemoveAt(LockedUsersDropDownList.SelectedIndex)
            LockedUsersRolesListBox.Items.Clear()
        End If
    End Sub


    Protected Sub ButtonDeleteUser_Click(sender As Object, e As EventArgs) Handles ButtonDeleteUser.Click
        If Not Me.ActiveUsersDropDownList.SelectedValue = "Select User..." Then
            Dim UserID As String = Me.ActiveUsersDropDownList.SelectedValue
            Dim DeleteSQL As String = "UPDATE [SatiUsers].[dbo].[aspnet_Users] SET IsAnonymous=1 WHERE UserID='" & UserID & "'"
            SatiCode.DeleteMyAltsRecords(DeleteSQL)
            Me.ActiveUsersDropDownList.Items.RemoveAt(Me.ActiveUsersDropDownList.SelectedIndex)
        End If
    End Sub

    Protected Sub RestoreUserButton_Click(sender As Object, e As EventArgs)
        If Not Me.InactiveUsersDropDownList.SelectedValue = "Select User..." Then
            Dim UserID As String = Me.InactiveUsersDropDownList.SelectedValue
            Dim DeleteSQL As String = "UPDATE [SatiUsers].[dbo].[aspnet_Users] SET IsAnonymous=0 WHERE UserID='" & UserID & "'"
            SatiCode.DeleteMyAltsRecords(DeleteSQL)
            Me.InactiveUsersDropDownList.Items.RemoveAt(Me.InactiveUsersDropDownList.SelectedIndex)
            InactiveUsersListBox.Items.Clear()
        End If
    End Sub

    Sub LockUser(ByVal Action As String, DropDownList As DropDownList)
        'SELECT UserId, RoleId FROM aspnet_UsersInRoles WHERE (UserId LIKE '0')
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = "Data Source=PWI-31\SATIDB;Initial Catalog=SatiUsers;Persist Security Info=True;User ID=sati;Password=laptopia"
        Connection.Open()

        Dim My_DA As New Data.SqlClient.SqlDataAdapter
        Dim My_DS As New Data.DataSet
        Dim My_DR As Data.DataRow
        Dim LockUnlockButton As Button
        '*****************************************************************
        '************************Select***********************************
        '*****************************************************************
        Dim MySelectCmd As New System.Data.SqlClient.SqlCommand
        With MySelectCmd
            .CommandText = "SELECT UserId, IsApproved FROM aspnet_Membership WHERE (UserId = '" & DropDownList.SelectedValue & "')"
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

        LockUnlockButton = InterfaceToButtons(DropDownList)
        Select Case Action
            Case "Lock"
                My_DR = My_DS.Tables(0).Rows(0)
                My_DR.AcceptChanges()
                My_DR.BeginEdit()
                My_DR("IsApproved") = 0
                My_DR.EndEdit()
                My_DA.Update(My_DS, "aspnet_Membership")
                LockUnlockButton.BackColor = Drawing.Color.LightGray
                LockUnlockButton.ForeColor = Drawing.Color.Gray

            Case "UnLock"
                My_DR = My_DS.Tables(0).Rows(0)
                My_DR.AcceptChanges()
                My_DR.BeginEdit()
                My_DR("IsApproved") = 1
                My_DR.EndEdit()
                My_DA.Update(My_DS, "aspnet_Membership")
                LockUnlockButton.BackColor = Drawing.Color.LightGray
                LockUnlockButton.ForeColor = Drawing.Color.Gray

            Case "Look"
                My_DR = My_DS.Tables(0).Rows(0)

                ' TO DO: refactor logic below, not very maintainable as it stands
                If LockUnlockButton.Text = "Lock User" Then
                    'here if you're on the Modify Account interface
                    If My_DR("IsApproved") Then
                        'specified user is NOT locked out 
                        LockUnlockButton.BackColor = Drawing.Color.LawnGreen
                        LockUnlockButton.ForeColor = Drawing.Color.Black
                    Else
                        'specified user is locked out 
                        'gray out the buttons to signify user IS locked out
                        LockUnlockButton.BackColor = Drawing.Color.LightGray
                        LockUnlockButton.ForeColor = Drawing.Color.Gray
                    End If
                Else
                    'here if you're on the Unlock Users interface
                    LockUnlockButton.BackColor = Drawing.Color.Coral
                    LockUnlockButton.ForeColor = Drawing.Color.Black
                End If


        End Select
        Connection.Close()
    End Sub

End Class
