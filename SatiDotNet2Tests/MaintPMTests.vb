Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library
Imports System.Text.Json

Public Class LabelOrderTests
    Inherits Security

    Dim ChecklistBuilderAspx = New MaintPM()

    'USING NITROGEN DAILY AS SAMPLE CHECKLIST. IF THE LABEL ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
    <Fact>
    Public Sub LabelOrder1()
        'moving label 1 up on Nitrogen Daily checklist
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("388", "up", "Label")
        Assert.Equal("", Res("SqlQuery"))
    End Sub

    <Fact>
    Public Sub LabelOrder2()
        'moving label 2 up on Nitrogen Daily checklist
        Dim LabelKey As Integer = 389
        Dim ModifyOrderRes As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder(LabelKey, "up", "Label")
        Dim ParameterizedValuesConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("ParameterizedValues"))

        Assert.Equal(1, ParameterizedValuesConfig("@Order1")("value"))
        Assert.Equal(LabelKey, ParameterizedValuesConfig("@Key1")("value"))

        Assert.Equal(2, ParameterizedValuesConfig("@Order2")("value"))
        Assert.Equal(388, ParameterizedValuesConfig("@Key2")("value"))

        Assert.Contains("T_LogLabel", ModifyOrderRes("SqlQuery"))
        Assert.Contains("LabelOrder", ModifyOrderRes("SqlQuery"))
    End Sub

    <Fact>
    Public Sub LabelOrder3()
        'moving label 3 down on Nitrogen Daily checklist
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("390", "down", "Label")
        Assert.Equal("", Res("SqlQuery"))
    End Sub

    <Fact>
    Public Sub LabelOrder4()
        'moving label 2 down on Nitrogen Daily checklist
        Dim LabelKey As Integer = 389
        Dim ModifyOrderRes As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder(LabelKey, "down", "Label")
        Dim ParameterizedValuesConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("ParameterizedValues"))

        Assert.Equal(3, ParameterizedValuesConfig("@Order1")("value"))
        Assert.Equal(LabelKey, ParameterizedValuesConfig("@Key1")("value"))

        Assert.Equal(2, ParameterizedValuesConfig("@Order2")("value"))
        Assert.Equal(390, ParameterizedValuesConfig("@Key2")("value"))

        Assert.Contains("T_LogLabel", ModifyOrderRes("SqlQuery"))
        Assert.Contains("LabelOrder", ModifyOrderRes("SqlQuery"))
    End Sub

    <Fact>
    Public Sub LabelOrder5()
        'enusre LabelOrder for Label at top of Phase stack cannot be moved up, as PhaseOrder trumps LabelOrder
        Dim PhaseController As New PhaseController(82) 'EDG monthly checklist
        Dim LabelKey As Integer = 602 'top label in phase 3 for checklist
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("602", "up", "Label")

        Assert.Equal(3, PhaseController.GetPhases()(LabelKey)("PhaseOrder")) 'baseline check. if this test fails, ensure value in LabelKey variable is the [Key] DB field value for the top most label in Phase 3 for EDG monthly checklist
        Assert.Equal("", Res("SqlQuery")) 'b/c T_LogLabel record 602 is the top most Label in Phase 3, it can NOT precede the LabelOrder of a record in another Phase
    End Sub

    <Fact>
    Public Sub LabelOrder6()
        'enusre LabelOrder for Label at bottom of Phase cannot be moved down, as PhaseOrder trumps LabelOrder
        Dim PhaseController As New PhaseController(82) 'EDG monthly checklist
        Dim LabelKey As Integer
        Dim Res As Dictionary(Of String, String)

        LabelKey = GetSingleDbField("SELECT [Key] FROM [ALTS].[dbo].[T_LogLabel] WHERE PhaseKey=2 AND LabelOrder=(SELECT MAX(LabelOrder) FROM [ALTS].[dbo].[T_LogLabel] WHERE PhaseKey=2)", New Dictionary(Of String, Dictionary(Of String, String)), "Key") 'bottom label in phase 2 for checklist
        Res = ChecklistBuilderAspx.ModifyOrder(LabelKey, "down", "Label")

        Assert.Equal(2, PhaseController.GetPhases()(LabelKey)("PhaseOrder")) 'baseline check. if this test fails, ensure value in LabelKey variable is the [Key] DB field value for the bottom most label in Phase 2 for EDG monthly checklist
        Assert.Equal("", Res("SqlQuery")) 'b/c T_LogLabel record 603 is the bottom most Label in Phase 2, it can NOT precede the LabelOrder of a record in another Phase
    End Sub

    'USING NITROGEN DAILY AS SAMPLE CHECKLIST. IF THE LABEL ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
End Class

Public Class CommentOrderTests
    Dim ChecklistBuilderAspx = New MaintPM()
    Dim Security = New Security()

    'USING R.O Daily AS SAMPLE CHECKLIST. IF THE COMMENT ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
    <Fact>
    Public Sub CommentOrder1()
        'moving comment 1 up on Nitrogen Daily checklist
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("53", "up", "Comment")
        Assert.Equal("", Res("SqlQuery"))
    End Sub

    <Fact>
    Public Sub CommentOrder2()
        Dim CommentKey As Integer = 54
        Dim ModifyOrderRes As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder(CommentKey, "up", "Comment")
        Dim ParameterizedValuesConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("ParameterizedValues"))

        Assert.Equal(1, ParameterizedValuesConfig("@Order1")("value"))
        Assert.Equal(CommentKey, ParameterizedValuesConfig("@Key1")("value"))

        Assert.Equal(2, ParameterizedValuesConfig("@Order2")("value"))
        Assert.Equal(53, ParameterizedValuesConfig("@Key2")("value"))

        Assert.Contains("T_LogCommentList", ModifyOrderRes("SqlQuery"))
        Assert.Contains("CommentOrder", ModifyOrderRes("SqlQuery"))
    End Sub

    <Fact>
    Public Sub CommentOrder3()
        'moving comment 3 down on Nitrogen Daily checklist
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("55", "down", "Comment")
        Assert.Equal("", Res("SqlQuery"))
    End Sub

    <Fact>
    Public Sub CommentOrder4()
        'moving comment 2 down on Nitrogen Daily checklist
        Dim CommentKey As Integer = 54
        Dim ModifyOrderRes As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder(CommentKey, "down", "Comment")
        Dim ParameterizedValuesConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("ParameterizedValues"))

        Assert.Equal(3, ParameterizedValuesConfig("@Order1")("value"))
        Assert.Equal(CommentKey, ParameterizedValuesConfig("@Key1")("value"))

        Assert.Equal(2, ParameterizedValuesConfig("@Order2")("value"))
        Assert.Equal(55, ParameterizedValuesConfig("@Key2")("value"))

        Assert.Contains("T_LogCommentList", ModifyOrderRes("SqlQuery"))
        Assert.Contains("CommentOrder", ModifyOrderRes("SqlQuery"))
    End Sub
    'USING R.O Daily AS SAMPLE CHECKLIST. IF THE COMMENT ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
End Class

Public Class PhaseOrderTests
    Dim ChecklistBuilderAspx = New MaintPM()
    Dim Security = New Security()

    'USING EDG Monthly Exercise PM (AreaKey 82) AS SAMPLE CHECKLIST. IF THE COMMENT ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
    <Fact>
    Public Sub PhaseOrder1()
        'moving EDG Monthly Exercise PM 1 up on EDG Monthly Exercise PM
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("1", "up", "Phase")
        Assert.Equal("", Res("SqlQuery"))
    End Sub

    <Fact>
    Public Sub PhaseOrder2()
        Dim PhaseKey As Integer = 2
        Dim ModifyOrderRes As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder(PhaseKey, "up", "Phase")
        Dim ParameterizedValuesConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("ParameterizedValues"))

        Assert.Equal(1, ParameterizedValuesConfig("@Order1")("value"))
        Assert.Equal(PhaseKey, ParameterizedValuesConfig("@Key1")("value"))

        Assert.Equal(2, ParameterizedValuesConfig("@Order2")("value"))
        Assert.Equal(1, ParameterizedValuesConfig("@Key2")("value"))

        Assert.Contains("T_LogPhase", ModifyOrderRes("SqlQuery"))
        Assert.Contains("PhaseOrder", ModifyOrderRes("SqlQuery"))
    End Sub

    <Fact>
    Public Sub PhaseOrder3()
        'moving phase 3 down on EDG Monthly Exercise PM
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("3", "down", "Phase")
        Assert.Equal("", Res("SqlQuery"))
    End Sub

    <Fact>
    Public Sub PhaseOrder4()
        Dim PhaseKey As Integer = 2
        Dim ModifyOrderRes As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder(PhaseKey, "down", "Phase")
        Dim ParameterizedValuesConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("ParameterizedValues"))

        Assert.Equal(3, ParameterizedValuesConfig("@Order1")("value"))
        Assert.Equal(PhaseKey, ParameterizedValuesConfig("@Key1")("value"))

        Assert.Equal(2, ParameterizedValuesConfig("@Order2")("value"))
        Assert.Equal(3, ParameterizedValuesConfig("@Key2")("value"))

        Assert.Contains("T_LogPhase", ModifyOrderRes("SqlQuery"))
        Assert.Contains("PhaseOrder", ModifyOrderRes("SqlQuery"))
    End Sub
    'USING EDG Monthly Exercise PM AS SAMPLE CHECKLIST. IF THE COMMENT ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
End Class

Public Class GetAreaDdlSelectCommandTests
    Dim ChecklistBuilderAspx As New MaintPM()
    Dim Security As New Security()
    Const ExpectedQuery As String = "SELECT A.Area, A.[Key] FROM [ALTS].[dbo].[T_LogArea] A LEFT JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE Status='live' AND (A.IntervalKey=@AreaIntervalKey OR @AreaIntervalKey=-1 OR (A.IntervalKey IS NULL AND DATEDIFF(DAY, A.DateCreated, GETDATE()) = 0)) AND OneTimeDate IS NULL OR (OneTimeDate IS NOT NULL AND ((SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key])=0 OR (SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key]) IS NULL)) ORDER BY A.Area"
    Const ExpectedQueryWithDepartment As String = "SELECT A.Area, A.[Key] FROM [ALTS].[dbo].[T_LogArea] A LEFT JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE Status='live' AND (A.IntervalKey=@AreaIntervalKey OR @AreaIntervalKey=-1 OR (A.IntervalKey IS NULL AND DATEDIFF(DAY, A.DateCreated, GETDATE()) = 0)) AND OneTimeDate IS NULL AND DepartmentKey=@DepartmentKey OR (OneTimeDate IS NOT NULL AND ((SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key])=0 OR (SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key]) IS NULL)) ORDER BY A.Area"

    <Theory>
    <InlineData(Nothing)>
    <InlineData("All")>
    Private Sub Negative1AreaIntervalKey(AreaIntervalKey As String)
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.GetAreaDdlSelectConfig(AreaIntervalKey, Nothing, "live")
        Assert.True(Res("AreaIntervalKey") = -1)
        Assert.Equal(ExpectedQuery, Res("SelectQuery"))
    End Sub

    <Theory>
    <InlineData(Nothing, ExpectedQuery)>
    <InlineData(1, ExpectedQueryWithDepartment)>
    <InlineData(2, ExpectedQueryWithDepartment)>
    Private Sub ValidArgsAndSelectQueryResponse(DepartmentKey As String, ExpectedSelectQuery As String)
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.GetAreaDdlSelectConfig(3, DepartmentKey, "live")
        Assert.True(Res("AreaIntervalKey") = 3)
        Assert.Equal(ExpectedSelectQuery, Res("SelectQuery"))
    End Sub

    <Fact>
    Private Sub ViewFromQsIsNothing()
        Dim ViewFromQs As String = Nothing
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.GetAreaDdlSelectConfig(3, 1, ViewFromQs)

        Assert.True(Res("AreaIntervalKey") = 3)
        Assert.Equal(ExpectedQueryWithDepartment, Res("SelectQuery"))
    End Sub

    <Theory>
    <InlineData("live")>
    <InlineData("archived")>
    Private Sub ToggleViewFromQsArgValueTestCases(ViewFromQs As String)
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.GetAreaDdlSelectConfig(3, 1, ViewFromQs)
        Dim ExpectedSelectQuery As String = ExpectedQueryWithDepartment.Replace("Status='live'", "Status='" & ViewFromQs & "'")

        Assert.True(Res("AreaIntervalKey") = 3)
        Assert.Equal(ExpectedSelectQuery, Res("SelectQuery"))
    End Sub
End Class

Public Class MaintPMCloneTests
    Inherits MaintPM
    Private SqlParameters As New SqlParameters()
    Private Security As New Security()

    Private Function GetRandAreaList(ModifyCasing As Boolean) As List(Of String)
        Dim DbResults As New List(Of String)
        Dim SqlQuery As String

        For I As Integer = 0 To 20 '20 test cases!
            If ModifyCasing Then
                If I Mod 2 = 0 Then
                    SqlQuery = "SELECT TOP 1 LOWER(Area) As Area FROM [ALTS].[dbo].[T_LogArea] ORDER BY NEWID();"
                Else
                    SqlQuery = "SELECT TOP 1 UPPER(Area) As Area FROM [ALTS].[dbo].[T_LogArea] ORDER BY NEWID();"
                End If
            Else
                SqlQuery = "SELECT TOP 1 Area FROM [ALTS].[dbo].[T_LogArea] ORDER BY NEWID();"
            End If

            DbResults.Add(Security.GetSingleDbField(SqlQuery, New Dictionary(Of String, Dictionary(Of String, String)), "Area"))
        Next

        Return DbResults
    End Function

    <Fact>
    Private Sub ExactMatchTestCases()
        Dim RandAreaList As List(Of String) = GetRandAreaList(False)

        For Each AreaName As String In RandAreaList
            Assert.True(DoesPM_Exist(AreaName))
        Next
    End Sub

    <Fact>
    Private Sub SimilarMatchTestCases()
        Dim RandAreaList As List(Of String) = GetRandAreaList(True)

        For Each AreaName As String In RandAreaList
            Assert.True(DoesPM_Exist(AreaName))
        Next
    End Sub

    <Fact>
    Private Sub DoesPM_Exist_ErrorSafeguardingIntegrationTests()
        Dim RandAreaList As List(Of String) = GetRandAreaList(True)

        For Each AreaName As String In RandAreaList
            Dim Res As Dictionary(Of String, Dictionary(Of String, String)) = ClonePM(2, AreaName)
            Assert.Equal("*Error: PM/Checklist already exists*", Res("AreaTable")("Message"))
        Next
    End Sub

    <Theory>
    <InlineData(Nothing)>
    <InlineData("")>
    Public Sub T_LogArea_AreaKeyEdgeCases(AreaKey As String)
        'Nothing or an empty string for arg 1 (AreaKey) is an edgecase
        'an empty string is the only edgecase for arg 2 (Area)
        Dim AreaName As String = "your mom"
        Dim Res As Dictionary(Of String, Dictionary(Of String, String)) = ClonePM(AreaKey, AreaName)

        Assert.False(Res("AreaTable").ContainsKey("SqlQuery"))
        Assert.False(Res("AreaTable").ContainsKey("QueryConfig"))
        Assert.True(Res.Count = 1) 'T_LogArea should be the only key

        Assert.False(Boolean.Parse(Res("AreaTable")("Success")))
        Assert.Equal("*Error: missing PM/Checklist to clone*", Res("AreaTable")("Message"))
    End Sub

    <Theory>
    <InlineData("")>
    <InlineData(" ")>
    Public Sub T_LogArea_AreaNameEdgeCases(NewAreaName As String)
        'AreaName is an empty string is the edge case
        Dim Res As Dictionary(Of String, Dictionary(Of String, String)) = ClonePM(2, NewAreaName)

        Assert.False(Res("AreaTable").ContainsKey("SqlQuery"))
        Assert.False(Res("AreaTable").ContainsKey("QueryConfig"))
        Assert.True(Res.Count = 1) 'T_LogArea should be the only key

        Assert.False(Boolean.Parse(Res("AreaTable")("Success")))
        Assert.Equal("*Error: missing PM/Checklist name*", Res("AreaTable")("Message"))
    End Sub

    <Theory>
    <InlineData(3, 4)>
    <InlineData(235, 238)>
    Public Sub T_LogAreaInsert(AreaKey As Integer, ClonedPM_Key As Integer)
        Dim AreaName As String = "Tell Sandie I said hi!"
        Dim ClonePM_Res As Dictionary(Of String, Dictionary(Of String, String)) = ClonePM(AreaKey, AreaName, ClonedPM_Key)
        Dim CloneHash As New Dictionary(Of String, String) From {
            {"AreaKey", AreaKey},
            {"Area", AreaName}
        }

        Assert.True(SqlParameters.ValidParameterizedValues(CloneHash, ClonePM_Res("AreaTable")))
        Assert.Equal("INSERT INTO [ALTS].[dbo].[T_LogArea] (GroupKey, DepartmentKey, IntervalKey, Area, OneTimeDate, DateCreated, Assignee, Active, Status) SELECT GroupKey, DepartmentKey, IntervalKey, @Area, OneTimeDate, DateCreated, Assignee, Active, Status FROM [ALTS].[dbo].[T_LogArea] WHERE [Key] = @AreaKey; Select CAST(SCOPE_IDENTITY() As INT);", ClonePM_Res("AreaTable")("SqlQuery"))
    End Sub

    <Theory>
    <InlineData(6, 8)>
    <InlineData(43, 48)>
    Public Sub T_LogLabelInsert(AreaKey As Integer, ClonedPM_Key As Integer)
        Dim ClonePM_Res As Dictionary(Of String, Dictionary(Of String, String)) = ClonePM(AreaKey, "Tell Tim he's awesome!", ClonedPM_Key)
        Dim CloneHash As New Dictionary(Of String, String) From {
            {"AreaKey", AreaKey},
            {"ClonedPM_Key", ClonedPM_Key}
        }
        Dim ClonePM_TableRes As Dictionary(Of String, String) = ClonePM_Res("LabelTable")

        Assert.True(SqlParameters.ValidParameterizedValues(CloneHash, ClonePM_TableRes))
        Assert.Equal("INSERT INTO [ALTS].[dbo].[T_LogLabel] (AreaKey, UnitKey, PhaseKey, [Label], [Range], LabelOrder, FieldType) Select @ClonedPM_Key, UnitKey, PhaseKey, [Label], [Range], LabelOrder, FieldType FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=@AreaKey;", ClonePM_TableRes("SqlQuery"))
    End Sub

    <Theory>
    <InlineData(6, 7)>
    <InlineData(43, 48)>
    Public Sub T_LogCommentListInsert(AreaKey As Integer, ClonedPM_Key As Integer)
        Dim ClonePM_Res As Dictionary(Of String, Dictionary(Of String, String)) = ClonePM(AreaKey, "hahahahahahahah", ClonedPM_Key)
        Dim CloneHash As New Dictionary(Of String, String) From {
            {"AreaKey", AreaKey},
            {"ClonedPM_Key", ClonedPM_Key}
        }
        Dim ClonePM_TableRes As Dictionary(Of String, String) = ClonePM_Res("CommentTable")

        Assert.True(SqlParameters.ValidParameterizedValues(CloneHash, ClonePM_TableRes))
        Assert.Equal("INSERT INTO [ALTS].[dbo].[T_LogCommentList] (AreaKey, Comment, CommentOrder) Select @ClonedPM_Key, Comment, CommentOrder FROM [ALTS].[dbo].[T_LogCommentList] WHERE AreaKey=@AreaKey;", ClonePM_TableRes("SqlQuery"))
    End Sub

    <Theory>
    <InlineData(6, 7)>
    <InlineData(43, 48)>
    Public Sub T_LogStampListInsert(AreaKey As Integer, ClonedPM_Key As Integer)
        Dim ClonePM_Res As Dictionary(Of String, Dictionary(Of String, String)) = ClonePM(AreaKey, "i'm running out of ideas", ClonedPM_Key)
        Dim CloneHash As New Dictionary(Of String, String) From {
            {"AreaKey", AreaKey},
            {"ClonedPM_Key", ClonedPM_Key}
        }
        Dim ClonePM_TableRes As Dictionary(Of String, String) = ClonePM_Res("StampTable")

        Assert.True(SqlParameters.ValidParameterizedValues(CloneHash, ClonePM_TableRes))
        Assert.Equal("INSERT INTO [ALTS].[dbo].[T_LogStampList] (AreaKey, [Title], [TitleKey], [RoleID], Active) Select @ClonedPM_Key, [Title], [TitleKey], [RoleID], Active FROM [ALTS].[dbo].[T_LogStampList] WHERE AreaKey=@AreaKey;", ClonePM_TableRes("SqlQuery"))
    End Sub

    <Theory>
    <InlineData(6, 7)>
    <InlineData(43, 48)>
    Public Sub T_LogPhaseInsert(AreaKey As Integer, ClonedPM_Key As Integer)
        Dim ClonePM_Res As Dictionary(Of String, Dictionary(Of String, String)) = ClonePM(AreaKey, "pew pew pew", ClonedPM_Key)
        Dim CloneHash As New Dictionary(Of String, String) From {
            {"AreaKey", AreaKey},
            {"ClonedPM_Key", ClonedPM_Key}
        }
        Dim ClonePM_TableRes As Dictionary(Of String, String) = ClonePM_Res("PhaseTable")

        Assert.True(SqlParameters.ValidParameterizedValues(CloneHash, ClonePM_TableRes))
        Assert.Equal("INSERT INTO [ALTS].[dbo].[T_LogPhase] (AreaKey, [Phase], [PhaseOrder]) Select @ClonedPM_Key, [Phase], [PhaseOrder] FROM [ALTS].[dbo].[T_LogPhase] WHERE AreaKey=@AreaKey;", ClonePM_TableRes("SqlQuery"))
    End Sub

End Class

Public Class ModifyPmStatusTests
    Inherits MaintPM
    Private SqlParameters As New SqlParameters()
    Private AreaKeyThatDoesNotNorWillEverExist As Integer = 1

    <Theory>
    <InlineData(1)>
    <InlineData(12)>
    <InlineData(99)>
    <InlineData(453)>
    <InlineData(4034)>
    Private Sub RemovePmTestCasesWithoutSqlExecution(AreaKey As Integer)
        Dim ClonePM_Res As Dictionary(Of String, String) = RemovePM(AreaKey, True)
        Dim CloneHash As New Dictionary(Of String, String) From {
            {"AreaKey", AreaKey}
        }

        Assert.True(SqlParameters.ValidParameterizedValues(CloneHash, ClonePM_Res))
        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogArea] SET Status='removed' WHERE [Key]=@AreaKey", ClonePM_Res("SqlQuery"))
    End Sub

    <Fact>
    Private Sub RemovePmTestCasesWithSqlExecution()
        'using a [Key] field value that does not nor will ever exists
        Dim ClonePM_Res As Dictionary(Of String, String) = RemovePM(AreaKeyThatDoesNotNorWillEverExist)
        Dim CloneHash As New Dictionary(Of String, String) From {
            {"AreaKey", AreaKeyThatDoesNotNorWillEverExist}
        }

        Assert.True(Boolean.Parse(ClonePM_Res("Success")))
    End Sub

    <Theory>
    <InlineData(4)>
    <InlineData(15)>
    <InlineData(86)>
    <InlineData(587)>
    <InlineData(3409)>
    Private Sub ArchivePmTestCasesWithoutSqlExecution(AreaKey As Integer)
        Dim ClonePM_Res As Dictionary(Of String, String) = ArchivePM(AreaKey, True)
        Dim CloneHash As New Dictionary(Of String, String) From {
            {"AreaKey", AreaKey}
        }

        Assert.True(SqlParameters.ValidParameterizedValues(CloneHash, ClonePM_Res))
        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogArea] SET Status='archived' WHERE [Key]=@AreaKey", ClonePM_Res("SqlQuery"))
    End Sub

    <Fact>
    Private Sub ArchivePmTestCasesWithSqlExecution()
        'using a [Key] field value that does not nor will ever exists
        Dim ClonePM_Res As Dictionary(Of String, String) = ArchivePM(AreaKeyThatDoesNotNorWillEverExist)
        Dim CloneHash As New Dictionary(Of String, String) From {
            {"AreaKey", AreaKeyThatDoesNotNorWillEverExist}
        }

        Assert.True(Boolean.Parse(ClonePM_Res("Success")))
    End Sub

    <Theory>
    <InlineData(3)>
    <InlineData(53)>
    <InlineData(76)>
    <InlineData(235)>
    <InlineData(2389)>
    Private Sub ReactivatePmTestCasesWithoutSqlExecution(AreaKey As Integer)
        Dim ClonePM_Res As Dictionary(Of String, String) = ReactivatePM(AreaKey, True)
        Dim CloneHash As New Dictionary(Of String, String) From {
            {"AreaKey", AreaKey}
        }

        Assert.True(SqlParameters.ValidParameterizedValues(CloneHash, ClonePM_Res))
        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogArea] SET Status='live' WHERE [Key]=@AreaKey", ClonePM_Res("SqlQuery"))
    End Sub

    <Fact>
    Private Sub ReactivatePmTestCasesWithSqlExecution()
        'using a [Key] field value that does not nor will ever exists
        Dim ClonePM_Res As Dictionary(Of String, String) = ReactivatePM(AreaKeyThatDoesNotNorWillEverExist)
        Dim CloneHash As New Dictionary(Of String, String) From {
            {"AreaKey", AreaKeyThatDoesNotNorWillEverExist}
        }

        Assert.True(Boolean.Parse(ClonePM_Res("Success")))
    End Sub
End Class

