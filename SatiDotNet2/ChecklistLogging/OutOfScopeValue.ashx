<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.IO
Imports System.Text.Json
Imports SatiDotNet2.Library
Imports System.Data

Public Class StreamData
    Inherits Security
    Implements IHttpHandler, IReadOnlySessionState

    Private Format As New Format()
    Private LogAspx As New LogAspxLibrary()
    Private ActivePm As New ActivePm()
    Private KeyFromQueryString As Integer

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim Res As New Dictionary(Of String, Object)
        Dim JsonString As String
        Dim Json As New Dictionary(Of String, Object)

        context.Response.ContentType = "application/json"

        Using reader As New StreamReader(context.Request.InputStream)
            JsonString = reader.ReadToEnd()
        End Using
        Json = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(JsonString)

        KeyFromQueryString = Json("dataId").ToString()
        Dim LabelKey As Integer = Json("labelId").ToString()
        Dim IsValid As Boolean = Json("value").ToString()

        'in case queries executed in LabelValueValidity fail, execute them again
        Try
            Res = LabelValueValidity(IsValid, KeyFromQueryString, LabelKey)
        Catch ex As Exception
            Try
                Res = LabelValueValidity(IsValid, KeyFromQueryString, LabelKey)
            Catch ex2 As Exception
                Res("Success") = False
            End Try
        End Try

        'change Res 'Success' key to 'success' (for client side uniformity)
        Res("success") = Res("Success")
        Res.Remove("Success")
        context.Response.Write(JsonSerializer.Serialize(Res))
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Function LabelValueValidity(IsValid As Boolean, DataKey As Integer, LabelKey As Integer) As Dictionary(Of String, Object)
        Dim OutOfRangeJson As Dictionary(Of String, Object) = ActivePm.GetOutOfRange(DataKey)

        OutOfRangeJson(LabelKey) = IsValid

        Return ActivePm.SetOutOfRange(DataKey, JsonSerializer.Serialize(OutOfRangeJson))
    End Function
End Class
