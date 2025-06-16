Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library
Imports System.Text.Json
Imports System.Linq

Public Class PhaseControllerTests
    Inherits Security
    Dim PhaseController As New PhaseController(82) 'EDG monthly checklist
    Dim TestDummyPhaseController As New PhaseController(75) 'dummy checklist
    Private LogAspx As New LogAspxLibrary()
    Private T_LogDataInputs As String
    Dim PhaseStageConfig As New Dictionary(Of Integer, Dictionary(Of String, String))
    Dim OgPhaseStageConfig As New Dictionary(Of Integer, Dictionary(Of String, String))

    Dim PhaseController2 As New PhaseController(61) 'No phases exist for this log instance
    Dim PhaseController2InputsField As String = "{""564"":{""Date"":""2025-03-22 00:00:00"",""Operator"":""Chase Dostie"",""Value"":""15.1""},""565"":{""Date"":""2025-03-22 00:00:00"",""Operator"":""Chase Dostie"",""Value"":""18""},""566"":{""Date"":""2025-03-22 00:00:00"",""Operator"":""Chase Dostie"",""Value"":""13.5""}}"
    Dim PhaseController2Config As Dictionary(Of Integer, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(PhaseController2InputsField)
    Private KeyToPhaseHash As New Dictionary(Of Integer, String) From {
        {1, "Before generator is running"},
        {2, "After generator is at operating temperature"},
        {3, "After generator is off"}
    }

    Private Function CloneDictionary(OriginalHash As Dictionary(Of Integer, Dictionary(Of String, String))) As Dictionary(Of Integer, Dictionary(Of String, String))
        Return OriginalHash.ToDictionary(
                Function(outer) outer.Key,
                Function(outer) outer.Value.ToDictionary(
                    Function(inner) inner.Key,
                    Function(inner) inner.Value
                )
            )
    End Function

    Private Sub OgEnvironment() 'if this sub is called, returning PhaseStageConfig to its original, no values filled statte
        PhaseStageConfig = CloneDictionary(OgPhaseStageConfig)
        Instantiate()
    End Sub

    Private Sub Instantiate()
        PhaseController = New PhaseController(82, PhaseStageConfig)
    End Sub

    Public Sub New() 'constructor, testing against T_LogArea record with [Key] of 82
        '595 - 599, PhaseKey 1
        '600 & 603, PhaseKey2
        '602, PhaseKey3
        T_LogDataInputs = "{""595"":{""Date"":"""",""Operator"":"""",""Value"":""""},""596"":{""Date"":"""",""Operator"":"""",""Value"":""""},""597"":{""Date"":"""",""Operator"":"""",""Value"":""""},""598"":{""Date"":"""",""Operator"":"""",""Value"":""""},""599"":{""Date"":"""",""Operator"":"""",""Value"":""""},""600"":{""Date"":"""",""Operator"":"""",""Value"":""""},""602"":{""Date"":"""",""Operator"":"""",""Value"":""""},""603"":{""Date"":"""",""Operator"":"""",""Value"":""""}}"
        PhaseStageConfig = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(T_LogDataInputs)
        OgPhaseStageConfig = CloneDictionary(PhaseStageConfig)
    End Sub


    <Fact>
    Public Sub GetPhaseTest1()
        'no values filled. GetPhase should return 1
        OgEnvironment()
        Assert.Equal(1, PhaseController.GetPhase())
    End Sub

    <Fact>
    Public Sub GetPhaseTest2()
        'values for inputs 595 - 598 are filled. GetPhase should return 1, b/c 599 is still left unfilled, and it also has a PhaseKey of 1
        OgEnvironment()
        PhaseStageConfig(595)("Value") = "1"
        PhaseStageConfig(596)("Value") = "1"
        PhaseStageConfig(597)("Value") = "1"
        PhaseStageConfig(598)("Value") = "1"
        Instantiate()
        Assert.Equal(1, PhaseController.GetPhase())
    End Sub

    <Fact>
    Public Sub GetPhaseTest3()
        'all values for PhaseKey 1 are filled. return should be 2
        OgEnvironment()
        PhaseStageConfig(595)("Value") = "1"
        PhaseStageConfig(596)("Value") = "1"
        PhaseStageConfig(597)("Value") = "1"
        PhaseStageConfig(598)("Value") = "1"
        PhaseStageConfig(599)("Value") = "1"
        Instantiate()
        Assert.Equal(2, PhaseController.GetPhase())
    End Sub

    <Fact>
    Public Sub GetPhaseTest4()
        'all values for PhaseKey 1 are filled, and 1 of 2 values for phase 2 are filled. return should be 2 
        OgEnvironment()
        PhaseStageConfig(595)("Value") = "1"
        PhaseStageConfig(596)("Value") = "1"
        PhaseStageConfig(597)("Value") = "1"
        PhaseStageConfig(598)("Value") = "1"
        PhaseStageConfig(599)("Value") = "1"
        PhaseStageConfig(600)("Value") = "1"
        Instantiate()
        Assert.Equal(2, PhaseController.GetPhase())
    End Sub

    <Fact>
    Public Sub GetPhaseTest5()
        'all values for PhaseKey 1 & 2 are filled. return should be 3
        OgEnvironment()

        FillPhaseStageConfig()
        PhaseStageConfig(602)("Value") = "0"
        PhaseStageConfig(672)("Value") = "0"

        Instantiate()
        Assert.Equal(3, PhaseController.GetPhase())
    End Sub

    <Fact>
    Public Sub GetPhaseTest6()
        'all values for all phases are filled. return should be 3 (the last phase)
        OgEnvironment()
        FillPhaseStageConfig()
        Instantiate()
        Assert.Equal(3, PhaseController.GetPhase())
    End Sub

    Public Sub FillPhaseStageConfig(Optional AreaKey As Integer = -1)
        Dim LabelKeysList As New List(Of Integer)
        Dim DS As Data.DataSet
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))

        If AreaKey = -1 Then AreaKey = 82 'EDG Monthly

        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", AreaKey},
            {"typeOf", "int"}
        }
        DS = GetMyDataSetParamQuery("SELECT [Key] FROM [ALTS].[dbo].[T_LogLabel] WHERE AreaKey=@AreaKey", QueryConfig)
        For Each DR As Data.DataRow In DS.Tables(0).Rows
            LabelKeysList.Add(DR("Key"))
        Next

        PhaseStageConfig.Clear()

        For Each LabelKey As Integer In LabelKeysList
            PhaseStageConfig(LabelKey) = New Dictionary(Of String, String) From {
                {"Value", "1"}
            }
        Next
    End Sub

    <Fact>
    Public Sub GetPhaseTest8()
        'ensure DS within PhaseController receives changes within the PhaseOrder DB field value change upon calling GetPhase()
        Dim BlankConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim LabelKey As Integer = 602

        OgEnvironment()

        ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogLabel] SET PhaseKey=2 WHERE [Key]=" & LabelKey, BlankConfig)
        Instantiate()
        Assert.Equal(2, PhaseController.GetPhases()(LabelKey)("PhaseOrder"))

        ExecuteSqlParamQuery("UPDATE [ALTS].[dbo].[T_LogLabel] SET PhaseKey=3 WHERE [Key]=" & LabelKey, BlankConfig) 'return back to og
        Instantiate()
        Assert.Equal(3, PhaseController.GetPhases()(LabelKey)("PhaseOrder"))
    End Sub

    <Fact>
    Private Sub ScissorLiftPmGetPhaseFunctionBug()
        'Upon reaching 'Platform' phase within 'SCISSOR LIFT OPERATORS INSPECTION CHECKLIST' checklist, PhaseController.GetPhase() returns 3
        'B/c of the return being 3, the 'Platform' phase continues to stay disabled
        'this prevents the operator from filling out the checklist from 'Platform' phase onward
        FillPhaseStageConfig(86)
        PhaseController = New PhaseController(86, PhaseStageConfig)
        Assert.Equal(Of Integer)(7, PhaseController.GetPhase())
    End Sub

    Private Sub AreaKey83Environment()
        Dim Inputs As String = "{""604"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""605"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""606"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""607"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""608"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""609"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""610"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""611"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""612"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""613"":{""Date"":"""",""Operator"":"""",""Value"":""""}}"
        PhaseStageConfig = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(Inputs)
        PhaseController = New PhaseController(83, PhaseStageConfig)
    End Sub

    <Fact>
    Public Sub GetPhaseTest7()
        'set AreaKey within class to 83, which uses PhaseKey 4 & 5, with PhaseOrder of 1 & 2. Blank slate, return Phase associated with PhaseOrder 1
        AreaKey83Environment()
        Assert.Equal(1, PhaseController.GetPhase())
    End Sub

    <Fact>
    Public Sub GetPhasesTest1()
        'execute GetPhases() on PhaseController, expect a non null return
        OgEnvironment()
        Assert.NotEqual(Nothing, PhaseController.GetPhases())
    End Sub

    <Fact>
    Public Sub GetPhasesTest2()
        '595 - 599, PhaseKey 1
        '600 & 603, PhaseKey2
        '602, PhaseKey3

        'execute GetPhases() on PhaseController, expect an identical dictionary return 
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim DS As Data.DataSet
        Dim RC As Integer
        Dim GetPhasesExpected As New Dictionary(Of Integer, Dictionary(Of String, String))

        OgEnvironment()

        DS = GetMyDataSetParamQuery("SELECT L.[Key] As LabelKey, P.Phase, P.PhaseOrder FROM [ALTS].[dbo].[T_LogLabel] L INNER JOIN [ALTS].[dbo].[T_LogPhase] P ON P.[Key]=L.PhaseKey WHERE L.AreaKey=82 ORDER BY P.PhaseOrder", QueryConfig)
        RC = DS.Tables(0).Rows.Count

        For I As Integer = 0 To RC - 1
            Dim DR As Data.DataRow = DS.Tables(0).Rows(I)
            Dim SubHash As New Dictionary(Of String, String)

            SubHash("Phase") = DR("Phase")
            SubHash("PhaseOrder") = DR("PhaseOrder")

            GetPhasesExpected(DR("LabelKey")) = SubHash
        Next

        Assert.Equal(Of String)(JsonSerializer.Serialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(GetPhasesExpected), JsonSerializer.Serialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(PhaseController.GetPhases()))
    End Sub

    <Fact>
    Public Sub GetPhasesTest3()
        'execute GetPhases() on PhaseController, instantiated with AreaKey 75, which should have a label with a NULL PhaseOrder (purposefully done)
        'ensure result from PhaseController.GetPhases() is not NULL and does NOT include the Label with a NULL PhaseOrder 
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim DS As Data.DataSet
        Dim RC As Integer
        Dim GetPhasesExpected As New Dictionary(Of Integer, Dictionary(Of String, String))
        Dim Inputs As String = "{""586"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""587"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""588"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""589"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""590"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""591"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""592"":{""Date"":"""",""Operator"":"""",""Value"":""""}, ""593"":{""Date"":"""",""Operator"":"""",""Value"":""""}}"

        DS = GetMyDataSetParamQuery("SELECT L.[Key] As LabelKey, P.Phase, P.PhaseOrder FROM [ALTS].[dbo].[T_LogLabel] L LEFT JOIN [ALTS].[dbo].[T_LogPhase] P ON P.[Key]=L.PhaseKey WHERE L.AreaKey=75 ORDER BY P.PhaseOrder", QueryConfig)
        RC = DS.Tables(0).Rows.Count

        For I As Integer = 0 To RC - 1
            Dim DR As Data.DataRow = DS.Tables(0).Rows(I)
            Dim SubHash As New Dictionary(Of String, String)

            Try 'in case Phase is NULL
                SubHash("Phase") = DR("Phase")
                SubHash("PhaseOrder") = DR("PhaseOrder")
            Catch ex As Exception
                SubHash("Phase") = String.Empty
                SubHash("PhaseOrder") = 0
            End Try

            GetPhasesExpected(DR("LabelKey")) = SubHash
        Next

        OgEnvironment()

        PhaseStageConfig = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(Inputs)
        PhaseController = New PhaseController(75, PhaseStageConfig)

        'use Assert.Equal(Of ...) to compare the 2 Dictionaries, b/c they do NOT need to be sorted the same way
        Assert.Equal(Of Dictionary(Of Integer, Dictionary(Of String, String)))(GetPhasesExpected, PhaseController.GetPhases())
    End Sub

    <Fact>
    Public Sub GetPhasesTest4()
        'execute GetPhases() on PhaseController with a Checklist that does NOT contain Phases (Nitrogen Daily) and contains values on input(s) (bug existed with these conditions)
        Dim ExpectedRes As New Dictionary(Of Integer, Dictionary(Of String, String))
        Dim HardCodedRes As Dictionary(Of Integer, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))("{""388"":{""Date"":"""",""Operator"":"""",""Value"":""12""},""389"":{""Date"":"""",""Operator"":"""",""Value"":""32""},""390"":{""Date"":"""",""Operator"":"""",""Value"":""""}}")
        Dim LabelKeys As List(Of Integer)

        PhaseController = New PhaseController(48, HardCodedRes)
        LabelKeys = HardCodedRes.Keys.ToList()
        For Each LabelKey As Integer In LabelKeys
            ExpectedRes(LabelKey) = New Dictionary(Of String, String) From {
                {"Phase", String.Empty},
                {"PhaseOrder", 0}
            }
        Next

        Assert.Equal(Of Dictionary(Of Integer, Dictionary(Of String, String)))(ExpectedRes, PhaseController.GetPhases())
    End Sub

    <Fact>
    Public Sub PmWithPhasesAndSomeNonPhasedLabels()
        'http://pwi-40:81/ChecklistLogging/Log.aspx?Key=1031
        Dim InputsFake As Dictionary(Of Integer, Dictionary(Of String, String))
        Dim PhaseController As PhaseController
        Dim DT As New Data.DataTable
        Dim DR As Data.DataRow = DT.NewRow()

        DT.Columns.Add("Inputs", GetType(String))
        DT.Columns.Add("Operator", GetType(String))
        DT.Columns.Add("Date", GetType(Date))

        DR("Inputs") = "{""586"":{""Date"":"""",""Operator"":"""",""Value"":""""},""587"":{""Date"":"""",""Operator"":"""",""Value"":""""},""588"":{""Date"":"""",""Operator"":"""",""Value"":""""},""589"":{""Date"":"""",""Operator"":"""",""Value"":""""},""590"":{""Date"":"""",""Operator"":"""",""Value"":""""},""591"":{""Date"":"""",""Operator"":"""",""Value"":""""},""592"":{""Date"":"""",""Operator"":"""",""Value"":""""},""593"":{""Date"":"""",""Operator"":"""",""Value"":""""}}"
        DR("Operator") = DBNull.Value
        DR("Date") = "2025-06-12 00:00:00"

        InputsFake = LogAspx.GetInputs(DR)
        PhaseController = New PhaseController(75, InputsFake)

        'phasing enable/disable logic starts at index 1
        'thus, even though the non phased labels are NOT all filled out, the return should be 1
        Assert.Equal(1, PhaseController.GetPhase())
    End Sub
End Class

'Public Class PhaseControllerDeleteBatchTests
'    Inherits PhaseController

'    <Theory>
'    <InlineData(Nothing, 2)>
'    <InlineData(2, Nothing)>
'    Public Sub NullEdgecases(AreaKey As String, BatchKey As String)
'        Assert.False(Boolean.Parse(DeleteBatch(AreaKey, BatchKey)("Success")))
'    End Sub
'End Class

Public Class PhaseControllerDeleteBatchTests
    Inherits PhaseController
    Private Rnd As New Random()

    Private Function AreHashesEqual(Hash1 As Dictionary(Of Integer, String), Hash2 As Dictionary(Of Integer, String)) As Boolean
        If Hash1.Count <> Hash2.Count Then
            Return False
        End If

        ' Compare key-value pairs
        For Each kvp As KeyValuePair(Of Integer, String) In Hash1
            If Not Hash2.ContainsKey(kvp.Key) Then
                Return False
            End If

            If Hash2(kvp.Key) <> kvp.Value Then
                Return False
            End If
        Next

        Return True
    End Function

    <Fact>
    Public Sub NullEdgecase()
        'if nothing is passed as arg, return blank SortedDictionary
        Dim ExpectedRes As New Dictionary(Of Integer, String)
        Assert.Equal(JsonSerializer.Serialize(ExpectedRes), JsonSerializer.Serialize(GetDetachedLabels(Nothing)))
    End Sub

    <Fact>
    Public Sub BatchedPmTests()
        Dim TestDS As New Data.DataSet()
        Dim TestDT As New Data.DataTable()
        Dim ExpectedRes As New Dictionary(Of Integer, String)

        TestDT.Columns.Add("LabelKey", GetType(Integer))
        TestDT.Columns.Add("Label", GetType(String))
        TestDT.Columns.Add("PhaseKey", GetType(Integer))

        For I As Integer = 0 To 5
            Dim TestDR As Data.DataRow = TestDT.NewRow()
            Dim PhaseKey As Object = If(Rnd.Next(0, 2) = 1, I, DBNull.Value)
            Dim Label As String = "Label_" & I
            Dim LabelKey As Integer = I

            TestDR("LabelKey") = LabelKey
            TestDR("Label") = Label
            TestDR("PhaseKey") = PhaseKey

            If IsDBNull(PhaseKey) Then
                ExpectedRes(LabelKey) = Label
            End If

            TestDT.Rows.Add(TestDR)
        Next

        TestDS.Tables.Add(TestDT)
        Assert.True(AreHashesEqual(ExpectedRes, GetDetachedLabels(45, TestDS)))
    End Sub
End Class

Public Class AssignPhaseTests
    Inherits PhaseController
    Private SqlParameters As New SqlParameters()

    <Fact>
    Public Sub Arg1Null()
        Assert.False(Boolean.Parse(AssignPhase(Nothing, 3)("Success")))
    End Sub

    <Fact>
    Public Sub NullAssignPhaseTestCase()
        Dim LabelKey As Integer = 23
        Dim AssignPhaseRes As Dictionary(Of String, String) = AssignPhase(LabelKey, Nothing, True)
        Dim AssignPhaseHash As New Dictionary(Of String, String) From {
            {"LabelKey", LabelKey},
            {"PhaseKey", Nothing}
        }

        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogLabel] SET PhaseKey=@PhaseKey WHERE [Key]=@LabelKey", AssignPhaseRes("SqlQuery"))
        Assert.True(SqlParameters.ValidParameterizedValues(AssignPhaseHash, AssignPhaseRes))
    End Sub

    <Fact>
    Public Sub NullAssignPhaseWithSqlExecutions()
        'sql does NOT complain when an update query is ran on a record that doesn't exists in a table
        'do just that, to ensure return is as expected
        Dim LabelKey As Integer = 23
        Dim AssignPhaseRes As Dictionary(Of String, String) = AssignPhase(LabelKey, Nothing)
        Dim AssignPhaseHash As New Dictionary(Of String, String) From {
            {"LabelKey", LabelKey},
            {"PhaseKey", Nothing}
        }

        Assert.False(AssignPhaseRes.ContainsKey("SqlQuery"))
        Assert.False(AssignPhaseRes.ContainsKey("QueryConfig"))
        Assert.True(Boolean.Parse(AssignPhaseRes("Success")))
    End Sub

    <Theory>
    <InlineData(3, 4)>
    <InlineData(235, 238)>
    Public Sub AssignPhaseTestCases(LabelKey As Integer, PhaseKey As String)
        Dim AssignPhaseRes As Dictionary(Of String, String) = AssignPhase(LabelKey, PhaseKey, True)
        Dim AssignPhaseHash As New Dictionary(Of String, String) From {
            {"LabelKey", LabelKey},
            {"PhaseKey", PhaseKey}
        }

        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogLabel] SET PhaseKey=@PhaseKey WHERE [Key]=@LabelKey", AssignPhaseRes("SqlQuery"))
        Assert.True(SqlParameters.ValidParameterizedValues(AssignPhaseHash, AssignPhaseRes))
    End Sub

    <Theory>
    <InlineData(3, 4)>
    <InlineData(235, 238)>
    Public Sub AssignPhaseWithSqlExecutions(LabelKey As Integer, PhaseKey As Integer)
        'sql does NOT complain when an update query is ran on a record that doesn't exists in a table
        'do just that, to ensure return is as expected
        Dim AssignPhaseRes As Dictionary(Of String, String) = AssignPhase(LabelKey, PhaseKey)
        Dim AssignPhaseHash As New Dictionary(Of String, String) From {
            {"LabelKey", LabelKey},
            {"PhaseKey", PhaseKey}
        }

        Assert.False(AssignPhaseRes.ContainsKey("SqlQuery"))
        Assert.False(AssignPhaseRes.ContainsKey("QueryConfig"))
        Assert.True(Boolean.Parse(AssignPhaseRes("Success")))
    End Sub

End Class

Public Class GroupsOrPhasesInUseTests
    Inherits PhaseController
    Private Security As New Security()
    Private PMsWithPhases As New List(Of Integer)

    Sub New()
        'the sql query below returns primary keys of records in T_LogArea where 1 or more relevant records in T_LogLabel where PhaseKey field contains a value other than NULL 
        Dim PMsWithPhasesDS As Data.DataSet = Security.GetMyDataSetParamQuery("SELECT DISTINCT(AreaKey) As AreaKey FROM [ALTS].[dbo].[T_LogLabel] GROUP BY PhaseKey, AreaKey HAVING PhaseKey IS NOT NULL ORDER BY AreaKey", New Dictionary(Of String, Dictionary(Of String, String)))

        For Each PMsWithPhasesDR As Data.DataRow In PMsWithPhasesDS.Tables(0).Rows
            PMsWithPhases.Add(PMsWithPhasesDR("AreaKey"))
        Next
    End Sub

    <Fact>
    Private Sub NothingAsArg()
        Assert.False(GroupsOrPhasesInUse(Nothing))
    End Sub

    <Fact>
    Private Sub TrueReturnTestCases()
        For Each PMWithPhases As Integer In PMsWithPhases
            Assert.True(GroupsOrPhasesInUse(PMWithPhases))
        Next
    End Sub

    <Fact>
    Private Sub FalseReturnTestCases()
        Dim AreaDS As Data.DataSet = Security.GetMyDataSetParamQuery("SELECT TOP(20) [Key] FROM [ALTS].[dbo].[T_LogArea]", New Dictionary(Of String, Dictionary(Of String, String)))

        For Each AreaDR As Data.DataRow In AreaDS.Tables(0).Rows
            Dim AreaKey As Integer = AreaDR("Key")

            If PMsWithPhases.Contains(AreaKey) = False Then
                Assert.False(GroupsOrPhasesInUse(AreaKey))
            End If
        Next

    End Sub
End Class

Public Class DeleteBatchTests
    Inherits PhaseController
    Private Security As New Security()
    Private SqlParameters As New SqlParameters()

    <Fact>
    Public Sub UnsuccessfulEdgecases()
        Assert.False(Boolean.Parse(DeletePhaseOrGroup(Nothing)("Success")))
    End Sub

    <Theory>
    <InlineData(4)>
    <InlineData(238)>
    Public Sub DeletePhaseOrGroupTestCases(PhaseOrGroupKey As String)
        Dim DeletePhaseOrGroupRes As Dictionary(Of String, String) = DeletePhaseOrGroup(PhaseOrGroupKey, True)
        Dim DeletePhaseOrGroupHash As New Dictionary(Of String, String) From {
            {"PhaseOrGroupKey", PhaseOrGroupKey}
        }

        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogLabel] SET PhaseKey=NULL WHERE PhaseKey=@PhaseOrGroupKey; DELETE FROM [ALTS].[dbo].[T_LogPhase] WHERE [Key]=@PhaseOrGroupKey;", DeletePhaseOrGroupRes("SqlQuery"))
        Assert.True(SqlParameters.ValidParameterizedValues(DeletePhaseOrGroupHash, DeletePhaseOrGroupRes))
    End Sub

    <Theory>
    <InlineData(-1)>
    <InlineData(0)>
    Public Sub DeletePhaseOrGroupWithSqlExecutionsTestCases(PhaseOrGroupKey As String)
        'sql does NOT complain when a sql query is ran on a record that doesn't exists in a table
        'do just that, to ensure return is as expected
        Dim DeletePhaseOrGroupRes As Dictionary(Of String, String) = DeletePhaseOrGroup(PhaseOrGroupKey)
        Dim DeletePhaseOrGroupHash As New Dictionary(Of String, String) From {
            {"PhaseOrGroupKey", PhaseOrGroupKey}
        }

        Assert.False(DeletePhaseOrGroupRes.ContainsKey("SqlQuery"))
        Assert.False(DeletePhaseOrGroupRes.ContainsKey("QueryConfig"))
        Assert.True(Boolean.Parse(DeletePhaseOrGroupRes("Success")))
    End Sub

End Class

Public Class GetLabel_IdxTests
    Inherits PhaseController
    Private Security As New Security()
    Private SomeLabelsHavePhasesTestDs As Data.DataSet
    Private TestDsAndAllLabelsThatHavePhases As Data.DataSet
    Private NoPhasesTestDs As Data.DataSet

    Sub New()
        SomeLabelsHavePhasesTestDs = BuildTestDsAndSomeLabelThatHavePhases()
        NoPhasesTestDs = BuildNoPhasesTestDs()
    End Sub

    Private Sub AddDsRow(DT As Data.DataTable, RowConfig As Dictionary(Of String, Object))
        Dim DR As Data.DataRow = DT.NewRow()

        DR("LabelKey") = RowConfig("LabelKey")
        DR("PhaseKey") = RowConfig("PhaseKey")
        DR("PhaseOrder") = RowConfig("PhaseOrder")
        DR("LabelOrder") = RowConfig("LabelOrder")

        DT.Rows.Add(DR)
    End Sub

    <Fact>
    Public Sub NullArg()
        Assert.Equal(-1, GetLabel_Idx(Nothing))
    End Sub

    <Fact>
    Public Sub NullPhaseKeyAndSomeLabelsHavePhases()
        '1) T_LogLabel PhaseKey field value is NULL and Phases for the associated PM/Checklist exist
        Assert.Equal(1, GetLabel_Idx(593, SomeLabelsHavePhasesTestDs))
    End Sub

    <Fact>
    Public Sub NonNullPhaseKeyAndSomeLabelsHavePhases()
        '4) T_LogLabel PhaseKey field value is NOT NULL but not all associated records in T_LogLabel have a non null field value for PhaseKey field
        Assert.Equal(5, GetLabel_Idx(589, SomeLabelsHavePhasesTestDs))
    End Sub

    Private Function BuildNoPhasesTestDs() As Data.DataSet
        Dim DS As New Data.DataSet()
        Dim DT As New Data.DataTable()

        DT.Columns.Add("LabelKey", GetType(Integer))
        DT.Columns.Add("PhaseKey", GetType(Integer))
        DT.Columns.Add("PhaseOrder", GetType(Integer))
        DT.Columns.Add("LabelOrder", GetType(Integer))

        'AreaKey 58
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 553},
            {"PhaseKey", DBNull.Value},
            {"PhaseOrder", DBNull.Value},
            {"LabelOrder", 1}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 554},
            {"PhaseKey", DBNull.Value},
            {"PhaseOrder", DBNull.Value},
            {"LabelOrder", 2}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 555},
            {"PhaseKey", DBNull.Value},
            {"PhaseOrder", DBNull.Value},
            {"LabelOrder", 3}
        })

        DS.Tables.Add(DT)

        Return DS

    End Function

    Private Function BuildTestDsAndSomeLabelThatHavePhases() As Data.DataSet
        Dim DS As New Data.DataSet()
        Dim DT As New Data.DataTable()

        DT.Columns.Add("LabelKey", GetType(Integer))
        DT.Columns.Add("PhaseKey", GetType(Integer))
        DT.Columns.Add("PhaseOrder", GetType(Integer))
        DT.Columns.Add("LabelOrder", GetType(Integer))

        'AreaKey 75
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 592},
            {"PhaseKey", DBNull.Value},
            {"PhaseOrder", DBNull.Value},
            {"LabelOrder", 6}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 593},
            {"PhaseKey", DBNull.Value},
            {"PhaseOrder", DBNull.Value},
            {"LabelOrder", 7}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 590},
            {"PhaseKey", DBNull.Value},
            {"PhaseOrder", DBNull.Value},
            {"LabelOrder", 8}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 586},
            {"PhaseKey", 109},
            {"PhaseOrder", 2},
            {"LabelOrder", 1}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 586},
            {"PhaseKey", 109},
            {"PhaseOrder", 2},
            {"LabelOrder", 1}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 589},
            {"PhaseKey", 109},
            {"PhaseOrder", 2},
            {"LabelOrder", 5}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 588},
            {"PhaseKey", 110},
            {"PhaseOrder", 3},
            {"LabelOrder", 2}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 587},
            {"PhaseKey", 110},
            {"PhaseOrder", 3},
            {"LabelOrder", 3}
        })
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"LabelKey", 591},
            {"PhaseKey", 110},
            {"PhaseOrder", 3},
            {"LabelOrder", 4}
        })

        DS.Tables.Add(DT)

        Return DS
    End Function

End Class

Public Class GetSectionTypeTests
    Inherits PhaseController
    Private Security As New Security()

    <Fact>
    Public Sub NullArg()
        Assert.Equal("none", GetSectionType(Nothing))
    End Sub

    <Theory>
    <InlineData("none")>
    <InlineData("phase")>
    <InlineData("group")>
    Public Sub SectionTypeTestsWithFakeData(SectionType As String)
        'get 20 records with:
        '1) random primary key values from T_LogArea as AreaKey
        '2) arg passed to this invocation as the 'SectionType' field value
        Dim RandomChecklistsDS As Data.DataSet = Security.GetMyDataSetParamQuery("SELECT TOP(20) [Key] As AreaKey, '" & SectionType & "' As SectionType FROM [ALTS].[dbo].[T_LogArea] ORDER BY NEWID();", New Dictionary(Of String, Dictionary(Of String, String)))

        For Each RandomChecklistsDR As Data.DataRow In RandomChecklistsDS.Tables(0).Rows
            Dim DrSectionType As String = RandomChecklistsDR("SectionType")

            Assert.Equal(SectionType, DrSectionType)
            Assert.Equal(DrSectionType, GetSectionType(RandomChecklistsDR("AreaKey"), RandomChecklistsDR))
        Next
    End Sub

    <Fact>
    Public Sub SectionTypeTestsWithRealData()
        'get 20 records LIVE records
        Dim RandomChecklistsDS As Data.DataSet = Security.GetMyDataSetParamQuery("SELECT TOP(20) [Key] As AreaKey, SectionType FROM [ALTS].[dbo].[T_LogArea] ORDER BY NEWID();", New Dictionary(Of String, Dictionary(Of String, String)))

        For Each RandomChecklistsDR As Data.DataRow In RandomChecklistsDS.Tables(0).Rows
            Dim DrSectionType As String = RandomChecklistsDR("SectionType")
            Assert.Equal(DrSectionType, GetSectionType(RandomChecklistsDR("AreaKey")))
        Next
    End Sub

End Class

Public Class SetSectionTypeTests
    Inherits PhaseController
    Private SqlQuery As String = "UPDATE [ALTS].[dbo].[T_LogArea] SET SectionType=@SectionType WHERE [Key]=@AreaKey"

    <Fact>
    Public Sub NullArg()
        Dim SetTypeRes As Dictionary(Of String, String) = SetSectionType(Nothing, "none")

        Assert.False(Boolean.Parse(SetTypeRes("Success")))
    End Sub

    <Theory>
    <InlineData(34, "none")>
    <InlineData(46, "none")>
    <InlineData(34, "group")>
    <InlineData(23, "group")>
    <InlineData(3434, "phase")>
    <InlineData(23, "phase")>
    Public Sub SetSectionTypeWithFakeData(AreaKey As String, SectionType As String)
        Dim SetTypeRes As Dictionary(Of String, String) = SetSectionType(AreaKey, SectionType, True)
        Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@AreaKey", GetParamVarHash(AreaKey, "int")},
            {"@SectionType", GetParamVarHash(SectionType, "int")}
        }
        Dim SqlConfigFromRes As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(SetTypeRes("QueryConfig"))

        Assert.Equal(SqlQuery, SetTypeRes("SqlQuery"))
        Assert.Equal(Of Dictionary(Of String, Dictionary(Of String, String)))(SqlConfig, SqlConfigFromRes)
    End Sub

    <Theory>
    <InlineData(0, "none")>
    <InlineData(0, "group")>
    <InlineData(0, "phase")>
    Public Sub SetSectionTypeWithRealData(AreaKey As String, SectionType As String)
        Dim SetTypeRes As Dictionary(Of String, String) = SetSectionType(AreaKey, SectionType)
        Dim SqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@AreaKey", GetParamVarHash(AreaKey, "int")},
            {"@SectionType", GetParamVarHash(SectionType, "int")}
        }

        Assert.True(SetTypeRes.Count = 1)
        Assert.True(Boolean.Parse(SetTypeRes("Success")))
    End Sub
End Class
