Imports System.Text.Json
Imports System.Web.UI.WebControls

Public Class StampIndicator
    Inherits Security

    Private TitleToIcon As New Dictionary(Of String, String)
    Public StampIconCss As String = "width: 20px; height: 20px; border-radius: 50% 50%; cursor: pointer;"

    Sub New()
    End Sub

    Public Function Icons(T_LogDataKey As Integer) As List(Of String)
        Dim IconsList As New List(Of String)
        Dim StatusBoardDS As Data.DataSet
        Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))

        QueryObject("@DataRecordKey") = New Dictionary(Of String, String) From {
            {"value", T_LogDataKey},
            {"typeOf", "int"}
        }

        If NumOfStamps(T_LogDataKey) > 0 Then
            Dim RC As Integer

            StatusBoardDS = GetMyDataSetParamQuery("SELECT ST.Base64Icon FROM [ALTS].[dbo].[T_LogStamp] S  INNER JOIN [ALTS].[dbo].[T_LogStampList] SL ON S.StampKey=SL.[Key]  INNER JOIN [ALTS].[dbo].[T_LogStampTitle] ST ON SL.TitleKey=ST.[Key]  WHERE S.DataRecordKey=@DataRecordKey AND S.Active=1  ORDER BY ST.[Key]", QueryObject)
            RC = StatusBoardDS.Tables(0).Rows.Count

            For I As Integer = 0 To RC - 1
                Dim StatusBoardDR As Data.DataRow = StatusBoardDS.Tables(0).Rows(I)

                IconsList.Add(StatusBoardDR("Base64Icon"))
            Next
        End If

        Return IconsList
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

    Public Function GetTitleIconHash() As Dictionary(Of String, String)
        Dim CurrDS As New Data.DataSet

        CurrDS = GetMyDataSetParamQuery("SELECT Title, Base64Icon FROM [ALTS].[dbo].[T_LogStampTitle]", New Dictionary(Of String, Dictionary(Of String, String)))
        For Each CurrDR As DataRow In CurrDS.Tables(0).Rows
            TitleToIcon(CurrDR("Title")) = CurrDR("Base64Icon")
        Next

        Return TitleToIcon
    End Function

    Function CreateIcons(ParentControl As Panel, T_LogDataKey As Integer, IconClick As Action(Of Integer), Optional AttachIcons As Boolean = True) As Panel
        Dim Icon As ImageButton
        Dim DbKey As Integer = ParentControl.ID.Split("_")(1)
        Dim SbUrls As List(Of String) = Icons(T_LogDataKey)

        If AttachIcons = False Then Return ParentControl

        If SbUrls.Count > 2 Then 'only add the css below when needed, to prevent excessive whitespace
            ParentControl.Attributes.Add("style", "grid-template-columns: 1fr 1fr;")
        End If

        For Each SbUrl As String In SbUrls
            Icon = New ImageButton()
            Icon.ImageUrl = SbUrl
            Icon.Attributes.Add("style", StampIconCss)
            AddHandler Icon.Click, Sub(sender As Object, e As EventArgs)
                                       IconClick(T_LogDataKey)
                                   End Sub

            ParentControl.Controls.Add(Icon)
        Next

        Return ParentControl
    End Function

End Class
