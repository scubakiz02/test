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
        Dim DS As Data.DataSet = Security2.GetMyDataSetParamQuery("SELECT * FROM [SatiTest].[dbo].[T_LogSqlInjectionPrevention]", New Dictionary(Of String, String))
        Assert.True(If(DS.Tables.Count > 0, True, False))
    End Sub

    <Fact>
    Public Sub GetMyDataSetParamQuery2()
        'executing sql query with table that does NOT exist
        Dim DS As Data.DataSet = Security2.GetMyDataSetParamQuery("SELECT * FROM [SatiTest].[dbo].[T_LogjectionPrevention]", New Dictionary(Of String, String))
        Assert.Equal(Nothing, DS)
    End Sub

    '<Fact>
    'Public Sub GetMyDataSetParamQuery3()
    '    'executing sql query with 1 condition at end
    '    Dim QueryObject As New SortedDictionary(Of Integer, Dictionary(Of String, String))
    '    QueryObject(0) = New Dictionary(Of String, String) From {
    '        {"query", "SELECT * FROM [SatiTest].[dbo].[T_LogSqlInjectionPrevention] WHERE username=@username"},
    '        {"paramVar", "@username"},
    '        {"param", "jork-frol-pliy"},
    '        {"type", "VarChar"}
    '    }
    '    Dim DS As Data.DataSet = Security2.GetMyDataSetParamQuery(QueryObject)
    '    Dim DR As Data.DataRow
    '    Assert.True(If(DS.Tables.Count > 0, True, False))
    'End Sub
End Class