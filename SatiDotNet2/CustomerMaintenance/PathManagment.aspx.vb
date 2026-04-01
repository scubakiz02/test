
Partial Class CustomerMaintenance_PathManagment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)

        UpdatePath()
    End Sub

    Sub UpdatePath()
        Try
            Me.SqlDataSourcePath.SelectCommand = "SELECT PathName, ProcessOrder, StageName FROM dbo.CannedPaths WHERE (PathName = N'" & Me.PathsDropDownList.SelectedValue.ToString & "')"

            Me.GridView1.DataBind()
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub PathsDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.PathNameLabel.Text = Me.PathsDropDownList.SelectedValue.ToString
        UpdatePath()
        Path2Editload()
    End Sub


    Sub Path2Editload()
        Dim PathName As String = Me.PathsDropDownList.SelectedValue.ToString


        Me.SqlDataSourceIdsForPath.SelectCommand = "SELECT ID, PathName FROM dbo.WI_Rev WHERE (EffectiveDtd < { fn NOW() }) AND (ExpirationDtd > { fn NOW() } OR ExpirationDtd IS NULL) AND (PathName = N'" & PathName & "')"
        Me.ListBoxIdsForPath.DataBind()
    End Sub

    Protected Sub ButtonUp_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub ButtonDown_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
End Class
