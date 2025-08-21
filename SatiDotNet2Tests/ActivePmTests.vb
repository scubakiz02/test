Imports Xunit
Imports SatiDotNet2.Library
Imports System.Text.Json

Public Class ActivePmGetStateTests
    Inherits ActivePm
    'ActivePm Class GetState function returns an Object (which is really Dictionary(Of String, Object)
    'the return (at least initially) is used for caching purposes to poll data for the Status Board on a defined interval

    Private _InputsFieldValueShell As String = "{""586"":{""Date"":"""",""Operator"":"""",""Value"":""""}}"

    Private Sub GetVirginLogStateDatasets(ByRef FakeStampDs As DataSet, ByRef FakeLogDs As DataSet)
        FakeStampDs = GetFakeStampDs()
        FakeLogDs = GetFakeLogDs(False)
    End Sub

    <Fact>
    Public Sub VirginTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "virgin"}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetVirginLogStateDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

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

        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub






    Private Sub GetIncompleteLogStateDatasets(ByRef FakeStampDs As DataSet, ByRef FakeLogDs As DataSet)
        Dim InputsJson As Dictionary(Of Integer, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(_InputsFieldValueShell)

        FakeStampDs = GetFakeStampDs()

        InputsJson(586)("Value") = "My wife said if I don't get off Reddit right now she's going to come over and smash my face into the keyboard. I laughed and said 'I would like to se.;,lm;, l,; ;,lmadsc;l,xc k, sca,;lasxc.;,c #'.;cxvc, lmxz;,lm x/.;x zc ,kxmk;lnlp,zx ;,.x.c,'"
        FakeLogDs = GetFakeLogDs(False, JsonSerializer.Serialize(InputsJson))
    End Sub

    <Fact>
    Public Sub IncompleteTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "incomplete"}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetIncompleteLogStateDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

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

        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub








    Private Sub GetSubmittedLogStateAddStampsDatasets(ByRef FakeStampDs As DataSet, ByRef FakeLogDs As DataSet)
        FakeStampDs = GetFakeStampDs()
        FakeLogDs = GetFakeLogDs()
    End Sub

    <Fact>
    Public Sub JustSubmittedAddStampsTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "submitted"},
            {"addStamps", New List(Of String) From {"F&M Manager", "Q/SHE Manager", "Prod Sup"}},
            {"removeStamps", New List(Of String) From {"Maint Sup"}}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetSubmittedLogStateAddStampsDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

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

        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

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

        FakeLogDs = GetFakeLogDs()
    End Sub

    <Fact>
    Public Sub Stamp2Of3ReceivedTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "submitted"},
            {"addStamps", New List(Of String) From {"Prod Sup"}},
            {"removeStamps", New List(Of String) From {"F&M Manager", "Q/SHE Manager", "Maint Sup"}}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetSubmittedLogStateStamp2Of3ReceivedDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

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

        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub









    Private Sub GetLogCompletedOnTimeDatasets(ByRef FakeStampDs As DataSet, ByRef FakeLogDs As DataSet)
        FakeStampDs = GetFakeStampDsWtihStamps()
        FakeLogDs = GetFakeLogDs()
    End Sub

    <Fact>
    Public Sub LogCompletedOnTimeTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "completed"}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetLogCompletedOnTimeDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

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

        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub







    Private Sub GetLogCompletedLastDatasets(ByRef FakeStampDs As DataSet, ByRef FakeLogDs As DataSet)
        FakeStampDs = GetFakeStampDsWtihStamps()
        FakeLogDs = GetFakeLogDs(True, Nothing, False)
    End Sub

    <Fact>
    Public Sub LogCompletedLateTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "delete"}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetLogCompletedLastDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

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

        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub





    Private Sub GetDuplicateLogDatasets(ByRef FakeStampDs As DataSet, ByRef FakeLogDs As DataSet)
        FakeStampDs = GetFakeStampDsWtihStamps()

        FakeLogDs = GetFakeLogDs()
        FakeLogDs.Tables(0).Rows(0)("IsLogDuplicated") = True 'only 1 row exists in fake log dataset
    End Sub

    <Fact>
    Public Sub DuplicateLogTest()
        'create stamp dataset with all relevant stamps received and log dataset with true field value for IsLogDuplicated
        'without IsLogDuplicated field, the return log state should be completed
        'however, IsLogDuplicated field value should cause return log state to be an error
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "error"}
        }
        Dim FakeStampDs As New Data.DataSet
        Dim FakeLogDs As New Data.DataSet

        GetDuplicateLogDatasets(FakeStampDs, FakeLogDs)

        Dim LogStateConfig As Object = GetState(0, GetFakeStampDsWtihStamps, FakeLogDs)

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

        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

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
            {"Inputs", InputsFieldValue}
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

        DT.Rows.Add(DR)
    End Sub

End Class
