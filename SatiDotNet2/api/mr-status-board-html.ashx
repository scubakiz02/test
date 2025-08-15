<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.Threading
Imports System.Text.Json
Imports SatiDotNet2.Library
Imports System.Runtime.Caching

Public Class StreamData
    Inherits Security
    Implements IHttpHandler, IReadOnlySessionState

    Public Sub ProcessRequest(Context As HttpContext) Implements IHttpHandler.ProcessRequest
        Context.Response.ContentType = "application/json"

        Dim Res As New Dictionary(Of String, String)
        Res("html") = GetBuildHtml()

        Context.Response.Write(JsonSerializer.Serialize(Res))
    End Sub

    Private Function GetBuildHtml() As String
        Dim I As Integer = 0
        Dim II As Integer = 0
        Dim RC As Integer = 0
        Dim Department As String = ""
        Dim SB As New StringBuilder
        Dim TempNote As String
        Dim DS As New Data.DataSet
        Dim DR As Data.DataRow
        Dim DR_Look As Data.DataRow
        Dim CountNotes As Integer


        DS = GetMyDataSetParamQuery("SELECT TOP (100) PERCENT dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.Q_MR_Open.MR_Key, dbo.Q_MR_Open.[Ticket Type], dbo.Q_MR_Open.NoteType, dbo.Q_MR_Open.Note, dbo.Q_MR_Open.IssueDate FROM dbo.T_Tools LEFT OUTER JOIN dbo.Q_MR_Open ON dbo.T_Tools.[Key] = dbo.Q_MR_Open.Tool WHERE (dbo.T_Tools.UpTimeReport = 1) GROUP BY dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.Q_MR_Open.MR_Key, dbo.Q_MR_Open.[Ticket Type], dbo.Q_MR_Open.NoteType, dbo.Q_MR_Open.Note, dbo.Q_MR_Open.IssueDate  ORDER BY dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.Q_MR_Open.MR_Key", New Dictionary(Of String, Dictionary(Of String, String)))
        RC = DS.Tables(0).Rows.Count

        For I = 0 To RC - 1
            DR = DS.Tables(0).Rows(I)

            If Not DR("Department").ToString = Department Then
                SB.Append("<h2 style=""color: #0000FF""> " & DR("Department").ToString & "</h1> ")
                Department = DR("Department").ToString
            End If

            SB.Append("<input ")
            SB.Append("iD=""Button" & I & """ ")
            SB.Append("type=""button"" ")
            If IsDBNull(DR("IssueDate")) Then
                SB.Append("value=""" & DR("Tool").ToString & """ ")
            Else
                SB.Append("value=""" & DR("Tool").ToString & " (" & DateDiff(DateInterval.Hour, DR("IssueDate"), DateAndTime.Now) & "hr)" & """  ")
            End If
            'DateDiff(DateInterval.Hour, DR("IssueDate"), DateAndTime.Now)

            CountNotes = 1
            Do
                If DR("NoteType").ToString = "" Then
                    SB.Append("title=""Looks Good!"" ")
                    CountNotes = 0
                    Exit Do
                Else
                    If I = RC - 1 Then
                        Exit Do
                    End If
                    DR_Look = DS.Tables(0).Rows(I + CountNotes)
                    If DR("MR_Key").ToString = DR_Look("MR_Key").ToString Then
                        CountNotes += 1
                    Else
                        Exit Do
                    End If
                End If
            Loop

            If Not CountNotes = 0 Then
                TempNote = ""
                For II = 0 To CountNotes - 1
                    DR_Look = DS.Tables(0).Rows(I + II)
                    Select Case DR_Look("NoteType").ToString
                        Case "Org"
                            TempNote = TempNote & "Org Note: " & DR_Look("Note").ToString & Chr(13) & Chr(13)
                        Case "Tech"
                            TempNote = TempNote & "Tech Note: " & DR_Look("Note").ToString & Chr(13) & Chr(13)
                    End Select
                Next
                TempNote = TempNote.Replace(Chr(34), Chr(39))
                SB.Append("title=""" & TempNote & """ ")
                If CountNotes > 1 Then
                    I = I + II - 1
                End If

            End If

            Select Case DR("Ticket Type").ToString
                Case = ""
                    SB.Append("style=""width: 250px; height: 50px; background-color: #33CC33;"" ") ' green 33CC33
                Case = "Standard"
                    SB.Append("style=""width: 250px; height: 50px; background-color: #FFFF66;"" ") 'Yellow FFFF66
                Case = "Down"
                    SB.Append("style=""width: 250px; height: 50px; background-color: Red;"" ") 'Red
            End Select


            SB.Append(" />")

        Next

        Return SB.ToString()
    End Function



    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class