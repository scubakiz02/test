Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library
Imports System.Text.Json

Public Class LabelOrderTests
    Dim ChecklistBuilderAspx = New ChecklistBuilderAspxLibrary()
    Dim Security = New Security()

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
        Dim LabelKey As Integer = 603 'top label in phase 3 for checklist
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder(LabelKey, "down", "Label")

        Assert.Equal(2, PhaseController.GetPhases()(LabelKey)("PhaseOrder")) 'baseline check. if this test fails, ensure value in LabelKey variable is the [Key] DB field value for the bottom most label in Phase 2 for EDG monthly checklist
        Assert.Equal("", Res("SqlQuery")) 'b/c T_LogLabel record 603 is the bottom most Label in Phase 2, it can NOT precede the LabelOrder of a record in another Phase
    End Sub

    'USING NITROGEN DAILY AS SAMPLE CHECKLIST. IF THE LABEL ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
End Class

Public Class CommentOrderTests
    Dim ChecklistBuilderAspx = New ChecklistBuilderAspxLibrary()
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
    Dim ChecklistBuilderAspx = New ChecklistBuilderAspxLibrary()
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
    Dim ChecklistBuilderAspx = New ChecklistBuilderAspxLibrary()
    Dim Security = New Security()
    Dim ExpectedQuery As String = "SELECT A.Area, A.[Key] FROM [ALTS].[dbo].[T_LogArea] A LEFT JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE (A.IntervalKey=@AreaIntervalKey OR @AreaIntervalKey=-1 OR (A.IntervalKey IS NULL AND DATEDIFF(DAY, A.DateCreated, GETDATE()) = 0)) AND OneTimeDate IS NULL OR (OneTimeDate IS NOT NULL AND ((SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key])=0 OR (SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key]) IS NULL)) ORDER BY A.Area"
    Dim ExpectedQueryWithDepartment As String = "SELECT A.Area, A.[Key] FROM [ALTS].[dbo].[T_LogArea] A LEFT JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE (A.IntervalKey=@AreaIntervalKey OR @AreaIntervalKey=-1 OR (A.IntervalKey IS NULL AND DATEDIFF(DAY, A.DateCreated, GETDATE()) = 0)) AND OneTimeDate IS NULL AND DepartmentKey=@DepartmentKey OR (OneTimeDate IS NOT NULL AND ((SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key])=0 OR (SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key]) IS NULL)) ORDER BY A.Area"

    <Fact>
    Public Sub GetAreaDdlSelectCommandTest1()
        'pass in a null Area IntervalKey and 'All' Department (2nd arg). expect AreaIntervalKey return of -1 as well as the expected query
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.GetAreaDdlSelectConfig(Nothing, Nothing)
        Assert.True(Res("AreaIntervalKey") = -1 AndAlso Res("SelectQuery") = ExpectedQuery)
    End Sub

    <Fact>
    Public Sub GetAreaDdlSelectCommandTest2()
        'pass in a 'All' Area IntervalKey and 'All' Department (2nd arg). expect AreaIntervalKey return of -1 as well as the expected query
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.GetAreaDdlSelectConfig("All", Nothing)
        Assert.True(Res("AreaIntervalKey") = -1 AndAlso Res("SelectQuery") = ExpectedQuery)
    End Sub

    <Fact>
    Public Sub GetAreaDdlSelectCommandTest3()
        'pass in existing Area IntervalKey and 'All' Department (2nd arg). expect it back for AreaIntervalKey as well as the expected query
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.GetAreaDdlSelectConfig(3, Nothing)
        Assert.True(Res("AreaIntervalKey") = 3 AndAlso Res("SelectQuery") = ExpectedQuery)
    End Sub

    <Fact>
    Public Sub GetAreaDdlSelectCommandTest4()
        'pass in existing Area IntervalKey and a valid Department key (2nd arg). expect AreaIntervalKey as well as the expected query WITH department as a return
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.GetAreaDdlSelectConfig(3, 1)
        Assert.True(Res("AreaIntervalKey") = 3 AndAlso Res("SelectQuery") = ExpectedQueryWithDepartment)
    End Sub

    <Fact>
    Public Sub GetAreaDdlSelectCommandTest5()
        'pass in existing Area IntervalKey and a valid Department key (2nd arg). expect AreaIntervalKey as well as the expected query WITH department as a return
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.GetAreaDdlSelectConfig(3, 2)
        Assert.True(Res("AreaIntervalKey") = 3 AndAlso Res("SelectQuery") = ExpectedQueryWithDepartment)
    End Sub
End Class