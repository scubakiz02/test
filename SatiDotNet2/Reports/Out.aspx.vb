
Partial Class Reports_Out
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        LoadData()
    End Sub

    Sub LoadData()
        Dim DS As New Data.DataSet
        Dim MySQL As String

        Select Case Me.DropDownList1.SelectedItem.Text
            Case "Daily"
                MySQL = "SELECT TheDate, Passed, [200Total], [300Total], [200Target], [300Target] FROM dbo.Q_BI_LaserOutDD"

            Case "Weekly"
                MySQL = "SELECT TheDate, Passed, [200Total], [300Total], [200Target], [300Target] FROM dbo.Q_BI_LaserOutWW"

            Case Else
                Exit Sub
        End Select
        DS = SatiCode.GetMyDataSet(MySQL)

        Chart1.Series("Series200mmTarget").XValueMember = "TheDate"
        Chart1.Series("Series200mmTarget").XValueType = DataVisualization.Charting.ChartValueType.DateTime
        Chart1.Series("Series200mmTarget").YValueMembers = "200Target"

        Chart1.Series("Series200mmOut").XValueMember = "TheDate"
        Chart1.Series("Series200mmOut").XValueType = DataVisualization.Charting.ChartValueType.DateTime
        Chart1.Series("Series200mmOut").YValueMembers = "200Total"

        Chart1.Series("Series300mmTarget").XValueMember = "TheDate"
        Chart1.Series("Series300mmTarget").XValueType = DataVisualization.Charting.ChartValueType.DateTime
        Chart1.Series("Series300mmTarget").YValueMembers = "300Target"

        Chart1.Series("Series300mmOut").XValueMember = "TheDate"
        Chart1.Series("Series300mmOut").XValueType = DataVisualization.Charting.ChartValueType.DateTime
        Chart1.Series("Series300mmOut").YValueMembers = "300Total"

        Me.Chart1.ChartAreas(0).AxisX.IntervalAutoMode = DataVisualization.Charting.IntervalAutoMode.FixedCount

        Me.Chart1.ChartAreas(0).AxisX.Interval = 1
        Me.Chart1.ChartAreas(0).AxisX.IsReversed = True
        Me.Chart1.Titles(0).Text = Me.DropDownList1.SelectedItem.Text

        Me.Chart1.DataSource = DS
        Me.Chart1.DataBind()


    End Sub

    Private Sub Reports_Out_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)

        Try
            If Not Page.IsPostBack Then
                If Not Request.QueryString("Range") = "" Then
                    Me.DropDownList1.SelectedItem.Text = Request.QueryString("Range")
                    Me.DropDownList1.Enabled = False
                End If
            End If

            '?Range=Daily
            '?Range=Weekly
        Catch ex As Exception

        End Try


        LoadData()
    End Sub
End Class
