
Partial Class PC_ViewPalletHolding
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub

    Protected Sub ButtonLoad_Click(sender As Object, e As EventArgs) Handles ButtonLoad.Click
        LoadLoadData()
    End Sub


    Sub LoadLoadData()
        Dim ViewBy As String = ""
        Dim ScanKey As String = DropDownList1.SelectedItem.Text
        If RadioButtonID.Checked = True Then
            ViewBy = "ID"
        End If
        If RadioButtonLot.Checked = True Then
            ViewBy = "LOT"
        End If
        If RadioButtonCustomer.Checked = True Then
            ViewBy = "Customer"
        End If

        Select Case ViewBy
            Case "ID"
                SqlDataSourceMyData.SelectCommand = "SELECT TOP (100) PERCENT dbo.MainID.CustomerID, LEFT(dbo.LabelsMade.Lot, 4) AS [ID/Lot], SUM(dbo.LabelsMade.Wafers) AS QTY, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number FROM dbo.LabelsMade INNER JOIN dbo.ShippingInventory ON dbo.LabelsMade.LabelRecordNumber = dbo.ShippingInventory.LotEntry INNER JOIN dbo.T_PH_Table INNER JOIN dbo.T_PH_DayScans ON dbo.T_PH_Table.PH_Key = dbo.T_PH_DayScans.PH_Key ON dbo.ShippingInventory.Carton_Key = dbo.T_PH_Table.CB INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO INNER JOIN dbo.MainID ON LEFT(dbo.LabelsMade.Lot, 4) = dbo.MainID.MainID WHERE (dbo.T_PH_DayScans.ScanKey = CONVERT(DATETIME, '" & ScanKey & "', 102)) GROUP BY LEFT(dbo.LabelsMade.Lot, 4), dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.MainID.CustomerID ORDER BY [ID/Lot]"


            Case "LOT"
                SqlDataSourceMyData.SelectCommand = "SELECT TOP (100) PERCENT dbo.MainID.CustomerID, dbo.LabelsMade.Lot AS [ID/Lot], SUM(dbo.LabelsMade.Wafers) AS QTY, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number FROM dbo.LabelsMade INNER JOIN dbo.ShippingInventory ON dbo.LabelsMade.LabelRecordNumber = dbo.ShippingInventory.LotEntry INNER JOIN dbo.T_PH_Table INNER JOIN dbo.T_PH_DayScans ON dbo.T_PH_Table.PH_Key = dbo.T_PH_DayScans.PH_Key ON dbo.ShippingInventory.Carton_Key = dbo.T_PH_Table.CB INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO INNER JOIN dbo.MainID ON LEFT(dbo.LabelsMade.Lot, 4) = dbo.MainID.MainID WHERE (dbo.T_PH_DayScans.ScanKey = CONVERT(DATETIME, '" & ScanKey & "', 102)) GROUP BY dbo.LabelsMade.Lot, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.MainID.CustomerID ORDER BY dbo.LabelsMade.Lot"


            Case "Customer"
                SqlDataSourceMyData.SelectCommand = "SELECT TOP (100) PERCENT dbo.MainID.CustomerID, LEFT(dbo.LabelsMade.Lot, 4) AS [ID/Lot], SUM(dbo.LabelsMade.Wafers) AS QTY, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number FROM dbo.LabelsMade INNER JOIN dbo.ShippingInventory ON dbo.LabelsMade.LabelRecordNumber = dbo.ShippingInventory.LotEntry INNER JOIN dbo.T_PH_Table INNER JOIN dbo.T_PH_DayScans ON dbo.T_PH_Table.PH_Key = dbo.T_PH_DayScans.PH_Key ON dbo.ShippingInventory.Carton_Key = dbo.T_PH_Table.CB INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO INNER JOIN dbo.MainID ON LEFT(dbo.LabelsMade.Lot, 4) = dbo.MainID.MainID WHERE (dbo.T_PH_DayScans.ScanKey = CONVERT(DATETIME, '" & ScanKey & "', 102)) GROUP BY LEFT(dbo.LabelsMade.Lot, 4), dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.MainID.CustomerID ORDER BY dbo.MainID.CustomerID, [ID/Lot]"


        End Select



    End Sub


    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        LoadLoadData()
    End Sub
    Protected Sub RadioButtonID_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonID.CheckedChanged
        LoadLoadData()
    End Sub
    Protected Sub RadioButtonLot_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonLot.CheckedChanged
        LoadLoadData()
    End Sub
    Protected Sub RadioButtonCustomer_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonCustomer.CheckedChanged
        LoadLoadData()
    End Sub
End Class
