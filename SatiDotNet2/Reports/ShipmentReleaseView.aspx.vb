
Partial Class Reports_ShipmentReleaseView
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub
    Protected Sub CalendarStart_SelectionChanged(sender As Object, e As EventArgs) Handles CalendarStart.SelectionChanged
        MakeView()
    End Sub
    Protected Sub CalendarEnd_SelectionChanged(sender As Object, e As EventArgs) Handles CalendarEnd.SelectionChanged
        MakeView()
    End Sub

    Sub MakeView()

        'SELECT TOP (100) PERCENT dbo.T_ShipmentsPending.Released, dbo.T_ShipmentsPending.ReleasedDate AS [Released Shipment], LEFT(dbo.LabelsMade.Lot, 4) AS LotID, dbo.T_ShipmentsPending.PickTicket, SUM(dbo.ShippingInventory.Total_Qty) AS Qty, dbo.T_ShipmentsPending.EventTime AS [Made Shipment] FROM dbo.T_ShipmentsPending INNER JOIN dbo.ShippingInventory ON dbo.T_ShipmentsPending.PickTicket = dbo.ShippingInventory.PickTicket INNER JOIN dbo.LabelsMade ON dbo.ShippingInventory.LotEntry = dbo.LabelsMade.LabelRecordNumber WHERE (dbo.T_ShipmentsPending.ReleasedDate >= CONVERT(DATETIME, '2018-05-02 00:00:00', 102) AND dbo.T_ShipmentsPending.ReleasedDate <= CONVERT(DATETIME, '2018-05-09 00:00:00', 102)) GROUP BY dbo.T_ShipmentsPending.PickTicket, dbo.T_ShipmentsPending.EventTime, LEFT(dbo.LabelsMade.Lot, 4), dbo.T_ShipmentsPending.ReleasedDate, dbo.T_ShipmentsPending.Released ORDER BY [Released Shipment], LotID, dbo.T_ShipmentsPending.PickTicket

        Dim MySQL As String
        MySQL = ""
        MySQL = "SELECT TOP (100) PERCENT dbo.T_ShipmentsPending.ReleasedDate AS [Released Shipment], LEFT(dbo.LabelsMade.Lot, 4) AS LotID, dbo.T_ShipmentsPending.PickTicket, SUM(dbo.ShippingInventory.Total_Qty) AS Qty, dbo.T_ShipmentsPending.EventTime AS [Made Shipment] FROM dbo.T_ShipmentsPending INNER JOIN dbo.ShippingInventory ON dbo.T_ShipmentsPending.PickTicket = dbo.ShippingInventory.PickTicket INNER JOIN dbo.LabelsMade ON dbo.ShippingInventory.LotEntry = dbo.LabelsMade.LabelRecordNumber "
        'Where
        'WHERE (dbo.T_ShipmentsPending.ReleasedDate >= CONVERT(DATETIME, '2018-05-02 00:00:00', 102) AND dbo.T_ShipmentsPending.ReleasedDate <= CONVERT(DATETIME, '2018-05-09 00:00:00', 102)) GROUP BY dbo.T_ShipmentsPending.PickTicket, dbo.T_ShipmentsPending.EventTime, LEFT(dbo.LabelsMade.Lot, 4), dbo.T_ShipmentsPending.ReleasedDate, dbo.T_ShipmentsPending.Released ORDER BY [Released Shipment], LotID, dbo.T_ShipmentsPending.PickTicket

        Try
            MySQL = MySQL & "WHERE (dbo.T_ShipmentsPending.ReleasedDate >= CONVERT(DATETIME, '" & Me.CalendarStart.SelectedDate & "', 102)"
        Catch ex As Exception
            MySQL = MySQL & "WHERE (dbo.T_ShipmentsPending.ReleasedDate >= CONVERT(DATETIME, '" & DateAndTime.Now & "', 102)"
        End Try

        MySQL = MySQL & " AND "

        Try
            MySQL = MySQL & "dbo.T_ShipmentsPending.ReleasedDate <= CONVERT(DATETIME, '" & Me.CalendarEnd.SelectedDate & "', 102)) "
        Catch ex As Exception
            MySQL = MySQL & "dbo.T_ShipmentsPending.ReleasedDate <= CONVERT(DATETIME, '" & DateAndTime.Now & "', 102)) "
        End Try

        MySQL = MySQL & "GROUP BY dbo.T_ShipmentsPending.PickTicket, dbo.T_ShipmentsPending.EventTime, LEFT(dbo.LabelsMade.Lot, 4), dbo.T_ShipmentsPending.ReleasedDate, dbo.T_ShipmentsPending.Released "

        If Not Me.TextBox_ID_Filter.Text = "" Then
            MySQL = MySQL & "HAVING (LEFT(dbo.LabelsMade.Lot, 4) LIKE '" & Me.TextBox_ID_Filter.Text & "%')"
        End If

        MySQL = MySQL & "ORDER BY [Released Shipment] DESC, LotID, dbo.T_ShipmentsPending.PickTicket"

        Me.SqlDataSource1.SelectCommand = MySQL
        Me.GridView1.DataBind()

    End Sub

    Protected Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox_ID_Filter.TextChanged
        MakeView()
    End Sub
End Class
