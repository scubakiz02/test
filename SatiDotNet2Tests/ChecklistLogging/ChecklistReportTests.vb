Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library

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

    <Fact>
    Public Sub ConstructorTest1()
        'execute sql query with the same GroupKey as FDCG instantiation of GroupReport class. Compare the 2 DataSets, expect false
        Dim DS As Data.DataSet

        DS = Security.GetMyDataSetParamQuery(FDCG.ConstructorQuery, QueryConfig)

        Assert.False(AreDataSetsEqual(DS, FDCG.GetDS()))
    End Sub

    <Fact>
    Public Sub ConstructorTest2()
        'instantiate GroupReport class with GroupKey of 2, and compare it to DS of GroupKey 1, which should NOT be same, returning false
        Dim DS As Data.DataSet

        DS = Security.GetMyDataSetParamQuery(FDCG.ConstructorQuery, QueryConfig)

        Assert.False(AreDataSetsEqual(DS, New GroupReport(New Dictionary(Of String, String) From {
            {"GroupKey", 2},
            {"AreaKey", 0}
        }).GetDS()))
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