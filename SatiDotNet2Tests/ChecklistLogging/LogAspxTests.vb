Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library
Imports System.Text.Json

Public Class DateValidation
    Dim LogAspx = New LogAspxLibrary()

    <Fact>
    Public Sub ValidDateTest1()
        'ensure empty input is false
        Assert.Equal("*Format Error: MM/YY*", LogAspx.ValidDate(""))
    End Sub

    <Fact>
    Public Sub ValidDateTest2()
        'if letters are included, result is false
        Assert.Equal("*Format Error: MM/YY*", LogAspx.ValidDate("1d/23"))
    End Sub

    <Fact>
    Public Sub ValidDateTest3()
        '5 characters with a '/' as the 3rd, or else false
        Assert.Equal("*Format Error: MM/YY*", LogAspx.ValidDate("08"))
    End Sub

    <Fact>
    Public Sub ValidDateTest4()
        '5 characters with a '/' as the 3rd, or else false
        Assert.Equal("*Format Error: MM/YY*", LogAspx.ValidDate("08/0"))
    End Sub

    <Fact>
    Public Sub ValidDateTest5()
        '5 characters with a '/' as the 3rd, or else false
        Assert.Equal("*Format Error: MM/YY*", LogAspx.ValidDate("08/09/2002"))
    End Sub

    <Fact>
    Public Sub ValidDateTest6()
        'should return true. this is the format the function is looking for (MM/YY)
        Assert.Equal("", LogAspx.ValidDate("08/" & Microsoft.VisualBasic.Right((Today.Year + 10).ToString(), 2)))
    End Sub

    <Fact>
    Public Sub ValidDateTest7()
        'if the date is in the past, return false
        Assert.Equal("*Error: Date is in the past*", LogAspx.ValidDate("08/24"))
    End Sub

    <Fact>
    Public Sub ValidDateTest8()
        'testing today's date in MM/DD/YYYY format should return false
        Assert.Equal("*Format Error: MM/YY*", LogAspx.ValidDate(Today.Date.ToString()))
    End Sub

    <Fact>
    Public Sub ValidDateTest10()
        'testing april of 2054, which should return true. In a previous iteration of ValidDate(), it interpreted this as april of 1954
        Assert.Equal("", LogAspx.ValidDate("04/54"))
    End Sub

    <Fact>
    Public Sub ValidDateTest9()
        'if the date is the current month and year in valid format (MM/YY), it should be true
        Dim CurrYearAs2 As String = Microsoft.VisualBasic.Right(Today.Year.ToString(), 2)
        Dim CurrMonth As String = If(Today.Month < 10, "0" & Today.Month.ToString(), Today.Month.ToString())
        Assert.Equal("", LogAspx.ValidDate(CurrMonth & "/" & CurrYearAs2))
    End Sub
End Class

Public Class ValidateBackColor
    Dim LogAspx = New LogAspxLibrary()

    <Fact>
    Public Sub ReturnTrueTest1()
        Assert.True(LogAspx.ReturnTrue())
    End Sub

    <Fact>
    Public Sub ValidateBackColorTest1()
        'if num of notes > 0, regardless of back color, return true
        Assert.True(LogAspx.ValidateByBackColor(1, "Red"))
    End Sub

    <Fact>
    Public Sub ValidateBackColorTest2()
        'if num of notes is 0, and backcolor is red, return false
        Assert.False(LogAspx.ValidateByBackColor(0, "Red"))
    End Sub

    <Fact>
    Public Sub ValidateBackColorTest3()
        'if num of notes is 0, and backcolor is yellow, return nothing
        Dim Res As Boolean? = LogAspx.ValidateByBackColor(0, "ffe6e600") 'ff at beginning = ##
        Assert.True(If(Res Is Nothing, True, False))
    End Sub

    <Fact>
    Public Sub ValidateBackColorTest4()
        'if backcolor is gray in hex, return true
        Assert.True(LogAspx.ValidateByBackColor(0, "fff5f5f5")) 'ff at beginning = ##
    End Sub

    <Fact>
    Public Sub ValidateBackColorTest5()
        'if backcolor is gray as a string, return true
        Assert.True(LogAspx.ValidateByBackColor(0, "WhiteSmoke"))
    End Sub

End Class
Public Class GetRange
    Dim LogAspx = New LogAspxLibrary()
    Dim T_LogDataDT As New DataTable()
    Dim T_LogDataDR As Data.DataRow
    Dim T_LogLabelDT As New DataTable()
    Dim T_LogLabelDR As Data.DataRow
    Dim KeyFromQueryString As String = "321"
    Dim T_LogLabelRange As String = "10-19"
    Dim T_LogDataRange As String = "10-18"
    Dim LabelKey As Integer = 589
    Dim T_LogDataObject As New Dictionary(Of String, String) From {
        {LabelKey, T_LogDataRange}
    }

    Public Sub New() 'constructor
        T_LogDataDT.Columns.Add("CompleteLog", GetType(Boolean))
        T_LogDataDT.Columns.Add("Ranges", GetType(String))
        T_LogLabelDT.Columns.Add("LabelKey", GetType(Integer))
        T_LogLabelDT.Columns.Add("Range", GetType(String))

        T_LogDataDR = T_LogDataDT.NewRow()
        T_LogLabelDR = T_LogLabelDT.NewRow()

        T_LogDataDR("Ranges") = JsonSerializer.Serialize(Of Dictionary(Of String, String))(T_LogDataObject)
        T_LogLabelDR("LabelKey") = LabelKey
        T_LogLabelDR("Range") = T_LogLabelRange
    End Sub

    <Fact>
    Public Sub GetRange1()
        'if in Log.aspx & T_LogData CompleteLog is true, return range from T_LogData Ranges field value
        T_LogDataDR("CompleteLog") = True
        Assert.Equal(T_LogDataRange, LogAspx.GetRange(KeyFromQueryString, T_LogDataDR, T_LogLabelDR))
    End Sub

    <Fact>
    Public Sub GetRange5()
        'in Log.aspx & T_LogData CompleteLog is true. HOWEVER, Ranges field value holds a null value for the label key, so return an empty string. 
        T_LogDataDR("Ranges") = JsonSerializer.Serialize(Of Dictionary(Of String, String))(New Dictionary(Of String, String) From {
            {LabelKey, Nothing}
        })
        T_LogDataDR("CompleteLog") = True
        Assert.Equal(String.Empty, LogAspx.GetRange(KeyFromQueryString, T_LogDataDR, T_LogLabelDR))
    End Sub

    <Fact>
    Public Sub GetRange2()
        'if in Log.aspx & T_LogData CompleteLog is false, return range from T_LogLabel
        T_LogDataDR("CompleteLog") = False
        Assert.Equal(T_LogLabelRange, LogAspx.GetRange(KeyFromQueryString, T_LogDataDR, T_LogLabelDR))
    End Sub

    <Fact>
    Public Sub GetRange3()
        'if in Log.aspx & T_LogData CompleteLog is false, BUT T_LogLabel range is NULL in DB, return empty string
        T_LogDataDR("CompleteLog") = False
        T_LogLabelDR("Range") = DBNull.Value
        Assert.Equal(String.Empty, LogAspx.GetRange(KeyFromQueryString, T_LogDataDR, T_LogLabelDR))
    End Sub

    <Fact>
    Public Sub GetRange4()
        'if in ChecklistBuilder.aspx (arg 1 is Nothing), return range from T_LogLabel Range field value
        T_LogDataDR = Nothing 'LogDR is never initialized if url does NOT have 'Key' in querystring
        Assert.Equal(T_LogLabelRange, LogAspx.GetRange(Nothing, T_LogDataDR, T_LogLabelDR))
    End Sub

    <Fact>
    Public Sub GetRange6()
        'if, for whatever reason, some abnormal args are passed and an error occurs, return an empty string
        T_LogDataDR = T_LogDataDT.NewRow()
        T_LogLabelDR = T_LogLabelDT.NewRow()
        Assert.Equal(String.Empty, LogAspx.GetRange(KeyFromQueryString, T_LogDataDR, T_LogLabelDR))
    End Sub
End Class

Public Class GetInputs
    Dim LogAspx = New LogAspxLibrary()
    Dim CorrectConfig As New Dictionary(Of Integer, Dictionary(Of String, String))
    Dim CorrectConfigStringified As String
    Dim LogDT As New DataTable()
    Dim LogDR As Data.DataRow
    Private Format As New Format()

    Public Sub New() 'constructor
        'using live data from T_LogData record Key 280
        'Date: "2025-03-16 00:00:00"
        'Inputs: {"575":"A4124","576":"08/25","577":"A4124","578":"08/25","579":"170900000055"} 
        Dim LogDate As String = Format.DateField("2025-03-16 00:00:00")
        Dim LogUser As String = "mark kiser"

        LogDT.Columns.Add("Date", GetType(Date))
        LogDT.Columns.Add("Inputs", GetType(String))
        LogDT.Columns.Add("Operator", GetType(String))

        LogDR = LogDT.NewRow()

        LogDR("Date") = LogDate
        LogDR("Operator") = LogUser

        CorrectConfig(575) = New Dictionary(Of String, String) From {
            {"Date", LogDate},
            {"Operator", LogUser},
            {"Value", "A4124"}
        }
        CorrectConfig(576) = New Dictionary(Of String, String) From {
            {"Date", LogDate},
            {"Operator", LogUser},
            {"Value", "08/25"}
        }
        CorrectConfig(577) = New Dictionary(Of String, String) From {
            {"Date", LogDate},
            {"Operator", LogUser},
            {"Value", "A4124"}
        }
        CorrectConfig(578) = New Dictionary(Of String, String) From {
            {"Date", LogDate},
            {"Operator", LogUser},
            {"Value", "08/25"}
        }
        CorrectConfig(579) = New Dictionary(Of String, String) From {
            {"Date", LogDate},
            {"Operator", LogUser},
            {"Value", "170900000055"}
        }

        CorrectConfigStringified = JsonSerializer.Serialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(CorrectConfig)
        LogDR("Inputs") = CorrectConfigStringified
    End Sub

    Private Function WipeInputsOperators() As String
        For Each kvp As KeyValuePair(Of Integer, Dictionary(Of String, String)) In CorrectConfig
            Dim LabelKey As Integer = kvp.Key
            Dim LabelKeyObject As Dictionary(Of String, String) = kvp.Value
            LabelKeyObject("Operator") = String.Empty
        Next

        Return JsonSerializer.Serialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(CorrectConfig)
    End Function

    <Fact>
    Public Sub GetInputs1()
        'if arg 1 is the correct structure (Dictionary(Of Integer, Dictionary(Of String, String))) with the correct key value pairs (Time, User, Value), then return it
        Assert.Equal(CorrectConfig, LogAspx.GetInputs(LogDR))
    End Sub

    <Fact>
    Public Sub GetInputs3()
        'if arg 1 follows the original structure (Dictionary(Of Integer, String))
        '1) restructure it into the new structure (Dictionary(Of Integer, Dictionary(Of String, String)))
        '2) map the extra key value pairs (operator & date) using data from arg 1
        '3) return the restructured dictionary
        'this test ALSO ensures Date holds the time
        LogDR("Inputs") = "{""575"":""A4124"",""576"":""08/25"",""577"":""A4124"",""578"":""08/25"",""579"":""170900000055""}"
        Assert.Equal(CorrectConfigStringified, JsonSerializer.Serialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(LogAspx.GetInputs(LogDR))) 'stringify return from GetInputs, to ensure the dictinaries are EXACTLY the same
    End Sub

    <Fact>
    Public Sub GetInputs5()
        'same as GetInputs3. except LogDR("Operator") is DBNull
        LogDR("Inputs") = "{""575"":""A4124"",""576"":""08/25"",""577"":""A4124"",""578"":""08/25"",""579"":""170900000055""}"
        LogDR("Operator") = DBNull.Value
        Assert.Equal(WipeInputsOperators(), JsonSerializer.Serialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(LogAspx.GetInputs(LogDR))) 'stringify return from GetInputs, to ensure the dictinaries are EXACTLY the same
    End Sub

    <Fact>
    Public Sub GetInputs4()
        'baseline test for T_LogData Inputs field value with the new format (Dictionary(Of Integer, Dictionary(Of String, String)))
        LogDR("Inputs") = "{""575"":{""Date"":""03/16/2025 12:00:00 AM"",""Operator"":""mark kiser"",""Value"":""A4124""},""576"":{""Date"":""03/16/2025 12:00:00 AM"",""Operator"":""mark kiser"",""Value"":""08/25""},""577"":{""Date"":""03/16/2025 12:00:00 AM"",""Operator"":""mark kiser"",""Value"":""A4124""},""578"":{""Date"":""03/16/2025 12:00:00 AM"",""Operator"":""mark kiser"",""Value"":""08/25""},""579"":{""Date"":""03/16/2025 12:00:00 AM"",""Operator"":""mark kiser"",""Value"":""170900000055""}}"
        Assert.Equal(CorrectConfigStringified, JsonSerializer.Serialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(LogAspx.GetInputs(LogDR))) 'stringify return from GetInputs, to ensure the dictinaries are EXACTLY the same
    End Sub

    <Fact>
    Public Sub GetInputs6()
        'when converting empty json in old format into new format, ensure date is an empty string if value and operator are
        LogDR("Inputs") = "{""575"":"""",""576"":"""",""577"":"""",""578"":"""",""579"":""""}" 'old format
        LogDR("Operator") = String.Empty
        Dim NewFormat As String = "{""575"":{""Date"":"""",""Operator"":"""",""Value"":""""},""576"":{""Date"":"""",""Operator"":"""",""Value"":""""},""577"":{""Date"":"""",""Operator"":"""",""Value"":""""},""578"":{""Date"":"""",""Operator"":"""",""Value"":""""},""579"":{""Date"":"""",""Operator"":"""",""Value"":""""}}"
        Assert.Equal(NewFormat, JsonSerializer.Serialize(Of Dictionary(Of Integer, Dictionary(Of String, String)))(LogAspx.GetInputs(LogDR))) 'stringify return from GetInputs, to ensure the dictinaries are EXACTLY the same
    End Sub

End Class