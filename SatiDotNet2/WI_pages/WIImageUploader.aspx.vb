Imports System.IO

Partial Class WI_WIImageUploader
    Inherits System.Web.UI.Page

    'Loads the page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If SelectImages.Items.Count < 4 Then
                setDropDownDefaultImages()
            End If
        End If
    End Sub

    'Handles the Upload clicks
    Protected Sub cmdUpload_Click(sender As Object, e As EventArgs)
        If Not WIUploader.FileName = "" Then
            If ImgUpTextBox.Value.Length > 3 Then
                Dim savedFile As String = SetImagePath(ImgUpTextBox.Value) + WIUploader.FileName
                Dim dropName As String = ""

                If File.Exists(savedFile) Then
                    Dim count As Integer = 1
                    While File.Exists(savedFile)
                        savedFile = savedFile.Replace(".", count & ".")
                        count += 1
                    End While
                End If

                Dim curListItem As ListItem = New ListItem()
                curListItem.Text = savedFile.Substring(savedFile.LastIndexOf("\") + 1)
                curListItem.Value = savedFile

                WIUploader.SaveAs(savedFile)
                SelectImages.Items.Add(curListItem)

                successMessage.Text = "UPLOAD: Successful"
            End If
        End If
    End Sub

    'Updated success message once something is selected
    Protected Sub SelectImages_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SelectImages.SelectedIndexChanged
        successMessage.Text = ""
    End Sub

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

    'Post back controls
    Protected Sub setLockOnPostBack()
        If ImgUpTextBox.Value.Length > 4 Then
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "RAP", "resetAfterPost()", True)
        End If
    End Sub

    'This updates the images drop down list
    Protected Sub UpdateImageList(sender As Object, e As EventArgs)
        If ImgUpTextBox.Value.Length > 3 Then
            Dim imageFiles As String = SetImagePath(ImgUpTextBox.Value)
            cmdUpload.Enabled = True
            WIUploader.Enabled = True

            If SelectImages.Items.Count > 4 Then
                SelectImages.Items.Clear()
                setDropDownDefaultImages()
            End If

            If Directory.Exists(imageFiles) Then
                If Directory.GetFiles(imageFiles).Length > 0 Then
                    For Each curFil As String In Directory.GetFiles(imageFiles)
                        Dim curFilName As String = curFil.Substring(curFil.LastIndexOf("\") + 1)

                        Dim curListItem As ListItem = New ListItem()
                        curListItem.Text = curFilName
                        curListItem.Value = curFil

                        SelectImages.Items.Add(curListItem)
                    Next
                End If
            End If
        Else
            cmdUpload.Enabled = False
            WIUploader.Enabled = False
        End If
    End Sub

    'Sets the default drop down images and values
    Protected Sub setDropDownDefaultImages()
        Dim defTag As ListItem = New ListItem()
        Dim def_TB As ListItem = New ListItem()
        Dim def_LA As ListItem = New ListItem()
        Dim def_RA As ListItem = New ListItem()

        defTag.Text = "Select an Image or Textbox"
        defTag.Value = "NULL"

        def_TB.Text = "Floating Textbox"
        def_TB.Value = "textarea"

        def_LA.Text = "Left Arrow"
        def_LA.Value = buildFrameSrc(Session("WI_View").ToString + "Images/" & "LeftArrow.png")

        def_RA.Text = "Right Arrow"
        def_RA.Value = buildFrameSrc(Session("WI_View").ToString + "Images/" & "RightArrow.png")

        SelectImages.Items.Add(defTag)
        SelectImages.Items.Add(def_TB)
        SelectImages.Items.Add(def_LA)
        SelectImages.Items.Add(def_RA)
    End Sub

    'Deletes an image in the drop down list which changes
    Protected Sub imageDelete_Click(sender As Object, e As EventArgs) Handles imageDelete.Click
        If SelectImages.SelectedIndex > 3 Then
            Dim imageFile As String = SetImagePath(ImgUpTextBox.Value)
            If Directory.Exists(imageFile) Then
                If File.Exists(imageFile + SelectImages.SelectedValue) Then
                    File.Delete(imageFile + SelectImages.SelectedValue)
                    SelectImages.Items.Remove(SelectImages.SelectedItem)
                End If
            End If
        End If
    End Sub

    'function that updates the physical path with the virtual path
    Protected Function buildFrameSrc(curSrc As String) As String
        curSrc = curSrc.Substring(curSrc.IndexOf("/WI/"))
        curSrc = curSrc.Replace("\", "/")

        Return curSrc
    End Function
End Class
