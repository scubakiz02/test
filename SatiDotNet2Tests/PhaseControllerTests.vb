Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library
Imports System.Text.Json
Imports System.Linq

Public Class PhaseControllerTests
    Inherits Security
    Dim PhaseController As New PhaseController(82) 'EDG monthly checklist
    Dim TestDummyPhaseController As New PhaseController(75) 'dummy checklist
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

        DS = GetMyDataSetParamQuery("SELECT L.[Key] As LabelKey, P.Phase, P.PhaseOrder FROM [ALTS].[dbo].[T_LogLabel] L INNER JOIN [ALTS].[dbo].[T_LogPhase] P ON P.[Key]=L.PhaseKey WHERE L.AreaKey=75 ORDER BY P.PhaseOrder", QueryConfig)
        RC = DS.Tables(0).Rows.Count

        For I As Integer = 0 To RC - 1
            Dim DR As Data.DataRow = DS.Tables(0).Rows(I)
            Dim SubHash As New Dictionary(Of String, String)

            Try 'in case Phase is NULL
                SubHash("Phase") = DR("Phase")
                SubHash("PhaseOrder") = DR("PhaseOrder")

                GetPhasesExpected(DR("LabelKey")) = SubHash
            Catch ex As Exception
                Continue For
            End Try
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
        PhaseController = New PhaseController(48, JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))("{""388"":{""Date"":"""",""Operator"":"""",""Value"":""12""},""389"":{""Date"":"""",""Operator"":"""",""Value"":""32""},""390"":{""Date"":"""",""Operator"":"""",""Value"":""""}}"))
        Assert.Equal(Nothing, PhaseController.GetPhases())
    End Sub


    <Fact>
    Public Sub NoPhasesTest1()
        'test GetPhase() function against instantiation of class with T_LogData AreaKey 61, which does NOT contain phases
        Assert.Equal(Nothing, PhaseController2.GetPhases())
    End Sub
End Class