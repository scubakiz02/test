Imports Class1

Partial Class Production_ReprintLabel
    Inherits System.Web.UI.Page

    Dim Saticode As New Class1

    Protected Sub TextBox1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim code As String
        Dim WB As String
        Dim LabelsMadeKey As Integer = ""
        WB = UCase(Me.WBScanTextBox.Text)

        If WB.Contains("WB") Then
            Dim MyDS As New Data.DataSet
            Dim DR As Data.DataRow
            WB = Mid(WB, 3)
            MyDS = Saticode.GetMyDataSet("SELECT dbo.T_FGI_Boxes.BoxInvNumber, dbo.T_FGI_Boxes.LabelsMadeKey, dbo.LabelsMade.Lot FROM dbo.T_FGI_Boxes INNER JOIN dbo.LabelsMade ON dbo.T_FGI_Boxes.LabelsMadeKey = dbo.LabelsMade.LabelRecordNumber WHERE (dbo.T_FGI_Boxes.BoxInvNumber = " & WB & ")")
            DR = MyDS.Tables(0).Rows(0)
            LabelsMadeKey = DR("LabelsMadeKey")

            Dim PrinterName As String = ""

            PrinterName = "\\PWI-40\" & Me.PrinterDropDownList.SelectedItem.Text

            code = Saticode.MakeLabel(False, "WB", "PWC", Left(DR("Lot"), 4), DR("Lot"), 0, 0, 0, PrinterName, "", 1, "", "", New Data.DataSet, "", "", User.Identity.Name.ToString, True, LabelsMadeKey)
        End If

        If WB.Contains("W") Then
            WB = Mid(WB, 2)
            'code = Saticode.MakeLabel(False, "WB", "PWC", "", "", 0, 0, 0, Me.PrinterDropDownList.SelectedItem.Text, 1, "", "", New Data.DataSet, "", "", User.Identity.Name.ToString, True, WB)
        End If
        'code = Saticode.MakeNewUpdatedLabel_OldSystem(WB, Me.PrinterDropDownList.SelectedItem.Text, User.Identity.Name.ToString)

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
