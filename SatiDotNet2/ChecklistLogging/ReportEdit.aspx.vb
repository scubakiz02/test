
Imports System.Text.Json
Imports SatiDotNet2.Library

Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim Security As New Security
    Dim LabelFromQueryString As String
    Dim DateFromQueryString As String
    Dim AreaFromQueryString As String
    Dim DS As New Data.DataSet
    Dim DR As Data.DataRow
    Dim RC As Integer
    Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))
    Private Report As Report

    Private Sub MR_OpenTicketStatusBoard_Load(sender As Object, e As EventArgs) Handles Me.Load
        'MenuAuthenication.CheckPageAuthenication(Page, User, Server)
        'MenuAuthenication.CheckGroupAuthenication("Office", Server)
        LabelFromQueryString = Request.QueryString("Label")
        DateFromQueryString = Request.QueryString("Date")
        AreaFromQueryString = Request.QueryString("Area")

        If AreaFromQueryString IsNot Nothing Then
            Report = New Report(New Dictionary(Of String, String) From {
                {"GroupKey", 0},
                {"AreaKey", AreaFromQueryString}
            })
        End If

        If LabelFromQueryString IsNot Nothing AndAlso DateFromQueryString IsNot Nothing Then
            If Not IsPostBack Then
                Dim Input As New Dictionary(Of String, String)

                QueryObject("@LabelKey") = New Dictionary(Of String, String) From {
                    {"value", LabelFromQueryString},
                    {"typeOf", "int"}
                }
                QueryObject("@Date") = New Dictionary(Of String, String) From {
                    {"value", DateFromQueryString},
                    {"typeOf", "string"}
                }

                DR = Security.GetMyDataSetParamQuery("SELECT D.[Key], D.Inputs, D.Date, L.Label FROM [ALTS].[dbo].[T_LogLabel] L INNER JOIN [ALTS].[dbo].[T_LogData] D ON L.AreaKey=D.AreaKey WHERE L.[Key]=@LabelKey AND DATEDIFF(DAY, @Date, D.Date)=0", QueryObject).Tables(0).Rows(0)
                DbLabelFieldLabel.Text = DR("Label")
                DataLabel.Text = DR("Date")

                Input = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(DR("Inputs"))(LabelFromQueryString)

                DbDateTextBox.Text = Input("Date")
                DbOperatorTextBox.Text = Input("Operator")
                DbValueTextBox.Text = Input("Value")
            End If
        End If
    End Sub

    Protected Sub ExitIframeButton_onClick(sender As Object, e As EventArgs)
        If sender.Text = "Update" Then
            Dim Config As New Dictionary(Of String, String) From {
                {"LabelKey", LabelFromQueryString},
                {"Date", DateFromQueryString}
            }
            Dim Mods As New Dictionary(Of String, String) From {
                {"Value", DbValueTextBox.Text},
                {"Date", DbDateTextBox.Text},
                {"Operator", DbOperatorTextBox.Text}
            }

            Report.Override(Config, Mods, True)
            Session("Report").ConfigureDS()
        End If

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "disableIframe", "disableIframe();", True)
    End Sub
End Class
