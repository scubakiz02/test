Imports WH_InvintoryTableAdapters

Partial Class WHInventory
    Inherits System.Web.UI.Page

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "" Then
            Session("ID") = e.CommandArgument.ToString
            RePopGrid2()
        End If
        Me.IDTextBox.Text = Session("ID").ToString
    End Sub

    Protected Sub GridView2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView2.RowCommand
        Dim Row As String = e.CommandArgument.ToString
        Dim WareHouseInvTable As New T_WH_InvintoryTableAdapter
        Dim WL As String
        Dim Qty As String
        WL = Me.GridView2.Rows(Row).Cells(0).Text

        Select Case e.CommandName.ToString
            Case "Zero"
                Qty = "-" & Me.GridView2.Rows(Row).Cells(1).Text
                WareHouseInvTable.InsertTransaction(Session("ID").ToString, WL, "Correction", Qty, System.DateTime.Now.ToShortDateString, User.Identity.Name.ToString, System.DateTime.Now.ToShortDateString)
                ChangeType()
            Case "Add"


            Case "Adj"

                Qty = InputBox("How Much Do you want to adjust? ")
                WareHouseInvTable.InsertTransaction(Session("ID").ToString, WL, "Adj", Qty, System.DateTime.Now.ToShortDateString, User.Identity.Name.ToString, System.DateTime.Now.ToShortDateString)
                ChangeType()
        End Select
    End Sub
    Sub RePopGrid2()
        Dim NewSQL As String
        NewSQL = "SELECT Waferlog, SUM(Qty) AS Qty FROM dbo.T_WH_Invintory GROUP BY MainID, Waferlog HAVING (MainID = N'" & Session("ID").ToString & "') AND (NOT (SUM(Qty) = 0))"
        SqlDataSource2.SelectCommand = NewSQL
        Me.GridView1.DataBind()
        ChangeType()
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
                NewSQL = "SELECT dbo.Customer.Customer_Name, dbo.MainID.CustomerID, dbo.T_WH_Invintory.MainID, SUM(dbo.T_WH_Invintory.Qty) AS Qty FROM dbo.MainID LEFT OUTER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID RIGHT OUTER JOIN dbo.T_WH_Invintory ON dbo.MainID.MainID = dbo.T_WH_Invintory.MainID GROUP BY dbo.T_WH_Invintory.MainID, dbo.MainID.CustomerID, dbo.Customer.Customer_Name ORDER BY dbo.T_WH_Invintory.MainID"
            Case "Special"
                NewSQL = "SELECT dbo.Customer.Customer_Name, dbo.MainID.CustomerID, dbo.T_WH_Receivatory.MainID, SUM(dbo.T_WH_Receivatory.Qty) AS Qty FROM dbo.MainID LEFT OUTER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID RIGHT OUTER JOIN dbo.T_WH_Receivatory ON dbo.MainID.MainID = dbo.T_WH_Receivatory.MainID GROUP BY dbo.T_WH_Receivatory.MainID, dbo.MainID.CustomerID, dbo.Customer.Customer_Name ORDER BY dbo.T_WH_Receivatory.MainID"
        End Select

        SqlDataSource1.SelectCommand = NewSQL
        Me.GridView1.DataBind()
    End Sub

    Protected Sub NRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ChangeType()
    End Sub

    Protected Sub RadioButton2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ChangeType()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub
End Class
