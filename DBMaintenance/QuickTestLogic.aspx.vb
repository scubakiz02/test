
Partial Class DBMaintenance_QuickTestLogic
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub ButtonCBFullDataRecordCheck_Click(sender As Object, e As EventArgs) Handles ButtonCBFullDataRecordCheck.Click
        Me.LabelCBFullDataRecordCheck.Text = SatiCode.CBFullDataRecordCheck(Me.TextBoxCBFullDataRecordCheck_CB.Text, Me.TextBoxCBFullDataRecordCheck_Para.Text)
    End Sub
    Protected Sub ButtonCB_CheckAndFix_Geo_Click(sender As Object, e As EventArgs) Handles ButtonCB_CheckAndFix_Geo.Click
        Me.LabelCB_CheckAndFix_Geo.Text = SatiCode.CB_CheckAndFix_Geo(Me.TextBoxCB_CheckAndFix_Geo.Text)
    End Sub
    Protected Sub ButtonPartical_Click(sender As Object, e As EventArgs) Handles ButtonPartical.Click
        Me.TextBoxParticalSpec.Text = SatiCode.CheckWaferBoxData(Me.TextBoxBoxType.Text, Me.TextBoxParticalSpec.Text)
    End Sub
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SatiCode.Run_300mm_FGI_Scan()
    End Sub



    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SatiCode.DataTweakTest()

    End Sub
End Class
