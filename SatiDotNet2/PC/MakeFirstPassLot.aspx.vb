Imports DBCharTableAdapters
Imports CannedPathTableAdapters
Imports UniqueprocessesTableAdapters
Imports WaferMoverTableTableAdapters
Imports WH_InvintoryTableAdapters
'Imports System.Windows.Forms

Partial Class MakeFirstPassLot
    Inherits System.Web.UI.Page
    Dim Row As String

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "FirstLot" Then
            Row = e.CommandArgument.ToString
            Session("ID") = Me.GridView1.Rows(Row).Cells(2).Text
            Session("Qty") = Me.GridView1.Rows(Row).Cells(4).Text
            Session("WL") = Me.GridView1.Rows(Row).Cells(3).Text
            Response.Redirect(Session("MakeLotPart2").ToString)
        End If

    End Sub

    Sub ChangeType()
        Dim Type As String = ""
        If Me.NRadioButton.Checked Then
            Type = "Normal"
        End If

        If Me.SRadioButton.Checked Then
            Type = "Special"
        End If

        Dim NewSQL As String = ""
        Select Case Type
            Case "Normal"
                NewSQL = "SELECT dbo.Customer.Customer_Name, dbo.MainID.CustomerID, dbo.T_WH_Invintory.MainID, SUM(dbo.T_WH_Invintory.Qty) AS Qty, dbo.T_WH_Invintory.Waferlog FROM dbo.MainID LEFT OUTER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID RIGHT OUTER JOIN dbo.T_WH_Invintory ON dbo.MainID.MainID = dbo.T_WH_Invintory.MainID GROUP BY dbo.T_WH_Invintory.MainID, dbo.MainID.CustomerID, dbo.Customer.Customer_Name, dbo.T_WH_Invintory.Waferlog HAVING (NOT (SUM(dbo.T_WH_Invintory.Qty) = 0)) ORDER BY dbo.T_WH_Invintory.MainID"
                Session("InvType") = "Normal"
            Case "Special" 'T_WH_Receivatory
                NewSQL = "SELECT dbo.Customer.Customer_Name, dbo.MainID.CustomerID, dbo.T_WH_Receivatory.MainID, SUM(dbo.T_WH_Receivatory.Qty) AS Qty, dbo.T_WH_Receivatory.Waferlog FROM dbo.MainID LEFT OUTER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID RIGHT OUTER JOIN dbo.T_WH_Receivatory ON dbo.MainID.MainID = dbo.T_WH_Receivatory.MainID GROUP BY dbo.T_WH_Receivatory.MainID, dbo.MainID.CustomerID, dbo.Customer.Customer_Name, dbo.T_WH_Receivatory.Waferlog HAVING (NOT (SUM(dbo.T_WH_Receivatory.Qty) = 0)) ORDER BY dbo.T_WH_Receivatory.MainID"
                Session("InvType") = "Special"
        End Select

        FirstPassLotIDSqlDataSource.SelectCommand = NewSQL
        Me.GridView1.DataBind()
    End Sub

    Protected Sub SRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ChangeType()
    End Sub

    Protected Sub NRadioButton_CheckedChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        ChangeType()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("PC", Server)
    End Sub
End Class

