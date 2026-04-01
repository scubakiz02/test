Imports System.Data
Imports System.IO

Partial Class DBMaintenance_EditRoles
    Inherits System.Web.UI.Page

    Dim SatiCode As New Class1
    Private Shared schema As Data.DataTable
    Dim elementCellCols = New Dictionary(Of String, Integer) From {{"Li", -1}, {"Na", -1}, {"Mg", -1}, {"Al", -1}, {"K", -1}, {"Ca", -1}, {"Ti", -1}, {"V", -1}, {"Cr", -1}, {"Mn", -1}, {"Fe", -1}, {"Co", -1}, {"Ni", -1}, {"Zn", -1}, {"Cu", -1}, {"Sr", -1}, {"Mo", -1}, {"Ba", -1}, {"W", -1}, {"Pb", -1}}
    Dim ROOT As String ' = Session("SDS")
    Dim ROOT_View As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        MenuAuthenication.CheckGroupAuthenication("EditRoles", Server)
        Me.Page.Form.Enctype = "multipart/form-data"
        ROOT = Session("SDS")
        ROOT_View = Session("SDS_View")

        Try
            If Not Page.IsPostBack Then 'on initial rendering of page
                Me.LiveFormView.DataBind()
                schema = SatiCode.GetSchema("T_Metals_MDL")
                differentiateLabels(LiveFormView)
            End If
        Catch ex As Exception
            'run js alert() function
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert1", "alert('Error: Refresh page and try again');", True)
        End Try
    End Sub

    Sub differentiateLabels(FormView As FormView)
        'differentiate labels with .01 value
        If FormView.CurrentMode = FormViewMode.ReadOnly And Not schema Is Nothing Then
            For Each column As DataColumn In schema.Columns
                If column.DataType.Name = "Double" Then
                    Dim Label As Label = CType(FormView.FindControl(column.Caption & "Label"), Label)

                    If Not Label Is Nothing Then
                        If Label.Text = "0.01" Then
                            Label.ForeColor = System.Drawing.Color.LightGray
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Protected Sub UploadFile(sender As Object, e As EventArgs)

        Dim fileName As String = IO.Path.GetFileName(Uploader.FileName)
        Dim uploadDirectory As String = Server.MapPath("~/Uploads/")
        Dim TestFile As String = fileName
        Dim match As Match = Regex.Match(TestFile, "[% < > : / \ | ? *]")

        'MAKES SURE THAT ONLY CSV FILES CAN BE IN THE SYSTEM
        If fileName = "" Then
            ErrorMessage.Text = "CHOOSE A FILE BEFORE UPLOADING"
            Exit Sub
        ElseIf Not fileName.EndsWith(".csv") Then
            ErrorMessage.Text = "FILE HAS TO BE A CSV"
            Exit Sub
        Else
            ErrorMessage.Text = ""
        End If

        'REMOVES CHARACTERS THAT ARENT ALLOWED IN FILE NAMES
        Do While match.Success
            Dim key As String = match.Value
            TestFile = TestFile.Replace(key, String.Empty)
            match = match.NextMatch()
        Loop
        fileName = TestFile

        ' Ensure the upload directory exists
        If Not Directory.Exists(uploadDirectory) Then
            Directory.CreateDirectory(uploadDirectory)
        End If

        Dim CompletePath As String = Path.Combine(uploadDirectory, fileName)
        Uploader.PostedFile.SaveAs(CompletePath)

        'read from file and do programmatic entries
        Try
            Using MyReader As New Microsoft.VisualBasic.
            FileIO.TextFieldParser(CompletePath)
                MyReader.TextFieldType = FileIO.FieldType.Delimited
                MyReader.SetDelimiters(",")

                Dim currentRow As String()

                While Not MyReader.EndOfData
                    currentRow = MyReader.ReadFields()

                    If (currentRow(0) = "Element") Then
                        Dim count As Integer = 0

                        For Each currentField In currentRow
                            If elementCellCols.ContainsKey(currentField) Then
                                elementCellCols(currentField) = count
                            End If
                            count = count + 1
                        Next
                    End If

                    If (currentRow(2).Contains("at/cm")) Then
                        Dim currentField As String
                        For Each currentField In currentRow
                            For Each Element As String In elementCellCols.Keys
                                Dim Tbx As TextBox = CType(LiveFormView.FindControl(Element & "TextBox"), TextBox)

                                If Not Tbx Is Nothing Then
                                    Dim value As String = currentRow(elementCellCols(Element))
                                    Tbx.Text = E10(value)
                                End If
                            Next

                        Next
                    End If
                End While
            End Using
        Catch ex As Exception
            'run js alert() function
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert2", "alert('Error reading file. Try browsing again');", True)
        End Try
    End Sub


    Protected Sub RB_StatusChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim selectedRadio As RadioButton = CType(sender, RadioButton)

        If selectedRadio.Text = "Archived" Then
            Me.CurrPanel.Visible = False
            Me.ArchivedPanel.Visible = True
            Me.ArchivedPanel.DataBind()
        Else
            Me.CurrPanel.Visible = True
            Me.ArchivedPanel.Visible = False
            Me.CurrPanel.DataBind()
        End If
    End Sub

    Function E10(value As String) As String
        If String.IsNullOrEmpty(value) Then
            Return value
        End If

        Dim delimitPrep = value.Replace("E+", ";")
        Dim splitArr() As String = delimitPrep.Split(";")
        Dim float As Double = Double.Parse(splitArr(0))
        Dim EFormat As Double = Double.Parse(splitArr(1))
        Return (float / (10 ^ (10 - EFormat))).ToString()
    End Function

    Sub UploadVisibility(Visibility As Boolean)
        Uploader.Visible = Visibility
        CreateButton.Visible = Visibility
        ErrorMessage.Text = ""
    End Sub

    Protected Sub UpdateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

    Protected Sub EditCancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

    Protected Sub NewButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        UploadVisibility(True)
    End Sub

    Protected Sub InsertCancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        UploadVisibility(False)
    End Sub

    Protected Sub EditButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

    Protected Sub DeleteButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

    Protected Sub RestoreButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        SatiCode.DeleteMyAltsRecords(Me.SqlDataSourceMDL2.DeleteCommand)

        Dim prevUpdateCommand = SqlDataSourceMDL2.UpdateCommand
        SqlDataSourceMDL2.UpdateCommand = "UPDATE T_Metals_MDL SET ExpireDate = NULL WHERE [Key]=" & CType(FormView2.FindControl("KeyLabel"), Label).Text
        SqlDataSourceMDL2.Update()
        SqlDataSourceMDL2.UpdateCommand = prevUpdateCommand

        CurrRB.Checked = True
        ArchivedRB.Checked = False
        RB_StatusChanged(CurrRB, EventArgs.Empty)
    End Sub

    Protected Sub InsertButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        UploadVisibility(False)
        For Each column As DataColumn In schema.Columns
            Dim Tbx As TextBox = CType(LiveFormView.FindControl(column.Caption & "TextBox"), TextBox)

            If Not Tbx Is Nothing Then
                'insert time into EnterDate textbox
                If Tbx.ID = "EnterDateTextBox" Then
                    Tbx.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                End If

                If column.DataType.Name = "Double" Then
                    'if blank, insert .01
                    If String.IsNullOrEmpty(Tbx.Text) Then
                        Tbx.Text = 0.01
                    End If

                    'check tbx for invalid data
                    Dim floatValue As Single

                    If Not Single.TryParse(Tbx.Text, floatValue) Then
                        'TO DO: signal to user where invalid data exists
                        Return
                    End If
                End If

            End If
        Next

        SatiCode.DeleteMyAltsRecords(Me.SqlDataSourceMDL2.DeleteCommand)
    End Sub

    Protected Sub LiveFormView_ModeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles LiveFormView.DataBound
        If LiveFormView.CurrentMode = FormViewMode.Insert Then
            Dim EnterDateTextBox As TextBox = CType(LiveFormView.FindControl("EnterDateTextBox"), TextBox)
            EnterDateTextBox.Text = Date.Today()

            Dim MDL_UserTextBox As TextBox = CType(LiveFormView.FindControl("MDL_UserTextBox"), TextBox)
            MDL_UserTextBox.Text = User.Identity.Name
        End If
        differentiateLabels(LiveFormView)
    End Sub

    Protected Sub ArchivedFormView_FormChanged(ByVal sender As Object, ByVal e As EventArgs) Handles FormView2.DataBound
        differentiateLabels(FormView2)
    End Sub
End Class
