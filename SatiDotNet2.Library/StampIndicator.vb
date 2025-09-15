Imports System.Text.Json
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls

Public Class StampIndicator
    Inherits Security

    Private _ActivePm As New ActivePm()

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

    Public Function GetCssClass(StampTitle As String) As String
        Dim IconCssClass As String = String.Empty

        Select Case StampTitle
            Case "F&M Manager"
                IconCssClass = "icon-fm-manager"

            Case "Q/SHE Manager"
                IconCssClass = "icon-qshe-manager"

            Case "Prod Sup"
                IconCssClass = "icon-prod-sup"

            Case "Maint Sup"
                IconCssClass = "icon-maint-sup"
        End Select

        Return IconCssClass
    End Function

    Public Function CreateStampHtml(ParentControl As Panel, T_LogDataKey As Integer) As Panel
        Dim DbKey As Integer = ParentControl.ID.Split("_")(1)
        Dim AddStamps As List(Of String) = _ActivePm.GetLogConfig(DbKey)("addStamps")
        Dim StampIconCss As String = "width: 20px; height: 20px; border-radius: 50% 50%; cursor: pointer;"

        For Each AddStamp As String In AddStamps
            Dim Stamp As New HtmlGenericControl("div")
            Dim StampCssClass As String = GetCssClass(AddStamp)

            Stamp.Attributes("class") = "stamp-icon " & StampCssClass
            Stamp.Attributes("onclick") = "newTab('Log.aspx?Key=" & T_LogDataKey & "'); return false;"

            ParentControl.Controls.Add(Stamp)
        Next

        Return ParentControl
    End Function
End Class
