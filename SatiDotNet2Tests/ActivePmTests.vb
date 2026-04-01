Imports System.Security.Cryptography
Imports System.Text.Json
Imports SatiDotNet2.Library
Imports Xunit

Class GetParentIdMock
    Public Function GetFakeDs(Interval As String, AssignedTo As Object, Optional DateTime As Date = Nothing, Optional IsBuildTime As Boolean = True) As Data.DataSet
        Dim DS As New Data.DataSet()
        Dim DT As New Data.DataTable()
        Dim LogStartDateAt As String

        If DateTime = Nothing Then
            LogStartDateAt = System.DateTime.Now.ToString("MM/dd/yyyy")
        Else
            LogStartDateAt = DateTime.ToString("MM/dd/yyyy")
        End If

        DT.Columns.Add("AssignedTo", GetType(String))
        DT.Columns.Add("Interval", GetType(String))
        DT.Columns.Add("IsBuildTime", GetType(Boolean))
        DT.Columns.Add("IsLogNewest", GetType(Boolean))
        DT.Columns.Add("LogStartDateAt", GetType(String))

        'add fake data to fake dataset
        AddDsRow(DT, New Dictionary(Of String, Object) From {
            {"AssignedTo", AssignedTo},
            {"Interval", Interval},
            {"IsBuildTime", IsBuildTime},
            {"IsLogNewest", True},
            {"LogStartDateAt", LogStartDateAt}
        })
        DS.Tables.Add(DT)

        Return DS
    End Function

    Private Sub AddDsRow(DT As Data.DataTable, RowConfig As Dictionary(Of String, Object))
        Dim DR As Data.DataRow = DT.NewRow()

        DR("AssignedTo") = RowConfig("AssignedTo")
        DR("Interval") = RowConfig("Interval")
        DR("IsBuildTime") = RowConfig("IsBuildTime")
        DR("IsLogNewest") = RowConfig("IsLogNewest")
        DR("LogStartDateAt") = RowConfig("LogStartDateAt")

        DT.Rows.Add(DR)
    End Sub
End Class

Public Class ActivePmGetLogConfigTests
    Inherits ActivePm

    Private _InputsFieldValueShell As String = "{""586"":{""Date"":"""",""Operator"":"""",""Value"":""""}}"
    Private _PmName As String = "pm/checklist name"
    Private _GetParentIdMock As New GetParentIdMock()

    Private Function Merge2Datasets(Ds1 As Data.DataSet, Ds2 As Data.DataSet) As Data.DataSet
        'this function merges 2 datasets with different schemas
        'each dataset has 1 row only
        Dim T1 As DataTable = Ds1.Tables(0)
        Dim T2 As DataTable = Ds2.Tables(0)
        Dim MergedTable As New DataTable()

        For Each col As DataColumn In T1.Columns
            MergedTable.Columns.Add(col.ColumnName, col.DataType)
        Next

        For Each Col As DataColumn In T2.Columns
            If Not MergedTable.Columns.Contains(Col.ColumnName) Then
                MergedTable.Columns.Add(Col.ColumnName, Col.DataType)
            End If
        Next

        Dim MergedRow As DataRow = MergedTable.NewRow()
        For Each col As DataColumn In T1.Columns
            MergedRow(col.ColumnName) = T1.Rows(0)(col.ColumnName)
        Next

        For Each col As DataColumn In T2.Columns
            MergedRow(col.ColumnName) = T2.Rows(0)(col.ColumnName)
        Next
        MergedTable.Rows.Add(MergedRow)

        Dim MergedDs As New DataSet()
        MergedDs.Tables.Add(MergedTable)
        Return MergedDs
    End Function

    Private Sub GetVirginLogStateDatasets(ByRef FakeStampDs As DataSet, ByRef FakeLogDs As DataSet)
        FakeStampDs = GetFakeStampDs()

        Dim Ds1 As Data.DataSet = _GetParentIdMock.GetFakeDs("DAILY", "D1")
        Dim Ds2 As Data.DataSet = GetFakeLogDs(False, Nothing, False)

        FakeLogDs = Merge2Datasets(Ds1, Ds2)
    End Sub

    <Fact>
    Public Sub VirginTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "virgin"},
            {"logParentId", "PastIssuesPanel"},
            {"pmName", _PmName}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetVirginLogStateDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetLogConfig(0, Nothing, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub

    <Fact>
    Public Sub PmOrChecklistDisabled_VirginLogState_Test()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "delete"}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetVirginLogStateDatasets(FakeStampDs, FakeLogDs)

        FakeLogDs.Tables(0).Rows(0)("IsActive") = False 'only 1 row exists in fake log dataset

        Dim LogStateConfig As Object = GetLogConfig(0, Nothing, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub






    Private Sub GetIncompleteLogStateDatasets(ByRef FakeStampDs As DataSet, ByRef FakeLogDs As DataSet)
        FakeStampDs = GetFakeStampDs()

        Dim Ds1 As Data.DataSet = _GetParentIdMock.GetFakeDs("DAILY", "D1")
        Dim InputsJson As Dictionary(Of Integer, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(_InputsFieldValueShell)
        InputsJson(586)("Value") = "My wife said if I don't get off Reddit right now she's going to come over and smash my face into the keyboard. I laughed and said 'I would like to se.;,lm;, l,; ;,lmadsc;l,xc k, sca,;lasxc.;,c #'.;cxvc, lmxz;,lm x/.;x zc ,kxmk;lnlp,zx ;,.x.c,'"
        Dim Ds2 As Data.DataSet = GetFakeLogDs(False, JsonSerializer.Serialize(InputsJson))

        FakeLogDs = Merge2Datasets(Ds1, Ds2)
    End Sub

    <Fact>
    Public Sub IncompleteTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "incomplete"},
            {"logParentId", "DailyD1Panel"},
            {"pmName", _PmName}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetIncompleteLogStateDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetLogConfig(0, Nothing, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub


    <Fact>
    Public Sub PmOrChecklistDisabled_IncompleteLogState_Test()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "delete"}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetIncompleteLogStateDatasets(FakeStampDs, FakeLogDs)

        FakeLogDs.Tables(0).Rows(0)("IsActive") = False 'only 1 row exists in fake log dataset

        Dim LogStateConfig As Object = GetLogConfig(0, Nothing, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub









    Private Sub GetSubmittedLogStateAddStampsDatasets(ByRef FakeStampDs As DataSet, ByRef FakeLogDs As DataSet)
        FakeStampDs = GetFakeStampDs()

        Dim Ds1 As Data.DataSet = _GetParentIdMock.GetFakeDs("2 YEARS", DBNull.Value)
        Dim Ds2 As Data.DataSet = GetFakeLogDs()

        FakeLogDs = Merge2Datasets(Ds1, Ds2)
    End Sub

    <Fact>
    Public Sub JustSubmittedAddStampsTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "submitted"},
            {"logParentId", "TwoYearPanel"},
            {"pmName", _PmName},
            {"addStamps", New List(Of String) From {"F&M Manager", "Q/SHE Manager", "Prod Sup"}},
            {"removeStamps", New List(Of String) From {"Maint Sup"}}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetSubmittedLogStateAddStampsDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetLogConfig(0, Nothing, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub


    <Fact>
    Public Sub PmOrChecklistDisabled_JustSubmittedAddStamps_Test()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "delete"}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetSubmittedLogStateAddStampsDatasets(FakeStampDs, FakeLogDs)

        FakeLogDs.Tables(0).Rows(0)("IsActive") = False 'only 1 row exists in fake log dataset

        Dim LogStateConfig As Object = GetLogConfig(0, Nothing, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub





    Private Sub GetSubmittedLogStateStamp2Of3ReceivedDatasets(ByRef FakeStampDs As DataSet, ByRef FakeLogDs As DataSet)
        FakeStampDs = GetFakeStampDs()
        For Each FakeDr As Data.DataRow In FakeStampDs.Tables(0).Rows
            If FakeDr("StampTitle") = "F&M Manager" Then
                FakeDr("StampDateTime") = System.DateTime.Now
            ElseIf FakeDr("StampTitle") = "Q/SHE Manager" Then
                FakeDr("StampDateTime") = Date.Parse(System.DateTime.Now).AddDays(-1)
            End If
        Next

        Dim Ds1 As Data.DataSet = _GetParentIdMock.GetFakeDs("DAILY", "Days (M-F)")
        Dim Ds2 As Data.DataSet = GetFakeLogDs()

        FakeLogDs = Merge2Datasets(Ds1, Ds2)
    End Sub

    <Fact>
    Public Sub Stamp2Of3ReceivedTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "submitted"},
            {"logParentId", "DailyMFShiftPanel"},
            {"pmName", _PmName},
            {"addStamps", New List(Of String) From {"Prod Sup"}},
            {"removeStamps", New List(Of String) From {"F&M Manager", "Q/SHE Manager", "Maint Sup"}}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetSubmittedLogStateStamp2Of3ReceivedDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetLogConfig(0, Nothing, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub


    <Fact>
    Public Sub PmOrChecklistDisabled_Stamp2Of3Received_Test()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "delete"}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetSubmittedLogStateStamp2Of3ReceivedDatasets(FakeStampDs, FakeLogDs)

        FakeLogDs.Tables(0).Rows(0)("IsActive") = False 'only 1 row exists in fake log dataset

        Dim LogStateConfig As Object = GetLogConfig(0, Nothing, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub









    Private Sub GetLogCompletedOnTimeDatasets(ByRef FakeStampDs As DataSet, ByRef FakeLogDs As DataSet)
        FakeStampDs = GetFakeStampDsWtihStamps()

        Dim Ds1 As Data.DataSet = _GetParentIdMock.GetFakeDs("3 YEARS", DBNull.Value)
        Dim Ds2 As Data.DataSet = GetFakeLogDs()

        FakeLogDs = Merge2Datasets(Ds1, Ds2)
    End Sub

    <Fact>
    Public Sub LogCompletedOnTimeTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "completed"},
            {"logParentId", "ThreeYearPanel"},
            {"pmName", _PmName}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetLogCompletedOnTimeDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetLogConfig(0, Nothing, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub

    <Theory>
    <InlineData(1245, "07/15/2025")>
    <InlineData(1151, "07/07/2025")>
    Public Sub LogCompletedStatusBoardDateAtInPastTest(DataKey As Integer, StatusBoardDateAt As String)
        'there's a bug that occurs when the status board date is in the past
        'any log that has a completed state will return a logState of 'delete'
        'it should be 'completed'
        'going to pull data from the DB to replicate this bug
        'These test cases are logs that have received all their stamps with their date field value from the DB
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "completed"},
            {"logParentId", "DailyMFShiftPanel"},
            {"pmName", "DI WATER DAILY"}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetLogCompletedOnTimeDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetLogConfig(DataKey, StatusBoardDateAt)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub

    <Fact>
    Public Sub PmOrChecklistDisabled_CompletedOnTimeLogState_Test()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "delete"}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetLogCompletedOnTimeDatasets(FakeStampDs, FakeLogDs)

        FakeLogDs.Tables(0).Rows(0)("IsActive") = False 'only 1 row exists in fake log dataset

        Dim LogStateConfig As Object = GetLogConfig(0, Nothing, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub








    Private Sub GetLogCompletedLastDatasets(ByRef FakeStampDs As DataSet, ByRef FakeLogDs As DataSet)
        FakeStampDs = GetFakeStampDsWtihStamps()

        Dim Ds1 As Data.DataSet = _GetParentIdMock.GetFakeDs("2 YEARS", DBNull.Value, Nothing, False)
        Dim Ds2 As Data.DataSet = GetFakeLogDs(True, Nothing, False)

        FakeLogDs = Merge2Datasets(Ds1, Ds2)
    End Sub

    <Fact>
    Public Sub LogCompletedLateTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "delete"}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetLogCompletedLastDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetLogConfig(0, Nothing, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub


    <Fact>
    Public Sub PmOrChecklistDisabled_CompletedLateLogState_Test()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "delete"}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetLogCompletedLastDatasets(FakeStampDs, FakeLogDs)

        FakeLogDs.Tables(0).Rows(0)("IsActive") = False 'only 1 row exists in fake log dataset

        Dim LogStateConfig As Object = GetLogConfig(0, Nothing, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub










    Private Sub GetDuplicateLogDatasets(ByRef FakeStampDs As DataSet, ByRef FakeLogDs As DataSet)
        FakeStampDs = GetFakeStampDsWtihStamps()

        Dim Ds1 As Data.DataSet = _GetParentIdMock.GetFakeDs("2 YEARS", DBNull.Value)
        Dim Ds2 As Data.DataSet = GetFakeLogDs(True, Nothing, False)

        FakeLogDs = Merge2Datasets(Ds1, Ds2)
        FakeLogDs.Tables(0).Rows(0)("IsLogDuplicated") = True 'only 1 row exists in fake log dataset
    End Sub

    <Fact>
    Public Sub DuplicateLogTest()
        'create stamp dataset with all relevant stamps received and log dataset with true field value for IsLogDuplicated
        'without IsLogDuplicated field, the return log state should be completed
        'however, IsLogDuplicated field value should cause return log state to be an error
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "error"},
            {"logParentId", "PastIssuesPanel"},
            {"pmName", _PmName}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetDuplicateLogDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetLogConfig(0, Nothing, GetFakeStampDsWtihStamps, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub

    <Fact>
    Public Sub PmOrChecklistDisabled_DuplicateLogState_Test()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "delete"}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetDuplicateLogDatasets(FakeStampDs, FakeLogDs)

        FakeLogDs.Tables(0).Rows(0)("IsActive") = False 'only 1 row exists in fake log dataset

        Dim LogStateConfig As Object = GetLogConfig(0, Nothing, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub







    Private Function GetFakeStampDsWtihStamps() As Data.DataSet
        Dim FakeStampDs As Data.DataSet = GetFakeStampDs(True, Nothing)

        For Each FakeDr As Data.DataRow In FakeStampDs.Tables(0).Rows
            FakeDr("StampDateTime") = System.DateTime.Now
        Next

        Return FakeStampDs
    End Function

    Private Function GetFakeStampDs(Optional CompleteLog As Boolean = True, Optional Inputs As String = Nothing) As Data.DataSet
        Dim DS As New Data.DataSet()
        Dim DT As New Data.DataTable()
        Dim InputsFieldValue As String

        DT.Columns.Add("StampTitle", GetType(String))
        DT.Columns.Add("IsStampActive", GetType(Boolean))
        DT.Columns.Add("StampDateTime", GetType(DateTime))

        If Inputs Is Nothing Then
            InputsFieldValue = _InputsFieldValueShell
        Else
            InputsFieldValue = Inputs
        End If

        'add fake data to fake dataset
        AddStampDsRow(DT, New Dictionary(Of String, Object) From {
            {"StampTitle", "F&M Manager"},
            {"IsStampActive", True},
            {"StampDateTime", DBNull.Value}
        })
        AddStampDsRow(DT, New Dictionary(Of String, Object) From {
            {"StampTitle", "Q/SHE Manager"},
            {"IsStampActive", True},
            {"StampDateTime", DBNull.Value}
        })
        AddStampDsRow(DT, New Dictionary(Of String, Object) From {
            {"StampTitle", "Prod Sup"},
            {"IsStampActive", True},
            {"StampDateTime", DBNull.Value}
        })
        AddStampDsRow(DT, New Dictionary(Of String, Object) From {
            {"StampTitle", "Maint Sup"},
            {"IsStampActive", False},
            {"StampDateTime", DBNull.Value}
        })
        DS.Tables.Add(DT)

        Return DS
    End Function

    Private Sub AddStampDsRow(DT As Data.DataTable, RowConfig As Dictionary(Of String, Object))
        Dim DR As Data.DataRow = DT.NewRow()

        DR("StampTitle") = RowConfig("StampTitle")
        DR("IsStampActive") = RowConfig("IsStampActive")
        DR("StampDateTime") = RowConfig("StampDateTime")

        DT.Rows.Add(DR)
    End Sub

    Private Function GetFakeLogDs(Optional IsLogComplete As Boolean = True, Optional Inputs As String = Nothing, Optional IsLogNewest As Boolean = True) As Data.DataSet
        Dim DS As New Data.DataSet()
        Dim DT As New Data.DataTable()
        Dim InputsFieldValue As String

        DT.Columns.Add("IsLogComplete", GetType(Boolean))
        DT.Columns.Add("IsLogNewest", GetType(Boolean))
        DT.Columns.Add("IsLogDuplicated", GetType(Boolean))
        DT.Columns.Add("IsActive", GetType(Boolean))
        DT.Columns.Add("Inputs", GetType(String))
        DT.Columns.Add("Area", GetType(String))

        If Inputs Is Nothing Then
            InputsFieldValue = _InputsFieldValueShell
        Else
            InputsFieldValue = Inputs
        End If

        'add fake data to fake dataset
        AddLogDsRow(DT, New Dictionary(Of String, Object) From {
            {"IsLogComplete", IsLogComplete},
            {"IsLogNewest", IsLogNewest},
            {"IsLogDuplicated", False},
            {"IsActive", True},
            {"Inputs", InputsFieldValue},
            {"Area", _PmName}
        })
        DS.Tables.Add(DT)

        Return DS
    End Function

    Private Sub AddLogDsRow(DT As Data.DataTable, RowConfig As Dictionary(Of String, Object))
        Dim DR As Data.DataRow = DT.NewRow()

        DR("IsLogComplete") = RowConfig("IsLogComplete")
        DR("IsLogNewest") = RowConfig("IsLogNewest")
        DR("IsLogDuplicated") = RowConfig("IsLogDuplicated")
        DR("IsActive") = RowConfig("IsActive")
        DR("Inputs") = RowConfig("Inputs")
        DR("Area") = RowConfig("Area")

        DT.Rows.Add(DR)
    End Sub

End Class

Public Class GetParentIdTests
    Inherits ActivePm

    Private _GetParentIdMock As New GetParentIdMock()

    <Theory>
    <InlineData("Day Shift", "DailyDayShiftPanel")>
    <InlineData("Night Shift", "DailyNightShiftPanel")>
    <InlineData("Days (M-F)", "DailyMFShiftPanel")>
    <InlineData("D1", "DailyD1Panel")>
    <InlineData("N1", "DailyN1Panel")>
    <InlineData("D2", "DailyD2Panel")>
    <InlineData("N2", "DailyN2Panel")>
    <InlineData("John Doe", "DailyUsersPanel")>
    Private Sub AssigneeTests(AssignedTo As String, ExpectedOutcome As String)
        Dim FakeDs As Data.DataSet = _GetParentIdMock.GetFakeDs("DAILY", AssignedTo)
        Assert.Equal(ExpectedOutcome, GetParentId(0, System.DateTime.Now, FakeDs))
    End Sub

    <Theory>
    <InlineData("DAILY", "DailyD1Panel")>
    <InlineData("WEEKLY", "WeeklyD1Panel")>
    <InlineData("MONTHLY", "MonthlyD1Panel")>
    Private Sub BasicIntervalTests(Interval As String, ExpectedOutcome As String)
        Dim FakeDs As Data.DataSet = _GetParentIdMock.GetFakeDs(Interval, "D1")
        Assert.Equal(ExpectedOutcome, GetParentId(0, System.DateTime.Now, FakeDs))
    End Sub

    <Theory>
    <InlineData("OneTimeD1Panel", "08/24/2025", "08/24/2025")>
    <InlineData("", "08/24/2025", "08/23/2025")>
    <InlineData("", "08/23/2025", "08/24/2025")>
    Public Sub OneTimeOnlyIntervalTest(ExpectedOutcome As String, StatusBoardAt As Date, LogStartDateAt As Date)
        Dim FakeDs As Data.DataSet = _GetParentIdMock.GetFakeDs("ONE TIME ONLY", "D1", LogStartDateAt)
        Assert.Equal(ExpectedOutcome, GetParentId(0, StatusBoardAt, FakeDs))
    End Sub

    <Theory>
    <InlineData("QUARTERLY", "QuarterlyPanel")>
    <InlineData("BIANNUAL", "BiAnnualPanel")>
    <InlineData("1 YEAR", "OneYearPanel")>
    <InlineData("2 YEARS", "TwoYearPanel")>
    <InlineData("3 YEARS", "ThreeYearPanel")>
    <InlineData("4 YEARS", "FourYearPanel")>
    <InlineData("5 YEARS", "FiveYearPanel")>
    Public Sub GreaterThanMonthlyIntervalTest(Interval As String, ExpectedOutcome As String)
        Dim FakeDs As Data.DataSet = _GetParentIdMock.GetFakeDs(Interval, DBNull.Value)
        Assert.Equal(ExpectedOutcome, GetParentId(0, System.DateTime.Now, FakeDs))
    End Sub
End Class
