
Imports System.Configuration
Imports System.Text.Encodings.Web
Imports System.Text.Json
Imports System.Threading.Tasks
Imports System.Web.Services
Imports Microsoft.Office.Interop.Excel
Imports Microsoft.PowerBI.Api.V2.Models
Imports SatiDotNet2.Library


Partial Class MR_OpenTicketStatusBoard
    Inherits System.Web.UI.Page
    Dim SatiCode As New Class1
    Dim LogAspx As New LogAspxLibrary
    Dim Security As New Security
    Dim LogStatus As String
    Dim StripeColor As String
    Dim SqlFunc As String
    Dim LogDS As New Data.DataSet
    Dim LogDR As Data.DataRow
    Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
    Dim SqlFuncDR As Data.DataRow

    Private Shared StampIndicator As New StampIndicator()
    Private _ActivePm As New ActivePm()
    Private _Security As New Security()

    Private Sub PageInit(sender As Object, e As EventArgs) Handles Me.Init
        Dim DS As New Data.DataSet
        Dim RC As Integer = 0
        Dim TodaysDate As Date = Date.Parse(System.DateTime.Now)
        Dim StampIndicatorLabels As New Dictionary(Of String, String)

        'check if intitial entry of webpage does NOT contain querystring. if so, redirect to ChecklistLoggingMainMaint.aspx
        If Request.QueryString.Count = 0 AndAlso (Session("WhereFromQueryString") Is Nothing OrElse Session("DepartmentFromQueryString") Is Nothing OrElse Session("ViewFromQueryString") Is Nothing) Then
            'reinitialize the session state variables then refresh the page
            'this process is critical in ensuring 24/7 uptime of status board
            Session("WhereFromQueryString") = TodaysDate.Date
            Session("DepartmentFromQueryString") = "Maintenance"
            Session("ViewFromQueryString") = "Focus"
            Response.Redirect(Request.Url.ToString(), False)
            Exit Sub
        ElseIf Request.QueryString.Count > 0 Then
            Dim QsDepartment As String = Request.QueryString("Department")
            Dim QsView As String = Request.QueryString("View")

            If Request.QueryString("WHERE") Is Nothing Then
                Session("WhereFromQueryString") = TodaysDate.Date
            Else 'If Request.QueryString("WHERE") IsNot Nothing Then
                Session("WhereFromQueryString") = Request.QueryString("WHERE")
            End If

            Session("DepartmentFromQueryString") = If(QsDepartment Is Nothing, Session("DepartmentFromQueryString"), QsDepartment)
            Session("ViewFromQueryString") = If(QsView Is Nothing, Session("ViewFromQueryString"), QsView)

            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path), False) 'redirect the user to the URL without query strings
            Exit Sub
        Else
            'MenuAuthentication hierarchy based on querystrings user loaded the page with
            'MenuAuthenication.CheckGroupAuthenication("EditRoles", Server)
            Dim RequiredRoles As String() = Security.GetStatusBoardRole(Session("ViewFromQueryString"), Session("DepartmentFromQueryString"), Date.Parse(Session("WhereFromQueryString")))
            If RequiredRoles.Contains(Nothing) = False Then
                MenuAuthenication.CheckGroupsAuthenication(RequiredRoles, Server)
            End If

            WhereLabel.Text = Session("WhereFromQueryString")

            If Session("ViewFromQueryString") = "Full" Then
                AdminPanel.Visible = True
            End If
        End If
    End Sub

    Sub SetButtonText(Button As Button, DR As Data.DataRow)
        If Not IsDBNull(DR("Assignee")) Then
            Button.Text = DR("Assignee") & " - " & DR("Area")
            ' Button.ForeColor = System.Drawing.Color.DarkBlue
        Else
            Button.Text = DR("Area")
        End If

    End Sub
End Class
