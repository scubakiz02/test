
Partial Class DBMaintenance_SATI_SDS
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1

    Dim ROOT As String ' = Session("SDS")
    Dim ROOT_View As String
    'Dim ROOT As String = "~/DBMaintenance/"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        MenuAuthenication.CheckGroupAuthenication("Office", Server)


        Me.Page.Form.Enctype = "multipart/form-data"
        ROOT = Session("SDS")
        ROOT_View = Session("SDS_View")
        If Not Me.IsPostBack Then
            Me.FooterErrorPanel.Visible = False
            Me.FooterRestorePanel.Visible = False
            Me.FooterErrorPanel2.Visible = False

            Me.TempPanel.Visible = False
            Me.Temp1.Visible = False
            Me.FooterRestorePanel.Visible = False

            Me.CurrentPanel.Visible = False
            Me.RetiredPanel.Visible = False
            Me.BothPanel.Visible = False

            If FooterErrorPanel.Visible = True Then
                FooterErrorPanel.Visible = False
                SATI_SDS_Table.ShowFooter = True
            End If

            If FooterErrorPanel2.Visible = True Then
                FooterErrorPanel2.Visible = False
                SATI_SDS_Retire.ShowFooter = True
            End If
        End If
    End Sub

    Protected Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs) Handles SearchTextBox.TextChanged
        If FooterErrorPanel.Visible = True Then
            FooterErrorPanel.Visible = False
            SATI_SDS_Table.ShowFooter = True
        End If

        If FooterErrorPanel2.Visible = True Then
            FooterErrorPanel2.Visible = False
            SATI_SDS_Retire.ShowFooter = True
        End If

        If Me.BothPanel.Visible = True Then
            Me.BothPanel.Visible = False
        End If

        If SearchTextBox.Text = "*" Then
            Me.BothPanel.Visible = True

            CurrentRadio.Checked = False
            RetiredRadio.Checked = False
            RebindAsterisk()
        Else
            If CurrentRadio.Checked = True Then
                RebindCurrent()
                CurrentPanel.Visible = True
                SATI_SDS_Table.ShowFooter = True
                RetiredPanel.Visible = False
                BothPanel.Visible = False
            ElseIf RetiredRadio.Checked = True Then
                RebindRetired()
                CurrentPanel.Visible = False
                RetiredPanel.Visible = True
                SATI_SDS_Retire.ShowFooter = True
                BothPanel.Visible = False
            Else
                CurrentPanel.Visible = False
                RetiredPanel.Visible = False
                SATI_SDS_Retire.ShowFooter = False
                BothPanel.Visible = False
            End If
        End If

        If Not ErrorMessage.Text = "------------------------------------------------------------------------------------" Then
            ErrorMessage.Text = "------------------------------------------------------------------------------------"
            Me.ErrorMessage.ForeColor = Drawing.Color.Black
            Response.Redirect(Request.RawUrl)
        End If

        ErrorMessage.Text = "------------------------------------------------------------------------------------"
        Me.ErrorMessage.ForeColor = Drawing.Color.Black
    End Sub

    Protected Sub CurrentRadio_CheckedChanged(sender As Object, e As EventArgs) Handles CurrentRadio.CheckedChanged
        If FooterErrorPanel.Visible = True Then
            FooterErrorPanel.Visible = False
            SATI_SDS_Table.ShowFooter = True
        End If

        If FooterErrorPanel2.Visible = True Then
            FooterErrorPanel2.Visible = False
            SATI_SDS_Retire.ShowFooter = True
        End If

        If SearchTextBox.Text = "*" Then
            BothPanel.Visible = True

            CurrentPanel.Visible = False
            RetiredPanel.Visible = False
            RetiredRadio.Checked = False
            RebindAsterisk()
        Else
            If Me.BothPanel.Visible = True Then
                Me.BothPanel.Visible = False
            End If

            BothPanel.Visible = False

            CurrentPanel.Visible = True
            SATI_SDS_Table.ShowFooter = True
            RetiredPanel.Visible = False
            RebindCurrent()
        End If

        If Not ErrorMessage.Text = "------------------------------------------------------------------------------------" Then
            ErrorMessage.Text = "------------------------------------------------------------------------------------"
            Me.ErrorMessage.ForeColor = Drawing.Color.Black
            Response.Redirect(Request.RawUrl)
        End If

        ErrorMessage.Text = "------------------------------------------------------------------------------------"
        Me.ErrorMessage.ForeColor = Drawing.Color.Black

    End Sub
    Sub RebindCurrent()
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim query As String = "SELECT [Key], Name, FileName, AKA, Notes, Op, ExpDate, ExpOp FROM T_SATI_SDS"
        Dim where As String = " WHERE (Name = @Name) AND (ExpDate > GETDATE() OR ExpDate IS NULL) AND (FileName IS NOT NULL) OR (ExpDate > GETDATE() OR ExpDate IS NULL) AND (AKA = @AKA) AND (FileName IS NOT NULL) OR (ExpDate > GETDATE() OR ExpDate IS NULL) AND (SUBSTRING(Name, 1, 1) = @Name) AND (FileName IS NOT NULL) OR (ExpDate > GETDATE() OR ExpDate IS NULL) AND (SUBSTRING(AKA, 1, 1) = @AKA) AND (FileName IS NOT NULL)"
        Dim order As String = " ORDER BY Name, FileName, ExpDate"

        If CurrentRadio.Checked Then
            where = " WHERE (Name = '" & Me.SearchTextBox.Text & "') And (ExpDate > '" & Date.Today & "' Or ExpDate Is NULL) AND (FileName IS NOT NULL) Or (ExpDate > '" & Date.Today & "' Or ExpDate Is NULL) And (AKA = '" & Me.SearchTextBox.Text & "') AND (FileName IS NOT NULL) Or (ExpDate > '" & Date.Today & "' Or ExpDate Is NULL) And (SUBSTRING(Name, 1, 1) = '" & Me.SearchTextBox.Text & "') AND (FileName IS NOT NULL) Or (ExpDate > '" & Date.Today & "' Or ExpDate Is NULL) And (SUBSTRING(AKA, 1, 1) = '" & Me.SearchTextBox.Text & "') AND (FileName IS NOT NULL)"

            RetiredRadio.Checked = False
        End If

        Me.SqlDataSourceSATI_SDS.SelectCommand = query & where & order
        Me.SATI_SDS_Table.DataBind()
    End Sub

    Protected Sub RetiredRadio_CheckedChanged(sender As Object, e As EventArgs) Handles RetiredRadio.CheckedChanged
        If FooterErrorPanel.Visible = True Then
            FooterErrorPanel.Visible = False
            SATI_SDS_Table.ShowFooter = True
        End If

        If FooterErrorPanel2.Visible = True Then
            FooterErrorPanel2.Visible = False
            SATI_SDS_Retire.ShowFooter = True
        End If

        If SearchTextBox.Text = "*" Then
            BothPanel.Visible = True

            CurrentPanel.Visible = False
            RetiredPanel.Visible = False
            CurrentRadio.Checked = False
            RebindAsterisk()
        Else
            If Me.BothPanel.Visible = True Then
                Me.BothPanel.Visible = False
            End If

            BothPanel.Visible = False

            CurrentPanel.Visible = False
            RetiredPanel.Visible = True
            SATI_SDS_Retire.ShowFooter = True
            RebindRetired()
        End If

        If Not ErrorMessage.Text = "------------------------------------------------------------------------------------" Then
            ErrorMessage.Text = "------------------------------------------------------------------------------------"
            Me.ErrorMessage.ForeColor = Drawing.Color.Black
            Response.Redirect(Request.RawUrl)
        End If

        ErrorMessage.Text = "------------------------------------------------------------------------------------"
        Me.ErrorMessage.ForeColor = Drawing.Color.Black

    End Sub
    Sub RebindRetired()
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim query As String = "Select [Key], Name, FileName, AKA, Notes, Op, ExpDate, ExpOp FROM T_SATI_SDS"
        Dim where As String = " WHERE (Name = @Name) AND (ExpDate <= GETDATE()) OR (ExpDate <= GETDATE()) AND (AKA = @AKA) OR (ExpDate <= GETDATE()) AND (SUBSTRING(Name, 1, 1) = @Name) OR (ExpDate <= GETDATE()) AND (SUBSTRING(AKA, 1, 1) = @AKA)"
        Dim order As String = " ORDER BY Name, ExpDate"

        If RetiredRadio.Checked Then
            where = " WHERE (Name = '" & Me.SearchTextBox.Text & "') AND (ExpDate <= '" & Date.Today & "') OR (AKA = '" & Me.SearchTextBox.Text & "') AND (ExpDate <= '" & Date.Today & "') OR (SUBSTRING(Name, 1, 1) = '" & Me.SearchTextBox.Text & "') AND (ExpDate <= '" & Date.Today & "') OR (SUBSTRING(AKA, 1, 1) = '" & Me.SearchTextBox.Text & "') AND (ExpDate <= '" & Date.Today & "')"

            CurrentRadio.Checked = False
        End If

        Me.SqlDataSourceRetired.SelectCommand = query & where & order
        Me.SATI_SDS_Retire.DataBind()
    End Sub

    Sub RebindAsterisk()
        If SearchTextBox.Text = "*" And CurrentRadio.Checked = False And RetiredRadio.Checked = False Then
            Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
            Dim query As String = "Select [Key], Name, FileName, AKA, Notes, Op, ExpDate, ExpOp FROM T_SATI_SDS"
            Dim where As String = " WHERE (FileName Is Not NULL) Or (FileName Is Not NULL) Or (FileName Is Not NULL) Or (FileName Is Not NULL)"
            Dim order As String = " ORDER BY Name, FileName, ExpDate"

            Me.SqlDataSourceBoth.SelectCommand = query & where & order
            Me.SATI_SDS_Both.DataBind()
        Else
            If CurrentRadio.Checked = True Then
                Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
                Dim query As String = "SELECT [Key], Name, FileName, AKA, Notes, Op, ExpDate, ExpOp FROM T_SATI_SDS"
                Dim where As String = " WHERE (ExpDate > GETDATE() OR ExpDate IS NULL) AND (FileName IS NOT NULL) OR (ExpDate > GETDATE() OR ExpDate IS NULL) AND (FileName IS NOT NULL) OR (ExpDate > GETDATE() OR ExpDate IS NULL) AND (FileName IS NOT NULL) OR (ExpDate > GETDATE() OR ExpDate IS NULL) AND (FileName IS NOT NULL)"
                Dim order As String = " ORDER BY Name, FileName, ExpDate"

                If CurrentRadio.Checked Then
                    where = " WHERE (ExpDate > '" & Date.Today & "' Or ExpDate Is NULL) AND (FileName IS NOT NULL) Or (ExpDate > '" & Date.Today & "' Or ExpDate Is NULL) And (FileName IS NOT NULL) Or (ExpDate > '" & Date.Today & "' Or ExpDate Is NULL) AND (FileName IS NOT NULL) Or (ExpDate > '" & Date.Today & "' Or ExpDate Is NULL) And (FileName IS NOT NULL)"

                    RetiredRadio.Checked = False
                End If

                Me.SqlDataSourceBoth.SelectCommand = query & where & order
                Me.SATI_SDS_Both.DataBind()

            ElseIf RetiredRadio.Checked = True Then
                Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
                Dim query As String = "Select [Key], Name, FileName, AKA, Notes, Op, ExpDate, ExpOp FROM T_SATI_SDS"
                Dim where As String = "WHERE (ExpDate <= GETDATE()) OR (ExpDate <= GETDATE()) OR (ExpDate <= GETDATE()) OR (ExpDate <= GETDATE())"
                Dim order As String = " ORDER BY Name, ExpDate"

                If RetiredRadio.Checked Then
                    where = " WHERE (ExpDate <= '" & Date.Today & "') OR (ExpDate <= '" & Date.Today & "') OR (ExpDate <= '" & Date.Today & "')"

                    CurrentRadio.Checked = False
                End If

                Me.SqlDataSourceBoth.SelectCommand = query & where & order
                Me.SATI_SDS_Both.DataBind()
            End If
        End If
    End Sub

    Protected Sub UploadFile(sender As Object, e As EventArgs)
        'THIS SAVES FILE AND THEN ADDS DATA TO DATABASE
        SetUpLoad()
    End Sub

    Sub SetUpLoad()
        Dim fileName As String = IO.Path.GetFileName(Uploader.FileName)

        'MAKES SURE THAT ONLY PDF FILES CAN BE IN THE SYSTEM
        If Not fileName.EndsWith(".pdf") Then
            Me.ErrorMessage.Text = "----------------------- SDS FILE HAS TO BE A PDF -----------------------"
            Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
            Exit Sub
        End If

        'REMOVES CHARACTERS THAT ARENT ALLOWED IN FILE NAMES
        Dim TestFile As String = fileName
        Dim match As Match = Regex.Match(TestFile, "[% < > : / \ | ? *]")

        Do While match.Success
            Dim key As String = match.Value
            TestFile = TestFile.Replace(key, String.Empty)
            match = match.NextMatch()
        Loop
        fileName = TestFile

        'Test inputs before use
        Dim AddName As String = CType(Me.NameTextBox, TextBox).Text
        Dim AddAKA As String = CType(Me.AKATextBox, TextBox).Text
        Dim AddExp As String = CType(Me.ExpDatetextBox, TextBox).Text
        Dim Addfile As String = IO.Path.GetFileName(Uploader.PostedFile.FileName).ToString
        Dim AddNote As String = CType(Me.NotesTextBox, TextBox).Text
        Dim AddOp As String = User.Identity.Name.ToString

        'Error Checks
        Dim errorO As Boolean = True
        Dim error0 As Boolean = True
        Dim errorPoint5 As Boolean = True
        Dim error1 As Boolean = True
        Dim successMessage As Boolean = False

        'placeholders
        Dim countHolder As Integer

        If Uploader.HasFile Then
            Dim pathCheck As String = ROOT + fileName

            If Uploader.FileName.Length >= 39 Then
                errorO = False
                Me.ErrorMessage.Text = "-------------- SDS FILENAME IS TOO LONG (MAX 35) --------------"
                Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
            Else
                If System.IO.File.Exists(pathCheck) Then
                    Dim counter As Integer = 2
                    Dim TempFileName As String = ""
                    While System.IO.File.Exists(pathCheck)
                        Dim dash As String = " - "
                        Dim copy As String = "Copy ("
                        Dim parent As String = ")"
                        TempFileName = fileName.Insert(fileName.IndexOf(".pdf"), dash)
                        TempFileName = TempFileName.Insert(TempFileName.IndexOf(".pdf"), copy)
                        TempFileName = TempFileName.Insert(TempFileName.IndexOf(".pdf"), counter.ToString())
                        TempFileName = TempFileName.Insert(TempFileName.IndexOf(".pdf"), parent)

                        pathCheck = ROOT + TempFileName
                        counter += 1
                    End While

                    errorPoint5 = False
                    countHolder = counter - 1

                    fileName = TempFileName
                    Me.ErrorMessage.Text = "--- File Exists. File saved as " & fileName & " ---"
                    Me.ErrorMessage.ForeColor = Drawing.Color.Yellow

                    If Not fileName = IO.Path.GetFileName(Uploader.PostedFile.FileName).ToString Then
                        Me.ViewState.Add("NewFileName", fileName)
                    End If

                Else
                    successMessage = True
                    Me.ErrorMessage.Text = "----------------- SDS FILE SAVED SUCCESSFULLY -----------------"
                    Me.ErrorMessage.ForeColor = Drawing.Color.DarkGreen
                End If
            End If
        Else
            error0 = False
            If Addfile = String.Empty Then
                Me.ErrorMessage.Text = "--------------- SDS FILE CAN-NOT BE LEFT EMPTY ---------------"
                Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
            End If
        End If

        If AddName = String.Empty Then
            error1 = False
            Me.ErrorMessage.Text = "-------------- SDS NAME CAN-NOT BE LEFT EMPTY --------------"
            Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
        End If


        Dim BoolAKA As Boolean
        Dim BoolExp As Boolean
        Dim BoolNot As Boolean
        Dim BoolNum As Integer = 0

        If AddAKA = String.Empty Then
            BoolAKA = True
            BoolNum += 1
        End If
        If AddExp = String.Empty Then
            BoolExp = True
            BoolNum += 1
        End If
        If AddNote = String.Empty Then
            BoolNot = True
            BoolNum += 1
        End If


        'SUCCESS MESSAGE IS MOST IMPORTANT
        If successMessage = True Then
            'SUCCESS MESSAGE AND ANY FYI's
            If BoolNum > 1 Then
                Me.ErrorMessage.Text = "-- SDS File saved successfully. FYI: multiple fields were not set --"
                Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
            Else
                'SINGLE FYI SIGNS
                If BoolAKA Then
                    Me.ErrorMessage.Text = "------- SDS File saved successfully -- ALIAS was not set -------"
                    Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
                End If
                If BoolExp Then
                    Me.ErrorMessage.Text = "---SDS File saved successfully -- EXPIRATION was not set ---"
                    Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
                End If
                If BoolNot Then
                    Me.ErrorMessage.Text = "------- SDS File saved successfully -- NOTES were not set -------"
                    Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
                End If
            End If

        Else
            'IF SUCCESSMESSAGE WAS NOT TRIGGERED
            If errorO And error0 And errorPoint5 And error1 Then
                'IF BOTH TWO CHECK FOR FYI's

                If BoolNum > 2 Then
                    'IF MORE THAN 3 FYI warnings appear
                    Me.ErrorMessage.Text = "----- FYI: Last entered SDS has multiple optional field not set -----"
                    Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
                Else
                    'DOUBLE FYI SIGNS
                    If BoolNum = 2 Then
                        If BoolAKA And BoolExp Then
                            Me.ErrorMessage.Text = "-- FYI: Last entered SDS has no ALIAS or EXPIRATION set --"
                            Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
                        End If
                        If BoolAKA And BoolNot Then
                            Me.ErrorMessage.Text = "------- FYI: Last entered SDS has no ALIAS or NOTES set -------"
                            Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
                        End If
                        If BoolExp And BoolNot Then
                            Me.ErrorMessage.Text = "-- FYI: Last entered SDS has no EXPIRATION or NOTES set --"
                            Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
                        End If
                    End If

                    'SINGLE FYI SIGNS
                    If BoolNum = 1 Then
                        If BoolAKA Then
                            Me.ErrorMessage.Text = "------------ FYI: Last entered SDS has no ALIAS set ------------"
                            Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
                        End If
                        If AddExp = String.Empty Then
                            Me.ErrorMessage.Text = "--------- FYI: Last entered SDS has no EXPIRATION set ----------"
                            Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
                        End If
                        If AddNote = String.Empty Then
                            Me.ErrorMessage.Text = "------------ FYI: Last entered SDS has no NOTES set -------------"
                            Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
                        End If
                    End If
                End If
            Else

                'THIS ENSURE THE ERROR FOR MISSING FILES SHOWS FIRST AND ONLY
                If error0 = False And errorPoint5 = True And error1 = True Then
                    If BoolNum > 1 Then
                        Me.ErrorMessage.Text = "-- SDS FILE cannot be empty -- multiple fields were not set --"
                        Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
                    Else
                        'SINGLE FYI SIGNS
                        If BoolAKA Then
                            Me.ErrorMessage.Text = "------- SDS FILE cannot be empty -- ALIAS would not be set -------"
                            Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
                        End If
                        If BoolExp Then
                            Me.ErrorMessage.Text = "--- SDS FILE cannot be empty -- EXPIRATION would not be set ---"
                            Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
                        End If
                        If BoolNot Then
                            Me.ErrorMessage.Text = "------- SDS FILE cannot be empty -- NOTES would not be set -------"
                            Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
                        End If
                    End If
                End If

                'FOR FILE UPDATE AND FYI's
                If error0 = True And errorPoint5 = False And error1 = True Then
                    If BoolNum > 1 Then
                        Me.ErrorMessage.Text = "-- File saved with -" & countHolder & " at the end -- multiple fields were not set --"
                        Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
                    Else
                        'SINGLE FYI SIGNS
                        If BoolAKA Then
                            Me.ErrorMessage.Text = "------ File saved with -" & countHolder & " at the end -- ALIAS would not be set ------"
                            Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
                        End If
                        If BoolExp Then
                            Me.ErrorMessage.Text = "-- File saved with -" & countHolder & " at the end -- EXPIRATION would not be set --"
                            Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
                        End If
                        If BoolNot Then
                            Me.ErrorMessage.Text = "------ File saved with -" & countHolder & " at the end -- NOTES would not be set ------"
                            Me.ErrorMessage.ForeColor = Drawing.Color.Yellow
                        End If
                    End If
                End If

                'FOR NAME EMPTY AND FYI's
                If error0 = True And errorPoint5 = True And error1 = False Then
                    If BoolNum > 1 Then
                        Me.ErrorMessage.Text = "-- SDS NAME cannot be empty -- multiple fields were not set --"
                        Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
                    Else
                        'SINGLE FYI SIGNS
                        If BoolAKA Then
                            Me.ErrorMessage.Text = "------- SDS NAME cannot be empty -- ALIAS wwould not be set -------"
                            Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
                        End If
                        If BoolExp Then
                            Me.ErrorMessage.Text = "--- SDS NAME cannot be empty -- EXPIRATION would not be set ---"
                            Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
                        End If
                        If BoolNot Then
                            Me.ErrorMessage.Text = "------- SDS NAME cannot be empty -- NOTES would not be set -------"
                            Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
                        End If
                    End If
                End If
            End If
        End If


        If RestoreUploaded.HasFile = True Then
            If RestoreUploaded.FileName.Length >= 39 Then
                error0 = False
                Me.FooterErrorPanel2.Visible = True
                Me.FooterErrorMessage2.Text = "ERROR: The Uploaded File Name is too long, Consider shortening. Max Length is 35"
                Me.FooterErrorMessage2.ForeColor = Drawing.Color.DarkRed
                Me.FooterErrorMessage2.Font.Size = 15
                Me.FooterErrorMessage2.Font.Bold = True
            End If
        End If

        'CHECK FOR ERRORS THEN SAVE EVERYTHING
        If errorO = True And error0 = True And error1 = True Then
            'SAVE NEW FILE
            Uploader.PostedFile.SaveAs(ROOT + fileName)
            'Uploader.PostedFile.SaveAs((Server.MapPath(ROOT) + fileName))

            'ADDS NEW ENTITY
            SqlDataSourceSATI_SDS.Insert()

            'CLEARS INPUT TEXTBOXES
            CType(Me.NameTextBox, TextBox).Text = String.Empty
            CType(Me.AKATextBox, TextBox).Text = String.Empty
            CType(Me.ExpDatetextBox, TextBox).Text = String.Empty
            Me.Uploader.Attributes.Clear()
            CType(Me.NotesTextBox, TextBox).Text = String.Empty

        Else
            If error0 = False And error1 = False Then
                Me.ErrorMessage.Text = "------ SDS FILE AND NAME CAN-NOT BE LEFT EMPTY ------"
                Me.ErrorMessage.ForeColor = Drawing.Color.DarkRed
            End If
        End If

        SATI_SDS_Table.DataBind()
        SATI_SDS_Retire.DataBind()
        SATI_SDS_Both.DataBind()

    End Sub

    Protected Sub SqlDataSourceSATI_SDS_Inserting(ByVal sender As Object, ByVal e As SqlDataSourceCommandEventArgs) Handles SqlDataSourceSATI_SDS.Inserting
        Dim AddName As String = CType(Me.NameTextBox, TextBox).Text
        Dim AddAKA As String = CType(Me.AKATextBox, TextBox).Text
        Dim AddExp As String = CType(Me.ExpDatetextBox, TextBox).Text
        Dim Addfile As String = IO.Path.GetFileName(Uploader.PostedFile.FileName).ToString
        Dim AddNote As String = CType(Me.NotesTextBox, TextBox).Text
        Dim AddOp As String = User.Identity.Name.ToString

        e.Command.Parameters("@Name").Value = AddName
        e.Command.Parameters("@AKA").Value = AddAKA

        Dim ED As Date
        If String.IsNullOrEmpty(AddExp) Then
            e.Command.Parameters("@ExpDate").Value = DBNull.Value
        Else
            Date.TryParse(AddExp, ED)
            e.Command.Parameters("@ExpDate").Value = ED
        End If

        If Me.ViewState("NewFileName") = "" Then ' Addfile = Me.ViewState("NewFileName") Then
            e.Command.Parameters("@FileName").Value = Addfile

        Else
            e.Command.Parameters("@FileName").Value = Me.ViewState("NewFileName")
        End If

        e.Command.Parameters("@Notes").Value = AddNote
        e.Command.Parameters("@OP").Value = AddOp

        ViewState.Clear()
    End Sub

    Protected Sub CancelUpload(sender As Object, e As EventArgs)
        CType(Me.NameTextBox, TextBox).Text = String.Empty
        CType(Me.AKATextBox, TextBox).Text = String.Empty
        CType(Me.ExpDatetextBox, TextBox).Text = String.Empty
        Me.Uploader.Attributes.Clear()
        CType(Me.NotesTextBox, TextBox).Text = String.Empty

        Response.Redirect(Request.RawUrl)
    End Sub

    Protected Sub DownloadFile(sender As Object, e As EventArgs)
        Dim filePath As String
        filePath = ROOT_View + CType(sender, LinkButton).CommandArgument
        OpenNewPage(Me.UpdatePane, filePath)
    End Sub

    Sub OpenNewPage(ByVal MyUpdatePanel As UpdatePanel, ByVal TheWebPage As String)

        Dim docGuid As String = Guid.NewGuid().ToString()
        Dim sb As StringBuilder = New StringBuilder("")
        Dim strRoot As String
        strRoot = Request.Url.GetLeftPart(UriPartial.Authority)
        sb.Append("window.open('" & TheWebPage & "');")
        ScriptManager.RegisterClientScriptBlock(MyUpdatePanel, MyUpdatePanel.GetType(), "NewClientScript", sb.ToString(), True)
    End Sub

    Protected Sub SATI_SDS_Table_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles SATI_SDS_Table.RowCommand
        If e.CommandName = "Edit" Then
            Dim index As Integer = e.CommandArgument
            Dim row As GridViewRow = SATI_SDS_Table.Rows(index)
            Dim keyEdit As Integer = SATI_SDS_Table.DataKeys(row.RowIndex)(0).ToString()

            Me.ViewState.Add("KeyEdit", keyEdit)
        End If
    End Sub

    Sub SATI_SDS_Table_RowEditing(sender As Object, de As GridViewEditEventArgs) Handles SATI_SDS_Table.RowEditing
        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT Op FROM T_SATI_SDS WHERE [Key] = " & Me.ViewState("KeyEdit") & ""

        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
        connect.ConnectionString = conString
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand
        command.CommandText = camString
        command.Connection = connect

        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If checker(0) = User.Identity.Name.ToString Then 'ADD GROUP IF NEEDED
                Me.SATI_SDS_Table.ShowFooter = True
                Me.FooterErrorPanel.Visible = False
                Me.ViewState.Add("FieldsT", True)
                Exit While
            Else
                de.Cancel = True
                Me.SATI_SDS_Table.ShowFooter = False
                Me.FooterErrorPanel.Visible = True
                Me.FooterErrorMessage.Text = "Permission Denied: You may only EDIT your own."
                Me.FooterErrorMessage.ForeColor = Drawing.Color.DarkRed
                Me.FooterErrorMessage.Font.Size = 15
                Me.FooterErrorMessage.Font.Bold = True
            End If
        End While

        connect.Close()

        If Me.ViewState("FieldsT") Then
            SATI_SDS_Table.DataBind()
        End If
        Me.ViewState().Clear()
    End Sub

    Sub RetireRow(sender As Object, e As EventArgs)
        Dim LKBT As LinkButton = CType(sender, LinkButton)
        Dim index As Integer = LKBT.CommandArgument
        Dim row As GridViewRow = SATI_SDS_Table.Rows(index)
        Dim key As Integer = SATI_SDS_Table.DataKeys(row.RowIndex)(0).ToString()


        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT Op FROM T_SATI_SDS WHERE [Key] = " & key & ""

        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
        connect.ConnectionString = conString
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand
        command.CommandText = camString
        command.Connection = connect

        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If checker(0) = User.Identity.Name.ToString Then 'ADD GROUP IF NEEDED
                RetireRowHelper(key)
                Exit While
            Else
                Me.SATI_SDS_Table.ShowFooter = False
                Me.FooterErrorPanel.Visible = True
                Me.FooterErrorMessage.Text = "Permission Denied: You may only RETIRE your own."
                Me.FooterErrorMessage.ForeColor = Drawing.Color.DarkRed
                Me.FooterErrorMessage.Font.Size = 15
                Me.FooterErrorMessage.Font.Bold = True
            End If
        End While
        connect.Close()

        SATI_SDS_Table.DataBind()
        SATI_SDS_Retire.DataBind()
        SATI_SDS_Both.DataBind()
    End Sub
    Sub RetireRowHelper(key As Integer)
        Dim ExpDate As Date = Date.Today
        Dim ExpOp As String = User.Identity.Name.ToString
        Dim query As String = "UPDATE [T_SATI_SDS] SET [ExpDate] = @ExpDate, [ExpOp] = @ExpOp WHERE [Key] = @Key"
        Dim connection As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString

        Using connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection(connection)
            Using command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand(query)
                command.Parameters.AddWithValue("@Key", key)
                command.Parameters.AddWithValue("@ExpDate", ExpDate)
                command.Parameters.AddWithValue("@ExpOp", ExpOp)
                command.Connection = connect
                connect.Open()
                command.ExecuteNonQuery()
                connect.Close()
            End Using
        End Using

        SATI_SDS_Table.DataBind()
        SATI_SDS_Retire.DataBind()
        SATI_SDS_Both.DataBind()
    End Sub

    Sub ReinstateFile(sender As Object, e As EventArgs)
        Me.FooterRestorePanel.Visible = False
        Me.SATI_SDS_Retire.ShowFooter = False
        Me.FooterErrorPanel2.Visible = False

        Dim LKBT As LinkButton = CType(sender, LinkButton)
        Dim index As Integer = LKBT.CommandArgument
        Dim row As GridViewRow = SATI_SDS_Retire.Rows(index)
        Dim key As Integer = SATI_SDS_Retire.DataKeys(row.RowIndex)(0).ToString()


        Dim conString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Dim camString As String = "SELECT Op FROM T_SATI_SDS WHERE [Key] = " & key & ""

        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
        connect.ConnectionString = conString
        connect.Open()

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand
        command.CommandText = camString
        command.Connection = connect

        If Not ErrorMessage.Text = "------------------------------------------------------------------------------------" Then
            ErrorMessage.Text = "------------------------------------------------------------------------------------"
            Me.ErrorMessage.ForeColor = Drawing.Color.Black
        End If

        Dim checker As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (checker.Read())
            If checker(0) = User.Identity.Name.ToString Then 'ADD GROUP IF NEEDED
                ReinstateFileHelper(key)
                Exit While
            Else
                Me.SATI_SDS_Table.ShowFooter = False
                Me.FooterErrorPanel2.Visible = True
                Me.FooterErrorMessage2.Text = "Permission Denied: You may only RESTORE your own."
                Me.FooterErrorMessage2.ForeColor = Drawing.Color.DarkRed
                Me.FooterErrorMessage2.Font.Size = 15
                Me.FooterErrorMessage2.Font.Bold = True
            End If
        End While
        connect.Close()

        CType(Me.RestoreExpDate, TextBox).Text = String.Empty
    End Sub
    Sub ReinstateFileHelper(key As Integer)
        Me.FooterRestorePanel.Visible = True
        Me.FooterErrorPanel2.Visible = False
        Me.SATI_SDS_Retire.ShowFooter = False


        Me.ViewState.Add("RESTORE", key)
    End Sub
    Protected Sub YesAdd_Click(sender As Object, e As EventArgs) Handles YesAdd.Click
        Me.FooterRestorePanel.Visible = False
        Me.FooterErrorPanel2.Visible = False
        Me.SATI_SDS_Retire.ShowFooter = False

        Dim error0 As Boolean = True

        If Me.RestoreUploaded.HasFile = False Or Me.RestoreExpDate.Text = String.Empty Then

            If Me.RestoreUploaded.HasFile = False Then
                Me.FooterErrorPanel2.Visible = True
                Me.FooterErrorMessage2.Text = "FYI: FILE uploader was empty; Original retired SDS file was used for restoration."
                Me.FooterErrorMessage2.ForeColor = Drawing.Color.Yellow
                Me.FooterErrorMessage2.Font.Size = 15
                Me.FooterErrorMessage2.Font.Bold = True
            End If
            If Me.RestoreExpDate.Text = String.Empty Then
                Me.FooterErrorPanel2.Visible = True
                Me.FooterErrorMessage2.Text = "FYI: EXPIRATION Date was empty; nothing set."
                Me.FooterErrorMessage2.ForeColor = Drawing.Color.Yellow
                Me.FooterErrorMessage2.Font.Size = 15
                Me.FooterErrorMessage2.Font.Bold = True
            End If

            If Me.RestoreUploaded.HasFile = False And Me.RestoreExpDate.Text = String.Empty Then
                Me.FooterErrorPanel2.Visible = True
                Me.FooterErrorMessage2.Text = "FYI: FILE uploader was empty; Original Retired SDS file was used. -- EXPIRATION Date was empty; nothing set."
                Me.FooterErrorMessage2.ForeColor = Drawing.Color.Yellow
                Me.FooterErrorMessage2.Font.Size = 15
                Me.FooterErrorMessage2.Font.Bold = True
            End If
        End If

        Dim RED As Date
        Dim TestRED As Date = #1/1/0001 12:00:00 AM#

        Date.TryParse(Me.RestoreExpDate.Text, RED)
        If Not RED = TestRED Then
            If RED <= Date.Today Then
                error0 = False
                Me.FooterErrorPanel2.Visible = True
                Me.FooterErrorMessage2.Text = "FYI: EXPIRATION Date has to be set for a future date."
                Me.FooterErrorMessage2.ForeColor = Drawing.Color.Yellow
                Me.FooterErrorMessage2.Font.Size = 15
                Me.FooterErrorMessage2.Font.Bold = True
            End If
        End If


        If RestoreUploaded.HasFile = True Then
            If RestoreUploaded.FileName.Length >= 39 Then
                error0 = False
                Me.FooterErrorPanel2.Visible = True
                Me.FooterErrorMessage2.Text = "ERROR: The Uploaded File Name is too long, Consider shortening. Max Length is 35"
                Me.FooterErrorMessage2.ForeColor = Drawing.Color.DarkRed
                Me.FooterErrorMessage2.Font.Size = 15
                Me.FooterErrorMessage2.Font.Bold = True
            End If
        End If


        If error0 = True Then
            SqlDataSourceRetired.Insert()
        End If

        Me.FooterRestorePanel.Visible = False
        Me.FooterErrorPanel2.Visible = True
        Me.SATI_SDS_Retire.ShowFooter = False

        SATI_SDS_Table.DataBind()
        SATI_SDS_Retire.DataBind()
        SATI_SDS_Both.DataBind()
    End Sub

    Protected Sub SqlDataSourceRetire_Inserting(ByVal sender As Object, ByVal e As SqlDataSourceCommandEventArgs) Handles SqlDataSourceRetired.Inserting
        Dim AddName As String = ""
        Dim AddAKA As String = ""
        Dim AddNote As String = ""
        Dim AddOp As String = ""
        Dim Addfile As String = ""

        'NON STATIC VARIABLES
        Dim AddExp As String = CType(Me.RestoreExpDate, TextBox).Text
        Dim fileName As String = IO.Path.GetFileName(RestoreUploaded.FileName)

        Dim connectString As String = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString

        Dim commandName As String = "SELECT Name FROM T_SATI_SDS WHERE [Key] = " & ViewState("RESTORE") & ""
        Dim commandAlias As String = "SELECT AKA FROM T_SATI_SDS WHERE [Key] = " & ViewState("RESTORE") & ""
        Dim commandNotes As String = "SELECT Notes FROM T_SATI_SDS WHERE [Key] = " & ViewState("RESTORE") & ""
        Dim commandOp As String = "SELECT Op FROM T_SATI_SDS WHERE [Key] = " & ViewState("RESTORE") & ""

        Dim commandFileName As String = "SELECT FileName FROM T_SATI_SDS WHERE [Key] = " & ViewState("RESTORE") & ""

        Dim connect As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection
        connect.ConnectionString = connectString

        Dim command As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand

        connect.Open()
        'GET NAME FROM SERVER
        command.CommandText = commandName
        command.Connection = connect
        Dim NameFinder As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (NameFinder.Read())
            AddName = NameFinder.GetString(0)
        End While
        connect.Close()

        connect.Open()
        'GET ALIAS FROM SERVER
        command.CommandText = commandAlias
        command.Connection = connect
        Dim AliasFinder As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (AliasFinder.Read())
            AddAKA = AliasFinder.GetString(0)
        End While
        connect.Close()

        connect.Open()
        'GET NOTES FROM SERVER
        command.CommandText = commandNotes
        command.Connection = connect
        Dim NotesFinder As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (NotesFinder.Read())
            AddNote = NotesFinder.GetString(0)
        End While
        connect.Close()

        connect.Open()
        'GET OP FROM SERVER
        command.CommandText = commandOp
        command.Connection = connect
        Dim OpFinder As Data.SqlClient.SqlDataReader = command.ExecuteReader()
        While (OpFinder.Read())
            AddOp = OpFinder.GetString(0)
        End While
        connect.Close()

        connect.Open()
        If RestoreUploaded.HasFile = True Then
            Dim pathCheck As String = ROOT + fileName

            If System.IO.File.Exists(pathCheck) Then
                Dim counter As Integer = 2
                Dim TempFileName As String = ""
                While System.IO.File.Exists(pathCheck)
                    Dim dash As String = " - "
                    Dim copy As String = "Copy ("
                    Dim parent As String = ")"
                    TempFileName = fileName.Insert(fileName.IndexOf(".pdf"), dash)
                    TempFileName = TempFileName.Insert(TempFileName.IndexOf(".pdf"), copy)
                    TempFileName = TempFileName.Insert(TempFileName.IndexOf(".pdf"), counter.ToString())
                    TempFileName = TempFileName.Insert(TempFileName.IndexOf(".pdf"), parent)

                    counter += 1
                    pathCheck = ROOT + TempFileName
                End While

                fileName = TempFileName
                Me.FooterErrorMessage2.Text = "File Exists. File saved as " & fileName & " for restoration."
                Me.FooterErrorMessage2.ForeColor = Drawing.Color.Yellow
                Me.FooterErrorMessage2.Font.Size = 15
                Me.FooterErrorMessage2.Font.Bold = True

                If Not fileName = IO.Path.GetFileName(RestoreUploaded.PostedFile.FileName).ToString Then
                    Me.ViewState.Add("NewFileName", fileName)
                End If

            Else
                Me.ErrorMessage.Text = "SDS FILE SAVED AND RESTORED SUCCESSFULLY"
                Me.ErrorMessage.ForeColor = Drawing.Color.DarkGreen
                Me.FooterErrorMessage2.Font.Size = 15
                Me.FooterErrorMessage2.Font.Bold = True
            End If

            'SAVE NEW FILE
            Uploader.PostedFile.SaveAs(ROOT + fileName)
            'Uploader.PostedFile.SaveAs((Server.MapPath(ROOT) + fileName))

            Addfile = fileName
        Else
            command.CommandText = commandFileName
            command.Connection = connect
            Dim FileFinder As Data.SqlClient.SqlDataReader = command.ExecuteReader()
            While (FileFinder.Read())
                Addfile = FileFinder.GetString(0)
            End While
        End If
        connect.Close()


        e.Command.Parameters("@Name").Value = AddName
        e.Command.Parameters("@AKA").Value = AddAKA

        Dim ED As Date
        If String.IsNullOrEmpty(AddExp) Then
            e.Command.Parameters("@ExpDate").Value = DBNull.Value
        Else
            Date.TryParse(AddExp, ED)
            e.Command.Parameters("@ExpDate").Value = ED
        End If

        e.Command.Parameters("@FileName").Value = Addfile
        e.Command.Parameters("@Notes").Value = AddNote
        e.Command.Parameters("@OP").Value = AddOp
        e.Command.Parameters("@ExpOp").Value = DBNull.Value 'User.Identity.Name.ToString
    End Sub

    Protected Sub CancelRestore(sender As Object, e As EventArgs) Handles NoAdd.Click
        Me.RestoreUploaded.Attributes.Clear()
        CType(Me.RestoreExpDate, TextBox).Text = String.Empty
    End Sub

End Class
