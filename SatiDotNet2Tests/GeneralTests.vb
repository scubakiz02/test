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

        InsertIntoQuerySuccess = Security.ExecuteSqlParamQuery("INSERT INTO [SatiTest].[dbo].[T_LogSqlInjectionPrevention] (username, password, fullname, willitnull) VALUES (@username, @password, @fullname, 'not null')", QueryObject)
        DeleteQuerySuccess = Security.ExecuteSqlParamQuery("DELETE FROM [SatiTest].[dbo].[T_LogSqlInjectionPrevention] WHERE username=@username AND password=@password AND fullname=@fullname", QueryObject)

        Assert.True(If(InsertIntoQuerySuccess AndAlso DeleteQuerySuccess, True, False))
    End Sub

    <Fact>
    Public Sub ExecuteSqlParamQuery5()
        'ensure you can make a non-null field value null in DB
        Dim QueryObject As New Dictionary(Of String, Dictionary(Of String, String))
        QueryObject("@id") = New Dictionary(Of String, String) From {
            {"value", "3"},
            {"typeOf", "int"}
        }
        QueryObject("@null") = New Dictionary(Of String, String) From {
            {"value", Nothing},
            {"typeOf", "int"}
        }

        Assert.True(Security.ExecuteSqlParamQuery("UPDATE [SatiTest].[dbo].[T_LogSqlInjectionPrevention] SET willitnull=@null WHERE id=@id", QueryObject))
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

        'jork-frol-pliy is the 'username' field value for id 1, which should NOT be in the result. SxhNFEsp$A!m7Bx4 is the password field value for record with id of 3, which should be in the result
        Assert.True(If(Res.Contains("jork-frol-pliy") = False AndAlso Res.Contains("SxhNFEsp$A!m7Bx4"), True, False))
    End Sub
End Class

Public Class StripIllegalFileSysCharsTests
    Dim Security = New Security()

    <Fact>
    Public Sub StripIllegalFileSysChars1()
        'baseline test
        Assert.Equal("ADE P1 Presort Monthly\Month of 03-2025", Security.StripIllegalFileSysChars("ADE P1 Presort Monthly", "Month of 03-2025"))
    End Sub

    <Fact>
    Public Sub StripIllegalFileSysChars2()
        'ensure single quote ' char is stripped for arg 1
        Assert.Equal("DI WATER DAILY\03-12-2025", Security.StripIllegalFileSysChars("DI' WATER DAILY'", "03-12-2025"))
    End Sub

    <Fact>
    Public Sub StripIllegalFileSysChars3()
        'ensure # char is stripped for arg 1
        Assert.Equal("R.O. Daily\03-06-2025", Security.StripIllegalFileSysChars("R.O. #Daily", "03-06-2025"))
    End Sub

    <Fact>
    Public Sub StripIllegalFileSysChars4()
        'ensure double quotes " char is stripped for arg 1
        Assert.Equal("R.O. Daily\03-06-2025", Security.StripIllegalFileSysChars("R.O."" Daily""", "03-06-2025"))
    End Sub

    <Fact>
    Public Sub StripIllegalFileSysChars5()
        'ensure slashes (backward & forward) are stripped for arg 1
        Assert.Equal("R.O. Daily\03-06-2025", Security.StripIllegalFileSysChars("R.O.\ Daily/", "03-06-2025"))
    End Sub

    <Fact>
    Public Sub StripIllegalFileSysChars6()
        'ensure colon char " is stripped for arg 1
        Assert.Equal("R.O. Daily\03-06-2025", Security.StripIllegalFileSysChars("R.O.: Daily", "03-06-2025"))
    End Sub

    <Fact>
    Public Sub StripIllegalFileSysChars7()
        'problem child testcase for arg 1
        Assert.Equal("dummy checklist\Month of 03-2025", Security.StripIllegalFileSysChars("dummy ""checklist""", "Month of 03-2025"))
    End Sub

    <Fact>
    Public Sub StripIllegalFileSysChars8()
        'ensure slashes (/\) are replaced with '-' char in arg 2
        Assert.Equal("dummy checklist\Month of 03-2025", Security.StripIllegalFileSysChars("dummy ""checklist""", "Month of 03/2025"))
    End Sub

    <Fact>
    Public Sub StripIllegalFileSysChars9()
        'ensure slashes (/\) are replaced with '-' char in arg 2
        Assert.Equal("dummy checklist\Month of 03-2025", Security.StripIllegalFileSysChars("dummy ""checklist""", "Month of 03\2025"))
    End Sub
End Class


Public Class GetStatusBoardRole
    Dim Security = New Security()

    <Fact>
    Public Sub GetStatusBoardRoleTest1()
        'if Where does not match today, return should be 'admin'
        Assert.Equal(New String() {"admin"}, Security.GetStatusBoardRole("Full", "Production", "03/09/2025"))
    End Sub

    <Fact>
    Public Sub GetStatusBoardRoleTest2()
        'if view is full & department is maintenance, return should be 'FMManagerApproval'
        Assert.Equal(New String() {"FMManagerApproval", "QSHEManagerApproval"}, Security.GetStatusBoardRole("Full", "Maintenance", Today.Date))
    End Sub

    <Fact>
    Public Sub GetStatusBoardRoleTest3()
        'if view is full & department is production, return should be 'PC'
        Assert.Equal(New String() {"PC"}, Security.GetStatusBoardRole("Full", "Production", Today.Date))
    End Sub

    <Fact>
    Public Sub GetStatusBoardRoleTest4()
        'if view is focus & department is production, return should be nothing
        Assert.Equal(New String() {Nothing}, Security.GetStatusBoardRole("Focus", "Production", Today.Date))
    End Sub

    <Fact>
    Public Sub GetStatusBoardRoleTest5()
        'if view is focus & department is all, return should be 'Maintenance'
        Assert.Equal(New String() {"Maintenance"}, Security.GetStatusBoardRole("Focus", "All", Today.Date))
    End Sub

    <Fact>
    Public Sub GetStatusBoardRoleTest6()
        'if view is focus & department is Maintenance, return should be 'Maintenance'
        Assert.Equal(New String() {"Maintenance"}, Security.GetStatusBoardRole("Focus", "Maintenance", Today.Date))
    End Sub

End Class

Public Class GetDepartmentTests
    Dim BrettTeets = New SatiUser("Brett Teets") 'Maintenance
    Dim SzymonTyburek = New SatiUser("Szymon Tyburek") 'admin
    Dim SzymonTyburekLowered = New SatiUser("szymon tyburek") 'admin
    Dim AndySoto = New SatiUser("Andy Soto") 'Production
    Dim SungJinwoo = New SatiUser("Sung Jinwoo") 'not a real user. if you don't recognize this name, watch 'Solo Leveling'. Very good anime.

    Dim T_LogDepartmentProdKey As Integer = 1
    Dim T_LogDepartmentMaintKey As Integer = 2

    <Fact>
    Public Sub GetDepartment1()
        'if user has admin role, return 'All'
        Assert.Equal("All", SzymonTyburek.GetDepartment())
    End Sub

    <Fact>
    Public Sub GetDepartment2()
        'same as GetDepartment1, but the name passed to the constructor is lowercased
        Assert.Equal("All", SzymonTyburekLowered.GetDepartment())
    End Sub

    <Fact>
    Public Sub GetDepartment3()
        'if user has PC role, return 'Production'
        Assert.Equal("Production", AndySoto.GetDepartment())
    End Sub

    <Fact>
    Public Sub GetDepartment4()
        'if user has a maintenance manager role (F&M or QSHE), return 'Maintenance'
        Assert.Equal("Maintenance", BrettTeets.GetDepartment())
    End Sub

    <Fact>
    Public Sub GetDepartment5()
        'if user does NOT exist, expect Nothing
        Assert.Equal(Nothing, SungJinwoo.GetDepartment())
    End Sub
End Class

Public Class GetDepartmentKeyTests
    Dim BrettTeets = New SatiUser("Brett Teets") 'Maintenance
    Dim SzymonTyburek = New SatiUser("Szymon Tyburek") 'admin
    Dim SzymonTyburekLowered = New SatiUser("szymon tyburek") 'admin
    Dim AndySoto = New SatiUser("Andy Soto") 'Production
    Dim SungJinwoo = New SatiUser("Sung Jinwoo") 'not a real user. if you don't recognize this name, watch 'Solo Leveling'. Very good anime.

    Dim T_LogDepartmentProdKey As Integer = 1
    Dim T_LogDepartmentMaintKey As Integer = 2

    <Fact>
    Public Sub GetDepartmentKey1()
        'if user does NOT exist, expect Nothing
        Assert.Equal(Nothing, SungJinwoo.GetDepartmentKey())
    End Sub

    <Fact>
    Public Sub GetDepartmentKey2()
        'if user has admin role, return Nothing
        Assert.Equal(Nothing, SzymonTyburek.GetDepartmentKey())
    End Sub

    <Fact>
    Public Sub GetDepartmentKey3()
        'if user has PC role, return 'Production'
        Assert.Equal(T_LogDepartmentProdKey, AndySoto.GetDepartmentKey())
    End Sub

    <Fact>
    Public Sub GetDepartmentKey4()
        'if user has a maintenance manager role (F&M or QSHE), return 'Maintenance'
        Assert.Equal(T_LogDepartmentMaintKey, BrettTeets.GetDepartmentKey())
    End Sub

End Class
'
Public Class AspWebpageClassTests
    Dim Url As String = "/ChecklistLogging/ChecklistReport.aspx"
    Dim ChecklistReport As New AspWebpage(Url, New List(Of String) From {"Area", "StartDate", "EndDate", "PageIdx"})

    <Fact>
    Public Sub ConstructorTest1()
        'instantiate w/o adding querystring values. Should return arg 1 passed to constructor
        Assert.Equal(Url, ChecklistReport.GetUrl())
    End Sub

    <Fact>
    Public Sub ConstructorTest2()
        'set url with 1 querystring key/value pair. Should return url with querystring value
        ChecklistReport.SetUrl("Area", "45")
        Assert.Equal(Url & "?Area=45", ChecklistReport.GetUrl())
    End Sub

    <Fact>
    Public Sub ConstructorTest3()
        'set url with 1 querystring key/value pair, but the value is Nothing. Should return url 
        ChecklistReport.SetUrl("Area", Nothing)
        Assert.Equal(Url, ChecklistReport.GetUrl())
    End Sub

    <Fact>
    Public Sub ConstructorTest4()
        'set url with several querystring key/value pairs. Should return url with querystring values that are not nothing
        ChecklistReport.SetUrl("Area", "45")
        ChecklistReport.SetUrl("PageIdx", "3")
        ChecklistReport.SetUrl("StartDate", Nothing)
        Assert.Equal(Url & "?Area=45&PageIdx=3", ChecklistReport.GetUrl())
    End Sub


    <Fact>
    Public Sub ConstructorTest5()
        'set several url querystring key/value pairs and ensure previous querystring key/value pairs are overwritten if the situation calls for it
        ChecklistReport.SetUrl("Area", "45")
        ChecklistReport.SetUrl("PageIdx", "3")
        ChecklistReport.SetUrl("Area", "48")
        Assert.Equal(Url & "?Area=48&PageIdx=3", ChecklistReport.GetUrl())
    End Sub
End Class


Public Class FormatDateTests
    Private Format As New Format()

    <Fact>
    Public Sub FormateDateTest1()
        Assert.Equal("03/25/2025 12:00:00 AM", Format.DateField("03/25/2025"))
    End Sub

    <Fact>
    Public Sub FormateDateTest2()
        Assert.Equal("03/31/2025 12:00:00 AM", Format.DateField("03-31-2025"))
    End Sub

    <Fact>
    Public Sub FormateDateTest3()
        Assert.Equal("03/31/2025 12:00:00 AM", Format.DateField("2025-03-31"))
    End Sub

    <Fact>
    Public Sub FormateDateTest4()
        Assert.Equal("03/31/2025 12:00:00 AM", Format.DateField("2025-03-31 00:00:00"))
    End Sub

    <Fact>
    Public Sub FormateDateTest5()
        Assert.Equal("03/26/2025 03:43:55 PM", Format.DateField("3/26/2025 3:43:55 PM"))
    End Sub

    <Fact>
    Public Sub FormateDateTest6()
        Assert.Equal("03/27/2025 10:53:19 AM", Format.DateField("3/27/2025 10:53:19 AM"))
    End Sub

End Class