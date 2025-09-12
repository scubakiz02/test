Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library
Imports System
Imports System.Globalization
Imports System.Text.Json

Public Class ReportTests
    Dim GroupKey As Integer = 1
    Public ReportConfig As New Dictionary(Of String, String) From {
        {"GroupKey", GroupKey},
        {"AreaKey", 58}
    }
    Public FDCG As New Report(ReportConfig) 'FDCG = Facilities Daily Checklist Group
    Dim Security As New Security
    Private QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
    Private ReadOnly InvalidDateMessage As String = "Error: Invalid date"
    Private ReadOnly OutOfRangeDateMessage As String = "Error: Out of dataset range"

    Public Sub New()
        Dim StartDate As Date = "03/16/2025"
        Dim EndDate As Date = "03/18/2025"

        FDCG.SetDateRange(StartDate, EndDate)

        QueryConfig("@GroupKey") = New Dictionary(Of String, String) From {
            {"value", GroupKey},
            {"typeOf", "int"}
        }
        QueryConfig("@AreaKey") = New Dictionary(Of String, String) From {
            {"value", 0}, '0 = all checklists
            {"typeOf", "int"}
        }
        QueryConfig("@StartDate") = New Dictionary(Of String, String) From {
            {"value", StartDate},
            {"typeOf", "date"}
        }
        QueryConfig("@EndDate") = New Dictionary(Of String, String) From {
            {"value", EndDate},
            {"typeOf", "date"}
        }
    End Sub


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

    Public Function AreDataTablesEqual(table1 As DataTable, table2 As DataTable) As Boolean
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

    '<Fact>
    'Public Sub ConstructorTest1()
    '    'execute sql query with the same GroupKey as FDCG instantiation of Report class. Compare the 2 DataSets, expect false
    '    Dim DS As Data.DataSet

    '    DS = Security.GetMyDataSetParamQuery(FDCG.ConstructorQuery, QueryConfig)

    '    Assert.False(AreDataSetsEqual(DS, FDCG.GetDS()))
    'End Sub

    '<Fact>
    'Public Sub ConstructorTest2()
    '    'instantiate Report class with GroupKey of 2, and compare it to DS of GroupKey 1, which should NOT be same, returning false
    '    Dim DS As Data.DataSet

    '    DS = Security.GetMyDataSetParamQuery(FDCG.ConstructorQuery, QueryConfig)

    '    Assert.False(AreDataSetsEqual(DS, New Report(New Dictionary(Of String, String) From {
    '        {"GroupKey", 2},
    '        {"AreaKey", 0}
    '    }).GetDS()))
    'End Sub


    <Fact>
    Public Sub ConstructorTest1()
        'ensure that if a date range does NOT exists, and after calling FDCG.OrderByDate(), the DS has no records
        FDCG.SetDateRange(Nothing, Nothing)
        Assert.Equal(Of Integer)(0, FDCG.OrderDSByDate().Tables(0).Rows.Count)
    End Sub


    <Fact>
    Public Sub ConstructorTest6()
        'instantiate Report class mocking FDCG, meaning its GetDS() should be the same
        Dim Report As New Report(ReportConfig)
        Report.SetDateRange("03/16/2025", "03/18/2025")

        Assert.True(AreDataSetsEqual(FDCG.GetDS(), Report.GetDS()))
    End Sub

    <Fact>
    Public Sub ConstructorTest4()
        'test ConfigureDS function against a live dataset
        Dim DS As Data.DataSet = FDCG.GetDS()
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

        Assert.True(AccurateData)
    End Sub

    <Fact>
    Public Sub ConstructorTest5()
        'test ConfigureDS function against a live dataset, with a specific AreaKey 
        Dim AreaKey As Integer = 60 'SC-3 Fume Scrubber Monitoring Daily
        Dim Report As New Report(New Dictionary(Of String, String) From {
            {"GroupKey", GroupKey},
            {"AreaKey", AreaKey}
        })
        Dim DS As Data.DataSet
        Dim DR As Data.DataRow
        Dim RC As Integer
        Dim AccurateData As Boolean = True

        DS = Report.GetDS()
        RC = DS.Tables(0).Rows.Count - 1

        QueryConfig("@AreaKey")("value") = AreaKey 'constructor sets this to 0

        For I As Integer = 0 To RC
            DR = DS.Tables(0).Rows(I)

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

        Assert.True(AccurateData)
    End Sub

    <Fact>
    Public Sub ConstructorTest7()
        'date range is passed to the constructor config object. ensure GetDS() from Report class is within the set date range
        Dim StartDate As Date = "03-22-2025"
        Dim EndDate As Date = "03-25-2025"
        Dim ReportConfig As New Dictionary(Of String, String) From {
            {"GroupKey", GroupKey},
            {"AreaKey", 0},
            {"StartDate", StartDate},
            {"EndDate", EndDate}
        }
        Dim DS As Data.DataSet = New Report(ReportConfig).GetDS()
        Dim RC = DS.Tables(0).Rows.Count - 1
        Dim OutOfRange As Boolean = False

        For I As Integer = 0 To RC
            Dim DR As Data.DataRow = DS.Tables(0).Rows(I)
            Dim InputDate As Date = DR("Date") 'Date field is the date the checklist log was created

            If InputDate.Date < StartDate.Date OrElse InputDate.Date > EndDate.Date Then
                OutOfRange = True
                Exit For
            End If

        Next

        Assert.False(OutOfRange)
    End Sub

    <Fact>
    Public Sub ConstructorTest8()
        'no records in dataset without supplying a date range first
        Dim ReportConfig As New Dictionary(Of String, String) From {
            {"GroupKey", GroupKey},
            {"AreaKey", 0},
            {"StartDate", Nothing},
            {"EndDate", Nothing}
        }
        Dim Report As New Report(ReportConfig)
        Dim DS As Data.DataSet
        Dim DR As Data.DataRow
        Dim AccurateData As Boolean = True

        DS = Report.GetDS()

        Assert.True(If(DS.Tables(0).Rows.Count = 0, True, False))
    End Sub

    <Fact>
    Public Sub SetDateRangeTest1()
        'date range is NOT passed to the constructor, but SetDateRange public function is called after the constructor runs. ensure GetDS() from Report class is within the set date range
        Dim StartDate As Date = "03-22-2025"
        Dim EndDate As Date = "03-25-2025"
        Dim DS As Data.DataSet = FDCG.SetDateRange(StartDate, EndDate)
        Dim RC = DS.Tables(0).Rows.Count - 1
        Dim OutOfRange As Boolean = False

        For I As Integer = 0 To RC
            Dim DR As Data.DataRow = DS.Tables(0).Rows(I)
            Dim InputDate As Date = DR("Date") 'Date field is the date the checklist log was created

            If InputDate.Date < StartDate.Date OrElse InputDate.Date > EndDate.Date Then
                OutOfRange = True
                Exit For
            End If

        Next

        Assert.False(OutOfRange)
    End Sub

    <Fact>
    Public Sub SetDateRangeTest2()
        'if start date is greater than end date, there should be no rows returned
        Dim StartDate As Date = "03-25-2025"
        Dim EndDate As Date = "03-22-2025"
        Dim DS As Data.DataSet = FDCG.SetDateRange(StartDate, EndDate)

        Assert.False(If(DS.Tables(0).Rows.Count > 0, True, False))
    End Sub

    <Fact>
    Public Sub SetDateRangeTest3()
        'start date and end date are the same date. There should be rows returned
        Dim DS As Data.DataSet = FDCG.SetDateRange("03-22-2025", "03-22-2025")

        Assert.True(If(DS.Tables(0).Rows.Count > 0, True, False))
    End Sub

    <Fact>
    Public Sub SetAreaTest1()
        'calling SetArea function. ensure GetDS() from Report class does not hold data for any other checklist
        Dim AreaKey As Integer = 55 'AWN Daily
        Dim DS As Data.DataSet = FDCG.SetArea(AreaKey)
        Dim RC = DS.Tables(0).Rows.Count - 1
        Dim OutOfSpec As Boolean = False

        For I As Integer = 0 To RC
            Dim DR As Data.DataRow = DS.Tables(0).Rows(I)

            If DR("AreaKey") <> AreaKey Then
                OutOfSpec = True
                Exit For
            End If

        Next

        Assert.False(OutOfSpec)
    End Sub

    <Fact>
    Public Sub SetAreaTest2()
        'calling SetArea function with AreaKey of 0, which means ALL checklists
        Dim DS As Data.DataSet = FDCG.SetArea(0)
        'Dim RC = DS.Tables(0).Rows.Count - 1

        Assert.True(DS.Tables(0).Rows.Count > 0)
    End Sub

    <Fact>
    Public Sub SetAreaTest3()
        'calling SetArea function with existing AreaKey
        Assert.True(FDCG.SetArea(58).Tables(0).Rows.Count > 0)
    End Sub

    <Fact>
    Public Sub SetGroupTest1()
        'calling SetGroup function. ensure GetDS() from Report class does not hold data for any other group
        Dim GroupKey As Integer = 2
        Dim DS As Data.DataSet = FDCG.SetGroup(GroupKey)
        Dim RC = DS.Tables(0).Rows.Count - 1
        Dim OutOfSpec As Boolean = False

        For I As Integer = 0 To RC
            Dim DR As Data.DataRow = DS.Tables(0).Rows(I)

            If DR("GroupKey") <> GroupKey Then
                OutOfSpec = True
                Exit For
            End If

        Next

        Assert.False(OutOfSpec)
    End Sub

    <Fact>
    Public Sub SetGroupTest2()
        'calling SetGroup function with GroupKey of 0, which means ALL checklists
        FDCG.SetDateRange("03/23/2025", "03/24/2025") 'so lights don't flicker from lots of DB being pulled
        Assert.True(FDCG.SetGroup(1).Tables(0).Rows.Count > 0)
    End Sub
    <Fact>
    Public Sub GetMaxFieldValsTest1()
        'calling GetMaxFieldVals function. test return field values against entire dataset for GroupKey 1 AreaKey 57
        FDCG.SetArea(48) 'Nitrogen Daily
        Dim MaxFieldVals As Dictionary(Of String, String) = FDCG.GetMaxFieldVals() 'idx slots for MaxFieldVals: Area, Label, Value, Date, InputOperator
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
        FDCG.SetArea(56) 'R.O. Daily
        Dim MaxFieldVals As Dictionary(Of String, String) = FDCG.GetMaxFieldVals() 'idx slots for MaxFieldVals: Area, Label, Value, Date, InputOperator
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

    'InvalidDateMessage
    'OutOfRangeDateMessage

    <Fact>
    Public Sub DateInRangeTest1()
        'if date is null, the return should equal InvalidDateMessage
        Assert.Equal(FDCG.InvalidDateMessage, FDCG.DateInRange(Nothing))
    End Sub

    <Fact>
    Public Sub DateInRangeTest2()
        'if date is not a valid date (MM/DD/YYYY), the return should equal InvalidDateMessage
        Assert.Equal(FDCG.InvalidDateMessage, FDCG.DateInRange("03/23/20"))
    End Sub

    <Fact>
    Public Sub DateInRangeTest3()
        'if date is valid, compare against date range. The input will match the lower bound, so expect empty string as return
        FDCG.SetDateRange("03/16/2025", "03/23/2025")
        Assert.Equal(String.Empty, FDCG.DateInRange("03/16/2025"))
    End Sub

    <Fact>
    Public Sub DateInRangeTest4()
        'if date is valid, compare against date range. The input will be in range, so expect empty string as return
        FDCG.SetDateRange("03/16/2025", "03/23/2025")
        Assert.Equal(String.Empty, FDCG.DateInRange("03/22/2025"))
    End Sub

    <Fact>
    Public Sub DateInRangeTest5()
        'if date is valid, compare against date range. The input will be out of range, so expect OutOfRangeDateMessage
        FDCG.SetDateRange("03/16/2025", "03/23/2025")
        Assert.Contains(FDCG.OutOfRangeDateMessage, FDCG.DateInRange("03/14/2025"))
    End Sub

    <Fact>
    Public Sub DateInRangeTest6()
        'no date range exists. valid date, so expect empty string
        FDCG.SetDateRange(Nothing, Nothing)
        Assert.Equal(String.Empty, FDCG.DateInRange("03/22/2025"))
    End Sub

    <Fact>
    Public Sub DateInRangeTest7()
        'no date range exists. invalid date (MM/DD/YYYY), so return should equal InvalidDateMessage
        FDCG.SetDateRange(Nothing, Nothing)
        Assert.Equal(FDCG.InvalidDateMessage, FDCG.DateInRange("03/22/5"))
    End Sub
End Class

Public Class SetAreasTests
    Inherits Security

    Private Report As New Report(New Dictionary(Of String, String) From {
        {"GroupKey", 1},
        {"AreaKey", 0}
    })
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
    Private DS As Data.DataSet
    Private OgDS As Data.DataSet
    Private Format As New Format()

    Public Sub New()
        Report.SetGroup(1)
        Report.SetDateRange("04/01/2025", "04/01/2025")

        OgDS = Report.GetDS()
    End Sub

    Private Function RemoveAreasFromDS(AreasToExclude As Integer()) As Data.DataSet 'programmatically delete rows that are tied to values in 'AreasToExclude' array
        Dim ReportDS As Data.DataSet = OgDS.Copy()

        For Each DR As Data.DataRow In ReportDS.Tables(0).Rows
            If AreasToExclude.Contains(DR("AreaKey")) Then
                DR.Delete() 'mark row for deletion
            End If
        Next
        ReportDS.Tables(0).AcceptChanges() 'remove DataRow(s) marked for deletion permanently

        Return ReportDS
    End Function
    Private Function IncludeAreasFromDS(AreasToInclude As Integer()) As Data.DataSet 'programmatically delete rows that are tied to values in 'AreasToExclude' array
        Dim ReportDS As Data.DataSet = OgDS.Copy()

        For Each DR As Data.DataRow In ReportDS.Tables(0).Rows
            If AreasToInclude.Contains(DR("AreaKey")) = False Then
                DR.Delete() 'mark row for deletion
            End If
        Next
        ReportDS.Tables(0).AcceptChanges() 'remove DataRow(s) marked for deletion permanently

        Return ReportDS
    End Function

    Private Function NumOfRows() As Integer
        Return Report.GetDS().Tables(0).Rows.Count
    End Function

    <Fact>
    Public Sub BaselineTest()
        'check if return dataset from class instantiation is as expected
        Assert.Equal(OgDS.Tables(0).Rows.Count, NumOfRows())
    End Sub

    <Fact>
    Public Sub SetAreasNothingArg()
        'if SetAreas receivs null as arg 1, return empty dataset
        Report.SetAreas(Nothing)
        Assert.Equal(0, NumOfRows())
    End Sub

    <Fact>
    Public Sub SetAreasEmptyListArg()
        'if SetAreas receivs empty list as arg 1, return empty dataset
        Report.SetAreas(New List(Of Integer))
        Assert.Equal(0, NumOfRows()) 'ensure number of rows is not confined to a single area
    End Sub

    <Fact>
    Public Sub SetAreasAllAreas()
        'pass list of all Areas for GroupKey 1, and check to see if instantiation holds the same dataset before function SetAreas in called
        Dim RecordSet As New RecordSet(Report.SetGroup(1))  'calling SetGroup function to reset GroupDS in Class instantiation

        SetAreasEmptyListArg() 'calling this will set GroupDS to be blank within Report Class instantiation

        Assert.True(RecordSet.DataSetsMatch(Report.SetAreas(GroupKey1AreasHash.Keys.ToList()))) 'ensure number of rows is not confined to a single area
    End Sub

    <Fact>
    Public Sub SetAreasExclude1Area()
        'pass list that excludes 1 Area from GroupKey1 and check to see if instantiation holds different dataset before function SetAreas in called
        Dim RecordSet As RecordSet
        Dim GroupKey1AreasHashNew As New Dictionary(Of Integer, String)(GroupKey1AreasHash)

        Report.SetGroup(1) 'sanity check

        RecordSet = New RecordSet(RemoveAreasFromDS({48}))
        GroupKey1AreasHashNew.Remove(48)

        Assert.True(RecordSet.DataSetsMatch(Report.SetAreas(GroupKey1AreasHashNew.Keys.ToList())))
    End Sub

    <Fact>
    Public Sub SetAreasExcludeThenIncludeAll()
        'excluding certain areas out from DS, then bringing them backing in does NOT work
        'refactor Report Class, so this unit test passes, which in turn mean the problem above no longer occurs
        SetAreasExclude1Area()
        SetAreasAllAreas()
    End Sub

    <Fact>
    Public Sub GetAreasTest()
        Assert.Equal(Of Dictionary(Of Integer, String))(GroupKey1AreasHash, Report.GetAreas())
    End Sub

    <Fact>
    Public Sub GetReportedAreasBaseLine()
        Report.SetGroup(1)
        Assert.Equal(Of Dictionary(Of Integer, String))(Report.GetAreas(), Report.GetReportedAreas())
    End Sub

    <Fact>
    Public Sub GetReportedAreas1()
        SetAreasExclude1Area()
        Assert.NotEqual(Of Dictionary(Of Integer, String))(GroupKey1AreasHash, Report.GetReportedAreas())
    End Sub

    <Fact>
    Public Sub GetReportedAreas2()
        SetAreasExcludeThenIncludeAll()
        Assert.Equal(Of Dictionary(Of Integer, String))(GroupKey1AreasHash, Report.GetReportedAreas())
    End Sub

    <Fact>
    Public Sub DoNotExcludeLabelsOnInvocationOfSetAreas()
        ' using 'Filter Checklist' functionality to exclude label(s), then including another checklist, so that 'Filter Label' functionality is no longer available
        ' This causes excluded label(s) from before to still be excluded
        Dim AreasToInclude As Integer() = {58, 59}
        Dim ExpectedDS As Data.DataSet = IncludeAreasFromDS(AreasToInclude)
        Dim ResDS As New Data.DataSet
        Dim ExpectedRecordSet As New RecordSet(ExpectedDS)

        Report.SetArea(58)

        Report.SetLabels({553, 554}.ToList()) 'exclude a label (LabelKey 555)
        ResDS = Report.SetAreas(AreasToInclude.ToList()) 'include another checklist

        'ensure excluded label(s) from before are NOT excluded anymore
        Assert.Equal(ExpectedDS.Tables(0).Rows.Count, ResDS.Tables(0).Rows.Count)
        Assert.True(ExpectedRecordSet.DataSetsMatch(ResDS))

        Report.SetGroup(1) 'reset status of Report class instantiation to og environment from constructor
    End Sub
End Class

Public Class LabelFunctionTests
    Inherits Security

    Private Report As New Report(New Dictionary(Of String, String) From {
        {"GroupKey", 1},
        {"AreaKey", 0}
    })
    Private NitrogenDailyLabelsHash As New Dictionary(Of Integer, String) From {
        {553, "Recirculation Water | >75 GPM"},
        {554, "DP Across Media | 1-3 inH20"},
        {555, "Recirculation Water | 3-10 pH"}
    }
    Private DS As Data.DataSet
    Private Format As New Format()

    Public Sub New()
        Report.SetArea(58) 'SC-1 Fume Scrubber Monitoring Daily
        Report.SetDateRange("04/01/2025", "04/01/2025")
    End Sub

    Public Function NumOfRows() As Integer
        Return Report.GetDS().Tables(0).Rows.Count
    End Function

    <Fact>
    Public Sub BaselineTest()
        'check if return dataset from class instantiation is as expected
        Assert.Equal(3, NumOfRows())
    End Sub

    <Fact>
    Public Sub SetLabelsNothingArg()
        'if SetLabels receivs null as arg 1, return empty dataset
        Report.SetLabels(Nothing)
        Assert.Equal(0, NumOfRows())
    End Sub

    <Fact>
    Public Sub SetLabelsEmptyListArg()
        'if SetLabels receivs empty list as arg 1, return empty dataset
        Report.SetLabels(New List(Of Integer))
        Assert.Equal(0, NumOfRows()) 'ensure number of rows is not confined to a single area
    End Sub

    <Fact>
    Public Sub SetLabelsAllLabels()
        'pass list of all labels for AreaKey 58, and check to see if instantiation holds the same dataset before function SetLabels in called
        Dim RecordSet As New RecordSet(Report.SetArea(58))  'calling SetArea function to reset GroupDS in Class instantiation

        SetLabelsEmptyListArg() 'calling this will set GroupDS to be blank within Report Class instantiation

        Assert.True(RecordSet.DataSetsMatch(Report.SetLabels(NitrogenDailyLabelsHash.Keys.ToList()))) 'ensure number of rows is not confined to a single area
    End Sub

    <Fact>
    Public Sub SetLabelsExcludeALabel()
        'pass list that excludes 1 label for AreaKey 58, and check to see if instantiation holds different dataset before function SetLabels in called
        Dim RecordSet As RecordSet
        Dim NitrogenDailyLabelsHashNew As New Dictionary(Of Integer, String)(NitrogenDailyLabelsHash)
        Dim RemovedKvp As New Dictionary(Of Integer, String) From {
            {553, "Recirculation Water | >75 GPM"}
        }
        Dim LabelsToInclude As New List(Of Integer)
        Dim DS As Data.DataSet = Report.GetDS()

        Report.SetArea(58) 'sanity check

        'remove rows from DataSet programmatically that include LabelToRemove
        NitrogenDailyLabelsHashNew.Remove(0)
        For Each DR As Data.DataRow In DS.Tables(0).Rows
            If RemovedKvp.ContainsKey(DR("LabelKey")) Then
                DR.Delete() 'mark DataRow for deletion
            Else
                LabelsToInclude.Add(DR("LabelKey"))
            End If
        Next
        DS.Tables(0).AcceptChanges() 'remove DataRow(s) marked for deletion permanently

        RecordSet = New RecordSet(DS)

        Assert.True(RecordSet.DataSetsMatch(Report.SetLabels(LabelsToInclude)))
    End Sub

    <Fact>
    Public Sub GetLabelsTest1()
        'ensure, after constructor is ran, that Report.GetLabels() returns a list of all labels from a checklist
        Assert.Equal(Of Dictionary(Of Integer, String))(NitrogenDailyLabelsHash, Report.GetLabels())
    End Sub

    <Fact>
    Public Sub GetLabelsTest2()
        'call several Report functions, to simulate an instantiation being used. All this to ensure that Report.GetLabels() returns a list of all labels from a checklist
        SetLabelsAllLabels()
        SetLabelsExcludeALabel()
        Assert.Equal(Of Dictionary(Of Integer, String))(NitrogenDailyLabelsHash, Report.GetLabels())
    End Sub

    <Fact>
    Public Sub GetLabelsTest3()
        'ensure, after changing Group, then changing Group back to 'All', that Report.GetLabels() returns an empty list
        Report.SetGroup(2)
        Report.SetGroup(0) 'return back to og from constructor
        Assert.Equal(Of Dictionary(Of Integer, String))(New Dictionary(Of Integer, String), Report.GetLabels())
    End Sub

    <Fact>
    Public Sub GetLabelsTest4()
        'ensure, after changing Area, then changing Area back to Nitrogen Daily, that Report.GetLabels() returns a list of all labels from Nitrogen Daily
        Report.SetArea(48)
        Report.SetArea(58) 'return back to og from constructor
        Assert.Equal(Of Dictionary(Of Integer, String))(NitrogenDailyLabelsHash, Report.GetLabels())
    End Sub


    <Fact>
    Public Sub GetLabelsTest5()
        'pass empty List to SetAreas() function as arg
        Report.SetAreas(New List(Of Integer))
        Assert.Equal(Of Dictionary(Of Integer, String))(New Dictionary(Of Integer, String), Report.GetLabels())
    End Sub

    <Fact>
    Public Sub GetLabelsReturnEmptyUntil1AreaIsLeft()
        Report.SetAreas(New List(Of Integer) From {58, 59})
        Report.SetAreas(New List(Of Integer) From {58})
        Assert.Equal(Of Dictionary(Of Integer, String))(NitrogenDailyLabelsHash, Report.GetLabels())
    End Sub

    <Fact>
    Public Sub OrderByDateTest1()
        'ensure, after calling Report.OrderByDate(), the DS is not equal to the dataset returned by Report.GetDS()
        Report.SetDateRange("04/01/2025", "04/03/2025")

        Dim RecordSet1 As New RecordSet(Report.GetDS())
        Dim RecordSet2 As New RecordSet(Report.OrderDSByDate())
        Assert.False(RecordSet1.DataSetsMatch(RecordSet2))
    End Sub

    <Fact>
    Public Sub OrderByDateTest2()
        'ensure that if a date range does NOT exists, and after calling Report.OrderByDate(), the DS has no records
        Report.SetDateRange(Nothing, Nothing)
        Assert.Equal(Of Integer)(0, Report.OrderDSByDate().Tables(0).Rows.Count)
    End Sub

    <Fact>
    Public Sub OrderByDateTest3()
        'call OrderDSByDate(), test against a live dataset to ensure each label is display for a date, then the next date
        Dim DR As Data.DataRow

        Report.SetLabels(NitrogenDailyLabelsHash.Keys.ToList()) 'no labels are excluded
        Report.SetDateRange("04/01/2025", "04/03/2025")
        DS = Report.OrderDSByDate()

        For I As Integer = 0 To 2 'ensure first 3 rows have the same date
            DR = DS.Tables(0).Rows(I)

            Assert.Equal(Of String)(DR("Date"), "04/01/2025")
        Next

        'since there are 3 labels for Nitrogen Daily, the date for the next DataRow should contains "04/02/2025"
        DR = DS.Tables(0).Rows(3)
        Assert.Equal(Of String)(DR("Date"), "04/02/2025")
    End Sub

    <Fact>
    Public Sub OrderByDateTest4()
        'if Report.OrderDSByDate() is called first, DS returned from GetDS() matches DS returned by Report.OrderDSByDate()
        Dim RecordSet1 As RecordSet

        Report.SetDateRange("04/01/2025", "04/03/2025")
        DS = Report.OrderDSByDate()

        RecordSet1 = New RecordSet(DS)

        Assert.True(RecordSet1.DataSetsMatch(New RecordSet(Report.GetDS())))
    End Sub

    <Fact>
    Public Sub UndoOrderByDateTest1()
        'Report.UndoOrderDSByDate() reverses result from Report.OrderDSByDate()
        Dim RecordSet As RecordSet

        Report.SetDateRange("04/01/2025", "04/03/2025")

        RecordSet = New RecordSet(Report.GetDS())

        Report.OrderDSByDate()

        Assert.True(RecordSet.DataSetsMatch(New RecordSet(Report.UndoOrderDSByDate())))
    End Sub

    <Fact>
    Public Sub DateTest1()
        'ensure InputDate includes date and time
        Dim DS As Data.DataSet = Report.GetDS()
        Dim AccurateData As Boolean = True

        For Each DR As Data.DataRow In DS.Tables(0).Rows
            If (IsDateOnly(DR("InputDate"))) Then 'b/c the dataset is on AreaKey 58 for 04/01/2025, which has date stamps for each input, this condition should NOT be true
                AccurateData = False
            End If
        Next

        Assert.True(AccurateData)
    End Sub

    <Fact>
    Public Sub DateTest3()
        'ensure Accurate StartDate and InputDate values.
        'Test against edgecase T_LogData Key 334, where the InputDate has a date different than StartDate field
        'This means operator filled in data AFTER the due date

        'setup environment for T_LogData Key 334
        Report.SetDateRange("2025-03-23", "2025-03-23")
        Report.SetArea(57)

        Dim DS As Data.DataSet = Report.GetDS()
        Dim ExpectedMismatches As Boolean = True

        For Each DR As Data.DataRow In DS.Tables(0).Rows
            If Date.Parse(DR("StartDate")).Day = Date.Parse(DR("InputDate")).Day Then
                ExpectedMismatches = False
                Exit For
            End If
        Next

        Assert.True(ExpectedMismatches)

        OgEnvironment()
    End Sub

    <Fact>
    Public Sub InputDateNeverNull1()
        'StartDate should ALWAYS have a value!!!!

        'Group=3&Area=75&StartDate=4/4/2025&EndDate=4/4/2025
        'the querystring above is an instance where StartDate field values display as empty
        'Group is 0 for current environment (All), so am NOT calling Report.SetGroup()

        Dim DS As Data.DataSet
        Dim NullInputDate As Boolean = False

        Report.SetArea(75)
        Report.SetDateRange("4/4/2025", "4/4/2025")
        DS = Report.GetDS()

        For Each DR As Data.DataRow In DS.Tables(0).Rows
            If String.IsNullOrEmpty(DR("StartDate")) Then
                NullInputDate = True
                Exit For
            End If
        Next

        Assert.False(NullInputDate)
    End Sub

    <Fact>
    Public Sub AccurateStartDate()
        'when InputValue is null, StartDate should have a value that matches T_LogData Date field value rather than today's date
        '?Group=1&Area=48&StartDate=4/14/2025&EndDate=4/20/2025

        Dim DS As Data.DataSet

        Report.SetArea(48)
        Report.SetDateRange("4/19/2025", "4/20/2025") 'null operator values for this date range
        Report.OrderDSByDate()
        DS = Report.GetDS()

        For Each DR As Data.DataRow In DS.Tables(0).Rows
            If String.IsNullOrEmpty(DR("Value")) Then
                Assert.NotEqual(Format.DateField(Today.Date), DR("StartDate"))
            End If
        Next
    End Sub

    Private Sub OgEnvironment()
        'return environment to og setup from constructor
        Report.SetArea(58) 'SC-1 Fume Scrubber Monitoring Daily
        Report.SetDateRange("04/01/2025", "04/01/2025")
    End Sub



    Private Function IsDateOnly(DateValue As String) As Boolean
        Dim parsedDate As DateTime
        Return DateTime.TryParseExact(DateValue, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, parsedDate)
    End Function
End Class

Public Class GetOperatorsTest
    Inherits Security
    Public Report As New Report(New Dictionary(Of String, String) From {
        {"GroupKey", 0},
        {"AreaKey", 0}
    })
    Private QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
    Sub New()
    End Sub

    <Fact>
    Public Sub DoNotReturnNullOrBasicUser()
        Dim DS As New Data.DataSet
        Dim OperatorList As New List(Of String)

        Report.SetArea(48)

        DS = Report.GetOperators()

        For Each DR As Data.DataRow In DS.Tables(0).Rows
            OperatorList.Add(If(IsDBNull(DR("Operator")), Nothing, DR("Operator")))
        Next

        '(hopefully) scale proof way of testing
        Assert.True(OperatorList.Contains("Chase Dostie"))
        Assert.False(OperatorList.Contains(Nothing))
        Assert.False(OperatorList.Contains("Brett Teets"))
    End Sub

    <Fact>
    Public Sub DoNotReturnSzymonOrBrett()
        Dim DS As New Data.DataSet
        Dim OperatorList As New List(Of String)

        Report.SetArea(0)

        DS = Report.GetOperators()

        For Each DR As Data.DataRow In DS.Tables(0).Rows
            OperatorList.Add(If(IsDBNull(DR("Operator")), Nothing, DR("Operator")))
        Next

        '(hopefully) scale proof way of testing
        Assert.True(OperatorList.Contains("Chase Dostie"))
        Assert.True(OperatorList.Contains("mark kiser"))
        Assert.False(OperatorList.Contains("Brett Teets"))
        Assert.False(OperatorList.Contains("Szymon Tyburek"))
    End Sub
End Class

Public Class AdminOverrideTests
    Inherits Security

    Private Report As New Report(New Dictionary(Of String, String) From {
        {"GroupKey", 0},
        {"AreaKey", 0}
    })
    Private NitrogenDailyLabels As New List(Of String) From {"Recirculation Water | >75 GPM", "DP Across Media | 1-3 inH20", "Recirculation Water | 3-10 pH"}
    Private OgDS As Data.DataSet
    Private LabelKey As Integer = 553
    Private RecordSet As New RecordSet(OgDS)

    Public Sub New()
        Report.SetArea(58) 'SC-1 Fume Scrubber Monitoring Daily
        Report.SetDateRange("04/14/2025", "04/14/2025")
        OgDS = Report.GetDS()
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
        Dim InputsJson As String = GetSingleDbField("SELECT Inputs FROM [ALTS].[dbo].[T_LogData] WHERE [Key]=514", New Dictionary(Of String, Dictionary(Of String, String)), "Inputs")
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
        Dim OverrideInputs As Dictionary(Of Integer, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(Report.Override(Config, Mods))

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
    Inherits Security

    Private Format As New Format()
    Private Report As New Report(New Dictionary(Of String, String) From {
        {"GroupKey", 0},
        {"AreaKey", 58}
    })
    Private DateLowestBound As String = GetSingleDbField("SELECT FORMAT(MIN(Date), 'MM/dd/yyyy') As DateLowestBound FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=58", New Dictionary(Of String, Dictionary(Of String, String)), "DateLowestBound")
    Private DateHighestBound As String = GetSingleDbField("SELECT FORMAT(MAX(Date), 'MM/dd/yyyy') As DateHighestBound FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=58", New Dictionary(Of String, Dictionary(Of String, String)), "DateHighestBound")

    Public Sub New()
        SetValidDateRange()
    End Sub

    Private Sub SetValidDateRange()
        Report.SetDateRange("04/14/2025", "04/14/2025")
    End Sub

    Private Function NumOfRecords()
        Return Report.GetDS().Tables(0).Rows.Count
    End Function

    Private Function DateTypeToMMDDYYYYStringFormat(Input As Date)
        Return Format.DateField(Input.ToString).Split(" ")(0)
    End Function

    <Fact>
    Private Sub Baseline()
        Assert.NotEqual(0, NumOfRecords())
    End Sub

    <Fact>
    Public Sub StartDateAtLowerBound()
        Report.SetDateRange(DateLowestBound, "04/14/2025")
        Assert.NotEqual(0, NumOfRecords())
    End Sub

    <Fact>
    Public Sub StartDateAtUpperBound()
        Report.SetDateRange(DateLowestBound, DateTypeToMMDDYYYYStringFormat(Today))
        Assert.NotEqual(0, NumOfRecords())
    End Sub

    <Fact>
    Public Sub DateBeforeLowerBound()
        Dim LowerBound As Date = Date.Parse(DateLowestBound).AddDays(-1)

        Report.SetDateRange(DateTypeToMMDDYYYYStringFormat(LowerBound), "04/14/2025")
        Assert.NotEqual(0, NumOfRecords())
    End Sub

    <Fact>
    Public Sub DateAfterUpperBound()
        Report.SetDateRange(DateLowestBound, DateTypeToMMDDYYYYStringFormat(Today.AddDays(1)))
        Assert.NotEqual(0, NumOfRecords())
    End Sub

    <Fact>
    Public Sub EndDatePrecedesStartDate()
        Report.SetDateRange(DateHighestBound, DateLowestBound)
        Assert.Equal(0, NumOfRecords())
    End Sub

    <Fact>
    Public Sub StartDateSucceedsEndDate()
        Report.SetDateRange(DateTypeToMMDDYYYYStringFormat(Date.Parse(DateLowestBound).AddDays(1)), DateLowestBound)
        Assert.Equal(0, NumOfRecords())
    End Sub

    <Fact>
    Public Sub EmptyStartDate()
        Report.SetDateRange(String.Empty, DateHighestBound)
        Assert.Equal(0, NumOfRecords())
    End Sub

    <Fact>
    Public Sub EmptyEndDate()
        Report.SetDateRange(DateLowestBound, String.Empty)
        Assert.Equal(0, NumOfRecords())
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

Public Class ColsWithIdenticalValueTests
    Inherits DummyDS

    Private RecordSet As RecordSet = InstantiationWithData(20)
    Private DS1 As Data.DataSet = RecordSet.GetDS()
    Private Report As New Report(New Dictionary(Of String, String) From { 'since ColsWithIdenticalValues() will grab DS within instantiation when argument is NOT passed, instantiate with a small dataset
        {"GroupKey", 1},
        {"AreaKey", 48}
    })
    Private MatchingColsList As New List(Of String) From {"Null"} 'DummyDS.InstantiationWithData() return a RecordSet with DBNull.Value for all cells in the 'Null' column

    Private Sub MatchCols(Cols As String())
        Dim RC As Integer = DS1.Tables(0).Rows.Count

        MatchingColsList = New List(Of String) From {"Null"} 'reset

        For I As Integer = 0 To RC - 1
            Dim DR As Data.DataRow = DS1.Tables(0).Rows(I)

            For Each Col In Cols 'match field to value in row 1
                DR(Col) = DS1.Tables(0).Rows(0)(Col)

                If MatchingColsList.Contains(Col) = False Then
                    MatchingColsList.Add(Col)
                End If
            Next
        Next
    End Sub

    <Fact>
    Public Sub EdgeCase()
        'pass empty DS to function
        Assert.Equal(New List(Of String), Report.ColsWithIdenticalValues(New Data.DataSet))
    End Sub

    <Fact>
    Public Sub NullDbValue()
        'ensure error does not occur when dealing with null db values
        Assert.Equal(MatchingColsList, Report.ColsWithIdenticalValues(DS1))
    End Sub

    <Fact>
    Public Sub ColBooleanMatching()
        Dim Res As List(Of String)

        'reset
        DS1 = RecordSet.GetDS()

        MatchCols({"Integer"})

        Res = Report.ColsWithIdenticalValues(DS1)

        MatchingColsList.Sort()
        Res.Sort()
        Assert.Equal(MatchingColsList, Res)
    End Sub

    <Fact>
    Public Sub ColsDateDecimalStringMatching()
        Dim Res As List(Of String)

        'reset
        DS1 = RecordSet.GetDS()

        MatchCols({"Date", "Decimal", "String"})

        Res = Report.ColsWithIdenticalValues(DS1)

        MatchingColsList.Sort()
        Res.Sort()
        Assert.Equal(MatchingColsList, Res)
    End Sub
End Class

Public Class OrderedByDateTests
    Private Report As New Report(New Dictionary(Of String, String) From {
        {"GroupKey", 1},
        {"AreaKey", 0}
    })
    Private OrderByLabelDS As Data.DataSet = Report.GetDS()
    Private OrderByDateDS As Data.DataSet = Report.OrderDSByDate()

    Sub New()

    End Sub

    <Fact>
    Public Sub OrderedByDateTrue()
        Report.OrderDSByDate()
        Assert.True(Report.OrderedByDate)
    End Sub

    <Fact>
    Public Sub OrderedByDateFalse()
        Report.UndoOrderDSByDate()
        Assert.False(Report.OrderedByDate)
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

Public Class GetExcelDataTests
    Inherits Report

    Private Function CreateFakeDsSchema() As Data.DataSet
        Dim DS As New Data.DataSet
        Dim DT As New Data.DataTable

        DT.Columns.Add("Area", GetType(String))
        DT.Columns.Add("FieldType", GetType(String))
        DT.Columns.Add("Label", GetType(String))
        DT.Columns.Add("Phase", GetType(String))
        DT.Columns.Add("Value", GetType(String))
        DT.Columns.Add("StartDate", GetType(String))
        DT.Columns.Add("InputDate", GetType(String))
        DT.Columns.Add("InputOperator", GetType(String))

        DS.Tables.Add(DT)

        Return DS
    End Function

    Private Sub CreateFakeDr(DT As Data.DataTable, Data As Dictionary(Of String, Object))
        Dim DR As Data.DataRow = DT.NewRow()

        For Each kvp As KeyValuePair(Of String, Object) In Data
            Dim Field As String = kvp.Key
            Dim FieldValue As Object = kvp.Value

            DR(Field) = FieldValue
        Next

        DT.Rows.Add(DR)
    End Sub

    Private Function StringifyMatrixHash(MatrixHash As Dictionary(Of String, List(Of String())))
        Return JsonSerializer.Serialize(
            MatrixHash.ToDictionary(Function(kv) kv.Key, Function(kv) kv.Value),
            New JsonSerializerOptions With {.WriteIndented = True}
        )
    End Function

    Private Function GetOneLogNoPhasesDataset() As Data.DataSet
        Dim DS As Data.DataSet = CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
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
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
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
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "6.99"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02: 44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "7.18"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06: 33:19 AM"},
            {"InputOperator", "andrew williams"}
        })

        Return DS
    End Function

    <Fact>
    Public Sub OneLogNoPhasesDatasetOrderedByInputTest()
        '1 log with no phases or groups (ordered by input)
        Dim DS As Data.DataSet = GetOneLogNoPhasesDataset()
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
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02: 44:29 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06: 33:19 AM", "andrew williams", "default"}
        }

        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub








    Private Function GetOneLogAllInputsPhasedDataset() As Data.DataSet
        Dim DS As Data.DataSet = CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "All Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
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
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
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
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", "phase 2"},
            {"Value", "6.99"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02: 44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "All Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", "phase 2"},
            {"Value", "7.18"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06: 33:19 AM"},
            {"InputOperator", "andrew williams"}
        })


        Return DS
    End Function

    <Fact>
    Public Sub OneLogAllInputsPhasedOrderedByInputTest()
        '1 log with all inputs in phases or groups  (ordered by input)
        Dim DS As Data.DataSet = GetOneLogAllInputsPhasedDataset()
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
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02: 44:29 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06: 33:19 AM", "andrew williams", "default"}
        }

        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub








    Private Function GetOneLogSomeInputsPhasedDataset() As Data.DataSet
        Dim DS As Data.DataSet = CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Some Inputs Phased Loooooooooooong Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
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
            {"Label", "Stage 1 / AIT AWN-1 Reading | 5.5-9 pH"},
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
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", "phase 2"},
            {"Value", "6.99"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02: 44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Some Inputs Phased Loooooooooooong Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", "phase 2"},
            {"Value", "7.18"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06: 33:19 AM"},
            {"InputOperator", "andrew williams"}
        })


        Return DS
    End Function

    <Fact>
    Public Sub OneLogSomeInputsPhasedOrderedByInputTest()
        '1 log with some inputs in phases or groups (ordered by input)
        Dim DS As Data.DataSet = GetOneLogSomeInputsPhasedDataset()
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
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02: 44:29 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06: 33:19 AM", "andrew williams", "default"}
        }

        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub





    <Fact>
    Public Sub SeveralLogsOrderedByInput()
        'several logs with various types (some/none/all inputs in phases or groups). All logs are ordered by input
        Dim Datasets As New List(Of DataSet) From {GetOneLogNoPhasesDataset(), GetOneLogSomeInputsPhasedDataset(), GetOneLogAllInputsPhasedDataset()}
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
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02: 44:29 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06: 33:19 AM", "andrew williams", "default"}
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
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02: 44:29 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06: 33:19 AM", "andrew williams", "default"}
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
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02: 44:29 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06: 33:19 AM", "andrew williams", "default"}
        }

        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub

    <Fact>
    Public Sub CheckboxFieldtypeDatasetOrderedByInput()
        '1 log with no phases or groups (ordered by input) 
        Dim DS As Data.DataSet = CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        CreateFakeDr(DT, New Dictionary(Of String, Object) From
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
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
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
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", "Checkbox"},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "1"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02: 44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "No Inputs Phased"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "7.18"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06: 33:19 AM"},
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
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "✔", "08/11/2025", "08/11/2025 02: 44:29 PM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06: 33:19 AM", "andrew williams", "default"}
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
        Dim DS As Data.DataSet = CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        CreateFakeDr(DT, New Dictionary(Of String, Object) From
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
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
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
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", "DP"},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "1/0"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02: 44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", "DP"},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "1/1"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06: 33:19 AM"},
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
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "✔/✘", "08/11/2025", "08/11/2025 02: 44:29 PM", "andrew williams", "default"}
        }
        ExpectedRes("Pm Or Checklist... (08/12/2025)") = New List(Of String()) From {
            New String() {"Pm Or Checklist Name", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "✘/✔", "08/12/2025", "08/12/2025 06:33:11 AM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "✔/✔", "08/12/2025", "08/12/2025 06: 33:19 AM", "andrew williams", "default"}
        }

        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub

    <Fact>
    Public Sub OneLogNoPhasesDatasetOrderedByDateTest()
        '1 log with no phases or groups (ordered by date) 
        Dim DS As Data.DataSet = CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        CreateFakeDr(DT, New Dictionary(Of String, Object) From
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
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
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
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "6.99"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02: 44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", DBNull.Value},
            {"Value", "7.18"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06: 33:19 AM"},
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
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02: 44:29 PM", "andrew williams", "default"}
        }
        ExpectedRes("Pm Or Checklist... (08/12/2025)") = New List(Of String()) From {
            New String() {"Pm Or Checklist Name", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "6.30", "08/12/2025", "08/12/2025 06:33:11 AM", "andrew williams", "default"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06: 33:19 AM", "andrew williams", "default"}
        }

        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub

    <Fact>
    Public Sub OneLogAllInputsPhasedOrderedByDate()
        '1 log with all inputs in phases or groups  (ordered by date)
        Dim DS As Data.DataSet = CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        CreateFakeDr(DT, New Dictionary(Of String, Object) From
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
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
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
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", "phase 2"},
            {"Value", "6.99"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02: 44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", "phase 2"},
            {"Value", "7.18"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06: 33:19 AM"},
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
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02: 44:29 PM", "andrew williams", "default"}
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
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06: 33:19 AM", "andrew williams", "default"}
        }


        Assert.Equal(
            StringifyMatrixHash(ExpectedRes),
            StringifyMatrixHash(GetExcelData(ReportInst))
        )
    End Sub

    <Fact>
    Public Sub OneLogSomeInputsPhasedOrderedByDate()
        '1 log with some inputs in phases or groups (ordered by date)
        Dim DS As Data.DataSet = CreateFakeDsSchema()
        Dim DT As Data.DataTable = DS.Tables(0)

        CreateFakeDr(DT, New Dictionary(Of String, Object) From
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
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
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
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", "phase 2"},
            {"Value", "6.99"},
            {"StartDate", "08/11/2025"},
            {"InputDate", "08/11/2025 02: 44:29 PM"},
            {"InputOperator", "andrew williams"}
        })
        CreateFakeDr(DT, New Dictionary(Of String, Object) From
        {
            {"Area", "Pm Or Checklist Name"},
            {"FieldType", DBNull.Value},
            {"Label", "Stage 2 / AIT AWN-2 Reading | 5.5-9 pH"},
            {"Phase", "phase 2"},
            {"Value", "7.18"},
            {"StartDate", "08/12/2025"},
            {"InputDate", "08/12/2025 06: 33:19 AM"},
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
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "6.99", "08/11/2025", "08/11/2025 02: 44:29 PM", "andrew williams", "default"}
        }
        ExpectedRes("Pm Or Checklist... (08/12/2025)") = New List(Of String()) From {
            New String() {"Pm Or Checklist Name", "", "", "", "", "A1"},
            New String() {"", "", "", "", "", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"Item", "Value", "Start Date", "Input Date", "Operator", "bold"},
            New String() {"Stage 1 / AIT AWN-1 Reading | 5.5-9 pH", "6.30", "08/12/2025", "08/12/2025 06:33:11 AM", "andrew williams", "default"},
            New String() {"", "", "", "", "", "default"},
            New String() {"phase 2", "", "", "", "", "bold"},
            New String() {"Stage 2 / AIT AWN-2 Reading | 5.5-9 pH", "7.18", "08/12/2025", "08/12/2025 06: 33:19 AM", "andrew williams", "default"}
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