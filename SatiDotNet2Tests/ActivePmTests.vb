Imports Xunit
Imports SatiDotNet2.Library
Imports System.Text.Json

Public Class ActivePmGetStateTests
    Inherits ActivePm
    'ActivePm Class GetState function returns an Object (which is really Dictionary(Of String, Object)
    'the return (at least initially) is used for caching purposes to poll data for the Status Board on a defined interval

    Private _InputsFieldValueShell As String = "{""586"":{""Date"":"""",""Operator"":"""",""Value"":""""}}"
    Private _StampIndicator As New StampIndicator()

    <Fact>
    Public Sub VirginTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "virgin"},
            {"logType", "current"}
        }
        Dim FakeStampDs As Data.DataSet = GetFakeStampDs()
        Dim FakeLogDs As Data.DataSet = GetFakeLogDs(False)
        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub

    <Fact>
    Public Sub IncompleteTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "incomplete"},
            {"logType", "current"}
        }
        Dim InputsJson As Dictionary(Of Integer, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(_InputsFieldValueShell)
        Dim FakeStampDs As Data.DataSet = GetFakeStampDs()
        Dim FakeLogDs As Data.DataSet
        Dim LogStateConfig As Object

        InputsJson(586)("Value") = "My wife said if I don't get off Reddit right now she's going to come over and smash my face into the keyboard. I laughed and said 'I would like to se.;,lm;, l,; ;,lmadsc;l,xc k, sca,;lasxc.;,c #'.;cxvc, lmxz;,lm x/.;x zc ,kxmk;lnlp,zx ;,.x.c,'"
        FakeLogDs = GetFakeLogDs(False, JsonSerializer.Serialize(InputsJson))
        LogStateConfig = GetState(0, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub

    <Fact>
    Public Sub JustSubmittedAddStampsTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "submitted"},
            {"addStamps", New List(Of String) From {"F&M Manager", "Q/SHE Manager", "Prod Sup"}},
            {"removeStamps", New List(Of String) From {"Maint Sup"}},
            {"logType", "current"}
        }
        Dim FakeStampDs As Data.DataSet = GetFakeStampDs()
        Dim FakeLogDs As Data.DataSet = GetFakeLogDs()
        Dim LogStateConfig As Object = GetState(0, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub

    <Fact>
    Public Sub Stamp2Of3ReceivedTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "submitted"},
            {"addStamps", New List(Of String) From {"Prod Sup"}},
            {"removeStamps", New List(Of String) From {"F&M Manager", "Q/SHE Manager", "Maint Sup"}},
            {"logType", "current"}
        }
        Dim FakeStampDs As Data.DataSet = GetFakeStampDs()
        Dim FakeLogDs As Data.DataSet = GetFakeLogDs()
        Dim LogStateConfig As Object

        For Each FakeDr As Data.DataRow In FakeStampDs.Tables(0).Rows
            If FakeDr("StampTitle") = "F&M Manager" Then
                FakeDr("StampDateTime") = System.DateTime.Now
            ElseIf FakeDr("StampTitle") = "Q/SHE Manager" Then
                FakeDr("StampDateTime") = Date.Parse(System.DateTime.Now).AddDays(-1)
            End If
        Next

        LogStateConfig = GetState(0, FakeStampDs, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub

    <Fact>
    Public Sub CurrentSectionCompleteLogTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "completed"},
            {"logType", "current"}
        }
        Dim FakeStampDsWtihStamps As Data.DataSet = GetFakeStampDsWtihStamps()
        Dim FakeLogDs As Data.DataSet = GetFakeLogDs()
        Dim LogStateConfig As Object = GetState(0, FakeStampDsWtihStamps, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub

    <Fact>
    Public Sub OverdueSectionCompleteLogTest()
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "completed"},
            {"logType", "overdue"}
        }

        Dim FakeStampDsWtihStamps As Data.DataSet = GetFakeStampDsWtihStamps()
        Dim FakeLogDs As Data.DataSet = GetFakeLogDs(True, Nothing, False)
        Dim LogStateConfig As Object = GetState(0, FakeStampDsWtihStamps, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub

    Private Function GetFakeStampDsWtihStamps() As Data.DataSet
        Dim FakeStampDs As Data.DataSet = GetFakeStampDs(True, Nothing)

        For Each FakeDr As Data.DataRow In FakeStampDs.Tables(0).Rows
            FakeDr("StampDateTime") = System.DateTime.Now
        Next

        Return FakeStampDs
    End Function

    <Fact>
    Public Sub DuplicateLogTest()
        'create stamp dataset with all relevant stamps received and log dataset with true field value for IsLogDuplicated
        'without IsLogDuplicated field, the return log state should be completed
        'however, IsLogDuplicated field value should cause return log state to be an error
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"logState", "error"},
            {"logType", "current"}
        }
        Dim FakeStampDsWtihStamps As Data.DataSet = GetFakeStampDsWtihStamps()
        Dim FakeLogDs As Data.DataSet = GetFakeLogDs()
        Dim LogStateConfig As Object

        FakeLogDs.Tables(0).Rows(0)("IsLogDuplicated") = True 'only 1 row exists in fake log dataset

        LogStateConfig = GetState(0, GetFakeStampDsWtihStamps, FakeLogDs)

        Assert.Equal(Of Object)(ExpectedRes, LogStateConfig)
    End Sub

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
        DR("Inputs") = RowConfig("Inputs")

        DT.Rows.Add(DR)
    End Sub

End Class
