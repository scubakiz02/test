Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library
Imports System.Threading

Public Class SecurityTests
    Dim Security = New Security()

    <Fact>
    Public Sub ReturnTrueTest1()
        Assert.True(Security.ReturnTrue())
    End Sub

    <Fact>
    Public Sub NoSqlInjectionTest1()
        Assert.True(Security.NoSqlInjection(""))
    End Sub

    'testing against destructive sql commands
    <Fact>
    Public Sub NoSqlInjectionTest2()
        Assert.False(Security.NoSqlInjection("DROP TABLE DummyTable"))
    End Sub

    <Fact>
    Public Sub NoSqlInjectionTest3()
        Assert.False(Security.NoSqlInjection("drop table DummyTable"))
    End Sub

    <Fact>
    Public Sub NoSqlInjectionTest4()
        Assert.False(Security.NoSqlInjection("select * from DummyTable"))
    End Sub
    'testing against destructive sql commands

    'testing against SQL Injection based on int=int
    '<Fact>
    'Public Sub NoSqlInjectionTest5()
    '    'SELECT UserId, Name, Password FROM Users WHERE UserId = 105 or 1=1;
    '    Assert.False(Security.NoSqlInjection("105 OR 1=1"))
    'End Sub

    '<Fact>
    'Public Sub NoSqlInjectionTest6()
    '    'SELECT UserId, Name, Password FROM Users WHERE UserId = 105 or 1=1;
    '    Assert.False(Security.NoSqlInjection("105 or 4279=4279"))
    'End Sub
    'testing against SQL Injection based on int=int
End Class

Public Class ExecuteSqlParamQueryTests
    Dim Security = New Security()

    <Fact>
    Public Sub ExecuteSqlParamQuery1()
        'blank sql statement, should return false
        Assert.False(Security.ExecuteSqlParamQuery("", New Dictionary(Of String, Dictionary(Of String, String))))
    End Sub

    <Fact>
    Public Sub ExecuteSqlParamQuery2()
        'update query should return true upon successful execution
        Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))
        QueryObject("@id") = New Dictionary(Of String, String) From {
            {"value", "4"},
            {"typeOf", "int"}
        }
        QueryObject("@password") = New Dictionary(Of String, String) From {
            {"value", "p9u3&o58W9LDa-efUdrL"},
            {"typeOf", "string"}
        }

        Assert.True(Security.ExecuteSqlParamQuery("UPDATE [SatiTest].[dbo].[T_LogSqlInjectionPrevention] SET password=@password WHERE id=@id", QueryObject))
    End Sub

    <Fact>
    Public Sub ExecuteSqlParamQuery3()
        'update query to return to reverse update done via ExecuteSqlParamQuery2. should return true upon successful execution.
        'IF THIS TEST FAILS, TESTS IN GetMyDataSetParamQueryTests CLASS WILL FAIL AS WELL, B/C THEY TEST THE RECORD WITH id VALUE OF 4, WHICH IS THE RECORD BEING REVERTED BACK TO ITS ORIGINAL VALUE HERE AFTER MODIFICATION IN ExecuteSqlParamQuery2
        Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))
        QueryObject("@id") = New Dictionary(Of String, String) From {
            {"value", "4"},
            {"typeOf", "int"}
        }
        QueryObject("@password") = New Dictionary(Of String, String) From {
            {"value", "R)y+j%Lg28petjgN"},
            {"typeOf", "string"}
        }

        Assert.True(Security.ExecuteSqlParamQuery("UPDATE [SatiTest].[dbo].[T_LogSqlInjectionPrevention] SET password=@password WHERE id=@id", QueryObject))
    End Sub

    <Fact>
    Public Sub ExecuteSqlParamQuery4()
        Dim InsertIntoQuerySuccess As Boolean
        Dim DeleteQuerySuccess As Boolean

        'insert into and delete query. should return true upon successful execution of both
        'IF THIS UNIT TEST FAILS, GetMyDataSetParamQuery1 WILL ALSO FAIL, B/C IT TESTS THE TABLE FOR 4 TOTAL RECORDS, AND THE # OF RECORDS IN THE TABLE WILL NOT BE 4 IF THE DELETE QUERY FAILS
        Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))
        QueryObject("@username") = New Dictionary(Of String, String) From {
            {"value", "cbacon"},
            {"typeOf", "string"}
        }
        QueryObject("@password") = New Dictionary(Of String, String) From {
            {"value", "0iJUN+*ini@et+YoF8yI"},
            {"typeOf", "string"}
        }
        QueryObject("@fullname") = New Dictionary(Of String, String) From {
            {"value", "Chris P. Bacon"},
            {"typeOf", "string"}
        }

        InsertIntoQuerySuccess = Security.ExecuteSqlParamQuery("INSERT INTO [SatiTest].[dbo].[T_LogSqlInjectionPrevention] VALUES (@username, @password, @fullname)", QueryObject)
        DeleteQuerySuccess = Security.ExecuteSqlParamQuery("DELETE FROM [SatiTest].[dbo].[T_LogSqlInjectionPrevention] WHERE username=@username AND password=@password AND fullname=@fullname", QueryObject)

        Assert.True(If(InsertIntoQuerySuccess AndAlso DeleteQuerySuccess, True, False))
    End Sub
End Class

Public Class GetMyDataSetParamQueryTests
    Dim Security2 = New Security()

    <Fact>
    Public Sub ReturnTrueTest1()
        Assert.True(Security2.ReturnTrue())
    End Sub

    <Fact>
    Public Sub GetMyDataSetParamQuery1()
        'executing sql query with no parameterized values
        Dim DS As Data.DataSet = Security2.GetMyDataSetParamQuery("SELECT * FROM [SatiTest].[dbo].[T_LogSqlInjectionPrevention]", New Dictionary(Of String, Dictionary(Of String, String)))
        Assert.True(If(DS.Tables(0).Rows.Count = 4, True, False))
    End Sub

    <Fact>
    Public Sub GetMyDataSetParamQuery2()
        'executing sql query with table that does NOT exist
        Dim DS As Data.DataSet = Security2.GetMyDataSetParamQuery("SELECT * FROM [SatiTest].[dbo].[T_LogjectionPrevention]", New Dictionary(Of String, Dictionary(Of String, String)))
        Assert.Equal(Nothing, DS)
    End Sub

    <Fact>
    Public Sub GetMyDataSetParamQuery3()
        'executing sql query with 1 parameterized value at end of query
        Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))
        QueryObject("@username") = New Dictionary(Of String, String) From {
            {"value", "jork-frol-pliy"},
            {"typeOf", "string"}
        }
        Dim DS As Data.DataSet = Security2.GetMyDataSetParamQuery("SELECT * FROM [SatiTest].[dbo].[T_LogSqlInjectionPrevention] WHERE username=@username", QueryObject)
        Dim DR As Data.DataRow = DS.Tables(0).Rows(0)
        Assert.True(If(DR("username") = "jork-frol-pliy" AndAlso DR("password") = "jxCv7$LEM!nuWcUb" AndAlso DR("fullname") = "john doe", True, False))
    End Sub

    <Fact>
    Public Sub GetMyDataSetParamQuery4()
        'executing sql query with 1 parameterized value at end of query
        Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))
        QueryObject("@id") = New Dictionary(Of String, String) From {
            {"value", "4"},
            {"typeOf", "int"}
        }
        Dim DS As Data.DataSet = Security2.GetMyDataSetParamQuery("SELECT * FROM [SatiTest].[dbo].[T_LogSqlInjectionPrevention] WHERE id=@id", QueryObject)
        Dim DR As Data.DataRow = DS.Tables(0).Rows(0)
        Assert.True(If(DR("username") = "benk-sef-rhid" AndAlso DR("password") = "R)y+j%Lg28petjgN" AndAlso DR("fullname") = "tim hughes", True, False))
    End Sub

    <Fact>
    Public Sub GetMyDataSetParamQuery5()
        'executing sql query with several parameterized values
        Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))
        QueryObject("@username") = New Dictionary(Of String, String) From {
            {"value", "seck-hor-zup"},
            {"typeOf", "string"}
        }
        QueryObject("@fullname") = New Dictionary(Of String, String) From {
            {"value", "karen smith"},
            {"typeOf", "string"}
        }
        Dim DS As Data.DataSet = Security2.GetMyDataSetParamQuery("SELECT password FROM [SatiTest].[dbo].[T_LogSqlInjectionPrevention] WHERE username=@username AND fullname=@fullname", QueryObject)
        Dim DR As Data.DataRow = DS.Tables(0).Rows(0)
        Assert.True(If(DR("password") = "zcKbRwe+5Nk9k&gY", True, False))
    End Sub

    <Fact>
    Public Sub GetMyDataSetParamQuery6()
        'executing sql query that returns several rows
        Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))
        QueryObject("@id") = New Dictionary(Of String, String) From {
            {"value", "2"},
            {"typeOf", "int"}
        }
        Dim DS As Data.DataSet = Security2.GetMyDataSetParamQuery("SELECT * FROM [SatiTest].[dbo].[T_LogSqlInjectionPrevention] WHERE id > @id", QueryObject)
        Dim DR As Data.DataRow
        Dim Res As New List(Of String)

        For I As Integer = 0 To DS.Tables(0).Rows.Count - 1
            DR = DS.Tables(0).Rows(I)

            Res.Add(DR("id"))
            Res.Add(DR("username"))
            Res.Add(DR("password"))
            Res.Add(DR("fullname"))
        Next

        'jork-frol-pliy is the 'username' field value for id 1, which should NOT be in the result. R)y+j%Lg28petjgN is the password field value for record with id of 4, which should be in the result
        Assert.True(If(Res.Contains("jork-frol-pliy") = False AndAlso Res.Contains("R)y+j%Lg28petjgN"), True, False))
    End Sub
End Class

Public Class StripIllegalFileSysCharsTests
    Dim Security = New Security()

    'Dim AreaRegexed As String = Regex.Replace(DR("Area"), "[:#]'\""", "")
    'Dim DatePeriod As String = Security.GetSingleDbField("Select DatePeriod FROM " & DR("SqlFunc") & "(@AreaKey, @SqlFunc2ndArg, @Date)", QueryConfig, "DatePeriod")
    '    Directory = Path.Combine(AreaRegexed, DatePeriod).Replace("/", "-")

    <Fact>
    Public Sub StripIllegalFileSysChars1()
        'baseline test
        Assert.Equal("", Security.StripIllegalFileSysChars(""))
        'Assert.Equal("ADE P1 Presort Monthly/Month of 03-2025", Security.StripIllegalFileSysChars("ADE P1 Presort Monthly", "Month of 03-2025"))
    End Sub

    <Fact>
    Public Sub StripIllegalFileSysChars2()
        'ensure single quote ' char is stripped
        Assert.Equal("", Security.StripIllegalFileSysChars("'"))
    End Sub

    <Fact>
    Public Sub StripIllegalFileSysChars3()
        'ensure # char is stripped
        Assert.Equal("", Security.StripIllegalFileSysChars("#"))
    End Sub

    <Fact>
    Public Sub StripIllegalFileSysChars4()
        'ensure double quotes " char is stripped
        Assert.Equal("", Security.StripIllegalFileSysChars(""""))
    End Sub

    <Fact>
    Public Sub StripIllegalFileSysChars5()
        'ensure slashes (backward & forward) are NOT stripped
        Assert.Equal("/\", Security.StripIllegalFileSysChars("/\"))
    End Sub

    <Fact>
    Public Sub StripIllegalFileSysChars6()
        'ensure colon char " is stripped
        Assert.Equal("", Security.StripIllegalFileSysChars(":"))
    End Sub

    <Fact>
    Public Sub StripIllegalFileSysChars7()
        'test with a string that has other chars
        Assert.Equal("dummy checklist", Security.StripIllegalFileSysChars("dummy ""checklist"""))
    End Sub
End Class
