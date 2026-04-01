Imports Class1


Imports System ' for text file stuff
Imports System.IO ' for text file stuff
Imports System.Web.Management
'IManagementUIService


Partial Class TestArea_TestMain
    Inherits System.Web.UI.Page

    Dim SatiCode As New Class1


 

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Call SatiCode.Message("Test Message fot the user")
        Message("Final Test")


    End Sub

    Sub Message(ByVal text As String)
        Dim strMessage As String
        strMessage = "Connection is Created"
        'finishes server processing, returns to client.
        Dim strScript As String = "<script language=JavaScript>"
        strScript += "alert(""" & text & """);"
        strScript += "</script"

        If (Not ClientScript.IsStartupScriptRegistered("clientScript")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "clientScript", strScript)
        End If

    End Sub


    Protected Sub MicronTestButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MicronTestButton.Click
        Dim Report As String
        Report = SatiCode.microntest(Me.FileNameTextBox.Text)

        If Report = "" Then
            Message("Check MicronDataPacks Folder For Your File")
        Else
            Message(Report)
        End If

    End Sub

    Protected Sub TryButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TryButton.Click
        Me.CSTextBox.Text = SatiCode.CheckSum(Me.M12TextBox.Text)
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.TextBox1.Text = SatiCode.GetNumber(Me.RandomUpperTextBox.Text, Me.RandomLowerTextBox.Text, Me.SoftmultiTextBox.Text)
        System.Threading.Thread.Sleep(200)
        Me.TextBox2.Text = SatiCode.GetNumber(Me.RandomUpperTextBox.Text, Me.RandomLowerTextBox.Text, Me.SoftmultiTextBox.Text)
        System.Threading.Thread.Sleep(200)
        Me.TextBox3.Text = SatiCode.GetNumber(Me.RandomUpperTextBox.Text, Me.RandomLowerTextBox.Text, Me.SoftmultiTextBox.Text)
        System.Threading.Thread.Sleep(200)
        Me.TextBox4.Text = SatiCode.GetNumber(Me.RandomUpperTextBox.Text, Me.RandomLowerTextBox.Text, Me.SoftmultiTextBox.Text)
        System.Threading.Thread.Sleep(200)
        Me.TextBox5.Text = SatiCode.GetNumber(Me.RandomUpperTextBox.Text, Me.RandomLowerTextBox.Text, Me.SoftmultiTextBox.Text)
        System.Threading.Thread.Sleep(200)
        Me.TextBox6.Text = SatiCode.GetNumber(Me.RandomUpperTextBox.Text, Me.RandomLowerTextBox.Text, Me.SoftmultiTextBox.Text)
        System.Threading.Thread.Sleep(200)
        Me.TextBox7.Text = SatiCode.GetNumber(Me.RandomUpperTextBox.Text, Me.RandomLowerTextBox.Text, Me.SoftmultiTextBox.Text)
        System.Threading.Thread.Sleep(200)
        Me.TextBox8.Text = SatiCode.GetNumber(Me.RandomUpperTextBox.Text, Me.RandomLowerTextBox.Text, Me.SoftmultiTextBox.Text)
        System.Threading.Thread.Sleep(200)
        Me.TextBox9.Text = SatiCode.GetNumber(Me.RandomUpperTextBox.Text, Me.RandomLowerTextBox.Text, Me.SoftmultiTextBox.Text)
        System.Threading.Thread.Sleep(200)
        Me.TextBox10.Text = SatiCode.GetNumber(Me.RandomUpperTextBox.Text, Me.RandomLowerTextBox.Text, Me.SoftmultiTextBox.Text)

    End Sub

    Protected Sub RandomUpperTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub CB_BoxButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CB_BoxButton.Click
        Me.ProblemLabel.Text = ""
        If Me.PreRadioButton.Checked = True Then
            Me.ProblemLabel.Text = SatiCode.CBFullDataRecordCheck(Me.CBBoxTextBox.Text, "PreGeo")
        End If
        If Me.PostRadioButton.Checked = True Then
            Me.ProblemLabel.Text = SatiCode.CBFullDataRecordCheck(Me.CBBoxTextBox.Text, "PostGeo")
        End If
        If Me.ParticalRadioButton.Checked = True Then
            Me.ProblemLabel.Text = SatiCode.CBFullDataRecordCheck(Me.CBBoxTextBox.Text, "Partical")
        End If
        If Me.AllRadioButton.Checked = True Then
            Me.ProblemLabel.Text = SatiCode.CBFullDataRecordCheck(Me.CBBoxTextBox.Text, "All")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub GetAddessButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GetAddessButton.Click

        Dim DS As Data.DataSet
        Dim DR As Data.DataRow
        If Me.RadioButtonNormal.Checked = True Then
            Dim AddressType As String
            If Me.RadioButtonShip.Checked = True Then
                AddressType = "Shipping"
            Else
                AddressType = "Billing"
            End If
            DS = SatiCode.GetAddress(AddressType, Me.IDTextBox.Text, "")
        Else
            DS = SatiCode.GetAddress("", "", Me.KeyTextBox.Text)

        End If
        DR = DS.Tables(0).Rows(0)
        Me.AddressTextBox.Text = DR("Row1").ToString & ", " & DR("Row2").ToString & ", " & DR("Row3").ToString
    End Sub


    Protected Sub PopUpButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ModalPopupExtender1.Show()

    End Sub

    Protected Sub GridView2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView2.RowCommand
        Dim test As String
        test = e.CommandArgument.ToString
        test = e.CommandName.ToString
        test = e.ToString

    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        Response.Redirect("c:\New Text Document.txt")
        'Response.Redirec()


    End Sub

    Protected Sub Button5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button5.Click
        Me.FormChange.Enabled = True
        Me.FormChange.EnableViewState = True


    End Sub

    
    Protected Sub GridView3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView3.SelectedIndexChanged
        DetailsView1.ChangeMode(DetailsViewMode.ReadOnly)
        Me.DetailsView1.DataSourceID = Me.GridView1.SelectedIndex
    End Sub

    Protected Sub Button6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button6.Click
        SatiCode.CB_CheckAndFix_Geo(Me.TextBoxcb.Text)
    End Sub

    Protected Sub Button7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button7.Click
        Dim Report As String
        Report = SatiCode.WaferTechXMLTest(Me.FileNameTextBox0.Text)

        If Report = "" Then
            Message("Check WaferTechDataPacks Folder For Your File")
        Else
            Message(Report)
        End If


    End Sub

    Protected Sub Button8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button8.Click
        Me.SatiCode.CofALotMetals(Me.TextBox11.Text)
    End Sub

    Protected Sub Button9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button9.Click
        Me.SatiCode.IntelDataPackTest(Me.TextBox12.Text)
    End Sub

    Protected Sub Button10_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button10.Click
        Dim filesnames() As String
        filesnames = System.IO.Directory.GetFiles("\\pw-9001\c$\Test\")
        If Not filesnames.Count = 0 Then
            Me.TextBox13.Text = "files: " & filesnames.Count
            Exit Sub
        Else
            Me.TextBox13.Text = "No files: " & filesnames.Count
        End If
    End Sub

    Protected Sub Button1CheckRFID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1CheckRFID.Click
        Me.TextBoxRFIDFind.Text = SatiCode.Check_RFID_Used_Last20Days(Me.TextBoxRFID.Text, Me.TextBoxRFIDDateTime.Text).ToString

    End Sub

    Protected Sub Button11_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button11.Click
        SatiCode.SendMail_To_From("Sati just blocked a label from being made due to an RFID tag that has been used within the last 20 days. RFID# ****Test Alert**** ", "RFID Lock", "WRAZMGMNT@purewaferinc.com", "Sati@purewaferinc.com")
    End Sub
    Protected Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click

    End Sub
    Protected Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        SatiCode.MaintenanceRequestCloseTest("53878") '53878 4874

    End Sub
End Class


