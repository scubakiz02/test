Imports System.IO

Partial Class WI_WorkInstructionEditor
    Inherits System.Web.UI.Page
    Dim AllFileNames As List(Of String)

    'Server side load function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        MenuAuthenication.CheckGroupAuthenication("WIEdit", Server)
    End Sub


    '|=============================================================================================| 
    '|                          _                 _   ___ _ _                                      |
    '|                         | |   ___  __ _ __| | | __(_) |___ ___                              |
    '|                         | |__/ _ \/ _` / _` | | _|| | / -_|_-<                              |
    '|                         |____\___/\__,_\__,_| |_| |_|_\___/__/                              |
    '|                                                                                             |   
    '|=============================================================================================|
    '|   AUTHOR: Aaron Williams                                                DATE: 10/22/2021    |
    '|   This section of the code behind file controls the functions for loading the html files    |
    '|   from the directory server to be loaded into a iframe                                      |
    '|=============================================================================================|

    'Subrountine that listens for the users input from js
    Protected Sub HiddenLoadWI_Click(sender As Object, e As EventArgs) Handles HiddenLoadWI.Click
        SatiUtility.DisableButton(Me.Page, LoadWI.ClientID)

        If LoadLotID.Text <> "" And (LoadRevID.Text <> "" Or LoadRevID.Text = "") Then
            Dim tmpLoadLotID As String
            If LoadLotID.Text.Contains("-") Then
                tmpLoadLotID = LoadLotID.Text.Substring(0, 4)
            Else
                tmpLoadLotID = LoadLotID.Text
            End If

            Dim curDir As String = GetFileDir(tmpLoadLotID)
            LoadHTMLFile(curDir)
        End If

        SatiUtility.EnableButton(Me.Page, LoadWI.ClientID)
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "RAP", "resetAfterPost()", True)
    End Sub

    'Function that returns the file directory of where the html files are located
    Protected Function GetFileDir(lot As String) As String
        Dim rootWI As String = Session("WI_View").ToString
        Dim lotFolder As String = Server.MapPath(rootWI.Substring(rootWI.IndexOf("/WI/")) & "WI_HTML_Files/" & lot.Substring(0, 2) & "00/")

        Return lotFolder + lot + "\"
    End Function

    'Function that returns the file directory of where the images for the html files are located
    Protected Function GetImagDir(lot As String) As String
        Dim rootWI As String = Session("WI_View").ToString
        Dim lotFolder As String = Server.MapPath(rootWI.Substring(rootWI.IndexOf("/WI/")) & "Images/" + lot.Substring(0, 2) + "00/")

        Return lotFolder + lot + "\"
    End Function

    'Subrountine that loads the files to be displayed to the user.
    Protected Sub LoadHTMLFile(dir As String)
        If Directory.Exists(dir) Then
            BuildTabs(dir)
        Else
            messageUpdater("Cannot open Work Instruction. The directory and files does Not exist.", True)
        End If
    End Sub






    '|=============================================================================================| 
    '|                       _   _       _               _   ___ _ _                               |
    '|                      | | | |_ __ | | ___  __ _ __| | | __(_) |___ ___                       |
    '|                      | |_| | '_ \| |/ _ \/ _` / _` | | _|| | / -_|_-<                       |
    '|                       \___/| .__/|_|\___/\__,_\__,_| |_| |_|_\___/__/                       |
    '|                            |_|                                                              |
    '|=============================================================================================|
    '|   AUTHOR: Aaron Williams                                                DATE: 10/22/2021    |
    '|   This section of the code behind file controls the functions for uploading the html files  |
    '|   from the orginal excel file on the document server to be saved and convered into an html, |
    '|   which will be loaded into an iframe for file Manipulation                                 |
    '|=============================================================================================|

    'Subrountine that listens for the users input from js
    Protected Sub HiddenUploadWI_Click(sender As Object, e As EventArgs) Handles HiddenUploadWI.Click
        SatiUtility.DisableButton(Me.Page, UploadWI.ClientID)

        If UpLoadLotID.Text <> "" And UpLoadRevID.Text <> "" Then
            Dim tmpUpLoadLotID As String
            If UpLoadLotID.Text.Contains("-") Then
                tmpUpLoadLotID = UpLoadLotID.Text.Substring(0, 4)
            Else
                tmpUpLoadLotID = UpLoadLotID.Text
            End If

            Dim pathDir As String = SetFilesPath(tmpUpLoadLotID)
            Dim pathImg As String = SetImagePath(tmpUpLoadLotID)
            Dim pathExl As String = SetExcelPath(tmpUpLoadLotID, UpLoadRevID.Text)
            Dim exHTML As List(Of String) = SetExHTML(tmpUpLoadLotID, UpLoadRevID.Text, pathDir, pathImg, pathExl)

            SaveHTMLFile(pathDir, tmpUpLoadLotID, UpLoadRevID.Text, exHTML)
        Else
            messageUpdater("Failed To upload. User information Not filled In.", True)
        End If

        SatiUtility.EnableButton(Me.Page, UploadWI.ClientID)
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "RAP", "resetAfterPost()", True)
    End Sub

    'Function that builds the file directory and returns the path for the file directory
    Protected Function SetFilesPath(lot As String) As String
        Dim rootWI As String = Session("WI_View").ToString
        Dim lotFolder As String = Server.MapPath(rootWI.Substring(rootWI.IndexOf("/WI/")) & "WI_HTML_Files/" + lot.Substring(0, 2) + "00/")

        Dim returnPath As String
        If Directory.Exists(lotFolder) Then
            If Directory.Exists(lotFolder + lot + "\") Then
                returnPath = lotFolder + lot + "\"
            Else
                Directory.CreateDirectory(lotFolder + lot + "\")
                Directory.CreateDirectory(lotFolder + lot + "\Archive\")
                returnPath = lotFolder + lot + "\"
            End If
        Else
            Directory.CreateDirectory(lotFolder)
            Directory.CreateDirectory(lotFolder + lot + "\")
            Directory.CreateDirectory(lotFolder + lot + "\Archive\")
            returnPath = lotFolder + lot + "\"
        End If

        Return returnPath
    End Function

    'Function that builds the image directory and returns the path for the image directory 
    Protected Function SetImagePath(lot As String) As String
        Dim rootWI As String = Session("WI_View").ToString
        Dim lotFolder As String = Server.MapPath(rootWI.Substring(rootWI.IndexOf("/WI/")) & "Images/" + lot.Substring(0, 2) + "00/")

        Dim returnPath As String
        If Directory.Exists(lotFolder) Then
            If Directory.Exists(lotFolder + lot + "\") Then
                returnPath = lotFolder + lot + "\"
            Else
                Directory.CreateDirectory(lotFolder + lot + "\")
                returnPath = lotFolder + lot + "\"
            End If
        Else
            Directory.CreateDirectory(lotFolder)
            Directory.CreateDirectory(lotFolder + lot + "\")
            returnPath = lotFolder + lot + "\"
        End If

        Return returnPath
    End Function

    'Function that returns the path to the desired excel file
    Protected Function SetExcelPath(lot As String, rev As String) As String
        Dim dir As String = lot.Substring(0, 2) + "00\"
        Return "\\pwi-40\docshare\Controlled Work Instructions\" + dir + lot + rev.ToLower() + ".xls"
    End Function

    'Function that takes in the excel information and returns it as a useable list
    Protected Function SetExHTML(lot As String, rev As String, dirPath As String, imgPath As String, excPath As String) As List(Of String)
        Dim reList As List(Of String) = New List(Of String)
        Dim Flex As New FlexCel.XlsAdapter.XlsFile(True)
        Dim flexHTML As FlexCel.Render.FlexCelHtmlExport

        Try
            Flex.Open(excPath)

            flexHTML = New FlexCel.Render.FlexCelHtmlExport(Flex, True)
            flexHTML.Export(dirPath + lot + rev.ToUpper() + ".html", imgPath)
            flexHTML.Dispose()
        Catch ex As Exception
            If flexHTML IsNot Nothing Then
                flexHTML.Dispose()
            End If

            File.Delete(dirPath + lot + rev.ToUpper() + ".html")
        End Try

        Dim exHTML As String() = IO.File.ReadAllLines(dirPath + lot + rev.ToLower() + ".html")
        Dim tempMerg As Boolean = True
        Dim failSafe As Integer = 0
        For i As Integer = 0 To exHTML.Length - 1
            If failSafe = 3 Then
                tempMerg = False
            End If

            If tempMerg = True Then
                If exHTML(i).Contains("</head>") Then
                    exHTML(i) = exHTML(i).Replace("</head>", "<link rel=""stylesheet"" href=""/WorkInstructionLayout.css"" /></head>")
                    reList.Add(exHTML(i))
                ElseIf exHTML(i).Contains("<body>") Then
                    Dim tempBody As String = getTemplateBodyText()
                    tempBody = tempBody.Replace("----", lot)
                    tempBody = tempBody.Replace(" -- ", rev)

                    If lot.StartsWith("2") Then
                        tempBody = tempBody.Replace("--- mm", "200 mm")
                    ElseIf lot.StartsWith("3") Then
                        tempBody = tempBody.Replace("--- mm", "300 mm")
                    End If

                    exHTML(i) = exHTML(i).Replace("<body>", tempBody)
                    reList.Add(exHTML(i))
                ElseIf exHTML(i).Contains("<table ") Or exHTML.Contains("<colgroup") Or exHTML(i).Contains("<col") Then
                    exHTML(i) = ""
                ElseIf exHTML(i).Contains("</colgroup>") Then
                    exHTML(i) = ""
                    tempMerg = False
                ElseIf exHTML(i).Contains("<tr") Then
                    exHTML(i) = ""
                ElseIf exHTML(i).Contains("<td") Or exHTML(i).Contains("</td") Then
                    exHTML(i) = ""
                ElseIf exHTML(i).Contains("</tr") Then
                    exHTML(i) = ""
                    failSafe += 1
                Else
                    reList.Add(exHTML(i))
                End If
            ElseIf tempMerg = False Then
                If exHTML(i).Contains("<td") Then
                    exHTML(i) = exHTML(i).Replace("<td", "<td onmousedown=""passClick(this);"" onmousemove=""passMove(this)"" onmouseup=""passUnclick(this);""")

                    If exHTML(i).Contains("&micro;") Then
                        exHTML(i) = exHTML(i).Replace("&micro;", "&mu;")
                    End If

                    If exHTML(i).Contains("<img ") Then
                        Dim newImgTag As String = GetNewImageTag(exHTML(i))
                        exHTML(i) = exHTML(i).Replace(exHTML(i), newImgTag)
                    End If

                    reList.Add(exHTML(i))
                ElseIf exHTML(i).Contains("</table") Then
                    If exHTML(i + 1).Contains("</body") Then
                        reList.Add("</tbody>")
                        reList.Add(exHTML(i))
                    End If
                ElseIf exHTML(i).Contains("</body") Then
                    reList.Add("<div id=""mainExTBodyEnd"" style=""display: none;""></div>")
                    reList.Add("<script src=""/scripts/WIScripts/ExcelEditorFrameControls.js"" type=""text/javascript""></script>")
                    reList.Add(exHTML(i))
                ElseIf exHTML(i).Contains("<img ") Then
                    Dim newImgTag As String = GetNewImageTag(exHTML(i))
                    exHTML(i) = exHTML(i).Replace(exHTML(i), newImgTag)
                    reList.Add(exHTML(i))
                Else
                    reList.Add(exHTML(i))
                End If
            End If
        Next

        File.Delete(dirPath + lot + rev.ToUpper() + ".html")
        Return reList
    End Function

    'Funciton that returns the permade top part of the document
    Protected Function getTemplateBodyText() As String
        Return "<body>" & vbCrLf &
"        <table id=""ExcelTableTitle"" class=""excelTable"" style=""width:100%; border-bottom: none;"" draggable=""false"" contenteditable=""false"">" & vbCrLf &
"            <tr style=""height: 60px;"">" & vbCrLf &
"               <td style=""width:100%"">" & vbCrLf &
"                    <table style=""width:100%"">" & vbCrLf &
"                        <tr>" & vbCrLf &
"                            <td style=""width: 1%;""></td>" & vbCrLf &
"                            <td style=""width: 9%;"">" & vbCrLf &
"                                <Label ID=""MMLabel"" style=""width:100%; font-weight:bold; font-size:18px"" contenteditable=""true"">--- mm</Label>&nbsp;&nbsp;&nbsp;" & vbCrLf &
"                            </td>" & vbCrLf &
"                            <td style=""width: 50%;"">" & vbCrLf &
"                                <Label ID=""WILabel"" style=""width:100%; font-weight:bold; font-size:18px"">WORK INSTRUCTIONS</Label>" & vbCrLf &
"                            </td>" & vbCrLf &
"                            <td style=""width: 40%; height: 25px; border: 3px double black;"">" & vbCrLf &
"                                <table style=""width: 100%"">" & vbCrLf &
"                                    <tr>" & vbCrLf &
"                                        <td style=""width:25%;"">" & vbCrLf &
"                                            <Label ID=""IDTempLabel"" style=""width:100%; font-weight:bold; font-size:28px"">ID # </Label>" & vbCrLf &
"                                        </td>" & vbCrLf &
"                                        <td style=""width:25%; background-color: white;"">" & vbCrLf &
"                                            <Label ID=""IDTempExcel"" style=""width:100%; font-weight:bold; font-size:28px"" contenteditable=""true"">----</Label>" & vbCrLf &
"                                        </td>" & vbCrLf &
"                                        <td style=""width:25%;"">" & vbCrLf &
"                                            <Label ID=""RevTempLabel"" style=""width:100%; font-weight:bold; font-size:28px""> Rev # </Label>" & vbCrLf &
"                                        </td>" & vbCrLf &
"                                        <td style=""width:25%; background-color: white;"">" & vbCrLf &
"                                            <Label ID=""RevTempExcel"" style=""width:100%; font-weight:bold; font-size:28px"" contenteditable=""true""> -- </Label>" & vbCrLf &
"                                        </td>" & vbCrLf &
"                                    </tr>" & vbCrLf &
"                                </table>" & vbCrLf &
"                            </td>" & vbCrLf &
"                        </tr>" & vbCrLf &
"                    </table>" & vbCrLf &
"                </td>" & vbCrLf &
"            </tr>" & vbCrLf &
"        </table>" & vbCrLf &
"        <table id=""MainExcelTable"" class=""excelTable"" onmouseleave=""passMoveOut(); return false;"" style=""border-top: none;"" draggable=""false"">" & vbCrLf &
"            <thead>" & vbCrLf &
"                <tr>" & vbCrLf &
"                    <th class=""excelHeader"">A</th>" & vbCrLf &
"                    <th class=""excelHeader"">B</th>" & vbCrLf &
"                    <th class=""excelHeader"">C</th>" & vbCrLf &
"                    <th class=""excelHeader"">D</th>" & vbCrLf &
"                    <th class=""excelHeader"">E</th>" & vbCrLf &
"                    <th class=""excelHeader"">F</th>" & vbCrLf &
"                    <th class=""excelHeader"">G</th>" & vbCrLf &
"                    <th class=""excelHeader"">H</th>" & vbCrLf &
"                    <th class=""excelHeader"">I</th>" & vbCrLf &
"                    <th class=""excelHeader"">J</th>" & vbCrLf &
"                    <th class=""excelHeader"">K</th>" & vbCrLf &
"                    <th class=""excelHeader"">L</th>" & vbCrLf &
"                    <th class=""excelHeader"">M</th>" & vbCrLf &
"                    <th class=""excelHeader"">N</th>" & vbCrLf &
"                    <th class=""excelHeader"">O</th>" & vbCrLf &
"                    <th class=""excelHeader"">P</th>" & vbCrLf &
"                    <th class=""excelHeader"">Q</th>" & vbCrLf &
"                    <th class=""excelHeader"">R</th>" & vbCrLf &
"                    <th class=""excelHeader"">S</th>" & vbCrLf &
"                    <th class=""excelHeader"">T</th>" & vbCrLf &
"                    <th class=""excelHeader"">U</th>" & vbCrLf &
"                    <th class=""excelHeader"">V</th>" & vbCrLf &
"                    <th class=""excelHeader"">W</th>" & vbCrLf &
"                    <th class=""excelHeader"">X</th>" & vbCrLf &
"                    <th class=""excelHeader"">Y</th>" & vbCrLf &
"                    <th class=""excelHeader"">Z</th>" & vbCrLf &
"                    <th class=""excelHeader"">A</th>" & vbCrLf &
"                    <th class=""excelHeader"">B</th>" & vbCrLf &
"                    <th class=""excelHeader"">C</th>" & vbCrLf &
"                    <th class=""excelHeader"">D</th>" & vbCrLf &
"                    <th class=""excelHeader"">E</th>" & vbCrLf &
"                    <th class=""excelHeader"">F</th>" & vbCrLf &
"                    <th class=""excelHeader"">G</th>" & vbCrLf &
"                    <th class=""excelHeader"">H</th>" & vbCrLf &
"                    <th class=""excelHeader"">I</th>" & vbCrLf &
"                    <th class=""excelHeader"">J</th>" & vbCrLf &
"                    <th class=""excelHeader"">K</th>" & vbCrLf &
"                    <th class=""engineeringExcelHeader"">L</th>" & vbCrLf &
"                </tr>" & vbCrLf &
"            </thead>" & vbCrLf &
"            <tbody id=""mainExTBodyStart"">"
    End Function

    'Subroutine that saves the uploaded file as a html document
    Protected Sub SaveHTMLFile(dir As String, lot As String, rev As String, exHTML As List(Of String))
        Dim filename As String = lot + rev.ToUpper() + "_UNCERTIFIED.html"
        Dim primPath As String = dir + filename
        Dim archPath As String = primPath.Replace(filename, "Archive\" + filename)
        Dim fileWriter As StreamWriter

        Try
            If File.Exists(primPath) = False Then
                Try
                    fileWriter = New StreamWriter(primPath)
                    For Each exList In exHTML
                        If exList.Contains("{") Or exList.Contains("}") Then
                            exList = exList.Replace("{", "{{")
                            exList = exList.Replace("}", "}}")
                        End If

                        If exList.Contains("<img ") And exList.Contains("alt=""Rectangle 1""") Then
                            exList = ""
                        End If

                        fileWriter.WriteLine(exList, 0, exList.Length)
                    Next

                    fileWriter.Close()
                    BuildTabs(dir)
                    messageUpdater("Successfully converted excel file. File is listed as UNCERTIFIED files.", False)
                Catch ex As Exception
                    If fileWriter IsNot Nothing Then
                        fileWriter.Close()
                    End If
                    messageUpdater("Failed to save convertered files. Please refesh and try again. If it continues ask for support.", True)
                End Try
            Else
                Try
                    File.Copy(primPath, archPath, True)

                    fileWriter = New StreamWriter(primPath)
                    For Each exList In exHTML
                        If exList.Contains("{") Or exList.Contains("}") Then
                            exList = exList.Replace("{", "{{")
                            exList = exList.Replace("}", "}}")
                        End If

                        fileWriter.WriteLine(exList, 0, exList.Length)
                    Next

                    fileWriter.Close()
                    BuildTabs(dir)
                    messageUpdater("Successfully converted excel file. Files are listed as CERTIFIED (normally named) files.", False)
                Catch ex As Exception
                    If fileWriter IsNot Nothing Then
                        fileWriter.Close()
                    End If
                    messageUpdater("Failed to save convertered files. Please refesh and try again. If it continues ask for support.", True)
                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub

    'Function that takes in the current string containing the img tag and will return it as a new img tag to work with the system.
    Protected Function GetNewImageTag(curImg As String) As String
        Dim tmpStr As String = ""
        Dim imgSty As String = ""
        Dim styStr As String = ""
        Dim retStr As String = ""

        If curImg.Contains("style='") Then
            styStr = curImg.Substring(curImg.IndexOf("style='"))
            tmpStr = styStr.Substring(styStr.LastIndexOf("'"))

            If tmpStr.Length > 0 Then
                imgSty = styStr.Replace(tmpStr, "") + "'"
                styStr = styStr.Replace(tmpStr, " position: absolute;' ")
            End If
        ElseIf curImg.Contains("style=""") Then
            styStr = curImg.Substring(curImg.IndexOf("style='"))
            tmpStr = styStr.Substring(styStr.LastIndexOf(""""))

            If tmpStr.Length > 0 Then
                imgSty = styStr.Replace(tmpStr, "") + """"
                styStr = styStr.Replace(tmpStr, " position: absolute;' ")
            End If
        End If

        tmpStr = curImg.Substring(curImg.IndexOf("<img "))
        retStr = tmpStr.Substring(tmpStr.IndexOf(">") + 1)

        If retStr.Length > 0 Then
            tmpStr = tmpStr.Replace(retStr, "")
        End If

        tmpStr = tmpStr.Replace(imgSty, "style='width: 95%; height: 95%; position: relative;' ")
        retStr = "<div class=""imgContainer"" title=""Right click to delete"" onclick=""passSelectImage(this); return false;"" " & styStr & " "">"
        retStr += tmpStr & "</div>"

        If curImg.Contains(tmpStr) And curImg.Equals(tmpStr) <> True Then
            retStr = curImg.Replace(tmpStr, retStr)
        End If

        If retStr.Contains("src='E:/") Then
            retStr = retStr.Replace("E:/", "/")
        End If

        Return retStr
    End Function







    '|=============================================================================================| 
    '|                _____ _ _        _____         _         _     _   _                         |
    '|               |   __|_| |___   |     |___ ___|_|___ _ _| |___| |_|_|___ ___                 |
    '|               |   __| | | -_|  | | | | .'|   | | . | | | | .'|  _| | . |   |                |
    '|               |__|  |_|_|___|  |_|_|_|__,|_|_|_|  _|___|_|__,|_| |_|___|_|_|                |
    '|                                                |_|                                          |
    '|=============================================================================================|
    '|   AUTHOR: Aaron Williams                                                DATE: 10/22/2021    |
    '|   This section of the code behind file controls the functions for certifing, uncertifing,   |
    '|   saving, achiving, and deleting the files from the server. These are only avalibalbe for   |
    '|   specific people in the plant.                                                             |
    '|=============================================================================================|

    'User input button selection from client js
    Protected Sub FileManipulation_Click(sender As Object, e As EventArgs) Handles FileManipulation.Click
        If LotText.Text.ToUpper <> String.Empty And RevText.Text.ToUpper <> String.Empty Then
            Dim varDir As String

            If Directory.Exists(GetFileDir(LotText.Text.ToUpper)) Then
                varDir = GetFileDir(LotText.Text.ToUpper)
            Else
                varDir = SetFilesPath(LotText.Text.ToUpper)
            End If

            If passedTp.Value = "Cert" Then
                CertCurrentWI_Click()
            ElseIf passedTp.Value = "Name" Then
                NameCurrentWI_Click()
            ElseIf passedTp.Value = "Save" Then
                SaveCurrentWI_Click()
                clearCertFiles(varDir)
            ElseIf passedTp.Value = "Rest" Then
                RestCurrentWI_Click()
                clearCertFiles(varDir)
            End If

            BuildTabs(GetFileDir(LotText.Text.ToUpper))
        End If
    End Sub

    'Subroutine that breaks apart uncertified file into departments
    Protected Sub CertCurrentWI_Click()
        SatiUtility.DisableButton(Me.Page, "CertCurrWI")

        If LotText.Text.ToUpper <> String.Empty And RevText.Text.ToUpper <> String.Empty Then
            Dim fileDirect As String = GetFileDir(LotText.Text.ToUpper)
            Dim file_Path As String = fileDirect + LotText.Text.ToUpper + RevText.Text.ToUpper
            Dim currHD As String() = Server.UrlDecode(currWIData.Text).Replace(Chr(13), "").Split(Chr(10))

            If User.IsInRole("WICert") Then
                Dim fileWriter As StreamWriter
                Try
                    Dim htmlHead As List(Of String) = breakHead(currHD)
                    Dim htmlBody As List(Of List(Of String)) = breakBody(currHD)
                    Dim htmlFoot As List(Of String) = breakFoot(currHD)
                    Dim fileNames As List(Of String) = getAllFileNames()
                    Dim count As Integer = 0

                    For Each fileName In fileNames
                        fileWriter = New StreamWriter(fileDirect + fileName)
                        For Each line In htmlHead
                            fileWriter.WriteLine(line)
                        Next

                        For Each line In htmlBody(count)
                            If line.Contains("�") Or line.Contains("&micro;") Then
                                line = line.Replace("&micro;", "&mu;")
                                line = line.Replace("�", "&mu;")
                            End If

                            fileWriter.WriteLine(line)
                        Next

                        For Each line In htmlFoot
                            fileWriter.WriteLine(line)
                        Next
                        fileWriter.Close()

                        count += 1
                    Next

                    Try
                        fileWriter = New StreamWriter(file_Path + "_UNCERTIFIED.html")
                        For Each line In currHD
                            If line.Contains("{") Or line.Contains("}") Then
                                line = line.Replace("{", "{{")
                                line = line.Replace("}", "}}")
                            End If

                            If line.Contains("�") Or line.Contains("&micro;") Then
                                line = line.Replace("&micro;", "&mu;")
                                line = line.Replace("�", "&mu;")
                            End If

                            fileWriter.WriteLine(line, 0, line.Length)
                        Next
                        fileWriter.Close()
                        messageUpdater("Successfully certified the UNCERTIFIED file. UNCERTIFIED file was updated.", True)
                    Catch exc As Exception
                        If fileWriter IsNot Nothing Then
                            fileWriter.Close()
                        End If
                        messageUpdater("Failed To update the UNCERTIFIED file. Any changes were lost. Please refesh And Try again. If it continues ask For support.", True)
                    End Try
                Catch ex As Exception
                    If fileWriter IsNot Nothing Then
                        fileWriter.Close()
                    End If
                    messageUpdater("Failed To certify the UNCERTIFIED file. Please refesh And Try again. If it continues ask For support.", True)
                End Try
            End If
        End If

        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "RAP", "resetAfterPost()", True)
    End Sub

    'Subroutine that renames a department file
    Protected Sub NameCurrentWI_Click()
        SatiUtility.DisableButton(Me.Page, "UCerCurrWI")

        If LotText.Text.ToUpper <> String.Empty And RevText.Text.ToUpper <> String.Empty Then
            Dim cFilPat As String = GetFileDir(LotHid.Value.ToUpper) + LotHid.Value.ToUpper + RevHid.Value.ToUpper + "_" + NamHid.Value.ToUpper + ".html"
            Dim iFilPat As String = GetFileDir(LotText.Text.ToUpper) + LotText.Text.ToUpper + RevText.Text.ToUpper + "_" + NamText.Text.ToUpper + ".html"
            Dim currHD As String() = Server.UrlDecode(currWIData.Text).Replace(Chr(13), "").Split(Chr(10))

            If User.IsInRole("WIEdit") Then
                Try
                    File.Copy(cFilPat, iFilPat, True)
                    File.Delete(cFilPat)
                    messageUpdater("Successfully renamed the current file With the inputed name.", False)
                Catch ex As Exception
                    messageUpdater("Failed To rename the current file With the inputed name. Please refesh And Try again. If it continues ask For support.", True)
                End Try
            End If
        End If

        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "RAP", "resetAfterPost()", True)
    End Sub

    'Subroutine that saves the edited uncertified document
    Protected Sub SaveCurrentWI_Click()
        SatiUtility.DisableButton(Me.Page, "SaveCurrWI")

        If LotText.Text.ToUpper <> String.Empty And RevText.Text.ToUpper <> String.Empty Then
            Dim iFilPat As String = GetFileDir(LotText.Text.ToUpper) + LotText.Text.ToUpper + RevText.Text.ToUpper + "_" + NamText.Text.ToUpper + ".html"
            Dim iArcPat As String = GetFileDir(LotText.Text.ToUpper) + "Archive\" + LotText.Text.ToUpper + RevText.Text.ToUpper + "_" + NamText.Text.ToUpper + ".html"
            Dim currHD As String() = Server.UrlDecode(currWIData.Text).Replace(Chr(13), "").Split(Chr(10))

            If User.IsInRole("WIEdit") Then
                If LotHid.Value = "Lot ID" And RevHid.Value = "Rev ID" Then
                    If File.Exists(iFilPat) Then
                        If File.Exists(iArcPat) Then
                            Try
                                File.Delete(iArcPat)
                                File.Move(iFilPat, iArcPat)
                            Catch ex As Exception
                                messageUpdater("Failed To save the UNCERTIFIED file. Please refesh And Try again. If it continues ask For support.", True)
                            End Try
                        Else
                            Try
                                File.Move(iFilPat, iArcPat)
                            Catch ex As Exception
                                messageUpdater("Failed To save the UNCERTIFIED file. Please refesh And Try again. If it continues ask For support.", True)
                            End Try
                        End If
                    End If
                Else
                    Dim cFilPat As String = GetFileDir(LotHid.Value.ToUpper) + LotHid.Value.ToUpper + RevHid.Value.ToUpper + "_" + NamHid.Value.ToUpper + ".html"
                    Dim cArcPat As String = GetFileDir(LotHid.Value.ToUpper) + "Archive\" + LotHid.Value.ToUpper + RevHid.Value.ToUpper + "_" + NamHid.Value.ToUpper + ".html"

                    If File.Exists(cArcPat) Then
                        Try
                            File.Delete(cArcPat)
                            File.Move(cFilPat, cArcPat)
                        Catch ex As Exception
                            messageUpdater("Failed To save the UNCERTIFIED file. Please refesh And Try again. If it continues ask For support.", True)
                        End Try
                    Else
                        Try
                            File.Move(cFilPat, cArcPat)
                        Catch ex As Exception
                            messageUpdater("Failed To save the UNCERTIFIED file. Please refesh And Try again. If it continues ask For support.", True)
                        End Try
                    End If
                End If

                Dim fileWriter As StreamWriter
                Try
                    fileWriter = New StreamWriter(iFilPat)
                    For Each line In currHD
                        If line.Contains("{") Or line.Contains("}") Then
                            line = line.Replace("{", "{{")
                            line = line.Replace("}", "}}")
                        End If

                        If line.Contains("�") Or line.Contains("&micro;") Then
                            line = line.Replace("&micro;", "&mu;")
                            line = line.Replace("�", "&mu;")
                        End If

                        fileWriter.WriteLine(line, 0, line.Length)
                    Next
                    fileWriter.Close()
                Catch ex As Exception
                    If fileWriter IsNot Nothing Then
                        fileWriter.Close()
                    End If
                    messageUpdater("Failed To save the UNCERTIFIED file. Please refesh And Try again. If it continues ask For support.", True)
                End Try
            End If
        End If
    End Sub

    'Subroutine that restores an archived document and swaps it with the current document
    Protected Sub RestCurrentWI_Click()
        SatiUtility.DisableButton(Me.Page, "RestCurrWI")

        If LotText.Text.ToUpper <> String.Empty And RevText.Text.ToUpper <> String.Empty Then
            If LotHid.Value.ToUpper <> "Lot ID" And RevHid.Value.ToUpper <> "Rev ID" Then
                If User.IsInRole("WIEdit") Then

                    Dim cFilPat As String = GetFileDir(LotHid.Value.ToUpper) + LotHid.Value.ToUpper + RevHid.Value.ToUpper + "_" + NamHid.Value.ToUpper + ".html"
                    Dim cArcPat As String = GetLastArchName(GetFileDir(LotHid.Value.ToUpper) + "Archive\", LotHid.Value.ToUpper, RevHid.Value.ToUpper, NamHid.Value.ToUpper)
                    Dim currHD As String() = Server.UrlDecode(currWIData.Text).Replace(Chr(13), "").Split(Chr(10))

                    If File.Exists(cArcPat) Then
                        Dim fileWriter As StreamWriter
                        Try
                            File.Delete(cFilPat)
                            File.Copy(cArcPat, cFilPat, True)
                            File.Delete(cArcPat)

                            fileWriter = New StreamWriter(cArcPat)
                            For Each line In currHD
                                If line.Contains("{") Or line.Contains("}") Then
                                    line = line.Replace("{", "{{")
                                    line = line.Replace("}", "}}")
                                End If

                                If line.Contains("�") Or line.Contains("&micro;") Then
                                    line = line.Replace("&micro;", "&mu;")
                                    line = line.Replace("�", "&mu;")
                                End If

                                fileWriter.WriteLine(line, 0, line.Length)
                            Next
                            fileWriter.Close()
                            messageUpdater("Successfully restored archived file. Saved As a UNCERTIFIED file. Old UNCERTIFIED file And any changes was archived.", False)
                        Catch ex As Exception
                            Try
                                fileWriter.Close()
                                Dim tempFile As String = cFilPat.Replace(NamHid.Value.ToUpper + ".html", "UNCERT_TEMP.html")
                                fileWriter = New StreamWriter(tempFile)
                                For Each line In currHD
                                    If line.Contains("{") Or line.Contains("}") Then
                                        line = line.Replace("{", "{{")
                                        line = line.Replace("}", "}}")
                                    End If

                                    If line.Contains("�") Or line.Contains("&micro;") Then
                                        line = line.Replace("&micro;", "&mu;")
                                        line = line.Replace("�", "&mu;")
                                    End If

                                    fileWriter.WriteLine(line, 0, line.Length)
                                Next
                                fileWriter.Close()
                                messageUpdater("Failed To restored Archived file. Save old UNCERTIFIED And any changes As UNCERT-TEMP. This will be deleted later.", True)
                            Catch exc As Exception
                                If fileWriter IsNot Nothing Then
                                    fileWriter.Close()
                                End If
                                messageUpdater(ex.ToString, True)
                                messageUpdater("Failed To restore archived file. Any changes were lost. Please refesh And Try again. If it continues ask For support.", True)
                            End Try
                        End Try
                    Else
                        Dim fileWriter As StreamWriter
                        Try
                            Dim tempFile As String = cArcPat.Replace(NamHid.Value.ToUpper + ".html", "UNCERT_TEMP.html")
                            fileWriter = New StreamWriter(tempFile)
                            For Each line In currHD
                                If line.Contains("{") Or line.Contains("}") Then
                                    line = line.Replace("{", "{{")
                                    line = line.Replace("}", "}}")
                                End If

                                If line.Contains("�") Or line.Contains("&micro;") Then
                                    line = line.Replace("&micro;", "&mu;")
                                    line = line.Replace("�", "&mu;")
                                End If

                                fileWriter.WriteLine(line, 0, line.Length)
                            Next
                            fileWriter.Close()
                            messageUpdater("No Archive file exist. Saved UNCERTIFIED document And any changes As UNCERT-TEMP. UNCERT-TEMP will be deleted later.", True)
                        Catch exc As Exception
                            If fileWriter IsNot Nothing Then
                                fileWriter.Close()
                            End If
                            messageUpdater("No Archive file exist. Failed To save a temporary file. Any changes were lost. Please refesh And Try again. If it continues ask For support.", True)
                        End Try
                    End If
                End If
            End If
        End If

        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "RAP", "resetAfterPost()", True)
    End Sub

    'Helper Function to break apart the head portion of the edited html document for certification
    Protected Function breakHead(doc As String()) As List(Of String)
        Dim reHead As List(Of String) = New List(Of String)

        For Each line In doc
            If line.Contains("id=""mainExTBodyStart""") Then
                reHead.Add(line)
                Exit For
            Else
                reHead.Add(line)
            End If
        Next

        Return reHead
    End Function

    'Helper Function to break apart the body portion of the edited html document for certification
    Protected Function breakBody(doc As String()) As List(Of List(Of String))
        Dim reBody As List(Of List(Of String)) = New List(Of List(Of String))
        Dim tempList As List(Of String) = New List(Of String)
        Dim departNames As List(Of String) = getDepartNames()

        Dim exLock As Boolean = False
        Dim mnLock As Boolean = True
        Dim fnLock As Boolean = False
        Dim countLock As Integer = 0
        Dim fileCount As Integer = 1

        For i As Integer = 0 To doc.Length - 1
            If doc(i).Contains("id=""mainExTBodyStart""") Then
                exLock = True
            ElseIf exLock = True Then
                If mnLock = True Then
                    If countLock <= 5 Then
                        If doc(i).Contains("<td") Then
                            Dim tmpDoc As String = doc(i)
                            tmpDoc = tmpDoc.Substring(tmpDoc.IndexOf(">") + 1)
                            Dim tmpReDoc As String = tmpDoc
                            tmpReDoc = tmpReDoc.Substring(tmpReDoc.IndexOf("<"))
                            tmpDoc = tmpDoc.Replace(tmpReDoc, "")

                            If tmpDoc.Contains("&amp;") Then
                                tmpDoc = tmpDoc.Replace("&amp;", "&")
                            End If

                            For Each name In departNames
                                If tmpDoc.ToUpper = name.ToUpper Then 'doc(i).Contains(name.ToUpper) Then
                                    setAllFileNames(name)
                                    mnLock = False
                                    fnLock = True
                                    countLock = 0
                                    Exit For
                                End If
                            Next
                        ElseIf doc(i).Contains("</tr") Then
                            countLock += 1
                            If countLock = 4 Then
                                If fnLock = False Then
                                    setAllFileNames("Unknown_Depart_" + fileCount.ToString)
                                    mnLock = False
                                    fnLock = False
                                    countLock = 0
                                    fileCount += 1
                                End If
                            End If
                        End If
                    End If
                Else
                    If doc(i).Contains(" --- PAGE-BREAK --- --- THIS-WAS-LEFT-BLANK-ON-PURPOSE --- ") Then
                        mnLock = True
                        fnLock = False
                        i -= 1
                    End If
                End If
                If doc(i).Contains("<tbody") Then
                    If doc(i + 1).Contains("</table") Then
                        If doc(i + 2).Contains("id=""mainExTBodyEnd""") Then
                            Exit For
                        End If
                    End If
                End If
            End If
        Next

        exLock = False
        mnLock = False

        For i As Integer = 0 To doc.Length - 1
            If doc(i).Contains("id=""mainExTBodyStart""") Then
                exLock = True
            ElseIf exLock = True Then
                If doc(i).Contains(" --- PAGE-BREAK --- --- THIS-WAS-LEFT-BLANK-ON-PURPOSE --- ") Then
                    mnLock = True
                End If

                If mnLock = True Then
                    Dim breakStr = getBreakPageInfo()
                    Dim sTempStr = doc(i).Substring(0, doc(i).IndexOf(breakStr))
                    Dim eTempStr = doc(i).Substring(doc(i).IndexOf(breakStr) + breakStr.Length)

                    tempList.Add(sTempStr)
                    tempList.Add(breakStr)
                    reBody.Add(tempList)

                    tempList = New List(Of String)
                    tempList.Add(eTempStr)
                    mnLock = False
                Else
                    If doc(i).Contains("</tbody") Then
                        If doc(i + 1).Contains("</table") Then
                            If doc(i + 2).Contains("id=""mainExTBodyEnd""") Then
                                tempList.Add(doc(i))
                                reBody.Add(tempList)
                                Exit For
                            Else
                                tempList.Add(doc(i))
                            End If
                        Else
                            tempList.Add(doc(i))
                        End If
                    Else
                        tempList.Add(doc(i))
                    End If
                End If
            End If
        Next

        Return reBody
    End Function

    'Helper Function to break apart the foot portion of the edited html document for certification
    Protected Function breakFoot(doc As String()) As List(Of String)
        Dim reFoot As List(Of String) = New List(Of String)
        Dim lock As Boolean = True

        For i As Integer = 0 To doc.Length - 1
            If doc(i).Contains("</tbody") Then
                If doc(i + 1).Contains("</table") Then
                    If doc(i + 2).Contains("id=""mainExTBodyEnd""") Then
                        lock = False
                    End If
                End If
            End If

            If lock = False Then
                reFoot.Add(doc(i))
            End If
        Next

        Return reFoot
    End Function

    'Heler Function to get the correct Rev letter
    Protected Function GetLastArchName(dir As String, lot As String, rev As String, nam As String) As String
        rev = rev.ToUpper()
        Dim roll As Boolean = False
        Dim charRev As List(Of Integer) = New List(Of Integer)

        For i As Integer = 0 To rev.Length - 1
            charRev.Add(Convert.ToInt32(rev(i)))
        Next

        For i As Integer = rev.Length - 1 To 0 Step -1
            If i <> 0 And charRev(i) = 65 And roll = False Then
                charRev.Remove(charRev(i))
            ElseIf roll = False Then
                charRev(i) -= 1
                roll = True
            End If

            If i = 0 And charRev(0) = 64 Then
                charRev(i) = 65
            End If
        Next
        Dim build As StringBuilder = New StringBuilder()
        For Each c In charRev
            build.Append(Chr(c))
        Next
        Return dir + lot + build.ToString() + "_" + nam + ".html"
    End Function

    'Subroutine that cleans the certified files when the main Uncertified file is changed
    Protected Sub clearCertFiles(curDir As String)
        For Each curfil As String In Directory.GetFiles(curDir)
            Try
                If curfil.Contains("_UNCERTIFIED") = False Then
                    File.Delete(curfil)
                End If
            Catch ex As Exception
                messageUpdater("Failed To remove older certified files", True)
            End Try
        Next
    End Sub





    '|=============================================================================================| 
    '|                   _   _ _   _ _ _ _          __  __     _   _            _                  |
    '|                  | | | | |_(_) (_) |_ _  _  |  \/  |___| |_| |_  ___  __| |___              |
    '|                  | |_| |  _| | | |  _| || | | |\/| / -_)  _| ' \/ _ \/ _` (_-<              |
    '|                   \___/ \__|_|_|_|\__|\_, | |_|  |_\___|\__|_||_\___/\__,_/__/              |
    '|                                       |__/                                                  |
    '|=============================================================================================|
    '|   AUTHOR: Aaron Williams                                                DATE: 10/22/2021    |
    '|   This section of the code behind file are utility methods that help aid other functions    |
    '|   These have no really grouping since they all help different function, hence why theses    |
    '|   are just utilities                                                                        |
    '|=============================================================================================|

    'Subroutine resets the unser inputs after postback
    Protected Sub ResetUpdateInputs()
        LotText.Attributes.Add("placeholder", "Lot ID Number")
        LotText.Attributes.Add("background-color", "white")
        RevText.Attributes.Add("Placeholder", "Rev ID Letter(s)Then")
        RevText.Attributes.Add("background-color", "white")
    End Sub

    'Subroutine that prints the system response to the user
    Protected Sub messageUpdater(message As String, colored As Boolean)
        UpdateMessage.Text = message

        If colored Then
            UpdateMessage.ForeColor = Drawing.Color.DarkRed
        Else
            UpdateMessage.ForeColor = Drawing.Color.Black
        End If
    End Sub

    'Subroutine that builds the edit tabs
    Protected Sub BuildTabs(folderName As String)
        Dim counter As Integer = 1
        Dim curTab As String = ""

        If Directory.GetFiles(folderName).Length > 0 Then
            For Each curFil As String In Directory.GetFiles(folderName)
                Dim newButs As HtmlGenericControl = New HtmlGenericControl("button")
                Dim newDivs As HtmlGenericControl = New HtmlGenericControl("div")
                Dim newFrames As HtmlGenericControl = New HtmlGenericControl("iframe")
                Dim fileName As String = curFil.Substring(curFil.LastIndexOf("\") + 1)
                fileName = fileName.Substring(0, fileName.LastIndexOf("."))

                Dim WI As String = "WI " & counter
                Dim WF As String = "WF_" & counter
                Dim WC As String = "WC_" & counter
                Dim WB As String = "WB_" & counter

                newButs.Attributes.Add("type", "button")
                newButs.Attributes.Add("id", WB)

                If counter = 1 Then
                    newButs.Attributes.Add("class", "tablinks active")
                    curTab = fileName
                Else
                    newButs.Attributes.Add("class", "tablinks")
                    If counter = 1 Then
                    End If
                End If

                newButs.Attributes.Add("value", WF)
                newButs.Attributes.Add("title", fileName)
                newButs.Attributes.Add("onclick", "openWI(event, '" & WC & "'); return false;")
                newButs.InnerHtml = fileName.Substring(fileName.IndexOf("_") + 1)

                newDivs.Attributes.Add("id", WC)
                newDivs.Attributes.Add("class", "tabcontent")
                If counter = 1 Then
                    newDivs.Attributes.Add("style", "display: block")
                Else
                    newDivs.Attributes.Add("style", "display: none")
                End If

                newFrames.Attributes.Add("src", buildFrameSrc(curFil))
                newFrames.Attributes.Add("id", WF)
                newFrames.Attributes.Add("name", WF)
                newFrames.Attributes.Add("title", WI)
                newFrames.Attributes.Add("style", "width: 950px; height: 100%; border: none;")

                If curFil.Contains("_UNCERTIFIED") Then
                    newFrames.Attributes.Add("onload", "document.getElementById('" & WF & "').contentWindow.enableControl(true)")
                Else
                    newFrames.Attributes.Add("onload", "document.getElementById('" & WF & "').contentWindow.enableControl(false)")
                End If

                newDivs.Controls.Add(newFrames)
                Me.WorkInstructionHolder.Controls.Add(newDivs)
                tabHolder.Controls.Add(newButs)

                If counter = 1 Then
                    LotLabel.InnerText = fileName.Substring(0, 4)

                    Dim tempRevLabel As String = fileName.Substring(4)
                    Dim tempRevEnd As String = fileName.Substring(fileName.IndexOf("_"))
                    tempRevLabel = tempRevLabel.Replace(tempRevEnd, "")

                    RevLabel.InnerText = tempRevLabel
                    NamLabel.InnerText = fileName.Substring(fileName.IndexOf("_") + 1)
                End If

                counter += 1
            Next
            messageUpdater("NOTE: Hover over editing options to learn more. When editing text - Please select it by highlighting it.", False)
            resetHiddenFields(curTab)
        Else
            messageUpdater("Cannot open Work Instruction. The file does not exist.", True)
        End If

        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ABAP", "adjustButtonAfterPost()", True)
    End Sub

    'Function that returns the list of department names (MIGHT NEED UPDATING)
    Protected Function getDepartNames() As List(Of String)
        Dim departNames As New List(Of String)(
                    {"Backside Laser Mark",
                     "CMP",
                     "DSP",
                     "DSP Clean",
                     "DSP Visual Inspection",
                     "Final ADE",
                     "Final Clean",
                     "Final E&H / LEO",
                     "Incoming Inspection",
                     "Incoming Inspection / T7",
                     "Incoming Visual / Scribe Visual",
                     "Lapping",
                     "Laser Inspection",
                     "Laser Mark",
                     "Metal Max #1",
                     "Metal Max #2",
                     "Packaging",
                     "Polish",
                     "Presort",
                     "PWC",
                     "Quality Control",
                     "Scrubbing",
                     "Strip & Etch",
                     "Visual Inspection",
                     "Laser Mark",
                     "Final E&H",
                     "LEO",
                     "Incoming Visual",
                     "Incoming Visual / T7",
                     "Type Sort"})

        Return departNames
    End Function

    'Function that returns the string that contains the page break information in html
    Protected Function getBreakPageInfo() As String
        Return "<tr class=""excelRow"" contenteditable=""false"" style=""height: 10px;""><td class=""pageBreak"" ondblclick=""passPageBreak(this);"" title="" --- HOLD CONTROL AND DOUBLE CLICK TO REMOVE --- "" colspan=""38"" style=""background: black; text-align: center;""> --- PAGE-BREAK --- --- THIS-WAS-LEFT-BLANK-ON-PURPOSE --- </td></tr>"
    End Function

    'Subroutine that sets the passed file name into a list
    Protected Sub setAllFileNames(fileName As String)
        If fileName.Contains("/") Then
            fileName = fileName.Replace("/", "-")
        End If

        If AllFileNames Is Nothing Then
            AllFileNames = New List(Of String)
            AllFileNames.Add(LotText.Text.ToUpper + RevText.Text.ToUpper + "_" + fileName.ToUpper + ".html")
        Else
            AllFileNames.Add(LotText.Text.ToUpper + RevText.Text.ToUpper + "_" + fileName.ToUpper + ".html")
        End If
    End Sub

    'Function that returns all the file names found in the file.
    Protected Function getAllFileNames() As List(Of String)
        Return AllFileNames
    End Function

    'Subroutine that sets the hidden labels for backend file manipluation
    Protected Sub resetHiddenFields(curTabName As String)
        Dim curName As String() = curTabName.Split("_")
        Dim curLot As String = curName(0).Substring(0, 4)
        Dim curRev As String = curName(0).Substring(4)

        LotHid.Value = curLot
        RevHid.Value = curRev
        NamHid.Value = curName(1)
        currWIData.Text = ""
    End Sub

    'function that updates the physical path with the virtual path
    Protected Function buildFrameSrc(curSrc As String) As String
        curSrc = curSrc.Substring(curSrc.IndexOf("\WI\"))
        curSrc = curSrc.Replace("\", "/")

        Return curSrc
    End Function
End Class