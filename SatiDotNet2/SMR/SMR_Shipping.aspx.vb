
Imports System.Text.Json
Imports SatiDotNet2.Library

Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Private Part As Part

    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.CheckGroupsAuthenication({"admin", "Maintenance", "Shipping"}, Server)
        Part = New Part()
    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BuildPartCart", "buildPartCart();", True)

        'make sure the select query only includes SMRs where maintenance tech has voiced parts need to be ordered
        'value for asp listitem 'All' is 0. This is the reasoning for the WHERE clause condition looking for integer 0
        ShippingGridView_SqlDataSource.SelectCommand = "SELECT T_SMR_PartToOrder.PartToOrder_Key As PrimaryKey, " &
                "T_SMR_TicketNotes.Note, T_SMR_TicketNotes.SatiUser As Tech, T_SMR_Parts.PartDescription, T_SMR_Parts.ManufacturerOrVendor, T_SMR_Parts.PW_PartNum, T_SMR_Parts.Vendor_PartNum, " &
                "T_SMR_PartToOrder.Qty, T_SMR_PartToOrder.ShippingStatus, FORMAT(T_SMR_PartToOrder.ExpectedDeliveryDate, 'MM/dd/yyyy') As ExpectedDeliveryDate, T_SMR_PartToOrder.PO_Num " &
                "FROM T_SMR_Tickets " &
                "INNER JOIN T_SMR_TicketNotes ON T_SMR_Tickets.SMR_Key = T_SMR_TicketNotes.SMR_Key " &
                "INNER JOIN T_SMR_PartToOrder ON T_SMR_TicketNotes.SMR_Key = T_SMR_PartToOrder.SMR_Key " &
                "INNER JOIN T_SMR_Parts ON T_SMR_PartToOrder.PartKey = T_SMR_Parts.[Key] WHERE T_SMR_Tickets.SMR_Key=@SMR_Key OR (@SMR_Key=0 AND T_SMR_Tickets.OrderParts=1) ORDER BY T_SMR_Tickets.IssueDate"
        ShippingGridView_SqlDataSource.SelectParameters.Clear()
        ShippingGridView_SqlDataSource.SelectParameters.Add("SMR_Key", ShippingDropDownList.SelectedValue)
        ShippingGridView.DataBind()
    End Sub

    'Protected Sub Parts_GridView_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles ShippingGridView.RowCommand
    '    If e.CommandName = "AddToCart" Then
    '        'Dim PartID As String = e.CommandArgument.ToString()
    '        'Dim Result As Dictionary(Of String, String) = Part.AddToCart(SMR_Key, PartID, True)

    '        'If Result("Success") = False Then
    '        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "displayCartErrorMessage", "displayCartErrorMessage( " & "' " & Result("Message") & "');", True)
    '        'End If
    '    End If
    'End Sub
End Class
