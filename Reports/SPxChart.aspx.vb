
Partial Class Reports_SPxChart
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub

    Sub TestIt()
        'SELECT Machine, SD, Wafers, [Station 1], [Station 2], [Station 3], Lot, ID# FROM dbo.Q_Bi_SPx_BinFall WHERE (Machine = N'SP3-2110224')
        Dim DS As New Data.DataSet
        DS = SatiCode.GetMyDataSetAutoData("SELECT top (40) SD, Wafers, [Station 1], [Station 2], [Station 3], Lot, ID#  FROM dbo.Q_Bi_SPx_BinFall WHERE (Machine = N'" & Me.DropDownList1.SelectedValue & "') ORDER BY SD DESC")

        Chart1.Series("Series1").XValueMember = "SD"
        Chart1.Series("Series1").XValueType = DataVisualization.Charting.ChartValueType.DateTime
        Chart1.Series("Series1").YValueMembers = "Station 1"

        Chart1.Series("Series2").XValueMember = "SD"
        Chart1.Series("Series2").YValueMembers = "Station 2"
        Chart1.Series("Series2").XValueType = DataVisualization.Charting.ChartValueType.DateTime

        Chart1.Series("Series3").XValueMember = "SD"
        Chart1.Series("Series3").YValueMembers = "Station 3"
        Chart1.Series("Series3").XValueType = DataVisualization.Charting.ChartValueType.DateTime


        Me.Chart1.ChartAreas(0).AxisY.Maximum = 25
        Me.Chart1.ChartAreas(0).AxisY.Minimum = 0
        Me.Chart1.ChartAreas(0).AxisY.MajorTickMark.Interval = 5
        Me.Chart1.ChartAreas(0).AxisY.MajorGrid.Interval = 10

        'Me.Chart1.ChartAreas(0).AxisX.Minimum = 5

        Me.Chart1.ChartAreas(0).AxisX.IntervalAutoMode = DataVisualization.Charting.IntervalAutoMode.FixedCount

        Me.Chart1.ChartAreas(0).AxisX.Interval = 1

        'Me.Chart1.ChartAreas(0).AxisX.
        'Me.Chart1.ChartAreas(0).AxisX.IsReversed = False

        Me.Chart1.Titles(0).Text = Me.DropDownList1.SelectedItem.Text

        Me.Chart1.DataSource = DS
        Me.Chart1.DataBind()

        Me.GridView1.DataSource = DS
        Me.GridView1.DataBind()

    End Sub

    Private Sub Reports_SPxChart_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If Not Request.QueryString("Tool") = "" Then
                    Select Case Request.QueryString("Tool").ToString
                        Case = "SP1-1"
                            Me.DropDownList1.SelectedIndex = 0
                        Case = "SP1-2"
                            Me.DropDownList1.SelectedIndex = 1
                        Case = "SP1-3"
                            Me.DropDownList1.SelectedIndex = 2
                        Case = "SP2"
                            Me.DropDownList1.SelectedIndex = 3
                        Case = "SP3"
                            Me.DropDownList1.SelectedIndex = 4
                    End Select
                    Me.DropDownList1.Enabled = False
                End If
            End If

            '?Tool=SP1-1
            '?Tool=SP1-2
            '?Tool=SP1-3
            '?Tool=SP2
            '?Tool=SP3
        Catch ex As Exception

        End Try
        TestIt()
    End Sub
    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        TestIt()
    End Sub
End Class
