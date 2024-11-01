
Partial Class Production_No_LPD_Data_300mm_WaferBox_Label
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim Qty As Integer = 25

    Protected Sub MakeLabelButton_Click(sender As Object, e As EventArgs) Handles MakeLabelButton.Click
        Dim File As String

        File = SatiCode.MakeLabel(False, "WB", "PWC", Mid(LotNumberTextBox.Text, 1, 4), LotNumberTextBox.Text, Qty, 1, Me.InstanceNumberTextBox.Text, Me.PrinterDropDownList.SelectedValue, "", 0, "", "", New Data.DataSet, "WB", "", User.Identity.Name.ToString, False, 0)

        Me.LotNumberTextBox.Text = ""
        Me.InstanceNumberTextBox.Text = ""
        Me.MakeLabelButton.Text = ""
        Me.MakeLabelButton.Visible = False
        CheckInputs()
        FeedBackLabel.Text = File
    End Sub
    Protected Sub LotNumberTextBox_TextChanged(sender As Object, e As EventArgs) Handles LotNumberTextBox.TextChanged
        CheckInputs()
    End Sub
    Protected Sub InstanceNumberTextBox_TextChanged(sender As Object, e As EventArgs) Handles InstanceNumberTextBox.TextChanged
        CheckInputs()
    End Sub

    Sub CheckInputs()
        Dim DS As Data.DataSet
        Dim DR As Data.DataRow
        Dim Ready As Int16
        Ready = 0


        Try
            LotNumberTextBox.BackColor = Drawing.Color.Red
            '************Check to make sure Lot Number is REAL*******************
            If SatiCode.IsLotNumberReal(Me.LotNumberTextBox.Text) = True Then
                LotNumberTextBox.BackColor = Drawing.Color.Lime
                Ready = Ready + 1
            Else
                LotNumberTextBox.BackColor = Drawing.Color.Red
            End If
        Catch ex As Exception
            LotNumberTextBox.BackColor = Drawing.Color.Red
        End Try

        Try
            InstanceNumberTextBox.BackColor = Drawing.Color.Red
            'SELECT InstanceID, COUNT(Slot) AS Qty FROM dbo.T7_InstanceInfo GROUP BY InstanceID HAVING (InstanceID = 159644)
            DS = SatiCode.GetMyDataSet("SELECT InstanceID, COUNT(Slot) AS Qty FROM dbo.T7_InstanceInfo GROUP BY InstanceID HAVING (InstanceID = " & InstanceNumberTextBox.Text & ")")
            DR = DS.Tables(0).Rows(0)
            If DS.Tables(0).Rows.Count = 1 Then
                Qty = DR("Qty")
                InstanceNumberTextBox.BackColor = Drawing.Color.Lime
                Ready = Ready + 1
            Else
                InstanceNumberTextBox.BackColor = Drawing.Color.Red
            End If
        Catch ex As Exception
            InstanceNumberTextBox.BackColor = Drawing.Color.Red
        End Try


        If Ready = 2 Then
            MakeLabelButton.Visible = True
            MakeLabelButton.Text = "Make Label for Lot# " & Me.LotNumberTextBox.Text & " From Instance# " & Me.InstanceNumberTextBox.Text & " For " & DR("Qty").ToString & " Wafers"
        Else
            MakeLabelButton.Visible = False
        End If
        FeedBackLabel.Text = ""

    End Sub

End Class
