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
        PackageDataForClient(_Cache.Get("LogStateChanges"), Context)
    End Sub

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