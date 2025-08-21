
Imports System.Text.Json
Imports SatiDotNet2.Library

Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim Security As New Security
    Dim AreaFromQueryString As String
    Dim DS As New Data.DataSet
    Dim DR As Data.DataRow
    Dim RC As Integer
    Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))

    Private _ActivePmCache As New ActivePmCache()

    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        'MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        'MenuAuthenication.CheckGroupAuthenication("Office", Server)
        AreaFromQueryString = Request.QueryString("Area")
        QueryObject("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaFromQueryString},
            {"typeOf", "int"}
        }

        If AreaFromQueryString IsNot Nothing Then
            If Not IsPostBack Then
                AreaLabel.Text = Security.GetSingleDbField("SELECT Area FROM [ALTS].[dbo].[T_LogArea] WHERE [Key]=@AreaKey", QueryObject, "Area")
                DS = Security.GetMyDataSetParamQuery("SELECT Stamp.Title As Text, Stamp.[Key] As Value, Stamped.Active AS Selected FROM [ALTS].[dbo].[T_LogStampTitle] Stamp INNER JOIN [ALTS].[dbo].[T_LogStampList] Stamped ON Stamp.[Key]=Stamped.TitleKey AND Stamped.AreaKey=@AreaKey", QueryObject)
                RC = DS.Tables(0).Rows.Count

                For I = 0 To RC - 1
                    Dim listItem As New ListItem()
                    DR = DS.Tables(0).Rows(I)

                    listItem.Text = DR("Text")
                    listItem.Value = DR("Value") 'associated Key within [T_LogStampTitle]
                    listItem.Selected = DR("Selected")

                    StampCheckBoxList.Items.Add(listItem)
                Next
            End If
        End If
    End Sub

    Protected Sub ExitIframeButton_onClick(sender As Object, e As EventArgs)
        If sender.Text = "Update" Then
            For Each ListItem As ListItem In StampCheckBoxList.Items
                QueryObject("@Active") = New Dictionary(Of String, String) From {
                    {"value", If(ListItem.Selected, True, False)},
                    {"typeOf", "bit"}
                }
                QueryObject("@TitleKey") = New Dictionary(Of String, String) From {
                    {"value", ListItem.Value},
                    {"typeOf", "int"}
                }

                Security.ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogStampList] SET Active=@Active WHERE TitleKey=@TitleKey AND AreaKey=@AreaKey", QueryObject)
            Next

            'sql query to get all submitted logs that do not have all of there stamps
            Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
                {"@AreaKey", Security.GetParamVarHash(AreaFromQueryString, "int")},
                {"@StartDateCutoffAt", Security.GetParamVarHash(Session("StartDateCutoffAt"), "string")}
            }
            Dim RelevantLogsDs As Data.DataSet = Security.GetMyDataSetParamQuery("SELECT [Key] As DataKey FROM [ALTS].[dbo].[T_LogData] D WHERE CompleteLog=1 AND AreaKey=@AreaKey AND D.Date > @StartDateCutoffAt", SqlConfig)

            For Each RelevantLogsDr As Data.DataRow In RelevantLogsDs.Tables(0).Rows
                _ActivePmCache.CacheAdd(RelevantLogsDr("DataKey"))
            Next
        End If

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "disableIframe", "disableIframe();", True)
    End Sub
End Class
