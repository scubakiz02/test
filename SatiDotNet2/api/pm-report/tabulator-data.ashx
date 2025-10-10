<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.Text.Json
Imports SatiDotNet2.Library

Public Class StreamData
    Implements IHttpHandler, IReadOnlySessionState

    Private _Format As New Format()
    Private _PmInput As New PmInput()

    Public Sub ProcessRequest(Context As HttpContext) Implements IHttpHandler.ProcessRequest
        Context.Response.ContentType = "application/json"

        ' GET data for tabulator instance as json
        Dim HttpRequestVars As Object = Context.Request.QueryString
        Dim GroupKey As String = HttpRequestVars("groupkey")
        Dim PmKeys As Object = ParseHttpArr(HttpRequestVars("pmKeys"))
        Dim InputKeys As Object = ParseHttpArr(HttpRequestVars("inputKeys"))
        Dim StartDateAt As String = _Format.DateNoTime(HttpRequestVars("startDateAt"))
        Dim EndDateAt As String = _Format.DateNoTime(HttpRequestVars("endDateAt"))

        Dim TabulatorConfig As List(Of Dictionary(Of String, Object)) = Context.Session("Report").GetTabulatorConfig()
        Context.Response.Write(JsonSerializer.Serialize(TabulatorConfig))
    End Sub

    Private Function ParseHttpArr(HttpArrStringified As String) As Object
        Dim Res As Object

        Try
            Res = JsonSerializer.Deserialize(Of List(Of Integer))(HttpArrStringified)
        Catch ex As Exception
            Res = Nothing
        End Try

        Return Res
    End Function

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class