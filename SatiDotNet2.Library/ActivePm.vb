Imports System.Drawing
Imports System.Text.Json
Imports System.Web
Imports SatiDotNet2.Library

Public Class ActivePm
    Inherits Security

    Private _LogAspxLibrary As New LogAspxLibrary()

    Public Function GetOutOfRange(DataKey As String) As Dictionary(Of String, Object)
        Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@DataKey", GetParamVarHash(DataKey, "int")}
        }
        Dim OutOfRangeStringified As String = GetSingleDbField("SELECT OutOfRange FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=@DataKey", SqlConfig, "OutOfRange")
        Return JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(OutOfRangeStringified)
    End Function

    Public Function SetOutOfRange(DataKey As Integer, NewOutOfRange As String) As Dictionary(Of String, Object)
        Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@DataKey", GetParamVarHash(DataKey, "int")},
            {"@OutOfRange", GetParamVarHash(NewOutOfRange, "varchar")}
        }
        Dim Res As New Dictionary(Of String, Object)

        Res = ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogData] SET OutOfRange=@OutOfRange WHERE [Key]=@DataKey", SqlConfig)
        Res.Remove("PrimaryKey")

        Return Res
    End Function

    Public Function GetLogConfig(DataKey As Integer, Optional StatusBoardDateAt As String = Nothing, Optional FakeStampDs As Data.DataSet = Nothing, Optional FakeLogDs As Data.DataSet = Nothing) As Dictionary(Of String, Object)
        Dim Res As New Dictionary(Of String, Object)
        Dim StampDs As Data.DataSet
        Dim LogState As String
        Dim LogDataDr As Data.DataRow
        Dim LogParentId As String

        If StatusBoardDateAt Is Nothing Then StatusBoardDateAt = System.DateTime.Now().ToString("MM/dd/yyyy")

        If FakeStampDs Is Nothing OrElse FakeLogDs Is Nothing Then
            Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
                {"@DataKey", GetParamVarHash(DataKey, "int")}
            }

            StampDs = GetMyDataSetParamQuery("Select ST.Title As StampTitle, " &
                "CASE WHEN SL.Active=1 Then 1 " &
                "Else 0 " &
                "End As IsStampActive, " &
            "S.Date As StampDateTime " &
            "From [ALTS].[dbo].[T_LogStamp] S " &
            "RIGHT Join [ALTS].[dbo].[T_LogStampList] SL On SL.[Key]=S.StampKey And S.Active=1 And DataRecordKey=@DataKey " &
            "INNER Join [ALTS].[dbo].[T_LogStampTitle] ST On SL.TitleKey=ST.[Key] " &
            "WHERE SL.AreaKey = (SELECT AreaKey FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=@DataKey) ORDER BY S.Date DESC", SqlConfig)

            LogDataDr = GetMyDataSetParamQuery("SELECT " &
                "CASE WHEN [Key]=(SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogData] SubD WHERE SubD.AreaKey=D.AreaKey ORDER BY Date DESC) Then 1 " &
                "Else 0 " &
                "End As IsLogNewest, CompleteLog As IsLogComplete, " &
                    "CASE WHEN (SELECT COUNT(*) FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=D.AreaKey AND Date=D.Date) > 1 Then 1 " &
                "Else 0 " &
                "End As IsLogDuplicated, " &
                "(SELECT Active FROM [ALTS].[dbo].[T_LogArea] WHERE [Key]=D.AreaKey) As IsActive, " &
                "(SELECT Area FROM [ALTS].[dbo].[T_LogArea] WHERE [Key]=D.AreaKey) As Area, " &
                "Inputs " &
                "FROM [ALTS].[dbo].[T_LogData] D WHERE [Key]=@DataKey", SqlConfig).Tables(0).Rows(0)

            LogParentId = GetParentId(DataKey, StatusBoardDateAt)
        Else
            StampDs = FakeStampDs
            LogDataDr = FakeLogDs.Tables(0).Rows(0)
            LogParentId = GetParentId(0, StatusBoardDateAt, FakeLogDs)
        End If

        Try
            Dim IsLogDuplicated As Boolean = LogDataDr("IsLogDuplicated")
            Dim IsLogComplete As Boolean = LogDataDr("IsLogComplete")
            Dim IsActive As Boolean = LogDataDr("IsActive")

            If IsActive = False Then Throw New InactiveLogException()
            If IsLogDuplicated Then Throw New DuplicateLogException()

            If IsLogComplete Then
                Dim AddStamps As New List(Of String)
                Dim RemoveStamps As New List(Of String)

                For Each StampDR As Data.DataRow In StampDs.Tables(0).Rows
                    Dim StampTitle As Object = StampDR("StampTitle")
                    Dim StampDateTime As Object = StampDR("StampDateTime")
                    Dim IsStampActive As Boolean = StampDR("IsStampActive")

                    If IsStampActive AndAlso IsDBNull(StampDateTime) Then
                        AddStamps.Add(StampTitle)
                    Else
                        RemoveStamps.Add(StampTitle)
                    End If
                Next

                'evaluate log state (submitted, completed, or delete)
                If AddStamps.Count = 0 Then
                    Dim IsLogNewest As Boolean = LogDataDr("IsLogNewest")
                    If IsLogNewest Then
                        'log completed on time
                        LogState = "completed"
                    Else
                        Throw New OverdueLogCompleted()
                    End If
                Else
                    LogState = "submitted"
                    Res("addStamps") = AddStamps
                    Res("removeStamps") = RemoveStamps
                End If
            Else
                Dim Inputs As String = LogDataDr("Inputs")
                Dim EveryInputEmpty As Boolean = _LogAspxLibrary.IsEveryInputEmpty(Inputs)

                If EveryInputEmpty Then
                    LogState = "virgin"
                Else
                    LogState = "incomplete"
                End If
            End If
        Catch ex As OverdueLogCompleted
            Res("logState") = "delete"
            Return Res
        Catch ex As InactiveLogException
            Res("logState") = "delete"
            Return Res
        Catch ex As DuplicateLogException
            LogState = "error"
        Catch ex As Exception
            'this block catches errors that are not accounted for
            LogState = "error"
        End Try

        Res("logState") = LogState
        Res("logParentId") = LogParentId
        Res("pmName") = LogDataDr("Area")

        Return Res
    End Function

    Public Function GetParentId(DataKey As String, StatusBoardDateAt As Date, Optional FakeDs As Data.DataSet = Nothing) As String
        Dim Ds As Data.DataSet
        Dim IsBuildTime As Boolean

        If FakeDs Is Nothing Then
            Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
                {"@DataKey", GetParamVarHash(DataKey, "int")},
                {"@StatusBoardDateAt", GetParamVarHash(StatusBoardDateAt.ToString("MM/dd/yyyy"), "string")}
            }

            'several queries exist here
            'the 1st & 2nd query extract data need to call sql function needed for IsBuildTime field value
            'can refactor later if needed
            'To do this, create a vb.net function that mimics the sql functions listed in T_LogAreaInterval SqlFunc field values
            'call this new routine rather than the 1st & 2nd sql functions
            Dim SqlFuncDs As Data.DataSet = GetMyDataSetParamQuery("SELECT
                A.[Key] As AreaKey, AI.SqlFunc, AI.SqlFunc2ndArg As SqlFuncIntervalMultiplier
                From [ALTS].[dbo].[T_LogData] D
                INNER JOIN [ALTS].[dbo].[T_LogArea] A ON D.AreaKey=A.[Key]
                INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] AI ON A.IntervalKey=AI.[Key]
                WHERE D.[Key]=@DataKey", SqlConfig)
            Try
                'the sql functions only return BuildInStatusBoard column when the function interval > Monthly
                'accounting for this with a try-catch block
                Dim AreaKey As Integer = SqlFuncDs.Tables(0).Rows(0)("AreaKey")
                Dim SqlFunc As String = SqlFuncDs.Tables(0).Rows(0)("SqlFunc")
                Dim SqlFuncIntervalMultiplier As Integer = SqlFuncDs.Tables(0).Rows(0)("SqlFuncIntervalMultiplier")

                SqlConfig("@AreaKey") = GetParamVarHash(AreaKey, "int")
                SqlConfig("@SqlFuncIntervalMultiplier") = GetParamVarHash(SqlFuncIntervalMultiplier, "int")

                IsBuildTime = GetSingleDbField("SELECT BuildInStatusBoard FROM [ALTS].[dbo]." & SqlFunc & "(@AreaKey, @SqlFuncIntervalMultiplier, @StatusBoardDateAt)", SqlConfig, "BuildInStatusBoard")

                SqlConfig.Remove("@AreaKey")
                SqlConfig.Remove("@SqlFuncIntervalMultiplier")
            Catch ex As Exception
                IsBuildTime = False
            End Try

            'finally done with the sql function mess!!!!
            'Now get the data needed to determine parent panel id
            Ds = GetMyDataSetParamQuery("SELECT 
                D.Date As LogStartDateAt, AI.Interval, A.Assignee As AssignedTo,
                    CASE WHEN D.[Key]=(SELECT TOP(1) [Key] FROM [ALTS].[dbo].[T_LogData] SubD WHERE SubD.AreaKey=D.AreaKey AND SubD.Date <= @StatusBoardDateAt ORDER BY Date DESC, [Key] DESC) Then 1
	                Else 0 End As 
                IsLogNewest
                FROM [ALTS].[dbo].[T_LogData] D
                INNER JOIN [ALTS].[dbo].[T_LogArea] A ON D.AreaKey=A.[Key]
                INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] AI ON A.IntervalKey=AI.[Key]
                WHERE D.[Key]=@DataKey", SqlConfig)
        Else
            Ds = FakeDs
            IsBuildTime = Ds.Tables(0).Rows(0)("IsBuildTime") 'ugly, but works with IsBuildTime declaration outside of this If block
        End If

        'I know these are long and obnoxious case statements
        'if you want to refactor:
        '   1) provide an overlay for pm-build (ChecklistBuild.aspx) Interval ddl options first
        '   2) then go and modify T_LogAreaInterval DB field values
        Dim Dr As Data.DataRow = Ds.Tables(0).Rows(0)
        Dim AssignedTo As Object = Dr("AssignedTo")
        Dim Interval As String = Dr("Interval")
        Dim StringBuild As String = String.Empty

        Dim IsLogNewest As Boolean = Dr("IsLogNewest")
        If IsLogNewest = False Then Return "PastIssuesPanel"

        Select Case Interval
            Case "ONE TIME ONLY"
                Dim LogStartDateAt As Date = Date.Parse(Dr("LogStartDateAt")).Date
                If StatusBoardDateAt.Date = LogStartDateAt Then
                    StringBuild += "OneTime"
                Else
                    Return String.Empty
                End If
            Case "DAILY"
                StringBuild += "Daily"
            Case "WEEKLY"
                StringBuild += "Weekly"
            Case "MONTHLY"
                StringBuild += "Monthly"
            Case Else
                ' Interval > Monthly. Ex: 1 year, 2 years, 5 years, etc.
                If IsBuildTime = False Then Return String.Empty

                Select Case Interval
                    Case "QUARTERLY"
                        Return "QuarterlyPanel"
                    Case "BIANNUAL"
                        Return "BiAnnualPanel"
                    Case "1 YEAR"
                        Return "OneYearPanel"
                    Case "2 YEARS"
                        Return "TwoYearPanel"
                    Case "3 YEARS"
                        Return "ThreeYearPanel"
                    Case "4 YEARS"
                        Return "FourYearPanel"
                    Case "5 YEARS"
                        Return "FiveYearPanel"
                End Select
        End Select

        Select Case AssignedTo
            Case "Day Shift"
                StringBuild += "DayShift"
            Case "Night Shift"
                StringBuild += "NightShift"
            Case "Days (M-F)"
                StringBuild += "MFShift"
            Case "D1"
                StringBuild += "D1"
            Case "N1"
                StringBuild += "N1"
            Case "D2"
                StringBuild += "D2"
            Case "N2"
                StringBuild += "N2"
            Case Else 'User
                StringBuild += "Users"
        End Select

        Return StringBuild + "Panel"
    End Function

    Public Function GetCurrLogDate(AreaKey As Integer, StatusBoardDateAt As String) As String
        'what is CurrLogDate? Great question!
        'the status board has a time travel feature (available to Sati admins only, intended for dev use to troubleshoot/debug)
        'and so, CurrLogDate is the start date of a pm/checklist log relevant to the status board time

        If StatusBoardDateAt Is Nothing Then StatusBoardDateAt = System.DateTime.Now.ToString("MM/dd/yyyy")

        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@AreaKey", GetParamVarHash(AreaKey, "int")}
        }
        Dim SqlFuncDr As Data.DataRow = GetMyDataSetParamQuery("SELECT I.SqlFunc, I.SqlFunc2ndArg FROM [ALTS].[dbo].[T_LogArea] A INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE A.[Key]=@AreaKey", QueryConfig).Tables(0).Rows(0)
        Dim SqlFunc As String = SqlFuncDr("SqlFunc")
        Dim SqlFuncIntervalMultiplier As String = SqlFuncDr("SqlFunc2ndArg")

        QueryConfig("@SqlFuncIntervalMultiplier") = GetParamVarHash(SqlFuncIntervalMultiplier, "int")
        QueryConfig("@Where") = GetParamVarHash(StatusBoardDateAt, "string")

        Return GetSingleDbField("Select CurrLogDate FROM " & SqlFunc & "(@AreaKey, @SqlFuncIntervalMultiplier, @Where)", QueryConfig, "CurrLogDate")
    End Function

    Public Function IsTimeForNewLog(AreaKey As Integer, StatusBoardDateAt As String) As Boolean
        If StatusBoardDateAt Is Nothing Then StatusBoardDateAt = System.DateTime.Now.ToString("MM/dd/yyyy")

        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@AreaKey", GetParamVarHash(AreaKey, "int")}
        }
        Dim SqlFuncDr As Data.DataRow = GetMyDataSetParamQuery("SELECT I.SqlFunc, I.SqlFunc2ndArg FROM [ALTS].[dbo].[T_LogArea] A INNER JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE A.[Key]=@AreaKey", QueryConfig).Tables(0).Rows(0)
        Dim SqlFunc As String = SqlFuncDr("SqlFunc")
        Dim SqlFuncIntervalMultiplier As String = SqlFuncDr("SqlFunc2ndArg")

        QueryConfig("@SqlFuncIntervalMultiplier") = GetParamVarHash(SqlFuncIntervalMultiplier, "int")
        QueryConfig("@Where") = GetParamVarHash(StatusBoardDateAt, "string")

        Dim IsTime As Boolean = GetSingleDbField("Select TimeForNewLog FROM " & SqlFunc & "(@AreaKey, @SqlFuncIntervalMultiplier, @Where)", QueryConfig, "TimeForNewLog")
        Return IsTime
    End Function
End Class

Public Class DuplicateLogException
    Inherits Exception
End Class

Public Class InactiveLogException
    Inherits Exception
End Class

Public Class OverdueLogCompleted
    Inherits Exception
End Class

