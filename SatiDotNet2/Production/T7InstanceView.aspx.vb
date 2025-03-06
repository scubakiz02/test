
Partial Class Production_T7InstanceView
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        UpdatePage()
    End Sub
    Sub UpdatePage()
        Dim DataInput As String
        DataInput = Me.TextBox1.Text
        If DataInput.Contains("WB") Or DataInput.Contains("CB") Then
            DataInput = Mid(DataInput, 3)

        End If
        If Me.InstanceRadioButton.Checked = True Then
            Me.SqlDataSource1.SelectCommand = "SELECT dbo.T7_InstanceInfo.Slot, dbo.T7_WaferActionTracking.T7, dbo.T7_WaferActionTracking.StartDate AS [Started Production], dbo.T7_WaferActionTracking.OrgLot, PreT7_GeoData.RecordDate AS [Pre Geo Date], PreT7_GeoData.CenterThick AS [Pre Geo CenterThick], PreT7_GeoData.Tool AS [Pre Geo Tool], PostT7_GeoData.RecordDate AS [Post Geo Date], PostT7_GeoData.CenterThick AS [Post Geo Center Thick], PostT7_GeoData.Type, PostT7_GeoData.Tool AS [Post Geo Tool], PreT7_GeoData.CenterThick - PostT7_GeoData.CenterThick AS [Microns Removed], dbo.T7_ParticalData.RecordDate AS [Laser Scan Date], dbo.T7_ParticalData.Tool AS [Laser Tool], DATEDIFF(dd, dbo.T7_WaferActionTracking.StartDate, dbo.T7_ParticalData.RecordDate) AS [Days In Prosess], dbo.T7_ParticalData.Map FROM dbo.T7_ParticalData RIGHT OUTER JOIN dbo.T7_WaferActionTracking ON dbo.T7_ParticalData.Partical_Key = dbo.T7_WaferActionTracking.Partical_Key LEFT OUTER JOIN dbo.T7_GeoData AS PreT7_GeoData ON dbo.T7_WaferActionTracking.PreGeo_Key = PreT7_GeoData.Geo_Key RIGHT OUTER JOIN dbo.T7_InstanceInfo ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key LEFT OUTER JOIN dbo.T7_GeoData AS PostT7_GeoData ON dbo.T7_WaferActionTracking.PostGeo_Key = PostT7_GeoData.Geo_Key WHERE (dbo.T7_InstanceInfo.InstanceID = '" & DataInput & "') ORDER BY dbo.T7_InstanceInfo.Slot"
            'Me.SqlDataSource1.SelectCommand = "SELECT dbo.T7_InstanceInfo.Slot, dbo.T7_WaferActionTracking.T7, dbo.T_WH_Invintory.EventTime AS [WH Rec], dbo.T_WH_Invintory.Waferlog AS [WH WL], dbo.T7_WaferActionTracking.StartDate AS [Started Production], dbo.T7_WaferActionTracking.OrgLot, PreT7_GeoData.RecordDate AS [Pre Geo Date], PreT7_GeoData.CenterThick AS [Pre Geo CenterThick], PreT7_GeoData.Tool AS [Pre Geo Tool], PostT7_GeoData.RecordDate AS [Post Geo Date], PostT7_GeoData.CenterThick AS [Post Geo Center Thick], PostT7_GeoData.Type, PostT7_GeoData.Tool AS [Post Geo Tool], PreT7_GeoData.CenterThick - PostT7_GeoData.CenterThick AS [Microns Removed], dbo.T7_ParticalData.RecordDate AS [Laser Scan Date], dbo.T7_ParticalData.Tool AS [Laser Tool], DATEDIFF(dd, dbo.T7_WaferActionTracking.StartDate, dbo.T7_ParticalData.RecordDate) AS [Days In Prosess], dbo.T7_ParticalData.Map FROM dbo.T7_ParticalData RIGHT OUTER JOIN dbo.T7_WaferActionTracking ON dbo.T7_ParticalData.Partical_Key = dbo.T7_WaferActionTracking.Partical_Key LEFT OUTER JOIN dbo.T7_GeoData AS PreT7_GeoData ON dbo.T7_WaferActionTracking.PreGeo_Key = PreT7_GeoData.Geo_Key RIGHT OUTER JOIN dbo.T7_InstanceInfo ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key LEFT OUTER JOIN dbo.T7_GeoData AS PostT7_GeoData ON dbo.T7_WaferActionTracking.PostGeo_Key = PostT7_GeoData.Geo_Key LEFT OUTER JOIN dbo.T_WH_Invintory ON dbo.T7_WaferActionTracking.OrgLot = dbo.T_WH_Invintory.Note WHERE (dbo.T_WH_Invintory.Action = N'Made Lot' OR dbo.T_WH_Invintory.Action IS NULL) AND (dbo.T7_InstanceInfo.InstanceID = '" & DataInput & "') AND (dbo.T7_WaferActionTracking.Active = N'Yes') ORDER BY dbo.T7_InstanceInfo.Slot"
            'Me.SqlDataSource1.SelectCommand = "SELECT dbo.T7_InstanceInfo.Slot, dbo.T7_WaferActionTracking.T7, dbo.T_WH_Invintory.EventTime AS [WH Rec], dbo.T_WH_Invintory.Waferlog AS [WH WL], dbo.T7_WaferActionTracking.StartDate AS [Started Production], dbo.T7_WaferActionTracking.OrgLot, PreT7_GeoData.RecordDate AS [Pre Geo Date], PreT7_GeoData.CenterThick AS [Pre Geo CenterThick], PreT7_GeoData.Tool AS [Pre Geo Tool], PostT7_GeoData.RecordDate AS [Post Geo Date], PostT7_GeoData.CenterThick AS [Post Geo Center Thick], PostT7_GeoData.Tool AS [Post Geo Tool], PreT7_GeoData.CenterThick - PostT7_GeoData.CenterThick AS [Microns Removed], dbo.T7_ParticalData.RecordDate AS [Laser Scan Date], dbo.T7_ParticalData.Tool AS [Laser Tool], DATEDIFF(dd, dbo.T7_WaferActionTracking.StartDate, dbo.T7_ParticalData.RecordDate) AS [Days In Prosess], PostT7_GeoData.Type, dbo.T7_ParticalData.Map FROM dbo.T7_ParticalData RIGHT OUTER JOIN dbo.T7_WaferActionTracking ON dbo.T7_ParticalData.Partical_Key = dbo.T7_WaferActionTracking.Partical_Key LEFT OUTER JOIN dbo.T7_GeoData PreT7_GeoData ON dbo.T7_WaferActionTracking.PreGeo_Key = PreT7_GeoData.Geo_Key RIGHT OUTER JOIN dbo.T7_InstanceInfo ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key LEFT OUTER JOIN dbo.T7_GeoData PostT7_GeoData ON dbo.T7_WaferActionTracking.PostGeo_Key = PostT7_GeoData.Geo_Key LEFT OUTER JOIN dbo.T_WH_Invintory ON dbo.T7_WaferActionTracking.OrgLot = dbo.T_WH_Invintory.Note WHERE (dbo.T_WH_Invintory.Action = N'Made Lot' OR dbo.T_WH_Invintory.Action IS NULL) AND (dbo.T7_InstanceInfo.InstanceID = '" & DataInput & "') ORDER BY dbo.T7_InstanceInfo.Slot DESC"
        End If

        If Me.WaferBoxRadioButton.Checked = True Then
            Me.SqlDataSource1.SelectCommand = "SELECT dbo.T7_InstanceInfo.Slot, dbo.T7_WaferActionTracking.T7, dbo.T7_WaferActionTracking.StartDate AS [Started Production], PreT7_GeoData.RecordDate AS [Pre Geo Date], PreT7_GeoData.CenterThick AS [Pre Geo CenterThick], PreT7_GeoData.Tool AS [Pre Geo Tool], PostT7_GeoData.RecordDate AS [Post Geo Date], PostT7_GeoData.CenterThick AS [Post Geo Center Thick], PostT7_GeoData.Type, PostT7_GeoData.Tool AS [Post Geo Tool], PreT7_GeoData.CenterThick - PostT7_GeoData.CenterThick AS [Microns Removed], dbo.T7_ParticalData.RecordDate AS [Laser Scan Date], dbo.T7_ParticalData.Tool AS [Laser Tool], DATEDIFF(dd, dbo.T7_WaferActionTracking.StartDate, dbo.T7_ParticalData.RecordDate) AS [Days In Prosess], dbo.T7_ParticalData.Map FROM dbo.T7_GeoData AS PreT7_GeoData RIGHT OUTER JOIN dbo.T7_WaferActionTracking ON PreT7_GeoData.Geo_Key = dbo.T7_WaferActionTracking.PreGeo_Key LEFT OUTER JOIN dbo.T7_ParticalData ON dbo.T7_WaferActionTracking.Partical_Key = dbo.T7_ParticalData.Partical_Key LEFT OUTER JOIN dbo.T7_GeoData AS PostT7_GeoData ON dbo.T7_WaferActionTracking.PostGeo_Key = PostT7_GeoData.Geo_Key RIGHT OUTER JOIN dbo.T7_InstanceInfo INNER JOIN dbo.T_FGI_Boxes ON dbo.T7_InstanceInfo.InstanceID = dbo.T_FGI_Boxes.InstanceKey ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key WHERE (dbo.T_FGI_Boxes.BoxInvNumber = " & DataInput & ") AND (dbo.T7_WaferActionTracking.Active = N'Yes') ORDER BY dbo.T7_InstanceInfo.Slot"
            'Me.SqlDataSource1.SelectCommand = "SELECT dbo.T7_InstanceInfo.Slot, dbo.T7_WaferActionTracking.T7, dbo.T_WH_Invintory.EventTime AS [WH Rec], dbo.T_WH_Invintory.Waferlog AS [WH WL], dbo.T7_WaferActionTracking.StartDate AS [Started Production], PreT7_GeoData.RecordDate AS [Pre Geo Date], PreT7_GeoData.CenterThick AS [Pre Geo CenterThick], PreT7_GeoData.Tool AS [Pre Geo Tool], PostT7_GeoData.RecordDate AS [Post Geo Date], PostT7_GeoData.CenterThick AS [Post Geo Center Thick], PostT7_GeoData.Type, PostT7_GeoData.Tool AS [Post Geo Tool], PreT7_GeoData.CenterThick - PostT7_GeoData.CenterThick AS [Microns Removed], dbo.T7_ParticalData.RecordDate AS [Laser Scan Date], dbo.T7_ParticalData.Tool AS [Laser Tool], DATEDIFF(dd, dbo.T7_WaferActionTracking.StartDate, dbo.T7_ParticalData.RecordDate) AS [Days In Prosess], dbo.T7_ParticalData.Map FROM dbo.T7_GeoData AS PreT7_GeoData RIGHT OUTER JOIN dbo.T7_WaferActionTracking ON PreT7_GeoData.Geo_Key = dbo.T7_WaferActionTracking.PreGeo_Key LEFT OUTER JOIN dbo.T7_ParticalData ON dbo.T7_WaferActionTracking.Partical_Key = dbo.T7_ParticalData.Partical_Key LEFT OUTER JOIN dbo.T7_GeoData AS PostT7_GeoData ON dbo.T7_WaferActionTracking.PostGeo_Key = PostT7_GeoData.Geo_Key RIGHT OUTER JOIN dbo.T7_InstanceInfo INNER JOIN dbo.T_FGI_Boxes ON dbo.T7_InstanceInfo.InstanceID = dbo.T_FGI_Boxes.InstanceKey ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key LEFT OUTER JOIN dbo.T_WH_Invintory ON dbo.T7_WaferActionTracking.OrgLot = dbo.T_WH_Invintory.Note WHERE (dbo.T_FGI_Boxes.BoxInvNumber = " & DataInput & ") AND (dbo.T_WH_Invintory.Action = N'Made Lot') AND (dbo.T7_WaferActionTracking.Active = N'Yes') ORDER BY dbo.T7_InstanceInfo.Slot"
            'Me.SqlDataSource1.SelectCommand = "SELECT  dbo.T7_InstanceInfo.Slot, dbo.T7_WaferActionTracking.T7, dbo.T_WH_Invintory.EventTime AS [WH Rec], dbo.T_WH_Invintory.Waferlog AS [WH WL], dbo.T7_WaferActionTracking.StartDate AS [Started Production], dbo.T7_WaferActionTracking.OrgLot, PreT7_GeoData.RecordDate AS [Pre Geo Date], PreT7_GeoData.CenterThick AS [Pre Geo CenterThick], PreT7_GeoData.Tool AS [Pre Geo Tool], PostT7_GeoData.RecordDate AS [Post Geo Date], PostT7_GeoData.CenterThick AS [Post Geo Center Thick], PostT7_GeoData.Tool AS [Post Geo Tool], PreT7_GeoData.CenterThick - PostT7_GeoData.CenterThick AS [Microns Removed], dbo.T7_ParticalData.RecordDate AS [Laser Scan Date], dbo.T7_ParticalData.Tool AS [Laser Tool], DATEDIFF(dd, dbo.T7_WaferActionTracking.StartDate, dbo.T7_ParticalData.RecordDate) AS [Days In Prosess], PostT7_GeoData.Type, dbo.T7_ParticalData.Map FROM dbo.T7_GeoData AS PreT7_GeoData RIGHT OUTER JOIN dbo.T7_WaferActionTracking ON PreT7_GeoData.Geo_Key = dbo.T7_WaferActionTracking.PreGeo_Key LEFT OUTER JOIN dbo.T7_ParticalData ON dbo.T7_WaferActionTracking.Partical_Key = dbo.T7_ParticalData.Partical_Key LEFT OUTER JOIN dbo.T7_GeoData AS PostT7_GeoData ON dbo.T7_WaferActionTracking.PostGeo_Key = PostT7_GeoData.Geo_Key RIGHT OUTER JOIN dbo.T7_InstanceInfo INNER JOIN dbo.T_FGI_Boxes ON dbo.T7_InstanceInfo.InstanceID = dbo.T_FGI_Boxes.InstanceKey ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key LEFT OUTER JOIN dbo.T_WH_Invintory ON dbo.T7_WaferActionTracking.OrgLot = dbo.T_WH_Invintory.Note WHERE (dbo.T_FGI_Boxes.BoxInvNumber = '" & DataInput & "') AND  (dbo.T_WH_Invintory.Action = N'Made Lot' OR dbo.T_WH_Invintory.Action IS NULL) ORDER BY dbo.T7_InstanceInfo.Slot DESC"
        End If

        If Me.FulDetailCheckBox.Checked = True Then
            For i As Int16 = 2 To Me.GridView1.Columns.Count - 1
                Me.GridView1.Columns(i).Visible = True
            Next
        Else
            For i As Int16 = 2 To Me.GridView1.Columns.Count - 1
                Me.GridView1.Columns(i).Visible = False
            Next
        End If
        GridView1.DataBind()
    End Sub

    Protected Sub FulDetailCheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdatePage()
    End Sub

    Protected Sub WaferBoxRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdatePage()
    End Sub

    Protected Sub InstanceRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdatePage()
    End Sub

    Protected Sub TextBox1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdatePage()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

        If e.CommandName = "Map" Then

            Me.MapRowLabel.Text = e.CommandArgument.ToString
            LookMap(Me.MapRowLabel.Text, "C")

            Me.Button2_ModalPopupExtender.Show()
        End If

        If e.CommandName = "NewMap" Then


        End If
    End Sub

    Protected Sub MapCloseButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MapCloseButton.Click

    End Sub

    Protected Sub ButtonClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClose.Click

    End Sub
    Sub LookMap(ByVal Row As Int16, ByVal Action As String)


        Dim MapFile As String = ""
        Select Case Action
            Case "B"
                If Row > 0 Then
                    MapFile = CType(Me.GridView1.Rows(Row - 1).Cells(15).FindControl("MapfileLabel"), Label).Text
                    Me.MapRowLabel.Text = Row - 1

                End If
            Case "C"
                MapFile = CType(Me.GridView1.Rows(Row).Cells(15).FindControl("MapfileLabel"), Label).Text
                Me.MapRowLabel.Text = Row

            Case "N"
                Dim RowCount As Int16 = Me.GridView1.Rows.Count
                If Row < RowCount - 1 Then
                    MapFile = CType(Me.GridView1.Rows(Row + 1).Cells(15).FindControl("MapfileLabel"), Label).Text
                    Me.MapRowLabel.Text = Row + 1

                End If
        End Select

        '*********************************************************
        '*********************************************************
        If MapFile.Contains("Z:\") Then
            MapFile = Session("SP2Files") & Mid(MapFile, 3)
        Else
            MapFile = Session("SatiMapsDir") & MapFile
        End If
        '*********************************************************
        '*********************************************************

        Me.CSlotLabel.Text = "Looking @ Slot: " & Me.MapRowLabel.Text + 1
        
        Me.MapImage.ImageUrl = MapFile
        Me.MapImage.DataBind()

    End Sub

    Protected Sub BackMapButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BackMapButton.Click
        LookMap(Me.MapRowLabel.Text, "B")
        Me.Button2_ModalPopupExtender.Show()
    End Sub

    Protected Sub NextMapButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles NextMapButton.Click
        LookMap(Me.MapRowLabel.Text, "N")
        Me.Button2_ModalPopupExtender.Show()
    End Sub

   
End Class
