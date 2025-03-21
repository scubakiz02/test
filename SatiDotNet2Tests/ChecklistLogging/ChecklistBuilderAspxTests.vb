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
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("388", "up", "Label")
        Assert.Equal("", Res("SqlQuery"))
    End Sub

    <Fact>
    Public Sub LabelOrder2()
        'moving label 2 up on Nitrogen Daily checklist
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim ModifyOrderRes As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("389", "up", "Label")
        Dim ParameterizedValuesConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("ParameterizedValues"))
        Dim UnitTestRes As Boolean

        'validate proper values in ParameterizedValuesConfig
        If ParameterizedValuesConfig("@Order1")("value") = "1" AndAlso ParameterizedValuesConfig("@Order1")("typeOf") = "int" Then

            If ParameterizedValuesConfig("@Key1")("value") = "389" AndAlso ParameterizedValuesConfig("@Key1")("typeOf") = "int" Then

                If ParameterizedValuesConfig("@Order2")("value") = "2" AndAlso ParameterizedValuesConfig("@Order2")("typeOf") = "int" Then

                    If ParameterizedValuesConfig("@Key2")("value") = "388" AndAlso ParameterizedValuesConfig("@Key2")("typeOf") = "int" Then

                        If ModifyOrderRes("SqlQuery").Contains("T_LogLabel") AndAlso ModifyOrderRes("SqlQuery").Contains("LabelOrder") Then

                            UnitTestRes = True

                        End If

                    End If

                End If

            End If

        Else
            UnitTestRes = False
        End If

        Assert.True(UnitTestRes)
    End Sub

    <Fact>
    Public Sub LabelOrder3()
        'moving label 3 down on Nitrogen Daily checklist
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("390", "down", "Label")
        Assert.Equal("", Res("SqlQuery"))
    End Sub

    <Fact>
    Public Sub LabelOrder4()
        'moving label 2 down on Nitrogen Daily checklist

        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim ModifyOrderRes As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("389", "down", "Label")
        Dim ParameterizedValuesConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("ParameterizedValues"))
        Dim UnitTestRes As Boolean

        'validate proper values in ParameterizedValuesConfig
        If ParameterizedValuesConfig("@Order1")("value") = "3" AndAlso ParameterizedValuesConfig("@Order1")("typeOf") = "int" Then

            If ParameterizedValuesConfig("@Key1")("value") = "389" AndAlso ParameterizedValuesConfig("@Key1")("typeOf") = "int" Then

                If ParameterizedValuesConfig("@Order2")("value") = "2" AndAlso ParameterizedValuesConfig("@Order2")("typeOf") = "int" Then

                    If ParameterizedValuesConfig("@Key2")("value") = "390" AndAlso ParameterizedValuesConfig("@Key2")("typeOf") = "int" Then

                        If ModifyOrderRes("SqlQuery").Contains("T_LogLabel") AndAlso ModifyOrderRes("SqlQuery").Contains("LabelOrder") Then

                            UnitTestRes = True

                        End If

                    End If

                End If

            End If

        Else
            UnitTestRes = False
        End If

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
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("53", "up", "Comment")
        Assert.Equal("", Res("SqlQuery"))
    End Sub

    <Fact>
    Public Sub CommentOrder2()
        'moving comment 2 up on Nitrogen Daily checklist
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim ModifyOrderRes As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("54", "up", "Comment")
        Dim ParameterizedValuesConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("ParameterizedValues"))
        Dim UnitTestRes As Boolean

        'validate proper values in ParameterizedValuesConfig
        If ParameterizedValuesConfig("@Order1")("value") = "1" AndAlso ParameterizedValuesConfig("@Order1")("typeOf") = "int" Then

            If ParameterizedValuesConfig("@Key1")("value") = "54" AndAlso ParameterizedValuesConfig("@Key1")("typeOf") = "int" Then

                If ParameterizedValuesConfig("@Order2")("value") = "2" AndAlso ParameterizedValuesConfig("@Order2")("typeOf") = "int" Then

                    If ParameterizedValuesConfig("@Key2")("value") = "53" AndAlso ParameterizedValuesConfig("@Key2")("typeOf") = "int" Then

                        If ModifyOrderRes("SqlQuery").Contains("T_LogCommentList") AndAlso ModifyOrderRes("SqlQuery").Contains("CommentOrder") Then

                            UnitTestRes = True

                        End If

                    End If

                End If

            End If

        Else
            UnitTestRes = False
        End If

        Assert.True(UnitTestRes)
    End Sub

    <Fact>
    Public Sub CommentOrder3()
        'moving comment 3 down on Nitrogen Daily checklist
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("55", "down", "Comment")
        Assert.Equal("", Res("SqlQuery"))
    End Sub

    <Fact>
    Public Sub CommentOrder4()
        'moving comment 2 down on Nitrogen Daily checklist
        Dim QueryConfig As New Dictionary(Of String, Dictionary(Of String, String))
        Dim ModifyOrderRes As Dictionary(Of String, String) = ChecklistBuilderAspx.ModifyOrder("54", "down", "Comment")
        Dim ParameterizedValuesConfig As Dictionary(Of String, Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of Dictionary(Of String, Dictionary(Of String, String)))(ModifyOrderRes("ParameterizedValues"))
        Dim UnitTestRes As Boolean

        'validate proper values in ParameterizedValuesConfig
        If ParameterizedValuesConfig("@Order1")("value") = "3" AndAlso ParameterizedValuesConfig("@Order1")("typeOf") = "int" Then

            If ParameterizedValuesConfig("@Key1")("value") = "54" AndAlso ParameterizedValuesConfig("@Key1")("typeOf") = "int" Then

                If ParameterizedValuesConfig("@Order2")("value") = "2" AndAlso ParameterizedValuesConfig("@Order2")("typeOf") = "int" Then

                    If ParameterizedValuesConfig("@Key2")("value") = "55" AndAlso ParameterizedValuesConfig("@Key2")("typeOf") = "int" Then

                        If ModifyOrderRes("SqlQuery").Contains("T_LogCommentList") AndAlso ModifyOrderRes("SqlQuery").Contains("CommentOrder") Then

                            UnitTestRes = True

                        End If

                    End If

                End If

            End If

        Else
            UnitTestRes = False
        End If

        Assert.True(UnitTestRes)
    End Sub
    'USING R.O Daily AS SAMPLE CHECKLIST. IF THE COMMENT ORDER HAS CHANGED, THESE TESTS WILL FAIL!!!!!!!!!
End Class

Public Class GetAreaDdlSelectCommandTests
    Dim ChecklistBuilderAspx = New ChecklistBuilderAspxLibrary()
    Dim Security = New Security()

    <Fact>
    Public Sub GetAreaDdlSelectCommandTest1()
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.GetAreaDdlSelectConfig(Nothing)
        Assert.True(Res("AreaIntervalKey") = -1 AndAlso Res("SelectQuery") = "SELECT A.Area, A.[Key] FROM [ALTS].[dbo].[T_LogArea] A LEFT JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE (A.IntervalKey=@AreaIntervalKey OR @AreaIntervalKey=-1 OR (A.IntervalKey IS NULL AND DATEDIFF(DAY, A.DateCreated, GETDATE()) = 0)) AND OneTimeDate IS NULL OR (OneTimeDate IS NOT NULL AND ((SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key])=0 OR (SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key]) IS NULL)) ORDER BY A.Area")
    End Sub

    <Fact>
    Public Sub GetAreaDdlSelectCommandTest2()
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.GetAreaDdlSelectConfig("All")
        Assert.True(Res("AreaIntervalKey") = -1 AndAlso Res("SelectQuery") = "SELECT A.Area, A.[Key] FROM [ALTS].[dbo].[T_LogArea] A LEFT JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE (A.IntervalKey=@AreaIntervalKey OR @AreaIntervalKey=-1 OR (A.IntervalKey IS NULL AND DATEDIFF(DAY, A.DateCreated, GETDATE()) = 0)) AND OneTimeDate IS NULL OR (OneTimeDate IS NOT NULL AND ((SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key])=0 OR (SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key]) IS NULL)) ORDER BY A.Area")
    End Sub

    <Fact>
    Public Sub GetAreaDdlSelectCommandTest3()
        Dim Res As Dictionary(Of String, String) = ChecklistBuilderAspx.GetAreaDdlSelectConfig(3)
        Assert.True(Res("AreaIntervalKey") = 3 AndAlso Res("SelectQuery") = "SELECT A.Area, A.[Key] FROM [ALTS].[dbo].[T_LogArea] A LEFT JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE (A.IntervalKey=@AreaIntervalKey OR @AreaIntervalKey=-1 OR (A.IntervalKey IS NULL AND DATEDIFF(DAY, A.DateCreated, GETDATE()) = 0)) AND OneTimeDate IS NULL OR (OneTimeDate IS NOT NULL AND ((SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key])=0 OR (SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key]) IS NULL)) ORDER BY A.Area")
    End Sub
End Class