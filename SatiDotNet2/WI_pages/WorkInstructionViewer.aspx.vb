
Imports System.IO

Partial Class WI_WorkInstructionViewer
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.CheckPageAuthenication(Page, User, Server)

        If Not Me.IsPostBack Then
            Dim QStr_Lot As String = Server.UrlDecode(Request.QueryString("LotID"))
            Dim QStr_Dep As String = Server.UrlDecode(Request.QueryString("Depart"))

            If QStr_Lot <> "" Then
                LotID.Text = QStr_Lot

                If QStr_Dep <> "" Then
                    loadingSetup(QStr_Lot, QStr_Dep)
                Else
                    QStr_Dep = "NONE"
                    loadingSetup(QStr_Lot, QStr_Dep)
                End If
            End If
        End If
    End Sub

    Protected Sub LoadWI_Click(sender As Object, e As EventArgs) Handles LoadWI.Click
        SatiUtility.DisableButton(Me.Page, LoadWI.ClientID)

        If LotID.Text <> String.Empty Then
            If LotID.Text.Length > 3 Then
                LotID.Attributes.Add("placeholder", "Lot ID Number")
                LotID.BackColor = Drawing.Color.White

                Dim curLot As String
                If LotID.Text.Contains("-") Then
                    curLot = LotID.Text.Substring(0, 4)
                    SearchRogue.Value = False
                Else
                    curLot = LotID.Text
                End If

                If SearchRogue.Value = True Then
                    SearchForDDL(curLot)
                    SearchRogue.Value = False
                Else
                    If DepartDropDown.SelectedValue <> "Department" Then
                        loadingSetup(curLot, DepartDropDown.SelectedValue)
                    Else
                        loadingSetup(curLot, "NONE")
                    End If
                End If
            Else
                LotID.BackColor = Drawing.Color.FromArgb(255, 197, 197)
            End If
        Else
            LotID.Attributes.Add("Placeholder", "REQUIRED")
            LotID.BackColor = Drawing.Color.FromArgb(5, 197, 197)
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "RAP", "resetAfterPost()", True)
        SatiUtility.EnableButton(Me.Page, LoadWI.ClientID)
    End Sub

    Protected Sub SearchForDDL(curLot As String)
        Dim folderName As String

        If LotID.Text <> "" Then
            If LotID.Text.Length > 3 Then
                folderName = getFileDirectory(curLot)
                folderName = folderName.Substring(folderName.IndexOf("/WI/"))
                folderName = Server.MapPath(folderName)

                If Directory.Exists(folderName) Then
                    If DepartDropDown.Items.Count > 1 Then
                        DepartDropDown.Items.Clear()
                        DepartDropDown.Items.Add("Department")
                    End If

                    For Each file As String In Directory.GetFiles(folderName)
                        Dim fileName As String = file.Substring(file.LastIndexOf("\") + 1)
                        fileName = fileName.Substring(0, fileName.LastIndexOf("."))

                        If file.Contains("_UNCERTIFIED") = False Then
                            Dim tempName As String = fileName.Substring(fileName.IndexOf("_") + 1)

                            DepartDropDown.Items.Add(tempName)
                        End If
                    Next
                End If
            Else
                LotID.BackColor = Drawing.Color.FromArgb(255, 197, 197)
            End If
        Else
            LotID.BackColor = Drawing.Color.FromArgb(255, 197, 197)
        End If

    End Sub

    Protected Sub loadingSetup(lot As String, dep As String)
        Dim file_Dict As String = getFileDirectory(lot)
        file_Dict = file_Dict.Substring(file_Dict.IndexOf("/WI/"))
        file_Dict = Server.MapPath(file_Dict)

        If dep.Contains("-") Then
            dep = dep.Replace("-", " ")
        End If

        Try
            If Directory.Exists(file_Dict) Then
                NotFoundPanel.Visible = False
                BuildTabs(file_Dict, dep)
            Else
                NotFoundPanel.Visible = True
                If FoundLabel.Text.Contains("ERROR") Then
                    FoundLabel.ForeColor = Drawing.Color.DarkRed
                    FoundLabel.Text = "NOTE: THE FILES YOU ARE LOOKING FOR WERE NOT FOUND. PLEASE CHECK YOUR INPUTS"
                End If
            End If
        Catch ex As Exception
            NotFoundPanel.Visible = True
            FoundLabel.ForeColor = Drawing.Color.Red
            FoundLabel.Text = "ERROR: THERE WAS A FAILT IN THE PROGRAM. TRY AGAIN. OTHERWISE CONTACT SITE ADMIN."
        End Try
    End Sub

    Protected Function getFileDirectory(LotNum As String) As String
        Dim currPath As String = Session("WI_View").ToString + "WI_HTML_Files/"
        Dim lotFolder As String = LotNum.Substring(0, 2) + "00/"

        Return currPath + lotFolder + LotNum + "/"
    End Function

    Protected Sub BuildTabs(folderName As String, srcName As String)
        Dim counter As Integer = 0
        Dim opened As Integer = 0

        Dim opCount As Integer = 0
        For Each file As String In Directory.GetFiles(folderName)
            Dim fileName As String = file.Substring(file.LastIndexOf("_") + 1)
            fileName = fileName.Substring(0, fileName.LastIndexOf("."))

            If file.Contains("_UNCERTIFIED") = False Then
                If fileName.Equals(srcName.ToUpper()) Then
                    opened = opCount
                End If

                opCount += 1
            End If
        Next

        For Each file As String In Directory.GetFiles(folderName)
            If file.Contains("_UNCERTIFIED") = False Then
                Dim newButs As HtmlGenericControl = New HtmlGenericControl("button")
                Dim newDivs As HtmlGenericControl = New HtmlGenericControl("div")
                Dim newFrames As HtmlGenericControl = New HtmlGenericControl("iframe")
                Dim fileName As String = file.Substring(file.LastIndexOf("_") + 1)
                fileName = fileName.Substring(0, fileName.LastIndexOf("."))

                Dim WI As String = "WI " & counter
                Dim WF As String = "WF_" & counter
                Dim WC As String = "WC_" & counter
                Dim WB As String = "WB_" & counter

                newButs.Attributes.Add("type", "button")
                If counter = opened Then
                    newButs.Attributes.Add("class", "tablinks active")
                Else
                    newButs.Attributes.Add("class", "tablinks")
                End If
                newButs.Attributes.Add("id", WB)
                newButs.Attributes.Add("value", WF)
                newButs.Attributes.Add("title", fileName)
                newButs.Attributes.Add("onclick", "openWI(event, '" & WC & "'); return false;")
                newButs.InnerHtml = fileName

                newDivs.Attributes.Add("id", WC)
                newDivs.Attributes.Add("class", "tabcontent")
                If counter = opened Then
                    newDivs.Attributes.Add("style", "display: block")
                Else
                    newDivs.Attributes.Add("style", "display: none")
                End If

                newFrames.Attributes.Add("src", buildFrameSrc(file))
                newFrames.Attributes.Add("id", WF)
                newFrames.Attributes.Add("namem", WF)
                newFrames.Attributes.Add("title", WI)
                newFrames.Attributes.Add("style", "width: 950px; height: 100%; border: none;")
                newFrames.Attributes.Add("onload", "document.getElementById('" & WF & "').contentWindow.enableControl(false)")

                newDivs.Controls.Add(newFrames)
                WorkIntructionHolder.Controls.Add(newDivs)
                tabHolder.Controls.Add(newButs)

                counter += 1
            End If
        Next
    End Sub

    'function that updates the physical path with the virtual path
    Protected Function buildFrameSrc(curSrc As String) As String
        curSrc = curSrc.Substring(curSrc.IndexOf("\WI\"))
        curSrc = curSrc.Replace("\", "/")

        Return curSrc
    End Function
End Class
