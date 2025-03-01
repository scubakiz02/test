Imports Class1
Partial Class DBMaintenance_TestLabel
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1
    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub

    Sub PrintLabels()
        Dim PrinterName As String = ""

        If Me.PrinterDropDownList.SelectedItem.Text.Contains("Zebra") Then
            PrinterName = "\\PWI-40\" & Me.PrinterDropDownList.SelectedItem.Text
        Else
            'PrinterName = "\\HVWIN7PRINT\" & Me.PrinterDropDownList.SelectedItem.Text
            PrinterName = "\\PWI-40\" & Me.PrinterDropDownList.SelectedItem.Text
        End If


        Dim File As String
        If Me.WBCheckBox.Checked = True Then
            If Not Me.CheckBoxReal.Checked = True Then
                File = Saticode.MakeLabel(True, "WB", "PWC", Left(Me.LotNumberTextBox.Text, 4), Me.LotNumberTextBox.Text, Me.TextBoxQty.Text, "1", "50", PrinterName, "", 0, "", "", New Data.DataSet, "WB", "", User.Identity.Name.ToString, False, 0)
            Else
                File = Saticode.MakeLabel(False, "WB", "PWC", Left(Me.LotNumberTextBox.Text, 4), Me.LotNumberTextBox.Text, Me.TextBoxQty.Text, "2", "50", PrinterName, "", 0, "", "", New Data.DataSet, "WB", "", User.Identity.Name.ToString, False, 0)
            End If

        End If

        If Me.CBCheckBox.Checked = True Then
            Saticode.MakeLabel(True, "CL", "Shipping WIP", Left(Me.LotNumberTextBox.Text, 4), Me.LotNumberTextBox.Text, "20", "1", "50", PrinterName, "", 0, "", "", Saticode.GetAddress("Shipping", Left(Me.LotNumberTextBox.Text, 4), ""), "WB", "", User.Identity.Name.ToString, False, 0)
        End If

        If Me.ADCheckBox.Checked = True Then
            File = Saticode.MakeLabel(True, "Address", "PWC", Left(Me.LotNumberTextBox.Text, 4), Me.LotNumberTextBox.Text, "20", "1", "50", PrinterName, "", 1, "", "", Saticode.GetAddress("Shipping", Left(Me.LotNumberTextBox.Text, 4), ""), "WB", "", User.Identity.Name.ToString, False, 0)
        End If

        If Me.InfoPadCheckBox.Checked = True Then
            File = Saticode.MakeLabel(True, "InfoPad", "", "", "", "20", "1", 50, PrinterName, "", 1, "", "", Saticode.GetAddress("Shipping", Left(Me.LotNumberTextBox.Text, 4), ""), "WB", Me.LotNumberTextBox.Text, User.Identity.Name.ToString, False, 0)
        End If

    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        PrintLabels()
    End Sub
End Class
