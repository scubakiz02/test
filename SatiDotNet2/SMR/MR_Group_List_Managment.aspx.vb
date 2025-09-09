

Partial Class MR_MR_Group_List_Managment
    Inherits System.Web.UI.Page
    Dim Saticode As New Class1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)
    End Sub
    Protected Sub RadioButtonNormal_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonNormal.CheckedChanged
        TypeChange()
    End Sub

    Protected Sub RadioButtonReport_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonReport.CheckedChanged
        TypeChange()
    End Sub

    Sub TypeChange()

        'SELECT ListName FROM dbo.T_MR_GroupLists WHERE (ReportList = 0)
        Dim DS As Data.DataSet
        Dim DR As Data.DataRow

        Me.PanelView.Visible = True
        Me.PanelNew.Visible = False
        Me.CheckBoxListViewedGroup.Visible = False
        Me.ButtonSaveChange.Visible = False

        If Me.RadioButtonNormal.Checked = True Then
            DS = Saticode.GetMyDataSet("SELECT ListName FROM dbo.T_MR_GroupLists WHERE (ReportList = 0) GROUP BY ListName")
        End If
        If Me.RadioButtonReport.Checked = True Then
            DS = Saticode.GetMyDataSet("SELECT ListName FROM dbo.T_MR_GroupLists WHERE (ReportList = 1) GROUP BY ListName")
        End If

        Me.DropDownListGroups.Items.Clear()
        Me.DropDownListGroups.Items.Add("Select Item..")

        If DS.Tables(0).Rows.Count > 0 Then

            For I As Integer = 0 To DS.Tables(0).Rows.Count - 1
                DR = DS.Tables(0).Rows(I)
                Me.DropDownListGroups.Items.Add(DR("ListName").ToString)
            Next

        End If


    End Sub

    Protected Sub RadioButtonAddNew_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonAddNew.CheckedChanged

        If Me.RadioButtonAddNew.Checked = True Then
            Me.PanelNew.Visible = True
            Me.PanelView.Visible = False
            ClearNewPanel()
            LoadToolsForNewGroup()
        End If

    End Sub

    Sub ClearNewPanel()
        Me.CheckBoxListNewGroupTools.Items.Clear()
        Me.TextBoxNewGroupListName.Text = ""
    End Sub

    Protected Sub ButtonMakeNewGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonMakeNewGroup.Click
        If Not Me.TextBoxNewGroupListName.Text = "" Then
            SaveNewGroup()
        End If
    End Sub

    Function IsGroupNameTaken(ByVal Name As String) As Boolean
        'SELECT ListName FROM dbo.T_MR_GroupLists WHERE (ReportList = 0 HAVING (ListName = 'test')
        'SELECT ListName FROM dbo.T_MR_GroupLists WHERE (ReportList = 0) GROUP BY ListName HAVING (ListName = 'test')
        Dim DS As Data.DataSet
        If Me.CheckBoxNewGroupList_ReportOrNot.Checked = True Then
            DS = Saticode.GetMyDataSet("SELECT ListName FROM dbo.T_MR_GroupLists WHERE (ReportList = 1) GROUP BY ListName HAVING (ListName = '" & Name & "')")
        Else
            DS = Saticode.GetMyDataSet("SELECT ListName FROM dbo.T_MR_GroupLists WHERE (ReportList = 0) GROUP BY ListName HAVING (ListName = '" & Name & "')")
        End If
        If DS.Tables(0).Rows.Count > 0 Then
            IsGroupNameTaken = True
        Else
            IsGroupNameTaken = False
        End If
    End Function

    Protected Sub ButtonSaveChange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSaveChange.Click

    End Sub

    Sub LoadToolsForNewGroup()
        'SELECT TOP 100 PERCENT Department, Tool FROM dbo.T_Tools ORDER BY Department, Tool
        Dim DS As Data.DataSet
        Dim DR As Data.DataRow

        DS = Saticode.GetMyDataSet("SELECT TOP 100 PERCENT Department, Tool, [Key] FROM dbo.T_Tools ORDER BY Department, Tool")
        Me.CheckBoxListNewGroupTools.Items.Clear()

        For I As Integer = 0 To DS.Tables(0).Rows.Count - 1
            DR = DS.Tables(0).Rows(I)
            Me.CheckBoxListNewGroupTools.Items.Add(DR("Department").ToString & ", " & DR("Tool").ToString)
            Me.CheckBoxListNewGroupTools.Items(I).Value = DR("Key").ToString
        Next

    End Sub

    Sub LoadGroup()
        Me.CheckBoxListViewedGroup.Visible = True
        Me.ButtonSaveChange.Visible = True
        Dim DS As Data.DataSet
        Dim DS2 As Data.DataSet
        Dim DR As Data.DataRow
        Dim MySql As String = ""

        'Load The Tool List
        DS = Saticode.GetMyDataSet("SELECT TOP 100 PERCENT Department, Tool, [Key] FROM dbo.T_Tools ORDER BY Department, Tool")
        Me.CheckBoxListViewedGroup.Items.Clear()

        For I As Integer = 0 To DS.Tables(0).Rows.Count - 1
            DR = DS.Tables(0).Rows(I)
            Me.CheckBoxListViewedGroup.Items.Add(DR("Department").ToString & ", " & DR("Tool").ToString)
            Me.CheckBoxListViewedGroup.Items(I).Value = DR("Key").ToString
        Next

        'Check The Tools In Group List
        'SELECT [Key], ToolKey, ListName, ReportList FROM dbo.T_MR_GroupLists WHERE (ListName = 'test1group') AND (ReportList = 1)
        If Me.RadioButtonNormal.Checked = True Then
            MySql = "SELECT [Key], ToolKey, ListName, ReportList FROM dbo.T_MR_GroupLists WHERE (ListName = '" & Me.DropDownListGroups.SelectedItem.Text & "') AND (ReportList = 0)"
            'DS2 = Saticode.GetMyDataSet("SELECT [Key], ToolKey, ListName, ReportList FROM dbo.T_MR_GroupLists WHERE (ListName = '" & Me.DropDownListGroups.SelectedItem.Text & "') AND (ReportList = 0)")
        End If

        If Me.RadioButtonReport.Checked = True Then
            MySql = "SELECT [Key], ToolKey, ListName, ReportList FROM dbo.T_MR_GroupLists WHERE (ListName = '" & Me.DropDownListGroups.SelectedItem.Text & "') AND (ReportList = 1)"
            'DS2 = Saticode.GetMyDataSet("SELECT [Key], ToolKey, ListName, ReportList FROM dbo.T_MR_GroupLists WHERE (ListName = '" & Me.DropDownListGroups.SelectedItem.Text & "') AND (ReportList = 1)")
        End If
        DS2 = Saticode.GetMyDataSet(MySql)

        For I As Integer = 0 To Me.CheckBoxListViewedGroup.Items.Count - 1
            For II As Integer = 0 To DS2.Tables(0).Rows.Count - 1
                DR = DS2.Tables(0).Rows(II)
                If Me.CheckBoxListViewedGroup.Items(I).Value = DR("ToolKey").ToString Then
                    Me.CheckBoxListViewedGroup.Items(I).Selected = True
                End If
            Next
        Next


    End Sub

    Sub SaveGroup()

    End Sub

    Sub SaveNewGroup()
        Me.LabelMakeNewGroupFeedBack.Text = ""
        If IsGroupNameTaken(Me.TextBoxNewGroupListName.Text) = True Then
            Me.LabelMakeNewGroupFeedBack.Text = "The List Name Has Been Taken! Group Not Saved"
            Exit Sub
        End If
        ModGroup(Me.TextBoxNewGroupListName.Text, "Add")

    End Sub

    Sub ModGroup(ByVal GroupName As String, ByVal Action_Add_Remove_Update As String)
        Dim ItemCount As Integer = 0
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        '*****************************************************************
        '************************Select***********************************
        '*****************************************************************
        Dim MR_GroupList_SelectCmd As New System.Data.SqlClient.SqlCommand
        With MR_GroupList_SelectCmd
            .CommandText = "SELECT [Key], ToolKey, ListName, ReportName, ReportList FROM dbo.T_MR_GroupLists WHERE (ListName = '" & GroupName & "')"
            .Connection = Connection
        End With
        DA.SelectCommand = MR_GroupList_SelectCmd

        '*****************************************************************
        '************************Insert***********************************
        '*****************************************************************
        Dim MR_GroupList_InsertCmd As New System.Data.SqlClient.SqlCommand
        With MR_GroupList_InsertCmd
            .CommandText = "INSERT INTO [dbo].[T_MR_GroupLists] ([ToolKey], [ListName], [ReportName], [ReportList]) VALUES (@ToolKey, @ListName, @ReportName, @ReportList); SELECT [Key], ToolKey, ListName, ReportName, ReportList FROM dbo.T_MR_GroupLists WHERE ([Key] = SCOPE_IDENTITY())"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@ToolKey", System.Data.SqlDbType.Int, 0, "ToolKey"), New System.Data.SqlClient.SqlParameter("@ListName", System.Data.SqlDbType.VarChar, 0, "ListName"), New System.Data.SqlClient.SqlParameter("@ReportName", System.Data.SqlDbType.VarBinary, 0, "ReportName"), New System.Data.SqlClient.SqlParameter("@ReportList", System.Data.SqlDbType.Bit, 0, "ReportList")})
        End With
        DA.InsertCommand = MR_GroupList_InsertCmd

        '*****************************************************************
        '************************Update***********************************
        '*****************************************************************
        Dim MR_GroupList_UpdateCmd As New System.Data.SqlClient.SqlCommand
        With MR_GroupList_UpdateCmd
            .CommandText = "UPDATE [dbo].[T_MR_GroupLists] SET [ToolKey] = @ToolKey, [ListName] = @ListName, [ReportName] = @ReportName, [ReportList] = @ReportList WHERE (([Key] = @Original_Key) AND ((@IsNull_ToolKey = 1 AND [ToolKey] IS NULL) OR ([ToolKey] = @Original_ToolKey)) AND ((@IsNull_ListName = 1 AND [ListName] IS NULL) OR ([ListName] = @Original_ListName)) AND ((@IsNull_ReportName = 1 AND [ReportName] IS NULL) OR ([ReportName] = @Original_ReportName)) AND ((@IsNull_ReportList = 1 AND [ReportList] IS NULL) OR ([ReportList] = @Original_ReportList))); SELECT [Key], ToolKey, ListName, ReportName, ReportList FROM dbo.T_MR_GroupLists WHERE ([Key] = @Key)"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@ToolKey", System.Data.SqlDbType.Int, 0, "ToolKey"), New System.Data.SqlClient.SqlParameter("@ListName", System.Data.SqlDbType.VarChar, 0, "ListName"), New System.Data.SqlClient.SqlParameter("@ReportName", System.Data.SqlDbType.VarBinary, 0, "ReportName"), New System.Data.SqlClient.SqlParameter("@ReportList", System.Data.SqlDbType.Bit, 0, "ReportList"), New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_ToolKey", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "ToolKey", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_ToolKey", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ToolKey", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_ListName", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "ListName", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_ListName", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ListName", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_ReportName", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "ReportName", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_ReportName", System.Data.SqlDbType.VarBinary, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReportName", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_ReportList", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "ReportList", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_ReportList", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReportList", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Key", System.Data.SqlDbType.Int, 4, "Key")})
        End With
        DA.UpdateCommand = MR_GroupList_UpdateCmd

        '*****************************************************************
        '************************Delete***********************************
        '*****************************************************************
        Dim MR_GroupList_DeleteCmd As New System.Data.SqlClient.SqlCommand
        With MR_GroupList_DeleteCmd
            .CommandText = "DELETE FROM [dbo].[T_MR_GroupLists] WHERE (([Key] = @Original_Key) AND ((@IsNull_ToolKey = 1 AND [ToolKey] IS NULL) OR ([ToolKey] = @Original_ToolKey)) AND ((@IsNull_ListName = 1 AND [ListName] IS NULL) OR ([ListName] = @Original_ListName)) AND ((@IsNull_ReportName = 1 AND [ReportName] IS NULL) OR ([ReportName] = @Original_ReportName)) AND ((@IsNull_ReportList = 1 AND [ReportList] IS NULL) OR ([ReportList] = @Original_ReportList)))"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Original_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_ToolKey", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "ToolKey", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_ToolKey", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ToolKey", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_ListName", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "ListName", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_ListName", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ListName", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_ReportName", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "ReportName", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_ReportName", System.Data.SqlDbType.VarBinary, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReportName", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_ReportList", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "ReportList", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_ReportList", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReportList", System.Data.DataRowVersion.Original, Nothing)})
        End With
        DA.DeleteCommand = MR_GroupList_DeleteCmd

        '*****************************************************************
        '************************Genral***********************************
        '*****************************************************************
        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_MR_GroupLists", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Key", "Key"), New System.Data.Common.DataColumnMapping("ToolKey", "ToolKey"), New System.Data.Common.DataColumnMapping("ListName", "ListName"), New System.Data.Common.DataColumnMapping("ReportName", "ReportName"), New System.Data.Common.DataColumnMapping("ReportList", "ReportList")})})

        Action_Add_Remove_Update = UCase(Action_Add_Remove_Update)
        Select Case Action_Add_Remove_Update
            Case "ADD"
                DA.Fill(DS)
                For A As Integer = 0 To Me.CheckBoxListNewGroupTools.Items.Count - 1
                    If Me.CheckBoxListNewGroupTools.Items(A).Selected = True Then
                        DR = DS.Tables("T_MR_GroupLists").NewRow
                        DR("ToolKey") = Me.CheckBoxListNewGroupTools.Items(A).Value
                        DR("ListName") = Me.TextBoxNewGroupListName.Text
                        DR("ReportList") = Me.CheckBoxNewGroupList_ReportOrNot.Checked

                        DS.Tables("T_MR_GroupLists").Rows.Add(DR)
                        DA.Update(DS, "T_MR_GroupLists")
                        If A = Me.CheckBoxListNewGroupTools.Items.Count - 1 Then
                            Me.LabelMakeNewGroupFeedBack.Text = "Saved"
                        End If

                    End If
                Next

            Case "REMOVE"


            Case "UPDATE"


        End Select

        Connection.Close()

    End Sub

    Protected Sub DropDownListGroups_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListGroups.SelectedIndexChanged
        If Not Me.DropDownListGroups.SelectedItem.Text = "Select Item.." Then
            LoadGroup()
        End If

    End Sub
End Class
