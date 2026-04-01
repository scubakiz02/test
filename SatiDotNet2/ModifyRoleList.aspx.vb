
Partial Class CreateNewRole
    Inherits System.Web.UI.Page

    Protected Sub Load_Page(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("MasterDBAdmin", Server)
    End Sub

    Protected Sub NewRoleButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles NewRoleButton.Click
        Try
            Dim newRole As String = NewRoleText.Text.Trim
            newRole = newRole.Replace(" ", "")
            newRole = CheckRoleExists(newRole)

            If newRole <> "" Then
                If CurRoleList.Items.Contains(CurRoleList.Items.FindByText(newRole)) Then
                    NewRoleText.BackColor = Drawing.Color.FromArgb(255, 197, 197)
                    CurRoleList.SelectedIndex = CurRoleList.Items.IndexOf(CurRoleList.Items.FindByText(newRole))
                    UpdateMessage(1, newRole)
                Else
                    NewRoleText.BackColor = Drawing.Color.White
                    Roles.CreateRole(newRole)
                    CurRoleList.DataBind()
                    CurRoleList.SelectedIndex = CurRoleList.Items.IndexOf(CurRoleList.Items.FindByText(newRole))
                    UpdateMessage(0, newRole)
                End If
            Else
                NewRoleText.BackColor = Drawing.Color.FromArgb(255, 197, 197)
                CurRoleList.SelectedIndex = -1
                UpdateMessage(2, "")
            End If
        Catch ex As Exception
            UpdateMessage(5, "")
        End Try
    End Sub

    Protected Function CheckRoleExists(norRole As String) As String
        Dim exists As String = norRole

        For Each role In CurRoleList.Items
            If norRole.ToUpper = role.ToString.ToUpper Then
                exists = role.ToString
            End If
        Next

        Return exists
    End Function

    Protected Sub RemoveRoleButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RemoveRoleButton.Click
        Try
            Dim selectedRole As String

            If CurRoleList.SelectedIndex <> -1 Then
                selectedRole = CurRoleList.SelectedItem.Text
            Else
                selectedRole = ""
            End If

            If selectedRole <> "" Then
                Roles.DeleteRole(selectedRole)
                CurRoleList.DataBind()
                CurRoleList.SelectedIndex = -1
                CurRoleList.BackColor = Drawing.Color.White
                UpdateMessage(3, selectedRole)
            Else
                CurRoleList.SelectedIndex = -1
                CurRoleList.BackColor = Drawing.Color.FromArgb(255, 197, 197)
                UpdateMessage(4, "")
            End If
        Catch ex As Exception
            UpdateMessage(5, "")
        End Try
    End Sub

    Protected Sub UpdateMessage(path As Integer, roleName As String)
        If path = 0 Then
            ErrorMessage.Text = "The new role named, " & roleName & ", was successfully added."
            ErrorMessage.ForeColor = Drawing.Color.Blue
            ErrorMessage.BackColor = Drawing.Color.Transparent
            ErrorMessage.Visible = True
        ElseIf path = 1 Then
            ErrorMessage.Text = "* The entered role named, " & roleName & ", already exists, please try again."
            ErrorMessage.ForeColor = Drawing.Color.Red
            ErrorMessage.BackColor = Drawing.Color.Transparent
            ErrorMessage.Visible = True
        ElseIf path = 2 Then
            ErrorMessage.Text = "* The new role name cannot be blank, please try again."
            ErrorMessage.ForeColor = Drawing.Color.Red
            ErrorMessage.BackColor = Drawing.Color.Transparent
            ErrorMessage.Visible = True
        ElseIf path = 3 Then
            ErrorMessage.Text = "The selected role named, " & roleName & ", was successfully removed."
            ErrorMessage.ForeColor = Drawing.Color.Blue
            ErrorMessage.BackColor = Drawing.Color.Transparent
            ErrorMessage.Visible = True
        ElseIf path = 4 Then
            ErrorMessage.Text = "* You did not select a role, please try again."
            ErrorMessage.ForeColor = Drawing.Color.Red
            ErrorMessage.BackColor = Drawing.Color.Transparent
            ErrorMessage.Visible = True
        ElseIf path = 5 Then
            ErrorMessage.Text = "* FATAL ERROR: Please Contact your SATI.Net Administrator *"
            ErrorMessage.ForeColor = Drawing.Color.Red
            ErrorMessage.BackColor = Drawing.Color.Black
            ErrorMessage.Visible = True
        End If
    End Sub
End Class