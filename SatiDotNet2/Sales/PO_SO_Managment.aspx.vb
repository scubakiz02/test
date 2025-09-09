
Partial Class Sales_PO_SO_Managment
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1
    Dim DS_Fabs As Data.DataSet
    Dim SqlDataSourceCurrentSOsText As String

    Private Sub Sales_PO_SO_Managment_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("Sales", Server)

        'Get_FAB_Dataset()
    End Sub

    Sub Get_FAB_Dataset()
        DS_Fabs = Saticode.GetMyDataSet("SELECT CustomerID FROM dbo.MainID WHERE (ExpirationDtd IS NULL OR ExpirationDtd > { fn NOW() }) GROUP BY CustomerID")
        Me.DropDownListFABs.DataSource = DS_Fabs

        Me.DropDownListFABs.DataBind()
    End Sub

    Protected Sub DropDownListFABs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListFABs.SelectedIndexChanged
        EvalView()
        If Me.DropDownListFABs.SelectedItem.Text = "Select..." Then
            Me.ButtonAddSO.Visible = False
        Else
            Me.ButtonAddSO.Visible = True
        End If
    End Sub

    Protected Sub RadioButtonPast_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonPast.CheckedChanged
        EvalView()
    End Sub
    Protected Sub RadioButtonCutternt_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonCutternt.CheckedChanged
        EvalView()
    End Sub
    Protected Sub RadioButtonFuture_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonFuture.CheckedChanged
        EvalView()
    End Sub

    Sub EvalView()
        If RadioButtonPast.Checked = True Then
            SetupView(0)
        End If
        If RadioButtonCutternt.Checked = True Then
            SetupView(1)
        End If

    End Sub



    Sub SetupView(View As Int16)
        Dim DS As New Data.DataSet
        Dim Fab As String = ""
        Me.MultiView1.ActiveViewIndex = 1 'View
        Fab = Me.DropDownListFABs.SelectedItem.Text



        Select Case View
            Case 0
                SqlDataSourceCurrentSOsText = ("SELECT TOP (100) PERCENT dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number AS PO, dbo.SO_LineItems.Qty AS [PO Qty], dbo.SO_Info.SO_Replaced AS [Past SO], dbo.SO_Info.EffectiveDtd, dbo.SO_Info.ExpirationDtd, SUM(dbo.ShippingInventory.Total_Qty) AS Shipped, dbo.SO_LineItems.Qty - SUM(dbo.ShippingInventory.Total_Qty) AS Balance FROM dbo.ShippingInventory INNER JOIN dbo.Pick_Log ON dbo.ShippingInventory.PickTicket = dbo.Pick_Log.PickTicket RIGHT OUTER JOIN dbo.MainID INNER JOIN dbo.SO_LineItems ON dbo.MainID.MainID = dbo.SO_LineItems.MainID INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO ON dbo.Pick_Log.SO = dbo.SO_Info.SO WHERE (dbo.MainID.CustomerID = N'" & Fab & "') GROUP BY dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.SO_Info.SO_Replaced, dbo.SO_Info.EffectiveDtd, dbo.SO_Info.ExpirationDtd, dbo.SO_LineItems.Qty HAVING (dbo.SO_Info.ExpirationDtd < { fn NOW() }) ORDER BY dbo.MainID.MainID")
                Me.SqlDataSourceCurrentSOs.SelectCommand = SqlDataSourceCurrentSOsText
                Me.SqlDataSourceCurrentSOs.DataBind()
            Case 1
                SqlDataSourceCurrentSOsText = ("SELECT TOP (100) PERCENT dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number AS PO, dbo.SO_LineItems.Qty AS [PO Qty], dbo.SO_Info.SO_Replaced AS [Past SO], dbo.SO_Info.EffectiveDtd, dbo.SO_Info.ExpirationDtd, SUM(dbo.ShippingInventory.Total_Qty) AS Shipped, dbo.SO_LineItems.Qty - SUM(dbo.ShippingInventory.Total_Qty) AS Balance FROM dbo.ShippingInventory INNER JOIN dbo.Pick_Log ON dbo.ShippingInventory.PickTicket = dbo.Pick_Log.PickTicket RIGHT OUTER JOIN dbo.MainID INNER JOIN dbo.SO_LineItems ON dbo.MainID.MainID = dbo.SO_LineItems.MainID INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO ON dbo.Pick_Log.SO = dbo.SO_Info.SO WHERE (dbo.MainID.CustomerID = N'" & Fab & "') GROUP BY dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.SO_Info.SO_Replaced, dbo.SO_Info.EffectiveDtd, dbo.SO_Info.ExpirationDtd, dbo.SO_LineItems.Qty HAVING (dbo.SO_Info.ExpirationDtd IS NULL) OR (dbo.SO_Info.ExpirationDtd > { fn NOW() }) ORDER BY dbo.MainID.MainID")
                Me.SqlDataSourceCurrentSOs.SelectCommand = SqlDataSourceCurrentSOsText
                Me.SqlDataSourceCurrentSOs.DataBind()
            Case 2
                SqlDataSourceCurrentSOsText = ("SELECT TOP (100) PERCENT dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number AS PO, dbo.SO_LineItems.Qty AS [PO Qty], dbo.SO_Info.SO_Replaced AS [Past SO], dbo.SO_Info.EffectiveDtd, dbo.SO_Info.ExpirationDtd, SUM(dbo.ShippingInventory.Total_Qty) AS Shipped, dbo.SO_LineItems.Qty - SUM(dbo.ShippingInventory.Total_Qty) AS Balance FROM dbo.ShippingInventory INNER JOIN dbo.Pick_Log ON dbo.ShippingInventory.PickTicket = dbo.Pick_Log.PickTicket RIGHT OUTER JOIN dbo.MainID INNER JOIN dbo.SO_LineItems ON dbo.MainID.MainID = dbo.SO_LineItems.MainID INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO ON dbo.Pick_Log.SO = dbo.SO_Info.SO WHERE (dbo.MainID.CustomerID = N'" & Fab & "') GROUP BY dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.SO_Info.SO_Replaced, dbo.SO_Info.EffectiveDtd, dbo.SO_Info.ExpirationDtd, dbo.SO_LineItems.Qty HAVING (dbo.SO_Info.ExpirationDtd < { fn NOW() }) ORDER BY dbo.MainID.MainID")
                Me.SqlDataSourceCurrentSOs.SelectCommand = SqlDataSourceCurrentSOsText
                Me.SqlDataSourceCurrentSOs.DataBind()
        End Select



    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand

        Dim row As String = e.CommandArgument.ToString

        'Dim MainID As String = CType(Me.GridView1.Rows(row).Cells(1).FindControl("LinkButtonMainID"), LinkButton).Text

        If e.CommandName = "EditSO" Then
            Me.LabelEditSO.Text = Me.GridView1.Rows(row).Cells(2).Text
            Me.TextBoxEdit_MainID.Text = CType(Me.GridView1.Rows(row).Cells(1).FindControl("LinkButtonMainID"), LinkButton).Text
            Me.TextBoxEdit_PO.Text = Me.GridView1.Rows(row).Cells(3).Text
            Me.TextBoxEdit_PO_Qty.Text = Me.GridView1.Rows(row).Cells(4).Text
            Me.TextBoxEdit_Past_SO.Text = Me.GridView1.Rows(row).Cells(5).Text
            Me.TextBoxEdit_Eff_Date.Text = Me.GridView1.Rows(row).Cells(6).Text
            Me.TextBoxEdit_Exp_Date.Text = Me.GridView1.Rows(row).Cells(7).Text

            Me.PanelSOEdit_ModalPopupExtender.Show()
        End If

        If e.CommandName = "ViewSharedIDs" Then
            Me.ModalPopupExtender1.Show()
            Me.LabelMainIDforSharedID.Text = e.CommandArgument.ToString
            Me.SharedIDsSqlDataSource.SelectCommand = "SELECT [SO_MainID], [Child_MainID] FROM [MainID_SO_LineItems] WHERE (SO_MainID = N'" & e.CommandArgument.ToString & "')"

            Me.IDsSqlDataSource.SelectCommand = "SELECT MainID, CustomerID FROM dbo.MainID WHERE (CustomerID = N'" & Me.DropDownListFABs.Text & "')"

        End If

        If e.CommandName = "ViewShipments" Then
            Me.PanelViewShipped_ModalPopupExtender.Show()

            Me.SqlDataSourceShipped.SelectCommand = "SELECT PickTicket, Total_Qty AS Qty, EventTime FROM Pick_Log WHERE (SO = N'" & e.CommandArgument.ToString & "')"
            Me.SqlDataSourceShipped.SelectCommand = "SELECT dbo.Pick_Log.PickTicket, SUM(dbo.ShippingInventory.Total_Qty) AS Qty, dbo.Pick_Log.EventTime FROM dbo.Pick_Log INNER JOIN dbo.ShippingInventory ON dbo.Pick_Log.PickTicket = dbo.ShippingInventory.PickTicket WHERE (dbo.Pick_Log.SO = N'" & e.CommandArgument.ToString & "') GROUP BY dbo.Pick_Log.PickTicket, dbo.Pick_Log.EventTime"

        End If

    End Sub



    Protected Sub AddChildIDButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim MainID As String
        Dim ChildID As String
        MainID = Me.LabelMainIDforSharedID.Text
        ChildID = Me.ChildIDDropDownList.SelectedItem.Text
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA_AddShare As New Data.SqlClient.SqlDataAdapter
        Dim DS_AddShare As New Data.DataSet
        Dim DR_AddShare As Data.DataRow

        Dim AddShareSelectCmd As New System.Data.SqlClient.SqlCommand
        With AddShareSelectCmd
            .CommandText = "SELECT SO_MainID, Child_MainID, Operator, EventTime FROM dbo.MainID_SO_LineItems WHERE (SO_MainID = N'')"
            .Connection = Connection
        End With
        DA_AddShare.SelectCommand = AddShareSelectCmd

        Dim AddShareInsertCmd As New System.Data.SqlClient.SqlCommand
        With AddShareInsertCmd
            .CommandText = "INSERT INTO [dbo].[MainID_SO_LineItems] ([SO_MainID], [Child_MainID], [Operator], [EventTime]) VALUES (@SO_MainID, @Child_MainID, @Operator, @EventTime); SELECT SO_MainID, Child_MainID, Operator, EventTime FROM dbo.MainID_SO_LineItems WHERE (Child_MainID = @Child_MainID) AND (SO_MainID = @SO_MainID)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@SO_MainID", System.Data.SqlDbType.NVarChar, 0, "SO_MainID"), New System.Data.SqlClient.SqlParameter("@Child_MainID", System.Data.SqlDbType.NVarChar, 0, "Child_MainID"), New System.Data.SqlClient.SqlParameter("@Operator", System.Data.SqlDbType.NVarChar, 0, "Operator"), New System.Data.SqlClient.SqlParameter("@EventTime", System.Data.SqlDbType.SmallDateTime, 0, "EventTime")})
        End With
        DA_AddShare.InsertCommand = AddShareInsertCmd

        DA_AddShare.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "MainID_SO_LineItems", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("SO_MainID", "SO_MainID"), New System.Data.Common.DataColumnMapping("Child_MainID", "Child_MainID"), New System.Data.Common.DataColumnMapping("Operator", "Operator"), New System.Data.Common.DataColumnMapping("EventTime", "EventTime")})})
        DA_AddShare.Fill(DS_AddShare)

        DR_AddShare = DS_AddShare.Tables("MainID_SO_LineItems").NewRow
        DR_AddShare("SO_MainID") = MainID
        DR_AddShare("Child_MainID") = ChildID
        DR_AddShare("Operator") = User.Identity.Name.ToString
        DR_AddShare("EventTime") = DateTime.Now.ToShortDateString
        DS_AddShare.Tables("MainID_SO_LineItems").Rows.Add(DR_AddShare)
        DA_AddShare.Update(DS_AddShare, "MainID_SO_LineItems")

        Me.SharedIDsSqlDataSource.SelectCommand = "SELECT [SO_MainID], [Child_MainID] FROM [MainID_SO_LineItems] WHERE (SO_MainID = N'" & MainID & "')"
        Me.GridViewSharedIDs.DataBind()

        Connection.Close()
    End Sub

    Protected Sub ButtonSaveSOEdit_Click(sender As Object, e As EventArgs) Handles ButtonSaveSOEdit.Click
        Saticode.ModSO("Update", "", Me.TextBoxEdit_MainID.Text, Me.LabelEditSO.Text, Me.TextBoxEdit_PO.Text, Me.TextBoxEdit_PO_Qty.Text, Me.TextBoxEdit_Past_SO.Text, Me.TextBoxEdit_Eff_Date.Text, Me.TextBoxEdit_Exp_Date.Text)
        EvalView()
        Me.PanelSOEdit_ModalPopupExtender.Hide()

    End Sub

    Protected Sub ButtonMakeNewSO_Click(sender As Object, e As EventArgs) Handles ButtonMakeNewSO.Click
        MakeNewSO(Me.LabelEditSO.Text, Me.TextBoxEdit_MainID.Text)
        EvalView()
    End Sub

    Protected Sub ButtonSavePO_Click(sender As Object, e As EventArgs) Handles ButtonSavePO.Click
        If Not Me.TextBoxEnterPastSO.Text = "" Then
            Saticode.ModSO("Expire", "", Me.DropDownListEnterID.SelectedItem.Text, Me.TextBoxEnterPastSO.Text, Me.TextBoxEnterPO.Text, Me.TextBoxEnterPO_Qty.Text, Me.TextBoxEnterPastSO.Text, Me.TextBoxEnterEffectiveDate.Text, Me.TextBoxEnterEffectiveDate.Text)
        End If

        Saticode.ModSO("Add", "", Me.DropDownListEnterID.SelectedItem.Text, Me.TextBoxEnterSO.Text, Me.TextBoxEnterPO.Text, Me.TextBoxEnterPO_Qty.Text, Me.TextBoxEnterPastSO.Text, Me.TextBoxEnterEffectiveDate.Text, Me.TextBoxEnterExpirationDate.Text)
        EvalView()
    End Sub

    Protected Sub ButtonAddSO_Click(sender As Object, e As EventArgs) Handles ButtonAddSO.Click
        MakeNewSO("", "")
        EvalView()
    End Sub

    Sub MakeNewSO(OldSO As String, MainID As String)
        Me.PanelEnterPO_ModalPopupExtender.Show()
        Me.IDsSqlDataSource.SelectCommand = "SELECT MainID, CustomerID FROM dbo.MainID WHERE (CustomerID = N'" & Me.DropDownListFABs.Text & "')"
        Me.DropDownListEnterID.DataBind()
        If Not MainID = "" Then
            Dim I As Int16
            Dim L As New ListItem
            L.Text = MainID
            I = Me.DropDownListEnterID.Items.IndexOf(L)
            Me.DropDownListEnterID.SelectedIndex = I
            Me.DropDownListEnterID.DataBind()
        End If
        Me.TextBoxEnterSO.Text = ""
        Me.TextBoxEnterPO.Text = ""
        Me.TextBoxEnterPO_Qty.Text = ""
        Me.TextBoxEnterPastSO.Text = ""
        Me.TextBoxEnterEffectiveDate.Text = ""
        Me.TextBoxEnterExpirationDate.Text = ""

        Dim Replace As Boolean = False

        If OldSO = "" Then
            Replace = False
        Else
            Replace = True
            Me.TextBoxEnterPastSO.Text = OldSO

        End If
        Me.TextBoxEnterEffectiveDate.Text = DateAndTime.Now.ToShortDateString


    End Sub


    Protected Sub ButtonRunReport_Click(sender As Object, e As EventArgs) Handles ButtonRunReport.Click
        'Get_Current_SO_Summary()

        Dim ReportName As String

        ReportName = Saticode.Make_Current_SO_Report()
        Me.HyperLinkReport.Visible = True
        Me.HyperLinkReport.NavigateUrl = Session("ReportFolder") & ReportName

    End Sub

    Sub Get_Current_SO_Summary()
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim Fab As String
        Dim Row As Integer = 3
        Dim FabWrite As Boolean = True

        'DS = Saticode.GetMyDataSet("SELECT TOP (100) PERCENT dbo.MainID.CustomerID, dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number AS PO, dbo.SO_LineItems.Qty AS [PO Qty], SUM(dbo.ShippingInventory.Total_Qty) AS Shipped, dbo.SO_LineItems.Qty - SUM(dbo.ShippingInventory.Total_Qty) AS Balance, dbo.SO_Info.EffectiveDtd, dbo.SO_Info.ExpirationDtd FROM dbo.ShippingInventory INNER JOIN dbo.Pick_Log ON dbo.ShippingInventory.PickTicket = dbo.Pick_Log.PickTicket RIGHT OUTER JOIN dbo.MainID INNER JOIN dbo.SO_LineItems ON dbo.MainID.MainID = dbo.SO_LineItems.MainID INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO ON dbo.Pick_Log.SO = dbo.SO_Info.SO GROUP BY dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.SO_Info.EffectiveDtd, dbo.SO_Info.ExpirationDtd, dbo.SO_LineItems.Qty, dbo.MainID.CustomerID HAVING (dbo.SO_Info.ExpirationDtd > { fn NOW() }) OR (dbo.SO_Info.ExpirationDtd IS NULL) ORDER BY dbo.MainID.CustomerID, dbo.MainID.MainID")

        'DS = Saticode.GetMyDataSet("SELECT TOP (100) PERCENT dbo.MainID.CustomerID, dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number AS PO, dbo.SO_LineItems.Qty AS [PO Qty], SUM(dbo.ShippingInventory.Total_Qty) AS Shipped, dbo.SO_LineItems.Qty - SUM(dbo.ShippingInventory.Total_Qty) AS Balance, dbo.SO_Info.EffectiveDtd, dbo.SO_Info.ExpirationDtd, dbo.MainIDSpec.PART_NUMBER, dbo.MainIDSpec.PART_REV_NUMBER, dbo.MainIDSpec.SPEC_NUMBER, dbo.MainIDSpec.SPEC_REV_NUMBER, dbo.MainID.Diameter, dbo.MainID.Exsil_Supplied AS Supply FROM dbo.MainID_MainIDSpec INNER JOIN dbo.MainIDSpec ON dbo.MainID_MainIDSpec.WaferSpec_Key = dbo.MainIDSpec.RecordNumber RIGHT OUTER JOIN dbo.MainID INNER JOIN dbo.SO_LineItems ON dbo.MainID.MainID = dbo.SO_LineItems.MainID INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO ON dbo.MainID_MainIDSpec.MainID = dbo.SO_LineItems.MainID LEFT OUTER JOIN dbo.ShippingInventory INNER JOIN dbo.Pick_Log ON dbo.ShippingInventory.PickTicket = dbo.Pick_Log.PickTicket ON dbo.SO_Info.SO = dbo.Pick_Log.SO GROUP BY dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.SO_Info.EffectiveDtd, dbo.SO_Info.ExpirationDtd, dbo.SO_LineItems.Qty, dbo.MainID.CustomerID, dbo.MainID_MainIDSpec.ExpirationDtd, dbo.MainIDSpec.PART_NUMBER, dbo.MainIDSpec.PART_REV_NUMBER, dbo.MainIDSpec.SPEC_NUMBER, dbo.MainIDSpec.SPEC_REV_NUMBER, dbo.MainID.Diameter, dbo.MainID.Exsil_Supplied HAVING (dbo.SO_Info.ExpirationDtd > { fn NOW() }) OR (dbo.SO_Info.ExpirationDtd IS NULL) AND (dbo.MainID_MainIDSpec.ExpirationDtd IS NULL) ORDER BY dbo.MainID.CustomerID, dbo.MainID.MainID")

        DS = Saticode.GetMyDataSet("SELECT TOP (100) PERCENT dbo.MainID.CustomerID, dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number AS PO, dbo.SO_LineItems.Qty AS [PO Qty], SUM(dbo.ShippingInventory.Total_Qty) AS Shipped, dbo.SO_LineItems.Qty - SUM(dbo.ShippingInventory.Total_Qty) AS Balance, dbo.SO_Info.EffectiveDtd, dbo.SO_Info.ExpirationDtd, dbo.MainIDSpec.PART_NUMBER, dbo.MainIDSpec.PART_REV_NUMBER, dbo.MainIDSpec.SPEC_NUMBER, dbo.MainIDSpec.SPEC_REV_NUMBER, dbo.MainID.Diameter, dbo.MainID.Exsil_Supplied AS Supply, CASE WHEN T_SO_Future_List.SO LIKE '%' THEN 'YES' END AS Queued FROM dbo.T_SO_Future_List RIGHT OUTER JOIN dbo.MainID INNER JOIN dbo.SO_LineItems ON dbo.MainID.MainID = dbo.SO_LineItems.MainID INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO ON dbo.T_SO_Future_List.MainID = dbo.SO_LineItems.MainID LEFT OUTER JOIN dbo.MainID_MainIDSpec INNER JOIN dbo.MainIDSpec ON dbo.MainID_MainIDSpec.WaferSpec_Key = dbo.MainIDSpec.RecordNumber ON dbo.SO_LineItems.MainID = dbo.MainID_MainIDSpec.MainID LEFT OUTER JOIN dbo.ShippingInventory INNER JOIN dbo.Pick_Log ON dbo.ShippingInventory.PickTicket = dbo.Pick_Log.PickTicket ON dbo.SO_Info.SO = dbo.Pick_Log.SO GROUP BY dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.SO_Info.EffectiveDtd, dbo.SO_Info.ExpirationDtd, dbo.SO_LineItems.Qty, dbo.MainID.CustomerID, dbo.MainID_MainIDSpec.ExpirationDtd, dbo.MainIDSpec.PART_NUMBER, dbo.MainIDSpec.PART_REV_NUMBER, dbo.MainIDSpec.SPEC_NUMBER, dbo.MainIDSpec.SPEC_REV_NUMBER, dbo.MainID.Diameter, dbo.MainID.Exsil_Supplied, CASE WHEN T_SO_Future_List.SO LIKE '%' THEN 'YES' END HAVING (dbo.SO_Info.ExpirationDtd > { fn NOW() }) OR (dbo.SO_Info.ExpirationDtd IS NULL) AND (dbo.MainID_MainIDSpec.ExpirationDtd IS NULL) ORDER BY dbo.MainID.CustomerID, dbo.MainID.MainID")


        Dim Flex As New FlexCel.XlsAdapter.XlsFile(True)

        Dim Path As String
        Path = "\\PWI-40\software$\LabelTemplates\Sati_SO_Report2.xls"
        Flex.Open(Path)
        Flex.ActiveSheetByName = "SO_Summary"

        DR = DS.Tables(0).Rows(0)
        Fab = DR("CustomerID")

        For i As Integer = 0 To DS.Tables(0).Rows.Count - 1
            DR = DS.Tables(0).Rows(i)
            If Not Fab = DR("CustomerID") Then
                Row = Row + 1
                FabWrite = True
            End If

            If FabWrite = True Then
                Flex.SetCellValue(Row, 2, DR("CustomerID"))
            End If
            Flex.SetCellValue(Row, 3, DR("MainID"))

            Flex.SetCellValue(Row, 4, DR("Diameter"))
            Flex.SetCellValue(Row, 5, DR("PART_NUMBER"))
            Flex.SetCellValue(Row, 6, DR("PART_REV_NUMBER"))
            Flex.SetCellValue(Row, 7, DR("SPEC_NUMBER"))
            Flex.SetCellValue(Row, 8, DR("SPEC_REV_NUMBER"))

            Flex.SetCellValue(Row, 9, DR("SO"))
            Flex.SetCellValue(Row, 10, DR("PO"))

            Flex.SetCellValue(Row, 11, DR("Queued"))

            Flex.SetCellValue(Row, 12, DR("PO Qty"))
            Flex.SetCellValue(Row, 13, DR("Shipped"))
            Flex.SetCellValue(Row, 14, DR("Balance"))
            Flex.SetCellValue(Row, 15, DR("EffectiveDtd"))
            Flex.SetCellValue(Row, 16, DR("ExpirationDtd"))

            Fab = DR("CustomerID")
            Row = Row + 1
            FabWrite = False
        Next

        Dim ReportName As String = "SO_Report_ " & User.Identity.Name.ToString & ".xls"


        Flex.RecalcAndVerify()

        Flex.Save("\\PWI-40\TempImageWebFiles$\" & ReportName)


        Me.HyperLinkReport.Visible = True
        Me.HyperLinkReport.NavigateUrl = Session("ReportFolder") & ReportName


    End Sub


End Class
