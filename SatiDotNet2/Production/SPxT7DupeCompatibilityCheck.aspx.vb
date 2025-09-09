Imports Class1

Partial Class Production_SPxT7DupeCompatibilityCheck
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Dim DS_Inbound As New Data.DataSet
    Dim DS_Station2 As New Data.DataSet
    Dim DS_Station3 As New Data.DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub

    Sub FindOut()
        If Me.InstanceTextBox.Text = "" Then
            BoxControlls("Clear")
            Exit Sub
        End If

        If Me.ToolListBox.SelectedValue = "" Then
            Exit Sub
        End If

        BoxControlls("Clear")

        Dim DR_Inbound As Data.DataRow
        Dim DR_Station2 As Data.DataRow
        Dim DR_Station3 As Data.DataRow
        Dim DR_SlotInfo As Data.DataRow

        Dim RC_Inbound As Int16
        Dim RC_Station2 As Int16
        Dim RC_Station3 As Int16

        Dim I_1 As Int16
        Dim I_2 As Int16
        Dim I_3 As Int16

        Dim Seq_layout(0 To 2, 0 To 25) As Integer

        Dim Cslot As Int16
        Dim Lslot As Int16

        'Update SPx
        SatiCode.UpdateSPxTool(Me.ToolListBox.SelectedValue.ToString)
        DS_Station2 = GetSPx(Me.ToolListBox.SelectedValue.ToString, "2")
        DS_Station3 = GetSPx(Me.ToolListBox.SelectedValue.ToString, "3")
        '**

        '**********************************
        '**********Inbound*****************
        '**********************************

        'Check the white list for ID 6610
        Dim WL As Boolean = False
        If NeedWhitelist(DS_Station2) Then
            WL = True
        End If

        Try
            DS_Inbound = GetInstance(Me.InstanceTextBox.Text)

            RC_Inbound = DS_Inbound.Tables(0).Rows.Count

            For I_1 = 1 To RC_Inbound
                DR_Inbound = DS_Inbound.Tables(0).Rows(I_1 - 1)

                CType(Me.Panel1.FindControl("InboundSlot" & DR_Inbound("Slot") & "FullTextBox"), TextBox).Text = DR_Inbound("T7").ToString
                CType(Me.Panel1.FindControl("InboundSlot" & DR_Inbound("Slot") & "SeqTextBox"), TextBox).Text = DR_Inbound("Seq")
                Try
                    Seq_layout(0, DR_Inbound("Slot")) = CType(DR_Inbound("Seq"), Integer)
                Catch ex As Exception
                    CType(Me.Panel1.FindControl("InboundSlot" & DR_Inbound("Slot") & "SeqTextBox"), TextBox).Text = "Pull"
                    CType(Me.Panel1.FindControl("InboundSlot" & DR_Inbound("Slot") & "SeqTextBox"), TextBox).BackColor = Drawing.Color.Red
                End Try

                If WL = True Then
                    If SatiCode.CheckScribeFor6610Whitelist(DR_Inbound("T7").ToString) = False Then
                        CType(Me.Panel1.FindControl("InboundSlot" & DR_Inbound("Slot") & "FullTextBox"), TextBox).Text = "Forbidden"
                        CType(Me.Panel1.FindControl("InboundSlot" & DR_Inbound("Slot") & "FullTextBox"), TextBox).BackColor = Drawing.Color.RoyalBlue
                        CType(Me.Panel1.FindControl("InboundSlot" & DR_Inbound("Slot") & "SeqTextBox"), TextBox).Text = "Pull"
                        CType(Me.Panel1.FindControl("InboundSlot" & DR_Inbound("Slot") & "SeqTextBox"), TextBox).BackColor = Drawing.Color.RoyalBlue
                    End If
                End If


                'If Not DR_Inbound("T7").ToString = "ERROR" Then '*******************Fix "Error" from leo tool 
                'CType(Me.Panel1.FindControl("InboundSlot" & DR_Inbound("Slot") & "FullTextBox"), TextBox).Text = DR_Inbound("T7").ToString
                'CType(Me.Panel1.FindControl("InboundSlot" & DR_Inbound("Slot") & "SeqTextBox"), TextBox).Text = DR_Inbound("Seq")
                'Seq_layout(0, DR_Inbound("Slot")) = CType(DR_Inbound("Seq"), Integer)
                'Else
                'CType(Me.Panel1.FindControl("InboundSlot" & DR_Inbound("Slot") & "FullTextBox"), TextBox).Text = DR_Inbound("T7").ToString
                'CType(Me.Panel1.FindControl("InboundSlot" & DR_Inbound("Slot") & "SeqTextBox"), TextBox).Text = "000"
                'Seq_layout(0, DR_Inbound("Slot")) = CType("000", Integer)
                'End If

            Next
        Catch ex As Exception
            Me.InfoTextBox.Text = Me.InfoTextBox.Text + "Error getting Records from Inbound Box"
        End Try

        '**********************************
        '**********Station2****************
        '**********************************
        Try
            Lslot = 26

            RC_Station2 = DS_Station2.Tables(0).Rows.Count
            For I_2 = 1 To RC_Station2
                DR_Station2 = DS_Station2.Tables(0).Rows(I_2 - 1)
                Cslot = DR_Station2("TransferToSlot")
                If Not Cslot < Lslot Then
                    Exit For
                End If
                DR_SlotInfo = GetSlot(DR_Station2("INumber"), DR_Station2("Islot"))
                CType(Me.Panel1.FindControl("Station2Slot" & Cslot & "FullTextBox"), TextBox).Text = DR_SlotInfo("T7").ToString
                CType(Me.Panel1.FindControl("Station2Slot" & Cslot & "SeqTextBox"), TextBox).Text = DR_SlotInfo("Seq")

                Try
                    Seq_layout(1, Cslot) = CType(DR_SlotInfo("Seq"), Integer)
                Catch ex As Exception
                    CType(Me.Panel1.FindControl("Station2Slot" & Cslot & "SeqTextBox"), TextBox).Text = "Pull"
                    CType(Me.Panel1.FindControl("Station2Slot" & Cslot & "SeqTextBox"), TextBox).BackColor = Drawing.Color.Red
                End Try


                Lslot = Cslot
            Next
        Catch ex As Exception
            Me.InfoTextBox.Text = Me.InfoTextBox.Text & " Error getting Records from Station 2 "
        End Try



        '**********************************
        '**********Station3****************
        '**********************************
        Try
            Lslot = 26

            RC_Station3 = DS_Station3.Tables(0).Rows.Count
            For I_3 = 1 To RC_Station3
                DR_Station3 = DS_Station3.Tables(0).Rows(I_3 - 1)
                Cslot = DR_Station3("TransferToSlot")
                If Not Cslot < Lslot Then
                    Exit For
                End If
                DR_SlotInfo = GetSlot(DR_Station3("INumber"), DR_Station3("Islot"))
                CType(Me.Panel1.FindControl("Station3Slot" & Cslot & "FullTextBox"), TextBox).Text = DR_SlotInfo("T7").ToString
                CType(Me.Panel1.FindControl("Station3Slot" & Cslot & "SeqTextBox"), TextBox).Text = DR_SlotInfo("Seq")

                Try
                    Seq_layout(2, Cslot) = CType(DR_SlotInfo("Seq"), Integer)
                Catch ex As Exception
                    CType(Me.Panel1.FindControl("Station3Slot" & Cslot & "SeqTextBox"), TextBox).Text = "Pull"
                    CType(Me.Panel1.FindControl("Station3Slot" & Cslot & "SeqTextBox"), TextBox).BackColor = Drawing.Color.Red
                End Try

                Lslot = Cslot
            Next
        Catch ex As Exception
            Me.InfoTextBox.Text = Me.InfoTextBox.Text & " Error getting Records from Station 3 "
        End Try


        'Check Array for Dupes
        For Pos1 As Int16 = 1 To 25
            If CType(Me.Panel1.FindControl("InboundSlot" & Pos1 & "FullTextBox"), TextBox).Text = "ERROR" Then
                CType(Me.Panel1.FindControl("InboundSlot" & Pos1 & "FullTextBox"), TextBox).BackColor = Drawing.Color.Red
                CType(Me.Panel1.FindControl("InboundSlot" & Pos1 & "SeqTextBox"), TextBox).BackColor = Drawing.Color.Red
            End If

            For Pos2 As Int16 = 1 To 25
                If Not Seq_layout(0, Pos1) = 0 Then
                    If Seq_layout(0, Pos1) = Seq_layout(1, Pos2) Then
                        CType(Me.Panel1.FindControl("InboundSlot" & Pos1 & "SeqTextBox"), TextBox).BackColor = Drawing.Color.Red
                        CType(Me.Panel1.FindControl("Station2Slot" & Pos2 & "SeqTextBox"), TextBox).BackColor = Drawing.Color.Red
                    End If
                    If Seq_layout(0, Pos1) = Seq_layout(2, Pos2) Then
                        CType(Me.Panel1.FindControl("InboundSlot" & Pos1 & "SeqTextBox"), TextBox).BackColor = Drawing.Color.Coral
                        CType(Me.Panel1.FindControl("Station3Slot" & Pos2 & "SeqTextBox"), TextBox).BackColor = Drawing.Color.Coral
                    End If

                    If Not Pos1 = Pos2 Then
                        If Seq_layout(0, Pos1) = Seq_layout(0, Pos2) Then
                            CType(Me.Panel1.FindControl("InboundSlot" & Pos1 & "SeqTextBox"), TextBox).BackColor = Drawing.Color.Yellow
                            CType(Me.Panel1.FindControl("InboundSlot" & Pos2 & "SeqTextBox"), TextBox).BackColor = Drawing.Color.Yellow
                        End If
                    End If
                End If
            Next
        Next




    End Sub

    Protected Sub ToolListBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        FindOut()
    End Sub

    Protected Sub IntanceTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles InstanceTextBox.TextChanged
        FindOut()
    End Sub

    Function GetInstance(ByVal Scaned As String) As Data.DataSet
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT dbo.T7_InstanceInfo.InstanceID, dbo.T7_InstanceInfo.Slot, dbo.T7_WaferActionTracking.T7, SUBSTRING(dbo.T7_WaferActionTracking.T7, 6, 3) AS Seq FROM dbo.T7_InstanceInfo INNER JOIN dbo.T7_WaferActionTracking ON dbo.T7_InstanceInfo.WAT_Key = dbo.T7_WaferActionTracking.WAT_Key WHERE (dbo.T7_InstanceInfo.InstanceID = " & Scaned & ") ORDER BY dbo.T7_InstanceInfo.Slot"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T7_InstanceInfo", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("InstanceID", "InstanceID"), New System.Data.Common.DataColumnMapping("Slot", "Slot"), New System.Data.Common.DataColumnMapping("T7", "T7"), New System.Data.Common.DataColumnMapping("Seq", "Seq")})})
        DA.Fill(DS)
        Connection.Close()
        GetInstance = DS
    End Function

    Function GetSlot(ByVal I_Number As String, ByVal I_Slot As String) As Data.DataRow
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT dbo.T7_WaferActionTracking.T7, SUBSTRING(dbo.T7_WaferActionTracking.T7, 6, 3) AS Seq FROM dbo.T7_InstanceInfo INNER JOIN dbo.T7_WaferActionTracking ON dbo.T7_InstanceInfo.WAT_Key = dbo.T7_WaferActionTracking.WAT_Key WHERE (dbo.T7_InstanceInfo.InstanceID = " & I_Number & ") AND (dbo.T7_InstanceInfo.Slot = " & I_Slot & ") ORDER BY dbo.T7_InstanceInfo.Slot"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T7_WaferActionTracking", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("T7", "T7"), New System.Data.Common.DataColumnMapping("Seq", "Seq")})})
        DA.Fill(DS)
        Connection.Close()
        GetSlot = DS.Tables(0).Rows(0)
    End Function

    Sub BoxControlls(ByVal CMD As String)

        Select Case CMD
            Case "Clear"
                For i As Int16 = 1 To 25
                    CType(Me.Panel1.FindControl("InboundSlot" & i & "FullTextBox"), TextBox).Text = ""
                    CType(Me.Panel1.FindControl("InboundSlot" & i & "SeqTextBox"), TextBox).Text = ""

                    CType(Me.Panel1.FindControl("Station2Slot" & i & "FullTextBox"), TextBox).Text = ""
                    CType(Me.Panel1.FindControl("Station2Slot" & i & "SeqTextBox"), TextBox).Text = ""

                    CType(Me.Panel1.FindControl("Station3Slot" & i & "FullTextBox"), TextBox).Text = ""
                    CType(Me.Panel1.FindControl("Station3Slot" & i & "SeqTextBox"), TextBox).Text = ""

                    CType(Me.Panel1.FindControl("InboundSlot" & i & "FullTextBox"), TextBox).BackColor = Drawing.Color.White
                    CType(Me.Panel1.FindControl("InboundSlot" & i & "SeqTextBox"), TextBox).BackColor = Drawing.Color.White

                    CType(Me.Panel1.FindControl("Station2Slot" & i & "FullTextBox"), TextBox).BackColor = Drawing.Color.White
                    CType(Me.Panel1.FindControl("Station2Slot" & i & "SeqTextBox"), TextBox).BackColor = Drawing.Color.White

                    CType(Me.Panel1.FindControl("Station3Slot" & i & "FullTextBox"), TextBox).BackColor = Drawing.Color.White
                    CType(Me.Panel1.FindControl("Station3Slot" & i & "SeqTextBox"), TextBox).BackColor = Drawing.Color.White

                    Me.InfoTextBox.Text = ""

                Next
            Case "Test"

                For i As Int16 = 1 To 25
                    CType(Me.Panel1.FindControl("InboundSlot" & i & "FullTextBox"), TextBox).Text = i
                    CType(Me.Panel1.FindControl("InboundSlot" & i & "SeqTextBox"), TextBox).Text = i

                    CType(Me.Panel1.FindControl("Station2Slot" & i & "FullTextBox"), TextBox).Text = i
                    CType(Me.Panel1.FindControl("Station2Slot" & i & "SeqTextBox"), TextBox).Text = i

                    CType(Me.Panel1.FindControl("Station3Slot" & i & "FullTextBox"), TextBox).Text = i
                    CType(Me.Panel1.FindControl("Station3Slot" & i & "SeqTextBox"), TextBox).Text = i
                Next
            Case Else
                Me.InstanceTextBox.Text = CMD

        End Select


    End Sub

    Function GetSPx(ByVal SPx As String, ByVal Station As String) As Data.DataSet




        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("AutoDataConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        '*****************************************************************
        '************************Select***********************************
        '*****************************************************************
        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd 'SPRecipeName
            .CommandText = "SELECT TOP 25 Comment2 AS INumber, SourceSlotID AS Islot, DestinationSlotID AS TransferToSlot, SPRecipeName AS RecipeName FROM dbo.SP1_Data WHERE (DestinationStationID = " & Station & ") AND (Machine = N'" & SPx & "') ORDER BY Entry DESC"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        'DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "WaferMover", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("MovementEntry", "MovementEntry"), New System.Data.Common.DataColumnMapping("LotEntry", "LotEntry"), New System.Data.Common.DataColumnMapping("Order", "Order"), New System.Data.Common.DataColumnMapping("InQty", "InQty"), New System.Data.Common.DataColumnMapping("OutQty", "OutQty"), New System.Data.Common.DataColumnMapping("LotStatus", "LotStatus"), New System.Data.Common.DataColumnMapping("Disposition", "Disposition"), New System.Data.Common.DataColumnMapping("Operator", "Operator"), New System.Data.Common.DataColumnMapping("EventTime", "EventTime")})})
        DA.Fill(DS)
        Connection.Close()
        GetSPx = DS

    End Function


    Function NeedWhitelist(SPxDataSet As Data.DataSet) As Boolean
        'find recipe name out in dataset

        'for testing*******
        'Return True
        'End Test *********

        Dim DR As Data.DataRow
        DR = SPxDataSet.Tables(0).Rows(0)
        If DR("RecipeName").ToString.Contains("6610") Then
            Return True
        Else
            Return False
        End If

    End Function




End Class
