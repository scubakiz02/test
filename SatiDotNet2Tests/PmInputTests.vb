Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library

Public Class NumberPmInputTests
    Inherits PmInput

    Private NumberFieldTypeDbValue As Object = "number" 'DB field value for 'number' fieldtype is currently NULL, but it should be 'number'
    Private OutOfScopeExpectedRes As New Dictionary(Of String, Object) From {
        {"state", "outOfScope"},
        {"endUserMessage", "*CAUTION: OUT OF RANGE*"}
    }
    Dim ValidExpectedRes As New Dictionary(Of String, Object) From {
        {"state", "valid"},
        {"endUserMessage", ""}
    }

    <Theory>
    <InlineData("e23")>
    <InlineData("")>
    Private Sub InvalidUserInputs(UserInput As String)
        Dim ExpectedRes As New Dictionary(Of String, Object) From {
            {"state", "invalid"},
            {"endUserMessage", "*ERROR: NOT A NUMBER*"}
        }
        Assert.Equal(Of Dictionary(Of String, Object))(ExpectedRes, ReportValidity(NumberFieldTypeDbValue, String.Empty, UserInput))
    End Sub

    <Theory>
    <InlineData("1")>
    <InlineData("2.2")>
    Private Sub ValidUserInputs(UserInput As String)
        Assert.Equal(Of Dictionary(Of String, Object))(ValidExpectedRes, ReportValidity(NumberFieldTypeDbValue, String.Empty, UserInput))
    End Sub

    <Theory>
    <InlineData("5-10", 4.99)>
    <InlineData("5-10", 11)>
    <InlineData("5.5-7.5", 5)>
    <InlineData("5.5-7.5", 7.51)>
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