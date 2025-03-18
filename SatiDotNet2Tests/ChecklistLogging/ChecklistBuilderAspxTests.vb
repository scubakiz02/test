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
        Assert.Equal("", ChecklistBuilderAspx.ModifyLabelOrder("388", "up"))
    End Sub

    <Fact>
    Public Sub LabelOrder2()
        'moving label 2 up on Nitrogen Daily checklist
        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=1 WHERE [Key]=389; UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=2 WHERE [Key]=388", ChecklistBuilderAspx.ModifyLabelOrder("389", "up"))
    End Sub

    <Fact>
    Public Sub LabelOrder3()
        'moving label 3 down on Nitrogen Daily checklist
        Assert.Equal("", ChecklistBuilderAspx.ModifyLabelOrder("390", "down"))
    End Sub

    <Fact>
    Public Sub LabelOrder4()
        'moving label 2 down on Nitrogen Daily checklist
        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=3 WHERE [Key]=389; UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=2 WHERE [Key]=390", ChecklistBuilderAspx.ModifyLabelOrder("389", "down"))
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
        Assert.Equal("", ChecklistBuilderAspx.ModifyCommentOrder("53", "up"))
    End Sub

    <Fact>
    Public Sub CommentOrder2()
        'moving comment 2 up on Nitrogen Daily checklist
        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogCommentList] SET CommentOrder=1 WHERE [Key]=54; UPDATE [ALTS].[dbo].[T_LogCommentList] SET CommentOrder=2 WHERE [Key]=53", ChecklistBuilderAspx.ModifyCommentOrder("54", "up"))
    End Sub

    <Fact>
    Public Sub CommentOrder3()
        'moving comment 3 down on Nitrogen Daily checklist
        Assert.Equal("", ChecklistBuilderAspx.ModifyCommentOrder("55", "down"))
    End Sub

    <Fact>
    Public Sub CommentOrder4()
        'moving comment 2 down on Nitrogen Daily checklist
        Assert.Equal("UPDATE [ALTS].[dbo].[T_LogCommentList] SET CommentOrder=3 WHERE [Key]=54; UPDATE [ALTS].[dbo].[T_LogCommentList] SET CommentOrder=2 WHERE [Key]=55", ChecklistBuilderAspx.ModifyCommentOrder("54", "down"))
    End Sub
    'USING R.O Daily AS SAMPLE CHECKLIST. IF THE COMMENT ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
End Class