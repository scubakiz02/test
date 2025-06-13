Imports System.Drawing
Imports SatiDotNet2.Library
Imports System.Text.Json

Public Class MaintPM
    Dim Sql As New Security

    Public Function ModifyOrder(Key As Integer, Action As String, Marker As String) As Dictionary(Of String, String)
        Dim Res As New Dictionary(Of String, String)
        Dim ParameterizedValuesConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim Table As String = "[ALTS].[dbo]."
        Dim FieldOfInterest As String

        Select Case Marker
            Case "Label"
                Return ModifyLabelOrderNew(Key, Action)
            Case "Phase"
                Table += "[T_LogPhase]"
                FieldOfInterest = "PhaseOrder"
            Case Else
                Table += "[T_LogCommentList]"
                FieldOfInterest = "CommentOrder"
        End Select

        Dim UpdateQueryTemplate As String = "UPDATE " & Table & " SET " & FieldOfInterest & "="
        Dim SqlQuery As String = UpdateQueryTemplate & "@Order1 WHERE [Key]=@Key1; " & UpdateQueryTemplate & "@Order2 WHERE [Key]=@Key2"
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        QueryConfig("@Key") = New Dictionary(Of String, String) From {
            {"value", Key},
            {"typeOf", "int"}
        }
        Dim KeyDS As Data.DataSet = Sql.GetMyDataSetParamQuery("SELECT (SELECT TOP(1) [Key] FROM " & Table & " Frst WHERE Frst.AreaKey=Curr.AreaKey ORDER BY Frst." & FieldOfInterest & ") As FirstKey, (SELECT TOP(1) [Key] FROM " & Table & " Prev WHERE Prev.AreaKey=Curr.AreaKey AND Prev." & FieldOfInterest & " < Curr." & FieldOfInterest & " ORDER BY Prev." & FieldOfInterest & " DESC) As PrevKey, (SELECT TOP(1) [Key] FROM " & Table & " Nxt WHERE Nxt.AreaKey=Curr.AreaKey AND " & FieldOfInterest & " > Curr." & FieldOfInterest & " ORDER BY Nxt." & FieldOfInterest & ") As NextKey, (SELECT TOP(1) [Key] FROM " & Table & " Lst WHERE Lst.AreaKey=Curr.AreaKey ORDER BY Lst." & FieldOfInterest & " DESC) As LastKey FROM " & Table & " Curr WHERE [Key]=@Key GROUP BY " & FieldOfInterest & ", [Key], AreaKey", QueryConfig)
        Dim KeyDR As Data.DataRow = KeyDS.Tables(0).Rows(0)
        Dim FirstKey As Integer = KeyDR("FirstKey")
        Dim PrevKey As Integer = If(IsDBNull(KeyDR("PrevKey")), -1, KeyDR("PrevKey")) 'ternary operator in case value is DBNull
        Dim NextKey As Integer = If(IsDBNull(KeyDR("NextKey")), -1, KeyDR("NextKey")) 'ternary operator in case value is DBNull
        Dim LastKey As Integer = KeyDR("LastKey")
        Dim FirstOrder As Integer
        Dim PrevOrder As String 'string variable type in case PrevKey is -1, which would mean this variable could be null (Nothing)
        Dim Order As Integer
        Dim NextOrder As String 'string variable type in case NextKey is -1, which would mean this variable could be null (Nothing)
        Dim LastOrder As Integer

        QueryConfig("@Key")("value") = FirstKey
        FirstOrder = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = PrevKey
        PrevOrder = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = Key
        Order = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = NextKey
        NextOrder = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        QueryConfig("@Key")("value") = LastKey
        LastOrder = Sql.GetSingleDbField("SELECT " & FieldOfInterest & " FROM " & Table & " WHERE [Key]=@Key", QueryConfig, FieldOfInterest)

        If Action = "up" Then
            If PrevKey = -1 Then 'if true, this means the label is already the top/up most label
                SqlQuery = ""
            Else
                ParameterizedValuesConfig("@Order1") = New Dictionary(Of String, String) From {
                    {"value", PrevOrder},
                    {"typeOf", "int"}
                }
                ParameterizedValuesConfig("@Key2") = New Dictionary(Of String, String) From {
                    {"value", PrevKey},
                    {"typeOf", "int"}
                }
            End If
        Else
            If NextKey = -1 Then 'if true, this means the label is already the bottom/down most label
                SqlQuery = ""
            Else
                ParameterizedValuesConfig("@Order1") = New Dictionary(Of String, String) From {
                    {"value", NextOrder},
                    {"typeOf", "int"}
                }
                ParameterizedValuesConfig("@Key2") = New Dictionary(Of String, String) From {
                    {"value", NextKey},
                    {"typeOf", "int"}
                }
            End If
        End If

        If String.IsNullOrEmpty(SqlQuery) = False Then 'these 2 key value pairs always keep the same value, hence why they're not in the if statement above
            ParameterizedValuesConfig("@Key1") = New Dictionary(Of String, String) From {
                {"value", Key},
                {"typeOf", "int"}
            }
            ParameterizedValuesConfig("@Order2") = New Dictionary(Of String, String) From {
                {"value", Order},
                {"typeOf", "int"}
            }
        End If

        Res("ParameterizedValues") = JsonSerializer.Serialize(ParameterizedValuesConfig)
        Res("SqlQuery") = SqlQuery

        Return Res
    End Function

    Public Function ModifyLabelOrderNew(Key As Integer, Action As String, Optional TestDS As Data.DataSet = Nothing) As Dictionary(Of String, String)
        Dim Res As New Dictionary(Of String, String)
        Dim LabelOrderTracking As New Dictionary(Of Integer, Dictionary(Of String, Object))
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim DS As Data.DataSet
        Dim SqlQuery As String = "UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=@LabelOrder WHERE [Key]=@SiblingLabelKey; UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=@SiblingLabelOrder WHERE [Key]=@LabelKey;"

        If TestDS Is Nothing Then
            Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
                {"@LabelKey", Sql.GetParamVarHash(Key, "int")}
            }
            DS = Sql.GetMyDataSetParamQuery("SELECT L.[Key] As LabelKey, Label, P.[Key] As PhaseKey, Phase, PhaseOrder, LabelOrder FROM [ALTS].[dbo].[T_LogLabel] L LEFT JOIN [ALTS].[dbo].[T_LogPhase] P ON L.PhaseKey=P.[Key] WHERE L.AreaKey=(SELECT AreaKey FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@LabelKey) ORDER BY P.PhaseOrder, L.LabelOrder", SqlConfig)
        Else
            DS = TestDS
        End If

        For I As Integer = 0 To DS.Tables(0).Rows.Count
            Dim DR As Data.DataRow = DS.Tables(0).Rows(I)
            Dim LabelKey As Integer = DR("LabelKey")
            Dim LabelOrder As Integer = DR("LabelOrder")
            Dim PhaseOrder As Object = DR("PhaseOrder")

            If LabelKey = Key Then
                Dim SiblingLabelKey As Integer
                Dim SiblingLabelOrder As Double

                'using a try catch block to throw errors when moving labels to a non existing slot
                'Ex: moving top most label up, moving bottom most label down
                Try
                    Dim SiblingDR As Data.DataRow
                    Dim DrPhaseOrder As Object

                    If Action = "up" Then
                        SiblingDR = DS.Tables(0).Rows(I - 1)
                    Else 'Action = "down"
                        SiblingDR = DS.Tables(0).Rows(I + 1)
                    End If

                    'check for matching PhaseOrder values
                    DrPhaseOrder = SiblingDR("PhaseOrder")
                    If IsDBNull(DrPhaseOrder) = False AndAlso DrPhaseOrder <> PhaseOrder Then
                        Throw New Exception()
                    End If

                    SiblingLabelOrder = SiblingDR("LabelOrder")
                    SiblingLabelKey = SiblingDR("LabelKey")
                Catch ex As Exception
                    Return Res 'return blank dictionary
                End Try

                QueryConfig("@LabelKey") = Sql.GetParamVarHash(LabelKey, "int")
                QueryConfig("@SiblingLabelKey") = Sql.GetParamVarHash(SiblingLabelKey, "int")
                QueryConfig("@LabelOrder") = Sql.GetParamVarHash(LabelOrder, "int")
                QueryConfig("@SiblingLabelOrder") = Sql.GetParamVarHash(SiblingLabelOrder, "int")

                Exit For
            End If

            LabelOrderTracking(LabelOrder) = New Dictionary(Of String, Object) From {
                {"LabelKey", LabelKey},
                {"PhaseOrder", DR("PhaseOrder")}
            }
        Next

        'execute sql update queries this function is invoked WITHOUT optional arg
        If TestDS Is Nothing Then Sql.GetMyDataSetParamQuery(SqlQuery, QueryConfig)

        Res("SqlQuery") = SqlQuery
        Res("QueryConfig") = JsonSerializer.Serialize(QueryConfig)

        Return Res
    End Function

    Private Function SamePhase(LabelKey1, LabelKey2) As Boolean
        Dim PhaseQuery As String = "SELECT PhaseKey FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@LabelKey"
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String)) From
        {
            {"@LabelKey", New Dictionary(Of String, String) From {
                {"value", String.Empty},
                {"typeOf", "int"}
            }}
        }
        Dim PhaseKey1 As Integer
        Dim PhaseKey2 As Integer

        QueryConfig("@LabelKey")("value") = LabelKey1
        PhaseKey1 = Sql.GetSingleDbField(PhaseQuery, QueryConfig, "PhaseKey")

        QueryConfig("@LabelKey")("value") = LabelKey2
        PhaseKey2 = Sql.GetSingleDbField(PhaseQuery, QueryConfig, "PhaseKey")

        If PhaseKey1 = PhaseKey2 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetAreaDdlSelectConfig(AreaIntervalKey As String, DepartmentKey As String, View As String) As Dictionary(Of String, String) 'this query is used in several areas, but needs to use the current value in Session("AreaIntervalKey"). That is why it in a function
        Dim Res As New Dictionary(Of String, String)
        Dim SelectQuery As String

        Res("AreaIntervalKey") = If(AreaIntervalKey Is Nothing OrElse AreaIntervalKey = "All", -1, AreaIntervalKey)

        SelectQuery = "SELECT A.Area, A.[Key] FROM [ALTS].[dbo].[T_LogArea] A " &
        "LEFT JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] " &
        "WHERE Status='" & If(View Is Nothing, "live", View) & "' " &
        "AND (A.IntervalKey=@AreaIntervalKey OR @AreaIntervalKey=-1 OR (A.IntervalKey IS NULL AND DATEDIFF(DAY, A.DateCreated, GETDATE()) = 0)) AND " &
        "OneTimeDate IS NULL" & If(DepartmentKey Is Nothing, String.Empty, " AND DepartmentKey=@DepartmentKey") & " " &
        "OR (OneTimeDate IS NOT NULL AND " &
        "((SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key])=0 OR (SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key]) IS NULL)) " &
        "ORDER BY A.Area"

        Res("SelectQuery") = SelectQuery

        Return Res
    End Function

    Private Sub PrepPmCloneConfig(PmCloneConfig As Dictionary(Of String, Dictionary(Of String, String)), Table As String, SqlQuery As String)
        PmCloneConfig(Table) = New Dictionary(Of String, String)
        PmCloneConfig(Table)("SqlQuery") = SqlQuery
    End Sub

    Private Function CloneTableRecords(TableConfig As Dictionary(Of String, String)) As String
        Dim QueryConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(TableConfig("QueryConfig"))
        Dim Res As String

        Try
            Res = Sql.ExecuteSqlParamQuery(TableConfig("SqlQuery"), QueryConfig)("PrimaryKey")
        Catch ex As Exception
            Res = String.Empty
        End Try

        Return Res
    End Function

    Private Sub RemoveInfoFromHttpRes(HttpRes As Dictionary(Of String, Dictionary(Of String, String)), Key As String)
        'remove info that the client doesn't need to see
        HttpRes(Key).Remove("QueryConfig")
        HttpRes(Key).Remove("SqlQuery")
    End Sub

    Public Function DoesPM_Exist(NewAreaName As String) As Boolean
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@NewAreaName", Sql.GetParamVarHash(NewAreaName, "string")}
        }

        If Sql.GetSingleDbField("SELECT COUNT(Area) As MatchCount FROM [ALTS].[dbo].[T_LogArea] WHERE Area LIKE ('%' + @NewAreaName + '%')", QueryConfig, "MatchCount") > 0 Then
            Return True
        End If
        Return False
    End Function

    Public Function ClonePM(AreaKey As String, AreaName As String, Optional TestClonedAreaKey As String = Nothing) As Dictionary(Of String, Dictionary(Of String, String))
        Dim Res As New Dictionary(Of String, Dictionary(Of String, String))
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim CloneAreaKey As Integer

        QueryConfig("@AreaKey") = Sql.GetParamVarHash(AreaKey, "int")
        PrepPmCloneConfig(Res, "AreaTable", "INSERT INTO [ALTS].[dbo].[T_LogArea] (GroupKey, DepartmentKey, IntervalKey, Area, OneTimeDate, DateCreated, Assignee, Active, Status) SELECT GroupKey, DepartmentKey, IntervalKey, @Area, OneTimeDate, DateCreated, Assignee, Active, Status FROM [ALTS].[dbo].[T_LogArea] WHERE [Key] = @AreaKey; Select CAST(SCOPE_IDENTITY() As INT);")

        Try
            If AreaKey Is Nothing OrElse AreaKey = String.Empty Then
                Throw New Exception("*Error: missing PM/Checklist to clone*")
            ElseIf Trim(AreaName) = String.Empty Then
                Throw New Exception("*Error: missing PM/Checklist name*")
            ElseIf DoesPM_Exist(AreaName) Then
                Throw New Exception("*Error: PM/Checklist already exists*")
            End If
        Catch ex As Exception
            RemoveInfoFromHttpRes(Res, "AreaTable")
            Res("AreaTable")("Success") = False
            Res("AreaTable")("Message") = ex.Message.ToString()
            Return Res
        End Try

        PrepPmCloneConfig(Res, "LabelTable", "INSERT INTO [ALTS].[dbo].[T_LogLabel] (AreaKey, UnitKey, PhaseKey, [Label], [Range], LabelOrder, FieldType) Select @ClonedPM_Key, UnitKey, PhaseKey, [Label], [Range], LabelOrder, FieldType FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=@AreaKey;")
        PrepPmCloneConfig(Res, "CommentTable", "INSERT INTO [ALTS].[dbo].[T_LogCommentList] (AreaKey, Comment, CommentOrder) Select @ClonedPM_Key, Comment, CommentOrder FROM [ALTS].[dbo].[T_LogCommentList] WHERE AreaKey=@AreaKey;")
        PrepPmCloneConfig(Res, "StampTable", "INSERT INTO [ALTS].[dbo].[T_LogStampList] (AreaKey, [Title], [TitleKey], [RoleID], Active) Select @ClonedPM_Key, [Title], [TitleKey], [RoleID], Active FROM [ALTS].[dbo].[T_LogStampList] WHERE AreaKey=@AreaKey;")
        PrepPmCloneConfig(Res, "PhaseTable", "INSERT INTO [ALTS].[dbo].[T_LogPhase] (AreaKey, [Phase], [PhaseOrder]) Select @ClonedPM_Key, [Phase], [PhaseOrder] FROM [ALTS].[dbo].[T_LogPhase] WHERE AreaKey=@AreaKey;")

        'T_LogArea is a unique 1 off case
        '1) there are multiple parameterized values
        '2) there's an expected return using 'SCOPE_IDENTITY()'
        'plus, this record needs to be executed first, b/c the other insert into sql queries depend on primary key from new record in T_LogArea
        QueryConfig("@Area") = Sql.GetParamVarHash(AreaName, "string")
        Res("AreaTable")("QueryConfig") = JsonSerializer.Serialize(QueryConfig)
        If TestClonedAreaKey Is Nothing Then
            CloneAreaKey = CloneTableRecords(Res("AreaTable"))

            'remove info from 'Res' that the client doesn't need to see
            RemoveInfoFromHttpRes(Res, "AreaTable")
            Res("AreaTable")("Success") = "True"
            Res("AreaTable")("CloneKey") = CloneAreaKey
        Else
            CloneAreaKey = TestClonedAreaKey
        End If
        QueryConfig.Remove("@Area") 'only needed for INSERT INTO query on T_LogArea

        'iterate and execute clone queries on the other tables
        QueryConfig("@ClonedPM_Key") = Sql.GetParamVarHash(CloneAreaKey, "int")
        For Each TableConfig As KeyValuePair(Of String, Dictionary(Of String, String)) In Res
            Dim TableName As String = TableConfig.Key

            If TableName = "AreaTable" Then Continue For 'dealing with this scenario before this for loop

            Res(TableName)("QueryConfig") = JsonSerializer.Serialize(QueryConfig)

            If TestClonedAreaKey Is Nothing Then
                CloneTableRecords(Res(TableName))
                RemoveInfoFromHttpRes(Res, TableName)
                Res(TableName)("Success") = True
            End If
        Next

        Return Res
    End Function

    Public Function RemovePM(AreaKey As String, Optional InvocateInTest As Boolean = False) As Dictionary(Of String, String)
        Return ModifyPmStatus(AreaKey, "removed", InvocateInTest)
    End Function

    Public Function ArchivePM(AreaKey As String, Optional InvocateInTest As Boolean = False) As Dictionary(Of String, String)
        Return ModifyPmStatus(AreaKey, "archived", InvocateInTest)
    End Function

    Public Function ReactivatePM(AreaKey As String, Optional InvocateInTest As Boolean = False) As Dictionary(Of String, String)
        Return ModifyPmStatus(AreaKey, "live", InvocateInTest)
    End Function

    Private Function ModifyPmStatus(AreaKey As String, Status As String, Optional InvocateInTest As Boolean = False) As Dictionary(Of String, String)
        Dim Res As New Dictionary(Of String, String)
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim SqlQuery As String = "UPDATE [ALTS].[dbo].[T_LogArea] SET Status='" & Status & "' WHERE [Key]=@AreaKey"

        QueryConfig("@AreaKey") = Sql.GetParamVarHash(AreaKey, "int")

        Res("QueryConfig") = JsonSerializer.Serialize(QueryConfig)
        Res("SqlQuery") = SqlQuery

        If InvocateInTest = False Then
            Dim SqlResult As Dictionary(Of String, Object) = Sql.ExecuteSqlParamQuery(SqlQuery, QueryConfig)
            Res.Remove("QueryConfig")
            Res.Remove("SqlQuery")

            Res("Success") = SqlResult("Success")
        End If

        Return Res
    End Function
End Class
