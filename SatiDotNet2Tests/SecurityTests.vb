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

End Class