Imports Xunit
Imports System.Data
Imports System.Transactions
Imports SatiDotNet2.Library

' All test classes share [Collection("CofA300mm")] to force sequential execution.
' Without it, xUnit runs classes in parallel, causing SQL Server deadlocks on
' shared tables (GFAAS Data, T_FGI_Boxes, LabelsMade).

''' <summary>
''' Tests for GetCofAData — verifies that per-wafer CofA data is returned with expected
''' geometry and identification columns (Lot, BoxID, Slot, PostCenterThk, TTV, Warp, Bow, TIR, CenterRes).
''' </summary>
<Collection("CofA300mm")>
Public Class CofA_300mm_GetCofADataTests
    Private _cofA As New CofA300mm()
    Private _cartonBox As String = "CB394951"

    ''' <summary>
    ''' Verifies GetCofAData returns a non-empty DataSet containing all required wafer measurement columns.
    ''' </summary>
    <Fact>
    Public Sub GetCofAData_ReturnsDataSet_ForKnownCarton()
        Dim cartonString As String = _cartonBox & Chr(13)
        Dim result As DataSet = _cofA.GetCofAData(cartonString, True, "")
        Assert.NotNull(result)
        Assert.True(result.Tables.Count > 0)
        Assert.True(result.Tables(0).Rows.Count > 0)

        ' Verify expected columns exist
        Assert.True(result.Tables(0).Columns.Contains("Lot"))
        Assert.True(result.Tables(0).Columns.Contains("BoxID"))
        Assert.True(result.Tables(0).Columns.Contains("Slot"))
        Assert.True(result.Tables(0).Columns.Contains("PostCenterThk"))
        Assert.True(result.Tables(0).Columns.Contains("TTV"))
        Assert.True(result.Tables(0).Columns.Contains("Warp"))
        Assert.True(result.Tables(0).Columns.Contains("Bow"))
        Assert.True(result.Tables(0).Columns.Contains("TIR"))
        Assert.True(result.Tables(0).Columns.Contains("CenterRes"))
    End Sub
End Class

''' <summary>
''' Tests for GetCofADataSumary — verifies that aggregate statistics (AVG for thickness,
''' TTV, TIR, resistivity, bow, warp) are returned for a known carton.
''' </summary>
<Collection("CofA300mm")>
Public Class CofA_300mm_GetCofADataSumaryTests
    Private _cofA As New CofA300mm()
    Private _cartonBox As String = "CB394951"

    ''' <summary>
    ''' Verifies GetCofADataSumary returns a non-null DataRow with all expected AVG columns.
    ''' </summary>
    <Fact>
    Public Sub GetCofADataSumary_ReturnsSummaryRow_ForKnownCarton()
        Dim cartonString As String = _cartonBox & Chr(13)
        Dim result As DataRow = _cofA.GetCofADataSumary(cartonString, True)
        Assert.NotNull(result)

        ' Verify summary columns exist (AVG/MIN/MAX/STDEV)
        Assert.NotNull(result("ThickAVG"))
        Assert.NotNull(result("TTVAvg"))
        Assert.NotNull(result("TIRAvg"))
        Assert.NotNull(result("ResAvg"))
        Assert.NotNull(result("BowAvg"))
        Assert.NotNull(result("WarpAvg"))
    End Sub
End Class

''' <summary>
''' Tests for Get300Metals — verifies that all 18 metal element columns are present
''' in the result set for a known wafer box instance.
''' </summary>
<Collection("CofA300mm")>
Public Class CofA_300mm_Get300MetalsTests
    Private _cofA As New CofA300mm()
    Private _instanceNumber As Integer = 555409

    ''' <summary>
    ''' Verifies Get300Metals returns data with all 18 metal columns (Ca through Ag).
    ''' </summary>
    <Fact>
    Public Sub Get300Metals_ReturnsMetalColumns_ForKnownInstance()
        Dim result As DataSet = _cofA.Get300Metals(_instanceNumber.ToString())
        Assert.NotNull(result)
        Assert.True(result.Tables.Count > 0)
        Assert.True(result.Tables(0).Rows.Count > 0)

        ' Verify all 18 metal columns exist
        Dim metals() As String = {"Ca", "Ma", "Ni", "Zn", "Al", "Fe", "Cr", "Cu", "Na", "K", "Co", "Mn", "Mo", "W", "Ti", "V", "Au", "Ag"}
        For Each metal As String In metals
            Assert.True(result.Tables(0).Columns.Contains(metal), $"Missing column: {metal}")
        Next
    End Sub
End Class

''' <summary>
''' Tests for GetCarton300mmMetals — exercises both the cache-hit path (metals already in GFAAS Data)
''' and the cache-miss path (metals computed via Get300Metals + WriteMetals).
''' Cache-miss tests use TransactionScope to roll back writes.
''' </summary>
<Collection("CofA300mm")>
Public Class CofA_300mm_GetCarton300mmMetalsTests
    Private _cofA As New CofA300mm()
    Private _instanceNumber As Integer = 555409

    ''' <summary>
    ''' Verifies that cached metals data (already in GFAAS Data) is returned with at least 2 rows.
    ''' </summary>
    <Fact>
    Public Sub GetCarton300mmMetals_ReturnsCachedMetals_WhenAlreadyInDatabase()
        Dim result As DataSet = _cofA.GetCarton300mmMetals(_instanceNumber.ToString())
        Assert.NotNull(result)
        Assert.True(result.Tables(0).Rows.Count > 1, "Expected at least 2 metals rows (cached)")
    End Sub

    ''' <summary>
    ''' Exercises the cache-miss path: metals are computed and written, then rolled back via TransactionScope.
    ''' </summary>
    <Fact>
    Public Sub GetCarton300mmMetals_ComputesAndWritesMetals_WhenNotCached()
        ' This test exercises the cache-miss path which calls Get300Metals + WriteMetals.
        ' Use TransactionScope to rollback the write.
        Using scope As New TransactionScope()
            Dim result As DataSet = _cofA.GetCarton300mmMetals("578099")
            Assert.NotNull(result)
            Assert.True(result.Tables(0).Rows.Count > 0, "Expected metals data to be computed")
        End Using
    End Sub
End Class

''' <summary>
''' Integration tests for WriteMetals behavior (exercised indirectly via GetCarton300mmMetals on an
''' uncached instance). Validates row count, metadata fields, column presence, and value floor (>= 0.01).
''' All tests use TransactionScope to roll back database writes.
''' </summary>
<Collection("CofA300mm")>
Public Class CofA_300mm_WriteMetals_MetalValues
    Private _cofA As New CofA300mm()
    Private _uncachedInstance As String = "578099"

    ''' <summary>
    ''' Helper that triggers the cache-miss path to exercise WriteMetals.
    ''' </summary>
    Private Function GetWriteMetalsResult() As DataSet
        Return _cofA.GetCarton300mmMetals(_uncachedInstance)
    End Function

    ''' <summary>Verifies WriteMetals produces exactly 2 rows (one per source sample).</summary>
    <Fact>
    Public Sub WriteMetals_ReturnsExactlyTwoRows()
        Using scope As New TransactionScope()
            Dim result As DataSet = GetWriteMetalsResult()
            Assert.NotNull(result)
            Assert.Equal(2, result.Tables(0).Rows.Count)
        End Using
    End Sub

    ''' <summary>Verifies each row has Source="SATI", Test Type="at/cm²", Location="Prescott", and a non-empty Idenyification.</summary>
    <Fact>
    Public Sub WriteMetals_SetsMetadataCorrectly()
        Using scope As New TransactionScope()
            Dim result As DataSet = GetWriteMetalsResult()
            For Each row As DataRow In result.Tables(0).Rows
                Assert.Equal("SATI", row("Source").ToString())
                Assert.Equal("at/cm²", row("Test Type").ToString())
                Assert.Equal("Prescott", row("Location").ToString())
                Assert.False(IsDBNull(row("Idenyification")), "Idenyification should not be null")
                Assert.True(row("Idenyification").ToString().Length > 0, "Idenyification should not be empty")
            Next
        End Using
    End Sub

    ''' <summary>Verifies all 18 metal element columns exist in the result.</summary>
    <Fact>
    Public Sub WriteMetals_AllEighteenMetalColumnsPresent()
        Using scope As New TransactionScope()
            Dim result As DataSet = GetWriteMetalsResult()
            Dim metals() As String = {"Ca", "Ma", "Ni", "Zn", "Al", "Fe", "Cr", "Cu", "Na", "K", "Co", "Mn", "Mo", "W", "Ti", "V", "Au", "Ag"}
            For Each metal As String In metals
                Assert.True(result.Tables(0).Columns.Contains(metal), $"Missing column: {metal}")
            Next
        End Using
    End Sub

    ''' <summary>Verifies every metal value in every row is >= 0.01 (the minimum floor applied by WriteMetals).</summary>
    <Fact>
    Public Sub WriteMetals_MetalValuesAtLeast001()
        Using scope As New TransactionScope()
            Dim result As DataSet = GetWriteMetalsResult()
            Dim metals() As String = {"Ca", "Ma", "Ni", "Zn", "Al", "Fe", "Cr", "Cu", "Na", "K", "Co", "Mn", "Mo", "W", "Ti", "V", "Au", "Ag"}
            For Each row As DataRow In result.Tables(0).Rows
                For Each metal As String In metals
                    Assert.False(IsDBNull(row(metal)), $"{metal} should not be DBNull")
                    Dim value As Double = CDbl(row(metal))
                    Assert.True(value >= 0.01, $"{metal} value {value} is below minimum 0.01")
                Next
            Next
        End Using
    End Sub
End Class
