<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.Web
Imports System.Threading
Imports System.Text.Json

Public Class StreamData : Implements IHttpHandler, IReadOnlySessionState

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        context.Response.BufferOutput = False

        Try 'in case there's errors with http request/response
            Dim Config As String
            Dim PastIssues As Func(Of Integer, String) = CType(context.Session("PastIssues"), Func(Of Integer, String))

            For Each AreaKey In context.Session("AreaKeys")
                If PastIssues IsNot Nothing Then
                    Config = PastIssues.Invoke(AreaKey) + "948ae0ab" 'in case http data chunk contains json for several checklists
                Else
                    Config = String.Empty
                End If

                context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(Config), 0, Config.Length)
                context.Response.Flush()
            Next
        Catch ex As HttpException
            context.Response.End()
        Finally
            context.Response.End()
        End Try
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class
