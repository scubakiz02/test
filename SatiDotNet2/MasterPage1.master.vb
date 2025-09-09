
Imports System.Windows.Forms

Partial Class MasterPage1
    Inherits System.Web.UI.MasterPage
    Dim ROOT_View As String

    Private Sub Page_Load(sender As Object, e As System.EventArgs)
        Me.Page.Form.DefaultFocus = MasterPagePanelMain.UniqueID

        If Not IsPostBack Then
            BuildDataSet()
        End If
    End Sub


    Function WaferBoxTOInstanceNumber(ByVal WB As String) As String
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT InstanceKey FROM dbo.T_FGI_Boxes WHERE (BoxInvNumber = " & WB & ")"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_FGI_Boxes", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("InstanceKey", "InstanceKey")})})
        DA.Fill(DS)
        Connection.Close()

        DR = DS.Tables(0).Rows(0)
        Return DR("InstanceKey").ToString

    End Function

    Function CardBoardBoxToWaferBox(ByVal CB As String) As String
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim DA As New Data.SqlClient.SqlDataAdapter
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow

        Dim SelectCmd As New System.Data.SqlClient.SqlCommand

        With SelectCmd
            .CommandText = "SELECT BoxInvNumber FROM dbo.T_FGI_Boxes WHERE (CartonNumber = " & CB & ")"
            .Connection = Connection
        End With
        DA.SelectCommand = SelectCmd

        DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_FGI_Boxes", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("BoxInvNumber", "BoxInvNumber")})})
        DA.Fill(DS)
        Connection.Close()

        DR = DS.Tables(0).Rows(0)
        Return DR("BoxInvNumber").ToString

    End Function

    Protected Sub CheckBoxViewTranslator_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxViewTranslator.CheckedChanged
        If Me.CheckBoxViewTranslator.Checked = True Then
            Me.PanelTranslator.Visible = True
        Else
            Me.PanelTranslator.Visible = False
        End If
    End Sub

    Protected Sub ButtonCloseWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCloseWindow.Click
        Me.PanelTranslator.Visible = False
        Me.CheckBoxViewTranslator.Checked = False
    End Sub

    Protected Sub ButtonFindInstance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFindInstance.Click
        Try
            If UCase(Me.TextBoxWB_to_In.Text) = "CP" Then
                Page.Response.Redirect("ChangePassword.aspx")
            Else
                Me.LabelIntance.Text = WaferBoxTOInstanceNumber(Me.TextBoxWB_to_In.Text)
            End If

        Catch ex As Exception
            Me.LabelIntance.Text = "None"
        End Try
    End Sub

    Protected Sub ButtonFindWaferBox_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFindWaferBox.Click
        Try
            Me.LabelWaferBox.Text = CardBoardBoxToWaferBox(Me.TextBoxCB_to_WB.Text)
        Catch ex As Exception
            Me.LabelWaferBox.Text = "None"
        End Try
    End Sub



    Protected Sub ComboSearchBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboSearchBox.SelectedIndexChanged
        ROOT_View = Session("SDS_View")

        If ComboSearchBox.SelectedValue = "" Or ComboSearchBox.SelectedValue = "Search By Name" Or ComboSearchBox.SelectedValue = "Search By Alias" Then
            ClearTextBox()
        Else
            OpenNewPage(Me.UpdatePaneSDS, ROOT_View + ComboSearchBox.SelectedValue.ToString)
        End If
        'clearTextBox() IT MIGHT BE NEEDED
    End Sub

    Sub OpenNewPage(ByVal MyUpdatePanel As UpdatePanel, ByVal TheWebPage As String)
        Dim docGuid As String = Guid.NewGuid().ToString()
        Dim sb As StringBuilder = New StringBuilder("")
        Dim strRoot As String
        strRoot = Request.Url.GetLeftPart(UriPartial.Authority)
        sb.Append("window.open('" & TheWebPage & "');")
        ScriptManager.RegisterClientScriptBlock(MyUpdatePanel, MyUpdatePanel.GetType(), "NewClientScript", sb.ToString(), True)
    End Sub

    'Protected Sub MakeShiftX_Click(sender As Object, ByVal e As EventArgs) Handles MakeShiftX.Click
    '    ClearTextBox()
    'End Sub

    Protected Sub ClearTextBox()
        BuildDataSet()
        ComboSearchBox.SelectedIndex = -1
        ComboSearchBox.Text = String.Empty
    End Sub

    Protected Sub BuildDataSet()
        Dim connect As New Data.SqlClient.SqlConnection
        connect.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim command As Data.SqlClient.SqlCommand
        Dim reader As Data.SqlClient.SqlDataReader

        If Preference.Checked = False Then
            'FOR ALISA LOOK UPS
            '                                        SELECT DISTINCT FileName, COALESCE (NULLIF (AKA, ''), Name) AS AKA_W_NULLS, ExpDate FROM T_SATI_SDS WHERE (ExpDate >= GETDATE() OR ExpDate IS NULL) ORDER BY AKA_W_NULLS
            command = New Data.SqlClient.SqlCommand("SELECT DISTINCT FileName, COALESCE (NULLIF (AKA, ''), Name) AS AKA_W_NULLS, ExpDate FROM T_SATI_SDS WHERE (ExpDate >= GETDATE() OR ExpDate IS NULL) ORDER BY AKA_W_NULLS", connect)
            connect.Open()
            reader = command.ExecuteReader()

            ComboSearchBox.DataSource = reader
            ComboSearchBox.DataTextField = "AKA_W_NULLS"
            ComboSearchBox.DataValueField = "FileName"
            ComboSearchBox.DataBind()

            connect.Close()
            reader.Close()

            ComboSearchBox.DropDownStyle = ComboBoxStyle.DropDownList
            ComboSearchBox.Items.Insert(0, "Search By Alias")
            ComboSearchBox.Items(0).Value = Nothing
        End If

        If Preference.Checked = True Then
            'FOR NAME LOOK UPS
            '                                                                         SELECT DISTINCT Name, FileName FROM T_SATI_SDS WHERE (ExpDate &gt;= GETDATE() OR ExpDate IS NULL) OR (FileName LIKE N' Copy ') ORDER BY Name
            command = New Data.SqlClient.SqlCommand("SELECT DISTINCT Name, FileName FROM T_SATI_SDS WHERE (ExpDate >= GETDATE() OR ExpDate IS NULL) ORDER BY Name", connect)
            connect.Open()
            reader = command.ExecuteReader()

            ComboSearchBox.DataSource = reader
            ComboSearchBox.DataTextField = "Name"
            ComboSearchBox.DataValueField = "FileName"
            ComboSearchBox.DataBind()

            connect.Close()
            reader.Close()

            ComboSearchBox.DropDownStyle = ComboBoxStyle.DropDownList
            ComboSearchBox.Items.Insert(0, "Search By Name")
            ComboSearchBox.Items(0).Value = Nothing
        End If

    End Sub
    Protected Sub Preference_CheckedChanged(sender As Object, e As EventArgs) Handles Preference.CheckedChanged
        If Preference.Checked = True Then
            If ComboSearchBox.SelectedValue = "Search By Name" Or ComboSearchBox.SelectedValue = "Search By Alias" Then
                BuildDataSet()
            Else
                ClearTextBox()
            End If
        End If
        If Preference.Checked = False Then
            If ComboSearchBox.SelectedValue = "Search By Name" Or ComboSearchBox.SelectedValue = "Search By Alias" Then
                BuildDataSet()
            Else
                ClearTextBox()
            End If
        End If
    End Sub
End Class