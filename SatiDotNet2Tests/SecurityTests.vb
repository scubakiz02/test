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

End Class