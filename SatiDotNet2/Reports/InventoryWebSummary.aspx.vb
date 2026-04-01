
Partial Class Reports_InventoryWebSummary
    Inherits System.Web.UI.Page

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        Dim CustomerID As String
        CustomerID = Me.DropDownList1.SelectedValue.ToString
        Me.InvSummarySqlDataSource.SelectCommand = "SELECT MainID, CustomerID, WHQty, Incoming, [S&E - Lap], Presort, Polish, ISNULL(Incoming, 0) + ISNULL([S&E - Lap], 0) + ISNULL(Presort, 0) + ISNULL(Polish, 0) AS [WIP Sum], [Final Pack], FGI, [Polish Rework], [Lap Rework], [S&E Rework], [Cleanroom Partials], [Polish Partials] FROM [ALTS].[dbo].[fctn_SATI_INV_By_ManageArea]() WHERE (CustomerID = N'" & CustomerID & "')"
        Me.GridView1.DataSource = Me.InvSummarySqlDataSource
        Me.GridView1.DataBind()

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        'fctn_SATI_INV_By_ManageArea
        'Me.InvSummarySqlDataSource.SelectCommand = "SELECT MainID, CustomerID, WHQty, Incoming, [S&E - Lap], Presort, Polish, ISNULL(Incoming, 0) + ISNULL([S&E - Lap], 0) + ISNULL(Presort, 0) + ISNULL(Polish, 0) AS [WIP Sum], [Final Pack], FGI, [Polish Rework], [Lap Rework], [S&E Rework], [Cleanroom Partials], [Polish Partials] FROM dbo.Q_Sati_INV_Summary"
        Me.InvSummarySqlDataSource.SelectCommand = "SELECT MainID, CustomerID, WHQty, Incoming, [S&E - Lap], Presort, Polish, ISNULL(Incoming, 0) + ISNULL([S&E - Lap], 0) + ISNULL(Presort, 0) + ISNULL(Polish, 0) AS [WIP Sum], [Final Pack], FGI, [Polish Rework], [Lap Rework], [S&E Rework], [Cleanroom Partials], [Polish Partials] FROM [ALTS].[dbo].[fctn_SATI_INV_By_ManageArea]()"
        Me.GridView1.DataSource = Me.InvSummarySqlDataSource
        Me.GridView1.DataBind()

        'SELECT MainID, CustomerID, WHQty, Incoming, [S&E - Lap], Presort, Polish, ISNULL(Incoming, 0) + ISNULL([S&E - Lap], 0) + ISNULL(Presort, 0) + ISNULL(Polish, 0) AS [WIP Sum], [Final Pack], FGI, [Polish Rework], [Lap Rework], [S&E Rework] FROM dbo.Q_Sati_INV_Summary
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub
End Class
