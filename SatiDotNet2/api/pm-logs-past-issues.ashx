<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.Web
Imports System.Threading
Imports System.Text.Json
Imports SatiDotNet2.Library

Public Class StreamData
    Inherits Security
    Implements IHttpHandler, IReadOnlySessionState
    Private _ActivePm As New ActivePm()

    Public Sub ProcessRequest(Context As HttpContext) Implements IHttpHandler.ProcessRequest
        Context.Response.ContentType = "application/json"

        Dim Department As String = Context.Request.QueryString("department")
        Dim View As String = Context.Request.QueryString("view")
        Dim StatusBoardDateAt As String = Context.Request.QueryString("StatusBoardDateAt")
        Dim StartDateCutoffAt As String = Context.Request.QueryString("StartDateCutoffAt")

        Dim Result As Dictionary(Of Integer, Dictionary(Of String, Object)) = GetOverdueLogsConfig(StartDateCutoffAt, StatusBoardDateAt, Department, View)
        Context.Response.Write(JsonSerializer.Serialize(Result))
    End Sub

    Private Function GetOverdueLogsConfig(StartDateCutoffAt As String, StatusBoardDateAt As String, Department As String, View As String) As Dictionary(Of Integer, Dictionary(Of String, Object))
        Dim Res As New Dictionary(Of Integer, Dictionary(Of String, Object))

        If StatusBoardDateAt Is Nothing Then StatusBoardDateAt = System.DateTime.Now.ToString("MM/dd/yyyy")
        If View = "Focus" Then Return Res

        Dim QueryConfig1 As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@Department", GetParamVarHash(Department, "string")}
        }
        Dim AreaKeyDs As Data.DataSet = GetMyDataSetParamQuery("SELECT A.[Key] As Areakey FROM [ALTS].[dbo].[T_LogArea] A INNER JOIN [ALTS].[dbo].[T_LogDepartment] D ON A.DepartmentKey=D.[Key] WHERE A.Status='live' AND D.Department=@Department", QueryConfig1)
        For Each AreaKeyDr As Data.DataRow In AreaKeyDs.Tables(0).Rows
            Dim AreaKey As Integer = AreaKeyDr("Areakey")
            Dim QueryConfig2 As New Dictionary(Of String, Dictionary(Of String, String)) From {
                {"@StartDateCutoffAt", GetParamVarHash(StartDateCutoffAt, "string")},
                {"@Where", GetParamVarHash(StatusBoardDateAt, "string")},
                {"@AreaKey", GetParamVarHash(AreaKey, "int")},
                {"@CurrLogDate", GetParamVarHash(_ActivePm.GetCurrLogDate(AreaKey, StatusBoardDateAt), "string")}
            }
            Dim DS As Data.DataSet = GetMyDataSetParamQuery("SELECT D.[Key], D.Date, D.Operator, D.Inputs, " &
                                   "A.Area, A.Assignee, " &
                                   "Sql.LogStatus, Sql.StripeColor, Sql.NumOfStamps, Sql.NumOfNeededStamps " &
                                   "FROM [ALTS].[dbo].[T_LogData] D " &
                                   "INNER JOIN [ALTS].[dbo].[T_LogArea] A ON D.AreaKey=A.[Key] " &
                                   "CROSS APPLY [ALTS].[dbo].[T_Log_ChecklistRecordInfo](D.[Key], 1, (SELECT Date FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=D.[Key])) Sql " &
                                   "WHERE " &
                                   "D.Date > @StartDateCutoffAt " &
                                   "AND D.AreaKey=@AreaKey " &
                                   "AND (D.[Key] <> (SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=@AreaKey AND CAST(D.Date As Date) < @CurrLogDate ORDER BY DATE DESC) " &
                                   "     OR (A.OneTimeDate IS NOT NULL AND @Where > D.Date)) " &
                                   "AND (D.CompleteLog <> 1 OR Sql.NumOfStamps < Sql.NumOfNeededStamps) " &
                                   "ORDER BY Date ASC", QueryConfig2)

            If DS IsNot Nothing Then
                'overdue logs exist
                For Each DR As Data.DataRow In DS.Tables(0).Rows
                    Dim Datakey As Integer = DR("Key")
                    Res(Datakey) = _ActivePm.GetLogConfig(Datakey, StatusBoardDateAt)
                Next
            End If
        Next

        Return Res
    End Function

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class
