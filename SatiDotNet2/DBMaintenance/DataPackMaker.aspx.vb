Imports Class1
Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.IO
Imports System.Diagnostics

Partial Public Class DBMaintenance_DataPackMaker
    'Public partial Class FileBrowser : Inherits System.Web.UI.Page
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MenuAuthenication.AuthenicationByPass(Page)

        If (Not Page.IsPostBack) Then
            ShowDirectoryContents(Server.MapPath("."))
        End If
    End Sub

    Sub Message(ByVal text As String)
        Dim strMessage As String
        strMessage = "Connection is Created"
        'finishes server processing, returns to client.
        Dim strScript As String = "<script language=JavaScript>"
        strScript += "alert(""" & text & """);"
        strScript += "</script"

        If (Not ClientScript.IsStartupScriptRegistered("clientScript")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "clientScript", strScript)
        End If

    End Sub

    Protected Sub Button7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button7.Click

        Dim Flex As New FlexCel.XlsAdapter.XlsFile(True)
        Dim Path As String

        'Path = "\\PWI-40\software$\LabelTemplates\LabelArchive\CofA Files\" & Me.FileNameTextBox0.Text & ".xls"
        Path = Me.FileNameTextBox0.Text
        Try
            Flex.Open(Path)
        Catch ex As Exception
            Me.FileNameTextBox0.Text = ("Bad File Name")
        End Try

        If Me.RadioButtonIBM.Checked = True Then
            Try
                SatiCode.MakeIBMDataPack(Flex, Me.TextBoxSeqNumber.Text, Me.CheckBoxNewOrOldIBM.Checked)
                Me.FileNameTextBox0.Text = ("Check IBM Data Packs Folder For Your File")
                Me.TextBoxSeqNumber.Text = Me.TextBoxSeqNumber.Text + 1
            Catch ex As Exception
                Me.FileNameTextBox0.Text = ("Data Pack Fail Error")
            End Try
        End If

        If Me.RadioButtonGF.Checked = True Then
            Try
                SatiCode.MakeGFDataPack(Flex)
                Me.FileNameTextBox0.Text = ("Check GF Data Packs Folder For Your File")
            Catch ex As Exception
                Me.FileNameTextBox0.Text = ("Data Pack Fail Error")
            End Try
        End If

        If Me.RadioButtonOnSemi200mm.Checked = True Then
            Try
                SatiCode.MakeOnSemiDataPack(Flex)
                Me.FileNameTextBox0.Text = ("Check OnSemi Data Packs Folder For Your File")
            Catch ex As Exception
                Me.FileNameTextBox0.Text = ("Data Pack Fail Error")
            End Try
        End If

        If Me.RadioButtonOnSemi300mm.Checked = True Then '******* The ON Semi took over the Global-NY plant "EFK". they still want to use the GF XML files.
            Try
                SatiCode.MakeGFDataPack(Flex)
                Me.FileNameTextBox0.Text = ("Check OnSemi Data Packs Folder For Your File")
            Catch ex As Exception
                Me.FileNameTextBox0.Text = ("Data Pack Fail Error")
            End Try
        End If

        If Me.RadioButtonWafetTech.Checked = True Then
            Try
                SatiCode.MakeWaferTechDataPack(Flex)
                Me.FileNameTextBox0.Text = ("Check Wafer Tech Data Packs Folder For Your File")
            Catch ex As Exception
                Me.FileNameTextBox0.Text = ("Data Pack Fail Error")
            End Try
        End If

        If Me.RadioButtonMicron.Checked = True Then
            Try
                SatiCode.MakeMicronDataPack(Flex, "")
                Me.FileNameTextBox0.Text = ("Check Micron Data Packs Folder For Your File")
            Catch ex As Exception
                Me.FileNameTextBox0.Text = ("Data Pack Fail Error")
            End Try
        End If

        If Me.RadioButtonSamsung.Checked = True Then
            Try
                SatiCode.MakeSamsungDataPack(Flex)
                Me.FileNameTextBox0.Text = ("Check Samsung Data Packs Folder For Your File")
            Catch ex As Exception
                Me.FileNameTextBox0.Text = ("Data Pack Fail Error")
            End Try
        End If

        If Me.RadioButtonFrescale.Checked = True Then
            Try
                SatiCode.MakeFreeScaleDataPack(Flex)
                Me.FileNameTextBox0.Text = ("Check Freescale Data Packs Folder For Your File")
            Catch ex As Exception
                Me.FileNameTextBox0.Text = ("Data Pack Fail Error")
            End Try
        End If

        If Me.RadioButtonAvago.Checked = True Then
            Try
                SatiCode.MakeAvagoDataPack(Flex)
                Me.FileNameTextBox0.Text = ("Check Avago Data Packs Folder For Your File")
            Catch ex As Exception
                Me.FileNameTextBox0.Text = ("Data Pack Fail Error")
            End Try
        End If

        If Me.RadioButtonIMEC.Checked = True Then
            Try
                SatiCode.MakeIMECDataPack(Flex)
                Me.FileNameTextBox0.Text = ("Check IMEC Data Packs Folder For Your File")
            Catch ex As Exception
                Me.FileNameTextBox0.Text = ("Data Pack Fail Error")
            End Try
        End If

        If Me.RadioButtonIntel.Checked = True Then
            Try
                SatiCode.MakeIntelDataPack(Flex)
                Me.FileNameTextBox0.Text = ("Check Intel Data Packs Folder For Your File")
            Catch ex As Exception
                Me.FileNameTextBox0.Text = ("Data Pack Fail Error")
            End Try
        End If

        If Me.RadioButtonIntelChinaExtra.Checked = True Then
            Try
                SatiCode.MakeIntelChinaDataPack(Flex)
                Me.FileNameTextBox0.Text = ("Check Intel Data Packs Folder For Your File")
            Catch ex As Exception
                Me.FileNameTextBox0.Text = ("Data Pack Fail Error")
            End Try
        End If

        If Me.RadioButtonMicronGrindTest.Checked = True Then
            Try
                SatiCode.MakeMicronDataPack_Grind(Flex, "")
                Me.FileNameTextBox0.Text = ("Check Micron Data Packs Folder For Your File")
            Catch ex As Exception
                Me.FileNameTextBox0.Text = ("Data Pack Fail Error")
            End Try
        End If

        If Me.RadioButtonWD.Checked = True Then
            Try
                SatiCode.MakeWD_File(Flex)
                Me.FileNameTextBox0.Text = ("Check WD Data Packs Folder For Your File")
            Catch ex As Exception
                Me.FileNameTextBox0.Text = ("Data Pack Fail Error")
            End Try
        End If

    End Sub



    Private Sub ShowDirectoryContents(ByVal path As String)
        ' Define the current directory.
        Dim dir As DirectoryInfo = New DirectoryInfo(path)

        ' Get the DirectoryInfo and FileInfo objects.
        Dim files As FileInfo() = dir.GetFiles()
        Dim dirs As DirectoryInfo() = dir.GetDirectories()

        ' Show the directory listing.
        lblCurrentDir.Text = "Currently showing " & path
        gridFileList.DataSource = files
        gridDirList.DataSource = dirs

        Page.DataBind()

        ' Clear any selection.
        gridFileList.SelectedIndex = -1

        ' Keep track of the current path.
        ViewState("CurrentPath") = path
        FileNameTextBox0.Text = ""

    End Sub

    Protected Sub gridFileList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Get the selected file.
        Dim file As String = CStr(gridFileList.DataKeys(gridFileList.SelectedIndex).Value)

        ' The FormView shows a collection (or list) of items.
        ' To accommodate this model, you must add the file object
        ' to a collection of some sort.
        Dim files As ArrayList = New ArrayList()
        files.Add(New FileInfo(file))


        ' Now show the selected file.
        formFileDetails.DataSource = files
        formFileDetails.DataBind()

        FileNameTextBox0.Text = file
        Me.Panel3.Visible = True

    End Sub

    Protected Function GetVersionInfoString(ByVal path As Object) As String
        Dim info As FileVersionInfo = FileVersionInfo.GetVersionInfo(CStr(path))
        Return info.FileName & " " & info.FileVersion & "<br>" & info.ProductName & " " & info.ProductVersion
    End Function

    Protected Sub cmdUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUp.Click
        'Dim strPath As String = CStr(ViewState("CurrentPath"))
        'strPath = Path.Combine(strPath, "..")
        'strPath = Path.GetFullPath(strPath)
        'ShowDirectoryContents(strPath)
        'ShowDirectoryContents("\\PWI-40\databases\Customer Data\" & Me.TextBoxID.Text)
        WhatToShow()
    End Sub

    Sub WhatToShow()
        ShowDirectoryContents("\\PWI-40\databases\Customer Data\" & Me.TextBoxID.Text)
    End Sub

    Protected Sub gridDirList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Get the selected directory.
        Dim dir As String = CStr(gridDirList.DataKeys(gridDirList.SelectedIndex).Value)

        ' Now refresh the directory list to
        ' show the selected directory.
        ShowDirectoryContents(dir)

    End Sub


    Protected Sub RadioButtonIBM_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonIBM.CheckedChanged
        If RadioButtonIBM.Checked = True Then
            Viewp2()
        End If
    End Sub

    Protected Sub RadioButtonOnSemi200mm_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonOnSemi200mm.CheckedChanged
        If RadioButtonOnSemi200mm.Checked = True Then
            Viewp2()
        End If
    End Sub

    Protected Sub RadioButtonOnSemi300mm_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonOnSemi300mm.CheckedChanged
        If RadioButtonOnSemi300mm.Checked = True Then
            Viewp2()
        End If
    End Sub

    Sub Viewp2()
        Me.Panel2.Visible = True
        If RadioButtonIBM.Checked = True Then
            Me.TextBoxSeqNumber.Visible = True
            Me.CheckBoxNewOrOldIBM.Visible = True
            Me.LabelSeq.Visible = True
        Else
            Me.TextBoxSeqNumber.Visible = False
            Me.CheckBoxNewOrOldIBM.Visible = False
            Me.LabelSeq.Visible = False
        End If

        WhatToShow() 'ShowDirectoryContents("\\PWI-40\databases\Customer Data")
        'Make Data Pack
    End Sub

    Protected Sub RadioButtonWafetTech_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonWafetTech.CheckedChanged
        If RadioButtonWafetTech.Checked = True Then
            Viewp2()
        End If
    End Sub

    Protected Sub RadioButtonMicron_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonMicron.CheckedChanged
        If RadioButtonMicron.Checked = True Then
            Viewp2()
        End If
    End Sub


    Protected Sub RadioButtonSamsung_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonSamsung.CheckedChanged
        If RadioButtonSamsung.Checked = True Then
            Viewp2()
        End If
    End Sub

    Protected Sub RadioButtonFrescale_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonFrescale.CheckedChanged
        If RadioButtonFrescale.Checked = True Then
            Viewp2()
        End If
    End Sub

    Protected Sub RadioButtonAvago_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonAvago.CheckedChanged
        If RadioButtonAvago.Checked = True Then
            Viewp2()
        End If
    End Sub

    Protected Sub RadioButtonIMEC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonIMEC.CheckedChanged
        If RadioButtonIMEC.Checked = True Then
            Viewp2()
        End If
    End Sub
    Protected Sub RadioButtonGF_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonGF.CheckedChanged
        If RadioButtonGF.Checked = True Then
            Viewp2()
        End If
    End Sub
    Protected Sub RadioButtonIntelChinaExtra_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonIntelChinaExtra.CheckedChanged
        If RadioButtonIntelChinaExtra.Checked = True Then
            Viewp2()
        End If
    End Sub
    Protected Sub RadioButtonIntel_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonIntel.CheckedChanged
        If RadioButtonIntel.Checked = True Then
            Viewp2()
        End If
    End Sub
    Protected Sub RadioButtonMicronGrindTest_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonMicronGrindTest.CheckedChanged
        If RadioButtonMicronGrindTest.Checked = True Then
            Viewp2()
        End If
    End Sub
    Protected Sub Button_Get_ISQUIT_Package_Click(sender As Object, e As EventArgs) Handles Button_Get_ISQUIT_Package.Click
        Me.TextBoxGackageName.Text = SatiCode.Make_ISquit_Package_Name()
    End Sub
    Protected Sub RadioButtonWD_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonWD.CheckedChanged
        If RadioButtonWD.Checked = True Then
            Viewp2()
        End If
    End Sub
End Class
