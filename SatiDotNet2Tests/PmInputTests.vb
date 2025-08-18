Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library

Public Class NumberPmInputTests
    Inherits PmInput

    Private NumberFieldTypeDbValue As Object = "number" 'DB field value for 'number' fieldtype is currently NULL, but it should be 'number'
    Private OutOfScopeExpectedRes As New Dictionary(Of String, Object) From {
        {"state", "outOfScope"},
        {"message", "*CAUTION: OUT OF RANGE*"}
    }
    Dim ValidExpectedRes As New Dictionary(Of String, Object) From {
        {"state", "valid"},
        {"message", ""}
    }

    <Theory>
    <InlineData("e23")>
    <InlineData("")>
    Private Sub InvalidUserInputs(UserInput As String)
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"state", "invalid"},
            {"message", "*ERROR: NOT A NUMBER*"}
        }
        Assert.Equal(Of Dictionary(Of String, Object))(ExpectedRes, ReportValidity(NumberFieldTypeDbValue, String.Empty, UserInput))
    End Sub

    <Theory>
    <InlineData("1")>
    <InlineData("2.2")>
    <InlineData("-7")>
    Private Sub ValidUserInputs(UserInput As String)
        Assert.Equal(Of Dictionary(Of String, Object))(ValidExpectedRes, ReportValidity(NumberFieldTypeDbValue, String.Empty, UserInput))
    End Sub

    <Theory>
    <InlineData("5-10", 4.99)>
    <InlineData("5-10", 11)>
    <InlineData("5.5-7.5", 5)>
    <InlineData("5.5-7.5", 7.51)>
    <InlineData("5 to 10", 4.99)>
    <InlineData("5 to 10", 10.01)>
    <InlineData("-5 to 10", -5.01)>
    <InlineData("-5 to 10", 10.01)>
    <InlineData("-7.55 to -7.45", -7.56)>
    <InlineData("-7.55 to -7.45", -7.44)>
    Private Sub RangeSpanTests(Range As String, UserInput As String)
        Assert.Equal(Of Dictionary(Of String, Object))(OutOfScopeExpectedRes, ReportValidity(NumberFieldTypeDbValue, Range, UserInput))
    End Sub

    <Theory>
    <InlineData("<10", 10)>
    <InlineData("<10", 10.01)>
    <InlineData("<10", 11)>
    Private Sub LessThanTests(Range As String, UserInput As String)
        Assert.Equal(Of Dictionary(Of String, Object))(OutOfScopeExpectedRes, ReportValidity(NumberFieldTypeDbValue, Range, UserInput))
    End Sub

    <Theory>
    <InlineData(">10", 10)>
    <InlineData(">10", 9.99)>
    <InlineData(">10", 3)>
    Private Sub GreaterThanTests(Range As String, UserInput As String)
        Assert.Equal(Of Dictionary(Of String, Object))(OutOfScopeExpectedRes, ReportValidity(NumberFieldTypeDbValue, Range, UserInput))
    End Sub

    <Theory>
    <InlineData(">10")>
    <InlineData("<10")>
    <InlineData("5-10")>
    <InlineData("5.5-7.5")>
    Private Sub AcceptTripleZerosAsValid(Range As String)
        Assert.Equal(Of Dictionary(Of String, Object))(ValidExpectedRes, ReportValidity(NumberFieldTypeDbValue, Range, "000"))
    End Sub

End Class

Public Class DeletePmInputTests
    Inherits PmInput
    Private Security As New Security()
    Private SqlParameters As New SqlParameters()

    <Fact>
    Public Sub PassNullAsArg()
        Assert.False(Boolean.Parse(Delete(Nothing)("Success")))
    End Sub

    <Theory>
    <InlineData(4)>
    <InlineData(238)>
    Public Sub DeleteWithoutSqlExecutionTestCases(LabelKey As String)
        Dim DeleteRes As Dictionary(Of String, String) = Delete(LabelKey, True)
        Dim DeleteHash As New Dictionary(Of String, String) From {
            {"LabelKey", LabelKey}
        }

        Assert.Equal("DELETE FROM [ALTS].[dbo].[T_LogLabel] WHERE [Key]=@LabelKey;", DeleteRes("SqlQuery"))
        Assert.True(SqlParameters.ValidParameterizedValues(DeleteHash, DeleteRes))
    End Sub

    <Theory>
    <InlineData(-1)>
    <InlineData(0)>
    Public Sub DeleteWithSqlExecutionsTestCases(LabelKey As String)
        'sql does NOT complain when a sql query is ran on a record that doesn't exists in a table
        'do just that, to ensure return from function after executing sql delete query is as expected
        Dim DeleteRes As Dictionary(Of String, String) = Delete(LabelKey)
        Dim DeleteHash As New Dictionary(Of String, String) From {
            {"LabelKey", LabelKey}
        }

        Assert.False(DeleteRes.ContainsKey("SqlQuery"))
        Assert.False(DeleteRes.ContainsKey("QueryConfig"))
        Assert.True(Boolean.Parse(DeleteRes("Success")))
    End Sub

End Class