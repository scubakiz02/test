Imports System.Drawing
Imports SatiDotNet2.Library
Imports System.Text.Json

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

    Public Function GetState(DataKey As Integer, Optional FakeStampDs As Data.DataSet = Nothing, Optional FakeLogDs As Data.DataSet = Nothing) As Dictionary(Of String, Object)
        Dim Res As New Dictionary(Of String, Object)
        Dim StampDs As Data.DataSet
        Dim LogState As String
        Dim LogDataDr As Data.DataRow

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
                "Inputs " &
                "FROM [ALTS].[dbo].[T_LogData] D WHERE [Key]=@DataKey", SqlConfig).Tables(0).Rows(0)
        Else
            StampDs = FakeStampDs
            LogDataDr = FakeLogDs.Tables(0).Rows(0) 'use this variable to access values fixed across all rows (IsLogNewest, IsLogComplete, Inputs)
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
            LogState = "delete"
        Catch ex As InactiveLogException
            LogState = "delete"
        Catch ex As DuplicateLogException
            LogState = "error"
        Catch ex As Exception
            'this block catches errors that are not accounted for
            LogState = "error"
        End Try

        Res("logState") = LogState

        Return Res
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

