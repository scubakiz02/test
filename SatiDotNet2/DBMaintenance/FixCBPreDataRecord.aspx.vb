Imports Class1

Partial Class DBMaintenance_FixCBPreDataRecord
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1

    Sub refreshGrid()
        Me.SqlDataSource1.SelectCommand = "SELECT dbo.T7_InstanceInfo.Slot, dbo.T7_InstanceInfo.WAT_Key, dbo.T7_WaferActionTracking.PreGeo_Key, dbo.T7_GeoData.CenterThick AS PreThick, dbo.T7_WaferActionTracking.PostGeo_Key, T7_GeoData_1.CenterThick AS PostThick, dbo.T7_GeoData.CenterThick - T7_GeoData_1.CenterThick AS Removal FROM dbo.T7_GeoData AS T7_GeoData_1 RIGHT OUTER JOIN dbo.T7_WaferActionTracking INNER JOIN dbo.T7_InstanceInfo ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key INNER JOIN dbo.T_FGI_Boxes ON dbo.T7_InstanceInfo.InstanceID = dbo.T_FGI_Boxes.InstanceKey ON T7_GeoData_1.Geo_Key = dbo.T7_WaferActionTracking.PostGeo_Key LEFT OUTER JOIN dbo.T7_GeoData ON dbo.T7_WaferActionTracking.PreGeo_Key = dbo.T7_GeoData.Geo_Key WHERE (dbo.T_FGI_Boxes.CartonNumber = " & Me.CartonTextBox.Text & ") ORDER BY dbo.T7_InstanceInfo.Slot"

    End Sub

    Protected Sub ChangeButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim RowCount As Int16 = Me.GridView1.Rows.Count
        Dim GeoKey As Integer = 0
        Dim Val As Double = 0.0
        Dim Wat As Integer = 0
        Dim TheCell As String
        For i As Int16 = 0 To RowCount - 1
            If Not CType(Me.GridView1.Rows(i).Cells(1).FindControl("Textbox1"), TextBox).Text = "" Then
                TheCell = Me.GridView1.Rows(i).Cells(2).Text.ToString
                If Me.GridView1.Rows(i).Cells(2).Text.ToString = "None" Then 'Make New Recored
                    GeoKey = Me.GridView1.Rows(i).Cells(7).Text
                    Val = CType(Me.GridView1.Rows(i).Cells(1).FindControl("Textbox1"), TextBox).Text
                    Wat = Me.GridView1.Rows(i).Cells(5).Text
                    Saticode.EditPreDataThick(Wat, 0, GeoKey, Val)
                Else 'mod old record
                    GeoKey = Me.GridView1.Rows(i).Cells(6).Text
                    Val = CType(Me.GridView1.Rows(i).Cells(1).FindControl("Textbox1"), TextBox).Text
                    Wat = Me.GridView1.Rows(i).Cells(5).Text
                    Saticode.EditPreDataThick(Wat, GeoKey, 0, Val)
                End If

            End If
        Next
        refreshGrid()
    End Sub

    Protected Sub CartonTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        refreshGrid()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub

    Protected Sub GridView1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.Load
        
    End Sub
End Class
