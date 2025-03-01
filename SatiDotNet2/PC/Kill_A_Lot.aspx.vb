Imports Class1
Partial Class PC_Kill_A_Lot
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If e.CommandName = "Kill" Then
            Dim row As String
            Dim Lot As String
            row = e.CommandArgument.ToString
            Lot = Me.GridView1.Rows(row).Cells(0).Text

            SatiCode.KillLot(Lot)
            Me.GridView1.DataBind()
            Message(Lot & " Was Removed")
            Me.LotTextBox.Text = ""

        End If
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim Lot As String
        Lot = Me.LotTextBox.Text
        SatiCode.KillLot(Lot)
        Me.GridView1.DataBind()
        Message(Lot & " Was Removed")
        Me.LotTextBox.Text = ""
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
End Class
