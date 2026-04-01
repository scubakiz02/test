Imports System.Diagnostics
Imports System.IO

Partial Class DirectoryNavigator
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If User.Identity.IsAuthenticated = True Then
            MenuAuthenication.AuthenicationByPass(Page)
        End If

        'This is needed to make the download popup happen on clients browser
        Dim ScriptManager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        ScriptManager.RegisterPostBackControl(Me.DownloadButton)

        If (Not Page.IsPostBack) Then
            Dim StartingPath As String

            If Server.UrlDecode(Request.QueryString("Nav")) = Nothing Then
                StartingPath = "Controlled Forms"
            Else
                StartingPath = Server.UrlDecode(Request.QueryString("Nav").ToString)
            End If

            DirectoryVeiwer(StartingPath)
            SetPathsRadioButton(StartingPath)
        End If
    End Sub

    Protected Sub FormsRB_CheckedChanged(sender As Object, e As EventArgs) Handles FormsRB.CheckedChanged
        If FormsRB.Checked = True Then
            ResetRadioButton()
            FormsRB.Checked = True
            DirectoryVeiwer("Controlled Forms")
        End If
    End Sub
    Protected Sub ProceduresRB_CheckedChanged(sender As Object, e As EventArgs) Handles ProceduresRB.CheckedChanged
        If ProceduresRB.Checked = True Then
            ResetRadioButton()
            ProceduresRB.Checked = True
            DirectoryVeiwer("Controlled Procedures")
        End If
    End Sub
    Protected Sub WorkInstructionRB_CheckedChanged(sender As Object, e As EventArgs) Handles WorkInstructionRB.CheckedChanged
        If WorkInstructionRB.Checked = True Then
            ResetRadioButton()
            WorkInstructionRB.Checked = True
            DirectoryVeiwer("Controlled Work Instructions")
        End If
    End Sub
    Protected Sub MiscLabelsRB_CheckedChanged(sender As Object, e As EventArgs) Handles MiscLabelsRB.CheckedChanged
        If MiscLabelsRB.Checked = True Then
            ResetRadioButton()
            MiscLabelsRB.Checked = True
            DirectoryVeiwer("Labels")
        End If
    End Sub
    Protected Sub RecipesRB_CheckedChanged(sender As Object, e As EventArgs) Handles RecipesRB.CheckedChanged
        If RecipesRB.Checked = True Then
            ResetRadioButton()
            RecipesRB.Checked = True
            DirectoryVeiwer("Controlled Recipes")
        End If
    End Sub

    Protected Sub ProLogsRB_CheckedChanged(sender As Object, e As EventArgs) Handles ProLogsRB.CheckedChanged
        If ProLogsRB.Checked = True Then
            ResetRadioButton()
            ProLogsRB.Checked = True
            DirectoryVeiwer("Pro-Logs")
        End If
    End Sub
    Protected Sub NavBW_Clicked(sender As Object, e As EventArgs) Handles NavBW.Click
        Dim BackPath As String = Me.WrittenPathLabel.Text
        BackPath = BackPath.Substring((BackPath.IndexOf(": ") + 2))
        BackPath = BackPath.Substring(0, BackPath.LastIndexOf("\"))

        If BackPath = "\\57.201.101.139\docshare" Then
            If SearchKey.Text = "" Then
                Exit Sub
            Else
                DirectoryVeiwer(ViewState("CurrentPath"))
            End If
        Else
            Me.ViewState("LastDirectoryPath") = Me.WrittenPathLabel.Text
            DirectoryVeiwer(BackPath)
            ViewState("NavBackPress") = True
        End If
    End Sub
    Protected Sub NavFW_Clicked(sender As Object, e As EventArgs) Handles NavFW.Click
        If ViewState("NavBackPress") = True Then
            Dim ForwardsPath As String = Me.ViewState("LastDirectoryPath")
            ForwardsPath = ForwardsPath.Substring((ForwardsPath.IndexOf(": ") + 2))

            If ForwardsPath = "\\57.201.101.139\docshare" Then
                Exit Sub
            Else
                ViewState("NavBackPress") = False
                DirectoryVeiwer(ForwardsPath)
            End If
        End If
    End Sub

    Protected Sub SetPathsRadioButton(PathPart As String)
        If PathPart = "Controlled Forms" Then
            ResetRadioButton()
            FormsRB.Checked = True
        ElseIf PathPart = "Controlled Procedures" Then
            ResetRadioButton()
            ProceduresRB.Checked = True
        ElseIf PathPart = "Controlled Work Instructions" Then
            ResetRadioButton()
            WorkInstructionRB.Checked = True
        ElseIf PathPart = "Labels" Then
            ResetRadioButton()
            MiscLabelsRB.Checked = True
        ElseIf PathPart = "Controlled Recipes" Then
            ResetRadioButton()
            RecipesRB.Checked = True
        ElseIf PathPart = "Pro-Logs" Then
            ResetRadioButton()
            ProLogsRB.Checked = True
        End If
    End Sub
    Protected Sub ResetRadioButton()
        FormsRB.Checked = False
        ProceduresRB.Checked = False
        WorkInstructionRB.Checked = False
        MiscLabelsRB.Checked = False
        RecipesRB.Checked = False
        ProLogsRB.Checked = False
    End Sub
    Protected Sub DirectoryVeiwer(ByVal Path As String)
        If Path.Contains("\") = False Then
            Path = "\\57.201.101.139\docshare\" + Path '
        End If

        'Define the current directory.
        Dim dir As DirectoryInfo = New DirectoryInfo(Path)
        'Define Directories and Files from this directory
        Dim files As FileInfo() = dir.GetFiles()
        Dim dirs As DirectoryInfo() = dir.GetDirectories()

        'Show the directory listing.
        GridFileList.DataSource = files
        GridDirList.DataSource = dirs
        GridDirList.DataBind()
        GridFileList.DataBind()

        GridFileList.SelectedIndex = -1

        'Keep track of the current path.
        ViewState("CurrentPath") = Path
        WrittenPathLabel.Text = "Currently showing: " & Path
        ViewState("NavBackPress") = False
    End Sub

    Protected Sub GridFileList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridFileList.SelectedIndexChanged
        Dim file As String = CStr(GridFileList.DataKeys(GridFileList.SelectedIndex).Value)

        Dim files As ArrayList = New ArrayList()
        files.Add(New FileInfo(file))

        FormFileDetails.DataSource = files
        FormFileDetails.DataBind()
    End Sub

    Protected Function GetVersionInfoString(ByVal path As Object) As String
        Dim info As FileVersionInfo = FileVersionInfo.GetVersionInfo(CStr(path))
        Return info.FileName & " " & info.FileVersion & "<br>" & info.ProductName & " " & info.ProductVersion
    End Function

    Protected Sub GridDirList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridDirList.SelectedIndexChanged
        Dim dir As String = CStr(GridDirList.DataKeys(GridDirList.SelectedIndex).Value)
        DirectoryVeiwer(dir)
    End Sub

    Protected Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click
        Dim SearchingDirectory As String = WrittenPathLabel.Text
        SearchingDirectory = SearchingDirectory.Substring(SearchingDirectory.IndexOf(": ") + 2)
        Dim SearchingKey As String = SearchKey.Text

        Dim ListOfFiledFound As List(Of String) = New List(Of String)
        ListOfFiledFound = SearchForFiles(SearchingDirectory, SearchingKey)
        Dim DirectInfo As DirectoryInfo
        Dim FoundFiles As List(Of FileInfo) = New List(Of FileInfo)

        For Each prim In ListOfFiledFound
            Dim Path As String = prim.Substring(0, prim.LastIndexOf("\"))
            Dim File As String = prim.Substring(prim.LastIndexOf("\") + 1)
            DirectInfo = New DirectoryInfo(Path)
            FoundFiles.AddRange(DirectInfo.GetFiles(File))
        Next

        GridFileList.DataSource = FoundFiles
        GridDirList.DataBind()
        GridFileList.DataBind()

        GridFileList.SelectedIndex = -1
    End Sub

    Function SearchForFiles(ByVal SearchingDirectory As String, ByVal SearchingKey As String) As List(Of String)
        Dim ReturnedData As New List(Of String)
        Dim FolderStack As New Stack(Of String)
        FolderStack.Push(SearchingDirectory)

        Do While FolderStack.Count > 0
            Dim ThisFolder As String = FolderStack.Pop
            Try
                For Each SubFolder In GetDirectories(ThisFolder)
                    FolderStack.Push(SubFolder)
                Next
                For Each File As String In Directory.GetFiles(ThisFolder)
                    Dim FileName As String = File.Substring(File.LastIndexOf("\") + 1)

                    If FileName.IndexOf(SearchingKey, 0, StringComparison.CurrentCultureIgnoreCase) > -1 Then
                        ReturnedData.AddRange(GetFiles(ThisFolder, FileName))
                    End If
                Next
            Catch ex As Exception
            End Try
        Loop

        Return ReturnedData
    End Function

    Private Function GetFiles(ThisFolder As String, ThisFile As String) As Array
        Return Directory.GetFiles(ThisFolder, ThisFile)
    End Function

    Private Function GetDirectories(thisFolder As String) As IEnumerable(Of Object)
        Return Directory.GetDirectories(thisFolder)
    End Function


    Protected Sub ScaleDown_Click(sender As Object, e As EventArgs) Handles ScaleDown.Click
        Dim NewSize As FontUnit = FormFileDetails.Font.Size
        Dim SizeStr As String
        Dim SizeInt As Integer

        Dim FontConverter As FontUnitConverter = New FontUnitConverter
        SizeStr = FontConverter.ConvertToString(NewSize)
        SizeStr = SizeStr.Substring(0, SizeStr.Length() - 2)
        SizeInt = Integer.Parse(SizeStr)
        SizeInt -= 1

        If SizeInt < 12 Then
            SizeInt = 12
        End If

        SizeStr = SizeInt.ToString()
        NewSize = FontConverter.ConvertFromString(SizeStr)

        FormFileDetails.Font.Size = NewSize
        GridDirList.Font.Size = NewSize
        GridFileList.Font.Size = NewSize
        FontSizeLabel.Text = "Font: " & SizeStr & "px"
    End Sub
    Protected Sub ScaleUp_Click(sender As Object, e As EventArgs) Handles ScaleUp.Click
        Dim NewSize As FontUnit = FormFileDetails.Font.Size
        Dim SizeStr As String
        Dim SizeInt As Integer

        Dim FontConverter As FontUnitConverter = New FontUnitConverter
        SizeStr = FontConverter.ConvertToString(NewSize)
        SizeStr = SizeStr.Substring(0, SizeStr.Length() - 2)
        SizeInt = Integer.Parse(SizeStr)
        SizeInt += 1

        If SizeInt > 18 Then
            SizeInt = 18
        End If

        SizeStr = SizeInt.ToString()
        NewSize = FontConverter.ConvertFromString(SizeStr)

        FormFileDetails.Font.Size = NewSize
        GridDirList.Font.Size = NewSize
        GridFileList.Font.Size = NewSize
        FontSizeLabel.Text = "Font: " & SizeStr & "px"
    End Sub

    Protected Sub DownloadButton_Click(sender As Object, e As EventArgs) Handles DownloadButton.Click
        Dim Path As String = CStr(GridFileList.DataKeys(GridFileList.SelectedIndex).Value)
        Dim File As String = Path.Substring(Path.LastIndexOf("\") + 1)
        Dim Ext As String = File.Substring(File.LastIndexOf("."))

        Dim Response As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
        Try
            Response.Clear()
            Response.ClearContent()
            Response.ClearHeaders()

            Response.ContentType = GetFileMIME(Ext)
            Response.AddHeader("Content-Disposition", "inline; filename=" + File)
            Response.TransmitFile(Path) 'Server.MapPath(Path))
            Response.Flush()
            Response.End()
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
    End Sub

    Protected Function GetFileMIME(FileExtention As String) As String
        Dim FilesMIME As String = ""
        If FileExtention = ".htm" Or FileExtention = ".html" Then
            FilesMIME = "text\HTML"
        ElseIf FileExtention = ".txt" Then
            FilesMIME = "text\plain"
        ElseIf FileExtention = ".doc" Or FileExtention = "rtf" Or FileExtention = ".docx" Then
            FilesMIME = "application\msword"
        ElseIf FileExtention = ".xls" Or FileExtention = ".xlsx" Then
            FilesMIME = "application\x-msexcel"
        ElseIf FileExtention = ".jpg" Or FileExtention = ".jpeg" Then
            FilesMIME = "image\jpeg"
        ElseIf FileExtention = ".gif" Then
            FilesMIME = "image\GIF"
        ElseIf FileExtention = ".pdf" Then
            FilesMIME = "application\pdf"
        End If

        Return FilesMIME
    End Function
End Class