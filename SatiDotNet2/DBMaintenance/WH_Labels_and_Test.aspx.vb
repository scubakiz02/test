
Partial Class DBMaintenance_WH_Labels_and_Test
    Inherits System.Web.UI.Page
    Dim saticode As New Class1
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub
    Protected Sub ButtonTestNC_Box_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonTestNC_Box.Click
        saticode.MakeLabel(False, "NC_Box", "Pattern", "2386", "", 0, 0, 123456789, "\\PWI-40\" & Me.DropDownListPrinterlist.SelectedItem.Text, "", 1, "", "", New Data.DataSet, "", "", "", False, 0)

    End Sub

    Protected Sub ButtonTestNC_Pallet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonTestNC_Pallet.Click
        saticode.MakeLabel(False, "NC_Pallet", "", "", "", 0, 0, 123456789, "\\PWI-40\" & Me.DropDownListPrinterlist.SelectedItem.Text, "", 1, "", "", New Data.DataSet, "", "", "", False, 0)
    End Sub
End Class
