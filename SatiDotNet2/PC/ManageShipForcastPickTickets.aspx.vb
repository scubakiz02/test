
Partial Class PC_ManageShipForcastPickTickets
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.PanelLink.Visible = True
        'Me.LabelDate.Text = DateAndTime.Now.ToShortDateString
        Me.Calendar1.SelectedDate = DateAndTime.Now.ToShortDateString
        SetDate()
        Me.LabelID.Text = Me.TextBoxID.Text

    End Sub

    Protected Sub Calendar1_SelectionChanged(sender As Object, e As EventArgs) Handles Calendar1.SelectionChanged
        SetDate()
    End Sub

    Sub SetDate()
        Dim WW As Int16
        WW = Saticode.GetWorkWeek(Calendar1.SelectedDate.Year, Calendar1.SelectedDate.DayOfYear)

        If WW = 53 Then
            WW = 1
        End If

        Me.LabelWW.Text = WW
        Me.LabelDOW.Text = DateAndTime.WeekdayName(Calendar1.SelectedDate.DayOfWeek + 1)
        Me.LabelDate.Text = Calendar1.SelectedDate.ToShortDateString
    End Sub

    Sub Find_SO_For_ID()
        Dim TheID As String

        'Look for the SO as a Main ID
        'SELECT dbo.SO_LineItems.MainID, dbo.SO_Info.SO, dbo.SO_Info.PO_NumberFROM dbo.SO_Info INNER JOIN dbo.SO_LineItems ON dbo.SO_Info.SO = dbo.SO_LineItems.SO WHERE (dbo.SO_LineItems.MainID = N'2386') AND (dbo.SO_Info.ExpirationDtd IS NULL)

        'Look for the ID as a Child ID
        'SELECT dbo.MainID_SO_LineItems.Child_MainID, dbo.SO_LineItems.MainID, dbo.SO_Info.SO, dbo.SO_Info.PO_Number FROM dbo.SO_LineItems INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO INNER JOIN dbo.MainID_SO_LineItems ON dbo.SO_LineItems.MainID = dbo.MainID_SO_LineItems.SO_MainID WHERE (dbo.SO_Info.ExpirationDtd IS NULL) AND (dbo.MainID_SO_LineItems.Child_MainID = N'147A')


    End Sub

    Protected Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        Me.PanelLink.Visible = False
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)

        Me.Label2.Text = DateAndTime.Now.ToString("yyyyMMdd_HHmmss", System.Globalization.CultureInfo.InvariantCulture)
    End Sub
End Class
