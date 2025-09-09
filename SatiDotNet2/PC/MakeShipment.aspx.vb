Imports Class1
Imports FlexCel


Partial Class PC_MakeShipment
    Inherits System.Web.UI.Page
    'Inherits System.Web.UI.MasterPage
    Dim Saticode As New Class1
    Dim Warn As Int16 = 0
    Dim CrossFabShip As Boolean
    Dim OrgID As String = ""


    Function GetMyDataSet(ByVal SQLString As String) As Data.DataSet
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = SQLString
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        'DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T7_InstanceInfo", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("InstanceID", "InstanceID"), New System.Data.Common.DataColumnMapping("Slot", "Slot"), New System.Data.Common.DataColumnMapping("T7", "T7"), New System.Data.Common.DataColumnMapping("Seq", "Seq")})})

        DA.Fill(DS)
        Connection.Close()
        GetMyDataSet = DS
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        MenuAuthenication.CheckGroupAuthenication("Shipping", Server)

        If Not Page.IsPostBack = True Then
            CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.PickTicketTextBox.ClientID)
            Me.PickTicketTextBox.Focus()
        End If

        'If Me.PickTicketTextBox.Enabled = True Then


        'End If

        If Warn > 0 Then
            Me.WarnLabel.Visible = True
        End If
    End Sub

    Protected Sub PickTicketTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PickTicketTextBox.TextChanged
        'Me.PickTicketTextBox.Text = UCase(Me.PickTicketTextBox.Text)
        'CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.TextBoxShipQty.ClientID)
        Me.TextBoxShipQty.Focus()

    End Sub

    Protected Sub TextBoxShipQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxShipQty.TextChanged
        If Me.TextBoxShipQty.Text = "" Or Me.TextBoxShipQty.Text = 0 Then
            Me.TextBoxShipQty.Focus()
            Exit Sub
        End If

        If GetPickTicketInfo(UCase(Me.PickTicketTextBox.Text), Me.TextBoxShipQty.Text) = True Then
            OrgID = ""
            LockDown(1, True)
            LockDown(2, True)
            Me.QtyLeftLabel.Text = Me.TextBoxShipQty.Text

            CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.CartonScanTextBox.ClientID)
        End If
    End Sub

    Sub LockDown(ByVal Zone As Int16, ByVal Lock As Boolean)
        Select Case Zone
            Case 1
                If Lock = True Then
                    Me.PickTicketTextBox.Enabled = False
                    Me.Step1Label.BackColor = Drawing.Color.LightGreen
                Else
                    Me.PickTicketTextBox.Enabled = True
                    Me.PickTicketTextBox.Text = ""
                    Me.Step1Label.BackColor = Drawing.Color.White
                End If

            Case 2
                If Lock = True Then
                    Me.Step2Label.BackColor = Drawing.Color.LightGreen
                Else
                    Me.Step2Label.BackColor = Drawing.Color.White
                End If

            Case 3
                If Lock = True Then
                    Me.Step3Label.BackColor = Drawing.Color.LightGreen
                    Me.GOStep4Button.Visible = True
                Else
                    Me.Step3Label.BackColor = Drawing.Color.White
                End If
            Case 4

        End Select
    End Sub

    Function GetPickTicketInfo(ByVal PickTicket As String, ShippingQty As String) As Boolean
        'Dim Connection As New Data.SqlClient.SqlConnection
        'Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        'Connection.Open()

        'Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DS2 As New Data.DataSet
        Dim DR As Data.DataRow
        Dim DR2 As Data.DataRow

        Try
            'Dim SelectCmd As New System.Data.SqlClient.SqlCommand
            'With SelectCmd
            '    .CommandText = "SELECT dbo.SalesSchedule.PickTicket, dbo.MainID.MainID, dbo.MainID.Diameter, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.MainID.PO_On_Label, dbo.LabelTemplatesDotNet.Template AS ShippingTemplate, dbo.CofA_Info.COFA FROM dbo.SalesSchedule LEFT OUTER JOIN dbo.LabelTemplatesDotNet INNER JOIN dbo.MainID_Template ON dbo.LabelTemplatesDotNet.[Key] = dbo.MainID_Template.Template_Key ON dbo.SalesSchedule.ID = dbo.MainID_Template.MainID LEFT OUTER JOIN dbo.CofA_Info ON dbo.SalesSchedule.ID = dbo.CofA_Info.ID_NUMBER LEFT OUTER JOIN dbo.SO_Info INNER JOIN dbo.SO_LineItems ON dbo.SO_Info.SO = dbo.SO_LineItems.SO ON dbo.SalesSchedule.SO_Key = dbo.SO_LineItems.[Key] LEFT OUTER JOIN dbo.MainID ON dbo.SalesSchedule.ID = dbo.MainID.MainID WHERE (dbo.SalesSchedule.PickTicket = N'" & PickTicket & "') AND (dbo.MainID_Template.Template_Type = N'CL')"
            '    .Connection = Connection
            'End With
            'DA.SelectCommand = SelectCmd
            'DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "CofA_Info", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("PickTicket", "PickTicket"), New System.Data.Common.DataColumnMapping("MainID", "MainID"), New System.Data.Common.DataColumnMapping("Diameter", "Diameter"), New System.Data.Common.DataColumnMapping("SO", "SO"), New System.Data.Common.DataColumnMapping("PO_Number", "PO_Number"), New System.Data.Common.DataColumnMapping("PO_On_Label", "PO_On_Label"), New System.Data.Common.DataColumnMapping("ShippingTemplate", "ShippingTemplate"), New System.Data.Common.DataColumnMapping("COFA", "COFA")})})
            'DA.Fill(DS)
            'DR = DS.Tables(0).Rows(0)

            DS = Saticode.GetMyDataSet("SELECT dbo.SalesSchedule.PickTicket, dbo.MainID.MainID, dbo.MainID.Diameter, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.MainID.PO_On_Label, dbo.LabelTemplatesDotNet.Template AS ShippingTemplate, dbo.CofA_Info.COFA FROM dbo.SalesSchedule LEFT OUTER JOIN dbo.LabelTemplatesDotNet INNER JOIN dbo.MainID_Template ON dbo.LabelTemplatesDotNet.[Key] = dbo.MainID_Template.Template_Key ON dbo.SalesSchedule.ID = dbo.MainID_Template.MainID LEFT OUTER JOIN dbo.CofA_Info ON dbo.SalesSchedule.ID = dbo.CofA_Info.ID_NUMBER LEFT OUTER JOIN dbo.SO_Info INNER JOIN dbo.SO_LineItems ON dbo.SO_Info.SO = dbo.SO_LineItems.SO ON dbo.SalesSchedule.SO_Key = dbo.SO_LineItems.[Key] LEFT OUTER JOIN dbo.MainID ON dbo.SalesSchedule.ID = dbo.MainID.MainID WHERE (dbo.SalesSchedule.PickTicket = N'" & PickTicket & "') AND (dbo.MainID_Template.Template_Type = N'CL')")
            DR = DS.Tables(0).Rows(0)

            Me.Panel2.Visible = True
            Me.IDLabel.Text = DR("MainID")
            OrgID = DR("MainID")
            Me.LabelOrgIDID.Text = OrgID
            Me.DiameterLabel.Text = DR("Diameter")
            Me.SoLabel.Text = DR("SO")
            Me.POLabel.Text = DR("PO_Number")
            Me.POonLabelLabel.Text = DR("PO_On_Label")
            Me.ShippingTemplateLabel.Text = DR("ShippingTemplate")
            Me.CofATemplateLabel.Text = DR("COFA")


            'Check SO balance
            DS2 = Saticode.Get_Quick_SO_Info(DR("SO"))
            DR2 = DS2.Tables(0).Rows(0)
            If DR2("Balance") < ShippingQty Then
                LoadReport()
                GetPickTicketInfo = False
                'Low SO Balance, View Report
                Exit Function
            End If


            GetPickTicketInfo = True
            Me.Panel3.Visible = True

            If CheckDupeHistoryEnabled(DR("MainID")) = True Then
                Me.LabelDupeCheckEnabel.Text = "Enabled"
            Else
                Me.LabelDupeCheckEnabel.Text = "Disabled"
            End If


            'Me.ScriptManager1.SetFocus(Me.CartonScanTextBox.ClientID)
            CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.CartonScanTextBox.ClientID)


        Catch ex As Exception
            GetPickTicketInfo = False
        End Try

        'Connection.Close()




    End Function

    Sub LoadReport()

        Dim ReportName As String
        ReportName = Saticode.Make_Current_SO_Report()
        Me.HyperLinkReport.Visible = True
        Me.HyperLinkReport.NavigateUrl = Session("ReportFolder") & ReportName

    End Sub



    Function CheckDupeHistoryEnabled(ByVal ID As String) As Boolean

        'Update with mainID table later
        ' For now using manual list

        Select Case ID
            Case "3262"
                Return True

            Case "3264"
                Return True

            Case "3265"
                Return True

            Case "3212"
                Return True

            Case "3604"
                Return True

            Case Else
                Return False

        End Select


    End Function

    Sub ClearForm()
        LockDown(1, False)
        Panel2.Visible = False

        LockDown(2, False)
        Panel3.Visible = False

        LockDown(3, False)
        Panel4.Visible = False

        Me.GOStep4Button.Visible = False
        Me.CartonsAddedTextBox.ReadOnly = False
        Me.CartonsAddedTextBox.Text = ""
        Me.CartonsAddedTextBox.ReadOnly = True
    End Sub

    Protected Sub CartonScanTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CartonScanTextBox.TextChanged
        Dim Carton As String
        'Me.CartonScanTextBox.Text = UCase(Me.CartonScanTextBox.Text)
        Carton = UCase(Me.CartonScanTextBox.Text)
        If Me.CartonScanTextBox.Text = "" Then
            'Me.CartonScanTextBox.Focus()
            Me.Page.SetFocus(CartonScanTextBox)
            Exit Sub
        End If

        Dim CartonReport As String

        'If Saticode.GetCustomerName(Me.LabelOrgIDID.Text) = "GLOBALFOUNDRIES" Then
        'If Carton.Contains("WB") Then
        'Make the Carton from the WB scan
        'Return the Caron ID then add it to shipment
        'Carton = Saticode.MakeCarton(UCase(Carton), "", True)
        'End If
        'End If

        If Carton.Contains("CB") Then
            Carton = Mid(Carton, 3)
            If Not Me.CartonsAddedTextBox.Text.Contains(Carton) Then
                CartonReport = CartonAdd(Carton)
            Else
                Me.CartonsAddedTextBox.Text = Carton & " All Ready In Shipment" & Chr(13) & Me.CartonsAddedTextBox.Text
                CartonReport = "Good"
            End If
        End If

        If Carton.Contains("C") Then
            Carton = Mid(Carton, 2)
            If Not Me.CartonsAddedTextBox.Text.Contains(Carton) Then
                CartonReport = CartonAddBulk(Carton)
            Else
                Me.CartonsAddedTextBox.Text = Carton & " All Ready In Shipment" & Chr(13) & Me.CartonsAddedTextBox.Text
                CartonReport = "Good"
            End If
            Me.LabelBulk.Visible = True
        End If

        If Not CartonReport = "Good" Then
            Me.ErrorInfoLabel.Visible = True
            Me.ErrorInfoLabel.Text = CartonReport
            Me.CartonsAddedTextBox.ReadOnly = False
            Me.CartonsAddedTextBox.Text = "Carton " & Carton & " " & CartonReport & Chr(13) & Me.CartonsAddedTextBox.Text
            Me.CartonsAddedTextBox.ReadOnly = True
        End If


        Me.CartonScanTextBox.Text = ""
        'Me.ScriptManager1.SetFocus(Me.CartonScanTextBox.ClientID)
        'CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.CartonScanTextBox.ClientID)
        'Me.CartonScanTextBox.Focus()
        Me.Page.SetFocus(CartonScanTextBox)
    End Sub

    Function CartonAddBulk(ByVal carton As String) As String
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()
        CartonAddBulk = ""
        'Me.ErrorInfoLabel.Visible = False
        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim RowCount As Int16
        Dim Qty As Integer
        Dim CartonQty As Integer = 0
        Dim CartonCount As Int16
        Try
            Dim SelectCmd As New System.Data.SqlClient.SqlCommand
            With SelectCmd
                .CommandText = "SELECT dbo.ShippingInventory.Carton_Key, dbo.LabelsMade.Lot, dbo.LabelsMade.Wafers AS Qty, dbo.MainIDSpec.PART_NUMBER AS Part, dbo.MainIDSpec.SPEC_NUMBER AS Spec, dbo.MainIDSpec.SPEC_REV_NUMBER AS SpecRev, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number AS PO, LEFT(dbo.LabelsMade.Lot, 4) AS LotID, dbo.MainID.CustomerID, dbo.LabelsMade.LabelRecordNumber FROM dbo.MainIDSpec INNER JOIN dbo.LabelsMade ON dbo.MainIDSpec.RecordNumber = dbo.LabelsMade.RecordNumber INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO INNER JOIN dbo.MainID_MainIDSpec ON dbo.MainIDSpec.RecordNumber = dbo.MainID_MainIDSpec.WaferSpec_Key INNER JOIN dbo.MainID ON dbo.MainID_MainIDSpec.MainID = dbo.MainID.MainID INNER JOIN dbo.ShippingInventory ON dbo.LabelsMade.LabelRecordNumber = dbo.ShippingInventory.LotEntry WHERE (dbo.ShippingInventory.Carton_Key = " & carton & ")"
                .Connection = Connection
            End With
            DA.SelectCommand = SelectCmd
            DA.Fill(DS)
            RowCount = DS.Tables(0).Rows.Count

            If RowCount = 0 Then
                Connection.Close()
                CartonAddBulk = "Could Not Find Data"
                Exit Function
            End If

            For i As Int16 = 0 To RowCount - 1
                DR = DS.Tables(0).Rows(i)
                'do first set of checks

                'Check ID
                If Not Me.IDLabel.Text = DR("LotID").ToString Then
                    If Me.QtyAddedLabel.Text = 0 Then
                        Me.IDLabel.Text = DR("LotID").ToString
                    Else
                        Return "Wrong ID This Is " & DR("LotID").ToString
                    End If

                End If


                If Me.POonLabelLabel.Text = "True" Then
                    'SO
                    If Not Me.SoLabel.Text = DR("SO").ToString Then
                        Return "Has SO Issue"
                    End If

                    'PO
                    If Not Me.POLabel.Text = DR("PO").ToString Then
                        Return "Has PO Issue"
                    End If
                End If

                'make sure carton is in Inventory

                'Spec
                If Not Me.SpecNumberLabel.Text = "" Then
                    If Not Me.SpecNumberLabel.Text = DR("Spec") Then
                        Return "Has Spec Issue"
                    End If
                Else
                    Me.SpecNumberLabel.Text = DR("Spec").ToString
                End If

                'Spec Rev
                If Not Me.SpecRevLabel.Text = "" Then
                    If Not Me.SpecRevLabel.Text = DR("SpecRev") Then
                        Return "Has Spec Rev Issue"
                    End If
                Else
                    Me.SpecRevLabel.Text = DR("SpecRev").ToString
                End If

                'Part Number
                If Not Me.PartNumberLabel.Text = "" Then
                    If Not Me.PartNumberLabel.Text = DR("Part") Then
                        Return "Has Part Number Issue"
                    End If
                Else
                    Me.PartNumberLabel.Text = DR("Part").ToString
                End If


                Qty = Me.QtyAddedLabel.Text + DR("Qty")
                Me.QtyAddedLabel.Text = Qty

                Qty = Me.QtyLeftLabel.Text - DR("Qty")
                Me.QtyLeftLabel.Text = Qty

                If Me.QtyLeftLabel.Text = "0" Then
                    Me.PalletCountDropDownList.Enabled = True
                Else
                    Me.PalletCountDropDownList.Enabled = False
                End If

                CartonQty = CartonQty + DR("Qty")

                CartonAddBulk = "Good"
                Me.CartonsAddedTextBox.ReadOnly = False

                Dim TheCustomer As String = DR("CustomerID").ToString

                TheCustomer = UCase(TheCustomer)

                Me.CartonsAddedTextBox.Text = "Added, WB" & DR("LabelRecordNumber").ToString & ", Lot# " & DR("Lot").ToString & ", Qty= +" & DR("Qty") & Chr(13) & Me.CartonsAddedTextBox.Text
                Me.CartonsAddedTextBox.ReadOnly = True

            Next
            Me.CartonsAddedTextBox.Text = "Good Scan Carton " & carton & ", Qty= +" & CartonQty & Chr(13) & Me.CartonsAddedTextBox.Text
            CartonCount = Me.CartonCountLabel.Text + 1
            Me.CartonCountLabel.Text = CartonCount

            Me.CartonScanTextBox.Text = ""

            'CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.CartonScanTextBox.ClientID)
            'Me.CartonScanTextBox.Focus()
            Me.Page.SetFocus(CartonScanTextBox)
        Catch ex As Exception
            CartonAddBulk = "Error"
        End Try

        Connection.Close()
        'Me.CartonScanTextBox.Focus()
        Me.Page.SetFocus(CartonScanTextBox)
    End Function

    Function CartonAdd(ByVal carton As String) As String


        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()
        CartonAdd = ""
        'Me.ErrorInfoLabel.Visible = False
        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim RowCount As Int16
        Dim Style As String
        Dim SlotData As Boolean
        Dim Qty As Integer
        Dim CartonCount As Int16
        Dim TheCustomer As String
        Dim DataScan As String = ""
        Dim DropOut As Boolean = False
        Dim DupeCheck As String = ""
        Dim CartonQty As Integer = 0

        Try
            'SELECT dbo.T_FGI_Boxes.CartonNumber, dbo.LabelsMade.Lot, dbo.LabelsMade.Wafers AS Qty, dbo.MainIDSpec.PART_NUMBER AS Part, dbo.MainIDSpec.SPEC_NUMBER AS Spec, dbo.MainIDSpec.SPEC_REV_NUMBER AS SpecRev, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number AS PO, dbo.T_FGI_Boxes.InstanceKey AS Ikey, LEFT(dbo.LabelsMade.Lot, 4) AS LotID, dbo.MainID.CustomerID, dbo.T_FGI_Boxes.BoxInvNumber AS WB_number FROM dbo.MainIDSpec INNER JOIN dbo.LabelsMade ON dbo.MainIDSpec.RecordNumber = dbo.LabelsMade.RecordNumber INNER JOIN dbo.T_FGI_Boxes ON dbo.LabelsMade.LabelRecordNumber = dbo.T_FGI_Boxes.LabelsMadeKey INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO INNER JOIN dbo.MainID_MainIDSpec ON dbo.MainIDSpec.RecordNumber = dbo.MainID_MainIDSpec.WaferSpec_Key INNER JOIN dbo.MainID ON dbo.MainID_MainIDSpec.MainID = dbo.MainID.MainID WHERE (dbo.T_FGI_Boxes.CartonNumber = 290418)
            DS = Saticode.GetMyDataSet("SELECT dbo.T_FGI_Boxes.CartonNumber, dbo.LabelsMade.Lot, dbo.LabelsMade.Wafers AS Qty, dbo.MainIDSpec.PART_NUMBER AS Part, dbo.MainIDSpec.SPEC_NUMBER AS Spec, dbo.MainIDSpec.SPEC_REV_NUMBER AS SpecRev, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number AS PO, dbo.T_FGI_Boxes.InstanceKey AS Ikey, LEFT(dbo.LabelsMade.Lot, 4) AS LotID, dbo.MainID.CustomerID, dbo.T_FGI_Boxes.BoxInvNumber AS WB_number FROM dbo.MainIDSpec INNER JOIN dbo.LabelsMade ON dbo.MainIDSpec.RecordNumber = dbo.LabelsMade.RecordNumber INNER JOIN dbo.T_FGI_Boxes ON dbo.LabelsMade.LabelRecordNumber = dbo.T_FGI_Boxes.LabelsMadeKey INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO INNER JOIN dbo.MainID_MainIDSpec ON dbo.MainIDSpec.RecordNumber = dbo.MainID_MainIDSpec.WaferSpec_Key INNER JOIN dbo.MainID ON dbo.MainID_MainIDSpec.MainID = dbo.MainID.MainID WHERE (dbo.T_FGI_Boxes.CartonNumber = " & carton & ")")
            Dim SelectCmd As New System.Data.SqlClient.SqlCommand
            'With SelectCmd '
            '.CommandText = "SELECT dbo.T_FGI_Boxes.CartonNumber, dbo.LabelsMade.Lot, dbo.LabelsMade.Wafers AS Qty, dbo.MainIDSpec.PART_NUMBER AS Part, dbo.MainIDSpec.SPEC_NUMBER AS Spec, dbo.MainIDSpec.SPEC_REV_NUMBER AS SpecRev, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number AS PO, dbo.T_FGI_Boxes.InstanceKey AS Ikey, LEFT(dbo.LabelsMade.Lot, 4) AS LotID, dbo.MainID.CustomerID FROM dbo.MainIDSpec INNER JOIN dbo.LabelsMade ON dbo.MainIDSpec.RecordNumber = dbo.LabelsMade.RecordNumber INNER JOIN dbo.T_FGI_Boxes ON dbo.LabelsMade.LabelRecordNumber = dbo.T_FGI_Boxes.LabelsMadeKey INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO INNER JOIN dbo.MainID_MainIDSpec ON dbo.MainIDSpec.RecordNumber = dbo.MainID_MainIDSpec.WaferSpec_Key INNER JOIN dbo.MainID ON dbo.MainID_MainIDSpec.MainID = dbo.MainID.MainID WHERE (dbo.T_FGI_Boxes.CartonNumber = " & carton & ")"
            '.CommandText = "SELECT dbo.T_FGI_Boxes.CartonNumber, dbo.LabelsMade.Lot, dbo.LabelsMade.Wafers AS Qty, dbo.MainIDSpec.PART_NUMBER AS Part, dbo.MainIDSpec.SPEC_NUMBER AS Spec, dbo.MainIDSpec.SPEC_REV_NUMBER AS SpecRev, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number AS PO, dbo.T_FGI_Boxes.InstanceKey AS Ikey, LEFT(dbo.LabelsMade.Lot, 4) AS LotID FROM dbo.MainIDSpec INNER JOIN dbo.LabelsMade ON dbo.MainIDSpec.RecordNumber = dbo.LabelsMade.RecordNumber INNER JOIN dbo.T_FGI_Boxes ON dbo.LabelsMade.LabelRecordNumber = dbo.T_FGI_Boxes.LabelsMadeKey INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO WHERE (dbo.T_FGI_Boxes.CartonNumber = " & carton & ")"
            '.Connection = Connection
            'End With
            'DA.SelectCommand = SelectCmd
            'DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "LabelsMade", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("CartonNumber", "CartonNumber"), New System.Data.Common.DataColumnMapping("Lot", "Lot"), New System.Data.Common.DataColumnMapping("Qty", "Qty"), New System.Data.Common.DataColumnMapping("Part", "Part"), New System.Data.Common.DataColumnMapping("Spec", "Spec"), New System.Data.Common.DataColumnMapping("SpecRev", "SpecRev"), New System.Data.Common.DataColumnMapping("SO", "SO"), New System.Data.Common.DataColumnMapping("PO", "PO"), New System.Data.Common.DataColumnMapping("Ikey", "Ikey"), New System.Data.Common.DataColumnMapping("LotID", "LotID"), New System.Data.Common.DataColumnMapping("CustomerID", "CustomerID")})})
            'DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "LabelsMade", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("CartonNumber", "CartonNumber"), New System.Data.Common.DataColumnMapping("Lot", "Lot"), New System.Data.Common.DataColumnMapping("Qty", "Qty"), New System.Data.Common.DataColumnMapping("Part", "Part"), New System.Data.Common.DataColumnMapping("Spec", "Spec"), New System.Data.Common.DataColumnMapping("SpecRev", "SpecRev"), New System.Data.Common.DataColumnMapping("SO", "SO"), New System.Data.Common.DataColumnMapping("PO", "PO"), New System.Data.Common.DataColumnMapping("Ikey", "Ikey"), New System.Data.Common.DataColumnMapping("LotID", "LotID")})})
            'DA.Fill(DS)
            RowCount = DS.Tables(0).Rows.Count
            If RowCount = 0 Then
                Style = "Old"
            Else
                Style = "New"
                SlotData = True
            End If

            For i As Int16 = 0 To RowCount - 1
                DR = DS.Tables(0).Rows(i)
                'do first set of checks

                'Check ID
                If Not Me.IDLabel.Text = DR("LotID").ToString Then
                    If Me.QtyAddedLabel.Text = 0 Then
                        Me.IDLabel.Text = DR("LotID").ToString
                    Else
                        Return "Wrong ID This Is " & DR("LotID").ToString
                    End If

                End If


                If Me.POonLabelLabel.Text = "True" Then
                    'SO
                    Dim SORec As String = DR("SO").ToString
                    If Not Me.SoLabel.Text = SORec Then
                        Return "Has SO Issue"
                    End If

                    'PO
                    If Not Me.POLabel.Text = DR("PO").ToString Then
                        Return "Has PO Issue"
                    End If
                End If

                'make sure carton is in Inventory

                'Spec
                If Not Me.SpecNumberLabel.Text = "" Then
                    If Not Me.SpecNumberLabel.Text = DR("Spec") Then
                        Return "Has Spec Issue"
                    End If
                Else
                    Me.SpecNumberLabel.Text = DR("Spec").ToString
                End If

                'Spec Rev
                If Not Me.SpecRevLabel.Text = "" Then
                    If Not Me.SpecRevLabel.Text = DR("SpecRev") Then
                        Return "Has Spec Rev Issue"
                    End If
                Else
                    Me.SpecRevLabel.Text = DR("SpecRev").ToString
                End If

                'Part Number
                If Not Me.PartNumberLabel.Text = "" Then
                    If Not Me.PartNumberLabel.Text = DR("Part") Then
                        Return "Has Part Number Issue"
                    End If
                Else
                    Me.PartNumberLabel.Text = DR("Part").ToString
                End If


                Qty = Me.QtyAddedLabel.Text + DR("Qty")
                Me.QtyAddedLabel.Text = Qty

                Qty = Me.QtyLeftLabel.Text - DR("Qty")
                Me.QtyLeftLabel.Text = Qty

                If Me.QtyLeftLabel.Text = "0" Then
                    Me.PalletCountDropDownList.Enabled = True
                Else
                    Me.PalletCountDropDownList.Enabled = False
                End If

                CartonAdd = "Good"
                Me.CartonsAddedTextBox.ReadOnly = False



                'Cut**************************************************************

                '***************** MICRON MISSING DATA SCAN *********************************
                '******************* IBM MISSING DATA SCAN **********************************
                '********************** Intel Dupe Check ************************************
                '******************* History Dupe Check for ID ******************************
                '****************** Check T7 Requirements ***********************************

                TheCustomer = DR("CustomerID").ToString
                DataScan = ""
                DropOut = False
                DupeCheck = ""

                TheCustomer = UCase(TheCustomer)

                'Missing data

                If TheCustomer.Contains("MICRON") Or TheCustomer.Contains("IBM") Or TheCustomer.Contains("GF") Then

                    DataScan = Saticode.CBFullDataRecordCheck(carton, "All")
                    If Not DataScan = "No Problems" Then
                        If Saticode.CB_CheckAndFix_Geo(carton) = 0 Then 'fixes any pre data
                            Warn = Warn + 1
                            Me.WarnLabel.Visible = True
                        Else
                            DataScan = Saticode.CBFullDataRecordCheck(carton, "All")
                            If Not DataScan = "No Problems" Then
                                Warn = Warn + 1
                                Me.WarnLabel.Visible = True
                            End If
                        End If
                    End If
                End If


                If TheCustomer.Contains("IBM") Or TheCustomer.Contains("GF") Then
                    'Check to make sure the FGI box was verified
                    If Saticode.FGI_RFID_Checked(carton) = False Then
                        Me.ErrorInfoLabel.Visible = True
                        Me.ErrorInfoLabel.Text = "Carton Not Verified"
                        Return "Box RFID Not Verified"
                    End If

                    'check for 3212 scribes

                End If


                Dim Whitelist As Boolean = False
                If Me.IDLabel.Text = "6610" Then
                    Whitelist = True
                End If


                'Dupe check
                If TheCustomer.Contains("INTEL") Then
                    'If Not TheCustomer = ("INTEL-CHINA") Then
                    DupeCheck = Saticode.CheckSeqDupe("I", DR("Ikey").ToString, Whitelist)
                    If Not DupeCheck = "" Then
                        DropOut = True
                    End If
                    'End If
                End If
                Dim MyReqID As String = DR("LotID").ToString
                Dim MyReqIKey As String = DR("Ikey")

                'Check for T7 requirements
                Dim CheckT7Requirement As String = Saticode.Check_For_T7_Requierments(MyReqID, MyReqIKey)
                If Not CheckT7Requirement = "Good" Then
                    Me.ErrorInfoLabel.Visible = True
                    Me.ErrorInfoLabel.Text = CheckT7Requirement & ". Rerun under Instance Number " & DR("Ikey").ToString & " and remove bad wafer."
                    Return CheckT7Requirement
                End If



                If DropOut = False Then
                    Me.CartonsAddedTextBox.Text = "Added, WB" & DR("WB_number").ToString & ", Lot# " & DR("Lot").ToString & ", Qty= +" & DR("Qty") & Chr(13) & Me.CartonsAddedTextBox.Text
                    CartonQty = CartonQty + DR("Qty")


                Else
                    Me.CartonsAddedTextBox.Text = "Dupe Check Function Returned " & DupeCheck & " For Carton " & carton
                    Me.ErrorInfoLabel.Visible = True
                    Me.ErrorInfoLabel.Text = "Error, Remove Carton And Start Over"
                    Exit For
                End If

                If TheCustomer.Contains("MICRON") Or TheCustomer.Contains("IBM") Or TheCustomer.Contains("GF") Then
                    Me.CartonsAddedTextBox.Text = "Pre Data Scan For " & carton & " Found " & DataScan & Chr(13) & Me.CartonsAddedTextBox.Text
                End If


                Me.CartonsAddedTextBox.ReadOnly = True


                'Cut End ********************************************************************************

            Next

            If DropOut = False Then
                'Me.CartonsAddedTextBox.Text = "Good Scan Carton " & carton & ", Lot# " & DR("Lot").ToString & ", Qty= +" & DR("Qty") & Chr(13) & Me.CartonsAddedTextBox.Text
                Me.CartonsAddedTextBox.Text = "Good Scan Carton " & carton & ", Qty= +" & CartonQty & Chr(13) & Me.CartonsAddedTextBox.Text
            End If

            If Not RowCount = 0 Then
                CartonCount = Me.CartonCountLabel.Text + 1
                Me.CartonCountLabel.Text = CartonCount
            Else
                CartonAdd = "Could Not Find Data"
            End If


            Me.CartonScanTextBox.Text = ""
            'Me.ScriptManager1.SetFocus(Me.CartonScanTextBox.ClientID)
            'CType(Page.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.CartonScanTextBox.ClientID)
            Me.Page.SetFocus(CartonScanTextBox)
        Catch ex As Exception
            CartonAdd = "Error"
        End Try

        Connection.Close()
        'Me.CartonScanTextBox.Focus()
        Me.Page.SetFocus(CartonScanTextBox)

    End Function


    Protected Sub PalletCountDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PalletCountDropDownList.SelectedIndexChanged
        Me.GOStep4Button.Visible = True
    End Sub

    Protected Sub GOStep4Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GOStep4Button.Click
        CheckForCrossFabShip()
        Me.Panel4.Visible = True
        LockDown(3, True)
    End Sub

    Sub MakeTheShipment()
        Dim HadError As Boolean = False
        Dim CartonCount As Integer
        Dim PickID As String
        Dim TheID As String
        Dim CartonString As String
        Dim ShippingAddress As Data.DataSet
        Dim BillingAddress As Data.DataSet
        CartonCount = CartonCountLabel.Text
        PickID = UCase(Me.PickTicketTextBox.Text)
        OrgID = Me.LabelOrgIDID.Text
        TheID = Me.IDLabel.Text

        If Me.LabelCrossFab.Text = "Yes" Then '****Cross Fab*********
            ShippingAddress = Saticode.GetAddress("Shipping", OrgID, "")
            BillingAddress = Saticode.GetAddress("Billing", OrgID, "")
        Else
            ShippingAddress = Saticode.GetAddress("Shipping", TheID, "")
            BillingAddress = Saticode.GetAddress("Billing", TheID, "")
        End If

        If LabelBulk.Visible = True Then
            CartonString = MakePacket("C")
        Else
            CartonString = MakePacket("CB")
        End If


        Me.CartonsAddedTextBox.ReadOnly = False
        Me.CartonsAddedTextBox.Text = ""
        Me.CartonsAddedTextBox.ReadOnly = True

        FilePanel.Visible = True
        Dim FNReply As String
        Me.CartonsAddedTextBox.Text = ""


        '***********************************************************************************************
        '***********************************************************************************************
        '***********************************************************************************************
        'History T7 Dupe Check for ID
        If Me.LabelDupeCheckEnabel.Text = "Enabled" Then

            Dim DR_ShipmentData As Data.DataRow
            Dim DS_ShipmentData As Data.DataSet
            DS_ShipmentData = Saticode.GetCofAData(CartonString, True, "")

            Dim DR_HistoryT7 As Data.DataRow
            Dim DS_HistoryT7 As Data.DataSet
            DS_HistoryT7 = Saticode.GetMyDataSet("SELECT TOP 100 PERCENT dbo.ShippingInventory.PickTicket, dbo.ShippingInventory.Confirmed, dbo.LabelsMade.Lot, dbo.T_FGI_Boxes.InstanceKey, dbo.T_FGI_Boxes.BoxInvNumber, dbo.T_FGI_Boxes.CartonNumber, dbo.T7_InstanceInfo.Slot, dbo.T7_WaferActionTracking.T7 FROM dbo.T7_WaferActionTracking INNER JOIN dbo.T7_InstanceInfo ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key INNER JOIN dbo.ShippingInventory INNER JOIN dbo.LabelsMade ON dbo.ShippingInventory.LotEntry = dbo.LabelsMade.LabelRecordNumber INNER JOIN dbo.T_FGI_Boxes ON dbo.ShippingInventory.Carton_Key = dbo.T_FGI_Boxes.CartonNumber ON dbo.T7_InstanceInfo.InstanceID = dbo.T_FGI_Boxes.InstanceKey WHERE (dbo.LabelsMade.Lot LIKE N'" & Me.IDLabel.Text & "-%') AND (NOT (dbo.ShippingInventory.PickTicket IS NULL)) AND (NOT (dbo.ShippingInventory.PickTicket LIKE N'')) ORDER BY dbo.ShippingInventory.PickTicket, dbo.ShippingInventory.Carton_Key, dbo.T7_InstanceInfo.Slot")
            'SELECT TOP 100 PERCENT dbo.ShippingInventory.PickTicket, dbo.ShippingInventory.Confirmed, dbo.LabelsMade.Lot, dbo.T_FGI_Boxes.InstanceKey, dbo.T_FGI_Boxes.BoxInvNumber, dbo.T_FGI_Boxes.CartonNumber, dbo.T7_InstanceInfo.Slot, dbo.T7_WaferActionTracking.T7 FROM dbo.T7_WaferActionTracking INNER JOIN dbo.T7_InstanceInfo ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key INNER JOIN dbo.ShippingInventory INNER JOIN dbo.LabelsMade ON dbo.ShippingInventory.LotEntry = dbo.LabelsMade.LabelRecordNumber INNER JOIN dbo.T_FGI_Boxes ON dbo.ShippingInventory.Carton_Key = dbo.T_FGI_Boxes.CartonNumber ON dbo.T7_InstanceInfo.InstanceID = dbo.T_FGI_Boxes.InstanceKey WHERE (dbo.LabelsMade.Lot LIKE N'3264-%') AND (NOT (dbo.ShippingInventory.PickTicket IS NULL)) AND (NOT (dbo.ShippingInventory.PickTicket LIKE N'')) ORDER BY dbo.ShippingInventory.PickTicket, dbo.ShippingInventory.Carton_Key, dbo.T7_InstanceInfo.Slot
            Dim DR_LookT7 As Data.DataRow
            Dim CountShipment As Integer = DS_ShipmentData.Tables(0).Rows.Count
            Dim CountHistory As Integer = DS_HistoryT7.Tables(0).Rows.Count
            Dim CS As Integer
            Dim Look As Integer = 0
            Dim CH As Integer
            Dim T7Error As String

            'Step 1
            'Compair Shipment box for dupes and nest History



            For CS = 0 To CountShipment - 1 ' grab top row of shipment T7
                DR_ShipmentData = DS_ShipmentData.Tables(0).Rows(CS)
                For Look = 1 + CS To CountShipment - 1 ' grab the row under the top row to the end and compair T7s
                    DR_LookT7 = DS_ShipmentData.Tables(0).Rows(Look)
                    If DR_ShipmentData("T7").ToString = DR_LookT7("T7").ToString Then
                        T7Error = "T7 " & DR_ShipmentData("T7").ToString & " Is a DUPE with Carton# " & DR_ShipmentData("CartonNumber") & " Slot# " & DR_ShipmentData("Slot") & ", AND Carton# " & DR_LookT7("CartonNumber").ToString & " Slot# " & DR_LookT7("Slot")
                        Me.CartonsAddedTextBox.Enabled = True
                        Me.CartonsAddedTextBox.Text = T7Error
                        Exit Sub
                    End If
                Next
                For CH = 0 To CountHistory - 1 'Compair the T7 with History data
                    DR_HistoryT7 = DS_HistoryT7.Tables(0).Rows(CH)
                    If DR_ShipmentData("T7").ToString = DR_HistoryT7("T7").ToString Then
                        T7Error = "T7 " & DR_ShipmentData("T7").ToString & " Is a DUPE with Carton# " & DR_ShipmentData("CartonNumber") & " Slot# " & DR_ShipmentData("Slot") & ", AND Carton# " & DR_HistoryT7("CartonNumber").ToString & " Slot# " & DR_HistoryT7("Slot") & " From Pick Ticket History " & DR_HistoryT7("PickTicket")
                        Me.CartonsAddedTextBox.Enabled = True
                        Me.CartonsAddedTextBox.Text = T7Error
                        Exit Sub
                    End If
                Next

            Next
        End If

        '***********************************************************************************************
        '***********************************************************************************************
        '***********************************************************************************************
        '***********************************************************************************************


        'Make Labels
        If Me.MakeLabelsCheckBox.Checked = True Then
            'Make Address Labels
            Dim SmallPrinter As String = "Zebra8"
            Dim BigPrinter As String = "Zebra5"
            If RadioButtonOffSite.Checked = False Then
                SmallPrinter = "Zebra6"
                BigPrinter = "Zebra7"
            End If

            Dim PalletCount As Int16

            If Not Me.PalletCountDropDownList.SelectedValue > 1 Then
                PalletCount = 1
            Else
                PalletCount = Me.PalletCountDropDownList.SelectedValue
            End If

            If Me.LabelCrossFab.Text = "Yes" Then '****Cross Fab*********
                FNReply = Saticode.MakeLabel(False, "Address", "", OrgID, "", 25, 1, 1, SmallPrinter, "", PalletCount * 4, "", "", ShippingAddress, "WB", "", User.Identity.Name.ToString, False, 0)
            Else
                FNReply = Saticode.MakeLabel(False, "Address", "", TheID, "", 25, 1, 1, SmallPrinter, "", PalletCount * 4, "", "", ShippingAddress, "WB", "", User.Identity.Name.ToString, False, 0)
            End If

            If FNReply.Contains("Error") Then
                HadError = True
                Me.CartonsAddedTextBox.ReadOnly = False
                Me.CartonsAddedTextBox.Text = FNReply & Me.CartonsAddedTextBox.Text
                Me.CartonsAddedTextBox.ReadOnly = True
                Exit Sub
            End If
            Me.CartonsAddedTextBox.ReadOnly = False
            Me.CartonsAddedTextBox.Text = PalletCount * 4 & " Address Labels Were Made..." & Chr(13) & Me.CartonsAddedTextBox.Text
            Me.CartonsAddedTextBox.ReadOnly = True

            'Make CL Labels
            Dim CLCount As Integer
            Dim CartonQTY As Integer



            'need to find each boxes qty and then make each label

            'GetCartonQty

            Dim CartonStingCopy As String = CartonString
            Dim Index As Int16 = 0
            Dim C As String = ""


            For CLCount = 1 To CartonCount
                If CartonStingCopy.Contains("C") Then
                    If CartonStingCopy.Contains("CB") Then
                        Index = 3
                        C = Mid(CartonStingCopy, Index, CartonStingCopy.IndexOf(Chr(13)) - 2)
                    Else
                        Index = 2
                        C = Mid(CartonStingCopy, Index, CartonStingCopy.IndexOf(Chr(13)) - 1)
                    End If
                End If

                CartonQTY = Saticode.GetCartonQty(C)

                CartonStingCopy = Mid(CartonStingCopy, CartonStingCopy.IndexOf(Chr(13)) + 2)

                If Me.LabelCrossFab.Text = "Yes" Then '****Cross Fab*********
                    FNReply = Saticode.MakeLabel(False, "CL", "Shipping WIP", OrgID, "", CartonQTY, 1, 1, BigPrinter, SmallPrinter, 1, CLCount & "/" & CartonCount, PickID, ShippingAddress, "WB", "", User.Identity.Name.ToString, False, 0)
                Else
                    FNReply = Saticode.MakeLabel(False, "CL", "Shipping WIP", TheID, "", CartonQTY, 1, 1, BigPrinter, SmallPrinter, 1, CLCount & "/" & CartonCount, PickID, ShippingAddress, "WB", "", User.Identity.Name.ToString, False, 0)
                End If

                If FNReply.Contains("Error") Then
                    HadError = True
                    Me.CartonsAddedTextBox.ReadOnly = False
                    Me.CartonsAddedTextBox.Text = FNReply & Chr(13) & Me.CartonsAddedTextBox.Text
                    Me.CartonsAddedTextBox.ReadOnly = True
                    Exit Sub
                End If
            Next
            Me.CartonsAddedTextBox.ReadOnly = False
            Me.CartonsAddedTextBox.Text = CartonCount & " Cardboard Labels Were Made..." & Chr(13) & Me.CartonsAddedTextBox.Text
            Me.CartonsAddedTextBox.ReadOnly = True
        End If

        'Make Packing Slip
        If Me.MakePackingSlipCheckBox.Checked Then
            If LabelBulk.Visible = True Then
                FNReply = Saticode.MakePackingSlipBulk(CartonString, PickID, Me.POLabel.Text, Me.QtyAddedLabel.Text, Me.CartonCountLabel.Text, Me.FreightAccountDropDownList.SelectedItem.Text, Me.CarrierDropDownList.SelectedItem.Text, Me.FreightDropDownList.SelectedItem.Text, ShippingAddress, BillingAddress, OrgID)
            Else
                FNReply = Saticode.MakePackingSlip(CartonString, PickID, Me.POLabel.Text, Me.QtyAddedLabel.Text, Me.CartonCountLabel.Text, Me.FreightAccountDropDownList.SelectedItem.Text, Me.CarrierDropDownList.SelectedItem.Text, Me.FreightDropDownList.SelectedItem.Text, ShippingAddress, BillingAddress, OrgID)
            End If


            If Not FNReply.Contains("Error") Then
                If Me.LabelCrossFab.Text = "Yes" Then '****Cross Fab*********
                    Me.PSHyperLink.NavigateUrl = Session("CustomerData") & OrgID & "\" & FNReply & ".xls"
                Else
                    Me.PSHyperLink.NavigateUrl = Session("CustomerData") & TheID & "\" & FNReply & ".xls"
                End If

                Me.CartonsAddedTextBox.ReadOnly = False
                Me.CartonsAddedTextBox.Text = "Packing Slip Was Made..." & Chr(13) & Me.CartonsAddedTextBox.Text
                Me.CartonsAddedTextBox.ReadOnly = True
            Else
                HadError = True
                Me.CartonsAddedTextBox.ReadOnly = False
                Me.CartonsAddedTextBox.Text = FNReply & Chr(13) & Me.CartonsAddedTextBox.Text
                Me.CartonsAddedTextBox.ReadOnly = True
                Exit Sub
            End If
        End If

        'Make CofA
        If Me.MakeCofACheckBox.Checked = True Then
            If LabelBulk.Visible = True Then
                FNReply = Saticode.MakeCofABulk(CartonString, PickID, OrgID) '200mm< bulk
            Else
                FNReply = Saticode.MakeCofA(CartonString, PickID, OrgID) '300mm
            End If

            If Not FNReply.Contains("Error") Then

                If Me.LabelCrossFab.Text = "Yes" Then '****Cross Fab*********
                    Me.CofAHyperLink.NavigateUrl = Session("CustomerData") & OrgID & "\" & FNReply & ".xls"
                Else
                    Me.CofAHyperLink.NavigateUrl = Session("CustomerData") & TheID & "\" & FNReply & ".xls"
                End If

                'PrintCofA("\\PWI-40\Customerdata$\" & TheID & "\" & FNReply & ".xls")
                Me.CartonsAddedTextBox.ReadOnly = False
                Me.CartonsAddedTextBox.Text = "CofA Was Made..." & Chr(13) & Me.CartonsAddedTextBox.Text
                Me.CartonsAddedTextBox.ReadOnly = True

                'Place Shipment in Ship Pending
                If AddToPendingShipments.Checked = True Then
                    Saticode.PendingShipmentAdd("Add", PickID, "Auto Add", User.Identity.Name.ToString)
                End If

            Else
                HadError = True
                Me.CartonsAddedTextBox.ReadOnly = False
                Me.CartonsAddedTextBox.Text = FNReply & Chr(13) & Me.CartonsAddedTextBox.Text
                Me.CartonsAddedTextBox.ReadOnly = True
                Exit Sub
            End If
        End If

        'Mark shipped in Invintory
        If HadError = False Then
            Dim ShippingID As String
            Dim PalletKey As String
            '1 Shipping_Log Table for input and get ShippingID, Function Shipping_LogTabl
            ShippingID = Saticode.Shipping_LogTable(Me.CarrierDropDownList.SelectedItem.Text, Me.FreightAccountDropDownList.SelectedItem.Text, Me.FreightDropDownList.SelectedItem.Text, User.Identity.Name.ToString)

            '2 ShippingUnit Table for input of ShippingID and Get PalletKey, Function Shipping_UnitTable
            PalletKey = Saticode.Shipping_UnitTable(ShippingID, User.Identity.Name.ToString)

            '3 Pick_Shipping Table for Input of PalletKey, Function Pick_Shipping_UnitTable
            Saticode.Pick_Shipping_UnitTable(PickID, PalletKey)

            FNReply = Saticode.ShippmentFinalRecordEntry(CartonString, PickID)

            If FNReply.Contains("Error") Then
                Me.CartonsAddedTextBox.ReadOnly = False
                Me.CartonsAddedTextBox.Text = FNReply & Chr(13) & Me.CartonsAddedTextBox.Text
                Me.CartonsAddedTextBox.ReadOnly = True
                Exit Sub
            Else
                Me.CartonsAddedTextBox.ReadOnly = False
                Me.CartonsAddedTextBox.Text = FNReply & Chr(13) & Me.CartonsAddedTextBox.Text

                Me.CartonsAddedTextBox.ReadOnly = True
            End If
        End If

        Me.CartonsAddedTextBox.Text = "Follow your CofA and Packing Slip for Printing...Done" & Chr(13) & Me.CartonsAddedTextBox.Text

    End Sub

    Function MakePacket(Type As String) As String
        MakePacket = ""
        Dim Line As String
        Dim TheTextBox As String = Me.CartonsAddedTextBox.Text

        Do
            If TheTextBox.Contains(Chr(13)) Then
                Line = Left(TheTextBox, TheTextBox.IndexOf(Chr(13)))
                If Line.Contains("Good Scan Carton") Then
                    Line = Mid(Line, 18)
                    MakePacket = MakePacket & Type & Left(Line, Line.IndexOf(",")) & Chr(13)

                End If
                TheTextBox = Mid(TheTextBox, TheTextBox.IndexOf(Chr(13)) + 2)
            Else
                Exit Do
            End If
        Loop

    End Function


    Sub PrintCofA(ByVal File As String)
        Dim Flex As New FlexCel.XlsAdapter.XlsFile(True)

        Flex.RecalcMode = Core.TRecalcMode.OnEveryChange

        Flex.Open(File)

        Flex.Recalc(True)

        Flex.ActiveSheetByName = "C of A"

        Flex.PrintOptions = FlexCel.Core.TPrintOptions.None

        Dim printlabel As New FlexCel.Render.FlexCelPrintDocument(Flex)

        printlabel.PrinterSettings.Copies = 1
        printlabel.Print()
        printlabel.Dispose()

    End Sub

    Sub CheckForCrossFabShip()
        CrossFabShip = Saticode.CrossFabShip(Me.IDLabel.Text)
        If CrossFabShip = True Then
            Me.LabelCrossFab.Text = "Yes"
        Else
            Me.LabelCrossFab.Text = "No"
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MakeTheShipment()
    End Sub

End Class
