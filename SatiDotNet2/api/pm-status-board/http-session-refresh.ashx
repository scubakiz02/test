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

        Try
            ' Explicitly read the session to ensure ASP.NET SessionStateModule loads the session 
            ' (this updates the session's last-accessed time and renews sliding expiration).
            Dim ReadSession As String = Context.Session.SessionID
        Catch ex As Exception
            ' ignore - just defensive in case session is not available
        End Try

        Context.Response.Write(JsonSerializer.Serialize(New Dictionary(Of String, String)))
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class