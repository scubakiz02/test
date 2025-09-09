

Partial Class SPC_SPC_Compile
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim DepartmentSelect As Boolean = False
    Dim DS_New_info As New Data.DataSet

    'Dim DT As Data.DataTable
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)

    End Sub


    Private Sub SPC_SPC_Compile_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        If IsPostBack Then
            'DT.Columns.Add("")

            DS_New_info.Tables.Add("MyData")
            DS_New_info.Tables("MyData").Columns.Add("Seq")
            DS_New_info.Tables("MyData").Columns.Add("Name")
            DS_New_info.Tables("MyData").Columns.Add("Para")
            DS_New_info.Tables("MyData").Columns.Add("LCL")
            DS_New_info.Tables("MyData").Columns.Add("Value")
            DS_New_info.Tables("MyData").Columns.Add("UCL")
            DS_New_info.Tables("MyData").Columns.Add("PK")
            DS_New_info.Tables("MyData").Columns.Add("LK")
            DS_New_info.Tables("MyData").Columns.Add("TK")
            DS_New_info.Tables("MyData").Columns.Add("OCAP_Low")
            DS_New_info.Tables("MyData").Columns.Add("OCAP_High")
            DS_New_info.Tables("MyData").Columns.Add("Recipe")
            DS_New_info.Tables("MyData").Columns.Add("RC")
        End If

    End Sub

    Sub LoadTools(Department As String)
        'SELECT Department FROM T_SPC_Tool_Info GROUP BY Department ORDER BY Department
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        DS = SatiCode.GetMyDataSetSPCData("SELECT TOP (100) PERCENT Tool_Name, SQL_Function FROM dbo.T_SPC_Tool_Info WHERE (Enable = 1) AND (Department = N'" & Department & "') ORDER BY Tool_Name")

        Me.DropDownListTools.Items.Clear()
        Me.DropDownListTools.Items.Add("Select Tool...")
        For i As Int16 = 0 To DS.Tables(0).Rows.Count - 1
            DR = DS.Tables(0).Rows(i)
            Me.DropDownListTools.Items.Add(DR("Tool_Name"))

        Next
    End Sub


    Protected Sub DropDownListDepartments_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListDepartments.SelectedIndexChanged
        Me.PanelData.Visible = False
        Me.PanelDCS.Visible = False
        Me.PanelData.Visible = False
        If Not DropDownListDepartments.SelectedItem.Text = "Select..." Then
            Me.PanelTools.Visible = True

            LoadTools(DropDownListDepartments.SelectedItem.Text)
        Else
            Me.PanelTools.Visible = False
        End If
    End Sub

    Protected Sub DropDownListTools_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListTools.SelectedIndexChanged
        Dim SQL_Fuction As String
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Me.DropDownList_Last_DSC.Items.Clear()
        Me.PanelData.Visible = False

        If Not Me.DropDownListTools.SelectedItem.Text = "Select Tool..." Then
            Me.PanelDCS.Visible = True

            If Me.DropDownListTools.SelectedItem.Text = "SP1-1" Then
                UpdateToolData("SP1")
            End If
            If Me.DropDownListTools.SelectedItem.Text = "SP1-2" Then
                UpdateToolData("SP2")
            End If
            If Me.DropDownListTools.SelectedItem.Text = "SP1-3" Then
                UpdateToolData("SP1-3")
            End If

            DS = SatiCode.GetMyDataSetSPCData("SELECT TOP (100) PERCENT Tool_Name, SQL_Function FROM dbo.T_SPC_Tool_Info WHERE (Department = N'" & Me.DropDownListDepartments.SelectedItem.Text & "') AND (Tool_Name = N'" & Me.DropDownListTools.SelectedItem.Text & "') ORDER BY Tool_Name")

            DR = DS.Tables(0).Rows(0)
            If Not IsDBNull(DR("SQL_Function")) Then
                'call the SQL Function. Then fill the avaliable DSC in dropdown.
                SQL_Fuction = DR("SQL_Function")
                Me.Label_SQLfunction.Text = SQL_Fuction
                'GetMyDataSetAutoData
                DS = SatiCode.GetMyDataSetAutoData("SELECT [" & SQL_Fuction & "_1].* FROM dbo.[" & SQL_Fuction & "]() AS [" & SQL_Fuction & "_1]")
                DR = DS.Tables(0).Rows(0)
                Me.DropDownList_Last_DSC.Items.Clear()
                Me.DropDownList_Last_DSC.Items.Add("Select DSC...")
                If Not DS.Tables(0).Rows.Count = 0 Then
                    For I As Int16 = 0 To DS.Tables(0).Rows.Count - 1
                        DR = DS.Tables(0).Rows(I)
                        Me.DropDownList_Last_DSC.Items.Add(DR("DSC").ToString)
                    Next
                End If

            End If
        Else
            Me.PanelDCS.Visible = False
        End If


    End Sub

    Sub BuildData()
        Dim DSC As String
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim SQL_Function As String = Me.Label_SQLfunction.Text
        Me.PanelData.Visible = False
        Me.Label_OCAP_Message.Text = ""

        If Not Me.DropDownList_Last_DSC.SelectedItem.Text = "Select DSC..." Then
            Me.PanelData.Visible = True


            DSC = Me.DropDownList_Last_DSC.SelectedItem.Text
            DS = SatiCode.GetMyDataSetAutoData("SELECT [" & SQL_Function & "_1].* FROM dbo.[" & SQL_Function & "]() AS [" & SQL_Function & "_1] WHERE (DSC = N'" & DSC & "')")
            DR = DS.Tables(0).Rows(0)

            Dim DS_Para As New Data.DataSet
            Dim DR_Para As Data.DataRow
            Dim Tool As String = Me.DropDownListTools.SelectedItem.Text
            'SELECT TOP (100) PERCENT dbo.T_SPC_Tool_Info.Tool_Name, dbo.T_SPC_Parameters.Seq_Flow, dbo.T_SPC_Parameters.Name, dbo.T_SPC_Parameters.DB_Column, dbo.T_SPC_Limits.Avg_LCL, dbo.T_SPC_Limits.Avg_UCL, dbo.T_SPC_Limits.Stdev_LCL, dbo.T_SPC_Limits.Stdev_UCL, dbo.T_SPC_Limits.Parameter_Key, dbo.T_SPC_Limits.[Key] AS Limit_Key FROM dbo.T_SPC_Parameters INNER JOIN dbo.T_SPC_Tool_Info ON dbo.T_SPC_Parameters.Tool_Key = dbo.T_SPC_Tool_Info.[Key] INNER JOIN dbo.T_SPC_Limits ON dbo.T_SPC_Parameters.[Key] = dbo.T_SPC_Limits.Parameter_Key WHERE (dbo.T_SPC_Tool_Info.Tool_Name = N'SP1-1') AND (dbo.T_SPC_Parameters.Enable = 1) AND (dbo.T_SPC_Limits.Enable = 1) ORDER BY dbo.T_SPC_Parameters.Seq_Flow
            DS_Para = SatiCode.GetMyDataSetSPCData("SELECT TOP (100) PERCENT dbo.T_SPC_Tool_Info.Tool_Name, dbo.T_SPC_Parameters.Seq_Flow, dbo.T_SPC_Parameters.Name, dbo.T_SPC_Parameters.DB_Column, dbo.T_SPC_Limits.Avg_LCL, dbo.T_SPC_Limits.Avg_UCL, dbo.T_SPC_Limits.Stdev_LCL, dbo.T_SPC_Limits.Stdev_UCL, dbo.T_SPC_Limits.Parameter_Key, dbo.T_SPC_Limits.[Key] AS Limit_Key, dbo.T_SPC_Tool_Info.[Key] AS Tool_Key, dbo.T_SPC_Parameters.OCAP_Low, dbo.T_SPC_Parameters.OCAP_High FROM dbo.T_SPC_Parameters INNER JOIN dbo.T_SPC_Tool_Info ON dbo.T_SPC_Parameters.Tool_Key = dbo.T_SPC_Tool_Info.[Key] INNER JOIN dbo.T_SPC_Limits ON dbo.T_SPC_Parameters.[Key] = dbo.T_SPC_Limits.Parameter_Key WHERE (dbo.T_SPC_Tool_Info.Tool_Name = N'" & Tool & "') AND (dbo.T_SPC_Parameters.Enable = 1) AND (dbo.T_SPC_Limits.Enable = 1) ORDER BY dbo.T_SPC_Parameters.Seq_Flow")


            Dim DS_New As New Data.DataSet
            Dim DR_New As Data.DataRow

            DS_New.Tables.Add("MyData")
            DS_New.Tables("MyData").Columns.Add("Seq")
            DS_New.Tables("MyData").Columns.Add("Name")
            DS_New.Tables("MyData").Columns.Add("Para")
            DS_New.Tables("MyData").Columns.Add("LCL")
            DS_New.Tables("MyData").Columns.Add("Value")
            DS_New.Tables("MyData").Columns.Add("UCL")
            'DS_New.Tables("MyData").Columns.Add("PK")
            'DS_New.Tables("MyData").Columns.Add("LK")
            'DS_New.Tables("MyData").Columns.Add("TK")
            'DS_New.Tables("MyData").Columns.Add("OCAP_Low")
            'DS_New.Tables("MyData").Columns.Add("OCAP_High")

            'Dim DS_New_info As New Data.DataSet
            Dim DR_New_Info As Data.DataRow


            If Not DS_Para.Tables(0).Rows.Count = 0 Then
                For i As Int16 = 0 To DS_Para.Tables(0).Rows.Count - 1
                    DR_Para = DS_Para.Tables(0).Rows(i)

                    DR_New = DS_New.Tables("MyData").NewRow
                    DR_New_Info = DS_New_info.Tables("MyData").NewRow

                    DR_New("Seq") = DR_Para("Seq_Flow")
                    DR_New("Name") = DR_Para("Name")
                    DR_New("Para") = "AVG"
                    DR_New("LCL") = DR_Para("Avg_LCL")
                    DR_New("Value") = DR(DR_Para("DB_Column").ToString & "_AVG")
                    DR_New("UCL") = DR_Para("Avg_UCL")

                    DR_New_Info("Seq") = DR_Para("Seq_Flow")
                    DR_New_Info("Name") = DR_Para("Name")
                    DR_New_Info("Para") = "AVG"
                    DR_New_Info("LCL") = DR_Para("Avg_LCL")
                    DR_New_Info("Value") = DR(DR_Para("DB_Column").ToString & "_AVG")
                    DR_New_Info("UCL") = DR_Para("Avg_UCL")
                    DR_New_Info("PK") = DR_Para("Parameter_Key")
                    DR_New_Info("LK") = DR_Para("Limit_Key")
                    DR_New_Info("TK") = DR_Para("Tool_Key")
                    DR_New_Info("OCAP_Low") = DR_Para("OCAP_Low")
                    DR_New_Info("OCAP_High") = DR_Para("OCAP_High")
                    DR_New_Info("Recipe") = DR("Recipe") 'Recipe

                    DS_New.Tables("MyData").Rows.Add(DR_New)
                    DS_New_info.Tables("MyData").Rows.Add(DR_New_Info)


                    If Not DR("R_Count") = "1" Then

                        DR_New = DS_New.Tables("MyData").NewRow
                        DR_New_Info = DS_New_info.Tables("MyData").NewRow

                        DR_New("Seq") = DR_Para("Seq_Flow")
                        DR_New("Name") = DR_Para("Name")
                        DR_New("Para") = "StDev"
                        DR_New("LCL") = DR_Para("Stdev_LCL")
                        DR_New("Value") = DR(DR_Para("DB_Column").ToString & "_StDev")
                        DR_New("UCL") = DR_Para("Stdev_UCL")

                        DR_New_Info("Seq") = DR_Para("Seq_Flow")
                        DR_New_Info("Name") = DR_Para("Name")
                        DR_New_Info("Para") = "StDev"
                        DR_New_Info("LCL") = DR_Para("Stdev_LCL")
                        DR_New_Info("Value") = DR(DR_Para("DB_Column").ToString & "_StDev")
                        DR_New_Info("UCL") = DR_Para("Stdev_UCL")
                        DR_New_Info("PK") = DR_Para("Parameter_Key")
                        DR_New_Info("LK") = DR_Para("Limit_Key")
                        DR_New_Info("TK") = DR_Para("Tool_Key")
                        DR_New_Info("OCAP_Low") = DR_Para("OCAP_Low")
                        DR_New_Info("OCAP_High") = DR_Para("OCAP_High")
                        DR_New_Info("Recipe") = DR("Recipe") 'Recipe

                        DS_New.Tables("MyData").Rows.Add(DR_New)
                        DS_New_info.Tables("MyData").Rows.Add(DR_New_Info)

                    End If



                Next
            End If
            Me.GridViewData.DataSource = DS_New
            Me.GridViewData.DataBind()



            Dim L As Double
            Dim U As Double
            Dim V As Double

            ' Green #33CC33

            Dim MyGreen As String = "#33CC33"
            Dim MyYellow As String = "#FFFF99"
            Dim MyRed As String = "#FF3300"
            Dim Pass As Boolean = True
            Dim OCAP As String = ""
            Dim OCAP_Message As String = ""

            For i = 0 To DS_New.Tables(0).Rows.Count - 1
                L = Me.GridViewData.Rows(i).Cells(3).Text
                V = Me.GridViewData.Rows(i).Cells(4).Text
                U = Me.GridViewData.Rows(i).Cells(5).Text

                Me.GridViewData.Rows(i).Cells(3).BackColor = Drawing.Color.LemonChiffon 'Drawing.Color.FromName(MyYellow)
                Me.GridViewData.Rows(i).Cells(5).BackColor = Drawing.Color.LemonChiffon 'Drawing.Color.FromName(MyYellow)

                If V > L And V < U Then
                    Me.GridViewData.Rows(i).Cells(4).BackColor = Drawing.Color.LightGreen 'Drawing.Color.FromName(MyGreen)
                Else
                    Pass = False
                    Me.GridViewData.Rows(i).Cells(4).BackColor = Drawing.Color.Salmon 'Drawing.Color.FromName(MyRed)
                    If V <= L Then
                        ' Me.GridViewData.Rows(i).Cells(3).BackColor = Drawing.Color.LightYellow
                        Me.GridViewData.Rows(i).Cells(3).Font.Bold = True
                        Me.GridViewData.Rows(i).Cells(4).Font.Bold = True

                        DR_New_Info = DS_New_info.Tables("MyData").Rows(i) 'OCAP_Low
                        OCAP = OCAP & DR_New_Info("Name") & ": "
                        OCAP = OCAP & DR_New_Info("OCAP_Low") & ". "
                        OCAP_Message = OCAP_Message & DR_New_Info("Name") & ", " & DR_New_Info("Para") & " Is Out. Below LUL. OCAP Given, " & DR_New_Info("OCAP_Low") & "<br />"
                    End If
                    If V >= U Then
                        'Me.GridViewData.Rows(i).Cells(5).BackColor = Drawing.Color.LightYellow
                        Me.GridViewData.Rows(i).Cells(5).Font.Bold = True
                        Me.GridViewData.Rows(i).Cells(4).Font.Bold = True

                        DR_New_Info = DS_New_info.Tables("MyData").Rows(i) 'OCAP_Low
                        OCAP = OCAP & DR_New_Info("Name") & ": "
                        OCAP = OCAP & DR_New_Info("OCAP_High") & ". "
                        OCAP_Message = OCAP_Message & DR_New_Info("Name") & ", " & DR_New_Info("Para") & " Is Out. Above UCL. OCAP Given, " & DR_New_Info("OCAP_Low") & "<br />"
                    End If
                End If
                '"<font size=5>This is</font> <font color=red><b>a test</b></font>"
            Next
            If Pass = True Then
                Me.Label_OCAP.Text = "Pass"
                Me.Label_OCAP_Message.Text = "Pass"
            Else
                Me.Label_OCAP.Text = OCAP
                Me.Label_OCAP_Message.Text = OCAP_Message
            End If

            'Find out if the DSC was used
            'SELECT [Key], TimeStamp, DSC, Op, Recipe, Tool_Key, Para_Key, Limit_Key, Value, Condition, OCAP, DataPoints FROM dbo.T_SPC_DataPoints WHERE (TimeStamp > CONVERT(DATETIME, '2018-08-01 00:00:00', 102)) AND (DSC = N'111111111')
            Dim MySQL As String = ""
            MySQL = "SELECT [Key], TimeStamp, DSC, Op, Recipe, Tool_Key, Para_Key, Limit_Key, Value, Condition, OCAP, DataPoints FROM dbo.T_SPC_DataPoints WHERE (TimeStamp >  CONVERT(DATETIME, '" & GetDateRange() & "', 102)) AND (DSC = N'" & DSC & "')"
            DS = SatiCode.GetMyDataSetSPCData(MySQL)

            If DS.Tables(0).Rows.Count = 0 Then
                Me.ButtonSubmit.Visible = True
            Else
                Me.ButtonSubmit.Visible = False
                DR = DS.Tables(0).Rows(0)
                Me.Label_OCAP.Text = "This was submitted on " & DR("TimeStamp") & " By " & DR("Op")
            End If

        End If
    End Sub

    Protected Sub DropDownList_Last_DSC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_Last_DSC.SelectedIndexChanged
        BuildData()
    End Sub

    Function GetDateRange() As DateTime
        GetDateRange = DateTime.Now.AddMonths(-2)
    End Function

    Protected Sub ButtonSubmit_Click(sender As Object, e As EventArgs) Handles ButtonSubmit.Click
        RecordData()


        BuildData()

    End Sub

    Sub RecordData()
        Dim DSC As String = Me.DropDownList_Last_DSC.SelectedItem.Text
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim SQL_Function As String = Me.Label_SQLfunction.Text

        DS = SatiCode.GetMyDataSetAutoData("SELECT [" & SQL_Function & "_1].* FROM dbo.[" & SQL_Function & "]() AS [" & SQL_Function & "_1] WHERE (DSC = N'" & DSC & "')")
        dr = ds.Tables(0).Rows(0)

        Dim DS_Para As New Data.DataSet
        Dim DR_Para As Data.DataRow
        Dim Tool As String = Me.DropDownListTools.SelectedItem.Text
        'SELECT TOP (100) PERCENT dbo.T_SPC_Tool_Info.Tool_Name, dbo.T_SPC_Parameters.Seq_Flow, dbo.T_SPC_Parameters.Name, dbo.T_SPC_Parameters.DB_Column, dbo.T_SPC_Limits.Avg_LCL, dbo.T_SPC_Limits.Avg_UCL, dbo.T_SPC_Limits.Stdev_LCL, dbo.T_SPC_Limits.Stdev_UCL, dbo.T_SPC_Limits.Parameter_Key, dbo.T_SPC_Limits.[Key] AS Limit_Key FROM dbo.T_SPC_Parameters INNER JOIN dbo.T_SPC_Tool_Info ON dbo.T_SPC_Parameters.Tool_Key = dbo.T_SPC_Tool_Info.[Key] INNER JOIN dbo.T_SPC_Limits ON dbo.T_SPC_Parameters.[Key] = dbo.T_SPC_Limits.Parameter_Key WHERE (dbo.T_SPC_Tool_Info.Tool_Name = N'SP1-1') AND (dbo.T_SPC_Parameters.Enable = 1) AND (dbo.T_SPC_Limits.Enable = 1) ORDER BY dbo.T_SPC_Parameters.Seq_Flow
        DS_Para = SatiCode.GetMyDataSetSPCData("SELECT TOP (100) PERCENT dbo.T_SPC_Tool_Info.Tool_Name, dbo.T_SPC_Parameters.Seq_Flow, dbo.T_SPC_Parameters.Name, dbo.T_SPC_Parameters.DB_Column, dbo.T_SPC_Limits.Avg_LCL, dbo.T_SPC_Limits.Avg_UCL, dbo.T_SPC_Limits.Stdev_LCL, dbo.T_SPC_Limits.Stdev_UCL, dbo.T_SPC_Limits.Parameter_Key, dbo.T_SPC_Limits.[Key] AS Limit_Key, dbo.T_SPC_Tool_Info.[Key] AS Tool_Key, dbo.T_SPC_Parameters.OCAP_Low, dbo.T_SPC_Parameters.OCAP_High FROM dbo.T_SPC_Parameters INNER JOIN dbo.T_SPC_Tool_Info ON dbo.T_SPC_Parameters.Tool_Key = dbo.T_SPC_Tool_Info.[Key] INNER JOIN dbo.T_SPC_Limits ON dbo.T_SPC_Parameters.[Key] = dbo.T_SPC_Limits.Parameter_Key WHERE (dbo.T_SPC_Tool_Info.Tool_Name = N'" & Tool & "') AND (dbo.T_SPC_Parameters.Enable = 1) AND (dbo.T_SPC_Limits.Enable = 1) ORDER BY dbo.T_SPC_Parameters.Seq_Flow")


        Dim DR_New_Info As Data.DataRow




        If Not DS_Para.Tables(0).Rows.Count = 0 Then
            For i As Int16 = 0 To DS_Para.Tables(0).Rows.Count - 1
                DR_Para = DS_Para.Tables(0).Rows(i)

                DR_New_Info = DS_New_info.Tables("MyData").NewRow

                DR_New_Info("Seq") = DR_Para("Seq_Flow")
                DR_New_Info("Name") = DR_Para("Name")
                DR_New_Info("Para") = "AVG"
                DR_New_Info("LCL") = DR_Para("Avg_LCL")
                DR_New_Info("Value") = dr(DR_Para("DB_Column").ToString & "_AVG")
                DR_New_Info("UCL") = DR_Para("Avg_UCL")
                DR_New_Info("PK") = DR_Para("Parameter_Key")
                DR_New_Info("LK") = DR_Para("Limit_Key")
                DR_New_Info("TK") = DR_Para("Tool_Key")
                DR_New_Info("OCAP_Low") = DR_Para("OCAP_Low")
                DR_New_Info("OCAP_High") = DR_Para("OCAP_High")
                DR_New_Info("Recipe") = DR("Recipe") 'Recipe
                DR_New_Info("RC") = DR("R_Count")

                DS_New_info.Tables("MyData").Rows.Add(DR_New_Info)

                If Not DR("R_Count") = "1" Then

                    DR_New_Info = DS_New_info.Tables("MyData").NewRow

                    DR_New_Info("Seq") = DR_Para("Seq_Flow")
                    DR_New_Info("Name") = DR_Para("Name")
                    DR_New_Info("Para") = "StDev"
                    DR_New_Info("LCL") = DR_Para("Stdev_LCL")
                    DR_New_Info("Value") = DR(DR_Para("DB_Column").ToString & "_StDev")
                    DR_New_Info("UCL") = DR_Para("Stdev_UCL")
                    DR_New_Info("PK") = DR_Para("Parameter_Key")
                    DR_New_Info("LK") = DR_Para("Limit_Key")
                    DR_New_Info("TK") = DR_Para("Tool_Key")
                    DR_New_Info("OCAP_Low") = DR_Para("OCAP_Low")
                    DR_New_Info("OCAP_High") = DR_Para("OCAP_High")
                    DR_New_Info("Recipe") = DR("Recipe") 'Recipe
                    DR_New_Info("RC") = DR("R_Count")

                    DS_New_info.Tables("MyData").Rows.Add(DR_New_Info)

                End If

            Next
        End If

        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("SATI_SPCConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        'Dim DS As New Data.DataSet
        'Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT [Key], TimeStamp, DSC, Op, Recipe, Seq, Name, Para, LCL, Value, UCL, Tool_Key, Para_Key, Limit_Key, DataPoints, Condition, OCAP FROM T_SPC_DataPoints WHERE ([Key] = 0)"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        Dim InsertCmd As New System.Data.SqlClient.SqlCommand
        With InsertCmd
            .CommandText = "INSERT INTO [T_SPC_DataPoints] ([TimeStamp], [DSC], [Op], [Recipe], [Seq], [Name], [Para], [LCL], [Value], [UCL], [Tool_Key], [Para_Key], [Limit_Key], [DataPoints], [Condition], [OCAP]) VALUES (@TimeStamp, @DSC, @Op, @Recipe, @Seq, @Name, @Para, @LCL, @Value, @UCL, @Tool_Key, @Para_Key, @Limit_Key, @DataPoints, @Condition, @OCAP); SELECT [Key], TimeStamp, DSC, Op, Recipe, Seq, Name, Para, LCL, Value, UCL, Tool_Key, Para_Key, Limit_Key, DataPoints, Condition, OCAP FROM T_SPC_DataPoints WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@TimeStamp", System.Data.SqlDbType.DateTime, 0, "TimeStamp"), New System.Data.SqlClient.SqlParameter("@DSC", System.Data.SqlDbType.NVarChar, 0, "DSC"), New System.Data.SqlClient.SqlParameter("@Op", System.Data.SqlDbType.NVarChar, 0, "Op"), New System.Data.SqlClient.SqlParameter("@Recipe", System.Data.SqlDbType.NVarChar, 0, "Recipe"), New System.Data.SqlClient.SqlParameter("@Seq", System.Data.SqlDbType.Int, 0, "Seq"), New System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.NVarChar, 0, "Name"), New System.Data.SqlClient.SqlParameter("@Para", System.Data.SqlDbType.NVarChar, 0, "Para"), New System.Data.SqlClient.SqlParameter("@LCL", System.Data.SqlDbType.Float, 0, "LCL"), New System.Data.SqlClient.SqlParameter("@Value", System.Data.SqlDbType.Float, 0, "Value"), New System.Data.SqlClient.SqlParameter("@UCL", System.Data.SqlDbType.Float, 0, "UCL"), New System.Data.SqlClient.SqlParameter("@Tool_Key", System.Data.SqlDbType.Int, 0, "Tool_Key"), New System.Data.SqlClient.SqlParameter("@Para_Key", System.Data.SqlDbType.Int, 0, "Para_Key"), New System.Data.SqlClient.SqlParameter("@Limit_Key", System.Data.SqlDbType.Int, 0, "Limit_Key"), New System.Data.SqlClient.SqlParameter("@DataPoints", System.Data.SqlDbType.Int, 0, "DataPoints"), New System.Data.SqlClient.SqlParameter("@Condition", System.Data.SqlDbType.NVarChar, 0, "Condition"), New System.Data.SqlClient.SqlParameter("@OCAP", System.Data.SqlDbType.NVarChar, 0, "OCAP")})
        End With
        DA.InsertCommand = InsertCmd

        Dim UpdateCmd As New System.Data.SqlClient.SqlCommand
        With UpdateCmd
            .CommandText = "UPDATE [T_SPC_DataPoints] SET [TimeStamp] = @TimeStamp, [DSC] = @DSC, [Op] = @Op, [Recipe] = @Recipe, [Seq] = @Seq, [Name] = @Name, [Para] = @Para, [LCL] = @LCL, [Value] = @Value, [UCL] = @UCL, [Tool_Key] = @Tool_Key, [Para_Key] = @Para_Key, [Limit_Key] = @Limit_Key, [DataPoints] = @DataPoints, [Condition] = @Condition, [OCAP] = @OCAP WHERE (([Key] = @Original_Key) AND ((@IsNull_TimeStamp = 1 AND [TimeStamp] IS NULL) OR ([TimeStamp] = @Original_TimeStamp)) AND ((@IsNull_DSC = 1 AND [DSC] IS NULL) OR ([DSC] = @Original_DSC)) AND ((@IsNull_Op = 1 AND [Op] IS NULL) OR ([Op] = @Original_Op)) AND ((@IsNull_Recipe = 1 AND [Recipe] IS NULL) OR ([Recipe] = @Original_Recipe)) AND ((@IsNull_Seq = 1 AND [Seq] IS NULL) OR ([Seq] = @Original_Seq)) AND ((@IsNull_Name = 1 AND [Name] IS NULL) OR ([Name] = @Original_Name)) AND ((@IsNull_Para = 1 AND [Para] IS NULL) OR ([Para] = @Original_Para)) AND ((@IsNull_LCL = 1 AND [LCL] IS NULL) OR ([LCL] = @Original_LCL)) AND ((@IsNull_Value = 1 AND [Value] IS NULL) OR ([Value] = @Original_Value)) AND ((@IsNull_UCL = 1 AND [UCL] IS NULL) OR ([UCL] = @Original_UCL)) AND ((@IsNull_Tool_Key = 1 AND [Tool_Key] IS NULL) OR ([Tool_Key] = @Original_Tool_Key)) AND ((@IsNull_Para_Key = 1 AND [Para_Key] IS NULL) OR ([Para_Key] = @Original_Para_Key)) AND ((@IsNull_Limit_Key = 1 AND [Limit_Key] IS NULL) OR ([Limit_Key] = @Original_Limit_Key)) AND ((@IsNull_DataPoints = 1 AND [DataPoints] IS NULL) OR ([DataPoints] = @Original_DataPoints)) AND ((@IsNull_Condition = 1 AND [Condition] IS NULL) OR ([Condition] = @Original_Condition)) AND ((@IsNull_OCAP = 1 AND [OCAP] IS NULL) OR ([OCAP] = @Original_OCAP))); SELECT [Key], TimeStamp, DSC, Op, Recipe, Seq, Name, Para, LCL, Value, UCL, Tool_Key, Para_Key, Limit_Key, DataPoints, Condition, OCAP FROM T_SPC_DataPoints WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@TimeStamp", System.Data.SqlDbType.DateTime, 0, "TimeStamp"), New System.Data.SqlClient.SqlParameter("@DSC", System.Data.SqlDbType.NVarChar, 0, "DSC"), New System.Data.SqlClient.SqlParameter("@Op", System.Data.SqlDbType.NVarChar, 0, "Op"), New System.Data.SqlClient.SqlParameter("@Recipe", System.Data.SqlDbType.NVarChar, 0, "Recipe"), New System.Data.SqlClient.SqlParameter("@Seq", System.Data.SqlDbType.Int, 0, "Seq"), New System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.NVarChar, 0, "Name"), New System.Data.SqlClient.SqlParameter("@Para", System.Data.SqlDbType.NVarChar, 0, "Para"), New System.Data.SqlClient.SqlParameter("@LCL", System.Data.SqlDbType.Float, 0, "LCL"), New System.Data.SqlClient.SqlParameter("@Value", System.Data.SqlDbType.Float, 0, "Value"), New System.Data.SqlClient.SqlParameter("@UCL", System.Data.SqlDbType.Float, 0, "UCL"), New System.Data.SqlClient.SqlParameter("@Tool_Key", System.Data.SqlDbType.Int, 0, "Tool_Key"), New System.Data.SqlClient.SqlParameter("@Para_Key", System.Data.SqlDbType.Int, 0, "Para_Key"), New System.Data.SqlClient.SqlParameter("@Limit_Key", System.Data.SqlDbType.Int, 0, "Limit_Key"), New System.Data.SqlClient.SqlParameter("@DataPoints", System.Data.SqlDbType.Int, 0, "DataPoints"), New System.Data.SqlClient.SqlParameter("@Condition", System.Data.SqlDbType.NVarChar, 0, "Condition"), New System.Data.SqlClient.SqlParameter("@OCAP", System.Data.SqlDbType.NVarChar, 0, "OCAP"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_TimeStamp", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "TimeStamp", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_TimeStamp", System.Data.SqlDbType.DateTime, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "TimeStamp", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_DSC", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "DSC", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_DSC", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DSC", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Op", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Op", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Op", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Op", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Recipe", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Recipe", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Recipe", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Recipe", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Seq", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Seq", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Seq", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Seq", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Name", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Name", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Name", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Name", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Para", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Para", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Para", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Para", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_LCL", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "LCL", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_LCL", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "LCL", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Value", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Value", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Value", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Value", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_UCL", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "UCL", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_UCL", System.Data.SqlDbType.Float, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "UCL", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Tool_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Tool_Key", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Tool_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Tool_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Para_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Para_Key", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Para_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Para_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Limit_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Limit_Key", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Limit_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Limit_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_DataPoints", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "DataPoints", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_DataPoints", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DataPoints", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Condition", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Condition", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Condition", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Condition", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_OCAP", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "OCAP", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_OCAP", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "OCAP", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = UpdateCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_SPC_DataPoints", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("TimeStamp", "TimeStamp"), New System.Data.Common.DataColumnMapping("DSC", "DSC"), New System.Data.Common.DataColumnMapping("Op", "Op"), New System.Data.Common.DataColumnMapping("Recipe", "Recipe"), New System.Data.Common.DataColumnMapping("Seq", "Seq"), New System.Data.Common.DataColumnMapping("Name", "Name"), New System.Data.Common.DataColumnMapping("Para", "Para"), New System.Data.Common.DataColumnMapping("LCL", "LCL"), New System.Data.Common.DataColumnMapping("Value", "Value"), New System.Data.Common.DataColumnMapping("UCL", "UCL"), New System.Data.Common.DataColumnMapping("Tool_Key", "Tool_Key"), New System.Data.Common.DataColumnMapping("Para_Key", "Para_Key"), New System.Data.Common.DataColumnMapping("Limit_Key", "Limit_Key"), New System.Data.Common.DataColumnMapping("DataPoints", "DataPoints"), New System.Data.Common.DataColumnMapping("Condition", "Condition"), New System.Data.Common.DataColumnMapping("OCAP", "OCAP")})})
        DA.Fill(DS)



        For i = 0 To DS_New_info.Tables(0).Rows.Count - 1
            DR_New_Info = DS_New_info.Tables(0).Rows(i)

            DR = DS.Tables("T_SPC_DataPoints").NewRow

            DR("TimeStamp") = DateAndTime.Now.ToLongTimeString
            DR("DSC") = Me.DropDownList_Last_DSC.SelectedItem.Text
            DR("Op") = UCase(User.Identity.Name.ToString)
            DR("Recipe") = DR_New_Info("Recipe")
            DR("Seq") = DR_New_Info("Seq")
            DR("Name") = DR_New_Info("Name")
            DR("Para") = DR_New_Info("Para")
            DR("LCL") = DR_New_Info("LCL")
            DR("Value") = DR_New_Info("Value")
            DR("UCL") = DR_New_Info("UCL")
            DR("Tool_Key") = DR_New_Info("TK")
            DR("Para_Key") = DR_New_Info("PK")
            DR("Limit_Key") = DR_New_Info("LK")
            DR("Condition") = ""
            DR("OCAP") = ""
            DR("DataPoints") = DR_New_Info("RC")

            DS.Tables("T_SPC_DataPoints").Rows.Add(DR)
            DA.Update(DS, "T_SPC_DataPoints")
            Connection.Close()

        Next

        If Not Me.Label_OCAP_Message.Text = "Pass" Then
            'Dim MyHTML As HtmlString
            Dim SB As New StringBuilder

            Dim SB2 As New StringBuilder ' StringBuilder SB = New StringBuilder();
            Dim SW2 As New IO.StringWriter(SB2)  'StringWriter sw = New StringWriter(SB);
            Dim HW As New HtmlTextWriter(SW2) 'HtmlTextWriter hw = New HtmlTextWriter(sw);
            GridViewData.RenderControl(HW) 'gv.RenderControl(hw);



            SB.Append(<h1 style="color: #0000FF">SATI.SPC</h1>)
            SB.Append(<br/>)
            SB.Append("<font size=5>Tool:</font> &nbsp; <font size=5 color=red><b>" & Me.DropDownListTools.SelectedItem.Text & "</b></font>")
            SB.Append(<br/>)
            SB.Append(<br/>)
            SB.Append("<div>")
            SB.Append(Me.Label_OCAP_Message.Text)
            SB.Append("</div>")
            SB.Append(<br/>)
            SB.Append(SB2)
            SB.Append("<div align=right>")
            SB.Append("<font color=Gray>" & UCase(User.Identity.Name.ToString) & "</font>")
            SB.Append(<br/>)
            SB.Append("<font color=Gray>" & DateAndTime.Now.ToLongTimeString & "</font>")
            SB.Append(<br/>)
            SB.Append("<font color=Gray>" & DateTime.Now.ToLongDateString & "</font>")
            SB.Append("</div>")



            SatiCode.SendMail_HTML(SB.ToString, "SPC Out " & Me.DropDownListTools.SelectedItem.Text, "AZ.SATISPC@purewafer.com", "AZ.SATISPC@purewafer.com")


        End If


        'Me.GridView1.DataSource = DS_New_info
        'Me.GridView1.DataBind()


    End Sub

    Sub UpdateToolData(Tool As String)
        Dim AutoDataConnection As New Data.SqlClient.SqlConnection
        AutoDataConnection.ConnectionString = ConfigurationManager.ConnectionStrings("AutoDataConnectionString").ConnectionString
        AutoDataConnection.Open()

        Select Case Tool
            Case "SP1"
                'Update SP1 Data
                Dim SP1DataCollector As New System.Data.SqlClient.SqlCommand
                With SP1DataCollector
                    .CommandText = "exsil_user.[SP1DataCollector_SP11Only]"
                    .CommandType = System.Data.CommandType.StoredProcedure
                    .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, False, CType(0, Byte), CType(0, Byte), "", System.Data.DataRowVersion.Current, Nothing)})
                    .Connection = AutoDataConnection
                End With
                SP1DataCollector.ExecuteNonQuery()
                AutoDataConnection.Close()
            Case "SP2"
                'Update SP12 Data
                Dim SP1DataCollector As New System.Data.SqlClient.SqlCommand
                With SP1DataCollector
                    .CommandText = "exsil_user.[SP1DataCollector_SP12Only]"
                    .CommandType = System.Data.CommandType.StoredProcedure
                    .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, False, CType(0, Byte), CType(0, Byte), "", System.Data.DataRowVersion.Current, Nothing)})
                    .Connection = AutoDataConnection
                End With
                SP1DataCollector.ExecuteNonQuery()
                AutoDataConnection.Close()
            Case "SP1-3"
                'Update SP12 Data
                Dim SP1DataCollector As New System.Data.SqlClient.SqlCommand
                With SP1DataCollector
                    .CommandText = "exsil_user.[SP1DataCollector_SP13Only]"
                    .CommandType = System.Data.CommandType.StoredProcedure
                    .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, False, CType(0, Byte), CType(0, Byte), "", System.Data.DataRowVersion.Current, Nothing)})
                    .Connection = AutoDataConnection
                End With
                SP1DataCollector.ExecuteNonQuery()
                AutoDataConnection.Close()
            Case ""

        End Select
    End Sub

End Class
