
Partial Class Reports_GeoViewer
    Inherits System.Web.UI.Page

    Sub UpdateADE()
        'SELECT location, class, cen_thk, ave_thk, resistivity, Res2, ttv, tir, bow, warp, type, EventTime, wafer, Receiver, id#, run#, wl# FROM dbo.ADE_data WHERE (id# = N'2967') AND (run# = N'2658s') AND (wl# = N'r626') ORDER BY ENTRY DESC


        'Build SQL
        Dim MySQL As String = ""

        'select***************
        MySQL = "SELECT location, class, cen_thk, ave_thk, resistivity, Res2, ttv, tir, bow, warp, type, EventTime, wafer, Receiver, id#, run#, wl# "

        'from*****************
        MySQL = MySQL & "FROM dbo.ADE_data "

        'Where****************
        Dim Hit As Boolean = False
        'MySQL = MySQL & "WHERE "
        'ID
        If Not Me.TextBoxID.Text = "" Then
            If Hit = False Then
                MySQL = MySQL & "WHERE (id# = N'" & Me.TextBoxID.Text & "') "
                Hit = True
            Else
                MySQL = MySQL & "AND (id# = N'" & Me.TextBoxID.Text & "') "
            End If
        End If
        'Run
        If Not Me.TextBoxRun.Text = "" Then
            If Hit = False Then
                MySQL = MySQL & "WHERE (run# = N'" & Me.TextBoxRun.Text & "') "
                Hit = True
            Else
                MySQL = MySQL & "AND (run# = N'" & Me.TextBoxRun.Text & "') "
            End If
        End If
        'WL
        If Not Me.TextBoxWL.Text = "" Then
            If Hit = False Then
                MySQL = MySQL & "WHERE (wl# = N'" & Me.TextBoxWL.Text & "') "
                Hit = True
            Else
                MySQL = MySQL & "AND (wl# = N'" & Me.TextBoxWL.Text & "') "
            End If
        End If

        'Record Types
        'Pass
        If Me.RadioButtonADERecords_Pass.Checked = True Then
            If Hit = False Then
                MySQL = MySQL & "WHERE (class = 'Accept') "
                Hit = True
            Else
                MySQL = MySQL & "AND (class = 'Accept') "
            End If
        End If
        'Not Pass
        If Me.RadioButtonADERecords_Other.Checked = True Then
            If Hit = False Then
                MySQL = MySQL & "WHERE (NOT (class = 'Accept')) "
                Hit = True
            Else
                MySQL = MySQL & "AND (NOT (class = 'Accept')) "
            End If
        End If
        'final
        If Me.RadioButtonADEToolFinal.Checked = True Then
            If Hit = False Then
                MySQL = MySQL & "WHERE (location LIKE N'Final%') "
                Hit = True
            Else
                MySQL = MySQL & "AND (location LIKE N'Final%') "
            End If
        End If
        'Presort
        If Me.RadioButtonADEToolPresort.Checked = True Then
            If Hit = False Then
                MySQL = MySQL & "WHERE (location LIKE N'Pre%') "
                Hit = True
            Else
                MySQL = MySQL & "AND (location LIKE N'Pre%') "
            End If
        End If
        'Order
        MySQL = MySQL & "ORDER BY ENTRY DESC"

        Me.SqlDataSourceADE.SelectCommand = MySQL
        Me.GridViewADE.DataBind()

        ADEGridLook()
    End Sub

    Sub updateHolo()
        'SELECT WaferClassName AS Class, CntThk, AvgThk, CntRes, AvgRes, TTV, TIR, Bow, TotWarp AS Warp, Dotation AS Type, Clock, WaferID, SourceSlot AS S_Slot, DestCarrierID AS Station, DestSlot AS D_Slot, LotID 
        'FROM dbo.DC_OCR 
        'WHERE (LotID = N'3079-8454P-R977') 
        'ORDER BY Clock DESC

        'Build SQL
        Dim MySQL As String = ""

        'select***************
        MySQL = "SELECT WaferClassName AS Class, CntThk, AvgThk, CntRes, AvgRes, TTV, TIR, Bow, TotWarp AS Warp, Dotation AS Type, Clock, WaferID, SourceSlot AS S_Slot, DestCarrierID AS Station, DestSlot AS D_Slot, LotID "

        'from*****************
        MySQL = MySQL & "FROM dbo.DC_OCR "

        'Where****************
        Dim Hit As Boolean = False
        'MySQL = MySQL & "WHERE "
        'ID
        If Not Me.TextBoxHoloLotNumber.Text = "" Then
            If Hit = False Then
                MySQL = MySQL & "WHERE (LotID = N'" & Me.TextBoxHoloLotNumber.Text & "') "
                Hit = True
            Else
                MySQL = MySQL & "AND (LotID = N'" & Me.TextBoxHoloLotNumber.Text & "') "
            End If
        End If

        'Record Types
        'Station 4
        If Me.RadioButtonHoloRecordsStation4.Checked = True Then
            If Hit = False Then
                MySQL = MySQL & "WHERE (DestCarrierID = 'Station4') "
                Hit = True
            Else
                MySQL = MySQL & "AND (DestCarrierID = 'Station4') "
            End If
        End If
        'Station 5
        If Me.RadioButtonHoloRecordsStation5.Checked = True Then
            If Hit = False Then
                MySQL = MySQL & "WHERE (DestCarrierID = 'Station5') "
                Hit = True
            Else
                MySQL = MySQL & "AND (DestCarrierID = 'Station5') "
            End If
        End If
        'Station 6
        If Me.RadioButtonHoloRecordsStation6.Checked = True Then
            If Hit = False Then
                MySQL = MySQL & "WHERE (DestCarrierID = 'Station6') "
                Hit = True
            Else
                MySQL = MySQL & "AND (DestCarrierID = 'Station6') "
            End If
        End If

        'Order
        MySQL = MySQL & "ORDER BY Clock DESC"

        Me.SqlDataSourceHolo.SelectCommand = MySQL
        Me.GridViewHolo.DataBind()
        HoloGridLook()
    End Sub

    Sub ADEGridLook()
        If Me.CheckBoxADEShowCenterThick.Checked = True Then
            Me.GridViewADE.Columns(2).Visible = True
        Else
            Me.GridViewADE.Columns(2).Visible = False
        End If

        If Me.CheckBoxADEShowAvgThick.Checked = True Then
            Me.GridViewADE.Columns(3).Visible = True
        Else
            Me.GridViewADE.Columns(3).Visible = False
        End If

        If Me.CheckBoxADEShowRes.Checked = True Then
            Me.GridViewADE.Columns(4).Visible = True
        Else
            Me.GridViewADE.Columns(4).Visible = False
        End If

        If Me.CheckBoxADEShowRes2.Checked = True Then
            Me.GridViewADE.Columns(5).Visible = True
        Else
            Me.GridViewADE.Columns(5).Visible = False
        End If

        If Me.CheckBoxADEShowTTV.Checked = True Then
            Me.GridViewADE.Columns(6).Visible = True
        Else
            Me.GridViewADE.Columns(6).Visible = False
        End If

        If Me.CheckBoxADEShowTIR.Checked = True Then
            Me.GridViewADE.Columns(7).Visible = True
        Else
            Me.GridViewADE.Columns(7).Visible = False
        End If

        If Me.CheckBoxADEShowBow.Checked = True Then
            Me.GridViewADE.Columns(8).Visible = True
        Else
            Me.GridViewADE.Columns(8).Visible = False
        End If

        If Me.CheckBoxADEShowWarp.Checked = True Then
            Me.GridViewADE.Columns(9).Visible = True
        Else
            Me.GridViewADE.Columns(9).Visible = False
        End If

        If Me.CheckBoxADEShowType.Checked = True Then
            Me.GridViewADE.Columns(10).Visible = True
        Else
            Me.GridViewADE.Columns(10).Visible = False
        End If

        If Me.CheckBoxADEShowDate.Checked = True Then
            Me.GridViewADE.Columns(11).Visible = True
        Else
            Me.GridViewADE.Columns(11).Visible = False
        End If

        If Me.CheckBoxADEShowWafer.Checked = True Then
            Me.GridViewADE.Columns(12).Visible = True
        Else
            Me.GridViewADE.Columns(12).Visible = False
        End If

        If Me.CheckBoxADEShowReceiver.Checked = True Then
            Me.GridViewADE.Columns(13).Visible = True
        Else
            Me.GridViewADE.Columns(13).Visible = False
        End If

        If Me.CheckBoxADEShowID.Checked = True Then
            Me.GridViewADE.Columns(14).Visible = True
        Else
            Me.GridViewADE.Columns(14).Visible = False
        End If

        If Me.CheckBoxADEShowRun.Checked = True Then
            Me.GridViewADE.Columns(15).Visible = True
        Else
            Me.GridViewADE.Columns(15).Visible = False
        End If

        If Me.CheckBoxADEShowWL.Checked = True Then
            Me.GridViewADE.Columns(16).Visible = True
        Else
            Me.GridViewADE.Columns(16).Visible = False
        End If
    End Sub

    Sub HoloGridLook()
        If Me.CheckBoxHoloShowCenterThick.Checked = True Then
            Me.GridViewHolo.Columns(1).Visible = True
        Else
            Me.GridViewHolo.Columns(1).Visible = False
        End If

        If Me.CheckBoxHoloShowAvgThick.Checked = True Then
            Me.GridViewHolo.Columns(2).Visible = True
        Else
            Me.GridViewHolo.Columns(2).Visible = False
        End If

        If Me.CheckBoxHoloShowResCenter.Checked = True Then
            Me.GridViewHolo.Columns(3).Visible = True
        Else
            Me.GridViewHolo.Columns(3).Visible = False
        End If

        If Me.CheckBoxHoloShowResAvg.Checked = True Then
            Me.GridViewHolo.Columns(4).Visible = True
        Else
            Me.GridViewHolo.Columns(4).Visible = False
        End If

        If Me.CheckBoxHoloShowTTV.Checked = True Then
            Me.GridViewHolo.Columns(5).Visible = True
        Else
            Me.GridViewHolo.Columns(5).Visible = False
        End If

        If Me.CheckBoxHoloShowTIR.Checked = True Then
            Me.GridViewHolo.Columns(6).Visible = True
        Else
            Me.GridViewHolo.Columns(6).Visible = False
        End If

        If Me.CheckBoxHoloShowBow.Checked = True Then
            Me.GridViewHolo.Columns(7).Visible = True
        Else
            Me.GridViewHolo.Columns(7).Visible = False
        End If

        If Me.CheckBoxHoloShowWarp.Checked = True Then
            Me.GridViewHolo.Columns(8).Visible = True
        Else
            Me.GridViewHolo.Columns(8).Visible = False
        End If

        If Me.CheckBoxHoloShowType.Checked = True Then
            Me.GridViewHolo.Columns(9).Visible = True
        Else
            Me.GridViewHolo.Columns(9).Visible = False
        End If

        If Me.CheckBoxHoloShowDate.Checked = True Then
            Me.GridViewHolo.Columns(10).Visible = True
        Else
            Me.GridViewHolo.Columns(10).Visible = False
        End If

        If Me.CheckBoxHoloShowWaferT7.Checked = True Then
            Me.GridViewHolo.Columns(11).Visible = True
        Else
            Me.GridViewHolo.Columns(11).Visible = False
        End If

        If Me.CheckBoxHoloShowS_Slot.Checked = True Then
            Me.GridViewHolo.Columns(12).Visible = True
        Else
            Me.GridViewHolo.Columns(12).Visible = False
        End If

        If Me.CheckBoxHoloShowStation.Checked = True Then
            Me.GridViewHolo.Columns(13).Visible = True
        Else
            Me.GridViewHolo.Columns(13).Visible = False
        End If

        If Me.CheckBoxHoloSlotShowDSlot.Checked = True Then
            Me.GridViewHolo.Columns(14).Visible = True
        Else
            Me.GridViewHolo.Columns(14).Visible = False
        End If

        If Me.CheckBoxHoloShowLot.Checked = True Then
            Me.GridViewHolo.Columns(15).Visible = True
        Else
            Me.GridViewHolo.Columns(15).Visible = False
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        UpdateADE()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)

        Me.SqlDataSourceADE.SelectCommand = ""
        Me.SqlDataSourceHolo.SelectCommand = ""
        PanelView()
    End Sub

    Protected Sub DropDownListTools_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListTools.SelectedIndexChanged
        PanelView()
    End Sub

    Sub PanelView()
        Me.PanelADE_DB.Visible = False
        Me.PanelADEBuild.Visible = False

        Me.PanelHologenix.Visible = False
        Me.PanelHoloBuild.Visible = False

        Me.PanelGigaMat.Visible = False
        Me.PanelLeo.Visible = False

        Dim tool As String = Me.DropDownListTools.SelectedValue.ToString

        If tool = "ADE" Then
            Me.PanelADE_DB.Visible = True
            Me.PanelADEBuild.Visible = True
        End If

        If tool = "Hologenix" Then
            Me.PanelHologenix.Visible = True
            Me.PanelHoloBuild.Visible = True
        End If

        If tool = "GigaMat" Then
            Me.PanelGigaMat.Visible = True
        End If

        If tool = "Leo" Then
            Me.PanelLeo.Visible = True
        End If
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        updateHolo()
    End Sub

    Protected Sub GridViewADE_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewADE.DataBound
        Me.LabelADERecordsFound.Text = Me.GridViewADE.Rows.Count
    End Sub

    Protected Sub GridViewHolo_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewHolo.DataBound
        Me.LabelHoloRecordsFound.Text = Me.GridViewHolo.Rows.Count
    End Sub
End Class
