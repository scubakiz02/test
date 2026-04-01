
Imports System.Activities.Expressions
Imports System.Drawing
Imports Microsoft.Office.Interop.Excel

Partial Class PC_NonConformingSkidBuilder
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        Me.SetFocus(ScanSkid)
        If Me.ArrowErrorLabel.ForeColor = Color.DarkRed Then
            Me.ArrowErrorLabel.Font.Size = 24
            Me.ArrowErrorLabel.Text = "&#x2193;"
            Me.ArrowErrorLabel.BackColor = Nothing
            Me.ArrowErrorLabel.ForeColor = Color.Goldenrod
        End If

        If Not Me.IsPostBack Then
            If ScanSkid.Text = "" Then
                GridHeaderPanel.Visible = False
                GridViewPanel.Visible = False
                BoxInputPanel.Visible = False
                Me.SetFocus(ScanSkid)
            End If
        End If
    End Sub
    Protected Sub GenButton_Click(sender As Object, e As EventArgs) Handles GenButton.Click
        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
        connect.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString


        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand
        command.CommandText = "INSERT INTO [T_NC_Skid] ([Note]) VALUES ('CREATED BY OPERATOR : " & User.Identity.Name.ToString & "')"
        command.Connection = connect
        connect.Open()
        command.ExecuteReader()
        connect.Close()

        Dim Stamp As DateTime = DateTime.Now
        Dim tempStamp As String = FormatDateTime(Stamp, DateFormat.ShortTime)
        Stamp = DateTime.Parse(tempStamp)

        connect = New Data.SqlClient.SqlConnection
        connect.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString

        command = New Data.SqlClient.SqlCommand
        command.CommandText = "SELECT NC_SKID FROM [T_NC_Skid] WHERE ([Note] = 'CREATED BY OPERATOR : " & User.Identity.Name.ToString & "' AND [TimeStamp] BETWEEN '" & Stamp.AddMinutes(-1) & "' AND '" & Stamp.AddMinutes(1) & "')"
        command.Connection = connect
        Dim Skid As String = "0"

        connect.Open()
        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            Skid = checker(0)
        End While
        connect.Close()

        Skid = Integer.Parse(Skid)
        CreateNewSkidLabel(Skid)
        SkidBoxInvDataBind(Skid)

        Me.ScanBox.Text = String.Empty
        Me.SetFocus(ScanBox)
        Me.ScanSkid.Text = "P" + Skid
        Me.SkidNum.Text = "P" + Skid
        Me.SkidNum.BackColor = System.Drawing.Color.Yellow

        GridHeaderPanel.Visible = True
        GridViewPanel.Visible = True
        BoxInputPanel.Visible = True
    End Sub
    Sub CreateNewSkidLabel(Skid As Integer)
        Dim Flex As New FlexCel.XlsAdapter.XlsFile(True)
        Dim Path As String = " \\PWI-40\software$\LabelTemplates\Non-Conforming Skid Label.xls"
        Flex.Open(Path)
        Flex.ActiveSheetByName = "NC Label"

        Dim Difference As Integer = 7 - Skid.ToString("D").Length
        Dim Placement As Integer = Skid.ToString("D").Length + Difference
        Flex.SetCellValue(3, 2, "P" + Skid.ToString("D" + Placement.ToString()))

        Flex.ConvertFormulasToValues(True)
        Flex.Recalc()
        Flex.PrintOptions = FlexCel.Core.TPrintOptions.None

        Dim SkidLabelPrint As New FlexCel.Render.FlexCelPrintDocument(Flex)
        Try
            With SkidLabelPrint
                .PrinterSettings.PrinterName = Me.GenPrinterlist.SelectedItem.Text
                .PrinterSettings.Copies = 1
                .Print()
                .Dispose()
            End With
        Catch ex As Exception
            Console.WriteLine("Failed To Print")
        End Try
    End Sub
    Protected Sub SkidSearch_Click(sender As Object, e As EventArgs) Handles SkidSearch.Click
        Dim Skid As String = Me.ScanSkid.Text
        If Skid.StartsWith("*") And Skid.EndsWith("*") Then
            Skid = Skid.Substring(1)
            Skid = Skid.Substring(0, Skid.Length - 1)
        End If

        Dim Upper As String = Skid.Substring(0, 1).ToUpper()
        Skid = Skid.Replace(Skid.Substring(0, 1), Upper)

        Dim Test As Integer
        If ScanSkid.Text = "" Then
            Me.SetFocus(ScanSkid)
            Exit Sub
        ElseIf Skid.Substring(0, 1) = "P" And Integer.TryParse(Skid.Substring(1), Test) Then
            GridHeaderPanel.Visible = True
            GridViewPanel.Visible = True
            BoxInputPanel.Visible = True

            Dim SendSkid As Integer
            Dim TempSkid As Integer
            If Integer.TryParse(Skid.Substring(1), TempSkid) Then
                SendSkid = TempSkid
            End If

            SkidBoxInvDataBind(SendSkid)
            Me.SkidNum.Text = Skid
            Me.SkidNum.BackColor = System.Drawing.Color.Yellow
            Me.ScanBox.Text = String.Empty
            Me.SetFocus(ScanBox)
        Else
            Me.ArrowErrorLabel.Font.Size = 14
            Me.ArrowErrorLabel.Text = "ERROR: SKID INPUT WAS NOT VALID"
            Me.ArrowErrorLabel.ForeColor = Color.DarkRed
            Me.ScanSkid.Text = String.Empty
            Me.SetFocus(ScanSkid)
        End If
    End Sub
    Protected Sub AddBox_Click(sender As Object, e As EventArgs) Handles AddBox.Click
        Dim Skid As String = Me.ScanSkid.Text
        If Skid.StartsWith("*") And Skid.EndsWith("*") Then
            Skid = Skid.Substring(1)
            Skid = Skid.Substring(0, Skid.Length - 1)
        End If
        Dim UpperSkid As String = Skid.Substring(0, 1).ToUpper()
        Skid = Skid.Replace(Skid.Substring(0, 1), UpperSkid)


        Dim BoxNum As String = ScanBox.Text
        If BoxNum.StartsWith("*") And BoxNum.EndsWith("*") Then
            BoxNum = BoxNum.Substring(1)
            BoxNum = BoxNum.Substring(0, BoxNum.Length - 1)
        End If
        Dim UpperBox As String = BoxNum.Substring(0, 2).ToUpper()
        BoxNum = BoxNum.Replace(BoxNum.Substring(0, 2), UpperBox)


        Dim IntTest As Integer
        If BoxNum = "" Then
            Me.ScanBox.Text = String.Empty
            Me.SetFocus(ScanBox)
            Exit Sub
        ElseIf BoxNum.Substring(0, 2) = "NC" And Integer.TryParse(BoxNum.Substring(2), IntTest) Then
            If SkidViewer.Rows.Count() >= 100 Then
                Me.ArrowErrorLabel.Font.Size = 14
                Me.ArrowErrorLabel.Text = "Error: SKID CANNOT EXCEED 100 BOXES"
                Me.ArrowErrorLabel.ForeColor = Color.DarkRed
                Me.ScanBox.Text = String.Empty
                Me.SetFocus(ScanBox)
            ElseIf CheckForBox(Integer.Parse(BoxNum.Substring(2)), Integer.Parse(Skid.Substring(1))) = True Then
                Me.ArrowErrorLabel.Font.Size = 14
                Me.ArrowErrorLabel.Text = "ERROR: BOX " & BoxNum & " CANNOT BE ADDED AGAIN"
                Me.ArrowErrorLabel.ForeColor = Color.DarkRed
                Me.ScanBox.Text = String.Empty
                Me.SetFocus(ScanBox)
            Else
                Dim IntSkid As Integer = Integer.Parse(Skid.Substring(1))
                Dim IntBox As Integer = Integer.Parse(BoxNum.Substring(2))

                Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
                connect.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
                connect.Open()

                Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand
                command.CommandText = "INSERT INTO [T_NC_Skid_BoxInv] ([NC_Skid], [NC_Inv_Box]) VALUES ('" & IntSkid & "', '" & IntBox & "')"
                command.Connection = connect
                command.ExecuteReader()
                connect.Close()

                SkidBoxInvDataBind(IntSkid)
                Me.ScanBox.Text = String.Empty
                Me.SetFocus(ScanBox)
                Me.SkidNum.Text = Skid
                Me.SkidNum.BackColor = System.Drawing.Color.Yellow
                Me.BoxNum.Text = BoxNum.Substring(2)
                Me.BoxNum.BackColor = System.Drawing.Color.LightBlue
            End If
        Else
            Me.ArrowErrorLabel.Font.Size = 14
            Me.ArrowErrorLabel.Text = "ERROR: BOX INPUT WAS NOT VALID"
            Me.ArrowErrorLabel.ForeColor = Color.DarkRed
            Me.ScanBox.Text = String.Empty
            Me.SetFocus(ScanBox)
        End If
    End Sub
    Protected Function CheckForBox(BoxNum As String, SkidNum As String) As Boolean
        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand
        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection

        connect.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        command.CommandText = "SELECT NC_Inv_Box FROM [T_NC_Skid_BoxInv] WHERE ([NC_Skid] = '" & SkidNum & "' AND [NC_Inv_Box] = '" & BoxNum & "')"
        command.Connection = connect
        Dim Box As String = "0"

        connect.Open()
        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            Box = checker(0)
        End While
        connect.Close()

        If Box IsNot "0" Then
            Return True
        End If
        Return False
    End Function
    Protected Sub SkidBoxInvDataBind(InputSkid As Integer)
        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
        connect.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        connect.Open()

        Me.SqlDataSourceSkidBox.SelectCommand = "SELECT [Key], NC_Skid, NC_Inv_Box, (SELECT SUM(Qty) AS Expr1 FROM T_NC_Box_Qty WHERE (NC_Inv_Box = T_NC_Skid_BoxInv.NC_Inv_Box)) AS Qty, TimeStamp FROM T_NC_Skid_BoxInv WHERE (NC_Skid = '" & InputSkid & "')"
        Me.SkidViewer.DataBind()
        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand

        command.CommandText = "SELECT [Key], NC_Skid, NC_Inv_Box, (SELECT SUM(Qty) AS Expr1 FROM T_NC_Box_Qty WHERE (NC_Inv_Box = T_NC_Skid_BoxInv.NC_Inv_Box)) AS Qty, TimeStamp FROM T_NC_Skid_BoxInv WHERE (NC_Skid = '" & InputSkid & "')"
        command.Connection = connect
        command.ExecuteNonQuery()
        connect.Close()
    End Sub
    Protected Sub SkidViewer_OnRowEditing(sender As Object, e As EventArgs) Handles SkidViewer.RowEditing, SkidViewer.RowCancelingEdit, SkidViewer.RowUpdated
        Dim Skid As String = Me.ScanSkid.Text()
        If Skid.StartsWith("*") And Skid.EndsWith("*") Then
            Skid = Skid.Substring(1)
            Skid = Skid.Substring(0, Skid.Length - 1)
        End If

        SkidBoxInvDataBind(Integer.Parse(Skid.Substring(1)))
        Me.ScanBox.Text = String.Empty
        Me.SetFocus(ScanBox)
    End Sub
    Protected Sub ExportButton_Click(sender As Object, e As EventArgs) Handles ExportButton.Click
        If Me.GridViewPanel.Visible = True Then
            ExportFLEXRemake()
        Else
            Me.ArrowErrorLabel.Font.Size = 14
            Me.ArrowErrorLabel.Text = "Error: Can not export; Nothing in Grid"
            Me.ArrowErrorLabel.ForeColor = Color.DarkRed
        End If
        Me.ScanBox.Text = String.Empty
        Me.SetFocus(ScanBox)
    End Sub
    Sub ExportFLEXRemake()
        Dim Flex As New FlexCel.XlsAdapter.XlsFile(True)
        Dim Path As String = "\\PWI-40\software$\LabelTemplates\Non-Conforming Skid Log.xls"
        Dim SheetCount As Integer = 0
        Dim SheetRow As Int16 = 3
        Dim SheetCol As Int16 = 2

        Flex.Open(Path)
        Flex.ActiveSheetByName = "Offsite Rejects"

        If SkidViewer.Rows.Count = 0 Then
            ExportLabel.Text = "Export Failed. No data in Grid."
            ExportLabel.ForeColor = Color.DarkRed
            Exit Sub
        Else
            Dim SheetLength As Integer
            If SizeSheet.SelectedValue = "Size?" Then
                ExportLabel.Text = "Export Failed. Select a Wafer Size."
                ExportLabel.ForeColor = Color.DarkRed
                Exit Sub
            ElseIf SizeSheet.SelectedValue = "300mm" Then
                SheetLength = 20
            ElseIf SizeSheet.SelectedValue = "200mm" Then
                SheetLength = 51
            End If

            Dim Skid As Integer = Integer.Parse(SkidViewer.Rows(0).Cells(0).Text())
            Dim Difference As Integer = 7 - Skid.ToString("D").Length
            Dim Placement As Integer = Skid.ToString("D").Length + Difference
            Flex.SetCellValue(56, 2, "P" + Skid.ToString("D" + Placement.ToString()))

            For prim As Integer = 0 To Me.SkidViewer.Rows.Count - 1
                For seco As Integer = 1 To 2
                    If SkidViewer.Rows(prim).Cells(seco).Text = "" Or SkidViewer.Rows(prim).Cells(seco).Text = "&nbsp;" Then
                        ExportLabel.Text = "Export Failed. Blank Box or Qty."
                        ExportLabel.ForeColor = Color.DarkRed
                        Exit Sub
                    Else
                        Flex.SetCellValue(SheetRow, SheetCol, Integer.Parse(SkidViewer.Rows(prim).Cells(seco).Text))
                    End If
                    SheetCol += 2
                Next
                SheetCol -= 4
                SheetRow += 2
                If SheetRow > SheetLength Then
                    If SheetCol = 2 Then
                        SheetCol = 6
                    ElseIf SheetCol = 6 Then
                        SheetCol = 10
                    ElseIf SheetCol = 10 Then
                        SheetCol = 14
                    End If
                    SheetRow = 3
                End If

                If prim = SkidViewer.Rows.Count - 1 Then
                    SaveNewExcelFile(Flex)
                End If
            Next
        End If

        ExportLabel.Text = "Export Table To Excel Document?"
        ExportLabel.ForeColor = Color.Black

    End Sub
    Sub SaveNewExcelFile(ByVal FLEX As FlexCel.XlsAdapter.XlsFile)
        Dim PathName As String = "\\57.201.101.139\TempImageWebFiles$\" 'Session("ReportFolder").ToString  '
        Dim FileName As String = "Non-Conforming Skid Log - " & User.Identity.Name.ToString & ".xls"
        Dim PF As String = PathName & FileName

        FLEX.Save(PF)

        Me.ViewExcelFile.Visible = True
        Me.ViewExcelFile.NavigateUrl = Session("ReportFolder").ToString & FileName
    End Sub
    Protected Sub SkidViewer_RowDeleted(sender As Object, e As EventArgs) Handles SkidViewer.RowDeleted
        Dim Skid As String = Me.ScanSkid.Text()
        If Skid.StartsWith("*") And Skid.EndsWith("*") Then
            Skid = Skid.Substring(1)
            Skid = Skid.Substring(0, Skid.Length - 1)
        End If

        SkidBoxInvDataBind(Integer.Parse(Skid.Substring(1)))
        Me.ScanBox.Text = String.Empty
        Me.SetFocus(ScanBox)
    End Sub
End Class