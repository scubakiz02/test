
Partial Class PC_ShipPending_PendingManagment
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        MenuAuthenication.CheckGroupAuthenication("Office", Server)

        'loadData()


        'Me.SqlDataSource1.SelectCommand = "SELECT TOP (100) PERCENT T_ShipmentsPending.PickTicket, ShippingUnit.ShippingID, ShippingUnit.Pallet_Key, T_ShipmentsPending.EventTime AS Made, LEFT (LabelsMade.Lot, 4) AS LotID, SUM(ShippingInventory.Total_Qty) AS Qty, T_ShipmentsPending.Notes, T_ShipmentsPending.[Key] AS Pend#, ShippingUnit.Tracking, Shipping_Log.Carrier, Shipping_Log.CustomerShippingAccount, Shipping_Log.ExsilFrieght FROM Pick_ShippingUnit INNER JOIN ShippingUnit ON Pick_ShippingUnit.Pallet_Key = ShippingUnit.Pallet_Key INNER JOIN T_ShipmentsPending INNER JOIN ShippingInventory ON T_ShipmentsPending.PickTicket = ShippingInventory.PickTicket INNER JOIN LabelsMade ON ShippingInventory.LotEntry = LabelsMade.LabelRecordNumber ON Pick_ShippingUnit.PickTicket = T_ShipmentsPending.PickTicket INNER JOIN Shipping_Log ON ShippingUnit.ShippingID = Shipping_Log.ShippingID WHERE (T_ShipmentsPending.Released = N'No') GROUP BY T_ShipmentsPending.PickTicket, T_ShipmentsPending.EventTime, T_ShipmentsPending.Notes, LEFT (LabelsMade.Lot, 4), T_ShipmentsPending.[Key], ShippingUnit.Tracking, Shipping_Log.Carrier, Shipping_Log.CustomerShippingAccount, Shipping_Log.ExsilFrieght, ShippingUnit.ShippingID, ShippingUnit.Pallet_Key ORDER BY LotID, T_ShipmentsPending.PickTicket"
        'Me.GridView1.DataSource = SqlDataSource1.SelectCommand
        'Me.GridView1.DataBind()

    End Sub

    Sub loadData()
        Dim DS As New Data.DataSet

        DS = SatiCode.GetMyDataSet("SELECT TOP (100) PERCENT T_ShipmentsPending.PickTicket, ShippingUnit.ShippingID, ShippingUnit.Pallet_Key, T_ShipmentsPending.EventTime AS Made, LEFT (LabelsMade.Lot, 4) AS LotID, SUM(ShippingInventory.Total_Qty) AS Qty, T_ShipmentsPending.Notes, T_ShipmentsPending.[Key] AS Pend#, ShippingUnit.Tracking, Shipping_Log.Carrier, Shipping_Log.CustomerShippingAccount, Shipping_Log.ExsilFrieght FROM Pick_ShippingUnit INNER JOIN ShippingUnit ON Pick_ShippingUnit.Pallet_Key = ShippingUnit.Pallet_Key INNER JOIN T_ShipmentsPending INNER JOIN ShippingInventory ON T_ShipmentsPending.PickTicket = ShippingInventory.PickTicket INNER JOIN LabelsMade ON ShippingInventory.LotEntry = LabelsMade.LabelRecordNumber ON Pick_ShippingUnit.PickTicket = T_ShipmentsPending.PickTicket INNER JOIN Shipping_Log ON ShippingUnit.ShippingID = Shipping_Log.ShippingID WHERE (T_ShipmentsPending.Released = N'No') GROUP BY T_ShipmentsPending.PickTicket, T_ShipmentsPending.EventTime, T_ShipmentsPending.Notes, LEFT (LabelsMade.Lot, 4), T_ShipmentsPending.[Key], ShippingUnit.Tracking, Shipping_Log.Carrier, Shipping_Log.CustomerShippingAccount, Shipping_Log.ExsilFrieght, ShippingUnit.ShippingID, ShippingUnit.Pallet_Key ORDER BY LotID, T_ShipmentsPending.PickTicket")
        Me.GridView1.DataSource = DS
        Me.GridView1.DataBind()

    End Sub

    Function ReleaseShipment(PickTicket As String) As String

        '

        '*********************************************************
        '************ Send Summary Email of Release **************
        '*********************************************************
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim SB As New StringBuilder
        Dim T As String

        DS = SatiCode.GetMyDataSet("SELECT TOP (100) PERCENT dbo.ShippingInventory.PickTicket, LEFT(dbo.LabelsMade.Lot, 4) AS LotID, dbo.ShippingUnit.ShippingID, dbo.ShippingUnit.Pallet_Key, SUM(dbo.ShippingInventory.Total_Qty) AS Qty, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.ShippingUnit.Tracking, dbo.Shipping_Log.Carrier, dbo.Shipping_Log.CustomerShippingAccount, dbo.Shipping_Log.ExsilFrieght FROM dbo.SO_LineItems INNER JOIN dbo.ShippingInventory INNER JOIN dbo.LabelsMade ON dbo.ShippingInventory.LotEntry = dbo.LabelsMade.LabelRecordNumber ON dbo.SO_LineItems.[Key] = dbo.LabelsMade.SO_Key INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO INNER JOIN dbo.Pick_ShippingUnit INNER JOIN dbo.ShippingUnit ON dbo.Pick_ShippingUnit.Pallet_Key = dbo.ShippingUnit.Pallet_Key INNER JOIN dbo.Shipping_Log ON dbo.ShippingUnit.ShippingID = dbo.Shipping_Log.ShippingID ON dbo.ShippingInventory.PickTicket = dbo.Pick_ShippingUnit.PickTicket GROUP BY LEFT(dbo.LabelsMade.Lot, 4), dbo.ShippingUnit.Tracking, dbo.Shipping_Log.Carrier, dbo.Shipping_Log.CustomerShippingAccount, dbo.Shipping_Log.ExsilFrieght, dbo.ShippingUnit.ShippingID, dbo.ShippingUnit.Pallet_Key, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.ShippingInventory.PickTicket HAVING (dbo.ShippingInventory.PickTicket = N'" & PickTicket & "') ORDER BY LotID")
        If DS.Tables(0).Rows.Count > 0 Then
            DR = DS.Tables(0).Rows(0)

            T = DR("Tracking").ToString

            If T = "" Then
                T = "No Data Entered"
            End If

            SB.Append(<h1 style="color: #85776D">Shipment Released from SATI Pending.</h1>)
            SB.Append(<br/>)
            SB.Append("<table style=""border-color: #D7D2C4; border-style: solid; width: 450px"" >")
            SB.Append("<tr ><td colspan=""2"" style=""text-align: center; font-size: large;"">Pick Ticket #:&nbsp;" & DR("PickTicket").ToString & "</td></tr>")
            SB.Append("<tr style=""background-color: #EFEDE7""><td>ID:&nbsp;" & DR("LotID").ToString & "</td><td>Qty:&nbsp;" & DR("Qty").ToString & "</td></tr>")
            SB.Append("<tr><td>SO #:&nbsp;" & DR("SO").ToString & "</td><td>PO#:&nbsp;" & DR("PO_Number").ToString & "</td></tr>")
            SB.Append("<tr style=""background-color: #EFEDE7""><td>Tracking Number:&nbsp;" & T & "</td><td>Carrier: &nbsp;" & DR("Carrier").ToString & "</td></tr>")
            SB.Append("</table>")
            SB.Append(<br/>)
            SB.Append("<div align=right>")
            SB.Append("<font color=Gray>" & UCase(User.Identity.Name.ToString) & "</font>")
            SB.Append(<br/>)
            SB.Append("<font color=Gray>" & DateAndTime.Now.ToLongTimeString & "</font>")
            SB.Append(<br/>)
            SB.Append("<font color=Gray>" & DateTime.Now.ToLongDateString & "</font>")
            SB.Append("</div>")

            SatiCode.SendMail_HTML(SB.ToString, "Shipment Release: ID# " & DR("LotID").ToString & ", Qty of " & DR("Qty").ToString, "AZ.ShipmentRelease@purewafer.com", "SATI@purewafer.com")

        End If

        Return (SatiCode.PendingShipmentAdd("Release", PickTicket, "", ""))



    End Function


    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim row As String
        Dim PickTicket As String
        row = e.CommandArgument.ToString
        PickTicket = Me.GridView1.Rows(row).Cells(0).Text

        If e.CommandName = "EditShip" Then
            OpenPop(row, PickTicket)
        End If

        If e.CommandName = "Release" Then
            If Me.GridView1.Rows(row).Cells(6).Text = "0" Then
                OpenPop(row, PickTicket)
                Exit Sub
            End If

            ReleaseShipment(PickTicket)
            Me.GridView1.DataBind()
        End If


    End Sub

    Sub OpenPop(Row As String, PickTicket As String)

        Me.EditModalPopupExtender.Show()

        If Me.GridView1.Rows(Row).Cells(6).Text = "0" Then
            Me.TextBoxTrackingNumber.Text = ""
        Else
            Me.TextBoxTrackingNumber.Text = Me.GridView1.Rows(Row).Cells(6).Text
        End If

        Me.DropDownListCarrier.SelectedIndex = Me.DropDownListCarrier.Items.IndexOf(Me.DropDownListCarrier.Items.FindByValue(Me.GridView1.Rows(Row).Cells(7).Text))

        Me.LabelPickTicket.Text = PickTicket

    End Sub

    Sub SavePop()
        SatiCode.Mod_ShippingUnit("ModTracking", Me.LabelPickTicket.Text, Me.TextBoxTrackingNumber.Text)
        SatiCode.Mod_Shipping_Log("ModCarrier", Me.LabelPickTicket.Text, Me.DropDownListCarrier.SelectedItem.Text)


    End Sub

    Protected Sub TextBoxScan_TextChanged(sender As Object, e As EventArgs) Handles TextBoxScan.TextChanged
        Dim Reply As String = ""
        Dim PT As String = Me.TextBoxScan.Text

        For i As Integer = 0 To Me.GridView1.Rows.Count - 1
            If PT = Me.GridView1.Rows(i).Cells(0).Text Then
                If Me.GridView1.Rows(i).Cells(6).Text = "0" Then
                    OpenPop(i, PT)
                    Me.TextBoxScan.Text = ""
                    Me.TextBoxScan.Focus()
                    Exit Sub
                Else
                    Reply = ReleaseShipment(PT)
                    If Reply = "Good" Then
                        Me.TextBoxScanInfo.Text = PT & " Was Released." & Chr(13) & Me.TextBoxScanInfo.Text
                    Else
                        Me.TextBoxScanInfo.Text = PT & " " & Reply & Chr(13) & Me.TextBoxScanInfo.Text
                    End If
                    Me.TextBoxScan.Text = ""
                    Me.TextBoxScan.Focus()

                End If
            End If
        Next

    End Sub

    Protected Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        SavePop()
        Me.GridView1.DataBind()
    End Sub

    Protected Sub ButtonSaveRealease_Click(sender As Object, e As EventArgs) Handles ButtonSaveRealease.Click
        If Not Me.TextBoxTrackingNumber.Text = "" Then
            SavePop()
            ReleaseShipment(Me.LabelPickTicket.Text)
        End If
        Me.GridView1.DataBind()

    End Sub
End Class
