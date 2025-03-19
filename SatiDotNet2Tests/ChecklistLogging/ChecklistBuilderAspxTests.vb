Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library

Public Class LabelOrderTests
    Dim ChecklistBuilderAspx = New ChecklistBuilderAspxLibrary()
    Dim Security = New Security()

    'USING NITROGEN DAILY AS SAMPLE CHECKLIST. IF THE LABEL ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
    <Fact>
    Public Sub LabelOrder1()
        'moving label 1 up on Nitrogen Daily checklist
        Assert.Equal("", ChecklistBuilderAspx.ModifyOrder("388", "up", "Label"))
    End Sub

    <Fact>
    Public Sub LabelOrder2()
        'moving label 2 up on Nitrogen Daily checklist
        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=1 WHERE [Key]=389; UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=2 WHERE [Key]=388", ChecklistBuilderAspx.ModifyOrder("389", "up", "Label"))
    End Sub

    <Fact>
    Public Sub LabelOrder3()
        'moving label 3 down on Nitrogen Daily checklist
        Assert.Equal("", ChecklistBuilderAspx.ModifyOrder("390", "down", "Label"))
    End Sub

    <Fact>
    Public Sub LabelOrder4()
        'moving label 2 down on Nitrogen Daily checklist
        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=3 WHERE [Key]=389; UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=2 WHERE [Key]=390", ChecklistBuilderAspx.ModifyOrder("389", "down", "Label"))
    End Sub
    'USING NITROGEN DAILY AS SAMPLE CHECKLIST. IF THE LABEL ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
End Class

Public Class CommentOrderTests
    Dim ChecklistBuilderAspx = New ChecklistBuilderAspxLibrary()
    Dim Security = New Security()

    'USING R.O Daily AS SAMPLE CHECKLIST. IF THE COMMENT ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
    <Fact>
    Public Sub CommentOrder1()
        'moving comment 1 up on Nitrogen Daily checklist
        Assert.Equal("", ChecklistBuilderAspx.ModifyOrder("53", "up", "Comment"))
    End Sub

    <Fact>
    Public Sub CommentOrder2()
        'moving comment 2 up on Nitrogen Daily checklist
        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogCommentList] SET CommentOrder=1 WHERE [Key]=54; UPDATE [ALTS].[dbo].[T_LogCommentList] SET CommentOrder=2 WHERE [Key]=53", ChecklistBuilderAspx.ModifyOrder("54", "up", "Comment"))
    End Sub

    <Fact>
    Public Sub CommentOrder3()
        'moving comment 3 down on Nitrogen Daily checklist
        Assert.Equal("", ChecklistBuilderAspx.ModifyOrder("55", "down", "Comment"))
    End Sub

    <Fact>
    Public Sub CommentOrder4()
        'moving comment 2 down on Nitrogen Daily checklist
        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogCommentList] SET CommentOrder=3 WHERE [Key]=54; UPDATE [ALTS].[dbo].[T_LogCommentList] SET CommentOrder=2 WHERE [Key]=55", ChecklistBuilderAspx.ModifyOrder("54", "down", "Comment"))
    End Sub
    'USING R.O Daily AS SAMPLE CHECKLIST. IF THE COMMENT ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
End Class