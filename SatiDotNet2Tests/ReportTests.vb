Imports System
Imports System.Globalization
Imports System.Runtime.InteropServices.ComTypes
Imports System.Text
Imports System.Text.Json
Imports SatiDotNet2.Library
Imports Xunit

Public Class ReportTestsLibrary
    Private _Config As New Dictionary(Of String, String) From {
        {"GroupKey", 0},
        {"AreaKey", 0}
    }

    Public Function CreateReport() As Report
        Dim Report As New Report(_Config)
        Report.SetGroup(1)
        Report.SetAreas({58}.ToList())
        Report.SetDateRange("08/23/2025", "08/24/2025")
        Return Report
    End Function

    Public Function AreDataSetsEqual(ds1 As DataSet, ds2 As DataSet) As Boolean
        If ds1 Is Nothing OrElse ds2 Is Nothing Then Return False

        ' Compare the number of tables
        If ds1.Tables.Count <> ds2.Tables.Count Then
            Return False
        End If

        ' Compare each table
        For i As Integer = 0 To ds1.Tables.Count - 1
            If Not AreDataTablesEqual(ds1.Tables(i), ds2.Tables(i)) Then
                Return False
            End If
        Next

        Return True
    End Function

    Private Function AreDataTablesEqual(table1 As DataTable, table2 As DataTable) As Boolean
        ' Check if both tables have the same column count and names
        If table1.Columns.Count <> table2.Columns.Count Then
            Return False
        End If

        For i As Integer = 0 To table1.Columns.Count - 1
            If table1.Columns(i).ColumnName <> table2.Columns(i).ColumnName Then
                Return False
            End If
        Next

        ' Check if both tables have the same row count
        If table1.Rows.Count <> table2.Rows.Count Then
            Return False
        End If

        ' Compare the rows
        For rowIndex As Integer = 0 To table1.Rows.Count - 1
            For colIndex As Integer = 0 To table1.Columns.Count - 1
                If Not Object.Equals(table1.Rows(rowIndex)(colIndex), table2.Rows(rowIndex)(colIndex)) Then
                    Return False
                End If
            Next
        Next

        Return True
    End Function

    Public Function RemoveAreasFromDS(ReportDS As Data.DataSet, AreasToExclude As Integer()) As Data.DataSet
        'programmatically delete rows that are tied to values in 'AreasToExclude' array
        For Each DR As Data.DataRow In ReportDS.Tables(0).Rows
            If AreasToExclude.Contains(DR("AreaKey")) Then
                DR.Delete() 'mark row for deletion
            End If
        Next
        ReportDS.Tables(0).AcceptChanges() 'remove DataRow(s) marked for deletion permanently

        Return ReportDS
    End Function

    Public Function NumOfRecords(Report As Report)
        Return Report.GetDS().Tables(0).Rows.Count
    End Function

    Public Function NumOfRecords(Ds As Data.DataSet)
        Return Ds.Tables(0).Rows.Count
    End Function
End Class

Public Class BuildWhereClauseTests
    Inherits Report
    Private _Security As New Security()

    <Theory>
    <InlineData(1, "09/13/2025", "09/17/2025")>
    <InlineData(3, "07/01/2029", "07/21/2029")>
    Public Sub BasicQueryTest(GroupKey As Integer, StartDate As String, EndDate As String)
        'what is a 'basic query' in this context? Great question!
        'a basic query is one where the config object contains only the required keys (GroupKey, StartDate, EndDate)
        Dim Config As New Dictionary(Of String, Object) From {
            {"GroupKey", GroupKey},
            {"StartDate", StartDate},
            {"EndDate", EndDate}
        }
        Dim ActualSqlConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim ActualWhereClause As String = BuildWhereClause(Config, ActualSqlConfig)
        Dim ExpectedSqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@GroupKey", _Security.GetParamVarHash(GroupKey, "int")},
            {"@StartDate", _Security.GetParamVarHash(StartDate, "string")},
            {"@EndDate", _Security.GetParamVarHash(EndDate, "string")}
        }
        Dim ExpectedWhereClause As String = "WHERE A.GroupKey=@GroupKey And D.Date >= @StartDate And D.Date <= @EndDate "

        Assert.Equal(ExpectedWhereClause, ActualWhereClause)
        Assert.Equal(Of Dictionary(Of String, Dictionary(Of String, String)))(ExpectedSqlConfig, ActualSqlConfig)
    End Sub

    <Theory>
    <InlineData(1, "09/13/2025", "09/17/2025", {1, 5})>
    Public Sub AreaKeysToExcludeTest(GroupKey As Integer, StartDate As String, EndDate As String, AreaKeysToExclude As Integer())
        Dim Config As New Dictionary(Of String, Object) From {
            {"GroupKey", GroupKey},
            {"StartDate", StartDate},
            {"EndDate", EndDate},
            {"AreasToExclude", AreaKeysToExclude.ToList()}
        }
        Dim ActualSqlConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim ActualWhereClause As String = BuildWhereClause(Config, ActualSqlConfig)
        Dim ExpectedSqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@GroupKey", _Security.GetParamVarHash(GroupKey, "int")},
            {"@AreaKey1", _Security.GetParamVarHash(1, "int")},
            {"@AreaKey5", _Security.GetParamVarHash(5, "int")},
            {"@StartDate", _Security.GetParamVarHash(StartDate, "string")},
            {"@EndDate", _Security.GetParamVarHash(EndDate, "string")}
        }
        Dim ExpectedWhereClause As String = "WHERE A.GroupKey=@GroupKey " &
            "And D.Date >= @StartDate And D.Date <= @EndDate " &
            "And A.[Key] Not In (@AreaKey1, @AreaKey5) "

        Assert.Equal(ExpectedWhereClause, ActualWhereClause)
        Assert.Equal(Of Dictionary(Of String, Dictionary(Of String, String)))(ExpectedSqlConfig, ActualSqlConfig)
    End Sub

    <Theory>
    <InlineData(1, "09/13/2025", "09/17/2025", {1, 5}, {3, 6})>
    Public Sub LabelKeysToExcludeTest(GroupKey As Integer, StartDate As String, EndDate As String, AreaKeysToExclude As Integer(), LabelKeysToExclude As Integer())
        'in the UI, end users will be focusing on 1 pm/checklist
        'this is the only time end users have the option to select inputs (a.k.a labels) to exclude
        Dim Config As New Dictionary(Of String, Object) From {
            {"GroupKey", GroupKey},
            {"StartDate", StartDate},
            {"EndDate", EndDate},
            {"AreasToExclude", AreaKeysToExclude.ToList()},
            {"LabelsToExclude", LabelKeysToExclude.ToList()}
        }
        Dim ActualSqlConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim ActualWhereClause As String = BuildWhereClause(Config, ActualSqlConfig)
        Dim ExpectedSqlConfig As New Dictionary(Of String, Dictionary(Of String, String)) From {
            {"@GroupKey", _Security.GetParamVarHash(GroupKey, "int")},
            {"@AreaKey1", _Security.GetParamVarHash(1, "int")},
            {"@AreaKey5", _Security.GetParamVarHash(5, "int")},
            {"@LabelKey3", _Security.GetParamVarHash(3, "int")},
            {"@LabelKey6", _Security.GetParamVarHash(6, "int")},
            {"@StartDate", _Security.GetParamVarHash(StartDate, "string")},
            {"@EndDate", _Security.GetParamVarHash(EndDate, "string")}
        }
        Dim ExpectedWhereClause As String = "WHERE A.GroupKey=@GroupKey " &
            "And D.Date >= @StartDate And D.Date <= @EndDate " &
            "And A.[Key] Not In (@AreaKey1, @AreaKey5) " &
            "And L.[Key] Not In (@LabelKey3, @LabelKey6) "

        Assert.Equal(ExpectedWhereClause, ActualWhereClause)
        Assert.Equal(Of Dictionary(Of String, Dictionary(Of String, String)))(ExpectedSqlConfig, ActualSqlConfig)
    End Sub
End Class

Public Class ReportTests
    Inherits ReportTestsLibrary

    <Fact>
    Public Sub NonNullDsTest()
        Dim Report As Report = CreateReport()
        Dim ReportDs As Data.DataSet = Report.GetDS()
        Assert.NotEqual(0, ReportDs.Tables(0).Rows.Count)
    End Sub


    <Fact>
    Public Sub TwoDatasetsSameEnvironmentTest()
        Dim Report1 As Report = CreateReport()
        Dim Report1Ds As Data.DataSet = Report1.GetDS()
        Dim Report2 As Report = CreateReport()
        Assert.NotEqual(0, Report1Ds.Tables(0).Rows.Count)
        Assert.True(AreDataSetsEqual(Report1Ds, Report2.GetDS()))
    End Sub

    <Fact>
    Public Sub DatasetValuesTest()
        'test ConfigureDS function against a live dataset
        Dim Report As Report = CreateReport()
        Dim DS As Data.DataSet = Report.GetDS()
        Dim DR As Data.DataRow
        Dim RC = DS.Tables(0).Rows.Count - 1
        Dim AccurateData As Boolean = True

        For I As Integer = 0 To RC
            DR = DS.Tables(0).Rows(I)

            If DR("DataKey") = 253 AndAlso DR("LabelKey") = 446 AndAlso DR("Value") <> 7.93 Then 'T_LogData Key 253, T_LogLabel Key 446. Inputs field is in old format. Input value should 7.93
                AccurateData = False
                Exit For
            ElseIf DR("DataKey") = 312 AndAlso DR("LabelKey") = 566 AndAlso DR("Value") <> 45.8 Then 'T_LogData Key 312, T_LogLabel Key 566. Inputs field is in new format. Input value should 45.8
                AccurateData = False
                Exit For
            ElseIf DR("DataKey") = 352 AndAlso DR("LabelKey") = 515 AndAlso DR("Value") <> 66.5 Then 'T_LogData Key 352, T_LogLabel Key 515. Inputs field is in new format. Input value should 66.5
                AccurateData = False
                Exit For

            End If
        Next

        Assert.NotEqual(0, RC)
        Assert.True(AccurateData)
    End Sub

    <Fact>
    Public Sub DatasetValuesTest2()
        'test ConfigureDS function against a live dataset, with a specific AreaKey 
        Dim AreaKey As Integer = 60 'SC-3 Fume Scrubber Monitoring Daily
        Dim Report As Report = CreateReport()
        Report.SetAreas({AreaKey}.ToList())
        Dim DS As Data.DataSet = Report.GetDS()
        Dim RC As Integer = DS.Tables(0).Rows.Count - 1
        Dim AccurateData As Boolean = True

        For I As Integer = 0 To RC
            Dim DR As Data.DataRow = DS.Tables(0).Rows(I)

            If DR("DataKey") = 253 Then 'T_LogData Key 253 is associated with a checklist other than the one passed to the constructor
                AccurateData = False
                Exit For
            ElseIf DR("DataKey") = 279 AndAlso DR("LabelKey") = 561 AndAlso DR("Value") <> 9.3 Then 'T_LogData Key 312, T_LogLabel Key 566. Inputs field is in the old format. Input value should 9.3
                AccurateData = False
                Exit For
            ElseIf DR("DataKey") = 357 AndAlso DR("LabelKey") = 560 AndAlso DR("Value") <> 0.48 Then 'T_LogData Key 352, T_LogLabel Key 560. Inputs field is in new format. Input value should .48
                AccurateData = False
                Exit For

            End If

        Next

        Assert.NotEqual(0, RC)
        Assert.True(AccurateData)
    End Sub

    <Fact>
    Public Sub DatasetDateRangeTest()
        'date range is passed to the constructor config object. ensure GetDS() from Report class is within the set date range
        Dim Report As Report = CreateReport()
        Dim StartDate As Date = "03-22-2025"
        Dim EndDate As Date = "03-25-2025"
        Report.SetDateRange(StartDate, EndDate)
        Dim DS As Data.DataSet = Report.GetDS()
        Dim RC = DS.Tables(0).Rows.Count - 1
        Dim OutOfRange As Boolean = False

        For I As Integer = 0 To RC
            Dim DR As Data.DataRow = DS.Tables(0).Rows(I)
            Dim InputDate As Date = DR("StartDate") 'StartDate field is the date the checklist log was created

            If InputDate.Date < StartDate.Date OrElse InputDate.Date > EndDate.Date Then
                OutOfRange = True
                Exit For
            End If
        Next

        Assert.NotEqual(0, RC)
        Assert.False(OutOfRange)
    End Sub


    <Fact>
    Public Sub DatasetDateRangeTest2()
        'start date and end date are the same date. There should be rows returned
        Dim Report As Report = CreateReport()
        Report.SetDateRange("03-22-2025", "03-22-2025")
        Dim Ds As Data.DataSet = Report.GetDS()

        Assert.NotEqual(0, Ds.Tables(0).Rows.Count)
    End Sub

    <Fact>
    Public Sub GetMaxFieldValsTest1()
        'calling GetMaxFieldVals function. test return field values against entire dataset for GroupKey 1 AreaKey 48
        Dim Report As Report = CreateReport()
        Report.SetAreas({48}.ToList()) 'Nitrogen Daily
        Dim MaxFieldVals As Dictionary(Of String, String) = Report.GetMaxFieldVals() 'idx slots for MaxFieldVals: Area, Label, Value, Date, InputOperator
        Dim AccurateResults As Boolean = True

        If MaxFieldVals("Label").Length < ("Check for leaks - blow-off").Length Then 'in case a new label with longer length is added
            AccurateResults = False
        ElseIf String.IsNullOrEmpty(MaxFieldVals("Value")) Then 'changes too often. not wise to test against a static value
            AccurateResults = False
        ElseIf String.IsNullOrEmpty(MaxFieldVals("InputDate")) Then 'all values are the same length
            AccurateResults = False
        ElseIf LCase(MaxFieldVals("Area")) <> "nitrogen daily" Then
            AccurateResults = False
        ElseIf String.IsNullOrEmpty(MaxFieldVals("InputOperator")) Then
            AccurateResults = False
        End If

        Assert.True(AccurateResults)
    End Sub

    <Fact>
    Public Sub GetMaxFieldValsTest2()
        'calling GetMaxFieldVals function. test return field values against entire dataset for GroupKey 1 AreaKey 56
        Dim Report As Report = CreateReport()
        Report.SetAreas({56}.ToList()) 'R.O. Daily
        Dim MaxFieldVals As Dictionary(Of String, String) = Report.GetMaxFieldVals() 'idx slots for MaxFieldVals: Area, Label, Value, Date, InputOperator
        Dim AccurateResults As Boolean = True

        If MaxFieldVals("Label").Contains("1st Pass System Pressure, Final Concentrate") = False Then
            AccurateResults = False
        ElseIf String.IsNullOrEmpty(MaxFieldVals("Value")) Then 'changes too often. not wise to test against a static value
            AccurateResults = False
        ElseIf String.IsNullOrEmpty(MaxFieldVals("InputDate")) Then 'all values are the same length
            AccurateResults = False
        ElseIf LCase(MaxFieldVals("Area")) <> "r.o. daily" Then
            AccurateResults = False
        ElseIf String.IsNullOrEmpty(MaxFieldVals("InputOperator")) Then
            AccurateResults = False
        End If

        Assert.True(AccurateResults)
    End Sub
End Class

Public Class SetAreasTests
    Inherits ReportTestsLibrary

    Private GroupKey1AreasHash As New Dictionary(Of Integer, String) From {
        {48, "Nitrogen Daily"},
        {55, "AWN Daily"},
        {56, "R.O. Daily"},
        {57, "DI WATER DAILY"},
        {58, "SC-1 Fume Scrubber Monitoring Daily"},
        {59, "SC-2 Fume Scrubber Monitoring Daily"},
        {60, "SC-3 Fume Scrubber Monitoring Daily"},
        {61, "Daily Fluoride Measurement Log"}
    }

    Private Function CustomReport() As Report
        Dim Report As Report = New Report(New Dictionary(Of String, String) From {
            {"GroupKey", 0},
            {"AreaKey", 0}
        })
        Report.SetGroup(1)
        Report.SetDateRange("08/01/2025", "08/01/2025")

        Return Report
    End Function

    Private Function IncludeAreasFromDS(AreasToInclude As Integer()) As Data.DataSet
        'programmatically delete rows that are not in 'AreasToInclude' array
        Dim ReportDS As Data.DataSet = CustomReport().GetDS()

        For Each DR As Data.DataRow In ReportDS.Tables(0).Rows
            If AreasToInclude.Contains(DR("AreaKey")) = False Then
                DR.Delete() 'mark row for deletion
            End If
        Next
        ReportDS.Tables(0).AcceptChanges() 'remove DataRow(s) marked for deletion permanently

        Return ReportDS
    End Function

    <Fact>
    Public Sub SetAreasEmptyListArg()
        'if SetAreas receivs empty list as arg 1, return empty dataset
        Dim Report As Report = CreateReport()
        Report.SetAreas(New List(Of Integer))
        Assert.Equal(0, NumOfRecords(Report))
    End Sub

    <Fact>
    Public Sub SetAreasAllAreas()
        Dim Report1 As Report = CustomReport()
        Dim ExpectedDS As Data.DataSet = Report1.GetDS()

        Dim Report2 As Report = CustomReport()
        Report2.SetAreas(GroupKey1AreasHash.Keys.ToList())
        Dim ActualDS As Data.DataSet = Report2.GetDS()

        Assert.NotEqual(0, NumOfRecords(Report2))
        Assert.True(AreDataSetsEqual(ActualDS, ExpectedDS))
    End Sub

    <Fact>
    Public Sub SetAreasExclude1Area()
        Dim GroupKey1AreasHashCopy As New Dictionary(Of Integer, String)(GroupKey1AreasHash)

        Dim ExpectedDS As Data.DataSet = RemoveAreasFromDS(CustomReport().GetDS(), {48})
        GroupKey1AreasHashCopy.Remove(48)

        Dim Report2 As Report = CustomReport()
        Report2.SetAreas(GroupKey1AreasHashCopy.Keys.ToList())
        Dim ActualDS As Data.DataSet = Report2.GetDS()

        Assert.NotEqual(0, NumOfRecords(Report2))
        Assert.True(AreDataSetsEqual(ActualDS, ExpectedDS))
    End Sub

    <Fact>
    Public Sub RestoreLabelsOnSetAreas()
        ' end user can filter inputs if they are dialed in on 1 pm/checklist
        ' if they were to then include another pm/checklist, all inputs from both pm/checklists must be included
        Dim AreasToInclude As Integer() = {58, 59}
        Dim ExpectedDS As Data.DataSet = IncludeAreasFromDS(AreasToInclude)

        Dim Report As Report = CustomReport()
        Report.SetAreas({58}.ToList())
        Report.SetLabels({553, 554}.ToList()) 'exclude a label (LabelKey 555)
        Report.SetAreas({58, 59}.ToList())
        Dim ActualDS As Data.DataSet = Report.GetDS()

        Assert.NotEqual(0, NumOfRecords(Report))
        Assert.True(AreDataSetsEqual(ActualDS, ExpectedDS))
    End Sub

    <Fact>
    Public Sub ExcludeAreaBeforeSetDateRangeTest()
        Dim GroupKey1AreasHashCopy As New Dictionary(Of Integer, String)(GroupKey1AreasHash)

        Dim ExpectedDS As Data.DataSet = RemoveAreasFromDS(CustomReport().GetDS(), {48})
        GroupKey1AreasHashCopy.Remove(48)

        Dim Report As Report = New Report(New Dictionary(Of String, String) From {
            {"GroupKey", 0},
            {"AreaKey", 0}
        })
        Report.SetGroup(1) 'end user interacts with group ddl
        Report.SetAreas(GroupKey1AreasHashCopy.Keys.ToList()) 'end user interacts with and unselects 1 pm/checklist from modal
        Report.SetDateRange("08/01/2025", "08/01/2025") 'end user interacts with date range pickers
        Dim ActualDS As Data.DataSet = Report.GetDS()

        Assert.NotEqual(0, NumOfRecords(Report))
        Assert.True(AreDataSetsEqual(ActualDS, ExpectedDS))
    End Sub
End Class

Public Class LabelFunctionTests
    Inherits ReportTestsLibrary

    Private LabelsHash As New Dictionary(Of Integer, String) From {
        {553, "Recirculation Water | >75 GPM"},
        {554, "DP Across Media | 1-3 inH20"},
        {555, "Recirculation Water | 3-10 pH"}
    }
    Private _AreaKey As Integer = 58

    Private Function CreateCustomReport() As Report
        '1 day, 3 records (for simplicity of troubleshooting)
        Dim Report As Report = CreateReport()
        Report.SetAreas({_AreaKey}.ToList())
        Report.SetDateRange("04/01/2025", "04/01/2025")
        Return Report
    End Function

    <Fact>
    Public Sub BaselineTest()
        'check if return dataset from class instantiation is as expected
        Dim Report As Report = CreateCustomReport()
        Assert.Equal(3, NumOfRecords(Report))
    End Sub

    <Fact>
    Public Sub SetLabelsAllLabels()
        'pass list of all labels for AreaKey 58, and check to see if instantiation holds the same dataset before function SetLabels in called
        Dim Report1 As Report = CreateCustomReport()
        Dim ExpectedDs As Data.DataSet = Report1.GetDS()

        Dim Report2 As Report = CreateCustomReport()
        Report2.SetLabels(LabelsHash.Keys.ToList())
        Dim ActualDs As Data.DataSet = Report2.GetDS()

        Assert.Equal(3, NumOfRecords(Report2))
        Assert.True(AreDataSetsEqual(ActualDs, ExpectedDs))
    End Sub

    <Fact>
    Public Sub SetLabelsExcludeALabel()
        'pass list that excludes 1 label for AreaKey 58, and check to see if instantiation holds different dataset before function SetLabels in called
        Dim LabelsHashNew As New Dictionary(Of Integer, String)(LabelsHash)
        Dim RemovedKvp As New Dictionary(Of Integer, String) From {
            {553, "Recirculation Water | >75 GPM"}
        }
        Dim LabelsToInclude As New List(Of Integer)
        Dim Report1 As Report = CreateCustomReport()
        Dim ExpectedDS As Data.DataSet = Report1.GetDS()

        'remove rows from DataSet programmatically that include LabelToRemove
        LabelsHashNew.Remove(0)
        For Each DR As Data.DataRow In ExpectedDS.Tables(0).Rows
            If RemovedKvp.ContainsKey(DR("LabelKey")) Then
                DR.Delete() 'mark DataRow for deletion
            Else
                LabelsToInclude.Add(DR("LabelKey"))
            End If
        Next
        ExpectedDS.Tables(0).AcceptChanges() 'remove DataRow(s) marked for deletion permanently

        Dim Report2 As Report = CreateCustomReport()
        Dim ActualDs As Data.DataSet = Report2.SetLabels(LabelsToInclude)

        Assert.Equal(2, NumOfRecords(Report2))
        Assert.True(AreDataSetsEqual(ActualDs, ExpectedDS))
    End Sub


    <Fact>
    Public Sub ExcludeLabelBeforeSetDateRangeTest()
        Dim Report1 As Report = CreateReport()
        Report1.SetAreas({58}.ToList())
        Report1.SetLabels({553, 554}.ToList()) 'end user interacts with and unselects 1 label from pm/checklist using label modal
        Report1.SetDateRange("08/01/2025", "08/01/2025") 'end user interacts with date range pickers
        Dim ExpectedDS As Data.DataSet = Report1.GetDS()

        Dim Report2 As Report = New Report(New Dictionary(Of String, String) From {
            {"GroupKey", 0},
            {"AreaKey", 0}
        })
        Report2.SetGroup(1) 'end user interacts with group ddl
        Report2.SetAreas({58}.ToList()) 'end user interacts with and unselects 1 pm/checklist using area modal
        Report2.SetLabels({553, 554}.ToList()) 'end user interacts with and unselects 1 label from pm/checklist using label modal
        Report2.SetDateRange("08/01/2025", "08/01/2025") 'end user interacts with date range pickers
        Dim ActualDS As Data.DataSet = Report2.GetDS()

        Assert.NotEqual(0, NumOfRecords(Report2))
        Assert.True(AreDataSetsEqual(ActualDS, ExpectedDS))
    End Sub
End Class

Public Class GetOperatorsTest
    Inherits ReportTestsLibrary

    Private _OperatorDs As Data.DataSet

    Sub New()
        _OperatorDs = BuildOperatorsDs()
    End Sub

    Private Function BuildOperatorsDs() As Data.DataSet
        Dim Report As Report = CreateReport()
        Report.SetAreas({58}.ToList())
        Report.SetDateRange("08/01/2025", "10/01/2025")
        Return Report.GetOperators()
    End Function

    <Fact>
    Public Sub NoDuplicateInputOperatorValues()
        Dim DS As Data.DataSet = _OperatorDs.Copy()
        Assert.NotEqual(0, NumOfRecords(DS))

        Dim OperatorHashset As New HashSet(Of String)
        For Each DR As Data.DataRow In DS.Tables(0).Rows
            Dim InputOperator As Object = DR("Operator")
            Assert.False(OperatorHashset.Contains(InputOperator))
            OperatorHashset.Add(InputOperator)
        Next
    End Sub

    <Fact>
    Public Sub NoDuplicateInputOperatorValues2()
        'some operators have duplicate entries in operator ds with different casing (Ex: Mark Kiser and mark kiser, Andrew Williams and andrew williams)
        'account for this edge case
        Dim DS As Data.DataSet = _OperatorDs.Copy()
        Assert.NotEqual(0, NumOfRecords(DS))

        Dim OperatorHashset As New HashSet(Of String)
        For Each DR As Data.DataRow In DS.Tables(0).Rows
            Dim InputOperator As Object = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DR("Operator").ToLower())
            Assert.False(OperatorHashset.Contains(InputOperator))
            OperatorHashset.Add(InputOperator)
        Next
    End Sub

    <Fact>
    Public Sub NoEmptyStringOperatorValues()
        Dim DS As Data.DataSet = _OperatorDs.Copy()
        Assert.NotEqual(0, NumOfRecords(DS))

        Dim OperatorHashset As New HashSet(Of String)
        For Each DR As Data.DataRow In DS.Tables(0).Rows
            Dim InputOperator As Object = DR("Operator")
            Assert.NotEqual("", InputOperator)
        Next
    End Sub
End Class

Public Class AdminOverrideTests
    Inherits ReportTestsLibrary

    Private _Security As New Security()
    Private _Report As Report
    Private LabelKey As Integer = 553

    Public Sub New()
        _Report = CreateReport()
        _Report.SetAreas({58}.ToList()) 'SC-1 Fume Scrubber Monitoring Daily
        _Report.SetDateRange("04/14/2025", "04/14/2025")
    End Sub

    <Fact>
    Public Sub DbSanityCheck()
        'sanity check DB field values that will be changed within foo unit test
        ' T_LogData [Key] 514, DB field values Value (153) & InputDate (04/14/2025 09:50:20 AM)

        ' SC-1 Fume Scrubber Monitoring Daily for 04/14/2025 (T_LogData [Key] = 514)
        '- LabelKey: 553 (Recirculation Water | >75 GPM)
        '- Value: 153	
        '- InputDate: 04/14/2025 09:50:20 AM
        '- Operator: Chase Dostie
        Dim InputsJson As String = _Security.GetSingleDbField("SELECT Inputs FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=514", New Dictionary(Of String, Dictionary(Of String, String)), "Inputs")
        Dim Inputs As Dictionary(Of Integer, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(InputsJson)

        Assert.Equal(Inputs(LabelKey)("Value"), 153)
        Assert.Equal(Inputs(LabelKey)("Date"), "04/14/2025 09:50:20 AM")
    End Sub

    <Fact>
    Public Sub OverrideValueAndDate()
        'override the Value And Date recorded in T_LogData Inputs field value for an Label 533 (Recirculation Water | >75 GPM) on 04/14/2025 (T_LogData [Key] 514)
        Dim NewValue As Integer = 154
        Dim NewDate As String = "04/14/2025 09:51:20 AM"
        Dim Mods As New Dictionary(Of String, String) From {
            {"Value", NewValue},
            {"Date", NewDate}
        }
        Dim Config As New Dictionary(Of String, String) From {
            {"LabelKey", LabelKey},
            {"Date", "04/14/2025"}
        }
        Dim OverrideInputs As Dictionary(Of Integer, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(_Report.Override(Config, Mods))

        Assert.Equal(OverrideInputs(LabelKey)("Value"), NewValue)
        Assert.Equal(OverrideInputs(LabelKey)("Date"), NewDate)
    End Sub
End Class

Public Class DummyDS
    Private Shared Random As New Random()

    Function InstantiationWithData(Optional RC As Integer = 1) As RecordSet
        Dim DS As New Data.DataSet
        Dim DT As New Data.DataTable

        DT.Columns.Add("Boolean", GetType(Boolean))
        DT.Columns.Add("String", GetType(String))
        DT.Columns.Add("Integer", GetType(Integer))
        DT.Columns.Add("Date", GetType(Date))
        DT.Columns.Add("Decimal", GetType(Decimal))
        DT.Columns.Add("Double", GetType(Double))
        DT.Columns.Add("Null", GetType(String))

        For I As Integer = 0 To RC - 1
            GenerateNewDR(DT)
        Next

        DS.Tables.Add(DT)

        Return New RecordSet(DS)
    End Function

    Function GetRandomString(length As Integer) As String
        Const chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
        Dim result As New Text.StringBuilder()
        Dim Random As New Random()

        For i As Integer = 1 To length
            result.Append(chars(Random.Next(0, chars.Length)))
        Next

        Return result.ToString()
    End Function

    Public Sub GenerateNewDR(DT As Data.DataTable)
        Dim startDate As Date = #1/1/2000#
        Dim endDate As Date = #12/31/2030#
        Dim range As Integer = (endDate - startDate).Days
        Dim DR As Data.DataRow = DT.NewRow()

        DR("Boolean") = Random.Next(0, 2) = 1
        DR("String") = GetRandomString(Random.Next(0, 10))
        DR("Integer") = Random.Next(0, 100)
        DR("Date") = startDate.AddDays(Random.Next(0, range))
        DR("Decimal") = CDec(Random.NextDouble()) * 100
        DR("Double") = Random.NextDouble() * 100
        DR("Null") = DBNull.Value

        DT.Rows.Add(DR)
    End Sub

End Class

Public Class ReturnZeroRecordsEdgecases
    Inherits ReportTestsLibrary

    Private _Config As New Dictionary(Of String, String) From {
        {"GroupKey", 0},
        {"AreaKey", 0}
    }

    'if connected to dev db and tests are failing, make sure records exist from 1 month ago
    Private UpperBoundAt As String = Today.AddMonths(-1).ToString("MM/dd/yyyy")
    Private LowerBoundAt As String = Date.Parse(UpperBoundAt).AddDays(-7).ToString("MM/dd/yyyy")

    <Fact>
    Public Sub SetGroupTest()
        Dim Report As New Report(_Config)
        Report.SetGroup(1)

        Dim ReportDs As Data.DataSet = Report.GetDS()
        Assert.Equal(0, ReportDs.Tables(0).Rows.Count)
    End Sub

    <Fact>
    Public Sub SetGroupAndAreasTest()
        Dim Report As New Report(_Config)
        Report.SetGroup(1)
        Report.SetAreas({58}.ToList())

        Dim ReportDs As Data.DataSet = Report.GetDS()
        Assert.Equal(0, ReportDs.Tables(0).Rows.Count)
    End Sub

    <Fact>
    Public Sub EndDatePrecedesStartDate()
        Dim Report As New Report(_Config)
        Report.SetGroup(1)
        Report.SetDateRange(UpperBoundAt, LowerBoundAt)
        Assert.Equal(0, NumOfRecords(Report))
    End Sub

    <Fact>
    Public Sub StartDateSucceedsEndDate()
        Dim Report As New Report(_Config)
        Report.SetGroup(1)
        Report.SetDateRange(Date.Parse(LowerBoundAt).AddDays(1), LowerBoundAt)
        Assert.Equal(0, NumOfRecords(Report))
    End Sub

    <Fact>
    Public Sub EmptyStartDate()
        Dim Report As New Report(_Config)
        Report.SetGroup(1)
        Report.SetDateRange(String.Empty, UpperBoundAt)
        Assert.Equal(0, NumOfRecords(Report))
    End Sub

    <Fact>
    Public Sub EmptyEndDate()
        Dim Report As New Report(_Config)
        Report.SetGroup(1)
        Report.SetDateRange(LowerBoundAt, String.Empty)
        Assert.Equal(0, NumOfRecords(Report))
    End Sub
End Class
Public Class RecordSetTests
    Inherits DummyDS

    Private TestDS1 As RecordSet
    Private DSWith1Row As RecordSet
    Private BlankDS As New RecordSet(New Data.DataSet)
    Dim NullDS As Data.DataSet = Nothing

    Sub New()
        TestDS1 = New RecordSet(New Data.DataSet)
        DSWith1Row = InstantiationWithData()
    End Sub

    <Fact>
    Public Sub NullDSTest()
        'if GlobalDS within DataSet is Nothing, return false
        TestDS1 = New RecordSet(NullDS)
        Assert.False(TestDS1.DataSetsMatch(BlankDS))
    End Sub

    <Fact>
    Public Sub NullArgToDataSetsMatch()
        'if arg passed to DataSet.DataSetsMatch() is Nothing, return false
        Assert.False(TestDS1.DataSetsMatch(New RecordSet(NullDS)))
    End Sub

    <Fact>
    Public Sub TwoBlankDataSets()
        'call DataSetsMatch on an instantiation with an empty DS. pass empty DS to DataSetsMatch
        Assert.True(TestDS1.DataSetsMatch(New RecordSet(New Data.DataSet)))
    End Sub

    <Fact>
    Public Sub NumOfRows()
        'call DataSetsMatch on 2 instantiations of RecordSet class that DO NOT hold the same # of rows
        TestDS1 = InstantiationWithData(2)
        Assert.False(TestDS1.DataSetsMatch(DSWith1Row))
    End Sub

    <Fact>
    Public Sub MatchingFieldValues()
        'call DataSetsMatch on 2 instantiations of RecordSet class that DO NOT hold the same data
        Dim DS1 As RecordSet = InstantiationWithData(3)
        Assert.False(DS1.DataSetsMatch(InstantiationWithData(3)))
    End Sub

    <Fact>
    Public Sub MatchingFieldValues2()
        'call DataSetsMatch on 2 instantiations of RecordSet class that do hold the same data, but NOT in the same order
        Dim RS As RecordSet = InstantiationWithData(3)
        Dim DS As Data.DataSet = RS.GetDS()
        Dim DS_Table As Data.DataTable = DS.Tables(0)
        Dim ReorderedDT As DataTable = DS_Table.Clone()
        Dim ReorderedDS As New DataSet

        ReorderedDT.ImportRow(DS_Table.Rows(2)) ' move this one first
        ReorderedDT.ImportRow(DS_Table.Rows(0))
        ReorderedDT.ImportRow(DS_Table.Rows(1))

        ReorderedDS.Tables.Add(ReorderedDT)

        Assert.False(RS.DataSetsMatch(New RecordSet(ReorderedDS)))
    End Sub

    <Fact>
    Public Sub DBNullBug()
        'call DataSetsMatch on the same instantiation of RecordSet class, to get the DBNull values to compare
        TestDS1 = InstantiationWithData(2)
        Assert.True(TestDS1.DataSetsMatch(TestDS1))
    End Sub
End Class

Public Class OrderedByDateTests
    Inherits ReportTestsLibrary

    Private Function CreateCustomReport() As Report
        Dim Report As Report = CreateReport()
        Report.SetDateRange("08/11/2025", "08/12/2025")
        Return Report
    End Function

    Private Function IsDsOrderedByDate(Ds As Data.DataSet) As Boolean
        Dim StartDateAt As String = "08/11/2025"
        For I As Integer = 0 To Ds.Tables(0).Rows.Count
            Dim Dr As Data.DataRow = Ds.Tables(0).Rows(I)
            Dim DateAt As String = Dr("StartDate")
            If I = 1 Then
                'the 2nd row dictates whether the dataset is ordered by date or input
                If DateAt = StartDateAt Then
                    'if the date of the 2nd row is the same as the 1st row, the dataset is ordered by input
                    'otherwise, the dataset is ordered by date
                    Return True
                End If
                Return False
            End If
        Next

        Return False 'placeholder
    End Function

    <Fact>
    Public Sub OrderDsByDateTest()
        Dim Report As Report = CreateCustomReport()
        Report.OrderDSByDate()
        Dim ActualDs As Data.DataSet = Report.GetDS()

        Assert.True(IsDsOrderedByDate(ActualDs))
    End Sub

    <Fact>
    Public Sub OrderDsByDateThenByInputTest()
        Dim Report As Report = CreateCustomReport()
        Dim ExpectedDs As Data.DataSet = Report.GetDS().Copy()

        Report.OrderDSByDate()
        Report.UndoOrderDSByDate()
        Dim ActualDs As Data.DataSet = Report.GetDS()

        Assert.False(IsDsOrderedByDate(ActualDs))
        Assert.True(AreDataSetsEqual(ExpectedDs, ActualDs))
    End Sub
End Class

Public Class GenerateActiveSheetNameTests
    Inherits Report

    <Theory>
    <InlineData("Short Name")>
    <InlineData("ExactlyThirtyOneCharacterssssss")>
    Public Sub DatasetOrderedByInput_CharLimitClearedTests(PmOrChecklistName As String)
        Dim Result As String = GenerateActiveSheetName(PmOrChecklistName)

        Assert.True(Result.Length <= 31)
        Assert.Equal(PmOrChecklistName, Result)
    End Sub

    <Fact>
    Public Sub DatasetOrderedByInput_CharLimitExceededTest()
        Dim PmOrChecklistName As String = "Loooooooooooooong Loooooooooooooooooooong Name"
        Dim Result As String = GenerateActiveSheetName(PmOrChecklistName)

        Assert.True(Result.Length = 31)
        Assert.Equal("Loooooooooooooong Looooooooo...", Result)
    End Sub

    <Theory>
    <InlineData("Short Name")>
    <InlineData("31Charactersssssss")>
    Public Sub DatasetOrderedByDate_CharLimitClearedTests(PmOrChecklistName As String)
        Dim DateNoTime As String = System.DateTime.Now().Date.ToString("MM/dd/yyyy")
        Dim Result As String = GenerateActiveSheetName(PmOrChecklistName, DateNoTime)

        Assert.True(Result.Length <= 31)
        Assert.Equal(PmOrChecklistName & " (" & DateNoTime & ")", Result)
    End Sub

    <Fact>
    Public Sub DatasetOrderedByDate_CharLimitExceededTest()
        Dim PmOrChecklistName As String = "Your mom is so ugly, bigfoot is scared of her"
        Dim DateNoTime As String = System.DateTime.Now().Date.ToString("MM/dd/yyyy")
        Dim Result As String = GenerateActiveSheetName(PmOrChecklistName, DateNoTime)

        Assert.True(Result.Length = 31)
        Assert.Equal("Your mom is so ..." & " (" & DateNoTime & ")", Result)
    End Sub
End Class

Public Class GetLineChartConfigTests
    Inherits Report

    Private _ReportMockDs As New ReportMockDs()

    Public Shared ReadOnly Property AwnStage1Results As IEnumerable(Of Object())
        Get
            Dim ReportMockDs As New ReportMockDs()
            Dim AwnDs As Data.DataSet = ReportMockDs.GetOneLogNoPhasesDataset()
            Dim ExpectedConfig As New Dictionary(Of String, Object) From {
                {"xAxisLabels", {"08/11/2025", "08/12/2025"}},
                {"data", {"8.19", "6.30"}},
                {"lowerBound", "5.5"},
                {"upperBound", "9"},
                {"graphTitle", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH (08/11/2025 - 08/12/2025)"},
                {"xAxisTitle", "Input Date"},
                {"yAxisTitle", "pH"}
            }
            Dim DsLabelKey As Integer = 560

            Return New List(Of Object()) From {
                New Object() {DsLabelKey, AwnDs, ExpectedConfig}
            }
        End Get
    End Property

    Public Shared ReadOnly Property AwnStage2Results As IEnumerable(Of Object())
        Get
            Dim ReportMockDs As New ReportMockDs()
            Dim AwnDs As Data.DataSet = ReportMockDs.GetOneLogNoPhasesDataset()
            Dim ExpectedConfig As New Dictionary(Of String, Object) From {
                {"xAxisLabels", {"08/11/2025", "08/12/2025"}},
                {"data", {"6.99", "7.18"}},
                {"lowerBound", "5.5"},
                {"upperBound", "9"},
                {"graphTitle", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH (08/11/2025 - 08/12/2025)"},
                {"xAxisTitle", "Input Date"},
                {"yAxisTitle", "pH"}
            }
            Dim DsLabelKey As Integer = 561

            Return New List(Of Object()) From {
                New Object() {DsLabelKey, AwnDs, ExpectedConfig}
            }
        End Get
    End Property

    Public Shared ReadOnly Property AwnIncompleteStage1Results As IEnumerable(Of Object())
        Get
            Dim ReportMockDs As New ReportMockDs()
            Dim Ds As Data.DataSet = ReportMockDs.GetOneLogNoPhasesIncompleteDataset()
            Dim ExpectedConfig As New Dictionary(Of String, Object) From {
                {"xAxisLabels", {"08/11/2025", "08/12/2025", "08/13/2025"}},
                {"data", {"8.19", Nothing, "6.30"}},
                {"lowerBound", "5.5"},
                {"upperBound", "9"},
                {"graphTitle", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH (08/11/2025 - 08/13/2025)"},
                {"xAxisTitle", "Input Date"},
                {"yAxisTitle", "pH"}
            }
            Dim DsLabelKey As Integer = 560

            Return New List(Of Object()) From {
                New Object() {DsLabelKey, Ds, ExpectedConfig}
            }
        End Get
    End Property





    Private Shared Function CreateCustomDs(Config As Dictionary(Of String, Object)) As Data.DataSet
        Dim ReportMockDs As New ReportMockDs()
        Dim Ds As Data.DataSet = ReportMockDs.GetOneLogNoPhasesDataset()

        Dim LabelKey = Config("LabelKey")
        For Each Dr As Data.DataRow In Ds.Tables(0).Rows
            If Dr("LabelKey") = LabelKey Then
                Dr("Range") = Config("Range")
                Dr("Unit") = Config("Unit")
                Dr("Label") = Config("Label")
            End If
        Next

        Return Ds
    End Function

    Public Shared ReadOnly Property NoBoundsExpectedResults As IEnumerable(Of Object())
        Get
            Dim DsLabelKey As Integer = 561

            Dim CustomDsConfig As New Dictionary(Of String, Object) From {
                {"LabelKey", DsLabelKey},
                {"Range", DBNull.Value},
                {"Unit", "pH"},
                {"Label", "Stage 2 / AIT AWN-2 Reading"}
            }
            Dim AwnDs As Data.DataSet = CreateCustomDs(CustomDsConfig)

            Dim ExpectedConfig As New Dictionary(Of String, Object) From {
                {"xAxisLabels", {"08/11/2025", "08/12/2025"}},
                {"data", {"6.99", "7.18"}},
                {"lowerBound", Nothing},
                {"upperBound", Nothing},
                {"graphTitle", "Stage 2 / AIT AWN-2 Reading (08/11/2025 - 08/12/2025)"},
                {"xAxisTitle", "Input Date"},
                {"yAxisTitle", "pH"}
            }

            Return New List(Of Object()) From {
                New Object() {DsLabelKey, AwnDs, ExpectedConfig}
            }
        End Get
    End Property

    Public Shared ReadOnly Property UpperBoundOnlyExpectedResults As IEnumerable(Of Object())
        Get
            Dim DsLabelKey As Integer = 561
            Dim Label As String = "Stage 2 / AIT AWN-2 Reading | <9 pH"

            Dim CustomDsConfig As New Dictionary(Of String, Object) From {
                {"LabelKey", DsLabelKey},
                {"Range", "<9"},
                {"Unit", "pH"},
                {"Label", Label}
            }
            Dim AwnDs As Data.DataSet = CreateCustomDs(CustomDsConfig)

            Dim ExpectedConfig As New Dictionary(Of String, Object) From {
                {"xAxisLabels", {"08/11/2025", "08/12/2025"}},
                {"data", {"6.99", "7.18"}},
                {"lowerBound", Nothing},
                {"upperBound", "9"},
                {"graphTitle", Label & " (08/11/2025 - 08/12/2025)"},
                {"xAxisTitle", "Input Date"},
                {"yAxisTitle", "pH"}
            }

            Return New List(Of Object()) From {
                New Object() {DsLabelKey, AwnDs, ExpectedConfig}
            }
        End Get
    End Property

    Public Shared ReadOnly Property LowerBoundOnlyExpectedResults As IEnumerable(Of Object())
        Get
            Dim DsLabelKey As Integer = 561
            Dim Label As String = "Stage 2 / AIT AWN-2 Reading | >5.5 pH"

            Dim CustomDsConfig As New Dictionary(Of String, Object) From {
                {"LabelKey", DsLabelKey},
                {"Range", ">5.5"},
                {"Unit", "pH"},
                {"Label", Label}
            }
            Dim AwnDs As Data.DataSet = CreateCustomDs(CustomDsConfig)

            Dim ExpectedConfig As New Dictionary(Of String, Object) From {
                {"xAxisLabels", {"08/11/2025", "08/12/2025"}},
                {"data", {"6.99", "7.18"}},
                {"lowerBound", "5.5"},
                {"upperBound", Nothing},
                {"graphTitle", Label & " (08/11/2025 - 08/12/2025)"},
                {"xAxisTitle", "Input Date"},
                {"yAxisTitle", "pH"}
            }

            Return New List(Of Object()) From {
                New Object() {DsLabelKey, AwnDs, ExpectedConfig}
            }
        End Get
    End Property

    Public Shared ReadOnly Property AwnNull_yAxisTitlExpectedResults As IEnumerable(Of Object())
        Get
            Dim DsLabelKey As Integer = 561

            Dim CustomDsConfig As New Dictionary(Of String, Object) From {
                {"LabelKey", DsLabelKey},
                {"Range", DBNull.Value},
                {"Unit", DBNull.Value},
                {"Label", ""}
            }
            Dim AwnDs As Data.DataSet = CreateCustomDs(CustomDsConfig)

            Dim ExpectedConfig As New Dictionary(Of String, Object) From {
                {"yAxisTitle", Nothing}
            }

            Return New List(Of Object()) From {
                New Object() {DsLabelKey, AwnDs, ExpectedConfig}
            }
        End Get
    End Property

    <Theory>
    <MemberData(NameOf(AwnStage1Results))>
    <MemberData(NameOf(AwnStage2Results))>
    <MemberData(NameOf(AwnIncompleteStage1Results))>
    Public Sub xAxisLabelsTest(LabelKey As Integer, FakeDs As Data.DataSet, ExpectedConfig As Dictionary(Of String, Object))
        Dim ActualConfig As Dictionary(Of String, Object) = GetLineChartConfig(LabelKey, FakeDs)
        Assert.Equal(Of String())(ExpectedConfig("xAxisLabels"), ActualConfig("xAxisLabels"))
    End Sub

    <Theory>
    <MemberData(NameOf(NoBoundsExpectedResults))>
    <MemberData(NameOf(UpperBoundOnlyExpectedResults))>
    <MemberData(NameOf(LowerBoundOnlyExpectedResults))>
    Public Sub LowerBoundTest(LabelKey As Integer, FakeDs As Data.DataSet, ExpectedConfig As Dictionary(Of String, Object))
        Dim ActualConfig As Dictionary(Of String, Object) = GetLineChartConfig(LabelKey, FakeDs)
        Assert.Equal(ExpectedConfig("lowerBound"), ActualConfig("lowerBound"))
    End Sub

    <Theory>
    <MemberData(NameOf(NoBoundsExpectedResults))>
    <MemberData(NameOf(UpperBoundOnlyExpectedResults))>
    <MemberData(NameOf(LowerBoundOnlyExpectedResults))>
    Public Sub UpperBoundTest(LabelKey As Integer, FakeDs As Data.DataSet, ExpectedConfig As Dictionary(Of String, Object))
        Dim ActualConfig As Dictionary(Of String, Object) = GetLineChartConfig(LabelKey, FakeDs)
        Assert.Equal(ExpectedConfig("upperBound"), ActualConfig("upperBound"))
    End Sub

    <Theory>
    <MemberData(NameOf(AwnStage1Results))>
    <MemberData(NameOf(AwnStage2Results))>
    <MemberData(NameOf(AwnIncompleteStage1Results))>
    Public Sub graphTitleTest(LabelKey As Integer, FakeDs As Data.DataSet, ExpectedConfig As Dictionary(Of String, Object))
        Dim ActualConfig As Dictionary(Of String, Object) = GetLineChartConfig(LabelKey, FakeDs)
        Assert.Equal(ExpectedConfig("graphTitle"), ActualConfig("graphTitle"))
    End Sub

    <Theory>
    <MemberData(NameOf(AwnStage1Results))>
    <MemberData(NameOf(AwnStage2Results))>
    <MemberData(NameOf(AwnIncompleteStage1Results))>
    Public Sub xAxisTitleTest(LabelKey As Integer, FakeDs As Data.DataSet, ExpectedConfig As Dictionary(Of String, Object))
        Dim ActualConfig As Dictionary(Of String, Object) = GetLineChartConfig(LabelKey, FakeDs)
        Assert.Equal(ExpectedConfig("xAxisTitle"), ActualConfig("xAxisTitle"))
    End Sub

    <Theory>
    <MemberData(NameOf(AwnStage1Results))>
    <MemberData(NameOf(AwnStage2Results))>
    <MemberData(NameOf(AwnIncompleteStage1Results))>
    <MemberData(NameOf(AwnNull_yAxisTitlExpectedResults))>
    Public Sub yAxisTitleTest(LabelKey As Integer, FakeDs As Data.DataSet, ExpectedConfig As Dictionary(Of String, Object))
        Dim ActualConfig As Dictionary(Of String, Object) = GetLineChartConfig(LabelKey, FakeDs)
        Assert.Equal(ExpectedConfig("yAxisTitle"), ActualConfig("yAxisTitle"))
    End Sub

    <Theory>
    <MemberData(NameOf(AwnStage1Results))>
    <MemberData(NameOf(AwnStage2Results))>
    <MemberData(NameOf(AwnIncompleteStage1Results))>
    Public Sub DataTest(LabelKey As Integer, FakeDs As Data.DataSet, ExpectedConfig As Dictionary(Of String, Object))
        Dim ActualConfig As Dictionary(Of String, Object) = GetLineChartConfig(LabelKey, FakeDs)
        Assert.Equal(Of String())(ExpectedConfig("data"), ActualConfig("data"))
    End Sub
End Class


Public Class ReportMockDs
    Public Function CreateFakeDsSchema() As Data.DataSet
        Dim DS As New Data.DataSet
        Dim DT As New Data.DataTable

        DT.Columns.Add("Area", GetType(String))
        DT.Columns.Add("FieldType", GetType(String))
        DT.Columns.Add("LabelKey", GetType(Integer))
        DT.Columns.Add("Label", GetType(String))
        DT.Columns.Add("Range", GetType(String))
        DT.Columns.Add("Unit", GetType(String))
        DT.Columns.Add("Phase", GetType(String))
        DT.Columns.Add("Value", GetType(String))
        DT.Columns.Add("StartDate", GetType(String))
        DT.Columns.Add("InputDate", GetType(String))
        DT.Columns.Add("InputOperator", GetType(String))

        DS.Tables.Add(DT)

        Return DS
    End Function

    Public Sub CreateFakeDr(DT As Data.DataTable, Data As Dictionary(Of String, Object))
        Dim DR As Data.DataRow = DT.NewRow()

        For Each kvp As KeyValuePair(Of String, Object) In Data
            Dim Field As String = kvp.Key
            Dim FieldValue As Object = kvp.Value

            DR(Field) = FieldValue
        Next

        DT.Rows.Add(DR)
    End Sub

    Public Function GetOneLogNoPhasesDataset() As Data.DataSet
        Dim DS As Data.DataSet = CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 560},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", DBNull.Value},
            {"Value", "8.19"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:22 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 560},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", DBNull.Value},
            {"Value", "6.30"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:11 AM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 561},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", DBNull.Value},
            {"Value", "6.99"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 561},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", DBNull.Value},
            {"Value", "7.18"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:19 AM"},
            {"InputOperator", "andrew williams"}
        })

        Return DS
    End Function

    Public Function GetOneLogNoPhasesIncompleteDataset() As Data.DataSet
        Dim DS As Data.DataSet = CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 560},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", DBNull.Value},
            {"Value", "8.19"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:22 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 560},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", DBNull.Value},
            {"Value", ""},
            {"StartDate", "08/12/2025"},
            {"InputDate", ""},
            {"InputOperator", ""}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 560},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", DBNull.Value},
            {"Value", "6.30"},
            {"StartDate", "08/13/2025"},
            {"InputDate", "08/13/2025 06:33:11 AM"},
            {"InputOperator", "andrew williams"}
        })



        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 561},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", DBNull.Value},
            {"Value", "6.99"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 561},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", DBNull.Value},
            {"Value", ""},
            {"StartDate", "08/12/2025"},
            {"InputDate", ""},
            {"InputOperator", ""}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 561},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", DBNull.Value},
            {"Value", "7.18"},
            {"StartDate", "08/13/2025"},
            {"InputDate", "08/13/2025 06:33:19 AM"},
            {"InputOperator", "andrew williams"}
        })

        Return DS
    End Function

    Public Function GetOneLogAllInputsPhasedDataset() As Data.DataSet
        Dim DS As Data.DataSet = CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "All Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 560},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", "phase 1"},
            {"Value", "8.19"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:22 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "All Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 560},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", "phase 1"},
            {"Value", "6.30"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:11 AM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "All Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 561},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", "phase 2"},
            {"Value", "6.99"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "All Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 561},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", "phase 2"},
            {"Value", "7.18"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:19 AM"},
            {"InputOperator", "andrew williams"}
        })


        Return DS
    End Function


    Public Function GetOneLogSomeInputsPhasedDataset() As Data.DataSet
        Dim DS As Data.DataSet = CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Some Inputs Phased Loooooooooooong Name"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 560},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", DBNull.Value},
            {"Value", "8.19"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:22 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Some Inputs Phased Loooooooooooong Name"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 560},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", DBNull.Value},
            {"Value", "6.30"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:11 AM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Some Inputs Phased Loooooooooooong Name"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 561},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", "phase 2"},
            {"Value", "6.99"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Some Inputs Phased Loooooooooooong Name"},
            {"FieldType", DBNull.Value},
            {"LabelKey", 561},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Range", "5.5-9"},
            {"Unit", "pH"},
            {"Phase", "phase 2"},
            {"Value", "7.18"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:19 AM"},
            {"InputOperator", "andrew williams"}
        })


        Return DS
    End Function
End Class

Public Class GetExcelDataTests
    Inherits Report

    Private _ReportMockDs As New ReportMockDs()

    Private Function StringifyMatrixHash(MatrixHash As Dictionary(Of String, List(Of String())))
        Return JsonSerializer.Serialize(
            MatrixHash.ToDictionary(Function(kv) kv.Key, Function(kv) kv.Value),
            New JsonSerializerOptions With {.WriteIndented = True}
        )
    End Function

    <Fact>
    Public Sub OneLogNoPhasesDatasetOrderedByInputTest()
        '1 log with no phases or groups (ordered by input)
        Dim DS As Data.DataSet = _ReportMockDs.GetOneLogNoPhasesDataset()
        Dim ReportInst As New Report()
        ReportInst.RebindGroupDS(DS)

        Dim ExpectedRes As New Dictionary(Of String, List(Of String()))
        ExpectedRes("No Inputs Phased") = New List(Of String()) From {
            New String() {"No Inputs Phased", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "8.19", "08/11/2025", "08/11/2025 02:44:22 PM", "andrew williams", "default"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "6.30", "08/12/2025", "08/12/2025 06:33:11 AM", "andrew williams", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02:44:29 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06:33:19 AM", "andrew williams", "default"}
        }

        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub







    <Fact>
    Public Sub OneLogAllInputsPhasedOrderedByInputTest()
        '1 log with all inputs in phases or groups  (ordered by input)
        Dim DS As Data.DataSet = _ReportMockDs.GetOneLogAllInputsPhasedDataset()
        Dim ReportInst As New Report()
        ReportInst.RebindGroupDS(DS)

        Dim ExpectedRes As New Dictionary(Of String, List(Of String()))
        ExpectedRes("All Inputs Phased") = New List(Of String()) From {
            New String() {"All Inputs Phased", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"", "", "", "", "", "default"},
            New String() {"phase 1", "", "", "", "", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "8.19", "08/11/2025", "08/11/2025 02:44:22 PM", "andrew williams", "default"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "6.30", "08/12/2025", "08/12/2025 06:33:11 AM", "andrew williams", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"phase 2", "", "", "", "", "bold"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02:44:29 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06:33:19 AM", "andrew williams", "default"}
        }

        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub








    <Fact>
    Public Sub OneLogSomeInputsPhasedOrderedByInputTest()
        '1 log with some inputs in phases or groups (ordered by input)
        Dim DS As Data.DataSet = _ReportMockDs.GetOneLogSomeInputsPhasedDataset()
        Dim ReportInst As New Report()
        ReportInst.RebindGroupDS(DS)

        Dim ExpectedRes As New Dictionary(Of String, List(Of String()))
        ExpectedRes("Some Inputs Phased Loooooooo...") = New List(Of String()) From {
            New String() {"Some Inputs Phased Loooooooooooong Name", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "8.19", "08/11/2025", "08/11/2025 02:44:22 PM", "andrew williams", "default"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "6.30", "08/12/2025", "08/12/2025 06:33:11 AM", "andrew williams", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"phase 2", "", "", "", "", "bold"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02:44:29 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06:33:19 AM", "andrew williams", "default"}
        }

        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub





    <Fact>
    Public Sub SeveralLogsOrderedByInput()
        'several logs with various types (some/none/all inputs in phases or groups). All logs are ordered by input
        Dim Datasets As New List(Of DataSet) From {_ReportMockDs.GetOneLogNoPhasesDataset(), _ReportMockDs.GetOneLogSomeInputsPhasedDataset(), _ReportMockDs.GetOneLogAllInputsPhasedDataset()}
        Dim MasterDS As New Data.DataSet
        For Each DS As DataSet In Datasets
            MasterDS.Merge(DS, preserveChanges:=True, missingSchemaAction:=MissingSchemaAction.Add)
        Next
        Dim DT As Data.DataTable = MasterDS.Tables(0)

        Dim ReportInst As New Report()
        ReportInst.RebindGroupDS(MasterDS)

        Dim ExpectedRes As New Dictionary(Of String, List(Of String()))
        ExpectedRes("No Inputs Phased") = New List(Of String()) From {
            New String() {"No Inputs Phased", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "8.19", "08/11/2025", "08/11/2025 02:44:22 PM", "andrew williams", "default"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "6.30", "08/12/2025", "08/12/2025 06:33:11 AM", "andrew williams", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02:44:29 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06:33:19 AM", "andrew williams", "default"}
        }
        ExpectedRes("Some Inputs Phased Loooooooo...") = New List(Of String()) From {
            New String() {"Some Inputs Phased Loooooooooooong Name", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "8.19", "08/11/2025", "08/11/2025 02:44:22 PM", "andrew williams", "default"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "6.30", "08/12/2025", "08/12/2025 06:33:11 AM", "andrew williams", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"phase 2", "", "", "", "", "bold"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02:44:29 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06:33:19 AM", "andrew williams", "default"}
        }
        ExpectedRes("All Inputs Phased") = New List(Of String()) From {
            New String() {"All Inputs Phased", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"", "", "", "", "", "default"},
            New String() {"phase 1", "", "", "", "", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "8.19", "08/11/2025", "08/11/2025 02:44:22 PM", "andrew williams", "default"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "6.30", "08/12/2025", "08/12/2025 06:33:11 AM", "andrew williams", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"phase 2", "", "", "", "", "bold"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02:44:29 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06:33:19 AM", "andrew williams", "default"}
        }

        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub

    <Fact>
    Public Sub CheckboxFieldtypeDatasetOrderedByInput()
        '1 log with no phases or groups (ordered by input) 
        Dim DS As Data.DataSet = _ReportMockDs.CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", "Checkbox"},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", ""},
            {"StartDate", "08/11/2025"},
            {"InputDate", DBNull.Value},
            {"InputOperator", ""}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", "Checkbox"},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "0"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:11 AM"},
            {"InputOperator", "andrew williams"}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", "Checkbox"},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "1"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "7.18"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:19 AM"},
            {"InputOperator", "andrew williams"}
        })

        Dim ReportInst As New Report()
        ReportInst.RebindGroupDS(DS)

        Dim ExpectedRes As New Dictionary(Of String, List(Of String()))
        ExpectedRes("No Inputs Phased") = New List(Of String()) From {
            New String() {"No Inputs Phased", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "✘", "08/11/2025", "", "", "default"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "✘", "08/12/2025", "08/12/2025 06:33:11 AM", "andrew williams", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "✔", "08/11/2025", "08/11/2025 02:44:29 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06:33:19 AM", "andrew williams", "default"}
        }

        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub



    '================= dataset ordered by date unit tests ====================
    <Fact>
    Public Sub CheckboxFieldtypeDatasetOrderedByDate()
        '1 log with no phases or groups (ordered by date) 
        Dim DS As Data.DataSet = _ReportMockDs.CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", "DP"},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "0/0"},
            {"StartDate", "08/11/2025"},
            {"InputDate", DBNull.Value},
            {"InputOperator", ""}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", "DP"},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "0/1"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:11 AM"},
            {"InputOperator", "andrew williams"}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", "DP"},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "1/0"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", "DP"},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "1/1"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:19 AM"},
            {"InputOperator", "andrew williams"}
        })

        Dim ReportInst As New Report()
        ReportInst.RebindGroupDS(DS)
        ReportInst.RebindIsOrderedByDate(True)

        Dim ExpectedRes As New Dictionary(Of String, List(Of String()))
        ExpectedRes("Pm Or Checklist... (08/11/2025)") = New List(Of String()) From {
            New String() {"Pm Or Checklist Name", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "✘/✘", "08/11/2025", "", "", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "✔/✘", "08/11/2025", "08/11/2025 02:44:29 PM", "andrew williams", "default"}
        }
        ExpectedRes("Pm Or Checklist... (08/12/2025)") = New List(Of String()) From {
            New String() {"Pm Or Checklist Name", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "✘/✔", "08/12/2025", "08/12/2025 06:33:11 AM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "✔/✔", "08/12/2025", "08/12/2025 06:33:19 AM", "andrew williams", "default"}
        }

        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub

    <Fact>
    Public Sub OneLogNoPhasesDatasetOrderedByDateTest()
        '1 log with no phases or groups (ordered by date) 
        Dim DS As Data.DataSet = _ReportMockDs.CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", "DP"},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", ""},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:22 PM"},
            {"InputOperator", "andrew williams"}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "6.30"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:11 AM"},
            {"InputOperator", "andrew williams"}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "6.99"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "7.18"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:19 AM"},
            {"InputOperator", "andrew williams"}
        })

        Dim ReportInst As New Report()
        ReportInst.RebindGroupDS(DS)
        ReportInst.RebindIsOrderedByDate(True)

        Dim ExpectedRes As New Dictionary(Of String, List(Of String()))
        ExpectedRes("Pm Or Checklist... (08/11/2025)") = New List(Of String()) From {
            New String() {"Pm Or Checklist Name", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "✘/✘", "08/11/2025", "08/11/2025 02:44:22 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02:44:29 PM", "andrew williams", "default"}
        }
        ExpectedRes("Pm Or Checklist... (08/12/2025)") = New List(Of String()) From {
            New String() {"Pm Or Checklist Name", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "6.30", "08/12/2025", "08/12/2025 06:33:11 AM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06:33:19 AM", "andrew williams", "default"}
        }

        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub

    <Fact>
    Public Sub OneLogAllInputsPhasedOrderedByDate()
        '1 log with all inputs in phases or groups  (ordered by date)
        Dim DS As Data.DataSet = _ReportMockDs.CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Phase", "phase 1"},
            {"Value", "8.19"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:22 PM"},
            {"InputOperator", "andrew williams"}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Phase", "phase 1"},
            {"Value", "6.30"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:11 AM"},
            {"InputOperator", "andrew williams"}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", "phase 2"},
            {"Value", "6.99"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", "phase 2"},
            {"Value", "7.18"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:19 AM"},
            {"InputOperator", "andrew williams"}
        })

        Dim ReportInst As New Report()
        ReportInst.RebindGroupDS(DS)
        ReportInst.RebindIsOrderedByDate(True)

        Dim ExpectedRes As New Dictionary(Of String, List(Of String()))
        ExpectedRes("Pm Or Checklist... (08/11/2025)") = New List(Of String()) From {
            New String() {"Pm Or Checklist Name", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"", "", "", "", "", "default"},
            New String() {"phase 1", "", "", "", "", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "8.19", "08/11/2025", "08/11/2025 02:44:22 PM", "andrew williams", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"phase 2", "", "", "", "", "bold"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02:44:29 PM", "andrew williams", "default"}
        }
        ExpectedRes("Pm Or Checklist... (08/12/2025)") = New List(Of String()) From {
            New String() {"Pm Or Checklist Name", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"", "", "", "", "", "default"},
            New String() {"phase 1", "", "", "", "", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "6.30", "08/12/2025", "08/12/2025 06:33:11 AM", "andrew williams", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"phase 2", "", "", "", "", "bold"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06:33:19 AM", "andrew williams", "default"}
        }


        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub

    <Fact>
    Public Sub OneLogSomeInputsPhasedOrderedByDate()
        '1 log with some inputs in phases or groups (ordered by date)
        Dim DS As Data.DataSet = _ReportMockDs.CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "8.19"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:22 PM"},
            {"InputOperator", "andrew williams"}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "6.30"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:11 AM"},
            {"InputOperator", "andrew williams"}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", "phase 2"},
            {"Value", "6.99"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02:44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        _ReportMockDs.CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", "phase 2"},
            {"Value", "7.18"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06:33:19 AM"},
            {"InputOperator", "andrew williams"}
        })

        Dim ReportInst As New Report()
        ReportInst.RebindGroupDS(DS)
        ReportInst.RebindIsOrderedByDate(True)

        Dim ExpectedRes As New Dictionary(Of String, List(Of String()))
        ExpectedRes("Pm Or Checklist... (08/11/2025)") = New List(Of String()) From {
            New String() {"Pm Or Checklist Name", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "8.19", "08/11/2025", "08/11/2025 02:44:22 PM", "andrew williams", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"phase 2", "", "", "", "", "bold"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02:44:29 PM", "andrew williams", "default"}
        }
        ExpectedRes("Pm Or Checklist... (08/12/2025)") = New List(Of String()) From {
            New String() {"Pm Or Checklist Name", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "6.30", "08/12/2025", "08/12/2025 06:33:11 AM", "andrew williams", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"phase 2", "", "", "", "", "bold"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06:33:19 AM", "andrew williams", "default"}
        }

        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub
End Class

Public Class GetFieldTypeValueTests
    Inherits Report

    <Fact>
    Public Sub NumberFieldTypeTests()
        Dim FieldType As Object = DBNull.Value
        Dim Value As Integer = 34
        Assert.Equal(Value, GetFieldTypeValue(Value, FieldType))
    End Sub

    <Theory>
    <InlineData("Checkbox", "1", "✔")>
    <InlineData("Checkbox", "0", "✘")>
    <InlineData("Checkbox", "", "✘")>
    Public Sub CheckboxFieldTypeTests(FieldType As String, Value As Object, DesiredRes As String)
        Assert.Equal(DesiredRes, GetFieldTypeValue(Value, FieldType))
    End Sub

    <Theory>
    <InlineData("DP", "0/0", "✘/✘")>
    <InlineData("DP", "", "✘/✘")>
    <InlineData("DP", "0/1", "✘/✔")>
    <InlineData("DP", "1/0", "✔/✘")>
    <InlineData("DP", "1/1", "✔/✔")>
    Public Sub DpFieldTypeTests(FieldType As String, Value As Object, DesiredRes As String)
        Assert.Equal(DesiredRes, GetFieldTypeValue(Value, FieldType))
    End Sub
End Class