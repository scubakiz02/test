
Partial Class Reports_InvFilterExport
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub ButtonGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGo.Click
        RunData()
    End Sub

    Sub RunData()
        If CheckBoxEmail.Checked = True Then
            Select Case Me.TextBoxEmail.Text
                Case ""
                    Me.TextBoxEmail.Text = "Enter Address"
                    Me.TextBoxEmail.BackColor = Drawing.Color.Coral
                    Exit Sub
                Case "Enter Address"
                    Exit Sub
            End Select
            
        End If

        Dim DS_FirstPass As New Data.DataSet
        Dim DR_FirstPass As Data.DataRow

        Dim DS_Rework As New Data.DataSet
        Dim DR_Rework As Data.DataRow

        Dim DS_ShipPending As New Data.DataSet
        Dim DR_ShipPending As Data.DataRow

        Dim DS_Main As New Data.DataSet
        Dim DR_Main As Data.DataRow

        Dim DS_Final As New Data.DataSet
        Dim DR_Final As Data.DataRow

        Dim MySQL As String = ""
        Dim added_Order As Boolean = False

        Dim OffSet As Integer = 0
        Dim LastCustomer As String = ""
        Dim LastDiameter As String = ""


        'Fill Datasets
        DS_FirstPass = SatiCode.GetMyDataSet("SELECT TOP 100 PERCENT ID, InQtyFirst, Management_Area, Management_Area_Index FROM dbo.Q_SATI_INV_By_ManageArea_By_FirstPass")
        DS_Rework = SatiCode.GetMyDataSet("SELECT TOP 100 PERCENT ID, InQtyRework, Management_Area, Management_Area_Index FROM dbo.Q_SATI_INV_By_ManageArea_By_Rework")
        DS_ShipPending = SatiCode.GetMyDataSet("SELECT LEFT(dbo.LabelsMade.Lot, 4) AS LotID, SUM(dbo.ShippingInventory.Total_Qty) AS Qty FROM dbo.T_ShipmentsPending INNER JOIN dbo.ShippingInventory ON dbo.T_ShipmentsPending.PickTicket = dbo.ShippingInventory.PickTicket INNER JOIN dbo.LabelsMade ON dbo.ShippingInventory.LotEntry = dbo.LabelsMade.LabelRecordNumber WHERE (dbo.T_ShipmentsPending.Released = N'No') GROUP BY dbo.T_ShipmentsPending.PickTicket, dbo.T_ShipmentsPending.EventTime, dbo.T_ShipmentsPending.Notes, LEFT(dbo.LabelsMade.Lot, 4) ORDER BY LEFT(dbo.LabelsMade.Lot, 4), dbo.T_ShipmentsPending.PickTicket")


        'DS_Main
        MySQL = "SELECT MainID, CustomerID, Diameter, WHQty, [Final Pack], FGI, [Polish Rework], [Lap Rework], [S&E Rework], [Cleanroom Partials], [Polish Partials] FROM dbo.Q_Sati_Inv_Phase1 ORDER BY "
        added_Order = False

        If CheckBox_Group_Size.Checked = True Then
            If added_Order = True Then
                MySQL = MySQL + ", Diameter"
            Else
                MySQL = MySQL + "Diameter"
            End If
            added_Order = True
        End If

        If CheckBox_Sort_Customer_ID.Checked = True Then
            If added_Order = True Then
                MySQL = MySQL + ", CustomerID"
            Else
                MySQL = MySQL + "CustomerID"
            End If
            added_Order = True
        End If

        If added_Order = True Then
            MySQL = MySQL + ", MainID"
        Else
            MySQL = MySQL + "MainID"
        End If
        DS_Main = SatiCode.GetMyDataSet(MySQL)

        DS_Final.Tables.Add("MyData")
        DS_Final.Tables("MyData").Columns.Add("ID")
        DS_Final.Tables("MyData").Columns.Add("Customer")
        DS_Final.Tables("MyData").Columns.Add("Diameter")
        DS_Final.Tables("MyData").Columns.Add("WHQty")
        DS_Final.Tables("MyData").Columns.Add("FGI")
        DS_Final.Tables("MyData").Columns.Add("Polish Rework")
        DS_Final.Tables("MyData").Columns.Add("Lap Rework")
        DS_Final.Tables("MyData").Columns.Add("S&E Rework")
        DS_Final.Tables("MyData").Columns.Add("Cleanroom Partials")
        DS_Final.Tables("MyData").Columns.Add("Polish Partials")
        DS_Final.Tables("MyData").Columns.Add("Inc_FP")
        DS_Final.Tables("MyData").Columns.Add("Inc_RW")
        DS_Final.Tables("MyData").Columns.Add("SE_FP")
        DS_Final.Tables("MyData").Columns.Add("SE_RW")
        DS_Final.Tables("MyData").Columns.Add("Sort_FP")
        DS_Final.Tables("MyData").Columns.Add("Sort_RW")
        DS_Final.Tables("MyData").Columns.Add("Polish_FP")
        DS_Final.Tables("MyData").Columns.Add("Polish_RW")
        DS_Final.Tables("MyData").Columns.Add("TotalWip")
        DS_Final.Tables("MyData").Columns.Add("TotalInv")
        DS_Final.Tables("MyData").Columns.Add("ShipPending")

        'fill new table with DS_Main
        For i As Integer = 0 To DS_Main.Tables(0).Rows.Count - 1
            DR_Main = DS_Main.Tables(0).Rows(i)
            DR_Final = DS_Final.Tables("MyData").NewRow
            DR_Final("ID") = DR_Main("MainID")
            DR_Final("Customer") = DR_Main("CustomerID")
            DR_Final("Diameter") = DR_Main("Diameter")
            DR_Final("WHQty") = DR_Main("WHQty")
            DR_Final("FGI") = CType(DR_Main("Final Pack"), Integer) + CType(DR_Main("FGI"), Integer)
            DR_Final("Polish Rework") = DR_Main("Polish Rework")
            DR_Final("Lap Rework") = DR_Main("Lap Rework")
            DR_Final("S&E Rework") = DR_Main("S&E Rework")
            DR_Final("Cleanroom Partials") = DR_Main("Cleanroom Partials")
            DR_Final("Polish Partials") = DR_Main("Polish Partials")
            DR_Final("Inc_FP") = 0
            DR_Final("Inc_RW") = 0
            DR_Final("SE_FP") = 0
            DR_Final("SE_RW") = 0
            DR_Final("Sort_FP") = 0
            DR_Final("Sort_RW") = 0
            DR_Final("Polish_FP") = 0
            DR_Final("Polish_RW") = 0
            DR_Final("TotalWip") = 0
            DR_Final("TotalInv") = 0
            DR_Final("ShipPending") = 0

            DS_Final.Tables("MyData").Rows.Add(DR_Final)
        Next

        For i As Integer = 0 To DS_Final.Tables(0).Rows.Count - 1
            DR_Final = DS_Final.Tables(0).Rows(i)
            For PS As Integer = 0 To DS_ShipPending.Tables(0).Rows.Count - 1
                DR_ShipPending = DS_ShipPending.Tables(0).Rows(PS)
                If DR_Final("ID") = DR_ShipPending("LotID") Then

                    DR_Final("ShipPending") = DR_Final("ShipPending") + DR_ShipPending("Qty")

                End If
            Next

        Next

        For i As Integer = 0 To DS_Final.Tables(0).Rows.Count - 1
            DR_Final = DS_Final.Tables(0).Rows(i)

            For FP As Integer = 0 To DS_FirstPass.Tables(0).Rows.Count - 1
                DR_FirstPass = DS_FirstPass.Tables(0).Rows(FP)
                If DR_Final("ID") = DR_FirstPass("ID") Then
                    Select Case DR_FirstPass("Management_Area")
                        Case "Incoming"
                            DR_Final("Inc_FP") = DR_Final("Inc_FP") + DR_FirstPass("InQtyFirst")
                        Case "Strip_Etch"
                            DR_Final("SE_FP") = DR_Final("SE_FP") + DR_FirstPass("InQtyFirst")
                        Case "Presort"
                            DR_Final("Sort_FP") = DR_Final("Sort_FP") + DR_FirstPass("InQtyFirst")
                        Case "Polish"
                            DR_Final("Polish_FP") = DR_Final("Polish_FP") + DR_FirstPass("InQtyFirst")
                    End Select
                End If
            Next

            For RW As Integer = 0 To DS_Rework.Tables(0).Rows.Count - 1
                DR_Rework = DS_Rework.Tables(0).Rows(RW)
                If DR_Final("ID") = DR_Rework("ID") Then
                    Select Case DR_Rework("Management_Area")
                        Case "Incoming"
                            DR_Final("Inc_RW") = DR_Final("Inc_RW") + DR_Rework("InQtyRework")
                        Case "Strip_Etch"
                            DR_Final("SE_RW") = DR_Final("SE_RW") + DR_Rework("InQtyRework")
                        Case "Presort"
                            DR_Final("Sort_RW") = DR_Final("Sort_RW") + DR_Rework("InQtyRework")
                        Case "Polish"
                            DR_Final("Polish_RW") = DR_Final("Polish_RW") + DR_Rework("InQtyRework")
                    End Select
                End If
            Next





            DR_Final("TotalWip") = CType(DR_Final("Inc_FP"), Integer) + CType(DR_Final("Inc_RW"), Integer) + CType(DR_Final("SE_FP"), Integer) + CType(DR_Final("SE_RW"), Integer) + CType(DR_Final("Sort_FP"), Integer) + CType(DR_Final("Sort_RW"), Integer) + CType(DR_Final("Polish_FP"), Integer) + CType(DR_Final("Polish_RW"), Integer)
            DR_Final("TotalInv") = CType(DR_Final("TotalWip"), Integer) + CType(DR_Final("WHQty"), Integer) + CType(DR_Final("FGI"), Integer) + CType(DR_Final("Polish Rework"), Integer) + CType(DR_Final("Lap Rework"), Integer) + CType(DR_Final("S&E Rework"), Integer) + CType(DR_Final("Cleanroom Partials"), Integer) + CType(DR_Final("Polish Partials"), Integer) + CType(DR_Final("ShipPending"), Integer)


        Next
        If Me.CheckBox_Remove_Zero_Lines.Checked = True Then
            Dim RC As Integer
            OffSet = 0
            RC = DS_Final.Tables(0).Rows.Count - 1
            For i As Integer = 0 To RC
                DR_Final = DS_Final.Tables(0).Rows(i - OffSet)
                If CType(DR_Final("TotalInv"), Integer) = 0 Then
                    DR_Final.Delete()
                    OffSet = OffSet + 1
                End If
            Next
        End If

        If Me.CheckBox_Remove_Zero_FGI.Checked = True Then
            Dim RC As Integer
            OffSet = 0
            RC = DS_Final.Tables(0).Rows.Count - 1
            For i As Integer = 0 To RC
                DR_Final = DS_Final.Tables(0).Rows(i - OffSet)
                If CType(DR_Final("FGI"), Integer) = 0 And DR_Final("ShipPending") = 0 Then 'added ship pending to logic
                    DR_Final.Delete()
                    OffSet = OffSet + 1
                End If
            Next
        End If

        'Me.GridView1.DataSource = DS_Final
        'Me.GridView1.DataBind()


        Dim Flex As New FlexCel.XlsAdapter.XlsFile(True)

        Dim Path As String
        Path = "\\PWI-40\software$\LabelTemplates\Sati_InvReport.xls"
        Flex.Open(Path)
        Flex.ActiveSheetByName = "InvReport"

        OffSet = 0
        For i As Integer = 0 To DS_Final.Tables(0).Rows.Count - 1
            DR_Final = DS_Final.Tables(0).Rows(i)

            If Me.CheckBox_Group_Size.Checked = True Then
                If Not LastDiameter = "" Then
                    If Not LastDiameter = DR_Final("Diameter") Then
                        OffSet = OffSet + 1
                    End If
                End If
            End If

            If Me.CheckBox_Sort_Customer_ID.Checked = True Then
                If Not LastCustomer = "" Then
                    If Not LastCustomer = DR_Final("Customer") Then
                        OffSet = OffSet + 1
                    End If
                End If
            End If
            Flex.SetCellValue(3 + i + OffSet, 1, DR_Final("ID"))
            Flex.SetCellValue(3 + i + OffSet, 2, CType(DR_Final("Diameter"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 3, DR_Final("Customer"))
            Flex.SetCellValue(3 + i + OffSet, 4, CType(DR_Final("WHQty"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 5, CType(DR_Final("Inc_FP"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 6, CType(DR_Final("Inc_RW"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 7, CType(DR_Final("SE_FP"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 8, CType(DR_Final("SE_RW"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 9, CType(DR_Final("Sort_FP"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 10, CType(DR_Final("Sort_RW"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 11, CType(DR_Final("Polish_FP"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 12, CType(DR_Final("Polish_RW"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 13, CType(DR_Final("TotalWip"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 14, CType(DR_Final("FGI"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 15, CType(DR_Final("ShipPending"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 16, CType(DR_Final("Polish Rework"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 17, CType(DR_Final("Lap Rework"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 18, CType(DR_Final("S&E Rework"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 19, CType(DR_Final("Polish Partials"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 20, CType(DR_Final("Cleanroom Partials"), Integer))
            Flex.SetCellValue(3 + i + OffSet, 21, CType(DR_Final("TotalInv"), Integer))
            LastCustomer = DR_Final("Customer")
            LastDiameter = DR_Final("Diameter")
        Next


        'Dim ReportName As String = "\\PWI-40\software$\LabelTemplates\LabelArchive\InvReportHold\InvReport " & User.Identity.Name.ToString & ".xls"
        Dim SaveDir As String = "\\PWI-40\TempImageWebFiles$\"
        Dim FileName As String = "InvReport2 " & User.Identity.Name.ToString & ".xls"

        Flex.Save(SaveDir & FileName) 'LabelTemplates\LabelArchive\InvReportHold

        Me.HyperLinkReport.Visible = True
        Me.HyperLinkReport.NavigateUrl = Session("ReportFolder") & FileName

        If Me.CheckBoxEmail.Checked = True Then
            If Not Me.TextBoxEmail.Text = "" Then
                SatiCode.SendMailWithFile("Inv Report From " & User.Identity.Name.ToString, "SATI.Net Inv Report", Me.TextBoxEmail.Text & "@purewafer.com", SaveDir & FileName)
            End If
        End If

    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub
End Class
