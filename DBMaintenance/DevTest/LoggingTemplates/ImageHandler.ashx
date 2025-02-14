<%@ WebHandler Language="VB" Class="ImageHandler" %>

Imports System
Imports System.Web
Imports System.Web.SessionState
Imports System.IO

Public Class ImageHandler : Implements IHttpHandler, IRequiresSessionState
    Dim BasePath As String = "\\pwi-40\IT$\DevCopy\ST\SatiPhotoLogs\"
    Dim SatiCode As New Class1

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim ImagePath As String = context.Request.QueryString("PhotoFilePath")
        Dim ImageInBinary As Byte()

        If System.IO.File.Exists(ImagePath) Then
            ImageInBinary = System.IO.File.ReadAllBytes(ImagePath)
        Else
            ImageInBinary = System.IO.File.ReadAllBytes(BasePath & "ImageNotFound.jpg") ' Return a default image if not found
        End If

        'SatiCode.Get_WaferBoxs_Per_ShippingBox("image/" & context.Request.QueryString("ContentType")) 'for debugging, since breakpoints are wonky for .ashx files

        context.Response.ContentType = "image/" & context.Request.QueryString("ContentType")
        context.Response.BinaryWrite(ImageInBinary)
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Function GetSingleDbField(SqlQuery As String, Field As String) As String
        Dim Res As String

        'using try catch block in case 'There is no row at position 0.', which means there are no associated record in Table
        Try
            Res = If(IsDBNull(SatiCode.GetMyDataSet(SqlQuery).Tables(0).Rows(0)(Field)), Nothing, SatiCode.GetMyDataSet(SqlQuery).Tables(0).Rows(0)(Field)) 'using ternary operator as a workaround to Null DB field values, which in that case the function will return Nothing
        Catch ex As Exception
            Res = Nothing
        End Try

        Return Res
    End Function

End Class
