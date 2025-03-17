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