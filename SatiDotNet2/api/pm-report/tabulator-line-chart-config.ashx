<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.Text.Json
Imports SatiDotNet2.Library

Public Class StreamData
    Implements IHttpHandler, IReadOnlySessionState

    Public Sub ProcessRequest(Context As HttpContext) Implements IHttpHandler.ProcessRequest
        Context.Response.ContentType = "application/json"

        Dim HttpRequestVars As Object = Context.Request.QueryString
        Dim LabelKey As String = HttpRequestVars("labelkey")

        Context.Response.Write(JsonSerializer.Serialize(Context.Session("Report").GetLineChartConfig(LabelKey)))
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class