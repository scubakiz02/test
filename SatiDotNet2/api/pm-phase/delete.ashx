<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.IO
Imports System.Text.Json
Imports SatiDotNet2.Library

Public Class StreamData
    Inherits PhaseController
    Implements IHttpHandler, IReadOnlySessionState

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim HttpBody As String
        Dim PhaseKey As Integer
        Dim Data As Dictionary(Of String, Object)
        Dim PhaseDeleteRes As Dictionary(Of String, String)
        Dim HttpRes As New Dictionary(Of String, Object)

        context.Response.ContentType = "application/json"

        Using reader As New StreamReader(context.Request.InputStream)
            HttpBody = reader.ReadToEnd()
        End Using

        Data = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(HttpBody)
        PhaseKey = Data("phaseKey").ToString()
        PhaseDeleteRes = DeletePhaseOrBatch(PhaseKey)

        If PhaseDeleteRes("Success") = "True" Then
            HttpRes("success") = True
        Else
            HttpRes("success") = False
        End If
        HttpRes("message") = PhaseDeleteRes("message")

        context.Response.Write(JsonSerializer.Serialize(HttpRes))
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class
