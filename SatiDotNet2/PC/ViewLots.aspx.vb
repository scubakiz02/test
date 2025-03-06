
Partial Class PC_ViewLots
    Inherits System.Web.UI.Page

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        Dim Stage As String
        Stage = Me.DropDownList1.SelectedValue.ToString
        Me.SqlDataSource1.SelectCommand = "SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.UniqueProcesses.StageName = N'" & Stage & "') GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"
        'Me.SqlDataSource1.SelectCommand = "SELECT dbo.UniqueProcesses.LotEntry, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out, dbo.UniqueProcesses.Complete, dbo.UniqueProcesses.StageName FROM dbo.UniqueProcesses LEFT OUTER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] WHERE (NOT (dbo.WaferMover.Disposition IS NULL)) AND (dbo.UniqueProcesses.StageName = N'" & Stage & "') GROUP BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.Complete, dbo.UniqueProcesses.ProcessOrder, dbo.UniqueProcesses.StageName HAVING (dbo.UniqueProcesses.Complete IS NULL) ORDER BY dbo.UniqueProcesses.LotEntry"
        Me.GridView1.DataBind()

    End Sub


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        'SELECT dbo.UniqueProcesses.LotEntry AS ID, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final Pack')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName
        Me.SqlDataSource1.SelectCommand = "SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"
        'Me.SqlDataSource1.SelectCommand = "SELECT dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out, dbo.UniqueProcesses.Complete FROM dbo.UniqueProcesses LEFT OUTER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] WHERE (NOT (dbo.WaferMover.Disposition IS NULL)) GROUP BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.Complete, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder HAVING (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) ORDER BY dbo.UniqueProcesses.LotEntry"
        Me.GridView1.DataBind()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub
End Class
