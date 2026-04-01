
Partial Class DBMaintenance_FGICheck
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Dim ScanString As String = Me.PScanTextBox.Text.ToString
    '    Me.PScanTextBox.Text = ""
    '    Dim Strip As String
    '    Dim Marker As Int16

    '    Do
    '        If ScanString.Length < 1 Then
    '            Exit Sub
    '        End If
    '        Marker = ScanString.IndexOf(Chr(13))
    '        Strip = Left(ScanString, Marker)
    '        'clean the main string
    '        ScanString = Mid(ScanString, Marker + 3)
    '        checkFGI(Strip)
    '    Loop
    'End Sub
    'Sub checkFGI(ByVal strip As String)
    '    Dim G1C As Integer = Me.GridView1.Rows.Count
    '    Dim i As Integer = 0
    '    Dim found As Boolean = False
    '    Dim NStrip As String
    '    If Left(strip, 1) = "C" Then
    '        NStrip = Mid(strip, 2)
    '        For i = 0 To G1C - 1
    '            If Me.GridView1.Rows(i).Cells(4).Text.ToString = NStrip Then
    '                CType(Me.GridView1.Rows(i).Cells(5).FindControl("CheckBox1"), CheckBox).Checked = True
    '                found = True
    '                Exit For
    '            End If
    '        Next
    '    End If
    '    If found = False Then
    '        Me.PScanTextBox.Text = Me.PScanTextBox.Text & strip & Chr(13)
    '    End If
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        If Page.IsPostBack Then
            Me.TextBox1.Focus()
        End If


    End Sub

    Sub CleanFGI(Carton As String, MainID As String)

        SatiCode.DeleteMyAltsRecords("DELETE FROM dbo.ShippingInventory WHERE (Carton_Key = " & Carton & ")")
        'Message(Carton & " Was Removed from ShippingInventory table")

        'if it is 300mm
        If SatiCode.GetDiameter(MainID) = "300" Then
            SatiCode.BoxTableMod("Clear", Carton, 0, "", 0, 0)
            'Message(Carton & " Was Removed from FGI_Boxes table")
        End If

        Me.GridView1.DataBind()
    End Sub

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "Clear" Then
            Dim row As String
            Dim Carton As String
            Dim MainID As String
            row = e.CommandArgument.ToString
            Carton = Me.GridView1.Rows(row).Cells(4).Text
            MainID = Me.GridView1.Rows(row).Cells(0).Text

            If CType(Me.GridView1.Rows(row).Cells(5).FindControl("CheckBox1"), CheckBox).Checked = True Then
                CleanFGI(Carton, MainID)
            End If

        End If
    End Sub

    Sub Message(ByVal text As String)
        'Dim strMessage As String
        'strMessage = "Connection is Created"
        'finishes server processing, returns to client.
        'Dim strScript As String = "<script language=JavaScript>"
        'strScript += "alert(""" & text & """);"
        'strScript += "</script"
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alertMessage", "alert('" & text & "')", True)



        'If (Not ClientScript.IsStartupScriptRegistered("clientScript")) Then
        '    ClientScript.RegisterClientScriptBlock(Me.GetType(), "clientScript", strScript)
        'Else

        'End If
    End Sub



    Protected Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

        Dim mydata As String
        Dim cartonNumber As String = ""
        Dim i As Int16
        mydata = TextBox1.Text

        For i = 0 To mydata.Length - 1
            If IsNumeric(mydata.Chars(i).ToString) Then
                cartonNumber = cartonNumber & mydata.Chars(i).ToString
            End If
        Next

        For i = 0 To Me.GridView1.Rows.Count - 1
            If Me.GridView1.Rows(i).Cells(4).Text = cartonNumber Then
                CType(Me.GridView1.Rows(i).Cells(7).FindControl("CheckBox2"), CheckBox).Checked = True
                TextBox1.Text = ""
            End If
        Next


    End Sub


End Class
