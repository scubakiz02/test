
Partial Class Reports_DayArchive
    Inherits System.Web.UI.Page

    Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
        Dim NewDate As String
        NewDate = Me.Calendar1.SelectedDate.ToString

        Me.SqlDataSource1.SelectCommand = "SELECT ID, WH, WIP, Rework, FGI FROM dbo.fctn_Sati_HistorySlice('" & NewDate & "') AS fctn_Sati_HistorySlice_1"
        Me.GridView1.DataBind()
        'SELECT ID, WH, WIP, Rework, FGI FROM dbo.fctn_Sati_HistorySlice('4/1/2007') AS fctn_Sati_HistorySlice_1
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub
End Class
