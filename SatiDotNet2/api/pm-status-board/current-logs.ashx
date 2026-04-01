<%@ WebHandler Language="VB" Class="StreamData" %>

Imports System.Threading
Imports System.Text.Json
Imports SatiDotNet2.Library
Imports System.Runtime.Caching

Public Class StreamData
    Inherits Security
    Implements IHttpHandler, IReadOnlySessionState
    Private _ActivePm As New ActivePm()
    Private _Security As New Security()
    Private _LogAspx As New LogAspxLibrary()

    Public Sub ProcessRequest(Context As HttpContext) Implements IHttpHandler.ProcessRequest
        Context.Response.ContentType = "application/json"

        Dim QueryStrings = Context.Request.QueryString
        Dim StatusBoardDateAt As String = QueryStrings("statusBoardDateAt")
        Dim Department As String = QueryStrings("department")

        CreateDbRecords(StatusBoardDateAt, Department)

        Dim HttpRes As List(Of KeyValuePair(Of Integer, Dictionary(Of String, Object))) = GetCurrentLogs(StatusBoardDateAt, Department)
        'Dim HttpRes As List(Of KeyValuePair(Of Integer, Dictionary(Of String, Object))) = GetCurrentLogsFake() 'for troubleshooting/debugging

        Context.Response.Write(JsonSerializer.Serialize(HttpRes))
    End Sub

    Private Function GetCurrentLogsDs(StatusBoardDateAt As String, Department As String) As Data.DataSet
        'this dataset retrieves all records for pm/checklist logs that are current relevant to the status board date
        'this means the pm/checklist must:
        '   1) Be Active
        '   2) Be Live
        '   3) Be a part of the department the end user has specified
        '   4) Have 1 input or more
        '   5) Have an assignee (Ex: D1, N1, etc.)

        Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
          {"@StatusBoardDateAt", GetParamVarHash(StatusBoardDateAt, "string")},
          {"@Department", GetParamVarHash(Department, "string")}
    }
        Dim Ds As Data.DataSet = GetMyDataSetParamQuery("SELECT A.[Key] As AreaKey, " &
            "(SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogData] WHERE Date <= @StatusBoardDateAt And AreaKey=A.[Key] ORDER BY Date DESC ) As CurrentLogDataKey " &
            "FROM [ALTS].[dbo].[T_LogArea] A " &
            "INNER JOIN [ALTS].[dbo].[T_LogDepartment] Dep On A.DepartmentKey=Dep.[Key] " &
            "WHERE A.Active=1 AND A.Status='live' AND Dep.Department=@Department AND EXISTS (SELECT 1 FROM [ALTS].[dbo].[T_LogLabel] L WHERE L.AreaKey = A.[Key]) AND A.Assignee IS NOT NULL " &
            "ORDER BY A.Area", SqlConfig)

        Return Ds
    End Function

    Private Sub CreateDbRecords(StatusBoardDateAt As String, Department As String)
        Dim Ds As Data.DataSet = GetCurrentLogsDs(StatusBoardDateAt, Department)
        For Each Dr As Data.DataRow In Ds.Tables(0).Rows
            Dim AreaKey As Integer = Dr("AreaKey")
            Dim LogStartDateAt As String = _ActivePm.GetCurrLogDate(AreaKey, StatusBoardDateAt)
            If DoesLogExistInDB(AreaKey, LogStartDateAt) = False Then
                'the purpose of this if condition is to prevent sql identity gaps, which is big jumps for the primary key of table records (Ex: last id 15315, next inserted id 21635)
                'the table listed also has a unique constraint on the AreaKey and Date columns, in case this logic fails
                CreateDbRecord(AreaKey, LogStartDateAt)
            End If
        Next
    End Sub

    Private Function DoesLogExistInDB(AreaKey As Integer, LogStartDateAt As String) As Boolean
        Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@AreaKey", _Security.GetParamVarHash(AreaKey, "int")},
            {"@LogStartDateAt", _Security.GetParamVarHash(LogStartDateAt, "string")}
        }
        Dim DbRecordCount As Integer = Integer.Parse(_Security.GetSingleDbField("SELECT COUNT(*) As DoesLogExistInDB " &
          "FROM [ALTS].[dbo].[T_LogData] " &
          "WHERE AreaKey=@AreaKey AND Date=@LogStartDateAt", SqlConfig, "DoesLogExistInDB"))

        If DbRecordCount = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Sub CreateDbRecord(AreaKey As Integer, LogStartDateAt As String)
        Dim Connection As New Data.SqlClient.SqlConnection
        Connection.ConnectionString = ConfigurationManager.ConnectionStrings("ALTSConnectionString").ConnectionString
        Connection.Open()

        Dim My_DA As New Data.SqlClient.SqlDataAdapter
        Dim My_DS As New Data.DataSet
        Dim My_DR As Data.DataRow
        Dim My_DS2 As New Data.DataSet
        Dim RC As Integer
        Dim My_DR2 As Data.DataRow
        Dim InputsMap As New Dictionary(Of Integer, String)
        Dim OutOfRangeMap As New Dictionary(Of Integer, String)
        Dim MapKey As String
        '*****************************************************************
        '************************Select***********************************
        '*****************************************************************
        Dim MySelectCmd As New System.Data.SqlClient.SqlCommand
        With MySelectCmd
            .CommandText = "SELECT Top(1) * FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=" & AreaKey & " ORDER BY Date DESC"
            .Connection = Connection
        End With
        My_DA.SelectCommand = MySelectCmd

        '*****************************************************************
        '************************Insert***********************************
        '*****************************************************************
        Dim MyInsertCmd As New System.Data.SqlClient.SqlCommand
        With MyInsertCmd
            .CommandText = "INSERT INTO T_LogData (AreaKey, Inputs, OutOfRange, Date, Operator, Shift, CompleteLog, ManagerStamp1, ManagerStamp2, ManagerStamp3, ToolNumber, Active) VALUES (@AreaKey, @Inputs, @OutOfRange, @Date, @Operator, @Shift, @CompleteLog, NULL, NULL, NULL, NULL, 'False')"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@AreaKey", System.Data.SqlDbType.Int, 0, "AreaKey"), New System.Data.SqlClient.SqlParameter("@Inputs", System.Data.SqlDbType.VarChar, 0, "Inputs"), New System.Data.SqlClient.SqlParameter("@OutOfRange", System.Data.SqlDbType.VarChar, 0, "OutOfRange"), New System.Data.SqlClient.SqlParameter("@Date", System.Data.SqlDbType.SmallDateTime, 0, "Date"), New System.Data.SqlClient.SqlParameter("@Operator", System.Data.SqlDbType.VarChar, 0, "Operator"), New System.Data.SqlClient.SqlParameter("@Shift", System.Data.SqlDbType.VarChar, 0, "Shift"), New System.Data.SqlClient.SqlParameter("@CompleteLog", System.Data.SqlDbType.Bit, 0, "CompleteLog")})
        End With
        My_DA.InsertCommand = MyInsertCmd

        '*****************************************************************
        '************************Update***********************************
        '*****************************************************************
        Dim MyUpdateCmd As New System.Data.SqlClient.SqlCommand
        With MyUpdateCmd
            .CommandText = "UPDATE T_LogData SET [Inputs] = @Inputs, [OutOfRange] = @OutOfRange, [Date] = @Date, [Operator] = @Operator, [CompleteLog] = @CompleteLog WHERE [Key]=@DataLogKey; SELECT TOP(1) * FROM T_LogData WHERE AreaKey=" & AreaKey & " ORDER BY Date DESC;"
            .Connection = Connection
            .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Inputs", System.Data.SqlDbType.VarChar, 0, "Inputs"), New System.Data.SqlClient.SqlParameter("@OutOfRange", System.Data.SqlDbType.VarChar, 0, "OutOfRange"), New System.Data.SqlClient.SqlParameter("@Date", System.Data.SqlDbType.SmallDateTime, 0, "Date"), New System.Data.SqlClient.SqlParameter("@Operator", System.Data.SqlDbType.VarChar, 0, "Operator"), New System.Data.SqlClient.SqlParameter("@CompleteLog", System.Data.SqlDbType.Bit, 0, "CompleteLog"), New System.Data.SqlClient.SqlParameter("@DataLogKey", System.Data.SqlDbType.Int, 0, "Key")})
        End With
        My_DA.UpdateCommand = MyUpdateCmd

        '*****************************************************************
        '************************Delete***********************************
        '*****************************************************************
        'Dim MyDeleteCmd As New System.Data.SqlClient.SqlCommand
        'With MyDeleteCmd
        '    .CommandText = "DELETE FROM [aspnet_UsersInRoles] WHERE (([UserId] = @Original_UserId) AND ([RoleId] = @Original_RoleId))"
        '    .Connection = Connection
        '    .Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Original_UserId", System.Data.SqlDbType.UniqueIdentifier, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "UserId", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_RoleId", System.Data.SqlDbType.UniqueIdentifier, 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "RoleId", System.Data.DataRowVersion.Original, Nothing)})
        'End With
        'My_DA.DeleteCommand = MyDeleteCmd

        '*****************************************************************
        '************************Genral***********************************
        '*****************************************************************
        My_DA.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "T_LogData", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("AreaKey", "AreaKey"), New System.Data.Common.DataColumnMapping("Inputs", "Inputs"), New System.Data.Common.DataColumnMapping("OutOfRange", "OutOfRange"), New System.Data.Common.DataColumnMapping("Date", "Date"), New System.Data.Common.DataColumnMapping("Operator", "Operator"), New System.Data.Common.DataColumnMapping("Shift", "Shift"), New System.Data.Common.DataColumnMapping("CompleteLog", "CompleteLog")})}) 'the fields that are dynamically generated
        My_DA.Fill(My_DS)

        'in case of db upload failure, closing code below in a try catch block
        Try
            My_DR = My_DS.Tables("T_LogData").NewRow
            My_DR("AreaKey") = AreaKey

            Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
                {"@AreaKey", _Security.GetParamVarHash(AreaKey, "int")}
            }
            My_DS2 = _Security.GetMyDataSetParamQuery("SELECT [Key] FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=@AreaKey", SqlConfig)
            RC = My_DS2.Tables(0).Rows.Count

            For I = 0 To RC - 1
                My_DR2 = My_DS2.Tables(0).Rows(I)
                MapKey = My_DR2("Key")
                InputsMap.Add(MapKey, "")
                OutOfRangeMap.Add(MapKey, Nothing)
            Next

            My_DR("OutOfRange") = JsonSerializer.Serialize(OutOfRangeMap)
            My_DR("Date") = LogStartDateAt
            My_DR("Operator") = Nothing
            My_DR("CompleteLog") = False
            My_DR("Shift") = _Security.GetSingleDbField("SELECT Shift FROM [ALTS].[dbo].[T_Log_GetShift]()", New Dictionary(Of String, Dictionary(Of String, String)), "Shift")
            My_DR("Inputs") = JsonSerializer.Serialize(InputsMap) 'old format (date & operator are NOT recorded for each input)
            My_DR("Inputs") = JsonSerializer.Serialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(_LogAspx.GetInputs(My_DR)) 'new format (date & operator are recorded for each input)
            My_DS.Tables("T_LogData").Rows.Add(My_DR)
            My_DA.Update(My_DS, "T_LogData")
        Catch ex As Exception
            Dim CatchErr As String = ex.Message.ToString()
            Dim Placeholder As String = "Yay"
        End Try

        Connection.Close()
    End Sub

    Private Function GetCurrentLogs(StatusBoardDateAt As String, Department As String) As List(Of KeyValuePair(Of Integer, Dictionary(Of String, Object)))
        Dim CurrLogsDs As Data.DataSet = GetCurrentLogsDs(StatusBoardDateAt, Department)

        Dim Res As New List(Of KeyValuePair(Of Integer, Dictionary(Of String, Object)))
        For Each Dr As Data.DataRow In CurrLogsDs.Tables(0).Rows
            Dim DataKey As Integer = Convert.ToInt32(Dr("CurrentLogDataKey"))
            Dim LogConfig As Dictionary(Of String, Object) = _ActivePm.GetLogConfig(DataKey, StatusBoardDateAt)
            Res.Add(New KeyValuePair(Of Integer, Dictionary(Of String, Object))(DataKey, LogConfig))
        Next

        Return Res
    End Function

    Private Function GetCurrentLogsFake() As List(Of KeyValuePair(Of Integer, Dictionary(Of String, Object)))
        Dim Res As New List(Of KeyValuePair(Of Integer, Dictionary(Of String, Object)))

        Res.Add(New KeyValuePair(Of Integer, Dictionary(Of String, Object))(15309, New Dictionary(Of String, Object) From {
            {"logState", "completed"},
            {"logParentId", "DailyDayShiftPanel"},
            {"pmName", "AWN Daily"}
        }))

        Res.Add(New KeyValuePair(Of Integer, Dictionary(Of String, Object))(15312, New Dictionary(Of String, Object) From {
            {"logState", "completed"},
            {"logParentId", "DailyDayShiftPanel"},
            {"pmName", "SC-1 Fume Scrubber Monitoring Daily"}
        }))

        Res.Add(New KeyValuePair(Of Integer, Dictionary(Of String, Object))(15315, New Dictionary(Of String, Object) From {
            {"logState", "completed"},
            {"logParentId", "DailyNightShiftPanel"},
            {"pmName", "Daily Fluoride Measurement Log"}
        }))

        Res.Add(New KeyValuePair(Of Integer, Dictionary(Of String, Object))(15308, New Dictionary(Of String, Object) From {
            {"logState", "completed"},
            {"logParentId", "DailyMFShiftPanel"},
            {"pmName", "Nitrogen Daily"}
        }))

        Res.Add(New KeyValuePair(Of Integer, Dictionary(Of String, Object))(15310, New Dictionary(Of String, Object) From {
            {"logState", "completed"},
            {"logParentId", "DailyMFShiftPanel"},
            {"pmName", "R.O. Daily"}
        }))

        Res.Add(New KeyValuePair(Of Integer, Dictionary(Of String, Object))(15311, New Dictionary(Of String, Object) From {
            {"logState", "completed"},
            {"logParentId", "DailyMFShiftPanel"},
            {"pmName", "DI WATER DAILY"}
        }))

        Res.Add(New KeyValuePair(Of Integer, Dictionary(Of String, Object))(15313, New Dictionary(Of String, Object) From {
            {"logState", "completed"},
            {"logParentId", "DailyMFShiftPanel"},
            {"pmName", "SC-2 Fume Scrubber Monitoring Daily"}
        }))

        Return Res
    End Function
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class