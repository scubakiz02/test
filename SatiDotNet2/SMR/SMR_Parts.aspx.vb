
Imports System.Text.Json
Imports SatiDotNet2.Library

Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Private Part As Part
    Private SMR_Key As String
    Private Hover As String

    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.CheckGroupsAuthenication({"admin", "Maintenance"}, Server)
        SMR_Key = Request.QueryString("TicketID")
        Hover = Request.QueryString("Hover")
        Part = New Part()

        If Hover IsNot Nothing AndAlso Boolean.Parse(Hover) Then
            AddPartPanel.Visible = False
        End If
    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        PartsSqlDataSource.SelectCommand = "SELECT PartToOrder_Key, [SMR_Key], [ManufacturerOrVendor], [PartDescription], [PW_PartNum], [Vendor_PartNum], [Qty], [Procured] From [ALTS].[dbo].[T_SMR_PartToOrder] WHERE SMR_Key=@SMR_Key"
        PartsSqlDataSource.SelectParameters.Clear()
        PartsSqlDataSource.SelectParameters.Add("SMR_Key", SMR_Key)
        Parts_GridView.DataBind()
    End Sub

    Private Sub ClearCreatePartSection()
        VendorOrManufacturer_TextBox.Text = String.Empty
        PartDescription_TextBox.Text = String.Empty
        PW_PartNum_TextBox.Text = String.Empty
        Vendor_PartNum_TextBox.Text = String.Empty
        Qty_TextBox.Text = String.Empty
        Procured_TextBox.SelectedValue = "False"
        CreatePartError_Label.Text = String.Empty
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SetTbxFocus", "document.getElementById('" & VendorOrManufacturer_TextBox.ClientID & "').focus()", True)
    End Sub

    Protected Sub AddButton_OnClick(sender As Object, e As EventArgs) Handles AddCreatePartSection_Button.Click
        'adds record to T_SMR_Parts
        Dim Res As Dictionary(Of String, String) = Part.AddPart(New Dictionary(Of String, String) From {
            {"ManufacturerOrVendor", VendorOrManufacturer_TextBox.Text},
            {"PartDescription", PartDescription_TextBox.Text},
            {"PW_PartNum", PW_PartNum_TextBox.Text},
            {"Vendor_PartNum", Vendor_PartNum_TextBox.Text},
            {"Qty", Qty_TextBox.Text},
            {"Procured", Procured_TextBox.SelectedValue},
            {"SMR_Key", SMR_Key}
        }, True)

        If Res("Success") Then
            ClearCreatePartSection()
            Parts_GridView.DataBind()
        Else
            CreatePartError_Label.Text = "*" & Res("Message") & "*"
        End If
    End Sub

    Protected Sub ClearButton_OnClick(sender As Object, e As EventArgs) Handles ClearCreatePartSection_Button.Click
        'clear all textbox fields within 'Create Part' interface
        'set focus on VendorOrManufacturer_TextBox using js
        ClearCreatePartSection()
    End Sub

    'Protected Sub PartCartCheckout_Button_OnClick(sender As Object, e As EventArgs) Handles PartCartCheckout_Button.Click
    '    Response.Redirect("SMR_Viewer.aspx")
    'End Sub

    'Protected Sub Parts_GridView_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles Parts_GridView.RowCommand
    '    If e.CommandName = "AddToCart" Then
    '        Dim PartID As String = e.CommandArgument.ToString()
    '        Dim Result As Dictionary(Of String, String) = Part.AddToCart(SMR_Key, PartID, True)

    '        If Result("Success") = False Then
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "displayCartErrorMessage", "displayCartErrorMessage(" & "'" & Result("Message") & "');", True)
    '        End If
    '    End If
    'End Sub
End Class
