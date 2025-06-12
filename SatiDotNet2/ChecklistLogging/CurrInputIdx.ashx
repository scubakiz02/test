<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.IO
Imports System.Text.Json
Imports SatiDotNet2.Library

Public Class StreamData
    Inherits PhaseController
    Implements IHttpHandler, IReadOnlySessionState

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim LabelKey As String
        Dim ResIdx As Integer
        Dim Res As New Dictionary(Of String, Object)

        context.Response.ContentType = "application/json"

        LabelKey = context.Request.QueryString("Label").ToString()
        ResIdx = GetLabel_Idx(LabelKey)

        If ResIdx >= 0 Then
            Res("success") = True
        Else
            Res("success") = False
        End If
        Res("idx") = ResIdx

        context.Response.Write(JsonSerializer.Serialize(Res))
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class
