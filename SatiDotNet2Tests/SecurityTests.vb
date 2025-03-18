Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library

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
            {"value", "1"},
            {"typeOf", "int"}
        }
        Dim DS As Data.DataSet = Security2.GetMyDataSetParamQuery("SELECT * FROM [SatiTest].[dbo].[T_LogSqlInjectionPrevention] WHERE id=@id", QueryObject)
        Dim DR As Data.DataRow = DS.Tables(0).Rows(0)
        Assert.True(If(DR("username") = "jork-frol-pliy" AndAlso DR("password") = "jxCv7$LEM!nuWcUb" AndAlso DR("fullname") = "john doe", True, False))
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