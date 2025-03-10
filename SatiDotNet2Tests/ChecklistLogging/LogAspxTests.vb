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







    <Fact>
    Public Sub GetStatusBoardRoleTest1()
        'if Where does not match today, return should be 'admin'
        Assert.Equal("admin", LogAspx.GetStatusBoardRole("Full", "Production", "03/09/2025"))
    End Sub

    <Fact>
    Public Sub GetStatusBoardRoleTest2()
        'if view is full & department is maintenance, return should be 'FMManagerApproval'
        Assert.Equal("FMManagerApproval", LogAspx.GetStatusBoardRole("Full", "Maintenance", Today.Date))
    End Sub

    <Fact>
    Public Sub GetStatusBoardRoleTest3()
        'if view is full & department is production, return should be 'PC'
        Assert.Equal("PC", LogAspx.GetStatusBoardRole("Full", "Production", Today.Date))
    End Sub

    <Fact>
    Public Sub GetStatusBoardRoleTest4()
        'if view is focus & department is production, return should be nothing
        Assert.Equal(Nothing, LogAspx.GetStatusBoardRole("Focus", "Production", Today.Date))
    End Sub

    <Fact>
    Public Sub GetStatusBoardRoleTest5()
        'if view is focus & department is all, return should be 'Maintenance'
        Assert.Equal("Maintenance", LogAspx.GetStatusBoardRole("Focus", "All", Today.Date))
    End Sub

    <Fact>
    Public Sub GetStatusBoardRoleTest6()
        'if view is focus & department is Maintenance, return should be 'Maintenance'
        Assert.Equal("Maintenance", LogAspx.GetStatusBoardRole("Focus", "Maintenance", Today.Date))
    End Sub


End Class