Imports System.Text.Json
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls

Public Class StampIndicator
    Inherits Security

    Private TitleToIcon As New Dictionary(Of String, String)

    Sub New()
    End Sub

    Public Function Icons(T_LogDataKey As Integer) As Dictionary(Of String, Dictionary(Of String, String))
        Dim IconsHash As New Dictionary(Of String, Dictionary(Of String, String))
        Dim StatusBoardDS As Data.DataSet
        Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))

        QueryObject("@DataRecordKey") = New Dictionary(Of String, String) From {
            {"value", T_LogDataKey},
            {"typeOf", "int"}
        }

        If NumOfStamps(T_LogDataKey) < NumOfNeededStamps(T_LogDataKey) Then
            Dim RC As Integer

            StatusBoardDS = GetMyDataSetParamQuery("SELECT SL.[Key] As StampKey, ST.Title, ST.Base64Icon, ST.IconImgFilePath
              FROM [ALTS].[dbo].[T_LogStampList] SL
              INNER JOIN [ALTS].[dbo].[T_LogStampTitle] ST ON SL.TitleKey=ST.[Key]  
              WHERE 
              AreaKey=(SELECT AreaKey FROM [ALTS].[dbo].[T_LogData] D WHERE [Key]=@DataRecordKey) 
              AND Active=1", QueryObject)
            RC = StatusBoardDS.Tables(0).Rows.Count

            QueryObject("@StampKey") = New Dictionary(Of String, String) From {
                {"value", String.Empty},
                {"typeOf", "int"}
            }

            For I As Integer = 0 To RC - 1
                Dim StatusBoardDR As Data.DataRow = StatusBoardDS.Tables(0).Rows(I)

                QueryObject("@StampKey")("value") = StatusBoardDR("StampKey")

                If GetSingleDbField("SELECT [Key] FROM [ALTS].[dbo].[T_LogStamp] S WHERE StampKey=@StampKey AND DataRecordKey=@DataRecordKey AND Active=1", QueryObject, "Key") Is Nothing Then
                    Dim IconHash As New Dictionary(Of String, String)

                    IconHash("Base64Icon") = StatusBoardDR("Base64Icon")
                    IconHash("IconImgFilePath") = StatusBoardDR("IconImgFilePath")

                    IconsHash(StatusBoardDR("Title")) = IconHash
                End If
            Next
        End If

        Return IconsHash
    End Function

    Private Function NumOfNeededStamps(T_LogDataKey As Integer) As Integer
        Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))
        QueryObject("@DataRecordKey") = New Dictionary(Of String, String) From {
            {"value", T_LogDataKey},
            {"typeOf", "int"}
        }

        Return GetSingleDbField("SELECT COUNT([Key]) As NumOfNeededStamps From [ALTS].[dbo].[T_LogStampList] Where Active = 1 And AreaKey = (Select AreaKey From [ALTS].[dbo].[T_LogData] Where [Key]=@DataRecordKey)", QueryObject, "NumOfNeededStamps")
    End Function

    Private Function NumOfStamps(T_LogDataKey As Integer) As Integer
        Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))
        QueryObject("@DataRecordKey") = New Dictionary(Of String, String) From {
            {"value", T_LogDataKey},
            {"typeOf", "int"}
        }

        Return GetSingleDbField("SELECT COUNT(S.[Key]) As NumOfStamps FROM [ALTS].[dbo].[T_LogStamp] S INNER Join [ALTS].[dbo].[T_LogStampList] SL ON S.StampKey=SL.[Key] WHERE DataRecordKey=@DataRecordKey And S.Active = 1", QueryObject, "NumOfStamps")
    End Function

    Public Function GetCssClass(IconImgFilePath As String) As String
        Dim IconCssClass As String = String.Empty

        Select Case IconImgFilePath
            Case "../Color/wrench-fill.png"
                IconCssClass = "icon-fm-manager"

            Case "../Color/list-checks-fill.png"
                IconCssClass = "icon-qshe-manager"

            Case "../Color/factory-fill.png"
                IconCssClass = "icon-prod-sup"

            Case "../Color/pipe-wrench-fill.png"
                IconCssClass = "icon-maint-sup"
        End Select

        Return IconCssClass
    End Function

    Public Function CreateStampHtml(ParentControl As Panel, T_LogDataKey As Integer) As Panel
        Dim DbKey As Integer = ParentControl.ID.Split("_")(1)
        Dim StatusBoardIconsHash As Dictionary(Of String, Dictionary(Of String, String)) = Icons(T_LogDataKey)
        Dim StampCssClasses As New List(Of String)
        Dim StampIconCss As String = "width: 20px; height: 20px; border-radius: 50% 50%; cursor: pointer;"

        For Each kvp As KeyValuePair(Of String, Dictionary(Of String, String)) In StatusBoardIconsHash
            Dim IconImgFilePath As String = kvp.Value("IconImgFilePath")
            Dim StampCssClass As String = GetCssClass(IconImgFilePath)
            Dim Stamp As New HtmlGenericControl("div")

            Stamp.Attributes("class") = "stamp-icon " & StampCssClass
            Stamp.Attributes("onclick") = "newTab('Log.aspx?Key=" & T_LogDataKey & "'); return false;"

            ParentControl.Controls.Add(Stamp)
        Next

        If StampCssClasses.Count > 2 Then 'only add the css below when needed, to prevent excessive whitespace
            ParentControl.Attributes.Add("style", "grid-template-columns: 1fr 1fr;")
        End If

        Return ParentControl
    End Function

End Class
