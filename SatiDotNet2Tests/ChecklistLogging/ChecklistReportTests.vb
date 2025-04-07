Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library
Imports System
Imports System.Globalization

Public Class GroupReportTests
    Dim GroupKey As Integer = 1
    Public GroupReportConfig As New Dictionary(Of String, String) From {
        {"GroupKey", GroupKey},
        {"AreaKey", 0}
    }
    Public FDCG As New GroupReport(GroupReportConfig) 'FDCG = Facilities Daily Checklist Group
    Dim Security As New Security
    Private QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
    Private ReadOnly InvalidDateMessage As String = "Error: Invalid date"
    Private ReadOnly OutOfRangeDateMessage As String = "Error: Out of dataset range"

    Public Sub New()
        Dim StartDate As Date = "03/16/2025"
        Dim EndDate As Date = "03/31/2025"

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
    '    'execute sql query with the same GroupKey as FDCG instantiation of GroupReport class. Compare the 2 DataSets, expect false
    '    Dim DS As Data.DataSet

    '    DS = Security.GetMyDataSetParamQuery(FDCG.ConstructorQuery, QueryConfig)

    '    Assert.False(AreDataSetsEqual(DS, FDCG.GetDS()))
    'End Sub

    '<Fact>
    'Public Sub ConstructorTest2()
    '    'instantiate GroupReport class with GroupKey of 2, and compare it to DS of GroupKey 1, which should NOT be same, returning false
    '    Dim DS As Data.DataSet

    '    DS = Security.GetMyDataSetParamQuery(FDCG.ConstructorQuery, QueryConfig)

    '    Assert.False(AreDataSetsEqual(DS, New GroupReport(New Dictionary(Of String, String) From {
    '        {"GroupKey", 2},
    '        {"AreaKey", 0}
    '    }).GetDS()))
    'End Sub


    <Fact>
    Public Sub ConstructorTest1()
        'ensure that if a date range does NOT exists, and after calling FDCG.OrderByDate(), the DS has no records
        FDCG.SetDateRange(Nothing, Nothing)
        Assert.Equal(Of Integer)(0, FDCG.OrderDSByDate().Tables(0).Rows.Count)
        FDCG.SetDateRange("03/16/2025", "03/31/2025") 'return date range to the og values, set in constructor of this class
    End Sub


    <Fact>
    Public Sub ConstructorTest6()
        'instantiate GroupReport class mocking FDCG, meaning its GetDS() should be the same
        Dim GroupReport As New GroupReport(GroupReportConfig)
        GroupReport.SetDateRange("03/16/2025", "03/31/2025")

        Assert.True(AreDataSetsEqual(FDCG.GetDS(), GroupReport.GetDS()))
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
        Dim GroupReport As New GroupReport(New Dictionary(Of String, String) From {
            {"GroupKey", GroupKey},
            {"AreaKey", AreaKey}
        })
        Dim DS As Data.DataSet
        Dim DR As Data.DataRow
        Dim RC As Integer
        Dim AccurateData As Boolean = True

        GroupReport.SetDateRange("03/16/2025", "03/31/2025")
        DS = GroupReport.GetDS()
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
        'date range is passed to the constructor config object. ensure GetDS() from GroupReport class is within the set date range
        Dim StartDate As Date = "03-22-2025"
        Dim EndDate As Date = "03-25-2025"
        Dim GroupReportConfig As New Dictionary(Of String, String) From {
            {"GroupKey", GroupKey},
            {"AreaKey", 0},
            {"StartDate", StartDate},
            {"EndDate", EndDate}
        }
        Dim DS As Data.DataSet = New GroupReport(GroupReportConfig).GetDS()
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
        Dim GroupReportConfig As New Dictionary(Of String, String) From {
            {"GroupKey", GroupKey},
            {"AreaKey", 0},
            {"StartDate", Nothing},
            {"EndDate", Nothing}
        }
        Dim GroupReport As New GroupReport(GroupReportConfig)
        Dim DS As Data.DataSet
        Dim DR As Data.DataRow
        Dim AccurateData As Boolean = True

        DS = GroupReport.GetDS()

        Assert.True(If(DS.Tables(0).Rows.Count = 0, True, False))
    End Sub

    <Fact>
    Public Sub SetDateRangeTest1()
        'date range is NOT passed to the constructor, but SetDateRange public function is called after the constructor runs. ensure GetDS() from GroupReport class is within the set date range
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
        'calling SetArea function. ensure GetDS() from GroupReport class does not hold data for any other checklist
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
        'calling SetArea function with AreaKey of 75 (dummy "checklist"). Its first log is after 03/16/2025 (the date of initial entry for all logs up till now)
        FDCG.SetGroup(0) 'ALL groups
        Dim DS As Data.DataSet = FDCG.SetArea(75)

        Assert.True(DS.Tables(0).Rows.Count > 0)
    End Sub

    <Fact>
    Public Sub SetGroupTest1()
        'calling SetGroup function. ensure GetDS() from GroupReport class does not hold data for any other group
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
        Dim DS As Data.DataSet = FDCG.SetGroup(0)

        Assert.True(DS.Tables(0).Rows.Count > 0)
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

Public Class LabelFunctionTests
    Inherits Security

    Private GroupReport As New GroupReport(New Dictionary(Of String, String) From {
        {"GroupKey", 0},
        {"AreaKey", 0}
    })
    Private NitrogenDailyLabels As New List(Of String) From {"Recirculation Water | >75 GPM", "DP Across Media | 1-3 inH20", "Recirculation Water | 3-10 pH"}
    Private DS As Data.DataSet

    Public Sub New()
        GroupReport.SetArea(58) 'SC-1 Fume Scrubber Monitoring Daily
        GroupReport.SetDateRange("04/01/2025", "04/01/2025")
    End Sub

    <Fact>
    Public Sub BaselineTest1()
        'check if return dataset from class instantiation is as expected
        Assert.Equal(3, GroupReport.GetDS().Tables(0).Rows.Count)
    End Sub

    <Fact>
    Public Sub ExcludeLabelsTest1()
        'pass empty List data structure, expect 3 rows in return, since date range is 1 day (04/01/2025)
        DS = GroupReport.ExcludeLabels(New List(Of String))
        Assert.Equal(3, DS.Tables(0).Rows.Count)
    End Sub

    <Fact>
    Public Sub ExcludeLabelsTest2()
        'pass List with 1 element, expect 2 rows in return
        DS = GroupReport.ExcludeLabels(New List(Of String) From {NitrogenDailyLabels(0)})
        Assert.Equal(2, DS.Tables(0).Rows.Count)
    End Sub

    <Fact>
    Public Sub ExcludeLabelsTest3()
        'pass List with 1 element, expect 2 rows in return. check to make sure the 2 rows left are the labels that should still exist
        Dim AccurateData = True
        Dim LabelToRemove = NitrogenDailyLabels(0)
        DS = GroupReport.ExcludeLabels(New List(Of String) From {LabelToRemove})

        For Each DR As Data.DataRow In DS.Tables(0).Rows
            If DR("Label") = LabelToRemove Then
                AccurateData = False
            End If
        Next

        Assert.True(AccurateData)
    End Sub

    <Fact>
    Public Sub ExcludeLabelsTest4()
        'pass List with 2 elements to arg, expect 1 row in return
        DS = GroupReport.ExcludeLabels(New List(Of String) From {NitrogenDailyLabels(0), NitrogenDailyLabels(1)})
        Assert.Equal(1, DS.Tables(0).Rows.Count)
    End Sub

    <Fact>
    Public Sub ExcludeLabelsTest5()
        'pass List with 2 elements, expect 1 row in return. check to make sure the row left is the one that should exist
        DS = GroupReport.ExcludeLabels(New List(Of String) From {NitrogenDailyLabels(0), NitrogenDailyLabels(1)})
        Assert.True(DS.Tables(0).Rows.Count = 1 AndAlso DS.Tables(0).Rows(0)("Label") = NitrogenDailyLabels(2))
    End Sub

    <Fact>
    Public Sub ExcludeLabelsTest6()
        'ensure GroupReport.ExcludeLabels() excludes only the most recently passed labels
        'pass List with 1 element, then call the same function again but pass List in with 2 elements
        'expect 1 row in return. check to make sure the row left is the one that should exist
        GroupReport.ExcludeLabels(New List(Of String) From {NitrogenDailyLabels(2)})
        DS = GroupReport.ExcludeLabels(New List(Of String) From {NitrogenDailyLabels(0), NitrogenDailyLabels(1)})
        Assert.True(DS.Tables(0).Rows.Count = 1 AndAlso DS.Tables(0).Rows(0)("Label") = NitrogenDailyLabels(2))
    End Sub

    <Fact>
    Public Sub GetLabelsTest1()
        'ensure, after constructor is ran, that GroupReport.GetLabels() returns a list of all labels from a checklist
        Assert.Equal(Of List(Of String))(NitrogenDailyLabels, GroupReport.GetLabels())
    End Sub

    <Fact>
    Public Sub GetLabelsTest2()
        'call several GroupReport functions, to simulate an instantiation being used. All this to ensure that GroupReport.GetLabels() returns a list of all labels from a checklist
        ExcludeLabelsTest4()
        ExcludeLabelsTest5()
        Assert.Equal(Of List(Of String))(NitrogenDailyLabels, GroupReport.GetLabels())
    End Sub

    <Fact>
    Public Sub GetLabelsTest3()
        'ensure, after changing Group, then changing Group back to 'All', that GroupReport.GetLabels() returns an empty list
        GroupReport.SetGroup(2)
        GroupReport.SetGroup(0) 'return back to og from constructor
        Assert.Equal(Of List(Of String))(New List(Of String), GroupReport.GetLabels())
    End Sub

    <Fact>
    Public Sub GetLabelsTest4()
        'ensure, after changing Area, then changing Area back to Nitrogen Daily, that GroupReport.GetLabels() returns a list of all labels from Nitrogen Daily
        GroupReport.SetArea(48)
        GroupReport.SetArea(58) 'return back to og from constructor
        Assert.Equal(Of List(Of String))(NitrogenDailyLabels, GroupReport.GetLabels())
    End Sub

    <Fact>
    Public Sub OrderByDateTest1()
        'ensure, after calling GroupReport.OrderByDate(), the DS is not equal to the dataset returned by GroupReport.GetDS()
        Assert.NotEqual(Of DataSet)(GroupReport.GetDS(), GroupReport.OrderDSByDate())
    End Sub

    <Fact>
    Public Sub OrderByDateTest2()
        'ensure that if a date range does NOT exists, and after calling GroupReport.OrderByDate(), the DS has no records
        GroupReport.SetDateRange(Nothing, Nothing)
        Assert.Equal(Of Integer)(0, GroupReport.OrderDSByDate().Tables(0).Rows.Count)
        GroupReport.SetDateRange("04/01/2025", "04/01/2025") 'return date range to the og values, set in constructor of this class
    End Sub

    <Fact>
    Public Sub OrderByDateTest3()
        'call OrderDSByDate(), test against a live dataset to ensure each label is display for a date, then the next date
        Dim AccurateData As Boolean = True
        Dim DR As Data.DataRow

        GroupReport.ExcludeLabels(New List(Of String)) 'no labels are excluded
        GroupReport.SetDateRange("04/01/2025", "04/03/2025")
        DS = GroupReport.OrderDSByDate()

        For I As Integer = 0 To 2 'ensure first 3 rows have the same date
            DR = DS.Tables(0).Rows(I)

            If DR("Label") <> NitrogenDailyLabels(I) OrElse DR("Date").Contains("04-01-2025") = False Then 'ensure the first 3 rows have a different label, but the same date
                AccurateData = False
                Exit For
            End If
        Next

        'since there are 3 labels for Nitrogen Daily, the date for the next DataRow should contains "04/02/2025"
        DR = DS.Tables(0).Rows(3)
        If DR("Date").Contains("04-02-2025") = False Then
            AccurateData = False
        End If

        Assert.True(AccurateData)
        GroupReport.SetDateRange("04/01/2025", "04/01/2025") 'return date range to the og values, set in constructor of this class
    End Sub

    <Fact>
    Public Sub OrderByDateTest4()
        'if GroupReport.OrderDSByDate() is called first, DS returned from GetDS() matches DS returned by GroupReport.OrderDSByDate()

        GroupReport.SetDateRange("04/01/2025", "04/03/2025")

        DS = GroupReport.OrderDSByDate()
        Assert.Equal(Of DataSet)(DS, GroupReport.GetDS())
        GroupReport.UndoOrderDSByDate() 'undo order ds by date, to return ds to og state

        GroupReport.SetDateRange("04/01/2025", "04/01/2025") 'return date range to the og values, set in constructor of this class
    End Sub

    '<Fact>
    'Public Sub UndoOrderByDateTest1()
    '    'GroupReport.UndoOrderDSByDate() reverses result from GroupReport.OrderDSByDate()

    '    GroupReport.SetDateRange("04/01/2025", "04/03/2025")

    '    DS = GroupReport.GetDS()
    '    GroupReport.OrderDSByDate()
    '    Assert.Equal(Of DataSet)(DS, GroupReport.UndoOrderDSByDate())

    '    GroupReport.SetDateRange("04/01/2025", "04/01/2025") 'return date range to the og values, set in constructor of this class
    'End Sub

    <Fact>
    Public Sub DateTest1()
        'ensure InputDate includes date and time
        Dim DS As Data.DataSet = GroupReport.GetDS()
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
        GroupReport.SetDateRange("2025-03-23", "2025-03-23")
        GroupReport.SetArea(57)

        Dim DS As Data.DataSet = GroupReport.GetDS()
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
        'Group is 0 for current environment (All), so am NOT calling GroupReport.SetGroup()

        Dim DS As Data.DataSet
        Dim NullInputDate As Boolean = False

        GroupReport.SetArea(75)
        GroupReport.SetDateRange("4/4/2025", "4/4/2025")
        DS = GroupReport.GetDS()

        For Each DR As Data.DataRow In DS.Tables(0).Rows
            If String.IsNullOrEmpty(DR("StartDate")) Then
                NullInputDate = True
                Exit For
            End If
        Next

        Assert.False(NullInputDate)
    End Sub

    Private Sub OgEnvironment()
        'return environment to og setup from constructor
        GroupReport.SetArea(58) 'SC-1 Fume Scrubber Monitoring Daily
        GroupReport.SetDateRange("04/01/2025", "04/01/2025")
    End Sub



    Private Function IsDateOnly(DateValue As String) As Boolean
        Dim parsedDate As DateTime
        Return DateTime.TryParseExact(DateValue, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, parsedDate)
    End Function
End Class
