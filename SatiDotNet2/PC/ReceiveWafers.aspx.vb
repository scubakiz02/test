Imports WH_InvintoryTableAdapters
Imports DBCharTableAdapters
Imports OldRecivingLogTableAdapters
Imports Class1

Partial Class ReceiveWafers
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1

    Protected Sub CustomerDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomerDropDownList.SelectedIndexChanged
        Dim NewSQL As String
        Dim Customer As String = Me.CustomerDropDownList.SelectedItem.Text
        'NewSQL = "SELECT DISTINCT dbo.Customer.Customer_Name, dbo.MainID.MainID FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID WHERE (dbo.MainID.[In-Out] = 1) AND (dbo.Customer.Customer_Name = N'" & Customer & "')"
        NewSQL = "SELECT DISTINCT dbo.Customer.Customer_Name, dbo.MainID.MainID, dbo.MainIDSpec.PART_NUMBER, dbo.MainID.CustomerID, dbo.MainID.MainID + N',  ' + dbo.MainIDSpec.PART_NUMBER + N',  ' + dbo.Customer.CustomerID AS [ID, Part, Fab] FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID INNER JOIN dbo.MainID_MainIDSpec ON dbo.MainID.MainID = dbo.MainID_MainIDSpec.MainID INNER JOIN dbo.MainIDSpec ON dbo.MainID_MainIDSpec.WaferSpec_Key = dbo.MainIDSpec.RecordNumber WHERE (dbo.MainID.[In-Out] = 1) AND (dbo.MainID_MainIDSpec.EffectiveDtd < { fn NOW() }) AND (dbo.MainID_MainIDSpec.ExpirationDtd IS NULL OR dbo.MainID_MainIDSpec.ExpirationDtd > { fn NOW() }) AND (dbo.Customer.Customer_Name = N'" & Customer & "')"
        SqlDataSourceID.SelectCommand = NewSQL
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Me.Button1.Text = "Exit" Then
            Response.Redirect("MainPC.aspx")
        End If

        If Me.Button1.Text = "Enter" Then
            Me.Button1.Text = "Exit"
            Dim WareHouseInvTable As New T_WH_InvintoryTableAdapter
            Dim WLTable As New DB_CharacteristicsTableAdapter
            Dim RS_WL As Data.DataRow
            RS_WL = WLTable.GetWaferLog.Rows(0)
            Dim NewWLNumber As String = RS_WL("Value") + 1

            If Me.NormalInvRadioButton.Checked = True Then
                'Add the waferlog in to the new Warehouse inv
                WareHouseInvTable.InsertWaferLog(Me.IDDropDownList.SelectedValue,
                Right(NewWLNumber, 4), "StartWL", Me.WaferQtyTextBox.Text,
                Me.PackingSlipTextBox.Text,
                Me.CarrierDropDownList.SelectedValue, Me.NoteTextBox.Text,
                Me.ContanmentTypeDropDownList.SelectedValue,
                Me.ContainmentQtyTextBox.Text, User.Identity.Name.ToString, System.DateTime.Now.ToShortDateString)

                'Add the waferlog to the old reciving log
                Dim oldLog As New ReceivingLogTableAdapter
                oldLog.InsertQuery(Me.IDDropDownList.SelectedValue, Right(NewWLNumber, 4), "Receiving Log", Me.WaferQtyTextBox.Text, True, Me.PackingSlipTextBox.Text,
                Me.CarrierDropDownList.SelectedValue, Me.NoteTextBox.Text, Me.ContanmentTypeDropDownList.SelectedValue, Me.ContainmentQtyTextBox.Text,
                User.Identity.Name.ToString, System.DateTime.Now.ToShortDateString)

            Else
                Saticode.ReceivatoryWafers("StartWL", Me.IDNoteTextBox.Text, Right(NewWLNumber, 4),
                Me.WaferQtyTextBox.Text, Me.PackingSlipTextBox.Text, Me.CarrierDropDownList.SelectedValue,
                Me.NoteTextBox.Text, Me.ContanmentTypeDropDownList.SelectedValue, Me.ContainmentQtyTextBox.Text, User.Identity.Name.ToString)

            End If


            Me.WLLabel.Visible = True
            Me.WLLabel.Text = "Your Wafer Log is " & Right(NewWLNumber, 4)



            WLTable.UpdateWaferLog("Waferlog", NewWLNumber, "Waferlog", RS_WL("Value"))
            RS_WL.BeginEdit()
            RS_WL("Value") = NewWLNumber
            RS_WL.EndEdit()


        End If



    End Sub

    Protected Sub NormalInvRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        WhatInv()
    End Sub

    Protected Sub SpecialInvRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        WhatInv()
    End Sub

    Sub WhatInv()
        If Me.SpecialInvRadioButton.Checked = True Then
            Me.SpecialPanel.Visible = True
            Me.NormalPanel.Visible = False
        End If

        If Me.NormalInvRadioButton.Checked = True Then
            Me.NormalPanel.Visible = True
            Me.SpecialPanel.Visible = False
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("Receiving", Server)
    End Sub
End Class
