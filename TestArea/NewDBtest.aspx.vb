Imports Class1

Partial Class TestArea_NewDBtest
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1

    Sub Test()
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim SQLString As String = "SELECT ChildLotNum, ParentLotNum, C_Order, P_Order, Qty, Action, Action_Key, Operator, EventTime, Error FROM dbo.ActionTracker WHERE (EventTime < CONVERT(DATETIME, '2012-02-02 00:00:00', 102))"

        Connection.ConnectionString = Session("SatiDB")
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = SQLString
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        'DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T7_InstanceInfo", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("InstanceID", "InstanceID"), New System.Data.Common.DataColumnMapping("Slot", "Slot"), New System.Data.Common.DataColumnMapping("T7", "T7"), New System.Data.Common.DataColumnMapping("Seq", "Seq")})})

        DA.Fill(DS)
        Connection.Close()

        GridView1.DataSource = DS
        GridView1.DataBind()

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Test()
    End Sub
End Class
