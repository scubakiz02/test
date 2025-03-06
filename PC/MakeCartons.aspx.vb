Imports Class1
Partial Class PC_MakeCartons
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        MakeTheLabel()
    End Sub

    Sub MakeTheLabel()
        Dim Printer As String
        If Not Me.PrinterDropDownList.SelectedItem.Text = "Select Printer..." Then
            Printer = Me.PrinterDropDownList.SelectedItem.Text
        Else
            Exit Sub
        End If
        If CheckBoxReprint.Checked = False Then
            SatiCode.MakeCarton_BK_2019_01_23(UCase(Me.WaferBoxTextBox.Text), Printer, False, False, 0)
        Else
            If UCase(Me.WaferBoxTextBox.Text.Contains("WB")) = True Then
                SatiCode.MakeCarton_BK_2019_01_23(Mid(Me.WaferBoxTextBox.Text, 3), Printer, False, True, 0)
            End If
            If UCase(Me.WaferBoxTextBox.Text.Contains("CB")) = True Then
                SatiCode.MakeCarton_BK_2019_01_23(Mid(Me.WaferBoxTextBox.Text, 3), Printer, False, True, 0)
            End If

        End If

        Me.WaferBoxTextBox.Text = ""
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("PC", Server)
    End Sub

    Protected Sub TextBoxScan_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxScan.TextChanged
        Dim Scan As String = UCase(Me.TextBoxScan.Text)
        Dim MySQL As String
        Dim DS As Data.DataSet
        Dim DR As Data.DataRow

        '300mm or new style
        If Scan.Contains("WB") Then
            '*******************************************
            '*************** QA Check ******************
            '*******************************************
            Dim WaferBoxQACheck As String = SatiCode.CheckWaferBoxData("WB", Mid(Scan, 3))

            If Not WaferBoxQACheck = "PASS" Then
                Dim Flex As New FlexCel.XlsAdapter.XlsFile
                Dim Path As String = "\\PWI-40\software$\LabelTemplates\labeltemplateWb.xls"
                MySQL = "SELECT dbo.T_FGI_Boxes.BoxInvNumber, dbo.T_FGI_Boxes.InstanceKey, dbo.T_FGI_Boxes.CartonNumber, dbo.LabelsMade.Lot FROM dbo.T_FGI_Boxes INNER JOIN dbo.LabelsMade ON dbo.T_FGI_Boxes.LabelsMadeKey = dbo.LabelsMade.LabelRecordNumber WHERE (dbo.T_FGI_Boxes.BoxInvNumber = " & Mid(Scan, 3) & ")"
                DS = SatiCode.GetMyDataSet(MySQL)
                DR = DS.Tables(0).Rows(0)
                Flex.Open(Path)
                Flex.ActiveSheetByName = "BoxInfo"
                Flex.SetCellValue(3, 1, "")
                Flex.SetCellValue(5, 1, DR("BoxInvNumber").ToString)
                Flex.SetCellValue(7, 1, DR("InstanceKey").ToString)
                Flex.SetCellValue(12, 1, WaferBoxQACheck)
                Flex.SetCellValue(14, 1, DR("Lot").ToString)
                Flex.ConvertFormulasToValues(True)
                Flex.Recalc()
                Flex.PrintOptions = FlexCel.Core.TPrintOptions.None
                Dim printlabel As New FlexCel.Render.FlexCelPrintDocument(Flex)
                With printlabel
                    .PrinterSettings.PrinterName = Me.PrinterDropDownList.SelectedItem.Text
                    .PrinterSettings.Copies = 1
                    .Print()
                    .Dispose()
                End With
                Me.WaferBoxTextBox.Text = ""
                Me.LabelBoxQty.Text = 0

                Exit Sub
            End If
            '*******************************************

            MySQL = "SELECT dbo.T_FGI_Boxes.BoxInvNumber, LEFT(dbo.LabelsMade.Lot, 4) AS BoxID FROM dbo.T_FGI_Boxes INNER JOIN dbo.LabelsMade ON dbo.T_FGI_Boxes.LabelsMadeKey = dbo.LabelsMade.LabelRecordNumber WHERE (dbo.T_FGI_Boxes.BoxInvNumber = " & Mid(Scan, 3) & ")"
            DS = SatiCode.GetMyDataSet(MySQL)
            DR = DS.Tables(0).Rows(0)
            Me.WaferBoxTextBox.Text = Me.WaferBoxTextBox.Text & Me.TextBoxScan.Text & Chr(10)
            Me.TextBoxScan.Text = ""
            Me.TextBoxScan.Focus()
            Me.LabelBoxQty.Text = Me.LabelBoxQty.Text + 1

            If SatiCode.Get_WaferBoxs_Per_ShippingBox(DR("BoxID")) = Me.LabelBoxQty.Text Then
                MakeTheLabel()
                Me.LabelBoxQty.Text = 0
            End If

        End If

    End Sub



End Class
