Imports System.Text
Imports Xunit
Imports SatiDotNet2.Library
Imports System.Text.Json

Public Class LabelOrderTests
    Dim ChecklistBuilderAspx = New ChecklistBuilderAspxLibrary()
    Dim Security = New Security()

    'USING NITROGEN DAILY AS SAMPLE CHECKLIST. IF THE LABEL ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
    <Fact>
    Public Sub LabelOrder1()
        'moving label 1 up on Nitrogen Daily checklist
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrderv2("388", "up", "Label")
        Assert.Equal("", Res("SqlQuery"))
    End Sub

    <Fact>
    Public Sub LabelOrder2()
        'moving label 2 up on Nitrogen Daily checklist
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim ModifyOrderRes As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrderv2("389", "up", "Label")
        Dim ParameterizedValuesConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("ParameterizedValues"))
        Dim UnitTestRes As Boolean

        'validate proper values in ParameterizedValuesConfig
        If ParameterizedValuesConfig("@LabelOrder1")("value") = "1" AndAlso ParameterizedValuesConfig("@LabelOrder1")("typeOf") = "int" Then

            If ParameterizedValuesConfig("@Key1")("value") = "389" AndAlso ParameterizedValuesConfig("@Key1")("typeOf") = "int" Then

                If ParameterizedValuesConfig("@LabelOrder2")("value") = "2" AndAlso ParameterizedValuesConfig("@LabelOrder2")("typeOf") = "int" Then

                    If ParameterizedValuesConfig("@Key2")("value") = "388" AndAlso ParameterizedValuesConfig("@Key2")("typeOf") = "int" Then

                        UnitTestRes = True

                    End If

                End If

            End If

        Else
            UnitTestRes = False
        End If

        ' Assert.Equal("UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=1 WHERE [Key]=389; UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=2 WHERE [Key]=388", ChecklistBuilderAspx.ModifyOrder("389", "up", "Label"))
        Assert.True(UnitTestRes)
    End Sub

    <Fact>
    Public Sub LabelOrder3()
        'moving label 3 down on Nitrogen Daily checklist
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrderv2("390", "down", "Label")
        Assert.Equal("", Res("SqlQuery"))
        'Assert.Equal("", ChecklistBuilderAspx.ModifyOrder("390", "down", "Label"))
    End Sub

    <Fact>
    Public Sub LabelOrder4()
        'moving label 2 down on Nitrogen Daily checklist

        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim ModifyOrderRes As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrderv2("389", "down", "Label")
        Dim ParameterizedValuesConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("ParameterizedValues"))
        Dim UnitTestRes As Boolean

        'validate proper values in ParameterizedValuesConfig
        If ParameterizedValuesConfig("@LabelOrder1")("value") = "3" AndAlso ParameterizedValuesConfig("@LabelOrder1")("typeOf") = "int" Then

            If ParameterizedValuesConfig("@Key1")("value") = "389" AndAlso ParameterizedValuesConfig("@Key1")("typeOf") = "int" Then

                If ParameterizedValuesConfig("@LabelOrder2")("value") = "2" AndAlso ParameterizedValuesConfig("@LabelOrder2")("typeOf") = "int" Then

                    If ParameterizedValuesConfig("@Key2")("value") = "390" AndAlso ParameterizedValuesConfig("@Key2")("typeOf") = "int" Then

                        UnitTestRes = True

                    End If

                End If

            End If

        Else
            UnitTestRes = False
        End If

        ' Assert.Equal("UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=3 WHERE [Key]=389; UPDATE [ALTS].[dbo].[T_LogLabel] SET LabelOrder=2 WHERE [Key]=390", ChecklistBuilderAspx.ModifyOrder("389", "down", "Label"))
        Assert.True(UnitTestRes)
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