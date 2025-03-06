
Partial Class Reports_Defects_ByIDs_ByDateRange
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub
    Protected Sub ButtonRun_Click(sender As Object, e As EventArgs) Handles ButtonRun.Click
        If Me.TextBox1.Text = "" Then
            Exit Sub
        End If
        If Me.TextBoxDateStart.Text = "" Then
            Exit Sub
        End If
        If Me.TextBoxDateEnd.Text = "" Then
            Exit Sub
        End If

        Dim SQLString As String = "SELECT TOP (100) PERCENT dbo.DefectTracking.DefectName, dbo.DefectTracking.Location AS code, SUM(dbo.DefectTracking.Qty) AS QTY_Sum FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry"
        Dim ID_List As String = Me.TextBox1.Text
        If Not ID_List.EndsWith(";") Then
            ID_List = ID_List & ";"
        End If
        Dim M As Boolean = False

        Do While ID_List.Contains(";")
            If M = False Then
                SQLString = SQLString & " WHERE (dbo.UniqueProcesses.LotEntry LIKE N'" & Mid(ID_List, 1, ID_List.IndexOf(";")) & "-%'"
                ID_List = Mid(ID_List, ID_List.IndexOf(";") + 2)
                M = True
            Else
                SQLString = SQLString & " OR dbo.UniqueProcesses.LotEntry LIKE N'" & Mid(ID_List, 1, ID_List.IndexOf(";")) & "-%'"
                ID_List = Mid(ID_List, ID_List.IndexOf(";") + 2)
            End If
        Loop

        SQLString = SQLString & ")"

        SQLString = SQLString & " AND (dbo.UniqueProcesses.Complete >= CONVERT(DATETIME, '" & Me.TextBoxDateStart.Text & " 00:00:00', 102)) AND (dbo.UniqueProcesses.Complete < CONVERT(DATETIME, '" & Me.TextBoxDateEnd.Text & " 23:59:59', 102))"
        SQLString = SQLString & " GROUP BY dbo.DefectTracking.DefectName, dbo.DefectTracking.Location ORDER BY QTY_Sum DESC"

        Me.SqlDataSource1.SelectCommand = SQLString
        Me.GridView1.DataBind()

    End Sub
End Class
