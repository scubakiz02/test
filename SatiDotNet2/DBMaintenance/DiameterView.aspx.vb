
Partial Class DBMaintenance_DiameterView
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LoadGridData()
    End Sub

    Sub LoadGridData()
        'SELECT TOP (100) PERCENT T7_InstanceInfo.Slot, ISNULL(Q_Diameter_T7_Active.Diameter, 300) AS Diameter, T7_WaferActionTracking.T7, T_FGI_Boxes.InstanceKey, T_FGI_Boxes.BoxInvNumber FROM T7_WaferActionTracking INNER JOIN T7_InstanceInfo ON T7_WaferActionTracking.WAT_Key = T7_InstanceInfo.WAT_Key LEFT OUTER JOIN Q_Diameter_T7_Active ON T7_WaferActionTracking.WAT_Key = Q_Diameter_T7_Active.WAT_Key LEFT OUTER JOIN T7_ParticalData ON T7_WaferActionTracking.Partical_Key = T7_ParticalData.Partical_Key RIGHT OUTER JOIN T_FGI_Boxes LEFT OUTER JOIN LabelsMade INNER JOIN MainID ON LEFT (LabelsMade.Lot, 4) = MainID.MainID INNER JOIN Q_Process_Info_GreenCatieFinalSpecs ON MainID.MainID = Q_Process_Info_GreenCatieFinalSpecs.ID_NUMBER INNER JOIN CofA_Info ON Q_Process_Info_GreenCatieFinalSpecs.ID_NUMBER = CofA_Info.ID_NUMBER ON T_FGI_Boxes.LabelsMadeKey = LabelsMade.LabelRecordNumber ON T7_InstanceInfo.InstanceID = T_FGI_Boxes.InstanceKey LEFT OUTER JOIN T7_GeoData AS T7_GeoData_1 ON T7_WaferActionTracking.PostGeo_Key = T7_GeoData_1.Geo_Key LEFT OUTER JOIN T7_GeoData ON T7_WaferActionTracking.PreGeo_Key = T7_GeoData.Geo_Key WHERE (T_FGI_Boxes.InstanceKey = 405927) ORDER BY Diameter
        If RadioButtonWB.Checked = True And Not TextBox1.Text = "" Then
            Me.SqlDataSource1.SelectCommand = "SELECT TOP (100) PERCENT ISNULL(Q_Diameter_T7_Active.Diameter, 300) AS Diameter, T7_WaferActionTracking.T7, T_FGI_Boxes.InstanceKey, T_FGI_Boxes.BoxInvNumber, MainID.CustomerID, MainID.MainID, T7_InstanceInfo.Slot FROM T7_WaferActionTracking INNER JOIN T7_InstanceInfo ON T7_WaferActionTracking.WAT_Key = T7_InstanceInfo.WAT_Key LEFT OUTER JOIN Q_Diameter_T7_Active ON T7_WaferActionTracking.WAT_Key = Q_Diameter_T7_Active.WAT_Key RIGHT OUTER JOIN T_FGI_Boxes LEFT OUTER JOIN LabelsMade INNER JOIN MainID ON LEFT (LabelsMade.Lot, 4) = MainID.MainID ON T_FGI_Boxes.LabelsMadeKey = LabelsMade.LabelRecordNumber ON T7_InstanceInfo.InstanceID = T_FGI_Boxes.InstanceKey WHERE (T_FGI_Boxes.BoxInvNumber  = " & Me.TextBox1.Text & ") ORDER BY Diameter"
            GridView1.DataBind()
        End If
        If RadioButtonI.Checked = True And Not TextBox1.Text = "" Then
            Me.SqlDataSource1.SelectCommand = "SELECT TOP (100) PERCENT ISNULL(Q_Diameter_T7_Active.Diameter, 300) AS Diameter, T7_WaferActionTracking.T7, T_FGI_Boxes.InstanceKey, T_FGI_Boxes.BoxInvNumber, MainID.CustomerID, MainID.MainID, T7_InstanceInfo.Slot FROM T7_WaferActionTracking INNER JOIN T7_InstanceInfo ON T7_WaferActionTracking.WAT_Key = T7_InstanceInfo.WAT_Key LEFT OUTER JOIN Q_Diameter_T7_Active ON T7_WaferActionTracking.WAT_Key = Q_Diameter_T7_Active.WAT_Key RIGHT OUTER JOIN T_FGI_Boxes LEFT OUTER JOIN LabelsMade INNER JOIN MainID ON LEFT (LabelsMade.Lot, 4) = MainID.MainID ON T_FGI_Boxes.LabelsMadeKey = LabelsMade.LabelRecordNumber ON T7_InstanceInfo.InstanceID = T_FGI_Boxes.InstanceKey WHERE (T_FGI_Boxes.InstanceKey = " & Me.TextBox1.Text & ") ORDER BY Diameter"
            GridView1.DataBind()
        End If
        Dim Limit As Double
        Dim Diameter As Double
        Dim C_G As Drawing.Color
        Dim C_O As Drawing.Color
        C_G = Drawing.Color.Green
        C_O = Drawing.Color.Red

        Try
            Limit = MyLimit(Me.GridView1.Rows(0).Cells(5).Text)

            'look at the diameter. if its out Highlight and show button else hide button
            For i As Int16 = 0 To GridView1.Rows.Count - 1
                Diameter = GridView1.Rows(i).Cells(0).Text
                GridView1.Rows(i).Cells(0).BackColor = C_G
                GridView1.Rows(i).Cells(7).Visible = False

                If Diameter < (300 - Limit) Then '299.8 
                    GridView1.Rows(i).Cells(0).BackColor = C_O
                    GridView1.Rows(i).Cells(7).Visible = True

                End If
                If Diameter > (300 + Limit) Then '300.2
                    GridView1.Rows(i).Cells(0).BackColor = C_O
                    GridView1.Rows(i).Cells(7).Visible = True
                End If


            Next


        Catch ex As Exception

        End Try

    End Sub


    Function MyLimit(TheID As String) As Double
        Select Case TheID
            Case "3628", "0" 'China has a +/-0.5
                MyLimit = 0.5
            Case Else
                MyLimit = 0.2 'standard Intel
        End Select
    End Function

    Private Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim row As String
        Dim TheT7 As String
        row = e.CommandArgument.ToString

        If e.CommandName = "Fix" Then
            TheT7 = GridView1.Rows(row).Cells(1).Text
            Record_Diameter(TheT7, 300)
            LoadGridData()
        End If
    End Sub

    Sub Record_Diameter(TheT7 As String, Diameter As Double)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = "Data Source=PWI-31\SATIDB;Initial Catalog=ALTS;Persist Security Info=True;User ID=exsil_user;Password=exsiluser"
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DS2 As New Data.DataSet
        Dim DR As Data.DataRow
        Dim DR2 As Data.DataRow
        Dim SelectCmd As New System.Data.SqlClient.SqlCommand
        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        Dim RowCount As Int16
        Dim Message As String = ""

        With SelectCmd
            .CommandText = "SELECT Diameter_Key, T7, Diameter, Tool, Op, Active FROM T7_DiameterData WHERE (Active = 1) AND (T7 = N'" & TheT7 & "')" '"SELECT Diameter_Key, T7, Diameter, Tool, Op, Active, TimeStamp FROM T7_DiameterData WHERE (Active = 1) AND (T7 = N'" & TheT7 & "')"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        With InsertCmd
            .CommandText = "INSERT INTO [T7_DiameterData] ([T7], [Diameter], [Tool], [Op], [Active]) VALUES (@T7, @Diameter, @Tool, @Op, @Active); SELECT Diameter_Key, T7, Diameter, Tool, Op, Active FROM T7_DiameterData WHERE (Diameter_Key = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@T7", System.Data.SqlDbType.NVarChar, 0, "T7"), New System.Data.SqlClient.SqlParameter("@Diameter", System.Data.SqlDbType.Real, 0, "Diameter"), New System.Data.SqlClient.SqlParameter("@Tool", System.Data.SqlDbType.NVarChar, 0, "Tool"), New System.Data.SqlClient.SqlParameter("@Op", System.Data.SqlDbType.NVarChar, 0, "Op"), New System.Data.SqlClient.SqlParameter("@Active", System.Data.SqlDbType.Bit, 0, "Active")})
        End With
        DA.InsertCommand = InsertCmd

        With UpdateCmd
            .CommandText = "UPDATE [T7_DiameterData] SET [T7] = @T7, [Diameter] = @Diameter, [Tool] = @Tool, [Op] = @Op, [Active] = @Active WHERE (([Diameter_Key] = @Original_Diameter_Key) AND ([T7] = @Original_T7) AND ([Diameter] = @Original_Diameter) AND ((@IsNull_Tool = 1 AND [Tool] IS NULL) OR ([Tool] = @Original_Tool)) AND ((@IsNull_Op = 1 AND [Op] IS NULL) OR ([Op] = @Original_Op)) AND ([Active] = @Original_Active)); SELECT Diameter_Key, T7, Diameter, Tool, Op, Active FROM T7_DiameterData WHERE (Diameter_Key = @Diameter_Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@T7", System.Data.SqlDbType.NVarChar, 0, "T7"), New System.Data.SqlClient.SqlParameter("@Diameter", System.Data.SqlDbType.Real, 0, "Diameter"), New System.Data.SqlClient.SqlParameter("@Tool", System.Data.SqlDbType.NVarChar, 0, "Tool"), New System.Data.SqlClient.SqlParameter("@Op", System.Data.SqlDbType.NVarChar, 0, "Op"), New System.Data.SqlClient.SqlParameter("@Active", System.Data.SqlDbType.Bit, 0, "Active"), New System.Data.SqlClient.SqlParameter("@Original_Diameter_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Diameter_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_T7", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "T7", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Diameter", System.Data.SqlDbType.Real, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Diameter", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Tool", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Tool", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Tool", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Tool", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Op", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Op", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Op", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Op", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Active", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Active", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Diameter_Key", System.Data.SqlDbType.Int, 4, "Diameter_Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T7_DiameterData", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Diameter_Key", "Diameter_Key"), New System.Data.Common.DataColumnMapping("T7", "T7"), New System.Data.Common.DataColumnMapping("Diameter", "Diameter"), New System.Data.Common.DataColumnMapping("Tool", "Tool"), New System.Data.Common.DataColumnMapping("Op", "Op"), New System.Data.Common.DataColumnMapping("Active", "Active")})})
        DA.Fill(DS)

        RowCount = DS.Tables(0).Rows.Count

        If RowCount > 0 Then

            For i As Int16 = 0 To RowCount - 1
                DR = DS.Tables(0).Rows(i)
                Try
                    DS2 = SatiCode.GetMyDataSet("SELECT Diameter_Key, T7, Diameter, Tool, TimeStamp FROM T7_DiameterData WHERE (Diameter_Key = " & DR("Diameter_Key") & ")")
                    DR2 = DS2.Tables(0).Rows(0)
                    Message = "Diameter Key: " & DR2("Diameter_Key").ToString & " was changed From " & DR2("Diameter").ToString & " to 300. The wafer was Org scan time was " & DR2("TimeStamp").ToString
                    SatiCode.SendMail365(Message, "Diameter Change", "tim.hughes@purewafer.com", "SATI@purewafer.com")
                Catch ex As Exception

                End Try


                DR.AcceptChanges()
                DR.BeginEdit()
                DR("Diameter") = Diameter
                DR.EndEdit()
                DA.Update(DS, "T7_DiameterData")
            Next
        End If

        Connection.Close()





    End Sub

End Class
