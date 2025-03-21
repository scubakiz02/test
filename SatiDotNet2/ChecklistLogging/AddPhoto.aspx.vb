
Imports System.Text.Json
Imports System.Data
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Web.Services
Imports SatiDotNet2.Library

Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim Security As New Security
    Dim DS As New Data.DataSet
    Dim DR As Data.DataRow
    Dim RC As Integer
    Dim uploadDirectory As String
    Dim VirtualDirectory As String
    Dim Directory As String
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
    Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))

    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        'MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        'MenuAuthenication.CheckGroupAuthenication("Office", Server)
        DataKeyFromQueryString = Request.QueryString("DataKey")
        QueryConfig("@DataKey") = New Dictionary(Of String, String) From {
            {"value", DataKeyFromQueryString},
            {"typeOf", "int"}
        }
        DR = Security.GetMyDataSetParamQuery("SELECT A.[Key], I.SqlFunc2ndArg, D.Date, A.Area, I.SqlFunc FROM [ALTS].[dbo].[T_LogData] D INNER JOIN [ALTS].[dbo].[T_LogArea] A ON D.AreaKey=A.[Key] INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE D.[Key]=@DataKey", QueryConfig).Tables(0).Rows(0)

        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", DR("Key")},
            {"typeOf", "int"}
        }
        QueryConfig("@SqlFunc2ndArg") = New Dictionary(Of String, String) From {
            {"value", DR("SqlFunc2ndArg")},
            {"typeOf", "float"}
        }
        QueryConfig("@Date") = New Dictionary(Of String, String) From {
            {"value", DR("Date")},
            {"typeOf", "string"}
        }
        QueryConfig("@SqlFunc") = New Dictionary(Of String, String) From {
            {"value", DR("SqlFunc")},
            {"typeOf", "string"}
        }

        'Directory = Path.Combine(Regex.Replace(DR("Area"), "[:#]'\""", ""), Security.GetSingleDbField("Select DatePeriod FROM " & DR("SqlFunc") & "(@AreaKey, @SqlFunc2ndArg, @Date)", QueryConfig, "DatePeriod").Replace("/", "-"))
        Dim AreaRegexed As String = Regex.Replace(DR("Area"), "[:#]'\""", "")
        Dim DatePeriod As String = Security.GetSingleDbField("Select DatePeriod FROM " & DR("SqlFunc") & "(@AreaKey, @SqlFunc2ndArg, @Date)", QueryConfig, "DatePeriod")
        Directory = Path.Combine(AreaRegexed, DatePeriod).Replace("/", "-")

        uploadDirectory = Path.Combine(Session("SUP_IO"), Directory).Replace("\", "/")
        VirtualDirectory = Path.Combine(Session("SUP_VD"), Directory).Replace("\", "/")
        SnapshotImageButton.ImageUrl = Path.Combine(VirtualDirectory, Request.QueryString("fileName"))
    End Sub

    Function StripString(ByVal input As String) As String
        Return Regex.Replace(input, "[^a-zA-Z0-9]", "").ToLower()
    End Function

    Function SqlProofSingleQuotes(Text As String) As String
        Return Text.Replace("'", "''") 'escape single quotes (') by doubling them ('')
    End Function

    Protected Sub CancelImage_OnClick(sender As Object, e As EventArgs)
        System.IO.File.Delete(Session("FileUploadDirectory"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "disableIframe", "disableIframe();", True)
    End Sub

    Protected Sub ExitIframeButton_onClick(sender As Object, e As EventArgs)
        'variables declared in UploadFile do NOT hold their value, so I tied them to the session
        Dim UserInput = ImgNameTextBox.Text
        Dim NewFileName As String
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
                    NewFileName = UserInput.Replace(" ", "_") & "." & If(ContentTypeToFormat.ContainsKey(Session("ContentType")), ContentTypeToFormat(Session("ContentType")), Session("ContentType"))
                    StrippedUserInput = StripString(UserInput)

                    'ensure checklist name does NOT currently exist in T_LogArea
                    For J = 0 To DuplicateRC - 1
                        DuplicateDR = DuplicateDS.Tables(0).Rows(J)

                        If StrippedUserInput = StripString(DuplicateDR("PhotoTitle")) Then
                            Throw New ArgumentException("*ERROR: A PHOTO WITH THIS TITLE EXISTS FOR THIS LOG*")
                        End If
                    Next

                    System.IO.File.Move(Session("FileUploadDirectory"), Path.Combine(uploadDirectory, NewFileName))

                    QueryConfig.Clear()
                    QueryConfig("@DataKey") = New Dictionary(Of String, String) From {
                        {"value", DataKeyFromQueryString},
                        {"typeOf", "int"}
                    }
                    QueryConfig("@UserInput") = New Dictionary(Of String, String) From {
                        {"value", SqlProofSingleQuotes(UserInput)},
                        {"typeOf", "string"}
                    }
                    QueryConfig("@ImagePath") = New Dictionary(Of String, String) From {
                        {"value", Path.Combine(VirtualDirectory, NewFileName)},
                        {"typeOf", "string"}
                    }
                    QueryConfig("@ContentType") = New Dictionary(Of String, String) From {
                        {"value", Session("ContentType")},
                        {"typeOf", "string"}
                    }
                    QueryConfig("@FileName") = New Dictionary(Of String, String) From {
                        {"value", NewFileName},
                        {"typeOf", "string"}
                    }
                    Security.ExecuteSqlParamQuery("INSERT INTO [ALTS].[dbo].[T_LogDataPhotos] (DataKey, PhotoTitle, PhotoFilePath, ContentType, FileName) VALUES (@DataKey, @UserInput, @ImagePath, @ContentType, @FileName)", QueryConfig)
                End If
            Else 'Cancel
                If uploadDirectory IsNot Nothing Then
                    System.IO.File.Delete(Session("FileUploadDirectory"))
                End If
            End If
        Catch ex As Exception
            UserErrorLabel.Text = ex.Message.ToString()
            Exit Sub
        End Try

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "disableIframe", "disableIframe();", True)
    End Sub
End Class
