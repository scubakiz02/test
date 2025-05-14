<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.Web
Imports System.Threading
Imports System.Text.Json

Public Class StreamData : Implements IHttpHandler, IReadOnlySessionState

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.Clear()
        context.Response.ContentType = "application/json"
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        context.Response.Buffer = False

        Try 'in case there's errors with http request/response
            Dim Config As New SortedDictionary(Of String, Dictionary(Of Integer, Dictionary(Of String, String)))
            Dim PastIssues As Func(Of Integer, SortedDictionary(Of String, Dictionary(Of Integer, Dictionary(Of String, String)))) = CType(context.Session("PastIssues"), Func(Of Integer, SortedDictionary(Of String, Dictionary(Of Integer, Dictionary(Of String, String)))))

            For Each AreaKey In context.Session("AreaKeys")
                If PastIssues IsNot Nothing Then
                    'added hash to end of stringin case http data chunk contains json for several checklists
                    Config = PastIssues.Invoke(AreaKey)

                    'streaming json return in chunks Using a 1KB Buffer
                    'using a 1KB buffer rather than the standard of a 4KB Buffer to match production environment
                    For Each PastIssuesControls As KeyValuePair(Of String, Dictionary(Of Integer, Dictionary(Of String, String))) In Config

                        'breaking data chunks into the smallest possible increments to ensure complete json objects are passed to the client
                        For Each PastIssuesControl As KeyValuePair(Of Integer, Dictionary(Of String, String)) In PastIssuesControls.Value
                            Dim JsonBytes() As Byte = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(PastIssuesControl) & vbLf) 'delimit increments by a newline (\n OR vbLf)
                            Dim TotalLength As Integer = JsonBytes.Length
                            Dim BufferSize As Integer = 1024
                            Dim Offset As Integer = 0

                            While Offset < TotalLength
                                Dim ChunkSize As Integer = Math.Min(BufferSize, TotalLength - Offset)

                                context.Response.OutputStream.Write(JsonBytes, Offset, ChunkSize)
                                context.Response.Flush() ' send chunk

                                Offset += ChunkSize
                            End While
                        Next
                    Next

                End If

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
