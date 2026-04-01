
Partial Class PC_WHReceivingHistory
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        If Not Me.Page.IsPostBack Then
            Me.SqlDataSource1.SelectCommand = ""
            Me.GridView1.DataBind()
        End If
       

    End Sub

    Protected Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
      Gridset

    End Sub

    Sub Gridset()
        'SelectCommand="SELECT dbo.T_WH_Invintory.EventTime AS Date, dbo.MainID.CustomerID AS Fab, dbo.T_WH_Invintory.MainID AS ID, dbo.T_WH_Invintory.Waferlog, dbo.T_WH_Invintory.Qty, dbo.T_WH_Invintory.PackingSlip, dbo.T_WH_Invintory.Carrier, dbo.T_WH_Invintory.Note FROM dbo.T_WH_Invintory INNER JOIN dbo.MainID ON dbo.T_WH_Invintory.MainID = dbo.MainID.MainID INNER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID WHERE (dbo.T_WH_Invintory.Action = N'StartWL') AND (dbo.T_WH_Invintory.EventTime > CONVERT (DATETIME, '2007-01-01 00:00:00', 102)) ORDER BY dbo.T_WH_Invintory.EventTime DESC"

        'Build SQl
        Dim SQLString As String
        SQLString = ""
        'Select
        SQLString = "SELECT dbo.T_WH_Invintory.EventTime AS Date, dbo.MainID.CustomerID AS Fab, dbo.T_WH_Invintory.MainID AS ID, dbo.T_WH_Invintory.Waferlog, dbo.T_WH_Invintory.Qty, dbo.T_WH_Invintory.PackingSlip, dbo.T_WH_Invintory.Carrier, dbo.T_WH_Invintory.Note "
        'From
        SQLString = SQLString & "FROM dbo.T_WH_Invintory INNER JOIN dbo.MainID ON dbo.T_WH_Invintory.MainID = dbo.MainID.MainID INNER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID "
        'Where
        SQLString = SQLString & "WHERE (dbo.T_WH_Invintory.Action = N'StartWL')" 'WHERE (dbo.T_WH_Invintory.Action = N'StartWL') AND (dbo.T_WH_Invintory.EventTime > CONVERT (DATETIME, '2007-01-01 00:00:00', 102)) 

        '***********************************************************
        'From Date
        If Me.CheckBoxFromDate.Checked = True Then
            SQLString = SQLString & " AND (dbo.T_WH_Invintory.EventTime >= CONVERT(DATETIME, '" & Me.TextBoxAfterDate.Text & "', 102))"
        End If

        'To Date
        If Me.CheckBoxToDate.Checked = True Then
            SQLString = SQLString & " AND (dbo.T_WH_Invintory.EventTime <= CONVERT(DATETIME, '" & Me.TextBoxBeforeDate.Text & "', 102))"
        End If

        'Size
        If Me.CheckBoxSize.Checked = True Then
            SQLString = SQLString & " AND (dbo.MainID.Diameter = " & Me.DropDownListSize.SelectedItem.Text & ")"
        End If

        'Customer
        If Me.CheckBoxCustomer.Checked = True Then
            SQLString = SQLString & " AND (dbo.Customer.Customer_Name = N'" & Me.DropDownListCustomer.SelectedItem.Text & "')"
        End If

        'Fab
        If Me.CheckBoxFab.Checked = True Then
            SQLString = SQLString & " AND (dbo.MainID.CustomerID = N'" & Me.DropDownListFab.SelectedItem.Text & "')"
        End If

        'ID
        If Me.CheckBoxID.Checked = True Then
            SQLString = SQLString & " AND (dbo.T_WH_Invintory.MainID = N'" & Me.DropDownListID.SelectedItem.Text & "')"
        End If
        '**************************************************************

        'OrderBy
        SQLString = SQLString & " ORDER BY dbo.T_WH_Invintory.EventTime DESC"

        Me.SqlDataSource1.SelectCommand = SQLString


        Me.GridView1.DataBind()
    End Sub

    Protected Sub CheckBoxFromDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxFromDate.CheckedChanged
        If Me.CheckBoxFromDate.Checked = True Then
            Me.TextBoxAfterDate.BackColor = Drawing.Color.LightGreen
        Else
            Me.TextBoxAfterDate.BackColor = Drawing.Color.White
        End If
    End Sub

    Protected Sub CheckBoxToDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxToDate.CheckedChanged
        If Me.CheckBoxToDate.Checked = True Then
            Me.TextBoxBeforeDate.BackColor = Drawing.Color.LightGreen
        Else
            Me.TextBoxBeforeDate.BackColor = Drawing.Color.White
        End If
    End Sub

    Protected Sub CheckBoxFab_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxFab.CheckedChanged
        If Me.CheckBoxFab.Checked = True Then
            Me.DropDownListFab.BackColor = Drawing.Color.LightGreen
        Else
            Me.DropDownListFab.BackColor = Drawing.Color.White
        End If
    End Sub

    Protected Sub CheckBoxID_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxID.CheckedChanged
        If Me.CheckBoxID.Checked = True Then
            Me.DropDownListID.BackColor = Drawing.Color.LightGreen
        Else
            Me.DropDownListID.BackColor = Drawing.Color.White
        End If
    End Sub


    Protected Sub CheckBoxCustomer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxCustomer.CheckedChanged
        If Me.CheckBoxCustomer.Checked = True Then
            Me.DropDownListCustomer.BackColor = Drawing.Color.LightGreen
        Else
            Me.DropDownListCustomer.BackColor = Drawing.Color.White
        End If
    End Sub

    Protected Sub CheckBoxSize_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxSize.CheckedChanged
        If Me.CheckBoxSize.Checked = True Then
            Me.DropDownListSize.BackColor = Drawing.Color.LightGreen
        Else
            Me.DropDownListSize.BackColor = Drawing.Color.White
        End If
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Gridset()
        If e.CommandName = "Select" Then
            Dim row As String = e.CommandArgument.ToString
            Dim MainID As String = Me.GridView1.Rows(row).Cells(3).Text
            Dim WL As String = Me.GridView1.Rows(row).Cells(4).Text
            Dim RecDate As String = Me.GridView1.Rows(row).Cells(1).Text
            Dim DS As New Data.DataSet
            Dim DR As Data.DataRow
            Dim Sati As String
            Dim Old As String

            'SELECT dbo.T_WH_Invintory.MainID, dbo.T_WH_Invintory.Waferlog, dbo.T_WH_Invintory.EventTime, dbo.T_WH_Invintory.Reveiving_Key AS SatiKey, dbo.ReceivingLog.Reveiving_Key AS DaveKey FROM dbo.T_WH_Invintory INNER JOIN dbo.ReceivingLog ON dbo.T_WH_Invintory.MainID = dbo.ReceivingLog.MainID AND dbo.T_WH_Invintory.Waferlog = dbo.ReceivingLog.Waferlog AND dbo.T_WH_Invintory.EventTime = dbo.ReceivingLog.EventTime WHERE (dbo.T_WH_Invintory.MainID = N'3090') AND (dbo.T_WH_Invintory.Waferlog = N'8811') AND (dbo.T_WH_Invintory.EventTime = CONVERT(DATETIME, '2016-01-05 00:00:00', 102))
            DS = SatiCode.GetMyDataSet("SELECT dbo.T_WH_Invintory.MainID, dbo.T_WH_Invintory.Waferlog, dbo.T_WH_Invintory.EventTime, dbo.T_WH_Invintory.Reveiving_Key AS SatiKey, dbo.ReceivingLog.Reveiving_Key AS DaveKey FROM dbo.T_WH_Invintory INNER JOIN dbo.ReceivingLog ON dbo.T_WH_Invintory.MainID = dbo.ReceivingLog.MainID AND dbo.T_WH_Invintory.Waferlog = dbo.ReceivingLog.Waferlog AND dbo.T_WH_Invintory.EventTime = dbo.ReceivingLog.EventTime WHERE (dbo.T_WH_Invintory.MainID = N'" & MainID & "') AND (dbo.T_WH_Invintory.Waferlog = N'" & WL & "') AND (dbo.T_WH_Invintory.EventTime = CONVERT(DATETIME, '" & RecDate & "', 102))")
            DR = DS.Tables(0).Rows(0)

            Sati = DR("SatiKey")
            Old = DR("DaveKey")

            Response.Redirect("~/PC/WH_Rec_Info.aspx?Sati_Key=" & Sati & "&Old_Key=" & Old)

        End If
    End Sub
End Class
