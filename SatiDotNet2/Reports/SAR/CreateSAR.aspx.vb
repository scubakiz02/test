
Partial Class Reports_SAR_CreateSAR
    Inherits System.Web.UI.Page


    Protected Sub GoButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GoButton.Click
        Dim IDcOUNT As Integer = Me.CheckBoxList1.Items.Count
        Dim I As Integer = 0
        Dim SelectedIDs As Integer = 0
        Dim IDString As String = ""

        For I = 0 To IDcOUNT - 1
            If Me.CheckBoxList1.Items(I).Selected = True Then
                SelectedIDs = SelectedIDs + 1
                If SelectedIDs = 1 Then
                    IDString = Me.CheckBoxList1.Items(I).Value.ToString
                Else
                    IDString = IDString & ", " & Me.CheckBoxList1.Items(I).Value.ToString
                End If

            End If
        Next
        Session("SAR_IDs") = IDString
        Session("SAR_ID_Count") = SelectedIDs
        Session("Customer") = Me.RadioButtonList1.SelectedValue.ToString
        Session("Export") = "No"
        If Me.CheckBoxExport.Checked = True Then
            If Not Me.TextBoxEmailAddress.Text = "" Then
                Session("EmailAddress") = Me.TextBoxEmailAddress.Text & "@Purewaferinc.com"
                Session("Export") = "Yes"
                Page.Response.Redirect("SAR_View.aspx")
            End If
        Else
            Page.Response.Redirect("SAR_View.aspx")
        End If


    End Sub

    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged
        Dim Customer As String = Me.RadioButtonList1.SelectedItem.Text.ToString
        Dim ID_SQL As String
        ID_SQL = "SELECT dbo.Customer.Customer_Name, dbo.MainID.MainID FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID WHERE (dbo.Customer.Customer_Name = N'" & Customer & "')"
        Me.IDsSqlDataSource.SelectCommand = ID_SQL
        Me.GoButton.Visible = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub

End Class
