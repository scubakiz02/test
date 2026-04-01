Imports TransferIDTableAdapters
Partial Class DBMaintenance_IDSplits
    Inherits System.Web.UI.Page

    Sub ReloadMe()
        Dim IDFrom As String = Me.IDDropDownList.SelectedItem.Text
        Me.OtherIDsSqlDataSource.SelectCommand = "SELECT dbo.MainID.CustomerID, MainID_1.MainID FROM dbo.MainID INNER JOIN dbo.MainID AS MainID_1 ON dbo.MainID.CustomerID = MainID_1.CustomerID GROUP BY dbo.MainID.MainID, dbo.MainID.CustomerID, MainID_1.MainID HAVING (dbo.MainID.MainID = N'" & IDFrom & "')"
        Me.StageLocationSqlDataSource.SelectCommand = "SELECT dbo.WI_Rev.ID, dbo.CannedPaths.ProcessOrder, dbo.CannedPaths.StageName FROM dbo.CannedPaths INNER JOIN dbo.WI_Rev ON dbo.CannedPaths.PathName = dbo.WI_Rev.PathName WHERE (dbo.WI_Rev.ID = N'" & IDFrom & "') ORDER BY dbo.CannedPaths.ProcessOrder"
        Me.TransferIdSqlDataSource.SelectCommand = "SELECT [From], [To], StageName, Operator, Created FROM dbo.TransferID_ByStage WHERE ([From] = N'" & IDFrom & "')"
        Me.AllStagesCheckBox.Checked = False

    End Sub

    Protected Sub IDDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles IDDropDownList.SelectedIndexChanged
        ReloadMe()
    End Sub

    Protected Sub GoButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GoButton.Click
        Dim TransferIDTable As New TransferID_ByStageTableAdapter
        Dim Stage As String
        Dim IDFrom As String = Me.IDDropDownList.SelectedItem.Text
        Dim IDTo As String = Me.ToIDDropDownList.SelectedItem.Text
        If Me.AllStagesCheckBox.Checked = True Then
            Stage = "All"
        Else
            Stage = Me.AtLocationDropDownList.SelectedItem.Text
        End If
        TransferIDTable.InsertTransferIDQuery(IDFrom, IDTo, Stage, User.Identity.Name.ToString, System.DateTime.Now.ToShortDateString)
        ReloadMe()
        'Response.Redirect("IDSplits.aspx")
    End Sub

    Protected Sub GridView1_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles GridView1.RowDeleted
        ReloadMe()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        'Server.Transfer("~/Login.aspx?ReturnUrl=~/CustomerMaintenance/IDSplits.aspx")

        MenuAuthenication.CheckGroupAuthenication("CustomerEdit", Server)
    End Sub
End Class
