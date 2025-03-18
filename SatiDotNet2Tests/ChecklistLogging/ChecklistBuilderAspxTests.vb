Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library

Public Class ModifyLabelOrderTests
    Dim LogAspx = New ChecklistBuilderAspxLibrary()
    Dim Security = New Security()

    'USING NITROGEN DAILY AS SAMPLE CHECKLIST. IF THE LABEL ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
    <Fact>
    Public Sub ModifyLabelOrder1()
        'moving label 1 up on Nitrogen Daily checklist
        Assert.Equal("", LogAspx.ModifyLabelOrder("388", "up"))
    End Sub

    <Fact>
    Public Sub ModifyLabelOrder2()
        'moving label 2 up on Nitrogen Daily checklist
        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=1 WHERE [Key]=389; UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=2 WHERE [Key]=388", LogAspx.ModifyLabelOrder("389", "up"))
    End Sub

    <Fact>
    Public Sub ModifyLabelOrder3()
        'moving label 3 down on Nitrogen Daily checklist
        Assert.Equal("", LogAspx.ModifyLabelOrder("390", "down"))
    End Sub

    <Fact>
    Public Sub ModifyLabelOrder4()
        'moving label 2 down on Nitrogen Daily checklist
        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=3 WHERE [Key]=389; UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=2 WHERE [Key]=390", LogAspx.ModifyLabelOrder("389", "down"))
    End Sub
    'USING NITROGEN DAILY AS SAMPLE CHECKLIST. IF THE LABEL ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
End Class