Imports System.Data
Imports System.IO

Partial Class DBMaintenance_EditRoles
    Inherits System.Web.UI.Page

    Dim SatiCode As New Class1
    Dim fileName As String
    Dim OcrFileContents As String
    Dim OcrError As String
    Dim uploadDirectory As String
    Dim AcceptedFormats As String() = {"tif", "tiff", "jpg", "jfif", "jpeg", "png", "gif", "bmp"}

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        MenuAuthenication.CheckGroupAuthenication("EditRoles", Server)
        Me.Page.Form.Enctype = "multipart/form-data"

        If Not Page.IsPostBack Then
            'Set IsAnonymous field of rows with a LastActivityDate at minimum 1 year ago
            SatiCode.DeleteMyAltsRecords("UPDATE [SatiUsers].[dbo].[aspnet_Users] SET IsAnonymous=1 WHERE IsAnonymous=0 AND LastActivityDate < DATEADD(YEAR,-1,CAST(GETDATE() AS DATE))")
        End If

        fileName = Request.QueryString("File")
        OcrFileContents = Session(fileName)
        OcrError = Session("OcrError")
        uploadDirectory = Server.MapPath(System.IO.Path.GetDirectoryName(Request.Url.AbsolutePath) + "/Files/")

        If OcrError Is Nothing Then
            If Not OcrFileContents Is Nothing And Uploader.FileName = "" Then 'checking FileName property in FileUpload control to prevent previous file being uploaded to Table again
                FileContentsLabel.Text = OcrFileContents

                'add record to db
                Dim SqlRow As Data.DataRow = CloudAction("New")

                For Each filePath As String In Directory.GetFiles(uploadDirectory)
                    File.Delete(filePath)
                Next

                'download file to drive from DB
                'File.WriteAllBytes(uploadDirectory + fileName, SqlRow("FileBlob")) ' Write the binary data to a file
            End If
        Else
            ErrorMessage.Text = OcrError
        End If
    End Sub

    Function CloudAction(ByVal Action As String) As Data.DataRow
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = Session("DBConnect")
        Connection.Open()

        Dim DA_MRTicket As New Data.SqlClient.SqlDataAdapter
        Dim DS_MRTicket As New Data.DataSet
        Dim DR_MRTicket As Data.DataRow
        '*****************************************************************
        '************************Select***********************************
        '*****************************************************************
        Dim MRTicketSelectCmd As New System.Data.SqlClient.SqlCommand
        With MRTicketSelectCmd
            .CommandText = "SELECT Id, FileName, FileText, FileBlob, UploadDate, DeleteDate, VisibleTo, UploadedBy FROM ALTS.dbo.T_SatiCloud" 'WHERE (MR_Key = '" & Ticket & "')
            .Connection = Connection
        End With
        DA_MRTicket.SelectCommand = MRTicketSelectCmd

        '*****************************************************************
        '************************Insert***********************************
        '*****************************************************************
        Dim MRTicketInsertCmd As New System.Data.SqlClient.SqlCommand
        With MRTicketInsertCmd
            .CommandText = "INSERT INTO ALTS.dbo.T_SatiCloud (FileName, FileText, FileBlob, UploadDate, VisibleTo, UploadedBy) VALUES (@FileName, @FileText, @FileBlob, @UploadDate, @VisibleTo, @UploadedBy) SELECT Id, FileName, FileText, FileBlob, UploadDate, DeleteDate, VisibleTo, UploadedBy FROM ALTS.dbo.T_SatiCloud WHERE Id = SCOPE_IDENTITY()"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@FileName", System.Data.SqlDbType.VarChar, 0, "FileName"), New System.Data.SqlClient.SqlParameter("@FileText", System.Data.SqlDbType.VarChar, 0, "FileText"), New System.Data.SqlClient.SqlParameter("@FileBlob", System.Data.SqlDbType.VarBinary, 0, "FileBlob"), New System.Data.SqlClient.SqlParameter("@UploadDate", System.Data.SqlDbType.SmallDateTime, 0, "UploadDate"), New System.Data.SqlClient.SqlParameter("@DeleteDate", System.Data.SqlDbType.SmallDateTime, 0, "DeleteDate"), New System.Data.SqlClient.SqlParameter("@VisibleTo", System.Data.SqlDbType.VarChar, 0, "VisibleTo"), New System.Data.SqlClient.SqlParameter("@UploadedBy", System.Data.SqlDbType.VarChar, 0, "UploadedBy")})
        End With
        DA_MRTicket.InsertCommand = MRTicketInsertCmd

        '*****************************************************************
        '************************Update***********************************
        '*****************************************************************
        Dim MRTicketUpdateCmd As New System.Data.SqlClient.SqlCommand
        With MRTicketUpdateCmd
            .CommandText = "UPDATE ALTS.dbo.T_SatiCloud SET VisibleTo='Szymon Tyburek,Test User, Jeff Sisko' WHERE Id=@Id"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Tool", System.Data.SqlDbType.Int, 0, "Tool"), New System.Data.SqlClient.SqlParameter("@Status", System.Data.SqlDbType.VarChar, 0, "Status"), New System.Data.SqlClient.SqlParameter("@IssueDate", System.Data.SqlDbType.SmallDateTime, 0, "IssueDate"), New System.Data.SqlClient.SqlParameter("@IssueUser", System.Data.SqlDbType.VarChar, 0, "IssueUser"), New System.Data.SqlClient.SqlParameter("@CloseDate", System.Data.SqlDbType.SmallDateTime, 0, "CloseDate"), New System.Data.SqlClient.SqlParameter("@CloseUser", System.Data.SqlDbType.VarChar, 0, "CloseUser"), New System.Data.SqlClient.SqlParameter("@Original_MR_Key", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MR_Key", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Tool", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Tool", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Tool", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Tool", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_Status", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "Status", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_Status", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Status", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_IssueDate", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "IssueDate", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_IssueDate", System.Data.SqlDbType.SmallDateTime, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "IssueDate", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_IssueUser", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "IssueUser", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_IssueUser", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "IssueUser", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_CloseDate", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "CloseDate", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_CloseDate", System.Data.SqlDbType.SmallDateTime, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CloseDate", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@IsNull_CloseUser", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, CType(0, Byte), CType(0, Byte), "CloseUser", System.Data.DataRowVersion.Original, True, Nothing, "", "", ""), New System.Data.SqlClient.SqlParameter("@Original_CloseUser", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CloseUser", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@MR_Key", System.Data.SqlDbType.Int, 4, "MR_Key")})
        End With
        DA_MRTicket.UpdateCommand = MRTicketUpdateCmd

        '*****************************************************************
        '************************Genral***********************************
        '*****************************************************************
        DA_MRTicket.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_SatiCloud", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("Id", "Id"), New System.Data.Common.DataColumnMapping("FileName", "FileName"), New System.Data.Common.DataColumnMapping("FileText", "FileText"), New System.Data.Common.DataColumnMapping("FileBlob", "FileBlob"), New System.Data.Common.DataColumnMapping("UploadDate", "UploadDate"), New System.Data.Common.DataColumnMapping("DeleteDate", "DeleteDate"), New System.Data.Common.DataColumnMapping("", ""), New System.Data.Common.DataColumnMapping("VisibleTo", "VisibleTo"), New System.Data.Common.DataColumnMapping("UploadedBy", "UploadedBy")})})
        DA_MRTicket.Fill(DS_MRTicket)

        Select Case Action
            Case "New"
                DR_MRTicket = DS_MRTicket.Tables("T_SatiCloud").NewRow
                DR_MRTicket("FileName") = fileName
                DR_MRTicket("FileText") = OcrFileContents
                DR_MRTicket("FileBlob") = Session("Blob")
                DR_MRTicket("UploadDate") = System.DateTime.Now.ToShortTimeString
                DR_MRTicket("VisibleTo") = "Szymon Tyburek,Test User"
                DR_MRTicket("UploadedBy") = User.Identity.Name.ToString
                DS_MRTicket.Tables("T_SatiCloud").Rows.Add(DR_MRTicket)
                DA_MRTicket.Update(DS_MRTicket, "T_SatiCloud")
                CloudAction = DR_MRTicket

            Case "ModStatus"
                DR_MRTicket = DS_MRTicket.Tables(0).Rows(0)
                DR_MRTicket.AcceptChanges()
                DR_MRTicket.BeginEdit()
                DR_MRTicket.EndEdit()
                DA_MRTicket.Update(DS_MRTicket, "T_SatiCloud")
            Case "Close"
                '*****************************************************************
                '** check to make sure they did not spam the close button ********
                '*****************************************************************
                Dim ds As Data.DataSet
                Dim DR As Data.DataRow
                Dim nullcheck As Boolean

                'SELECT CloseDate FROM dbo.T_SatiCloud WHERE (MR_Key = 53868)
                ds = SatiCode.GetMyDataSet("SELECT CloseDate FROM dbo.T_SatiCloud" & ")") 'WHERE (MR_Key = " & Ticket
                DR = ds.Tables(0).Rows(0)
                nullcheck = IsDBNull(DR("CloseDate"))
                If nullcheck = False Then
                    Exit Function
                End If

                '*****************************************************************
                '*****************************************************************

                DR_MRTicket = DS_MRTicket.Tables(0).Rows(0)
                DR_MRTicket.AcceptChanges()
                DR_MRTicket.BeginEdit()
                DR_MRTicket("CloseDate") = System.DateTime.Now.ToShortTimeString
                DR_MRTicket("CloseUser") = User.Identity.Name.ToString
                DR_MRTicket.EndEdit()
                DA_MRTicket.Update(DS_MRTicket, "T_SatiCloud")
        End Select
        Connection.Close()
    End Function

    Protected Sub UploadFile(sender As Object, e As EventArgs)
        fileName = IO.Path.GetFileName(Uploader.FileName)
        If fileName = "" Then
            ErrorMessage.Text = "CHOOSE A FILE BEFORE UPLOADING"
            Exit Sub
        End If

        Dim TestFile As String = fileName
        Dim match As Match = Regex.Match(TestFile, "[% < > : / \ | ? *]")
        Dim FileFormat As String = fileName.Split(".")(1)
        Dim AcceptedFormat As Boolean = True

        'Check for format other than an image
        If Not AcceptedFormats.Contains(FileFormat) Then
            AcceptedFormat = False
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

        Response.Redirect("OcrScan.aspx?File=" & fileName & "&AcceptedFormat=" & AcceptedFormat)
    End Sub
    Protected Sub ActiveUsersDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'modSQL(ActiveUsersDropDownList)
        'LockUser("Look", ActiveUsersDropDownList)
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
End Class
