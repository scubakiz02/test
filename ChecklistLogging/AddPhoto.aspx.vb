
Imports System.Text.Json
Imports System.Data
Imports System.IO
Imports System.Text.RegularExpressions

Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim AreaFromQueryString As String
    Dim DS As New Data.DataSet
    Dim DR As Data.DataRow
    Dim RC As Integer
    Dim fileName As String
    Dim uploadDirectory As String
    Dim DataKeyFromQueryString As String
    Dim AcceptedFormats As String() = {"tif", "tiff", "jpg", "jpeg", "png", "gif", "bmp"}
    Dim FormatToContentType As New Dictionary(Of String, String) From
     {
        {"jpg", "jpeg"},
        {"svg", "svg%2Bxml"}
     } '%2B is URL encoding for '+'
    Dim ContentTypeToFormat As New Dictionary(Of String, String) From
     {
        {"svg%2Bxml", "svg"}
     } '%2B is URL encoding for '+'


    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        'MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        'MenuAuthenication.CheckGroupAuthenication("Office", Server)
        AreaFromQueryString = Request.QueryString("Area")
        DataKeyFromQueryString = Request.QueryString("DataKey")
        DR = SatiCode.GetMyDataSet("SELECT A.[Key], I.SqlFunc2ndArg, D.Date, A.Area, I.SqlFunc FROM [ALTS].[dbo].[T_LogData] D INNER JOIN [ALTS].[dbo].[T_LogArea] A ON D.AreaKey=A.[Key] INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE D.[Key]=" & DataKeyFromQueryString).Tables(0).Rows(0)
        uploadDirectory = Path.Combine("\\pwi-40\IT$\DevCopy\ST\SatiPhotoLogs\" & Regex.Replace(DR("Area"), "[:#]", ""), GetSingleDbField("SELECT DatePeriod FROM " & DR("SqlFunc") & "(" & DR("Key") & ", " & DR("SqlFunc2ndArg") & ", '" & DR("Date") & "')", "DatePeriod").Replace("/", "-"))
        'uploadDirectory = "Q:DevCopy\ST\SatiPhotoLogs\" & DR("Area").Replace(" ", "_") & "\" & GetSingleDbField("SELECT DatePeriod FROM " & DR("SqlFunc") & "(" & DR("Key") & ", " & DR("SqlFunc2ndArg") & ", '" & DR("Date") & "')", "DatePeriod").Replace("/", "-").Replace(" ", "_")

        If Not IsPostBack Then
            'AreaLabel.Text = GetSingleDbField("SELECT Area FROM [ALTS].[dbo].[T_LogArea] WHERE [Key]=" & AreaFromQueryString, "Area")
            'DS = SatiCode.GetMyDataSet("SELECT Stamp.Title As Text, Stamp.[Key] As Value, Stamped.Active AS Selected FROM [ALTS].[dbo].[T_LogStampTitle] Stamp INNER JOIN [ALTS].[dbo].[T_LogStampList] Stamped ON Stamp.[Key]=Stamped.TitleKey AND Stamped.AreaKey=" & AreaFromQueryString)
            'RC = DS.Tables(0).Rows.Count

            'For I = 0 To RC - 1
            '    Dim listItem As New ListItem()
            '    DR = DS.Tables(0).Rows(I)

            '    listItem.Text = DR("Text")
            '    listItem.Value = DR("Value") 'associated Key within [T_LogStampTitle]
            '    listItem.Selected = DR("Selected")

            '    StampCheckBoxList.Items.Add(listItem)
            'Next
        End If
    End Sub

    Protected Sub UploadFile(sender As Object, e As EventArgs)
        Dim fileNameDelimited As String()
        Dim CompleteImagePath As String
        Dim Format As String
        Dim TestFile As String
        Dim match As Match
        Dim FileFormat As String

        If Not Uploader.HasFile Then
            '    ErrorMessage.Text = "CHOOSE A FILE BEFORE UPLOADING"
            Exit Sub
        End If

        fileName = IO.Path.GetFileName(Uploader.FileName)
        TestFile = fileName
        fileNameDelimited = fileName.Split(".")
        Format = fileNameDelimited(fileNameDelimited.Count - 1)
        match = Regex.Match(TestFile, "[% < > : / \ | ? *]")
        FileFormat = fileName.Split(".")(1)

        'Check for format other than an image
        If Not AcceptedFormats.Contains(FileFormat) Then
            'AcceptedFormat = False
        End If

        'REMOVES CHARACTERS THAT ARENT ALLOWED IN FILE NAMES
        Do While match.Success
            Dim key As String = match.Value
            TestFile = TestFile.Replace(key, String.Empty)
            match = match.NextMatch()
        Loop
        fileName = TestFile

        If Not Directory.Exists(uploadDirectory) Then
            Directory.CreateDirectory(uploadDirectory)
        End If

        CompleteImagePath = Path.Combine(uploadDirectory, fileName)
        Uploader.PostedFile.SaveAs(CompleteImagePath)

        'variables declared in UploadFile do NOT hold their value, so I tied them to the session
        Session("CompleteImagePath") = CompleteImagePath
        Session("ContentType") = If(FormatToContentType.ContainsKey(Format), FormatToContentType(Format), Format)

        UploadPanel.Visible = False
        CancelSetPanel.Visible = True
        SnapshotImageButton.Visible = True
        'SnapshotImageButton.ImageUrl = "ImageHandler.ashx?FileName=" & fileName & "&DataKey=" & DataKeyFromQueryString & "&Format=" & Session("Format")
        SnapshotImageButton.ImageUrl = "ImageHandler.ashx?PhotoFilePath=" & CompleteImagePath & "&ContentType=" & Session("ContentType")
    End Sub

    Function GetSingleDbField(SqlQuery As String, Field As String) As String
        Dim Res As String

        'using try catch block in case 'There is no row at position 0.', which means there are no associated record in Table
        Try
            Res = If(IsDBNull(SatiCode.GetMyDataSet(SqlQuery).Tables(0).Rows(0)(Field)), Nothing, SatiCode.GetMyDataSet(SqlQuery).Tables(0).Rows(0)(Field)) 'using ternary operator as a workaround to Null DB field values, which in that case the function will return Nothing
        Catch ex As Exception
            Res = Nothing
        End Try

        Return Res
    End Function

    Sub ExecuteSqlQuery(SqlQuery As String)
        Dim Connection As New Data.SqlClient.SqlConnection
        Dim MySQLCommand As New Data.SqlClient.SqlCommand
        Connection.ConnectionString = Session("DBConnect")
        Connection.Open()
        With MySQLCommand
            .CommandText = SqlQuery
            .Connection = Connection
        End With
        MySQLCommand.ExecuteNonQuery()
        Connection.Close()
    End Sub

    Function StripString(ByVal input As String) As String
        Return Regex.Replace(input, "[^a-zA-Z0-9]", "").ToLower()
    End Function

    Function SqlProofSingleQuotes(Text As String) As String
        Return Text.Replace("'", "''") 'escape single quotes (') by doubling them ('')
    End Function

    Protected Sub CancelImage_OnClick(sender As Object, e As EventArgs)
        Response.Redirect(Request.Url.ToString())
    End Sub

    Protected Sub ExitIframeButton_onClick(sender As Object, e As EventArgs)
        'variables declared in UploadFile do NOT hold their value, so I tied them to the session
        Dim CompleteImagePath As String = Session("CompleteImagePath")
        Dim UserInput = ImgNameTextBox.Text
        Dim FileName As String
        Dim DuplicateDS As Data.DataSet
        Dim DuplicateRC As Integer
        Dim DuplicateDR As Data.DataRow
        Dim StrippedUserInput As String

        Try
            If sender.Text = "Set" Then
                If String.IsNullOrEmpty(UserInput) Then
                    Throw New ArgumentException("*ERROR: PHOTO REQUIRES A TITLE*")
                ElseIf New Regex("[<>:""'/\\|?*]").IsMatch(UserInput) Then
                    Throw New ArgumentException("*ERROR: ILLEGAL CHARACTERS (<, >, :, ', "", /, \, |, ?, *) EXIST IN THE TITLE*")
                Else
                    DuplicateDS = SatiCode.GetMyDataSet("SELECT PhotoTitle FROM [ALTS].[dbo].[T_LogDataPhotos] P WHERE DataKey=" & DataKeyFromQueryString)
                    DuplicateRC = DuplicateDS.Tables(0).Rows.Count
                    FileName = UserInput.Replace(" ", "_") & "." & If(ContentTypeToFormat.ContainsKey(Session("ContentType")), ContentTypeToFormat(Session("ContentType")), Session("ContentType"))
                    StrippedUserInput = StripString(UserInput)

                    'ensure checklist name does NOT currently exist in T_LogArea
                    For J = 0 To DuplicateRC - 1
                        DuplicateDR = DuplicateDS.Tables(0).Rows(J)

                        If StrippedUserInput = StripString(DuplicateDR("PhotoTitle")) Then
                            Throw New ArgumentException("*ERROR: A PHOTO WITH THIS TITLE EXISTS FOR THIS LOG*")
                        End If
                    Next

                    'ExecuteSqlQuery("INSERT INTO [ALTS].[dbo].[T_LogDataPhotos] (DataKey, PhotoTitle, PhotoFilePath) VALUES (" & DataKeyFromQueryString & ", '" & Title & "', '" & Path.Combine(uploadDirectory, FileName) & "')")
                    My.Computer.FileSystem.RenameFile(CompleteImagePath, FileName)
                    ExecuteSqlQuery("INSERT INTO [ALTS].[dbo].[T_LogDataPhotos] (DataKey, PhotoTitle, PhotoFilePath, ContentType, FileName) VALUES (" & DataKeyFromQueryString & ", '" & SqlProofSingleQuotes(UserInput) & "', '" & Path.Combine(uploadDirectory, FileName) & "', '" & Session("ContentType") & "', '" & FileName & "')")
                End If
            Else 'Cancel
                If CompleteImagePath IsNot Nothing Then
                    System.IO.File.Delete(CompleteImagePath)
                End If
            End If
        Catch ex As Exception
            UserErrorLabel.Text = ex.Message.ToString()
            Exit Sub
        End Try

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "disableIframe", "disableIframe();", True)
    End Sub
End Class
