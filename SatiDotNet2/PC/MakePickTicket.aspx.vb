Imports Class1
Partial Class PC_MakePickTicket
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("Shipping", Server)
    End Sub

    Protected Sub ScheduledShipmentsGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles ScheduledShipmentsGridView.RowCommand

        Dim Row As Integer
        Dim SONumber As String
        Dim TheId As String
        Dim Seq As Int16
        Dim Qty As Int16
        Dim TheDate As DateTime
        Dim PickTicket As String
        Dim Entry As String
        If e.CommandName = "MakePT" Then
            Row = e.CommandArgument.ToString

            SONumber = Me.ScheduledShipmentsGridView.Rows(Row).Cells(3).Text
            TheId = Me.ScheduledShipmentsGridView.Rows(Row).Cells(2).Text
            Qty = Me.ScheduledShipmentsGridView.Rows(Row).Cells(5).Text
            Entry = Me.ScheduledShipmentsGridView.Rows(Row).Cells(8).Text
            Try
                TheDate = Me.ScheduledShipmentsGridView.Rows(Row).Cells(6).Text
            Catch ex As Exception

            End Try


            Seq = Saticode.GetNextPickNumber(SONumber)
            Saticode.ModPickLog("Add", "0", SONumber, Seq, Qty, TheDate)

            'CType(Seq, Integer)
            Select Case Seq
                Case Is < 10
                    PickTicket = SONumber & "-00" & Seq
                Case 10 To 99
                    PickTicket = SONumber & "-0" & Seq
                Case Is > 99
                    PickTicket = SONumber & "-" & Seq
            End Select


            'Saticode.SendMail365("PickTicket " & PickTicket & " Made.", PickTicket, "Tim.Hughes@purewafer.com", "Sati@purewafer.com")


            'GEt Lots
            Dim Connection As New Data.SqlClient.SqlConnection
            Connection.ConnectionString = Session("DBConnect")
            Connection.Open()

            Dim DA As New Data.SqlClient.SqlDataAdapter
            Dim DS As New Data.DataSet
            Dim DR As Data.DataRow

            Dim SelectCmd As New System.Data.SqlClient.SqlCommand

            With SelectCmd
                .CommandText = "SELECT TOP 100 PERCENT dbo.LabelsMade.Lot, SUM(dbo.ShippingInventory.Total_Qty) AS FGI, dbo.SO_LineItems.SO FROM dbo.ShippingInventory INNER JOIN dbo.LabelsMade ON dbo.ShippingInventory.LotEntry = dbo.LabelsMade.LabelRecordNumber INNER JOIN dbo.MainID ON LEFT(dbo.LabelsMade.Lot, 4) = dbo.MainID.MainID INNER JOIN dbo.SO_LineItems ON dbo.LabelsMade.SO_Key = dbo.SO_LineItems.[Key] GROUP BY dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.ShippingInventory.Confirmed, dbo.LabelsMade.Lot HAVING (dbo.ShippingInventory.Confirmed IS NULL) AND (dbo.MainID.MainID = N'" & TheId & "') AND (dbo.SO_LineItems.SO = N'" & SONumber & "') ORDER BY dbo.MainID.MainID"
                .Connection = Connection
            End With
            DA.SelectCommand = SelectCmd

            DA.Fill(DS)
            Connection.Close()


            Dim Flex As New FlexCel.XlsAdapter.XlsFile(True)
            Dim Path As String = "\\PWI-40\software$\LabelTemplates\SatiPickTicket.xls"

            Flex.Open(Path)
            Flex.Recalc(True)
            Flex.ActiveSheetByName = "DataInfo"
            Flex.SetCellValue(2, 2, User.Identity.Name.ToString) 'Issued By
            Flex.SetCellValue(3, 2, DateTime.Now.Date) 'Issued Date:
            Flex.SetCellValue(4, 2, DateTime.Now.TimeOfDay) 'Issued Time
            Flex.SetCellValue(5, 2, PickTicket) 'Pick Ticket#
            Flex.SetCellValue(6, 2, TheDate) 'Dock Date:
            Flex.SetCellValue(7, 2, TheId) 'The ID:

            Flex.ActiveSheetByName = "PickTicket"

            For i As Int16 = 0 To DS.Tables(0).Rows.Count - 1
                DR = DS.Tables(0).Rows(i)
                Flex.SetCellValue(11 + i, 2, DR("Lot").ToString)
                Flex.SetCellValue(11 + i, 1, DR("FGI").ToString)
                If i > 45 Then
                    Flex.SetCellValue(11 + (i - 45), 7, DR("Lot").ToString)
                    Flex.SetCellValue(11 + (i - 45), 6, DR("FGI").ToString)
                End If
            Next
            Flex.ConvertFormulasToValues(True)
            Flex.Recalc()
            Flex.PrintOptions = FlexCel.Core.TPrintOptions.None
            Flex.Save("\\PWI-40\LabelArchive$\PickTickets\" & PickTicket & ".xls")

            'Dim printlabel As New FlexCel.Render.FlexCelPrintDocument(Flex)
            'With printlabel
            '.PrinterSettings.Copies = 1
            '.Print()
            '.Dispose()
            'End With
            Me.PanelLink.Visible = True
            Saticode.ChangeSalesSchedule(Entry, "PickTicket", PickTicket)
            'Me.HyperLinkPickticket.NavigateUrl = "\\PWI-40\LabelArchive$\PickTickets\" & PickTicket & ".xls"
            Me.HyperLinkPickticket.NavigateUrl = "http://pwi-40:81/LabelTemp/PickTickets/" & PickTicket & ".xls"
        End If

    End Sub




    Protected Sub ButtonRunReport_Click(sender As Object, e As EventArgs) Handles ButtonRunReport.Click
        Dim ReportName As String

        ReportName = Saticode.Make_Current_SO_Report()
        Me.HyperLinkReport.Visible = True
        Me.HyperLinkReport.NavigateUrl = Session("ReportFolder") & ReportName
    End Sub
End Class
