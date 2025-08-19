<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.Threading
Imports System.Text.Json
Imports SatiDotNet2.Library
Imports System.Runtime.Caching

Public Class StreamData
    Inherits ActivePmCache
    Implements IHttpHandler, IReadOnlySessionState

    Private ReadOnly _Cache As MemoryCache = MemoryCache.Default
    Private _LastValue As Object

    Public Sub ProcessRequest(Context As HttpContext) Implements IHttpHandler.ProcessRequest
        Context.Response.ContentType = "application/json"

        If isMidnight() Then
            'force client refresh
            Context.Response.Clear()
            Context.Response.Write("{""refreshPage"": true}")
            Exit Sub
        End If

        PackageDataForClient(_Cache.Get("LogStateChanges"), Context)
    End Sub

    Private Function isMidnight() As Boolean
        'Dim Now As DateTime = DateTime.Parse("2025-08-19 00:00:04") 'for troubleshooting/testing
        Dim Now As DateTime = System.DateTime.Now()
        Dim Hour As Integer = Now.Hour
        Dim Minute As Integer = Now.Minute

        If Hour > 0 Then Return False 'not the 1st hour (midnight) of the day

        If Minute > 0 Then Return False 'not the 1st minute of the hour

        Return True
    End Function

    Private Sub PackageDataForClient(Data As Object, Context As HttpContext)
        Dim Json As Dictionary(Of Integer, Object) = DirectCast(Data, Dictionary(Of Integer, Object))

        If Json Is Nothing Then
            Json = New Dictionary(Of Integer, Object)
        End If

        Context.Response.Write(JsonSerializer.Serialize(Json))
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class