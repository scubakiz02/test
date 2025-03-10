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