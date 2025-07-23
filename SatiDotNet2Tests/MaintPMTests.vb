Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library
Imports System.Text.Json

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
        Assert.Equal("INSERT INTO [ALTS].[dbo].[T_LogArea] (GroupKey, DepartmentKey, IntervalKey, Area, SectionType, OneTimeDate, DateCreated, Assignee, Active, Status) SELECT GroupKey, DepartmentKey, IntervalKey, @Area, SectionType, OneTimeDate, DateCreated, Assignee, Active, Status FROM [ALTS].[dbo].[T_LogArea] WHERE [Key] = @AreaKey; Select CAST(SCOPE_IDENTITY() As INT);", ClonePM_Res("AreaTable")("SqlQuery"))
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
    <InlineData(6, 8)>
    <InlineData(43, 48)>
    Public Sub T_LogLabelInsert(AreaKey As Integer, FakeClonedPmAreaKey As Integer)
        'copy relevant records (insert into sql query) in T_LogLabel after retreiving primary key of created record in T_LogPhase (batched copying)
        Dim ClonePM_Res As Dictionary(Of String, Dictionary(Of String, String)) = ClonePM(AreaKey, "Tell Tim Hughes he's awesome!", FakeClonedPmAreaKey)
        Dim CloneHash As New Dictionary(Of String, String) From {
            {"AreaKey", AreaKey},
            {"ClonedPM_Key", FakeClonedPmAreaKey}
        }
        Dim ClonePM_TableRes As Dictionary(Of String, String) = ClonePM_Res("LabelTable")

        Assert.True(SqlParameters.ValidParameterizedValues(CloneHash, ClonePM_TableRes))
        Assert.Equal("INSERT INTO [ALTS].[dbo].[T_LogLabel] (AreaKey, UnitKey, PhaseKey, [Label], [Range], LabelOrder, FieldType) " +
                     "Select @ClonedPM_Key, UnitKey, @ClonedPhaseKey, [Label], [Range], LabelOrder, FieldType FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=@AreaKey AND PhaseKey IS NULL",
                     ClonePM_TableRes("SqlQuery"))
    End Sub

    <Theory>
    <InlineData(6, 23)>
    <InlineData(43, 222)>
    Public Sub T_LogPhaseInsertsWithoutSqlExecution(CurrentAreaKey As String, ClonedAreaKey As String)
        Dim FakeDS As Data.DataSet = T_LogPhaseSelectDS(CurrentAreaKey)
        Dim FakeDS_RowCount As Integer = FakeDS.Tables(0).Rows.Count - 1
        Dim T_LogPhase_InsertIntoQueries As List(Of Dictionary(Of String, String)) = GetSectionCloneQueries(CurrentAreaKey, ClonedAreaKey, FakeDS) 'pass mock DS of T_LogPhase select query as arg 2

        'make sure return from GetSectionCloneQueries is not blank
        Assert.NotEqual(New List(Of Dictionary(Of String, String)), T_LogPhase_InsertIntoQueries)

        'test insert into sql query for each new record in T_LogPhase individually
        For I As Integer = 0 To FakeDS_RowCount
            Dim FakeDR As Data.DataRow = FakeDS.Tables(0).Rows(I)
            Dim CloneSectionHash As New Dictionary(Of String, String) From {
                {"CurrentAreaKey", FakeDR("AreaKey")},
                {"ClonedAreaKey", ClonedAreaKey},
                {"PhaseKey", FakeDR("Key")}
            }
            Dim T_LogPhase_InsertIntoQuery As Dictionary(Of String, String) = T_LogPhase_InsertIntoQueries(I)

            Assert.True(SqlParameters.ValidParameterizedValues(CloneSectionHash, T_LogPhase_InsertIntoQuery))
            Assert.Equal("INSERT INTO [ALTS].[dbo].[T_LogPhase] (AreaKey, [Phase], [PhaseOrder]) " +
                     "Select @ClonedAreaKey, [Phase], [PhaseOrder] FROM [ALTS].[dbo].[T_LogPhase] WHERE [Key]=@PhaseKey;" +
                     "SELECT CAST(SCOPE_IDENTITY() As INT);",
                     T_LogPhase_InsertIntoQuery("SqlQuery"))
        Next
    End Sub

    Private Function T_LogPhaseSelectDS(AreaKey As String) As Data.DataSet
        Dim DS As New Data.DataSet()
        Dim DT As New Data.DataTable()

        'mock schema of T_LogPhase
        DT.Columns.Add("Key", GetType(Integer))
        DT.Columns.Add("AreaKey", GetType(Integer))
        DT.Columns.Add("Phase", GetType(String))
        DT.Columns.Add("PhaseOrder", GetType(Integer))

        'add fake data to fake dataset
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"Key", 234},
            {"AreaKey", AreaKey},
            {"Phase", "phase 1"},
            {"PhaseOrder", 1}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"Key", 236},
            {"AreaKey", AreaKey},
            {"Phase", "phase 2"},
            {"PhaseOrder", 2}
        })

        DS.Tables.Add(DT)

        Return DS
    End Function

    Private Sub AddDsRow(DT As Data.DataTable, RowConfig As Dictionary(Of String, Object))
        Dim DR As Data.DataRow = DT.NewRow()

        DR("Key") = RowConfig("Key")
        DR("AreaKey") = RowConfig("AreaKey")
        DR("Phase") = RowConfig("Phase")
        DR("PhaseOrder") = RowConfig("PhaseOrder")

        DT.Rows.Add(DR)
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

Public Class LabelOrderFunctionalityTests
    Inherits MaintPM

    Private TopLabelKey As Integer = 20
    Private MiddleLabelKey As Integer = 21
    Private BottomLabelKey As Integer = 22

    <Fact>
    Public Sub StandardLabelOrderUpChange()
        Dim ModifyOrderRes As Dictionary(Of String, String) = ModifyOrder(MiddleLabelKey, "up", "T_LogLabel", GetPmWithNoPhasesDs())
        Dim QueryConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("QueryConfig"))

        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=@LabelOrder WHERE [Key]=@SiblingLabelKey; UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=@SiblingLabelOrder WHERE [Key]=@LabelKey;", ModifyOrderRes("SqlQuery"))

        Assert.Equal(MiddleLabelKey, QueryConfig("@LabelKey")("value"))
        Assert.Equal(1, QueryConfig("@SiblingLabelOrder")("value"))

        Assert.Equal(TopLabelKey, QueryConfig("@SiblingLabelKey")("value"))
        Assert.Equal(2, QueryConfig("@LabelOrder")("value"))
    End Sub

    <Fact>
    Public Sub LabelOrderUpWithScatteredLabelOrders()
        Dim LabelKeyLabelOrder As Decimal = 0.5
        Dim PmWithNoPhasesDS As Data.DataSet = GetPmWithNoPhasesDs()
        Dim ModifyOrderRes As Dictionary(Of String, String)
        Dim QueryConfig As Dictionary(Of String, Dictionary(Of String, String))

        'top label has a label order of 1
        'middle label has a label order of 2
        'what if top label had a label order differential not equal to 1 from middle label?
        'that's what this test figures out
        PmWithNoPhasesDS.Tables(0).Rows(0)("LabelOrder") = LabelKeyLabelOrder

        ModifyOrderRes = ModifyOrder(MiddleLabelKey, "up", "T_LogLabel", PmWithNoPhasesDS)
        QueryConfig = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("QueryConfig"))

        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=@LabelOrder WHERE [Key]=@SiblingLabelKey; UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=@SiblingLabelOrder WHERE [Key]=@LabelKey;", ModifyOrderRes("SqlQuery"))

        Assert.Equal(MiddleLabelKey, QueryConfig("@LabelKey")("value"))
        Assert.Equal(LabelKeyLabelOrder, QueryConfig("@SiblingLabelOrder")("value"))

        Assert.Equal(TopLabelKey, QueryConfig("@SiblingLabelKey")("value"))
        Assert.Equal(2, QueryConfig("@LabelOrder")("value"))
    End Sub

    <Fact>
    Public Sub TopmostLabelOrderUpChange()
        Dim ModifyOrderRes As Dictionary(Of String, String) = ModifyOrder(TopLabelKey, "up", "T_LogLabel", GetPmWithNoPhasesDs())
        Assert.Equal(Of Dictionary(Of String, String))(New Dictionary(Of String, String), ModifyOrderRes)
    End Sub

    <Fact>
    Public Sub StandardLabelOrderDownChange()
        Dim ModifyOrderRes As Dictionary(Of String, String) = ModifyOrder(MiddleLabelKey, "down", "T_LogLabel", GetPmWithNoPhasesDs())
        Dim QueryConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("QueryConfig"))

        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=@LabelOrder WHERE [Key]=@SiblingLabelKey; UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=@SiblingLabelOrder WHERE [Key]=@LabelKey;", ModifyOrderRes("SqlQuery"))

        Assert.Equal(MiddleLabelKey, QueryConfig("@LabelKey")("value"))
        Assert.Equal(3, QueryConfig("@SiblingLabelOrder")("value"))

        Assert.Equal(BottomLabelKey, QueryConfig("@SiblingLabelKey")("value"))
        Assert.Equal(2, QueryConfig("@LabelOrder")("value"))
    End Sub

    <Fact>
    Public Sub DownmostLabelOrderUpChange()
        Dim ModifyOrderRes As Dictionary(Of String, String) = ModifyOrder(BottomLabelKey, "down", "T_LogLabel", GetPmWithNoPhasesDs())
        Assert.Equal(Of Dictionary(Of String, String))(New Dictionary(Of String, String), ModifyOrderRes)
    End Sub

    <Fact>
    Public Sub TopmostLabelWithinPhaseOrderUp()
        Dim LabelKey As Integer = 17
        Dim ModifyOrderRes As Dictionary(Of String, String) = ModifyOrder(LabelKey, "up", "T_LogLabel", GetPmWithSomeLabelsThatHavePhasesDs())

        Assert.Equal(Of Dictionary(Of String, String))(New Dictionary(Of String, String), ModifyOrderRes)
    End Sub

    <Fact>
    Public Sub BottomostLabelWithinPhaseOrderDown()
        Dim LabelKey As Integer = 20
        Dim ModifyOrderRes As Dictionary(Of String, String) = ModifyOrder(LabelKey, "down", "T_LogLabel", GetPmWithSomeLabelsThatHavePhasesDs())

        Assert.Equal(Of Dictionary(Of String, String))(New Dictionary(Of String, String), ModifyOrderRes)
    End Sub

    Private Sub AddDsRow(DT As Data.DataTable, RowConfig As Dictionary(Of String, Object))
        Dim DR As Data.DataRow = DT.NewRow()

        DR("LabelKey") = RowConfig("LabelKey")
        DR("PhaseKey") = RowConfig("PhaseKey")
        DR("PhaseOrder") = RowConfig("PhaseOrder")
        DR("LabelOrder") = RowConfig("LabelOrder")

        DT.Rows.Add(DR)
    End Sub

    Private Function GetPmWithNoPhasesDs() As Data.DataSet
        Dim DS As New Data.DataSet()
        Dim DT As New Data.DataTable()

        DT.Columns.Add("LabelKey", GetType(Integer))
        DT.Columns.Add("PhaseKey", GetType(Integer))
        DT.Columns.Add("PhaseOrder", GetType(Integer))
        DT.Columns.Add("LabelOrder", GetType(Decimal))

        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 20},
            {"PhaseKey", DBNull.Value},
            {"PhaseOrder", DBNull.Value},
            {"LabelOrder", 1}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 21},
            {"PhaseKey", DBNull.Value},
            {"PhaseOrder", DBNull.Value},
            {"LabelOrder", 2}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 22},
            {"PhaseKey", DBNull.Value},
            {"PhaseOrder", DBNull.Value},
            {"LabelOrder", 3}
        })

        DS.Tables.Add(DT)

        Return DS

    End Function

    Private Function GetPmWithSomeLabelsThatHavePhasesDs() As Data.DataSet
        Dim DS As New Data.DataSet()
        Dim DT As New Data.DataTable()

        DT.Columns.Add("LabelKey", GetType(Integer))
        DT.Columns.Add("PhaseKey", GetType(Integer))
        DT.Columns.Add("PhaseOrder", GetType(Integer))
        DT.Columns.Add("LabelOrder", GetType(Decimal))

        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 22},
            {"PhaseKey", DBNull.Value},
            {"PhaseOrder", DBNull.Value},
            {"LabelOrder", 7}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 20},
            {"PhaseKey", DBNull.Value},
            {"PhaseOrder", DBNull.Value},
            {"LabelOrder", 8}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 16},
            {"PhaseKey", 111},
            {"PhaseOrder", 1},
            {"LabelOrder", 1}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 18},
            {"PhaseKey", 111},
            {"PhaseOrder", 1},
            {"LabelOrder", 2}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 11},
            {"PhaseKey", 111},
            {"PhaseOrder", 1},
            {"LabelOrder", 4}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 17},
            {"PhaseKey", 112},
            {"PhaseOrder", 2},
            {"LabelOrder", 3}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 19},
            {"PhaseKey", 112},
            {"PhaseOrder", 2},
            {"LabelOrder", 5}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 13},
            {"PhaseKey", 112},
            {"PhaseOrder", 2},
            {"LabelOrder", 7}
        })

        DS.Tables.Add(DT)

        Return DS
    End Function

End Class

Public Class CommentOrderFunctionalityTests
    Inherits MaintPM

    Private TopCommentKey As Integer = 5
    Private MiddleCommentKey As Integer = 4
    Private BottomCommentKey As Integer = 6
    Private SwapCommentOrdersQuery As String = "UPDATE [ALTS].[dbo].[T_LogCommentList] SET CommentOrder=@CommentOrder WHERE [Key]=@SiblingCommentKey; UPDATE [ALTS].[dbo].[T_LogCommentList] SET CommentOrder=@SiblingCommentOrder WHERE [Key]=@CommentKey;"

    <Fact>
    Public Sub StandardCommentOrderUpChange()
        Dim ModifyOrderRes As Dictionary(Of String, String) = ModifyOrder(MiddleCommentKey, "up", "T_LogCommentList", GetFakeDs())
        Dim QueryConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("QueryConfig"))

        Assert.Equal(SwapCommentOrdersQuery, ModifyOrderRes("SqlQuery"))

        Assert.Equal(MiddleCommentKey, QueryConfig("@CommentKey")("value"))
        Assert.Equal(1, QueryConfig("@SiblingCommentOrder")("value"))

        Assert.Equal(TopCommentKey, QueryConfig("@SiblingCommentKey")("value"))
        Assert.Equal(2, QueryConfig("@CommentOrder")("value"))
    End Sub

    <Fact>
    Public Sub CommentOrderUpWithScatteredCommentOrders()
        Dim TopCommentKeyCommentOrder As Decimal = 0.5
        Dim FakeDs As Data.DataSet = GetFakeDs()
        Dim ModifyOrderRes As Dictionary(Of String, String)
        Dim QueryConfig As Dictionary(Of String, Dictionary(Of String, String))

        'TopCommentKey has comment order of 1
        'MiddleCommentKey has comment order of 2
        'what if TopCommentKey has a comment order differential (TopCommentKey comment order - MiddleCommentKey comment order) other than 1 from MiddleCommentKey comment order?
        'that's what this test figures out
        FakeDs.Tables(0).Rows(0)("CommentOrder") = TopCommentKeyCommentOrder

        ModifyOrderRes = ModifyOrder(MiddleCommentKey, "up", "T_LogCommentList", FakeDs)
        QueryConfig = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("QueryConfig"))

        Assert.Equal(SwapCommentOrdersQuery, ModifyOrderRes("SqlQuery"))

        Assert.Equal(MiddleCommentKey, QueryConfig("@CommentKey")("value"))
        Assert.Equal(TopCommentKeyCommentOrder, QueryConfig("@SiblingCommentOrder")("value"))

        Assert.Equal(TopCommentKey, QueryConfig("@SiblingCommentKey")("value"))
        Assert.Equal(2, QueryConfig("@CommentOrder")("value"))
    End Sub

    <Fact>
    Public Sub TopmostCommentOrderUpChange()
        Dim ModifyOrderRes As Dictionary(Of String, String) = ModifyOrder(TopCommentKey, "up", "T_LogCommentList", GetFakeDs())
        Assert.Equal(Of Dictionary(Of String, String))(New Dictionary(Of String, String), ModifyOrderRes)
    End Sub

    <Fact>
    Public Sub StandardCommentOrderDownChange()
        Dim ModifyOrderRes As Dictionary(Of String, String) = ModifyOrder(MiddleCommentKey, "down", "T_LogCommentList", GetFakeDs())
        Dim QueryConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("QueryConfig"))

        Assert.Equal(SwapCommentOrdersQuery, ModifyOrderRes("SqlQuery"))

        Assert.Equal(MiddleCommentKey, QueryConfig("@CommentKey")("value"))
        Assert.Equal(3, QueryConfig("@SiblingCommentOrder")("value"))

        Assert.Equal(BottomCommentKey, QueryConfig("@SiblingCommentKey")("value"))
        Assert.Equal(2, QueryConfig("@CommentOrder")("value"))
    End Sub

    <Fact>
    Public Sub DownmostCommentOrderDownChange()
        Dim ModifyOrderRes As Dictionary(Of String, String) = ModifyOrder(BottomCommentKey, "down", "T_LogCommentList", GetFakeDs())
        Assert.Equal(Of Dictionary(Of String, String))(New Dictionary(Of String, String), ModifyOrderRes)
    End Sub

    Private Sub AddDsRow(DT As Data.DataTable, RowConfig As Dictionary(Of String, Object))
        Dim DR As Data.DataRow = DT.NewRow()

        DR("CommentKey") = RowConfig("CommentKey")
        DR("CommentOrder") = RowConfig("CommentOrder")

        DT.Rows.Add(DR)
    End Sub

    Private Function GetFakeDs() As Data.DataSet
        Dim DS As New Data.DataSet()
        Dim DT As New Data.DataTable()

        DT.Columns.Add("CommentKey", GetType(Integer))
        DT.Columns.Add("CommentOrder", GetType(Decimal))

        'sql query has ORDER BY clause placed on CommentOrder field
        'That's why they're in order within this fake dataset
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"CommentKey", TopCommentKey},
            {"CommentOrder", 1}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"CommentKey", MiddleCommentKey},
            {"CommentOrder", 2}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"CommentKey", BottomCommentKey},
            {"CommentOrder", 3}
        })

        DS.Tables.Add(DT)

        Return DS

    End Function

End Class

Public Class PhaseOrderFunctionalityTests
    Inherits MaintPM

    Private TopPhaseKey As Integer = 5
    Private MiddlePhaseKey As Integer = 4
    Private BottomPhaseKey As Integer = 6
    Private SwapPhaseOrdersQuery As String = "UPDATE [ALTS].[dbo].[T_LogPhase] SET PhaseOrder=@PhaseOrder WHERE [Key]=@SiblingPhaseKey; UPDATE [ALTS].[dbo].[T_LogPhase] SET PhaseOrder=@SiblingPhaseOrder WHERE [Key]=@PhaseKey;"

    <Fact>
    Public Sub StandardPhaseOrderUpChange()
        Dim ModifyOrderRes As Dictionary(Of String, String) = ModifyOrder(MiddlePhaseKey, "up", "T_LogPhase", GetFakeDs())
        Dim QueryConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("QueryConfig"))

        Assert.Equal(SwapPhaseOrdersQuery, ModifyOrderRes("SqlQuery"))

        Assert.Equal(MiddlePhaseKey, QueryConfig("@PhaseKey")("value"))
        Assert.Equal(1, QueryConfig("@SiblingPhaseOrder")("value"))

        Assert.Equal(TopPhaseKey, QueryConfig("@SiblingPhaseKey")("value"))
        Assert.Equal(2, QueryConfig("@PhaseOrder")("value"))
    End Sub

    <Fact>
    Public Sub PhaseOrderUpWithScatteredPhaseOrders()
        Dim TopPhaseKeyPhaseOrder As Decimal = 0.5
        Dim FakeDs As Data.DataSet = GetFakeDs()
        Dim ModifyOrderRes As Dictionary(Of String, String)
        Dim QueryConfig As Dictionary(Of String, Dictionary(Of String, String))

        'TopPhaseKey has comment order of 1
        'MiddlePhaseKey has comment order of 2
        'what if TopPhaseKey has a comment order differential (TopPhaseKey comment order - MiddlePhaseKey comment order) other than 1 from MiddlePhaseKey comment order?
        'that's what this test figures out
        FakeDs.Tables(0).Rows(0)("PhaseOrder") = TopPhaseKeyPhaseOrder

        ModifyOrderRes = ModifyOrder(MiddlePhaseKey, "up", "T_LogPhase", FakeDs)
        QueryConfig = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("QueryConfig"))

        Assert.Equal(SwapPhaseOrdersQuery, ModifyOrderRes("SqlQuery"))

        Assert.Equal(MiddlePhaseKey, QueryConfig("@PhaseKey")("value"))
        Assert.Equal(TopPhaseKeyPhaseOrder, QueryConfig("@SiblingPhaseOrder")("value"))

        Assert.Equal(TopPhaseKey, QueryConfig("@SiblingPhaseKey")("value"))
        Assert.Equal(2, QueryConfig("@PhaseOrder")("value"))
    End Sub

    <Fact>
    Public Sub TopmostPhaseOrderUpChange()
        Dim ModifyOrderRes As Dictionary(Of String, String) = ModifyOrder(TopPhaseKey, "up", "T_LogPhase", GetFakeDs())
        Assert.Equal(Of Dictionary(Of String, String))(New Dictionary(Of String, String), ModifyOrderRes)
    End Sub

    <Fact>
    Public Sub StandardPhaseOrderDownChange()
        Dim ModifyOrderRes As Dictionary(Of String, String) = ModifyOrder(MiddlePhaseKey, "down", "T_LogPhase", GetFakeDs())
        Dim QueryConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("QueryConfig"))

        Assert.Equal(SwapPhaseOrdersQuery, ModifyOrderRes("SqlQuery"))

        Assert.Equal(MiddlePhaseKey, QueryConfig("@PhaseKey")("value"))
        Assert.Equal(3, QueryConfig("@SiblingPhaseOrder")("value"))

        Assert.Equal(BottomPhaseKey, QueryConfig("@SiblingPhaseKey")("value"))
        Assert.Equal(2, QueryConfig("@PhaseOrder")("value"))
    End Sub

    <Fact>
    Public Sub DownmostPhaseOrderDownChange()
        Dim ModifyOrderRes As Dictionary(Of String, String) = ModifyOrder(BottomPhaseKey, "down", "T_LogPhase", GetFakeDs())
        Assert.Equal(Of Dictionary(Of String, String))(New Dictionary(Of String, String), ModifyOrderRes)
    End Sub

    Private Sub AddDsRow(DT As Data.DataTable, RowConfig As Dictionary(Of String, Object))
        Dim DR As Data.DataRow = DT.NewRow()

        DR("PhaseKey") = RowConfig("PhaseKey")
        DR("PhaseOrder") = RowConfig("PhaseOrder")

        DT.Rows.Add(DR)
    End Sub

    Private Function GetFakeDs() As Data.DataSet
        Dim DS As New Data.DataSet()
        Dim DT As New Data.DataTable()

        DT.Columns.Add("PhaseKey", GetType(Integer))
        DT.Columns.Add("PhaseOrder", GetType(Decimal))

        'sql query has ORDER BY clause placed on PhaseOrder field
        'That's why they're in order within this fake dataset
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"PhaseKey", TopPhaseKey},
            {"PhaseOrder", 1}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"PhaseKey", MiddlePhaseKey},
            {"PhaseOrder", 2}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"PhaseKey", BottomPhaseKey},
            {"PhaseOrder", 3}
        })

        DS.Tables.Add(DT)

        Return DS

    End Function

End Class

