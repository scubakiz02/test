Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library

Public Class LogAspxTests
    Dim LogAspx = New LogAspxLibrary()

    <Fact>
    Public Sub ReturnTrueTest1()
        Assert.True(LogAspx.ReturnTrue())
    End Sub

    <Fact>
    Public Sub ValidateBackColorTest1()
        'if num of notes > 0, regardless of back color, return true
        Assert.True(LogAspx.ValidateByBackColor(1, "red"))
    End Sub

    <Fact>
    Public Sub ValidateBackColorTest2()
        'if num of notes is 0, and backcolor is red, return false
        Assert.False(LogAspx.ValidateByBackColor(0, "red"))
    End Sub

    <Fact>
    Public Sub ValidateBackColorTest3()
        'if num of notes is 0, and backcolor is yellow, return nothing
        Dim Res As Boolean? = LogAspx.ValidateByBackColor(0, "#E6E600")
        Assert.True(If(Res Is Nothing, True, False))
    End Sub

    <Fact>
    Public Sub ValidateBackColorTest4()
        'if backcolor is nothing (gray), return true
        Assert.True(LogAspx.ValidateByBackColor(0, Nothing))
    End Sub


End Class