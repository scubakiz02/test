

Partial Class PC_MakeCartonNew
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        MakeTheLabel()
        AfterPrint()
        Me.LabelCarton.Text = 0
        Me.TextBoxScanInput.Focus()
    End Sub

    Sub MakeTheLabel()
        Dim Printer As String
        If Not Me.PrinterDropDownList.SelectedItem.Text = "Select Printer..." Then
            Printer = Me.PrinterDropDownList.SelectedItem.Text
        Else
            Exit Sub
        End If
        If CheckBoxReprint.Checked = False Then
            SatiCode.MakeCarton(UCase(Me.WaferBoxTextBox.Text), Printer, False, False, 0, Me.LabelCarton.Text)
        Else
            If UCase(Me.WaferBoxTextBox.Text.Contains("WB")) = True Then
                SatiCode.MakeCarton(Mid(Me.WaferBoxTextBox.Text, 3), Printer, False, True, 0, Me.LabelCarton.Text)
            End If
            If UCase(Me.WaferBoxTextBox.Text.Contains("CB")) = True Then
                SatiCode.MakeCarton(Mid(Me.WaferBoxTextBox.Text, 3), Printer, False, True, 0, Me.LabelCarton.Text)
            End If

        End If
        Me.LabelBoxQty.Text = "0"
        AfterPrint()
        Me.TextBoxScanInput.Focus()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("PC", Server)

        Me.TextBoxScanInput.Focus()
    End Sub

    Sub AfterPrint()

        Me.TextBoxScanInfo.Text = ""
        Me.TextBoxScanInfo.BackColor = Drawing.Color.White

        Me.WaferBoxTextBox.Text = ""

        Me.LabelBoxQty.Text = "0"
        Me.LabelCarton.Text = 0
        Me.TextBoxScanInput.Focus()
    End Sub


    Function GetBoxData(MyScan As String) As Data.DataSet
        '200mm and less
        If MyScan.Contains("W") And Not MyScan.Contains("WB") Then
            GetBoxData = SatiCode.GetMyDataSet("SELECT dbo.LabelsMade.LabelRecordNumber, LEFT(dbo.LabelsMade.Lot, 4) AS BoxID, dbo.LabelsMade.Lot, dbo.LabelsMade.RecordNumber AS Spec_Key, dbo.MainIDSpec.SPEC_NUMBER, dbo.MainIDSpec.SPEC_REV_NUMBER, dbo.MainIDSpec.PART_NUMBER, dbo.MainIDSpec.PART_REV_NUMBER, dbo.LabelsMade.SO_Key, dbo.SO_Info.SO, dbo.SO_Info.PO_Number FROM dbo.MainIDSpec INNER JOIN dbo.LabelsMade INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO ON dbo.MainIDSpec.RecordNumber = dbo.LabelsMade.RecordNumber WHERE (dbo.LabelsMade.LabelRecordNumber = " & Mid(MyScan, 2) & ")")
        End If

        '300mm or new style
        If MyScan.Contains("WB") Then
            GetBoxData = SatiCode.GetMyDataSet("SELECT dbo.T_FGI_Boxes.BoxInvNumber, LEFT(dbo.LabelsMade.Lot, 4) AS BoxID, dbo.LabelsMade.Lot, dbo.LabelsMade.RecordNumber AS Spec_Key, dbo.MainIDSpec.SPEC_NUMBER, dbo.MainIDSpec.SPEC_REV_NUMBER, dbo.MainIDSpec.PART_NUMBER, dbo.MainIDSpec.PART_REV_NUMBER, dbo.LabelsMade.SO_Key, dbo.SO_Info.SO, dbo.SO_Info.PO_Number, dbo.T_FGI_Boxes.CartonNumber FROM dbo.MainIDSpec INNER JOIN dbo.T_FGI_Boxes INNER JOIN dbo.LabelsMade ON dbo.T_FGI_Boxes.LabelsMadeKey = dbo.LabelsMade.LabelRecordNumber INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO ON dbo.MainIDSpec.RecordNumber = dbo.LabelsMade.RecordNumber WHERE (dbo.T_FGI_Boxes.BoxInvNumber = " & Mid(MyScan, 3) & ")")
        End If
        Me.TextBoxScanInput.Focus()
    End Function

    Function GetCartonAddData(Carton As String) As Data.DataSet
        Dim DS As New Data.DataSet
        Dim dr As Data.DataRow
        Dim CartonCount As Int16


        'look up the carton.
        DS = SatiCode.GetMyDataSet("SELECT dbo.ShippingInventory.Carton_Key, dbo.ShippingInventory.Total_Qty, dbo.ShippingInventory.PickTicket, dbo.ShippingInventory.Confirmed, dbo.LabelsMade.Lot, LEFT(dbo.LabelsMade.Lot, 4) AS BoxID, dbo.LabelsMade.RecordNumber AS Spec_Key, dbo.MainIDSpec.PART_NUMBER, dbo.MainIDSpec.PART_REV_NUMBER, dbo.MainIDSpec.SPEC_NUMBER, dbo.MainIDSpec.SPEC_REV_NUMBER, dbo.LabelsMade.SO_Key, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number FROM dbo.ShippingInventory INNER JOIN dbo.LabelsMade ON dbo.ShippingInventory.LotEntry = dbo.LabelsMade.LabelRecordNumber INNER JOIN dbo.MainIDSpec ON dbo.LabelsMade.RecordNumber = dbo.MainIDSpec.RecordNumber INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO WHERE (dbo.ShippingInventory.Carton_Key = " & Carton & ")")

        'setup the screen with the number of boxes that have been already added to that carton
        'add all the spec info on right side of screen from past data.

        Me.WaferBoxTextBox.Text = ""
        CartonCount = DS.Tables(0).Rows.Count
        If CartonCount = 0 Then
            Me.WaferBoxTextBox.Text = "0"
            Exit Function
        End If
        dr = DS.Tables(0).Rows(0)
        Me.LabelCarton.Text = Carton

        Me.TextBoxScanInfo.Text = "Adding To Carton " & Carton & ".."
        Me.Label_Lot_ID.Text = dr("BoxID").ToString
        Me.LabelUnits.Text = SatiCode.Get_WaferBoxs_Per_ShippingBox(dr("BoxID"))
        Me.LabelBoxQtyMax.Text = Me.LabelUnits.Text
        Me.Label_Spec_Key.Text = dr("Spec_Key").ToString
        Me.Label_Spec.Text = dr("SPEC_NUMBER").ToString
        Me.Label_Spec_Rev.Text = dr("SPEC_REV_NUMBER").ToString
        Me.Label_Part.Text = dr("PART_NUMBER").ToString
        Me.Label_Part_Rev.Text = dr("PART_REV_NUMBER").ToString
        Me.Label_SO_Key.Text = dr("SO_Key").ToString
        Me.Label_SO.Text = dr("SO").ToString
        Me.Label_PO.Text = dr("PO_Number").ToString

        Me.LabelBoxQty.Text = CartonCount
        Me.TextBoxScanInput.Text = ""
        Me.TextBoxScanInput.Focus()

    End Function

    Sub WipeScreen()
        Me.Label_Lot_ID.Text = 0

    End Sub

    Sub Old_TextBoxScan()
        Dim Scan As String = UCase(Me.TextBoxScanInput.Text)
        Dim MySQL As String
        Dim DS As Data.DataSet
        Dim DR As Data.DataRow


        '200mm and less
        If Scan.Contains("W") And Not Scan.Contains("WB") Then

        End If


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

            Me.WaferBoxTextBox.Text = Me.WaferBoxTextBox.Text & Me.TextBoxScanInput.Text & Chr(10)
            Me.TextBoxScanInput.Text = ""
            Me.TextBoxScanInput.Focus()
            Me.LabelBoxQty.Text = Me.LabelBoxQty.Text + 1

            If SatiCode.Get_WaferBoxs_Per_ShippingBox(DR("BoxID")) = Me.LabelBoxQty.Text Then
                MakeTheLabel()
                Me.LabelBoxQty.Text = 0
            End If

        End If
    End Sub


    Protected Sub WaferBoxTextBox_TextChanged(sender As Object, e As EventArgs) Handles WaferBoxTextBox.TextChanged
        Me.TextBoxScanInput.Focus()
    End Sub
    Protected Sub TextBoxScanInput_TextChanged(sender As Object, e As EventArgs) Handles TextBoxScanInput.TextChanged
        Scan_Data()
        Me.TextBoxScanInput.Focus()
    End Sub

    Sub Scan_Data()
        Dim Scan As String = UCase(Me.TextBoxScanInput.Text)
        Dim MySQL As String
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Me.TextBoxScanInput.Focus()
        If Scan = "" Then
            Me.TextBoxScanInput.Focus()
            Exit Sub
        End If


        '********************************************************************************
        '**********************
        'Add To Carton ********
        '**********************

        '200mm and less
        If Scan.Contains("C") And Not Scan.Contains("CB") Then
            GetCartonAddData(Mid(Scan, 2))
            Me.TextBoxScanInput.Focus()
            Exit Sub
        End If

        '300mm or new style
        If Scan.Contains("CB") Then
            GetCartonAddData(Mid(Scan, 3))
            Me.TextBoxScanInput.Focus()
            Exit Sub
        End If
        '**********************************************************************************

        If Not Scan.Contains("W") Or Scan.Contains("C") Then
            Me.TextBoxScanInput.Focus()
            Exit Sub
        End If



        '*******************
        'Make Carton Box****
        '*******************

        '300mm QA Scan
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
                Me.LabelBoxQty.Text = "0"

                Exit Sub
            End If
            '*******************************************
        End If


        DS = GetBoxData(Scan)
        DR = DS.Tables(0).Rows(0)

        If Me.LabelBoxQty.Text = "0" Then
            Me.TextBoxScanInfo.Text = "Building Carton.."
            Me.Label_Lot_ID.Text = DR("BoxID").ToString
            Me.LabelUnits.Text = SatiCode.Get_WaferBoxs_Per_ShippingBox(DR("BoxID"))
            Me.LabelBoxQtyMax.Text = Me.LabelUnits.Text
            Me.Label_Spec_Key.Text = DR("Spec_Key").ToString
            Me.Label_Spec.Text = DR("SPEC_NUMBER").ToString
            Me.Label_Spec_Rev.Text = DR("SPEC_REV_NUMBER").ToString
            Me.Label_Part.Text = DR("PART_NUMBER").ToString
            Me.Label_Part_Rev.Text = DR("PART_REV_NUMBER").ToString
            Me.Label_SO_Key.Text = DR("SO_Key").ToString
            Me.Label_SO.Text = DR("SO").ToString
            Me.Label_PO.Text = DR("PO_Number").ToString
        Else

            If Not Me.Label_Lot_ID.Text = DR("BoxID").ToString Then
                Me.TextBoxScanInfo.Text = Me.TextBoxScanInfo.Text & "Box " & Scan & " was not added. Wrong ID!!"
                Me.TextBoxScanInfo.BackColor = Drawing.Color.LightCoral
                Exit Sub
            End If

            If Not Me.Label_Spec_Key.Text = DR("Spec_Key").ToString Then
                Me.TextBoxScanInfo.Text = Me.TextBoxScanInfo.Text & "Box " & Scan & " was not added. Wrong Spec Key!!"
                Me.TextBoxScanInfo.BackColor = Drawing.Color.LightCoral
                Exit Sub
            End If

            If Not Me.Label_Spec.Text = DR("SPEC_NUMBER").ToString Then
                Me.TextBoxScanInfo.Text = Me.TextBoxScanInfo.Text & "Box " & Scan & " was not added. Wrong Spec Number!!"
                Me.TextBoxScanInfo.BackColor = Drawing.Color.LightCoral
                Exit Sub
            End If

            If Not Me.Label_Spec_Rev.Text = DR("SPEC_REV_NUMBER").ToString Then
                Me.TextBoxScanInfo.Text = Me.TextBoxScanInfo.Text & "Box " & Scan & " was not added. Wrong Spec Rev!!"
                Me.TextBoxScanInfo.BackColor = Drawing.Color.LightCoral
                Exit Sub
            End If

            If Not Me.Label_Part.Text = DR("PART_NUMBER").ToString Then
                Me.TextBoxScanInfo.Text = Me.TextBoxScanInfo.Text & "Box " & Scan & " was not added. Wrong Part Number!!"
                Me.TextBoxScanInfo.BackColor = Drawing.Color.LightCoral
                Exit Sub
            End If

            If Not Me.Label_Part_Rev.Text = DR("PART_REV_NUMBER").ToString Then
                Me.TextBoxScanInfo.Text = Me.TextBoxScanInfo.Text & "Box " & Scan & " was not added. Wrong Part Rev!!"
                Me.TextBoxScanInfo.BackColor = Drawing.Color.LightCoral
                Exit Sub
            End If

            If SatiCode.Find_If_PO_On_Label(Me.Label_Lot_ID.Text) = True Then ' if the PO is on the label
                If Not Me.Label_SO_Key.Text = DR("SO_Key").ToString Then
                    Me.TextBoxScanInfo.Text = Me.TextBoxScanInfo.Text & "Box " & Scan & " was not added. Wrong SO Key!!"
                    Me.TextBoxScanInfo.BackColor = Drawing.Color.LightCoral
                    Exit Sub
                End If

                If Not Me.Label_SO.Text = DR("SO").ToString Then
                    Me.TextBoxScanInfo.Text = Me.TextBoxScanInfo.Text & "Box " & Scan & " was not added. Wrong SO!!"
                    Me.TextBoxScanInfo.BackColor = Drawing.Color.LightCoral
                    Exit Sub
                End If

                If Not Me.Label_PO.Text = DR("PO_Number").ToString Then
                    Me.TextBoxScanInfo.Text = Me.TextBoxScanInfo.Text & "Box " & Scan & " was not added. Wrong PO!!"
                    Me.TextBoxScanInfo.BackColor = Drawing.Color.LightCoral
                    Exit Sub
                End If
            End If


        End If


            Me.WaferBoxTextBox.Text = Me.WaferBoxTextBox.Text & Me.TextBoxScanInput.Text & Chr(10)
        Me.TextBoxScanInput.Text = ""
        Me.TextBoxScanInput.Focus()
        Me.LabelBoxQty.Text = Me.LabelBoxQty.Text + 1

        If Me.LabelUnits.Text = Me.LabelBoxQty.Text Or SatiCode.IS_RFID_Enable(DR("BoxID").ToString) = True Then
            Me.TextBoxScanInfo.Text = "Building Carton.."
            MakeTheLabel()
            AfterPrint()
        End If

        Me.TextBoxScanInput.Focus()
    End Sub

End Class
