Imports Class1

Partial Class Production_LabelRemakeWithCurrent
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1
    Protected Sub TextBox1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim code As String
        Dim WB As String
        WB = UCase(Me.WBScanTextBox.Text)

        Dim PrinterName As String = ""

        PrinterName = "\\PWI-40\" & Me.PrinterDropDownList.SelectedItem.Text

        code = Saticode.MakeNewUpdatedLabel_300Only(WB, PrinterName, User.Identity.Name.ToString)

        If code.Contains("Error") Then
            info(code)
        Else
            If code.Contains("Removed") Then
                info(WB & " Was Reprinted & " & code)
            Else
                info(WB & " Was Reprinted")
            End If
        End If

        Me.WBScanTextBox.Text = ""

        'Me.ScriptManager1.SetFocus(Me.WBScanTextBox.ClientID)
        CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.WBScanTextBox.ClientID)
    End Sub

    Protected Sub PrinterDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Me.PrinterDropDownList.SelectedItem.Text = "Select Printer..." Then
            Me.WBScanPanel.Visible = False
            info("Printer not selected")
        Else
            Me.WBScanPanel.Visible = True
            info("Printer " & Me.PrinterDropDownList.SelectedItem.Text & " Was selected and you are ready to Scan.")
            'Me.ScriptManager1.SetFocus(Me.WBScanTextBox.ClientID)
            CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.WBScanTextBox.ClientID)
        End If

    End Sub

    Sub info(ByVal info As String)
        Me.InfoTextBox.ReadOnly = False
        Me.InfoTextBox.Text = info & Chr(13) & Chr(13) & Me.InfoTextBox.Text
        Me.InfoTextBox.ReadOnly = True
    End Sub

 
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub
End Class
