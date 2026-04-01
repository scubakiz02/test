Imports class1
Partial Class DBMaintenance_CopyProcessInfoRecord
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        System.Threading.Thread.Sleep(500)
        Dim FromRecordNumber As Integer
        Dim ToIDNumber As String
        FromRecordNumber = Me.From_ID_DropDownList.SelectedValue
        ToIDNumber = Me.TO_ID_DropDownList.SelectedItem.Text
        SatiCode.CopyProcessIDRecords(FromRecordNumber, ToIDNumber)

        Me.AvalibleIDsSqlDataSource.SelectCommand = "SELECT dbo.MainID.MainID FROM dbo.MainID LEFT OUTER JOIN dbo.PROCESS_INFO ON dbo.MainID.MainID = dbo.PROCESS_INFO.ID_NUMBER WHERE (dbo.PROCESS_INFO.ID_NUMBER IS NULL)"
        Me.TO_ID_DropDownList.DataBind()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub
End Class
