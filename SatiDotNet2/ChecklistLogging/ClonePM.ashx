<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.IO
Imports System.Text.Json
Imports SatiDotNet2.Library

Public Class StreamData
    Inherits MaintPM
    Implements IHttpHandler, IReadOnlySessionState

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim HttpBody As String
        Dim AreaKeyToClone As String
        Dim NewAreaName As String
        Dim Data As Dictionary(Of String, Object)

        context.Response.ContentType = "application/json"

        Using reader As New StreamReader(context.Request.InputStream)
            HttpBody = reader.ReadToEnd()
        End Using

        Data = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(HttpBody)

        AreaKeyToClone = Data("areaKeyToClone").ToString()
        NewAreaName = Data("newAreaName").ToString()

        context.Response.Write(JsonSerializer.Serialize(ClonePM(AreaKeyToClone, NewAreaName)))
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class
