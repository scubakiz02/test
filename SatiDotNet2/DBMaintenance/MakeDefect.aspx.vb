Imports ID_DefectsTableAdapters
Imports MainIDTableAdapters
Partial Class DBMaintenance_MakeDefect
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim IdDefects As New T_ID_DefectsTableAdapter
        Dim IDs As New MainIDTableAdapter
        Dim IDCount, i As Integer
        Dim TheId As String
        IDCount = IDs.GetData.Rows.Count - 1
        Dim TheDefect As String = Me.DefectNameTextBox.Text
        Dim TheType As String = Me.TypeDropDownList.SelectedValue.ToString
        Dim TheGroup As String
        If Me.GroupDropDownList.Visible = False Then
            TheGroup = "Reject"
        Else
            TheGroup = Me.GroupDropDownList.SelectedValue.ToString
        End If
        For i = 0 To IDCount
            TheId = IDs.GetData.Rows(i).Item("MainID").ToString
            IdDefects.InsertDefectsRecords(TheId, TheDefect, TheType, TheGroup)
        Next


    End Sub

    Protected Sub TypeDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TypeDropDownList.SelectedIndexChanged
        If Me.TypeDropDownList.Text = "Rework" Then
            Me.GroupDropDownList.Visible = True
            Me.GroupLabel.Visible = True
        Else
            Me.GroupDropDownList.Visible = False
            Me.GroupLabel.Visible = False
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub
End Class
